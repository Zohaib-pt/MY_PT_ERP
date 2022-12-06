using DAL.oClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace PT_ERP.Controllers
{
    public class HRController : BaseController
    {
        static string EmpTempID;
        static int? EmpMasterStatic_ID;


        private readonly IOBasicData _oBasic;
        private readonly IOHR _oHR;
        private IConfiguration configuration;


        public HRController(IOBasicData oBasicBase, IOHR oHR, IConfiguration iConfig)
           : base(oBasicBase)
        {
            _oBasic = oBasicBase;
            _oHR = oHR;
            configuration = iConfig;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult NewEmployee(int? Emp_master_ID)
        {


            #region ViewBag Area
            //---Ven Category list for drop down in forms

            #endregion
            ViewBag.PageTitle = "New Employee";
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "HR";
            ViewBag.BreadCumAction = "HRList";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            //---Partners list for page

            if (Emp_master_ID == null || Emp_master_ID < 1)
            {


                //---Create Purchase master Ref(tem id)
                if (String.IsNullOrEmpty(EmpTempID))
                {
                    Guid obj = Guid.NewGuid();
                    EmpTempID = obj.ToString();
                }

                EmpMasterStatic_ID = Emp_master_ID;

                //---Get emp master id
                ViewBag.Emp_master_ID = Emp_master_ID;

                //--Get emp master
                _oHR.EmpMasterObj = _oHR.Get_EmployeeMasterByID_DAL(-1);

                //_oHR.EmpMasterList = _oHR.Get_vehicle_display_master_DAL_Id(-1);
                _oHR.EmpDocumentList = _oHR.Get_EmpDocument_DetailListByID_DAL(-1, EmpTempID);


            }
            else
            {

                //---Get old temp id from Emp_Documents  table that already creaded for this "PaymentMaster_ID" id. And then use this if new record added
                string OldTempID = _oHR.GetOldTempIDFromEmpDocDetail_DAL(Emp_master_ID);

                if (!String.IsNullOrEmpty(OldTempID))
                {
                    EmpTempID = OldTempID;
                }
                else
                {
                    if (String.IsNullOrEmpty(EmpTempID))
                    {
                        Guid obj = Guid.NewGuid();
                        EmpTempID = obj.ToString();
                    }
                }



                EmpMasterStatic_ID = Emp_master_ID;

                //---Get emp master id
                ViewBag.Emp_master_ID = Emp_master_ID;


                //--Get emp master
                _oHR.EmpMasterObj = _oHR.Get_EmployeeMasterByID_DAL(Emp_master_ID);


                //_oHR.NewEmployeeMasterObj = _oHR.Get_vehicle_display_master_DAL_Id(Emp_master_ID);
                _oHR.EmpDocumentList = _oHR.Get_EmpDocument_DetailListByID_DAL(Emp_master_ID, EmpTempID).ToList();
            }


            return View(_oHR);
        }

        [HttpGet]
        public IActionResult ListEmployee()
        {

            EmpTempID = null;
            EmpMasterStatic_ID = null;
            #region ViewBag Area
            //---Ven Category list for drop down in forms

            #endregion
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "HR";
            ViewBag.BreadCumAction = "HRList";
            ViewBag.PageTitle = "Employee List";
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

            //---Partners list for page

            _oHR.EmpMasterList = _oHR.Get_Emp_Main_master_DAL(c_ID).ToList();

            return View(_oHR);
        }

        [HttpPost]
        public IActionResult AddEmpMaster(string emp_Name, string Birth_Date, string Joining_Date, string Nationality, bool IsActive,
            string Desgination, string Basic_Salary, string Accomodation, string Transport, string Sp_Allowance, string Total_Salary, IFormFile file)
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

            if (String.IsNullOrEmpty(emp_Name))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewEmployee", "HR");
            }


            string message = "";
            string messageImageSave = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            string c_ID = company_ID;
            string temp_ID = EmpTempID;




            string ProfileImageName = "";
            string ProfileImagePath = "";

            #region saving file
            if (file != null)
            {
                if (file.Length > 0)
                {
                    try
                    {

                        //string Image = Path.Combine(Server.MapPath(filePath), imgname);


                        if (file.Length > 0)
                        {
                            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                            //Getting FileName
                            var fileName = Path.GetFileName(file.FileName);

                            //Assigning Unique Filename (Guid)
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                            //Getting file Extension
                            var fileExtension = Path.GetExtension(fileName);

                            // concatenating  FileName + FileExtension
                            ProfileImageName = String.Concat(myUniqueFileName, fileExtension);

                            // Combines two strings into a path.

                            ProfileImagePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{ProfileImageName}";

                            using (FileStream fs = System.IO.File.Create(ProfileImagePath))
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        messageImageSave = "Image not saved successfully!";
                    }

                }

            }

            #endregion


            //---saving CustomersMaster in database
            try
            {
                message = _oHR.Emp_Insert_EmpMainDetail_DAL(emp_Name, Birth_Date, Joining_Date, Nationality, IsActive, Desgination, Basic_Salary, Accomodation, Transport, Sp_Allowance,
                    Total_Salary, ProfileImageName, ProfileImagePath, temp_ID, c_ID, Created_by);
                if (message == "Saved Successfully!")
                {
                    TempData["Success"] = "Saved Successfully";
                    return RedirectToAction("ListEmployee", "HR");
                }
                else
                {
                    TempData["Error"] = messageImageSave + " " + message;
                    return RedirectToAction("NewEmployee", "HR");
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                TempData["Error"] = messageImageSave + " " + ErrorMessage;
                return RedirectToAction("NewEmployee", "HR");
            }




            //return View();
        }


        [HttpPost]
        public IActionResult UpdateEmpMaster(int? Emp_master_ID, string emp_Name, string Birth_Date, string Joining_Date, string Nationality, string IsActive,
            string Desgination, string Basic_Salary, string Accomodation, string Transport, string Sp_Allowance, string Total_Salary, IFormFile file, string Temp_ID)
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

            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();

            if (Emp_master_ID == null)
            {
                TempData["Error"] = "Emp id is null. Please try again!";
                return Redirect(url);
            }
            if (String.IsNullOrEmpty(emp_Name))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }


            bool IsActiveUpdate = false;
            if (IsActive == "on")
            {
                IsActiveUpdate = true;
            }
            else
            {
                IsActiveUpdate = false;
            }

            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            string temp_ID = EmpTempID;



            string ProfileImageName = "";
            string ProfileImagePath = "";
            string messageImageSave = "";

            #region saving file
            if (file != null)
            {
                if (file.Length > 0)
                {
                    try
                    {

                        //string Image = Path.Combine(Server.MapPath(filePath), imgname);


                        if (file.Length > 0)
                        {
                            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                            //Getting FileName
                            var fileName = Path.GetFileName(file.FileName);

                            //Assigning Unique Filename (Guid)
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                            //Getting file Extension
                            var fileExtension = Path.GetExtension(fileName);

                            // concatenating  FileName + FileExtension
                            ProfileImageName = String.Concat(myUniqueFileName, fileExtension);

                            // Combines two strings into a path.

                            ProfileImagePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{ProfileImageName}";

                            using (FileStream fs = System.IO.File.Create(ProfileImagePath))
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        messageImageSave = "Image not saved successfully!";
                    }

                }

            }

            #endregion


            //---checking if Profile image not updated
            ProfileImageName = String.IsNullOrEmpty(ProfileImageName) ? "" : ProfileImageName;
            ProfileImagePath = String.IsNullOrEmpty(ProfileImagePath) ? "" : ProfileImagePath;

            //---saving CustomersMaster in database
            try
            {
                message = _oHR.Emp_Update_EmpMainDetail_DAL(Emp_master_ID, emp_Name, Birth_Date, Joining_Date, Nationality, IsActiveUpdate, Desgination, Basic_Salary, Accomodation,
                    Transport, Sp_Allowance, Total_Salary, ProfileImageName, ProfileImagePath, temp_ID, c_ID, Modified_by);


                TempData["Success"] = "Updated Successfully";
                return RedirectToAction("ListEmployee", "HR");
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

            //return View();
        }


        [HttpPost]
        public IActionResult AddEmpDocuments(string empDoc_Type, IFormFile empDoc_FileName, string empDoc_ExpiryDate)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (String.IsNullOrEmpty(empDoc_Type))
            {

                return Json(new { message = "Doc type required" });
            }

            var newFileName = "";
            var filepath = "";
            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            string temp_ID = EmpTempID;
            EmpMasterStatic_ID = EmpMasterStatic_ID == null ? 0 : EmpMasterStatic_ID;

            #region saving file
            if (empDoc_FileName != null)
            {
                if (empDoc_FileName.Length > 0)
                {
                    try
                    {

                        //string Image = Path.Combine(Server.MapPath(filePath), imgname);


                        if (empDoc_FileName.Length > 0)
                        {
                            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                            //Getting FileName
                            var fileName = Path.GetFileName(empDoc_FileName.FileName);

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
                                empDoc_FileName.CopyTo(fs);
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

            //---saving CustomersDetail in database
            try
            {
                message = _oHR.Emp_Insert_Documents_DAL(empDoc_Type, newFileName, filepath, empDoc_ExpiryDate, temp_ID, Created_by);
                if (message == "Saved Successfully!")
                {

                    _oHR.EmpDocumentList = _oHR.Get_EmpDocument_DetailListByID_DAL(EmpMasterStatic_ID, temp_ID).ToList();

                    return PartialView("_EmployeeDocPartial", _oHR);
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
        public IActionResult DeleteEmpDocuments(int? empDoc_ID, string File_Name)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (empDoc_ID == null)
            {
                return Json(new { message = "Document ID  required" });
            }



            string message = "";

            string temp_ID = EmpTempID;
            EmpMasterStatic_ID = EmpMasterStatic_ID == null ? 0 : EmpMasterStatic_ID;


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





            //---Deleting emp documents
            try
            {
                message = _oHR.DeleteEmployee_Document_DAL(empDoc_ID);
                if (message == "Deleted Successfully!")
                {

                    _oHR.EmpDocumentList = _oHR.Get_EmpDocument_DetailListByID_DAL(EmpMasterStatic_ID, temp_ID).ToList();

                    return PartialView("_EmployeeDocPartial", _oHR);
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
        public IActionResult DeleteEmployee(int? emp_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();


            if (emp_ID == null || emp_ID < 1)
            {
                TempData["Error"] = "Employee id is null. Please try again";
                return Redirect(url);
            }
            string message = "";

            try
            {
                message = _oHR.DeleteEmployee_DAL(emp_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("ListEmployee", "HR");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TempData["Error"] = errorMessage;
                return Redirect(url);

            }


        }

    
        
        #region PAYROLL


        [HttpGet]
        public IActionResult GeneratePayroll(string? start_date, string? end_date, string? Acc_Period_ID)
        {


            #region ViewBag Area
            //---Ven Category list for drop down in forms

            #endregion
            ViewBag.PageTitle = "PayRoll";
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "GeneratePayroll";
            ViewBag.BreadCumAction = "GeneratePayrollList";
            ViewBag.PageTitle = "Manage Payroll";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            if(Acc_Period_ID == null || start_date ==null || end_date == null )
            {
                start_date = "";
                end_date = "";
                Acc_Period_ID = "0";
            }
            ViewBag.PeriodList = _oHR.Get_AccountingPeroid_DAL(c_ID).ToList();
           _oHR.PayMasterList = _oHR.Get_PayRollList_DAL(c_ID, start_date, end_date, Acc_Period_ID).ToList();



            //---Partners list for page



            return View(_oHR);
        }
        [HttpGet]
        public IActionResult StafPayRoll_Print(int payRoll_Master_ID)
        {


            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
       
                _oHR.PaySlipVoucharList_Print = _oHR.Get_PaySlipVoucharByID_DAL(payRoll_Master_ID).ToList();


                var demoViewPortrait = new ViewAsPdf(_oHR)
                {

                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Bottom = 20, Right = 1, Top = 10 },
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                    CustomSwitches =
                   "--footer-left \"  Printed At: " +
   DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
   " --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""

                };
                return demoViewPortrait;
            
           

        }

        [HttpPost]      
        public IActionResult InsertPayRoll(string pDate, int? Acc_Period_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            //string message = "";
            int? Created_by =  Convert.ToInt32(UserID_Admin);
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            int? c_ID = c_IDs;
           


            try
            {
                 _oHR.InsertPayRoll_DAL(pDate, Acc_Period_ID, c_ID, Created_by);

                return RedirectToAction("GeneratePayroll", "HR");


            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("GeneratePayroll", "HR");
            }



        }   
        // Insert PayRoll
        [HttpGet]
        public IActionResult PaySlipVouchers(int? payRoll_Master_ID)
        {

            
            #region ViewBag Area
            //---Ven Category list for drop down in forms

            #endregion
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "PaySlipVouchers";
            ViewBag.BreadCumAction = "PaySlipVouchersList";
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

            ViewBag.CR_accountIDList = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();

            _oHR.PaySlipVoucharList = _oHR.Get_PaySlipVoucharByID_DAL(payRoll_Master_ID);
            //---Partners list for page



            return View(_oHR);
        }

        [HttpPost]
        public IActionResult InsertIssuPaymentVouchar(List<string> emp_PayrollDetails_ID ,string pay_Account_ID)
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
            int c_ID = Convert.ToInt32(company_ID);

            int acc_id = Convert.ToInt32(pay_Account_ID);

            
            


            try
            {
                foreach (var item in emp_PayrollDetails_ID)
                {
                    int id = Convert.ToInt32(item);
                    message = _oHR.InsertIssuPaymentVouchar_DAL(id, acc_id);
                }
             
                if (message == "Posted Successfully!")
                {
                    TempData["Success"] = message;
                    return RedirectToAction("GeneratePayroll", "HR");
                }
                else
                {
                    TempData["Error"] = "Error!";
                    return RedirectToAction("GeneratePayroll", "HR");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("PaySlipVouchers", "HR");
            }

        }



        #endregion


        [HttpGet]
        public ActionResult NotificationList()
        {

            //DumpStaticFieldsBL();
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "HR";
            ViewBag.BreadCumAction = "NotificationList";


            ViewBag.SectionHeaderTitle = "HR";
            ViewBag.SectionSub_HeaderTitle = "Notification List";
            ViewBag.PageTitle = "Notification List";


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            int UserID = Convert.ToInt32(UserID_Admin);

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            //type = String.IsNullOrEmpty(type) ? "" : type;
            //Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;
            //Description = String.IsNullOrEmpty(Description) ? "" : Description;
            //Voucher_Amt = String.IsNullOrEmpty(Voucher_Amt) ? "" : Voucher_Amt;
            //Ledger_Amt = String.IsNullOrEmpty(Ledger_Amt) ? "" : Ledger_Amt;
            //Fix = String.IsNullOrEmpty(Fix) ? "" : Fix;





            //ViewBag.type = type;

            //ViewBag.Ref = Ref;
            //ViewBag.Description = Description;
            //ViewBag.Voucher_Amt = Voucher_Amt;
            //ViewBag.Ledger_Amt = Ledger_Amt;
            //ViewBag.Fix = Fix;


            //---list for page



            //  _oStock.BLMasterPlist = _oStock.Get_BlList_DAL(BLNO, StartDate, EndDate, c_ID).ToList().ToPagedList(page ?? 1, 10);

            _oHR.AlertNotificationList = _oHR.Get_AlertNotificationList_DAL(UserID).ToList();
            ViewBag.AlertCount = _oHR.Get_AlertNotificationList_DAL(UserID).ToList().Count;
            return View(_oHR);

        }

    }
}