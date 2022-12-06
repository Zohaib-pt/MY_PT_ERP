using DAL.Models;
using DAL.oClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace PT_ERP.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IOBasicData _oBasic;
        private readonly IOAdmin _oAdmin;
        private IConfiguration configuration;


        public AdminController(IOBasicData oBasicBase, IOAdmin oAdmin, IConfiguration iConfig)
           : base(oBasicBase)
        {
            _oBasic = oBasicBase;
            _oAdmin = oAdmin;
            configuration = iConfig;
        }

        public IActionResult Index()
        {
            return View();
        }
        //this method taking admin users
        [HttpGet]
        public IActionResult UserManagement()
        {
            ViewBag.PageTitle = "Users List";
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



            _oAdmin.AdminUsersList = _oAdmin.Get_AdminUsers_List_DAL(c_ID).ToList();
            return View(_oAdmin);


        }



        //this method is for getting adminRoles by adminuserID
        [HttpGet]
        public IActionResult AllRoles()
        {
            ViewBag.PageTitle = "Asign Role";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion



            _oAdmin.roleCategorieslist = _oAdmin.GetRolesCategoryList_DAL().ToList();



            return View(_oAdmin);

            //return View();


        }


        //this method is saving new rol in Roles_Category table
        [HttpPost]
        public IActionResult InsertRoles(string Role_Name)
        {
            string message = "";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (String.IsNullOrEmpty(Role_Name) || String.IsNullOrWhiteSpace(Role_Name))
            {
                TempData["Error"] = "Please fill the role field!";
                return RedirectToAction("AllRoles", "Admin");
            }

            if (Role_Name == null)
            {
                TempData["Error"] = "Please fill the role field!";
                return RedirectToAction("AllRoles", "Admin");
            }



            message = _oAdmin.Insert_Roles_Category_DAL(Role_Name);
            if (message == "Saved Successfully!")
            {

                TempData["Success"] = message;
                return RedirectToAction("AllRoles", "Admin");
            }
            else
            {
                TempData["Error"] = message;
                return RedirectToAction("AllRoles", "Admin");
            }


        }


        //method for deleting Role from Roles_Category Table
        [HttpPost]
        public IActionResult DeleteRole(int? RoleIDDelete)
        {
            string message = "";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion



            if (RoleIDDelete == null)
            {
                TempData["Error"] = "ID is null!";
                return RedirectToAction("AllRoles", "Admin");
            }


            message = _oAdmin.Delete_RolesCategory_DAL(RoleIDDelete);
            if (message == "Deleted sucessfully!")
            {

                TempData["Success"] = "Deleted sucessfully";
                return RedirectToAction("AllRoles", "Admin");
            }
            else
            {
                TempData["Error"] = "Error in deleting the Role!";
                return RedirectToAction("AllRoles", "Admin");
            }
        }



        [HttpPost]
        public IActionResult Admin_Users(string UserName, string FullName, string Designation, string Email, string User_Mobile_No, string User_OfficeNo, string password, int? c_ID, IFormFile UserAttachment)
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
            var newFileName = "";
            var filepath = "";

            #region//Email is already exist
            var IsExistEmail = _oAdmin.Get_AdminUsers_Email_DAL(Email);
            if (IsExistEmail)
            {
                TempData["Error"] = "Email id already exist";
                return RedirectToAction("UserManagement", "Admin");
            }
            #endregion

            #region//UserName is already exist
            var IsExistUser = _oAdmin.Get_AdminUsers_UserName_DAL(UserName);
            if (IsExistUser)
            {
                TempData["Error"] = "User Name already exist";
                return RedirectToAction("UserManagement", "Admin");

            }
            #endregion
        
  
            #region saving file
            if (UserAttachment != null)
            {
                if (UserAttachment.Length > 0)
                {
                    try
                    {

                        //string Image = Path.Combine(Server.MapPath(filePath), imgname);


                        if (UserAttachment.Length > 0)
                        {
                  
                            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                            //Getting FileName
                            var fileName = Path.GetFileName(UserAttachment.FileName);

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
                                UserAttachment.CopyTo(fs);
                                fs.Flush();
                            }
                            using (Image image = Image.Load(filepath))
                            {
                                image.Mutate(x => x
                                     .Resize(384, 167));
                                image.Save(filepath); // Automatic encoder selected based on extension.
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        TempData["Error"] = "Images not saved successfully!";
                    }

                }

            }


            #endregion
            message = _oAdmin.Insert_AdminUser_DAL(UserName, FullName, Designation, Email, User_Mobile_No, User_OfficeNo, password, c_ID, newFileName, filepath);

            if (message == "Saved Successfully!")
            {
                TempData["Success"] = "user added successfully";
                return RedirectToAction("UserManagement", "Admin");
            }
            else
            {
                TempData["Error"] = "Error in saving the record";
                return RedirectToAction("UserManagement", "Admin");
            }



        }



        //method for updating Admin Users
        [HttpPost]
        public IActionResult UpdateAdmin(int? AdminUser_IDUdpate, string EmailUpdate, string User_Mobile_NoUpdate, string User_OfficeNoUpdate, string FullNameUpdate, string DesignationUpdate, int? c_IDUpdate, IFormFile UserAttachment, string remove)
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
            var newFileName = "";
            var filepath = "";


            #region//Email is already exist
            var IsExistEmail = _oAdmin.Get_AdminUsers_Email_ForUpdate_DAL(AdminUser_IDUdpate, EmailUpdate);
            if (IsExistEmail)
            {
                TempData["Error"] = "Email id already exist";
                return RedirectToAction("UserManagement", "Admin");
            }
            #endregion
            //Getting image dimension(width and height)

            #region saving file
            if (remove == null){ remove = ""; }
            if (remove.Contains("Remove eSignature"))
            {
                UserAttachment = null;
            }
            if (UserAttachment != null)
            {
                if (UserAttachment.Length > 0)
                {
                    try
                    {

                        //string Image = Path.Combine(Server.MapPath(filePath), imgname);


                        if (UserAttachment.Length > 0)
                        {
                     
                            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                            //Getting FileName
                            var fileName = Path.GetFileName(UserAttachment.FileName);

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
                                UserAttachment.CopyTo(fs);
                                fs.Flush();
                            }
                            using (Image image = Image.Load(filepath))
                            {
                                image.Mutate(x => x
                                     .Resize(384, 167));
                                image.Save(filepath); // Automatic encoder selected based on extension.
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        TempData["Error"] = "Images not saved successfully!";
                    }

                }

            }


            #endregion

            message = _oAdmin.Update_AdminUsers_DAL(AdminUser_IDUdpate, EmailUpdate, User_Mobile_NoUpdate, User_OfficeNoUpdate, FullNameUpdate, DesignationUpdate, c_IDUpdate,newFileName, filepath);

            if (message == "Updated Successfully!")
            {
                TempData["Success"] = "User record updated successfully";
                return RedirectToAction("UserManagement", "Admin");
            }
            else
            {
                TempData["Error"] = "Error in saving the record";
                return RedirectToAction("UserManagement", "Admin");
            }


        }


        //method for deleting record from "Enquiries" table
        [HttpPost]
        public IActionResult DeleteAdminUser(int? AdminUser_IDDelete, string AdminUserName)
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


            if (AdminUser_IDDelete == null)
            {
                //return new HttpResponse("");
            }

            if (AdminUserName == "Admin" || AdminUser_IDDelete == 1)
            {
                TempData["Error"] = "Can not delete user name with Admin";

                return RedirectToAction("UserManagement", "Admin");
            }

            message = _oAdmin.Delete_AdminUsers_DAL(AdminUser_IDDelete);
            if (message == "Deleted sucessfully")
            {
                TempData["Success"] = message;

                return RedirectToAction("UserManagement", "Admin");
            }
            else
            {
                TempData["Error"] = "Error in deleting the record";
                return RedirectToAction("UserManagement", "Admin");
            }




        }



        [HttpGet]
        public IActionResult ViewRolesRight(int? RoleCategory_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion



            ViewBag.RoleCategory_ID = RoleCategory_ID;



            _oAdmin.rolesmenus = _oAdmin.Get_RolesMenus_ByID_DAL(RoleCategory_ID);


            return View(_oAdmin);
        }



        [HttpGet]
        public IActionResult GlobalSetting()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            ViewBag.PageTitle = "Global Setting";


            _oAdmin.AppSettingList = _oAdmin.GetAppSettingsList_DAL().ToList();


            return View(_oAdmin);
        }

        //---Method for Updating Engine type
        [HttpPost]
        public IActionResult UpdateGlobalSetting(int? IDUpdate, string VALUEUpdate)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (IDUpdate == null)
            {
                TempData["Error"] = "ID is null";
                return RedirectToAction("GlobalSetting", "Admin");
            }

            if (String.IsNullOrEmpty(VALUEUpdate))
            {
                TempData["Error"] = "Value field is null. Please try again.";
                return RedirectToAction("GlobalSetting", "Admin");
            }




            string message = "";

            //---saving in datebase
            try
            {
                message = _oAdmin.UpdateGlobalSetting_DAL(IDUpdate, VALUEUpdate);
                if (message == "Updated Successfully!")
                {
                    TempData["Success"] = "Updated Successfully!";
                    return RedirectToAction("GlobalSetting", "Admin");
                }
                else
                {
                    TempData["Error"] = "Error occured in saving the record!";
                    return RedirectToAction("GlobalSetting", "Admin");
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "Not saved successfully!";
                return RedirectToAction("GlobalSetting", "Admin");
            }

            //return View();
        }



        [HttpPost]
        public ActionResult AddRightsToRole(IFormCollection IFormItem)
        {
            string message_1 = "";
            string message_2 = "";



            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            //string AdURL = (String.IsNullOrEmpty(IFormItem["AdURL"]) ? "" : IFormItem["AdURL"]);
            //string AdStatus = (String.IsNullOrEmpty(IFormItem["AdStatus"]) ? "" : IFormItem["AdStatus"]);

            //Getting RoleCategory_ID
            string RoleCategory_ID;
            if (string.IsNullOrEmpty(IFormItem["RoleCategory_ID"]))
            {
                RoleCategory_ID = "0";
            }
            else
            {
                RoleCategory_ID = IFormItem["RoleCategory_ID"];
            }


            //Getting Menus
            bool Menu_DashBoard_View = !String.IsNullOrEmpty(IFormItem["Menu_DashBoard_View"]) ? true : false;

            bool Menu_Stock_View = !String.IsNullOrEmpty(IFormItem["Menu_Stock_View"]) ? true : false;

            bool Menu_Sale_View = !String.IsNullOrEmpty(IFormItem["Menu_Sale_View"]) ? true : false;

            bool Menu_Purchases_View = !String.IsNullOrEmpty(IFormItem["Menu_Purchases_View"]) ? true : false;

            bool Menu_Delivery_View = !String.IsNullOrEmpty(IFormItem["Menu_Delivery_View"]) ? true : false;

            bool Menu_Reports_View = !String.IsNullOrEmpty(IFormItem["Menu_Reports_View"]) ? true : false;

            bool Menu_Accounts_View = !String.IsNullOrEmpty(IFormItem["Menu_Accounts_View"]) ? true : false;

            bool Menu_BasicData_View = !String.IsNullOrEmpty(IFormItem["Menu_BasicData_View"]) ? true : false;

            bool Menu_Admin_View = !String.IsNullOrEmpty(IFormItem["Menu_Admin_View"]) ? true : false;

            bool Menu_HR_View = !String.IsNullOrEmpty(IFormItem["Menu_HR_View"]) ? true : false;

            bool DashBoard_Add = !String.IsNullOrEmpty(IFormItem["DashBoard_Add"]) ? true : false;
            bool DashBoard_Update = !String.IsNullOrEmpty(IFormItem["DashBoard_Update"]) ? true : false;
            bool DashBoard_Delete = !String.IsNullOrEmpty(IFormItem["DashBoard_Delete"]) ? true : false;

            bool Stock_Add = !String.IsNullOrEmpty(IFormItem["Stock_Add"]) ? true : false;
            bool Stock_Update = !String.IsNullOrEmpty(IFormItem["Stock_Update"]) ? true : false;
            bool Stock_Delete = !String.IsNullOrEmpty(IFormItem["Stock_Delete"]) ? true : false;

            bool Purchase_Add = !String.IsNullOrEmpty(IFormItem["Purchase_Add"]) ? true : false;
            bool Purchase_Update = !String.IsNullOrEmpty(IFormItem["Purchase_Update"]) ? true : false;
            bool Purchase_Delete = !String.IsNullOrEmpty(IFormItem["Purchase_Delete"]) ? true : false;

            bool Vanning_Add = !String.IsNullOrEmpty(IFormItem["Vanning_Add"]) ? true : false;
            bool Vanning_Uddate = !String.IsNullOrEmpty(IFormItem["Vanning_Uddate"]) ? true : false;
            bool Vanning_Delete = !String.IsNullOrEmpty(IFormItem["Vanning_Delete"]) ? true : false;

            bool Shipping_Add = !String.IsNullOrEmpty(IFormItem["Shipping_Add"]) ? true : false;
            bool Shipping_Update = !String.IsNullOrEmpty(IFormItem["Shipping_Update"]) ? true : false;
            bool Shipping_Delete = !String.IsNullOrEmpty(IFormItem["Shipping_Delete"]) ? true : false;

            bool Rekso_Add = !String.IsNullOrEmpty(IFormItem["Rekso_Add"]) ? true : false;
            bool Rekso_Update = !String.IsNullOrEmpty(IFormItem["Rekso_Update"]) ? true : false;
            bool Rekso_Delete = !String.IsNullOrEmpty(IFormItem["Rekso_Delete"]) ? true : false;

            bool Papers_Add = !String.IsNullOrEmpty(IFormItem["Papers_Add"]) ? true : false;
            bool Papers_Update = !String.IsNullOrEmpty(IFormItem["Papers_Update"]) ? true : false;
            bool Papers_Delete = !String.IsNullOrEmpty(IFormItem["Papers_Delete"]) ? true : false;

            bool Sale_Add = !String.IsNullOrEmpty(IFormItem["Sale_Add"]) ? true : false;
            bool Sale_Update = !String.IsNullOrEmpty(IFormItem["Sale_Update"]) ? true : false;
            bool Sale_Delete = !String.IsNullOrEmpty(IFormItem["Sale_Delete"]) ? true : false;

            bool Payments_Add = !String.IsNullOrEmpty(IFormItem["Payments_Add"]) ? true : false;
            bool Payments_Update = !String.IsNullOrEmpty(IFormItem["Payments_Update"]) ? true : false;
            bool Payments_Delete = !String.IsNullOrEmpty(IFormItem["Payments_Delete"]) ? true : false;

            bool Receipts_Add = !String.IsNullOrEmpty(IFormItem["Receipts_Add"]) ? true : false;
            bool Receipts_Update = !String.IsNullOrEmpty(IFormItem["Receipts_Update"]) ? true : false;
            bool Receipts_Delete = !String.IsNullOrEmpty(IFormItem["Receipts_Delete"]) ? true : false;

            bool Reports_Add = !String.IsNullOrEmpty(IFormItem["Reports_Add"]) ? true : false;
            bool Reports_Update = !String.IsNullOrEmpty(IFormItem["Reports_Update"]) ? true : false;
            bool Reports_Delete = !String.IsNullOrEmpty(IFormItem["Reports_Delete"]) ? true : false;

            bool BasicData_Add = !String.IsNullOrEmpty(IFormItem["BasicData_Add"]) ? true : false;
            bool BasicData_Update = !String.IsNullOrEmpty(IFormItem["BasicData_Update"]) ? true : false;
            bool BasicData_Delete = !String.IsNullOrEmpty(IFormItem["BasicData_Delete"]) ? true : false;

            bool Admin_Add = !String.IsNullOrEmpty(IFormItem["Admin_Add"]) ? true : false;
            bool Admin_Update = !String.IsNullOrEmpty(IFormItem["Admin_Update"]) ? true : false;
            bool Admin_Delete = !String.IsNullOrEmpty(IFormItem["Admin_Delete"]) ? true : false;





            message_1 = _oAdmin.Insert_Roles_Menus_DAL(RoleCategory_ID, Menu_DashBoard_View, Menu_Stock_View, Menu_Sale_View, Menu_Purchases_View, Menu_Delivery_View,
            Menu_Reports_View, Menu_Accounts_View, Menu_BasicData_View, Menu_Admin_View, Menu_HR_View);




            message_2 = _oAdmin.Insert_Roles_Forms_DAL(RoleCategory_ID, DashBoard_Add, DashBoard_Update, DashBoard_Delete, Stock_Add, Stock_Update, Stock_Delete, Purchase_Add, Purchase_Update, Purchase_Delete,
Vanning_Add, Vanning_Uddate, Vanning_Delete, Shipping_Add, Shipping_Update, Shipping_Delete, Rekso_Add, Rekso_Update, Rekso_Delete, Papers_Add, Papers_Update, Papers_Delete, Sale_Add, Sale_Update,
Sale_Delete, Payments_Add, Payments_Update, Payments_Delete, Receipts_Add, Receipts_Update, Receipts_Delete, Reports_Add, Reports_Update, Reports_Delete, BasicData_Add, BasicData_Update, BasicData_Delete, Admin_Add, Admin_Update, Admin_Delete);






            TempData["message"] = message_1 + " " + message_2;


            return RedirectToAction("ViewRolesRight", "Admin", new { RoleCategory_ID = RoleCategory_ID });


        }


        public JsonResult Get_MenyName_By_Type(int MenuType)
        {
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            List<AdminRights> menu = new List<AdminRights>();





            menu = _oAdmin.GetMenuList_DAL(MenuType).ToList();

            return Json(new { menu });

        }
        public IActionResult ViewRightsList(int? RoleCategory_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion



            //this id will be use on page for different purposes
            ViewBag.RoleCategory_ID = RoleCategory_ID;




            _oAdmin.adminrights = _oAdmin.Get_AdminRightsByAdminUser_ID_DAL(RoleCategory_ID).ToList();



            return View(_oAdmin);
        }



        //method for adding role to specific Admin
        [HttpPost]
        public IActionResult AddAdminRights(int MenuID, int? Role_ID, string View, string Add, string Edit, string Delete, string Print, string Excel, string IsAdmin, string MenuType)
        {
            string message = "";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            if (MenuType == null || MenuType == "0")
            {
                TempData["Error"] = "Please select MenuType from dropdown!";
                return RedirectToAction("ViewRightsList", "Admin", new { RoleCategory_ID = Role_ID });
            }
            if (MenuID == 0)
            {
                TempData["Error"] = "Please select Menu from dropdown!";
                return RedirectToAction("ViewRightsList", "Admin", new { RoleCategory_ID = Role_ID });
            }

            if (Role_ID == null)
            {
                TempData["Error"] = "Something Went null";
                return RedirectToAction("ViewRightsList", "Admin", new { RoleCategory_ID = Role_ID });
            }



            message = _oAdmin.Insert_AdminRoleForSepcificUser_DAL(MenuID, Role_ID, View, Add, Edit, Delete, Print, Excel, IsAdmin, MenuType);
            if (message.Contains("Saved Successfully!"))
            {

                TempData["Success"] = message;
                return RedirectToAction("ViewRightsList", "Admin", new { RoleCategory_ID = Role_ID });
            }
            else
            {
                TempData["Error"] = message;
                return RedirectToAction("ViewRightsList", "Admin", new { RoleCategory_ID = Role_ID });
            }
        }
        [HttpPost]
        public IActionResult UpdateAdminRights(int ID, int? Role_ID, string View, string Add, string Edit, string Delete, string Print, string Excel, string IsAdmin)
        {
            string message = "";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion





            if (ID < 1)
            {
                TempData["Error"] = "Something Went wrong";
                return RedirectToAction("ViewRightsList", "Admin", new { RoleCategory_ID = Role_ID });
            }



            message = _oAdmin.Update_AdminRoleForSepcificUser_DAL(ID, View, Add, Edit, Delete, Print, Excel, IsAdmin);
            if (message.Contains("Updated Successfully!"))
            {

                TempData["Success"] = message;
                return RedirectToAction("ViewRightsList", "Admin", new { RoleCategory_ID = Role_ID });
            }
            else
            {
                TempData["Error"] = message;
                return RedirectToAction("ViewRightsList", "Admin", new { RoleCategory_ID = Role_ID });
            }
        }

        [HttpPost]
        public IActionResult DeleteAdminRights(int? RightsIDDelete, int? Role_ID)
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


            if (RightsIDDelete == null)
            {
                //return new HttpResponse("");
            }



            message = _oAdmin.Delete_AdminRights_DAL(RightsIDDelete);
            if (message == "Deleted sucessfully")
            {
                TempData["Success"] = message;

                return RedirectToAction("ViewRightsList", "Admin", new { RoleCategory_ID = Role_ID });
            }
            else
            {
                TempData["Error"] = "Error in deleting the record";
                return RedirectToAction("ViewRightsList", "Admin", new { RoleCategory_ID = Role_ID });
            }




        }
        public IActionResult AdminRolesList(int? AdminUser_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            if (AdminUser_ID == null || AdminUser_ID == 0)
            {
                //return HttpNotFound();
            }

            //this id will be use on page for different purposes
            ViewBag.AdminUserID = AdminUser_ID;


            var RolesCategory = _oAdmin.GetRolesCategoryList_DAL().ToList();
            ViewBag.RolesCategory = RolesCategory;


            _oAdmin.adminroles = _oAdmin.Get_AdminRolesByAdminUser_ID_DAL(AdminUser_ID).ToList();



            return View(_oAdmin);
        }
        [HttpPost]
        public IActionResult AddAdminRole(string AdmRole, int? AdminUser_IDAdd)
        {
            string message = "";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion



            if (String.IsNullOrEmpty(AdmRole) || String.IsNullOrWhiteSpace(AdmRole))
            {
                TempData["Error"] = "Please select role from dropdown!";
                return RedirectToAction("AdminRolesList", "Admin", new { AdminUser_ID = AdminUser_IDAdd });
            }

            if (AdminUser_IDAdd == null)
            {
                TempData["Error"] = "something went null";
                return RedirectToAction("AdminRolesList", "Admin", new { AdminUser_ID = AdminUser_IDAdd });
            }



            message = _oAdmin.Insert_AdminRoleUser_DAL(AdmRole, AdminUser_IDAdd);
            if (message.Contains("Saved Successfully!"))
            {

                TempData["Success"] = message;
                return RedirectToAction("AdminRolesList", "Admin", new { AdminUser_ID = AdminUser_IDAdd });
            }
            else
            {
                TempData["Error"] = message;
                return RedirectToAction("AdminRolesList", "Admin", new { AdminUser_ID = AdminUser_IDAdd });
            }
        }
        [HttpPost]
        public IActionResult DeleteAdminRole(int? AdminUser_IDDelete, int? RoleIDDelete)
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


            if (AdminUser_IDDelete == null)
            {
                //return new HttpResponse("");
            }



            message = _oAdmin.Delete_AdminRoles_DAL(RoleIDDelete);
            if (message == "Deleted sucessfully")
            {
                TempData["Success"] = message;

                return RedirectToAction("AdminRolesList", "Admin", new { AdminUser_ID = AdminUser_IDDelete });
            }
            else
            {
                TempData["Error"] = "Error in deleting the record";
                return RedirectToAction("AdminRolesList", "Admin", new { AdminUser_ID = AdminUser_IDDelete });
            }




        }
    }
}
