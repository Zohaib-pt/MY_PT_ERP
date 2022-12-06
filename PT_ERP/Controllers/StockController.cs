using DAL.Models;
using DAL.oClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using X.PagedList;


namespace PT_ERP.Controllers
{
    public class StockController : BaseController
    {

        #region
        //---Static Fields for creating new vanning
        static string VanningNewTempID;
        static int? VanningMasterNewStatic_ID;

        static string Shipping_infoNewTempID;
        static int? Shipping_infoMasterNewStatic_ID;

        static string PurchaseMasterJPMaster_TempID;
        static int? PurchaseMasterJPStatic_ID;
        static string AuctionMaster_TempID;
        static int? AuctionMasterStatic_ID;


        //Creating For BL    1-7-2022   BY Yaseen
        static string BLMaster_TempID;
        static int? BLMasterStatic_ID;



        //Creating For Garage    1-14-2022   BY Yaseen
        static string  GarageMaster_TempID;
        static int?     GarageMasterStatic_ID;


        static string ApprovalURL;
        static string Urls;


        #endregion
        //---Static Fields for creating new Stock


        private readonly IOBasicData _oBasic;
        private readonly IOStock _oStock;
        private IConfiguration configuration;


        public StockController(IOBasicData oBasicBase, IOStock oStock, IConfiguration iConfig)
           : base(oBasicBase)
        {
            _oBasic = oBasicBase;
            _oStock = oStock;
            configuration = iConfig;

        }


        public IActionResult Index()
        {
            return View();
        }


        //-- by Rafay : Start Date : 16/01/2021
        [HttpGet]
        public IActionResult VehicleListwithShaken()
        {
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
            ViewBag.BreadCumController = "Purchases";
            ViewBag.BreadCumAction = "VehicleListwithShaken";
            ViewBag.PageTitle = "Vehicle List with Shaken";

            #region Veiwbags area
            ViewBag.DR_AccountsList = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();
            ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_ID).ToList();
            #endregion veiwbags area



            string chassis_No = HttpContext.Request.Query["txtChassis"].ToString();
            string StartDate = HttpContext.Request.Query["StartDate"].ToString();
            string EndDate = HttpContext.Request.Query["EndDate"].ToString();

            ViewBag.chassis_No = chassis_No;
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            _oStock.ShakeenList = _oStock.get_Shaken_List(chassis_No, StartDate, EndDate);

            return View(_oStock);
        }

        //-- by Rafay : Start Date : 16/01/2021





        //---Getting purchase list
        [HttpGet]

