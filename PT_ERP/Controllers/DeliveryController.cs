//using AspNetCore.Reporting;
//using AspNetCore.Reporting.ReportExecutionService;
using DAL.oClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Linq;
using X.PagedList;

namespace PT_ERP.Controllers
{
    public class DeliveryController : BaseController
    {





        private readonly IOBasicData _oBasic;
        private readonly IODelivery _oDelivery;
        private readonly IOAccounts _oAccounts;

        private IConfiguration configuration;

        public DeliveryController(IOBasicData oBasicBase, IOAccounts oAccounts, IConfiguration iConfig, IODelivery oDelivery)
           : base(oBasicBase)
        {
            _oBasic = oBasicBase;
            _oDelivery = oDelivery;
            _oAccounts = oAccounts;
            configuration = iConfig;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult NewPapers()
        {


            #region ViewBag Area
            //---Ven Category list for drop down in forms

            #endregion
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Paper";
            ViewBag.BreadCumAction = "PaperList";
            ViewBag.PageTitle = "New Paper";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            //---Partners list for page



            return View();
        }
        [HttpGet]
        public IActionResult PapersList()
        {


            #region ViewBag Area
            //---Ven Category list for drop down in forms

            #endregion
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Paper";
            ViewBag.BreadCumAction = "PaperList";
            ViewBag.PageTitle = "Paper List";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            //---Partners list for page



            return View();
        }

        [HttpGet]
        public IActionResult NewDelivery(int? DeliveryMaster_ID)
        {


            #region ViewBag Area
            //---Ven Category list for drop down in forms

            #endregion
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Paper";
            ViewBag.BreadCumAction = "PaperList";

            ViewBag.SectionHeaderTitle = "Stock";
            ViewBag.SectionSub_HeaderTitle = "New Delivery Order";
            ViewBag.PageTitle = "New Delivery";

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

            //---Partners list for page


            if (DeliveryMaster_ID == null || DeliveryMaster_ID < 1)
            {
                string DeliveryTempID = null;

                //---Create Purchase master Ref(tem id)
                if (String.IsNullOrEmpty(DeliveryTempID))
                {
                    Guid obj = Guid.NewGuid();
                    HttpContext.Session.SetString("DeliveryTempID", obj.ToString());

                }
                DeliveryTempID = HttpContext.Session.GetString("DeliveryTempID");


                //_oDelivery.EmpMasterList = _oDelivery.Get_vehicle_display_master_DAL_Id(-1);
                _oDelivery.DeliveryOrderDetailList = _oDelivery.Get_DeliveryOrder_DetailListByID_DAL(0, DeliveryTempID, c_ID);


            }
            else
            {

                //---Get old temp id from purchase detail table that already creaded for this "PurchaseMaster_ID" id. And then use this if new record added


                HttpContext.Session.SetString("DeliveryMasterStatic_ID", DeliveryMaster_ID.ToString());


                //---Get purchase master id
                ViewBag.DeliveryMaster_ID = DeliveryMaster_ID;




                //--Get purchase Master


                _oDelivery.DeliveryOrderMasterObj = _oDelivery.Get_DeliveryOrderMaster_Id(DeliveryMaster_ID);
                _oDelivery.DeliveryOrderDetailList = _oDelivery.Get_DeliveryOrder_DetailListByID_DAL(DeliveryMaster_ID, "", c_ID).ToList();
            }


            return View(_oDelivery);
        }

        [HttpPost]
        public IActionResult AddDeliveryMaster(string CarTakenDate, string CarTakenPerson, string CarTakenContact, string CarTaken)
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

            if (String.IsNullOrEmpty(CarTakenPerson))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewDelivery", "Delivery");
            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            string DeliveryTempID = HttpContext.Session.GetString("DeliveryTempID");
            string temp_ID = DeliveryTempID;



            //---saving CustomersMaster in database
            try
            {
                message = _oDelivery.Insert_DeliveryOrderMaster_DAL(CarTakenDate, CarTakenPerson, CarTakenContact, CarTaken, temp_ID, Created_by, c_ID);
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("NewDeliveryList", "Delivery");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("NewDelivery", "Delivery");
            }

            //return View();
        }


