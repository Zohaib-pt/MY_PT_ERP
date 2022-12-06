using DAL.Models;
using DAL.oClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Rotativa.AspNetCore;
using System.Text;


using Microsoft.AspNetCore.Authorization;



using System.Text.RegularExpressions;
using X.PagedList;
using Rotativa;
using NumericWordsConversion;

namespace QRLinkView.Controllers
{
    public class SalesController : BaseController
    {

        static string ReportFolder;

        private readonly IOBasicData _oBasic;
        private readonly IOStock _oStock;
        private readonly IOPurchases _oPurchase;
        private readonly IOSales _oSales;
        private readonly IOAccounts _oAccounts;
        private readonly IOReports _oReports;
        private readonly IOInventory _oInventory;
        private readonly IODelivery _oDelivery;
        private IConfiguration configuration;
        public SalesController(IOBasicData oBasicBase, IOStock oStock, IOPurchases oPurchase, IOSales oSales, IOAccounts oAccounts, IOReports oReport, IODelivery oDelivery, IOInventory oInventory, IConfiguration iConfig)
           : base(oBasicBase)
        {
            _oBasic = oBasicBase;
            _oStock = oStock;
            _oPurchase = oPurchase;
            _oSales = oSales;
            _oAccounts = oAccounts;
            _oReports = oReport;
            _oDelivery = oDelivery;
            _oInventory = oInventory;
            configuration = iConfig;

            ReportFolder = configuration.GetSection("AppSettings").GetSection("CompanyReportFolderCode").Value;

        }
   
   
        [HttpGet]
        public IActionResult NewSalesInvoiceView(string SaleMaster_ID,string c_id)
        {
           
            var key = "b14ca5898a4e4133bbce2ea2315a1916";
            var decryptedString = SaleMaster_ID;

            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;
            /* SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);*/

            ///Check If Cancelled by waiz
            var res = _oSales.SalesMasterPrintObj = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(Convert.ToInt32(decryptedString));
            string Status_ID = res.SaleStatus_ID;
            if (Status_ID != "5")
            {

            
            _oSales.SalesDetailPrintObj = _oSales.Get_SalesInvoiceDetailPrintByID_DAL(Convert.ToInt32(decryptedString)).ToList();
            
            _oSales.SalesDetailPrintObj_TTL = _oSales.Get_SalesInvoiceDetailPrintByID_ttl(Convert.ToInt32(decryptedString));
            _oSales.SalesReceiptDetailsList_Print = _oSales.Get_ReceiptDetailSaleListByID_DAL_Invoice_Print(Convert.ToInt32(decryptedString));
            string Host = Request.Host.ToString();

            string TotalAmount = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(Convert.ToInt32(decryptedString)).AfterVatTotal;
            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0";
            }
            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());


            _oSales.SalesDetailPrintObj_TTL.Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.SalesMasterPrintObj.Currency_ShortName
                + (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.SalesMasterPrintObj.Minor_ShortName + " only" : " only");



            string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "https://" + Host + "/Sales/PrintHeaderInvoiceSalesInv?c_ID=" + c_id + "&SaleMaster_ID=" + decryptedString,

            "https://" + Host + "/Sales/FooterHeaderInvoiceSalesInvGLM?c_ID=" + c_id + "&SaleMaster_ID=" + decryptedString);