        public IActionResult StockList(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
            string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID, int? page)
        {
            //---Dumping static fields


            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "StockList";


            ViewBag.SectionHeaderTitle = "Stock";
            ViewBag.SectionSub_HeaderTitle = "Stock List";
            ViewBag.PageTitle = "Stock List";


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
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_IDs).Where(x => x.VendorCat_ID == 2).ToList();
            
            
            //---Make list for drop down in forms
            ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_IDs).ToList();
            //---Get Colors List
            ViewBag.ColorsList = _oBasic.Get_ColorsList_DAL().ToList();
            //---Get Location List
            ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_IDs).ToList();
            //---Get Status List
            ViewBag.Status = _oStock.get_Stock_Status_DAL().ToList();

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.AttachmentVisibility_Only_Stock = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility_Only_Stock").Value;
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;

            #endregion veiwbags area


            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;

            c_ID = c_ID == 0 ? c_IDs : c_ID;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            BL_NO = String.IsNullOrEmpty(BL_NO) ? "" : BL_NO;
            BOE = String.IsNullOrEmpty(BOE) ? "" : BOE;
            loc_ID = loc_ID == null ? 0 : loc_ID;
            Stock_Status = String.IsNullOrEmpty(Stock_Status) ? "" : Stock_Status;
            VendorName = String.IsNullOrEmpty(VendorName) ? "" : VendorName;
            ModelYear = String.IsNullOrEmpty(ModelYear) ? "" : ModelYear;
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;
            StockType_ID = StockType_ID == null ? 0 : StockType_ID;

            ViewBag.ChassisNo_StockList = ChassisNo;
            ViewBag.Make_ID_StockList = Make_ID;
            ViewBag.MakeModel_description_ID_StockList = MakeModel_description_ID;
            ViewBag.Color_Exterior_ID_StockList = Color_Exterior_ID;
            ViewBag.PurchaseStartDate_StockList = PurchaseStartDate;
            ViewBag.PurchaseEndDate_StockList = PurchaseEndDate;
            ViewBag.BL_NO_StockList = BL_NO;
            ViewBag.BOE_StockList = BOE;
            ViewBag.PurchaseRef_StockList = PurchaseRef;
            ViewBag.SaleRef_StockList = SaleRef;
            ViewBag.c_ID_StockList = c_ID;
            ViewBag.Stock_Status_StockList = Stock_Status;
            ViewBag.loc_ID_StockList = loc_ID;
            ViewBag.VendorName_StockList = VendorName;
            ViewBag.ModelYear_StockList = ModelYear;
            ViewBag.Container_No_StockList = Container_No;
            ViewBag.StockType_ID_StockList = StockType_ID;

            //---list for page
            _oStock.StockListPagedObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList().ToPagedList(page ?? 1, 10);

            _oStock.StockList_TTL = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();


            _oStock.StockListStats = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);


            return View(_oStock);
        }

        //---Get purchase list by search filters
        [HttpGet]
        public IActionResult StockListBySearchFilers(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
            string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID, int? page)
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

            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;
            c_ID = c_ID == 0 ? 1 : c_ID;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            BL_NO = String.IsNullOrEmpty(BL_NO) ? "" : BL_NO;
            BOE = String.IsNullOrEmpty(BOE) ? "" : BOE;
            loc_ID = loc_ID == null ? 0 : loc_ID;
            Stock_Status = String.IsNullOrEmpty(Stock_Status) ? "" : Stock_Status;
            VendorName = String.IsNullOrEmpty(VendorName) ? "" : VendorName;
            ModelYear = String.IsNullOrEmpty(ModelYear) ? "" : ModelYear;
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;
            StockType_ID = StockType_ID == null ? 0 : StockType_ID;


            ViewBag.ChassisNo_StockList = ChassisNo;
            ViewBag.Make_ID_StockList = Make_ID;
            ViewBag.MakeModel_description_ID_StockList = MakeModel_description_ID;
            ViewBag.Color_Exterior_ID_StockList = Color_Exterior_ID;
            ViewBag.PurchaseStartDate_StockList = PurchaseStartDate;
            ViewBag.PurchaseEndDate_StockList = PurchaseEndDate;
            ViewBag.BL_NO_StockList = BL_NO;
            ViewBag.BOE_StockList = BOE;
            ViewBag.PurchaseRef_StockList = PurchaseRef;
            ViewBag.SaleRef_StockList = SaleRef;
            ViewBag.c_ID_StockList = c_ID;
            ViewBag.Stock_Status_StockList = Stock_Status;
            ViewBag.loc_ID_StockList = loc_ID;
            ViewBag.VendorName_StockList = VendorName;
            ViewBag.ModelYear_StockList = ModelYear;
            ViewBag.Container_No_StockList = Container_No;
            ViewBag.StockType_ID_StockList = StockType_ID;

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.AttachmentVisibility_Only_Stock = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility_Only_Stock").Value;
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;

            try
            {
                //  _oStock.StockListObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, c_ID);

                _oStock.StockListPagedObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList().ToPagedList(page ?? 1, 10);

                _oStock.StockList_TTL = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();


                _oStock.StockListStats = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);


                return PartialView("_StockListSearchPartial", _oStock);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }

        [HttpGet]
        public IActionResult AddNewStock(int? Stock_ID)
        {

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            #region ViewBag Area
            //---Make list for drop down in forms
            ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_ID).ToList();

            _oBasic.ModelDescList = _oBasic.Get_ModelList_DAL(c_ID).ToList();


            //---Get Colors List
            ViewBag.ColorsList = _oBasic.Get_ColorsList_DAL().ToList();
            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList();
            //---Get Vehicle Category
            ViewBag.VehicleCategory = _oBasic.Get_VehicleCategoryList_DAL(c_ID).ToList();
            //---Get Country
            ViewBag.MakeCountryList = _oBasic.Get_MakeCountryList_DAL(c_ID).ToList();
            //---Get Car Locations
            ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_ID).ToList();
            //---Get Showroom
            ViewBag.Showroom = _oBasic.Get_ShowRoom_DAL(c_ID).ToList();
            //---Get Destinations
            ViewBag.Destinations = _oBasic.Get_Destinations_DAL(c_ID).ToList();
            //---Get Port of Exit
            ViewBag.PortofExit = _oBasic.Get_PortOfExit_DAL(c_ID).ToList();
            //---Get EngineType
            ViewBag.EngineType = _oBasic.Get_EngineTypeList_DAL(c_ID).ToList();
           
            //---Get StockType
            ViewBag.StockType = _oBasic.Get_StockTypeList_DAL().ToList();
            ViewBag.ExportDestinations = _oBasic.Get_CarLocation_DAL(c_ID).Where(x => x.LocType == "Destination").ToList();

            ViewBag.PortFrom = _oBasic.Get_CarLocation_DAL(c_ID).Where(x => x.LocType == "PortFrom").ToList();
      
            ViewBag.PortTo = _oBasic.Get_CarLocation_DAL(c_ID).Where(x => x.LocType == "PortTo").ToList();
            ViewBag.DefaultCurrencyDisplay = configuration.GetSection("AppSettings").GetSection("DefaultCurrencyDisplay").Value;

            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;

            string IsJapanSpecsEnabled = configuration.GetSection("AppSettings").GetSection("IsJapanSpecsEnabled").Value;
            
            
            
            if (IsJapanSpecsEnabled == "false")
            {
                ViewBag.JapanSpecsVisible = "hidden";
                ViewBag.JapanSpecsdisabled = "";

                ViewBag.IsDubaiSpecsEnabled = "";
                ViewBag.Japan_Specs_for_Dubai_Company_Enabled = "";
            }
            else
            {
                ViewBag.JapanSpecsVisible = "";
                ViewBag.IsDubaiSpecsEnabled = "hidden";
                ViewBag.JapanSpecsdisabled = "readonly";
                ViewBag.Japan_Specs_for_Dubai_Company_Enabled = "hidden";
            }

            ViewBag.ShowAds_Link = configuration.GetSection("AppSettings").GetSection("ShowAds_Link").Value;



            #endregion
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "StockList";

            ViewBag.SectionHeaderTitle = "Stock";
            ViewBag.SectionSub_HeaderTitle = "New Stock";
            ViewBag.PageTitle = "Add New Stock";


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");




            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (Stock_ID == null || Stock_ID < 1)
            {




                //---Get purchase master id
                ViewBag.Stock_ID = 0;

                //--Get Stock
                _oStock.StockObject = _oStock.Get_Select_Stock_by_ID_DAL(0);

                _oStock.objStock_Exp_Details = _oStock.get_veh_expense_by_Stock_ID(0);

                ViewBag.ModelDesc = _oBasic.ModelDescList.Where(m => (m.Make_ID == (Convert.ToInt32(0)))).ToList();



            }
            else
            {

                ViewBag.Stock_ID = Stock_ID;

                // Stock details by Stock ID
                _oStock.StockObject = _oStock.Get_Select_Stock_by_ID_DAL(Stock_ID);
                _oStock.objStock_Exp_Details = _oStock.get_veh_expense_by_Stock_ID(Stock_ID);

                ViewBag.ModelDesc = _oBasic.ModelDescList.Where(m => (m.Make_ID == (Convert.ToInt32(_oStock.StockObject.Make_ID)))).ToList();

                _oStock.attachmentList = _oStock.GetStockMasterNew_Attachments_DAL(Stock_ID, "StockNew").ToList();
            }



            return View(_oStock);
        }



        [HttpPost]
        public IActionResult ChangeStockStatus(int Stock_ID, int Status_ID)
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
            int Modified_by = Convert.ToInt32(UserID_Admin);
            int c_ID = Convert.ToInt32(company_ID);

            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();


            if (Stock_ID == 0 || Status_ID == 0)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect("/Stock/" + configuration.GetSection("AppSettings").GetSection("IsStockList_Name").Value + "");
            }

            try
            {
                message = _oStock.Update_StockStatus_DAL(Stock_ID, Status_ID, Modified_by, c_ID);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = message;
                    return Redirect("/Stock/" + configuration.GetSection("AppSettings").GetSection("IsStockList_Name").Value + "");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect("/Stock/" + configuration.GetSection("AppSettings").GetSection("IsStockList_Name").Value + "");
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect("/Stock/" + configuration.GetSection("AppSettings").GetSection("IsStockList_Name").Value + "");
            }

        }
        [HttpPost]
        public IActionResult DeleteStockMaster(int? Stock_ID)
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

            if (Stock_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }



            try
            {
                message = _oStock.DeleteStockMaster_DAL(Stock_ID, Convert.ToInt32(UserID_Admin));
                if (message.Contains("SUCCESSFULLY"))
                {
                    return RedirectToAction("" + configuration.GetSection("AppSettings").GetSection("IsStockList_Name").Value + "", "Stock");
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

        //This method is for Upload Stock
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStock(string CHASSIS_NO, int loc_ID, int Showroom_ID, string vehicle_CC,
            int StockType_ID, int User_ID, string BL_NO, string ShipName, string Arrival_Date, string Leave_Date, int Vendor_ID, string PurchaseDate,
            string PurchaseRef, string Comments, string ModelYear, int color_exterior_ID, string Transmission, string Door, int make_ID,
            int MakeModel_description_ID, int Make_category_ID, string OPTIONS, string HS_CODE, string DRIVE, string FUEL_TYPE, string EngineType, string USED_NEW,
            int make_country_ID, string Engine_No, string GCC_Specs, string mileage, string WEIGHT, string seat, int color_interior_ID,
            string intGrade, string intRemarks, string extGrade, string extRemarks, string Option_Code, string KeyNo,
            string PriceOrignal, string PriceRate,
            string FreightCharges, string FreightRate, string AuctionFee, string PlatNumberFee, string ReksoFee, string RecycleFee,
            string CC_Exp,string Exp_Custom_Duty, string ME_Exp, string Paper_Exp, string Transport_Exp,
            string Comm_Exp, string AgentComm_Exp, string OtherCharges_Exp, string VAT_Exp,
            string SellingPrice, string txtBOE1,
            int Stock_ID, string ManufacturingDate, string AD_Link,string Vanning_Charges, string OtherCharges, string JP_Charges,string PriceTax,string LotNumber)
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

            int? Modified_by = Convert.ToInt32(UserID_Admin);


            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();

            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;



            try
            {
                message = _oStock.Update_Stock_DAL(CHASSIS_NO, loc_ID, Showroom_ID, vehicle_CC, StockType_ID, User_ID, BL_NO, ShipName, Arrival_Date,
                      Leave_Date, Vendor_ID, PurchaseDate, PurchaseRef, Comments, ModelYear, color_exterior_ID, Transmission, Door, make_ID,
              MakeModel_description_ID, Make_category_ID, OPTIONS, HS_CODE, DRIVE, FUEL_TYPE, EngineType, USED_NEW,
              make_country_ID, Engine_No, GCC_Specs, mileage, WEIGHT, seat, color_interior_ID, intGrade, intRemarks, extGrade, extRemarks, Option_Code,
              KeyNo, PriceOrignal, PriceRate, FreightCharges, FreightRate, AuctionFee, PlatNumberFee, ReksoFee, RecycleFee,
              CC_Exp,Exp_Custom_Duty, ME_Exp, Paper_Exp, Transport_Exp, Comm_Exp, AgentComm_Exp,
              OtherCharges_Exp, VAT_Exp, ManufacturingDate, SellingPrice, txtBOE1, Stock_ID, AD_Link
              ,  Vanning_Charges,  OtherCharges,  JP_Charges,  PriceTax,LotNumber);


                if (message == "Saved Successfully!")
                {

                    TempData["Success"] = "Updated Successfully!";
                    //  return RedirectToAction("?Stock_ID=" + Stock_ID.ToString(), "",);
                    return RedirectToAction("AddNewStock", "Stock", new { Stock_ID = Stock_ID });

                }
                else
                {


                    TempData["Error"] = "Error! " + message;
                    return Redirect(url);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
                return Redirect(url);
            }



        }

        //This method is for inserting Stock
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertStock(string CHASSIS_NO, int loc_ID, int Showroom_ID, string vehicle_CC,
            int StockType_ID, int User_ID, string BL_NO, string ShipName, string Arrival_Date, string Leave_Date, int Vendor_ID, string PurchaseDate,
            string PurchaseRef, string Comments, string ModelYear, int color_exterior_ID, string Transmission, string Door, int make_ID,
            int MakeModel_description_ID, int Make_category_ID, string OPTIONS, string HS_CODE, string DRIVE, string FUEL_TYPE, string EngineType, string USED_NEW,
            int make_country_ID, string Engine_No, string GCC_Specs, string mileage, string WEIGHT, string seat, int color_interior_ID,
            string intGrade, string intRemarks, string extGrade, string extRemarks, string Option_Code, string KeyNo,
            string PriceOrignal, string PriceRate,
            string FreightCharges, string FreightRate, string AuctionFee, string PlatNumberFee, string ReksoFee, string RecycleFee,
            string CC_Exp,string Exp_Custom_Duty, string ME_Exp, string Paper_Exp, string Transport_Exp,
            string Comm_Exp, string AgentComm_Exp, string OtherCharges_Exp, string VAT_Exp,
            string ManufacturingDate, string SellingPrice, string txtBOE1, string Vanning_Charges, string OtherCharges, string JP_Charges, string PriceTax)
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

            int? Created_by = Convert.ToInt32(UserID_Admin);
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            int? c_ID = c_IDs;



            try
            {
                message = _oStock.Insert_Stock_DAL(CHASSIS_NO, loc_ID, Showroom_ID, vehicle_CC, StockType_ID, User_ID, BL_NO, ShipName, Arrival_Date,
                    Leave_Date, Vendor_ID, PurchaseDate, PurchaseRef, Comments, ModelYear, color_exterior_ID, Transmission, Door, make_ID,
            MakeModel_description_ID, Make_category_ID, OPTIONS, HS_CODE, DRIVE, FUEL_TYPE, EngineType, USED_NEW,
            make_country_ID, Engine_No, GCC_Specs, mileage, WEIGHT, seat, color_interior_ID, intGrade, intRemarks, extGrade, extRemarks, Option_Code,
            KeyNo, PriceOrignal, PriceRate, FreightCharges, FreightRate, AuctionFee, PlatNumberFee, ReksoFee, RecycleFee,
            CC_Exp, Exp_Custom_Duty, ME_Exp, Paper_Exp, Transport_Exp, Comm_Exp, AgentComm_Exp,
            OtherCharges_Exp, VAT_Exp, ManufacturingDate, SellingPrice, txtBOE1, c_ID,  Vanning_Charges,  OtherCharges,  JP_Charges,  PriceTax);



                if (message == "Saved Successfully!")
                {

                    TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("AddNewStock", "Stock");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("AddNewStock", "Stock");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
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

        [HttpGet]
        public IActionResult MultilpleSoldDelivered()
        {


            #region ViewBag Area
            //---Ven Category list for drop down in forms

            #endregion
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Delivered";
            ViewBag.BreadCumAction = "DeliveredList";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            //---Partners list for page



            return View(_oStock);
        }
        [HttpGet]
        public IActionResult MultilpleSoldDeliveredLIST()
        {


            #region ViewBag Area
            //---Ven Category list for drop down in forms

            #endregion
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "MultilpleSoldDeliveredLIST";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            //---Partners list for page



            return View(_oStock);
        }


        [HttpGet]
        public IActionResult BulkUpdateStock()
        {


            #region ViewBag Area
            //---Ven Category list for drop down in forms

            #endregion
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "BulkUpdateStock";
            ViewBag.PageTitle = "Bulk Update Stock";
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

        #region Vanning
        //---new Vanning page
        [HttpGet]
        public IActionResult NewVanning(int? vanning_Master_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "NewVanning";

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


            //---Get Vendor Master
            // ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL().ToList();
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).Where(x => x.VendorCat_ID == 5).ToList(); // vanning category is 5



            //---Get Vendor Category List
            ViewBag.VendorCategoryList = _oBasic.Get_VendorCategory_DAL().ToList();

            ViewBag.SaleRefList = _oStock.Get_pa_Sale_SaleRef_DAL().ToList();

            #endregion


            if (vanning_Master_ID == null || vanning_Master_ID < 1)
            {


                //---Create Vanning master Ref(tem id)
                if (String.IsNullOrEmpty(VanningNewTempID))
                {
                    Guid obj = Guid.NewGuid();
                    VanningNewTempID = obj.ToString();
                }
                ViewBag.VanningRef = VanningNewTempID;
                VanningMasterNewStatic_ID = vanning_Master_ID;

                //---Get Vanning master id
                ViewBag.vanning_master_ID = vanning_Master_ID;

                //--Get Vanning Master
                _oStock.VanningMasterObj = _oStock.get_Vanning_Master_by_tempID_ID("", "0");

                //--Get Vanning detail
                _oStock.VanningDetailList = _oStock.get_Vanning_Details_by_TempID_ID(VanningNewTempID, "0").ToList();
            }
            else
            {

                //---Get old temp id from Vanning detail table that already creaded for this "vanning_master_ID" id. And then use this if new record added
                string OldTempID = _oStock.GetOldTempIDFromVanningDetail_DAL(vanning_Master_ID);



                VanningMasterNewStatic_ID = vanning_Master_ID;

                //---Get Vanning master id
                ViewBag.vanning_master_ID = vanning_Master_ID;
                ViewBag.SaleRefList = _oStock.Get_pa_Sale_SaleRef_DAL().ToList();
                //--Get Vanning Master
                _oStock.VanningMasterObj = _oStock.get_Vanning_Master_by_tempID_ID("", vanning_Master_ID.ToString());

                //--Get Vanning detail
                _oStock.VanningDetailList = _oStock.get_Vanning_Details_by_TempID_ID(OldTempID, vanning_Master_ID.ToString()).ToList();
            }



            return View(_oStock);
        }

        //---This method is for inserting Vanning detail
        [HttpPost]
        public IActionResult InsertVanningDetail(string Vanning_Master_ID, string Chassis, string From_Location_ID, string To_Location_ID,
            string Date_From, string Date_To, string txtAmount, string txtTaxAmount, string txtInspectionCharges, string txtInspectionChargestax, string txtTotal)
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
            int? Create_by = Convert.ToInt32(UserID_Admin);
            Temp_ID = VanningNewTempID;
            string OldTempID = _oStock.GetOldTempIDFromVanningDetail_DAL(VanningMasterNewStatic_ID);
            if (!String.IsNullOrEmpty(VanningNewTempID))
            {
                Temp_ID = VanningNewTempID;

            }
            else
            {
                Temp_ID = OldTempID;
            }
            VanningMasterNewStatic_ID = VanningMasterNewStatic_ID == null ? 0 : VanningMasterNewStatic_ID;

            if (Chassis == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            if (string.IsNullOrEmpty(Vanning_Master_ID))
            {
                Vanning_Master_ID = VanningMasterNewStatic_ID.ToString();

            }
            else
            {
                Vanning_Master_ID = "0";
            }

            int user_ID = Convert.ToInt32(UserID_Admin);
            try
            {
                message = _oStock.Insert_Vanning_Details_DAL(Vanning_Master_ID, Chassis, txtAmount, txtTaxAmount, txtInspectionCharges, txtInspectionChargestax, txtTotal, Temp_ID, user_ID);
                if (message == "Saved Successfully!")
                {
                    _oStock.VanningDetailList = _oStock.get_Vanning_Details_by_TempID_ID(Temp_ID, VanningMasterNewStatic_ID.ToString()).ToList();


                    return PartialView("_VanningDetailPartial", _oStock);
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
            //    ViewName = "_VanningDetailPartial",
            //    ViewData = new ViewDataDictionary
            //<List<Pa_VanningDetails_DAL>>(ViewData, _oStock.VanningDetailList)
            //};
        }


        //---This method is for Udpating Vanning detail
        [HttpPost]
        public IActionResult UpdateVanningDetail(int VanningDetails_ID, string Chassis, string txtAmount, string txtTaxAmount, string txtInspectionCharges, string txtInspectionChargestax, string txtTotal)
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
            Temp_ID = VanningNewTempID;
            string OldTempID = _oStock.GetOldTempIDFromVanningDetail_DAL(VanningMasterNewStatic_ID);
            if (!String.IsNullOrEmpty(VanningNewTempID))
            {
                Temp_ID = VanningNewTempID;

            }
            else
            {
                Temp_ID = OldTempID;
            }
            int user_ID = Convert.ToInt32(UserID_Admin);

            VanningMasterNewStatic_ID = VanningMasterNewStatic_ID == null ? 0 : VanningMasterNewStatic_ID;



            if (VanningDetails_ID == null)
            {
                return Json(new { message = "Stock ID is null. Please try again!" });
            }

            if (Chassis == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }





            try
            {
                message = _oStock.Update_Vanning_Details_DAL(Chassis, txtAmount, txtTaxAmount, txtInspectionCharges, txtInspectionChargestax, txtTotal, user_ID, VanningDetails_ID, Temp_ID);
                if (message == "Updated Successfully!")
                {
                    _oStock.VanningDetailList = _oStock.get_Vanning_Details_by_TempID_ID(Temp_ID, VanningMasterNewStatic_ID.ToString()).ToList();

                    return PartialView("_VanningDetailPartial", _oStock);
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
            //    ViewName = "_VanningDetailPartial",
            //    ViewData = new ViewDataDictionary
            //<List<Pa_VanningDetails_DAL>>(ViewData, _oStock.VanningDetailList)
            //};
        }




        //This method is for inserting Vanning Master
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertVanningMaster(string Vendor_ID, string Date, string trans_ref, string Comments, int Sale_ID_For_Inserting)
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
            int user_ID = Convert.ToInt32(UserID_Admin);
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            int? c_ID = c_IDs;
            Temp_ID = VanningNewTempID;
            //---It is the ref in table for new Vanning. Send this value hard coded from here

            if (Vendor_ID == null || String.IsNullOrEmpty(Date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewVanning", "Stock");
            }

            Vendor_ID = Vendor_ID == null ? "" : Vendor_ID;
            trans_ref = trans_ref == null ? "" : trans_ref;
            Comments = Comments == null ? "" : Comments;

            string Sale_ID_For_Insert = HttpContext.Session.GetString("Sale_ID_For_Insert");
            if (!string.IsNullOrEmpty(Sale_ID_For_Insert))
            {
                Sale_ID_For_Inserting = Convert.ToInt32(Sale_ID_For_Insert);
            }
            else
            {
                Sale_ID_For_Inserting = 0;
            }

            try
            {
                message = _oStock.Insert_Vanning_Master_DAL(Vendor_ID, Date, trans_ref, Comments, "0", "0", Temp_ID, user_ID, Sale_ID_For_Inserting);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("VanningList", "Stock");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("NewVanning", "Stock");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("NewVanning", "Stock");
            }



        }


        //get subclassification list


        [HttpPost]
        public IActionResult DeleteVanningDetail(int? VanningDetails_ID)
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
            Temp_ID = VanningNewTempID;
            string OldTempID = _oStock.GetOldTempIDFromVanningDetail_DAL(VanningMasterNewStatic_ID);
            if (!String.IsNullOrEmpty(VanningNewTempID))
            {
                Temp_ID = VanningNewTempID;
            }
            else
            {
                Temp_ID = OldTempID;
            }
            VanningMasterNewStatic_ID = VanningMasterNewStatic_ID == null ? 0 : VanningMasterNewStatic_ID;

            if (VanningDetails_ID == null)
            {
                return Json(new { message = "ID is null. Please try again!" });
            }

            try
            {
                message = _oStock.DeleteVanningDetail_DAL(VanningDetails_ID);
                if (message == "Deleted Successfully!")
                {
                    _oStock.VanningDetailList = _oStock.get_Vanning_Details_by_TempID_ID(Temp_ID, VanningMasterNewStatic_ID.ToString()).ToList();


                    return PartialView("_VanningDetailPartial", _oStock);
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

        //This method is for inserting New Vanning Master
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateVanningMaster(string Vendor_ID, string Date, string trans_ref, string Comments, string Amount, string Tax_Amount, string vanning_master_ID)
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
            int user_ID = Convert.ToInt32(UserID_Admin);
            Temp_ID = VanningNewTempID;
            string OldTempID = _oStock.GetOldTempIDFromVanningDetail_DAL(VanningMasterNewStatic_ID);
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


            if (Vendor_ID == null || String.IsNullOrEmpty(Date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            if (vanning_master_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again!";
                return Redirect(url);
            }



            Vendor_ID = Vendor_ID == null ? "" : Vendor_ID;
            trans_ref = trans_ref == null ? "" : trans_ref;
            Comments = Comments == null ? "" : Comments;


            try
            {
                message = _oStock.Update_Vanning_Master_DAL(Vendor_ID, Date, trans_ref, Comments, Amount, Tax_Amount, Temp_ID, user_ID, vanning_master_ID);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Updated Successfully!";
                    return RedirectToAction("VanningList", "Stock");
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

        public ActionResult GetChasisBySaleRef(int Sale_ID, string temp_ID)
        {

            //I stored the Sale_ID in session because this Sale_ID will be passed into "Insert_Shipping_Info_DAL" Json method of this controller. Further, this Sale_ID will be passed into "pa_Insert_Shipping_Info" procedure with Type="BYSALEREF"
            // from "Insert_Shipping_Info_DAL" method of "oShipping_Info" class in DAL.
            //Session["Sale_ID_For_Insert"] = Sale_ID;

            HttpContext.Session.SetString("Sale_ID_For_Insert", Sale_ID.ToString());

            _oStock.VanningDetailList = _oStock.get_pa_Vanning_details_by_Sale_ID_new("BYSALEREF", Sale_ID, temp_ID).ToList();


            return PartialView("_VanningDetailPartial", _oStock);

        }

        [HttpPost]
        public IActionResult Cancel_Vanning(int? vanning_master_ID, int Status_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            int modify_id = Convert.ToInt32(UserID_Admin);
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

            if (vanning_master_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oStock.Cancel_Vanning_DAL(vanning_master_ID, Status_ID, c_ID, modify_id);
                if (message == "Cancel Successfully")
                {
                    return RedirectToAction("VanningList", "Stock");
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
        public IActionResult VanningList(string txtVanningRef, string StartDate, string EndDate, string Party_ID_Name, int? page)
        {
            //---Dumping static fields
            DumpStaticFieldsVanning();



            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "VanningList";
            ViewBag.PageTitle = "Vanning List";


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
            #region Veiwbags area
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList().Where(x => x.VendorCat_ID == 5).ToList(); // vanning category is 5;
            //---Get PurchaseStatus
            ViewBag.PurchaseStatus = _oBasic.Get_StatusList_byType_DAL("Purchase").ToList();
            #endregion veiwbags area


            //string txtVanningRef;
            //if (String.IsNullOrEmpty(f["txtVanningRef"]))
            //{
            //    txtVanningRef = "";
            //}
            //else
            //{
            //    txtVanningRef = f["txtVanningRef"];
            //}
            //string StartDate;
            //if (String.IsNullOrEmpty(f["StartDate"]))
            //{
            //    StartDate = "";
            //}
            //else
            //{
            //    StartDate = f["StartDate"];
            //}
            //string EndDate;
            //if (String.IsNullOrEmpty(f["EndDate"]))
            //{
            //    EndDate = "";
            //}
            //else
            //{
            //    EndDate = f["EndDate"];
            //}
            //string Party_ID_Name;
            //if (String.IsNullOrEmpty(f["txtparty_name"]))
            //{
            //    Party_ID_Name = "";
            //}
            //else
            //{
            //    Party_ID_Name = f["txtparty_name"];
            //}


            txtVanningRef = String.IsNullOrEmpty(txtVanningRef) ? "" : txtVanningRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Party_ID_Name = String.IsNullOrEmpty(Party_ID_Name) ? "" : Party_ID_Name;


            ViewBag.txtVanningRef_VanningList = txtVanningRef;
            ViewBag.StartDate_VanningList = StartDate;
            ViewBag.EndDate_VanningList = EndDate;
            ViewBag.Party_ID_Name_VanningList = Party_ID_Name;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;



            //---purchase master list for page
            ViewBag.RecordsPerPage = 10;
            _oStock.VanningListPagedObject = _oStock.get_Vanning_List(txtVanningRef, StartDate, EndDate, Party_ID_Name).ToList().ToPagedList(page ?? 1, 10);
            _oStock.VanningListTotal = _oStock.get_Vanning_List_Total(txtVanningRef, StartDate, EndDate, Party_ID_Name).ToList();


            return View(_oStock);
        }

        [HttpGet]
        public IActionResult VanningList_SearchFilter(string txtVanningRef, string StartDate, string EndDate, string Party_ID_Name, int? page)
        {
            //---Dumping static fields
            DumpStaticFieldsVanning();



            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "VanningList";
            ViewBag.PageTitle = "Vanning List";


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
            #region Veiwbags area
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList().Where(x => x.VendorCat_ID == 5).ToList(); // vanning category is 5;
            //---Get PurchaseStatus
            ViewBag.PurchaseStatus = _oBasic.Get_StatusList_byType_DAL("Purchase").ToList();
            #endregion veiwbags area


            //string txtVanningRef;
            //if (String.IsNullOrEmpty(f["txtVanningRef"]))
            //{
            //    txtVanningRef = "";
            //}
            //else
            //{
            //    txtVanningRef = f["txtVanningRef"];
            //}
            //string StartDate;
            //if (String.IsNullOrEmpty(f["StartDate"]))
            //{
            //    StartDate = "";
            //}
            //else
            //{
            //    StartDate = f["StartDate"];
            //}
            //string EndDate;
            //if (String.IsNullOrEmpty(f["EndDate"]))
            //{
            //    EndDate = "";
            //}
            //else
            //{
            //    EndDate = f["EndDate"];
            //}
            //string Party_ID_Name;
            //if (String.IsNullOrEmpty(f["txtparty_name"]))
            //{
            //    Party_ID_Name = "";
            //}
            //else
            //{
            //    Party_ID_Name = f["txtparty_name"];
            //}


            txtVanningRef = String.IsNullOrEmpty(txtVanningRef) ? "" : txtVanningRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Party_ID_Name = String.IsNullOrEmpty(Party_ID_Name) ? "" : Party_ID_Name;

            ViewBag.txtVanningRef_VanningList = txtVanningRef;
            ViewBag.StartDate_VanningList = StartDate;
            ViewBag.EndDate_VanningList = EndDate;
            ViewBag.Party_ID_Name_VanningList = Party_ID_Name;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;



            //---purchase master list for page
            ViewBag.RecordsPerPage = 10;
            _oStock.VanningListPagedObject = _oStock.get_Vanning_List(txtVanningRef, StartDate, EndDate, Party_ID_Name).ToList().ToPagedList(page ?? 1, 10);
            _oStock.VanningListTotal = _oStock.get_Vanning_List_Total(txtVanningRef, StartDate, EndDate, Party_ID_Name).ToList();


            return PartialView("_VanningList", _oStock);
        }

        public void DumpStaticFieldsVanning()
        {
            VanningNewTempID = null;
            VanningMasterNewStatic_ID = null;


        }
        [HttpPost]
        public IActionResult Delete_Vanning(int? vanning_master_ID)
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

            if (vanning_master_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oStock.Delete_Vanning_DAL(vanning_master_ID);
                if (message == "Deleted Successfully!")
                {
                    return RedirectToAction("VanningList", "Stock");
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




        #region Shipping_Info
        //---new Shipping_info page
        [HttpGet]
        public IActionResult shippinginfo(int? Shipping_info_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "shippinginfo";

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


            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).Where(x => x.VendorCat_ID == 6);  // shipping category is 6
            // ViewBag.ExportDestinations = _oBasic.Get_Destinations_DAL().ToList();
            ViewBag.ExportDestinations = _oBasic.Get_CarLocation_DAL(c_ID).Where(x => x.LocType == "Destination").ToList();

            //---Get Vendor Category List
            ViewBag.VendorCategoryList = _oBasic.Get_VendorCategory_DAL().ToList();

            ViewBag.SaleRefList = _oStock.Get_pa_Sale_SaleRef_DAL_Shipping().ToList();
            //ViewBag.PortFrom = _oStock.Get_PortFrom().ToList();
            ViewBag.PortFrom = _oBasic.Get_CarLocation_DAL(c_ID).Where(x => x.LocType == "PortFrom").ToList();
            //ViewBag.PortTo = _oStock.Get_PortTo().ToList();
            ViewBag.PortTo = _oBasic.Get_CarLocation_DAL(c_ID).Where(x => x.LocType == "PortTo").ToList();
            #endregion


            if (Shipping_info_ID == null || Shipping_info_ID < 1)
            {


                //---Create Shipping_info master Ref(tem id)
                if (String.IsNullOrEmpty(Shipping_infoNewTempID))
                {
                    Guid obj = Guid.NewGuid();
                    Shipping_infoNewTempID = obj.ToString();
                }
                ViewBag.Shipping_infoRef = Shipping_infoNewTempID;
                Shipping_infoMasterNewStatic_ID = Shipping_info_ID;

                //---Get Shipping_info master id
                ViewBag.Shipping_info_master_ID = Shipping_info_ID;

                //--Get Shipping_info Master
                _oStock.Shipping_infoMasterObj = _oStock.get_shipping_info_by_tempID_ID("", "0");

                //--Get Shipping_info detail
                _oStock.Shipping_infoDetailList = _oStock.get_shipping_info_details_by_tempID_ID(Shipping_infoNewTempID, "0").ToList();
            }
            else
            {

                //---Get old temp id from Shipping_info detail table that already creaded for this "Shipping_info_master_ID" id. And then use this if new record added
                string OldTempID = _oStock.GetOldTempIDFromShipping_infDetail_DAL(Shipping_info_ID);



                Shipping_infoMasterNewStatic_ID = Shipping_info_ID;


                ViewBag.Shipping_info_master_ID = Shipping_info_ID;


                _oStock.Shipping_infoMasterObj = _oStock.get_shipping_info_by_tempID_ID("", Shipping_info_ID.ToString());


                _oStock.Shipping_infoDetailList = _oStock.get_shipping_info_details_by_tempID_ID(OldTempID, Shipping_info_ID.ToString()).ToList();
            }



            return View(_oStock);
        }

        //---This method is for inserting Shipping_info detail
        [HttpPost]
        public IActionResult InsertShipping_infoDetail(string Chassis, string htemp_ID, string Shipping_info_ID, string Berth_Carry_Charges, string Berth_Carry_ChargesTax)
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
            int? Create_by = Convert.ToInt32(UserID_Admin);
            Temp_ID = Shipping_infoNewTempID;
            string OldTempID = _oStock.GetOldTempIDFromShipping_infDetail_DAL(Shipping_infoMasterNewStatic_ID);
            if (!String.IsNullOrEmpty(Shipping_infoNewTempID))
            {
                Temp_ID = Shipping_infoNewTempID;

            }
            else
            {
                Temp_ID = OldTempID;
            }
            Shipping_infoMasterNewStatic_ID = Shipping_infoMasterNewStatic_ID == null ? 0 : Shipping_infoMasterNewStatic_ID;

            if (Chassis == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            if (string.IsNullOrEmpty(Shipping_info_ID))
            {
                Shipping_info_ID = Shipping_infoMasterNewStatic_ID.ToString();

            }
            else
            {
                Shipping_info_ID = "0";
            }
            if (String.IsNullOrEmpty(Berth_Carry_ChargesTax) || String.IsNullOrWhiteSpace(Berth_Carry_ChargesTax))
            {
                Berth_Carry_ChargesTax = "0";
            }
            if (String.IsNullOrEmpty(Berth_Carry_ChargesTax) || String.IsNullOrWhiteSpace(Berth_Carry_ChargesTax))
            {
                Berth_Carry_ChargesTax = "0";
            }
            int user_ID = Convert.ToInt32(UserID_Admin);
            try
            {
                message = _oStock.Insert_Shipping_Info_details_DAL(Chassis, Temp_ID, Shipping_info_ID, Berth_Carry_Charges, Berth_Carry_ChargesTax, user_ID);
                if (message == "Saved Successfully!")
                {
                    _oStock.Shipping_infoDetailList = _oStock.get_shipping_info_details_by_tempID_ID(Temp_ID, Shipping_infoMasterNewStatic_ID.ToString()).ToList();


                    return PartialView("_Shipping_infoDetailPartial", _oStock);
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


        //---This method is for Udpating Shipping_info detail
        [HttpPost]
        public IActionResult UpdateShipping_infoDetail(string Chassis, string Berth_Carry_Charges, string Inspection_Charges, string Berth_Carry_ChargesTax, string Inspection_ChargesTax, int Shipping_infoDetails_ID)
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
            Temp_ID = Shipping_infoNewTempID;
            string OldTempID = _oStock.GetOldTempIDFromShipping_infDetail_DAL(Shipping_infoMasterNewStatic_ID);
            if (!String.IsNullOrEmpty(Shipping_infoNewTempID))
            {
                Temp_ID = Shipping_infoNewTempID;

            }
            else
            {
                Temp_ID = OldTempID;
            }
            int user_ID = Convert.ToInt32(UserID_Admin);

            Shipping_infoMasterNewStatic_ID = Shipping_infoMasterNewStatic_ID == null ? 0 : Shipping_infoMasterNewStatic_ID;



            if (Shipping_infoDetails_ID == null)
            {
                return Json(new { message = "Stock ID is null. Please try again!" });
            }

            if (Chassis == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            if (String.IsNullOrEmpty(Berth_Carry_ChargesTax) || String.IsNullOrWhiteSpace(Berth_Carry_ChargesTax))
            {
                Berth_Carry_ChargesTax = "0";
            }
            if (String.IsNullOrEmpty(Berth_Carry_ChargesTax) || String.IsNullOrWhiteSpace(Berth_Carry_ChargesTax))
            {
                Berth_Carry_ChargesTax = "0";
            }



            try
            {
                message = _oStock.Update_Shipping_Info_details_DAL(Chassis, Berth_Carry_Charges, Berth_Carry_ChargesTax, user_ID, Shipping_infoDetails_ID);
                if (message == "Updated Successfully!")
                {
                    _oStock.Shipping_infoDetailList = _oStock.get_shipping_info_details_by_tempID_ID(Temp_ID, Shipping_infoMasterNewStatic_ID.ToString()).ToList();

                    return PartialView("_Shipping_infoDetailPartial", _oStock);
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

        //---This method is for getting Vanning detail by id for update pop up form int the new Vanning form
        [HttpGet]
        public IActionResult GetVanningDetailByID(int? ID)
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

            if (ID == 0)
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

                #endregion

                _oStock.VanningObject = _oStock.GetVanningDetailByID_DAL(ID);

                return PartialView("_VanningDetailByIDPartial", _oStock);


            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured! Please try again!" });
            }


        }



        //---This method is for getting Vanning detail by id for update pop up form int the new Vanning form
        [HttpGet]
        public IActionResult GetShipping_infoDetailByID(int? ID, int Sale_ID)
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

            if (ID == 0)
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

                #endregion


                _oStock.Shipping_infoObject = _oStock.GetShipping_InfoDetailByID_DAL(ID);

                return PartialView("_Shipping_infoDetailByIDPartial", _oStock);


            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured! Please try again!" });
            }


        }

        //This method is for inserting Shipping_info Master
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertShipping_infoMaster(string Invoice_ref, string Booking_ref, string Shipper_ID, string Vessel_Name, string Vessel_Name_VoyNo,
            int Port_of_Loading_ID, string ETD, int Port_of_Discharge_ID, int Final_Destination_ID, string ETA, string Notify_Party, string Berth,
            string Berth_in_date, string BL_no, string Inspection_Type, string htemp_ID, string ContainerType,string Container_Count)
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
            int user_ID = Convert.ToInt32(UserID_Admin);
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            int? c_ID = c_IDs;
            Temp_ID = Shipping_infoNewTempID;
            //---It is the ref in table for new shippinginfo. Send this value hard coded from here

            if (Shipper_ID == null)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("shippinginfo", "Stock");
            }

            Shipper_ID = Shipper_ID == null ? "" : Shipper_ID;
            Invoice_ref = Invoice_ref == null ? "" : Invoice_ref;
            Booking_ref = Booking_ref == null ? "" : Booking_ref;

            string Sale_ID_For_Insert = HttpContext.Session.GetString("Sale_ID_For_Insert");
            int Sale_ID_For_Inserting;
            if (!string.IsNullOrEmpty(Sale_ID_For_Insert))
            {
                Sale_ID_For_Inserting = Convert.ToInt32(Sale_ID_For_Insert);
            }
            else
            {
                Sale_ID_For_Inserting = 0;
            }

            try
            {
                message = _oStock.Insert_Shipping_Info_DAL(Invoice_ref, Booking_ref, Shipper_ID, Vessel_Name, Vessel_Name_VoyNo, Port_of_Loading_ID, ETD,
                    Port_of_Discharge_ID, Final_Destination_ID, ETA, Notify_Party, Berth, Berth_in_date, BL_no, Inspection_Type,
                    Temp_ID, user_ID, Sale_ID_For_Inserting,ContainerType, Container_Count);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("Shipping_infoList", "Stock");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("shippinginfo", "Stock");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("shippinginfo", "Stock");
            }



        }


        //get subclassification list


        [HttpPost]
        public IActionResult DeleteShipping_infoDetail(int? Shipping_infoDetails_ID)
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
            Temp_ID = Shipping_infoNewTempID;
            string OldTempID = _oStock.GetOldTempIDFromShipping_infDetail_DAL(Shipping_infoMasterNewStatic_ID);
            if (!String.IsNullOrEmpty(Shipping_infoNewTempID))
            {
                Temp_ID = Shipping_infoNewTempID;
            }
            else
            {
                Temp_ID = OldTempID;
            }
            Shipping_infoMasterNewStatic_ID = Shipping_infoMasterNewStatic_ID == null ? 0 : Shipping_infoMasterNewStatic_ID;

            if (Shipping_infoDetails_ID == null)
            {
                return Json(new { message = "ID is null. Please try again!" });
            }

            try
            {
                message = _oStock.DeleteShipping_InfoDetail_DAL(Shipping_infoDetails_ID);
                if (message == "Deleted Successfully!")
                {
                    _oStock.Shipping_infoDetailList = _oStock.get_shipping_info_details_by_tempID_ID(Temp_ID, Shipping_infoMasterNewStatic_ID.ToString()).ToList();


                    return PartialView("_Shipping_infoDetailPartial", _oStock);
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

        //This method is for inserting New Shipping_info Master
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateShipping_infoMaster(string Invoice_ref, string txtInvDate, string Booking_ref, string Shipper_ID, string Vessel_Name, string Vessel_Name_VoyNo,
            int Port_of_Loading_ID, string ETD, int Port_of_Discharge_ID, int Final_Destination_ID, string ETA, string Notify_Party, string txtBerth,
            string Berth_in_date, string BL_no, string Inspection_Type, string htemp_ID, int shipinfoId, string ContainerType,string Container_Count)
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
            int user_ID = Convert.ToInt32(UserID_Admin);
            Temp_ID = Shipping_infoNewTempID;
            string OldTempID = _oStock.GetOldTempIDFromShipping_infDetail_DAL(Shipping_infoMasterNewStatic_ID);
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


            if (Shipper_ID == null)
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            if (shipinfoId == null)
            {
                TempData["Error"] = "ID is null. Please try again!";
                return Redirect(url);
            }



            Shipper_ID = Shipper_ID == null ? "" : Shipper_ID;
            Invoice_ref = Invoice_ref == null ? "" : Invoice_ref;
            Booking_ref = Booking_ref == null ? "" : Booking_ref;


            try
            {
                message = _oStock.Update_Shipping_Info_DAL(Invoice_ref, txtInvDate, Booking_ref, Shipper_ID, Vessel_Name, Vessel_Name_VoyNo, Port_of_Loading_ID, ETD,
                    Port_of_Discharge_ID, Final_Destination_ID, ETA, Notify_Party, txtBerth, Berth_in_date, BL_no, Inspection_Type,
                    Temp_ID, user_ID, shipinfoId,  ContainerType, Container_Count);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Updated Successfully!";
                    return RedirectToAction("Shipping_infoList", "Stock");
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

        public IActionResult GetShipping_infoChasisBySaleRef(int Sale_ID, string temp_ID)
        {

            //I stored the Sale_ID in session because this Sale_ID will be passed into "Insert_Shipping_Info_DAL" Json method of this controller. Further, this Sale_ID will be passed into "pa_Insert_Shipping_Info" procedure with Type="BYSALEREF"
            // from "Insert_Shipping_Info_DAL" method of "oShipping_Info" class in DAL.
            //Session["Sale_ID_For_Insert"] = Sale_ID;

            HttpContext.Session.SetString("Sale_ID_For_Insert", Sale_ID.ToString());

            _oStock.Shipping_infoDetailList = _oStock.pa_Select_Shipping_Info_details_by_TempID_ID("BYSALEREF", Sale_ID, temp_ID).ToList();


            return PartialView("_Shipping_infoDetailPartial", _oStock);

        }

        [HttpPost]
        public IActionResult Cancel_Shipping_info(int? Shipping_info_master_ID ,int Status_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");       
            int Modify_ID = Convert.ToInt32(UserID_Admin);
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

            if (Shipping_info_master_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oStock.Cancel_Shipping_Info_DAL(Shipping_info_master_ID, Status_ID,c_ID,Modify_ID);
                if (message == "Saved Successfully!")
                {
                    return RedirectToAction("Shipping_infoList", "Stock");
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
        public IActionResult Shipping_infoList(string trans_ref, string StartDate, string EndDate, string Shipper_Name, int? page)
        {
            //---Dumping static fields
            DumpStaticFieldsShipping_info();



            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "Shipping info List";
            ViewBag.PageTitle = "Shipping Info";


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
            #region Veiwbags area
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_ShippingCompaniesList_DAL(c_ID).Where(x => x.VendorCat_ID == 6).ToList(); // Shipping category is 6;
            //---Get PurchaseStatus
            ViewBag.PurchaseStatus = _oBasic.Get_StatusList_byType_DAL("Purchase").ToList();
            #endregion veiwbags area


            //string trans_ref;
            //if (String.IsNullOrEmpty(f["trans_ref"]))
            //{
            //    trans_ref = "";
            //}
            //else
            //{
            //    trans_ref = f["trans_ref"];
            //}
            //string StartDate;
            //if (String.IsNullOrEmpty(f["StartDate"]))
            //{
            //    StartDate = "";
            //}
            //else
            //{
            //    StartDate = f["StartDate"];
            //}
            //string EndDate;
            //if (String.IsNullOrEmpty(f["EndDate"]))
            //{
            //    EndDate = "";
            //}
            //else
            //{
            //    EndDate = f["EndDate"];
            //}
            //string Shipper_Name;
            //if (String.IsNullOrEmpty(f["Shipper_Name"]))
            //{
            //    Shipper_Name = "";
            //}
            //else
            //{
            //    Shipper_Name = f["Shipper_Name"];
            //}


            trans_ref = String.IsNullOrEmpty(trans_ref) ? "" : trans_ref;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Shipper_Name = String.IsNullOrEmpty(Shipper_Name) ? "" : Shipper_Name;

            ViewBag.trans_ref_Shipping_infoList = trans_ref;
            ViewBag.StartDate_infoList = StartDate;
            ViewBag.EndDate_infoList = EndDate;
            ViewBag.Shipper_Name_Shipping_infoList = Shipper_Name;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;



            //---purchase master list for page
            ViewBag.RecordsPerPage = 10;
            _oStock.Shipping_infoListPagedObject = _oStock.get_shipping_info_List(trans_ref, StartDate, EndDate, Shipper_Name).ToList().ToPagedList(page ?? 1, 10);
            _oStock.Shipping_infoListTotal = _oStock.get_shipping_info_List_Total(trans_ref, StartDate, EndDate, Shipper_Name).ToList();


            return View(_oStock);
        }

        [HttpGet]
        public IActionResult Shipping_infoList_SearchFilter(string trans_ref, string StartDate, string EndDate, string Shipper_Name, int? page)
        {
            //---Dumping static fields
            DumpStaticFieldsShipping_info();



            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "Shipping_infoList";


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
            #region Veiwbags area
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_ShippingCompaniesList_DAL(c_ID).Where(x => x.VendorCat_ID == 6).ToList(); // Shipping category is 6;
            //---Get PurchaseStatus
            ViewBag.PurchaseStatus = _oBasic.Get_StatusList_byType_DAL("Purchase").ToList();
            #endregion veiwbags area


            //string trans_ref;
            //if (String.IsNullOrEmpty(f["trans_ref"]))
            //{
            //    trans_ref = "";
            //}
            //else
            //{
            //    trans_ref = f["trans_ref"];
            //}
            //string StartDate;
            //if (String.IsNullOrEmpty(f["StartDate"]))
            //{
            //    StartDate = "";
            //}
            //else
            //{
            //    StartDate = f["StartDate"];
            //}
            //string EndDate;
            //if (String.IsNullOrEmpty(f["EndDate"]))
            //{
            //    EndDate = "";
            //}
            //else
            //{
            //    EndDate = f["EndDate"];
            //}
            //string Shipper_Name;
            //if (String.IsNullOrEmpty(f["Shipper_Name"]))
            //{
            //    Shipper_Name = "";
            //}
            //else
            //{
            //    Shipper_Name = f["Shipper_Name"];
            //}


            trans_ref = String.IsNullOrEmpty(trans_ref) ? "" : trans_ref;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Shipper_Name = String.IsNullOrEmpty(Shipper_Name) ? "" : Shipper_Name;

            ViewBag.trans_ref_Shipping_infoList = trans_ref;
            ViewBag.StartDate_infoList = StartDate;
            ViewBag.EndDate_infoList = EndDate;
            ViewBag.Shipper_Name_Shipping_infoList = Shipper_Name;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;




            //---purchase master list for page
            ViewBag.RecordsPerPage = 10;
            _oStock.Shipping_infoListPagedObject = _oStock.get_shipping_info_List(trans_ref, StartDate, EndDate, Shipper_Name).ToList().ToPagedList(page ?? 1, 10);
            _oStock.Shipping_infoListTotal = _oStock.get_shipping_info_List_Total(trans_ref, StartDate, EndDate, Shipper_Name).ToList();

            return PartialView("_Shipping_infoList", _oStock);
        }




        public void DumpStaticFieldsShipping_info()
        {
            Shipping_infoNewTempID = null;
            Shipping_infoMasterNewStatic_ID = null;


        }
        [HttpPost]
        public IActionResult Delete_Shipping_info(int? Shipping_info_master_ID)
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

            if (Shipping_info_master_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oStock.Delete_Shipping_Info_DAL(Shipping_info_master_ID);
                if (message == "Deleted Successfully!")
                {
                    return RedirectToAction("Shipping_infoList", "Stock");
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

        #region PaperManagerHSC
        [HttpGet]
        public IActionResult PaperManager(int? id)
        {

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "shippinginfo";

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
            ViewBag.ModelList = _oBasic.Get_ModelList_DAL(c_ID).ToList();
            ViewBag.WordList = _oStock.Get_Words().ToList();
            ViewBag.CitiesList = _oStock.Get_Cities().ToList();
            ViewBag.EngineList = _oBasic.Get_EngineTypeList_DAL(c_ID).ToList();






            #endregion


            if (id == null || id < 1)
            {



                ViewBag.id = id;

                _oStock.PapersMasterObj = _oStock.Paper_Details(0);


            }
            else
            {




                ViewBag.id = id;
                _oStock.PapersMasterObj = _oStock.Paper_Details(id);



            }



            return View(_oStock);
        }
        [HttpPost]
        public IActionResult InsertPaper(string CHASSIS_No, string RDate, string paperType, string isRecieved,
          string Vehicle_CC, string Engine_Type, string Registration, string RegistrationDate, string ManufactureDate, string EngineType, string lenght, string width, string height, string Weight, string gweight, string SEAT, string Drive, string _ref
            , string city, string tbNumber1, string word, string tbNumber2, string tbYear, string tbMonth, string tbMYear, string tbMMonth)
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

            int user_ID = Convert.ToInt32(UserID_Admin);
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            int? c_ID = c_IDs;

            Registration = city + "-" + tbNumber1 + "-" + word + "-" + tbNumber2;
            RegistrationDate = tbMonth + "-" + tbYear;
            ManufactureDate = tbMMonth + "-" + tbMYear;
            gweight = String.IsNullOrEmpty(gweight) ? "" : gweight;
            if (CHASSIS_No == null)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("PaperManager", "Stock");
            }
            var RecievedDate = DateTime.Now;
            var regdate = Convert.ToDateTime(RegistrationDate);
            var mnfdate = Convert.ToDateTime(ManufactureDate);
            DateTime Created_at = DateTime.Now;
            string Modified_at = "";
            Engine_Type = EngineType;
            try
            {
                message = _oStock.Insert_Paper(CHASSIS_No, RecievedDate.ToString(), paperType, isRecieved, _ref, Created_at, user_ID, Modified_at, user_ID, 0, Vehicle_CC, Engine_Type, Registration, regdate, mnfdate, EngineType, lenght, width, height, Weight, gweight, SEAT, Drive);
                if (message == "DATA SAVED SUCCESSFULLY")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("PaperList", "Stock");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("PaperManager", "Stock");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("PaperManager", "Stock");
            }



        }
        [HttpPost]
        public IActionResult UpdatePaper(int id, string CHASSIS_No, string RDate, string paperType, string isRecieved,
        string Vehicle_CC, string Registration, string RegistrationDate, string ManufactureDate, string EngineType, string lenght, string width, string height, string gweight, string weight, string SEAT, string Drive, string _ref
            , string city, string tbNumber1, string word, string tbNumber2, string tbYear, string tbMonth, string tbMYear, string tbMMonth)
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
            int user_ID = Convert.ToInt32(UserID_Admin);



            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();


            if (CHASSIS_No == null)
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            if (id == 0)
            {
                TempData["Error"] = "ID is null. Please try again!";
                return Redirect(url);
            }



            DateTime Modified_at = DateTime.Now;

            string Created_at = DateTime.Now.ToString();
            Registration = city + "-" + tbNumber1 + "-" + word + "-" + tbNumber2;
            RegistrationDate = tbMonth + "-" + tbYear;
            ManufactureDate = tbMMonth + "-" + tbMYear;
            try
            {
                var RecievedDate = DateTime.Now;
                var regdate = Convert.ToDateTime(RegistrationDate);
                var mnfdate = Convert.ToDateTime(ManufactureDate);
                message = _oStock.Update_Paper(id, CHASSIS_No, RecievedDate.ToString(), paperType, isRecieved, _ref, Convert.ToDateTime(Created_at), user_ID, Modified_at, user_ID, 0, Vehicle_CC, Registration, regdate, mnfdate, EngineType, lenght, width, height, gweight, weight, SEAT, Drive);
                if (message == "DATA SAVED SUCCESSFULLY")
                {

                    //TempData["Success"] = "Updated Successfully!";
                    return RedirectToAction("PaperList", "Stock");
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
        public IActionResult DeletePaper(string id)
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

            if (id == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oStock.Delete_Paper_DAL(id);
                if (message == "Deleted Successfully!")
                {
                    return RedirectToAction("Shipping_infoList", "Stock");
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
        public ActionResult PaperList(string chassis_No, string StartDate, string EndDate, int? page)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            else
            {
                //string StartDate;
                //if (String.IsNullOrEmpty(f["StartDate"]))
                //{
                //    StartDate = "";
                //}
                //else
                //{
                //    StartDate = f["StartDate"];
                //}
                //string EndDate;
                //if (String.IsNullOrEmpty(f["EndDate"]))
                //{
                //    EndDate = "";
                //}
                //else
                //{
                //    EndDate = f["EndDate"];
                //}
                //string chassis_No;
                //if (String.IsNullOrEmpty(f["txtChassis"]))
                //{
                //    chassis_No = "";
                //}
                //else
                //{
                //    chassis_No = f["txtChassis"];
                //}

                chassis_No = String.IsNullOrEmpty(chassis_No) ? "" : chassis_No;
                StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
                EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;

                ViewBag.chassis_No_paperlist = chassis_No;
                ViewBag.StartDate_paperlist = StartDate;
                ViewBag.EndDate_paperlist = EndDate;
                ViewBag.PageTitle = "Paper List";


                ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
                _oStock.PapersListObject_search = _oStock.get_Papers_List(chassis_No, StartDate, EndDate).ToList().ToPagedList(page ?? 1, 10);

                return View(_oStock);
            }
        }

        [HttpGet]
        public ActionResult PaperList_SearchFilter(string chassis_No, string StartDate, string EndDate, int? page)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            else
            {
                //string StartDate;
                //if (String.IsNullOrEmpty(f["StartDate"]))
                //{
                //    StartDate = "";
                //}
                //else
                //{
                //    StartDate = f["StartDate"];
                //}
                //string EndDate;
                //if (String.IsNullOrEmpty(f["EndDate"]))
                //{
                //    EndDate = "";
                //}
                //else
                //{
                //    EndDate = f["EndDate"];
                //}
                //string chassis_No;
                //if (String.IsNullOrEmpty(f["txtChassis"]))
                //{
                //    chassis_No = "";
                //}
                //else
                //{
                //    chassis_No = f["txtChassis"];
                //}

                chassis_No = String.IsNullOrEmpty(chassis_No) ? "" : chassis_No;
                StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
                EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;

                ViewBag.chassis_No_paperlist = chassis_No;
                ViewBag.StartDate_paperlist = StartDate;
                ViewBag.EndDate_paperlist = EndDate;





                ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
                _oStock.PapersListObject_search = _oStock.get_Papers_List(chassis_No, StartDate, EndDate).ToList().ToPagedList(page ?? 1, 10);

                return PartialView("_PaperList", _oStock);
            }
        }




        #endregion
        #region Purchase JP 

        [HttpGet]
        public IActionResult NewPurchaseMasterJP(int PurchaseMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Purchase";
            ViewBag.BreadCumAction = "NewPurchaseMasterJP";
            ViewBag.PageTitle = "New Purchase Master";

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
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID)
                .Where(x => x.VendorCat_ID == 3).ToList();  // Auction Vendor Category is 3
            ViewBag.ReskoMaster = _oBasic.Get_VendorMasterList_DAL(c_ID)
                .Where(x => x.VendorCat_ID == 4).ToList();  // Rikso Vendor Category is 4
            ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_ID).ToList();
            //---Get Colors List
            ViewBag.ColorsList = _oBasic.Get_ColorsList_DAL().ToList();
            ViewBag.StockType = _oBasic.Get_StockTypeList_DAL().ToList();
            //---Get Car Locations


         //   ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_ID).Where(x => x.LocType == "PurchaseLocation").ToList();
          //  ViewBag.GoingLocation = _oStock.GoingLocation().Where(x => x.LocType == "GoingLocation").ToList();
         //   ViewBag.GoingLocation = _oBasic.Get_CarLocation_DAL(c_ID).Where(x => x.LocType == "GoingLocation").ToList();
           


            #endregion




            if (PurchaseMaster_ID == 0 || PurchaseMaster_ID < 1)
            {

                //---Create payment master Ref(tem id)
                if (String.IsNullOrEmpty(PurchaseMasterJPMaster_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    PurchaseMasterJPMaster_TempID = obj.ToString();
                }

                PurchaseMasterJPStatic_ID = PurchaseMaster_ID;

                //---Get Purchase master id
                ViewBag.PurchaseMaster_ID = PurchaseMaster_ID;
                _oStock.PurchaseMasterRef = _oStock.LoadRef();

                //--Get Purchase Master
                _oStock.PurchaseMasterObj = _oStock.Get_PurchaseMasterJPMasterByID_DAL(-1);


                //--Get Vendor list
                ViewBag.VendorCategoryList = _oBasic.Get_VendorCategory_DAL().ToList();

                //Yaseen
                ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).Where(x => x.VendorCat_ID == 3).ToList();
               
                ViewBag.ReskoMaster  = _oBasic.Get_VendorMasterList_DAL(c_ID).Where(x => x.VendorCat_ID == 4).ToList();

                
                ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_ID).Where(x => x.LocType == "PurchaseLocation").ToList();          
                ViewBag.GoingLocation = _oBasic.Get_CarLocation_DAL(c_ID).Where(x => x.LocType == "GoingLocation").ToList();
             
                
                
                
                //---Grand Total 
                _oStock.PurchaseDetailGrandTotal = _oStock.Get_purchaseDetailsTotal(-1).FirstOrDefault();

                //---Purchase detial  list
                _oStock.PurchaseDetailList = _oStock.Get_PurchaseMasterJPDetailListByID_DAL(PurchaseMasterJPMaster_TempID, -1).ToList();




            }
            else
            {

                //---Get old temp id from Purchase detail table that already creaded for this "PurchaseMaster_ID" id. And then use this if new record added
                string OldTempID = _oStock.GetOldTempIDFromPurchaseDetail_DAL(PurchaseMaster_ID);




                PurchaseMasterJPStatic_ID = PurchaseMaster_ID;

                //---Get sale master id
                ViewBag.PurchaseMaster_ID = PurchaseMaster_ID;

                //--Get sale Master

                _oStock.PurchaseMasterObj = _oStock.Get_PurchaseMasterJPMasterByID_DAL(PurchaseMaster_ID);

                //---Purchase detail  list 
                _oStock.PurchaseDetailList = _oStock.Get_PurchaseMasterJPDetailListByID_DAL(OldTempID, PurchaseMaster_ID).ToList();


                //--Get Vendor list
                ViewBag.VendorCategoryList = _oBasic.Get_VendorCategory_DAL().ToList();
               

                //Yaseen
                ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).Where(x => x.VendorCat_ID == 3).ToList();
                ViewBag.ReskoMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).Where(x => x.VendorCat_ID == 4).ToList();

               
                ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_ID).Where(x => x.LocType == "PurchaseLocation").ToList();          
                ViewBag.GoingLocation = _oBasic.Get_CarLocation_DAL(c_ID).Where(x => x.LocType == "GoingLocation").ToList();
              

                //---Grand Total 
                _oStock.PurchaseDetailGrandTotal = _oStock.Get_purchaseDetailsTotal(PurchaseMaster_ID).FirstOrDefault();

                _oStock.PurchaseMasters = _oStock.PurchaseMaster(Convert.ToInt32(PurchaseMaster_ID)).FirstOrDefault();


            }



            return View(_oStock);
        }


        //---insert into Purchase  detial
        [HttpPost]
        public IActionResult InsertPurchaseMasterJPDetail(string Chassis, int Make, int Model, int Color, int loc, string Ref, string Lot,
            string Date, decimal? Price, int PriceRate, decimal PriceTax, decimal RecycleFee,
            decimal auctionfee, decimal auctionfeetax, decimal NumberPlate, decimal TotalPrice,
            string Year, int GoingToLocation, int VendorName, decimal ReksoFee,
            decimal ReksoFeeTax, int StockType, string Transmission, int hdPurchaseMaster_ID)
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

            Temp_ID = PurchaseMasterJPMaster_TempID;

            if (hdPurchaseMaster_ID == 0)
            {
                Temp_ID = PurchaseMasterJPMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }
            PurchaseMasterJPStatic_ID = PurchaseMasterJPStatic_ID == null ? -1 : PurchaseMasterJPStatic_ID;


            try
            {
                message = _oStock.InsertPurchaseMasterJPDetail_DAL(Chassis, Make, Model, Color, loc, Ref, Lot,
                Price, PriceRate, PriceTax, RecycleFee,
             auctionfee, auctionfeetax, NumberPlate, TotalPrice,
             Year, GoingToLocation, VendorName, ReksoFee,
             ReksoFeeTax, StockType, Transmission, hdPurchaseMaster_ID, Temp_ID, c_ID, Created_by);
                if (message == "Added Successfully")
                {
                    //---Purchase detial  list 
                    _oStock.PurchaseDetailList = _oStock.Get_PurchaseMasterJPDetailListByID_DAL(Temp_ID, hdPurchaseMaster_ID).ToList();


                    return PartialView("_PurchaseMasterJPDetailList", _oStock);
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



        //---method for inserting Purchase  detail
        [HttpPost]
        public IActionResult UpdatePurchaseMasterJPDetail(int? PurchaseDetailID, string Chassis, int? Make, int? Model, int? Color, int? loc, string Lot,
            decimal? Price, decimal? PriceRate, decimal? PriceTax, decimal? RecycleFee,
            decimal? auctionfee, decimal? auctionfeetax, decimal? NumberPlate, decimal? TotalPrice, string Year,
            int? GoingToLocation, int? VendorName,
            decimal? ReksoFee, decimal? ReksoFeeTax, int? StockType, string Transmission, int hdPurchaseMaster_ID)
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
            if (PurchaseDetailID == null)
            {
                return Json(new { message = "Sale Id is null" });
            }
            if (Price == null || String.IsNullOrEmpty(Chassis))
            {
                return Json(new { message = "Please enter Purchase price!" });
            }
            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = PurchaseMasterJPMaster_TempID;
            if (hdPurchaseMaster_ID == 0)
            {
                Temp_ID = PurchaseMasterJPMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }





            try
            {
                message = _oStock.UpdatePurchaseMasterJPDetail_DAL(PurchaseDetailID, Chassis, Make, Model, Color, loc, Lot,
                Price, PriceRate, PriceTax, RecycleFee,
             auctionfee, auctionfeetax, NumberPlate, TotalPrice,
             Year, GoingToLocation, VendorName, ReksoFee,
             ReksoFeeTax, StockType, Transmission, Temp_ID, c_ID, Modified_by);
                if (message == "Updated Successfully")
                {
                    //---Purchase detial  list 
                    _oStock.PurchaseDetailList = _oStock.Get_PurchaseMasterJPDetailListByID_DAL(Temp_ID, hdPurchaseMaster_ID).ToList();


                    return PartialView("_PurchaseMasterJPDetailList", _oStock);
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

        //---Insert  Purchase master 
        [HttpPost]
        public IActionResult InsertPurchaseMasterJPMaster(int? Vendor, string Ref, string PurchaseRef, string Date, string PaymentDueDate, string SupplierContact, string SupplierAddress)
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
            Temp_ID = PurchaseMasterJPMaster_TempID;
            string Purchasetype = "PRJ"; //---It is the ref in table for new Purchase master. Send this value hard coded from here

            if (Vendor == null || String.IsNullOrEmpty(Date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewPurchaseMasterJP", "Stock");
            }

            Vendor = Vendor == null ? 0 : Vendor;


            try
            {
                message = _oStock.InsertPurchaseMasterJPMaster_DAL(Vendor, Ref, PurchaseRef, Date, PaymentDueDate, SupplierContact, SupplierAddress, Purchasetype, Temp_ID, c_ID, Created_by);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("PurchaseListJP", "Stock");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("NewPurchaseMasterJP", "Stock");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("NewPurchaseMasterJP", "Stock");
            }


        }

        //---Insert  Purchase master 
        [HttpPost]
        public IActionResult UpdatePurchaseMasterJPMaster(int? PurchaseMaster_ID, string PurchaseRef, string Date, int? Vendor, string Ref, string SupplierContact, string SupplierAddress)
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
            Temp_ID = PurchaseMasterJPMaster_TempID;
            string OldTempID = _oStock.GetOldTempIDFromPurchaseDetail_DAL(PurchaseMaster_ID);
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

            if (Vendor == null || String.IsNullOrEmpty(Date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            Vendor = Vendor == null ? 0 : Vendor;


            try
            {
                message = _oStock.UpdatePurchaseMasterJPMaster_DAL(PurchaseMaster_ID, PurchaseRef, Date, Vendor, Ref, SupplierContact, SupplierAddress, Temp_ID, c_ID, Modified_by);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("PurchaseListJP", "Stock");
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



        //---Method for deleting Purchase  master
        [HttpPost]
        public IActionResult DeletePurchaseMasterJPMaster(int? PurchaseMaster_ID)
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
                message = _oStock.DeletePurchaseMaster_DAL(PurchaseMaster_ID, Convert.ToInt32(UserID_Admin));
                if (message.Contains("SUCCESSFULLY"))
                {
                    return RedirectToAction("PurchaseListJP", "Stock");
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
        public JsonResult GetMakeCodeDetail(string Code)
        {

            int? ModelDesc_ID = 0;
            int? Make_ID = 0;


            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function
            _oBasic.GetCodeObj = _oBasic.GetCodeDetail_DAL(Code.Split('-')[0]);

            if (_oBasic.GetCodeObj.ModelDesc_ID>0)
            {

                ModelDesc_ID = _oBasic.GetCodeObj.ModelDesc_ID;
                Make_ID = _oBasic.GetCodeObj.Make_ID;
     

            }
            else
            {

                ModelDesc_ID = null;
                Make_ID = null;

            }


            return Json(new { ModelDesc_ID, Make_ID });

        }


        [HttpGet]
        public JsonResult GetDetailsByCode(string Code)
        {

            int? ModelDesc_ID = 0;
            int? Make_ID = 0;
            int? VehicalCountry = 0;
            int? VehicalCategory = 0;
            int? Engine_Type = 0;
            string Drive = "";
            string Fuel_Type = "";
            string Door = "";
            string Hs_Code = "";
            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function
            _oBasic.GetCodeObj = _oBasic.GetDetailsByCode_DAL(Code.Split('-')[0]);

            if (_oBasic.GetCodeObj.ModelDesc_ID > 0)
            {

                ModelDesc_ID = _oBasic.GetCodeObj.ModelDesc_ID;
                Make_ID = _oBasic.GetCodeObj.Make_ID;
                VehicalCountry = Convert.ToInt32(_oBasic.GetCodeObj.MakeCountry_ID);
                VehicalCategory = _oBasic.GetCodeObj.VehCategory_ID;
                Engine_Type = Convert.ToInt32(_oBasic.GetCodeObj.EngineType);
                Drive = _oBasic.GetCodeObj.Drive;
                Fuel_Type = _oBasic.GetCodeObj.FuelType;
                Door = _oBasic.GetCodeObj.Door;
                Hs_Code = _oBasic.GetCodeObj.Hs_Code;

            }
            else
            {

                ModelDesc_ID = null;
                Make_ID = null;

            }


            return Json(new { ModelDesc_ID, Make_ID,VehicalCountry,VehicalCategory,Engine_Type,Drive,Fuel_Type, Door, Hs_Code });

        }

        [HttpPost]
        public IActionResult Cancel_PurchaseJP(int? PurchaseMaster_IDStatus, int? Status_ID)
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
            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();

            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            string Trans_Type = "Purchase_JP";
            string message = "";

            if (PurchaseMaster_IDStatus == null || Status_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oStock.Cancel_PurchaseJP(PurchaseMaster_IDStatus, Status_ID, c_ID, Trans_Type, Modified_by);
                if (message.Contains("Successfully"))
                {
                    return RedirectToAction("PurchaseListJP", "Stock");
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
        public IActionResult DeletePurchaseDetail(int? StockDetails_ID)
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
            Temp_ID = PurchaseMasterJPMaster_TempID;

            PurchaseMasterJPStatic_ID = PurchaseMasterJPStatic_ID == -1 ? 0 : PurchaseMasterJPStatic_ID;
            string OldTempID = _oStock.GetOldTempIDFromPurchaseDetail_DAL(PurchaseMasterJPStatic_ID);
            if (!String.IsNullOrEmpty(PurchaseMasterJPMaster_TempID))
            {
                Temp_ID = PurchaseMasterJPMaster_TempID;

            }
            else
            {
                Temp_ID = OldTempID;
            }
            PurchaseMasterJPStatic_ID = PurchaseMasterJPStatic_ID == null ? -1 : PurchaseMasterJPStatic_ID;

            if (PurchaseMasterJPStatic_ID == null)
            {
                return Json(new { message = "ID is null. Please try again!" });
            }

            try
            {
                message = _oStock.DeletePurchaseDetail_DAL(StockDetails_ID);
                if (message == "Deleted Successfully!")
                {
                    _oStock.PurchaseDetailList = _oStock.Get_PurchaseMasterJPDetailListByID_DAL(Temp_ID, PurchaseMasterJPStatic_ID).ToList();


                    return PartialView("_PurchaseMasterJPDetailList", _oStock);
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
        public IActionResult PurchaseListJP(string PurchaseRef, int Vendor_ID, string From_Date, string To_Date, int Status_ID,string ChassisNo, int c_ID, int? page)
        {
            //---Dumping static fields
            DumpStaticFieldsPurchase();



            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Purchases";
            ViewBag.BreadCumAction = "PurchaseList";
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
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_IDs).Where(x =>
            x.VendorCat_ID == 3).ToList();  // Auction Vendor Category is 3
            //---Get PurchaseStatus
            ViewBag.PurchaseStatus = _oBasic.Get_StatusList_byType_DAL("Purchase").ToList();
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;




            #endregion veiwbags area


            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            Vendor_ID = Vendor_ID == 0 ? Vendor_ID : Vendor_ID;
            From_Date = String.IsNullOrEmpty(From_Date) ? "" : From_Date;
            To_Date = String.IsNullOrEmpty(To_Date) ? "" : To_Date;
            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Status_ID = Status_ID == null ? 0 : Status_ID;

            c_ID = c_ID == 0 ? c_IDs : c_ID;

            ViewBag.PurchaseRef_PurchaseList = PurchaseRef;
            ViewBag.Vendor_ID_PurchaseList = Vendor_ID;
            ViewBag.From_Date_PurchaseList = From_Date;
            ViewBag.To_Date_PurchaseList = To_Date;
            ViewBag.ChassisNo_PurchaseList = ChassisNo;
            ViewBag.Status_ID_PurchaseList = Status_ID;
            ViewBag.c_ID_PurchaseList = c_ID;
            //---purchase master list for page
             ViewBag.RecordsPerPage = 10;
            _oStock.PurchaseMasterList = _oStock.pa_Select_PurchaseMaster(PurchaseRef, From_Date, To_Date, Vendor_ID, Status_ID, ChassisNo, c_ID).ToList().ToPagedList(page ?? 1, 10);
            _oStock.purchaseMasterTotal = _oStock.Get_PurchaseMasterList_Total(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, ChassisNo, c_ID).ToList();

            return View(_oStock);
        }

        //---Get purchase list by search filters
        [HttpGet]
        public IActionResult PurchaseListJP_BySearchFilers(string PurchaseRef, int Vendor_ID, string From_Date, string To_Date, int Status_ID,string ChassisNo, int c_ID, int? page)
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
            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Status_ID = Status_ID == null ? 0 : Status_ID;
            c_ID = c_ID == null ? 1 : c_ID;

            ViewBag.PurchaseRef_PurchaseList = PurchaseRef;
            ViewBag.Vendor_ID_PurchaseList = Vendor_ID;
            ViewBag.From_Date_PurchaseList = From_Date;
            ViewBag.To_Date_PurchaseList = To_Date;
            ViewBag.ChassisNo_PurchaseList = ChassisNo;
            ViewBag.Status_ID_PurchaseList = Status_ID;
            ViewBag.c_ID_PurchaseList = c_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            try
            {
                ViewBag.RecordsPerPage = 10;
                _oStock.PurchaseMasterList = _oStock.pa_Select_PurchaseMaster(PurchaseRef, From_Date, To_Date, Vendor_ID, Status_ID, ChassisNo, c_ID).ToList().ToPagedList(page ?? 1, 10);
                _oStock.purchaseMasterTotal = _oStock.Get_PurchaseMasterList_Total(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, ChassisNo, c_ID).ToList();


                return PartialView("_PurchaseListJP_SearchFilter", _oStock);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }


        //-- by Rafay : Start Date : 12/01/2021

        [HttpGet]
        public IActionResult reksolist(string PurchaseRef, string StartDate, string EndDate, int RekSo_Vendor_ID, int? page)
           {

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
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;



            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "reksolist";
            ViewBag.PageTitle = "Rekso List";
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            //Yaseen VendorCat_ID == 3
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).Where(x => x.VendorCat_ID == 4).ToList();

            _oStock.ReksoList = _oStock.pa_select_StockParties(PurchaseRef, StartDate, EndDate, RekSo_Vendor_ID).ToList().ToPagedList(page ?? 1, 10);
            _oStock.ReksoList_total = _oStock.pa_select_StockParties_total(PurchaseRef, StartDate, EndDate, RekSo_Vendor_ID).ToList();

            return View(_oStock);
        }

        //-- by Rafay : End Date : 12/01/2021


        [HttpGet]
        public IActionResult reksolist_SearchFilter(string PurchaseRef, string StartDate, string EndDate, int RekSo_Vendor_ID, int? page)
        {

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
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            //    RekSo_Vendor_ID == RekSo_Vendor_ID ? 0 : RekSo_Vendor_ID;

            RekSo_Vendor_ID = RekSo_Vendor_ID == null ? 0 : RekSo_Vendor_ID;


            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "reksolist";


            //return View(_oStock);

            try
            {
                ViewBag.RecordsPerPage = 10;
                ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).Where(x => x.VendorCat_ID == 4).ToList();

                _oStock.ReksoList = _oStock.pa_select_StockParties(PurchaseRef, StartDate, EndDate, RekSo_Vendor_ID).ToList().ToPagedList(page ?? 1, 10);
                _oStock.ReksoList_total = _oStock.pa_select_StockParties_total(PurchaseRef, StartDate, EndDate, RekSo_Vendor_ID).ToList();

                return PartialView("_reksolist_SearchFilter", _oStock);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }


        }

        public void DumpStaticFieldsPurchase()
        {
            PurchaseMasterJPMaster_TempID = null;
            PurchaseMasterJPStatic_ID = null;
            AuctionMaster_TempID = null;
            AuctionMasterStatic_ID = null;


        }
        #endregion
        #region stock attachment 

        //---Get purchase list by search filters
        [HttpGet]
        public IActionResult GetStockMasterNew_Attachments(int? StockMaster_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            string Type = "StockNew";

            try
            {
                _oStock.attachmentList = _oStock.GetStockMasterNew_Attachments_DAL(StockMaster_ID, Type).ToList();

                return PartialView("_StockListAttachment", _oStock);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }


        [HttpPost]
        public IActionResult InsertAttachments_StockMaster(int? StockMaster_ID, string Type, string Document_Type, IFormFile Attachment)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            if (StockMaster_ID == null || String.IsNullOrEmpty(Document_Type) || Attachment == null)
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

                message = _oStock.Insert_StockMasterAttachment_DAL(StockMaster_ID, Document_Type, Type, newFileName, filepath, Created_by);
                if (message == "Saved Successfully!")
                {

                    _oStock.attachmentList = _oStock.GetStockMasterNew_Attachments_DAL(StockMaster_ID, Type).ToList();

                    return PartialView("_StockListAttachment", _oStock);
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

        //---this method is for deleting attachment in Stock list of type new
        [HttpGet]
        public IActionResult DeleteAttachmentStockNew(int? StockMaster_ID, int? Attachment_ID, string Type, string File_Name)
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
            if (StockMaster_ID == null)
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

                message = _oStock.Delete_Attachment_StockNew(Attachment_ID);
                if (message == "Deleted Successfully!")
                {

                    _oStock.attachmentList = _oStock.GetStockMasterNew_Attachments_DAL(StockMaster_ID, Type).ToList();

                    return PartialView("_StockListAttachment", _oStock);
                }
                else
                {
                    TempData["Error"] = message;
                    return PartialView("_StockListAttachment", _oStock);
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



        #endregion


        #region Auction

        [HttpGet]
        public IActionResult AddAuction(int Auction_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "NewAuction";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(UserName_Admin) || string.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion



            #region ViewBag Area



            _oStock.StockList = _oStock.Get_AllStock_DAL().ToList();
            #endregion




            if (Auction_ID == 0 || Auction_ID < 1)
            {


                //---Create payment master Ref(tem id)
                if (string.IsNullOrEmpty(AuctionMaster_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    AuctionMaster_TempID = obj.ToString();
                }

                AuctionMasterStatic_ID = Auction_ID;

                //---Get Stock master id
                ViewBag.Auction_ID = Auction_ID;

                //--Get Stock Master
                _oStock.AuctionMasterObj = _oStock.Get_AuctionMasterByID_DAL(-1);




                //---Stock detial invoice list for Stock invoice type Bychassis
                _oStock.AuctionDetailList = _oStock.Get_AuctionDetailListBy_DAL(AuctionMaster_TempID, 0).ToList();


            }
            else
            {

                //---Get old temp id from Stock detail table that already creaded for this "Auction_ID" id. And then use this if new record added
                string OldTempID = _oStock.GetOldTempIDFromAuctionDetail_DAL(Auction_ID);




                AuctionMasterStatic_ID = Auction_ID;

                //---Get sale master id
                ViewBag.Auction_ID = Auction_ID;

                //--Get sale Master

                _oStock.AuctionMasterObj = _oStock.Get_AuctionMasterByID_DAL(Auction_ID);

                //---Stock detial invoice list for Stock invoice type Bychassis
                _oStock.AuctionDetailList = _oStock.Get_AuctionDetailListBy_DAL(OldTempID, Auction_ID).ToList();



            }



            return View(_oStock);
        }


        //---insert into Stock invoice detial
        [HttpPost]
        public IActionResult InsertAuctionDetail(int? HdAuction_ID, int[] Stock_ID)
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


            if (Stock_ID == null)
            {
                return Json(new { message = "Please enter Stock!" });
            }
            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = AuctionMaster_TempID;


            if (HdAuction_ID == 0)
            {
                Temp_ID = AuctionMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            try
            {
                foreach (var item in Stock_ID)
                {
                    try
                    {
                        message = _oStock.InsertAuctionDetail_DAL(HdAuction_ID, item, Temp_ID);

                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        return Json(new { message = ErrorMessage });
                    }

                }


                if (message == "Added Successfully!")
                {
                    //---Stock detial invoice list for Stock invoice type Bychassis
                    _oStock.AuctionDetailList = _oStock.Get_AuctionDetailListBy_DAL(Temp_ID, HdAuction_ID).ToList();



                    return PartialView("_AuctionDetailList", _oStock);
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
        public IActionResult InsertAuctionMaster(string Auction_Date, string Ref, string Remarks
     )
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
            Temp_ID = AuctionMaster_TempID;

            if (String.IsNullOrEmpty(Auction_Date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewAuction", "Stock");
            }




            try
            {
                message = _oStock.InsertAuctionMaster_DAL(Auction_Date, Ref, Remarks,
      Temp_ID, c_ID, Created_by);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("AuctionList", "Stock");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("NewAuction", "Stock");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
                return RedirectToAction("NewAuction", "Stock");
            }


        }

        //---Insert  Stock invoice master
        [HttpPost]
        public IActionResult UpdateAuctionMaster(string Auction_Date, string Ref, string Remarks, int? Auction_ID
         )
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
            Temp_ID = AuctionMaster_TempID;
            string OldTempID = _oStock.GetOldTempIDFromAuctionDetail_DAL(Auction_ID);
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


            if (String.IsNullOrEmpty(Auction_Date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }



            try
            {
                message = _oStock.UpdateAuctionMaster_DAL(Auction_Date, Ref, Remarks, Auction_ID,
           Temp_ID, c_ID, Modified_by);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("AuctionList", "Stock");
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






        //---Method for deleting Stock invoice detail
        [HttpPost]
        public IActionResult DeleteAuctionDetail(int? Detail_ID, int HdAuction_ID)
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
            string Temp_ID = "";
            if (Detail_ID == null)
            {

                return Json(new { message = "ID is null. Please try again" });
            }

            if (HdAuction_ID == 0)
            {
                Temp_ID = AuctionMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            try
            {
                message = _oStock.DeleteAuctionDetail_DAL(Detail_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = message;
                    _oStock.AuctionDetailList = _oStock.Get_AuctionDetailListBy_DAL(Temp_ID, HdAuction_ID).ToList();



                    return PartialView("_AuctionDetailList", _oStock);

                }
                else
                {

                    return Json(new { message = message });
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

        public IActionResult AuctionList()
        {
            //---Dumping static fields

            DumpStaticFieldsAuction();
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "AuctionList";


            ViewBag.SectionHeaderTitle = "Stock";
            ViewBag.SectionSub_HeaderTitle = "Auction List";
            ViewBag.PageTitle = "Auction List";


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            //---list for page
            _oStock.AuctionMasterList = _oStock.Get_AuctionList_DAL().ToList();


            return View(_oStock);
        }
        public void DumpStaticFieldsAuction()
        {
            AuctionMaster_TempID = null;
            AuctionMasterStatic_ID = null;
        }

        #endregion
        #region Meharstock
        [HttpGet]

        public IActionResult StockList_MM(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
    string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID, int? page)
        {
            //---Dumping static fields


            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "StockList_MM";


            ViewBag.SectionHeaderTitle = "Stock";
            ViewBag.SectionSub_HeaderTitle = "Stock List";
            ViewBag.PageTitle = "Stock List";


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
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_IDs).Where(x => x.VendorCat_ID == 2).ToList();
            //---Make list for drop down in forms
            ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_IDs).ToList();
            //---Get Colors List
            ViewBag.ColorsList = _oBasic.Get_ColorsList_DAL().ToList();
            //---Get Location List
            ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_IDs).ToList();
            //---Get Status List
            ViewBag.Status = _oStock.get_Stock_Status_DAL().ToList();

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.AttachmentVisibility_Only_Stock = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility_Only_Stock").Value;
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;

            #endregion veiwbags area


            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;

            c_ID = c_ID == 0 ? c_IDs : c_ID;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            BL_NO = String.IsNullOrEmpty(BL_NO) ? "" : BL_NO;
            BOE = String.IsNullOrEmpty(BOE) ? "" : BOE;
            loc_ID = loc_ID == null ? 0 : loc_ID;
            Stock_Status = String.IsNullOrEmpty(Stock_Status) ? "" : Stock_Status;
            VendorName = String.IsNullOrEmpty(VendorName) ? "" : VendorName;
            ModelYear = String.IsNullOrEmpty(ModelYear) ? "" : ModelYear;
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;
            StockType_ID = StockType_ID == null ? 0 : StockType_ID;

            ViewBag.ChassisNo_StockList = ChassisNo;
            ViewBag.Make_ID_StockList = Make_ID;
            ViewBag.MakeModel_description_ID_StockList = MakeModel_description_ID;
            ViewBag.Color_Exterior_ID_StockList = Color_Exterior_ID;
            ViewBag.PurchaseStartDate_StockList = PurchaseStartDate;
            ViewBag.PurchaseEndDate_StockList = PurchaseEndDate;
            ViewBag.BL_NO_StockList = BL_NO;
            ViewBag.BOE_StockList = BOE;
            ViewBag.PurchaseRef_StockList = PurchaseRef;
            ViewBag.SaleRef_StockList = SaleRef;
            ViewBag.c_ID_StockList = c_ID;
            ViewBag.Stock_Status_StockList = Stock_Status;
            ViewBag.loc_ID_StockList = loc_ID;
            ViewBag.VendorName_StockList = VendorName;
            ViewBag.ModelYear_StockList = ModelYear;
            ViewBag.Container_No_StockList = Container_No;
            ViewBag.StockType_ID_StockList = StockType_ID;
            //---list for page
            _oStock.StockListPagedObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList().ToPagedList(page ?? 1, 10);

            _oStock.StockList_TTL = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();


            _oStock.StockListStats = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);


            return View(_oStock);
        }

        //---Get purchase list by search filters
        [HttpGet]
        public IActionResult StockListBySearchFilers_MM(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
            string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID, int? page)
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

            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;
            c_ID = c_ID == 0 ? 1 : c_ID;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            BL_NO = String.IsNullOrEmpty(BL_NO) ? "" : BL_NO;
            BOE = String.IsNullOrEmpty(BOE) ? "" : BOE;
            loc_ID = loc_ID == null ? 0 : loc_ID;
            Stock_Status = String.IsNullOrEmpty(Stock_Status) ? "" : Stock_Status;
            VendorName = String.IsNullOrEmpty(VendorName) ? "" : VendorName;
            ModelYear = String.IsNullOrEmpty(ModelYear) ? "" : ModelYear;
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;
            StockType_ID = StockType_ID == null ? 0 : StockType_ID;

            ViewBag.ChassisNo_StockList = ChassisNo;
            ViewBag.Make_ID_StockList = Make_ID;
            ViewBag.MakeModel_description_ID_StockList = MakeModel_description_ID;
            ViewBag.Color_Exterior_ID_StockList = Color_Exterior_ID;
            ViewBag.PurchaseStartDate_StockList = PurchaseStartDate;
            ViewBag.PurchaseEndDate_StockList = PurchaseEndDate;
            ViewBag.BL_NO_StockList = BL_NO;
            ViewBag.BOE_StockList = BOE;
            ViewBag.PurchaseRef_StockList = PurchaseRef;
            ViewBag.SaleRef_StockList = SaleRef;
            ViewBag.c_ID_StockList = c_ID;
            ViewBag.Stock_Status_StockList = Stock_Status;
            ViewBag.loc_ID_StockList = loc_ID;
            ViewBag.VendorName_StockList = VendorName;
            ViewBag.ModelYear_StockList = ModelYear;
            ViewBag.Container_No_StockList = Container_No;
            ViewBag.StockType_ID_StockList = StockType_ID;

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.AttachmentVisibility_Only_Stock = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility_Only_Stock").Value;
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;

            try
            {
                //  _oStock.StockListObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, c_ID);

                _oStock.StockListPagedObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList().ToPagedList(page ?? 1, 10);

                _oStock.StockList_TTL = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();


                _oStock.StockListStats = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);


                return PartialView("_StockListSearchPartial_MM", _oStock);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }

        #endregion
        #region Intl
        [HttpGet]

        public IActionResult StockList_im(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
    string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID, int? page)
        {
            //---Dumping static fields


            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "StockList_im";


            ViewBag.SectionHeaderTitle = "Stock";
            ViewBag.SectionSub_HeaderTitle = "Stock List";
            ViewBag.PageTitle = "Stock List";


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
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_IDs).Where(x => x.VendorCat_ID == 2).ToList();
            //---Make list for drop down in forms
            ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_IDs).ToList();
            //---Get Colors List
            ViewBag.ColorsList = _oBasic.Get_ColorsList_DAL().ToList();
            //---Get Location List
            ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_IDs).ToList();
            //---Get Status List
            ViewBag.Status = _oStock.get_Stock_Status_DAL().ToList();

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.AttachmentVisibility_Only_Stock = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility_Only_Stock").Value;
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;

            #endregion veiwbags area


            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;

            c_ID = c_ID == 0 ? c_IDs : c_ID;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            BL_NO = String.IsNullOrEmpty(BL_NO) ? "" : BL_NO;
            BOE = String.IsNullOrEmpty(BOE) ? "" : BOE;
            loc_ID = loc_ID == null ? 0 : loc_ID;
            Stock_Status = String.IsNullOrEmpty(Stock_Status) ? "" : Stock_Status;
            VendorName = String.IsNullOrEmpty(VendorName) ? "" : VendorName;
            ModelYear = String.IsNullOrEmpty(ModelYear) ? "" : ModelYear;
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;
            StockType_ID = StockType_ID == null ? 0 : StockType_ID;

            ViewBag.ChassisNo_StockList = ChassisNo;
            ViewBag.Make_ID_StockList = Make_ID;
            ViewBag.MakeModel_description_ID_StockList = MakeModel_description_ID;
            ViewBag.Color_Exterior_ID_StockList = Color_Exterior_ID;
            ViewBag.PurchaseStartDate_StockList = PurchaseStartDate;
            ViewBag.PurchaseEndDate_StockList = PurchaseEndDate;
            ViewBag.BL_NO_StockList = BL_NO;
            ViewBag.BOE_StockList = BOE;
            ViewBag.PurchaseRef_StockList = PurchaseRef;
            ViewBag.SaleRef_StockList = SaleRef;
            ViewBag.c_ID_StockList = c_ID;
            ViewBag.Stock_Status_StockList = Stock_Status;
            ViewBag.loc_ID_StockList = loc_ID;
            ViewBag.VendorName_StockList = VendorName;
            ViewBag.ModelYear_StockList = ModelYear;
            ViewBag.Container_No_StockList = Container_No;
            ViewBag.StockType_ID_StockList = StockType_ID;
            //---list for page
            _oStock.StockListPagedObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No,StockType_ID).ToList().ToPagedList(page ?? 1, 10);

            _oStock.StockList_TTL = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();


            _oStock.StockListStats = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);


            return View(_oStock);
        }

        //---Get purchase list by search filters
        [HttpGet]
        public IActionResult StockListBySearchFilers_im(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
            string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID, int? page)
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

            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;
            c_ID = c_ID == 0 ? 1 : c_ID;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            BL_NO = String.IsNullOrEmpty(BL_NO) ? "" : BL_NO;
            BOE = String.IsNullOrEmpty(BOE) ? "" : BOE;
            loc_ID = loc_ID == null ? 0 : loc_ID;
            Stock_Status = String.IsNullOrEmpty(Stock_Status) ? "" : Stock_Status;
            VendorName = String.IsNullOrEmpty(VendorName) ? "" : VendorName;
            ModelYear = String.IsNullOrEmpty(ModelYear) ? "" : ModelYear;
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;
            StockType_ID = StockType_ID == null ? 0 : StockType_ID;

            ViewBag.ChassisNo_StockList = ChassisNo;
            ViewBag.Make_ID_StockList = Make_ID;
            ViewBag.MakeModel_description_ID_StockList = MakeModel_description_ID;
            ViewBag.Color_Exterior_ID_StockList = Color_Exterior_ID;
            ViewBag.PurchaseStartDate_StockList = PurchaseStartDate;
            ViewBag.PurchaseEndDate_StockList = PurchaseEndDate;
            ViewBag.BL_NO_StockList = BL_NO;
            ViewBag.BOE_StockList = BOE;
            ViewBag.PurchaseRef_StockList = PurchaseRef;
            ViewBag.SaleRef_StockList = SaleRef;
            ViewBag.c_ID_StockList = c_ID;
            ViewBag.Stock_Status_StockList = Stock_Status;
            ViewBag.loc_ID_StockList = loc_ID;
            ViewBag.VendorName_StockList = VendorName;
            ViewBag.ModelYear_StockList = ModelYear;
            ViewBag.Container_No_StockList = Container_No;
            ViewBag.StockType_ID_StockList = StockType_ID;

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.AttachmentVisibility_Only_Stock = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility_Only_Stock").Value;
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;

            try
            {
                //  _oStock.StockListObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, c_ID);

                _oStock.StockListPagedObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList().ToPagedList(page ?? 1, 10);

                _oStock.StockList_TTL = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();


                _oStock.StockListStats = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);


                return PartialView("_StockListSearchPartial_im", _oStock);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }

        #endregion
        #region StockList-Sales
        [HttpGet]

        public IActionResult StockList_Sales(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
     string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID, int? page)
        {
            //---Dumping static fields


            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "StockList";


            ViewBag.SectionHeaderTitle = "Stock";
            ViewBag.SectionSub_HeaderTitle = "Stock List";
            ViewBag.PageTitle = "Stock List";


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
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_IDs).Where(x => x.VendorCat_ID == 2).ToList();
            //---Make list for drop down in forms
            ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_IDs).ToList();
            //---Get Colors List
            ViewBag.ColorsList = _oBasic.Get_ColorsList_DAL().ToList();
            //---Get Location List
            ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_IDs).ToList();
            //---Get Status List
            ViewBag.Status = _oStock.get_Stock_Status_DAL().ToList();

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.AttachmentVisibility_Only_Stock = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility_Only_Stock").Value;
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;

            #endregion veiwbags area


            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;

            c_ID = c_ID == 0 ? c_IDs : c_ID;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            BL_NO = String.IsNullOrEmpty(BL_NO) ? "" : BL_NO;
            BOE = String.IsNullOrEmpty(BOE) ? "" : BOE;
            loc_ID = loc_ID == null ? 0 : loc_ID;
            Stock_Status = String.IsNullOrEmpty(Stock_Status) ? "" : Stock_Status;
            VendorName = String.IsNullOrEmpty(VendorName) ? "" : VendorName;
            ModelYear = String.IsNullOrEmpty(ModelYear) ? "" : ModelYear;
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;
            StockType_ID = StockType_ID == null ? 0 : StockType_ID;

            ViewBag.ChassisNo_StockList = ChassisNo;
            ViewBag.Make_ID_StockList = Make_ID;
            ViewBag.MakeModel_description_ID_StockList = MakeModel_description_ID;
            ViewBag.Color_Exterior_ID_StockList = Color_Exterior_ID;
            ViewBag.PurchaseStartDate_StockList = PurchaseStartDate;
            ViewBag.PurchaseEndDate_StockList = PurchaseEndDate;
            ViewBag.BL_NO_StockList = BL_NO;
            ViewBag.BOE_StockList = BOE;
            ViewBag.PurchaseRef_StockList = PurchaseRef;
            ViewBag.SaleRef_StockList = SaleRef;
            ViewBag.c_ID_StockList = c_ID;
            ViewBag.Stock_Status_StockList = Stock_Status;
            ViewBag.loc_ID_StockList = loc_ID;
            ViewBag.VendorName_StockList = VendorName;
            ViewBag.ModelYear_StockList = ModelYear;
            ViewBag.Container_No_StockList = Container_No;
            ViewBag.StockType_ID_StockList = StockType_ID;
            //---list for page
            _oStock.StockListPagedObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList().ToPagedList(page ?? 1, 10);

            _oStock.StockList_TTL = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();


            _oStock.StockListStats = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);


            return View(_oStock);
        }

        //---Get purchase list by search filters
        [HttpGet]
        public IActionResult StockListBySearchFilers_Sales(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
            string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID, int? page)
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

            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;
            c_ID = c_ID == 0 ? 1 : c_ID;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            BL_NO = String.IsNullOrEmpty(BL_NO) ? "" : BL_NO;
            BOE = String.IsNullOrEmpty(BOE) ? "" : BOE;
            loc_ID = loc_ID == null ? 0 : loc_ID;
            Stock_Status = String.IsNullOrEmpty(Stock_Status) ? "" : Stock_Status;
            VendorName = String.IsNullOrEmpty(VendorName) ? "" : VendorName;
            ModelYear = String.IsNullOrEmpty(ModelYear) ? "" : ModelYear;
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;
            StockType_ID = StockType_ID == null ? 0 : StockType_ID;


            ViewBag.ChassisNo_StockList = ChassisNo;
            ViewBag.Make_ID_StockList = Make_ID;
            ViewBag.MakeModel_description_ID_StockList = MakeModel_description_ID;
            ViewBag.Color_Exterior_ID_StockList = Color_Exterior_ID;
            ViewBag.PurchaseStartDate_StockList = PurchaseStartDate;
            ViewBag.PurchaseEndDate_StockList = PurchaseEndDate;
            ViewBag.BL_NO_StockList = BL_NO;
            ViewBag.BOE_StockList = BOE;
            ViewBag.PurchaseRef_StockList = PurchaseRef;
            ViewBag.SaleRef_StockList = SaleRef;
            ViewBag.c_ID_StockList = c_ID;
            ViewBag.Stock_Status_StockList = Stock_Status;
            ViewBag.loc_ID_StockList = loc_ID;
            ViewBag.VendorName_StockList = VendorName;
            ViewBag.ModelYear_StockList = ModelYear;
            ViewBag.Container_No_StockList = Container_No;
            ViewBag.StockType_ID_StockList = StockType_ID;

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.AttachmentVisibility_Only_Stock = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility_Only_Stock").Value;
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;

            try
            {
                //  _oStock.StockListObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, c_ID);

                _oStock.StockListPagedObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList().ToPagedList(page ?? 1, 10);

                _oStock.StockList_TTL = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();


                _oStock.StockListStats = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);


                return PartialView("StockListBySearchFilers_Sales", _oStock);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }

        #endregion
        #region carpoint
        [HttpGet]

        public IActionResult StockList_cp(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
         string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID, int? page)
        {
            //---Dumping static fields
            
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "StockList";


            ViewBag.SectionHeaderTitle = "Stock";
            ViewBag.SectionSub_HeaderTitle = "Stock List";
            ViewBag.PageTitle = "Stock List";


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string Roll_ID = HttpContext.Session.GetString("Roll_ID");
            if (Roll_ID != "1")
            {
                return RedirectToAction("VehiclesDashboard", "Home");
            }
            #endregion
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            #region Veiwbags area
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_IDs).Where(x => x.VendorCat_ID == 2).ToList();
            //---Make list for drop down in forms
            ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_IDs).ToList();
            //---Get Colors List
            ViewBag.ColorsList = _oBasic.Get_ColorsList_DAL().ToList();
            //---Get Location List
            ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_IDs).ToList();
            //---Get Status List
            ViewBag.Status = _oStock.get_Stock_Status_DAL().ToList();

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.AttachmentVisibility_Only_Stock = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility_Only_Stock").Value;
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;

            #endregion veiwbags area


            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;

            c_ID = c_ID == 0 ? c_IDs : c_ID;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            BL_NO = String.IsNullOrEmpty(BL_NO) ? "" : BL_NO;
            BOE = String.IsNullOrEmpty(BOE) ? "" : BOE;
            loc_ID = loc_ID == null ? 0 : loc_ID;
            Stock_Status = String.IsNullOrEmpty(Stock_Status) ? "" : Stock_Status;
            VendorName = String.IsNullOrEmpty(VendorName) ? "" : VendorName;
            ModelYear = String.IsNullOrEmpty(ModelYear) ? "" : ModelYear;
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;
            StockType_ID = StockType_ID == null ? 0 : StockType_ID;

            ViewBag.ChassisNo_StockList = ChassisNo;
            ViewBag.Make_ID_StockList = Make_ID;
            ViewBag.MakeModel_description_ID_StockList = MakeModel_description_ID;
            ViewBag.Color_Exterior_ID_StockList = Color_Exterior_ID;
            ViewBag.PurchaseStartDate_StockList = PurchaseStartDate;
            ViewBag.PurchaseEndDate_StockList = PurchaseEndDate;
            ViewBag.BL_NO_StockList = BL_NO;
            ViewBag.BOE_StockList = BOE;
            ViewBag.PurchaseRef_StockList = PurchaseRef;
            ViewBag.SaleRef_StockList = SaleRef;
            ViewBag.c_ID_StockList = c_ID;
            ViewBag.Stock_Status_StockList = Stock_Status;
            ViewBag.loc_ID_StockList = loc_ID;
            ViewBag.VendorName_StockList = VendorName;
            ViewBag.ModelYear_StockList = ModelYear;
            ViewBag.Container_No_StockList = Container_No;
            ViewBag.StockType_ID_StockList = StockType_ID;
            //---list for page
            _oStock.StockListPagedObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList().ToPagedList(page ?? 1, 10);

            _oStock.StockList_TTL = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();


            _oStock.StockListStats = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);


            return View(_oStock);
        }

        //---Get purchase list by search filters
        [HttpGet]
        public IActionResult StockListBySearchFilers_cp(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
            string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID, int? page)
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

            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;
            c_ID = c_ID == 0 ? 1 : c_ID;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            BL_NO = String.IsNullOrEmpty(BL_NO) ? "" : BL_NO;
            BOE = String.IsNullOrEmpty(BOE) ? "" : BOE;
            loc_ID = loc_ID == null ? 0 : loc_ID;
            Stock_Status = String.IsNullOrEmpty(Stock_Status) ? "" : Stock_Status;
            VendorName = String.IsNullOrEmpty(VendorName) ? "" : VendorName;
            ModelYear = String.IsNullOrEmpty(ModelYear) ? "" : ModelYear;
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;
            StockType_ID = StockType_ID == null ? 0 : StockType_ID;


            ViewBag.ChassisNo_StockList = ChassisNo;
            ViewBag.Make_ID_StockList = Make_ID;
            ViewBag.MakeModel_description_ID_StockList = MakeModel_description_ID;
            ViewBag.Color_Exterior_ID_StockList = Color_Exterior_ID;
            ViewBag.PurchaseStartDate_StockList = PurchaseStartDate;
            ViewBag.PurchaseEndDate_StockList = PurchaseEndDate;
            ViewBag.BL_NO_StockList = BL_NO;
            ViewBag.BOE_StockList = BOE;
            ViewBag.PurchaseRef_StockList = PurchaseRef;
            ViewBag.SaleRef_StockList = SaleRef;
            ViewBag.c_ID_StockList = c_ID;
            ViewBag.Stock_Status_StockList = Stock_Status;
            ViewBag.loc_ID_StockList = loc_ID;
            ViewBag.VendorName_StockList = VendorName;
            ViewBag.ModelYear_StockList = ModelYear;
            ViewBag.Container_No_StockList = Container_No;
            ViewBag.StockType_ID_StockList = StockType_ID;

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.AttachmentVisibility_Only_Stock = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility_Only_Stock").Value;
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;

            try
            {
                //  _oStock.StockListObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, c_ID);

                _oStock.StockListPagedObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList().ToPagedList(page ?? 1, 10);

                _oStock.StockList_TTL = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();


                _oStock.StockListStats = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);


                return PartialView("_StockListSearchPartial_cp", _oStock);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }


        #endregion
        #region hscjapanstocklist
        [HttpGet]

        public IActionResult StockList_JP(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
    string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No,int StockType_ID, int? page)
        {
            //---Dumping static fields


            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "StockList";


            ViewBag.SectionHeaderTitle = "Stock";
            ViewBag.SectionSub_HeaderTitle = "Stock List";
            ViewBag.PageTitle = "Stock List";


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
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_IDs).Where(x => x.VendorCat_ID == 3).ToList();
            //---Make list for drop down in forms
            ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_IDs).ToList();
            //---Get Colors List
            ViewBag.ColorsList = _oBasic.Get_ColorsList_DAL().ToList();
            //---Get Location List
            ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_IDs).ToList();
            //---Get Status List
            ViewBag.Status = _oStock.get_Stock_Status_DAL().ToList();
            //---Get StockType
            ViewBag.StockType = _oBasic.Get_StockTypeList_DAL().ToList();

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.AttachmentVisibility_Only_Stock = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility_Only_Stock").Value;
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;

            #endregion veiwbags area


            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;

            c_ID = c_ID == 0 ? c_IDs : c_ID;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            BL_NO = String.IsNullOrEmpty(BL_NO) ? "" : BL_NO;
            BOE = String.IsNullOrEmpty(BOE) ? "" : BOE;
            loc_ID = loc_ID == null ? 0 : loc_ID;
            StockType_ID = StockType_ID == null ? 0 : StockType_ID;
            Stock_Status = String.IsNullOrEmpty(Stock_Status) ? "" : Stock_Status;
            VendorName = String.IsNullOrEmpty(VendorName) ? "" : VendorName;
            ModelYear = String.IsNullOrEmpty(ModelYear) ? "" : ModelYear;
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;

            ViewBag.ChassisNo_StockList = ChassisNo;
            ViewBag.Make_ID_StockList = Make_ID;
            ViewBag.MakeModel_description_ID_StockList = MakeModel_description_ID;
            ViewBag.Color_Exterior_ID_StockList = Color_Exterior_ID;
            ViewBag.PurchaseStartDate_StockList = PurchaseStartDate;
            ViewBag.PurchaseEndDate_StockList = PurchaseEndDate;
            ViewBag.BL_NO_StockList = BL_NO;
            ViewBag.BOE_StockList = BOE;
            ViewBag.PurchaseRef_StockList = PurchaseRef;
            ViewBag.SaleRef_StockList = SaleRef;
            ViewBag.c_ID_StockList = c_ID;
            ViewBag.Stock_Status_StockList = Stock_Status;
            ViewBag.loc_ID_StockList = loc_ID;
            ViewBag.VendorName_StockList = VendorName;
            ViewBag.ModelYear_StockList = ModelYear;
            ViewBag.Container_No_StockList = Container_No;
            ViewBag.StockType_ID_StockList = StockType_ID;

            //---list for page
            _oStock.StockListPagedObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList().ToPagedList(page ?? 1, 10);

            _oStock.StockList_TTL = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();


            _oStock.StockListStats = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);


            return View(_oStock);
        }


        #region StockList_fm

        [HttpGet]

        public IActionResult StockList_fm(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
    string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID, int? page)
        {
            //---Dumping static fields


            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "StockList";


            ViewBag.SectionHeaderTitle = "Stock";
            ViewBag.SectionSub_HeaderTitle = "Stock List";
            ViewBag.PageTitle = "Stock List";


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
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_IDs).Where(x => x.VendorCat_ID == 3).ToList();
            //---Make list for drop down in forms
            ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_IDs).ToList();
            //---Get Colors List
            ViewBag.ColorsList = _oBasic.Get_ColorsList_DAL().ToList();
            //---Get Location List
            ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_IDs).ToList();
            //---Get Status List
            ViewBag.Status = _oStock.get_Stock_Status_DAL().ToList();
            //---Get StockType
            ViewBag.StockType = _oBasic.Get_StockTypeList_DAL().ToList();

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.AttachmentVisibility_Only_Stock = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility_Only_Stock").Value;
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;

            #endregion veiwbags area


            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;

            c_ID = c_ID == 0 ? c_IDs : c_ID;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            BL_NO = String.IsNullOrEmpty(BL_NO) ? "" : BL_NO;
            BOE = String.IsNullOrEmpty(BOE) ? "" : BOE;
            loc_ID = loc_ID == null ? 0 : loc_ID;
            StockType_ID = StockType_ID == null ? 0 : StockType_ID;
            Stock_Status = String.IsNullOrEmpty(Stock_Status) ? "" : Stock_Status;
            VendorName = String.IsNullOrEmpty(VendorName) ? "" : VendorName;
            ModelYear = String.IsNullOrEmpty(ModelYear) ? "" : ModelYear;
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;

            ViewBag.ChassisNo_StockList = ChassisNo;
            ViewBag.Make_ID_StockList = Make_ID;
            ViewBag.MakeModel_description_ID_StockList = MakeModel_description_ID;
            ViewBag.Color_Exterior_ID_StockList = Color_Exterior_ID;
            ViewBag.PurchaseStartDate_StockList = PurchaseStartDate;
            ViewBag.PurchaseEndDate_StockList = PurchaseEndDate;
            ViewBag.BL_NO_StockList = BL_NO;
            ViewBag.BOE_StockList = BOE;
            ViewBag.PurchaseRef_StockList = PurchaseRef;
            ViewBag.SaleRef_StockList = SaleRef;
            ViewBag.c_ID_StockList = c_ID;
            ViewBag.Stock_Status_StockList = Stock_Status;
            ViewBag.loc_ID_StockList = loc_ID;
            ViewBag.VendorName_StockList = VendorName;
            ViewBag.ModelYear_StockList = ModelYear;
            ViewBag.Container_No_StockList = Container_No;
            ViewBag.StockType_ID_StockList = StockType_ID;

            //---list for page
            _oStock.StockListPagedObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList().ToPagedList(page ?? 1, 10);

            _oStock.StockList_TTL = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();


            _oStock.StockListStats = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);


            return View(_oStock);
        }


        //---Get purchase list by search filters
        [HttpGet]
        public IActionResult StockListBySearchFilers_fm(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
            string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID, int? page)
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

            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;
            c_ID = c_ID == 0 ? 1 : c_ID;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            BL_NO = String.IsNullOrEmpty(BL_NO) ? "" : BL_NO;
            BOE = String.IsNullOrEmpty(BOE) ? "" : BOE;
            loc_ID = loc_ID == null ? 0 : loc_ID;
            Stock_Status = String.IsNullOrEmpty(Stock_Status) ? "" : Stock_Status;
            VendorName = String.IsNullOrEmpty(VendorName) ? "" : VendorName;
            ModelYear = String.IsNullOrEmpty(ModelYear) ? "" : ModelYear;
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;
            StockType_ID = StockType_ID == null ? 0 : StockType_ID;


            ViewBag.ChassisNo_StockList = ChassisNo;
            ViewBag.Make_ID_StockList = Make_ID;
            ViewBag.MakeModel_description_ID_StockList = MakeModel_description_ID;
            ViewBag.Color_Exterior_ID_StockList = Color_Exterior_ID;
            ViewBag.PurchaseStartDate_StockList = PurchaseStartDate;
            ViewBag.PurchaseEndDate_StockList = PurchaseEndDate;
            ViewBag.BL_NO_StockList = BL_NO;
            ViewBag.BOE_StockList = BOE;
            ViewBag.PurchaseRef_StockList = PurchaseRef;
            ViewBag.SaleRef_StockList = SaleRef;
            ViewBag.c_ID_StockList = c_ID;
            ViewBag.Stock_Status_StockList = Stock_Status;
            ViewBag.loc_ID_StockList = loc_ID;
            ViewBag.VendorName_StockList = VendorName;
            ViewBag.ModelYear_StockList = ModelYear;
            ViewBag.Container_No_StockList = Container_No;
            ViewBag.StockType_ID_StockLis = StockType_ID;



            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.AttachmentVisibility_Only_Stock = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility_Only_Stock").Value;
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;

            try
            {
                //  _oStock.StockListObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, c_ID);

                _oStock.StockListPagedObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList().ToPagedList(page ?? 1, 10);

                _oStock.StockList_TTL = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();


                _oStock.StockListStats = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);


                return PartialView("_StockListSearchPartial_fm", _oStock);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }


        #endregion


        //---Get purchase list by search filters
        [HttpGet]
        public IActionResult StockListBySearchFilers_JP(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
            string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID, int? page)
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

            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;
            c_ID = c_ID == 0 ? 1 : c_ID;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            BL_NO = String.IsNullOrEmpty(BL_NO) ? "" : BL_NO;
            BOE = String.IsNullOrEmpty(BOE) ? "" : BOE;
            loc_ID = loc_ID == null ? 0 : loc_ID;
            Stock_Status = String.IsNullOrEmpty(Stock_Status) ? "" : Stock_Status;
            VendorName = String.IsNullOrEmpty(VendorName) ? "" : VendorName;
            ModelYear = String.IsNullOrEmpty(ModelYear) ? "" : ModelYear;
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;
            StockType_ID = StockType_ID == null ? 0 : StockType_ID;

            
            ViewBag.ChassisNo_StockList = ChassisNo;
            ViewBag.Make_ID_StockList = Make_ID;
            ViewBag.MakeModel_description_ID_StockList = MakeModel_description_ID;
            ViewBag.Color_Exterior_ID_StockList = Color_Exterior_ID;
            ViewBag.PurchaseStartDate_StockList = PurchaseStartDate;
            ViewBag.PurchaseEndDate_StockList = PurchaseEndDate;
            ViewBag.BL_NO_StockList = BL_NO;
            ViewBag.BOE_StockList = BOE;
            ViewBag.PurchaseRef_StockList = PurchaseRef;
            ViewBag.SaleRef_StockList = SaleRef;
            ViewBag.c_ID_StockList = c_ID;
            ViewBag.Stock_Status_StockList = Stock_Status;
            ViewBag.loc_ID_StockList = loc_ID;
            ViewBag.VendorName_StockList = VendorName;
            ViewBag.ModelYear_StockList = ModelYear;
            ViewBag.Container_No_StockList = Container_No; 
            ViewBag.StockType_ID_StockLis = StockType_ID;



            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.AttachmentVisibility_Only_Stock = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility_Only_Stock").Value;
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;

            try
            {
                //  _oStock.StockListObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, c_ID);

                _oStock.StockListPagedObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList().ToPagedList(page ?? 1, 10);

                _oStock.StockList_TTL = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();


                _oStock.StockListStats = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);


                return PartialView("_StockListSearchPartial_JP", _oStock);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }

        #endregion
        [HttpGet]
        public JsonResult GetVendorDetail(int? Vendor_ID)
        {
            string ContactNumber = "";
            string ContactAddress = "";
     


            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function
            _oBasic.GetVendorObj = _oBasic.GetVendorDetail_DAL(Vendor_ID);

            if (!String.IsNullOrEmpty(_oBasic.GetVendorObj.PartnerName))
            {

                ContactNumber = _oBasic.GetVendorObj.ContactNo;
                ContactAddress = _oBasic.GetVendorObj.ContactAddress;
                

            }
            else
            {
                ContactNumber = null;
                ContactAddress = null;
            
            }


            return Json(new { ContactNumber, ContactAddress });

        }
        #region Notification Update

        [HttpGet]
  
        public IActionResult UpdateShipping_infoAlert(int Shipping_info_ID)
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
         
     
            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();






            Shipping_info_ID = Shipping_info_ID == null ? 0 : Shipping_info_ID;


            try
            {
                message = _oStock.Update_Shipping_Alert_DAL(Shipping_info_ID);
                if (message.Contains("Successfully!"))
                {

                  
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
        //---This method is for Insert_PlateNumberDetail
        [HttpPost]
        public IActionResult Insert_PlateNumberDetail(string stock_ID, string PlateNumberfee, string refundAmount, string cancel_Date, string refund_date, string recieved_Date, string Account_Debit_ID,
     string Status, string AdjustedPurchase_ref)
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
 
            string Create_by =UserID_Admin;
     
      
            VanningMasterNewStatic_ID = VanningMasterNewStatic_ID == null ? 0 : VanningMasterNewStatic_ID;

            if (string.IsNullOrEmpty(stock_ID))
            {
                return Json(new { message = "Some Thing Went Wrong!" });
            }

            if (string.IsNullOrEmpty(AdjustedPurchase_ref))
            {
                return Json(new { message = "Please Adjust Purchase Ref" });

            }
     

            int user_ID = Convert.ToInt32(UserID_Admin);
            try
            {
                message = _oStock.Insert_Shaken_DAL( stock_ID, PlateNumberfee,  refundAmount,  cancel_Date,  refund_date,  recieved_Date,  Account_Debit_ID,
      Status,  AdjustedPurchase_ref, Create_by, Create_by);
                if (message.Contains("Successfully"))
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


            //return new PartialViewResult
            //{
            //    ViewName = "_VanningDetailPartial",
            //    ViewData = new ViewDataDictionary
            //<List<Pa_VanningDetails_DAL>>(ViewData, _oStock.VanningDetailList)
            //};
        }


        #region  Bill of Lading

        [HttpGet]

        public IActionResult BLList(string BLNO, string StartDate, string EndDate, int? page)
        {
            //---Dumping static fields

            DumpStaticFieldsBL();
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "BLList";


            ViewBag.SectionHeaderTitle = "Stock";
            ViewBag.SectionSub_HeaderTitle = "BL List";
            ViewBag.PageTitle = "BL List";


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

            BLNO = String.IsNullOrEmpty(BLNO) ? "" : BLNO;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            
            
            

            ViewBag.BLNO = BLNO;
                       
            ViewBag.From_Date = StartDate;
            ViewBag.To_Date = EndDate;
            
            
            //---list for page

            

           _oStock.BLMasterPlist = _oStock.Get_BlList_DAL(BLNO, StartDate, EndDate, c_ID).ToList().ToPagedList(page ?? 1, 10);

            return View(_oStock);
        }


        [HttpGet]

        public IActionResult BLList_Search(string BLNO, string StartDate, string EndDate, int? page)
        {
            //---Dumping static fields

            DumpStaticFieldsBL();
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "BLList";


            ViewBag.SectionHeaderTitle = "Stock";
            ViewBag.SectionSub_HeaderTitle = "BL List";
            ViewBag.PageTitle = "BL List";


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

            BLNO = String.IsNullOrEmpty(BLNO) ? "" : BLNO;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;




            ViewBag.BLNO = BLNO;
            ViewBag.From_Date = StartDate;
            ViewBag.To_Date = EndDate;


            //---list for page

            _oStock.BLMasterPlist = _oStock.Get_BlList_DAL(BLNO, StartDate, EndDate, c_ID).ToList().ToPagedList(page ?? 1, 10);

            return PartialView("_BLList_Search", _oStock);
        }

        public void DumpStaticFieldsBL()
        {
            BLMaster_TempID = null;
            BLMasterStatic_ID = null;
        }

        [HttpGet]
        public IActionResult AddBL(int BLNO_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "AddBL";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(UserName_Admin) || string.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).Where(x => x.VendorCat_ID == 2).ToList();


            #region ViewBag Area

            _oStock.StockList = _oStock.Get_AllStock_DAL().ToList();
            #endregion
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();
            ViewBag.Default_Curr_Rate = "1";
            ViewBag.Default_Currency_ID = "1";
            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;


            if (BLNO_ID == 0 || BLNO_ID < 1)
            {
               
                //---Create payment master Ref(tem id)
                if (string.IsNullOrEmpty(BLMaster_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    BLMaster_TempID = obj.ToString();
                }

                BLMasterStatic_ID = BLNO_ID;

                //---Get Stock master id
                ViewBag.BL_ID = BLNO_ID;

                //--Get Stock Master
                _oStock.BLMasterObj = _oStock.Get_BLMasterByID_DAL(-1);




                //---Stock detial invoice list for Stock invoice type Bychassis
                _oStock.BLDetailList = _oStock.Get_BLDetailListBy_DAL(BLMaster_TempID, 0).ToList();


            }
            else
            {
                Urls = null;

                ApprovalURL = HttpContext.Request.Headers["Referer"];
                Urls = ApprovalURL;
                //---Get old temp id from Stock detail table that already creaded for this "Auction_ID" id. And then use this if new record added
                string OldTempID = _oStock.GetOldTempIDFromBLDetail_DAL(BLNO_ID);


                BLMasterStatic_ID = BLNO_ID;

                //---Get Stock master id
                ViewBag.BL_ID = BLNO_ID;

                //--Get sale Master

                _oStock.BLMasterObj = _oStock.Get_BLMasterByID_DAL(BLNO_ID);
                //---Stock detial invoice list for Stock invoice type Bychassis
                _oStock.BLDetailList = _oStock.Get_BLDetailListBy_DAL(OldTempID, BLNO_ID).ToList();
                
            }



            return View(_oStock);
        }

        //---insert into Stock invoice detial
        [HttpPost]
        public IActionResult InsertBLDetail(int? HdBL_ID, int[] Stock_ID)
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


            if (Stock_ID == null)
            {
                return Json(new { message = "Please enter Stock!" });
            }
            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = BLMaster_TempID;


            if (HdBL_ID == 0)
            {
                Temp_ID = BLMaster_TempID;
            }
           
            else
            {
                Temp_ID = "0";
            }

            try
            {
                foreach (var item in Stock_ID)
                {
                    try
                    {
                        message = _oStock.InsertBLDetail_DAL(HdBL_ID, item, Temp_ID);

                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        return Json(new { message = ErrorMessage });
                    }

                }


                if (message == "Added Successfully!")
                {
                    //---Stock detial invoice list for Stock invoice type Bychassis
                    _oStock.BLDetailList = _oStock.Get_BLDetailListBy_DAL(Temp_ID, HdBL_ID).ToList();

                    return PartialView("_BLDetailList", _oStock);
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
        public IActionResult InsertBLMaster(string ShipName, string ShipLeavingDate, string Arrival_Date, string Clearing_Charges, string Custom_Duty, string Other_Charage, string BLNO_Date, string Remarks,string Total_BL_Charges, int Currency_ID, string Currency_Rate,string Total_With_Rate, int? Vendor_ID, string BlNO)
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
            Temp_ID = BLMaster_TempID;

            if (String.IsNullOrEmpty(BLNO_Date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("AddBL", "Stock");
            }

            try
            {
                message = _oStock.InsertBLMaster_DAL(ShipName,ShipLeavingDate,Arrival_Date,Clearing_Charges,Custom_Duty,Other_Charage , BLNO_Date, Remarks, Total_BL_Charges ,
      Temp_ID, c_ID, Created_by, Currency_ID,  Currency_Rate, Total_With_Rate,Vendor_ID, BlNO);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("BLList", "Stock");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("AddBL", "Stock");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
                return RedirectToAction("AddBL", "Stock");
            }


        }

        //---Insert  Stock invoice master
        [HttpPost]
        public IActionResult UpdateBLMaster(string ShipName, string ShipLeavingDate, string Arrival_Date, string Clearing_Charges, string Custom_Duty, string Other_Charage, string BLNO_Date, string Remarks, string Total_BL_Charges, int? BLNO_ID, int Currency_ID, string Currency_Rate,string Total_With_Rate,int? Vendor_ID, string BlNO
         )
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
            Temp_ID = BLMaster_TempID;

            string OldTempID = _oStock.GetOldTempIDFromBLDetail_DAL(BLNO_ID);

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


            if (String.IsNullOrEmpty(BLNO_Date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }



            try
            {
                message = _oStock.UpdateBLMaster_DAL(ShipName, ShipLeavingDate,Arrival_Date,Clearing_Charges,Custom_Duty,Other_Charage , BLNO_Date, Remarks, Total_BL_Charges ,BLNO_ID,
           Temp_ID, c_ID, Modified_by, Currency_ID,Currency_Rate, Total_With_Rate,Vendor_ID, BlNO);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("BLList", "Stock");
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

        //---Method for deleting Stock invoice detail
        [HttpPost]
        public IActionResult DeleteBLDetail(int? BLDetail_ID, int HdBL_ID)
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
            string Temp_ID = "";
            if (BLDetail_ID == null)
            {

                return Json(new { message = "ID is null. Please try again" });
            }

            if (HdBL_ID == 0)
            {
                Temp_ID = BLMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            try
            {
                message = _oStock.DeleteBLDetail_DAL(BLDetail_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = message;
                    _oStock.BLDetailList = _oStock.Get_BLDetailListBy_DAL(Temp_ID, HdBL_ID).ToList();

                    return PartialView("_BLDetailList", _oStock);

                }
                else
                {

                    return Json(new { message = message });
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

        //By Yaseen 1-14-2022
        #region WorkShop 

        public void DumpStaticFieldsGarage()
        {
            GarageMaster_TempID = null;
            GarageMasterStatic_ID = null;

        }


        //---Method for deleting Stock invoice detail
        [HttpPost]
        public IActionResult DeleteGarageDetail(int? Garage_Details_ID, int HdGarageMaster_ID)
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
            string Temp_ID = "";
            if (Garage_Details_ID == null)
            {

                return Json(new { message = "ID is null. Please try again" });
            }

            if (HdGarageMaster_ID == 0)
            {
                Temp_ID = GarageMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            try
            {
                message = _oStock.DeleteGarageDetail_DAL(Garage_Details_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = message;

                    _oStock.GarageDetailList = _oStock.Get_GarageDetailListBy_DAL(Temp_ID, HdGarageMaster_ID).ToList();

                    return PartialView("_GarageDetailsList", _oStock);

                }
                else
                {

                    return Json(new { message = message });
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
        public IActionResult InsertGarageMaster(string Date_Send, string Remarks ,int? Vendor_ID)
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
            Temp_ID = GarageMaster_TempID;


           

            if (String.IsNullOrEmpty(Date_Send))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("AddGarage", "Stock");
            }

            try
            {
                message = _oStock.InsertGarageMaster_DAL(Date_Send, Remarks,Vendor_ID,
      Temp_ID, c_ID, Created_by);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("GarageList", "Stock");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("AddGarage", "Stock");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
                return RedirectToAction("AddGarage", "Stock");
            }


        }

        [HttpPost]
        public IActionResult UpdateGarageMaster(string Date_Send, string Remarks,int? Vendor_ID, int GarageMaster_ID)
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
            Temp_ID = GarageMaster_TempID;

            if (String.IsNullOrEmpty(Date_Send))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("AddGarage", "Stock");
            }

            try
            {
                message = _oStock.UpdateGarageMaster_DAL(Date_Send, Remarks, Vendor_ID, GarageMaster_ID,
      Temp_ID, c_ID, Created_by);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("GarageList", "Stock");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("AddGarage", "Stock");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
                return RedirectToAction("AddGarage", "Stock");
            }


        }


        [HttpGet]

        public IActionResult GarageList()
        {
            //---Dumping static fields

            DumpStaticFieldsGarage();
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "GarageList";


            ViewBag.SectionHeaderTitle = "Stock";
            ViewBag.SectionSub_HeaderTitle = "Garage List";
            ViewBag.PageTitle = "Garage List";


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion



            //---list for page

            _oStock.GarageMasterList = _oStock.Get_GarageList_DAL().ToList();

            return View(_oStock);

        }


        [HttpGet]
        public IActionResult AddGarage(int GarageMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "AddGarage";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(UserName_Admin) || string.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion



            #region ViewBag Area
            _oStock.StockList = _oStock.Get_AllStock_DAL().ToList();
            #endregion

            //---Get Vendor Master
            
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).Where(x => x.PartnerType == "GR").ToList();

            if (GarageMaster_ID == 0 || GarageMaster_ID < 1)
            {

                //---Create payment master Ref(tem id)
                if (string.IsNullOrEmpty(GarageMaster_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    GarageMaster_TempID = obj.ToString();
                }

                GarageMasterStatic_ID = GarageMaster_ID;

                //---Get Stock master id
                ViewBag.Garage_Master_ID = GarageMaster_ID;

                //--Get Stock Master
                _oStock.GarageMasterObj = _oStock.Get_GarageMasterByID_DAL(-1);

                //---Stock detial invoice list for Stock invoice type Bychassis
               _oStock.GarageDetailList = _oStock.Get_GarageDetailListBy_DAL(GarageMaster_TempID, 0).ToList();


            }
            else
            {

                //---Get old temp id from Stock detail table that already creaded for this "Auction_ID" id. And then use this if new record added
                string OldTempID = _oStock.GetOldTempIDFromGarageDetail_DAL(GarageMaster_ID);

                GarageMasterStatic_ID = GarageMaster_ID;

                //---Get Stock master id
                ViewBag.Garage_Master_ID = GarageMaster_ID;

                //--Get sale Master

                _oStock.GarageMasterObj = _oStock.Get_GarageMasterByID_DAL(GarageMaster_ID);
                //---Stock detial invoice list for Stock invoice type Bychassis
                _oStock.GarageDetailList = _oStock.Get_GarageDetailListBy_DAL(OldTempID, GarageMaster_ID).ToList();

            }



            return View(_oStock);
        }




        //---insert into Stock invoice detial
        [HttpPost]
        public IActionResult InsertGarageDetail(int? HdGarage_Master_ID, int[] Stock_ID,string[] Description,string[] Amount)
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


            if (Stock_ID == null)
            {
                return Json(new { message = "Please enter Stock!" });
            }
            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = GarageMaster_TempID;


            if (HdGarage_Master_ID == 0)
            {
                Temp_ID = GarageMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            try
            {
                foreach (var item in Stock_ID)
                {
                    foreach (var dec in Description)
                    {
                        foreach (var amnt in Amount)
                        {
                            try
                            {
                                message = _oStock.InsertGarageDetail_DAL(HdGarage_Master_ID, item, Temp_ID, dec, amnt);

                            }
                            catch (Exception ex)
                            {
                                string ErrorMessage = ex.Message;
                                return Json(new { message = ErrorMessage });
                            }
                        }
                        
                    }
                    
                    

                }


                if (message == "Added Successfully!")
                {
                    //---Stock detial invoice list for Stock invoice type Bychassis
                    _oStock.GarageDetailList = _oStock.Get_GarageDetailListBy_DAL(Temp_ID, HdGarage_Master_ID).ToList();

                    return PartialView("_GarageDetailsList", _oStock);
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
        public IActionResult ChangeBlnoStatus(int? Blno_ID, int? Status_ID)
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


            if (Blno_ID == null || Status_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oStock.ChangeBlnoStatus_DAL(Blno_ID, Status_ID, c_ID, Modified_by);
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

        #endregion
        //By Yaseen 1-14-2022





        [HttpGet]

        public IActionResult StockList_am(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
         string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID, int? page)
        {
            //---Dumping static fields

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Stock";
            ViewBag.BreadCumAction = "StockList";


            ViewBag.SectionHeaderTitle = "Stock";
            ViewBag.SectionSub_HeaderTitle = "Stock List";
            ViewBag.PageTitle = "Stock List";


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            //string Roll_ID = HttpContext.Session.GetString("Roll_ID");
            //if (Roll_ID != "1")
            //{
            //    return RedirectToAction("VehiclesDashboard", "Home");
            //}
            #endregion
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            #region Veiwbags area
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_IDs).Where(x => x.VendorCat_ID == 2).ToList();
            //---Make list for drop down in forms
            ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_IDs).ToList();
            //---Get Colors List
            ViewBag.ColorsList = _oBasic.Get_ColorsList_DAL().ToList();
            //---Get Location List
            ViewBag.CarLocations = _oBasic.Get_CarLocation_DAL(c_IDs).ToList();
            //---Get Status List
            ViewBag.Status = _oStock.get_Stock_Status_DAL().ToList();

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.AttachmentVisibility_Only_Stock = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility_Only_Stock").Value;
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;

            #endregion veiwbags area


            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;

            c_ID = c_ID == 0 ? c_IDs : c_ID;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            BL_NO = String.IsNullOrEmpty(BL_NO) ? "" : BL_NO;
            BOE = String.IsNullOrEmpty(BOE) ? "" : BOE;
            loc_ID = loc_ID == null ? 0 : loc_ID;
            Stock_Status = String.IsNullOrEmpty(Stock_Status) ? "" : Stock_Status;
            VendorName = String.IsNullOrEmpty(VendorName) ? "" : VendorName;
            ModelYear = String.IsNullOrEmpty(ModelYear) ? "" : ModelYear;
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;
            StockType_ID = StockType_ID == null ? 0 : StockType_ID;

            ViewBag.ChassisNo_StockList = ChassisNo;
            ViewBag.Make_ID_StockList = Make_ID;
            ViewBag.MakeModel_description_ID_StockList = MakeModel_description_ID;
            ViewBag.Color_Exterior_ID_StockList = Color_Exterior_ID;
            ViewBag.PurchaseStartDate_StockList = PurchaseStartDate;
            ViewBag.PurchaseEndDate_StockList = PurchaseEndDate;
            ViewBag.BL_NO_StockList = BL_NO;
            ViewBag.BOE_StockList = BOE;
            ViewBag.PurchaseRef_StockList = PurchaseRef;
            ViewBag.SaleRef_StockList = SaleRef;
            ViewBag.c_ID_StockList = c_ID;
            ViewBag.Stock_Status_StockList = Stock_Status;
            ViewBag.loc_ID_StockList = loc_ID;
            ViewBag.VendorName_StockList = VendorName;
            ViewBag.ModelYear_StockList = ModelYear;
            ViewBag.Container_No_StockList = Container_No;
            ViewBag.StockType_ID_StockList = StockType_ID;
            //---list for page
            _oStock.StockListPagedObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList().ToPagedList(page ?? 1, 20);

            _oStock.StockList_TTL = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();


            _oStock.StockListStats = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);


            return View(_oStock);
        }

        //---Get purchase list by search filters
        [HttpGet]
        public IActionResult StockListBySearchFilers_am(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
            string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID, int? page)
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

            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;
            c_ID = c_ID == 0 ? 1 : c_ID;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            BL_NO = String.IsNullOrEmpty(BL_NO) ? "" : BL_NO;
            BOE = String.IsNullOrEmpty(BOE) ? "" : BOE;
            loc_ID = loc_ID == null ? 0 : loc_ID;
            Stock_Status = String.IsNullOrEmpty(Stock_Status) ? "" : Stock_Status;
            VendorName = String.IsNullOrEmpty(VendorName) ? "" : VendorName;
            ModelYear = String.IsNullOrEmpty(ModelYear) ? "" : ModelYear;
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;
            StockType_ID = StockType_ID == null ? 0 : StockType_ID;


            ViewBag.ChassisNo_StockList = ChassisNo;
            ViewBag.Make_ID_StockList = Make_ID;
            ViewBag.MakeModel_description_ID_StockList = MakeModel_description_ID;
            ViewBag.Color_Exterior_ID_StockList = Color_Exterior_ID;
            ViewBag.PurchaseStartDate_StockList = PurchaseStartDate;
            ViewBag.PurchaseEndDate_StockList = PurchaseEndDate;
            ViewBag.BL_NO_StockList = BL_NO;
            ViewBag.BOE_StockList = BOE;
            ViewBag.PurchaseRef_StockList = PurchaseRef;
            ViewBag.SaleRef_StockList = SaleRef;
            ViewBag.c_ID_StockList = c_ID;
            ViewBag.Stock_Status_StockList = Stock_Status;
            ViewBag.loc_ID_StockList = loc_ID;
            ViewBag.VendorName_StockList = VendorName;
            ViewBag.ModelYear_StockList = ModelYear;
            ViewBag.Container_No_StockList = Container_No;
            ViewBag.StockType_ID_StockList = StockType_ID;

            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.AttachmentVisibility_Only_Stock = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility_Only_Stock").Value;
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;

            try
            {
                //  _oStock.StockListObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, c_ID);

                _oStock.StockListPagedObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList().ToPagedList(page ?? 1, 20);

                _oStock.StockList_TTL = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();


                _oStock.StockListStats = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);


                return PartialView("_StockListSearchPartial_am", _oStock);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }

    }
}