        [HttpPost]
        public IActionResult Update_DeliveryOrderMaster(int? DeliveryMaster_ID, string CarTakenDate, string CarTakenPerson, string CarTakenContact, string CarTaken)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(CarTakenPerson) || DeliveryMaster_ID == null || DeliveryMaster_ID < 1)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewDelivery", "Delivery");
            }


            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            string DeliveryOrder_Date = CarTakenDate;


            //---saving CustomersMaster in database
            try
            {
                message = _oDelivery.Update_DeliveryOrderMaster_DAL(DeliveryMaster_ID, CarTakenDate, CarTakenPerson, CarTakenContact, CarTaken, DeliveryOrder_Date, Modified_by);
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("NewDeliveryList", "Delivery");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not updated successfully!";
                return RedirectToAction("NewDelivery", "Delivery");
            }

            //return View();
        }

        [HttpPost]
        public IActionResult AddDeliveryDetail(string Chassis_No, int? DeliveryMaster_ID)
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

            if (String.IsNullOrEmpty(Chassis_No))
            {
                TempData["Error"] = "Please fill all required fields!";

            }


            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            string temp_ID = "";
            string DeliveryTempID = HttpContext.Session.GetString("DeliveryTempID");
            temp_ID = DeliveryTempID;
            if (string.IsNullOrEmpty(temp_ID))
            { temp_ID = ""; }


            string DeliveryMasterStatic_ID = HttpContext.Session.GetString("DeliveryMasterStatic_ID");


            if (!string.IsNullOrEmpty(DeliveryMasterStatic_ID))
            {
                DeliveryMaster_ID = Convert.ToInt32(DeliveryMasterStatic_ID);
            }
            else
            {
                DeliveryMaster_ID = 0;
            }


            //---saving CustomersDetail in database
            try
            {
                message = _oDelivery.Insert_DeliveryOrderDetail_DAL(Chassis_No, DeliveryMaster_ID, temp_ID, Created_by, c_ID);
                if (message == "Added Successfully!")
                {

                    _oDelivery.DeliveryOrderDetailList = _oDelivery.Get_DeliveryOrder_DetailListByID_DAL(DeliveryMaster_ID, temp_ID, c_ID).ToList();

                    return PartialView("_PartialDeliveryDetail", _oDelivery);
                }
                else if (message == "Chassis Not Sold or Not Exists!")
                {
                    return Json(new { message = message });

                }
                else
                {

                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = message });
            }

            //return View();
        }

        [HttpGet]
        public IActionResult NewDeliveryList(string DeliveryRef, string StartDate, string EndDate, string Chassis, int? Customer_ID, int? page)
        {


            HttpContext.Session.Remove("DeliveryTempID");
            HttpContext.Session.Remove("DeliveryMasterStatic_ID");
            #region ViewBag Area
            //---Ven Category list for drop down in forms

            #endregion
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Delivery";
            ViewBag.BreadCumAction = "DeliveryList";

            ViewBag.SectionHeaderTitle = "Stock";
            ViewBag.SectionSub_HeaderTitle = "New Delivery List";
            ViewBag.PageTitle = "New Delivery List";

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
            DeliveryRef = String.IsNullOrEmpty(DeliveryRef) ? "" : DeliveryRef;
            Chassis = String.IsNullOrEmpty(Chassis) ? "" : Chassis;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Customer_ID = Customer_ID == 0 || Customer_ID == null ? 0 : Customer_ID;
            ViewBag.DeliveryRef_Delivery = DeliveryRef;
            ViewBag.StartDate_Delivery = StartDate;
            ViewBag.EndDate_Delivery = EndDate;
            ViewBag.Chassis_Delivery = Chassis;
            ViewBag.Customer_ID_Delivery = Customer_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            ViewBag.AllCustomers = _oBasic.Get_CustomersMaster_DAL(c_ID).ToList();
            ViewBag.RecordsPerPage = 10;
            _oDelivery.DeliveryOrderMasterList = _oDelivery.Get_DeliveryOrder_master_DAL(DeliveryRef, StartDate, EndDate, Chassis, Customer_ID, c_ID).ToList().ToPagedList(page ?? 1, 10);

            return View(_oDelivery);


        }


        [HttpGet]
        public IActionResult NewDeliveryList_Search(string DeliveryRef, string StartDate, string EndDate, string Chassis, int? Customer_ID, int? page)
        {


            HttpContext.Session.Remove("DeliveryTempID");
            HttpContext.Session.Remove("DeliveryMasterStatic_ID");
            #region ViewBag Area
            //---Ven Category list for drop down in forms

            #endregion
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Delivery";
            ViewBag.BreadCumAction = "DeliveryList";
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
            DeliveryRef = String.IsNullOrEmpty(DeliveryRef) ? "" : DeliveryRef;
            Chassis = String.IsNullOrEmpty(Chassis) ? "" : Chassis;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Customer_ID = Customer_ID == 0 || Customer_ID == null ? 0 : Customer_ID;
            ViewBag.DeliveryRef_Delivery = DeliveryRef;
            ViewBag.StartDate_Delivery = StartDate;
            ViewBag.EndDate_Delivery = EndDate;
            ViewBag.Chassis_Delivery = Chassis;
            ViewBag.Customer_ID_Delivery = Customer_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            try
            {
                ViewBag.AllCustomers = _oBasic.Get_CustomersMaster_DAL(c_ID).ToList();
                ViewBag.RecordsPerPage = 10;
                _oDelivery.DeliveryOrderMasterList = _oDelivery.Get_DeliveryOrder_master_DAL(DeliveryRef, StartDate, EndDate, Chassis, Customer_ID, c_ID).ToList().ToPagedList(page ?? 1, 10);
                return PartialView("_DeliverySearchList", _oDelivery);
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }
        }




        [HttpPost]
        public IActionResult DeleteDeliveryDetail(int? DeliveryDetails_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            string temp_ID = "";
            string DeliveryTempID = HttpContext.Session.GetString("DeliveryTempID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            temp_ID = DeliveryTempID;
            if (string.IsNullOrEmpty(temp_ID))
            { temp_ID = ""; }
            int? DeliveryMasterStatic_ID = Convert.ToInt32(HttpContext.Session.GetString("DeliveryMasterStatic_ID"));
            DeliveryMasterStatic_ID = DeliveryMasterStatic_ID == null ? -1 : DeliveryMasterStatic_ID;

            if (DeliveryDetails_ID == null || DeliveryDetails_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("NewDelivery", "Delivery");
            }
            string message = "";



            try
            {

                message = _oDelivery.Delete_Delivery_details_DAL(DeliveryDetails_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = "Deleted Successfully!";
                    _oDelivery.DeliveryOrderDetailList = _oDelivery.Get_DeliveryOrder_DetailListByID_DAL(DeliveryMasterStatic_ID, temp_ID, c_ID).ToList();

                    return PartialView("_PartialDeliveryDetail", _oDelivery);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return Json(new { message = errorMessage });

            }


        }
        [HttpPost]
        public IActionResult DeleteDeliveryMaster(int? DeliveryMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (DeliveryMaster_ID == null || DeliveryMaster_ID < 1)
            {
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("NewDeliveryList", "Delivery");
            }
            string message = "";



            try
            {
                message = _oDelivery.Delete_Delete_master_DAL(DeliveryMaster_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("NewDeliveryList", "Delivery");
                }
                else
                {
                    TempData["Error"] = "Some thing went wrong. Please try again!";
                    return RedirectToAction("NewDeliveryList", "Delivery");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = "Some thing went wrong. Please try again!";
                return RedirectToAction("NewDelivery", "Delivery");

            }


        }



        [HttpGet]
        public IActionResult DepositReturn(int DV_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Delivery";
            ViewBag.BreadCumAction = "DepositReturn";

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
            ViewBag.ExportDestinations = _oBasic.Get_Destinations_DAL(c_ID).ToList();
            ViewBag.PortOfExit = _oBasic.Get_PortOfExit_DAL(c_ID).ToList();
            //---Ven Category list for drop down in forms
            ViewBag.CustomerCategoryList = _oBasic.Get_CustomerCategory_DAL().ToList();

            //---Get Accounts List
            ViewBag.DR_AccountsList = _oBasic.Select_PV_OnAccounts_DAL(c_ID).ToList();
            //---Get pay via list
            ViewBag.CR_accountIDList = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();
            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            #endregion

            if (DV_ID == 0 || DV_ID < 1)
            {
                string DepositReceivedMaster_TempID = null;

                //---Create payment master Ref(tem id)
                if (String.IsNullOrEmpty(DepositReceivedMaster_TempID))
                {
                    Guid obj = Guid.NewGuid();

                    HttpContext.Session.SetString("DepositReceivedMaster_TempID", obj.ToString());
                }
                DepositReceivedMaster_TempID = HttpContext.Session.GetString("DepositReceivedMaster_TempID");
                HttpContext.Session.SetString("DepositReceivedMasterStatic_ID", DV_ID.ToString());


                //---Get payment master id
                ViewBag.DV_ID = DV_ID;


                //--Get deposit voucher master
                _oDelivery.DepositMasterObj = _oDelivery.Get_DepositVoucherMasterByID_DAL(-1);

                //--Get received chassis detail list
                _oDelivery.DepositsDetailList = _oDelivery.Get_DepositVoucherReceivedListByID_DAL(DepositReceivedMaster_TempID, -1).ToList();

                //--Get receipt
                _oDelivery.receiptDetailList = _oDelivery.Get_ReceiptDetailDepositVoucherlListByID_DAL(DepositReceivedMaster_TempID, -1).ToList();

                //---deposit return payment list
                _oDelivery.paymentDetailList = _oDelivery.Get_PaymentDetailListByID_DAL(DepositReceivedMaster_TempID, -1).ToList();
            }
            else
            {

                //---Get old temp id from deposit detail table that already creaded for this "DV_ID" id. And then use this if new record added
                string OldTempID = _oDelivery.GetOldTempIDFromDepositDetail_DAL(DV_ID);




                HttpContext.Session.SetString("DepositReceivedMasterStatic_ID", DV_ID.ToString());

                //---Get depos
                ViewBag.DV_ID = DV_ID;

                //--Get deposit voucher master
                _oDelivery.DepositMasterObj = _oDelivery.Get_DepositVoucherMasterByID_DAL(DV_ID);

                //--Get received chassis detail list
                _oDelivery.DepositsDetailList = _oDelivery.Get_DepositVoucherReceivedListByID_DAL("0", DV_ID).ToList();

                //--Get receipt
                _oDelivery.receiptDetailList = _oDelivery.Get_ReceiptDetailDepositVoucherlListByID_DAL("0", DV_ID).ToList();

                //---deposit return payment list
                _oDelivery.paymentDetailList = _oDelivery.Get_PaymentDetailListByID_DAL("0", DV_ID).ToList();
            }






            return View(_oDelivery);
        }






        #region Deposit Management
        [HttpGet]
        public IActionResult DepositRecieved(int? DV_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Delivery";
            ViewBag.BreadCumAction = "DepositRecieved";

            ViewBag.SectionHeaderTitle = "Deposit";
            ViewBag.SectionSub_HeaderTitle = "Deposit Recieve";
            ViewBag.SectionSub_HeaderMainTitle = "Deposit";
            ViewBag.PageTitle = "Deposit Recieved";


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
            ViewBag.ExportDestinations = _oBasic.Get_Destinations_DAL(c_ID).ToList();
            ViewBag.PortOfExit = _oBasic.Get_PortOfExit_DAL(c_ID).ToList();
            //---Ven Category list for drop down in forms
            ViewBag.CustomerCategoryList = _oBasic.Get_CustomerCategory_DAL().ToList();

            //---Get Accounts List
            ViewBag.DR_AccountsList = _oBasic.Select_PV_OnAccounts_DAL(c_ID).ToList();
            //---Get pay via list
            ViewBag.CR_accountIDList = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();
            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            #endregion

            if (DV_ID == null || DV_ID < 1)
            {
                string DepositReceivedMaster_TempID = null;
                if (String.IsNullOrEmpty(DepositReceivedMaster_TempID))
                {
                    Guid obj = Guid.NewGuid();

                    HttpContext.Session.SetString("DepositReceivedMaster_TempID", obj.ToString());
                }
                DepositReceivedMaster_TempID = HttpContext.Session.GetString("DepositReceivedMaster_TempID");
                HttpContext.Session.SetString("DepositReceivedMasterStatic_ID", DV_ID.ToString());


                //---Get payment master id
                ViewBag.DV_ID = DV_ID;


                //--Get deposit voucher master
                _oDelivery.DepositMasterObj = _oDelivery.Get_DepositVoucherMasterByID_DAL(-1);

                //--Get received chassis detail list
                _oDelivery.DepositsDetailList = _oDelivery.Get_DepositVoucherReceivedListByID_DAL(DepositReceivedMaster_TempID, -1).ToList();

                //--Get receipt
                _oDelivery.receiptDetailList = _oDelivery.Get_ReceiptDetailDepositVoucherlListByID_DAL(DepositReceivedMaster_TempID, -1).ToList();
            }
            else
            {

                //---Get old temp id from deposit detail table that already creaded for this "DV_ID" id. And then use this if new record added
                string OldTempID = _oDelivery.GetOldTempIDFromDepositDetail_DAL(DV_ID);




                HttpContext.Session.SetString("DepositReceivedMasterStatic_ID", DV_ID.ToString());

                //---Get depos
                ViewBag.DV_ID = DV_ID;

                //--Get deposit voucher master
                _oDelivery.DepositMasterObj = _oDelivery.Get_DepositVoucherMasterByID_DAL(DV_ID);

                //--Get received chassis detail list
                _oDelivery.DepositsDetailList = _oDelivery.Get_DepositVoucherReceivedListByID_DAL(OldTempID, DV_ID).ToList();

                //--Get receipt
                _oDelivery.receiptDetailList = _oDelivery.Get_ReceiptDetailDepositVoucherlListByID_DAL(OldTempID, DV_ID).ToList();
            }






            return View(_oDelivery);
        }


        [HttpGet]
        public IActionResult DepositList(string DV_Ref, string Party_ID_Name, string StartDate, string EndDate, string Chassis_no, string Cheque_no, int? c_ID, int? page)
        {
            //---Dumping static fields
            DumpStaticFieldsDelivery();

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Deposit";
            ViewBag.BreadCumAction = "DepositList";


            ViewBag.SectionHeaderTitle = "Sales";
            ViewBag.SectionSub_HeaderTitle = "Deposit List";

            ViewBag.PageTitle = "Deposit List";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            #region Veiwbags area
            //---Get Vendor Master
            ViewBag.CustomerName = _oBasic.Get_CustomersMaster_DAL(c_IDs).ToList();
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;


            #endregion veiwbags area ends here

            DV_Ref = String.IsNullOrEmpty(DV_Ref) ? "" : DV_Ref;
            Party_ID_Name = String.IsNullOrEmpty(Party_ID_Name) ? "" : Party_ID_Name;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_no = String.IsNullOrEmpty(Chassis_no) ? "" : Chassis_no;

            c_ID = c_ID == null || c_ID == 0 ? c_IDs : c_ID;
            Cheque_no = String.IsNullOrEmpty(Cheque_no) ? "" : Cheque_no;
            ViewBag.DV_Ref_Deposit = DV_Ref;
            ViewBag.Party_ID_Name_Deposit = Party_ID_Name;
            ViewBag.StartDate_Deposit = StartDate;
            ViewBag.EndDate_Deposit = EndDate;
            ViewBag.c_ID_Deposit = c_ID;
            ViewBag.ChassisNo_Deposit = Chassis_no;
            ViewBag.Cheque_no_Deposit = Cheque_no;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            //---delivery master list for page
            ViewBag.RecordsPerPage = 10;
            _oDelivery.DepositsDetailListIPageList = _oDelivery.Get_DepositMaster_List_DAL(DV_Ref, Party_ID_Name, StartDate, EndDate, Chassis_no, Cheque_no, c_ID).ToList().ToPagedList(page ?? 1, 10);


            return View(_oDelivery);
        }


        [HttpGet]
        public IActionResult DepositListSearch(string DV_Ref, string Party_ID_Name, string StartDate, string EndDate, string Chassis_no, string Cheque_no, int c_ID, int? page)
        {
            //---Dumping static fields
            DumpStaticFieldsDelivery();

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Delivery";
            ViewBag.BreadCumAction = "DepositList";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            #region Veiwbags area
            //---Get Vendor Master
            ViewBag.CustomerName = _oBasic.Get_CustomersMaster_DAL(c_IDs).ToList();
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;


            #endregion veiwbags area ends here

            DV_Ref = String.IsNullOrEmpty(DV_Ref) ? "" : DV_Ref;
            Party_ID_Name = String.IsNullOrEmpty(Party_ID_Name) ? "" : Party_ID_Name;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_no = String.IsNullOrEmpty(Chassis_no) ? "" : Chassis_no;

            c_ID = c_ID == 0 ? c_IDs : c_ID;
            Cheque_no = String.IsNullOrEmpty(Cheque_no) ? "" : Cheque_no;
            ViewBag.DV_Ref_Deposit = DV_Ref;
            ViewBag.Party_ID_Name_Deposit = Party_ID_Name;
            ViewBag.StartDate_Deposit = StartDate;
            ViewBag.EndDate_Deposit = EndDate;
            ViewBag.c_ID_Deposit = c_ID;
            ViewBag.ChassisNo_Deposit = Chassis_no;
            ViewBag.Cheque_no_Deposit = Cheque_no;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;




            try
            {
                //---delivery master list for page
                ViewBag.RecordsPerPage = 10;
                _oDelivery.DepositsDetailListIPageList = _oDelivery.Get_DepositMaster_List_DAL(DV_Ref, Party_ID_Name, StartDate, EndDate, Chassis_no, Cheque_no, c_ID).ToList().ToPagedList(page ?? 1, 10);


                return PartialView("_Search_DepositList_Partial", _oDelivery);

            }


            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }



        //---chassis validation function for deposit
        [HttpGet]
        public JsonResult ValidateChassisNoForDeposit(string ChassisNo)
        {
            string message = "";
            string stock_ID = "0";
            string customerName = "";
            string contactNumber = "";
            string exportTo_Destination_ID = "";
            string port_of_Exit_ID = "";
            string customer_ID = "";



            _oDelivery.ChassisValidationObj = _oDelivery.ValidateChassisNoForDeposit_DAL(ChassisNo);

            if (_oDelivery.ChassisValidationObj.Message == "Chassis Exists" && !String.IsNullOrEmpty(_oDelivery.ChassisValidationObj.Stock_ID))
            {
                message = "Chassis is valid";
                stock_ID = _oDelivery.ChassisValidationObj.Stock_ID;
                customerName = _oDelivery.ChassisValidationObj.CustomerName;
                contactNumber = _oDelivery.ChassisValidationObj.ContactNumber;
                exportTo_Destination_ID = _oDelivery.ChassisValidationObj.ExportTo_Destination_ID;
                port_of_Exit_ID = _oDelivery.ChassisValidationObj.Port_of_Exit_ID;
                customer_ID = _oDelivery.ChassisValidationObj.Customer_ID;
            }
            else
            {

                message = "Invalid Chassis";
                stock_ID = null;
                customerName = null;
                contactNumber = null;
                exportTo_Destination_ID = null;
                port_of_Exit_ID = null;
                customer_ID = null;
            }


            return Json(new { message, stock_ID, customerName, contactNumber, exportTo_Destination_ID, port_of_Exit_ID, customer_ID });

        }


        //---insert deposit voucher detail
        [HttpPost]
        public IActionResult InsertDepositVoucherDetail(string getChassisNo, int? Stock_ID, double? Car_Sale_Value, double? Deposit, double? Fix_Deductable_Charges, double? PaperExpense_Changes,
            double? VAT_Security_Deposit, double? Other_Charges, string External_Trans_Ref, DateTime? ShipDate, DateTime? Submission_Date, int hdDV_ID)
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

            if (Stock_ID == null)
            {
                return Json(new { message = "Stock ID is null!" });
            }
            if (String.IsNullOrEmpty(getChassisNo))
            {
                return Json(new { message = "Chassis is null!" });
            }



            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            string ShippingDate = "";
            string Submit_Date = "";
            string DepositReceivedMaster_TempID = HttpContext.Session.GetString("DepositReceivedMaster_TempID");
            Temp_ID = DepositReceivedMaster_TempID;
            if (hdDV_ID == 0)
            {
                Temp_ID = DepositReceivedMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }


            if (ShipDate != null)
            {
                ShippingDate = Convert.ToString(ShipDate);
            }
            else
            {
                ShippingDate = Convert.ToString(DateTime.Now);
            }
            if (Submission_Date != null)
            {
                Submit_Date = Convert.ToString(Submission_Date);
            }
            else
            {
                Submit_Date = Convert.ToString(DateTime.Now);
            }

            try
            {
                message = _oDelivery.InsertDepositVoucherDetail_DAL(Stock_ID, Car_Sale_Value, Deposit, Fix_Deductable_Charges, PaperExpense_Changes,
                 VAT_Security_Deposit, Other_Charges, External_Trans_Ref, ShippingDate, Submit_Date, Temp_ID, c_ID, Created_by, hdDV_ID);
                if (message == "Saved Successfully!")
                {
                    //deposit voucher received list
                    _oDelivery.DepositsDetailList = _oDelivery.Get_DepositVoucherReceivedListByID_DAL(Temp_ID, hdDV_ID).ToList();


                    return PartialView("_DepositReceivedPartial_1", _oDelivery);
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


        //---insert deposit voucher detail
        [HttpPost]
        public IActionResult UpdateDepositVoucherDetail(int? DVdetails_ID, string getChassisNo, int? Stock_ID, double? Car_Sale_Value, double? Deposit, double? Fix_Deductable_Charges, double? PaperExpense_Changes,
            double? VAT_Security_Deposit, double? Other_Charges, string External_Trans_Ref, DateTime? ShipDate, DateTime? Submission_Date, int hdDV_ID)
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

            if (Stock_ID == null)
            {
                return Json(new { message = "Stock ID is null!" });
            }
            if (String.IsNullOrEmpty(getChassisNo))
            {
                return Json(new { message = "Chassis is null!" });
            }



            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            string ShippingDate = "";
            string Submit_Date = "";

            string DepositReceivedMaster_TempID = HttpContext.Session.GetString("DepositReceivedMaster_TempID");
            Temp_ID = DepositReceivedMaster_TempID;
            if (hdDV_ID == 0)
            {
                Temp_ID = DepositReceivedMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }


            if (ShipDate != null)
            {
                ShippingDate = Convert.ToString(ShipDate);
            }
            else
            {
                ShippingDate = Convert.ToString(DateTime.Now);
            }
            if (Submission_Date != null)
            {
                Submit_Date = Convert.ToString(Submission_Date);
            }
            else
            {
                Submit_Date = Convert.ToString(DateTime.Now);
            }

            try
            {
                message = _oDelivery.UpdateDepositVoucherDetail_DAL(DVdetails_ID, Stock_ID, Car_Sale_Value, Deposit, Fix_Deductable_Charges, PaperExpense_Changes,
                 VAT_Security_Deposit, Other_Charges, External_Trans_Ref, ShippingDate, Submit_Date, Temp_ID, c_ID, Modified_by);
                if (message == "Saved Successfully!")
                {
                    //deposit voucher received list
                    _oDelivery.DepositsDetailList = _oDelivery.Get_DepositVoucherReceivedListByID_DAL(Temp_ID, hdDV_ID).ToList();


                    return PartialView("_DepositReceivedPartial_1", _oDelivery);
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





        //---Insert  deposit voucher master
        [HttpPost]
        public IActionResult Insert_DepositVoucherMaster(int? Customer_ID, string Customer_Name, string Customer_Contact, string refund_Customer, string refund_Contact, int? export_to_Destination_ID, int? port_ID, string Date_Return, string Date_Taken)
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
            string DepositReceivedMaster_TempID = HttpContext.Session.GetString("DepositReceivedMaster_TempID");
            Temp_ID = DepositReceivedMaster_TempID;

            if (Customer_ID == null)
            {
                TempData["Error"] = "Customer ID is null!";
                return RedirectToAction("DepositRecieved", "Delivery");
            }

            if (String.IsNullOrEmpty(Customer_Name))
            {
                TempData["Error"] = "Customer Name is empty!";
                return RedirectToAction("DepositRecieved", "Delivery");
            }


            try
            {
                message = _oDelivery.InsertDepositVoucherMaster_DAL(Customer_ID, Customer_Name, Customer_Contact, refund_Customer, refund_Contact, export_to_Destination_ID, port_ID, Date_Return,
                    Date_Taken, Temp_ID, c_ID, Created_by);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("DepositList", "Delivery");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("DepositRecieved", "Delivery");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("DepositRecieved", "Delivery");
            }


        }


        //---udpate  receipt master general
        [HttpPost]
        public IActionResult Update_DepositVoucherMaster(int? DV_ID, int? Customer_ID, string Customer_Name, string Customer_Contact, string refund_Customer, string refund_Contact, int? export_to_Destination_ID,
            int? port_ID, string Date_Return, string Date_Taken)
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
            string DepositReceivedMaster_TempID = HttpContext.Session.GetString("DepositReceivedMaster_TempID");
            Temp_ID = DepositReceivedMaster_TempID;
            string OldTempID = _oDelivery.GetOldTempIDFromDepositDetail_DAL(DV_ID);
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


            if (Customer_ID == null || String.IsNullOrEmpty(Customer_Name))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            if (DV_ID == null)
            {
                TempData["Error"] = "Master ID is null. Please try again!";
                return Redirect(url);
            }




            try
            {
                message = _oDelivery.Update_DepositVoucherMaster_DAL(DV_ID, Customer_ID, Customer_Name, Customer_Contact, refund_Customer, refund_Contact, export_to_Destination_ID, port_ID, Date_Return,
                   Date_Taken, Temp_ID, c_ID, Modified_by);
                if (message.Contains("Successfully!"))
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("DepositList", "Delivery");
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



        //---Insert  depsosit master general
        [HttpPost]
        public IActionResult Insert_DepositReturnMaster(int? DV_ID, int Customer_ID, string refund_Customer, string refund_Contact, string Date_Return)
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
            string DepositReceivedMaster_TempID = HttpContext.Session.GetString("DepositReceivedMaster_TempID");
            Temp_ID = DepositReceivedMaster_TempID;
            if (Temp_ID == null)
            {
                Temp_ID = "0";
            }
            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();


            if (String.IsNullOrEmpty(refund_Customer) || String.IsNullOrEmpty(refund_Contact) || string.IsNullOrEmpty(Date_Return))
            {
                TempData["Error"] = "Refund Customer Name/Contact or Refund Date is missing !";
                return Redirect(url);
            }

            if (DV_ID == null)
            {
                TempData["Error"] = "Master ID is null. Please try again!";
                return Redirect(url);
            }




            try
            {
                message = _oDelivery.Insert_DepositReturnMaster_DAL(DV_ID, Customer_ID, refund_Customer, refund_Contact, Date_Return,
                   Temp_ID, c_ID, Modified_by);
                if (message.Contains("Successfully!"))
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("DepositList", "Delivery");
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



        //---insert into receipt detail general
        [HttpPost]
        public IActionResult InsertReceiptDetail_Deposit(int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
            double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date, string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, int hdDV_ID)
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
            string DepositReceivedMaster_TempID = HttpContext.Session.GetString("DepositReceivedMaster_TempID");
            Temp_ID = DepositReceivedMaster_TempID;
            if (hdDV_ID == 0)
            {
                Temp_ID = DepositReceivedMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }


            int ReceiptMaster_ID = 0;

            if (Amount == null || CR_accountID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            try
            {
                message = _oAccounts.InsertReceiptDetailDeposit_General_DAL(DR_accountID, Amount, other_curr_amount, Currency_ID, Currency_Rate, CR_accountID,
                    Description, trans_ref, Cheque_Bank_Name, cheque_Date, Cheque_no, VAT_Rate, VAT_Exp, Total_Amount, Temp_ID, c_ID, Created_by, hdDV_ID, ReceiptMaster_ID);
                if (message == "Saved Successfully!")
                {

                    _oDelivery.receiptDetailList = _oDelivery.Get_ReceiptDetailDepositVoucherlListByID_DAL(Temp_ID, hdDV_ID).ToList();

                    return PartialView("_DepositReceived_ReceiptGen", _oDelivery);
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


        //---Update into receipt detail general
        [HttpPost]
        public IActionResult UpdateReceiptDetail_Deposit(int? ReceiptDetail_ID, int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
            double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date, string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, int hdDV_ID, int PDC_Recieving_Acc_ID)
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

            string DepositReceivedMaster_TempID = HttpContext.Session.GetString("DepositReceivedMaster_TempID");
            Temp_ID = DepositReceivedMaster_TempID;
            if (hdDV_ID == 0)
            {
                Temp_ID = DepositReceivedMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }



            if (ReceiptDetail_ID == null)
            {
                return Json(new { message = "Receipt detail id is null. Please try again!" });
            }

            if (Amount == null || CR_accountID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            try
            {
                message = _oAccounts.UpdateReceiptDetail_General_DAL(ReceiptDetail_ID, DR_accountID, Amount, other_curr_amount, Currency_ID, Currency_Rate, CR_accountID,
                    Description, trans_ref, Cheque_Bank_Name, cheque_Date, Cheque_no, VAT_Rate, VAT_Exp, Total_Amount, Temp_ID, c_ID, Modified_by, PDC_Recieving_Acc_ID);
                if (message == "Updated Successfully!")
                {

                    _oDelivery.receiptDetailList = _oDelivery.Get_ReceiptDetailDepositVoucherlListByID_DAL(Temp_ID, hdDV_ID).ToList();

                    return PartialView("_DepositReceived_ReceiptGen", _oDelivery);
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




        //---insert into payment detial general
        [HttpPost]
        public IActionResult InsertDepositReturnPaymentDetail(int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
            string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount, int hdDV_ID)
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
            string DepositReceivedMaster_TempID = HttpContext.Session.GetString("DepositReceivedMaster_TempID");
            Temp_ID = DepositReceivedMaster_TempID;
            if (hdDV_ID == 0)
            {
                Temp_ID = DepositReceivedMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }



            if (DR_accountID == null || Amount == null || CR_accountID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            try
            {
                message = _oDelivery.InsertDepositReturnPaymentDetail_DAL(DR_accountID, Amount, VAT_Rate, Currency_ID, Currency_Rate, Amount_Others, CR_accountID, Description,
             trans_ref, cheque_bank_name, cheque_date, cheque_no, VAT_Exp, Total_Amount, Temp_ID, c_ID, Created_by, hdDV_ID);
                if (message == "Saved Successfully!")
                {
                    _oDelivery.paymentDetailList = _oDelivery.Get_PaymentDetailListByID_DAL(Temp_ID, hdDV_ID).ToList();

                    return PartialView("_DepositRetunPaymentPartial", _oDelivery);
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
        public IActionResult UpdateDepositReturnPaymentDetail(int? PaymentDetails_ID, int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
            string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount, int hdDV_ID)
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
            string DepositReceivedMaster_TempID = HttpContext.Session.GetString("DepositReceivedMaster_TempID");

            Temp_ID = DepositReceivedMaster_TempID;
            if (hdDV_ID == 0)
            {
                Temp_ID = DepositReceivedMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }



            if (PaymentDetails_ID == null || DR_accountID == null || Amount == null || CR_accountID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            try
            {
                message = _oDelivery.UpdateDepositReturnPaymentDetail_DAL(PaymentDetails_ID, DR_accountID, Amount, VAT_Rate, Currency_ID, Currency_Rate, Amount_Others, CR_accountID, Description,
             trans_ref, cheque_bank_name, cheque_date, cheque_no, VAT_Exp, Total_Amount, Temp_ID, c_ID, Modified_by);
                if (message == "Updated Successfully!")
                {

                    _oDelivery.paymentDetailList = _oDelivery.Get_PaymentDetailListByID_DAL(Temp_ID, hdDV_ID).ToList();

                    return PartialView("_DepositRetunPaymentPartial", _oDelivery);

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
        public IActionResult DeleteDepositReturnPaymentDetail(int? PaymentDetails_ID)
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
            string DepositReceivedMaster_TempID = HttpContext.Session.GetString("DepositReceivedMaster_TempID");
            Temp_ID = DepositReceivedMaster_TempID;
            int? DepositReceivedMasterStatic_ID = Convert.ToInt32(HttpContext.Session.GetString("DepositReceivedMasterStatic_ID"));
            string OldTempID = _oDelivery.GetOldTempIDFromDepositDetail_DAL(DepositReceivedMasterStatic_ID);
            if (!String.IsNullOrEmpty(DepositReceivedMaster_TempID))
            {
                Temp_ID = DepositReceivedMaster_TempID;
            }
            else
            {
                Temp_ID = OldTempID;
            }
            DepositReceivedMasterStatic_ID = DepositReceivedMasterStatic_ID == null ? -1 : DepositReceivedMasterStatic_ID;


            if (PaymentDetails_ID == null)
            {
                return Json(new { message = "ID is null. Please try again!" });
            }

            try
            {
                message = _oAccounts.DeletePaymentDetail_DAL(PaymentDetails_ID);
                if (message == "Deleted Successfully!")
                {
                    _oDelivery.paymentDetailList = _oDelivery.Get_PaymentDetailListByID_DAL(Temp_ID, DepositReceivedMasterStatic_ID).ToList();

                    return PartialView("_DepositRetunPaymentPartial", _oDelivery);
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




        //---Method for deleting receipt  detail gen
        [HttpPost]
        public IActionResult DeleteDepositVoucherDetail(int? DVdetails_ID)
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

            if (DVdetails_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }



            try
            {
                message = _oDelivery.DeleteDepositVoucherDetail_DAL(DVdetails_ID);
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

        //---Method for deleting receipt  detail gen
        [HttpPost]
        public IActionResult DeleteReceiptDetailDepositVoucher(int? ReceiptDetail_ID)
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
                message = _oAccounts.DeleteReceiptDetail_DAL(ReceiptDetail_ID);
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

        //----dumping static ids, and temp fields
        public void DumpStaticFieldsDelivery()
        {

            HttpContext.Session.Remove("DepositReceivedMaster_TempID");
            HttpContext.Session.Remove("DepositReceivedMasterStatic_ID");


        }

        //-----------
        //---Get sales invoice attachment list 
        [HttpGet]
        public IActionResult GetDepositInvoice_Attachments(int? DV_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //--send this value hard coded from here for getting attachment for  master invoice
            string Type = "Deposit_Voucher";

            try
            {
                _oDelivery.attachmentList = _oDelivery.GetDVMaster_Attachments_DAL(DV_ID, Type).ToList();

                return PartialView("_DepositAttachment", _oDelivery);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }
        }
        [HttpGet]
        public IActionResult GetDelievery_Attachments(int? DeliveryMaster_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            //--send this value hard coded from here for getting attachment for  master invoice
            string Type = "Delivery_Note";

            try
            {
                _oDelivery.attachmentList = _oDelivery.GetDVMaster_Attachments_DAL(DeliveryMaster_ID, Type).ToList();

                return PartialView("_DeliveryAttachment", _oDelivery);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }
        }



        [HttpPost]
        public IActionResult InsertAttachments_DepositMaster(int? DV_ID, string Type, string Document_Type, IFormFile Attachment)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            if (DV_ID == null || String.IsNullOrEmpty(Document_Type) || Attachment == null)
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

                message = _oDelivery.Insert_Attachment_DV_DAL(DV_ID, Document_Type, Type, newFileName, filepath, Created_by);
                if (message == "Saved Successfully!")
                {

                    _oDelivery.attachmentList = _oDelivery.GetDVMaster_Attachments_DAL(DV_ID, Type).ToList();

                    return PartialView("_DepositAttachment", _oDelivery);
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
        public IActionResult InsertAttachments_DeliveryMaster(int? DeliveryMaster_ID, string Type, string Document_Type, IFormFile Attachment)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            if (DeliveryMaster_ID == null || String.IsNullOrEmpty(Document_Type) || Attachment == null)
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

                message = _oDelivery.Insert_Attachment_DV_DAL(DeliveryMaster_ID, Document_Type, Type, newFileName, filepath, Created_by);
                if (message == "Saved Successfully!")
                {

                    _oDelivery.attachmentList = _oDelivery.GetDVMaster_Attachments_DAL(DeliveryMaster_ID, Type).ToList();

                    return PartialView("_DeliveryAttachment", _oDelivery);
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
        public IActionResult DeleteAttachmentDeposit(int? DV_ID, int? Attachment_ID, string Type, string File_Name)
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

            if (DV_ID == null)
            {
                //TempData["Error"] = "Please fill all required fields!";
                return Json(new { message = "DV id is null" });
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

                message = _oDelivery.Delete_Attachment(Attachment_ID);
                if (message == "Deleted Successfully!")
                {

                    _oDelivery.attachmentList = _oDelivery.GetDVMaster_Attachments_DAL(DV_ID, Type).ToList();

                    return PartialView("_DepositAttachment", _oDelivery);
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
        public IActionResult DeleteAttachmentDelivery(int? DeliveryMaster_ID, int? Attachment_ID, string Type, string File_Name)
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

            if (DeliveryMaster_ID == null)
            {
                //TempData["Error"] = "Please fill all required fields!";
                return Json(new { message = "Delivery id is null" });
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

                message = _oDelivery.Delete_Attachment(Attachment_ID);
                if (message == "Deleted Successfully!")
                {

                    _oDelivery.attachmentList = _oDelivery.GetDVMaster_Attachments_DAL(DeliveryMaster_ID, Type).ToList();

                    return PartialView("_DeliveryAttachment", _oDelivery);
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


    }
}
