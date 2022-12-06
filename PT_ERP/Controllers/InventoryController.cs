using DAL.oClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using X.PagedList;

namespace PT_ERP.Controllers
{
    public class InventoryController : BaseController
    {
        static string ProductFormulaMaster_TempID;
        static int? ProductFormulaMasterStatic_ID;

        static string ProductionMaster_TempID;
        static int? ProductionMasterStatic_ID;

        static string DismentalMaster_TempID;
        static int? DismentalMasterStatic_ID;

        static string AssemblyMaster_TempID;
        static int? AssemblyMasterStatic_ID;

        static string InventoryTransferMaster_TempID;
        static int? InventoryTransferMasterStatic_ID;

        private readonly IOBasicData _oBasic;
        private readonly IOInventory _oInventory;
        private IConfiguration configuration;
  
        public InventoryController(IOBasicData oBasicBase, IOInventory oInventory, IConfiguration iConfig)
           : base(oBasicBase)
        {
            _oBasic = oBasicBase;
            _oInventory = oInventory;
            configuration = iConfig;
        }


        #region ProductFormula

        [HttpGet]
        public IActionResult NewProductFormula(int Formula_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Inventory";
            ViewBag.BreadCumAction = "NewProductFormula";


            ViewBag.SectionHeaderTitle = "Dismental Parts";
            ViewBag.SectionSub_HeaderTitle = "Inventory";
            ViewBag.PageTitle = "Inventory";

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

            ViewBag.UOMList = _oBasic.Get_UOM_DAL(c_ID).ToList();
            //---Get Item master list
            ViewBag.ItemMaster = _oBasic.Get_ItemMasterList_DAL().ToList();
            _oInventory.InventoryFormulaRef = _oInventory.FormulaLoadRef();
            #endregion




            if (Formula_ID == 0 || Formula_ID < 1)
            {


                //---Create payment master Ref(tem id)
                if (string.IsNullOrEmpty(ProductFormulaMaster_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    ProductFormulaMaster_TempID = obj.ToString();
                }

                ProductFormulaMasterStatic_ID = Formula_ID;

                //---Get Inventory master id
                ViewBag.Formula_ID = Formula_ID;

                //--Get Inventory Master
                _oInventory.ProductFormulaMasterObj = _oInventory.Get_ProductFormulaMasterByID_DAL(-1);




                //---Inventory detial invoice list for Inventory invoice type Bychassis
                _oInventory.ProductFormulaDetailList = _oInventory.Get_ProductFormulaDetailListBy_DAL(ProductFormulaMaster_TempID, 0).ToList();


            }
            else
            {

                //---Get old temp id from Inventory detail table that already creaded for this "Formula_ID" id. And then use this if new record added
                string OldTempID = _oInventory.GetOldTempIDFromProductFormulaDetail_DAL(Formula_ID);




                ProductFormulaMasterStatic_ID = Formula_ID;

                //---Get sale master id
                ViewBag.Formula_ID = Formula_ID;

                //--Get sale Master

                _oInventory.ProductFormulaMasterObj = _oInventory.Get_ProductFormulaMasterByID_DAL(Formula_ID);

                //---Inventory detial invoice list for Inventory invoice type Bychassis
                _oInventory.ProductFormulaDetailList = _oInventory.Get_ProductFormulaDetailListBy_DAL(OldTempID, Formula_ID).ToList();



            }



            return View(_oInventory);
        }


        //---insert into Inventory invoice detial
        [HttpPost]
        public IActionResult InsertProductFormulaDetail(int? HdFormula_ID, string ItemID, string UOM, double? UnitPrice, double?
        Amount, string MajorQty, string MinorQty)
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


            if (ItemID == null)
            {
                return Json(new { message = "Please enter ItemID!" });
            }
            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = ProductFormulaMaster_TempID;

            UnitPrice = UnitPrice == null ? 0 : UnitPrice;
            Amount = Amount == null ? 0 : Amount;
            if (HdFormula_ID == 0)
            {
                Temp_ID = ProductFormulaMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            try
            {
                message = _oInventory.InsertProductFormulaDetail_DAL(HdFormula_ID, ItemID, UOM, UnitPrice,
        Amount, Temp_ID, MajorQty, MinorQty);
                if (message == "Added Successfully!")
                {
                    //---Inventory detial invoice list for Inventory invoice type Bychassis
                    _oInventory.ProductFormulaDetailList = _oInventory.Get_ProductFormulaDetailListBy_DAL(Temp_ID, HdFormula_ID).ToList();



                    return PartialView("_ProductFormulaDetailList", _oInventory);
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


        //---Update  Inventory invoice detail Chassis
        [HttpPost]
        public IActionResult UpdateProductFormulaDetailByChassis(int? ProductFormula_IN_ID, string ItemID, string UOM, double? UnitPrice, double?
        Amount, string MajorQty, string MinorQty, int? HdFormula_ID)
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

            if (ProductFormula_IN_ID == null)
            {
                return Json(new { message = "Inventory detail id is null" });
            }

            if (ItemID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            UnitPrice = UnitPrice == null ? 0 : UnitPrice;
            Amount = Amount == null ? 0 : Amount;

            Temp_ID = ProductFormulaMaster_TempID;
            if (HdFormula_ID == 0)
            {
                Temp_ID = ProductFormulaMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }
            ProductFormulaMasterStatic_ID = ProductFormulaMasterStatic_ID == null ? -1 : ProductFormulaMasterStatic_ID;


            try
            {
                message = _oInventory.UpdateProductFormulaDetail_DAL(ProductFormula_IN_ID, ItemID, UOM, UnitPrice,
        Amount, Temp_ID, MajorQty, MinorQty);
                if (message == "Added Successfully!")
                {
                    //---Inventory detial invoice list for Inventory invoice type Bychassis
                    _oInventory.ProductFormulaDetailList = _oInventory.Get_ProductFormulaDetailListBy_DAL(Temp_ID, HdFormula_ID).ToList();



                    return PartialView("_ProductFormulaDetailList", _oInventory);
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


        //---Insert  Inventory invoice master
        [HttpPost]
        public IActionResult InsertProductFormulaMaster(string P_Date, string Ref, string FormulaName, string Production_Details, string ItemID
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
            Temp_ID = ProductFormulaMaster_TempID;

            if (ItemID == null || String.IsNullOrEmpty(P_Date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewProductFormula", "Inventory");
            }


            ItemID = ItemID == null ? "0" : ItemID;

            try
            {
                message = _oInventory.InsertProductFormulaMaster_DAL(P_Date, Ref, FormulaName, Production_Details, ItemID,
      Temp_ID, c_ID, Created_by);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("ProductFormulaList", "Inventory");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("NewProductFormula", "Inventory");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
                return RedirectToAction("NewProductFormula", "Inventory");
            }


        }

        //---Insert  Inventory invoice master
        [HttpPost]
        public IActionResult UpdateProductFormulaMaster(string P_Date, string Ref, string FormulaName, string Production_Details, string ItemID, int? Formula_ID, string isActive
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
            Temp_ID = ProductFormulaMaster_TempID;
            string OldTempID = _oInventory.GetOldTempIDFromProductFormulaDetail_DAL(Formula_ID);
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


            if (ItemID == null || String.IsNullOrEmpty(P_Date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            ItemID = ItemID == null ? "0" : ItemID;


            try
            {
                message = _oInventory.UpdateProductFormulaMaster_DAL(P_Date, Ref, FormulaName, Production_Details, ItemID, Formula_ID, isActive,
           Temp_ID, c_ID, Modified_by);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("ProductFormulaList", "Inventory");
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






        //---Method for deleting Inventory invoice detail
        [HttpPost]
        public IActionResult DeleteProductFormulaDetail(int? ProductFormula_IN_ID, int HdFormula_ID)
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
            if (ProductFormula_IN_ID == null)
            {

                return Json(new { message = "ID is null. Please try again" });
            }

            if (HdFormula_ID == 0)
            {
                Temp_ID = ProductFormulaMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            try
            {
                message = _oInventory.DeleteProductFormulaDetail_DAL(ProductFormula_IN_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = message;
                    _oInventory.ProductFormulaDetailList = _oInventory.Get_ProductFormulaDetailListBy_DAL(Temp_ID, HdFormula_ID).ToList();



                    return PartialView("_ProductFormulaDetailList", _oInventory);

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
        public void DumpStaticFieldsInventory()
        {
            ProductFormulaMaster_TempID = null;
            ProductFormulaMasterStatic_ID = null;

            ProductionMaster_TempID = null;
            ProductionMasterStatic_ID = null;

            InventoryTransferMaster_TempID = null;
            InventoryTransferMasterStatic_ID = null;

            DismentalMaster_TempID = null;
            DismentalMasterStatic_ID = null;

            AssemblyMaster_TempID = null;
           AssemblyMasterStatic_ID = null;
        }

        [HttpGet]
        public IActionResult ProductFormulaList(string Ref, string StartDate, string EndDate,
    int c_ID, int? page)
        {
            //---dumping static fields
            DumpStaticFieldsInventory();

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Inventory";
            ViewBag.BreadCumAction = "ProductFormulaList";
            ViewBag.PageTitle = "Product Formula List";




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


            #endregion veiwbags area ends here

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;


            ViewBag.Ref_ProductFormula = Ref;
            ViewBag.StartDate_ProductFormula = StartDate;
            ViewBag.EndDate_ProductFormula = EndDate;

            ViewBag.c_ID_ProductFormula = c_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            //---purchase Inventory master invoice list for page
            ViewBag.RecordsPerPage = 10;

            _oInventory.ProductFormulaMasterList = _oInventory.Get_ProductFormulaMasterInvoiceList_DAL(Ref, StartDate, EndDate, c_ID).ToPagedList(page ?? 1, 10);


            return View(_oInventory);
        }
        //---Get Inventory master invoice list by search filters
        [HttpGet]
        public IActionResult GetInventoryMasterInvoiceListBySearchFilers(string Ref, string StartDate, string EndDate,
    int c_ID, int? page)
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
            Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;


            ViewBag.Ref_ProductFormula = Ref;
            ViewBag.StartDate_ProductFormula = StartDate;
            ViewBag.EndDate_ProductFormula = EndDate;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            ViewBag.c_ID_ProductFormula = c_ID;
            try
            {
                //---purchase Inventory master invoice list for page
                ViewBag.RecordsPerPage = 10;
                _oInventory.ProductFormulaMasterList = _oInventory.Get_ProductFormulaMasterInvoiceList_DAL(Ref, StartDate, EndDate, c_ID).ToPagedList(page ?? 1, 10);

                return PartialView("_InventoryMasterInvoiceListSearch", _oInventory);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }


        #endregion


        #region production

        [HttpGet]
        public IActionResult NewProduction(int Production_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Inventory";
            ViewBag.BreadCumAction = "NewProduction";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(UserName_Admin) || string.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            #region ViewBag Area


            //---Get Item master list
            ViewBag.ItemMaster = _oBasic.Get_ItemMasterList_DAL().ToList();
            ViewBag.FormulaMaster = _oInventory.Get_Formula_DAL().ToList();
            #endregion




            if (Production_ID == 0 || Production_ID < 1)
            {


                //---Create payment master Ref(tem id)
                if (string.IsNullOrEmpty(ProductionMaster_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    ProductionMaster_TempID = obj.ToString();
                }

                ProductionMasterStatic_ID = Production_ID;

                //---Get Inventory master id
                ViewBag.Production_ID = Production_ID;

                //--Get Inventory Master
                _oInventory.ProductionMasterObj = _oInventory.Get_ProductionMasterByID_DAL(-1);




                //---Inventory detial invoice list for Inventory invoice type Bychassis
                _oInventory.ProductionDetailList = _oInventory.Get_Production_Material_OUT_DetailListBy_DAL(ProductionMaster_TempID, -1).ToList();


            }
            else
            {

                //---Get old temp id from Inventory detail table that already creaded for this "Production_ID" id. And then use this if new record added
                string OldTempID = _oInventory.GetOldTempIDFromProductionDetail_DAL(Production_ID);




                ProductionMasterStatic_ID = Production_ID;

                //---Get sale master id
                ViewBag.Production_ID = Production_ID;

                //--Get sale Master

                _oInventory.ProductionMasterObj = _oInventory.Get_ProductionMasterByID_DAL(Production_ID);

                //---Inventory detial invoice list for Inventory invoice type Bychassis
                _oInventory.ProductionDetailList = _oInventory.Get_Production_Material_OUT_DetailListBy_DAL(OldTempID, Production_ID).ToList();



            }



            return View(_oInventory);
        }


        //---insert into Inventory invoice detial
        [HttpPost]
        public IActionResult InsertProductionDetail(int? HdProduction_ID, string ItemID, double? DirectCost, double?
        InDirectCost, string UOM, string Quantity, int formula_ID)
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


            if (ItemID == null)
            {
                return Json(new { message = "Please enter ItemID!" });
            }
            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = ProductionMaster_TempID;

            DirectCost = DirectCost == null ? 0 : DirectCost;
            InDirectCost = InDirectCost == null ? 0 : InDirectCost;
            if (HdProduction_ID == 0)
            {
                Temp_ID = ProductionMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            try
            {
                message = _oInventory.InsertProduction_Material_Out_Detail_DAL(HdProduction_ID, ItemID, DirectCost,
        InDirectCost, UOM, Quantity, Temp_ID, formula_ID);
                if (message == "Added Successfully!")
                {
                    //---Inventory detial invoice list for Inventory invoice type Bychassis
                    _oInventory.ProductionDetailList = _oInventory.Get_Production_Material_OUT_DetailListBy_DAL(Temp_ID, HdProduction_ID).ToList();



                    return PartialView("_ProductionDetailList", _oInventory);
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


        //---Update  Inventory invoice detail Chassis
        [HttpPost]
        public IActionResult UpdateProductionDetailById(int? Material_OUT_ID, string ItemID, double? DirectCost, double?
        InDirectCost, string UOM, string Quantity, int formula_ID, double? UnitPrice, double? Amount, int? HdProduction_ID)
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

            if (Material_OUT_ID == null)
            {
                return Json(new { message = "Inventory detail id is null" });
            }

            if (ItemID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            UnitPrice = UnitPrice == null ? 0 : UnitPrice;
            Amount = Amount == null ? 0 : Amount;
            DirectCost = DirectCost == null ? 0 : DirectCost;
            InDirectCost = InDirectCost == null ? 0 : InDirectCost;
            Temp_ID = ProductionMaster_TempID;
            if (HdProduction_ID == 0)
            {
                Temp_ID = ProductionMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }
            ProductionMasterStatic_ID = ProductionMasterStatic_ID == null ? -1 : ProductionMasterStatic_ID;


            try
            {
                message = _oInventory.UpdateProduction_Material_Out_DAL(Material_OUT_ID, ItemID, DirectCost,
        InDirectCost, UOM, Quantity, formula_ID, UnitPrice, Amount);
                if (message == "Updated Successfully!")
                {
                    //---Inventory detial invoice list for Inventory invoice type Bychassis
                    _oInventory.ProductionDetailList = _oInventory.Get_Production_Material_OUT_DetailListBy_DAL(Temp_ID, HdProduction_ID).ToList();



                    return PartialView("_ProductionDetailList", _oInventory);
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


        //---Insert  Inventory invoice master
        [HttpPost]
        public IActionResult InsertProductionMaster(string P_Date, string Ref, string CustomerRef, string Supervisor,
     string Production_Details)
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
            string Created_by = UserID_Admin;
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = ProductionMaster_TempID;

            if (Ref == null || String.IsNullOrEmpty(P_Date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewProduction", "Inventory");
            }




            try
            {
                message = _oInventory.InsertProductionMaster_DAL(P_Date, Ref, CustomerRef, Supervisor,
      Production_Details, Created_by, c_ID, Temp_ID);
                if (message == "Added Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("ProductionList", "Inventory");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("NewProduction", "Inventory");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
                return RedirectToAction("NewProduction", "Inventory");
            }


        }

        //---Insert  Inventory invoice master
        [HttpPost]
        public IActionResult UpdateProductionMaster(int Production_ID, string P_Date, string Ref, string CustomerRef, string Supervisor,
     string Production_Details)
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
            string Modified_by = UserID_Admin;
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = ProductionMaster_TempID;
            string OldTempID = _oInventory.GetOldTempIDFromProductionDetail_DAL(Production_ID);
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


            if (Ref == null || String.IsNullOrEmpty(P_Date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }




            try
            {
                message = _oInventory.UpdateProductionMaster_DAL(Production_ID, P_Date, Ref, CustomerRef, Supervisor,
      Production_Details, Modified_by, c_ID, Temp_ID);
                if (message == "Added Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("ProductionList", "Inventory");
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






        //---Method for deleting Inventory invoice detail
        [HttpPost]
        public IActionResult DeleteProductionDetail(int? Material_OUT_ID)
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

            if (Material_OUT_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }



            try
            {
                message = _oInventory.DeleteProduction_Material_Out_DAL(Material_OUT_ID);
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
        public IActionResult ProductionList(string Ref, string StartDate, string EndDate,
    int c_ID, int? page)
        {
            //---dumping static fields
            DumpStaticFieldsInventory();

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Inventory";
            ViewBag.BreadCumAction = "ProductionList";




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


            #endregion veiwbags area ends here

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;


            ViewBag.Ref_Production = Ref;
            ViewBag.StartDate_Production = StartDate;
            ViewBag.EndDate_Production = EndDate;

            ViewBag.c_ID_Production = c_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            //---purchase Inventory master invoice list for page
            ViewBag.RecordsPerPage = 10;

            _oInventory.ProductionMasterList = _oInventory.Get_ProductionMaster_DAL(Ref, StartDate, EndDate, c_ID).ToPagedList(page ?? 1, 10);


            return View(_oInventory);
        }
        //---Get Inventory master invoice list by search filters
        [HttpGet]
        public IActionResult GetProductionListBySearchFilers(string Ref, string StartDate, string EndDate,
    int c_ID, int? page)
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
            Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;


            ViewBag.Ref_Production = Ref;
            ViewBag.StartDate_Production = StartDate;
            ViewBag.EndDate_Production = EndDate;

            ViewBag.c_ID_Production = c_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            try
            {
                //---purchase Inventory master invoice list for page
                ViewBag.RecordsPerPage = 10;
                _oInventory.ProductionMasterList = _oInventory.Get_ProductionMaster_DAL(Ref, StartDate, EndDate, c_ID).ToPagedList(page ?? 1, 10);

                return PartialView("_ProductionListSearch", _oInventory);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }

        #endregion


        #region InventoryTransfer

        [HttpGet]
        public IActionResult NewInventoryTransfer(int Transfer_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Inventory";
            ViewBag.BreadCumAction = "NewInventoryTransfer";
            ViewBag.PageTitle = "Inventory Transfer";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(UserName_Admin) || string.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion


            #region ViewBag Area


            //---Get Item master list
            ViewBag.ItemMaster = _oBasic.Get_ItemMasterList_DAL().ToList();
            ViewBag.ItemLocMaster = _oBasic.Get_ItemLocationList_DAL(c_ID).ToList();
            _oInventory.InventoryTransferRef = _oInventory.TransferLoadRef();

            #endregion




            if (Transfer_ID == 0 || Transfer_ID < 1)
            {


                //---Create payment master Ref(tem id)
                if (string.IsNullOrEmpty(InventoryTransferMaster_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    InventoryTransferMaster_TempID = obj.ToString();
                }

                InventoryTransferMasterStatic_ID = Transfer_ID;

                //---Get Inventory master id
                ViewBag.Transfer_ID = Transfer_ID;

                //--Get Inventory Master
                _oInventory.InventoryTransferMasterObj = _oInventory.Get_InventoryTransfer_MasterByID_DAL(-1);




                //---Inventory detial invoice list for Inventory invoice type Bychassis
                _oInventory.InventoryTransferDetailList = _oInventory.Get_InventoryTransfer_DetailListBy_DAL(InventoryTransferMaster_TempID, -1).ToList();


            }
            else
            {

                //---Get old temp id from Inventory detail table that already creaded for this "Transfer_ID" id. And then use this if new record added
                string OldTempID = _oInventory.GetOldTempIDFromInventory_TransferDetail_DAL(Transfer_ID);




                InventoryTransferMasterStatic_ID = Transfer_ID;

                //---Get sale master id
                ViewBag.Transfer_ID = Transfer_ID;

                //--Get sale Master

                _oInventory.InventoryTransferMasterObj = _oInventory.Get_InventoryTransfer_MasterByID_DAL(Transfer_ID);

                //---Inventory detial invoice list for Inventory invoice type Bychassis
                _oInventory.InventoryTransferDetailList = _oInventory.Get_InventoryTransfer_DetailListBy_DAL(OldTempID, Transfer_ID).ToList();



            }



            return View(_oInventory);
        }


        //---insert into Inventory invoice detial
        [HttpPost]
        public IActionResult InsertInventoryTransferDetail(int? HdTransfer_ID, int ItemId, string UM_ID, string
        MajorQty, string MinorQty, string ItemDesc, string Unit_Price, string Amount, string job_ref, int OldLoc_ID, int NewLoc_ID, string comments, string quantity)
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


            if (ItemId == 0)
            {
                return Json(new { message = "Please enter ItemID!" });
            }
            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int c_ID = Convert.ToInt32(company_ID);

            Temp_ID = InventoryTransferMaster_TempID;

            Unit_Price = Unit_Price == null ? "0" : Unit_Price;
            Amount = Amount == null ? "0" : Amount;
            OldLoc_ID = OldLoc_ID == 0 ? 0 : OldLoc_ID;
            NewLoc_ID = NewLoc_ID == 0 ? 0 : NewLoc_ID;
            if (HdTransfer_ID == 0)
            {
                Temp_ID = InventoryTransferMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            try
            {
                message = _oInventory.InsertInventory_Transfer_Detail_DAL(HdTransfer_ID, ItemId, UM_ID,
        MajorQty, MinorQty, ItemDesc, Unit_Price, Amount, job_ref, OldLoc_ID, NewLoc_ID, Temp_ID, c_ID, comments, quantity);
                if (message.Contains("Successfully"))
                {
                    //---Inventory detial invoice list for Inventory invoice type Bychassis
                    _oInventory.InventoryTransferDetailList = _oInventory.Get_InventoryTransfer_DetailListBy_DAL(Temp_ID, HdTransfer_ID).ToList();



                    return PartialView("_InventoryTransferDetailList", _oInventory);
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


        //---Update  Inventory invoice detail Chassis
        [HttpPost]
        public IActionResult UpdateInventoryTransferDetailByChassis(int? ID, int? HdTransfer_ID, int ItemId, string ItemDesc, string Unit_Price, string Amount, string job_ref, int OldLoc_ID, int NewLoc_ID, string comments, string quantity)
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

            if (ID == null)
            {
                return Json(new { message = "Inventory detail id is null" });
            }

            if (ItemId == 0)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Unit_Price = Unit_Price == null ? "0" : Unit_Price;
            Amount = Amount == null ? "0" : Amount;
            OldLoc_ID = OldLoc_ID == 0 ? 0 : OldLoc_ID;
            NewLoc_ID = NewLoc_ID == 0 ? 0 : NewLoc_ID;

            Temp_ID = InventoryTransferMaster_TempID;
            if (HdTransfer_ID == 0)
            {
                Temp_ID = InventoryTransferMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }
            InventoryTransferMasterStatic_ID = InventoryTransferMasterStatic_ID == null ? -1 : InventoryTransferMasterStatic_ID;


            try
            {
                message = _oInventory.UpdateInventory_Transfer_Detail_DAL(ID, HdTransfer_ID, ItemId, ItemDesc, Unit_Price, Amount, job_ref, OldLoc_ID, NewLoc_ID, comments, quantity);
                if (message.Contains("Successfully"))
                {
                    //---Inventory detial invoice list for Inventory invoice type Bychassis
                    _oInventory.InventoryTransferDetailList = _oInventory.Get_InventoryTransfer_DetailListBy_DAL(Temp_ID, HdTransfer_ID).ToList();



                    return PartialView("_InventoryTransferDetailList", _oInventory);
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


        //---Insert  Inventory invoice master
        [HttpPost]
        public IActionResult InsertInventoryTransferMaster(string transferDate, string Ref, string OtherDetails, int? Loc_ID
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
            string Created_by = UserID_Admin;
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = InventoryTransferMaster_TempID;

            if (Ref == null || String.IsNullOrEmpty(transferDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewInventoryTransfer", "Inventory");
            }


            Loc_ID = Loc_ID == 0 ? 0 : Loc_ID;

            try
            {
                message = _oInventory.InsertInventoryTransferMaster_DAL(transferDate, Ref, OtherDetails, Loc_ID,
      Created_by, Temp_ID);
                if (message.Contains("Successfully"))
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("InventoryTransferList", "Inventory");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("NewInventoryTransfer", "Inventory");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
                return RedirectToAction("NewInventoryTransfer", "Inventory");
            }


        }

        //---Insert  Inventory invoice master
        [HttpPost]
        public IActionResult UpdateInventoryTransferMaster(int Transfer_ID, string transferDate, string Ref, string OtherDetails, int? Loc_ID)
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
            string Modified_by = UserID_Admin;
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = InventoryTransferMaster_TempID;
            string OldTempID = _oInventory.GetOldTempIDFromInventory_TransferDetail_DAL(Transfer_ID);
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


            if (Ref == null || String.IsNullOrEmpty(transferDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            Loc_ID = Loc_ID == 0 ? 0 : Loc_ID;


            try
            {
                message = _oInventory.UpdateInventory_TransferMaster_DAL(Transfer_ID, transferDate, Ref, OtherDetails, Loc_ID,
       Modified_by, Temp_ID);
                if (message.Contains("Successfully"))
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("InventoryTransferList", "Inventory");
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






        //---Method for deleting Inventory invoice detail
        [HttpPost]
        public IActionResult DeleteInventoryTransferDetail(int? ID)
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

            if (ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }



            try
            {
                message = _oInventory.DeleteInventory_Transfer_Details(ID);
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
        public IActionResult InventoryTransferList()
        {
            //---dumping static fields
            DumpStaticFieldsInventory();

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Inventory";
            ViewBag.BreadCumAction = "InventoryTransferList";
            ViewBag.PageTitle = "Inventory Transfer List";




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


            #endregion veiwbags area ends here

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            //c_ID = c_ID == 0 ? c_IDs : c_ID;
            //Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;
            //StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            //EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;


            //ViewBag.Ref_InventoryTransfer = Ref;
            //ViewBag.StartDate_InventoryTransfer = StartDate;
            //ViewBag.EndDate_InventoryTransfer = EndDate;

            //ViewBag.c_ID_InventoryTransfer = c_ID;

            //---purchase Inventory master invoice list for page
            ViewBag.RecordsPerPage = 10;

            _oInventory.InventoryTransferMasterList = _oInventory.Get_Inventory_TransferMaster_DAL().ToList();


            return View(_oInventory);
        }
        //---Get Inventory master invoice list by search filters
        [HttpGet]
        public IActionResult GetInventoryTransferMasterInvoiceListBySearchFilers()
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion
            //c_ID = c_ID == 0 ? 1 : c_ID;
            //Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;
            //StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            //EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;


            //ViewBag.Ref_InventoryTransfer = Ref;
            //ViewBag.StartDate_InventoryTransfer = StartDate;
            //ViewBag.EndDate_InventoryTransfer = EndDate;

            //ViewBag.c_ID_InventoryTransfer = c_ID;
            try
            {
                //---purchase Inventory master invoice list for page
                ViewBag.RecordsPerPage = 10;
                _oInventory.InventoryTransferMasterList = _oInventory.Get_Inventory_TransferMaster_DAL().ToList();

                return PartialView("_InventoryMasterInvoiceListSearch", _oInventory);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }


        #endregion

        [HttpGet]
        public JsonResult GetItemDetail(string Item)
        {
            string ItemDescription = "";
            string UnitPrice = "";
            string SalePrice = "";
            string UOM = "";
            int Location_ID = 0;


            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function
            _oBasic.GetItemObj = _oBasic.GetItemDetail_DAL(Item.Split(' ')[0]);

            if (!String.IsNullOrEmpty(_oBasic.GetItemObj.ItemCode))
            {

                ItemDescription = _oBasic.GetItemObj.ItemDescription;
                UnitPrice = _oBasic.GetItemObj.UnitPrice;
                SalePrice = _oBasic.GetItemObj.SalePrice;
                UOM = _oBasic.GetItemObj.UOM;
                Location_ID = _oBasic.GetItemObj.location_ID;

            }
            else
            {

                ItemDescription = null;
                UnitPrice = null;
                SalePrice = null;
                UOM = null;
                Location_ID = 0;
            }


            return Json(new { ItemDescription, UnitPrice, SalePrice, UOM, Location_ID });

        }

        [HttpGet]
        public JsonResult GetBarcodeDetail(string Barcode)
        {
            string ItemDescription = "";
            string UnitPrice = "";
            string SalePrice = "";
            string UOM = "";
            int Item_ID = 0;


            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function
            _oBasic.GetItemObj = _oBasic.GetBarcodeDetail_DAL(Barcode);

            if (!String.IsNullOrEmpty(_oBasic.GetItemObj.ItemCode))
            {

                ItemDescription = _oBasic.GetItemObj.ItemDescription;
                UnitPrice = _oBasic.GetItemObj.UnitPrice;
                SalePrice = _oBasic.GetItemObj.SalePrice;
                UOM = _oBasic.GetItemObj.UOM;
                Item_ID = _oBasic.GetItemObj.Item_ID;

            }
            else
            {

                ItemDescription = null;
                UnitPrice = null;
                SalePrice = null;
                UOM = null;
                Item_ID = 0;
            }


            return Json(new { ItemDescription, UnitPrice, SalePrice, UOM, Item_ID });

        }
        #region Dismental

        [HttpGet]
        public IActionResult NewDismental(int Dismental_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Inventory";
            ViewBag.BreadCumAction = "NewDismental";
            ViewBag.PageTitle = "New Dismental";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(UserName_Admin) || string.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            #region ViewBag Area


            //---Get Item master list
            ViewBag.ItemMaster = _oBasic.Get_ItemMasterList_DAL().ToList();
            ViewBag.FormulaMaster = _oInventory.Get_Formula_DAL().ToList();
            _oInventory.InventoryDismentalRef = _oInventory.DismentalLoadRef();
            #endregion




            if (Dismental_ID == 0 || Dismental_ID < 1)
            {


                //---Create payment master Ref(tem id)
                if (string.IsNullOrEmpty(DismentalMaster_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    DismentalMaster_TempID = obj.ToString();
                }

                DismentalMasterStatic_ID = Dismental_ID;

                //---Get Inventory master id
                ViewBag.Dismental_ID = Dismental_ID;

                //--Get Inventory Master
                _oInventory.DismentalMasterObj = _oInventory.Get_DismentalMasterByID_DAL(-1);




                //---Inventory detial invoice list for Inventory invoice type Bychassis
                _oInventory.DismentalDetailList = _oInventory.Get_Dismental_Material_IN_DetailListBy_DAL(DismentalMaster_TempID, -1).ToList();


            }
            else
            {

                //---Get old temp id from Inventory detail table that already creaded for this "Dismental_ID" id. And then use this if new record added
                string OldTempID = _oInventory.GetOldTempIDFromDismentalDetail_DAL(Dismental_ID);




                DismentalMasterStatic_ID = Dismental_ID;

                //---Get sale master id
                ViewBag.Dismental_ID = Dismental_ID;

                //--Get sale Master

                _oInventory.DismentalMasterObj = _oInventory.Get_DismentalMasterByID_DAL(Dismental_ID);

                //---Inventory detial invoice list for Inventory invoice type Bychassis
                _oInventory.DismentalDetailList = _oInventory.Get_Dismental_Material_IN_DetailListBy_DAL(OldTempID, Dismental_ID).ToList();



            }



            return View(_oInventory);
        }

   
        //---insert into Inventory invoice detial
        [HttpPost]
        public IActionResult InsertDismentalDetail(int HdDismental_ID, int[] Item_ID, string[] Cost, string[] Quantity)
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


            if (Item_ID == null)
            {
                return Json(new { message = "Please enter ItemID!" });
            }
           //---Validation area ends here
      
           
 
            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = DismentalMaster_TempID;

            if (HdDismental_ID == 0)
            {
                Temp_ID = DismentalMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            try
            {

                if (Item_ID != null && Item_ID.Length != 0)
                {
                    for (int i = 0; i < Item_ID.Length; i++)
                    {
                        message = _oInventory.InsertDismental_Material_In_Detail_DAL(HdDismental_ID, Item_ID[i], Cost[i], Quantity[i], Temp_ID);

                    }

                }
                else
                {
                    message = "Please Check Any One to perform this task";
                }
                
           
                        
             
                if (message.Contains("Successfully"))
                {
                    //---Inventory detial invoice list for Inventory invoice type Bychassis
                    _oInventory.DismentalDetailList = _oInventory.Get_Dismental_Material_IN_DetailListBy_DAL(Temp_ID, HdDismental_ID).ToList();



                    return PartialView("_DismentalDetailListgrid", _oInventory);
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


        //---Update  Inventory invoice detail Chassis
        [HttpPost]
        public IActionResult UpdateDismentalDetailById(int? Dismental_Dtl_ID, string ItemID, string Cost, string Quantity, int? HdDismental_ID)
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

            if (Dismental_Dtl_ID == null)
            {
                return Json(new { message = "Inventory detail id is null" });
            }

            if (ItemID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
    
            Temp_ID = DismentalMaster_TempID;
            if (HdDismental_ID == 0)
            {
                Temp_ID = DismentalMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }
            DismentalMasterStatic_ID = DismentalMasterStatic_ID == null ? -1 : DismentalMasterStatic_ID;


            try
            {
                message = _oInventory.UpdateDismental_Material_In_DAL( Dismental_Dtl_ID,  ItemID,  Cost,  Quantity);
                if (message == "Updated Successfully!")
                {
                    //---Inventory detial invoice list for Inventory invoice type Bychassis
                    _oInventory.DismentalDetailList = _oInventory.Get_Dismental_Material_IN_DetailListBy_DAL(Temp_ID, HdDismental_ID).ToList();



                    return PartialView("_DismentalDetailList", _oInventory);
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


        //---Insert  Inventory invoice master
        [HttpPost]
        public IActionResult InsertDismentalMaster(string P_Date, string Ref, string CustomerRef, string Supervisor,
     string Dismental_Chassis)
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
            string Created_by = UserID_Admin;
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = DismentalMaster_TempID;

            if (Ref == null || String.IsNullOrEmpty(P_Date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewDismental", "Inventory");
            }




            try
            {
                message = _oInventory.InsertDismentalMaster_DAL(P_Date, Ref, CustomerRef, Supervisor,
      Dismental_Chassis, Created_by, c_ID, Temp_ID);
                if (message.Contains("Successfully"))
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("DismentalList", "Inventory");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("NewDismental", "Inventory");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
                return RedirectToAction("NewDismental", "Inventory");
            }


        }

        //---Insert  Inventory invoice master
        [HttpPost]
        public IActionResult UpdateDismentalMaster(int Dismental_ID, string P_Date, string Ref, string CustomerRef, string Supervisor,
     string Dismental_Chassis)
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
            string Modified_by = UserID_Admin;
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = DismentalMaster_TempID;
            string OldTempID = _oInventory.GetOldTempIDFromDismentalDetail_DAL(Dismental_ID);
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


            if (Ref == null || String.IsNullOrEmpty(P_Date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }




            try
            {
                message = _oInventory.UpdateDismentalMaster_DAL(Dismental_ID, P_Date, Ref, CustomerRef, Supervisor,
      Dismental_Chassis, Modified_by, c_ID, Temp_ID);
                if (message.Contains("Successfully"))
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("DismentalList", "Inventory");
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






        //---Method for deleting Inventory invoice detail
        [HttpPost]
        public IActionResult DeleteDismentalDetail(int? Material_OUT_ID)
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

            if (Material_OUT_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }



            try
            {
                message = _oInventory.DeleteDismental_Material_In_DAL(Material_OUT_ID);
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
        public IActionResult DismentalList(string Ref, string StartDate, string EndDate,
    int c_ID, int? page)
        {
            //---dumping static fields
            DumpStaticFieldsInventory();

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Inventory";
            ViewBag.BreadCumAction = "DismentalList";
            ViewBag.PageTitle = "Dismental List";




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


            #endregion veiwbags area ends here

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;


            ViewBag.Ref_Dismental = Ref;
            ViewBag.StartDate_Dismental = StartDate;
            ViewBag.EndDate_Dismental = EndDate;

            ViewBag.c_ID_Dismental = c_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            //---purchase Inventory master invoice list for page
            ViewBag.RecordsPerPage = 10;

            _oInventory.DismentalMasterList = _oInventory.Get_DismentalMaster_DAL(Ref, StartDate, EndDate, c_ID).ToPagedList(page ?? 1, 10);


            return View(_oInventory);
        }
        //---Get Inventory master invoice list by search filters
        [HttpGet]
        public IActionResult GetDismentalListBySearchFilers(string Ref, string StartDate, string EndDate,
    int c_ID, int? page)
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
            Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;


            ViewBag.Ref_Dismental = Ref;
            ViewBag.StartDate_Dismental = StartDate;
            ViewBag.EndDate_Dismental = EndDate;

            ViewBag.c_ID_Dismental = c_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            try
            {
                //---purchase Inventory master invoice list for page
                ViewBag.RecordsPerPage = 10;
                _oInventory.DismentalMasterList = _oInventory.Get_DismentalMaster_DAL(Ref, StartDate, EndDate, c_ID).ToPagedList(page ?? 1, 10);

                return PartialView("_DismentalListSearch", _oInventory);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }
        [HttpGet]
        public IActionResult GetFormulaDetails(int FormulaId)
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
                _oInventory.FormulaDetailList = _oInventory.GetFormulaDetails_DAL(FormulaId).ToList();
                _oInventory.DismentalMasterObj = _oInventory.Get_DismentalMasterByID_DAL(-1);
                return PartialView("_DismentalDetailList", _oInventory);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }
        #endregion
        #region Assembly

        [HttpGet]
        public IActionResult NewAssembly(int Assembly_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Inventory";
            ViewBag.BreadCumAction = "NewAssembly";

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


            //---Get Item master list
            ViewBag.ItemMaster = _oBasic.Get_ItemMasterList_DAL().ToList();
            ViewBag.FormulaMaster = _oInventory.Get_Formula_DAL().ToList();
            _oInventory.InventoryAssemblyRef = _oInventory.AssemblyLoadRef();
            ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_ID).ToList();

            _oBasic.ModelDescList = _oBasic.Get_ModelList_DAL(c_ID).ToList();
            //---Get Colors List
            ViewBag.ColorsList = _oBasic.Get_ColorsList_DAL().ToList();
            #endregion




            if (Assembly_ID == 0 || Assembly_ID < 1)
            {


                //---Create payment master Ref(tem id)
                if (string.IsNullOrEmpty(AssemblyMaster_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    AssemblyMaster_TempID = obj.ToString();
                }

                AssemblyMasterStatic_ID = Assembly_ID;

                //---Get Inventory master id
                ViewBag.Assembly_ID = Assembly_ID;
               

                //--Get Inventory Master
                _oInventory.AssemblyMasterObj = _oInventory.Get_AssemblyMasterByID_DAL(-1);


                _oInventory.AssemblyStockMasterObj = _oInventory.Get_AssemblyStockMasterByID_DAL(-1);

                //---Inventory detial invoice list for Inventory invoice type Bychassis
                _oInventory.AssemblyDetailList = _oInventory.Get_Assembly_Material_IN_DetailListBy_DAL(AssemblyMaster_TempID, -1).ToList();

                ViewBag.ModelDesc = _oBasic.ModelDescList.Where(m => (m.Make_ID == (Convert.ToInt32(0)))).ToList();

            }
            else
            {

                //---Get old temp id from Inventory detail table that already creaded for this "Assembly_ID" id. And then use this if new record added
                string OldTempID = _oInventory.GetOldTempIDFromAssemblyDetail_DAL(Assembly_ID);




                AssemblyMasterStatic_ID = Assembly_ID;

                //---Get sale master id
                ViewBag.Assembly_ID = Assembly_ID;

                //--Get sale Master
        

                _oInventory.AssemblyMasterObj = _oInventory.Get_AssemblyMasterByID_DAL(Assembly_ID);
                _oInventory.AssemblyStockMasterObj = _oInventory.Get_AssemblyStockMasterByID_DAL(Assembly_ID);

                //---Inventory detial invoice list for Inventory invoice type Bychassis
                _oInventory.AssemblyDetailList = _oInventory.Get_Assembly_Material_IN_DetailListBy_DAL(OldTempID, Assembly_ID).ToList();

                ViewBag.ModelDesc = _oBasic.ModelDescList.Where(m => (m.Make_ID == (Convert.ToInt32(_oInventory.AssemblyStockMasterObj.make_ID)))).ToList();

            }



            return View(_oInventory);
        }


        //---insert into Inventory invoice detial
        [HttpPost]
        public IActionResult InsertAssemblyDetail(int HdAssembly_ID, int[] Item_ID, string[] Cost, string[] Quantity, string[] Remarks)
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


            if (Item_ID == null)
            {
                return Json(new { message = "Please enter ItemID!" });
            }
            //---Validation area ends here



            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = AssemblyMaster_TempID;

            if (HdAssembly_ID == 0)
            {
                Temp_ID = AssemblyMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            try
            {

                if (Item_ID != null && Item_ID.Length != 0)
                {
                    for (int i = 0; i < Item_ID.Length; i++)
                    {
                        Remarks[i] = String.IsNullOrEmpty(Remarks[i]) ? "" : Remarks[i];
                        message = _oInventory.InsertAssembly_Material_In_Detail_DAL(HdAssembly_ID, Item_ID[i], Cost[i], Quantity[i], Remarks[i], Temp_ID);

                    }

                }
                else
                {
                    message = "Please Check Any One to perform this task";
                }




                if (message.Contains("Successfully"))
                {
                    //---Inventory detial invoice list for Inventory invoice type Bychassis
                    _oInventory.AssemblyDetailList = _oInventory.Get_Assembly_Material_IN_DetailListBy_DAL(Temp_ID, HdAssembly_ID).ToList();



                    return PartialView("_AssemblyDetailListgrid", _oInventory);
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


        //---Update  Inventory invoice detail Chassis
        [HttpPost]
        public IActionResult UpdateAssemblyDetailById(int? Assembly_Dtl_ID, string ItemID, string Cost, string Quantity, int? HdAssembly_ID)
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

            if (Assembly_Dtl_ID == null)
            {
                return Json(new { message = "Inventory detail id is null" });
            }

            if (ItemID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            //---Validation area ends here


            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = AssemblyMaster_TempID;
            if (HdAssembly_ID == 0)
            {
                Temp_ID = AssemblyMaster_TempID;
            }
            else
            {
                Temp_ID = "0";
            }
            AssemblyMasterStatic_ID = AssemblyMasterStatic_ID == null ? -1 : AssemblyMasterStatic_ID;


            try
            {
                message = _oInventory.UpdateAssembly_Material_In_DAL(Assembly_Dtl_ID, ItemID, Cost, Quantity);
                if (message == "Updated Successfully!")
                {
                    //---Inventory detial invoice list for Inventory invoice type Bychassis
                    _oInventory.AssemblyDetailList = _oInventory.Get_Assembly_Material_IN_DetailListBy_DAL(Temp_ID, HdAssembly_ID).ToList();



                    return PartialView("_AssemblyDetailList", _oInventory);
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


        //---Insert  Inventory invoice master
        [HttpPost]
        public IActionResult InsertAssemblyMaster(string P_Date, string Ref, string CustomerRef, string Supervisor,
     string CHASSIS_NO,int make_ID,int MakeModel_description_ID,int ModelYear,int color_exterior_ID,int color_interior_ID,int ItemID,string Assembly_Type)
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
            string Created_by = UserID_Admin;
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = AssemblyMaster_TempID;

            if (Ref == null || String.IsNullOrEmpty(P_Date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewAssembly", "Inventory");
            }




            try
            {
               string MasterId = _oInventory.InsertAssemblyMaster_DAL(P_Date, Ref, CustomerRef, Supervisor,
      "", Created_by, c_ID, Temp_ID,  ItemID,  Assembly_Type);
                if (Convert.ToInt32(MasterId) > 0)
                {
                    message = "Successfully";
                }
                else
                {
                    message = "Not Successfully";
                }
                if (!string.IsNullOrEmpty(CHASSIS_NO))
                {
                    message = _oInventory.InsertAssemblyStockMaster_DAL(MasterId, CHASSIS_NO, make_ID, MakeModel_description_ID, ModelYear,
          color_exterior_ID, color_interior_ID);
                }
              
                if (message.Contains("Successfully"))
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("AssemblyList", "Inventory");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("NewAssembly", "Inventory");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
                return RedirectToAction("NewAssembly", "Inventory");
            }


        }

        //---Insert  Inventory invoice master
        [HttpPost]
        public IActionResult UpdateAssemblyMaster(int Assembly_ID, string P_Date, string Ref, string CustomerRef, string Supervisor,
      string CHASSIS_NO, int make_ID, int MakeModel_description_ID, int ModelYear, int color_exterior_ID, int color_interior_ID, int ItemID, string Assembly_Type)
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
            string Modified_by = UserID_Admin;
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = AssemblyMaster_TempID;
            string OldTempID = _oInventory.GetOldTempIDFromAssemblyDetail_DAL(Assembly_ID);
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


            if (Ref == null || String.IsNullOrEmpty(P_Date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }




            try
            {
                message = _oInventory.UpdateAssemblyMaster_DAL(Assembly_ID, P_Date, Ref, CustomerRef, Supervisor,
                 "", Modified_by, c_ID, Temp_ID, ItemID, Assembly_Type);
                if (!string.IsNullOrEmpty(CHASSIS_NO))
                {
                    string mes = _oInventory.DeleteAssembly_Material_Out_DAL(Assembly_ID);
                    message = _oInventory.InsertAssemblyStockMaster_DAL(Assembly_ID.ToString(), CHASSIS_NO, make_ID, MakeModel_description_ID, ModelYear,
     color_exterior_ID, color_interior_ID);
                }
                if (message.Contains("Successfully"))
                {
                    
                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("AssemblyList", "Inventory");
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






        //---Method for deleting Inventory invoice detail
        [HttpPost]
        public IActionResult DeleteAssemblyDetail(int? Material_OUT_ID)
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

            if (Material_OUT_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }



            try
            {
                message = _oInventory.DeleteAssembly_Material_In_DAL(Material_OUT_ID);
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
        public IActionResult AssemblyList(string Ref, string StartDate, string EndDate,
    int c_ID, int? page)
        {
            //---dumping static fields
            DumpStaticFieldsInventory();

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Inventory";
            ViewBag.BreadCumAction = "AssemblyList";




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


            #endregion veiwbags area ends here

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;


            ViewBag.Ref_Assembly = Ref;
            ViewBag.StartDate_Assembly = StartDate;
            ViewBag.EndDate_Assembly = EndDate;

            ViewBag.c_ID_Assembly = c_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            //---purchase Inventory master invoice list for page
            ViewBag.RecordsPerPage = 10;

            _oInventory.AssemblyMasterList = _oInventory.Get_AssemblyMaster_DAL(Ref, StartDate, EndDate, c_ID).ToPagedList(page ?? 1, 10);


            return View(_oInventory);
        }
        //---Get Inventory master invoice list by search filters
        [HttpGet]
        public IActionResult GetAssemblyListBySearchFilers(string Ref, string StartDate, string EndDate,
    int c_ID, int? page)
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
            Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;


            ViewBag.Ref_Assembly = Ref;
            ViewBag.StartDate_Assembly = StartDate;
            ViewBag.EndDate_Assembly = EndDate;

            ViewBag.c_ID_Assembly = c_ID;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
            try
            {
                //---purchase Inventory master invoice list for page
                ViewBag.RecordsPerPage = 10;
                _oInventory.AssemblyMasterList = _oInventory.Get_AssemblyMaster_DAL(Ref, StartDate, EndDate, c_ID).ToPagedList(page ?? 1, 10);

                return PartialView("_AssemblyListSearch", _oInventory);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }
        [HttpGet]
        public IActionResult GetFormulaDetailsAssembly(int FormulaId)
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
                _oInventory.FormulaDetailList = _oInventory.GetFormulaDetails_DAL(FormulaId).ToList();
                _oInventory.AssemblyMasterObj = _oInventory.Get_AssemblyMasterByID_DAL(-1);
                return PartialView("_AssemblyDetailList", _oInventory);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }
        #endregion
    }
}
