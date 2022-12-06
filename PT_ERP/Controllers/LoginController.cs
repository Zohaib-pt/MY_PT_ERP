using DAL.Models;
using DAL.oClasses;
using DAL.oClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace PT_ERP.Controllers
{
    public class LoginController : Controller
    {

        private readonly IOHome _oHome;
        private readonly IOBasicData _oBasic;
        private readonly IConfiguration _oConfiguration;

        public LoginController(IOBasicData oBasicBase, IOHome oHome, IConfiguration oConfiguration)

        {
            _oHome = oHome;
            _oBasic = oBasicBase;
            _oConfiguration = oConfiguration;
        }



        public IActionResult Index()
        {

            string CompanyName = _oConfiguration.GetSection("AppSettings").GetSection("CompanyName").Value;
            ViewBag.PageTitle = "Login";
            ViewBag.CompanyName = CompanyName;

            return View();
        }


        public ActionResult Login(string txtusername, string txtpassword)
        {
            ViewBag.PageTitle = "Login";
            try
            {
                bool IsExist = _oHome.Check_If_User_Exist_DAL(txtusername, txtpassword);
                if (IsExist == false)
                {
                    TempData["msg"] = "Invalid Email or Password...!";
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    //following "Get_User_Info_DAL" method only for retrieving User_Name and User_ID so that can show user name in top header
                    var obj = _oHome.Get_Users_Info_DAL(txtusername, txtpassword);



                    HttpContext.Session.SetString("UserName", obj.UserName);
                    HttpContext.Session.SetString("UserID", Convert.ToString(obj.AdminUser_ID));
                    HttpContext.Session.SetString("c_ID", Convert.ToString(obj.c_ID));
                    HttpContext.Session.SetString("Roll_ID", Convert.ToString(obj.Role_id));

                    //By Yaseen
                    //Get AlterNotification


                    var AlterNotification = _oBasic.Get_AlterNotiList_Dal(Convert.ToInt32(obj.AdminUser_ID));

              
                    
                    


                    var Alters = JsonConvert.SerializeObject(AlterNotification);

                    
                    HttpContext.Session.SetString("Alters", Alters);

                    //By Yaseen 
                    //Get AlterNotification

                    //---Getting Menu List

                    //---Get Main Menu
                    var MainMenuList = _oBasic.Get_MainMenu_DAL(Convert.ToInt32(obj.AdminUser_ID));
                    //--Get Menu level 1
                    var MenuLevel1List = _oBasic.Get_MenuLevel_1_DAL(Convert.ToInt32(obj.AdminUser_ID));

                    //--Get Menu Level 2
                    var MenuLevel2List = _oBasic.Get_MenuLevel_2_DAL(Convert.ToInt32(obj.AdminUser_ID));

                    //---Saving menus in sessions
                    var MainMenu = JsonConvert.SerializeObject(MainMenuList);
                    HttpContext.Session.SetString("MainMenu", MainMenu);

                    var Menu1 = JsonConvert.SerializeObject(MenuLevel1List);
                    HttpContext.Session.SetString("MenuLevel1", Menu1);

                    var Menu2 = JsonConvert.SerializeObject(MenuLevel2List);
                    HttpContext.Session.SetString("MenuLevel2", Menu2);

                    // Retrieve session. Please donot remove this comment. Because it is help full for deserialize purpoese
                    //var str = HttpContext.Session.GetString("MainMenu");
                    //var obj3 = JsonConvert.DeserializeObject<List<Pa_MainMenu_DAL>>(str);

                    //TempData["msg"] = "Invalid Email or Password...!";


                    string View = _oConfiguration.GetSection("AppSettings").GetSection("ISDashboard").Value;
                    
                    string Home_Dashboard = _oConfiguration.GetSection("AppSettings").GetSection("Home_Dashboard").Value;
                    HttpContext.Session.SetString("Home_Dashboard", Home_Dashboard);



                    string EnableStockList = _oConfiguration.GetSection("AppSettings").GetSection("EnableStockListAfterLogin").Value;
                   
                    if (EnableStockList == "true")
                    {
                        return RedirectToAction("StockList_fm", "Stock");
                    }
                    return RedirectToAction(View, "Home");
                }
            }
            catch (Exception ex)
            {
                string exp = ex.Message;
                TempData["msg"] = "Invalid Email or Password...!";
                return RedirectToAction("Index", "Login");
            }

        }

        [HttpPost]
        public JsonResult ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {

            string UserName = HttpContext.Session.GetString("UserName").ToString();
            int User_ID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            bool IsExist = _oHome.Check_If_User_Exist_DAL(UserName, oldPassword);


            if (IsExist == true)
            {
                bool isSuccess = _oHome.Change_User_Password(User_ID, newPassword);

                return Json(isSuccess);
            }
            else
            {
                return Json(false);
            }



        }


        public IActionResult logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Login");

        }



    }




}

