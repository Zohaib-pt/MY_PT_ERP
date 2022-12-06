using DAL.Models;
using DAL.oClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using X.PagedList;

namespace PT_ERP.Controllers
{
    public class PurchasesController : BaseController
    {
        #region
        //---Static Fields for creating new purchase
        static string PurchaseNewTempID;
        static int? PurchaseMasterNewStatic_ID;

        //---Static Fields for creating new Other purchase
        static string PurchaseOtherTempID;
        static int? PurchaseMasterOtherStatic_ID;


        static string GRVOtherTempID;
        static int? GRVMasterOtherStatic_ID;

        //---Static Fields for creating other purchases
        static string PurchaseReturnTempID;
        static int? Purchase_Return_Static_ID;

        //---Static temp_ID Fields for bulk upload
        static string BulkUpload_TempID;

        static string ApprovalURL;
        static string OtherURL;
        static string Urls;
        #endregion


        private readonly IOBasicData _oBasic;
        private readonly IOPurchases _oPurchase;
        private IConfiguration configuration;

        public PurchasesController(IOBasicData oBasicBase, IOPurchases oPurchase, IConfiguration iConfig)
           : base(oBasicBase)
        {
            _oBasic = oBasicBase;
            _oPurchase = oPurchase;
            configuration = iConfig;
        }

        //---Getting purchase list
        [HttpGet]

        public IActionResult PurchaseList(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, string ChassisNo, int? Status_ID,int? c_ID, int? page)
        {
            //---Dumping static fields
            DumpStaticFieldsPurchase();



            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Purchases";
            ViewBag.BreadCumAction = "PurchaseList";

            ViewBag.SectionHeaderTitle = "Purchases";
            ViewBag.SectionSub_HeaderTitle = "Purchase List";
            ViewBag.PageTitle = "Purchase List";



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
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_IDs).ToList();
            //---Get PurchaseStatus
            ViewBag.PurchaseStatus = _oBasic.Get_StatusList_byType_DAL("Purchase").ToList();

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;


            #endregion veiwbags area


            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;

            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;
            From_Date = String.IsNullOrEmpty(From_Date) ? "" : From_Date;
            To_Date = String.IsNullOrEmpty(To_Date) ? "" : To_Date;
            Status_ID = Status_ID == null ? 0 : Status_ID;

            c_ID = c_ID == null || c_ID == 0 ? c_IDs : c_ID;

            
            ViewBag.PurchaseRef_PurchaseList = PurchaseRef;
            ViewBag.Vendor_ID_PurchaseList = Vendor_ID;
            ViewBag.From_Date_PurchaseList = From_Date;
            ViewBag.To_Date_PurchaseList = To_Date;
            ViewBag.Status_ID_PurchaseList = Status_ID;
            ViewBag.c_ID_PurchaseList = c_ID;
            ViewBag.ChassisNo_PurchaseList = ChassisNo;

            //---purchase master list for page
            ViewBag.RecordsPerPage = 10;
            _oPurchase.purchaseMasterIPagedList = _oPurchase.Get_PurchaseMasterList_DAL(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, ChassisNo, c_ID).ToList().ToPagedList(page ?? 1, 10);

            _oPurchase.purchaseMasterTotal = _oPurchase.Get_PurchaseMasterList_TTL(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, "", c_ID).ToList();

            return View(_oPurchase);
        }





