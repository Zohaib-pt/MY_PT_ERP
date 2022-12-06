using DAL.Models;
using DAL.oClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using NumericWordsConversion;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using X.PagedList;


namespace QRLinkView.Controllers
{
    public class AccountsController : BaseController
    {
        #region interfaces variables
        private readonly IOReports _oReports;
        private readonly IOBasicData _oBasic;
        private readonly IOAccounts _oAccounts;
        private IConfiguration configuration;
        #endregion

        public AccountsController(IOBasicData oBasicBase, IOAccounts oAccounts, IConfiguration iConfig, IOReports oReport)
         : base(oBasicBase)
        {
            _oBasic = oBasicBase;
            _oAccounts = oAccounts;
            configuration = iConfig;
            _oReports = oReport;
        }
        //---get method of  Receipt Voucher Genenral
        //public static string DecryptString(string key, string cipherText)
        //{
        //    byte[] iv = new byte[16];
        //    byte[] buffer = Convert.FromBase64String(cipherText);

        //    using (Aes aes = Aes.Create())
        //    {
        //        aes.Key = Encoding.UTF8.GetBytes(key);
        //        aes.IV = iv;
        //        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        //        using (MemoryStream memoryStream = new MemoryStream(buffer))
        //        {
        //            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
        //            {
        //                using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
        //                {
        //                    return streamReader.ReadToEnd();
        //                }
        //            }
        //        }
        //    }
        //}
        [HttpGet]
        public IActionResult ReceiptVoucherGen(string ReceiptMaster_ID,string c_id)
        {
            //var key = "b14ca5898a4e4133bbce2ea2315a1916";
            //var decryptedString = DecryptString(key, ReceiptMaster_ID);
            _oReports.ReceiptVoucher_Print = _oReports.GetDataForReceiptMaster_Print(Convert.ToInt32(ReceiptMaster_ID));



            string TotalAmount = _oReports.GetDataForReceiptMaster_Print(Convert.ToInt32(ReceiptMaster_ID)).Select(x => x.Total_Amount).LastOrDefault();
            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0";
            }

            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            
            
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = null;

            if (TotalAmount != "0")
            {

             match = regex.Match(TotalAmount.Split(".")[1].ToString());

            }




            _oReports.ReceiptVoucher_Print.LastOrDefault().Total_Amt_inWords = converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[0])) + " " +
                _oReports.GetDataForReceiptMaster_Print(Convert.ToInt32(ReceiptMaster_ID)).LastOrDefault().Currency_ShortName
                + (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oReports.GetDataForReceiptMaster_Print(Convert.ToInt32(ReceiptMaster_ID)).LastOrDefault().Minor_ShortName + " only" : " only");

            string Host = Request.Host.ToString();
            string customSwitches = string.Format("--header-spacing \"0\" --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "https://" + Host + "/Accounts/HeaderReceiptVoucher?c_ID=" + c_id,

            "https://" + Host + "/Accounts/FooterReceiptVoucherGLM?ReceiptMaster_ID=" + ReceiptMaster_ID + "&c_ID=" + c_id);

            //return View(_oReports);
            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 65, Right = 5, Top = 25 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches

            };
            return demoViewPortrait;
        }


     
        [AllowAnonymous]
        public IActionResult HeaderReceiptVoucher(int? c_ID)
        {
            ViewBag.ReportCompanyRec = c_ID;
            return View();
        }

        [AllowAnonymous]
        public IActionResult FooterReceiptVoucherGLM(int ReceiptMaster_ID, int? c_ID)
        {
            ViewBag.ReportCompanyRec_f = c_ID;
            _oReports.ReceiptVoucher_Print = _oReports.GetDataForReceiptMaster_Print(ReceiptMaster_ID);
            return View(_oReports);
        }

    }
}
