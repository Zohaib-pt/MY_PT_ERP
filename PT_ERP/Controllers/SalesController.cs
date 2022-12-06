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
using System.Text;
using X.PagedList;
using ZXing;
using ZXing.QrCode;


namespace PT_ERP.Controllers
{
    public class SalesController : BaseController
    {


        #region static variables region
        //---Static Fields for creating new sales invoice
        static string SalesInvoiceMaster_TempID;
        static int? SaleInvoiceMasterStatic_ID;

        //---Vehicle Display temp id and static fields
        static string VehicleDisplayStaticTempID;
        static int? VehicleDisplayMasterStatic_ID;

        //---static fields for creating new sales booking
        static string SalesBookingMaster_TempID;
        static int? SaleBookingMasterStatic_ID;

        //---Static Fields for creating new sales performa invoice
        static string SalePerforma_Master_TempID;
        static int? SalePerforma_MasterStatic_ID;

        //---Static Fields for creating new sales performa invoice
        static string SaleCancel_Master_TempID;
        static int? SaleCancel_MasterStatic_ID;

        static string SalesMasterJPMaster_TempID;
        static int? SalesMasterJPStatic_ID;

        static string SalesReturnTempID;
        static int? SalesReturnMasterStatic_ID;


        static string DOOtherTempID;
        static int? DOMasterOtherStatic_ID;

        static string PaperAfterSaleTempID;
        static int? PaperAfterSaleStatic_ID;

        static string ApprovalURL;
        static string Urls;
        #endregion



        private readonly IOBasicData _oBasic;
        private readonly IOSales _oSales;
        private IConfiguration configuration;
        private readonly IOStock _oStock;
        private readonly OSales _ooSales;
        public SalesController(IOBasicData oBasicBase, IOStock oStock, IOSales oSales, IConfiguration iConfig)
         : base(oBasicBase)
        {
            _oBasic = oBasicBase;
            _oSales = oSales;
            _oStock = oStock;
            configuration = iConfig;
        }

        public IActionResult Index()
        {
            return View();
        }


        #region SALE BOOKING