            var demoViewPortrait = new ViewAsPdf(_oSales)
            {
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 50, Right = 5, Top = 70 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches
            };
            return demoViewPortrait;
            }
            else
            {
                return RedirectToAction("InvoiceNotFound", "Sales");
            }
        }

        public IActionResult InvoiceNotFound()
        {
            ViewBag.Message = String.Format("Your Invoice Has Been Canceled");
            return View();        
        
        }
        [HttpGet]
        public IActionResult NewPerformaInvoice(string SaleMaster_ID,string c_id)
        {
        
            var key = "b14ca5898a4e4133bbce2ea2315a1916";
            var decryptedString = SaleMaster_ID;
          
            _oSales.PerformaPrintObj = _oSales.Get_PerformaInvoiceDetailPrintByID_DAL(Convert.ToInt32(decryptedString));
            _oSales.PerformaPrintObj_Specs = _oSales.Get_PerformaInvoiceDetailPrintByID_Specs_DAL(Convert.ToInt32(decryptedString));
            string TotalAmount = "";
            if (_oSales.PerformaPrintObj.Count() > 0)
            {
                TotalAmount = _oSales.Get_PerformaInvoiceDetailPrintByID_DAL(Convert.ToInt32(decryptedString)).Select(x => x.Total_Amt_inWords).LastOrDefault();

            }
            if (_oSales.PerformaPrintObj_Specs.Count() > 0)
            {
                TotalAmount = _oSales.Get_PerformaInvoiceDetailPrintByID_Specs_DAL(Convert.ToInt32(decryptedString)).Select(x => x.Total_Amt_inWords).LastOrDefault();
            }


            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0.00";
            }
            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());

            if (_oSales.PerformaPrintObj.Count() > 0)
            {
                _oSales.PerformaPrintObj.LastOrDefault().Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.PerformaPrintObj.LastOrDefault().Currency_ShortName
+ (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.PerformaPrintObj.LastOrDefault().Minor_ShortName + " only" : " only");

            }
            if (_oSales.PerformaPrintObj_Specs.Count() > 0)
            {
                _oSales.PerformaPrintObj_Specs.LastOrDefault().Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.PerformaPrintObj_Specs.LastOrDefault().Currency_ShortName
+ (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.PerformaPrintObj_Specs.LastOrDefault().Minor_ShortName + " only" : " only");

            }


            string Host = Request.Host.ToString();


            string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "https://" + Host + "/Sales/PrintHeaderInvoicePerformaInv?c_ID=" + c_id + "&SaleMaster_ID=" + decryptedString,

            "https://" + Host + "/Sales/FooterHeaderInvoicePerformaInv?c_ID=" + c_id + "&SaleMaster_ID=" + decryptedString);


            var demoViewPortrait = new ViewAsPdf(_oSales)
            {
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 50, Right = 5, Top = 70 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches
            };
            return demoViewPortrait;

        }

        [HttpGet]
        public IActionResult WordDocuments(string WordDocument_ID,string c_id)
        {
    
        
            var key = "b14ca5898a4e4133bbce2ea2315a1916";
            var decryptedString =  DecryptString(key, WordDocument_ID);
            _oBasic.WordListById = _oBasic.Get_WordList(Convert.ToInt32(c_id), Convert.ToInt32(decryptedString));


            byte[] fileBytes = System.IO.File.ReadAllBytes(_oBasic.WordListById.PDFName);

            return File(fileBytes, "application/pdf", _oBasic.WordListById.PDFName);


        }

        #region Decrypt Method
         /// Derypt Method By Waiz
        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        #endregion



        [AllowAnonymous]
        public IActionResult PrintHeaderInvoiceSalesInv(int? c_ID, int? SaleMaster_ID)
        {
            ViewBag.ReportCompanySalInv = c_ID;
            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.SalesMasterPrintObj = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID);
            _oSales.SalesDetailPrintObj = _oSales.Get_SalesInvoiceDetailPrintByID_DAL(SaleMaster_ID).ToList();
            _oSales.SalesDetailPrintObj_TTL = _oSales.Get_SalesInvoiceDetailPrintByID_ttl(SaleMaster_ID);
            return View(_oSales);
        }

        [AllowAnonymous]
        public IActionResult FooterHeaderInvoiceSalesInvGLM(int? c_ID, int SaleMaster_ID)
        {
            ViewBag.ReportCompanyPur_F = c_ID;
            _oSales.SalesMasterPrintObj = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID);
            return View(_oSales);
        }



        //Nafeel
        [AllowAnonymous]
        public IActionResult PrintHeaderInvoicePerformaInv(int? c_ID, int? SaleMaster_ID)
        {
            ViewBag.ReportCompanyPerformaInv = c_ID;
            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.PerformaPrintObj_Specs = _oSales.Get_PerformaInvoiceDetailPrintByID_Specs_DAL(SaleMaster_ID);
            _oSales.PerformaPrintObj = _oSales.Get_PerformaInvoiceDetailPrintByID_DAL(SaleMaster_ID);
            return View(_oSales);
        }
        [AllowAnonymous]
        public IActionResult FooterHeaderInvoicePerformaInv(int? c_ID, int? SaleMaster_ID)
        {
            ViewBag.ReportCompanyPer_F = c_ID;
            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.PerformaPrintObj_Specs = _oSales.Get_PerformaInvoiceDetailPrintByID_Specs_DAL(SaleMaster_ID);
            _oSales.PerformaPrintObj = _oSales.Get_PerformaInvoiceDetailPrintByID_DAL(SaleMaster_ID);
            return View(_oSales);
        }


    }
}
