using DAL.Models;
using DAL.oClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PT_ERP.Controllers
{
    public class BaseController : Controller
    {


        private readonly IOBasicData _oBasic;


        public BaseController(IOBasicData oBasicBase)
        {

            _oBasic = oBasicBase;



        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //By Yaseen
            //get AlterNotification from Session
            var alters = HttpContext.Session.GetString("Alters");

            ViewBag.Home_Dashboard = HttpContext.Session.GetString("Home_Dashboard");

            //  ViewBag.AlertCounts = List.Count;

            if (!String.IsNullOrEmpty(alters))
            {

                  var AlterNotification = JsonConvert.DeserializeObject<List<AlertNotification_DAL>>(alters);

                ViewBag.AlterNotification = AlterNotification;

                //ViewBag.AlertCount = AlterNotification[0].alert_count;
            }
            else
            {
                  ViewBag.AlterNotification = null;
                ViewBag.AlertCount = 0;
            }
            //By Yaseen 

            #region Getting Website Menu



            //---Get Main Menu from session
            var mainmenu = HttpContext.Session.GetString("MainMenu");
            if (!String.IsNullOrEmpty(mainmenu))
            {
                ViewBag.MainMenuList = JsonConvert.DeserializeObject<List<Pa_MainMenu_DAL>>(mainmenu);
            }
            else
            {
                ViewBag.MainMenuList = null;
            }


            //--Get Menu level 1 from session
            var menulevel1 = HttpContext.Session.GetString("MenuLevel1");
            if (!String.IsNullOrEmpty(menulevel1))
            {
                ViewBag.MenuLevel1List = JsonConvert.DeserializeObject<List<Pa_MenuLevel1_DAL>>(menulevel1);
            }
            else
            {
                ViewBag.MenuLevel1List = null;
            }


            //--Get Menu Level 2 from session
            var menulevel2 = HttpContext.Session.GetString("MenuLevel2");
            if (!String.IsNullOrEmpty(menulevel2))
            {
                ViewBag.MenuLevel2List = JsonConvert.DeserializeObject<List<Pa_MenuLevel2_DAL>>(menulevel2);
            }
            else
            {
                ViewBag.MenuLevel2List = null;
            }
            var ETA_Alert = _oBasic.Get_Notification_ETA_DAL();
            var ETA_Alerts = JsonConvert.SerializeObject(ETA_Alert);
           
            if (!String.IsNullOrEmpty(ETA_Alerts))
            {
                ViewBag.ETA = JsonConvert.DeserializeObject<List<pa_Shipping_Info>>(ETA_Alerts);
            }
            else
            {
                ViewBag.ETA = null;
            }

            #endregion


            #region getting all roles of admin

            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            if (!String.IsNullOrEmpty(UserName_Admin) || !String.IsNullOrEmpty(UserID_Admin))
            {



                _oBasic.rolesmenus = _oBasic.Get_AdminRolesByAdminUserID_DAL(Convert.ToInt32(UserID_Admin));
                ViewBag.AdminRolesRights = _oBasic.rolesmenus;

                ViewBag.UserNameValidate = UserName_Admin;
                ViewBag.CompanyValidate = c_ID;
                ViewBag.User = "admin";
                ViewBag.CompanyOne = 1;
                ViewBag.CompanyTwo = 2;
            }



            else
            {
                //following some lines in else codition are only for avoiding null reference at runtime bcz this veiwBag.AdminRolesRights is used in too many pages.

                _oBasic.rolesmenus = _oBasic.Get_AdminRolesByAdminUserID_DAL(0);
                ViewBag.AdminRolesRights = _oBasic.rolesmenus;

                ViewBag.UserNameValidate = UserName_Admin;
                ViewBag.CompanyValidate = c_ID;
                ViewBag.User = "admin";
                ViewBag.CompanyOne = 1;
                ViewBag.CompanyTwo = 2;
            }

            #endregion


        }
       


    }
}