        [HttpGet]
        public IActionResult NewSalesBooking(int SaleMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "NewSalesBooking";

            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "New Sales Booking";
            ViewBag.SectionSub_HeaderMainTitle = "Sales";
            ViewBag.PageTitle = "New Sales Booking";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            #region ViewBag Area
            //---Get Customer Master
            ViewBag.CustomerMaster = _oBasic.Get_CustomersList_DAL(c_ID).ToList();

            //---Ven Category list for drop down in forms
            ViewBag.CustomerCategoryList = _oBasic.Get_CustomerCategory_DAL().ToList();

            ViewBag.SellerList = _oBasic.Get_PartnersSeller_DAL(c_ID).ToList();
            ViewBag.AgentsList = _oBasic.Get_PartnersAgent_DAL(c_ID).ToList();
            ViewBag.ExportDestinations = _oBasic.Get_Destinations_DAL(c_ID).ToList();
            ViewBag.PortOfExit = _oBasic.Get_PortOfExit_DAL(c_ID).ToList();
            //---get shipping companies
            ViewBag.ShippingCompanies = _oBasic.Get_ShippingCompaniesList_DAL(c_ID).ToList();
            //--get export companies list
            ViewBag.ExportCompanies = _oBasic.Get_ExportCompaniesList_DAL(c_ID).ToList();
            //--get Basic Banks list
            ViewBag.BasicBanksList = _oBasic.Get_BasicBanksList_DAL(c_ID).ToList();
            //--get showrooms lsit
            ViewBag.ShowroomsList = _oBasic.Get_ShowRoom_DAL(c_ID).ToList();
            //---Get Accounts List
            ViewBag.CR_AccountsList = _oBasic.Select_RV_OnAccounts_DAL(c_ID).ToList();

            //---Get pay via list
            ViewBag.DR_AccountsList = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();
            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();

            ViewBag.Default_Curr_Rate = "1";
            ViewBag.Default_Currency_ID = "5";
            #endregion




            if (SaleMaster_ID == 0 || SaleMaster_ID < 1)
            {

                //---Create payment master Ref(tem id)
                if (String.IsNullOrEmpty(SalesBookingMaster_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    SalesBookingMaster_TempID = obj.ToString();
                }

                SaleBookingMasterStatic_ID = SaleMaster_ID;

                //---Get sales master id
                ViewBag.SaleMaster_ID = SaleMaster_ID;

                //--Get sales Master
                _oSales.SaleMasterObj = _oSales.Get_SalesBookingMasterByID_DAL(-1);
                _oSales.SalesReceiptDetailsList = _oSales.Get_Sales_ReceiptDetails_DAL(-1).ToList();

                //---sales detial Booking list
                _oSales.SalesDetailList = _oSales.Get_SalesBookingDetailListByID_DAL(SalesBookingMaster_TempID, -1).ToList();

                _oSales.SalesReceiptDetailsList = _oSales.Get_ReceiptDetailSaleListByID_DAL_Invoice(SalesBookingMaster_TempID, -1).ToList();
            }
            else
            {

                //---Get old temp id from sales detail table that already creaded for this "SaleMaster_ID" id. And then use this if new record added
                string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL(SaleMaster_ID);




                SaleBookingMasterStatic_ID = SaleMaster_ID;

                //---Get sale master id
                ViewBag.SaleMaster_ID = SaleMaster_ID;

                //--Get sale Master

                _oSales.SaleMasterObj = _oSales.Get_SalesBookingMasterByID_DAL(SaleMaster_ID);

                //---sales detail Booking list 
                _oSales.SalesDetailList = _oSales.Get_SalesBookingDetailListByID_DAL(OldTempID, SaleMaster_ID).ToList();

                _oSales.SalesReceiptDetailsList = _oSales.Get_ReceiptDetailSaleListByID_DAL_Invoice("", SaleMaster_ID).ToList();
            }



            return View(_oSales);
        }

        //---insert into sales Booking detial
        [HttpPost]
        public IActionResult InsertSalesBookingDetail(string Chassis, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, int hdSaleMaster_ID, double? Discount)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //---Validation area starts here

            if (Unit_Price == null || String.IsNullOrEmpty(Chassis))
            {
                return Json(new { message = "Please enter sale price!" });
            }
            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = SalesBookingMaster_TempID;
            if (hdSaleMaster_ID == 0)
            {
                Temp_ID = SalesBookingMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }



            try
            {
                message = _oSales.InsertSalesBookingDetail_DAL(Chassis, Unit_Price, VATRate, VATExp, Total_Amount, Temp_ID, c_ID, Created_by, hdSaleMaster_ID, Discount);
                if (message == "Saved Successfully!")
                {
                    //---sales detial booking list 
                    _oSales.SalesDetailList = _oSales.Get_SalesBookingDetailListByID_DAL(Temp_ID, hdSaleMaster_ID).ToList();


                    return PartialView("_SalesBookingDetailList", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }

        //---method for inserting sales booking detail
        [HttpPost]
        public IActionResult UpdateSalesBookingDetail(int? SalesDetails_ID, string Chassis, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, int hdSaleMaster_ID, double? Discount)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //---Validation area starts here
            if (SalesDetails_ID == null)
            {
                return Json(new { message = "Sale Id is null" });
            }
            if (Unit_Price == null || String.IsNullOrEmpty(Chassis))
            {
                return Json(new { message = "Please enter sale price!" });
            }
            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = SalesBookingMaster_TempID;
            if (hdSaleMaster_ID == 0)
            {
                Temp_ID = SalesBookingMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }




            try
            {
                message = _oSales.UpdateSalesBookingDetail_DAL(SalesDetails_ID, Chassis, Unit_Price, VATRate, VATExp, Total_Amount, Temp_ID, c_ID, Modified_by, Discount);
                if (message == "Updated Successfully!")
                {
                    //---sales detial booking list 
                    _oSales.SalesDetailList = _oSales.Get_SalesBookingDetailListByID_DAL(Temp_ID, hdSaleMaster_ID).ToList();


                    return PartialView("_SalesBookingDetailList", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }

        //---Insert  sales master booking
        [HttpPost]
        public IActionResult InsertSalesBookingMaster(int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string CustomerTRN,
            string Manualbook_ref, int? Seller_ID, int? Agent_ID, int? ExportTo_Destination_ID, int? Port_of_Exit_ID, string Sale_trans_type, int? shipping_co_ID, int? ExporterCo_ID, int? Bank_to_Transfer_payment_ID, int? Showroom_ID
, string Remarks, int Currency_ID, string Currency_Rate)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = SalesBookingMaster_TempID;
            string Saletype = "SB";  //---It is the ref in table for new sales master. Send this value hard coded from here

            if (Customer_ID == null || String.IsNullOrEmpty(SaleDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewSalesBooking", "Sales");
            }

            Customer_ID = Customer_ID == null ? 0 : Customer_ID;


            try
            {
                message = _oSales.InsertSalesBookingMaster_DAL(Customer_ID, Customer_Contact, Invoice_address, Saletype, SaleDate, CustomerTRN, Manualbook_ref, Seller_ID, Agent_ID, ExportTo_Destination_ID, Port_of_Exit_ID, Sale_trans_type, shipping_co_ID, ExporterCo_ID, Bank_to_Transfer_payment_ID, Showroom_ID, Temp_ID, c_ID, Created_by, Remarks, Currency_ID, Currency_Rate);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("SalesBookingList", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("NewSalesBooking", "Sales");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("NewSalesBooking", "Sales");
            }


        }

        //---Insert  sales master booking
        [HttpPost]
        public IActionResult UpdateSalesBookingMaster(int? SaleMaster_ID, int? Customer_ID, string Customer_Contact, string Invoice_address,
            string SaleDate, string CustomerTRN, string Manualbook_ref, int? Seller_ID, int? Agent_ID, int? ExportTo_Destination_ID, int? Port_of_Exit_ID, string Sale_trans_type, int? shipping_co_ID, int? ExporterCo_ID, int? Bank_to_Transfer_payment_ID, int? Showroom_ID
, string Remarks, int Currency_ID, string Currency_Rate)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = SalesBookingMaster_TempID;
            string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL(SaleMaster_ID);
            if (!String.IsNullOrEmpty(OldTempID))
            {
                Temp_ID = OldTempID;
            }
            else
            {
                if (String.IsNullOrEmpty(Temp_ID))
                {
                    Guid obj = Guid.NewGuid();
                    Temp_ID = obj.ToString();
                }
            }
            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();

            if (Customer_ID == null || String.IsNullOrEmpty(SaleDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            Customer_ID = Customer_ID == null ? 0 : Customer_ID;


            try
            {
                message = _oSales.UpdateSalesBookingMaster_DAL(SaleMaster_ID, Customer_ID, Customer_Contact, Invoice_address, SaleDate, CustomerTRN, Manualbook_ref, Seller_ID, Agent_ID, ExportTo_Destination_ID, Port_of_Exit_ID, Sale_trans_type, shipping_co_ID, ExporterCo_ID, Bank_to_Transfer_payment_ID, Showroom_ID, Temp_ID, c_ID, Modified_by, Remarks, Currency_ID, Currency_Rate);
                if (message == "Updated Successfully!")
                {

                    TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("SalesBookingList", "Sales");
                }if (message == "Saved Successfully!")
                {

                    TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("SalesBookingList", "Sales");
                }
                else 
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }


        }

        [HttpPost]
        public IActionResult ChangeSalesBookingMasterStatus(int? SaleMaster_ID, int? Status_ID, string Trans_Type)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion

            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();


            if (SaleMaster_ID == null || Status_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                string QRName= QRGenerator(SaleMaster_ID, c_ID.ToString());
                message = _oSales.ChangeSalesMasterStatus_DAL(SaleMaster_ID, Status_ID, c_ID, Trans_Type, Modified_by, QRName);
             
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = message;
                    return Redirect(Urls);
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }
        [HttpPost]
        public IActionResult ChangeSalesBookingMasterStatus_Performa(int? SaleMaster_ID, int? Status_ID, string Trans_Type)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion

            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();


            if (SaleMaster_ID == null || Status_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                string QRName = QRGenerator_Performa(SaleMaster_ID,c_ID.ToString()); 
                message = _oSales.ChangeSalesMasterStatus_Performa_DAL(SaleMaster_ID, Status_ID, c_ID, Trans_Type, Modified_by, QRName);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = message;
                    return Redirect(Urls);
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }

        [HttpPost]
        public IActionResult ChangeSalesMasterStatus_trd(int? SaleMaster_ID, int? Status_ID, string Trans_Type)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion

            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();


            if (SaleMaster_ID == null || Status_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oSales.ChangeSalesMasterStatus_trd_DAL(SaleMaster_ID, Status_ID, c_ID, Trans_Type, Modified_by);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = message;
                    return Redirect(Urls);
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }


        //---Method for deleting sales booking master
        [HttpPost]
        public IActionResult DeleteSalesBookingMaster(int? SaleMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion
            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();



            string message = "";

            if (SaleMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }



            try
            {
                message = _oSales.DeleteSalesMaster_DAL(SaleMaster_ID);
                if (message == "Deleted Successfully!")
                {
                    return RedirectToAction("SalesBookingList", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }



        #endregion


        #region SALE INVOICE 

        [HttpGet]
        public IActionResult NewSalesInvoice(int SaleMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "NewSalesInvoice";


            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "New Sales Invoice";
            ViewBag.SectionSub_HeaderMainTitle = "Sales";

            ViewBag.PageTitle = "New Sales Invoice";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            #region ViewBag Area
            //---Get Customer Master
            ViewBag.CustomerMaster = _oBasic.Get_CustomersList_DAL(c_ID).ToList();

            ViewBag.SellerList = _oBasic.Get_PartnersSeller_DAL(c_ID).ToList();
            ViewBag.AgentsList = _oBasic.Get_PartnersAgent_DAL(c_ID).ToList();
            ViewBag.ExportDestinations = _oBasic.Get_Destinations_DAL(c_ID).ToList();
            ViewBag.PortOfExit = _oBasic.Get_PortOfExit_DAL(c_ID).ToList();

            //---Ven Category list for drop down in forms
            ViewBag.CustomerCategoryList = _oBasic.Get_CustomerCategory_DAL().ToList();
            //---get shipping companies
            ViewBag.ShippingCompanies = _oBasic.Get_ShippingCompaniesList_DAL(c_ID).ToList();
            //--get export companies list
            ViewBag.ExportCompanies = _oBasic.Get_ExportCompaniesList_DAL(c_ID).ToList();
            //--get Basic Banks list
            ViewBag.BasicBanksList = _oBasic.Get_BasicBanksList_DAL(c_ID).ToList();
            //--get showrooms lsit
            ViewBag.ShowroomsList = _oBasic.Get_ShowRoom_DAL(c_ID).ToList();
            //---Get Accounts List
            ViewBag.CR_AccountsList = _oBasic.Select_RV_OnAccounts_DAL(c_ID).ToList();

            //---Get pay via list
            ViewBag.DR_AccountsList = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();
            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();

            ViewBag.Default_Curr_Rate = "1";
            ViewBag.Default_Currency_ID = "5";

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;





            #endregion


            if (SaleMaster_ID == 0 || SaleMaster_ID < 1)
            {


                //---Create payment master Ref(tem id)
                if (String.IsNullOrEmpty(SalesInvoiceMaster_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    SalesInvoiceMaster_TempID = obj.ToString();
                }

                SaleInvoiceMasterStatic_ID = SaleMaster_ID;

                //---Get sales master id
                ViewBag.SaleMaster_ID = SaleMaster_ID;

                //--Get sales Master
                _oSales.SaleMasterObj = _oSales.Get_SalesInvoiceMasterByID_DAL(-1, c_ID);

                _oSales.SalesReceiptDetailsList = _oSales.Get_Sales_ReceiptDetails_DAL(-1).ToList();


                //---sales detial invoice list for sales invoice type Bychassis
                _oSales.SalesDetailList = _oSales.Get_SalesIvoiceDetailListByChassis_DAL(SalesInvoiceMaster_TempID,c_ID, -1).ToList();

                _oSales.SalesReceiptDetailsList = _oSales.Get_ReceiptDetailSaleListByID_DAL_Invoice(SalesInvoiceMaster_TempID, -1).ToList();
            }
            else
            {
                Urls = null;

                ApprovalURL = HttpContext.Request.Headers["Referer"];
                Urls = ApprovalURL;
                //---Get old temp id from sales detail table that already creaded for this "SaleMaster_ID" id. And then use this if new record added
                string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL(SaleMaster_ID);




                SaleInvoiceMasterStatic_ID = SaleMaster_ID;

                //---Get sale master id
                ViewBag.SaleMaster_ID = SaleMaster_ID;

                //--Get sale Master

                _oSales.SaleMasterObj = _oSales.Get_SalesInvoiceMasterByID_DAL(SaleMaster_ID,c_ID);

                ///Check If Cancelled by waiz
                var res = _oSales.SaleMasterObj = _oSales.Get_SalesInvoiceMasterByID_DAL(SaleMaster_ID, c_ID);
                if (res.SaleStatus_ID == "5")
                {
                    ViewBag.canceled = "disabled";
                }
                _oSales.SaleMasterBalanceObj = _oSales.Get_SalesInvoiceMasterBalance(SaleMaster_ID, c_ID);

                //---sales detial invoice list for sales invoice type Bychassis
                _oSales.SalesDetailList = _oSales.Get_SalesIvoiceDetailListByChassis_DAL(OldTempID, SaleMaster_ID,c_ID).ToList();

                _oSales.SalesReceiptDetailsList = _oSales.Get_ReceiptDetailSaleListByID_DAL_Invoice("", SaleMaster_ID).ToList();


            }



            return View(_oSales);
        }


        //---insert into sales invoice detial
        [HttpPost]
        public IActionResult InsertSalesInvoiceDetail(string SalesInvoiceType, string Chassis, string ItemDesc, double? Unit_Price,
            double? VATRate, double? VATExp, double? Total_Amount, int hdSaleMaster_ID, double? Discount)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //---Validation area starts here
            if (SalesInvoiceType == "ByChassis")
            {
                if (String.IsNullOrEmpty(Chassis))
                {
                    return Json(new { message = "Please enter valid chassis No!" });
                }
            }
            else if (SalesInvoiceType == "ByService")
            {
                if (String.IsNullOrEmpty(ItemDesc))
                {
                    return Json(new { message = "Please enter description!" });
                }
            }
            if (Unit_Price == null)
            {
                return Json(new { message = "Please enter sale price!" });
            }
            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int c_ID = Convert.ToInt32(company_ID);

            Temp_ID = SalesInvoiceMaster_TempID;


            if (hdSaleMaster_ID == 0)
            {
                Temp_ID = SalesInvoiceMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            try
            {
                message = _oSales.InsertSalesInvoiceDetail_DAL(SalesInvoiceType, Chassis, ItemDesc, Unit_Price, VATRate,
                    VATExp, Total_Amount, Temp_ID, c_ID, Created_by, hdSaleMaster_ID, Discount);
                if (message == "Saved Successfully!")
                {
                    //---sales detial invoice list for sales invoice type Bychassis
                    _oSales.SalesDetailList = _oSales.Get_SalesIvoiceDetailListByChassis_DAL(Temp_ID, hdSaleMaster_ID,c_ID).ToList();



                    return PartialView("_SalesInvoiceDetailList", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }


        //---Update  sales invoice detail Chassis
        [HttpPost]
        public IActionResult UpdateSalesInvoiceDetailByChassis(int? SalesDetails_ID, string Chassis, double? Unit_Price,
            double? VATRate, double? VATExp, double? Total_Amount, int hdSaleMaster_ID, double? Discount)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //---Validation area starts here

            if (SalesDetails_ID == null)
            {
                return Json(new { message = "Sales detail id is null" });
            }

            if (String.IsNullOrEmpty(Chassis) || Unit_Price == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int c_ID = Convert.ToInt32(company_ID);


            Temp_ID = SalesInvoiceMaster_TempID;
            if (hdSaleMaster_ID == 0)
            {
                Temp_ID = SalesInvoiceMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }
            SaleInvoiceMasterStatic_ID = SaleInvoiceMasterStatic_ID == null ? -1 : SaleInvoiceMasterStatic_ID;


            try
            {
                message = _oSales.UpdateSalesInvoiceDetail_DAL(SalesDetails_ID, Chassis, "", Unit_Price, VATRate, VATExp, Total_Amount, Temp_ID, c_ID, Modified_by, Discount);
                if (message == "Updated Successfully!")
                {
                    //---sales detial invoice list for sales invoice type Bychassis
                    _oSales.SalesDetailList = _oSales.Get_SalesIvoiceDetailListByChassis_DAL(Temp_ID, hdSaleMaster_ID,c_ID).ToList();



                    return PartialView("_SalesInvoiceDetailList", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }


        //---Insert  sales invoice master
        [HttpPost]
        public IActionResult InsertSalesInvoiceMaster(int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string CustomerTRN,
            string Manualbook_ref, int? Seller_ID, int? Agent_ID, int? ExportTo_Destination_ID, int? Port_of_Exit_ID, string Sale_trans_type, int?
            shipping_co_ID, int? ExporterCo_ID, int? Bank_to_Transfer_payment_ID, int? Showroom_ID, string Remarks, int Currency_ID, string Currency_Rate, string Customer_Name)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = SalesInvoiceMaster_TempID;
            string Saletype = "SO";  //---It is the ref in table for new Payment master. Send this value hard coded from here

            if (Customer_ID == null || String.IsNullOrEmpty(SaleDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewSalesInvoice", "Sales");
            }

            Customer_ID = Customer_ID == null ? 0 : Customer_ID;


            try
            {
                message = _oSales.InsertSalesInvoiceMaster_DAL(Customer_ID, Customer_Contact, Invoice_address, Saletype, SaleDate, CustomerTRN, Manualbook_ref,
                    Seller_ID, Agent_ID, ExportTo_Destination_ID, Port_of_Exit_ID, Sale_trans_type, shipping_co_ID, ExporterCo_ID, Bank_to_Transfer_payment_ID,
                    Showroom_ID, Temp_ID, c_ID, Created_by, Remarks, Currency_ID, Currency_Rate, Customer_Name);
                if (message == "Saved Successfully!")
                {
                   
                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("SalesInvoiceList", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("NewSalesInvoice", "Sales");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
                return RedirectToAction("NewSalesInvoice", "Sales");
            }


        }

        //---Insert  sales invoice master
        [HttpPost]
        public IActionResult UpdateSalesInvoiceMaster(int? SaleMaster_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate,
            string CustomerTRN, string Manualbook_ref, int? Seller_ID, int? Agent_ID, int? ExportTo_Destination_ID, int? Port_of_Exit_ID,
            string Sale_trans_type, int? shipping_co_ID, int? ExporterCo_ID, int? Bank_to_Transfer_payment_ID, int? Showroom_ID, string Remarks, int Currency_ID, string Currency_Rate, string Customer_Name)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = SalesInvoiceMaster_TempID;
            string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL(SaleMaster_ID);
            if (!String.IsNullOrEmpty(OldTempID))
            {
                Temp_ID = OldTempID;
            }
            else
            {
                if (String.IsNullOrEmpty(Temp_ID))
                {
                    Guid obj = Guid.NewGuid();
                    Temp_ID = obj.ToString();
                }
            }
            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();


            if (Customer_ID == null || String.IsNullOrEmpty(SaleDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            Customer_ID = Customer_ID == null ? 0 : Customer_ID;


            try
            {
                message = _oSales.UpdateSalesInvoiceMaster_DAL(SaleMaster_ID, Customer_ID, Customer_Contact, Invoice_address, SaleDate, CustomerTRN,
                    Manualbook_ref, Seller_ID, Agent_ID, ExportTo_Destination_ID, Port_of_Exit_ID, Sale_trans_type, shipping_co_ID, ExporterCo_ID,
                    Bank_to_Transfer_payment_ID, Showroom_ID, Temp_ID, c_ID, Modified_by, Remarks, Currency_ID, Currency_Rate, Customer_Name);
                if (message.Contains("Successfully"))
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("SalesInvoiceList", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }


        }


        //---Method for deleting sales master invoice
        [HttpPost]
        public IActionResult DeleteSalesInvoiceMaster(int? SaleMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion
            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();



            string message = "";

            if (SaleMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }



            try
            {
                message = _oSales.DeleteSalesMaster_DAL(SaleMaster_ID);
                if (message.Contains("SUCCESSFULLY"))
                {
                    return RedirectToAction("SalesInvoiceList", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }



        //---Method for deleting sales invoice detail
        [HttpPost]
        public IActionResult DeleteSalesInvoiceDetail(int SalesDetails_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion
            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();

            string message = "";

            if (SalesDetails_ID == 0)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }



            try
            {
                message = _oSales.DeleteSalesDetail_DAL(SalesDetails_ID,c_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = message;
                    return Redirect(url);
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }



        //---Update into sales invoice detail Service
        [HttpPost]
        public IActionResult UpdateSalesInvoiceDetailByService(int? SalesDetails_ID, string ItemDesc, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, double? Discount)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //---Validation area starts here

            if (SalesDetails_ID == null)
            {
                return Json(new { message = "Sales detail id is null" });
            }

            if (String.IsNullOrEmpty(ItemDesc) || Unit_Price == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int c_ID = Convert.ToInt32(company_ID);


            Temp_ID = SalesInvoiceMaster_TempID;
            string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL(SaleInvoiceMasterStatic_ID);
            if (!String.IsNullOrEmpty(SalesInvoiceMaster_TempID))
            {
                Temp_ID = SalesInvoiceMaster_TempID;

            }
            else
            {
                Temp_ID = OldTempID;
            }
            SaleInvoiceMasterStatic_ID = SaleInvoiceMasterStatic_ID == null ? -1 : SaleInvoiceMasterStatic_ID;


            try
            {
                message = _oSales.UpdateSalesInvoiceDetail_DAL(SalesDetails_ID, "", ItemDesc, Unit_Price, VATRate, VATExp, Total_Amount, Temp_ID, c_ID, Modified_by, Discount);
                if (message == "Updated Successfully!")
                {
                    //---sales detial invoice list for sales invoice type Bychassis
                    _oSales.SalesDetailList = _oSales.Get_SalesIvoiceDetailListByChassis_DAL(Temp_ID, SaleInvoiceMasterStatic_ID,c_ID).ToList();



                    return PartialView("_SalesInvoiceDetailList", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }



        #endregion







        //---Method for deleting sales invoice detail
        [HttpPost]
        public IActionResult DeleteSalesBookingDetail(int SalesDetails_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);

            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion
            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();

            string message = "";

            if (SalesDetails_ID == 0)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }



            try
            {
                message = _oSales.DeleteSalesDetail_DAL(SalesDetails_ID,c_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = message;
                    return Redirect(url);
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }








        //---convert sales booking by chassis
        [HttpGet]
        public IActionResult Convert_SalesBooking_by_Chassis(int? SaleMaster_ID, string SelectedChassis)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion



            string message = "";

            int? Created_by = Convert.ToInt32(UserID_Admin);


            if (String.IsNullOrEmpty(SelectedChassis))
            {
                return Json(new { message = "Please checked some row before converting!" });
            }

            if (SaleMaster_ID == null)
            {
                return Json(new { message = "Sale id is null. Please try again!" });
            }

            try
            {
                message = _oSales.Convert_SalesBooking_by_Chassis_DAL(SaleMaster_ID, SelectedChassis, Created_by);
                if (message == "Convert Successfully")
                {
                    return Json(new { message = "Converted Successfully!" });
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }



        }

        //---convert sales booking by sales master id
        [HttpGet]
        public IActionResult convert_Salebooking_By_SalesMasterID(int? SaleMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion



            string message = "";

            int? Created_by = Convert.ToInt32(UserID_Admin);



            if (SaleMaster_ID == null)
            {
                return Json(new { message = "Sale id is null. Please try again!" });
            }

            try
            {
                message = _oSales.convert_Salebooking_By_SalesMasterID_DAL(SaleMaster_ID);
                if (message == "Convert Successfully!")
                {
                    return Json(new { code = 1 });
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }



        }


        [HttpGet]
        public IActionResult SalesInvoiceList(string SaleRef, string ManualRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string Chassis_No, int c_ID, int? page,int Make,int Model_Desc,string Model_Year)
        {
            //---dumping static fields
            DumpStaticFieldsSales();

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "SalesInvoiceList";

            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "Sale Invoice List";

            ViewBag.PageTitle = "Sales Invoice List";

            
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region Veiwbags area
            //---Status
            ViewBag.ddlStatus = _oBasic.get_SalesTrans_Status_DAL().ToList();
            ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_IDs).ToList();
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            #endregion veiwbags area ends here

           // string company_ID = HttpContext.Session.GetString("c_ID");
           // int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            ManualRef = String.IsNullOrEmpty(ManualRef) ? "" : ManualRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_No = String.IsNullOrEmpty(Chassis_No) ? "" : Chassis_No;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;
            Status_ID = Status_ID == 0 ? 0 : Status_ID;
            Make = Make == 0 ? 0 : Make;
            Model_Desc = Model_Desc == 0 ? 0 : Model_Desc;
            Model_Year = String.IsNullOrEmpty(Model_Year) ? "" : Model_Year;
            //ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_ID).ToList();
            ViewBag.SaleRef_SaleInvoice = SaleRef;
            ViewBag.ManualRef_SaleInvoice = ManualRef;
            ViewBag.StartDate_SaleInvoice = StartDate;
            ViewBag.EndDate_SaleInvoice = EndDate;
            ViewBag.Customer_Name_SaleInvoice = Customer_Name;
            ViewBag.Chassis_No_SaleInvoice = Chassis_No;
            ViewBag.Status_ID_SaleInvoice = Status_ID;
            ViewBag.c_ID_SaleInvoice = c_ID;

            //---purchase sales master invoice list for page
            ViewBag.RecordsPerPage = 10;

            _oSales.SalesMasterIPagedList = _oSales.Get_SalesMasterInvoiceList_DAL(SaleRef, ManualRef, StartDate, EndDate, Customer_Name, Chassis_No, Status_ID, c_ID,Make,Model_Desc,Model_Year).ToPagedList(page ?? 1, 10);
            _oSales.SalesMasterListTOTAL = _oSales.Get_SalesMasterInvoiceList_TTL_DAL(SaleRef, ManualRef, StartDate, EndDate, Customer_Name, Chassis_No, Status_ID, c_ID, Make, Model_Desc, Model_Year).ToList();

            return View(_oSales);
        }
        //---Get Sales master invoice list by search filters
        [HttpGet]
        public IActionResult GetSalesMasterInvoiceListBySearchFilers(string SaleRef, string ManualRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string Chassis_No, int c_ID, int? page, int Make, int Model_Desc, string Model_Year)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            ManualRef = String.IsNullOrEmpty(ManualRef) ? "" : ManualRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_No = String.IsNullOrEmpty(Chassis_No) ? "" : Chassis_No;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;
            Status_ID = Status_ID == 0 ? 0 : Status_ID;
            Make = Make == 0 ? 0 : Make;
            Model_Desc = Model_Desc == 0 ? 0 : Model_Desc;
            Model_Year = String.IsNullOrEmpty(Model_Year) ? "" : Model_Year;

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            ViewBag.SaleRef_SaleInvoice = SaleRef;
            ViewBag.ManualRef_SaleInvoice = ManualRef;
            ViewBag.StartDate_SaleInvoice = StartDate;
            ViewBag.EndDate_SaleInvoice = EndDate;
            ViewBag.Customer_Name_SaleInvoice = Customer_Name;
            ViewBag.Chassis_No_SaleInvoice = Chassis_No;
            ViewBag.Status_ID_SaleInvoice = Status_ID;
            ViewBag.c_ID_SaleInvoice = c_ID;
            try
            {
                //---purchase sales master invoice list for page
                ViewBag.RecordsPerPage = 10;
                _oSales.SalesMasterIPagedList = _oSales.Get_SalesMasterInvoiceList_DAL(SaleRef, ManualRef, StartDate, EndDate, Customer_Name, Chassis_No, Status_ID, c_ID, Make, Model_Desc, Model_Year).ToPagedList(page ?? 1, 10);
                _oSales.SalesMasterListTOTAL = _oSales.Get_SalesMasterInvoiceList_TTL_DAL(SaleRef, ManualRef, StartDate, EndDate, Customer_Name, Chassis_No, Status_ID, c_ID, Make, Model_Desc, Model_Year).ToList();


                return PartialView("_SalesMasterInvoiceListSearch", _oSales);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }


        [HttpGet]
        public IActionResult SalesBookingList(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int c_ID, int? page)
        {
            //---dumping static fields
            DumpStaticFieldsSales();

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales Booking ";
            ViewBag.BreadCumAction = "Sales Booking List";

            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "Sales Booking List";
            ViewBag.PageTitle = "Sales Booking List";


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region Veiwbags area
            //---Get Vendor Master
            //ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL().ToList();

            #endregion veiwbags area ends here

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_No = String.IsNullOrEmpty(Chassis_No) ? "" : Chassis_No;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;

            ViewBag.SaleRef_Booking = SaleRef;
            ViewBag.StartDate_Booking = StartDate;
            ViewBag.EndDate_Booking = EndDate;
            ViewBag.Customer_Name_Booking = Customer_Name;
            ViewBag.Chassis_No_Booking = Chassis_No;
            ViewBag.c_ID_Booking = c_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            //---purchase sales master invoice list for page
            ViewBag.RecordsPerPage = 10;
            _oSales.SalesMasterIPagedList = _oSales.Get_SalesMasterBooking_List_DAL(SaleRef, StartDate, EndDate, Customer_Name, Chassis_No, 0, c_ID).ToPagedList(page ?? 1, 10);

            return View(_oSales);
        }

        [HttpGet]
        public IActionResult GetSalesMasterBookingListBySearchFilers(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int c_ID, int? page)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion
            c_ID = c_ID == 0 ? 1 : c_ID;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_No = String.IsNullOrEmpty(Chassis_No) ? "" : Chassis_No;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;

            ViewBag.SaleRef_Booking = SaleRef;
            ViewBag.StartDate_Booking = StartDate;
            ViewBag.EndDate_Booking = EndDate;
            ViewBag.Customer_Name_Booking = Customer_Name;
            ViewBag.Chassis_No_Booking = Chassis_No;
            ViewBag.c_ID_Booking = c_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            try
            {
                //---purchase sales master invoice list for page
                ViewBag.RecordsPerPage = 10;
                _oSales.SalesMasterIPagedList = _oSales.Get_SalesMasterBooking_List_DAL(SaleRef, StartDate, EndDate, Customer_Name, Chassis_No, 0, c_ID).ToPagedList(page ?? 1, 10);

                return PartialView("_SalesMasterBookingListSearch", _oSales);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }




        [HttpGet]
        public IActionResult PerformaInvoiceList(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int c_ID, int? page)
        {
            //---dumping static fields
            DumpStaticFieldsSales();

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "PerformaInvoiceList";


            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "Performa Invoice List";
            ViewBag.PageTitle = "Performa Invoice List";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region Veiwbags area
            //---Get Vendor Master
            //ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL().ToList();

            #endregion veiwbags area ends here


            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_No = String.IsNullOrEmpty(Chassis_No) ? "" : Chassis_No;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;

            ViewBag.SaleRef_Performa = SaleRef;
            ViewBag.StartDate_Performa = StartDate;
            ViewBag.EndDate_Performa = EndDate;
            ViewBag.Customer_Name_Performa = Customer_Name;
            ViewBag.Chassis_No_Performa = Chassis_No;
            ViewBag.c_ID_Performa = c_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            //---purchase sales master invoice list for page
            ViewBag.RecordsPerPage = 10;
            _oSales.SalesMasterIPagedList = _oSales.Get_PerformaInvoice_List_DAL(SaleRef, StartDate, EndDate, Customer_Name, Chassis_No, c_ID).ToList().ToPagedList(page ?? 1, 10);

            return View(_oSales);
        }

        [HttpGet]
        public IActionResult GetPerformaListListBySearchFilters(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int c_ID, int? page)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion
            c_ID = c_ID == 0 ? 1 : c_ID;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_No = String.IsNullOrEmpty(Chassis_No) ? "" : Chassis_No;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;

            ViewBag.SaleRef_Performa = SaleRef;
            ViewBag.StartDate_Performa = StartDate;
            ViewBag.EndDate_Performa = EndDate;
            ViewBag.Customer_Name_Performa = Customer_Name;
            ViewBag.Chassis_No_Performa = Chassis_No;
            ViewBag.c_ID_Performa = c_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            try
            {
                ViewBag.RecordsPerPage = 10;
                _oSales.SalesMasterIPagedList = _oSales.Get_PerformaInvoice_List_DAL(SaleRef, StartDate, EndDate, Customer_Name, Chassis_No, c_ID).ToList().ToPagedList(page ?? 1, 10);

                return PartialView("_SalesMasterPerformaInvoiceListSearch", _oSales);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }


        [HttpGet]
        public IActionResult SalesCancelReturnList(string SaleRef, string StartDate, string EndDate, string Customer_Name, int? c_ID, int? page)
        {
            //---dumping static fields
            DumpStaticFieldsSales();

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "SalesCancelReturnList";

            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "Sales Cancel/Return List";

            ViewBag.PageTitle = "Sales Cancel / Return List";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region Veiwbags area
            //---Get Vendor Master
            //ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL().ToList();
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            #endregion veiwbags area ends here
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);


            c_ID = c_ID == null || c_ID == 0 ? c_IDs : c_ID;

            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;


            ViewBag.SaleRef_SaleReturn = SaleRef;
            ViewBag.StartDate_SaleReturn = StartDate;
            ViewBag.EndDate_SaleReturn = EndDate;
            ViewBag.Customer_Name_SaleReturn = Customer_Name;
            ViewBag.c_ID_SaleReturn = c_ID;


            ViewBag.RecordsPerPage = 10;
            //---purchase sales master invoice list for page
            _oSales.SalesMaster_InvoiceReturnList = _oSales.Get_SalesInvoiceReturnList_DAL(SaleRef, StartDate, EndDate, Customer_Name, Convert.ToInt32(c_ID)).ToList().ToPagedList(page ?? 1, 10);

            return View(_oSales);
        }


        [HttpGet]
        public IActionResult GetSaleReturnListBySearchFilters(string SaleRef, string StartDate, string EndDate, string Customer_Name, string c_ID, int? page)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion
            int cID = Convert.ToInt32(c_ID);
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;

            ViewBag.SaleRef_SaleReturn = SaleRef;
            ViewBag.StartDate_SaleReturn = StartDate;
            ViewBag.EndDate_SaleReturn = EndDate;
            ViewBag.Customer_Name_SaleReturn = Customer_Name;
            ViewBag.c_ID_SaleReturn = c_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            try
            {
                //---purchase sales master invoice list for page
                ViewBag.RecordsPerPage = 10;
                _oSales.SalesMasterList = _oSales.Get_SalesInvoiceReturnList_DAL(SaleRef, StartDate, EndDate, Customer_Name, cID).ToList().ToList().ToPagedList(page ?? 1, 10);

                return PartialView("_SalesMasterReturnInvoiceListSearch", _oSales);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }


        [HttpGet]
        public IActionResult NewPerformaInvoice(int? SaleMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "NewPerformaInvoice";

            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "Performa Invoice";
            ViewBag.SectionSub_HeaderMainTitle = "Sales";
            ViewBag.PageTitle = "New Performa Invoice";

            ViewBag.Default_Curr_Rate = "1";
            ViewBag.Default_Currency_ID = "5";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            #region ViewBag Area
            //---Get Customer Master
            ViewBag.CustomerMaster = _oBasic.Get_CustomersList_DAL(c_ID).ToList();

            //---Ven Category list for drop down in forms
            ViewBag.CustomerCategoryList = _oBasic.Get_CustomerCategory_DAL().ToList();


            ViewBag.SellerList = _oBasic.Get_PartnersSeller_DAL(c_ID).ToList();
            ViewBag.AgentsList = _oBasic.Get_PartnersAgent_DAL(c_ID).ToList();
            ViewBag.ExportDestinations = _oBasic.Get_Destinations_DAL(c_ID).ToList();
            ViewBag.PortOfExit = _oBasic.Get_PortOfExit_DAL(c_ID).ToList();
            //---get shipping companies
            ViewBag.ShippingCompanies = _oBasic.Get_ShippingCompaniesList_DAL(c_ID).ToList();
            //--get export companies list
            ViewBag.ExportCompanies = _oBasic.Get_ExportCompaniesList_DAL(c_ID).ToList();
            //--get Basic Banks list
            ViewBag.BasicBanksList = _oBasic.Get_BasicBanksList_DAL(c_ID).ToList();
            //--get showrooms lsit
            ViewBag.ShowroomsList = _oBasic.Get_ShowRoom_DAL(c_ID).ToList();
            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();
            
            #endregion




            if (SaleMaster_ID == null || SaleMaster_ID < 1)
            {


                //---Create sales master Ref(tem id)
                if (String.IsNullOrEmpty(SalePerforma_Master_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    SalePerforma_Master_TempID = obj.ToString();
                }

                SalePerforma_MasterStatic_ID = SaleMaster_ID;

                //---Get sales master id
                ViewBag.SaleMaster_ID = SaleMaster_ID;

                //--Get sales Master
                _oSales.SaleMasterObj = _oSales.Get_SalesPerformaMasterByID_DAL(-1);


                //---sales detial invoice list for sales invoice type Bychassis
                _oSales.SalesDetailList = _oSales.Get_SalesPerformaDetailListByChassis_DAL(SalePerforma_Master_TempID, -1).ToList();
                _oSales.SalesDetailList_Specs = _oSales.Get_SalesPerformaDetailListByChassis_Specs_DAL(SalePerforma_Master_TempID, -1).ToList();


            }
            else
            {

                //---Get old temp id from sales detail table that already creaded for this "SaleMaster_ID" id. And then use this if new record added
                string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL(SaleMaster_ID);



                Urls = null;

                ApprovalURL = HttpContext.Request.Headers["Referer"];
                Urls = ApprovalURL;

                SalePerforma_MasterStatic_ID = SaleMaster_ID;

                //---Get sale master id
                ViewBag.SaleMaster_ID = SaleMaster_ID;

                //--Get sale Master

                _oSales.SaleMasterObj = _oSales.Get_SalesPerformaMasterByID_DAL(SaleMaster_ID);

                //---sales detial invoice list for sales invoice type Bychassis
                _oSales.SalesDetailList = _oSales.Get_SalesPerformaDetailListByChassis_DAL(OldTempID, SaleMaster_ID).ToList();
                _oSales.SalesDetailList_Specs = _oSales.Get_SalesPerformaDetailListByChassis_Specs_DAL(OldTempID, SaleMaster_ID).ToList();



            }



            return View(_oSales);
        }

        //---insert into sales performa detail
        [HttpPost]
        public IActionResult InsertSalesPerformaDetail(string SalesInvoiceType, string Chassis, string ItemDesc, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, int hdSaleMaster_ID
)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //---Validation area starts here
            if (SalesInvoiceType == "ByChassis")
            {
                if (String.IsNullOrEmpty(Chassis))
                {
                    return Json(new { message = "Please enter valid chassis No!" });
                }
            }
            else if (SalesInvoiceType == "ByService")
            {
                if (String.IsNullOrEmpty(ItemDesc))
                {
                    return Json(new { message = "Please enter description!" });
                }
            }
            if (Unit_Price == null)
            {
                return Json(new { message = "Please enter sale price!" });
            }
            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = SalePerforma_Master_TempID;

            if (hdSaleMaster_ID == 0)
            {
                Temp_ID = SalePerforma_Master_TempID;
            }
            else
            {
                Temp_ID = "0";
            }
            SalePerforma_MasterStatic_ID = SalePerforma_MasterStatic_ID == null ? -1 : SalePerforma_MasterStatic_ID;


            try
            {
                message = _oSales.InsertSalesPerformaDetail_DAL(SalesInvoiceType, Chassis, ItemDesc, Unit_Price, VATRate, VATExp, Total_Amount, Temp_ID, c_ID, Created_by, hdSaleMaster_ID);
                if (message == "Saved Successfully!")
                {

                    //---sales detial invoice list for sales invoice type Bychassis
                    _oSales.SalesDetailList = _oSales.Get_SalesPerformaDetailListByChassis_DAL(Temp_ID, hdSaleMaster_ID).ToList();



                    return PartialView("_SalesPerformaDetailList", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }


        //---insert into sales performa detail
        [HttpPost]
        public IActionResult InsertSalesPerformaDetail_Specs(string SalesInvoiceType, string Chassis, string ItemDesc, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, double? Quantity, int hdSaleMaster_ID
)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //---Validation area starts here
            if (SalesInvoiceType == "ByChassis")
            {
                if (String.IsNullOrEmpty(Chassis))
                {
                    return Json(new { message = "Please enter valid chassis No!" });
                }
            }
            else if (SalesInvoiceType == "ByService")
            {
                if (String.IsNullOrEmpty(ItemDesc))
                {
                    return Json(new { message = "Please enter description!" });
                }
            }
            if (Unit_Price == null)
            {
                return Json(new { message = "Please enter sale price!" });
            }
            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = SalePerforma_Master_TempID;

            if (hdSaleMaster_ID == 0)
            {
                Temp_ID = SalePerforma_Master_TempID;
            }
            else
            {
                Temp_ID = "0";
            }
            SalePerforma_MasterStatic_ID = SalePerforma_MasterStatic_ID == null ? -1 : SalePerforma_MasterStatic_ID;


            try
            {
                message = _oSales.InsertSalesPerformaDetail_Specs_DAL(SalesInvoiceType, Chassis, ItemDesc, Unit_Price, VATRate, VATExp, Total_Amount, Temp_ID, c_ID, Created_by, Quantity, hdSaleMaster_ID);
                if (message == "Saved Successfully!")
                {

                    //---sales detial invoice list for sales invoice type Bychassis
                    _oSales.SalesDetailList_Specs = _oSales.Get_SalesPerformaDetailListByChassis_Specs_DAL(Temp_ID, hdSaleMaster_ID).ToList();



                    return PartialView("_SalesPerformaDetailList", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }


        //---Insert  sales performa master
        [HttpPost]
        public IActionResult InsertSalesPerformaMaster(int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string CustomerTRN, string Manualbook_ref,
            int? Seller_ID, int? Agent_ID, int? ExportTo_Destination_ID, int? Port_of_Exit_ID, string Sale_trans_type, int? shipping_co_ID, int? ExporterCo_ID, int? Bank_to_Transfer_payment_ID, int? Showroom_ID, string Performa_Validity, int Currency_ID, string Currency_Rate)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = SalePerforma_Master_TempID;
            string Saletype = "PI";  //---It is the ref in table for new sale master. Send this value hard coded from here

            if (Customer_ID == null || String.IsNullOrEmpty(SaleDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewPerformaInvoice", "Sales");
            }

            Customer_ID = Customer_ID == null ? 0 : Customer_ID;


            try
            {
                message = _oSales.InsertSalesPerformaMaster_DAL(Customer_ID, Customer_Contact, Invoice_address, Saletype, SaleDate, CustomerTRN, Manualbook_ref, Seller_ID, Agent_ID, ExportTo_Destination_ID,
                    Port_of_Exit_ID, Sale_trans_type, shipping_co_ID, ExporterCo_ID, Bank_to_Transfer_payment_ID, Showroom_ID, Performa_Validity, Temp_ID, c_ID, Created_by, Currency_ID, Currency_Rate);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("PerformaInvoiceList", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("NewPerformaInvoice", "Sales");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("NewPerformaInvoice", "Sales");
            }


        }

        //---Insert  sales invoice master
        [HttpPost]
        public IActionResult UpdateSalesPerformaMaster(int? SaleMaster_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string CustomerTRN, string Manualbook_ref,
            int? Seller_ID, int? Agent_ID, int? ExportTo_Destination_ID, int? Port_of_Exit_ID, string Sale_trans_type, int? shipping_co_ID, int? ExporterCo_ID, int? Bank_to_Transfer_payment_ID, int? Showroom_ID, string Performa_Validity, int Currency_ID, string Currency_Rate)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = SalePerforma_Master_TempID;
            string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL(SaleMaster_ID);

            if (!String.IsNullOrEmpty(OldTempID))
            {
                Temp_ID = OldTempID;
            }
            else
            {
                if (String.IsNullOrEmpty(Temp_ID))
                {
                    Guid obj = Guid.NewGuid();
                    Temp_ID = obj.ToString();
                }
            }
            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();


            if (Customer_ID == null || String.IsNullOrEmpty(SaleDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            Customer_ID = Customer_ID == null ? 0 : Customer_ID;


            try
            {
                message = _oSales.UpdateSalesPerformaMaster_DAL(SaleMaster_ID, Customer_ID, Customer_Contact, Invoice_address, SaleDate, CustomerTRN, Manualbook_ref, Seller_ID, Agent_ID,
                    ExportTo_Destination_ID, Port_of_Exit_ID, Sale_trans_type, shipping_co_ID, ExporterCo_ID, Bank_to_Transfer_payment_ID, Showroom_ID, Performa_Validity, Temp_ID,
                    c_ID, Modified_by, Currency_ID, Currency_Rate);
                if (message == "Updated Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("PerformaInvoiceList", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }


        }


        //---Method for deleting sales master invoice
        [HttpPost]
        public IActionResult DeleteSalesPerformaMaster(int? SaleMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion
            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();



            string message = "";

            if (SaleMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }



            try
            {
                message = _oSales.DeleteSalesMaster_DAL(SaleMaster_ID);
                if (message == "Deleted Successfully!")
                {
                    return RedirectToAction("PerformaInvoiceList", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }

        //---Method for deleting sales invoice detail
        [HttpPost]
        public IActionResult DeleteSalesPerformaDetail(int? SalesDetails_ID, int hdSaleMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_id = Convert.ToInt32(company_ID);
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion
            //----getting calling page url


            string message = "";
            string Temp_ID = "";

            if (hdSaleMaster_ID == 0)
            {
                Temp_ID = SalePerforma_Master_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            try
            {
                ////add Perameter C_id by waiz for deleting Performa Invoice chassis Data/////
                message = _oSales.DeleteSalesDetail_Performa_DAL(SalesDetails_ID, c_id);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = message;
                    _oSales.SalesDetailList = _oSales.Get_SalesPerformaDetailListByChassis_DAL(Temp_ID, hdSaleMaster_ID).ToList();



                    return PartialView("_SalesPerformaDetailList", _oSales);
                }
                else
                {
                    return Json(new { message = "Record not deleted! Please try again!" });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured! Please try again!" });
            }

        }
        [HttpPost]
        public IActionResult DeleteSalesPerformaDetail_Specs(int? SalesDetails_ID, int hdSaleMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion
            //----getting calling page url


            string message = "";

            string Temp_ID = "";

            if (hdSaleMaster_ID == 0)
            {
                Temp_ID = SalePerforma_Master_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            try
            {
                message = _oSales.DeleteSalesDetail_Specs_DAL(SalesDetails_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = message;
                    _oSales.SalesDetailList_Specs = _oSales.Get_SalesPerformaDetailListByChassis_Specs_DAL(Temp_ID, hdSaleMaster_ID).ToList();



                    return PartialView("_SalesPerformaDetailList", _oSales);
                }
                else
                {
                    return Json(new { message = "Record not deleted! Please try again!" });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured! Please try again!" });
            }

        }


        //---Update  sales performa invoice detail Chassis
        [HttpPost]
        public IActionResult UpdateSalesPerformaDetailByChassis(int? SalesDetails_ID, string Chassis, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, int hdSaleMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //---Validation area starts here

            if (SalesDetails_ID == null)
            {
                return Json(new { message = "Sales detail id is null" });
            }

            if (String.IsNullOrEmpty(Chassis) || Unit_Price == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);


            Temp_ID = SalePerforma_Master_TempID;

            if (hdSaleMaster_ID == 0)
            {
                Temp_ID = SalePerforma_Master_TempID;
            }
            else
            {
                Temp_ID = "0";
            }
            SalePerforma_MasterStatic_ID = SalePerforma_MasterStatic_ID == null ? -1 : SalePerforma_MasterStatic_ID;


            try
            {
                message = _oSales.UpdateSalesPerformaDetail_DAL(SalesDetails_ID, Chassis, "", Unit_Price, VATRate, VATExp, Total_Amount, Temp_ID, c_ID, Modified_by);
                if (message == "Updated Successfully!")
                {
                    //---sales detial invoice list for sales invoice type Bychassis
                    _oSales.SalesDetailList = _oSales.Get_SalesPerformaDetailListByChassis_DAL(Temp_ID, hdSaleMaster_ID).ToList();


                    return PartialView("_SalesPerformaDetailList", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }

        //---Update into sales performa detail Service
        [HttpPost]
        public IActionResult UpdateSalesPerformaDetailByService(int? SalesDetails_ID, string ItemDesc, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, double? Quantity, int hdSaleMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //---Validation area starts here

            if (SalesDetails_ID == null)
            {
                return Json(new { message = "Sales detail id is null" });
            }

            if (String.IsNullOrEmpty(ItemDesc) || Unit_Price == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);


            Temp_ID = SalePerforma_Master_TempID;

            if (hdSaleMaster_ID == 0)
            {
                Temp_ID = SalePerforma_Master_TempID;
            }
            else
            {
                Temp_ID = "0";
            }
            SalePerforma_MasterStatic_ID = SalePerforma_MasterStatic_ID == null ? -1 : SalePerforma_MasterStatic_ID;


            try
            {
                message = _oSales.UpdateSalesPerformaDetail_Specs_DAL(SalesDetails_ID, "", ItemDesc, Unit_Price, VATRate, VATExp, Total_Amount, Temp_ID, c_ID, Modified_by, Quantity);
                if (message == "Updated Successfully!")
                {
                    //---sales detial invoice list for sales invoice type Bychassis
                    _oSales.SalesDetailList_Specs = _oSales.Get_SalesPerformaDetailListByChassis_Specs_DAL(Temp_ID, hdSaleMaster_ID).ToList();



                    return PartialView("_SalesPerformaDetailList", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }



        [HttpGet]
        public IActionResult NewSalesCancelReturn(int? SaleMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "NewSalesCancelReturn";
            ViewBag.PageTitle = "New Sales Cancel/Return";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            #region ViewBag Area
            //---Get Customer Master
            ViewBag.CustomerMaster = _oBasic.Get_CustomersMaster_DAL(c_ID).ToList();


            #endregion




            if (SaleMaster_ID == null || SaleMaster_ID < 1)
            {


                //---Create sales master Ref(tem id)
                if (String.IsNullOrEmpty(SaleCancel_Master_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    SaleCancel_Master_TempID = obj.ToString();
                }

                SaleCancel_MasterStatic_ID = SaleMaster_ID;

                //---Get sales master id
                ViewBag.SaleMaster_ID = SaleMaster_ID;

                //--Get sales Master
                _oSales.SaleMasterObj = _oSales.Get_SalesCancelReturnMasterByID_DAL(-1);


                //---sales detial cancel list of detail
                _oSales.SalesDetailList = _oSales.Get_SalesCancelDetailListByID_DAL(SaleCancel_Master_TempID, -1).ToList();


            }
            else
            {

                //---Get old temp id from sales detail table that already creaded for this "SaleMaster_ID" id. And then use this if new record added
                string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL(SaleMaster_ID);




                SaleCancel_MasterStatic_ID = SaleMaster_ID;

                //---Get sale master id
                ViewBag.SaleMaster_ID = SaleMaster_ID;

                //--Get sale Master

                _oSales.SaleMasterObj = _oSales.Get_SalesCancelReturnMasterByID_DAL(SaleMaster_ID);


                //---sales detial invoice list for sales invoice type 
                _oSales.SalesDetailList = _oSales.Get_SalesCancelDetailListByID_DAL(OldTempID, SaleMaster_ID).ToList();


            }



            return View(_oSales);
        }



        //---method for inserting return purchase record that already exists in purchase detail table and then returning the list of new insered record
        [HttpGet]
        public IActionResult Insert_And_GetSalesReturnDetail(string SaleRef)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = SaleCancel_Master_TempID;
            string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL(SaleCancel_MasterStatic_ID);
            if (!String.IsNullOrEmpty(SaleCancel_Master_TempID))
            {
                Temp_ID = SaleCancel_Master_TempID;

            }
            else
            {
                Temp_ID = OldTempID;
            }
            SaleCancel_MasterStatic_ID = SaleCancel_MasterStatic_ID == null ? -1 : SaleCancel_MasterStatic_ID;

            if (String.IsNullOrEmpty(SaleRef))
            {
                return Json(new { message = "Please enter valid purchase ref" });
            }

            try
            {
                message = _oSales.CheckIfRefExistInSalesMaster_DAL(SaleRef);

                if (message == "Ref Exists")
                {

                    message = _oSales.InsertSalesDetails_CancelReturn(Temp_ID, SaleRef);
                    if (message == "Saved Successfully!")
                    {

                        //---sales detial invoice list for sales invoice type Bychassis
                        _oSales.SalesDetailList = _oSales.Get_SalesCancelDetailListByID_DAL(Temp_ID, SaleCancel_MasterStatic_ID).ToList();

                        return PartialView("_SalesCancelReturnDetailList", _oSales);

                    }
                    else
                    {
                        return Json(new { message = message });
                    }


                }

                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }



        }


        //---Method for deleting sales cancel detail
        [HttpPost]
        public IActionResult DeleteSalesDetail_CancelReturn(int SalesDetails_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);


            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion
            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();

            string message = "";

            if (SalesDetails_ID == 0)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }



            try
            {
                message = _oSales.DeleteSalesDetail_DAL(SalesDetails_ID,c_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = message;
                    return Redirect(url);
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }

        //---Method for deleting sales master invoice
        [HttpPost]
        public IActionResult DeleteSalesMaster_CancelReturn(int? SaleMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion
            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();



            string message = "";

            if (SaleMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }



            try
            {
                message = _oSales.DeleteSalesMaster_DAL(SaleMaster_ID);
                if (message == "Deleted Successfully!")
                {
                    return RedirectToAction("SalesCancelReturnList", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }

        //---Insert  sales Cancel Return master
        [HttpPost]
        public IActionResult InsertSalesMaster_CancelReturn(int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = SaleCancel_Master_TempID;
            string Saletype = "SX";  //---It is the ref in table for new sale master. Send this value hard coded from here

            if (Customer_ID == null || String.IsNullOrEmpty(SaleDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewSalesCancelReturn", "Sales");
            }

            Customer_ID = Customer_ID == null ? 0 : Customer_ID;


            try
            {
                message = _oSales.InsertSalesMaster_CancelReturn_DAL(Customer_ID, Customer_Contact, Invoice_address, Saletype, SaleDate, Temp_ID, c_ID, Created_by);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("SalesCancelReturnList", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("NewSalesCancelReturn", "Sales");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("NewSalesCancelReturn", "Sales");
            }


        }

        //---Update  sales Cancel Return master
        [HttpPost]
        public IActionResult UpdateSalesMaster__CancelReturn(int? SaleMaster_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = SaleCancel_Master_TempID;
            string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL(SaleMaster_ID);
            if (!String.IsNullOrEmpty(OldTempID))
            {
                Temp_ID = OldTempID;
            }
            else
            {
                if (String.IsNullOrEmpty(Temp_ID))
                {
                    Guid obj = Guid.NewGuid();
                    Temp_ID = obj.ToString();
                }
            }
            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();


            if (Customer_ID == null || String.IsNullOrEmpty(SaleDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            Customer_ID = Customer_ID == null ? 0 : Customer_ID;


            try
            {
                message = _oSales.UpdateSalesMaster_CancelReturn_DAL(SaleMaster_ID, Customer_ID, Customer_Contact, Invoice_address, SaleDate, Temp_ID, c_ID, Modified_by);
                if (message == "Updated Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("SalesCancelReturnList", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }


        }


        [HttpPost]
        public IActionResult ChangeSalesMaster_CancelReturnStatus(int? SaleMaster_ID, int? Status_ID, string Trans_Type)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion

            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();


            if (SaleMaster_ID == null || Status_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oSales.ChangeSalesMasterStatus_DAL(SaleMaster_ID, Status_ID, c_ID, Trans_Type, Modified_by,"");
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = message;
                    return Redirect(url);
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }

        //---Get sales invoice attachment list 
        [HttpGet]
        public IActionResult GetSalesMasterInvoice_Attachments(int? SaleMaster_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //--send this value hard coded from here for getting attachment for sales master invoice
            string Type = "Sales_Invoice";

            try
            {
                _oSales.attachmentList = _oSales.GetSalesInvoiceMaster_Attachments_DAL(SaleMaster_ID, Type).ToList();

                return PartialView("_SalesInvoiceAttachment", _oSales);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }
        }
        [HttpGet]
        public IActionResult GetVehicleMaster_Attachments(int? vehicle_display_master_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //--send this value hard coded from here for getting attachment for sales master invoice
            string Type = "Vehicle_Display";

            try
            {
                _oSales.attachmentList = _oSales.GetSalesInvoiceMaster_Attachments_DAL(vehicle_display_master_ID, Type).ToList();

                return PartialView("_VehicleDisplayAttachment", _oSales);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }
        }



        [HttpPost]
        public IActionResult InsertAttachments_SalesInvoiceMaster(int? SaleMaster_ID, string Type, string Document_Type, IFormFile Attachment)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            if (SaleMaster_ID == null || String.IsNullOrEmpty(Document_Type) || Attachment == null)
            {

                return Json(new { message = "Please fill all required fields!" });
            }

            var newFileName = "";
            var filepath = "";
            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            #region saving file
            if (Attachment != null)
            {
                if (Attachment.Length > 0)
                {
                    try
                    {

                        //string Image = Path.Combine(Server.MapPath(filePath), imgname);


                        if (Attachment.Length > 0)
                        {
                            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                            //Getting FileName
                            var fileName = Path.GetFileName(Attachment.FileName);

                            //Assigning Unique Filename (Guid)
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                            //Getting file Extension
                            var fileExtension = Path.GetExtension(fileName);

                            // concatenating  FileName + FileExtension
                            newFileName = String.Concat(myUniqueFileName, fileExtension);

                            // Combines two strings into a path.

                            filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{newFileName}";

                            using (FileStream fs = System.IO.File.Create(filepath))
                            {
                                Attachment.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        return Json(new { message = ErrorMessage });
                    }

                }

            }


            #endregion

            //---saving attachment in database
            try
            {

                message = _oSales.Insert_Attachment_SalesInvoice_DAL(SaleMaster_ID, Document_Type, Type, newFileName, filepath, Created_by);
                if (message == "Saved Successfully!")
                {

                    _oSales.attachmentList = _oSales.GetSalesInvoiceMaster_Attachments_DAL(SaleMaster_ID, Type).ToList();

                    return PartialView("_SalesInvoiceAttachment", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                return Json(new { message = ErrorMessage });
            }

            //return View();
        }

        [HttpPost]
        public IActionResult InsertAttachments_VehicleDisplayMaster(int? vehicle_display_master_ID, string Type, string Document_Type, IFormFile Attachment)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            if (vehicle_display_master_ID == null || String.IsNullOrEmpty(Document_Type) || Attachment == null)
            {

                return Json(new { message = "Please fill all required fields!" });
            }

            var newFileName = "";
            var filepath = "";
            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            #region saving file
            if (Attachment != null)
            {
                if (Attachment.Length > 0)
                {
                    try
                    {

                        //string Image = Path.Combine(Server.MapPath(filePath), imgname);


                        if (Attachment.Length > 0)
                        {
                            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                            //Getting FileName
                            var fileName = Path.GetFileName(Attachment.FileName);

                            //Assigning Unique Filename (Guid)
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                            //Getting file Extension
                            var fileExtension = Path.GetExtension(fileName);

                            // concatenating  FileName + FileExtension
                            newFileName = String.Concat(myUniqueFileName, fileExtension);

                            // Combines two strings into a path.

                            filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{newFileName}";

                            using (FileStream fs = System.IO.File.Create(filepath))
                            {
                                Attachment.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        return Json(new { message = ErrorMessage });
                    }

                }

            }


            #endregion

            //---saving attachment in database
            try
            {

                message = _oSales.Insert_Attachment_SalesInvoice_DAL(vehicle_display_master_ID, Document_Type, Type, newFileName, filepath, Created_by);
                if (message == "Saved Successfully!")
                {

                    _oSales.attachmentList = _oSales.GetSalesInvoiceMaster_Attachments_DAL(vehicle_display_master_ID, Type).ToList();

                    return PartialView("_VehicleDisplayAttachment", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                return Json(new { message = ErrorMessage });
            }

            //return View();
        }



        [HttpGet]
        public IActionResult DeleteAttachmentSalesInvoice(int? SaleMaster_ID, int? Attachment_ID, string Type, string File_Name)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            string message = "";

            if (SaleMaster_ID == null)
            {
                //TempData["Error"] = "Please fill all required fields!";
                return Json(new { message = "Sales master id is null" });
            }

            if (Attachment_ID == null)
            {

                return Json(new { message = "Attachment_ID is null" });
            }
            if (String.IsNullOrEmpty(Type))
            {

                return Json(new { message = "Document type is null" });
            }


            #region Deleting actual file from folder
            try
            {
                string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                string filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{File_Name}";

                if ((System.IO.File.Exists(filepath)))
                {
                    System.IO.File.Delete(filepath);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });

            }
            #endregion



            try
            {

                message = _oSales.Delete_Attachment(Attachment_ID);
                if (message == "Deleted Successfully!")
                {

                    _oSales.attachmentList = _oSales.GetSalesInvoiceMaster_Attachments_DAL(SaleMaster_ID, Type).ToList();

                    return PartialView("_SalesInvoiceAttachment", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                return Json(new { message = ErrorMessage });
            }

            //return View();
        }

        [HttpGet]
        public IActionResult DeleteAttachmentVehicleDisplay(int? vehicle_display_master_ID, int? Attachment_ID, string Type, string File_Name)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            string message = "";

            if (vehicle_display_master_ID == null)
            {
                //TempData["Error"] = "Please fill all required fields!";
                return Json(new { message = "Vehicle master id is null" });
            }

            if (Attachment_ID == null)
            {

                return Json(new { message = "Attachment_ID is null" });
            }
            if (String.IsNullOrEmpty(Type))
            {

                return Json(new { message = "Document type is null" });
            }


            #region Deleting actual file from folder
            try
            {
                string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                string filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{File_Name}";

                if ((System.IO.File.Exists(filepath)))
                {
                    System.IO.File.Delete(filepath);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });

            }
            #endregion



            try
            {

                message = _oSales.Delete_Attachment(Attachment_ID);
                if (message == "Deleted Successfully!")
                {

                    _oSales.attachmentList = _oSales.GetSalesInvoiceMaster_Attachments_DAL(vehicle_display_master_ID, Type).ToList();

                    return PartialView("_VehicleDisplayAttachment", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                return Json(new { message = ErrorMessage });
            }

            //return View();
        }


        //----dumping static ids, and temp fields
        public void DumpStaticFieldsSales()
        {
            SalesInvoiceMaster_TempID = null;
            SaleInvoiceMasterStatic_ID = null;

            VehicleDisplayStaticTempID = null;
            VehicleDisplayMasterStatic_ID = null;

            SalesBookingMaster_TempID = null;
            SaleBookingMasterStatic_ID = null;

            SalePerforma_Master_TempID = null;
            SalePerforma_MasterStatic_ID = null;

            SaleCancel_Master_TempID = null;
            SaleCancel_MasterStatic_ID = null;

            SalesMasterJPMaster_TempID = null;
            SalesMasterJPStatic_ID = null;

            DOOtherTempID = null;
            DOMasterOtherStatic_ID = null;

            PaperAfterSaleTempID= null;
            PaperAfterSaleStatic_ID= null;

        }


        #region Delivery New/Update











        [HttpGet]
        public IActionResult VehicleDisplay(int? vehicle_display_master_ID)
        {
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            #region ViewBag Area

            ViewBag.CarLocationList = _oSales.Get_CarLocation_DAL(c_ID).ToList();

            ViewBag.Emp_NameList = _oSales.Get_Emp_Name_DAL(c_ID).ToList();




            ViewBag.MaxRef = _oSales.Get_MaxRef().Ref;
            #endregion
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "VehicleDisplay";
            ViewBag.BreadCumAction = "VehicleDisplayList";

            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "Vehicle Display List";
            ViewBag.PageTitle = "Vehicle Display List";


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (vehicle_display_master_ID == null || vehicle_display_master_ID < 1)
            {


                //---Create Purchase master Ref(tem id)
                if (String.IsNullOrEmpty(VehicleDisplayStaticTempID))
                {
                    Guid obj = Guid.NewGuid();
                    VehicleDisplayStaticTempID = obj.ToString();
                }


                _oSales.VehicleDisplayMasterObj = _oSales.Get_vehicle_display_master_DAL_Id(-1);
                _oSales.VehicleDisplayDetailList = _oSales.Get_vehicle_display_DetailListByID_DAL(-1, VehicleDisplayStaticTempID);


            }
            else
            {

                //---Get old temp id from purchase detail table that already creaded for this "SalesMaster_ID" id. And then use this if new record added
                string OldTempID = _oSales.GetOldTempIDFromDisplayDetail_DAL(vehicle_display_master_ID);

                if (!String.IsNullOrEmpty(OldTempID))
                {
                    VehicleDisplayStaticTempID = OldTempID;
                }
                else
                {
                    if (String.IsNullOrEmpty(VehicleDisplayStaticTempID))
                    {
                        Guid obj = Guid.NewGuid();
                        VehicleDisplayStaticTempID = obj.ToString();
                    }
                }


                VehicleDisplayMasterStatic_ID = vehicle_display_master_ID;

                //---Get purchase master id
                ViewBag.VehicleDisplayMaster_ID = vehicle_display_master_ID;




                //--Get purchase Master


                _oSales.VehicleDisplayMasterObj = _oSales.Get_vehicle_display_master_DAL_Id(vehicle_display_master_ID);
                _oSales.VehicleDisplayDetailList = _oSales.Get_vehicle_display_DetailListByID_DAL(vehicle_display_master_ID, "").ToList();
            }


            return View(_oSales);
        }
        public IActionResult VehicleDisplayList(string Ref, string StartDate, string EndDate, string Chassis, int? page)
        {
            //---dumping static fields
            DumpStaticFieldsSales();


            #region ViewBag Area
            //---Ven Category list for drop down in forms

            #endregion
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "VehicleDisplayList";
            ViewBag.PageTitle = "Vehicle Display List";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);


            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;
            Chassis = String.IsNullOrEmpty(Chassis) ? "" : Chassis;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;


            ViewBag.Ref_VehicleDisplay = Ref;
            ViewBag.StartDate_VehicleDisplay = StartDate;
            ViewBag.EndDate_VehicleDisplay = EndDate;
            ViewBag.Chassis_VehicleDisplay = Chassis;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.RecordsPerPage = 10;
            _oSales.VehicleDisplayMasterList = _oSales.Get_vehicle_display_master_DAL(Ref, StartDate, EndDate, Chassis, c_ID).ToList().ToPagedList(page ?? 1, 10);
            //---Partners list for page



            return View(_oSales);
        }
        public IActionResult VehicleDisplayList_Search(string Ref, string StartDate, string EndDate, string Chassis, int? page)
        {
            //---dumping static fields
            DumpStaticFieldsSales();


            #region ViewBag Area
            //---Ven Category list for drop down in forms

            #endregion
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "VehicleDisplay";
            ViewBag.BreadCumAction = "VehicleDisplayList";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);

            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;
            Chassis = String.IsNullOrEmpty(Chassis) ? "" : Chassis;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;

            ViewBag.Ref_VehicleDisplay = Ref;
            ViewBag.StartDate_VehicleDisplay = StartDate;
            ViewBag.EndDate_VehicleDisplay = EndDate;
            ViewBag.Chassis_VehicleDisplay = Chassis;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            try
            {
                ViewBag.RecordsPerPage = 10;
                _oSales.VehicleDisplayMasterList = _oSales.Get_vehicle_display_master_DAL(Ref, StartDate, EndDate, Chassis, c_ID).ToList().ToPagedList(page ?? 1, 10);
                return PartialView("_VehicleDisplaySearchList", _oSales);
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new
                {
                    message = ErrorMessage
                });
            }
        }
        [HttpPost]
        public IActionResult AddVehicleDisplayMaster(string Delivery_DateTime, string Showroom_Loc_ID, string delivered_by_EmpID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);


            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (String.IsNullOrEmpty(delivered_by_EmpID))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("VehicleDisplay", "Sales");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            string temp_ID = VehicleDisplayStaticTempID;



            //---saving CustomersMaster in database
            try
            {
                message = _oSales.Insert_vehicle_display_master_DAL(Delivery_DateTime, Showroom_Loc_ID, delivered_by_EmpID, temp_ID, Created_by, c_ID);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("VehicleDisplayList", "Sales");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("VehicleDisplay", "Sales");
            }

            //return View();
        }

        //---method for deleting CustomersMaster
        [HttpPost]
        public IActionResult DeleteVehicleDisplayMaster(int? vehicle_display_master_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (vehicle_display_master_ID == null || vehicle_display_master_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("VehicleDisplayList", "Sales");
            }
            string message = "";



            try
            {
                message = _oSales.Delete_vehicle_display_master_DAL(vehicle_display_master_ID);
                if (message == "Save Successfully!")
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("VehicleDisplayList", "Sales");
                }
                else
                {
                    TempData["Error"] = "Some thing went wrong. Please try again!";
                    return RedirectToAction("VehicleDisplayList", "Sales");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("VehicleDisplay", "Sales");

            }


        }


        //method for update CustomersMaster
        [HttpPost]
        public IActionResult UpdateVehicleDisplayMaster(int? vehicle_display_master_ID, string Delivery_DateTime, string Showroom_Loc_ID, string delivered_by_EmpID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(delivered_by_EmpID) || vehicle_display_master_ID == null || vehicle_display_master_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("VehicleDisplayList", "Sales");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            string temp_ID = VehicleDisplayStaticTempID;


            //---saving CustomersMaster in database
            try
            {
                message = _oSales.Update_vehicle_display_master_DAL(vehicle_display_master_ID, Delivery_DateTime,
                    Showroom_Loc_ID, delivered_by_EmpID, Modified_by, c_ID);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("VehicleDisplayList", "Sales");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("VehicleDisplay", "Sales");
            }

            //return View();
        }

        [HttpPost]
        public IActionResult AddVehicleDisplayDetail(string chassis_no, string return_Date)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);

            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            string message = "";
            if (String.IsNullOrEmpty(chassis_no))
            {
                message = "Please Add Chassis No!";

                return Json(new { message = message });
            }



            int? Created_by = Convert.ToInt32(UserID_Admin);

            string temp_ID = "";
            temp_ID = VehicleDisplayStaticTempID;
            if (string.IsNullOrEmpty(temp_ID))
            { temp_ID = ""; }
            int? masterId = 0;
            masterId = VehicleDisplayMasterStatic_ID;
            if (masterId == null)
            {
                masterId = 0;
            }

            //---saving CustomersDetail in database
            try
            {
                message = _oSales.Insert_vehicle_display_detail_DAL(masterId, chassis_no, return_Date, temp_ID, Created_by, c_ID);
                if (message == "Save Successfully!")
                {

                    _oSales.VehicleDisplayDetailList = _oSales.Get_vehicle_display_DetailListByID_DAL(masterId, temp_ID).ToList();

                    return PartialView("_VehDisplayPartial", _oSales);
                }
                else
                {

                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }

            //return View();
        }

        //---method for deleting CustomersDetail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteVehicleDisplayDetail(int? vehicle_display_details_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            string Temp_ID = "";
            Temp_ID = VehicleDisplayStaticTempID;
            VehicleDisplayMasterStatic_ID = VehicleDisplayMasterStatic_ID == null ? -1 : VehicleDisplayMasterStatic_ID;
            if (vehicle_display_details_ID == null || vehicle_display_details_ID < 1)
            {
                return Json(new { message = "ID is null. Please try again!" });
            }
            string message = "";



            try
            {

                message = _oSales.Delete_vehicle_display_detail_DAL(vehicle_display_details_ID);
                if (message == "Deleted Successfully!")
                {

                    _oSales.VehicleDisplayDetailList = _oSales.Get_vehicle_display_DetailListByID_DAL(VehicleDisplayMasterStatic_ID, Temp_ID).ToList();

                    return PartialView("_VehDisplayPartial", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                return Json(new { message = errorMessage });

            }


        }


        //method for update CustomersDetail
        [HttpPost]
        public IActionResult UpdateVehicleDisplayDetail(int? vehicle_display_master_ID, int? vehicle_display_details_ID, string chassis_no, string return_Date)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(chassis_no) || vehicle_display_details_ID == null || vehicle_display_details_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return PartialView("_VehDisplayPartial", _oSales);
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);



            //---saving CustomersDetail in database
            try
            {
                message = _oSales.Update_vehicle_display_detail_DAL(vehicle_display_master_ID, vehicle_display_details_ID, chassis_no, return_Date, Modified_by);
                if (message == "Save Successfully!")
                {
                    string temp_ID = "";
                    temp_ID = VehicleDisplayStaticTempID;
                    if (string.IsNullOrEmpty(temp_ID))
                    { temp_ID = ""; }

                    _oSales.VehicleDisplayDetailList = _oSales.Get_vehicle_display_DetailListByID_DAL(VehicleDisplayMasterStatic_ID, temp_ID).ToList();

                    return PartialView("_VehDisplayPartial", _oSales);
                }

                else { return Json(new { message = message }); }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }

            //return View();
        }
        [HttpGet]
        public JsonResult GetCustomerDetail(int? Customer_ID)
        {
            string ContactNumber = "";
            string ContactAddress = "";
            string TRN = "";


            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function
            _oBasic.GetCustomerObj = _oBasic.GetCustomerDetail_DAL(Customer_ID);

            if (!String.IsNullOrEmpty(_oBasic.GetCustomerObj.CustomerName))
            {

                ContactNumber = _oBasic.GetCustomerObj.ContactNumber;
                ContactAddress = _oBasic.GetCustomerObj.ContactAddress;
                TRN = _oBasic.GetCustomerObj.TRN;

            }
            else
            {
                ContactNumber = null;
                ContactAddress = null;
                TRN = null;
            }


            return Json(new { ContactNumber, ContactAddress, TRN });

        }


        [HttpGet]
        public JsonResult GetSalesDetail(int? Stock_ID)
        {

            string CustomerName = "";
            string Customer_Contact = "";
            string SaleDate = "";
            string Total_Amt = "";
            string Paid_amt = "";
            string Bal_due = "";
            string ExportTo_Destination_ID = "";
            string Port_of_Exit_ID = "";
            string PortFrom = "";
            string PortTO = "";
            string Destination = "";

            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function
            _oSales.GetSalesObj_Stock = _oSales.GetSalesDetail_DAL(Stock_ID);

            if (!String.IsNullOrEmpty(_oSales.GetSalesObj_Stock.CustomerName))
            {

                CustomerName = _oSales.GetSalesObj_Stock.CustomerName;
                Customer_Contact = _oSales.GetSalesObj_Stock.Customer_Contact;
                SaleDate = _oSales.GetSalesObj_Stock.SaleDate;
                Total_Amt = _oSales.GetSalesObj_Stock.Total_Amt;
                Paid_amt = _oSales.GetSalesObj_Stock.Paid_amt;
                Bal_due = _oSales.GetSalesObj_Stock.Bal_due;
                ExportTo_Destination_ID = _oSales.GetSalesObj_Stock.ExportTo_Destination_ID;
                Port_of_Exit_ID = _oSales.GetSalesObj_Stock.Port_of_Exit_ID;
                PortFrom = _oSales.GetSalesObj_Stock.PortFrom;
                PortTO = _oSales.GetSalesObj_Stock.PortTO;
                Destination = _oSales.GetSalesObj_Stock.Destination;

            }
            else
            {
                CustomerName = null;
                Customer_Contact = null;
                SaleDate = null;
                Total_Amt = null;
                Paid_amt = null;
                Bal_due = null;
                ExportTo_Destination_ID = null;
                Port_of_Exit_ID = null;
                PortFrom = null;
                PortTO = null;
                Destination = null;
            }


            return Json(new { CustomerName, Customer_Contact, SaleDate, Total_Amt, Paid_amt, Bal_due, ExportTo_Destination_ID, Port_of_Exit_ID , PortFrom , PortTO , Destination });

        }
        #endregion



        #region Sales JP 

        [HttpGet]
        public IActionResult NewSalesMasterJP(int? Sales_ID)
        {
            //---BreadCrumbs defintion

            ViewBag.BreadCumAction = "NewSalesMasterJP";

            ViewBag.BreadCumController = "Sales";


            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "Sales Invoice";
            ViewBag.SectionSub_HeaderMainTitle = "Sales";

            ViewBag.PageTitle = "Sales Invoice";




            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            #region ViewBag Area
            ViewBag.CustomerMaster = _oBasic.Get_CustomersList_DAL(c_ID).ToList();
            //---Ven Category list for drop down in forms
            ViewBag.CustomerCategoryList = _oBasic.Get_CustomerCategory_DAL().ToList();

            ViewBag.BasicBanksList = _oBasic.Get_BasicBanksList_DAL(c_ID).ToList();

            #endregion




            if (Sales_ID == null || Sales_ID < 1)
            {

                //---Create payment master Ref(tem id)
                if (String.IsNullOrEmpty(SalesMasterJPMaster_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    SalesMasterJPMaster_TempID = obj.ToString();
                }

                SalesMasterJPStatic_ID = Sales_ID;

                //---Get Sales master id
                ViewBag.SalesMaster_ID = Sales_ID;
                _oSales.SalesMasterRefJP = _oSales.SaleLoadRef();
                //--Get Sales Master
                _oSales.SalesMasterObjJP = _oSales.Get_SalesMasterJPMasterByID_DAL(-1);


                //---Sales detial  list
                _oSales.SalesDetailListJP = _oSales.Get_SalesMasterJPDetailListByID_DAL(SalesMasterJPMaster_TempID, -1).ToList();


            }
            else
            {

                //---Get old temp id from Sales detail table that already creaded for this "SalesMaster_ID" id. And then use this if new record added
                string OldTempID = _oSales.GetOldTempIDFromSaleDetail_DAL(Sales_ID);




                SalesMasterJPStatic_ID = Sales_ID;

                //---Get sale master id
                ViewBag.SalesMaster_ID = Sales_ID;

                //--Get sale Master

                _oSales.SalesMasterObjJP = _oSales.Get_SalesMasterJPMasterByID_DAL(Sales_ID);

                //---Sales detail  list 
                _oSales.SalesDetailListJP = _oSales.Get_SalesMasterJPDetailListByID_DAL(OldTempID, Sales_ID).ToList();

                //---Sale Detail Grand Totoal
                _oSales.SaleDetailGrandTotals = _oSales.pa_select_saledetails_Total(Sales_ID.ToString());

            }



            return View(_oSales);
        }

        //---insert into Sales  detial
        [HttpPost]
        public IActionResult InsertSalesMasterJPDetail(string Chassis, string Ref,
             string Price, string PriceRate, string PriceTax, string RecycleFee,
            string auctionfee, string auctionfeetax, string NumberPlate, string OfficeCharges, string OfficeChargesTax, int hdSalesMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //---Validation area starts here

            if (Price == null || String.IsNullOrEmpty(Chassis))
            {
                return Json(new { message = "Please enter  price!" });
            }
            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = SalesMasterJPMaster_TempID;
            string OldTempID = _oSales.GetOldTempIDFromSaleDetail_DAL(SalesMasterJPStatic_ID);
            if (hdSalesMaster_ID == 0)
            {
                Temp_ID = SalesMasterJPMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }
            SalesMasterJPStatic_ID = SalesMasterJPStatic_ID == null ? -1 : SalesMasterJPStatic_ID;


            try
            {
                message = _oSales.InsertSalesMasterJPDetail_DAL(Chassis, Ref,
              Price, PriceRate, PriceTax, RecycleFee,
             auctionfee, auctionfeetax, NumberPlate, OfficeCharges, OfficeChargesTax, hdSalesMaster_ID, Temp_ID, c_ID, Created_by);
                if (message == "Added successfully!")
                {
                    //---Sales detial  list 
                    _oSales.SalesDetailListJP = _oSales.Get_SalesMasterJPDetailListByID_DAL(Temp_ID, hdSalesMaster_ID).ToList();


                    return PartialView("_SalesMasterJPDetailList", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }

        //---method for inserting Sales  detail
        [HttpPost]
        public IActionResult UpdateSalesMasterJPDetail(int? SaleDetailID, string Chassis,
             string Price, string PriceRate, string PriceTax, string RecycleFee,
            string auctionfee, string auctionfeetax, string NumberPlate, string OfficeCharges, string OfficeChargesTax, int hdSalesMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //---Validation area starts here
            if (SaleDetailID == null)
            {
                return Json(new { message = "Sale Id is null" });
            }
            if (Price == null || String.IsNullOrEmpty(Chassis))
            {
                return Json(new { message = "Please enter Sales price!" });
            }
            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = SalesMasterJPMaster_TempID;
            string OldTempID = _oSales.GetOldTempIDFromSaleDetail_DAL(SalesMasterJPStatic_ID);
            if (hdSalesMaster_ID == 0)
            {
                Temp_ID = SalesMasterJPMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }
            SalesMasterJPStatic_ID = SalesMasterJPStatic_ID == null ? -1 : SalesMasterJPStatic_ID;


            try
            {
                message = _oSales.UpdateSalesMasterJPDetail_DAL(SaleDetailID, Chassis,
              Price, PriceRate, PriceTax, RecycleFee,
             auctionfee, auctionfeetax, NumberPlate, OfficeCharges, OfficeChargesTax, Temp_ID, c_ID, Modified_by);
                if (message == "Saved Successfully!")
                {
                    //---Sales detial  list 
                    _oSales.SalesDetailListJP = _oSales.Get_SalesMasterJPDetailListByID_DAL(Temp_ID, hdSalesMaster_ID).ToList();


                    return PartialView("_SalesMasterJPDetailList", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }

        //---Insert  Sales master 
        [HttpPost]
        public IActionResult InsertSalesMasterJPMaster(int CustomerName, string Ref, string SaleRef, string Date, string ManualRef,
            string SaleDate, string CustomerContact, string CustomerAddress, string SalesType,int Bank_to_Transfer_payment_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);

            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);

            Temp_ID = SalesMasterJPMaster_TempID;
            string Saletype = "SOJP";  //---It is the ref in table for new Sales master. Send this value hard coded from here

            if (CustomerName == 0 || String.IsNullOrEmpty(SaleDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewSalesMasterJP", "Sales");
            }




            try
            {
                message = _oSales.InsertSalesMasterJPMaster_DAL(CustomerName, Ref, SaleRef, Date, ManualRef, SaleDate, CustomerContact, CustomerAddress, SalesType, Saletype, Temp_ID, c_ID, Created_by, Bank_to_Transfer_payment_ID);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("SalesListJP", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("NewSalesMasterJP", "Sales");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("NewSalesMasterJP", "Sales");
            }


        }

        //---Insert  Sales master 
        [HttpPost]
        public IActionResult UpdateSalesMasterJPMaster(string SaleRef, int? Sale_ID, string Date, string CustomerName, string Ref, int? ID, string CustomerContact, string CustomerAddress, string SalesType, string ManualRef,int Bank_to_Transfer_payment_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = SalesMasterJPMaster_TempID;
            string OldTempID = _oSales.GetOldTempIDFromSaleDetail_DAL(Sale_ID);
            if (!String.IsNullOrEmpty(OldTempID))
            {
                Temp_ID = OldTempID;
            }
            else
            {
                if (String.IsNullOrEmpty(Temp_ID))
                {
                    Guid obj = Guid.NewGuid();
                    Temp_ID = obj.ToString();
                }
            }
            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();

            if (CustomerName == null || String.IsNullOrEmpty(Date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }




            try
            {
                message = _oSales.UpdateSalesMasterJPMaster_DAL(SaleRef, Sale_ID, Date, CustomerName, Ref, ID, CustomerContact, CustomerAddress, SalesType, ManualRef, Temp_ID, c_ID, Modified_by, Bank_to_Transfer_payment_ID);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("SalesListJP", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }


        }



        //---Method for deleting Sales  master
        [HttpPost]
        public IActionResult DeleteSalesMasterJPMaster(int? SaleMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion
            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();



            string message = "";

            if (SaleMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }



            try
            {
                message = _oSales.DeleteSalesMasterJP_DAL(SaleMaster_ID);
                if (message == "SUCCESSFULLY Sales DELETED")
                {
                    return RedirectToAction("SalesListJP", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }


        [HttpPost]
        public IActionResult Cancel_SalesJP(int? SaleMaster_ID,int Status_ID, string Trans_Type)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            int Modify_ID = Convert.ToInt32(UserID_Admin);

            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion
            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();



            string message = "";

            if (SaleMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oSales.Cancel_SaleJP(SaleMaster_ID,Status_ID,c_ID,Trans_Type, Modify_ID);
                if (message == "Saved Successfully!")
                {
                    return RedirectToAction("SalesListJP", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }
        [HttpPost]
        public IActionResult DeleteSalesDetail(int? StockDetails_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            Temp_ID = SalesMasterJPMaster_TempID;
            string OldTempID = _oSales.GetOldTempIDFromSaleDetail_DAL(SalesMasterJPStatic_ID);
            if (!String.IsNullOrEmpty(SalesMasterJPMaster_TempID))
            {
                Temp_ID = SalesMasterJPMaster_TempID;

            }
            else
            {
                Temp_ID = OldTempID;
            }
            SalesMasterJPStatic_ID = SalesMasterJPStatic_ID == null ? -1 : SalesMasterJPStatic_ID;

            if (SalesMasterJPStatic_ID == null)
            {
                return Json(new { message = "ID is null. Please try again!" });
            }

            try
            {
                message = _oSales.DeleteSaleDetail_DAL(StockDetails_ID);
                if (message == "Deleted Successfully!")
                {
                    _oSales.SalesDetailListJP = _oSales.Get_SalesMasterJPDetailListByID_DAL(Temp_ID, SalesMasterJPStatic_ID).ToList();


                    return PartialView("_SalesMasterJPDetailList", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }

        }


        [HttpGet]
        public IActionResult SalesListJP(string sale_Ref, string endDate, string startDate, int customerName, int? page)
        {



            //string sale_Ref, string customerName, string startDate, string endDate
            //---Dumping static fields
            DumpStaticFieldsSales();





            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "SalesList";
            ViewBag.PageTitle = "Sales List";

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            ViewBag.CustomerMaster = _oBasic.Get_CustomersList_DAL(c_ID).ToList();





            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion






            //string sale_Ref = HttpContext.Request.Query["sale_Ref"].ToString();
            //if (String.IsNullOrEmpty(sale_Ref))
            //{
            //    sale_Ref = "";
            //}



            //string startDate = HttpContext.Request.Query["startDate"].ToString();
            //if (String.IsNullOrEmpty(startDate))
            //{
            //    startDate = "";
            //}



            //string endDate = HttpContext.Request.Query["endDate"].ToString();
            //if (String.IsNullOrEmpty(endDate))
            //{
            //    endDate = "";
            //}



            //string customerName = HttpContext.Request.Query["customerName"].ToString();
            //if (String.IsNullOrEmpty(customerName))
            //{
            //    customerName = "";
            //}




            sale_Ref = String.IsNullOrEmpty(sale_Ref) ? "" : sale_Ref;



            startDate = String.IsNullOrEmpty(startDate) ? "" : startDate;
            endDate = String.IsNullOrEmpty(endDate) ? "" : endDate;




            ViewBag.sale_Ref_SalesListJP = sale_Ref;
            ViewBag.startDate_SalesListJP = startDate;
            ViewBag.endDate_SalesListJP = endDate;
            ViewBag.customerName_SalesListJP = customerName;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;



            //---Sales master list for page
            ViewBag.RecordsPerPage = 10;



            _oSales.SalesMasterListJP = _oSales.pa_Select_SalesMaster(sale_Ref, customerName, startDate, endDate).ToPagedList(page ?? 1, 10);
            _oSales.SalesMasterListJP_total = _oSales.pa_Select_SalesMaster_total(sale_Ref, customerName, startDate, endDate).ToList();






            return View(_oSales);
        }



        [HttpGet]
        public IActionResult SalesListJP_FilterSearch(string sale_Ref, string endDate, string startDate, int customerName, int? page)
        {



            //string sale_Ref, string customerName, string startDate, string endDate
            //---Dumping static fields
            DumpStaticFieldsSales();





            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "SalesList";
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion


            ViewBag.CustomerMaster = _oBasic.Get_CustomersList_DAL(c_ID).ToList();




            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion






            //string sale_Ref = HttpContext.Request.Query["sale_Ref"].ToString();
            //if (String.IsNullOrEmpty(sale_Ref))
            //{
            //    sale_Ref = "";
            //}



            //string startDate = HttpContext.Request.Query["startDate"].ToString();
            //if (String.IsNullOrEmpty(startDate))
            //{
            //    startDate = "";
            //}



            //string endDate = HttpContext.Request.Query["endDate"].ToString();
            //if (String.IsNullOrEmpty(endDate))
            //{
            //    endDate = "";
            //}



            //string customerName = HttpContext.Request.Query["customerName"].ToString();
            //if (String.IsNullOrEmpty(customerName))
            //{
            //    customerName = "";
            //}




            sale_Ref = String.IsNullOrEmpty(sale_Ref) ? "" : sale_Ref;



            startDate = String.IsNullOrEmpty(startDate) ? "" : startDate;
            endDate = String.IsNullOrEmpty(endDate) ? "" : endDate;



            ViewBag.sale_Ref_SalesListJP = sale_Ref;
            ViewBag.startDate_SalesListJP = startDate;
            ViewBag.endDate_SalesListJP = endDate;
            ViewBag.customerName_SalesListJP = customerName;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;





            //---Sales master list for page
            ViewBag.RecordsPerPage = 10;




            _oSales.SalesMasterListJP = _oSales.pa_Select_SalesMaster(sale_Ref, customerName, startDate, endDate).ToPagedList(page ?? 1, 10);
            _oSales.SalesMasterListJP_total = _oSales.pa_Select_SalesMaster_total(sale_Ref, customerName, startDate, endDate).ToList();





            return PartialView("_SalesListJP", _oSales);




        }
        #endregion


        #region ReceiptDetailSale


        //---insert into receipt detail Sale
        [HttpPost]
        public IActionResult InsertReceiptDetail_Sale(int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
            double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date, string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, int hdSaleMaster_ID, int hdCustomer_ID, string ReceiptDate)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = SalesInvoiceMaster_TempID;
            if (hdSaleMaster_ID == 0)
            {
                Temp_ID = SalesInvoiceMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            // SaleInvoiceMasterStatic_ID = SaleInvoiceMasterStatic_ID == null ? -1 : SaleInvoiceMasterStatic_ID;



            if (DR_accountID == null || Amount == null || CR_accountID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            try
            {
                message = _oSales.InsertReceiptDetail_Sale_DAL_Invoice(DR_accountID, Amount, other_curr_amount, Currency_ID, Currency_Rate, CR_accountID,
                    Description, trans_ref, Cheque_Bank_Name, cheque_Date, Cheque_no, VAT_Rate, VAT_Exp, Total_Amount, Temp_ID, c_ID, Created_by, hdSaleMaster_ID, hdCustomer_ID, ReceiptDate);
                if (message == "Saved Successfully!")
                {

                    _oSales.SalesReceiptDetailsList = _oSales.Get_ReceiptDetailSaleListByID_DAL_Invoice(Temp_ID, hdSaleMaster_ID).ToList();

                    return PartialView("_ReceiptDetail_SaleList_Partial_Sales", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }



        //---insert into receipt detail Sale Booking
        [HttpPost]
        public IActionResult InsertReceiptDetail_SaleBooking(int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
            double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date, string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, int hdSaleMaster_ID, int hdCustomer_ID, string ReceiptDate)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = SalesBookingMaster_TempID;
            if (hdSaleMaster_ID == 0)
            {
                Temp_ID = SalesBookingMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            // SaleInvoiceMasterStatic_ID = SaleInvoiceMasterStatic_ID == null ? -1 : SaleInvoiceMasterStatic_ID;



            if (DR_accountID == null || Amount == null || CR_accountID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            try
            {
                message = _oSales.InsertReceiptDetail_Sale_DAL_Invoice(DR_accountID, Amount, other_curr_amount, Currency_ID, Currency_Rate, CR_accountID,
                    Description, trans_ref, Cheque_Bank_Name, cheque_Date, Cheque_no, VAT_Rate, VAT_Exp, Total_Amount, Temp_ID, c_ID, Created_by, hdSaleMaster_ID, hdCustomer_ID, ReceiptDate);
                if (message == "Saved Successfully!")
                {

                    _oSales.SalesReceiptDetailsList = _oSales.Get_ReceiptDetailSaleListByID_DAL_Invoice(Temp_ID, hdSaleMaster_ID).ToList();

                    return PartialView("_ReceiptDetail_SaleList_Partial_Sales", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }



        //---Update into receipt detail Sale
        [HttpPost]
        public IActionResult UpdateReceiptDetail_Sale(int? ReceiptDetail_ID, int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
            double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date, string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, int hdSaleMaster_ID, string ReceiptDate)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = SalesInvoiceMaster_TempID;
            if (hdSaleMaster_ID == 0)
            {
                Temp_ID = SalesInvoiceMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }
            SaleInvoiceMasterStatic_ID = SaleInvoiceMasterStatic_ID == null ? -1 : SaleInvoiceMasterStatic_ID;

            if (ReceiptDetail_ID == null)
            {
                return Json(new { message = "Receipt detail id is null. Please try again!" });
            }

            if (DR_accountID == null || Amount == null || CR_accountID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            try
            {
                message = _oSales.UpdateReceiptDetail_Sale_DAL_Invoice(ReceiptDetail_ID, DR_accountID, Amount, other_curr_amount, Currency_ID, Currency_Rate, CR_accountID,
                    Description, trans_ref, Cheque_Bank_Name, cheque_Date, Cheque_no, VAT_Rate, VAT_Exp, Total_Amount, Temp_ID, c_ID, Modified_by, ReceiptDate);
                if (message == "Updated Successfully!")
                {

                    _oSales.SalesReceiptDetailsList = _oSales.Get_ReceiptDetailSaleListByID_DAL_Invoice(Temp_ID, hdSaleMaster_ID).ToList();

                    return PartialView("_ReceiptDetail_SaleList_Partial_Sales", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }


        //---Method for deleting receipt  detail Sale
        [HttpPost]
        public IActionResult DeleteReceiptDetailSale(int? ReceiptDetail_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion
            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();

            string message = "";

            if (ReceiptDetail_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }



            try
            {
                message = _oSales.DeleteReceiptDetail_DAL_Invoice(ReceiptDetail_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = message;
                    return Redirect(url);
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }


        #endregion





        #region SALE INVOICE_TRD 

        [HttpGet]
        public IActionResult NewSalesInvoice_TRD(int SaleMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "NewSalesInvoice";


            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "Sales Invoice";
            ViewBag.SectionSub_HeaderMainTitle = "Sales";

            ViewBag.PageTitle = "Sales Invoice";


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            #region ViewBag Area
            //---Get Customer Master
            ViewBag.UOMList = _oBasic.Get_UOM_DAL(c_ID).ToList();
            ViewBag.CustomerMaster = _oBasic.Get_CustomersList_DAL(c_ID).ToList();

            ViewBag.SellerList = _oBasic.Get_PartnersSeller_DAL(c_ID).ToList();
            ViewBag.AgentsList = _oBasic.Get_PartnersAgent_DAL(c_ID).ToList();
            ViewBag.ExportDestinations = _oBasic.Get_Destinations_DAL(c_ID).ToList();
            ViewBag.PortOfExit = _oBasic.Get_PortOfExit_DAL(c_ID).ToList();
            ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_ID).ToList();
            ViewBag.ItemLocation = _oBasic.Get_ItemLocationList_DAL(c_ID).ToList();
            //---Ven Category list for drop down in forms
            ViewBag.CustomerCategoryList = _oBasic.Get_CustomerCategory_DAL().ToList();
            //---get shipping companies
            ViewBag.ShippingCompanies = _oBasic.Get_ShippingCompaniesList_DAL(c_ID).ToList();
            //--get export companies list
            ViewBag.ExportCompanies = _oBasic.Get_ExportCompaniesList_DAL(c_ID).ToList();
            //--get Basic Banks list
            ViewBag.BasicBanksList = _oBasic.Get_BasicBanksList_DAL(c_ID).ToList();
            //--get showrooms lsit
            ViewBag.ShowroomsList = _oBasic.Get_ShowRoom_DAL(c_ID).ToList();
            
           
            //---Get Accounts List
            ViewBag.DR_AccountsList = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();
            
            //---Get pay via list
            ViewBag.CR_AccountsList = _oBasic.Select_RV_OnAccounts_DAL(c_ID).ToList();

            //---Get Accounts List
            ViewBag.DR_AccountsListPD = _oBasic.Select_PV_OnAccounts_Trd(c_ID).ToList();

            ViewBag.CR_AccountsListPD = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();




            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();
            //---Get Item master list
            ViewBag.ItemMaster = _oBasic.Get_ItemMasterList_DAL().ToList();

                #endregion

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;


            if (SaleMaster_ID == 0 || SaleMaster_ID < 1)
            {


                //---Create payment master Ref(tem id)
                if (String.IsNullOrEmpty(SalesInvoiceMaster_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    SalesInvoiceMaster_TempID = obj.ToString();
                }

                SaleInvoiceMasterStatic_ID = SaleMaster_ID;

                //---Get sales master id
                ViewBag.SaleMaster_ID = SaleMaster_ID;

                //--Get sales Master
                _oSales.SaleMasterObj = _oSales.Get_SalesInvoiceMasterByID_DAL_TRD(-1);




                //---sales detial invoice list for sales invoice type Bychassis
                _oSales.SalesDetailList_trd_online = _oSales.Get_SalesIvoiceDetailListByChassis_DAL_TRD(SalesInvoiceMaster_TempID, -1).ToList();




            }
            else
            {
                Urls = null;

                ApprovalURL = HttpContext.Request.Headers["Referer"];
                Urls = ApprovalURL;
                //---Get old temp id from sales detail table that already creaded for this "SaleMaster_ID" id. And then use this if new record added
                string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL(SaleMaster_ID);




                SaleInvoiceMasterStatic_ID = SaleMaster_ID;

                //---Get sale master id
                ViewBag.SaleMaster_ID = SaleMaster_ID;

                //--Get sale Master

                _oSales.SaleMasterObj = _oSales.Get_SalesInvoiceMasterByID_DAL_TRD(SaleMaster_ID);

                //---sales detial invoice list for sales invoice type Bychassis
                _oSales.SalesDetailList_trd_online = _oSales.Get_SalesIvoiceDetailListByChassis_DAL_TRD(OldTempID, SaleMaster_ID).ToList();

                _oSales.SalesReceiptDetailsList = _oSales.Get_ReceiptDetailSaleListByID_DAL_Invoice("", SaleMaster_ID).ToList();

                _oSales.PaymentDetailList = _oSales.Select_PaymentList_PVSV_ByID_DAL("", SaleMaster_ID).ToList();
            
                               
            }



            return View(_oSales);
        }


        //---insert into sales invoice detial
        [HttpPost]
        public IActionResult InsertSalesInvoiceDetail_TRD(string SalesInvoiceType, int Item_ID, string ItemDesc, double? Unit_Price,
            double? VATRate, double? VATExp, double? Total_Amount, int hdSaleMaster_ID, string Quantity, string UM_ID, int Location_ID, string Serial, string Barcode,int Seller_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            if (Unit_Price == null)
            {
                return Json(new { message = "Please enter unit price!" });
            }
            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = SalesInvoiceMaster_TempID;


            if (hdSaleMaster_ID == 0)
            {
                Temp_ID = SalesInvoiceMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            try
            {
                message = _oSales.InsertSalesInvoiceDetail_DAL_TRD(SalesInvoiceType, Item_ID, ItemDesc, Unit_Price, VATRate,
                    VATExp, Total_Amount, Temp_ID, c_ID, Created_by, hdSaleMaster_ID, Quantity, UM_ID, Location_ID, Serial, Barcode,Seller_ID);
                if (message.Contains("Saved Successfully!"))
                {
                    //---sales detial invoice list for sales invoice type Bychassis
                    _oSales.SalesDetailList = _oSales.Get_SalesIvoiceDetailListByChassis_DAL_TRD(Temp_ID, hdSaleMaster_ID).ToList();

                    return PartialView("_SalesInvoiceDetailList_TRD", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }


        //---Update  sales invoice detail Chassis
        [HttpPost]
        public IActionResult UpdateSalesInvoiceDetailByChassis_TRD(int? SalesDetails_ID, int Item_ID, string ItemDesc, double? Unit_Price,
            double? VAT_Rate, double? VAT_Exp, double? Total_Amount, int hdSaleMaster_ID, string Quantity, string UM_ID, int Location_ID, string Serial, string Barcode,int Seller_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //---Validation area starts here

            if (SalesDetails_ID == null)
            {
                return Json(new { message = "Sales detail id is null" });
            }

            if (Item_ID == 0 || Unit_Price == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);


            Temp_ID = SalesInvoiceMaster_TempID;
            if (hdSaleMaster_ID == 0)
            {
                Temp_ID = SalesInvoiceMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }
            SaleInvoiceMasterStatic_ID = SaleInvoiceMasterStatic_ID == null ? -1 : SaleInvoiceMasterStatic_ID;


            try
            {
                message = _oSales.UpdateSalesInvoiceDetail_DAL_TRD(SalesDetails_ID, Item_ID, ItemDesc, Unit_Price, VAT_Rate, VAT_Exp, Total_Amount, Temp_ID, c_ID, Modified_by, Quantity, UM_ID, Location_ID, Serial, Barcode,Seller_ID);
                if (message == "Updated Successfully!")
                {
                    //---sales detial invoice list for sales invoice type Bychassis
                    _oSales.SalesDetailList = _oSales.Get_SalesIvoiceDetailListByChassis_DAL_TRD(Temp_ID, hdSaleMaster_ID).ToList();



                    return PartialView("_SalesInvoiceDetailList_TRD", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }


        //---Insert  sales invoice master
        [HttpPost]
        public IActionResult InsertSalesInvoiceMaster_TRD(int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string CustomerTRN,
            string Manualbook_ref, string Sale_trans_type, string Remarks,string Customer_Name,string OtherCost,string Tip)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = SalesInvoiceMaster_TempID;
            string Saletype = "SI";  //---It is the ref in table for new Payment master. Send this value hard coded from here

            if (Customer_ID == null || String.IsNullOrEmpty(SaleDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewSalesInvoice_TRD", "Sales");
            }

            Customer_ID = Customer_ID == null ? 0 : Customer_ID;


            try
            {
                message = _oSales.InsertSalesInvoiceMaster_DAL_TRD(Customer_ID, Customer_Contact, Invoice_address, Saletype, SaleDate, CustomerTRN, Manualbook_ref,
                     Sale_trans_type, Temp_ID, c_ID, Created_by, Remarks,  Customer_Name, OtherCost,Tip,"0","0","0");
     
                if (message.Contains("Successfully"))
                {
                    string SaleMaster_ID = _oSales.SalesID_DAL_TRD();
                    //TempData["Success"] = "Saved Successfully!";
                    DumpStaticFieldsSales();
                    return RedirectToAction("NewSalesInvoice_TRD", new { SaleMaster_ID = SaleMaster_ID });
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("NewSalesInvoice_TRD", "Sales");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
                return RedirectToAction("NewSalesInvoice_TRD", "Sales");
            }


        }

        //---Insert  sales invoice master
        [HttpPost]
        public IActionResult UpdateSalesInvoiceMaster_TRD(int? SaleMaster_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate,
            string CustomerTRN, string Manualbook_ref, string Sale_trans_type, string Remarks, string Customer_Name,string OtherCost,string Tip)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = SalesInvoiceMaster_TempID;
            string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL(SaleMaster_ID);
            if (!String.IsNullOrEmpty(OldTempID))
            {
                Temp_ID = OldTempID;
            }
            else
            {
                if (String.IsNullOrEmpty(Temp_ID))
                {
                    Guid obj = Guid.NewGuid();
                    Temp_ID = obj.ToString();
                }
            }
            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();


            if (Customer_ID == null || String.IsNullOrEmpty(SaleDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            Customer_ID = Customer_ID == null ? 0 : Customer_ID;


            try
            {
                message = _oSales.UpdateSalesInvoiceMaster_DAL_TRD(SaleMaster_ID, Customer_ID, Customer_Contact, Invoice_address, SaleDate, CustomerTRN, Manualbook_ref, Sale_trans_type, Temp_ID, c_ID, Modified_by, Remarks,  Customer_Name, OtherCost,Tip,"0","0","0");
               
                if (message.Contains("Successfully"))
                {
                    DumpStaticFieldsSales();
                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("NewSalesInvoice_TRD", new { SaleMaster_ID = SaleMaster_ID });
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }


        }


        //---Method for deleting sales master invoice
        [HttpPost]
        public IActionResult DeleteSalesInvoiceMaster_TRD(int? SaleMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion
            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();



            string message = "";

            if (SaleMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }



            try
            {
                message = _oSales.DeleteSalesMaster_DAL_TRD(SaleMaster_ID);
                if (message.Contains("SUCCESSFULLY"))
                {
                    return RedirectToAction("SalesInvoiceList_TRD", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }



        //---Method for deleting sales invoice detail
        [HttpPost]
        public IActionResult DeleteSalesInvoiceDetail_TRD(int? SalesDetails_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion
            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();

            string message = "";

            if (SalesDetails_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }



            try
            {
                message = _oSales.DeleteSalesDetail_DAL_TRD(SalesDetails_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = message;
                    return Redirect(url);
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }


        [HttpGet]
        public IActionResult SalesInvoiceList_TRD(string SaleRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string ItemId,string MNumber, int c_ID, string OrderRef, string OrderStatus, int? page)
        {
            //---dumping static fields
            DumpStaticFieldsSales();

            //---BreadCrumbs defintion
            //  ViewBag.BreadCumController = "Sales";
            //  ViewBag.BreadCumAction = "SalesInvoiceList";

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "SalesInvoiceList";

            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "Sale Invoice List";

            ViewBag.PageTitle = "Sales Invoice List";




            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region Veiwbags area


            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            //---Status
            ViewBag.ddlStatus = _oBasic.get_SalesTrans_Status_DAL().ToList();
            ViewBag.AllItems = _oBasic.Get_ItemMasterList_DAL().ToList();

            #endregion veiwbags area ends here

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            ItemId = String.IsNullOrEmpty(ItemId) ? "" : ItemId;
            MNumber = String.IsNullOrEmpty(MNumber) ? "" : MNumber;
            OrderRef = string.IsNullOrEmpty(OrderRef) ? "" : OrderRef;
            OrderStatus = string.IsNullOrEmpty(OrderStatus) ? "" : OrderStatus;

            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;
            Status_ID = Status_ID == 0 ? 0 : Status_ID;

            ViewBag.SaleRef_SaleInvoice = SaleRef;
            ViewBag.StartDate_SaleInvoice = StartDate;
            ViewBag.EndDate_SaleInvoice = EndDate;
            ViewBag.Customer_Name_SaleInvoice = Customer_Name;
            ViewBag.ItemId_SaleInvoice = ItemId;
            ViewBag.MNumber_SaleInvoice = MNumber;
            ViewBag.Status_ID_SaleInvoice = Status_ID;
            ViewBag.c_ID_SaleInvoice = c_ID;
            ViewBag.OrderRef_SaleInvoice = OrderRef;
            ViewBag.OrderStatus_SaleInvoice = OrderStatus;

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;
            //---purchase sales master invoice list for page
            ViewBag.RecordsPerPage = 10;

            _oSales.SalesMasterIPagedList = _oSales.Get_SalesMasterInvoiceList_DAL_TRD(SaleRef, StartDate, EndDate, Customer_Name, ItemId, MNumber, Status_ID, c_ID,OrderRef, OrderStatus).ToPagedList(page ?? 1, 10);
            _oSales.SalesMasterListTOTAL = _oSales.Get_SalesMasterInvoiceList_TTL_DAL_TRD(SaleRef, StartDate, EndDate, Customer_Name, ItemId,MNumber, Status_ID, c_ID,OrderRef,OrderStatus).ToList();



            return View(_oSales);
        }

        public JsonResult GetLocDescListByID(string Item_ID)
        {


            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            var LocDescList = new List<Pa_ItemLocations_DAL>();

            if (String.IsNullOrEmpty(Item_ID))
            {

             
                return Json(LocDescList);
            }

            _oSales.LocDescList = _oSales.Get_LocList_DAL(Item_ID).ToList();

            LocDescList= _oSales.Get_LocList_DAL(Item_ID).ToList();




            return Json(LocDescList);
            //return Json(ModelDescList, new Newtonsoft.Json.JsonSerializerSettings());

        }
        //---Get Sales master invoice list by search filters
        [HttpGet]
        public IActionResult GetSalesMasterInvoiceListBySearchFilers_TRD(string SaleRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string ItemId,string MNumber, int c_ID,  string OrderRef, string OrderStatus, int? page)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion
            c_ID = c_ID == 0 ? 1 : c_ID;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            ItemId = String.IsNullOrEmpty(ItemId) ? "" : ItemId;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;
            Status_ID = Status_ID == 0 ? 0 : Status_ID;
            MNumber = String.IsNullOrEmpty(MNumber) ? "" : MNumber;

            OrderRef = string.IsNullOrEmpty(OrderRef) ? "" : OrderRef;
            OrderStatus = string.IsNullOrEmpty(OrderStatus) ? "" : OrderStatus;



            ViewBag.SaleRef_SaleInvoice = SaleRef;
            ViewBag.StartDate_SaleInvoice = StartDate;
            ViewBag.EndDate_SaleInvoice = EndDate;
            ViewBag.Customer_Name_SaleInvoice = Customer_Name;
            ViewBag.ItemId_SaleInvoice = ItemId;
            ViewBag.MNumber_SaleInvoice = MNumber;
            ViewBag.Status_ID_SaleInvoice = Status_ID;
            ViewBag.c_ID_SaleInvoice = c_ID;
            ViewBag.OrderRef_SaleInvoice = OrderRef;
            ViewBag.OrderStatus_SaleInvoice = OrderStatus;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;
            try
            {
                //---purchase sales master invoice list for page
                ViewBag.RecordsPerPage = 10;
                _oSales.SalesMasterIPagedList = _oSales.Get_SalesMasterInvoiceList_DAL_TRD(SaleRef, StartDate, EndDate, Customer_Name, ItemId, MNumber, Status_ID, c_ID,OrderRef, OrderStatus).ToPagedList(page ?? 1, 10);
                _oSales.SalesMasterListTOTAL = _oSales.Get_SalesMasterInvoiceList_TTL_DAL_TRD(SaleRef, StartDate, EndDate, Customer_Name, ItemId, MNumber, Status_ID, c_ID, OrderRef,OrderStatus).ToList();


                return PartialView("_SalesMasterInvoiceListSearch_TRD", _oSales);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }


        #endregion
        [HttpGet]
        public IActionResult NewDeliveryOrder(int SaleMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "NewDeliveryOrder";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            #region ViewBag Area
            //---Get Customer Master
            ViewBag.CustomerMaster = _oBasic.Get_CustomersList_DAL(c_ID).ToList();

            ViewBag.SellerList = _oBasic.Get_PartnersSeller_DAL(c_ID).ToList();
            ViewBag.AgentsList = _oBasic.Get_PartnersAgent_DAL(c_ID).ToList();
            ViewBag.ExportDestinations = _oBasic.Get_Destinations_DAL(c_ID).ToList();
            ViewBag.PortOfExit = _oBasic.Get_PortOfExit_DAL(c_ID).ToList();
            ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_ID).ToList();
            //---Ven Category list for drop down in forms
            ViewBag.CustomerCategoryList = _oBasic.Get_CustomerCategory_DAL().ToList();
            //---get shipping companies
            ViewBag.ShippingCompanies = _oBasic.Get_ShippingCompaniesList_DAL(c_ID).ToList();
            //--get export companies list
            ViewBag.ExportCompanies = _oBasic.Get_ExportCompaniesList_DAL(c_ID).ToList();
            //--get Basic Banks list
            ViewBag.BasicBanksList = _oBasic.Get_BasicBanksList_DAL(c_ID).ToList();
            //--get showrooms lsit
            ViewBag.ShowroomsList = _oBasic.Get_ShowRoom_DAL(c_ID).ToList();
            //---Get Accounts List
            ViewBag.CR_AccountsList = _oBasic.Select_RV_OnAccounts_DAL(c_ID).ToList();

            //---Get pay via list
            ViewBag.DR_AccountsList = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();
            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();
            //---Get Item master list
            ViewBag.ItemMaster = _oBasic.Get_ItemMasterList_DAL().ToList();
            #endregion




            if (SaleMaster_ID == 0 || SaleMaster_ID < 1)
            {


                //---Create payment master Ref(tem id)
                if (String.IsNullOrEmpty(SalesInvoiceMaster_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    SalesInvoiceMaster_TempID = obj.ToString();
                }

                SaleInvoiceMasterStatic_ID = SaleMaster_ID;

                //---Get sales master id
                ViewBag.SaleMaster_ID = SaleMaster_ID;

                //--Get sales Master
                _oSales.SaleMasterObj = _oSales.Get_SalesInvoiceMasterByID_DAL_TRD(-1);




                //---sales detial invoice list for sales invoice type Bychassis
                _oSales.SalesDetailList = _oSales.Get_SalesIvoiceDetailListByChassis_DAL_TRD(SalesInvoiceMaster_TempID, -1).ToList();


            }
            else
            {
                Urls = null;

                ApprovalURL = HttpContext.Request.Headers["Referer"];
                Urls = ApprovalURL;
                //---Get old temp id from sales detail table that already creaded for this "SaleMaster_ID" id. And then use this if new record added
                string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL(SaleMaster_ID);


                SaleInvoiceMasterStatic_ID = SaleMaster_ID;

                //---Get sale master id
                ViewBag.SaleMaster_ID = SaleMaster_ID;

                //--Get sale Master

                _oSales.SaleMasterObj = _oSales.Get_SalesInvoiceMasterByID_DAL_TRD(SaleMaster_ID);

                //---sales detial invoice list for sales invoice type Bychassis
                _oSales.SalesDetailList = _oSales.Get_SalesIvoiceDetailListByChassis_DAL_TRD(OldTempID, SaleMaster_ID).ToList();



            }



            return View(_oSales);
        }

        [HttpGet]
        public IActionResult DeliveryOrderList(string SaleRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string ItemId,string MNumber, int c_ID,string OrderRef, string OrderStatus, int? page)
        {
            //---dumping static fields
            DumpStaticFieldsSales();

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "DeliveryOrderList";




            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region Veiwbags area
            //---Status
            ViewBag.ddlStatus = _oBasic.get_SalesTrans_Status_DAL().ToList();

            #endregion veiwbags area ends here

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            ItemId = String.IsNullOrEmpty(ItemId) ? "" : ItemId;
            OrderRef = string.IsNullOrEmpty(OrderRef) ? "" : OrderRef;
            
            OrderStatus = string.IsNullOrEmpty(OrderStatus) ? "" : OrderStatus;
            

            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;
            Status_ID = Status_ID == 0 ? 0 : Status_ID;

            ViewBag.SaleRef_SaleInvoice = SaleRef;
            ViewBag.StartDate_SaleInvoice = StartDate;
            ViewBag.EndDate_SaleInvoice = EndDate;
            ViewBag.Customer_Name_SaleInvoice = Customer_Name;
            ViewBag.Chassis_No_SaleInvoice = ItemId;
            ViewBag.Status_ID_SaleInvoice = Status_ID;
            ViewBag.c_ID_SaleInvoice = c_ID;
            ViewBag.OrderRef_SaleInvoice = OrderRef;


            ViewBag.OrderStatus_SaleInvoice = OrderStatus;

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            //---purchase sales master invoice list for page
            ViewBag.RecordsPerPage = 10;

            _oSales.SalesMasterIPagedList = _oSales.Get_SalesMasterInvoiceList_DAL_TRD(SaleRef, StartDate, EndDate, Customer_Name, ItemId,MNumber, Status_ID, c_ID,OrderRef, OrderStatus).ToPagedList(page ?? 1, 10);
            _oSales.SalesMasterListTOTAL = _oSales.Get_SalesMasterInvoiceList_TTL_DAL_TRD(SaleRef, StartDate, EndDate, Customer_Name, ItemId, MNumber, Status_ID, c_ID,OrderRef,OrderStatus).ToList();



            return View(_oSales);
        }


        [HttpGet]
        public IActionResult NewSalesOrder(int SaleMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "NewSalesInvoice";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            #region ViewBag Area
            //---Get Customer Master
            ViewBag.CustomerMaster = _oBasic.Get_CustomersList_DAL(c_ID).ToList();

            ViewBag.SellerList = _oBasic.Get_PartnersSeller_DAL(c_ID).ToList();
            ViewBag.AgentsList = _oBasic.Get_PartnersAgent_DAL(c_ID).ToList();
            ViewBag.ExportDestinations = _oBasic.Get_Destinations_DAL(c_ID).ToList();
            ViewBag.PortOfExit = _oBasic.Get_PortOfExit_DAL(c_ID).ToList();
            ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_ID).ToList();
            //---Ven Category list for drop down in forms
            ViewBag.CustomerCategoryList = _oBasic.Get_CustomerCategory_DAL().ToList();
            //---get shipping companies
            ViewBag.ShippingCompanies = _oBasic.Get_ShippingCompaniesList_DAL(c_ID).ToList();
            //--get export companies list
            ViewBag.ExportCompanies = _oBasic.Get_ExportCompaniesList_DAL(c_ID).ToList();
            //--get Basic Banks list
            ViewBag.BasicBanksList = _oBasic.Get_BasicBanksList_DAL(c_ID).ToList();
            //--get showrooms lsit
            ViewBag.ShowroomsList = _oBasic.Get_ShowRoom_DAL(c_ID).ToList();
            //---Get Accounts List
            ViewBag.CR_AccountsList = _oBasic.Select_RV_OnAccounts_DAL(c_ID).ToList();

            //---Get pay via list
            ViewBag.DR_AccountsList = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();
            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();
            //---Get Item master list
            ViewBag.ItemMaster = _oBasic.Get_ItemMasterList_DAL().ToList();
            #endregion




            if (SaleMaster_ID == 0 || SaleMaster_ID < 1)
            {


                //---Create payment master Ref(tem id)
                if (String.IsNullOrEmpty(SalesInvoiceMaster_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    SalesInvoiceMaster_TempID = obj.ToString();
                }

                SaleInvoiceMasterStatic_ID = SaleMaster_ID;

                //---Get sales master id
                ViewBag.SaleMaster_ID = SaleMaster_ID;

                //--Get sales Master
                _oSales.SaleMasterObj = _oSales.Get_SalesInvoiceMasterByID_DAL_TRD(-1);




                //---sales detial invoice list for sales invoice type Bychassis
                _oSales.SalesDetailList = _oSales.Get_SalesIvoiceDetailListByChassis_DAL_TRD(SalesInvoiceMaster_TempID, -1).ToList();


            }
            else
            {
                Urls = null;

                ApprovalURL = HttpContext.Request.Headers["Referer"];
                Urls = ApprovalURL;
                //---Get old temp id from sales detail table that already creaded for this "SaleMaster_ID" id. And then use this if new record added
                string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL(SaleMaster_ID);




                SaleInvoiceMasterStatic_ID = SaleMaster_ID;

                //---Get sale master id
                ViewBag.SaleMaster_ID = SaleMaster_ID;

                //--Get sale Master

                _oSales.SaleMasterObj = _oSales.Get_SalesInvoiceMasterByID_DAL_TRD(SaleMaster_ID);

                //---sales detial invoice list for sales invoice type Bychassis
                _oSales.SalesDetailList = _oSales.Get_SalesIvoiceDetailListByChassis_DAL_TRD(OldTempID, SaleMaster_ID).ToList();



            }



            return View(_oSales);
        }

        [HttpGet]
        public IActionResult SalesOrderList(string SaleRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string ItemId,string MNumber, int c_ID,string OrderRef, string OrderStatus,int? page)
        {
            //---dumping static fields
            DumpStaticFieldsSales();

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "DeliveryOrderList";




            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region Veiwbags area
            //---Status
            ViewBag.ddlStatus = _oBasic.get_SalesTrans_Status_DAL().ToList();

            #endregion veiwbags area ends here

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            ItemId = String.IsNullOrEmpty(ItemId) ? "" : ItemId;
            OrderRef = string.IsNullOrEmpty(OrderRef) ? "" : OrderRef;

            OrderStatus = string.IsNullOrEmpty(OrderStatus) ? "" : OrderStatus;

            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;
            Status_ID = Status_ID == 0 ? 0 : Status_ID;

            ViewBag.SaleRef_SaleInvoice = SaleRef;
            ViewBag.StartDate_SaleInvoice = StartDate;
            ViewBag.EndDate_SaleInvoice = EndDate;
            ViewBag.Customer_Name_SaleInvoice = Customer_Name;
            ViewBag.Chassis_No_SaleInvoice = ItemId;
            ViewBag.Status_ID_SaleInvoice = Status_ID;
            ViewBag.c_ID_SaleInvoice = c_ID;
            ViewBag.OrderRef_SaleInvoice = OrderRef;
            ViewBag.OrderStatus_SaleInvoice = OrderStatus;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            //---purchase sales master invoice list for page
            ViewBag.RecordsPerPage = 10;

            _oSales.SalesMasterIPagedList = _oSales.Get_SalesMasterInvoiceList_DAL_TRD(SaleRef, StartDate, EndDate, Customer_Name, ItemId, MNumber, Status_ID, c_ID,OrderRef,OrderStatus).ToPagedList(page ?? 1, 10);
            _oSales.SalesMasterListTOTAL = _oSales.Get_SalesMasterInvoiceList_TTL_DAL_TRD(SaleRef, StartDate, EndDate, Customer_Name, ItemId, MNumber, Status_ID, c_ID,OrderRef,OrderStatus).ToList();



            return View(_oSales);
        }


        #region Dashboard Graph        

        [HttpGet]
        public JsonResult getLAST_7DAYS_SALE_TRD()
        {




            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function

            List<object> iData = new List<object>();
            DataTable data = new DataTable();

            var model = _oSales.getLAST_7DAYS_SALE_TRD();
            DataTable dt = new DataTable();
            dt = model.Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        List<object> x = new List<object>();

                        iData.Add(new { Month = dr["DAYSS"].ToString(), NetTotal = dr["AMT"].ToString() });
                    }
                }
            }

            return Json(iData);

        }

        public JsonResult GetTopSaleValueItems_trd(string StartDate, string EndDate)
        {




            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function

            List<object> iData = new List<object>();
            DataTable data = new DataTable();
            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "0" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "0" : EndDate);
            var model = _oSales.GetTopSaleValueItems_trd(StartDate, EndDate);
            DataTable dt = new DataTable();
            dt = model.Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        List<object> x = new List<object>();

                        iData.Add(new { Month = dr["ItemName"].ToString(), NetTotal = dr["Sale"].ToString() });
                    }
                }
                else
                {
                    iData.Add(new { Month = "", NetTotal = "" });
                }
            }

            return Json(iData);

        }

        public JsonResult TopPayables_trd(string StartDate, string EndDate)
        {




            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function

            List<object> iData = new List<object>();
            DataTable data = new DataTable();
            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "0" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "0" : EndDate);
            var model = _oSales.TopPayables_trd(StartDate, EndDate);
            DataTable dt = new DataTable();
            dt = model.Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        List<object> x = new List<object>();

                        iData.Add(new { Vendor_Name = dr["Vendor_Name"].ToString(), Amount = dr["Amount"].ToString() });
                    }
                }
                else
                {
                    iData.Add(new { Vendor_Name = "", Amount = "" });
                }
            }

            return Json(iData);

        }

        public JsonResult TopSaleCategory_trd(string StartDate, string EndDate)
        {


            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function

            List<object> iData = new List<object>();
            DataTable data = new DataTable();
            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "0" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "0" : EndDate);
            var model = _oSales.TopSaleCategory_trd(StartDate, EndDate);
            DataTable dt = new DataTable();
            dt = model.Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        List<object> x = new List<object>();

                        iData.Add(new { Category_Name = dr["Category_Name"].ToString(), Amount = dr["Amount"].ToString() });
                    }
                }
                else
                {
                    iData.Add(new { Category_Name = "", Amount = "" });
                }
            }

            return Json(iData);

        }


        public JsonResult TopItemByCost_trd(string StartDate, string EndDate)
        {




            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function

            List<object> iData = new List<object>();
            DataTable data = new DataTable();
            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "0" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "0" : EndDate);
            var model = _oSales.TopItemByCost_trd(StartDate, EndDate);
            DataTable dt = new DataTable();
            dt = model.Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        List<object> x = new List<object>();

                        iData.Add(new { ItemName = dr["ItemName"].ToString(), Amount = dr["Amount"].ToString() });
                    }
                }
                else
                {
                    iData.Add(new { ItemName = "", Amount = "" });
                }
            }

            return Json(iData);

        }

        public JsonResult Sale_By_Month_trd()
        {




            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function

            List<object> iData = new List<object>();
            DataTable data = new DataTable();

            var model = _oSales.Sale_By_Month_trd("2021", 1);
            DataTable dt = new DataTable();
            dt = model.Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        List<object> x = new List<object>();

                        iData.Add(new { Month = dr["MONN"].ToString(), NetTotal = dr["AMT"].ToString() });
                    }
                }
            }

            return Json(iData);

        }
        [HttpGet]
        public JsonResult StockLocation_Summary()
        {


            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);

            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function

            List<object> iData = new List<object>();
            DataTable data = new DataTable();

            var model = _oSales.get_StockLocation_Summary(c_ID);
            DataTable dt = new DataTable();
            dt = model.Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        List<object> x = new List<object>();

                        iData.Add(new { Month = dr["CarLocation"].ToString(), NetTotal = dr["LocationCount"].ToString() });
                    }
                }
            }

            return Json(iData);

        }


        [HttpGet]
        public JsonResult All_Accounts_Balance_Summary()
        {

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);


            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function

            List<object> iData = new List<object>();
            DataTable data = new DataTable();

            var model = _oSales.get_All_Accounts_Balance(c_ID);
            DataTable dt = new DataTable();
            dt = model.Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        List<object> x = new List<object>();

                        iData.Add(new { Month = dr["AccountName"].ToString(), NetTotal = dr["Balance"].ToString() });
                    }
                }
            }

            return Json(iData);

        }
        [HttpGet]
        public JsonResult MakeModel_Summary()
        {


            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);

            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function

            List<object> iData = new List<object>();
            DataTable data = new DataTable();

            var model = _oSales.get_MakeModel_Summary(c_ID);
            DataTable dt = new DataTable();
            dt = model.Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        List<object> x = new List<object>();

                        iData.Add(new { Month = dr["MakeModel"].ToString(), NetTotal = dr["Sale_Count"].ToString() });
                    }
                }
            }

            return Json(iData);

        }
        [HttpGet]
        public JsonResult getLAST_7DAYS_SALE()
        {




            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function

            List<object> iData = new List<object>();
            DataTable data = new DataTable();

            var model = _oSales.getLAST_7DAYS_SALE();
            DataTable dt = new DataTable();
            dt = model.Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        List<object> x = new List<object>();

                        iData.Add(new { Month = dr["DAYSS"].ToString(), NetTotal = dr["AMT"].ToString() });
                    }
                }
            }

            return Json(iData);

        }

        [HttpGet]
        public JsonResult getSale_By_Month()
        {




            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function

            List<object> iData = new List<object>();
            DataTable data = new DataTable();

            var model = _oSales.getSale_By_Month();
            DataTable dt = new DataTable();
            dt = model.Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        List<object> x = new List<object>();

                        iData.Add(new { Month = dr["MONN"].ToString(), NetTotal = dr["AMT"].ToString() });
                    }
                }
            }

            return Json(iData);

        }
        #endregion
        #region salessummarybydate
        [HttpGet]
        public IActionResult SalesSummaryByDateList(string StartDate, string EndDate, int? page)
        {
            //---dumping static fields


            //---BreadCrumbs defintion
            //  ViewBag.BreadCumController = "Sales";
            //  ViewBag.BreadCumAction = "SalesInvoiceList";

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "SalesSummaryList";

            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "Sales Summary List";

            ViewBag.PageTitle = "Sales Summary List";




            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region Veiwbags area




            #endregion veiwbags area ends here

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
       
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;




            ViewBag.StartDate_SaleInvoice = StartDate;
            ViewBag.EndDate_SaleInvoice = EndDate;
     
            //---purchase sales master invoice list for page
            ViewBag.RecordsPerPage = 10;

            _oSales.SalesMasterByDateIPagedList = _oSales.Get_SalesSummaryByDate_DAL(StartDate, EndDate).ToPagedList(page ?? 1, 10);




            return View(_oSales);
        }
        //---Get Sales master invoice list by search filters
        [HttpGet]
        public IActionResult SalesSummaryByDateListListBySearchFilers(string StartDate, string EndDate, int? page)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
      
            ViewBag.StartDate_SaleInvoice = StartDate;
            ViewBag.EndDate_SaleInvoice = EndDate;
    
            try
            {
                //---purchase sales master invoice list for page
                ViewBag.RecordsPerPage = 10;
                _oSales.SalesMasterByDateIPagedList = _oSales.Get_SalesSummaryByDate_DAL(StartDate, EndDate).ToPagedList(page ?? 1, 10);

                return PartialView("_SalesSummaryByDateListSearch", _oSales);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }

        [HttpGet]
        public IActionResult GetVoidDetails(string SaleDate)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion




            try
            {
                _oSales.VoidDetailList = _oSales.GetVoidDetails_DAL(SaleDate).ToList();

                return PartialView("_VoidDetailList", _oSales);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }


        [HttpGet]
        public IActionResult GetDiscountDetails(string SaleDate)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion




            try
            {
                _oSales.DiscountDetailList = _oSales.GetDiscountDetails_DAL(SaleDate).ToList();
                _oSales.DiscountDetailList_TTL   = _oSales.GetDiscountDetails_DAL_TTL(SaleDate).ToList();

                return PartialView("_DiscountDetailList", _oSales);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }
        #endregion


        #region DeliveryOrder
        //---ohter Sales get method
        [HttpGet]
        public IActionResult SalesDeliveryOrder(int DOMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "NewSalesDeliveryOrder";
            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "SalesDeliveryOrder";
            ViewBag.SectionSub_HeaderMainTitle = "SalesDeliveryOrder List";
            ViewBag.PageTitle = "SalesDeliveryOrder";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            #region ViewBag Area
            ViewBag.UOMList = _oBasic.Get_UOM_DAL(c_ID).ToList();
            ViewBag.CustomerMaster = _oBasic.Get_CustomersList_DAL(c_ID).ToList();

            ViewBag.SellerList = _oBasic.Get_PartnersSeller_DAL(c_ID).ToList();
            ViewBag.AgentsList = _oBasic.Get_PartnersAgent_DAL(c_ID).ToList();
            ViewBag.ExportDestinations = _oBasic.Get_Destinations_DAL(c_ID).ToList();
            ViewBag.PortOfExit = _oBasic.Get_PortOfExit_DAL(c_ID).ToList();
            ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_ID).ToList();
            ViewBag.ItemLocation = _oBasic.Get_ItemLocationList_DAL(c_ID).ToList();
            //---Ven Category list for drop down in forms
            ViewBag.CustomerCategoryList = _oBasic.Get_CustomerCategory_DAL().ToList();
            //---get shipping companies
            ViewBag.ShippingCompanies = _oBasic.Get_ShippingCompaniesList_DAL(c_ID).ToList();
            //--get export companies list
            ViewBag.ExportCompanies = _oBasic.Get_ExportCompaniesList_DAL(c_ID).ToList();
            //--get Basic Banks list
            ViewBag.BasicBanksList = _oBasic.Get_BasicBanksList_DAL(c_ID).ToList();
            //--get showrooms lsit
            ViewBag.ShowroomsList = _oBasic.Get_ShowRoom_DAL(c_ID).ToList();
            //---Get Accounts List
            ViewBag.CR_AccountsList = _oBasic.Select_RV_OnAccounts_DAL(c_ID).ToList();

            //---Get pay via list
            ViewBag.DR_AccountsList = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();
            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();
            //---Get Item master list
            ViewBag.ItemMaster = _oBasic.Get_ItemMasterList_DAL().ToList();
            ViewBag.SaleRef = _oSales.GetDORefDetails_Other_DAL().ToList();

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;



            #endregion


            if (DOMaster_ID == 0 || DOMaster_ID < 1)
            {

                //---Create DO master Ref(tem id)
                if (String.IsNullOrEmpty(DOOtherTempID))
                {
                    Guid obj = Guid.NewGuid();
                    DOOtherTempID = obj.ToString();
                }

                DOMasterOtherStatic_ID = DOMaster_ID;

                //---Get DO master id
                ViewBag.DOMaster_ID = DOMaster_ID;

                //--Get DO Master
                _oSales.DOMasterObj = _oSales.Get_SalesDeliveryOrderMaster_ByID_DAL(-1);

                //--Get DO detail 
                _oSales.DODetailList = _oSales.Get_SalesDeliveryOrderDetailListByID_DAL(DOOtherTempID, -1).ToList();
            }
            else
            {

                Urls = null;

                ApprovalURL = HttpContext.Request.Headers["Referer"];
                Urls = ApprovalURL;

                //---Get old temp id from DO detail table that already creaded for this "DOMaster_ID" id. And then use this if new record added
                string OldTempID = _oSales.GetOldTempIDFromSalesDeliveryOrderDetail_DAL(DOMaster_ID);



                DOMasterOtherStatic_ID = DOMaster_ID;

                //---Get DO master id
                ViewBag.DOMaster_ID = DOMaster_ID;



                //--Get DO Master
                _oSales.DOMasterObj = _oSales.Get_SalesDeliveryOrderMaster_ByID_DAL(DOMaster_ID);

                //--Get DO detail
                _oSales.DODetailList = _oSales.Get_SalesDeliveryOrderDetailListByID_DAL(OldTempID, DOMaster_ID).ToList();
            }



            return View(_oSales);
        }

        //---Getting Other DO list
     
        [HttpGet]
        public IActionResult SalesDOInvoiceList(string SaleRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string ItemId, int c_ID, int? page)
        {
            //---dumping static fields
            DumpStaticFieldsSales();

            //---BreadCrumbs defintion
            //  ViewBag.BreadCumController = "Sales";
            //  ViewBag.BreadCumAction = "SalesInvoiceList";

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "DeliveryOrderList";

            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "Delivery Order List";

            ViewBag.PageTitle = "Delivery Order List";




            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region Veiwbags area


            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            //---Status
            ViewBag.ddlStatus = _oBasic.get_SalesTrans_Status_DAL().ToList();
            ViewBag.AllItems = _oBasic.Get_ItemMasterList_DAL().ToList();

            #endregion veiwbags area ends here

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            ItemId = String.IsNullOrEmpty(ItemId) ? "" : ItemId;



            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;


            ViewBag.SaleRef_SaleInvoice = SaleRef;
            ViewBag.StartDate_SaleInvoice = StartDate;
            ViewBag.EndDate_SaleInvoice = EndDate;
            ViewBag.Customer_Name_SaleInvoice = Customer_Name;
            ViewBag.ItemId_SaleInvoice = ItemId;


            ViewBag.c_ID_SaleInvoice = c_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;
            //---purchase sales master invoice list for page
            ViewBag.RecordsPerPage = 10;

            _oSales.DOOtherMasterList = _oSales.Get_SalesDeliveryOrderMaster_DAL(SaleRef, StartDate, EndDate, Customer_Name, ItemId, Status_ID, c_ID).ToPagedList(page ?? 1, 10);
           

            return View(_oSales);
        }
        //---Get Sales master invoice list by search filters
        [HttpGet]
        public IActionResult GetSalesDOMasterInvoiceListBySearchFilers(string SaleRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string ItemId, int c_ID, int? page)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion
            c_ID = c_ID == 0 ? 1 : c_ID;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            ItemId = String.IsNullOrEmpty(ItemId) ? "" : ItemId;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;


            ViewBag.SaleRef_SaleInvoice = SaleRef;
            ViewBag.StartDate_SaleInvoice = StartDate;
            ViewBag.EndDate_SaleInvoice = EndDate;
            ViewBag.Customer_Name_SaleInvoice = Customer_Name;
            ViewBag.ItemId_SaleInvoice = ItemId;
            ViewBag.c_ID_SaleInvoice = c_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;
            try
            {
                //---purchase sales master invoice list for page
                ViewBag.RecordsPerPage = 10;
                _oSales.DOOtherMasterList = _oSales.Get_SalesDeliveryOrderMaster_DAL(SaleRef, StartDate, EndDate, Customer_Name, ItemId, Status_ID, c_ID).ToPagedList(page ?? 1, 10);


                return PartialView("_DOOtherListSearchPartial", _oSales);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }



        [HttpPost]
        public IActionResult InsertSalesDeliveryOrderDetail( int[] Item_ID, string[] ItemDesc, double?[] Unit_Price,
            double?[] VATRate, double?[] VATExp, double?[] Total_Amount, int hdDOMaster_ID, string[] Quantity, string[] UM_ID, int[] Location_ID, int[] SaleDetails_ID, int[] SaleMaster_Id)
        {



            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            if (Quantity == null)
            {
                return Json(new { message = "Please enter Received Qty!" });
            }




            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = DOOtherTempID;
      
            if (hdDOMaster_ID == 0)
            {
                Temp_ID = DOOtherTempID;
            }
            else
            {
                Temp_ID = "0";
            }

            DOMasterOtherStatic_ID = DOMasterOtherStatic_ID == null ? -1 : DOMasterOtherStatic_ID;

            try
            {

                if (SaleDetails_ID != null && SaleDetails_ID.Length != 0)
                {

                    for (int i = 0; i < SaleDetails_ID.Length; i++)
                    {

                        message = _oSales.InsertSalesDeliveryOrderDetail_DAL(Item_ID[i], ItemDesc[i], Unit_Price[i], VATRate[i],
                    VATExp[i], Total_Amount[i], Temp_ID, c_ID, Created_by, hdDOMaster_ID, SaleMaster_Id[i], Quantity[i], UM_ID[i], Location_ID[i], SaleDetails_ID[i]);

                    }

                }
                else
                {
                    message = "Please Check Any One to perform this task";
                }




                if (message.Contains("Successfully"))
                {
                    //---Inventory detial invoice list for Inventory invoice type Bychassis
                    _oSales.DODetailList = _oSales.Get_SalesDeliveryOrderDetailListByID_DAL(Temp_ID, hdDOMaster_ID).ToList();

                    return PartialView("_DODetail_Other_Partial", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }



        }




        [HttpPost]
        public IActionResult InsertSalesDOInvoiceMaster(int? Customer_ID, string Customer_Contact, string Invoice_address, string DODate, string CustomerTRN,
                 string Manualbook_ref, int? Seller_ID, string Remarks, string Customer_Name, int SaleMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = DOOtherTempID;
            string Saletype = "SI";  //---It is the ref in table for new Payment master. Send this value hard coded from here

            if (Customer_ID == null || String.IsNullOrEmpty(DODate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("SalesDeliveryOrder", "Sales");
            }

            Customer_ID = Customer_ID == null ? 0 : Customer_ID;


            try
            {
                message = _oSales.InsertSalesDeliveryOrderMaster_DAL(Customer_ID, Customer_Contact, Invoice_address, DODate, CustomerTRN, Manualbook_ref,
                    Seller_ID, Temp_ID, c_ID, Created_by, Remarks, Customer_Name,SaleMaster_ID);

                if (message.Contains("Successfully"))
                {
                    

                    return RedirectToAction("SalesDOInvoiceList", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("SalesDeliveryOrder", "Sales");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
                return RedirectToAction("SalesDeliveryOrder", "Sales");
            }


        }

        [HttpPost]
        public IActionResult UpdateSalesDOInvoiceMaster(int DOMaster_ID,int? SaleMaster_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string DODate,
          string CustomerTRN, string Manualbook_ref, int? Seller_ID, string Remarks, string Customer_Name)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = DOOtherTempID;
            string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL(SaleMaster_ID);
            if (!String.IsNullOrEmpty(OldTempID))
            {
                Temp_ID = OldTempID;
            }
            else
            {
                if (String.IsNullOrEmpty(Temp_ID))
                {
                    Guid obj = Guid.NewGuid();
                    Temp_ID = obj.ToString();
                }
            }
            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();


            if (Customer_ID == null || String.IsNullOrEmpty(DODate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            Customer_ID = Customer_ID == null ? 0 : Customer_ID;


            try
            {
                message = _oSales.UpdateSalesDeliveryOrderMaster_DAL(DOMaster_ID, SaleMaster_ID, Customer_ID, Customer_Contact, Invoice_address, DODate, CustomerTRN, Manualbook_ref, Seller_ID, Temp_ID, c_ID, Modified_by, Remarks, Customer_Name);

                if (message.Contains("Successfully"))
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("SalesDOInvoiceList", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }


        }
        [HttpPost]
        public IActionResult DeleteDOMaster(int? DOMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion


            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();

            string message = "";

            if (DOMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oSales.DeleteSalesDeliveryOrderMaster_DAL(DOMaster_ID);
                if (message.Contains("Successfully"))
                {
                    return RedirectToAction("SalesDeliveryOrderList", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }
        //---Delete DO detail other
        [HttpPost]
        public IActionResult DeleteDODetail(int? DODetails_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            Temp_ID = DOOtherTempID;
            string OldTempID = _oSales.GetOldTempIDFromSalesDeliveryOrderDetail_DAL(DOMasterOtherStatic_ID);
            if (!String.IsNullOrEmpty(DOOtherTempID))
            {
                Temp_ID = DOOtherTempID;
            }
            else
            {
                Temp_ID = OldTempID;
            }
            DOMasterOtherStatic_ID = DOMasterOtherStatic_ID == null ? -1 : DOMasterOtherStatic_ID;

            if (DODetails_ID == null)
            {
                return Json(new { message = "ID is null. Please try again!" });
            }

            try
            {
                message = _oSales.DeleteSalesDeliveryOrder_Material_In_DAL(DODetails_ID);
                if (message == "Deleted Successfully!")
                {
                    _oSales.DODetailList = _oSales.Get_SalesDeliveryOrderDetailListByID_DAL(Temp_ID, DOMasterOtherStatic_ID).ToList();

                    return PartialView("_DODetail_Other_Partial", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }

        }


        [HttpGet]
        public IActionResult Get_SaleRefDetails(int SaleMaster_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion




            try
            {
                _oSales.SaleDetailList = _oSales.Get_SalesDetailListByRef_DAL(SaleMaster_ID).ToList();
             

                return PartialView("_DODetailList", _oSales);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }
        [HttpGet]
        public JsonResult GetSaleRefDetail(int SaleMaster_ID)
        {
            string Customer_ID = "0";
            string Customer_Name = "";
            string Customer_Contact = "";
            string Invoice_address = "";
            string CustomerTRN = "";
            string Remarks = "";
            string Manualbook_ref = "";
            string Seller_ID = "";



            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function
            _oSales.SaleMasterObj = _oSales.Get_SalesInvoiceMasterByID_DAL_TRD(SaleMaster_ID);

            if (_oSales.SaleMasterObj.Customer_ID !="" && _oSales.SaleMasterObj.Customer_ID != null)
            {

                Customer_ID = _oSales.SaleMasterObj.Customer_ID;
                Customer_Name = _oSales.SaleMasterObj.Customer_Name;
                Customer_Contact = _oSales.SaleMasterObj.Customer_Contact;
                Invoice_address = _oSales.SaleMasterObj.Invoice_address;
                CustomerTRN = _oSales.SaleMasterObj.CustomerTRN;
                Remarks = _oSales.SaleMasterObj.Remarks;
                Manualbook_ref = _oSales.SaleMasterObj.Manualbook_ref;
                Seller_ID = _oSales.SaleMasterObj.Seller_ID;
      

            }
            else
            {
                Customer_ID = null;
                Customer_Name = null;
                Customer_Contact = null;
                Invoice_address = null;
                CustomerTRN = null;
                Remarks = null;
                Manualbook_ref = null;
                Seller_ID = null;
       
            }


            return Json(new { Customer_ID, Customer_Name, Customer_Contact, Invoice_address, CustomerTRN , Remarks, Manualbook_ref, Seller_ID });

        }
        #endregion

        #region SalesReturn
        //---ohter Sales get method
        [HttpGet]
        public IActionResult SalesReturn(int SaleMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "NewSalesReturn";
            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "SalesReturn";
            ViewBag.SectionSub_HeaderMainTitle = "SalesReturn List";
            ViewBag.PageTitle = "SalesReturn";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            #region ViewBag Area
            ViewBag.UOMList = _oBasic.Get_UOM_DAL(c_ID).ToList();
            ViewBag.CustomerMaster = _oBasic.Get_CustomersList_DAL(c_ID).ToList();

            ViewBag.SellerList = _oBasic.Get_PartnersSeller_DAL(c_ID).ToList();
            ViewBag.AgentsList = _oBasic.Get_PartnersAgent_DAL(c_ID).ToList();
            ViewBag.ExportDestinations = _oBasic.Get_Destinations_DAL(c_ID).ToList();
            ViewBag.PortOfExit = _oBasic.Get_PortOfExit_DAL(c_ID).ToList();
            ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_ID).ToList();
            ViewBag.ItemLocation = _oBasic.Get_ItemLocationList_DAL(c_ID).ToList();
            //---Ven Category list for drop down in forms
            ViewBag.CustomerCategoryList = _oBasic.Get_CustomerCategory_DAL().ToList();
            //---get shipping companies
            ViewBag.ShippingCompanies = _oBasic.Get_ShippingCompaniesList_DAL(c_ID).ToList();
            //--get export companies list
            ViewBag.ExportCompanies = _oBasic.Get_ExportCompaniesList_DAL(c_ID).ToList();
            //--get Basic Banks list
            ViewBag.BasicBanksList = _oBasic.Get_BasicBanksList_DAL(c_ID).ToList();
            //--get showrooms lsit
            ViewBag.ShowroomsList = _oBasic.Get_ShowRoom_DAL(c_ID).ToList();
            //---Get Accounts List
            ViewBag.CR_AccountsList = _oBasic.Select_RV_OnAccounts_DAL(c_ID).ToList();

            //---Get pay via list
           // ViewBag.DR_AccountsList = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();

            ViewBag.DR_AccountsList = _oBasic.Select_PV_OnAccounts_SaleReturn_Trd(c_ID).ToList();
            
            
            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();
            //---Get Item master list
            ViewBag.ItemMaster = _oBasic.Get_ItemMasterList_DAL().ToList();
            ViewBag.SaleRef = _oSales.GetSaleRefDetails_Other_DAL_Return().ToList();

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;



            #endregion


            if (SaleMaster_ID == 0 || SaleMaster_ID < 1)
            {

                //---Create DO master Ref(tem id)
                if (String.IsNullOrEmpty(SalesReturnTempID))
                {
                    Guid obj = Guid.NewGuid();
                    SalesReturnTempID = obj.ToString();
                }

                SalesReturnMasterStatic_ID = SaleMaster_ID;

                //---Get DO master id
                ViewBag.SaleMaster_ID = SaleMaster_ID;

                //--Get DO Master
                _oSales.SalesReturnMasterObj = _oSales.Get_SalesMaster_ByID_DAL_Return(-1);

                //--Get DO detail 
                _oSales.SalesReturnDetailList = _oSales.Get_SalesDetailListByID_DAL_Return(SalesReturnTempID, -1).ToList();
            }
            else
            {

                Urls = null;

                ApprovalURL = HttpContext.Request.Headers["Referer"];
                Urls = ApprovalURL;

                //---Get old temp id from DO detail table that already creaded for this "SaleMaster_ID" id. And then use this if new record added
                string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL_Return(SaleMaster_ID);

        

                SalesReturnMasterStatic_ID = SaleMaster_ID;

                //---Get DO master id
                ViewBag.SaleMaster_ID = SaleMaster_ID;



                //--Get DO Master
                _oSales.SalesReturnMasterObj = _oSales.Get_SalesMaster_ByID_DAL_Return(SaleMaster_ID);

                //--Get DO detail
                _oSales.SalesReturnDetailList = _oSales.Get_SalesDetailListByID_DAL_Return(OldTempID, SaleMaster_ID).ToList();
                _oSales.PaymentDetailList = _oSales.Select_PaymentList_PVSV_ByID_DAL(OldTempID, SaleMaster_ID).ToList();

            }



            return View(_oSales);
        }

        //---Getting Other DO list

        [HttpGet]
        public IActionResult SalesReturnList(string SaleRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string ItemId, int c_ID, int? page)
        {
            //---dumping static fields
            DumpStaticFieldsSales();

            //---BreadCrumbs defintion
            //  ViewBag.BreadCumController = "Sales";
            //  ViewBag.BreadCumAction = "SalesInvoiceList";

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "ReturnList";

            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "Delivery Order List";

            ViewBag.PageTitle = "Delivery Order List";




            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region Veiwbags area


            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            //---Status
            ViewBag.ddlStatus = _oBasic.get_SalesTrans_Status_DAL().ToList();
            ViewBag.AllItems = _oBasic.Get_ItemMasterList_DAL().ToList();

            #endregion veiwbags area ends here

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            ItemId = String.IsNullOrEmpty(ItemId) ? "" : ItemId;



            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;


            ViewBag.SaleRef_SaleInvoice = SaleRef;
            ViewBag.StartDate_SaleInvoice = StartDate;
            ViewBag.EndDate_SaleInvoice = EndDate;
            ViewBag.Customer_Name_SaleInvoice = Customer_Name;
            ViewBag.ItemId_SaleInvoice = ItemId;


            ViewBag.c_ID_SaleInvoice = c_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;
            //---purchase sales master invoice list for page
            ViewBag.RecordsPerPage = 10;

            //_oSales.SalesReturnMasterList = _oSales.Get_SalesMaster_DAL_Return(SaleRef, StartDate, EndDate, Customer_Name, ItemId, Status_ID, c_ID).ToPagedList(page ?? 1, 10);
            _oSales.SalesReturnMasterList = _oSales.Get_SalesMaster_DAL_Return_trd(SaleRef, StartDate, EndDate, Customer_Name, ItemId, Status_ID, c_ID).ToPagedList(page ?? 1, 10);


            return View(_oSales);
        }
        //---Get Sales master invoice list by search filters
        [HttpGet]
        public IActionResult GetSalesReturnMasterInvoiceListBySearchFilers(string SaleRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string ItemId, int c_ID, int? page)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion
            c_ID = c_ID == 0 ? 1 : c_ID;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            ItemId = String.IsNullOrEmpty(ItemId) ? "" : ItemId;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;


            ViewBag.SaleRef_SaleInvoice = SaleRef;
            ViewBag.StartDate_SaleInvoice = StartDate;
            ViewBag.EndDate_SaleInvoice = EndDate;
            ViewBag.Customer_Name_SaleInvoice = Customer_Name;
            ViewBag.ItemId_SaleInvoice = ItemId;
            ViewBag.c_ID_SaleInvoice = c_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;
            try
            {
                //---purchase sales master invoice list for page
                ViewBag.RecordsPerPage = 10;
                //_oSales.SalesReturnMasterList = _oSales.Get_SalesMaster_DAL_Return(SaleRef, StartDate, EndDate, Customer_Name, ItemId, Status_ID, c_ID).ToPagedList(page ?? 1, 10);
                _oSales.SalesReturnMasterList = _oSales.Get_SalesMaster_DAL_Return_trd(SaleRef, StartDate, EndDate, Customer_Name, ItemId, Status_ID, c_ID).ToPagedList(page ?? 1, 10);


                return PartialView("_SalesReturnListSearchPartial", _oSales);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }



        [HttpPost]
        public IActionResult InsertSalesReturnDetail(int[] Item_ID, string[] ItemDesc, double?[] Unit_Price,
            double?[] VATRate, double?[] VATExp, double?[] Total_Amount, int hdSaleMaster_ID, string[] Quantity, string[] UM_ID, int[] Location_ID, int[] SaleDetails_ID, int[] SaleMaster_Id)
        {



            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            if (Quantity == null)
            {
                return Json(new { message = "Please enter Received Qty!" });
            }




            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = SalesReturnTempID;

            if (hdSaleMaster_ID == 0)
            {
                Temp_ID = SalesReturnTempID;
            }
            else
            {
                Temp_ID = "0";
            }

            SalesReturnMasterStatic_ID = SalesReturnMasterStatic_ID == null ? -1 : SalesReturnMasterStatic_ID;

            try
            {

                if (SaleDetails_ID != null && SaleDetails_ID.Length != 0)
                {

                    for (int i = 0; i < SaleDetails_ID.Length; i++)
                    {
                        

                        message = _oSales.InsertSalesDetail_DAL_Return(Item_ID[i], ItemDesc[i], Unit_Price[i], VATRate[i],
                    VATExp[i], Total_Amount[i], Temp_ID, c_ID, Created_by, hdSaleMaster_ID, SaleMaster_Id[i], Quantity[i], UM_ID[i], Location_ID[i], SaleDetails_ID[i]);

                    }

                }
                else
                {
                    message = "Please Check Any One to perform this task";
                }




                if (message.Contains("Successfully"))
                {
                    //---Inventory detial invoice list for Inventory invoice type Bychassis
                    _oSales.SalesReturnDetailList = _oSales.Get_SalesDetailListByID_DAL_Return(Temp_ID, hdSaleMaster_ID).ToList();

                    return PartialView("_SaleReturnDetail_Other_Partial", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }



        }




        [HttpPost]
        public IActionResult InsertSalesInvoiceMaster_Return(int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string CustomerTRN,
                 string Manualbook_ref, int? Seller_ID, string Remarks, string Customer_Name, int SaleMasterRef_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = SalesReturnTempID;
             //---It is the ref in table for new Payment master. Send this value hard coded from here

            if (Customer_ID == null || String.IsNullOrEmpty(SaleDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("SalesReturn", "Sales");
            }

            Customer_ID = Customer_ID == null ? 0 : Customer_ID;
            string SaleType = "SX";

            try
            {
                message = _oSales.InsertSalesMaster_DAL_Return(Customer_ID, Customer_Contact, Invoice_address, SaleDate, CustomerTRN, Manualbook_ref,
                    Seller_ID, Temp_ID, c_ID, Created_by, Remarks, Customer_Name, SaleType, SaleMasterRef_ID);

                if (message.Contains("Successfully"))
                {


                    return RedirectToAction("SalesReturnList", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("SalesReturn", "Sales");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
                return RedirectToAction("SalesReturn", "Sales");
            }


        }

        [HttpPost]
        public IActionResult UpdateSalesMaster_Return(int SaleMaster_ID, int? SaleMasterRef_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate,
          string CustomerTRN, string Manualbook_ref, int? Seller_ID, string Remarks, string Customer_Name)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = SalesReturnTempID;
            string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL(SaleMaster_ID);
            if (!String.IsNullOrEmpty(OldTempID))
            {
                Temp_ID = OldTempID;
            }
            else
            {
                if (String.IsNullOrEmpty(Temp_ID))
                {
                    Guid obj = Guid.NewGuid();
                    Temp_ID = obj.ToString();
                }
            }
            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();


            if (Customer_ID == null || String.IsNullOrEmpty(SaleDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            Customer_ID = Customer_ID == null ? 0 : Customer_ID;


            try
            {
                message = _oSales.UpdateSalesMaster_DAL_Return(SaleMaster_ID, SaleMasterRef_ID, Customer_ID, Customer_Contact, Invoice_address, SaleDate, CustomerTRN, Manualbook_ref, Seller_ID, Temp_ID, c_ID, Modified_by, Remarks, Customer_Name);

                if (message.Contains("Successfully"))
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("SalesReturnList", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }


        }
        [HttpPost]
        public IActionResult DeleteMaster_Return(int? SaleMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion


            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();

            string message = "";

            if (SaleMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oSales.DeleteSalesMaster_DAL_Return(SaleMaster_ID);
                if (message.Contains("Successfully"))
                {
                    return RedirectToAction("SalesReturnList", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }
        //---Delete DO detail other
        [HttpPost]
        public IActionResult DeleteDetail_Return(int? SalesDetails_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            Temp_ID = SalesReturnTempID;
            string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL_Return(SalesReturnMasterStatic_ID);
            if (!String.IsNullOrEmpty(SalesReturnTempID))
            {
                Temp_ID = SalesReturnTempID;
            }
            else
            {
                Temp_ID = OldTempID;
            }
            SalesReturnMasterStatic_ID = SalesReturnMasterStatic_ID == null ? -1 : SalesReturnMasterStatic_ID;

            if (SalesDetails_ID == null)
            {
                return Json(new { message = "ID is null. Please try again!" });
            }

            try
            {
                message = _oSales.DeleteSalesDetail_DAL_Return(SalesDetails_ID);
                if (message == "Deleted Successfully!")
                {
                    _oSales.SalesReturnDetailList = _oSales.Get_SalesDetailListByID_DAL_Return(Temp_ID, SalesReturnMasterStatic_ID).ToList();

                    return PartialView("_SaleReturnDetail_Other_Partial", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }

        }


        [HttpGet]
        public IActionResult Get_SaleRefDetails_Return(int SaleMaster_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion




            try
            {
                _oSales.SaleDetailList = _oSales.Get_SalesDetailListByRef_DAL_Return(SaleMaster_ID).ToList();


                return PartialView("_SalesReturnDetailList", _oSales);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }

        #endregion
        #region AfterSalesJP
        [HttpGet]
        public IActionResult PaperAfterSalesMaster_jp(int papers_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "NewPaperAfterSales";
            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "Paper AfterSales";
            ViewBag.PageTitle = "Paper AfterSales";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion



            if (papers_ID == null || papers_ID < 1)
            {


                //---Create Purchase master Ref(tem id)
                if (String.IsNullOrEmpty(PaperAfterSaleTempID))
                {
                    Guid obj = Guid.NewGuid();
                    PaperAfterSaleTempID = obj.ToString();
                }

                PaperAfterSaleStatic_ID = papers_ID;

                //---Get purchase master id
                ViewBag.papers_ID = papers_ID;

                //--Get purchase Master
                _oSales.PaperAfterSalesMasterObj = _oSales.Get_Pa_Select_paperAfterSaleMaster_jp_ByID(-1);

                //--Get purchase detail 
                _oSales.PaperAfterSalesDetailList = _oSales.Get_paperAfterSaleDetails_DAL(PaperAfterSaleTempID, -1).ToList();
            }
            else
            {



                //---Get old temp id from purchase detail table that already creaded for this "papers_ID" id. And then use this if new record added
                string OldTempID = _oSales.GetOldTempIDFrom_DAL(papers_ID);



                PaperAfterSaleStatic_ID = papers_ID;

                //---Get purchase master id
                ViewBag.papers_ID = papers_ID;



                //--Get purchase Master
                _oSales.PaperAfterSalesMasterObj = _oSales.Get_Pa_Select_paperAfterSaleMaster_jp_ByID(papers_ID);

                //--Get purchase detail
                _oSales.PaperAfterSalesDetailList = _oSales.Get_paperAfterSaleDetails_DAL(OldTempID, papers_ID).ToList();
            }



            return View(_oSales);
        }

        //---method for inserting return purchase record that already exists in purchase detail table and then returning the list of new insered record
        [HttpGet]
        public IActionResult Insert_And_GetpaperAfterSaleDetails(string SaleRef,string Chassis, int hdpapers_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = PaperAfterSaleTempID;
            string OldTempID = _oSales.GetOldTempIDFrom_DAL(PaperAfterSaleStatic_ID);
            if (hdpapers_ID == 0)
            {
                Temp_ID = PaperAfterSaleTempID;

            }
            else
            {
                Temp_ID = "0";
            }
            PaperAfterSaleStatic_ID = PaperAfterSaleStatic_ID == null ? -1 : PaperAfterSaleStatic_ID;

    

            try
            {
                if (!string.IsNullOrEmpty(SaleRef))
                {
                    message = _oSales.CheckIfRefExistInSalesMaster_jp_DAL(SaleRef);
                }
               else if (!string.IsNullOrEmpty(Chassis))
                {
                    message = _oSales.CheckIfRefExistInChassis_DAL(Chassis);
                }
                
                if (message == "Ref Exists")
                {
                    if (!string.IsNullOrEmpty(SaleRef))
                    {
                        message = _oSales.Insert_paperAfterSaleDetails_jp(Temp_ID, SaleRef, hdpapers_ID);
                    }
                    else if (!string.IsNullOrEmpty(Chassis))
                    {
                        message = _oSales.Insert_paperAfterSaleDetails_Chassis_jp(Temp_ID, Chassis, hdpapers_ID);
                    }
                
                    if (message.Contains("Successfully!"))
                    {
                        _oSales.PaperAfterSalesDetailList = _oSales.Get_paperAfterSaleDetails_DAL(Temp_ID, hdpapers_ID).ToList();

                        return PartialView("_paperAfterSaleDetails", _oSales);
                    }
                    else
                    {
                        return Json(new { message = message });
                    }


                }
                else if (message == "Ref Not Exists")
                {
                    return Json(new { message = message });
                }
                else
                {
                    return Json(new { message = "Ref Not Exists" });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }

        }

        //---this method for deleting purchase detail return
        [HttpPost]
        public IActionResult DeleteAfterSaleDetail_Return(int? papers_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            Temp_ID = PaperAfterSaleTempID;
            string OldTempID = _oSales.GetOldTempIDFrom_DAL(PaperAfterSaleStatic_ID);
            if (!String.IsNullOrEmpty(PaperAfterSaleTempID))
            {
                Temp_ID = PaperAfterSaleTempID;
            }
            else
            {
                Temp_ID = OldTempID;
            }
            PaperAfterSaleStatic_ID = PaperAfterSaleStatic_ID == null ? -1 : PaperAfterSaleStatic_ID;

            if (papers_ID == null)
            {
                return Json(new { message = "ID is null. Please try again!" });
            }

            try
            {
                message = _oSales.DeleteaperAfterSaleDetails_DAL(papers_ID);
                if (message == "Deleted Successfully!")
                {
                    _oSales.PaperAfterSalesDetailList = _oSales.Get_paperAfterSaleDetails_DAL(Temp_ID, PaperAfterSaleStatic_ID).ToList();

                    return PartialView("_paperAfterSaleDetails", _oSales);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }

        }




        //This method is for inserting return purchase Master
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertpaperAfterSaleMaster(string DateCreated, string DateSubmission, string DateValid, string Remarks)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = PaperAfterSaleTempID;
            string PurchaseType = "PX";  //---It is the ref in table for new purchase. Send this value hard coded from here

            if (String.IsNullOrEmpty(DateCreated))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("PaperAfterSalesMaster_jp", "Sales");
            }

          


            try
            {
                message = _oSales.Insert_paperAfterSaleMaster_Return_DAL(DateCreated, DateSubmission, DateValid, Remarks, Temp_ID, Created_by);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("PaperAfterSalesMasterList", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("PaperAfterSalesMaster_jp", "Sales");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("PaperAfterSalesMaster_jp", "Sales");
            }



        }

        //This method is for updating  purchase Master return
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatepaperAfterSaleMaster(int? papers_ID, string DateCreated, string DateSubmission, string DateValid, string Remarks)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = PaperAfterSaleTempID;
            string OldTempID = _oSales.GetOldTempIDFrom_DAL(papers_ID);
            if (!String.IsNullOrEmpty(OldTempID))
            {
                Temp_ID = OldTempID;
            }
            else
            {
                if (String.IsNullOrEmpty(Temp_ID))
                {
                    Guid obj = Guid.NewGuid();
                    Temp_ID = obj.ToString();
                }
            }

            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();


            if (String.IsNullOrEmpty(DateCreated))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            if (papers_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again!";
                return Redirect(url);
            }





            try
            {
                message = _oSales.Update_paperAfterSaleMaster_DAL(papers_ID, DateCreated, DateSubmission, DateValid, Remarks, Temp_ID, Modified_by);
                if (message == "Updated Successfully!")
                {

                    //TempData["Success"] = "Updated Successfully!";
                    return RedirectToAction("PaperAfterSalesMasterList", "Sales");
                }
                else
                {


                    TempData["Error"] = "An error occured. Please try again!";
                    return Redirect(url);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }



        }

        //---Method for deleting purchase master return
        [HttpPost]
        public IActionResult DeletePurchaseMaster_Return(int? papers_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion


            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();

            string message = "";

            if (papers_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                //message = _oSales.DeletePurchaseMaster_DAL(papers_ID, 0);
                if (message.Contains("SUCCESSFULLY"))
                {
                    return RedirectToAction("PaperAfterSalesMasterList", "Sales");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }

        [HttpGet]
        public IActionResult PaperAfterSalesMasterList(string Chassis_No, string EndDate, string StartDate, string SaleRef, int? page)
        {
            //---Dumping static fields
            DumpStaticFieldsSales();


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "Paper After Sales Master List";
            ViewBag.PageTitle = "Paper After Sales Master List";

            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_No = String.IsNullOrEmpty(Chassis_No) ? "" : Chassis_No;


            ViewBag.RecordsPerPage = 10;
            _oSales.PaperAfterSalesMaster_jpMasterList = _oSales.Get_PaperAfterSalesMasterList_DAL( Chassis_No,  EndDate,  StartDate,  SaleRef).ToList().ToPagedList(page ?? 1, 10);
            return View(_oSales);


        }

        //---Get Othes Purchase list by search filters
        [HttpGet]
        public IActionResult GetPaperAfterSalesMasterList_BySearchFitlers(string Chassis_No, string EndDate, string StartDate, string SaleRef, int? page)
        {
            string message = "";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_No = String.IsNullOrEmpty(Chassis_No) ? "" : Chassis_No;
            try
            {
                ViewBag.RecordsPerPage = 10;
                _oSales.PaperAfterSalesMaster_jpMasterList = _oSales.Get_PaperAfterSalesMasterList_DAL(Chassis_No, EndDate, StartDate, SaleRef).ToList().ToPagedList(page ?? 1, 10);
                return PartialView("_PaperAfterSalesMasterListSearchPartial", _oSales);
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }
        #endregion
        #region QRGENERATOR
        //public static string EncryptString(string key, string plainText)
        //{
        //    byte[] iv = new byte[16];
        //    byte[] array;

        //    using (Aes aes = Aes.Create())
        //    {
        //        aes.Key = Encoding.UTF8.GetBytes(key);
        //        aes.IV = iv;

        //        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        //        using (MemoryStream memoryStream = new MemoryStream())
        //        {
        //            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
        //            {
        //                using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
        //                {
        //                    streamWriter.Write(plainText);
        //                }

        //                array = memoryStream.ToArray();
        //            }
        //        }
        //    }

        //    return Convert.ToBase64String(array);
        //}
        public string QRGenerator(int? SaleMaster_ID,string c_ID)
        {
            string url = configuration.GetSection("AppSettings").GetSection("SalesURL").Value;
            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            var writer = new QRCodeWriter();
            //generate QR Code
            //var key = "b14ca5898a4e4133bbce2ea2315a1916";
            //var encryptedString = EncryptString(key, SaleMaster_ID.ToString());
            var resultBit = writer.encode(url+SaleMaster_ID+ "&c_id="+c_ID, BarcodeFormat.QR_CODE, 75, 75);
            //get Bitmatrix result
            var matrix = resultBit;

            //convert bitmatrix into image 
            int scale = 2;

            Bitmap result = new Bitmap(matrix.Width * scale, matrix.Height * scale);
            for (int x = 0; x < matrix.Height; x++)
                for (int y = 0; y < matrix.Width; y++)
                {
                    Color pixel = matrix[x, y] ? Color.Black : Color.White;
                    for (int i = 0; i < scale; i++)
                        for (int j = 0; j < scale; j++)
                            result.SetPixel(x * scale + i, y * scale + j, pixel);
                }
            string filepath = "";
            string imgname = "";
            #region
            int length = 4;
            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }

            //appending str_build text to image name
            imgname = str_build.ToString()+"_"+SaleMaster_ID+"QR.png";
            #endregion

            //get wwwroot folder location
            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;


            filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{imgname}";

            //save result as png inside 'Images' folder
            result.Save(filepath);
            return imgname;
        }
        public string QRGenerator_Performa(int? SaleMaster_ID,string c_ID)
        {
            string url = configuration.GetSection("AppSettings").GetSection("PerformaURL").Value;
            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            var writer = new QRCodeWriter();
            //generate QR Code
            //var key = "b14ca5898a4e4133bbce2ea2315a1916";
            //var encryptedString = EncryptString(key, SaleMaster_ID.ToString());
            var resultBit = writer.encode(url+SaleMaster_ID+ "&c_id="+ c_ID, BarcodeFormat.QR_CODE, 75, 75);
            //get Bitmatrix result
            var matrix = resultBit;

            //convert bitmatrix into image 
            int scale = 2;

            Bitmap result = new Bitmap(matrix.Width * scale, matrix.Height * scale);
            for (int x = 0; x < matrix.Height; x++)
                for (int y = 0; y < matrix.Width; y++)
                {
                    Color pixel = matrix[x, y] ? Color.Black : Color.White;
                    for (int i = 0; i < scale; i++)
                        for (int j = 0; j < scale; j++)
                            result.SetPixel(x * scale + i, y * scale + j, pixel);
                }
            string filepath = "";
            string imgname = "";
            #region
            int length = 4;
            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }

            //appending str_build text to image name
            imgname = str_build.ToString() + "_" + SaleMaster_ID + "QR.png";
            #endregion

            //get wwwroot folder location
            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;


            filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{imgname}";

            //save result as png inside 'Images' folder
            result.Save(filepath);
            return imgname;
        }
        #endregion


        #region Sale Dashboard

      
        public IActionResult SalesDashboard(int? Make, int? Model_Desc, int? Color, int? page, string StartDate, string EndDate, string Model_Year)
        {

            //---dumping static fields
            DumpStaticFieldsSales();

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "SalesDashboard";

            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "Sales Dashboard";

            ViewBag.PageTitle = "Sales Dashboard";


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region Veiwbags area
            //---Status
            ViewBag.ColorsList = _oBasic.Get_ColorsList_DAL().ToList();
            ViewBag.ddlStatus = _oBasic.get_SalesTrans_Status_DAL().ToList();
            ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_IDs).ToList();
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            #endregion veiwbags area ends here

            // string company_ID = HttpContext.Session.GetString("c_ID");
            // int c_IDs = Convert.ToInt32(company_ID);
            c_IDs = c_IDs == 0 ? 0 : c_IDs;
            Make = Make == null ? 0 : Make;
            Model_Desc = Model_Desc == null ? 0 : Model_Desc;
            Color = Color == null ? 0 : Color;
           // page = page == null ? 0 : page;
            Model_Year = String.IsNullOrEmpty(Model_Year) ? "" : Model_Year;
            StartDate = string.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = string.IsNullOrEmpty(EndDate) ? "" : EndDate;
            //ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_ID).ToList();

            ViewBag.Make = Make;
            ViewBag.Model_Desc = Model_Desc;
            ViewBag.Color = Color;
            ViewBag.Model_Year = Model_Year;
            ViewBag.c_IDs = c_IDs;
            ViewBag.StartDate_SaleInvoice = StartDate;
            ViewBag.EndDate_SaleInvoice = EndDate;

            //---purchase sales master invoice list for page
            ViewBag.RecordsPerPage = 10;

             _oSales.SalesDashboardIPagedList = _oSales.Pa_Select_SaleDashboard(Make, Model_Desc, Color, StartDate, EndDate, Model_Year, c_IDs).ToPagedList(page ?? 1, 10);
            //_oSales.SalesMasterListTOTAL = _oSales.Get_SalesMasterInvoiceList_TTL_DAL(SaleRef, ManualRef, StartDate, EndDate, Customer_Name, Chassis_No, Status_ID, c_ID, Make, Model_Desc, Model_Year).ToList();

             

            return View(_oSales);

        }



        [HttpGet]
        public IActionResult GetSalesDashboardListBySearchFilers(int? Make, int? Model_Desc, int? Color, int? page, string StartDate, string EndDate, string Model_Year)
        {

            DumpStaticFieldsSales();

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "SalesDashboard";

            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "Sales Dashboard";

            ViewBag.PageTitle = "Sales Dashboard";


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region Veiwbags area
            //---Status
            ViewBag.ColorsList = _oBasic.Get_ColorsList_DAL().ToList();
            ViewBag.ddlStatus = _oBasic.get_SalesTrans_Status_DAL().ToList();
            ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_IDs).ToList();
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            #endregion veiwbags area ends here

            // string company_ID = HttpContext.Session.GetString("c_ID");
            // int c_IDs = Convert.ToInt32(company_ID);
            c_IDs = c_IDs == 0 ? 0 : c_IDs;
            Make = Make == null ? 0 : Make;
            Model_Desc = Model_Desc == null ? 0 : Model_Desc;
            Color = Color == null ? 0 : Color;
            // page = page == null ? 0 : page;
            Model_Year = String.IsNullOrEmpty(Model_Year) ? "" : Model_Year;
            StartDate = string.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = string.IsNullOrEmpty(EndDate) ? "" : EndDate;
            //ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_ID).ToList();

            ViewBag.Make = Make;
            ViewBag.Model_Desc = Model_Desc;
            ViewBag.Color = Color;
            ViewBag.Model_Year = Model_Year;
            ViewBag.c_IDs = c_IDs;
            ViewBag.StartDate_SaleInvoice = StartDate;
            ViewBag.EndDate_SaleInvoice = EndDate;

            //---purchase sales master invoice list for page
            ViewBag.RecordsPerPage = 10;
            try
            {
                //---purchase sales master invoice list for page
                ViewBag.RecordsPerPage = 10;
                _oSales.SalesDashboardIPagedList = _oSales.Pa_Select_SaleDashboard(Make, Model_Desc, Color, StartDate, EndDate, Model_Year, c_IDs).ToPagedList(page ?? 1, 10);
                //_oSales.SalesMasterListTOTAL = _oSales.Get_SalesMasterInvoiceList_TTL_DAL(SaleRef, ManualRef, StartDate, EndDate, Customer_Name, Chassis_No, Status_ID, c_ID, Make, Model_Desc, Model_Year).ToList();



                return PartialView("_SalesDashboardListSearch", _oSales);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }

        [HttpGet]
        public IActionResult GetSaleDetails(string Make,string Model_Desc,string Model_Year,string Ext_Color,string Production_Date)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            c_IDs = c_IDs == 0 ? 0 : c_IDs;
           
            // page = page == null ? 0 : page;
            Model_Year = String.IsNullOrEmpty(Model_Year) ? "" : Model_Year;
            Make = string.IsNullOrEmpty(Make) ? "" : Make;
            Model_Desc = string.IsNullOrEmpty(Model_Desc) ? "" : Model_Desc;
            Ext_Color = string.IsNullOrEmpty(Ext_Color) ? "" : Ext_Color;
            Production_Date = string.IsNullOrEmpty(Production_Date) ? "" : Production_Date;
            _oSales.pa_SelectSaleDetails(Make, Model_Desc, Model_Year, Ext_Color, Production_Date, c_IDs).ToList();

            return View("SalesDashboard", _oSales);
        }
        #endregion



        #region Payment Details

        //---insert into payment detial general
        [HttpPost]
        public IActionResult InsertPaymentDetail(int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
            string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount, int hdSaleMaster_ID)
                {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = SalesInvoiceMaster_TempID;
            if (hdSaleMaster_ID == 0)
            {
                Temp_ID = SalesInvoiceMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            // SaleInvoiceMasterStatic_ID = SaleInvoiceMasterStatic_ID == null ? -1 : SaleInvoiceMasterStatic_ID;



            if (DR_accountID == null || Amount == null || CR_accountID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            try
            {
                message = _oSales.InsertPaymentDetail_PVSV_DAL(DR_accountID, Amount, VAT_Rate, Currency_ID, Currency_Rate, Amount_Others, CR_accountID, Description,
             trans_ref, cheque_bank_name, cheque_date, cheque_no, VAT_Exp, Total_Amount, Temp_ID, c_ID, Created_by, hdSaleMaster_ID);
                if (message == "Saved Successfully!")
                {
                    _oSales.PaymentDetailList = _oSales.Select_PaymentList_PVSV_ByID_DAL(Temp_ID, hdSaleMaster_ID).ToList();

                    return PartialView("_PaymentDetail_SaleList_Partial_Sales", _oSales);
                }
                else
                {
                    return Json(new { message = "Record not saved! Please try again!" });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured! Please try again!" });
            }


        }


        //---Update  payment detail of deposit return
        [HttpPost]
        public IActionResult UpdatePaymentDetail(int? PaymentDetails_ID, int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
            string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount, int hdSaleMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = SalesInvoiceMaster_TempID;
            if (hdSaleMaster_ID == 0)
            {
                Temp_ID = SalesInvoiceMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }
            SaleInvoiceMasterStatic_ID = SaleInvoiceMasterStatic_ID == null ? -1 : SaleInvoiceMasterStatic_ID;

            if (PaymentDetails_ID == null)
            {
                return Json(new { message = "Payment detail id is null. Please try again!" });
            }

            if (DR_accountID == null || Amount == null || CR_accountID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            try
            {
                message = _oSales.UpdateSalesPayment_PVSV_DAL(PaymentDetails_ID, DR_accountID, Amount, VAT_Rate, Currency_ID, Currency_Rate, Amount_Others, CR_accountID, Description,
             trans_ref, cheque_bank_name, cheque_date, cheque_no, VAT_Exp, Total_Amount, Temp_ID, c_ID, Modified_by);
                if (message == "Updated Successfully!")
                {

                    _oSales.PaymentDetailList = _oSales.Select_PaymentList_PVSV_ByID_DAL(Temp_ID, hdSaleMaster_ID).ToList();

                    return PartialView("_PaymentDetail_SaleList_Partial_Sales", _oSales);

                }
                else
                {
                    return Json(new { message = "Record not saved! Please try again!" });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured! Please try again!" });
            }

        }



        //---Delete  payment detail general
        [HttpPost]
        public IActionResult DeletePaymentDetail(int? PaymentDetails_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion
            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();

            string message = "";

            if (PaymentDetails_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }




            try
            {
                message = _oSales.DeletePaymentDetail_DAL(PaymentDetails_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = message;
                    return Redirect(url);
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }

        #endregion




        // By Yaseen 2-14-2022 
        #region Sale Invoice For Qunooz
        [HttpGet]
        public IActionResult NewSalesInvoice__Onl_TRD(int SaleMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "NewSalesInvoice";
            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "Sales Invoice";
            ViewBag.SectionSub_HeaderMainTitle = "Sales";
            ViewBag.PageTitle = "Sales Invoice";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            #region ViewBag Area
            //---Get Customer Master
            ViewBag.UOMList = _oBasic.Get_UOM_DAL(c_ID).ToList();
            ViewBag.CustomerMaster = _oBasic.Get_CustomersList_DAL(c_ID).ToList();

            ViewBag.SellerList = _oBasic.Get_PartnersSeller_DAL(c_ID).ToList();
            ViewBag.AgentsList = _oBasic.Get_PartnersAgent_DAL(c_ID).ToList();
            ViewBag.ExportDestinations = _oBasic.Get_Destinations_DAL(c_ID).ToList();
            ViewBag.PortOfExit = _oBasic.Get_PortOfExit_DAL(c_ID).ToList();
            ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_ID).ToList();
            ViewBag.ItemLocation = _oBasic.Get_ItemLocationList_DAL(c_ID).ToList();
            //---Ven Category list for drop down in forms
            ViewBag.CustomerCategoryList = _oBasic.Get_CustomerCategory_DAL().ToList();
            //---get shipping companies
            ViewBag.ShippingCompanies = _oBasic.Get_ShippingCompaniesList_DAL(c_ID).ToList();
            //--get export companies list
            ViewBag.ExportCompanies = _oBasic.Get_ExportCompaniesList_DAL(c_ID).ToList();
            //--get Basic Banks list
            ViewBag.BasicBanksList = _oBasic.Get_BasicBanksList_DAL(c_ID).ToList();
            //--get showrooms lsit
            ViewBag.ShowroomsList = _oBasic.Get_ShowRoom_DAL(c_ID).ToList();


            //---Get Accounts List
            ViewBag.DR_AccountsList = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();

            //---Get pay via list
            ViewBag.CR_AccountsList = _oBasic.Select_RV_OnAccounts_DAL(c_ID).ToList();

            //---Get Accounts List
            ViewBag.DR_AccountsListPD = _oBasic.Select_PV_OnAccounts_Trd(c_ID).ToList();

            ViewBag.CR_AccountsListPD = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();

            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();
            //---Get Item master list
            ViewBag.ItemMaster = _oBasic.Get_ItemMasterList_DAL().ToList();

            #endregion

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;


            if (SaleMaster_ID == 0 || SaleMaster_ID < 1)
            {


                //---Create payment master Ref(tem id)
                if (String.IsNullOrEmpty(SalesInvoiceMaster_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    SalesInvoiceMaster_TempID = obj.ToString();
                }

                SaleInvoiceMasterStatic_ID = SaleMaster_ID;

                //---Get sales master id
                ViewBag.SaleMaster_ID = SaleMaster_ID;

                //--Get sales Master
                _oSales.SaleMasterObj = _oSales.Get_SalesInvoiceMasterByID_DAL_TRD(-1);

                //---sales detial invoice list for sales invoice type Bychassis
                _oSales.SalesDetailList_trd_online = _oSales.Get_SalesIvoiceDetailListByChassis_DAL_TRD(SalesInvoiceMaster_TempID, -1).ToList();
            }
            else
            {
                Urls = null;

                ApprovalURL = HttpContext.Request.Headers["Referer"];
                Urls = ApprovalURL;
                //---Get old temp id from sales detail table that already creaded for this "SaleMaster_ID" id. And then use this if new record added
                string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL(SaleMaster_ID);




                SaleInvoiceMasterStatic_ID = SaleMaster_ID;

                //---Get sale master id
                ViewBag.SaleMaster_ID = SaleMaster_ID;

                //--Get sale Master

                _oSales.SaleMasterObj = _oSales.Get_SalesInvoiceMasterByID_DAL_TRD(SaleMaster_ID);

                //---sales detial invoice list for sales invoice type Bychassis
                _oSales.SalesDetailList_trd_online = _oSales.Get_SalesIvoiceDetailListByChassis_DAL_TRD(OldTempID, SaleMaster_ID).ToList();

                _oSales.SalesReceiptDetailsList = _oSales.Get_ReceiptDetailSaleListByID_DAL_Invoice("", SaleMaster_ID).ToList();

                _oSales.PaymentDetailList = _oSales.Select_PaymentList_PVSV_ByID_DAL("", SaleMaster_ID).ToList();


            }



            return View(_oSales);
        }


        [HttpGet]
        public IActionResult SalesInvoiceList_Onl_TRD(string SaleRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string ItemId, string MNumber, int c_ID, string OrderRef, string OrderStatus ,int? page)
        {
            //---dumping static fields
            DumpStaticFieldsSales();

            //---BreadCrumbs defintion
            //  ViewBag.BreadCumController = "Sales";
            //  ViewBag.BreadCumAction = "SalesInvoiceList";

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "SalesInvoiceList";

            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "Sale Invoice List";

            ViewBag.PageTitle = "Sales Invoice List";




            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region Veiwbags area


            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            //---Status
            ViewBag.ddlStatus = _oBasic.get_SalesTrans_Status_DAL().ToList();
            //Order Status
            ViewBag.OSStatus = _oBasic.get_SalesOther_Status_DAL().ToList();

            ViewBag.AllItems = _oBasic.Get_ItemMasterList_DAL().ToList();

            #endregion veiwbags area ends here

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            ItemId = String.IsNullOrEmpty(ItemId) ? "" : ItemId;
            MNumber = String.IsNullOrEmpty(MNumber) ? "" : MNumber;
            OrderRef = string.IsNullOrEmpty(OrderRef) ? "" : OrderRef;
           
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;
            Status_ID = Status_ID == 0 ? 0 : Status_ID;

            OrderStatus = string.IsNullOrEmpty(OrderStatus) ? "" : OrderStatus;

            ViewBag.SaleRef_SaleInvoice = SaleRef;
            ViewBag.StartDate_SaleInvoice = StartDate;
            ViewBag.EndDate_SaleInvoice = EndDate;
            ViewBag.Customer_Name_SaleInvoice = Customer_Name;
            ViewBag.ItemId_SaleInvoice = ItemId;
            ViewBag.MNumber_SaleInvoice = MNumber;
            ViewBag.Status_ID_SaleInvoice = Status_ID;
            ViewBag.c_ID_SaleInvoice = c_ID;
            ViewBag.OrderRef_SaleInvoice = OrderRef;
            ViewBag.OrderStatus_SaleInvoice = OrderStatus;

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;
            //---purchase sales master invoice list for page
            ViewBag.RecordsPerPage = 10;

            _oSales.SalesMasterIPagedList = _oSales.Get_SalesMasterInvoiceList_DAL_TRD(SaleRef, StartDate, EndDate, Customer_Name, ItemId, MNumber, Status_ID, c_ID, OrderRef, OrderStatus).ToPagedList(page ?? 1, 10);
            _oSales.SalesMasterListTOTAL = _oSales.Get_SalesMasterInvoiceList_TTL_DAL_TRD(SaleRef, StartDate, EndDate, Customer_Name, ItemId, MNumber, Status_ID, c_ID, OrderRef,OrderStatus).ToList();



            return View(_oSales);
        }

        //---Get Sales master invoice list by search filters
        [HttpGet]
        public IActionResult GetSalesMasterInvoiceListBySearchFilers_Onl_TRD(string SaleRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string ItemId, string MNumber, int c_ID, string OrderRef, string OrderStatus, int? page)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion
            c_ID = c_ID == 0 ? 1 : c_ID;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            ItemId = String.IsNullOrEmpty(ItemId) ? "" : ItemId;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;
            Status_ID = Status_ID == 0 ? 0 : Status_ID;
            MNumber = String.IsNullOrEmpty(MNumber) ? "" : MNumber;

            OrderRef = string.IsNullOrEmpty(OrderRef) ? "" : OrderRef;
            OrderStatus = string.IsNullOrEmpty(OrderStatus) ? "" : OrderStatus;
         


            ViewBag.SaleRef_SaleInvoice = SaleRef;
            ViewBag.StartDate_SaleInvoice = StartDate;
            ViewBag.EndDate_SaleInvoice = EndDate;
            ViewBag.Customer_Name_SaleInvoice = Customer_Name;
            ViewBag.ItemId_SaleInvoice = ItemId;
            ViewBag.MNumber_SaleInvoice = MNumber;
            ViewBag.Status_ID_SaleInvoice = Status_ID;
            ViewBag.c_ID_SaleInvoice = c_ID;
            ViewBag.OrderRef_SaleInvoice = OrderRef;

            ViewBag.OrderStatus_SaleInvoice = OrderStatus;

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;
            try
            {
                //---purchase sales master invoice list for page
                ViewBag.RecordsPerPage = 10;
                _oSales.SalesMasterIPagedList = _oSales.Get_SalesMasterInvoiceList_DAL_TRD(SaleRef, StartDate, EndDate, Customer_Name, ItemId, MNumber, Status_ID, c_ID, OrderRef, OrderStatus).ToPagedList(page ?? 1, 10);
                _oSales.SalesMasterListTOTAL = _oSales.Get_SalesMasterInvoiceList_TTL_DAL_TRD(SaleRef, StartDate, EndDate, Customer_Name, ItemId, MNumber, Status_ID, c_ID, OrderRef,OrderStatus).ToList();


                return PartialView("_SalesMasterInvoiceListSearch_Onl_TRD", _oSales);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }


        //---Insert  sales invoice master
        [HttpPost]
        public IActionResult InsertSalesInvoiceMaster_Onl_TRD(int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string CustomerTRN,
            string Manualbook_ref, string Sale_trans_type, string Remarks, string Customer_Name, string OtherCost,string Tip, string Commision, string Wallet,string ShippingCharges)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = SalesInvoiceMaster_TempID;
            string Saletype = "SI";  //---It is the ref in table for new Payment master. Send this value hard coded from here

            if (Customer_ID == null || String.IsNullOrEmpty(SaleDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewSalesInvoice__Onl_TRD", "Sales");
            }

            Customer_ID = Customer_ID == null ? 0 : Customer_ID;


            try
            {
                message = _oSales.InsertSalesInvoiceMaster_DAL_TRD(Customer_ID, Customer_Contact, Invoice_address, Saletype, SaleDate, CustomerTRN, Manualbook_ref,
                     Sale_trans_type, Temp_ID, c_ID, Created_by, Remarks, Customer_Name, OtherCost,Tip,Commision,Wallet, ShippingCharges);

                if (message.Contains("Successfully"))
                {
                    string SaleMaster_ID = _oSales.SalesID_DAL_TRD();
                    //TempData["Success"] = "Saved Successfully!";
                    DumpStaticFieldsSales();
                    return RedirectToAction("NewSalesInvoice__Onl_TRD", new { SaleMaster_ID = SaleMaster_ID });
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("NewSalesInvoice__Onl_TRD", "Sales");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
                return RedirectToAction("NewSalesInvoice__Onl_TRD", "Sales");
            }


        }

        //---Insert  sales invoice master
        [HttpPost]
        public IActionResult UpdateSalesInvoiceMaster_Onl_TRD(int? SaleMaster_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate,
            string CustomerTRN, string Manualbook_ref,  string Sale_trans_type, string Remarks, string Customer_Name, string OtherCost, string Tip, string Commision, string Wallet,string ShippingCharges)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = SalesInvoiceMaster_TempID;
            string OldTempID = _oSales.GetOldTempIDFromSalesDetail_DAL(SaleMaster_ID);
            if (!String.IsNullOrEmpty(OldTempID))
            {
                Temp_ID = OldTempID;
            }
            else
            {
                if (String.IsNullOrEmpty(Temp_ID))
                {
                    Guid obj = Guid.NewGuid();
                    Temp_ID = obj.ToString();
                }
            }
            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();


            if (Customer_ID == null || String.IsNullOrEmpty(SaleDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            Customer_ID = Customer_ID == null ? 0 : Customer_ID;


            try
            {
                message = _oSales.UpdateSalesInvoiceMaster_DAL_TRD(SaleMaster_ID, Customer_ID, Customer_Contact, Invoice_address, SaleDate, CustomerTRN, Manualbook_ref,  Sale_trans_type, Temp_ID, c_ID, Modified_by, Remarks, Customer_Name, OtherCost, Tip, Commision, Wallet, ShippingCharges);

                if (message.Contains("Successfully"))
                {
                    DumpStaticFieldsSales();
                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("NewSalesInvoice__Onl_TRD", new { SaleMaster_ID = SaleMaster_ID });
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }


        }


        #endregion
        //By Yaseen 2-14-2022

    }
}