        //---This method is for inserting purchase detail
        [HttpPost]
        public IActionResult InsertPurchaseDetail(int? Make_ID, int? Model_ID, int? Color_ID, string mileage, int Make_category_ID, string EngineType, int StockType_ID,
            string Drive, string Used_New, string Fuel_Type, string BL_NO, string Selling_Price, string OtherCost, string OthersSpecs, int? Quantity, int? Currency_ID,
            double? Currency_Rate, string Unit_Price, string Amount, string Amount_Others, string Ref, int hdPurchaseMaster_ID, string Chassis, string Year,
            int? color_interior_ID, int? loc, int? c_ID, string PriceTax, string AuctionFee, string ReksoFee, string RecycleFee,
            string Vanning_Charges, string FreightCharges, string FreightRate, string Cont_NO, string AuctionName, string Other_Charges, string JP_Charges,string Door,string Hs_Code)
        {



            #region login checking
            string company_ID = HttpContext.Session.GetString("c_ID");
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion




            string message = "";
            string Temp_ID = "";
            int? Create_by = Convert.ToInt32(UserID_Admin);
            Temp_ID = PurchaseNewTempID;
            if (hdPurchaseMaster_ID == 0)
            {
                Temp_ID = PurchaseNewTempID;
            }
            else
            {
                Temp_ID = "0";
            }
            PurchaseMasterNewStatic_ID = PurchaseMasterNewStatic_ID == null ? 0 : PurchaseMasterNewStatic_ID;

            if (Quantity == null)
            {
                Quantity = 1;

            }


            if (Make_ID == null || Model_ID == null || Quantity == null ||
                Unit_Price == null || Amount == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            c_ID = Convert.ToInt32(company_ID);
            Color_ID = Color_ID == null ? 0 : Color_ID;
            OthersSpecs = String.IsNullOrEmpty(OthersSpecs) ? "" : OthersSpecs;
            Currency_ID = Currency_ID == null ? 0 : Currency_ID;
            Currency_Rate = Currency_Rate == null ? 0 : Currency_Rate;
            Amount_Others = Amount_Others == null ? "0" : Amount_Others;
            Amount = Amount == null ? "0" : Amount;


             
            Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;

            try
            {
                message = _oPurchase.InsertPurchaseDetail_DAL(Make_ID, Model_ID, Color_ID, mileage, Make_category_ID, EngineType, StockType_ID, Drive, Used_New, Fuel_Type, BL_NO, Selling_Price, OtherCost, OthersSpecs, Quantity, Currency_ID,
            Currency_Rate, Unit_Price, Amount, Amount_Others, Ref, Temp_ID, Create_by, hdPurchaseMaster_ID, Chassis, Year, color_interior_ID, loc, c_ID,
            PriceTax, AuctionFee, ReksoFee, RecycleFee, Vanning_Charges, FreightCharges, FreightRate, Cont_NO, AuctionName, Other_Charges, JP_Charges,Door,Hs_Code);
                if (message == "Saved Successfully!")
                {
                    _oPurchase.purchaseDetailList = _oPurchase.Get_PurchaseDetailListByID_DAL(Temp_ID, hdPurchaseMaster_ID).ToList();

                    return PartialView("_PurchaseDetailPartial", _oPurchase);
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


            //return new PartialViewResult
            //{
            //    ViewName = "_PurchaseDetailPartial",
            //    ViewData = new ViewDataDictionary
            //<List<Pa_PurchaseDetails_DAL>>(ViewData, _oPurchase.purchaseDetailList)
            //};
        }


        //---This method is for Udpating purchase detail
        [HttpPost]
        public IActionResult UpdatePurchaseDetail(int? PurchaseDetails_ID, int? Make_ID, int? Model_ID, int? Color_ID, string mileage, int Make_category_ID,
            string EngineType, int StockType_ID, string Drive, string Used_New, string Fuel_Type, string BL_NO, string Selling_Price, string OtherCost,
            string OthersSpecs, int? Quantity, int? Currency_ID,
            double? Currency_Rate, string Unit_Price, string Amount, string Amount_Others, string Ref, int hdPurchaseMaster_ID, string Chassis, string Year,
            int? color_interior_ID, int? loc, int? c_ID, int? StockID, string PriceTax, string AuctionFee, string ReksoFee, string RecycleFee,
            string Vanning_Charges, string FreightCharges, string FreightRate, string Cont_NO, string AuctionName, string Other_Charges, string JP_Charges, string Door, string Hs_Code)
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
            Temp_ID = PurchaseNewTempID;
            string OldTempID = _oPurchase.GetOldTempIDFromPurchaseDetail_DAL(PurchaseMasterNewStatic_ID);
            if (hdPurchaseMaster_ID == 0)
            {
                Temp_ID = PurchaseNewTempID;
            }
            else
            {
                Temp_ID = "0";
            }
            int? Modified_by = Convert.ToInt32(UserID_Admin);

            PurchaseMasterNewStatic_ID = PurchaseMasterNewStatic_ID == null ? 0 : PurchaseMasterNewStatic_ID;



            if (PurchaseDetails_ID == null)
            {
                return Json(new { message = "Stock ID is null. Please try again!" });
            }

            if (Quantity == null)
            {
                Quantity = 1;
            }


            if (Make_ID == null || Model_ID == null || Quantity == null
                 || Unit_Price == null || Amount == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            Color_ID = Color_ID == null ? 0 : Color_ID;
            OthersSpecs = String.IsNullOrEmpty(OthersSpecs) ? "" : OthersSpecs;
            Currency_ID = Currency_ID == null ? 0 : Currency_ID;
            Currency_Rate = Currency_Rate == null ? 0 : Currency_Rate;
            Amount_Others = Amount_Others == null ? "0" : Amount_Others;
            Amount = Amount == null ? "0" : Amount;
            Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;



            try
            {
                message = _oPurchase.UpdatePurchaseDetail_DAL(PurchaseDetails_ID, Make_ID, Model_ID, Color_ID, mileage, Make_category_ID, EngineType, StockType_ID, Drive, Used_New, Fuel_Type, BL_NO, Selling_Price, OtherCost, OthersSpecs, Quantity, Currency_ID,
            Currency_Rate, Unit_Price, Amount, Amount_Others, Ref, Modified_by, Chassis, Year, color_interior_ID, loc, c_ID, StockID, PriceTax, AuctionFee, ReksoFee, RecycleFee,
            Vanning_Charges, FreightCharges, FreightRate, Cont_NO, AuctionName, Other_Charges, JP_Charges,Door,Hs_Code);
                if (message == "Updated Successfully!")
                {
                    _oPurchase.purchaseDetailList = _oPurchase.Get_PurchaseDetailListByID_DAL(Temp_ID, hdPurchaseMaster_ID).ToList();

                    return PartialView("_PurchaseDetailPartial", _oPurchase);
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


            //return new PartialViewResult
            //{
            //    ViewName = "_PurchaseDetailPartial",
            //    ViewData = new ViewDataDictionary
            //<List<Pa_PurchaseDetails_DAL>>(ViewData, _oPurchase.purchaseDetailList)
            //};
        }

        //---This method is for getting purchase detail by id for update pop up form int the new purchase form
        [HttpGet]
        public IActionResult GetPuchaseDetailByID(int? PurchaseDetails_ID)
        {



            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion


            if (PurchaseDetails_ID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            try
            {
                #region ViewBag Area
                //---Make list for drop down in forms
                ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_ID).ToList();
                //---Get Colors List
                ViewBag.ColorsList = _oBasic.Get_ColorsList_DAL().ToList();
                //---Get currency list
                ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();
                ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_ID).ToList();

                //New
                ViewBag.VehicleCategory = _oBasic.Get_VehicleCategoryList_DAL(c_ID).ToList();


                //---Get EngineType
                ViewBag.EngineType = _oBasic.Get_EngineTypeList_DAL(c_ID).ToList();


                //---Get StockType
                ViewBag.StockType = _oBasic.Get_StockTypeList_DAL().ToList();

                #endregion
                _oPurchase.purchaseMasterObj = _oPurchase.Get_PurchaseMasterByID_DAL(PurchaseMasterNewStatic_ID);

                _oPurchase.purchaseObject = _oPurchase.GetPuchaseDetailByID_DAL(PurchaseDetails_ID);

                return PartialView("_PurchaseDetailByIDPartial", _oPurchase);


            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured! Please try again!" });
            }


        }


        //---Get purchase list by search filters
        [HttpGet]
        public IActionResult GetPurchaseListBySearchFilers(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, string ChassisNo, int? c_ID, int? page)
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

            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;

            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;
            From_Date = String.IsNullOrEmpty(From_Date) ? "" : From_Date;
            To_Date = String.IsNullOrEmpty(To_Date) ? "" : To_Date;
            Status_ID = Status_ID == null ? 0 : Status_ID;
            c_ID = c_ID == null ? 1 : c_ID;
            


            ViewBag.PurchaseRef_PurchaseList = PurchaseRef;
            ViewBag.Vendor_ID_PurchaseList = Vendor_ID;
            ViewBag.From_Date_PurchaseList = From_Date;
            ViewBag.To_Date_PurchaseList = To_Date;
            ViewBag.Status_ID_PurchaseList = Status_ID;
            ViewBag.c_ID_PurchaseList = c_ID;

            ViewBag.ChassisNo_PurchaseList = ChassisNo;


            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            try
            {
                ViewBag.RecordsPerPage = 10;
                _oPurchase.purchaseMasterIPagedList = _oPurchase.Get_PurchaseMasterList_DAL(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID,ChassisNo, c_ID).ToList().ToPagedList(page ?? 1, 10);
                _oPurchase.purchaseMasterTotal = _oPurchase.Get_PurchaseMasterList_TTL(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, ChassisNo, c_ID).ToList();
                return PartialView("_PurchaseListSearchPartial", _oPurchase);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }


        //---Get Othes Purchase list by search filters
        [HttpGet]
        public IActionResult GetPurchaseList_OthersBySearchFitlers(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID, int? page)
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

            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;
            From_Date = String.IsNullOrEmpty(From_Date) ? "" : From_Date;
            To_Date = String.IsNullOrEmpty(To_Date) ? "" : To_Date;
            Status_ID = Status_ID == null ? 0 : Status_ID;
            c_ID = c_ID == null ? 1 : c_ID;


            ViewBag.PurchaseRef_PurchaseOther = PurchaseRef;
            ViewBag.Vendor_ID_PurchaseOther = Vendor_ID;
            ViewBag.From_Date_PurchaseOther = From_Date;
            ViewBag.To_Date_PurchaseOther = To_Date;
            ViewBag.Status_ID_PurchaseOther = Status_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.c_ID_PurchaseOther = c_ID;
            try
            {
                ViewBag.RecordsPerPage = 10;
                _oPurchase.purchaseOtherMasterList = _oPurchase.Get_PurchaseMasterList_Other_DAL(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, c_ID).ToList().ToPagedList(page ?? 1, 10);
                return PartialView("_PurchaseOtherListSearchPartial", _oPurchase);
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }


        [HttpGet]
        public IActionResult PurchaseReturnList(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID, int? page)
        {
            //---Dumping static fields
            DumpStaticFieldsPurchase();


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            ViewBag.SectionHeaderTitle = "Purchases";
            ViewBag.SectionSub_HeaderTitle = "Purchasen Return List";
            ViewBag.PageTitle = "Purchase Return List";
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;
            From_Date = String.IsNullOrEmpty(From_Date) ? "" : From_Date;
            To_Date = String.IsNullOrEmpty(To_Date) ? "" : To_Date;
            Status_ID = Status_ID == null ? 0 : Status_ID;
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == null || c_ID == 0 ? c_IDs : c_ID;

            ViewBag.PurchaseRef_PurchaseReturn = PurchaseRef;
            ViewBag.Vendor_ID_PurchaseReturn = Vendor_ID;
            ViewBag.From_Date_PurchaseReturn = From_Date;
            ViewBag.To_Date_PurchaseReturn = To_Date;
            ViewBag.Status_ID_PurchaseReturn = Status_ID;
            ViewBag.c_ID_PurchaseReturn = c_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            ViewBag.RecordsPerPage = 10;
            _oPurchase.purchaseReturnMasterList = _oPurchase.Get_PurchaseMasterReturnList_DAL(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, c_ID).ToList().ToPagedList(page ?? 1, 10);
            return View(_oPurchase);


        }

        //---Get Othes Purchase list by search filters
        [HttpGet]
        public IActionResult GetPurchaseReturnList_BySearchFitlers(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID, int? page)
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

            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;
            From_Date = String.IsNullOrEmpty(From_Date) ? "" : From_Date;
            To_Date = String.IsNullOrEmpty(To_Date) ? "" : To_Date;
            Status_ID = Status_ID == null ? 0 : Status_ID;
            c_ID = c_ID == null ? 1 : c_ID;

            ViewBag.PurchaseRef_PurchaseReturn = PurchaseRef;
            ViewBag.Vendor_ID_PurchaseReturn = Vendor_ID;
            ViewBag.From_Date_PurchaseReturn = From_Date;
            ViewBag.To_Date_PurchaseReturn = To_Date;
            ViewBag.Status_ID_PurchaseReturn = Status_ID;
            ViewBag.c_ID_PurchaseReturn = c_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            try
            {
                ViewBag.RecordsPerPage = 10;
                _oPurchase.purchaseReturnMasterList = _oPurchase.Get_PurchaseMasterReturnList_DAL(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, c_ID).ToList().ToPagedList(page ?? 1, 10);
                return PartialView("_PurchaseReturnListSearchPartial", _oPurchase);
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }




        //This method is for inserting purchase Master
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertPurchaseMaster(int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string PurchaseDate, int? VAT_Rate, double? Discount)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            int? c_ID = c_IDs;
            Temp_ID = PurchaseNewTempID;
            string PurchaseType = "PR";  //---It is the ref in table for new purchase. Send this value hard coded from here

            if (Vendor_ID == null || String.IsNullOrEmpty(PurchaseDate))
            {
                TempData["Error"] = "Please Select Vendor!";
                return RedirectToAction("NewPurchase", "Purchases");
            }

            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;
            Discount = Discount == null ? 0 : Discount;


            try
            {
                message = _oPurchase.InsertPurchaseMaster_DAL(Vendor_ID, Vendor_PruchaseTo, Vendor_Address, PurchaseType, PurchaseDate, VAT_Rate, Discount, Temp_ID, c_ID, Created_by);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("PurchaseList", "Purchases");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("NewPurchase", "Purchases");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("NewPurchase", "Purchases");
            }



        }


        //get subclassification list
        public JsonResult GetModelDescListByID(string Make_ID)
        {


            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            var ModelDescList = new List<Pa_ModelDesc_DAL>();
            //Make_ID = null;

            if (String.IsNullOrEmpty(Make_ID))
            {

                //return Json(ModelDescList, new Newtonsoft.Json.JsonSerializerSettings());
                return Json(ModelDescList);
            }

            _oBasic.ModelDescList = _oBasic.Get_ModelList_DAL(c_ID).ToList();

            ModelDescList = _oBasic.ModelDescList.Where(m => (m.Make_ID == (Convert.ToInt32(Make_ID)))).ToList();




            return Json(ModelDescList);
            //return Json(ModelDescList, new Newtonsoft.Json.JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult DeletePurchaseDetail(int? PurchaseDetails_ID)
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
            Temp_ID = PurchaseNewTempID;
            string OldTempID = _oPurchase.GetOldTempIDFromPurchaseDetail_DAL(PurchaseMasterNewStatic_ID);
            if (!String.IsNullOrEmpty(PurchaseNewTempID))
            {
                Temp_ID = PurchaseNewTempID;
            }
            else
            {
                Temp_ID = OldTempID;
            }
            PurchaseMasterNewStatic_ID = PurchaseMasterNewStatic_ID == null ? 0 : PurchaseMasterNewStatic_ID;

            if (PurchaseDetails_ID == null)
            {
                return Json(new { message = "ID is null. Please try again!" });
            }

            try
            {
                message = _oPurchase.DeletePurchaseDetail_DAL(PurchaseDetails_ID);
                if (message == "Deleted Successfully!")
                {
                    _oPurchase.purchaseDetailList = _oPurchase.Get_PurchaseDetailListByID_DAL(Temp_ID, PurchaseMasterNewStatic_ID).ToList();

                    return PartialView("_PurchaseDetailPartial", _oPurchase);
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

        //This method is for inserting New purchase Master
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePurchaseMaster(int? PurchaseMaster_ID, int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string PurchaseDate, int? VAT_Rate, double? Discount)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            Temp_ID = PurchaseNewTempID;
            string OldTempID = _oPurchase.GetOldTempIDFromPurchaseDetail_DAL(PurchaseMaster_ID);
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


            if (Vendor_ID == null || String.IsNullOrEmpty(PurchaseDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            if (PurchaseMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again!";
                return Redirect(url);
            }



            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;
            Discount = Discount == null ? 0 : Discount;

            try
            {
                message = _oPurchase.UpdatePurchaseMaster_DAL(PurchaseMaster_ID, Vendor_ID, Vendor_PruchaseTo, Vendor_Address, PurchaseDate, VAT_Rate, Discount, Temp_ID, Modified_by);
                if (message.Contains("Successfully"))
                {

                    //TempData["Success"] = "Updated Successfully!";
                    return RedirectToAction("PurchaseList", "Purchases");
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


        //---ohter purchases get method
        [HttpGet]
        public IActionResult OtherPurchase(int? PurchaseMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Purchases";
            ViewBag.BreadCumAction = "NewPurchase";
            ViewBag.SectionHeaderTitle = "Purchases";
            ViewBag.SectionSub_HeaderTitle = "Purchase";
            ViewBag.SectionSub_HeaderMainTitle = "Purchase List";
            ViewBag.PageTitle = "Purchase";
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
           // ViewBag.UOMList = _oBasic.Get_UOM_DAL(c_ID).ToList();
            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList();
            //---Get Item master list
            ViewBag.ItemMaster = _oBasic.Get_ItemMasterList_DAL().ToList();
            //---Get Vendor Category List
            ViewBag.VendorCategoryList = _oBasic.Get_VendorCategory_DAL().ToList();
            ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_ID).ToList();
            ViewBag.ItemLocation = _oBasic.Get_ItemLocationList_DAL(c_ID).ToList();

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;


            ViewBag.PO_SerialLabel = configuration.GetSection("AppSettings").GetSection("PO_SerialLabel").Value;



            #endregion


            if (PurchaseMaster_ID == null || PurchaseMaster_ID < 1)
            {

                //---Create Purchase master Ref(tem id)
                if (String.IsNullOrEmpty(PurchaseOtherTempID))
                {
                    Guid obj = Guid.NewGuid();
                    PurchaseOtherTempID = obj.ToString();
                }

                PurchaseMasterOtherStatic_ID = PurchaseMaster_ID;

                //---Get purchase master id
                ViewBag.PurchaseMaster_ID = PurchaseMaster_ID;

                //--Get purchase Master
                _oPurchase.purchaseMasterObj = _oPurchase.Get_PurchaseMaster_OtherByID_DAL(-1);

                //--Get purchase detail 
                _oPurchase.purchaseDetailList = _oPurchase.Get_PurchaseDetailOtherListByID_DAL(PurchaseOtherTempID, -1).ToList();

            
            }
            else
            {

                Urls = null;

                OtherURL = HttpContext.Request.Headers["Referer"];
                Urls = OtherURL;

                //---Get old temp id from purchase detail table that already creaded for this "PurchaseMaster_ID" id. And then use this if new record added
                string OldTempID = _oPurchase.GetOldTempIDFromPurchaseDetail_DAL(PurchaseMaster_ID);



                PurchaseMasterOtherStatic_ID = PurchaseMaster_ID;

                //---Get purchase master id
                ViewBag.PurchaseMaster_ID = PurchaseMaster_ID;



                //--Get purchase Master
                _oPurchase.purchaseMasterObj = _oPurchase.Get_PurchaseMaster_OtherByID_DAL(PurchaseMaster_ID);

                //--Get purchase detail
                _oPurchase.purchaseDetailList = _oPurchase.Get_PurchaseDetailOtherListByID_DAL(OldTempID, PurchaseMaster_ID).ToList();

            }



            return View(_oPurchase);
        }

        //---Getting Other purchase list
        [HttpGet]

        public IActionResult OtherPurchaseList(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID, int? page)
        {
            //---Dumping static fields
            DumpStaticFieldsPurchase();


            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Purchases";
            ViewBag.BreadCumAction = "OtherPurchaseList";
            ViewBag.SectionHeaderTitle = "Purchases";
            ViewBag.SectionSub_HeaderTitle = "Other Purchase List";
            ViewBag.PageTitle = "Other Purchase List";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;
            From_Date = String.IsNullOrEmpty(From_Date) ? "" : From_Date;
            To_Date = String.IsNullOrEmpty(To_Date) ? "" : To_Date;
            Status_ID = Status_ID == null ? 0 : Status_ID;
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == null || c_ID == 0 ? c_IDs : c_ID;

            #region Veiwbags area
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_IDs).ToList();
            //---Get PurchaseStatus
            ViewBag.PurchaseStatus = _oBasic.Get_StatusList_byType_DAL("Purchase").ToList();

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;


            #endregion veiwbags area

            ViewBag.PurchaseRef_PurchaseOther = PurchaseRef;
            ViewBag.Vendor_ID_PurchaseOther = Vendor_ID;
            ViewBag.From_Date_PurchaseOther = From_Date;
            ViewBag.To_Date_PurchaseOther = To_Date;
            ViewBag.Status_ID_PurchaseOther = Status_ID;
            ViewBag.c_ID_PurchaseOther = c_ID;

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            ViewBag.RecordsPerPage = 10;
            //---Purchase master other  list for page
            _oPurchase.purchaseOtherMasterList = _oPurchase.Get_PurchaseMasterList_Other_DAL(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, c_ID).ToList().ToPagedList(page ?? 1, 10);


            return View(_oPurchase);
        }

        //---This method is for inserting other purchase detail record in purchase detail table
        [HttpPost]
        [HttpPost]
        public IActionResult InsertOtherPurchaseDetail(int? item_ID, string item_Description, double? Quantity, string UOM, int? Currency_ID, double? Currency_Rate, double? Unit_Price, int? VAT_Rate,
           double? VAT_Exp, double? Discount, double? Amount, double? TtlAmount, double? Amount_Others, int hdPurchaseMaster_ID, int Location_ID, string Serial, string Barcode)
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
            Temp_ID = PurchaseOtherTempID;
            if (hdPurchaseMaster_ID == 0)
            {
                Temp_ID = PurchaseOtherTempID;
            }
            else
            {
                Temp_ID = "0";
            }
            PurchaseMasterOtherStatic_ID = PurchaseMasterOtherStatic_ID == null ? -1 : PurchaseMasterOtherStatic_ID;

            if (Quantity == null ||
                 Unit_Price == null || Amount == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }


            item_ID = item_ID == null ? 0 : item_ID;
            Currency_ID = Currency_ID == null ? 0 : Currency_ID;
            Currency_Rate = Currency_Rate == null ? 0 : Currency_Rate;
            UOM = String.IsNullOrEmpty(UOM) ? "" : UOM;
            VAT_Exp = VAT_Exp == null ? 0 : VAT_Exp;
            VAT_Rate = VAT_Exp == null ? 0 : VAT_Rate;
            Discount = Discount == null ? 0 : Discount;


            try
            {
                message = _oPurchase.InsertPurchaseDetail_Other_DAL(item_ID, item_Description, Quantity, UOM, Currency_ID, Currency_Rate, Unit_Price, VAT_Rate,
            VAT_Exp, Discount, Amount, TtlAmount, Amount_Others, Temp_ID, c_ID, Created_by, hdPurchaseMaster_ID, Location_ID, Serial, Barcode);
                if (message == "Saved Successfully!")
                {

                    _oPurchase.purchaseDetailList = _oPurchase.Get_PurchaseDetailOtherListByID_DAL(Temp_ID, hdPurchaseMaster_ID).ToList();

                    return PartialView("_PurchaseDetail_Other_Partial", _oPurchase);
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




        //---This method is for Updating other purchase detail record in purchase detail table
        [HttpPost]
        public IActionResult UpdateOtherPurchaseDetail(int? PurchaseDetails_ID, int? item_ID, string item_Description, double? Quantity, string UOM, int? Currency_ID, double? Currency_Rate, double? Unit_Price, int? VAT_Rate,
           double? VAT_Exp, double? Discount, double? Amount, double? TtlAmount, double? Amount_Others, int hdPurchaseMaster_ID, int Location_ID, string Serial,string Barcode)
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
            Temp_ID = PurchaseOtherTempID;
            if (hdPurchaseMaster_ID == 0)
            {
                Temp_ID = PurchaseOtherTempID;
            }
            else
            {
                Temp_ID = "0";
            }
            PurchaseMasterOtherStatic_ID = PurchaseMasterOtherStatic_ID == null ? -1 : PurchaseMasterOtherStatic_ID;
            if (Quantity == null ||
                 Unit_Price == null || Amount == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }
            item_ID = item_ID == null ? 0 : item_ID;
            Currency_ID = Currency_ID == null ? 0 : Currency_ID;
            Currency_Rate = Currency_Rate == null ? 0 : Currency_Rate;
            UOM = String.IsNullOrEmpty(UOM) ? "" : UOM;
            VAT_Exp = VAT_Exp == null ? 0 : VAT_Exp;
            VAT_Rate = VAT_Exp == null ? 0 : VAT_Rate;
            Discount = Discount == null ? 0 : Discount;
            try
            {
                message = _oPurchase.UpdatePurchaseDetail_Other_DAL(PurchaseDetails_ID, item_ID, item_Description, Quantity, UOM, Currency_ID, Currency_Rate, Unit_Price, VAT_Rate,
            VAT_Exp, Discount, Amount, TtlAmount, Amount_Others, Temp_ID, c_ID, Modified_by, Location_ID, Serial, Barcode);
                if (message == "Updated Successfully!")
                {
                    _oPurchase.purchaseDetailList = _oPurchase.Get_PurchaseDetailOtherListByID_DAL(Temp_ID, hdPurchaseMaster_ID).ToList();
                    return PartialView("_PurchaseDetail_Other_Partial", _oPurchase);
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


        //This method is for inserting New purchase Master
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOtherPurchaseMaster(int? PurchaseMaster_ID, int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string PurchaseDate)
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
            int? Modified_by = Convert.ToInt32(UserID_Admin);

            Temp_ID = PurchaseOtherTempID;
            string OldTempID = _oPurchase.GetOldTempIDFromPurchaseDetail_DAL(PurchaseMaster_ID);
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


            if (Vendor_ID == null || String.IsNullOrEmpty(PurchaseDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            if (PurchaseMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again!";
                return Redirect(url);
            }



            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;


            try
            {
                message = _oPurchase.UpdateOtherPurchaseMaster_DAL(PurchaseMaster_ID, Vendor_ID, Vendor_PruchaseTo, Vendor_Address, PurchaseDate, Temp_ID, c_ID, Modified_by);
                if (message.Contains("Successfully"))
                {

                    //TempData["Success"] = "Updated Successfully!";
                    return RedirectToAction("OtherPurchaseList", "Purchases");
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


        //This method is for inserting other purchase Master
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertOtherPurchaseMaster(int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string PurchaseDate)
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
            Temp_ID = PurchaseOtherTempID;
            string PurchaseType = "PO";  //---It is the ref in table for new purchase. Send this value hard coded from here

            if (Vendor_ID == null || String.IsNullOrEmpty(PurchaseDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("OtherPurchase", "Purchases");
            }

            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;


            try
            {
                message = _oPurchase.Insert_OtherPurchaseMaster_DAL(Vendor_ID, Vendor_PruchaseTo, Vendor_Address, PurchaseType, PurchaseDate, Temp_ID, c_ID, Created_by);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("OtherPurchaseList", "Purchases");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("OtherPurchase", "Purchases");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("OtherPurchase", "Purchases");
            }



        }


        //---Method for deleting purchase master New
        [HttpPost]
        public IActionResult DeletePurchaseMasterNew(int? PurchaseMaster_ID)
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



            if (PurchaseMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }
            string message = "";


            try
            {
                message = _oPurchase.DeletePurchaseMaster_DAL(PurchaseMaster_ID, Convert.ToInt32(UserID_Admin));
                if (message.Contains("SUCCESSFULLY"))
                {
                    return RedirectToAction("PurchaseList", "Purchases");
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

        //---Method for deleting purchase master other
        [HttpPost]
        public IActionResult DeletePurchaseMaster_Other(int? PurchaseMaster_ID)
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

            if (PurchaseMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oPurchase.DeletePurchaseMasterOther_DAL(PurchaseMaster_ID, 0);
                if (message.Contains("Successfully"))
                {
                    return RedirectToAction("OtherPurchaseList", "Purchases");
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

        //---Method for deleting purchase master New
        [HttpPost]
        public IActionResult ChangePurchaseMasterStatus(int? PurchaseMaster_ID, int? Status_ID)
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
            string Trans_Type = "Purchase_New";
            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();


            if (PurchaseMaster_ID == null || Status_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oPurchase.ChangePurchaseMasterStatus_DAL(PurchaseMaster_ID, Status_ID, c_ID, Trans_Type, Modified_by);
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


        //---Method for deleting purchase master New
        [HttpPost]
        public IActionResult ChangePurchaseMasterStatus_Other(int? PurchaseMaster_ID, int? Status_ID)
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
            string Trans_Type = "Purchase_Other";

            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();


            if (PurchaseMaster_ID == null || Status_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {


                message = _oPurchase.ChangePurchaseMasterStatus_DAL(PurchaseMaster_ID, Status_ID, c_ID, Trans_Type, Modified_by);
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


        //---Delete purchase detail other
        [HttpPost]
        public IActionResult DeletePurchaseDetailOther(int? PurchaseDetails_ID)
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
            Temp_ID = PurchaseOtherTempID;
            string OldTempID = _oPurchase.GetOldTempIDFromPurchaseDetail_DAL(PurchaseMasterOtherStatic_ID);
            if (!String.IsNullOrEmpty(PurchaseOtherTempID))
            {
                Temp_ID = PurchaseOtherTempID;
            }
            else
            {
                Temp_ID = OldTempID;
            }
            PurchaseMasterOtherStatic_ID = PurchaseMasterOtherStatic_ID == null ? -1 : PurchaseMasterOtherStatic_ID;

            if (PurchaseDetails_ID == null)
            {
                return Json(new { message = "ID is null. Please try again!" });
            }

            try
            {
                message = _oPurchase.DeletePurchaseDetail_DAL(PurchaseDetails_ID);
                if (message == "Deleted Successfully!")
                {
                    _oPurchase.purchaseDetailList = _oPurchase.Get_PurchaseDetailOtherListByID_DAL(Temp_ID, PurchaseMasterOtherStatic_ID).ToList();

                    return PartialView("_PurchaseDetail_Other_Partial", _oPurchase);
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

        //---Purchase return voucher get page
        [HttpGet]
        public IActionResult PurchaseReturn(int? PurchaseMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Purchases";
            ViewBag.BreadCumAction = "NewPurchase";
            ViewBag.SectionHeaderTitle = "Purchases";
            ViewBag.SectionSub_HeaderTitle = "Purchase Return";
            ViewBag.PageTitle = "Purchase Return";

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

            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList();
            //---Get Item master list
            ViewBag.ItemMaster = _oBasic.Get_ItemMasterList_DAL().ToList();

            #endregion


            if (PurchaseMaster_ID == null || PurchaseMaster_ID < 1)
            {


                //---Create Purchase master Ref(tem id)
                if (String.IsNullOrEmpty(PurchaseReturnTempID))
                {
                    Guid obj = Guid.NewGuid();
                    PurchaseReturnTempID = obj.ToString();
                }

                Purchase_Return_Static_ID = PurchaseMaster_ID;

                //---Get purchase master id
                ViewBag.PurchaseMaster_ID = PurchaseMaster_ID;

                //--Get purchase Master
                _oPurchase.purchaseMasterObj = _oPurchase.Get_PurchaseMaster_Return_ByID_DAL(-1);

                //--Get purchase detail 
                _oPurchase.purchaseDetailList = _oPurchase.Get_PurchaseDetailReturnListByID_DAL(PurchaseReturnTempID, -1).ToList();
            }
            else
            {



                //---Get old temp id from purchase detail table that already creaded for this "PurchaseMaster_ID" id. And then use this if new record added
                string OldTempID = _oPurchase.GetOldTempIDFromPurchaseDetail_DAL(PurchaseMaster_ID);



                Purchase_Return_Static_ID = PurchaseMaster_ID;

                //---Get purchase master id
                ViewBag.PurchaseMaster_ID = PurchaseMaster_ID;



                //--Get purchase Master
                _oPurchase.purchaseMasterObj = _oPurchase.Get_PurchaseMaster_Return_ByID_DAL(PurchaseMaster_ID);

                //--Get purchase detail
                _oPurchase.purchaseDetailList = _oPurchase.Get_PurchaseDetailReturnListByID_DAL(OldTempID, PurchaseMaster_ID).ToList();
            }



            return View(_oPurchase);
        }

        //---method for inserting return purchase record that already exists in purchase detail table and then returning the list of new insered record
        [HttpGet]
        public IActionResult Insert_And_GetPurhaseReturnDetail(string PurchaseRef)
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
            Temp_ID = PurchaseReturnTempID;
            string OldTempID = _oPurchase.GetOldTempIDFromPurchaseDetail_DAL(Purchase_Return_Static_ID);
            if (!String.IsNullOrEmpty(PurchaseReturnTempID))
            {
                Temp_ID = PurchaseReturnTempID;

            }
            else
            {
                Temp_ID = OldTempID;
            }
            Purchase_Return_Static_ID = Purchase_Return_Static_ID == null ? -1 : Purchase_Return_Static_ID;

            if (String.IsNullOrEmpty(PurchaseRef))
            {
                return Json(new { message = "Please enter valid purchase ref" });
            }

            try
            {
                message = _oPurchase.CheckIfRefExistInPurchaseMaster_DAL(PurchaseRef);
                if (message == "Ref Exists")
                {

                    message = _oPurchase.Insert_PurhaseReturnDetail_DAL(Temp_ID, PurchaseRef);
                    if (message == "Saved Successfully!")
                    {
                        _oPurchase.purchaseDetailList = _oPurchase.Get_PurchaseDetailReturnListByID_DAL(PurchaseReturnTempID, Purchase_Return_Static_ID).ToList();

                        return PartialView("_PurchaseDetail_Return_Partial", _oPurchase);
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
                    return Json(new { message = message });
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
        public IActionResult DeletePurchaseDetail_Return(int? PurchaseDetails_ID)
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
            Temp_ID = PurchaseReturnTempID;
            string OldTempID = _oPurchase.GetOldTempIDFromPurchaseDetail_DAL(Purchase_Return_Static_ID);
            if (!String.IsNullOrEmpty(PurchaseReturnTempID))
            {
                Temp_ID = PurchaseReturnTempID;
            }
            else
            {
                Temp_ID = OldTempID;
            }
            Purchase_Return_Static_ID = Purchase_Return_Static_ID == null ? -1 : Purchase_Return_Static_ID;

            if (PurchaseDetails_ID == null)
            {
                return Json(new { message = "ID is null. Please try again!" });
            }

            try
            {
                message = _oPurchase.DeletePurchaseDetail_DAL(PurchaseDetails_ID);
                if (message == "Deleted Successfully!")
                {
                    _oPurchase.purchaseDetailList = _oPurchase.Get_PurchaseDetailReturnListByID_DAL(Temp_ID, Purchase_Return_Static_ID).ToList();

                    return PartialView("_PurchaseDetail_Return_Partial", _oPurchase);
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
        public IActionResult InsertPurchaseMaster_Return(int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string PurchaseDate)
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
            Temp_ID = PurchaseReturnTempID;
            string PurchaseType = "PX";  //---It is the ref in table for new purchase. Send this value hard coded from here

            if (Vendor_ID == null || String.IsNullOrEmpty(PurchaseDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("PurchaseReturn", "Purchases");
            }

            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;


            try
            {
                message = _oPurchase.Insert_PurchaseMaster_Return_DAL(Vendor_ID, Vendor_PruchaseTo, Vendor_Address, PurchaseType, PurchaseDate, Temp_ID, c_ID, Created_by);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("PurchaseReturnList", "Purchases");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("PurchaseReturn", "Purchases");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("PurchaseReturn", "Purchases");
            }



        }

        //This method is for updating  purchase Master return
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePurchaseMaster_Return(int? PurchaseMaster_ID, int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string PurchaseDate)
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
            Temp_ID = PurchaseReturnTempID;
            string OldTempID = _oPurchase.GetOldTempIDFromPurchaseDetail_DAL(PurchaseMaster_ID);
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


            if (Vendor_ID == null || String.IsNullOrEmpty(PurchaseDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            if (PurchaseMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again!";
                return Redirect(url);
            }



            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;


            try
            {
                message = _oPurchase.Update_Return_PurchaseMaster_DAL(PurchaseMaster_ID, Vendor_ID, Vendor_PruchaseTo, Vendor_Address, PurchaseDate, Temp_ID, c_ID, Modified_by);
                if (message == "Updated Successfully!")
                {

                    //TempData["Success"] = "Updated Successfully!";
                    return RedirectToAction("PurchaseReturnList", "Purchases");
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
        public IActionResult DeletePurchaseMaster_Return(int? PurchaseMaster_ID)
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

            if (PurchaseMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oPurchase.DeletePurchaseMaster_DAL(PurchaseMaster_ID, 0);
                if (message.Contains("SUCCESSFULLY"))
                {
                    return RedirectToAction("PurchaseReturnList", "Purchases");
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

        //---Method for Changing purchase Return Status
        [HttpPost]
        public IActionResult Change_Status_PurchaseMasterReturn(int? PurchaseMaster_ID, int? Status_ID)
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
            string Trans_Type = "Purchase_Return";
            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();


            if (PurchaseMaster_ID == null || Status_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {

                message = _oPurchase.ChangePurchaseMasterStatus_DAL(PurchaseMaster_ID, Status_ID, c_ID, Trans_Type, Modified_by);
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




        [HttpGet]
        public IActionResult UploadStockBulk()
        {


            #region ViewBag Area
            //---Ven Category list for drop down in forms

            #endregion
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "UploadStockBulk";
            ViewBag.BreadCumAction = "UploadStockBulkList";
            ViewBag.SectionHeaderTitle = "Stock";
            ViewBag.SectionSub_HeaderTitle = "Upload Stock";
            ViewBag.PageTitle = "Upload Stock Bulk";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            //---Create BulkUpload_TempID id
            if (String.IsNullOrEmpty(BulkUpload_TempID))
            {
                Guid obj = Guid.NewGuid();
                BulkUpload_TempID = obj.ToString();
            }


            //---get bul upload stock list

            _oPurchase.ImportHolderList = _oPurchase.Get_StockPuchaseBulkList_DAL().ToList();

            return View(_oPurchase);
        }



        //---Get purchase list by search filters
        [HttpGet]
        public IActionResult GetPurchaseMasterNew_Attachments(int? PurchaseMaster_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            string Type = "PurchaseNew";

            try
            {
                _oPurchase.attachmentList = _oPurchase.GetPurchaseMasterNew_Attachments_DAL(PurchaseMaster_ID, Type).ToList();

                return PartialView("_PurchaseListAttachment", _oPurchase);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }


        [HttpPost]
        public IActionResult InsertAttachments_PurchaseMaster(int? PurchaseMaster_ID, string Type, string Document_Type, IFormFile Attachment)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            if (PurchaseMaster_ID == null || String.IsNullOrEmpty(Document_Type) || Attachment == null)
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


                message = _oPurchase.Insert_PurchaseMasterAttachment_DAL(PurchaseMaster_ID, Document_Type, Type, newFileName, filepath, Created_by);
                if (message == "Saved Successfully!")
                {

                    _oPurchase.attachmentList = _oPurchase.GetPurchaseMasterNew_Attachments_DAL(PurchaseMaster_ID, Type).ToList();

                    return PartialView("_PurchaseListAttachment", _oPurchase);
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


        //---Stock in PO Get method
        [HttpGet]
        public IActionResult StockInPO(string PurchaseRef)
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
            PurchaseRef = (String.IsNullOrEmpty(PurchaseRef) || String.IsNullOrWhiteSpace(PurchaseRef) ? "" : PurchaseRef);

            _oPurchase.StockInPoList = _oPurchase.get_Stock_in_PO_byPurchaseRef_DAL(PurchaseRef).ToList();
            return View(_oPurchase);
        }


        //---this method is for deleting attachment in purchase list of type new
        [HttpGet]
        public IActionResult DeleteAttachmentPurchaseNew(int? PurchaseMaster_ID, int? Attachment_ID, string Type, string File_Name)
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
            //string url = Request.Headers["Referer"].ToString();
            if (PurchaseMaster_ID == null)
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

                message = _oPurchase.Delete_Attachment_PurchaseNew(Attachment_ID);
                if (message == "Deleted Successfully!")
                {

                    _oPurchase.attachmentList = _oPurchase.GetPurchaseMasterNew_Attachments_DAL(PurchaseMaster_ID, Type).ToList();

                    return PartialView("_PurchaseListAttachment", _oPurchase);
                }
                else
                {
                    TempData["Error"] = message;
                    return PartialView("_PurchaseListAttachment", _oPurchase);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                TempData["Error"] = ErrorMessage;
                return Json(new { message = ErrorMessage });
            }

            //return View();
        }


        //---Get purchase other attachment list by search filters
        [HttpGet]
        public IActionResult GetPurchaseMasterOther_Attachments(int? PurchaseMaster_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            string Type = "Purchases_OtherPurchase";

            try
            {
                _oPurchase.attachmentList = _oPurchase.GetPurchaseMasterNew_Attachments_DAL(PurchaseMaster_ID, Type).ToList();

                return PartialView("_PurchaseListOtherAttachment", _oPurchase);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }
        }

        //Insert Attachment method of purchase other
        [HttpPost]
        public IActionResult InsertAttachments_PurchaseOther(int? PurchaseMaster_ID, string Type, string Document_Type, IFormFile Attachment)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            if (PurchaseMaster_ID == null || String.IsNullOrEmpty(Document_Type) || Attachment == null)
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

                message = _oPurchase.Insert_PurchaseMasterAttachment_DAL(PurchaseMaster_ID, Document_Type, Type, newFileName, filepath, Created_by);
                if (message == "Saved Successfully!")
                {

                    _oPurchase.attachmentList = _oPurchase.GetPurchaseMasterNew_Attachments_DAL(PurchaseMaster_ID, Type).ToList();

                    return PartialView("_PurchaseListOtherAttachment", _oPurchase);
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

        //---this method is for deleting attachment in purchase list of type other
        [HttpGet]
        public IActionResult DeleteAttachmentPurchaseOther(int? PurchaseMaster_ID, int? Attachment_ID, string Type, string File_Name)
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

            if (PurchaseMaster_ID == null)
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

                message = _oPurchase.Delete_Attachment_PurchaseNew(Attachment_ID);
                if (message == "Deleted Successfully!")
                {

                    _oPurchase.attachmentList = _oPurchase.GetPurchaseMasterNew_Attachments_DAL(PurchaseMaster_ID, Type).ToList();

                    return PartialView("_PurchaseListAttachment", _oPurchase);
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
        public IActionResult Method2(IFormFile postedFile)
        {
            string FilePath = string.Empty;


            string conString = configuration.GetSection("ConnectionStrings").GetSection("DBConnect").Value;

            SqlConnection conSql = new SqlConnection(conString);


            FilePath = Path.GetTempFileName();

            var fileLocation = new FileInfo(FilePath);

            using (FileStream fs = System.IO.File.Create(FilePath))
            {
                postedFile.CopyTo(fs);
                fs.Flush();
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(fileLocation))

            {

                ExcelWorksheet workSheet = package.Workbook.Worksheets["Sheet1"];

                int totalRows = workSheet.Dimension.Rows;

                DataTable tblcustomers = new DataTable();

                tblcustomers.Columns.Add("customerId", typeof(int));

                tblcustomers.Columns.Add("FirstName");

                tblcustomers.Columns.Add("LastName");

                tblcustomers.Columns.Add("Role");

                tblcustomers.Columns.Add("Gender");

                for (int i = 2; i <= totalRows; i++)

                {

                    DataRow dr = tblcustomers.NewRow();

                    dr["customerId"] = workSheet.Cells[i, 1].Value.ToString();

                    dr["FirstName"] = workSheet.Cells[i, 2].Value.ToString();



                    dr["LastName"] = workSheet.Cells[i, 3].Value.ToString();



                    dr["Role"] = workSheet.Cells[i, 4].Value.ToString();

                    dr["Gender"] = workSheet.Cells[i, 5].Value.ToString();

                    tblcustomers.Rows.Add(dr);

                }

                SqlBulkCopy objbulk = new SqlBulkCopy(conSql);

                objbulk.DestinationTableName = "Customers";

                //objbulk.ColumnMappings.Add("FirstName", "FirstName");

                //objbulk.ColumnMappings.Add("LastName", "LastName");

                //objbulk.ColumnMappings.Add("Role", "Role");

                //objbulk.ColumnMappings.Add("Gender", "Gender");

                conSql.Open();

                objbulk.WriteToServer(tblcustomers);

                conSql.Close();

            }

            return View();
        }

        //--insert excel file int "imort holder table"
        [HttpPost]
        public IActionResult ImportExcelBulkUpdate(IFormFile postedFile)
        {


            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserID_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }


            int? User_ID = Convert.ToInt32(UserID_Admin);
            int c_ID = Convert.ToInt32(company_ID);


            if (postedFile == null || postedFile.Length <= 0)
            {
                return Json(new { message = "Please select an excel file" });
            }

            string message = "";
            var newFileName = "";
            var filepath = "";
            string Temp_ID = "";
            Temp_ID = BulkUpload_TempID;


            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

            var fileName = Path.GetFileName(postedFile.FileName);

            //Assigning Unique Filename (Guid)
            var myUniqueFileName = Convert.ToString(Guid.NewGuid());

            //Getting file Extension
            var fileExtension = Path.GetExtension(fileName);

            // concatenating  FileName + FileExtension
            newFileName = String.Concat(myUniqueFileName, fileExtension);

            // Combines two strings into a path.

            filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{newFileName}";

            if (!newFileName.EndsWith(".xlsx"))
            {

                return Json(new { message = "Excel file should be only be with .xlsx extension" });
            }


            using (FileStream fs = System.IO.File.Create(filepath))
            {
                postedFile.CopyTo(fs);
                fs.Flush();
            }
            //---only for non commercial use bcz package EPPlus required certificate for commercial use
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var ep = new ExcelPackage(new FileInfo(filepath));
            var ws = ep.Workbook.Worksheets["Sheet1"];

            var listImportHolder = new List<importHolder>();


            try
            {



                for (int rw = 2; rw <= ws.Dimension.End.Row; rw++)
                {
                    listImportHolder.Add(new importHolder
                    {
                        MAKENAME = ws?.Cells[rw, 1]?.Value?.ToString().Trim(),
                        MAKEMODELNAME = ws?.Cells[rw, 2]?.Value?.ToString().Trim(),
                        CHASSIS_NO = ws?.Cells[rw, 3]?.Value?.ToString().Trim(),
                        MODEL = ws?.Cells[rw, 4]?.Value?.ToString().Trim(),
                        COLORNAME = ws?.Cells[rw, 5]?.Value?.ToString().Trim(),
                        COLOR_INT = ws?.Cells[rw, 6]?.Value?.ToString().Trim(),
                        PRICE = ws?.Cells[rw, 7]?.Value?.ToString().Trim(),
                        PRICE_RATE = ws?.Cells[rw, 8]?.Value?.ToString().Trim(),
                        FREIGHT = ws?.Cells[rw, 9]?.Value?.ToString().Trim(),
                        FR_RATE = ws?.Cells[rw, 10]?.Value?.ToString().Trim(),
                        SELLING_PRICE = ws?.Cells[rw, 11]?.Value?.ToString().Trim(),
                        TRANSMISSION = ws?.Cells[rw, 12]?.Value?.ToString().Trim(),
                        DOOR = ws?.Cells[rw, 13]?.Value?.ToString().Trim(),
                        DRIVE = ws?.Cells[rw, 14]?.Value?.ToString().Trim(),
                        ENGINE_NO = ws?.Cells[rw, 15]?.Value?.ToString().Trim(),
                        WEIGHT = ws?.Cells[rw, 16]?.Value?.ToString().Trim(),
                        HS_CODE = ws?.Cells[rw, 17]?.Value?.ToString().Trim(),
                        ENGINE_POWER = ws?.Cells[rw, 18]?.Value?.ToString().Trim(),
                        MILEAGE = ws?.Cells[rw, 19]?.Value?.ToString().Trim(),
                        VEHICLE_CC = ws?.Cells[rw, 20]?.Value?.ToString().Trim(),
                        USED_NEW = ws?.Cells[rw, 21]?.Value?.ToString().Trim(),
                        OPTIONS = ws?.Cells[rw, 22]?.Value?.ToString().Trim(),
                        FUEL_TYPE = ws?.Cells[rw, 23]?.Value?.ToString().Trim(),
                        MAKECATEGORYNAME = ws?.Cells[rw, 24]?.Value?.ToString().Trim(),
                        VENDORNAME = ws?.Cells[rw, 25]?.Value?.ToString().Trim(),
                        // PURCHASEDATE = Convert.ToDateTime(ws?.Cells[rw, 26]?.Value?.ToString().Trim()).ToString("dd-MMM-yyyy"),

                        PURCHASEDATE = DateTime.FromOADate(Convert.ToDouble(ws?.Cells[rw, 26]?.Value?.ToString().Trim())).ToString("dd-MMM-yyyy"),
                        //PURCHASEDATE = Convert.ToDateTime(ws?.Cells[rw, 26]?.Value?.ToString().Trim()).ToString("dd-MMM-yyyy"),

                        PURCHASE_REF = ws?.Cells[rw, 27]?.Value?.ToString().Trim(),
                        BL_NO = ws?.Cells[rw, 28]?.Value?.ToString().Trim(),
                        SHIP_NAME = ws?.Cells[rw, 29]?.Value?.ToString().Trim(),
                        SHIPDATE = ws?.Cells[rw, 30]?.Value?.ToString().Trim(),
                        LEAVE_DATE = ws?.Cells[rw, 31]?.Value?.ToString().Trim(),
                        ENTRY_DATE = ws?.Cells[rw, 32]?.Value?.ToString().Trim(),
                        BOE = ws?.Cells[rw, 33]?.Value?.ToString().Trim(),
                        LOCATION = ws?.Cells[rw, 34]?.Value?.ToString().Trim(),
                        MAKECOUNTRYNAME = ws?.Cells[rw, 35]?.Value?.ToString().Trim(),
                        AVAILABILITY = ws?.Cells[rw, 36]?.Value?.ToString().Trim(),
                        STOCK_TYPE = ws?.Cells[rw, 37]?.Value?.ToString().Trim(),
                        SHOWROOM = ws?.Cells[rw, 38]?.Value?.ToString().Trim(),
                        COMMENTS = ws?.Cells[rw, 39]?.Value?.ToString().Trim(),
                        OPTION_CODE = ws?.Cells[rw, 40]?.Value?.ToString().Trim(),
                        KEYNO = ws?.Cells[rw, 41]?.Value?.ToString().Trim(),

                        //PRODUCTION_DATE = Convert.ToDateTime(ws?.Cells[rw, 42]?.Value?.ToString().Trim()).ToString("dd-MMM-yyyy"),
                        PRODUCTION_DATE = DateTime.FromOADate(Convert.ToDouble(ws?.Cells[rw, 42]?.Value?.ToString().Trim())).ToString("dd-MMM-yyyy"),
                        //PRODUCTION_DATE = Convert.ToDateTime(ws?.Cells[rw, 42]?.Value?.ToString().Trim()).ToString("dd-MMM-yyyy"),

                        EXP_TRANSPORT = ws?.Cells[rw, 43]?.Value?.ToString().Trim(),
                        EXP_AGENT_COMMISSION = ws?.Cells[rw, 44]?.Value?.ToString().Trim(),
                        EXP_CUSTOM_DUTY = ws?.Cells[rw, 45]?.Value?.ToString().Trim(),
                        EXP_OTHERS = ws?.Cells[rw, 46]?.Value?.ToString().Trim(),
                        EXP_CC = ws?.Cells[rw, 47]?.Value?.ToString().Trim(),
                        EXP_MAINT = ws?.Cells[rw, 48]?.Value?.ToString().Trim(),
                        EXP_COMM_OTHERS = ws?.Cells[rw, 49]?.Value?.ToString().Trim(),
                        EXP_PAPER = ws?.Cells[rw, 50]?.Value?.ToString().Trim(),

                        OTHER_REF = ws?.Cells[rw, 51]?.Value?.ToString().Trim(),
                        TAX10 = ws?.Cells[rw, 52]?.Value?.ToString().Trim(),
                        AUCTIONFEE = ws?.Cells[rw, 53]?.Value?.ToString().Trim(),
                        REKSO = ws?.Cells[rw, 54]?.Value?.ToString().Trim(),
                        RECYCLE = ws?.Cells[rw, 55]?.Value?.ToString().Trim(),
                        LOADING = ws?.Cells[rw, 56]?.Value?.ToString().Trim(),
                        AUCTIONNAME = ws?.Cells[rw, 57]?.Value?.ToString().Trim(),
                        CONT_NO = ws?.Cells[rw, 58]?.Value?.ToString().Trim(),
                        OTHERS_JP = ws?.Cells[rw, 59]?.Value?.ToString().Trim(),
                        JP_CHARGES = ws?.Cells[rw, 60]?.Value?.ToString().Trim()


                    });
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                return Json(new { message = errorMessage });
            }

            // No save the data in database

            // clear / delete all data from ImportHolder
            _oPurchase.Clear_ImportHolder();

            try
            {
                foreach (var item in listImportHolder)
                {

                    if (Convert.ToDateTime(item.PRODUCTION_DATE).Year < 1901)
                    {
                        item.PRODUCTION_DATE = null;

                    }



                    message = _oPurchase.InsertImportHolder_DAL(item.MAKENAME, item.MAKEMODELNAME, item.CHASSIS_NO, item.MODEL, item.COLORNAME, item.COLOR_INT, item.PRICE, item.PRICE_RATE, item.FREIGHT, item.FR_RATE,
                    item.SELLING_PRICE, item.TRANSMISSION, item.DOOR, item.DRIVE, item.ENGINE_NO, item.WEIGHT, item.HS_CODE, item.ENGINE_POWER, item.MILEAGE, item.VEHICLE_CC, item.USED_NEW, item.OPTIONS, item.FUEL_TYPE, item.MAKECATEGORYNAME, item.VENDORNAME,
                    item.PURCHASEDATE, item.PURCHASE_REF, item.BL_NO, item.SHIP_NAME, item.SHIPDATE, item.LEAVE_DATE, item.ENTRY_DATE, item.BOE, item.LOCATION, item.MAKECOUNTRYNAME, item.AVAILABILITY, item.STOCK_TYPE, item.SHOWROOM,
                    item.COMMENTS, item.OPTION_CODE, item.KEYNO, item.PRODUCTION_DATE, item.EXP_TRANSPORT, item.EXP_AGENT_COMMISSION, item.EXP_CUSTOM_DUTY, item.EXP_OTHERS,
                    item.EXP_CC, item.EXP_MAINT, item.EXP_COMM_OTHERS, item.EXP_PAPER, item.OTHER_REF, item.TAX10, item.AUCTIONFEE, item.REKSO, item.RECYCLE, item.LOADING, item.AUCTIONNAME,
                    item.CONT_NO, item.OTHERS_JP, item.JP_CHARGES, c_ID, item.User_ID);

                    if (message != "Saved Successfully!")
                    {
                        //insert failure chassis if error occured in insert into importHolder table
                        string message2 = _oPurchase.InsertImportFailure_DAL(item.CHASSIS_NO, message);
                        TempData["Error"] = "Following chassis not saved successfully: " + TempData["Error"] + " " + item.CHASSIS_NO;
                        //return Json(new { message = "An error occured in saving data in the database" });
                    }
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


            //Now delete the excel file that you save temprory 
            #region Deleting actual file from folder

            if ((System.IO.File.Exists(filepath)))
            {
                System.IO.File.Delete(filepath);
            }

            #endregion



            //--Return view
            _oPurchase.ImportHolderList = _oPurchase.Get_StockPuchaseBulkList_DAL().ToList();

            return PartialView("_StockBulkUploadPartial", _oPurchase);


        }

        //--insert data from importHolder table into stock table
        [HttpGet]
        public IActionResult InsertBulkFromImportHolder()
        {
            string message = "";
            string Temp_ID = "";
            Temp_ID = BulkUpload_TempID;

            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserID_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }


            int? User_ID = Convert.ToInt32(UserID_Admin);
            int c_ID = Convert.ToInt32(company_ID);


            try
            {
                message = _oPurchase.InsertBulkFromImportHolder_DAL(Temp_ID, Convert.ToInt32(UserID_Admin), c_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = message;
                    return RedirectToAction("UploadStockBulk", "Purchases");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("UploadStockBulk", "Purchases");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("UploadStockBulk", "Purchases");
            }
            //return View();
        }





        public void DumpStaticFieldsPurchase()
        {
            PurchaseNewTempID = null;
            PurchaseMasterNewStatic_ID = null;
            PurchaseOtherTempID = null;
            PurchaseMasterOtherStatic_ID = null;
            PurchaseReturnTempID = null;
            Purchase_Return_Static_ID = null;
            BulkUpload_TempID = null;

            GRVOtherTempID = null;
            GRVMasterOtherStatic_ID = null;
        }


        #region NEW PURCHASE

        //New Purchase:


        [HttpGet]
        public IActionResult NewPurchase(int? PurchaseMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Purchases";
            ViewBag.BreadCumAction = "NewPurchase";

            ViewBag.SectionHeaderTitle = "Purchases";
            ViewBag.SectionSub_HeaderTitle = "Purchase";
            ViewBag.SectionSub_HeaderMainTitle = "Purchase List";
            ViewBag.PageTitle = "Purchase";
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
            //---Make list for drop down in forms
            ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_ID).ToList();
            //---Get Colors List
            ViewBag.ColorsList = _oBasic.Get_ColorsList_DAL().ToList();
            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).Where(z => z.VendorCat_ID == 2).ToList();
            //---Get PurchaseStatus
            ViewBag.PurchaseStatus = _oBasic.Get_StatusList_byType_DAL("Purchase").ToList();

            ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_ID).ToList();

            //---Get Vendor Category List
            ViewBag.VendorCategoryList = _oBasic.Get_VendorCategory_DAL().Where(z => z.VendorCat_ID == 2).ToList();

            //New
            ViewBag.VehicleCategory = _oBasic.Get_VehicleCategoryList_DAL(c_ID).ToList();


            //---Get EngineType
            ViewBag.EngineType = _oBasic.Get_EngineTypeList_DAL(c_ID).ToList();


            //---Get StockType
            ViewBag.StockType = _oBasic.Get_StockTypeList_DAL().ToList();


            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;
            ViewBag.DefaultCurrencyDisplay = configuration.GetSection("AppSettings").GetSection("DefaultCurrencyDisplay").Value;

            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;

            ViewBag.CarCode = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;


            #endregion


            if (PurchaseMaster_ID == null || PurchaseMaster_ID < 1)
            {


                //---Create Purchase master Ref(tem id)
                if (String.IsNullOrEmpty(PurchaseNewTempID))
                {
                    Guid obj = Guid.NewGuid();
                    PurchaseNewTempID = obj.ToString();
                }

                PurchaseMasterNewStatic_ID = PurchaseMaster_ID;

                //---Get purchase master id
                ViewBag.PurchaseMaster_ID = PurchaseMaster_ID;

                //--Get purchase Master
                _oPurchase.purchaseMasterObj = _oPurchase.Get_PurchaseMasterByID_DAL(-1);

                //--Get purchase detail
                _oPurchase.purchaseDetailList = _oPurchase.Get_PurchaseDetailListByID_DAL(PurchaseNewTempID, -1).ToList();
            }
            else
            {
                Urls = null;

                ApprovalURL = HttpContext.Request.Headers["Referer"];
                Urls = ApprovalURL;
                //---Get old temp id from purchase detail table that already creaded for this "PurchaseMaster_ID" id. And then use this if new record added
                string OldTempID = _oPurchase.GetOldTempIDFromPurchaseDetail_DAL(PurchaseMaster_ID);



                PurchaseMasterNewStatic_ID = PurchaseMaster_ID;

                //---Get purchase master id
                ViewBag.PurchaseMaster_ID = PurchaseMaster_ID;

                //--Get purchase Master
                _oPurchase.purchaseMasterObj = _oPurchase.Get_PurchaseMasterByID_DAL(PurchaseMaster_ID);

                //--Get purchase detail
                _oPurchase.purchaseDetailList = _oPurchase.Get_PurchaseDetailListByID_DAL(OldTempID, PurchaseMaster_ID).ToList();
            }



            return View(_oPurchase);
        }



        #endregion

        #region NEW PURCHASE JAPAN
        [HttpGet]
        public ActionResult NewPurchasejp(int? PurchaseMaster_ID)
        {


            //if (System.Web.HttpContext.Current.Session["UserName"] == null)
            //{
            //    return RedirectToAction("logout", "Dashboard");
            //}
            //else
            //{

            //    ViewBag.ddlYear = new SelectList(db.tblModels, "MODEL", "MODEL");
            //    ViewBag.ddlCountry = new SelectList(db.tblmake_country, "make_country_id", "country_name");
            //    ViewBag.GoingLocationList = new SelectList(db.select_LocationByType("GoingLocation").ToList(), "ID", "CAR_LOCATION");
            //    ViewBag.LocationsPurchaseList = new SelectList(db.select_LocationByType("PurchaseLocation").ToList(), "ID", "CAR_LOCATION");
            //    ViewBag.globalLocationCity = new SelectList(db.tblsub_GlobalLocation_city, "id", "sub_globalLocation");
            //    ViewBag.transmissionlist = new SelectList(db.transmissions, "name", "name");
            //    ViewBag.ddlglobalLocation = new SelectList(db.GlobalLocations, "ID", "globalLocation1");
            //    ViewBag.Auction_Vendors = new SelectList(db.select_Vendor_By_Type("Auction").ToList(), "Vendor_ID", "Vendor_Name");
            //    ViewBag.Rikso_Vendors = new SelectList(db.select_Vendor_By_Type("Rikso").ToList(), "Vendor_ID", "Vendor_Name");
            //    ViewBag.ddlSelectStockType = new SelectList(db.tblStockTypes, "stockType", "ID");
            //    ViewBag.ddlGlobalLocation = new SelectList(db.GlobalLocations, "globalLocation", "ID");
            //    ViewBag.ddlMake = new SelectList(db.tblmakes, "make_ID", "make");
            //    ViewBag.ddlVvendor_master = new SelectList(db.vVendors, "Vendor_ID", "Vendor_Name");
            //    ViewBag.ddlModel = new SelectList(db.tblmake_details, "id", "model_description");
            //    ViewBag.ddlColor = new SelectList(db.tblcolors, "color_id", "color");
            // }


            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Purchases";
            ViewBag.BreadCumAction = "NewPurchasejp";

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
            //---Make list for drop down in forms
            ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_ID).ToList();
            //---Get Colors List
            ViewBag.ColorsList = _oBasic.Get_ColorsList_DAL().ToList();
            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList();
            //---Get PurchaseStatus
            ViewBag.PurchaseStatus = _oBasic.Get_StatusList_byType_DAL("Purchase").ToList();
            //---Get Vendor Category List
            ViewBag.VendorCategoryList = _oBasic.Get_VendorCategory_DAL().ToList();

            ViewBag.Auction_Vendors = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList();
            ViewBag.Rikso_Vendors = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList();





            #endregion


            if (PurchaseMaster_ID == null || PurchaseMaster_ID < 1)
            {


                //---Create Purchase master Ref(tem id)
                if (String.IsNullOrEmpty(PurchaseNewTempID))
                {
                    Guid obj = Guid.NewGuid();
                    PurchaseNewTempID = obj.ToString();
                }

                PurchaseMasterNewStatic_ID = PurchaseMaster_ID;

                //---Get purchase master id
                ViewBag.PurchaseMaster_ID = PurchaseMaster_ID;

                //--Get purchase Master
                _oPurchase.purchaseMasterObj = _oPurchase.Get_PurchaseMasterByID_DAL(-1);

                //--Get purchase detail
                _oPurchase.purchaseDetailList = _oPurchase.Get_PurchaseDetailListByID_DAL(PurchaseNewTempID, -1).ToList();
            }
            else
            {

                //---Get old temp id from purchase detail table that already creaded for this "PurchaseMaster_ID" id. And then use this if new record added
                string OldTempID = _oPurchase.GetOldTempIDFromPurchaseDetail_DAL(PurchaseMaster_ID);
                if (!String.IsNullOrEmpty(OldTempID))
                {
                    PurchaseNewTempID = OldTempID;
                }
                else
                {
                    if (String.IsNullOrEmpty(PurchaseNewTempID))
                    {
                        Guid obj = Guid.NewGuid();
                        PurchaseNewTempID = obj.ToString();
                    }
                }


                PurchaseMasterNewStatic_ID = PurchaseMaster_ID;

                //---Get purchase master id
                ViewBag.PurchaseMaster_ID = PurchaseMaster_ID;

                //--Get purchase Master
                _oPurchase.purchaseMasterObj = _oPurchase.Get_PurchaseMasterByID_DAL(PurchaseMaster_ID);

                //--Get purchase detail
                _oPurchase.purchaseDetailList = _oPurchase.Get_PurchaseDetailListByID_DAL(PurchaseNewTempID, PurchaseMaster_ID).ToList();
            }



            return View(_oPurchase);



            //if (id == null || id == "" || String.IsNullOrEmpty(id) || String.IsNullOrWhiteSpace(id))
            //    {


            //        if (ViewBag.AutoV == AutoI)
            //        {
            //            Auto();
            //        }
            //        var Detail = new commonStock();
            //        Detail.PurchaseMasters = dblayar.PurchaseMaster(Convert.ToInt32(SID)).FirstOrDefault();
            //        Detail.PurchaseList = dblayar.Purchase(AutoI);
            //        Detail.PurchaseDetailGrandTotal = dblayar.purchaseDetailsTotal(AutoI).FirstOrDefault();
            //        ViewBag.AutoV = AutoI;
            //        ViewBag.LoadPMID = LoadPMID;
            //        TempData["TotalResult"] = "Please Add Atleast One Stock Detail";
            //        TempData.Keep();
            //        return View(Detail);
            //    }
            //    else
            //    {
            //        LoadID(id);

            //        if (System.Web.HttpContext.Current.Request.QueryString["id"] != null)
            //        {
            //            ViewBag.hID = System.Web.HttpContext.Current.Request.QueryString["id"].ToString();
            //            id = System.Web.HttpContext.Current.Request.QueryString["id"].ToString();
            //        }



            //        var Detail = new commonStock();
            //        Detail.PurchaseMasters = dblayar.PurchaseMaster(LoadPMID).FirstOrDefault();
            //        Detail.PurchaseList = dblayar.Purchase(id);
            //        Detail.PurchaseDetailGrandTotal = dblayar.purchaseDetailsTotal(id).FirstOrDefault();
            //        TempData["TotalResult"] = "Please Add Atleast One Stock Detail";
            //        TempData.Keep();
            //        return View(Detail);

            //    }



        }

        #endregion

        [HttpGet]
        public IActionResult NewPurchaseOrder(int? PurchaseMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Purchases";
            ViewBag.BreadCumAction = "NewPurchaseOrder";

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

            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList();
            //---Get Item master list
            ViewBag.ItemMaster = _oBasic.Get_ItemMasterList_DAL().ToList();
            //---Get Vendor Category List
            ViewBag.VendorCategoryList = _oBasic.Get_VendorCategory_DAL().ToList();
            ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_ID).ToList();

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;


            #endregion


            if (PurchaseMaster_ID == null || PurchaseMaster_ID < 1)
            {

                //---Create Purchase master Ref(tem id)
                if (String.IsNullOrEmpty(PurchaseOtherTempID))
                {
                    Guid obj = Guid.NewGuid();
                    PurchaseOtherTempID = obj.ToString();
                }

                PurchaseMasterOtherStatic_ID = PurchaseMaster_ID;

                //---Get purchase master id
                ViewBag.PurchaseMaster_ID = PurchaseMaster_ID;

                //--Get purchase Master
                _oPurchase.purchaseMasterObj = _oPurchase.Get_PurchaseMaster_OtherByID_DAL(-1);

                //--Get purchase detail 
                _oPurchase.purchaseDetailList = _oPurchase.Get_PurchaseDetailOtherListByID_DAL(PurchaseOtherTempID, -1).ToList();
            }
            else
            {

                Urls = null;

                OtherURL = HttpContext.Request.Headers["Referer"];
                Urls = OtherURL;

                //---Get old temp id from purchase detail table that already creaded for this "PurchaseMaster_ID" id. And then use this if new record added
                string OldTempID = _oPurchase.GetOldTempIDFromPurchaseDetail_DAL(PurchaseMaster_ID);



                PurchaseMasterOtherStatic_ID = PurchaseMaster_ID;

                //---Get purchase master id
                ViewBag.PurchaseMaster_ID = PurchaseMaster_ID;



                //--Get purchase Master
                _oPurchase.purchaseMasterObj = _oPurchase.Get_PurchaseMaster_OtherByID_DAL(PurchaseMaster_ID);

                //--Get purchase detail
                _oPurchase.purchaseDetailList = _oPurchase.Get_PurchaseDetailOtherListByID_DAL(OldTempID, PurchaseMaster_ID).ToList();
            }



            return View(_oPurchase);
        }
        [HttpGet]

        public IActionResult OrderPurchaseList(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID, int? page)
        {
            //---Dumping static fields
            DumpStaticFieldsPurchase();


            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Purchases";
            ViewBag.BreadCumAction = "OrderPurchaseList";


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;
            From_Date = String.IsNullOrEmpty(From_Date) ? "" : From_Date;
            To_Date = String.IsNullOrEmpty(To_Date) ? "" : To_Date;
            Status_ID = Status_ID == null ? 0 : Status_ID;
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == null || c_ID == 0 ? c_IDs : c_ID;

            #region Veiwbags area
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_IDs).ToList();
            //---Get PurchaseStatus
            ViewBag.PurchaseStatus = _oBasic.Get_StatusList_byType_DAL("Purchase").ToList();

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;


            #endregion veiwbags area

            ViewBag.PurchaseRef_PurchaseOther = PurchaseRef;
            ViewBag.Vendor_ID_PurchaseOther = Vendor_ID;
            ViewBag.From_Date_PurchaseOther = From_Date;
            ViewBag.To_Date_PurchaseOther = To_Date;
            ViewBag.Status_ID_PurchaseOther = Status_ID;
            ViewBag.c_ID_PurchaseOther = c_ID;
            ViewBag.RecordsPerPage = 10;
            //---Purchase master other  list for page
            _oPurchase.purchaseOtherMasterList = _oPurchase.Get_PurchaseMasterList_Other_DAL(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, c_ID).ToList().ToPagedList(page ?? 1, 10);


            return View(_oPurchase);
        }


        #region GoodsReceived
        //---ohter purchases get method
        [HttpGet]
        public IActionResult PurchasesReceived(int GRVMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Purchases";
            ViewBag.BreadCumAction = "NewPurchasesReceived";
            ViewBag.SectionHeaderTitle = "Purchases";
            ViewBag.SectionSub_HeaderTitle = "PurchasesReceived";
            ViewBag.SectionSub_HeaderMainTitle = "PurchasesReceived List";
            ViewBag.PageTitle = "PurchasesReceived";
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
            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList();
            //---Get Item master list
            ViewBag.ItemMaster = _oBasic.Get_ItemMasterList_DAL().ToList();
            //---Get Vendor Category List
            ViewBag.VendorCategoryList = _oBasic.Get_VendorCategory_DAL().ToList();
            ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_ID).ToList();
            ViewBag.ItemLocation = _oBasic.Get_ItemLocationList_DAL(c_ID).ToList();
            ViewBag.ItemLocation = _oBasic.Get_ItemLocationList_DAL(c_ID).ToList();
            ViewBag.PurchaseRef = _oPurchase.GetGRVRefDetails_Other_DAL().ToList();

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;

            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;



            #endregion


            if (GRVMaster_ID == 0 || GRVMaster_ID < 1)
            {

                //---Create GRV master Ref(tem id)
                if (String.IsNullOrEmpty(GRVOtherTempID))
                {
                    Guid obj = Guid.NewGuid();
                    GRVOtherTempID = obj.ToString();
                }

                GRVMasterOtherStatic_ID = GRVMaster_ID;

                //---Get GRV master id
                ViewBag.GRVMaster_ID = GRVMaster_ID;

                //--Get GRV Master
                _oPurchase.GRVMasterObj = _oPurchase.Get_ReceivedMaster_ByID_DAL(-1);

                //--Get GRV detail 
                _oPurchase.GRVDetailList = _oPurchase.Get_ReceivedDetailListByID_DAL(GRVOtherTempID, -1).ToList();
            }
            else
            {

                Urls = null;

                OtherURL = HttpContext.Request.Headers["Referer"];
                Urls = OtherURL;

                //---Get old temp id from GRV detail table that already creaded for this "GRVMaster_ID" id. And then use this if new record added
                string OldTempID = _oPurchase.GetOldTempIDFromGoodsReceivedDetail_DAL(GRVMaster_ID);



                GRVMasterOtherStatic_ID = GRVMaster_ID;

                //---Get GRV master id
                ViewBag.GRVMaster_ID = GRVMaster_ID;



                //--Get GRV Master
                _oPurchase.GRVMasterObj = _oPurchase.Get_ReceivedMaster_ByID_DAL(GRVMaster_ID);

                //--Get GRV detail
                _oPurchase.GRVDetailList = _oPurchase.Get_ReceivedDetailListByID_DAL(OldTempID, GRVMaster_ID).ToList();
            }



            return View(_oPurchase);
        }

        //---Getting Other GRV list
        [HttpGet]

        public IActionResult PurchasesReceivedList(string GRVRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID, int? page)
        {
            //---Dumping static fields
            DumpStaticFieldsPurchase();


            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Purchases";
            ViewBag.BreadCumAction = "PurchasesReceivedList";
            ViewBag.SectionHeaderTitle = "Purchases";
            ViewBag.SectionSub_HeaderTitle = "Other GRV List";
            ViewBag.PageTitle = "Other GRV List";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            GRVRef = String.IsNullOrEmpty(GRVRef) ? "" : GRVRef;
            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;
            From_Date = String.IsNullOrEmpty(From_Date) ? "" : From_Date;
            To_Date = String.IsNullOrEmpty(To_Date) ? "" : To_Date;
            Status_ID = Status_ID == null ? 0 : Status_ID;
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == null || c_ID == 0 ? c_IDs : c_ID;

            #region Veiwbags area
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_IDs).ToList();
            //---Get Purchasestatus
            ViewBag.Purchasestatus = _oBasic.Get_StatusList_byType_DAL("GRV").ToList();

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;


            #endregion veiwbags area

            ViewBag.GRVRef_GRVOther = GRVRef;
            ViewBag.Vendor_ID_GRVOther = Vendor_ID;
            ViewBag.From_Date_GRVOther = From_Date;
            ViewBag.To_Date_GRVOther = To_Date;
            ViewBag.Status_ID_GRVOther = Status_ID;
            ViewBag.c_ID_GRVOther = c_ID;

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            ViewBag.RecordsPerPage = 10;
            //---GRV master other  list for page
            _oPurchase.GRVOtherMasterList = _oPurchase.Get_GoodsReceivedMaster_DAL(GRVRef, Vendor_ID, From_Date, To_Date, Status_ID, c_ID).ToList().ToPagedList(page ?? 1, 10);


            return View(_oPurchase);
        }

        //---This method is for inserting other GRV detail record in GRV detail table
   
        [HttpPost]
        public IActionResult InsertPurchasesReceivedDetail(int?[] item_ID, string[] item_Description, double?[] Quantity, string[] UOM, int?[] Currency_ID, double?[] Currency_Rate, double?[] Unit_Price, int?[] VAT_Rate,
           double?[] VAT_Exp, double?[] Discount, double?[] Amount, double?[] TtlAmount, double?[] Amount_Others, int hdGRVMaster_ID, int[] Location_ID, int[] PurchaseDetails_ID, int[] PurchaseMaster_Id)
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


            if (Quantity == null )
            {
                return Json(new { message = "Please enter Received Qty!" });
            }
         



            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = GRVOtherTempID;
        
            if (hdGRVMaster_ID == 0)
            {
                Temp_ID = GRVOtherTempID;
            }
            else
            {
                Temp_ID = "0";
            }

            GRVMasterOtherStatic_ID = GRVMasterOtherStatic_ID == null ? -1 : GRVMasterOtherStatic_ID;

            try
            {

                if (PurchaseDetails_ID != null && PurchaseDetails_ID.Length != 0)
                {
                   
                    for (int i = 0; i < PurchaseDetails_ID.Length; i++)
                    {
                    
                        message = _oPurchase.InsertGoodsReceivedDetail_DAL(item_ID[i], item_Description[i], Quantity[i], UOM[i], Currency_ID[i], Currency_Rate[i], Unit_Price[i], VAT_Rate[i],
                        VAT_Exp[i], Discount[i], Amount[i], TtlAmount[i], Amount_Others[i], Temp_ID, c_ID, Created_by, hdGRVMaster_ID, Location_ID[i], PurchaseDetails_ID[i], PurchaseMaster_Id[i]);

                    }

                }
                else
                {
                    message = "Please Check Any One to perform this task";
                }




                if (message.Contains("Successfully"))
                {
                    //---Inventory detial invoice list for Inventory invoice type Bychassis
                    _oPurchase.GRVDetailList = _oPurchase.Get_ReceivedDetailListByID_DAL(Temp_ID, hdGRVMaster_ID).ToList();

                    return PartialView("_GRVDetail_Other_Partial", _oPurchase);
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




        //---This method is for Updating other GRV detail record in GRV detail table
        [HttpPost]
        public IActionResult UpdatePurchasesReceivedDetail(int? GRVDetails_ID, int? item_ID, string item_Description, double? Quantity, string UOM, int? Currency_ID, double? Currency_Rate, double? Unit_Price, int? VAT_Rate,
           double? VAT_Exp, double? Discount, double? Amount, double? TtlAmount, double? Amount_Others, int hdGRVMaster_ID, int Location_ID, string Serial, string Barcode)
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
            Temp_ID = GRVOtherTempID;
            if (hdGRVMaster_ID == 0)
            {
                Temp_ID = GRVOtherTempID;
            }
            else
            {
                Temp_ID = "0";
            }
            GRVMasterOtherStatic_ID = GRVMasterOtherStatic_ID == null ? -1 : GRVMasterOtherStatic_ID;
            if (item_Description == null || Quantity == null ||
                 Unit_Price == null || Amount == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }
            item_ID = item_ID == null ? 0 : item_ID;
            Currency_ID = Currency_ID == null ? 0 : Currency_ID;
            Currency_Rate = Currency_Rate == null ? 0 : Currency_Rate;
            UOM = String.IsNullOrEmpty(UOM) ? "" : UOM;
            VAT_Exp = VAT_Exp == null ? 0 : VAT_Exp;
            VAT_Rate = VAT_Exp == null ? 0 : VAT_Rate;
            Discount = Discount == null ? 0 : Discount;
            try
            {
                message = _oPurchase.UpdateGoodsReceivedDetail_DAL(GRVDetails_ID, item_ID, item_Description, Quantity, UOM, Currency_ID, Currency_Rate, Unit_Price, VAT_Rate,
            VAT_Exp, Discount, Amount, TtlAmount, Amount_Others, Temp_ID, c_ID, Modified_by, Location_ID, Serial, Barcode);
                if (message == "Updated Successfully!")
                {
                    _oPurchase.GRVDetailList = _oPurchase.Get_ReceivedDetailListByID_DAL(Temp_ID, hdGRVMaster_ID).ToList();
                    return PartialView("_GRVDetail_Other_Partial", _oPurchase);
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


        //This method is for inserting New GRV Master
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePurchasesReceivedMaster(int? GRVMaster_ID, int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string GRVDate, int PurchaseMaster_ID)
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
            int? Modified_by = Convert.ToInt32(UserID_Admin);

            Temp_ID = GRVOtherTempID;
            string OldTempID = _oPurchase.GetOldTempIDFromGoodsReceivedDetail_DAL(GRVMaster_ID);
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


            if (Vendor_ID == null || String.IsNullOrEmpty(GRVDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            if (GRVMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again!";
                return Redirect(url);
            }



            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;


            try
            {
                message = _oPurchase.UpdateGoodsReceivedMaster_DAL(GRVMaster_ID, Vendor_ID, Vendor_PruchaseTo, Vendor_Address, GRVDate, Temp_ID, c_ID, Modified_by,  PurchaseMaster_ID);
                if (message.Contains("Successfully"))
                {

                    //TempData["Success"] = "Updated Successfully!";
                    return RedirectToAction("PurchasesReceivedList", "Purchases");
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


        //This method is for inserting other GRV Master
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertPurchasesReceivedMaster(int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string GRVDate,int PurchaseMaster_ID)
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
            Temp_ID = GRVOtherTempID;
            string GRVType = "PO";  //---It is the ref in table for new GRV. Send this value hard coded from here

            if (Vendor_ID == null || String.IsNullOrEmpty(GRVDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("PurchasesReceived", "Purchases");
            }

            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;


            try
            {
                message = _oPurchase.InsertGoodsReceivedMaster_DAL(Vendor_ID, Vendor_PruchaseTo, Vendor_Address, GRVType, GRVDate, Temp_ID, c_ID, Created_by,PurchaseMaster_ID);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("PurchasesReceivedList", "Purchases");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("PurchasesReceived", "Purchases");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("PurchasesReceived", "Purchases");
            }



        }

        [HttpPost]
        public IActionResult DeleteGRVMaster_Other(int? GRVMaster_ID)
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

            if (GRVMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oPurchase.DeleteGoodsReceivedMaster_DAL(GRVMaster_ID);
                if (message.Contains("Successfully"))
                {
                    return RedirectToAction("PurchasesReceivedList", "Purchases");
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
        //---Delete GRV detail other
        [HttpPost]
        public IActionResult DeleteGRVDetailOther(int? GRVDetails_ID)
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
            Temp_ID = GRVOtherTempID;
            string OldTempID = _oPurchase.GetOldTempIDFromGoodsReceivedDetail_DAL(GRVMasterOtherStatic_ID);
            if (!String.IsNullOrEmpty(GRVOtherTempID))
            {
                Temp_ID = GRVOtherTempID;
            }
            else
            {
                Temp_ID = OldTempID;
            }
            GRVMasterOtherStatic_ID = GRVMasterOtherStatic_ID == null ? -1 : GRVMasterOtherStatic_ID;

            if (GRVDetails_ID == null)
            {
                return Json(new { message = "ID is null. Please try again!" });
            }

            try
            {
                message = _oPurchase.DeleteGoodsReceived_Material_In_DAL(GRVDetails_ID);
                if (message == "Deleted Successfully!")
                {
                    _oPurchase.GRVDetailList = _oPurchase.Get_ReceivedDetailListByID_DAL(Temp_ID, GRVMasterOtherStatic_ID).ToList();

                    return PartialView("_GRVDetail_Other_Partial", _oPurchase);
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

        //---Get Othes GRV list by search filters
        [HttpGet]
        public IActionResult GetGRVList_OthersBySearchFitlers(string GRVRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID, int? page)
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

            GRVRef = String.IsNullOrEmpty(GRVRef) ? "" : GRVRef;
            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;
            From_Date = String.IsNullOrEmpty(From_Date) ? "" : From_Date;
            To_Date = String.IsNullOrEmpty(To_Date) ? "" : To_Date;
            Status_ID = Status_ID == null ? 0 : Status_ID;
            c_ID = c_ID == null ? 1 : c_ID;


            ViewBag.GRVRef_GRVOther = GRVRef;
            ViewBag.Vendor_ID_GRVOther = Vendor_ID;
            ViewBag.From_Date_GRVOther = From_Date;
            ViewBag.To_Date_GRVOther = To_Date;
            ViewBag.Status_ID_GRVOther = Status_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.c_ID_GRVOther = c_ID;
            try
            {
                ViewBag.RecordsPerPage = 10;
                _oPurchase.GRVOtherMasterList = _oPurchase.Get_GoodsReceivedMaster_DAL(GRVRef, Vendor_ID, From_Date, To_Date, Status_ID, c_ID).ToList().ToPagedList(page ?? 1, 10);
                return PartialView("_GRVOtherListSearchPartial", _oPurchase);
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }
        [HttpGet]
        public IActionResult Get_PurchaseRefDetails(int PurchaseMaster_ID)
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
                _oPurchase.PurchaseDetailList = _oPurchase.Get_PurchaseRefDetails_DAL(PurchaseMaster_ID).ToList();
                _oPurchase.GRVMasterObj = _oPurchase.Get_ReceivedMaster_ByID_DAL(-1);

                return PartialView("_GRVDetailList", _oPurchase);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }
        [HttpGet]
        public JsonResult GetPurchaseRefDetail(int PurchaseMaster_ID)
        {
            int? Vendor_ID = 0;
            string Vendor_PruchaseTo = "";
            string Vendor_Address = "";
            string PurchaseRef = "";
            string PurchaseDate = "";


            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function
            _oPurchase.purchaseMasterObj = _oPurchase.Get_PurchaseMaster_OtherByID_DAL(PurchaseMaster_ID);

            if (_oPurchase.purchaseMasterObj.Vendor_ID>0 && _oPurchase.purchaseMasterObj.Vendor_ID != null)
            {

                Vendor_ID = _oPurchase.purchaseMasterObj.Vendor_ID;
                Vendor_PruchaseTo = _oPurchase.purchaseMasterObj.Vendor_PruchaseTo;
                Vendor_Address = _oPurchase.purchaseMasterObj.Vendor_Address;
                PurchaseRef = _oPurchase.purchaseMasterObj.PurchaseRef;
                PurchaseDate = _oPurchase.purchaseMasterObj.PurchaseDate;
        

            }
            else
            {
                Vendor_ID = _oPurchase.purchaseMasterObj.Vendor_ID;
                Vendor_PruchaseTo = _oPurchase.purchaseMasterObj.Vendor_PruchaseTo;
                Vendor_Address = _oPurchase.purchaseMasterObj.Vendor_Address;
                PurchaseRef = _oPurchase.purchaseMasterObj.PurchaseRef;
                PurchaseDate = _oPurchase.purchaseMasterObj.PurchaseDate;
            }


            return Json(new { Vendor_ID, Vendor_PruchaseTo, Vendor_Address, PurchaseRef , PurchaseDate });

        }
        #endregion

    }
}
