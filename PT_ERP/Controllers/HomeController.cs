using DAL.oClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using ChartJSCore.Helpers;
//using ChartJSCore.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PT_ERP.Models;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace PT_ERP.Controllers
{
    public class HomeController : BaseController
    {

        private readonly IOHome _oHome;
        
        // By Yaseen 
        private readonly IOAccounts _oAccounts;

        private readonly IOHR _oHR;

        private IConfiguration configuration;


        public HomeController(IOBasicData oBasicBase, IOPurchases oPurchase, IOHome oHome, IOAccounts oAccounts  ,IOHR oHR, IConfiguration iConfig)
           : base(oBasicBase)
        {
            _oHome = oHome;
            _oAccounts = oAccounts;
            _oHR = oHR;
            configuration = iConfig;

        }

        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

        public IActionResult Index()
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

            _oHome.DashboardObject = _oHome.Get_Dashboard_Stats(c_ID);



            ViewBag.ShowGenInventoryDashboard = configuration.GetSection("AppSettings").GetSection("ShowGenInventoryDashboard").Value;

            ViewBag.HideVehDashboardBarGraph = configuration.GetSection("AppSettings").GetSection("HideVehDashboardBarGraph").Value;


            ViewBag.DataPoints = JsonConvert.SerializeObject(_oHome.Get_Inv_LAST_7DAYS_SALE(c_ID), _jsonSetting);





            return View(_oHome);
        }


        [HttpPost]
        public JsonResult Get_Week_Days()
        {

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);

            var weekendName = _oHome.Get_Inv_LAST_7DAYS_SALE(c_ID).Select(x => x.DAYSS);
            return Json(weekendName);
        }

        [HttpPost]
        public JsonResult Get_Amt()
        {

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);

            var Amt = _oHome.Get_Inv_LAST_7DAYS_SALE(c_ID).Select(x => x.AMT);
            return Json(Amt);
        }

        [HttpPost]
        public JsonResult GET_Sale_Label()
        {

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);

            var Amt = _oHome.Get_Inv_LAST_7DAYS_SALE(c_ID).Select(x => x.AMT);
            return Json(Amt);
        }


        [HttpPost]
        public JsonResult GET_Top_Sale_Value_Items_trd_Sale()
        {
            var Amt = _oHome.TopSaleCategory_trd().Select(x => x.Sale);
            return Json(Amt);
        }



        [HttpPost]
        public JsonResult GET_Top_Sale_Value_Items_trd_ItemName()
        {
            var Amt = _oHome.TopSaleCategory_trd().Select(x => x.Item_Name);
            return Json(Amt);
        }








        //public void LineChart()
        //    {
        //    string[] weekdays = new string[7];
        //    double[] weekdaysValue = new double[7];

        //    Chart chart = new Chart();
        //    List<DAL.Models.DashboardStats> obj = new List<DAL.Models.DashboardStats>();



        //    _oHome.DashboardListObject = _oHome.Get_Inv_LAST_7DAYS_SALE(1);

        //    foreach (DAL.Models.DashboardStats item in _oHome.DashboardListObject)
        //    {
        //        weekdays.ToList().Add(item.DAYSS);
        //        weekdaysValue.ToList().Add(Convert.ToDouble(item.AMT));
        //    }

        //        chart.Type = Enums.ChartType.Line;

        //    ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();
        //    // data.Labels = new List<string>() { "Sat", "Sun", "Mon", "Tue", "Wed", "Thr", "Fri" };

        //    data.Labels = weekdays;

        //    LineDataset dataset = new LineDataset()
        //    {
        //        Label = "Last 7 Days Sale",
        //        Data = new List<double> { 4,5,8,10,30 },
        //        Fill = "false",
        //        LineTension = 0.1,
        //        BackgroundColor = ChartColor.FromRgba(75, 192, 192, 0.4),
        //        BorderColor = ChartColor.FromRgb(75, 192, 192),
        //        BorderCapStyle = "butt",
        //        BorderDash = new List<int> { },
        //        BorderDashOffset = 0.0,
        //        BorderJoinStyle = "miter",
        //        PointBorderColor = new List<ChartColor> { ChartColor.FromRgb(75, 192, 192) },
        //        PointBackgroundColor = new List<ChartColor> { ChartColor.FromHexString("#ffffff") },
        //        PointBorderWidth = new List<int> { 1 },
        //        PointHoverRadius = new List<int> { 5 },
        //        PointHoverBackgroundColor = new List<ChartColor> { ChartColor.FromRgb(75, 192, 192) },
        //        PointHoverBorderColor = new List<ChartColor> { ChartColor.FromRgb(220, 220, 220) },
        //        PointHoverBorderWidth = new List<int> { 2 },
        //        PointRadius = new List<int> { 1 },
        //        PointHitRadius = new List<int> { 10 },
        //        SpanGaps = false
        //    };

        //    data.Datasets = new List<Dataset>();
        //    data.Datasets.Add(dataset);

        //    chart.Data = data;

        //    ViewData["chart"] = chart;

        //    }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult VehiclesDashboard()
        {
            ViewBag.PageTitle = "Home";
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

            _oHome.DashboardObject = _oHome.Get_Dashboard_Stats(c_ID);
            _oHome.DashboardCurrencyObject = _oHome.Get_Dashboard_Currency(c_ID);
            _oHome.AllBankBalanceListObject = _oHome.get_All_bank_balance(c_ID);

            int AlterCount = _oHR.Get_AlertNotificationList_DAL(Convert.ToInt32(UserID_Admin)).ToList().Count;
           
            HttpContext.Session.SetString("AlterCount",Convert.ToString(AlterCount));

            ViewBag.ShowGenInventoryDashboard = configuration.GetSection("AppSettings").GetSection("ShowGenInventoryDashboard").Value;

            ViewBag.HideVehDashboardBarGraph = configuration.GetSection("AppSettings").GetSection("HideVehDashboardBarGraph").Value;

            
            ViewBag.HideCashOrBank = configuration.GetSection("AppSettings").GetSection("ShowCashBankFromDashboard").Value;

            ViewBag.DataPoints = JsonConvert.SerializeObject(_oHome.Get_Inv_LAST_7DAYS_SALE(c_ID), _jsonSetting);


            
            return View(_oHome);
        }
    }
}
