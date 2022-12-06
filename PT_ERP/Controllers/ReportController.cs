using AspNetCore.Reporting;
using AspNetCore.Reporting.ReportExecutionService;
using ClosedXML.Excel;
using DAL.oClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NumericWordsConversion;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using X.PagedList;
using DAL.Models;


namespace PT_ERP.Controllers
{
    public class ReportController : BaseController
    {

        static string ReportFolder;

        private readonly IOBasicData _oBasic;
        private readonly IOStock _oStock;
        private readonly IOPurchases _oPurchase;
        private readonly IOSales _oSales;
        private readonly IOAccounts _oAccounts;
        private readonly IOReports _oReports;
        private readonly IOInventory _oInventory;
        private readonly IODelivery _oDelivery;
        private IConfiguration configuration;



        public ReportController(IOBasicData oBasicBase, IOStock oStock, IOPurchases oPurchase, IOSales oSales, IOAccounts oAccounts, IOReports oReport, IODelivery oDelivery, IOInventory oInventory, IConfiguration iConfig)
           : base(oBasicBase)
        {
            _oBasic = oBasicBase;
            _oStock = oStock;
            _oPurchase = oPurchase;
            _oSales = oSales;
            _oAccounts = oAccounts;
            _oReports = oReport;
            _oDelivery = oDelivery;
            _oInventory = oInventory;
            configuration = iConfig;

            ReportFolder = configuration.GetSection("AppSettings").GetSection("CompanyReportFolderCode").Value;

        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetStockPrint(string reportName, string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
            string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID)
        {
            string ReportType = "pdf";
            var returnString = GenerateStockReport(reportName, ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }
        [HttpGet]
        public IActionResult GetStockExcel(string reportName, string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
    string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID, string VendorName, string ModelYear, int StockType_ID, string Container_No)
        {
            string ReportType = "excel";
            var returnString = GenerateStockReport(reportName, ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".xls");


        }
        public byte[] GenerateStockReport(string reportName, string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
            string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);


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
            StockType_ID = StockType_ID == null ? 0 : StockType_ID;

            var model = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);
            report.AddDataSource("StockDataSet", model);

            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("StockDataSet", "");
            return result.MainStream;
        }
        private RenderType GetRenderType(string reportType)
        {
            var renderType = RenderType.Pdf;
            switch (reportType.ToLower())
            {
                default:
                case "pdf":
                    renderType = RenderType.Pdf;
                    break;
                case "word":
                    renderType = RenderType.Word;
                    break;
                case "excel":
                    renderType = RenderType.Excel;
                    break;
            }

            return renderType;
        }
        [HttpGet]
        public IActionResult GetPurchasePrint(string reportName, string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, string ChassisNo, int? c_ID)
        {
            string ReportType = "pdf";
            var returnString = GeneratePurchaseReport(reportName, PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, ChassisNo, c_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }
        [HttpGet]
        public IActionResult GetPurchaseExcel(string reportName, string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, string ChassisNo, int? c_ID)
        {
            string ReportType = "excel";
            var returnString = GeneratePurchaseReport(reportName, PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, ChassisNo, c_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".xls");


        }
        public byte[] GeneratePurchaseReport(string reportName, string PurchaseRef, int? Vendor_ID,
            string From_Date, string To_Date, int? Status_ID, string ChassisNo, int? c_ID, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);


            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;
            From_Date = String.IsNullOrEmpty(From_Date) ? "" : From_Date;
            To_Date = String.IsNullOrEmpty(To_Date) ? "" : To_Date;
            Status_ID = Status_ID == null ? 0 : Status_ID;
            c_ID = c_ID == null ? 1 : c_ID;

            var model = _oPurchase.Get_PurchaseMasterList_DAL(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, ChassisNo, c_ID);
            report.AddDataSource("PurchaseDataSet", model);

            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("PurchaseDataSet", "");
            return result.MainStream;
        }


        [HttpGet]
        public IActionResult GetPurchaseOtherPrint(string reportName, string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID)
        {
            string ReportType = "pdf";
            var returnString = GeneratePurchaseOtherReport(reportName, PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, c_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }
        [HttpGet]
        public IActionResult GetPurchaseOtherExcel(string reportName, string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID)
        {
            string ReportType = "excel";
            var returnString = GeneratePurchaseOtherReport(reportName, PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, c_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".xls");


        }
        public byte[] GeneratePurchaseOtherReport(string reportName, string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);


            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;
            From_Date = String.IsNullOrEmpty(From_Date) ? "" : From_Date;
            To_Date = String.IsNullOrEmpty(To_Date) ? "" : To_Date;
            Status_ID = Status_ID == null ? 0 : Status_ID;
            c_ID = c_ID == null ? 1 : c_ID;

            var model = _oPurchase.Get_PurchaseMasterList_Other_DAL(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, c_ID);
            report.AddDataSource("PurchaseDataSet", model);

            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("PurchaseDataSet", "");
            return result.MainStream;
        }


        [HttpGet]
        public IActionResult GetPurchaseReturnPrint(string reportName, string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID)
        {
            string ReportType = "pdf";
            var returnString = GeneratePurchaseReturnReport(reportName, PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, c_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }
        [HttpGet]
        public IActionResult GetPurchaseReturnExcel(string reportName, string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID)
        {
            string ReportType = "excel";
            var returnString = GeneratePurchaseReturnReport(reportName, PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, c_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".xls");


        }
        public byte[] GeneratePurchaseReturnReport(string reportName, string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);


            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;
            From_Date = String.IsNullOrEmpty(From_Date) ? "" : From_Date;
            To_Date = String.IsNullOrEmpty(To_Date) ? "" : To_Date;
            Status_ID = Status_ID == null ? 0 : Status_ID;
            c_ID = c_ID == null ? 1 : c_ID;

            var model = _oPurchase.Get_PurchaseMasterReturnList_DAL(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, c_ID);
            report.AddDataSource("PurchaseDataSet", model);

            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("PurchaseDataSet", "");
            return result.MainStream;
        }
        [HttpGet]
        public IActionResult GetPerformaInvoicePrint(string reportName, string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, string c_ID)
        {
            string ReportType = "pdf";
            var returnString = GeneratePerformaInvoiceReport(reportName, SaleRef, StartDate, EndDate, Customer_Name, Chassis_No, c_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }
        [HttpGet]
        public IActionResult GetPerformaInvoiceExcel(string reportName, string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, string c_ID)
        {
            string ReportType = "excel";
            var returnString = GeneratePerformaInvoiceReport(reportName, SaleRef, StartDate, EndDate, Customer_Name, Chassis_No, c_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".xls");


        }
        public byte[] GeneratePerformaInvoiceReport(string reportName, string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, string c_ID, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);


            int cID = Convert.ToInt32(c_ID);
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_No = String.IsNullOrEmpty(Chassis_No) ? "" : Chassis_No;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;

            var model = _oSales.Get_PerformaInvoice_List_DAL(SaleRef, StartDate, EndDate, Customer_Name, Chassis_No, cID).ToList();
            report.AddDataSource("SaleDataSet", model);

            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("SaleDataSet", "");
            return result.MainStream;
        }
        [HttpGet]
        public IActionResult GetSalesBookingPrint(string reportName, string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, string c_ID)
        {
            string ReportType = "pdf";
            var returnString = GenerateSalesBookingReport(reportName, SaleRef, StartDate, EndDate, Customer_Name, Chassis_No, c_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }
        [HttpGet]
        public IActionResult GetSalesBookingExcel(string reportName, string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, string c_ID)
        {
            string ReportType = "excel";
            var returnString = GenerateSalesBookingReport(reportName, SaleRef, StartDate, EndDate, Customer_Name, Chassis_No, c_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".xls");


        }
        public byte[] GenerateSalesBookingReport(string reportName, string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, string c_ID, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);


            int cID = Convert.ToInt32(c_ID);
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_No = String.IsNullOrEmpty(Chassis_No) ? "" : Chassis_No;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;

            var model = _oSales.Get_SalesMasterBooking_List_DAL(SaleRef, StartDate, EndDate, Customer_Name, Chassis_No, 0, cID).ToList();
            report.AddDataSource("SaleDataSet", model);

            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("SaleDataSet", "");
            return result.MainStream;
        }
        [HttpGet]
        public IActionResult GetSalesInvoicePrint(string reportName, string SaleRef, string ManualRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string Chassis_No, string c_ID, int Make, int Model_Desc, string Model_Year)
        {
            Make = Make == 0 ? 0 : Make;
            Model_Desc = Model_Desc == 0 ? 0 : Model_Desc;
            Model_Year = String.IsNullOrEmpty(Model_Year) ? "" : Model_Year;
            string ReportType = "pdf";
            var returnString = GenerateSalesInvoiceReport(reportName, SaleRef, ManualRef, StartDate, EndDate, Customer_Name, Status_ID, Chassis_No, c_ID, ReportType, Make, Model_Desc, Model_Year);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }
        [HttpGet]
        public IActionResult GetSalesInvoiceExcel(string reportName, string SaleRef, string ManualRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string Chassis_No, string c_ID, int Make, int Model_Desc, string Model_Year)
        {
            Make = Make == 0 ? 0 : Make;
            Model_Desc = Model_Desc == 0 ? 0 : Model_Desc;
            Model_Year = String.IsNullOrEmpty(Model_Year) ? "" : Model_Year;
            string ReportType = "excel";
            var returnString = GenerateSalesInvoiceReport(reportName, SaleRef, ManualRef, StartDate, EndDate, Customer_Name, Status_ID, Chassis_No, c_ID, ReportType, Make, Model_Desc, Model_Year);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".xls");


        }
        public byte[] GenerateSalesInvoiceReport(string reportName, string ManualRef, string SaleRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string Chassis_No, string c_ID, string ReportType, int Make, int Model_Desc, string Model_Year)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);


            int cID = Convert.ToInt32(c_ID);
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_No = String.IsNullOrEmpty(Chassis_No) ? "" : Chassis_No;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;
            Status_ID = Status_ID == null ? 0 : Status_ID;
            Make = Make == 0 ? 0 : Make;
            Model_Desc = Model_Desc == 0 ? 0 : Model_Desc;
            Model_Year = String.IsNullOrEmpty(Model_Year) ? "" : Model_Year;
            var model = _oSales.Get_SalesMasterInvoiceList_DAL(SaleRef, ManualRef, StartDate, EndDate, Customer_Name, Chassis_No, Status_ID, cID, Make, Model_Desc, Model_Year).ToList();
            report.AddDataSource("SaleDataSet", model);

            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("SaleDataSet", "");
            return result.MainStream;
        }
        [HttpGet]
        public IActionResult GetSalesReturnPrint(string reportName, string SaleRef, string StartDate, string EndDate, string Customer_Name, string c_ID)
        {
            string ReportType = "pdf";
            var returnString = GenerateSalesReturnReport(reportName, SaleRef, StartDate, EndDate, Customer_Name, c_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }
        [HttpGet]
        public IActionResult GetSalesReturnExcel(string reportName, string SaleRef, string StartDate, string EndDate, string Customer_Name, string c_ID)
        {
            string ReportType = "excel";
            var returnString = GenerateSalesReturnReport(reportName, SaleRef, StartDate, EndDate, Customer_Name, c_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".xls");


        }
        public byte[] GenerateSalesReturnReport(string reportName, string SaleRef, string StartDate, string EndDate, string Customer_Name, string c_ID, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);


            int cID = Convert.ToInt32(c_ID);
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;

            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;

            var model = _oSales.Get_SalesInvoiceReturnList_DAL(SaleRef, StartDate, EndDate, Customer_Name, cID).ToList();
            report.AddDataSource("SaleDataSet", model);

            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("SaleDataSet", "");
            return result.MainStream;
        }

        [HttpGet]
        public IActionResult GetPaymentsPrint(string reportName, string PaymentRef, int Party_ID_Name, string StartDate, string EndDate,
                   string PaidToNameText, string ChassisNo, string PurchaseRef, string VoucherType, int c_ID, string Cheque_no)
        {
            string ReportType = "pdf";
            var returnString = GeneratePaymentsReport(reportName, PaymentRef, Party_ID_Name, StartDate, EndDate, PaidToNameText, ChassisNo, PurchaseRef, VoucherType, c_ID, ReportType, Cheque_no);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }
        [HttpGet]
        public IActionResult GetPaymentsExcel(string reportName, string PaymentRef, int Party_ID_Name, string StartDate, string EndDate,
                    string PaidToNameText, string ChassisNo, string PurchaseRef, string VoucherType, int c_ID, string Cheque_no)
        {
            string ReportType = "excel";
            var returnString = GeneratePaymentsReport(reportName, PaymentRef, Party_ID_Name, StartDate, EndDate, PaidToNameText, ChassisNo, PurchaseRef, VoucherType, c_ID, ReportType, Cheque_no);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".xls");


        }
        public byte[] GeneratePaymentsReport(string reportName, string PaymentRef, int Party_ID_Name, string StartDate, string EndDate,
            string PaidToNameText, string ChassisNo, string PurchaseRef, string VoucherType, int c_ID, string ReportType, string Cheque_no)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);


            PaymentRef = String.IsNullOrEmpty(PaymentRef) ? "" : PaymentRef;
            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            PaidToNameText = String.IsNullOrEmpty(PaidToNameText) ? "" : PaidToNameText;
            VoucherType = String.IsNullOrEmpty(VoucherType) ? "" : VoucherType;

            Party_ID_Name = Party_ID_Name == 0 ? 0 : Party_ID_Name;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            c_ID = c_ID == 0 ? 1 : c_ID;

            var model = _oAccounts.Get_PaymentMasterList_DAL(PaymentRef, Party_ID_Name, StartDate, EndDate, ChassisNo, PaidToNameText, PurchaseRef, VoucherType, c_ID, Cheque_no).ToList();
            report.AddDataSource("AccountDataSet", model);

            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("AccountDataSet", "");
            return result.MainStream;
        }

        [HttpGet]
        public IActionResult GetReceiptPrint(string reportName, string ReceiptRef, int Party_ID, string PartyNameText, string SaleRef, string Chassis_no, string StartDate, string EndDate, string Cheque_no, int c_ID)
        {
            string ReportType = "pdf";
            var returnString = GenerateReceiptReport(reportName, ReceiptRef, Party_ID, PartyNameText, SaleRef, Chassis_no, StartDate, EndDate, Cheque_no, c_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }
        [HttpGet]
        public IActionResult GetReceiptExcel(string reportName, string ReceiptRef, int Party_ID, string PartyNameText, string SaleRef, string Chassis_no, string StartDate, string EndDate, string Cheque_no, int c_ID)
        {
            string ReportType = "excel";
            var returnString = GenerateReceiptReport(reportName, ReceiptRef, Party_ID, PartyNameText, SaleRef, Chassis_no, StartDate, EndDate, Cheque_no, c_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".xls");


        }



        public byte[] GenerateReceiptReport(string reportName, string ReceiptRef, int Party_ID, string PartyNameText, string SaleRef, string Chassis_no, string StartDate, string EndDate,
           string Cheque_no, int c_ID, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);



            ReceiptRef = String.IsNullOrEmpty(ReceiptRef) ? "" : ReceiptRef;
            Party_ID = Party_ID == 0 ? 0 : Party_ID;
            PartyNameText = String.IsNullOrEmpty(PartyNameText) ? "" : PartyNameText;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            Chassis_no = String.IsNullOrEmpty(Chassis_no) ? "" : Chassis_no;

            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;

            var model = _oAccounts.Get_ReceiptMaster_List_DAL(ReceiptRef, Party_ID, PartyNameText, SaleRef, Chassis_no, StartDate, EndDate, Cheque_no, c_ID).ToList();
            report.AddDataSource("AccountDataSet", model);

            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("AccountDataSet", "");
            return result.MainStream;
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

            ModelDescList = _oBasic.ModelDescList.Where(m => m.Make_ID == (Convert.ToInt32(Make_ID))).ToList();




            return Json(ModelDescList);
            //return Json(ModelDescList, new Newtonsoft.Json.JsonSerializerSettings());

        }

        //Reports Tab
        [HttpGet]
        public ActionResult SalesReport(string StartDate, string EndDate, int? Customer_ID, string Sale_Ref, int Vendor_ID, string cartype, string chassis, int? page, string Pur_Ref, int? Make, int? Model_Desc, string Model_Year)
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
            ViewBag.MakeList = _oBasic.Get_MakeModelList_DAL(c_ID).ToList();
            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Sale Report";
            ViewBag.PageTitle = "Sale Report";
            if (StartDate == null && EndDate == null)
            {
                DateTime date = DateTime.Now;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);



                StartDate = firstDayOfMonth.ToString("dd-MMM-yyyy");
                EndDate = lastDayOfMonth.ToString("dd-MMM-yyyy");
            }

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "SalesReport";
            //---getting all customers
            ViewBag.AllCustomers = _oBasic.Get_CustomersMaster_DAL(c_ID).ToList();

            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList();
            ViewBag.Make = _oBasic.Get_MakeList_DAL(c_ID).ToList();

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            Customer_ID = (Customer_ID == null ? 0 : Customer_ID);
            Sale_Ref = (String.IsNullOrEmpty(Sale_Ref) || String.IsNullOrWhiteSpace(Sale_Ref) ? "" : Sale_Ref);
            Vendor_ID = Vendor_ID == 0 ? 0 : Vendor_ID;
            cartype = (String.IsNullOrEmpty(cartype) || String.IsNullOrWhiteSpace(cartype) ? "" : cartype);
            chassis = (String.IsNullOrEmpty(chassis) || String.IsNullOrWhiteSpace(chassis) ? "" : chassis);
            Pur_Ref = (String.IsNullOrEmpty(Pur_Ref) || String.IsNullOrWhiteSpace(Pur_Ref) ? "" : Pur_Ref);
            Make = (Make == null ? 0 : Make);
            Model_Desc = (Model_Desc == null ? 0 : Model_Desc);
            Model_Year = (String.IsNullOrEmpty(Model_Year) || String.IsNullOrWhiteSpace(Model_Year) ? "" : Model_Year);


            ViewBag.StartDate_SaleReport = StartDate;
            ViewBag.EndDate_SaleReport = EndDate;
            ViewBag.Customer_ID_SaleReport = Customer_ID;
            ViewBag.Sale_Ref_SaleReport = Sale_Ref;
            ViewBag.Vendor_ID_Sale_Ref_SaleReport = Vendor_ID;
            ViewBag.cartype_SaleReport = cartype;
            ViewBag.chassis_SaleReport = chassis;
            ViewBag.PurRef = Pur_Ref;
            ViewBag.Makep = Make;
            ViewBag.ModelDesc = Model_Desc;
            ViewBag.ModelYear = Model_Year;

            _oReports.SaleReport = _oReports.pa_Select_SalesReport_DAL(StartDate, EndDate, Customer_ID, Sale_Ref, c_ID, Vendor_ID, cartype, chassis, Pur_Ref, Make, Model_Desc, Model_Year).ToPagedList(page ?? 1, 10);

            _oReports.SaleReportTTL = _oReports.pa_Select_SalesReport_TTL_DAL(StartDate, EndDate, Customer_ID, Sale_Ref, c_ID, Vendor_ID, cartype, chassis, Pur_Ref, Make, Model_Desc, Model_Year);



            return View(_oReports);
        }



        [HttpGet]
        public ActionResult SalesReport_Search(string StartDate, string EndDate, int? Customer_ID, string Sale_Ref, int Vendor_ID, string cartype, string chassis, string Pur_Ref, int? Make, int? Model_Desc, string Model_Year, int? page)
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

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Sale Report";
            ViewBag.PageTitle = "Sale Report";


            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "SalesReport";
            //---getting all customers
            ViewBag.AllCustomers = _oBasic.Get_CustomersMaster_DAL(c_ID).ToList();

            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList();


            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            Customer_ID = (Customer_ID == null ? 0 : Customer_ID);
            Sale_Ref = (String.IsNullOrEmpty(Sale_Ref) || String.IsNullOrWhiteSpace(Sale_Ref) ? "" : Sale_Ref);
            Vendor_ID = (Vendor_ID == 0 ? 0 : Vendor_ID);
            cartype = (String.IsNullOrEmpty(cartype) || String.IsNullOrWhiteSpace(cartype) ? "" : cartype);
            chassis = (String.IsNullOrEmpty(chassis) || String.IsNullOrWhiteSpace(chassis) ? "" : chassis);
            Pur_Ref = (String.IsNullOrEmpty(Pur_Ref) || String.IsNullOrWhiteSpace(Pur_Ref) ? "" : Pur_Ref);
            Make = (Make == null ? 0 : Make);
            Model_Desc = (Model_Desc == null ? 0 : Model_Desc);
            Model_Year = (String.IsNullOrEmpty(Model_Year) || String.IsNullOrWhiteSpace(Model_Year) ? "" : Model_Year);

            ViewBag.StartDate_SaleReport = StartDate;
            ViewBag.EndDate_SaleReport = EndDate;
            ViewBag.Customer_ID_SaleReport = Customer_ID;
            ViewBag.Sale_Ref_SaleReport = Sale_Ref;
            ViewBag.Vendor_ID_Sale_Ref_SaleReport = Vendor_ID;
            ViewBag.cartype_SaleReport = cartype;
            ViewBag.chassis_SaleReport = chassis;
            ViewBag.PurRef = Pur_Ref;
            ViewBag.Makep = Make;
            ViewBag.ModelDesc = Model_Desc;
            ViewBag.ModelYear = Model_Year;

            // FOR REPORTINg Purpose
            TempData["StartSale"] = StartDate;
            TempData["EndSale"] = EndDate;


            _oReports.SaleReport = _oReports.pa_Select_SalesReport_DAL(StartDate, EndDate, Customer_ID, Sale_Ref, c_ID, Vendor_ID, cartype, chassis, Pur_Ref, Make, Model_Desc, Model_Year).ToPagedList(page ?? 1, 10);

            //TempData["CusName"] = cusname[0].Customer_Name;
            _oReports.SaleReportTTL = _oReports.pa_Select_SalesReport_TTL_DAL(StartDate, EndDate, Customer_ID, Sale_Ref, c_ID, Vendor_ID, cartype, chassis, Pur_Ref, Make, Model_Desc, Model_Year);



            return PartialView("_SaleReportSearch", _oReports);
        }







        //This method is for getting Ledger list
        [HttpGet]
        public ActionResult VendorReport(string StartDate, string EndDate, int? VendorID, int? page)
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

            ViewBag.StartDate_VendorReport = StartDate;
            ViewBag.EndDate_VendorReport = EndDate;
            ViewBag.VendorID_VendorReport = VendorID;

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "VendorReport";

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Vendor Report";
            ViewBag.PageTitle = "Vendor Report";
            ViewBag.AllVendors = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList();



            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);


            _oReports.VendorLedgerMasterObj = _oReports.pa_Select_VendorReportByID_DAL(VendorID);

            _oReports.LedgerPagedList = _oReports.pa_Select_VendorLedger_DAL(StartDate, EndDate, VendorID.ToString(), c_ID).ToPagedList(page ?? 1, 10);
            _oReports.Ledger_TTL = _oReports.pa_Select_VendorLedger_DAL_TTL(StartDate, EndDate, VendorID.ToString(), c_ID).ToList();


            return View(_oReports);
        }
        [HttpGet]
        public ActionResult VendorReport_Search(string StartDate, string EndDate, int? VendorID, int? page)
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
            //if (StartDate == null && EndDate == null)
            //{
            //    DateTime date = DateTime.Now;
            //    var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            //    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            //    StartDate = firstDayOfMonth.ToString("dd-MMM-yyyy");
            //    EndDate = lastDayOfMonth.ToString("dd-MMM-yyyy");
            //}

            ViewBag.StartDate_VendorReport = StartDate;
            ViewBag.EndDate_VendorReport = EndDate;
            ViewBag.VendorID_VendorReport = VendorID;

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "VendorReport";

            //for Reporting purpuse            
            TempData["Start2"] = StartDate;
            TempData["End2"] = EndDate;
            TempData["VendorId"] = VendorID;

            ViewBag.AllVendors = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList();

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            _oReports.VendorLedgerMasterObj = _oReports.pa_Select_VendorReportByID_DAL(VendorID);

            var partyName = _oReports.LedgerPagedList = _oReports.pa_Select_VendorLedger_DAL(StartDate, EndDate, VendorID.ToString(), c_ID).ToPagedList(page ?? 1, 10);

            if (partyName.Count() > 0)
            {
                TempData["PartyName"] = partyName[0].PartyName;

            }




            _oReports.Ledger_TTL = _oReports.pa_Select_VendorLedger_DAL_TTL(StartDate, EndDate, VendorID.ToString(), c_ID).ToList();

            return PartialView("_VendorReport", _oReports);
        }
        [HttpGet]
        public IActionResult GetVendorPrint(string reportName, string StartDate, string EndDate, int? VendorID)
        {
            string ReportType = "pdf";
            var returnString = GenerateVendorReport(reportName, StartDate, EndDate, VendorID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }
        [HttpGet]
        public IActionResult GetVendorExcel(string reportName, string StartDate, string EndDate, int? VendorID)
        {
            string ReportType = "excel";
            var returnString = GenerateVendorReport(reportName, StartDate, EndDate, VendorID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".xls");


        }


        public byte[] GenerateVendorReport(string reportName, string StartDate, string EndDate, int? VendorID, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);


            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            VendorID = VendorID == null ? 0 : VendorID;

            var model = _oReports.pa_Select_VendorLedger_DAL(StartDate, EndDate, VendorID.ToString(), c_ID).ToList();
            var models = _oReports.pa_Select_VendorLedger_DAL_TTL(StartDate, EndDate, VendorID.ToString(), c_ID).ToList();
            report.AddDataSource("AccountDataSet", model);
            report.AddDataSource("AccountDataSet_TTL", models);

            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("AccountDataSet", "");
            report.AddDataSource("AccountDataSet_TTL", "");
            return result.MainStream;
        }

        #region Vendor Report By Chassis

        //This method is for getting Ledger list
        [HttpGet]
        public ActionResult VendorReport_byChassis(string StartDate, string EndDate, int VendorID)
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

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "VendorbyChassis";

            ViewBag.AllVendors = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList();

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);

            _oReports.VendorReport_byChassis = _oReports.select_VendorReport_Chassis_Wise_DAL(StartDate, EndDate, VendorID, c_ID).ToList();
            _oReports.VendorReport_byChassis_TTL = _oReports.select_VendorReport_Chassis_Wise_TTL_DAL(StartDate, EndDate, VendorID, c_ID).ToList();


            return View(_oReports);
        }

        [HttpGet]
        public ActionResult VendorReport_byChassis_Search(string StartDate, string EndDate, int VendorID)
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
            //if (StartDate == null && EndDate == null)
            //{
            //    DateTime date = DateTime.Now;
            //    var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            //    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            //    StartDate = firstDayOfMonth.ToString("dd-MMM-yyyy");
            //    EndDate = lastDayOfMonth.ToString("dd-MMM-yyyy");
            //}

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "VendorbyChassis";

            ViewBag.AllVendors = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList();

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);

            _oReports.VendorReport_byChassis = _oReports.select_VendorReport_Chassis_Wise_DAL(StartDate, EndDate, VendorID, c_ID).ToList();
            _oReports.VendorReport_byChassis_TTL = _oReports.select_VendorReport_Chassis_Wise_TTL_DAL(StartDate, EndDate, VendorID, c_ID).ToList();

            return PartialView("_VendorReport_byChassis", _oReports);
        }


        [HttpGet]
        public IActionResult VendorReport_byChassis_Print(string StartDate, string EndDate, int VendorID)
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

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            VendorID = VendorID == 0 ? 0 : VendorID;
            _oReports.VendorReport_byChassis = _oReports.select_VendorReport_Chassis_Wise_DAL(StartDate, EndDate, VendorID, c_ID).ToList();
            _oReports.VendorReport_byChassis_TTL = _oReports.select_VendorReport_Chassis_Wise_TTL_DAL(StartDate, EndDate, VendorID, c_ID).ToList();

            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
  "--footer-left \"  Printed At: " +
DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
" --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };
            return demoViewPortrait;

        }

        #endregion

        //By Jahangir 17 Jan 2021 10:06 am
        #region ITEM CARD REPORTS / INVENTORY STOCK REPORT
        public ActionResult InventoryReportItemCard(string ItemCode, string ItemSerialNO,
         string StartDate, string EndDate, int Loc_ID)
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
            Loc_ID = 0;
            ItemSerialNO = "";
            ItemCode = string.IsNullOrEmpty(ItemCode) ? "" : ItemCode;

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);


            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "InventoryReportItemCard";
            ViewBag.BreadCumTitle = "Inventory Report";

            ViewBag.AllItems = _oBasic.Get_ItemMasterList_DAL().ToList();




            _oReports.ItemCardReportList = _oReports.Select_ItemCard_Report_DAL(ItemCode, ItemSerialNO, StartDate, EndDate, Loc_ID, c_ID).ToList();
            _oReports.ItemCardReportList_TTL = _oReports.Select_ItemCard_Report_TTL_DAL(ItemCode, ItemSerialNO, StartDate, EndDate, Loc_ID, c_ID).ToList();


            return View(_oReports);
        }

        public ActionResult InventoryReportItemCardParts(string ItemCode, int Item_ID, string traditional, string Make, string Fuel, string Transmission, string Drive,
            int ItemGroup_ID, int ItemCategory_ID, string Year, string ItemSerialNO, string StartDate, string EndDate, int Loc_ID, string EngineSpecsCode_ID, int? page)
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

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "InventoryReportItemCard";
            ViewBag.BreadCumTitle = "Inventory Report";
            ViewBag.PageTitle = "Inventory Report";

            ViewBag.AllItems = _oBasic.Get_ItemMasterList_DAL().ToList();
            ViewBag.ItemGroupList = _oBasic.Get_ItemGroup_DAL(c_ID).ToList();
            ViewBag.ItemCategoryList = _oBasic.Get_ItemCategory_DAL(c_ID).ToList();
            ViewBag.MakeList = _oBasic.Get_MakeModelList_DAL(c_ID).ToList();
            ViewBag.EngineSpecsCodeList = _oBasic.Get_EngineSpecsCodeList_DAL(c_ID).ToList();
            ItemCode = string.IsNullOrEmpty(ItemCode) ? "" : ItemCode;

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);

            Item_ID = Item_ID == 0 ? 0 : Item_ID;

            traditional = string.IsNullOrEmpty(traditional) ? "" : traditional;
            Make = string.IsNullOrEmpty(Make) ? "" : Make;
            Fuel = string.IsNullOrEmpty(Fuel) ? "" : Fuel;
            Transmission = string.IsNullOrEmpty(Transmission) ? "" : Transmission;
            Drive = string.IsNullOrEmpty(Drive) ? "" : Drive;

            ItemSerialNO = string.IsNullOrEmpty(ItemSerialNO) ? "" : ItemSerialNO;
            ItemGroup_ID = ItemGroup_ID == 0 ? 0 : ItemGroup_ID;
            ItemCategory_ID = ItemCategory_ID == 0 ? 0 : ItemCategory_ID;
            Loc_ID = Loc_ID == 0 ? 0 : Loc_ID;
            EngineSpecsCode_ID = string.IsNullOrEmpty(EngineSpecsCode_ID) ? "" : EngineSpecsCode_ID;


            Year = string.IsNullOrEmpty(Year) ? "" : Year;

            ViewBag.ItemCode = ItemCode;
            ViewBag.Item_ID = Item_ID;
            ViewBag.traditional = traditional;
            ViewBag.Make = Make;
            ViewBag.Fuel = Fuel;
            ViewBag.Transmission = Transmission;
            ViewBag.Drive = Drive;
            ViewBag.ItemGroup_ID = ItemGroup_ID;
            ViewBag.ItemCategory_ID = ItemCategory_ID;
            ViewBag.Year = Year;
            ViewBag.ItemSerialNO = ItemSerialNO;
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = StartDate;
            ViewBag.Loc_ID = Loc_ID;
            ViewBag.EngineSpecsCode_ID = EngineSpecsCode_ID;


            _oReports.ItemCardReportListParts = _oReports.Select_ItemCard_Report_Parts_DAL(ItemCode, Item_ID, traditional, Make, Fuel, Transmission, Drive, ItemGroup_ID, ItemCategory_ID, Year, ItemSerialNO,
            StartDate, EndDate, Loc_ID, EngineSpecsCode_ID, c_ID).ToPagedList(page ?? 1, 10);
            _oReports.ItemCardReportList_TTL = _oReports.Select_ItemCard_Report_parts_TTL_DAL(ItemCode, Item_ID, traditional, Make, Fuel, Transmission, Drive, ItemGroup_ID, ItemCategory_ID, Year, ItemSerialNO,
            StartDate, EndDate, Loc_ID, EngineSpecsCode_ID, c_ID).ToList();


            return View(_oReports);
        }


        public ActionResult InventoryReportItemCardParts_Search(string ItemCode, int Item_ID, string traditional, string Make, string Fuel, string Transmission, string Drive,
           int ItemGroup_ID, int ItemCategory_ID, string Year, string ItemSerialNO, string StartDate, string EndDate, int Loc_ID, string EngineSpecsCode_ID, int? page)
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



            ItemCode = string.IsNullOrEmpty(ItemCode) ? "" : ItemCode;

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);

            Item_ID = Item_ID == 0 ? 0 : Item_ID;

            //     ItemName = ItemName == "0" ? "" : ItemName.Split(' ')[1];
            //      ItemName = string.IsNullOrEmpty(ItemName) ? "" : ItemName.Split(' ')[1];
            traditional = string.IsNullOrEmpty(traditional) ? "" : traditional;
            Make = string.IsNullOrEmpty(Make) ? "" : Make;
            Fuel = string.IsNullOrEmpty(Fuel) ? "" : Fuel;
            Transmission = string.IsNullOrEmpty(Transmission) ? "" : Transmission;
            Drive = string.IsNullOrEmpty(Drive) ? "" : Drive;

            ItemSerialNO = string.IsNullOrEmpty(ItemSerialNO) ? "" : ItemSerialNO;
            ItemGroup_ID = ItemGroup_ID == 0 ? 0 : ItemGroup_ID;
            ItemCategory_ID = ItemCategory_ID == 0 ? 0 : ItemCategory_ID;
            Loc_ID = Loc_ID == 0 ? 0 : Loc_ID;
            EngineSpecsCode_ID = string.IsNullOrEmpty(EngineSpecsCode_ID) ? "" : EngineSpecsCode_ID;
            Year = string.IsNullOrEmpty(Year) ? "" : Year;




            ViewBag.ItemCode = ItemCode;
            ViewBag.Item_ID = Item_ID;
            ViewBag.traditional = traditional;
            ViewBag.Make = Make;
            ViewBag.Fuel = Fuel;
            ViewBag.Transmission = Transmission;
            ViewBag.Drive = Drive;
            ViewBag.ItemGroup_ID = ItemGroup_ID;
            ViewBag.ItemCategory_ID = ItemCategory_ID;
            ViewBag.Year = Year;
            ViewBag.ItemSerialNO = ItemSerialNO;
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = StartDate;
            ViewBag.Loc_ID = Loc_ID;
            ViewBag.EngineSpecsCode_ID = EngineSpecsCode_ID;
            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "InventoryReportItemCard";
            ViewBag.BreadCumTitle = "Inventory Report";

            ViewBag.AllItems = _oBasic.Get_ItemMasterList_DAL().ToList();
            ViewBag.ItemGroupList = _oBasic.Get_ItemGroup_DAL(c_ID).ToList();
            ViewBag.ItemCategoryList = _oBasic.Get_ItemCategory_DAL(c_ID).ToList();
            ViewBag.MakeList = _oBasic.Get_MakeModelList_DAL(c_ID).ToList();
            ViewBag.EngineSpecsCodeList = _oBasic.Get_EngineSpecsCodeList_DAL(c_ID).ToList();


            _oReports.ItemCardReportListParts = _oReports.Select_ItemCard_Report_Parts_DAL(ItemCode, Item_ID, traditional, Make, Fuel, Transmission, Drive, ItemGroup_ID, ItemCategory_ID, Year, ItemSerialNO,
            StartDate, EndDate, Loc_ID, EngineSpecsCode_ID, c_ID).ToPagedList(page ?? 1, 10);
            _oReports.ItemCardReportList_TTL = _oReports.Select_ItemCard_Report_parts_TTL_DAL(ItemCode, Item_ID, traditional, Make, Fuel, Transmission, Drive, ItemGroup_ID, ItemCategory_ID, Year, ItemSerialNO,
            StartDate, EndDate, Loc_ID, EngineSpecsCode_ID, c_ID).ToList();


            return PartialView("_InventoryReportItemCardParts", _oReports);
        }



        #endregion

        [HttpGet]
        public ActionResult PayableSummary()
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

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "PayableSummary";

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Payable Summary";
            ViewBag.PageTitle = "Payable Summary";


            _oReports.payableSummary = _oReports.pa_Select_Payable_Summary_DAL(c_ID).ToList();


            return View(_oReports);
        }
        [HttpGet]
        public IActionResult GetPayableSummaryPrint(string reportName)
        {
            string ReportType = "pdf";
            var returnString = GeneratePayableSummaryReport(reportName, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }
        [HttpGet]
        public IActionResult GetPayableSummaryExcel(string reportName)
        {
            string ReportType = "excel";
            var returnString = GeneratePayableSummaryReport(reportName, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".xls");


        }
        public byte[] GeneratePayableSummaryReport(string reportName, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);



            var model = _oReports.pa_Select_Payable_Summary_DAL(c_ID).ToList();
            report.AddDataSource("AccountDataSet", model);

            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("AccountDataSet", "");
            return result.MainStream;
        }
        [HttpGet]
        public ActionResult CustomerReport(string StartDate, string EndDate, int? Customer_ID, int? page)
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


            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            Customer_ID = Customer_ID == null ? 0 : Customer_ID;


            ViewBag.StartDate_CustomerReport = StartDate;
            ViewBag.EndDate_CustomerReport = EndDate;
            ViewBag.Customer_ID_CustomerReport = Customer_ID;

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "CustomerReport";

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Customer Report";
            ViewBag.PageTitle = "Customer Report";

            ViewBag.IsVehicleAppHidden = configuration.GetSection("AppSettings").GetSection("IsVehicleAppHidden").Value;


            //---getting all customers
            ViewBag.AllCustomers = _oBasic.Get_CustomersMaster_DAL(c_ID).ToList();



            _oReports.CustomerLedgerMasterObj = _oReports.pa_Select_CustomerReportByID_DAL(Customer_ID);

            _oReports.CustomerLedger = _oReports.pa_Select_CustomerLedger_DAL(StartDate, EndDate, Customer_ID.ToString(), c_ID).ToPagedList(page ?? 1, 10);

            _oReports.CustomerLedger_TTL = _oReports.pa_Select_CustomerLedger_DAL_TTL(StartDate, EndDate, Customer_ID.ToString(), c_ID).ToList();
            return View(_oReports);

        }

        [HttpGet]
        public ActionResult CustomerReport_Search(string StartDate, string EndDate, int? Customer_ID, int? page)
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

            //if (StartDate == null && EndDate == null)
            //{
            //    DateTime date = DateTime.Now;
            //    var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            //    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            //    StartDate = firstDayOfMonth.ToString("dd-MMM-yyyy");
            //    EndDate = lastDayOfMonth.ToString("dd-MMM-yyyy");
            //}
            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            Customer_ID = Customer_ID == null ? 0 : Customer_ID;

            ViewBag.StartDate_CustomerReport = StartDate;
            ViewBag.EndDate_CustomerReport = EndDate;
            ViewBag.Customer_ID_CustomerReport = Customer_ID;


            //Use for Printing Report
            TempData["StartCus"] = StartDate;
            TempData["EndCus"] = EndDate;
            TempData["CustomerID"] = Customer_ID;




            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "CustomerReport";

            ViewBag.IsVehicleAppHidden = configuration.GetSection("AppSettings").GetSection("IsVehicleAppHidden").Value;



            //---getting all customers
            ViewBag.AllCustomers = _oBasic.Get_CustomersMaster_DAL(c_ID).ToList();
            _oReports.CustomerLedgerMasterObj = _oReports.pa_Select_CustomerReportByID_DAL(Customer_ID);


            var partyname = _oReports.CustomerLedger = _oReports.pa_Select_CustomerLedger_DAL(StartDate, EndDate, Customer_ID.ToString(), c_ID).ToPagedList(page ?? 1, 10);
            _oReports.CustomerLedger_TTL = _oReports.pa_Select_CustomerLedger_DAL_TTL(StartDate, EndDate, Customer_ID.ToString(), c_ID).ToList();

            if (partyname.Count > 0)
            {
                TempData["PartyName"] = partyname[0].PartyName;

            }


            // ViewBag.partyName = partyname[0].PartyName;
            return PartialView("_CustomerReport", _oReports);
        }

        [HttpGet]
        public IActionResult GetCustomerPrint(string reportName, string StartDate, string EndDate, int? Customer_ID)
        {
            string ReportType = "pdf";
            var returnString = GenerateCustomerReport(reportName, StartDate, EndDate, Customer_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }
        [HttpGet]
        public IActionResult GetCustomerExcel(string reportName, string StartDate, string EndDate, int? Customer_ID)
        {
            string ReportType = "excel";
            var returnString = GenerateCustomerReport(reportName, StartDate, EndDate, Customer_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".xls");


        }
        public byte[] GenerateCustomerReport(string reportName, string StartDate, string EndDate, int? Customer_ID, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);


            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);


            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            Customer_ID = Customer_ID == null ? 0 : Customer_ID;

            var model = _oReports.pa_Select_CustomerLedger_DAL(StartDate, EndDate, Customer_ID.ToString(), c_ID).ToList();
            var models = _oReports.pa_Select_CustomerLedger_DAL_TTL(StartDate, EndDate, Customer_ID.ToString(), c_ID).ToList();
            report.AddDataSource("AccountDataSet", model);
            report.AddDataSource("AccountDataSet_TTL", models);

            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("AccountDataSet", "");
            report.AddDataSource("AccountDataSet_TTL", "");
            return result.MainStream;

        }



        [HttpGet]
        public ActionResult RecievableSummary()
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

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Receivable Summary";
            ViewBag.PageTitle = "Recievable Summary";





            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "RecievableSummary";
            _oReports.ReceiveAbleSummary = _oReports.pa_Select_Recievable_Summary_DAL(c_ID).ToList();


            return View(_oReports);
        }
        [HttpGet]
        public IActionResult GetReceivableSummaryPrint(string reportName)
        {
            string ReportType = "pdf";
            var returnString = GenerateReceivableSummaryReport(reportName, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }
        [HttpGet]
        public IActionResult GetReceivableSummaryExcel(string reportName)
        {
            string ReportType = "excel";
            var returnString = GenerateReceivableSummaryReport(reportName, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".xls");


        }
        public byte[] GenerateReceivableSummaryReport(string reportName, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);



            var model = _oReports.pa_Select_Recievable_Summary_DAL(c_ID).ToList();
            report.AddDataSource("AccountDataSet", model);

            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("AccountDataSet", "");
            return result.MainStream;
        }


        [HttpGet]
        public ActionResult StockReport(string StartPurchaseDate, string EndPurchaseDate, string Chassis_No, string PurchaseRef, int? Make_ID, int? MakeModel_description_ID, int? Color_ID)
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

            if (StartPurchaseDate == null && EndPurchaseDate == null)
            {
                DateTime date = DateTime.Now;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                StartPurchaseDate = firstDayOfMonth.ToString("dd-MMM-yyyy");
                EndPurchaseDate = lastDayOfMonth.ToString("dd-MMM-yyyy");
            }

            ViewBag.MakeList = _oBasic.Get_MakeList_DAL(c_ID).ToList();
            ViewBag.ModelList = _oBasic.Get_ModelList_DAL(c_ID);
            ViewBag.ColorList = _oBasic.Get_ColorList_DAL(c_ID);

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "StockReport";

            TempData["StartPurchaseDate"] = StartPurchaseDate;
            TempData["EndPurchaseDate"] = EndPurchaseDate;



            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Stock Repor";
            ViewBag.PageTitle = "Stock Report";

            StartPurchaseDate = (String.IsNullOrEmpty(StartPurchaseDate) || String.IsNullOrWhiteSpace(StartPurchaseDate) ? "" : StartPurchaseDate);
            EndPurchaseDate = (String.IsNullOrEmpty(EndPurchaseDate) || String.IsNullOrWhiteSpace(EndPurchaseDate) ? "" : EndPurchaseDate);


            Chassis_No = (String.IsNullOrEmpty(Chassis_No) || String.IsNullOrWhiteSpace(Chassis_No) ? "" : Chassis_No);
            PurchaseRef = (String.IsNullOrEmpty(PurchaseRef) || String.IsNullOrWhiteSpace(PurchaseRef) ? "" : PurchaseRef);
            Make_ID = (Make_ID == null ? 0 : Make_ID);
            MakeModel_description_ID = (MakeModel_description_ID == null ? 0 : MakeModel_description_ID);
            Color_ID = (Color_ID == null ? 0 : Color_ID);


            _oReports.StockReportList = _oReports.pa_Select_StockReport_DAL(StartPurchaseDate, EndPurchaseDate, Chassis_No, PurchaseRef, Make_ID, MakeModel_description_ID, Color_ID, c_ID).ToList();





            return View(_oReports);
        }


        [HttpGet]
        public IActionResult GetStockReportPrint(string reportName, string StartPurchaseDate, string EndPurchaseDate, string Chassis_No, string PurchaseRef, int? Make_ID, int? MakeModel_description_ID, int? Color_ID)
        {
            string ReportType = "pdf";
            var returnString = GenerateStockReportReport(reportName, StartPurchaseDate, EndPurchaseDate, Chassis_No, PurchaseRef, Make_ID, MakeModel_description_ID, Color_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }
        [HttpGet]
        public IActionResult GetStockReportExcel(string reportName, string StartPurchaseDate, string EndPurchaseDate, string Chassis_No, string PurchaseRef, int? Make_ID, int? MakeModel_description_ID, int? Color_ID)
        {
            string ReportType = "excel";
            var returnString = GenerateStockReportReport(reportName, StartPurchaseDate, EndPurchaseDate, Chassis_No, PurchaseRef, Make_ID, MakeModel_description_ID, Color_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".xls");


        }
        public byte[] GenerateStockReportReport(string reportName, string StartPurchaseDate, string EndPurchaseDate, string Chassis_No, string PurchaseRef, int? Make_ID, int? MakeModel_description_ID, int? Color_ID, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);


            StartPurchaseDate = (String.IsNullOrEmpty(StartPurchaseDate) || String.IsNullOrWhiteSpace(StartPurchaseDate) ? "" : StartPurchaseDate);
            EndPurchaseDate = (String.IsNullOrEmpty(EndPurchaseDate) || String.IsNullOrWhiteSpace(EndPurchaseDate) ? "" : EndPurchaseDate);


            Chassis_No = (String.IsNullOrEmpty(Chassis_No) || String.IsNullOrWhiteSpace(Chassis_No) ? "" : Chassis_No);
            PurchaseRef = (String.IsNullOrEmpty(PurchaseRef) || String.IsNullOrWhiteSpace(PurchaseRef) ? "" : PurchaseRef);
            Make_ID = (Make_ID == null ? 0 : Make_ID);
            MakeModel_description_ID = (MakeModel_description_ID == null ? 0 : MakeModel_description_ID);
            Color_ID = (Color_ID == null ? 0 : Color_ID);
            var model = _oReports.pa_Select_StockReport_DAL(StartPurchaseDate, EndPurchaseDate, Chassis_No, PurchaseRef, Make_ID, MakeModel_description_ID, Color_ID, c_ID).ToList();
            report.AddDataSource("StockDataSet", model);

            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("StockDataSet", "");
            return result.MainStream;
        }




        [HttpGet]
        public IActionResult GetSaleInvoicePrintById(string reportName, int? SaleMaster_ID)
        {
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            if (c_ID == 2)
            {
                reportName = "SalesInvoicePrint_HC";
            }
            string ReportType = "pdf";
            var returnString = GenerateSalesInvoicePrint_RDLC(reportName, SaleMaster_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }

        [HttpGet]
        public IActionResult GetSaleInvoiceAgreementPrint_ById(string reportName, int? SaleMaster_ID)
        {
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            if (c_ID == 2)
            {
                reportName = "SalesInvoicePrintAgreement_HC";
            }
            string ReportType = "pdf";
            var returnString = GenerateSalesInvoicePrint_RDLC(reportName, SaleMaster_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }

        public byte[] GenerateSalesInvoicePrint_RDLC(string reportName, int? SaleMaster_ID, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);





            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            var model = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID);
            var models = _oSales.Get_SalesInvoiceDetailPrintByID_DAL(SaleMaster_ID).ToList();

            report.AddDataSource("dsPurchaseInvoicePrintMaster", model);

            report.AddDataSource("dsPurchaseInvoicePrintDtl", models);
            var result = report.Execute(GetRenderType(ReportType), 1, parameters);

            report.AddDataSource("dsPurchaseInvoicePrintMaster", "");

            report.AddDataSource("dsPurchaseInvoicePrintDtl", "");
            return result.MainStream;
        }
        [HttpGet]
        public IActionResult GetPerformaInvoicePrintById(string reportName, int? SaleMaster_ID)
        {
            string ReportType = "pdf";
            var returnString = GeneratePerformaInvoiceReport(reportName, SaleMaster_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }

        public byte[] GeneratePerformaInvoiceReport(string reportName, int? SaleMaster_ID, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);





            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);

            var model = _oSales.GetDataForPrintDtl(SaleMaster_ID);
            DataTable dtPerforma = new DataTable();
            NumericWordsConverter converter = new NumericWordsConverter();
            dtPerforma = model.Tables[0];
            string values = "";
            foreach (DataRow dr in dtPerforma.Rows)
            {
                values = dr["Total_Amount"].ToString();

            }
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(values.Split(".")[1].ToString());

            dtPerforma.Rows[0]["Total_Amt_inWords"] =
            converter.ToWords(Convert.ToInt64(values.Split('.')[0])) + " AED "
            + (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(values.Split(".")[1]))
            + " fils only" : " only");

            report.AddDataSource("dsPerformaInvoicePrintMasterDtl", dtPerforma);


            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("dsPerformaInvoicePrintMasterDtl", "");
            return result.MainStream;
        }

        [HttpGet]
        public IActionResult GetPaymentMasterPrintById(string reportName, int? PaymentMaster_ID)
        {

            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            if (c_ID == 2)
            {
                reportName = "PV_GL_NewCompany";
            }
            string ReportType = "pdf";
            var returnString = GeneratePaymentMasterReport(reportName, PaymentMaster_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }

        public byte[] GeneratePaymentMasterReport(string reportName, int? PaymentMaster_ID, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);





            PaymentMaster_ID = (PaymentMaster_ID == null ? 0 : PaymentMaster_ID);

            var model = _oAccounts.GetDataForPrintPaymentMaster(PaymentMaster_ID);
            DataTable dtPayment = new DataTable();
            dtPayment = model.Tables[0];

            report.AddDataSource("dsPrintPaymentMaster", dtPayment);


            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("dsPrintPaymentMaster", "");
            return result.MainStream;
        }

        [HttpGet]
        public IActionResult GetReceiptMasterPrintById(string reportName, int? ReceiptMaster_ID)
        {
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            if (c_ID == 2)
            {
                reportName = "RV_GL_NewCompany";
            }
            string ReportType = "pdf";
            var returnString = GenerateReceiptMasterReport(reportName, ReceiptMaster_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }

        public byte[] GenerateReceiptMasterReport(string reportName, int? ReceiptMaster_ID, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);





            ReceiptMaster_ID = (ReceiptMaster_ID == null ? 0 : ReceiptMaster_ID);

            var model = _oAccounts.GetDataForPrintReceiptMaster(ReceiptMaster_ID);
            DataTable dtReceipt = new DataTable();
            dtReceipt = model.Tables[0];

            report.AddDataSource("dsPrintReceiptMaster", dtReceipt);


            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("dsPrintReceiptMaster", "");

            return result.MainStream;
        }

        [HttpGet]
        public IActionResult GetVehicleDisplayPrintById(string reportName, int? vehicle_display_master_ID)
        {


            var ReportCode = configuration.GetSection("AppSettings").GetSection("CompanyReportFolderCode").Value;
            if(ReportCode =="AM")
            {
                reportName = "vehDisplayShowroomPrint_AM";
            }
            else if(ReportCode == "GL")
            {
                string company_ID = HttpContext.Session.GetString("c_ID");
                int? c_ID = Convert.ToInt32(company_ID);
                if (c_ID == 2)
                {
                    reportName = "vehDisplayShowroomPrint_GL2";
                }
                else
                {
                    reportName = "vehDisplayShowroomPrint_GL";

                }

            }
            string ReportType = "pdf";

            var returnString = GenerateVehicleDisplayReport(reportName, vehicle_display_master_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }

        public byte[] GenerateVehicleDisplayReport(string reportName, int? vehicle_display_master_ID, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);





            vehicle_display_master_ID = (vehicle_display_master_ID == null ? 0 : vehicle_display_master_ID);

            var model = _oSales.select_vehicle_display_Print_By_ID(vehicle_display_master_ID);
            DataTable dtReceipt = new DataTable();
            dtReceipt = model.Tables[0];

            report.AddDataSource("dsPrintvehicledisplaymaster", dtReceipt);


            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("dsPrintvehicledisplaymaster", "");
            return result.MainStream;
        }

        #region Financial Statements

        [HttpGet]
        public IActionResult GetincomestatementPrint(string reportName, string From_Date, string To_Date)
        {
            string ReportType = "pdf";
            var returnString = GenerateincomestatementReport(reportName, From_Date, To_Date, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }

        public byte[] GenerateincomestatementReport(string reportName, string From_Date, string To_Date, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);
            parameters.Add("sd", From_Date);
            parameters.Add("ed", To_Date);




            From_Date = (String.IsNullOrEmpty(From_Date) || String.IsNullOrWhiteSpace(From_Date) ? "" : From_Date);
            To_Date = (String.IsNullOrEmpty(To_Date) || String.IsNullOrWhiteSpace(To_Date) ? "" : To_Date);


            var model_INC = _oAccounts.IncomeStatment_INC_DAL(From_Date, To_Date, 1).ToList();
            var model_EXP = _oAccounts.IncomeStatment_Exp_DAL(From_Date, To_Date, 1).ToList();
            var model_CGS = _oAccounts.IncomeStatment_CGS_DAL(From_Date, To_Date, 1).ToList();
            var model_Exp_Fin = _oAccounts.IncomeStatment_Exp_Fin_DAL(From_Date, To_Date, 1).ToList();
            var model_DisRet = _oAccounts.IncomeStatment_disRet(From_Date, To_Date, 1).ToList();
            var model_IncomeStatement = _oAccounts.IncomeStateMent_DAL(From_Date, To_Date, 1).ToList();



            report.AddDataSource("dsIncomeStatment_INC", model_INC);
            report.AddDataSource("dsIncomeStatment_Exp", model_EXP);
            report.AddDataSource("dsIncomeStatment_CGS", model_CGS);
            report.AddDataSource("dsIncomeStatment_Exp_Fin", model_Exp_Fin);
            report.AddDataSource("dsIncomeStatment_disRet", model_DisRet);
            report.AddDataSource("dsIncomeStatment", model_IncomeStatement);

            var resultd = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("dsIncomeStatment_INC", "");
            report.AddDataSource("dsIncomeStatment_Exp", "");
            report.AddDataSource("dsIncomeStatment_CGS", "");
            report.AddDataSource("dsIncomeStatment_Exp_Fin", "");
            report.AddDataSource("dsIncomeStatment_disRet", "");
            report.AddDataSource("dsIncomeStatment", "");
            return resultd.MainStream;

        }



        [HttpGet]
        public IActionResult GetBalanceSheetPrint(string reportName, string From_Date, string To_Date)
        {
            string ReportType = "pdf";
            var returnString = GenerateBalanceSheetReport(reportName, From_Date, To_Date, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);


        }

        public byte[] GenerateBalanceSheetReport(string reportName, string From_Date, string To_Date, string ReportType)
        {

            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);
            parameters.Add("sd", From_Date);
            parameters.Add("ed", To_Date);




            From_Date = (String.IsNullOrEmpty(From_Date) || String.IsNullOrWhiteSpace(From_Date) ? "" : From_Date);
            To_Date = (String.IsNullOrEmpty(To_Date) || String.IsNullOrWhiteSpace(To_Date) ? "" : To_Date);



            var model_BS = _oAccounts.Balance_Sheet_DAL(From_Date, To_Date, 1).ToList();
            var model_currAsset = _oAccounts.BalSh_byCurrentAssets_DAL(From_Date, To_Date, 1).ToList();
            var model_currLaib = _oAccounts.BalSh_byCurrentLaibilities_DAL(From_Date, To_Date, 1).ToList();
            var model_fixasset = _oAccounts.BalSh_byFixedAssets_DAL(From_Date, To_Date, 1).ToList();
            var model_lngLaib = _oAccounts.BalSh_byLongTermLaibilities_DAL(From_Date, To_Date, 1).ToList();
            var model_Drawing = _oAccounts.BalSh_byDrawings_DAL(From_Date, To_Date, 1).ToList();
            var model_Capital = _oAccounts.BalSh_byCapitalInvestments_DAL(From_Date, To_Date, 1).ToList();
            var model_netIncome = _oAccounts.Get_NetIncome_onPrint_DAL(From_Date, To_Date, 1).ToList();

            report.AddDataSource("finalization_Balance_Sheet", model_BS);
            report.AddDataSource("finalization_BalSh_byCurrentAssets", model_currAsset);
            report.AddDataSource("finalization_BalSh_byCurrentLaibilities", model_currLaib);
            report.AddDataSource("finalization_BalSh_byFixedAssets", model_fixasset);
            report.AddDataSource("finalization_BalSh_byLongTermLaibilities", model_lngLaib);
            report.AddDataSource("finalization_BalSh_byDrawings", model_Drawing);
            report.AddDataSource("finalization_BalSh_byCapitalInvestments", model_Capital);
            report.AddDataSource("finalization_Get_NetIncome_onPrint", model_netIncome);





            var results = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("finalization_Balance_Sheet", "");
            report.AddDataSource("finalization_BalSh_byCurrentAssets", "");
            report.AddDataSource("finalization_BalSh_byCurrentLaibilities", "");
            report.AddDataSource("finalization_BalSh_byFixedAssets", "");
            report.AddDataSource("finalization_BalSh_byLongTermLaibilities", "");
            report.AddDataSource("finalization_BalSh_byDrawings", "");
            report.AddDataSource("finalization_BalSh_byCapitalInvestments", "");
            report.AddDataSource("finalization_Get_NetIncome_onPrint", "");
            return results.MainStream;
        }

        #endregion



        public Dictionary<string, string> Parameters = new Dictionary<string, string>();
        [HttpGet]
        public IActionResult GetDeliveryPrintById(string reportName, int? DeliveryMaster_ID)
        {


            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";
            string ReportType = "pdf";
            string mimtype = "";
            int extension = 1;
            LocalReport localReport = new LocalReport(rdlcFilePath);
            var model = _oSales.select_Delivery_Print_By_ID(DeliveryMaster_ID);
            DataTable dtReceipt = new DataTable();
            dtReceipt = model.Tables[0];
            localReport.AddDataSource("dsPapers_SoldNotDelevered_Print", dtReceipt);
            if (Parameters != null && Parameters.Count > 0)// if you use parameter in report
            {
                List<ReportParameter> reportparameter = new List<ReportParameter>();
                foreach (var record in Parameters)
                {
                    reportparameter.Add(new ReportParameter());
                }
            }
            var result = localReport.Execute(RenderType.Pdf, extension, parameters:
             Parameters, mimtype);

            byte[] bytes = result.MainStream;

            return File(bytes, "application/pdf");



        }
        [HttpGet]
        public IActionResult GetDepositPrintById(int? DV_ID)
        {
            string mimtype = "";
            int extension = 1;
            var _reportPath = "";
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            if (c_ID == 1)
            {
                _reportPath = @"RdlcReports\dv_new_GL.rdlc";
            }
            else if (c_ID == 2)
            {
                _reportPath = @"RdlcReports\dv_new_GL_NewCompany.rdlc";
            }
            var localReport = new LocalReport(_reportPath);



            var model = _oDelivery.GetDataForPrintDtl(DV_ID);
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();
            dt = model.Tables[0];
            localReport.AddDataSource("dsprint_get_dvDetails_print", dt);



            var reportParams = new Dictionary<string, string>();

            if (reportParams != null && reportParams.Count > 0)// if you use parameter in report
            {
                List<ReportParameter> reportparameter = new List<ReportParameter>();
                foreach (var record in reportParams)
                {
                    reportparameter.Add(new ReportParameter());
                }

            }



            var result = localReport.Execute(RenderType.Pdf, extension, parameters: reportParams, mimtype);
            byte[] file = result.MainStream;

            Stream stream = new MemoryStream(file);
            return File(stream, "application/pdf");



        }
        [HttpGet]
        public IActionResult GetDepositClaimPrintById(int? DV_IDs)
        {
            string mimtype = "";
            int extension = 1;
            var _reportPath = "";
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            if (c_ID == 1)
            {
                _reportPath = @"RdlcReports\dvclaim_GL.rdlc";
            }
            else if (c_ID == 2)
            {
                _reportPath = @"RdlcReports\dvclaim_GL_NewCompany.rdlc";
            }
            var localReport = new LocalReport(_reportPath);



            var model = _oDelivery.GetDataForPrintDtl_Claim(DV_IDs);
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();
            dt = model.Tables[0];
            localReport.AddDataSource("dsprint_get_dvDetails", dt);



            var reportParams = new Dictionary<string, string>();

            if (reportParams != null && reportParams.Count > 0)// if you use parameter in report
            {
                List<ReportParameter> reportparameter = new List<ReportParameter>();
                foreach (var record in reportParams)
                {
                    reportparameter.Add(new ReportParameter());
                }

            }



            var result = localReport.Execute(RenderType.Pdf, extension, parameters: reportParams, mimtype);
            byte[] file = result.MainStream;

            Stream stream = new MemoryStream(file);
            return File(stream, "application/pdf");



        }
        [HttpGet]
        public IActionResult GetPurchaseReporPrintById(string reportName, int? PurchaseMaster_ID)
        {
            string ReportType = "pdf";
            var returnString = GeneratePurchaseReport_Print(reportName, PurchaseMaster_ID, ReportType);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Pdf);

        }

        public byte[] GeneratePurchaseReport_Print(string reportName, int? PurchaseMaster_ID, string ReportType)
        {
            string rdlcFilePath = @"RdlcReports\" + reportName + ".rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);
            PurchaseMaster_ID = (PurchaseMaster_ID == null ? 0 : PurchaseMaster_ID);

            var model1 = _oReports.GetDataForPurchasePrintMaster(PurchaseMaster_ID).ToList();
            var model2 = _oReports.GetDataForPurchasePrintDetail(PurchaseMaster_ID).ToList();

            report.AddDataSource("DataSetPurchaseMaster", model1);
            report.AddDataSource("DataSetPurchaseDetail", model2);



            var result = report.Execute(GetRenderType(ReportType), 1, parameters);
            report.AddDataSource("DataSetPurchaseMaster", "");
            report.AddDataSource("DataSetPurchaseDetail", "");
            return result.MainStream;
        }


        [HttpGet]
        public IActionResult SalesInvoice_Print(int? SaleMaster_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");

            int? c_ID = Convert.ToInt32(company_ID);
            ViewBag.ReportCompanyPur_F = c_ID;
            #endregion
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;
            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.SalesMasterPrintObj = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID);
            _oSales.SalesDetailPrintObj = _oSales.Get_SalesInvoiceDetailPrintByID_DAL(SaleMaster_ID).ToList();
            _oSales.SalesDetailPrintObj_TTL = _oSales.Get_SalesInvoiceDetailPrintByID_ttl(SaleMaster_ID);
            _oSales.SalesReceiptDetailsList_Print = _oSales.Get_ReceiptDetailSaleListByID_DAL_Invoice_Print(SaleMaster_ID);
            string Host = Request.Host.ToString();

            string TotalAmount = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID).AfterVatTotal;
            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0";
            }
            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());


            _oSales.SalesDetailPrintObj_TTL.Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.SalesMasterPrintObj.Currency_ShortName
                + (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.SalesMasterPrintObj.Minor_ShortName + " only" : " only");



            string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/PrintHeaderInvoiceSalesInv?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID,

            "http://" + Host + "/Report/FooterHeaderInvoiceSalesInv?c_ID=" + c_ID);


            var demoViewPortrait = new ViewAsPdf(_oSales)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 48, Right = 5, Top = 70 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;


        }

        [HttpGet]
        public IActionResult SalesInvoice_PrintGLM(int? SaleMaster_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");

            int? c_ID = Convert.ToInt32(company_ID);
            ViewBag.ReportCompanyPur_F = c_ID;
            #endregion
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;
            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.SalesMasterPrintObj = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID);
            _oSales.SalesDetailPrintObj = _oSales.Get_SalesInvoiceDetailPrintByID_DAL(SaleMaster_ID).ToList();
            _oSales.SalesDetailPrintObj_TTL = _oSales.Get_SalesInvoiceDetailPrintByID_ttl(SaleMaster_ID);
            _oSales.SalesReceiptDetailsList_Print = _oSales.Get_ReceiptDetailSaleListByID_DAL_Invoice_Print(SaleMaster_ID);
            string Host = Request.Host.ToString();

            string TotalAmount = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID).AfterVatTotal;
            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0";
            }
            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());


            _oSales.SalesDetailPrintObj_TTL.Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.SalesMasterPrintObj.Currency_ShortName
                + (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.SalesMasterPrintObj.Minor_ShortName + " only" : " only");



            string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/PrintHeaderInvoiceSalesInv?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID,

            "http://" + Host + "/Report/FooterHeaderInvoiceSalesInvGLM?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID);


            var demoViewPortrait = new ViewAsPdf(_oSales)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 75, Right = 5, Top = 60 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;


        }
        [HttpGet]
        public IActionResult StockReport_Print(string StartPurchaseDate, string EndPurchaseDate, string Chassis_No, string PurchaseRef, int? Make_ID, int? MakeModel_description_ID, int? Color_ID)
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

            Chassis_No = (String.IsNullOrEmpty(Chassis_No) || String.IsNullOrWhiteSpace(Chassis_No) ? "" : Chassis_No);
            PurchaseRef = (String.IsNullOrEmpty(PurchaseRef) || String.IsNullOrWhiteSpace(PurchaseRef) ? "" : PurchaseRef);
            Make_ID = (Make_ID == null ? 0 : Make_ID);
            MakeModel_description_ID = (MakeModel_description_ID == null ? 0 : MakeModel_description_ID);
            Color_ID = (Color_ID == null ? 0 : Color_ID);

            //For Report Purpuse
            ViewBag.start = StartPurchaseDate;
            ViewBag.end = EndPurchaseDate;


            _oReports.StockReportList = _oReports.pa_Select_StockReport_DAL(StartPurchaseDate, EndPurchaseDate, Chassis_No, PurchaseRef, Make_ID, MakeModel_description_ID, Color_ID, c_ID).ToList();
            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
  "--footer-left \"  Printed At: " +
DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
" --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };
            return demoViewPortrait;

        }
        [HttpGet]
        public IActionResult SaleReport_Print(string StartDate, string EndDate, int? Customer_ID, string Sale_Ref, int Vendor_ID, string cartype, string chassis, string Pur_Ref, int? Make, int? Model_Desc, string Model_Year)
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

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            Sale_Ref = (String.IsNullOrEmpty(Sale_Ref) || String.IsNullOrWhiteSpace(Sale_Ref) ? "" : Sale_Ref);
            Customer_ID = Customer_ID == null ? 0 : Customer_ID;
            Vendor_ID = (Vendor_ID == 0 ? 0 : Vendor_ID);
            cartype = (String.IsNullOrEmpty(cartype) || String.IsNullOrWhiteSpace(cartype) ? "" : cartype);
            chassis = (String.IsNullOrEmpty(chassis) || String.IsNullOrWhiteSpace(chassis) ? "" : chassis);
            Pur_Ref = (String.IsNullOrEmpty(Pur_Ref) || String.IsNullOrWhiteSpace(Pur_Ref) ? "" : Pur_Ref);
            Make = (Make == null ? 0 : Make);
            Model_Desc = (Model_Desc == null ? 0 : Model_Desc);
            Model_Year = (String.IsNullOrEmpty(Model_Year) || String.IsNullOrWhiteSpace(Model_Year) ? "" : Model_Year);

            _oReports.SaleReportPrint = _oReports.pa_Select_SalesReport_DAL(StartDate, EndDate, Customer_ID, Sale_Ref, c_ID, Vendor_ID, cartype, chassis, Pur_Ref, Make, Model_Desc, Model_Year).ToList();

            _oReports.SaleReportTTLPrint = _oReports.pa_Select_SalesReport_TTL_DAL(StartDate, EndDate, Customer_ID, Sale_Ref, c_ID, Vendor_ID, cartype, chassis, Pur_Ref, Make, Model_Desc, Model_Year);
            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
 "--footer-left \"  Printed At: " +
DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
" --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };
            return demoViewPortrait;

        }




        [HttpGet]
        public IActionResult StockReport_Excel(string StartPurchaseDate, string EndPurchaseDate, string Chassis_No, string PurchaseRef, int? Make_ID, int? MakeModel_description_ID, int? Color_ID)
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

            Chassis_No = (String.IsNullOrEmpty(Chassis_No) || String.IsNullOrWhiteSpace(Chassis_No) ? "" : Chassis_No);
            PurchaseRef = (String.IsNullOrEmpty(PurchaseRef) || String.IsNullOrWhiteSpace(PurchaseRef) ? "" : PurchaseRef);
            Make_ID = (Make_ID == null ? 0 : Make_ID);
            MakeModel_description_ID = (MakeModel_description_ID == null ? 0 : MakeModel_description_ID);
            Color_ID = (Color_ID == null ? 0 : Color_ID);
            _oReports.StockReportList = _oReports.pa_Select_StockReport_DAL(StartPurchaseDate, EndPurchaseDate, Chassis_No, PurchaseRef, Make_ID, MakeModel_description_ID, Color_ID, c_ID).ToList();

            using (var workbook = new XLWorkbook())
            {

                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("StockReport");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Ref";
                worksheet.Cell(currentRow, 2).Value = "Chassis No";
                worksheet.Cell(currentRow, 3).Value = "Make";
                worksheet.Cell(currentRow, 4).Value = "Model Desciption";
                worksheet.Cell(currentRow, 5).Value = "Color";
                worksheet.Cell(currentRow, 6).Value = "Transmission";
                worksheet.Cell(currentRow, 7).Value = "Loc";
                worksheet.Cell(currentRow, 8).Value = "Date";
                worksheet.Cell(currentRow, 9).Value = "Stock Type";
                worksheet.Cell(currentRow, 10).Value = "Model Year";
                worksheet.Cell(currentRow, 11).Value = "Vendor Name";
                worksheet.Cell(currentRow, 12).Value = "Stock Status";


                foreach (var item in _oReports.StockReportList)
                {
                    currentRow++;

                    worksheet.Cell(currentRow, 1).Value = item.PurchaseRef;
                    worksheet.Cell(currentRow, 2).Value = item.Chassis_No;
                    worksheet.Cell(currentRow, 3).Value = item.Make;
                    worksheet.Cell(currentRow, 4).Value = item.ModelDesciption;
                    worksheet.Cell(currentRow, 5).Value = item.Color;
                    worksheet.Cell(currentRow, 6).Value = item.Transmission;
                    worksheet.Cell(currentRow, 7).Value = item.CarLocation;
                    worksheet.Cell(currentRow, 8).Value = item.PurchaseDate;
                    worksheet.Cell(currentRow, 9).Value = item.StockType;
                    worksheet.Cell(currentRow, 10).Value = item.ModelYear;
                    worksheet.Cell(currentRow, 11).Value = item.Vendor_Name;
                    worksheet.Cell(currentRow, 12).Value = item.Stock_Status;
                }


                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "StockReport.xlsx");
                }



            }


        }


        [HttpGet]
        public IActionResult SaleReport_Excel(string StartDate, string EndDate, int? Customer_ID, string Sale_Ref, int Vendor_ID, string cartype, string chassis, string Pur_Ref, int? Make, int? Model_Desc, string Model_Year)
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

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            Sale_Ref = (String.IsNullOrEmpty(Sale_Ref) || String.IsNullOrWhiteSpace(Sale_Ref) ? "" : Sale_Ref);
            Customer_ID = Customer_ID == null ? 0 : Customer_ID;
            Vendor_ID = (Vendor_ID == 0 ? 0 : Vendor_ID);
            cartype = (String.IsNullOrEmpty(cartype) || String.IsNullOrWhiteSpace(cartype) ? "" : cartype);
            chassis = (String.IsNullOrEmpty(chassis) || String.IsNullOrWhiteSpace(chassis) ? "" : chassis);
            Pur_Ref = (String.IsNullOrEmpty(Pur_Ref) || String.IsNullOrWhiteSpace(Pur_Ref) ? "" : Pur_Ref);
            Make = (Make == null ? 0 : Make);
            Model_Desc = (Model_Desc == null ? 0 : Model_Desc);
            Model_Year = (String.IsNullOrEmpty(Model_Year) || String.IsNullOrWhiteSpace(Model_Year) ? "" : Model_Year);

            _oReports.SaleReportPrint = _oReports.pa_Select_SalesReport_DAL(StartDate, EndDate, Customer_ID, Sale_Ref, c_ID, Vendor_ID, cartype, chassis, Pur_Ref, Make, Model_Desc, Model_Year).ToList();

            _oReports.SaleReportTTLPrint = _oReports.pa_Select_SalesReport_TTL_DAL(StartDate, EndDate, Customer_ID, Sale_Ref, c_ID, Vendor_ID, cartype, chassis, Pur_Ref, Make, Model_Desc, Model_Year);

            using (var workbook = new XLWorkbook())
            {

                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("SaleReportPrint");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Sale Ref";
                worksheet.Cell(currentRow, 2).Value = "Sale Value";
                worksheet.Cell(currentRow, 3).Value = "Chassis No";
                worksheet.Cell(currentRow, 4).Value = "Sale Date";
                worksheet.Cell(currentRow, 5).Value = "Make";
                worksheet.Cell(currentRow, 6).Value = "Model Desciption";
                worksheet.Cell(currentRow, 7).Value = "Customer Name";
                worksheet.Cell(currentRow, 8).Value = "Profit Lost";
                worksheet.Cell(currentRow, 9).Value = "Total Expense";
                worksheet.Cell(currentRow, 10).Value = "Total Cost";

                foreach (var item in _oReports.SaleReportPrint)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.SaleRef;
                    worksheet.Cell(currentRow, 2).Value = item.Sale_Value;
                    worksheet.Cell(currentRow, 3).Value = item.Chassis_no;
                    worksheet.Cell(currentRow, 4).Value = item.Date;
                    worksheet.Cell(currentRow, 5).Value = item.make;
                    worksheet.Cell(currentRow, 6).Value = item.model_description;
                    worksheet.Cell(currentRow, 7).Value = item.Customer_Name;
                    worksheet.Cell(currentRow, 8).Value = item.ProfitLost;

                    if (!string.IsNullOrEmpty(item.Total_Expense) && !string.IsNullOrWhiteSpace(item.Total_Expense))
                    {
                        worksheet.Cell(currentRow, 9).Value = Convert.ToDecimal(item.Total_Expense);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 9).Value = item.Total_Expense;
                    }


                    if (!string.IsNullOrEmpty(item.Total_Cost) && !string.IsNullOrWhiteSpace(item.Total_Cost))
                    {
                        worksheet.Cell(currentRow, 10).Value = Convert.ToDecimal(item.Total_Cost);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 10).Value = item.Total_Cost;
                    }

                }

                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "SaleReport.xlsx");
                }
            }
        }







        [HttpGet]
        public IActionResult VendorReport_Excel(string StartDate, string EndDate, int? VendorID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            VendorID = VendorID == null ? 0 : VendorID;
            _oReports.Ledger_Print = _oReports.pa_Select_VendorLedger_DAL(StartDate, EndDate, VendorID.ToString(), c_ID).ToList();
            _oReports.Ledger_TTL_Print = _oReports.pa_Select_VendorLedger_DAL_TTL_Print(StartDate, EndDate, VendorID.ToString(), c_ID);


            using (var workbook = new XLWorkbook())
            {

                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("VendorReport");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Date";
                worksheet.Cell(currentRow, 2).Value = "Debit";
                worksheet.Cell(currentRow, 3).Value = "Credit";
                worksheet.Cell(currentRow, 4).Value = "Description";
                worksheet.Cell(currentRow, 5).Value = "Chassis No";
                worksheet.Cell(currentRow, 6).Value = "Party Name";
                worksheet.Cell(currentRow, 7).Value = "Ref #";


                foreach (var item in _oReports.Ledger_Print)
                {
                    currentRow++;

                    worksheet.Cell(currentRow, 1).Value = item.Date;


                    if (!string.IsNullOrEmpty(item.Debit) && !string.IsNullOrWhiteSpace(item.Debit))
                    {
                        worksheet.Cell(currentRow, 2).Value = Convert.ToDecimal(item.Debit);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 2).Value = item.Debit;
                    }

                    if (!string.IsNullOrEmpty(item.Credit) && !string.IsNullOrWhiteSpace(item.Credit))
                    {
                        worksheet.Cell(currentRow, 3).Value = Convert.ToDecimal(item.Credit);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 3).Value = item.Credit;
                    }

                    worksheet.Cell(currentRow, 4).Value = item.Description;
                    worksheet.Cell(currentRow, 5).Value = item.Chassis_No;
                    worksheet.Cell(currentRow, 6).Value = item.PartyName;
                    worksheet.Cell(currentRow, 7).Value = item.trans_ref;
                }

                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "VendorReport.xlsx");
                }
            }
        }


        [HttpGet]
        public IActionResult VendorReport_Print(string StartDate, string EndDate, int? VendorID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            VendorID = VendorID == null ? 0 : VendorID;

            _oReports.Ledger_Print = _oReports.pa_Select_VendorLedger_DAL(StartDate, EndDate, VendorID.ToString(), c_ID).ToList();
            _oReports.Ledger_TTL_Print = _oReports.pa_Select_VendorLedger_DAL_TTL_Print(StartDate, EndDate, VendorID.ToString(), c_ID);

            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
  "--footer-left \"  Printed At: " +
DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
" --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };
            return demoViewPortrait;

        }
        [HttpGet]
        public IActionResult VendorReport_byChassis_Excel(string StartDate, string EndDate, int VendorID)
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

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            VendorID = VendorID == 0 ? 0 : VendorID;
            _oReports.VendorReport_byChassis = _oReports.select_VendorReport_Chassis_Wise_DAL(StartDate, EndDate, VendorID, c_ID).ToList();
            _oReports.VendorReport_byChassis_TTL = _oReports.select_VendorReport_Chassis_Wise_TTL_DAL(StartDate, EndDate, VendorID, c_ID).ToList();


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("VendorReport_byChassis");
                var currentRow = 1;


                worksheet.Cell(currentRow, 1).Value = "Date";
                worksheet.Cell(currentRow, 2).Value = "Ref";
                worksheet.Cell(currentRow, 3).Value = "Vendor";
                worksheet.Cell(currentRow, 4).Value = "Chassis";
                worksheet.Cell(currentRow, 5).Value = "Price";
                worksheet.Cell(currentRow, 6).Value = "Paid";
                worksheet.Cell(currentRow, 7).Value = "Balance";




                foreach (var item in _oReports.VendorReport_byChassis)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.PurchaseDate;
                    worksheet.Cell(currentRow, 2).Value = item.PurchaseRef;
                    worksheet.Cell(currentRow, 3).Value = item.Vendor_Name;
                    worksheet.Cell(currentRow, 4).Value = item.Chassis_No;
                    worksheet.Cell(currentRow, 5).Value = item.TotalPrice;
                    worksheet.Cell(currentRow, 6).Value = item.Paid;
                    worksheet.Cell(currentRow, 7).Value = item.Balance;

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "VendorReport_byChassis.xlsx");
                }
            }





        }

        [HttpGet]
        public IActionResult CustomerReport_Print(string StartDate, string EndDate, int? Customer_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");



            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            if (c_ID == 1)
            {
                ViewBag.CompanyName = configuration.GetSection("AppSettings").GetSection("FirstCompanyName").Value;
            } else
            {
                ViewBag.CompanyName = configuration.GetSection("AppSettings").GetSection("SecondCompanyName").Value;
            }

            #endregion

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            Customer_ID = Customer_ID == null ? 0 : Customer_ID;



            ViewBag.IsVehicleAppHidden = configuration.GetSection("AppSettings").GetSection("IsVehicleAppHidden").Value;


            //   _oReports.CustomerLedger_Print = _oReports.pa_Select_CustomerLedger_DAL(StartDate, EndDate, Customer_ID.ToString(), c_ID).ToList();

            _oReports.CustomerLedger_Print = _oReports.pa_Select_CustomerLedger_DAL(StartDate, EndDate, Customer_ID.ToString(), c_ID).ToList();
            _oReports.CustomerLedger_TTL_Print = _oReports.pa_Select_CustomerLedger_DAL_TTL1(StartDate, EndDate, Customer_ID.ToString(), c_ID);




            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
"--footer-left \"  Printed At: " +
DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
" --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };
            return demoViewPortrait;

        }

        [HttpGet]
        public IActionResult CustomerReport_Excel(string StartDate, string EndDate, int? Customer_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            Customer_ID = Customer_ID == null ? 0 : Customer_ID;


            _oReports.CustomerLedger_Print = _oReports.pa_Select_CustomerLedger_DAL(StartDate, EndDate, Customer_ID.ToString(), c_ID).ToList();
            _oReports.CustomerLedger_TTL_Print = _oReports.pa_Select_CustomerLedger_DAL_TTL1(StartDate, EndDate, Customer_ID.ToString(), c_ID);



            using (var workbook = new XLWorkbook())
            {

                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("CustomerReport");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Date";
                worksheet.Cell(currentRow, 2).Value = "Debit";
                worksheet.Cell(currentRow, 3).Value = "Credit";
                worksheet.Cell(currentRow, 4).Value = "Description";
                worksheet.Cell(currentRow, 5).Value = "Chassis No";
                worksheet.Cell(currentRow, 6).Value = "Party Name";
                worksheet.Cell(currentRow, 7).Value = "Ref #";





                foreach (var item in _oReports.CustomerLedger_Print)
                {
                    currentRow++;

                    worksheet.Cell(currentRow, 1).Value = item.Date;
                    worksheet.Cell(currentRow, 2).Value = item.Debit;
                    worksheet.Cell(currentRow, 3).Value = item.Credit;
                    worksheet.Cell(currentRow, 4).Value = item.Description;
                    worksheet.Cell(currentRow, 5).Value = item.Chassis_No;
                    worksheet.Cell(currentRow, 6).Value = item.PartyName;
                    worksheet.Cell(currentRow, 7).Value = item.trans_ref;

                }


                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "CustomerReport.xlsx");
                }



            }





        }

        [HttpGet]
        public IActionResult PayableReport_Print()
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



            _oReports.payableSummary = _oReports.pa_Select_Payable_Summary_DAL(c_ID).ToList();



            string footer = "--footer-center \"  Created Date: " +
DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Page: [page]/[toPage]\"" +
" --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\"";

            var demoViewPortrait = new ViewAsPdf(_oReports)
            {

                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 20 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,

                CustomSwitches = footer

            };
            return demoViewPortrait;

        }
        [HttpGet]
        public IActionResult PayableReport_Excel()
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



            _oReports.payableSummary = _oReports.pa_Select_Payable_Summary_DAL(c_ID).ToList();


            using (var workbook = new XLWorkbook())
            {

                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("payableSummary");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Party ID";
                worksheet.Cell(currentRow, 2).Value = "Party Name";
                worksheet.Cell(currentRow, 3).Value = "Debit";
                worksheet.Cell(currentRow, 4).Value = "Credit";
                worksheet.Cell(currentRow, 5).Value = "Balance";

                foreach (var item in _oReports.payableSummary)
                {
                    currentRow++;

                    worksheet.Cell(currentRow, 1).Value = item.Party_ID;
                    worksheet.Cell(currentRow, 2).Value = item.PartyName;

                    if (!string.IsNullOrEmpty(item.Debit) && !string.IsNullOrWhiteSpace(item.Debit))
                    {
                        worksheet.Cell(currentRow, 3).Value = Convert.ToDecimal(item.Debit);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 3).Value = item.Debit;
                    }


                    if (!string.IsNullOrEmpty(item.Credit) && !string.IsNullOrWhiteSpace(item.Credit))
                    {
                        worksheet.Cell(currentRow, 4).Value = Convert.ToDecimal(item.Credit);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 4).Value = item.Credit;
                    }

                    worksheet.Cell(currentRow, 5).Value = item.Balance;
                }
                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "PayableSummery.xlsx");
                }
            }
        }


        [HttpGet]
        public IActionResult ReceivableReport_Print()
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



            _oReports.ReceiveAbleSummary = _oReports.pa_Select_Recievable_Summary_DAL(c_ID).ToList();



            string footer = "--footer-center \"  Created Date: " +
DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Page: [page]/[toPage]\"" +
" --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\"";

            var demoViewPortrait = new ViewAsPdf(_oReports)
            {

                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 20 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,

                CustomSwitches = footer

            };
            return demoViewPortrait;

        }
        [HttpGet]
        public IActionResult ReceivableReport_Excel()
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



            _oReports.ReceiveAbleSummary = _oReports.pa_Select_Recievable_Summary_DAL(c_ID).ToList();


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ReceivableSummary");
                var currentRow = 1;


                worksheet.Cell(currentRow, 1).Value = "Party ID";
                worksheet.Cell(currentRow, 2).Value = "Party Name";
                worksheet.Cell(currentRow, 3).Value = "Debit";
                worksheet.Cell(currentRow, 4).Value = "Credit";
                worksheet.Cell(currentRow, 5).Value = "Balance";




                foreach (var item in _oReports.ReceiveAbleSummary)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.Party_ID;
                    worksheet.Cell(currentRow, 2).Value = item.PartyName;

                    if (!string.IsNullOrEmpty(item.Debit) && !string.IsNullOrWhiteSpace(item.Debit))
                    {
                        worksheet.Cell(currentRow, 3).Value = Convert.ToDecimal(item.Debit);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 3).Value = item.Debit;
                    }

                    if (!string.IsNullOrEmpty(item.Credit) && !string.IsNullOrWhiteSpace(item.Credit))
                    {
                        worksheet.Cell(currentRow, 4).Value = Convert.ToDecimal(item.Credit);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 4).Value = item.Credit;
                    }
                    worksheet.Cell(currentRow, 5).Value = item.Balance;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "ReceivableReport.xlsx");
                }
            }






        }


        [HttpGet]
        public IActionResult StockReportList_Excel(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
           string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, bool optional_cost, int StockType_ID)
        {
            using (var workbook = new XLWorkbook())
            {

                #region login checking
                string UserName_Admin = HttpContext.Session.GetString("UserName");
                string UserID_Admin = HttpContext.Session.GetString("UserID");
                if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
                {
                    return RedirectToAction("Index", "Login");
                }
                #endregion

                ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
                Make_ID = Make_ID == null ? 0 : Make_ID;
                MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
                Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
                PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
                PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;
                string company_ID = HttpContext.Session.GetString("c_ID");
                int c_IDs = Convert.ToInt32(company_ID);
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

                ViewBag.isOptional_Cost = optional_cost;

                _oStock.StockListGetObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();

                _oStock.StockList_TTL1 = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();

                _oStock.StockListStats1 = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);
                _oStock.StockListStats1.isOptionalCost = optional_cost;



                var worksheet = workbook.Worksheets.Add("StockList");
                var currentRow = 1;

                if (optional_cost == false)
                {
                    worksheet.Cell(currentRow, 1).Value = "S.NO";
                    worksheet.Cell(currentRow, 2).Value = "Chassis No";
                    worksheet.Cell(currentRow, 3).Value = "Make";
                    worksheet.Cell(currentRow, 4).Value = "Make Model";
                    worksheet.Cell(currentRow, 5).Value = "Color";
                    worksheet.Cell(currentRow, 6).Value = "Model Year";
                    worksheet.Cell(currentRow, 7).Value = "Stock Type";
                    worksheet.Cell(currentRow, 9).Value = "Purchase Date";
                    worksheet.Cell(currentRow, 10).Value = "Location";
                    worksheet.Cell(currentRow, 12).Value = "Status";

                }


                if (optional_cost == true)
                {
                    worksheet.Cell(currentRow, 1).Value = "S.NO";
                    worksheet.Cell(currentRow, 2).Value = "Chassis No";
                    worksheet.Cell(currentRow, 3).Value = "Make";
                    worksheet.Cell(currentRow, 4).Value = "Make Model";
                    worksheet.Cell(currentRow, 5).Value = "Color";
                    worksheet.Cell(currentRow, 6).Value = "Model Year";
                    worksheet.Cell(currentRow, 7).Value = "Stock Type";
                    worksheet.Cell(currentRow, 8).Value = "Vendor";
                    worksheet.Cell(currentRow, 9).Value = "Purchase Date";
                    worksheet.Cell(currentRow, 10).Value = "Location";
                    worksheet.Cell(currentRow, 11).Value = "Total Cost";
                    worksheet.Cell(currentRow, 12).Value = "Status";

                }






                int counter = 0;
                foreach (var item in _oStock.StockListGetObject)
                {
                    currentRow++;
                    counter++;

                    if (optional_cost == true)
                    {
                        worksheet.Cell(currentRow, 1).Value = counter;
                        worksheet.Cell(currentRow, 2).Value = item.Chassis_No;
                        worksheet.Cell(currentRow, 3).Value = item.Make;
                        worksheet.Cell(currentRow, 4).Value = item.ModelDesciption;
                        worksheet.Cell(currentRow, 5).Value = item.Color;
                        worksheet.Cell(currentRow, 6).Value = item.ModelYear;
                        worksheet.Cell(currentRow, 7).Value = item.StockTypeName;
                        worksheet.Cell(currentRow, 8).Value = item.Vendor_Name;
                        worksheet.Cell(currentRow, 9).Value = item.PurchaseDate;
                        worksheet.Cell(currentRow, 10).Value = item.CarLocation;
                        worksheet.Cell(currentRow, 11).Value = item.Total_Cost;
                        worksheet.Cell(currentRow, 12).Value = item.Stock_Status;
                    }



                    if (optional_cost == false)
                    {
                        worksheet.Cell(currentRow, 1).Value = counter;
                        worksheet.Cell(currentRow, 2).Value = item.Chassis_No;
                        worksheet.Cell(currentRow, 3).Value = item.Make;
                        worksheet.Cell(currentRow, 4).Value = item.ModelDesciption;
                        worksheet.Cell(currentRow, 5).Value = item.Color;
                        worksheet.Cell(currentRow, 6).Value = item.ModelYear;
                        worksheet.Cell(currentRow, 7).Value = item.StockTypeName;
                        worksheet.Cell(currentRow, 9).Value = item.PurchaseDate;
                        worksheet.Cell(currentRow, 10).Value = item.CarLocation;
                        worksheet.Cell(currentRow, 12).Value = item.Stock_Status;
                    }

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "StockList.xlsx");
                }
            }


        }



        [HttpGet]
        public IActionResult StockReportList_am_Excel(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
           string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, bool optional_cost, int StockType_ID)
        {
            using (var workbook = new XLWorkbook())
            {

                #region login checking
                string UserName_Admin = HttpContext.Session.GetString("UserName");
                string UserID_Admin = HttpContext.Session.GetString("UserID");
                if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
                {
                    return RedirectToAction("Index", "Login");
                }
                #endregion

                ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
                Make_ID = Make_ID == null ? 0 : Make_ID;
                MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
                Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
                PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
                PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;
                string company_ID = HttpContext.Session.GetString("c_ID");
                int c_IDs = Convert.ToInt32(company_ID);
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

                ViewBag.isOptional_Cost = optional_cost;

                _oStock.StockListGetObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();

                _oStock.StockList_TTL1 = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();

                _oStock.StockListStats1 = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);
                _oStock.StockListStats1.isOptionalCost = optional_cost;



                var worksheet = workbook.Worksheets.Add("StockList");
                var currentRow = 1;

                if (optional_cost == false)
                {
                    worksheet.Cell(currentRow, 1).Value = "S#";
                    worksheet.Cell(currentRow, 2).Value = "Chassis";
                    worksheet.Cell(currentRow, 3).Value = "CCode";
                    worksheet.Cell(currentRow, 4).Value = "Make";
                    worksheet.Cell(currentRow, 5).Value = "Model";
                    worksheet.Cell(currentRow, 6).Value = "Color";
                    worksheet.Cell(currentRow, 7).Value = "Year";
                    worksheet.Cell(currentRow, 8).Value = "PurDate";
                    worksheet.Cell(currentRow, 9).Value = "Vendor";
                    worksheet.Cell(currentRow, 10).Value = "Price";
                    worksheet.Cell(currentRow, 11).Value = "Exp";
                    worksheet.Cell(currentRow, 12).Value = "Total";
                    worksheet.Cell(currentRow, 13).Value = "Location";
                    worksheet.Cell(currentRow, 14).Value = "Status";

                }


                if (optional_cost == true)
                {
                    worksheet.Cell(currentRow, 1).Value = "S#";
                    worksheet.Cell(currentRow, 2).Value = "Chassis";
                    worksheet.Cell(currentRow, 3).Value = "CCode";
                    worksheet.Cell(currentRow, 4).Value = "Make";
                    worksheet.Cell(currentRow, 5).Value = "Model";
                    worksheet.Cell(currentRow, 6).Value = "Color";
                    worksheet.Cell(currentRow, 7).Value = "Year";
                    worksheet.Cell(currentRow, 8).Value = "PurDate";
                    worksheet.Cell(currentRow, 9).Value = "Vendor";
                    worksheet.Cell(currentRow, 10).Value = "Price";
                    worksheet.Cell(currentRow, 11).Value = "Exp";
                    worksheet.Cell(currentRow, 12).Value = "Total";
                    worksheet.Cell(currentRow, 13).Value = "Location";
                    worksheet.Cell(currentRow, 14).Value = "Status";

                }






                int counter = 0;
                foreach (var item in _oStock.StockListGetObject)
                {
                    currentRow++;
                    counter++;

                    if (optional_cost == true)
                    {
                        worksheet.Cell(currentRow, 1).Value = counter;
                        worksheet.Cell(currentRow, 2).Value = item.Chassis_No;
                        worksheet.Cell(currentRow, 3).Value = item.LotNumber;
                        worksheet.Cell(currentRow, 4).Value = item.Make;
                        worksheet.Cell(currentRow, 5).Value = item.ModelDesciption;
                        worksheet.Cell(currentRow, 6).Value = item.Color;
                        worksheet.Cell(currentRow, 7).Value = item.ModelYear;   
                        worksheet.Cell(currentRow, 8).Value = item.PurchaseDate;
                        worksheet.Cell(currentRow, 9).Value = item.Vendor_Name;
                        worksheet.Cell(currentRow, 10).Value = item.PriceOrignal;   
                        worksheet.Cell(currentRow, 11).Value = item.TotalExpense;
                        worksheet.Cell(currentRow, 12).Value = item.Total_Cost;
                        worksheet.Cell(currentRow, 13).Value = item.CarLocation;
                        worksheet.Cell(currentRow, 14).Value = item.Stock_Status;
                    }



                    if (optional_cost == false)
                    {
                        worksheet.Cell(currentRow, 1).Value = counter;
                        worksheet.Cell(currentRow, 2).Value = item.Chassis_No;
                        worksheet.Cell(currentRow, 3).Value = item.LotNumber;
                        worksheet.Cell(currentRow, 4).Value = item.Make;
                        worksheet.Cell(currentRow, 5).Value = item.ModelDesciption;
                        worksheet.Cell(currentRow, 6).Value = item.Color;
                        worksheet.Cell(currentRow, 7).Value = item.ModelYear;
                        worksheet.Cell(currentRow, 8).Value = item.PurchaseDate;
                        worksheet.Cell(currentRow, 9).Value = item.Vendor_Name;
                        worksheet.Cell(currentRow, 10).Value = item.PriceOrignal;
                        worksheet.Cell(currentRow, 11).Value = item.TotalExpense;
                        worksheet.Cell(currentRow, 12).Value = item.Total_Cost;
                        worksheet.Cell(currentRow, 13).Value = item.CarLocation;
                        worksheet.Cell(currentRow, 14).Value = item.Stock_Status;
                    }

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "StockList.xlsx");
                }
            }


        }


        [HttpGet]
        public IActionResult StockList_Print(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
           string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, bool optional_cost, int StockType_ID)
        {
            ViewBag.PageTitle = "Stock List Print";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
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

            ViewBag.isOptional_Cost = optional_cost;

            _oStock.StockListGetObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();

            _oStock.StockList_TTL1 = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();

            _oStock.StockListStats1 = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);
            _oStock.StockListStats1.isOptionalCost = optional_cost;

            var demoViewPortrait = new ViewAsPdf(_oStock)
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



        [HttpGet]
        public IActionResult StockList_Print_am(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
           string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, bool optional_cost, int StockType_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            ViewBag.PageTitle = "Stock List Print";
           

            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
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

            ViewBag.isOptional_Cost = optional_cost;

            _oStock.StockListGetObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();

            _oStock.StockList_TTL1 = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();

            _oStock.StockListStats1 = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);
            _oStock.StockListStats1.isOptionalCost = optional_cost;

            var demoViewPortrait = new ViewAsPdf(_oStock)
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

        [HttpGet]
        public IActionResult SalesInvoiceList_print(string SaleRef, string ManualRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int Status_ID,
            int c_ID, int Make, int Model_Desc, string Model_Year)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            ViewBag.PageTitle = "Sales Invoice List";

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

            if (Model_Year == "undefined")
            {
                Model_Year = "";
            }
            _oSales.SalesMasterIPagedList1 = _oSales.Get_SalesMasterInvoiceList_DAL(SaleRef, ManualRef, StartDate, EndDate, Customer_Name, Chassis_No, Status_ID, c_ID, Make, Model_Desc, Model_Year);
            _oSales.SalesMasterListTOTAL1 = _oSales.Get_SalesMasterInvoiceList_TTL_DAL(SaleRef, ManualRef, StartDate, EndDate, Customer_Name, Chassis_No, Status_ID, c_ID, Make, Model_Desc, Model_Year).ToList();






            var demoViewPortrait = new ViewAsPdf(_oSales)
            {

                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
           "--footer-center \"  Printed At: " +
         DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Page: [page]-[toPage]\"" +
         " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\""

            };
            return demoViewPortrait;
        }

        [HttpGet]

        public IActionResult PurchaseList_print(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, string ChassisNo, int? c_ID)
        {


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

            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;



            _oPurchase.purchaseMasterIPagedList1 = _oPurchase.Get_PurchaseMasterList_DAL(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, ChassisNo, c_ID).ToList();

            _oPurchase.purchaseMasterTotal = _oPurchase.Get_PurchaseMasterList_TTL(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, ChassisNo, c_ID).ToList();


            var demoViewPortrait = new ViewAsPdf(_oPurchase)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
   "--footer-left \"  Printed At: " +
 DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
 " --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };
            return demoViewPortrait;

        }


        [HttpGet]

        public IActionResult PurchaseList_Excel(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, string ChassisNo, int? c_ID)
        {

            using (var workbook = new XLWorkbook())
            {

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


                ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;

                _oPurchase.purchaseMasterIPagedList1 = _oPurchase.Get_PurchaseMasterList_DAL(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, ChassisNo, c_ID).ToList();



                var worksheet = workbook.Worksheets.Add("PurchaseList");

                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "Ref";
                worksheet.Cell(currentRow, 2).Value = "Vendor";
                worksheet.Cell(currentRow, 3).Value = "Date";
                worksheet.Cell(currentRow, 4).Value = "Amount";
                worksheet.Cell(currentRow, 5).Value = "VAT";
                worksheet.Cell(currentRow, 6).Value = "Total";
                worksheet.Cell(currentRow, 7).Value = "Discount";
                worksheet.Cell(currentRow, 8).Value = "Paid";
                worksheet.Cell(currentRow, 9).Value = "Balance";
                worksheet.Cell(currentRow, 10).Value = "Status";



                int counter = 0;
                foreach (var item in _oPurchase.purchaseMasterIPagedList1)
                {
                    currentRow++;
                    counter++;

                    worksheet.Cell(currentRow, 1).Value = item.PurchaseRef;
                    worksheet.Cell(currentRow, 2).Value = item.Vendor_Name;
                    worksheet.Cell(currentRow, 3).Value = item.PurchaseDate;
                    worksheet.Cell(currentRow, 4).Value = item.Total_Amount;
                    worksheet.Cell(currentRow, 5).Value = item.VAT_Exp;
                    worksheet.Cell(currentRow, 6).Value = item.Total;
                    worksheet.Cell(currentRow, 7).Value = item.Discount;
                    worksheet.Cell(currentRow, 8).Value = item.Paid;
                    worksheet.Cell(currentRow, 9).Value = item.Bal_Due;
                    worksheet.Cell(currentRow, 10).Value = item.PurchaseStatus;

                }



                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "PurchaseList.xlsx");
                }



            }





        }




        public static decimal Parse(string input)
        {
            return decimal.Parse(Regex.Replace(input, @"[^\d.]", ""));
        }
        public IActionResult TrailBalance_Excel(string StartDate, string EndDate)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);


            _oAccounts.TrailBalanceList = _oAccounts.Get_TrailBalanceList_DAL(StartDate, EndDate, c_ID);


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("TrailBalance");
                var currentRow = 1;


                worksheet.Cell(currentRow, 1).Value = "Account";
                worksheet.Cell(currentRow, 2).Value = "DR";
                worksheet.Cell(currentRow, 3).Value = "CR";




                foreach (var item in _oAccounts.TrailBalanceList)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.ACCOUNT;
                    if (!string.IsNullOrEmpty(item.Debit) && !string.IsNullOrWhiteSpace(item.Debit))
                    {
                        worksheet.Cell(currentRow, 2).Value = Convert.ToDecimal(item.Debit);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 2).Value = item.Debit;
                    }
                    if (!string.IsNullOrEmpty(item.Credit) && !string.IsNullOrWhiteSpace(item.Credit))
                    {
                        worksheet.Cell(currentRow, 3).Value = Convert.ToDecimal(item.Credit);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 3).Value = item.Credit;
                    }


                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "TrailBalance.xlsx");
                }
            }



        }



        [HttpGet]
        public IActionResult PaymentsList_Excel(string PaymentRef, int Party_ID_Name, string StartDate, string EndDate, string PaidToNameText,
                  string ChassisNo, string PurchaseRef, string VoucherType, int? c_ID, string Cheque_no)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            PaymentRef = String.IsNullOrEmpty(PaymentRef) ? "" : PaymentRef;
            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            PaidToNameText = String.IsNullOrEmpty(PaidToNameText) ? "" : PaidToNameText;
            VoucherType = String.IsNullOrEmpty(VoucherType) ? "" : VoucherType;
            Party_ID_Name = Party_ID_Name == 0 ? 0 : Party_ID_Name;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == null || c_ID == 0 ? c_IDs : c_ID;
            Cheque_no = String.IsNullOrEmpty(Cheque_no) ? "" : Cheque_no;


            _oAccounts.paymentMasterListIPaged1 = _oAccounts.Get_PaymentMasterList_DAL(PaymentRef, Party_ID_Name, StartDate, EndDate,
                ChassisNo, PaidToNameText, PurchaseRef, VoucherType, Convert.ToInt32(c_ID), Cheque_no).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("PaymentList");
                var currentRow = 1;


                worksheet.Cell(currentRow, 1).Value = "Payment Ref";
                worksheet.Cell(currentRow, 2).Value = "Date";
                worksheet.Cell(currentRow, 3).Value = "Paid To";
                worksheet.Cell(currentRow, 4).Value = "Explanation";
                worksheet.Cell(currentRow, 5).Value = "Total";




                foreach (var item in _oAccounts.paymentMasterListIPaged1)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.PaymentMaster_ref;
                    worksheet.Cell(currentRow, 2).Value = item.Date;
                    worksheet.Cell(currentRow, 3).Value = item.Party_ID_Name;
                    worksheet.Cell(currentRow, 4).Value = item.Explanation;

                    if (!string.IsNullOrEmpty(item.Total_Amount) && !string.IsNullOrWhiteSpace(item.Total_Amount))
                    {
                        worksheet.Cell(currentRow, 5).Value = Convert.ToDecimal(item.Total_Amount);

                    }
                    else
                    {
                        worksheet.Cell(currentRow, 5).Value = item.Total_Amount;
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "PaymentList.xlsx");
                }
            }
        }




        [HttpGet]
        public IActionResult PaymentsList_Print(string PaymentRef, int Party_ID_Name, string StartDate, string EndDate, string PaidToNameText,
              string ChassisNo, string PurchaseRef, string VoucherType, int? c_ID, string Cheque_no)
        {



            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            PaymentRef = String.IsNullOrEmpty(PaymentRef) ? "" : PaymentRef;
            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            PaidToNameText = String.IsNullOrEmpty(PaidToNameText) ? "" : PaidToNameText;
            VoucherType = String.IsNullOrEmpty(VoucherType) ? "" : VoucherType;

            Party_ID_Name = Party_ID_Name == 0 ? 0 : Party_ID_Name;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == null || c_ID == 0 ? c_IDs : c_ID;
            Cheque_no = String.IsNullOrEmpty(Cheque_no) ? "" : Cheque_no;


            _oAccounts.paymentMasterListIPaged1 = _oAccounts.Get_PaymentMasterList_DAL(PaymentRef, Party_ID_Name, StartDate, EndDate,
                ChassisNo, PaidToNameText, PurchaseRef, VoucherType, Convert.ToInt32(c_ID), Cheque_no).ToList();

            _oAccounts.PaymentMasterListTTL_obj1 = _oAccounts.Get_PaymentMasterList_TTL_DAL(PaymentRef, Party_ID_Name, StartDate, EndDate,
               ChassisNo, PaidToNameText, PurchaseRef, VoucherType, Convert.ToInt32(c_ID), Cheque_no).ToList();



            var demoViewPortrait = new ViewAsPdf(_oAccounts)
            {

                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
           "--footer-left \"  Printed At: " +
         DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
         " --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""

            };
            return demoViewPortrait;

            //return View(_oAccounts);



        }

        [HttpGet]
        public IActionResult ReceiptList_print(string ReceiptRef, int Party_ID, string PartyNameText, string SaleRef, string Chassis_no, string StartDate, string EndDate, string Cheque_no, int c_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            ReceiptRef = String.IsNullOrEmpty(ReceiptRef) ? "" : ReceiptRef;
            Party_ID = Party_ID == 0 ? 0 : Party_ID;
            PartyNameText = String.IsNullOrEmpty(PartyNameText) ? "" : PartyNameText;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            Chassis_no = String.IsNullOrEmpty(Chassis_no) ? "" : Chassis_no;

            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            Cheque_no = String.IsNullOrEmpty(Cheque_no) ? "" : Cheque_no;

            //---receipt master list for page
            _oAccounts.receiptMasterIPagedList1 = _oAccounts.Get_ReceiptMaster_List_DAL1(ReceiptRef, Party_ID, PartyNameText, SaleRef, Chassis_no, StartDate, EndDate, Cheque_no, c_ID).ToList();

            _oAccounts.receiptMasterList_TTL1 = _oAccounts.Get_ReceiptMaster_List_TTL_DAL(ReceiptRef, Party_ID, PartyNameText, SaleRef, Chassis_no, StartDate, EndDate, Cheque_no, c_ID).ToList();

            var demoViewPortrait = new ViewAsPdf(_oAccounts)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
      "--footer-left \"  Printed At: " +
    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
    " --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };
            return demoViewPortrait;

        }


        [HttpGet]
        public IActionResult PaymentVoucher_Print(int? PaymentMaster_ID)
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





            _oReports.PaymentVoucher_Print = _oReports.GetDataForPaymentMaster_Print(PaymentMaster_ID);
            string TotalAmount = _oReports.GetDataForPaymentMaster_Print(PaymentMaster_ID).Select(x => x.Total_Amount).LastOrDefault();
            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0";
            }
            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());

            _oReports.PaymentVoucher_Print.LastOrDefault().Total_Amt_inWords = converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[0])) + " " + _oReports.GetDataForPaymentMaster_Print(PaymentMaster_ID).LastOrDefault().Currency_ShortName
                 + (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oReports.GetDataForPaymentMaster_Print(PaymentMaster_ID).LastOrDefault().Minor_ShortName + " only" : " only");

            string Host = Request.Host.ToString();

            string customSwitches = string.Format("--header-spacing \"0\" --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/PrintHeaderInvoice?c_ID=" + c_ID,

            "http://" + Host + "/Report/FooterPaymentVoucher?PaymentMaster_ID=" + PaymentMaster_ID + "&c_ID=" + c_ID);



            //return View(_oReports);
            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 54, Right = 5, Top = 25 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;


        }

        [HttpGet]
        public IActionResult ReceiptVoucher_Print(int? ReceiptMaster_ID)
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



            _oReports.ReceiptVoucher_Print = _oReports.GetDataForReceiptMaster_Print(ReceiptMaster_ID);





            string TotalAmount = _oReports.GetDataForReceiptMaster_Print(ReceiptMaster_ID).Select(x => x.Total_Amount).LastOrDefault();
            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0";
            }

            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());

            _oReports.ReceiptVoucher_Print.LastOrDefault().Total_Amt_inWords = converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[0])) + " " +
                _oReports.GetDataForReceiptMaster_Print(ReceiptMaster_ID).LastOrDefault().Currency_ShortName
                + (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oReports.GetDataForReceiptMaster_Print(ReceiptMaster_ID).LastOrDefault().Minor_ShortName + " only" : " only");









            string Host = Request.Host.ToString();



            string customSwitches = string.Format("--header-spacing \"0\" --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/HeaderReceiptVoucher?c_ID=" + c_ID,

              "http://" + Host + "/Report/FooterReceiptVoucher?ReceiptMaster_ID=" + ReceiptMaster_ID + "&c_ID=" + c_ID);

            //return View(_oReports);
            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 35, Right = 5, Top = 25 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;

        }


        [HttpGet]
        public IActionResult ReceiptVoucher_Print2(int? ReceiptMaster_ID)
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



            _oReports.ReceiptVoucher_Print = _oReports.GetDataForReceiptMaster_Print(ReceiptMaster_ID);


            var Sale_Id = _oReports.ReceiptVoucher_Print = _oReports.GetDataForReceiptMaster_Print(ReceiptMaster_ID);

            foreach (var item in Sale_Id)
            {
                ViewBag.Sale_ID = item.Sale_ID;

            }


            if (ViewBag.Sale_ID != null)
            {
                int SaleMaster_id = ViewBag.Sale_ID;

                _oReports.Sale_Info_Print = _oReports.GetDataForSaleInfo_Print(SaleMaster_id);


            }


            string TotalAmount = _oReports.GetDataForReceiptMaster_Print(ReceiptMaster_ID).Select(x => x.Total_Amount).LastOrDefault();
            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0";
            }

            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());

            _oReports.ReceiptVoucher_Print.LastOrDefault().Total_Amt_inWords = converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[0])) + " " +
                _oReports.GetDataForReceiptMaster_Print(ReceiptMaster_ID).LastOrDefault().Currency_ShortName
                + (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oReports.GetDataForReceiptMaster_Print(ReceiptMaster_ID).LastOrDefault().Minor_ShortName + " only" : " only");









            string Host = Request.Host.ToString();



            string customSwitches = string.Format("--header-spacing \"0\" --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/HeaderReceiptVoucher?c_ID=" + c_ID,

              "http://" + Host + "/Report/FooterReceiptVoucher?ReceiptMaster_ID=" + ReceiptMaster_ID + "&c_ID=" + c_ID);

            //return View(_oReports);
            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 35, Right = 5, Top = 25 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;

        }


        [HttpGet]
        public IActionResult ReceiptVoucher_PrintGLM(int? ReceiptMaster_ID)
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



            _oReports.ReceiptVoucher_Print = _oReports.GetDataForReceiptMaster_Print(ReceiptMaster_ID);



            string TotalAmount = _oReports.GetDataForReceiptMaster_Print(ReceiptMaster_ID).Select(x => x.Total_Amount).LastOrDefault();
            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0";
            }

            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());

            _oReports.ReceiptVoucher_Print.LastOrDefault().Total_Amt_inWords = converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[0])) + " " +
                _oReports.GetDataForReceiptMaster_Print(ReceiptMaster_ID).LastOrDefault().Currency_ShortName
                + (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oReports.GetDataForReceiptMaster_Print(ReceiptMaster_ID).LastOrDefault().Minor_ShortName + " only" : " only");



            string Host = Request.Host.ToString();



            string customSwitches = string.Format("--header-spacing \"0\" --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/HeaderReceiptVoucher?c_ID=" + c_ID,

              "http://" + Host + "/Report/FooterReceiptVoucherGLM?ReceiptMaster_ID=" + ReceiptMaster_ID + "&c_ID=" + c_ID);

            //return View(_oReports);
            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 65, Right = 5, Top = 25 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;

        }
        [HttpGet]
        public IActionResult ReceiptList_Excel(string ReceiptRef, int Party_ID, string PartyNameText, string SaleRef, string Chassis_no, string StartDate, string EndDate, string Cheque_no, int c_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            ReceiptRef = String.IsNullOrEmpty(ReceiptRef) ? "" : ReceiptRef;
            Party_ID = Party_ID == 0 ? 0 : Party_ID;
            PartyNameText = String.IsNullOrEmpty(PartyNameText) ? "" : PartyNameText;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            Chassis_no = String.IsNullOrEmpty(Chassis_no) ? "" : Chassis_no;

            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;


            Cheque_no = String.IsNullOrEmpty(Cheque_no) ? "" : Cheque_no;

            //---receipt master list for page
            _oAccounts.receiptMasterIPagedList1 = _oAccounts.Get_ReceiptMaster_List_DAL1(ReceiptRef, Party_ID, PartyNameText, SaleRef, Chassis_no, StartDate, EndDate, Cheque_no, c_ID).ToList();

            _oAccounts.receiptMasterList_TTL1 = _oAccounts.Get_ReceiptMaster_List_TTL_DAL(ReceiptRef, Party_ID, PartyNameText, SaleRef, Chassis_no, StartDate, EndDate, Cheque_no, c_ID).ToList();


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ReceiptList");
                var currentRow = 1;


                worksheet.Cell(currentRow, 1).Value = "Receipt Ref";
                worksheet.Cell(currentRow, 2).Value = "DATE";
                worksheet.Cell(currentRow, 3).Value = "Paid To";
                worksheet.Cell(currentRow, 4).Value = "Explanation";
                worksheet.Cell(currentRow, 5).Value = "Total";




                foreach (var item in _oAccounts.receiptMasterIPagedList1)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.ReceiptMaster_ref;
                    worksheet.Cell(currentRow, 2).Value = item.ReceiptDate;
                    worksheet.Cell(currentRow, 3).Value = item.Party_ID_Name;
                    worksheet.Cell(currentRow, 4).Value = item.Explanation;
                    worksheet.Cell(currentRow, 5).Value = item.Total_Amount;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "ReceiptList.xlsx");
                }
            }

        }

        [HttpGet]
        public IActionResult PurchaseInvoicePrint(int? PurchaseMaster_ID)
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



            _oReports.PurchaseMasterPrint = _oReports.GetDataForPurchaseMaster(PurchaseMaster_ID);
            _oReports.PurchaseDetailPrint = _oReports.GetDataForPurchasePrintDetail(PurchaseMaster_ID).ToList();
            _oReports.PurchaseStockSummaryPrint = _oReports.GetDataForStockSummaryPrintDetail(PurchaseMaster_ID).ToList();



            string Host = Request.Host.ToString();


            //return View(_oReports);

            string customSwitches = string.Format("--header-spacing \"0\" --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/HEADERPurchaseInvoice?c_ID=" + c_ID,

              "http://" + Host + "/Report/FooterHeaderInvoiceSalesInv?c_ID=" + c_ID);




            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 48, Right = 5, Top = 25 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;



        }

        [AllowAnonymous]
        public IActionResult FooterHeaderInvoice()
        {
            return View("~/Views/Report/" + ReportFolder + "/FooterHeaderInvoice.cshtml");
        }

        [AllowAnonymous]
        public IActionResult PrintHeaderInvoiceSalesInv(int? c_ID, int? SaleMaster_ID)
        {
            ViewBag.ReportCompanySalInv = c_ID;


            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.SalesMasterPrintObj = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID);
            _oSales.SalesDetailPrintObj = _oSales.Get_SalesInvoiceDetailPrintByID_DAL(SaleMaster_ID).ToList();
            _oSales.SalesDetailPrintObj_TTL = _oSales.Get_SalesInvoiceDetailPrintByID_ttl(SaleMaster_ID);
            return View("~/Views/Report/" + ReportFolder + "/PrintHeaderInvoiceSalesInv.cshtml", _oSales);
        }
        [AllowAnonymous]
        public IActionResult PrintHeaderInvoiceSaleBooking(int? c_ID, int? SaleMaster_ID)
        {
            ViewBag.ReportCompanySalInv = c_ID;


            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.SalesMasterPrintObj = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID);
            _oSales.SalesDetailPrintObj = _oSales.Get_SalesInvoiceDetailPrintByID_DAL(SaleMaster_ID).ToList();
            _oSales.SalesDetailPrintObj_TTL = _oSales.Get_SalesInvoiceDetailPrintByID_ttl(SaleMaster_ID);
            return View("~/Views/Report/" + ReportFolder + "/PrintHeaderInvoiceSaleBooking.cshtml", _oSales);
        }
        [AllowAnonymous]
        public IActionResult PrintHeaderInvoiceSalesInvThermal(int? c_ID, int? SaleMaster_ID)
        {
            ViewBag.ReportCompanySalInv = c_ID;
            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.SalesMasterPrintObj = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID);
            _oSales.SalesDetailPrintObj = _oSales.Get_SalesInvoiceDetailPrintByID_DAL(SaleMaster_ID).ToList();
            _oSales.SalesDetailPrintObj_TTL = _oSales.Get_SalesInvoiceDetailPrintByID_ttl(SaleMaster_ID);
            return View("~/Views/Report/" + ReportFolder + "/PrintHeaderInvoiceSalesInvThermal.cshtml", _oSales);
        }
        [AllowAnonymous]
        public IActionResult PrintHeaderInvoice(int? c_ID)
        {
            ViewBag.ReportCompanyPayment = c_ID;
            return View("~/Views/Report/" + ReportFolder + "/PrintHeaderInvoice.cshtml");
        }


        [AllowAnonymous]
        public IActionResult FooterHeaderInvoiceSalesInvGLM(int? c_ID, int SaleMaster_ID)
        {
            ViewBag.ReportCompanyPur_F = c_ID;
            _oSales.SalesMasterPrintObj = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID);
            return View("~/Views/Report/" + ReportFolder + "/FooterHeaderInvoiceSalesInvGLM.cshtml", _oSales);
        }
        [AllowAnonymous]
        public IActionResult FooterHeaderInvoiceSalesInv(int? c_ID)
        {
            ViewBag.ReportCompanyPur_F = c_ID;

            return View("~/Views/Report/" + ReportFolder + "/FooterHeaderInvoiceSalesInv.cshtml");
        }


        [HttpGet]
        public IActionResult PerformaInvoiceList_print(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int c_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion



            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_No = String.IsNullOrEmpty(Chassis_No) ? "" : Chassis_No;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;


            _oSales.SalesMasterIPagedList1 = _oSales.Get_PerformaInvoice_List_DAL(SaleRef, StartDate, EndDate, Customer_Name, Chassis_No, c_ID).ToList();




            var demoViewPortrait = new ViewAsPdf(_oSales)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
   "--footer-left \"  Printed At: " +
 DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
 " --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };
            return demoViewPortrait;

            //return View(_oSales);
        }


        [HttpGet]
        public IActionResult PerformaInvoiceList_Excel(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int c_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_No = String.IsNullOrEmpty(Chassis_No) ? "" : Chassis_No;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;


            _oSales.SalesMasterIPagedList1 = _oSales.Get_PerformaInvoice_List_DAL(SaleRef, StartDate, EndDate, Customer_Name, Chassis_No, c_ID).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("SalesMasterIPagedList1");
                var currentRow = 1;


                worksheet.Cell(currentRow, 1).Value = "Ref";
                worksheet.Cell(currentRow, 2).Value = "Date";
                worksheet.Cell(currentRow, 3).Value = "Customer Name";
                worksheet.Cell(currentRow, 4).Value = "Amount";
                worksheet.Cell(currentRow, 5).Value = "VAT";
                worksheet.Cell(currentRow, 6).Value = "Total";
                worksheet.Cell(currentRow, 7).Value = "Status";

                foreach (var item in _oSales.SalesMasterIPagedList1)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.SaleRef;
                    worksheet.Cell(currentRow, 2).Value = item.SaleDate;
                    worksheet.Cell(currentRow, 3).Value = item.Customer_Name;

                    if (!string.IsNullOrEmpty(item.Amount) && !string.IsNullOrWhiteSpace(item.Amount))
                    {
                        worksheet.Cell(currentRow, 4).Value = Convert.ToDecimal(item.Amount);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 4).Value = item.Amount;
                    }

                    worksheet.Cell(currentRow, 5).Value = item.VATExp;

                    if (!string.IsNullOrEmpty(item.Total_Amt) && !string.IsNullOrWhiteSpace(item.Total_Amt))
                    {
                        worksheet.Cell(currentRow, 6).Value = Convert.ToDecimal(item.Total_Amt);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 6).Value = item.Total_Amt;
                    }

                    worksheet.Cell(currentRow, 7).Value = item.Status;

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "PerformaInvoiceList.xlsx");
                }
            }

            //return View(_oSales);
        }




        [HttpGet]
        public IActionResult SaleInvoiceAgreement_Print(int SaleMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);

            ViewBag.vC_ID = c_ID;


            //if (c_ID == 1)
            //{
            //    ViewBag.ActiveCompanyName = configuration.GetSection("AppSettings").GetSection("FirstCompanyName").Value;
            //}
            //else
            //{
            //    ViewBag.ActiveCompanyName = configuration.GetSection("AppSettings").GetSection("SecondCompanyName").Value;

            //}




            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            _oSales.SalesAggrMasterPrintObj = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID);
            _oSales.SalesAggrDetailPrintObj = _oSales.Get_SalesInvoiceDetailPrintByID_DAL(SaleMaster_ID).ToList();
            _oSales.SalesAggrDetailPrintObj_Summary = _oSales.Get_SaleSummaryDescription_DAL(SaleMaster_ID, c_ID).ToList();
            _oSales.SalesAggrDetailPrintObj_TTL = _oSales.Get_SalesInvoiceDetailPrintByID_ttl(SaleMaster_ID);

            string UrlHeader = Url.Action("PrintHeader", "Report");
            string Host = Request.Host.ToString();

            string TotalAmount = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID).AfterVatTotal;
            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0";
            }

            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());


            _oSales.SalesAggrDetailPrintObj_TTL.Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.SalesAggrMasterPrintObj.Currency_ShortName
                + (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.SalesAggrMasterPrintObj.Minor_ShortName + " only" : " only");



            string customSwitches = string.Format("--header-spacing \"0\" --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

           "http://" + Host + "/Report/PrintHeader?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID,

           "http://" + Host + "/Report/PrintFooter?c_ID=" + c_ID);


            //return View(_oSales);
            return new ViewAsPdf(_oSales)
            {
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 78, Right = 5, Top = 40 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches + "--footer-left \"    Page: [page]-[toPage]\"" +
 "  --footer-font-size \"10\"  --footer-font-name \"Segoe UI\""




            };
        }

        [HttpGet]
        public IActionResult SalesInvoiceList_Excel(string SaleRef, string ManualRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int Status_ID,
            int c_ID, int Make, int Model_Desc, string Model_Year)
        {

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
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_No = String.IsNullOrEmpty(Chassis_No) ? "" : Chassis_No;
            ManualRef = String.IsNullOrEmpty(ManualRef) ? "" : ManualRef;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;
            Status_ID = Status_ID == 0 ? 0 : Status_ID;
            Make = Make == 0 ? 0 : Make;
            Model_Desc = Model_Desc == 0 ? 0 : Model_Desc;
            Model_Year = String.IsNullOrEmpty(Model_Year) ? "" : Model_Year;



            _oSales.SalesMasterIPagedList1 = _oSales.Get_SalesMasterInvoiceList_DAL(SaleRef, ManualRef, StartDate, EndDate, Customer_Name, Chassis_No, Status_ID, c_ID, Make, Model_Desc, Model_Year);
            _oSales.SalesMasterListTOTAL1 = _oSales.Get_SalesMasterInvoiceList_TTL_DAL(SaleRef, ManualRef, StartDate, EndDate, Customer_Name, Chassis_No, Status_ID, c_ID, Make, Model_Desc, Model_Year).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("SalesInvoiceList");
                var currentRow = 1;



                worksheet.Cell(currentRow, 1).Value = "Ref";
                worksheet.Cell(currentRow, 2).Value = "Date";
                worksheet.Cell(currentRow, 3).Value = "Customer Name";
                worksheet.Cell(currentRow, 4).Value = "Total";
                worksheet.Cell(currentRow, 5).Value = "Paid";
                worksheet.Cell(currentRow, 6).Value = "Balance";
                worksheet.Cell(currentRow, 7).Value = "Status";




                foreach (var item in _oSales.SalesMasterIPagedList1)
                {
                    currentRow++;

                    worksheet.Cell(currentRow, 1).Value = item.SaleRef;
                    worksheet.Cell(currentRow, 2).Value = item.SaleDate;
                    worksheet.Cell(currentRow, 3).Value = item.Customer_Name;
                    worksheet.Cell(currentRow, 4).Value = item.Total_Amt;
                    worksheet.Cell(currentRow, 5).Value = item.Paid_amt;
                    worksheet.Cell(currentRow, 6).Value = item.Bal_due;
                    worksheet.Cell(currentRow, 7).Value = item.Status;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "SalesInvoiceList.xlsx");
                }
            }


        }




        [AllowAnonymous]
        public IActionResult PrintHeader(int c_ID, int SaleMaster_ID)
        {


            ViewBag.ReportCompanySaleAgr = c_ID;
            _oSales.SalesAggrMasterPrintObj = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID);
            _oSales.SalesAggrDetailPrintObj = _oSales.Get_SalesInvoiceDetailPrintByID_DAL(SaleMaster_ID).ToList();
            _oSales.SalesAggrDetailPrintObj_Summary = _oSales.Get_SaleSummaryDescription_DAL(SaleMaster_ID, c_ID).ToList();
            _oSales.SalesAggrDetailPrintObj_TTL = _oSales.Get_SalesInvoiceDetailPrintByID_ttl(SaleMaster_ID);
            return View("~/Views/Report/" + ReportFolder + "/PrintHeader.cshtml", _oSales);
        }


        [AllowAnonymous]
        public IActionResult HEADERPurchaseInvoice(int? c_ID)
        {
            ViewBag.ReportCompanyPur = c_ID;
            return View("~/Views/Report/" + ReportFolder + "/HEADERPurchaseInvoice.cshtml");
        }
        [AllowAnonymous]
        public IActionResult HeaderReceiptVoucher(int? c_ID)
        {
            ViewBag.ReportCompanyRec = c_ID;
            return View("~/Views/Report/" + ReportFolder + "/HeaderReceiptVoucher.cshtml");
        }
        [AllowAnonymous]
        public IActionResult FooterPaymentVoucher(int PaymentMaster_ID, int? c_ID)
        {
            ViewBag.ReportCompanyPay_f = c_ID;
            _oReports.PaymentVoucher_Print = _oReports.GetDataForPaymentMaster_Print(PaymentMaster_ID);
            return View("~/Views/Report/" + ReportFolder + "/FooterPaymentVoucher.cshtml", _oReports);
        }
        [AllowAnonymous]
        public IActionResult PrintFooter(int? c_ID)
        {
            ViewBag.ReportCompanySaleAgr_F = c_ID;

            if (c_ID == 1)
            {
                ViewBag.ActiveCompanyName = configuration.GetSection("AppSettings").GetSection("FirstCompanyName").Value;
            }
            else
            {
                ViewBag.ActiveCompanyName = configuration.GetSection("AppSettings").GetSection("SecondCompanyName").Value;

            }


            return View("~/Views/Report/" + ReportFolder + "/PrintFooter.cshtml");
        }

        [AllowAnonymous]
        public IActionResult FooterReceiptVoucher(int ReceiptMaster_ID, int? c_ID)
        {
            ViewBag.ReportCompanyRec_f = c_ID;
            _oReports.ReceiptVoucher_Print = _oReports.GetDataForReceiptMaster_Print(ReceiptMaster_ID);
            return View("~/Views/Report/" + ReportFolder + "/FooterReceiptVoucher.cshtml", _oReports);
        }
        [AllowAnonymous]
        public IActionResult FooterReceiptVoucherGLM(int ReceiptMaster_ID, int? c_ID)
        {
            ViewBag.ReportCompanyRec_f = c_ID;
            _oReports.ReceiptVoucher_Print = _oReports.GetDataForReceiptMaster_Print(ReceiptMaster_ID);
            return View("~/Views/Report/" + ReportFolder + "/FooterReceiptVoucherGLM.cshtml", _oReports);
        }
        //---Getting Other purchase list
        [HttpGet]

        public IActionResult OtherPurchaseList_print(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID)
        {



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


            //---Purchase master other  list for page
            _oPurchase.purchaseOtherMasterList1 = _oPurchase.Get_PurchaseMasterList_Other_DAL1(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, c_ID).ToList();

            var demoViewPortrait = new ViewAsPdf(_oPurchase)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
               "--footer-left \"  Printed At: " +
             DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
             " --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };
            return demoViewPortrait;


        }


        //---Getting Other purchase list
        [HttpGet]
        public IActionResult OtherPurchaseList_Excel(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID)
        {



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


            //---Purchase master other  list for page
            _oPurchase.purchaseOtherMasterList1 = _oPurchase.Get_PurchaseMasterList_Other_DAL1(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, c_ID).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("PurchaseOtherList");
                var currentRow = 1;


                worksheet.Cell(currentRow, 1).Value = "Ref";
                worksheet.Cell(currentRow, 2).Value = "Vendor";
                worksheet.Cell(currentRow, 3).Value = "Date";
                worksheet.Cell(currentRow, 4).Value = "Amt";
                worksheet.Cell(currentRow, 5).Value = "VAT";
                worksheet.Cell(currentRow, 6).Value = "Total";
                worksheet.Cell(currentRow, 7).Value = "Paid";
                worksheet.Cell(currentRow, 8).Value = "Balance";
                worksheet.Cell(currentRow, 9).Value = "Status";

                foreach (var item in _oPurchase.purchaseOtherMasterList1)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.PurchaseRef;
                    worksheet.Cell(currentRow, 2).Value = item.Vendor_Name;
                    worksheet.Cell(currentRow, 3).Value = item.PurchaseDate;
                    worksheet.Cell(currentRow, 4).Value = item.Total;
                    worksheet.Cell(currentRow, 5).Value = item.VAT_Exp;

                    if (!string.IsNullOrEmpty(item.Total_Amount) && !string.IsNullOrWhiteSpace(item.Total_Amount))
                    {
                        worksheet.Cell(currentRow, 6).Value = Convert.ToDecimal(item.Total_Amount);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 6).Value = item.Total_Amount;
                    }


                    if (!string.IsNullOrEmpty(item.Paid) && !string.IsNullOrWhiteSpace(item.Paid))
                    {
                        worksheet.Cell(currentRow, 7).Value = Convert.ToDecimal(item.Paid);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 7).Value = item.Paid;
                    }


                    if (!string.IsNullOrEmpty(item.Bal_Due) && !string.IsNullOrWhiteSpace(item.Bal_Due))
                    {
                        worksheet.Cell(currentRow, 8).Value = Convert.ToDecimal(item.Bal_Due);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 8).Value = item.Bal_Due;
                    }


                    worksheet.Cell(currentRow, 9).Value = item.PurchaseStatus;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "PurchaseOtherList.xlsx");
                }
            }
        }




        [HttpGet]
        public IActionResult DepositRecieved_print(string DV_Ref, string Party_ID_Name, string StartDate, string EndDate, string Chassis_no, int? c_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion



            DV_Ref = String.IsNullOrEmpty(DV_Ref) ? "" : DV_Ref;
            Party_ID_Name = String.IsNullOrEmpty(Party_ID_Name) ? "" : Party_ID_Name;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_no = String.IsNullOrEmpty(Chassis_no) ? "" : Chassis_no;
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == null || c_ID == 0 ? c_IDs : c_ID;

            //---delivery master list for page

            _oDelivery.DepositsDetailListIPageList1 = _oDelivery.Get_DepositMaster_List_DAL1(DV_Ref, Party_ID_Name, StartDate, EndDate, Chassis_no, c_ID).ToList();

            //return View(_oDelivery);

            var demoViewPortrait = new ViewAsPdf(_oDelivery)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
     "--footer-left \"  Printed At: " +
   DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
   " --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };
            return demoViewPortrait;


        }

        [HttpGet]
        public IActionResult SalesBookingList_print(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int c_ID)
        {


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
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_No = String.IsNullOrEmpty(Chassis_No) ? "" : Chassis_No;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;


            _oSales.SalesMasterIPagedList1_1 = _oSales.Get_SalesMasterBooking_List_DAL1(SaleRef, StartDate, EndDate, Customer_Name, Chassis_No, 0, c_ID).ToList();


            var demoViewPortrait = new ViewAsPdf(_oSales)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
  "--footer-left \"  Printed At: " +
DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
" --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };
            return demoViewPortrait;

        }


        [HttpGet]
        public IActionResult SalesBookingList_Excel(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int c_ID)
        {

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
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_No = String.IsNullOrEmpty(Chassis_No) ? "" : Chassis_No;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;


            _oSales.SalesMasterIPagedList1_1 = _oSales.Get_SalesMasterBooking_List_DAL(SaleRef, StartDate, EndDate, Customer_Name, Chassis_No, 0, c_ID).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("SalesMasterIPagedList1_1");
                var currentRow = 1;


                worksheet.Cell(currentRow, 1).Value = "Sale Ref";
                worksheet.Cell(currentRow, 2).Value = "Sale Date";
                worksheet.Cell(currentRow, 3).Value = "Customer Name";
                worksheet.Cell(currentRow, 4).Value = "Total Amount";
                worksheet.Cell(currentRow, 5).Value = "Paid";
                worksheet.Cell(currentRow, 6).Value = "Bal Due";
                worksheet.Cell(currentRow, 7).Value = "Status";




                foreach (var item in _oSales.SalesMasterIPagedList1_1)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.SaleRef;
                    worksheet.Cell(currentRow, 2).Value = item.SaleDate;
                    worksheet.Cell(currentRow, 3).Value = item.Customer_Name;

                    if (!string.IsNullOrEmpty(item.Total_Amt) && !string.IsNullOrWhiteSpace(item.Total_Amt))
                    {
                        worksheet.Cell(currentRow, 4).Value = Convert.ToDecimal(item.Total_Amt);

                    }
                    else
                    {
                        worksheet.Cell(currentRow, 4).Value = item.Total_Amt;
                    }

                    if (!string.IsNullOrEmpty(item.Total_Paid) && !string.IsNullOrWhiteSpace(item.Total_Paid))
                    {
                        worksheet.Cell(currentRow, 5).Value = Convert.ToDecimal(item.Total_Paid);

                    }
                    else
                    {
                        worksheet.Cell(currentRow, 5).Value = item.Total_Paid;
                    }

                    worksheet.Cell(currentRow, 6).Value = item.Bal_due;


                    worksheet.Cell(currentRow, 7).Value = item.SaleStatus;


                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "SalesBookingList.xlsx");
                }
            }


        }



        [HttpGet]
        public IActionResult DepositList_print(string DV_Ref, string Party_ID_Name, string StartDate, string EndDate, string Chassis_no, string Cheque_no, int? c_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            DV_Ref = String.IsNullOrEmpty(DV_Ref) ? "" : DV_Ref;
            Party_ID_Name = String.IsNullOrEmpty(Party_ID_Name) ? "" : Party_ID_Name;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Chassis_no = String.IsNullOrEmpty(Chassis_no) ? "" : Chassis_no;
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == null || c_ID == 0 ? c_IDs : c_ID;
            Cheque_no = String.IsNullOrEmpty(Cheque_no) ? "" : Cheque_no;

            _oDelivery.DepositsDetailListIPageList1_1 = _oDelivery.Get_DepositMaster_List_DAL1_1(DV_Ref, Party_ID_Name, StartDate, EndDate, Chassis_no, Cheque_no, c_ID).ToList();

            var demoViewPortrait = new ViewAsPdf(_oDelivery)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
 "--footer-left \"  Printed At: " +
DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
" --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };
            return demoViewPortrait;


            // return View(_oDelivery);
        }

        [HttpGet]
        public IActionResult DepositList_Excel(string DV_Ref, string Party_ID_Name, string StartDate, int? page, string EndDate, string Chassis_no, string Cheque_no, int? c_ID)
        {

            using (var workbook = new XLWorkbook())
            {


                #region login checking
                string UserName_Admin = HttpContext.Session.GetString("UserName");
                string UserID_Admin = HttpContext.Session.GetString("UserID");
                if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
                {
                    return RedirectToAction("Index", "Login");
                }
                #endregion


                DV_Ref = String.IsNullOrEmpty(DV_Ref) ? "" : DV_Ref;
                Party_ID_Name = String.IsNullOrEmpty(Party_ID_Name) ? "" : Party_ID_Name;
                StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
                EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
                Chassis_no = String.IsNullOrEmpty(Chassis_no) ? "" : Chassis_no;
                string company_ID = HttpContext.Session.GetString("c_ID");
                int c_IDs = Convert.ToInt32(company_ID);
                c_ID = c_ID == null || c_ID == 0 ? c_IDs : c_ID;
                Cheque_no = String.IsNullOrEmpty(Cheque_no) ? "" : Cheque_no;

                _oDelivery.DepositsDetailListIPageList1_1 = _oDelivery.Get_DepositMaster_List_DAL1_1(DV_Ref, Party_ID_Name, StartDate, EndDate, Chassis_no, Cheque_no, c_ID).ToList();

                var worksheet = workbook.Worksheets.Add("DepositList");

                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "Ref";
                worksheet.Cell(currentRow, 2).Value = "Date";
                worksheet.Cell(currentRow, 3).Value = "Customer Name";
                worksheet.Cell(currentRow, 4).Value = "Deposit";
                worksheet.Cell(currentRow, 5).Value = "Others";
                worksheet.Cell(currentRow, 6).Value = "Total";

                worksheet.Cell(currentRow, 7).Value = "Recieved";
                worksheet.Cell(currentRow, 8).Value = "Return";



                int counter = 0;
                foreach (var item in _oDelivery.DepositsDetailListIPageList1_1)
                {
                    currentRow++;
                    counter++;

                    worksheet.Cell(currentRow, 1).Value = item.DV_Ref;
                    worksheet.Cell(currentRow, 2).Value = item.Date_Taken;
                    worksheet.Cell(currentRow, 3).Value = item.CUSTOMER_NAME;
                    worksheet.Cell(currentRow, 4).Value = item.Deposit;
                    worksheet.Cell(currentRow, 5).Value = item.Others;

                    if (!string.IsNullOrEmpty(item.Total_Collected) && !string.IsNullOrWhiteSpace(item.Total_Collected))
                    {
                        worksheet.Cell(currentRow, 6).Value = Convert.ToDecimal(item.Total_Collected);

                    }
                    else
                    {
                        worksheet.Cell(currentRow, 6).Value = item.Total_Collected;
                    }

                    if (!string.IsNullOrEmpty(item.Amount_Recieved) && !string.IsNullOrWhiteSpace(item.Amount_Recieved))
                    {
                        worksheet.Cell(currentRow, 7).Value = Convert.ToDecimal(item.Amount_Recieved);

                    }
                    else
                    {
                        worksheet.Cell(currentRow, 7).Value = item.Amount_Recieved;
                    }


                    if (!string.IsNullOrEmpty(item.Amount_Return))
                    {
                        worksheet.Cell(currentRow, 8).Value = Convert.ToDecimal(item.Amount_Return);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 8).Value = item.Amount_Return;
                    }

                }
                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "DepositList.xlsx");
                }
            }
        }











        public IActionResult TrailBalance_print(string StartDate, string EndDate)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);


            _oAccounts.TrailBalanceList = _oAccounts.Get_TrailBalanceList_DAL(StartDate, EndDate, c_ID);
            _oAccounts.TrailBalanceList_ttl = _oAccounts.Get_TrailBalanceList_TTL(StartDate, EndDate, c_ID);

            var demoViewPortrait = new ViewAsPdf(_oAccounts)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
"--footer-left \"  Printed At: " +
DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
" --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };


            return demoViewPortrait;
        }

        [HttpGet]
        public IActionResult Ledger_print(string From_Date, string To_Date, int AccountID, string Trans_Ref)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            From_Date = (String.IsNullOrEmpty(From_Date) || String.IsNullOrWhiteSpace(From_Date) ? "" : From_Date);
            To_Date = (String.IsNullOrEmpty(To_Date) || String.IsNullOrWhiteSpace(To_Date) ? "" : To_Date);
            AccountID = (AccountID == 0 ? 0 : AccountID);
            Trans_Ref = (String.IsNullOrEmpty(Trans_Ref) || String.IsNullOrWhiteSpace(Trans_Ref) ? "" : Trans_Ref);
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);



            _oAccounts.LedgerMasterIPagedList_1 = _oAccounts.Get_LedgerList_DAL_NEW(From_Date, AccountID, To_Date, Trans_Ref, c_ID);
            _oAccounts.LedgerMasterIPagedList_TTL1 = _oAccounts.Get_LedgerList_DAL_TTL1(From_Date, AccountID, To_Date, Trans_Ref, c_ID).ToList();


            var demoViewPortrait = new ViewAsPdf(_oAccounts)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--footer-left \"  Printed At: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" + " --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""

            };

            return demoViewPortrait;
        }



        [HttpGet]
        public IActionResult Ledger_Excel(string From_Date, string To_Date, int AccountID, string Trans_Ref)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            From_Date = (String.IsNullOrEmpty(From_Date) || String.IsNullOrWhiteSpace(From_Date) ? "" : From_Date);
            To_Date = (String.IsNullOrEmpty(To_Date) || String.IsNullOrWhiteSpace(To_Date) ? "" : To_Date);
            AccountID = (AccountID == 0 ? 0 : AccountID);
            Trans_Ref = (String.IsNullOrEmpty(Trans_Ref) || String.IsNullOrWhiteSpace(Trans_Ref) ? "" : Trans_Ref);
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);



            _oAccounts.LedgerMasterIPagedList_1 = _oAccounts.Get_LedgerList_DAL_NEW(From_Date, AccountID, To_Date, Trans_Ref, c_ID);
            _oAccounts.LedgerMasterIPagedList_TTL1 = _oAccounts.Get_LedgerList_DAL_TTL1(From_Date, AccountID, To_Date, Trans_Ref, c_ID).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Ledger");
                var currentRow = 1;


                worksheet.Cell(currentRow, 1).Value = "Date";
                worksheet.Cell(currentRow, 2).Value = "Description";
                worksheet.Cell(currentRow, 3).Value = "Debit";
                worksheet.Cell(currentRow, 4).Value = "Credit";
                worksheet.Cell(currentRow, 5).Value = "Account";
                worksheet.Cell(currentRow, 6).Value = "Ref";

                foreach (var item in _oAccounts.LedgerMasterIPagedList_1)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.Date;
                    worksheet.Cell(currentRow, 2).Value = item.Description;
                    if (!string.IsNullOrEmpty(item.Debit) && !string.IsNullOrWhiteSpace(item.Debit))
                    {
                        worksheet.Cell(currentRow, 3).Value = Convert.ToDecimal(item.Debit);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 3).Value = item.Debit;
                    }

                    if (!string.IsNullOrEmpty(item.Credit) && !string.IsNullOrWhiteSpace(item.Credit))
                    {
                        worksheet.Cell(currentRow, 4).Value = Convert.ToDecimal(item.Credit);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 4).Value = item.Credit;
                    }
                    worksheet.Cell(currentRow, 5).Value = item.ACCOUNT;
                    worksheet.Cell(currentRow, 6).Value = item.trans_ref;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Ledger.xlsx");
                }
            }
        }




        [HttpGet]
        public ActionResult Expense_Direct_List(string StartDate, string EndDate, int? DR_accountID, string Chassis_No, int? page)
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

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "Expense_Direct_List";
            ViewBag.PageTitle = "Expense Direct List";


            ViewBag.DR_AccountsList = _oBasic.Select_Expense_Accounts_DAL(c_IDs).ToList();

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            DR_accountID = DR_accountID == null ? 0 : DR_accountID;

            Chassis_No = (String.IsNullOrEmpty(Chassis_No) || String.IsNullOrWhiteSpace(Chassis_No) ? "" : Chassis_No);

            ViewBag.StartDate_ExpenseDirectReport = StartDate;
            ViewBag.EndDate_ExpenseDirectReport = EndDate;
            ViewBag.DR_accountID_ExpenseDirectReport = DR_accountID;
            ViewBag.Chassis_No_ExpenseDirectReport = Chassis_No;

            _oReports.ExpenseDirectReport = _oReports.pa_Select_Expense__Direct_DAL(StartDate, EndDate, DR_accountID, Chassis_No, c_IDs).ToPagedList(page ?? 1, 10);
            _oReports.ExpenseReport_TTL_Direct = _oReports.pa_Select_Expense__Direct_TTL_DAL(StartDate, EndDate, DR_accountID, Chassis_No, c_IDs);


            return View(_oReports);
        }
        [HttpGet]
        public ActionResult ExpenseDirectReport_Search(string StartDate, string EndDate, int? DR_accountID, string Chassis_No, int? page)
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

            //if (StartDate == null && EndDate == null)
            //{
            //    DateTime date = DateTime.Now;
            //    var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            //    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            //    StartDate = firstDayOfMonth.ToString("dd-MMM-yyyy");
            //    EndDate = lastDayOfMonth.ToString("dd-MMM-yyyy");
            //}
            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            DR_accountID = DR_accountID == null ? 0 : DR_accountID;

            TempData["Start"] = StartDate;
            TempData["End"] = EndDate;
            Chassis_No = (String.IsNullOrEmpty(Chassis_No) || String.IsNullOrWhiteSpace(Chassis_No) ? "" : Chassis_No);


            ViewBag.StartDate_ExpenseDirectReport = StartDate;
            ViewBag.EndDate_ExpenseDirectReport = EndDate;
            ViewBag.DR_accountID_ExpenseDirectReport = DR_accountID;
            ViewBag.Chassis_No_ExpenseDirectReport = Chassis_No;

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "Expense_Direct_List";
            //---getting all customers
            //  ViewBag.AllCustomers = _oBasic.Get_CustomersMaster_DAL().ToList();

            _oReports.ExpenseDirectReport = _oReports.pa_Select_Expense__Direct_DAL(StartDate, EndDate, DR_accountID, Chassis_No, c_IDs).ToPagedList(page ?? 1, 10);
            _oReports.ExpenseReport_TTL_Direct = _oReports.pa_Select_Expense__Direct_TTL_DAL(StartDate, EndDate, DR_accountID, Chassis_No, c_IDs);

            return PartialView("_Expense_Direct_List", _oReports);

        }
        [HttpGet]
        public ActionResult Expense(string StartDate, string EndDate, int? DR_accountID, string Chassis_No, int? page)
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

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "Expense";
            ViewBag.PageTitle = "Expense";


            ViewBag.DR_AccountsList = _oBasic.Select_Expense_Accounts_DAL(c_IDs).ToList();

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            DR_accountID = DR_accountID == null ? 0 : DR_accountID;

            Chassis_No = (String.IsNullOrEmpty(Chassis_No) || String.IsNullOrWhiteSpace(Chassis_No) ? "" : Chassis_No);


            ViewBag.StartDate_ExpenseReport = StartDate;
            ViewBag.EndDate_ExpenseReport = EndDate;
            ViewBag.DR_accountID_ExpenseReport = DR_accountID;
            ViewBag.Chassis_No_ExpenseReport = Chassis_No;

            _oReports.ExpenseLedger = _oReports.pa_Select_Expense__InDirect_DAL(StartDate, EndDate, DR_accountID, Chassis_No, c_IDs).ToPagedList(page ?? 1, 10);
            _oReports.ExpenseReport_TTL = _oReports.pa_Select_Expense__InDirect_TTL_DAL(StartDate, EndDate, DR_accountID, Chassis_No, c_IDs);
            // _oReports.ExpenseLedger = _oReports.pa_Select_Expense__InDirect_DAL(StartDate, EndDate, DR_accountID, Chassis_No, c_IDs).ToPagedList(page ?? 1, 10);


            return View(_oReports);
        }

        [HttpGet]
        public ActionResult ExpenseReport_Search(string StartDate, string EndDate, int? DR_accountID, string Chassis_No, int? page)
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

            //if (StartDate == null && EndDate == null)
            //{
            //    DateTime date = DateTime.Now;
            //    var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            //    var lastDayOfMonthV
            //    StartDate = firstDayOfMonth.ToString("dd-MMM-yyyy");
            //    EndDate = lastDayOfMonth.ToString("dd-MMM-yyyy");
            //}

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            DR_accountID = DR_accountID == null ? 0 : DR_accountID;

            Chassis_No = (String.IsNullOrEmpty(Chassis_No) || String.IsNullOrWhiteSpace(Chassis_No) ? "" : Chassis_No);


            ViewBag.StartDate_ExpenseReport = StartDate;
            ViewBag.EndDate_ExpenseReport = EndDate;
            ViewBag.DR_accountID_ExpenseReport = DR_accountID;
            ViewBag.Chassis_No_ExpenseReport = Chassis_No;

            //For Reporting Purpuse
            TempData["StartIn"] = StartDate;
            TempData["EndIn"] = EndDate;


            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "Expense";
            //---getting all customers
            // ViewBag.AllCustomers = _oBasic.Get_CustomersMaster_DAL().ToList();

            _oReports.ExpenseLedger = _oReports.pa_Select_Expense__InDirect_DAL(StartDate, EndDate, DR_accountID, Chassis_No, c_IDs).ToPagedList(page ?? 1, 10);
            _oReports.ExpenseReport_TTL = _oReports.pa_Select_Expense__InDirect_TTL_DAL(StartDate, EndDate, DR_accountID, Chassis_No, c_IDs);
            return PartialView("_Expense", _oReports);

        }
        [HttpGet]
        public IActionResult ExpenseReport_Print(string StartDate, string EndDate, int? DR_accountID, string Chassis_No)
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

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            DR_accountID = DR_accountID == null ? 0 : DR_accountID;

            Chassis_No = (String.IsNullOrEmpty(Chassis_No) || String.IsNullOrWhiteSpace(Chassis_No) ? "" : Chassis_No);

            _oReports.ExpenseReport = _oReports.pa_Select_Expense__InDirect_DAL(StartDate, EndDate, DR_accountID, Chassis_No, c_IDs).ToList();
            _oReports.ExpenseReport_TTL = _oReports.pa_Select_Expense__InDirect_TTL_DAL(StartDate, EndDate, DR_accountID, Chassis_No, c_IDs);


            string footer = "--footer-center \"  Created Date: " +
DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Page: [page]/[toPage]\"" +
" --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\"";

            var demoViewPortrait = new ViewAsPdf(_oReports)
            {

                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 20 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,

                CustomSwitches = footer

            };
            return demoViewPortrait;

        }


        [HttpGet]
        public IActionResult ExpenseReport_Excel(string StartDate, string EndDate, int? DR_accountID, string Chassis_No)
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

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            DR_accountID = DR_accountID == null ? 0 : DR_accountID;

            Chassis_No = (String.IsNullOrEmpty(Chassis_No) || String.IsNullOrWhiteSpace(Chassis_No) ? "" : Chassis_No);

            _oReports.ExpenseReport = _oReports.pa_Select_Expense__InDirect_DAL(StartDate, EndDate, DR_accountID, Chassis_No, c_IDs).ToList();
            _oReports.ExpenseReport_TTL = _oReports.pa_Select_Expense__InDirect_TTL_DAL(StartDate, EndDate, DR_accountID, Chassis_No, c_IDs);


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ExpenseReport");
                var currentRow = 1;


                worksheet.Cell(currentRow, 1).Value = "ExpenseAccount";
                worksheet.Cell(currentRow, 2).Value = "PayVia";
                worksheet.Cell(currentRow, 3).Value = "stock_ID";
                worksheet.Cell(currentRow, 4).Value = "Description";
                worksheet.Cell(currentRow, 5).Value = "PaymentDate";
                worksheet.Cell(currentRow, 6).Value = "Currency_Rate";
                worksheet.Cell(currentRow, 7).Value = "Amount";
                worksheet.Cell(currentRow, 8).Value = "VAT_Exp";
                worksheet.Cell(currentRow, 9).Value = "Total_Amount";

                foreach (var item in _oReports.ExpenseReport)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.ExpenseAccount;
                    worksheet.Cell(currentRow, 2).Value = item.PayVia;
                    worksheet.Cell(currentRow, 3).Value = item.stock_ID;
                    worksheet.Cell(currentRow, 4).Value = item.Description;
                    worksheet.Cell(currentRow, 5).Value = item.PaymentDate;
                    worksheet.Cell(currentRow, 6).Value = item.Currency_Rate;

                    if (!string.IsNullOrEmpty(item.Amount) && !string.IsNullOrWhiteSpace(item.Amount))
                    {
                        worksheet.Cell(currentRow, 7).Value = Convert.ToDecimal(item.Amount);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 7).Value = item.Amount;
                    }

                    worksheet.Cell(currentRow, 8).Value = item.VAT_Exp;

                    if (!string.IsNullOrEmpty(item.Total_Amount) && !string.IsNullOrWhiteSpace(item.Total_Amount))
                    {
                        worksheet.Cell(currentRow, 9).Value = Convert.ToDecimal(item.Total_Amount);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 9).Value = item.Total_Amount;
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "ExpenseReport.xlsx");
                }
            }

        }









        [HttpGet]
        public IActionResult ExpenseReport_Direct_Print(string StartDate, string EndDate, int? DR_accountID, string Chassis_No)
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

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            DR_accountID = DR_accountID == null ? 0 : DR_accountID;

            Chassis_No = (String.IsNullOrEmpty(Chassis_No) || String.IsNullOrWhiteSpace(Chassis_No) ? "" : Chassis_No);



            _oReports.ExpenseReport_Direct = _oReports.pa_Select_Expense__Direct_DAL(StartDate, EndDate, DR_accountID, Chassis_No, c_IDs).ToList();
            _oReports.ExpenseReport_TTL_Direct = _oReports.pa_Select_Expense__Direct_TTL_DAL(StartDate, EndDate, DR_accountID, Chassis_No, c_IDs);


            string footer = "--footer-center \"  Created Date: " +
DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Page: [page]/[toPage]\"" +
" --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\"";

            var demoViewPortrait = new ViewAsPdf(_oReports)
            {

                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 20 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,

                CustomSwitches = footer

            };
            return demoViewPortrait;

        }
        [HttpGet]
        public IActionResult ExpenseReport_Direct_Excel(string StartDate, string EndDate, int? DR_accountID, string Chassis_No)
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

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            DR_accountID = DR_accountID == null ? 0 : DR_accountID;

            Chassis_No = (String.IsNullOrEmpty(Chassis_No) || String.IsNullOrWhiteSpace(Chassis_No) ? "" : Chassis_No);



            _oReports.ExpenseReport_Direct = _oReports.pa_Select_Expense__Direct_DAL(StartDate, EndDate, DR_accountID, Chassis_No, c_IDs).ToList();
            _oReports.ExpenseReport_TTL_Direct = _oReports.pa_Select_Expense__Direct_TTL_DAL(StartDate, EndDate, DR_accountID, Chassis_No, c_IDs);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Expense_Direct_List");
                var currentRow = 1;


                worksheet.Cell(currentRow, 1).Value = "ExpenseAccount";
                worksheet.Cell(currentRow, 2).Value = "PayVia";
                worksheet.Cell(currentRow, 3).Value = "stock_ID";
                worksheet.Cell(currentRow, 4).Value = "Chassis_No";
                worksheet.Cell(currentRow, 5).Value = "PaymentDate";
                worksheet.Cell(currentRow, 6).Value = "Currency_Rate";
                worksheet.Cell(currentRow, 7).Value = "Amount";
                worksheet.Cell(currentRow, 8).Value = "VAT_Exp";
                worksheet.Cell(currentRow, 9).Value = "Total_Amount";

                foreach (var item in _oReports.ExpenseReport_Direct)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.ExpenseAccount;
                    worksheet.Cell(currentRow, 2).Value = item.PayVia;
                    worksheet.Cell(currentRow, 3).Value = item.stock_ID;
                    worksheet.Cell(currentRow, 4).Value = item.Chassis_No;
                    worksheet.Cell(currentRow, 5).Value = item.PaymentDate;
                    worksheet.Cell(currentRow, 6).Value = item.Currency_Rate;



                    if (!string.IsNullOrEmpty(item.Amount) && !string.IsNullOrWhiteSpace(item.Amount))
                    {
                        worksheet.Cell(currentRow, 7).Value = Convert.ToDecimal(item.Amount);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 7).Value = item.Amount;
                    }

                    worksheet.Cell(currentRow, 8).Value = item.VAT_Exp;


                    if (!string.IsNullOrEmpty(item.Total_Amount) && !string.IsNullOrWhiteSpace(item.Total_Amount))
                    {
                        worksheet.Cell(currentRow, 9).Value = Convert.ToDecimal(item.Total_Amount);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 9).Value = item.Total_Amount;
                    }


                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Expense_Direct_List.xlsx");
                }
            }
        }












        [HttpGet]
        public IActionResult DVReport_Print(int? DV_ID)
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

            _oReports.DepositVoucher_Print = _oReports.GetDataForDpositMaster_Print(DV_ID);
            _oReports.DepositVoucherCheque_Print = _oReports.GetDataForDpositMasterCheque_Print(DV_ID);



            string Host = Request.Host.ToString();


            string customSwitches = string.Format("--header-spacing \"0\" --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/HeaderDepositVoucher?DV_ID=" + DV_ID + "&c_ID=" + c_ID,

              "http://" + Host + "/Report/FooterDepositVoucher?DV_ID=" + DV_ID + "&c_ID=" + c_ID);

            //return View(_oReports);
            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 90, Right = 5, Top = 50 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches + "--footer-left \"    Page: [page]-[toPage]\"" +
 "  --footer-font-size \"10\"  --footer-font-name \"Segoe UI\""




            };
            return demoViewPortrait;

        }

        [HttpGet]
        public IActionResult DepositReturn(int? DV_ID)
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

            _oReports.DepositVoucher_Print = _oReports.GetDataForDpositMaster_Print(DV_ID);
            _oReports.DepositVoucherCheque_Print = _oReports.GetDataForDpositMasterCheque_Print(DV_ID);




            string Host = Request.Host.ToString();


            string customSwitches = string.Format("--header-spacing \"0\" --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/HeaderDepositReturn?c_ID=" + c_ID,

              "http://" + Host + "/Report/FooterDepositReturnVoucher?c_ID=" + c_ID);

            //return View(_oReports);
            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 75, Right = 5, Top = 30 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;

        }
        [AllowAnonymous]
        public IActionResult HeaderDepositVoucher(int DV_ID, int? c_ID)
        {
            ViewBag.ReportCompanyDep = c_ID;
            _oReports.DepositVoucher_Print = _oReports.GetDataForDpositMaster_Print(DV_ID);
            _oReports.DepositVoucherCheque_Print = _oReports.GetDataForDpositMasterCheque_Print(DV_ID);
            return View("~/Views/Report/" + ReportFolder + "/HeaderDepositVoucher.cshtml", _oReports);
        }
        [AllowAnonymous]
        public IActionResult HeaderDepositReturn(int? c_ID)
        {
            ViewBag.ReportCompanyDep = c_ID;

            return View("~/Views/Report/" + ReportFolder + "/HeaderDepositReturn.cshtml");
        }
        [AllowAnonymous]
        public IActionResult FooterDepositVoucher(int DV_ID, int? c_ID)
        {
            ViewBag.ReportCompanyDep_f = c_ID;
            _oReports.DepositVoucher_Print = _oReports.GetDataForDpositMaster_Print(DV_ID);
            return View("~/Views/Report/" + ReportFolder + "/FooterDepositVoucher.cshtml", _oReports);
        }
        [AllowAnonymous]
        public IActionResult FooterDepositReturnVoucher(int? c_ID)
        {
            ViewBag.ReportCompanyDepRep_f = c_ID;

            return View("~/Views/Report/" + ReportFolder + "/FooterDepositReturnVoucher.cshtml");
        }
        [HttpGet]
        public IActionResult Performa_Print(int? SaleMaster_ID)
        {




            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion

            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.PerformaPrintObj = _oSales.Get_PerformaInvoiceDetailPrintByID_DAL(SaleMaster_ID);
            _oSales.PerformaPrintObj_Specs = _oSales.Get_PerformaInvoiceDetailPrintByID_Specs_DAL(SaleMaster_ID);
            string TotalAmount = "";
            if (_oSales.PerformaPrintObj.Count() > 0)
            {
                TotalAmount = _oSales.Get_PerformaInvoiceDetailPrintByID_DAL(SaleMaster_ID).Select(x => x.Total_Amt_inWords).LastOrDefault();

            }
            if (_oSales.PerformaPrintObj_Specs.Count() > 0)
            {
                TotalAmount = _oSales.Get_PerformaInvoiceDetailPrintByID_Specs_DAL(SaleMaster_ID).Select(x => x.Total_Amt_inWords).LastOrDefault();
            }


            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0.00";
            }
            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());

            if (_oSales.PerformaPrintObj.Count() > 0)
            {
                _oSales.PerformaPrintObj.LastOrDefault().Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.PerformaPrintObj.LastOrDefault().Currency_ShortName
+ (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.PerformaPrintObj.LastOrDefault().Minor_ShortName + " only" : " only");

            }
            if (_oSales.PerformaPrintObj_Specs.Count() > 0)
            {
                _oSales.PerformaPrintObj_Specs.LastOrDefault().Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.PerformaPrintObj_Specs.LastOrDefault().Currency_ShortName
+ (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.PerformaPrintObj_Specs.LastOrDefault().Minor_ShortName + " only" : " only");

            }


            string Host = Request.Host.ToString();


            string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/PrintHeaderInvoicePerformaInv?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID,

            "http://" + Host + "/Report/FooterHeaderInvoicePerformaInv?c_ID=" + c_ID);


            var demoViewPortrait = new ViewAsPdf(_oSales)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 48, Right = 5, Top = 80 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;


        }

        [HttpGet]
        public IActionResult Performa_Print2(int? SaleMaster_ID)
        {




            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion

            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.PerformaPrintObj = _oSales.Get_PerformaInvoiceDetailPrintByID_DAL(SaleMaster_ID);
            _oSales.PerformaPrintObj_Specs = _oSales.Get_PerformaInvoiceDetailPrintByID_Specs_DAL(SaleMaster_ID);
            string TotalAmount = "";
            if (_oSales.PerformaPrintObj.Count() > 0)
            {
                TotalAmount = _oSales.Get_PerformaInvoiceDetailPrintByID_DAL(SaleMaster_ID).Select(x => x.Total_Amt_inWords).LastOrDefault();

            }
            if (_oSales.PerformaPrintObj_Specs.Count() > 0)
            {
                TotalAmount = _oSales.Get_PerformaInvoiceDetailPrintByID_Specs_DAL(SaleMaster_ID).Select(x => x.Total_Amt_inWords).LastOrDefault();
            }


            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0.00";
            }
            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());

            if (_oSales.PerformaPrintObj.Count() > 0)
            {
                _oSales.PerformaPrintObj.LastOrDefault().Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.PerformaPrintObj.LastOrDefault().Currency_ShortName
+ (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.PerformaPrintObj.LastOrDefault().Minor_ShortName + " only" : " only");

            }
            if (_oSales.PerformaPrintObj_Specs.Count() > 0)
            {
                _oSales.PerformaPrintObj_Specs.LastOrDefault().Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.PerformaPrintObj_Specs.LastOrDefault().Currency_ShortName
+ (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.PerformaPrintObj_Specs.LastOrDefault().Minor_ShortName + " only" : " only");

            }


            string Host = Request.Host.ToString();


            string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/PrintHeaderInvoicePerformaInv?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID,

            "http://" + Host + "/Report/FooterHeaderInvoicePerformaInv?c_ID=" + c_ID);


            var demoViewPortrait = new ViewAsPdf(_oSales)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 48, Right = 5, Top = 80 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;


        }

        [HttpGet]
        public IActionResult Performa_Print3(int? SaleMaster_ID)
        {




            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion

            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.PerformaPrintObj = _oSales.Get_PerformaInvoiceDetailPrintByID_DAL(SaleMaster_ID);
            _oSales.PerformaPrintObj_Specs = _oSales.Get_PerformaInvoiceDetailPrintByID_Specs_DAL(SaleMaster_ID);
            string TotalAmount = "";
            if (_oSales.PerformaPrintObj.Count() > 0)
            {
                TotalAmount = _oSales.Get_PerformaInvoiceDetailPrintByID_DAL(SaleMaster_ID).Select(x => x.Total_Amt_inWords).LastOrDefault();

            }
            if (_oSales.PerformaPrintObj_Specs.Count() > 0)
            {
                TotalAmount = _oSales.Get_PerformaInvoiceDetailPrintByID_Specs_DAL(SaleMaster_ID).Select(x => x.Total_Amt_inWords).LastOrDefault();
            }


            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0.00";
            }
            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());

            if (_oSales.PerformaPrintObj.Count() > 0)
            {
                _oSales.PerformaPrintObj.LastOrDefault().Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.PerformaPrintObj.LastOrDefault().Currency_ShortName
+ (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.PerformaPrintObj.LastOrDefault().Minor_ShortName + " only" : " only");

            }
            if (_oSales.PerformaPrintObj_Specs.Count() > 0)
            {
                _oSales.PerformaPrintObj_Specs.LastOrDefault().Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.PerformaPrintObj_Specs.LastOrDefault().Currency_ShortName
+ (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.PerformaPrintObj_Specs.LastOrDefault().Minor_ShortName + " only" : " only");

            }


            string Host = Request.Host.ToString();


            //string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ");

            //"http://" + Host + "/Report/PrintHeaderInvoicePerformaInv?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID,

            //"http://" + Host + "/Report/FooterHeaderInvoicePerformaInv?c_ID=" + c_ID);


            var demoViewPortrait = new ViewAsPdf(_oSales)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 48, Right = 5, Top = 50 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
              //  CustomSwitches = customSwitches




            };
            return demoViewPortrait;


        }


        [HttpGet]
        public IActionResult Performa_PrintGLM(int? SaleMaster_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion

            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.PerformaPrintObj = _oSales.Get_PerformaInvoiceDetailPrintByID_DAL(SaleMaster_ID);
            _oSales.PerformaPrintObj_Specs = _oSales.Get_PerformaInvoiceDetailPrintByID_Specs_DAL(SaleMaster_ID);
            string TotalAmount = "";
            if (_oSales.PerformaPrintObj.Count() > 0)
            {
                TotalAmount = _oSales.Get_PerformaInvoiceDetailPrintByID_DAL(SaleMaster_ID).Select(x => x.Total_Amt_inWords).LastOrDefault();

            }
            if (_oSales.PerformaPrintObj_Specs.Count() > 0)
            {
                TotalAmount = _oSales.Get_PerformaInvoiceDetailPrintByID_Specs_DAL(SaleMaster_ID).Select(x => x.Total_Amt_inWords).LastOrDefault();
            }


            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0.00";
            }
            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());

            if (_oSales.PerformaPrintObj.Count() > 0)
            {
                _oSales.PerformaPrintObj.LastOrDefault().Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.PerformaPrintObj.LastOrDefault().Currency_ShortName
+ (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.PerformaPrintObj.LastOrDefault().Minor_ShortName + " only" : " only");

            }
            if (_oSales.PerformaPrintObj_Specs.Count() > 0)
            {
                _oSales.PerformaPrintObj_Specs.LastOrDefault().Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.PerformaPrintObj_Specs.LastOrDefault().Currency_ShortName
+ (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.PerformaPrintObj_Specs.LastOrDefault().Minor_ShortName + " only" : " only");

            }


            string Host = Request.Host.ToString();


            string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/PrintHeaderInvoicePerformaInv?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID,

            "http://" + Host + "/Report/FooterHeaderInvoicePerformaInvGLM?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID);


            var demoViewPortrait = new ViewAsPdf(_oSales)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 75, Right = 5, Top = 70 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;


        }
        [HttpGet]
        public IActionResult Performa_PrintGLM2(int? SaleMaster_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion

            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.PerformaPrintObj = _oSales.Get_PerformaInvoiceDetailPrintByID_DAL(SaleMaster_ID);
            _oSales.PerformaPrintObj_Specs = _oSales.Get_PerformaInvoiceDetailPrintByID_Specs_DAL(SaleMaster_ID);
            string TotalAmount = "";
            if (_oSales.PerformaPrintObj.Count() > 0)
            {
                TotalAmount = _oSales.Get_PerformaInvoiceDetailPrintByID_DAL(SaleMaster_ID).Select(x => x.Total_Amt_inWords).LastOrDefault();

            }
            if (_oSales.PerformaPrintObj_Specs.Count() > 0)
            {
                TotalAmount = _oSales.Get_PerformaInvoiceDetailPrintByID_Specs_DAL(SaleMaster_ID).Select(x => x.Total_Amt_inWords).LastOrDefault();
            }


            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0.00";
            }
            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());

            if (_oSales.PerformaPrintObj.Count() > 0)
            {
                _oSales.PerformaPrintObj.LastOrDefault().Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.PerformaPrintObj.LastOrDefault().Currency_ShortName
+ (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.PerformaPrintObj.LastOrDefault().Minor_ShortName + " only" : " only");

            }
            if (_oSales.PerformaPrintObj_Specs.Count() > 0)
            {
                _oSales.PerformaPrintObj_Specs.LastOrDefault().Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.PerformaPrintObj_Specs.LastOrDefault().Currency_ShortName
+ (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.PerformaPrintObj_Specs.LastOrDefault().Minor_ShortName + " only" : " only");

            }


            string Host = Request.Host.ToString();


            string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/PrintHeaderInvoicePerformaInv?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID,

            "http://" + Host + "/Report/FooterHeaderInvoicePerformaInvGLM?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID);


            var demoViewPortrait = new ViewAsPdf(_oSales)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 75, Right = 5, Top = 70 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;


        }


        [AllowAnonymous]
        public IActionResult PrintHeaderInvoicePerformaInv(int? c_ID, int? SaleMaster_ID)
        {
            ViewBag.ReportCompanyPerformaInv = c_ID;
            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.PerformaPrintObj_Specs = _oSales.Get_PerformaInvoiceDetailPrintByID_Specs_DAL(SaleMaster_ID);
            _oSales.PerformaPrintObj = _oSales.Get_PerformaInvoiceDetailPrintByID_DAL(SaleMaster_ID);
            return View("~/Views/Report/" + ReportFolder + "/PrintHeaderInvoicePerformaInv.cshtml", _oSales);
        }
        [AllowAnonymous]
        public IActionResult FooterHeaderInvoicePerformaInv(int? c_ID)
        {
            ViewBag.ReportCompanyPer_F = c_ID;

            return View("~/Views/Report/" + ReportFolder + "/FooterHeaderInvoicePerformaInv.cshtml");
        }
        [AllowAnonymous]
        public IActionResult FooterHeaderInvoicePerformaInvGLM(int? c_ID, int? SaleMaster_ID)
        {
            ViewBag.ReportCompanyPer_F = c_ID;
            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.PerformaPrintObj_Specs = _oSales.Get_PerformaInvoiceDetailPrintByID_Specs_DAL(SaleMaster_ID);
            _oSales.PerformaPrintObj = _oSales.Get_PerformaInvoiceDetailPrintByID_DAL(SaleMaster_ID);
            return View("~/Views/Report/" + ReportFolder + "/FooterHeaderInvoicePerformaInvGLM.cshtml", _oSales);
        }


        [HttpGet]
        public IActionResult DeliveryNoteReport_Print(int? DeliveryMaster_ID)
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

            _oSales.DeliveryReport_Print = _oSales.Get_DeliveryNotePrintByID_DAL(DeliveryMaster_ID);




            string Host = Request.Host.ToString();


            string customSwitches = string.Format("--header-spacing \"0\" --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/HeaderDeliveryNote?DeliveryMaster_ID=" + DeliveryMaster_ID + "&c_ID=" + c_ID,

              "http://" + Host + "/Report/FooterDeliveryNote?DeliveryMaster_ID=" + DeliveryMaster_ID + "&c_ID=" + c_ID);

            //return View(_oReports);
            var demoViewPortrait = new ViewAsPdf(_oSales)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 130, Right = 5, Top = 40 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches + "--footer-right \"    Page: [page]-[toPage]\"" +
 "  --footer-font-size \"10\"  --footer-font-name \"Segoe UI\""




            };
            return demoViewPortrait;

        }



        [AllowAnonymous]
        public IActionResult HeaderDeliveryNote(int DeliveryMaster_ID, int? c_ID)
        {
            ViewBag.ReportCompanyDep = c_ID;
            _oSales.DeliveryReport_Print = _oSales.Get_DeliveryNotePrintByID_DAL(DeliveryMaster_ID);
            return View("~/Views/Report/" + ReportFolder + "/HeaderDeliveryNote.cshtml", _oSales);
        }


        [AllowAnonymous]
        public IActionResult FooterDeliveryNote(int DeliveryMaster_ID, int? c_ID)
        {
            ViewBag.ReportCompanyDep_f = c_ID;
            _oSales.DeliveryReport_Print = _oSales.Get_DeliveryNotePrintByID_DAL(DeliveryMaster_ID);
            return View("~/Views/Report/" + ReportFolder + "/FooterDeliveryNote.cshtml", _oSales);
        }




        //Faraz New Work


        [HttpGet]
        public IActionResult ProductionList_Print(string Ref, string StartDate, string EndDate,
   int c_ID)
        {


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
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;





            _oInventory.ProductionMasterList1 = _oInventory.Get_ProductionMaster_DAL(Ref, StartDate, EndDate, c_ID).ToList();

            var demoViewPortrait = new ViewAsPdf(_oInventory)
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
        [HttpGet]
        public IActionResult ProductionList_Excel(string Ref, string StartDate, string EndDate,
  int c_ID)
        {


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
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;



            _oInventory.ProductionMasterList1 = _oInventory.Get_ProductionMaster_DAL(Ref, StartDate, EndDate, c_ID).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ProductionList");
                var currentRow = 1;




                worksheet.Cell(currentRow, 1).Value = "Ref";
                worksheet.Cell(currentRow, 2).Value = "P_Date";
                worksheet.Cell(currentRow, 3).Value = "Customer Ref";
                worksheet.Cell(currentRow, 4).Value = "Details";
                worksheet.Cell(currentRow, 5).Value = "Supervisor";


                foreach (var item in _oInventory.ProductionMasterList1)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.Ref;
                    worksheet.Cell(currentRow, 2).Value = item.P_Date;
                    worksheet.Cell(currentRow, 3).Value = item.CustomerRef;
                    worksheet.Cell(currentRow, 4).Value = item.Production_Details;
                    worksheet.Cell(currentRow, 5).Value = item.Supervisor;

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        " ProductionList.xlsx");
                }
            }


        }


        [HttpGet]
        public IActionResult ProductFormulaList_print(string Ref, string StartDate, string EndDate,
    int c_ID)
        {



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
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;




            _oInventory.ProductFormulaMasterList1 = _oInventory.Get_ProductFormulaMasterInvoiceList_DAL(Ref, StartDate, EndDate, c_ID).ToList();


            var demoViewPortrait = new ViewAsPdf(_oInventory)
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
        [HttpGet]
        public IActionResult ProductFormulaList_Excel(string Ref, string StartDate, string EndDate,
    int c_ID)
        {





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
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;




            _oInventory.ProductFormulaMasterList1 = _oInventory.Get_ProductFormulaMasterInvoiceList_DAL(Ref, StartDate, EndDate, c_ID).ToList();


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ProductFormulaList");
                var currentRow = 1;





                worksheet.Cell(currentRow, 1).Value = "Ref";
                worksheet.Cell(currentRow, 2).Value = "P_Date";
                worksheet.Cell(currentRow, 3).Value = "FormulaName";
                worksheet.Cell(currentRow, 4).Value = "ItemID";
                worksheet.Cell(currentRow, 5).Value = "Created_At";


                foreach (var item in _oInventory.ProductFormulaMasterList1)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.Ref;
                    worksheet.Cell(currentRow, 2).Value = item.P_Date;
                    worksheet.Cell(currentRow, 3).Value = item.FormulaName;
                    worksheet.Cell(currentRow, 4).Value = item.ItemID;
                    worksheet.Cell(currentRow, 5).Value = item.Created_At;

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "ProductFormulaList.xlsx");
                }
            }



        }


        [HttpGet]
        public IActionResult SalesInvoiceList_TRD_Print(string SaleRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string ItemId, string MNumber, int c_ID, string OrderRef, string OrderStatus)
        {


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
            MNumber = String.IsNullOrEmpty(MNumber) ? "" : MNumber;
            OrderRef = string.IsNullOrEmpty(OrderRef) ? "" : OrderRef;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;
            Status_ID = Status_ID == 0 ? 0 : Status_ID;



            _oSales.SalesMasterIPagedListnew = _oSales.Get_SalesMasterInvoiceList_DAL_TRD(SaleRef, StartDate, EndDate, Customer_Name, ItemId, MNumber, Status_ID, c_ID, OrderRef, OrderStatus);
            _oSales.SalesMasterListTOTALnew = _oSales.Get_SalesMasterInvoiceList_TTL_DAL_TRD(SaleRef, StartDate, EndDate, Customer_Name, ItemId, MNumber, Status_ID, c_ID, OrderRef,OrderStatus).ToList();

            var demoViewPortrait = new ViewAsPdf(_oSales)
            {

                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
           "--footer-center \"  Printed At: " +
         DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Page: [page]-[toPage]\"" +
         " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\""

            };
            return demoViewPortrait;


        }

        [HttpGet]
        public IActionResult SalesInvoiceList_TRD_Excel(string SaleRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string ItemId, string MNumber, int c_ID, string OrderRef, string OrderStatus)
        {


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
            MNumber = String.IsNullOrEmpty(MNumber) ? "" : MNumber;
            OrderRef = string.IsNullOrEmpty(OrderRef) ? "" : OrderRef;
            Customer_Name = String.IsNullOrEmpty(Customer_Name) ? "" : Customer_Name;
            Status_ID = Status_ID == 0 ? 0 : Status_ID;
            OrderRef = string.IsNullOrEmpty(OrderRef) ? "" : OrderRef;


            _oSales.SalesMasterIPagedListnew = _oSales.Get_SalesMasterInvoiceList_DAL_TRD(SaleRef, StartDate, EndDate, Customer_Name, ItemId, MNumber, Status_ID, c_ID, OrderRef, OrderStatus);
            _oSales.SalesMasterListTOTALnew = _oSales.Get_SalesMasterInvoiceList_TTL_DAL_TRD(SaleRef, StartDate, EndDate, Customer_Name, ItemId, MNumber, Status_ID, c_ID, OrderRef,OrderStatus).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("SalesInvoiceList_TRD");
                var currentRow = 1;



                worksheet.Cell(currentRow, 1).Value = "Sale Ref";
                worksheet.Cell(currentRow, 2).Value = "Sale Date";
                worksheet.Cell(currentRow, 3).Value = "Customer Name";
                worksheet.Cell(currentRow, 4).Value = "Total Amount";
                worksheet.Cell(currentRow, 5).Value = "Paid";
                worksheet.Cell(currentRow, 6).Value = "Bal Due";
                worksheet.Cell(currentRow, 7).Value = " Status";


                foreach (var item in _oSales.SalesMasterIPagedListnew)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.SaleRef;
                    worksheet.Cell(currentRow, 2).Value = item.SaleDate;
                    worksheet.Cell(currentRow, 3).Value = item.Customer_Name;
                    worksheet.Cell(currentRow, 4).Value = item.Total_Amt;
                    worksheet.Cell(currentRow, 5).Value = item.Paid_amt;
                    worksheet.Cell(currentRow, 6).Value = item.Bal_due;
                    worksheet.Cell(currentRow, 7).Value = item.SaleStatus;

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "SalesInvoiceList_TRD.xlsx");
                }
            }



        }





        //faraz japan work

        [HttpGet]
        public IActionResult VanningList_Print(string txtVanningRef, string StartDate, string EndDate, string Party_ID_Name)
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


            _oStock.VanningListPagedObject1 = _oStock.get_Vanning_List(txtVanningRef, StartDate, EndDate, Party_ID_Name);
            _oStock.VanningListTotal = _oStock.get_Vanning_List_Total(txtVanningRef, StartDate, EndDate, Party_ID_Name).ToList();

            var demoViewPortrait = new ViewAsPdf(_oStock)
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


        [HttpGet]
        public IActionResult VanningList_Excel(string txtVanningRef, string StartDate, string EndDate, string Party_ID_Name)
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


            _oStock.VanningListPagedObject1 = _oStock.get_Vanning_List(txtVanningRef, StartDate, EndDate, Party_ID_Name);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("VanningList");
                var currentRow = 1;






                worksheet.Cell(currentRow, 1).Value = "Ref";
                worksheet.Cell(currentRow, 2).Value = "Date";
                worksheet.Cell(currentRow, 3).Value = "Vendor Name";
                worksheet.Cell(currentRow, 4).Value = "Comments";
                worksheet.Cell(currentRow, 5).Value = "Total Amount";
                worksheet.Cell(currentRow, 6).Value = "Paid";
                worksheet.Cell(currentRow, 7).Value = "Balance";
                worksheet.Cell(currentRow, 8).Value = " Status";


                foreach (var item in _oStock.VanningListPagedObject1)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.vanning_ref;
                    worksheet.Cell(currentRow, 2).Value = item.Date;
                    worksheet.Cell(currentRow, 3).Value = item.Vendor_Name;
                    worksheet.Cell(currentRow, 4).Value = item.Comments;

                    if (!string.IsNullOrEmpty(item.Total_Amount) && !string.IsNullOrWhiteSpace(item.Total_Amount))
                    {
                        worksheet.Cell(currentRow, 5).Value = Convert.ToDecimal(item.Total_Amount);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 5).Value = item.Total_Amount;
                    }

                    if (!string.IsNullOrEmpty(item.Paid) && !string.IsNullOrWhiteSpace(item.Paid))
                    {
                        worksheet.Cell(currentRow, 6).Value = Convert.ToDecimal(item.Paid);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 6).Value = item.Paid;
                    }

                    worksheet.Cell(currentRow, 7).Value = item.Balance;
                    worksheet.Cell(currentRow, 8).Value = item.Purchased_Status;

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "VanningList.xlsx");
                }
            }

        }





        [HttpGet]
        public IActionResult Shipping_infoList_print(string trans_ref, string StartDate, string EndDate, string Shipper_Name)
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

            _oStock.Shipping_infoListPagedObject1 = _oStock.get_shipping_info_List(trans_ref, StartDate, EndDate, Shipper_Name).ToList();
            _oStock.Shipping_infoListTotal = _oStock.get_shipping_info_List_Total(trans_ref, StartDate, EndDate, Shipper_Name).ToList();


            var demoViewPortrait = new ViewAsPdf(_oStock)
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



        [HttpGet]
        public IActionResult Shipping_infoList_Excel(string trans_ref, string StartDate, string EndDate, string Shipper_Name)
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
            #region Veiwbags area
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_ShippingCompaniesList_DAL(c_ID).Where(x => x.VendorCat_ID == 6).ToList(); // Shipping category is 6;
            //---Get PurchaseStatus
            ViewBag.PurchaseStatus = _oBasic.Get_StatusList_byType_DAL("Purchase").ToList();
            #endregion veiwbags area

            trans_ref = String.IsNullOrEmpty(trans_ref) ? "" : trans_ref;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            Shipper_Name = String.IsNullOrEmpty(Shipper_Name) ? "" : Shipper_Name;


            _oStock.Shipping_infoListPagedObject1 = _oStock.get_shipping_info_List(trans_ref, StartDate, EndDate, Shipper_Name).ToList();


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Shipping_infoList");
                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = " Trans Ref";
                worksheet.Cell(currentRow, 2).Value = " Shipper Name";
                worksheet.Cell(currentRow, 3).Value = "Invoice_Date";
                worksheet.Cell(currentRow, 4).Value = " Total";
                worksheet.Cell(currentRow, 5).Value = " Paid";
                worksheet.Cell(currentRow, 6).Value = "Balance ";
                worksheet.Cell(currentRow, 7).Value = "Status";


                foreach (var item in _oStock.Shipping_infoListPagedObject1)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.Shipping_Info_Ref;
                    worksheet.Cell(currentRow, 2).Value = item.Shipper_Name;
                    worksheet.Cell(currentRow, 3).Value = item.trans_Date;


                    if (!string.IsNullOrEmpty(item.Total_Amount) && !string.IsNullOrWhiteSpace(item.Total_Amount))
                    {
                        worksheet.Cell(currentRow, 4).Value = Convert.ToDecimal(item.Total_Amount);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 4).Value = item.Total_Amount;
                    }

                    if (!string.IsNullOrEmpty(item.Paid) && !string.IsNullOrWhiteSpace(item.Paid))
                    {
                        worksheet.Cell(currentRow, 5).Value = Convert.ToDecimal(item.Paid);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 5).Value = item.Paid;
                    }

                    worksheet.Cell(currentRow, 6).Value = item.Balance;
                    worksheet.Cell(currentRow, 7).Value = item.PaymentStatus;

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Shipping_infoList.xlsx");
                }
            }

        }






        [HttpGet]
        public IActionResult PurchaseListJP_print(string PurchaseRef, int Vendor_ID, string From_Date, string To_Date, int Status_ID, string ChassisNo, int c_ID)
        {



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
            Status_ID = Status_ID == 0 ? Status_ID : Status_ID;

            c_ID = c_ID == 0 ? c_IDs : c_ID;



            _oStock.PurchaseMasterList1 = _oStock.pa_Select_PurchaseMaster(PurchaseRef, From_Date, To_Date, Vendor_ID, Status_ID, ChassisNo, c_ID).ToList();
            _oStock.purchaseMasterTotal = _oStock.Get_PurchaseMasterList_Total(PurchaseRef, Vendor_ID, From_Date, To_Date, Status_ID, ChassisNo, c_ID).ToList();


            var demoViewPortrait = new ViewAsPdf(_oStock)
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
        [HttpGet]
        public IActionResult PurchaseListJP_Excel(string PurchaseRef, int Vendor_ID, string From_Date, string To_Date, int Status_ID, string ChassisNo, int c_ID)
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
            int c_IDs = Convert.ToInt32(company_ID);
            #endregion
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
            Status_ID = Status_ID == 0 ? Status_ID : Status_ID;

            c_ID = c_ID == 0 ? c_IDs : c_ID;



            _oStock.PurchaseMasterList1 = _oStock.pa_Select_PurchaseMaster(PurchaseRef, From_Date, To_Date, Vendor_ID, Status_ID, ChassisNo, c_ID).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("PurchaseListJP");
                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "Ref";
                worksheet.Cell(currentRow, 2).Value = "Date";
                worksheet.Cell(currentRow, 3).Value = "Vendor Name";
                worksheet.Cell(currentRow, 4).Value = "Total";
                worksheet.Cell(currentRow, 5).Value = "Paid";
                worksheet.Cell(currentRow, 6).Value = "Balance";


                foreach (var item in _oStock.PurchaseMasterList1)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.PurchaseRef;
                    worksheet.Cell(currentRow, 2).Value = item.PurchaseDate;
                    worksheet.Cell(currentRow, 3).Value = item.Vendor_Name;

                    if (!string.IsNullOrEmpty(item.Total_Amount) && !string.IsNullOrWhiteSpace(item.Total_Amount))
                    {
                        worksheet.Cell(currentRow, 4).Value = Convert.ToDecimal(item.Total_Amount);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 4).Value = item.Total_Amount;
                    }

                    if (!string.IsNullOrEmpty(item.Paid) && !string.IsNullOrWhiteSpace(item.Paid))
                    {
                        worksheet.Cell(currentRow, 5).Value = Convert.ToDecimal(item.Paid);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 5).Value = item.Paid;
                    }

                    worksheet.Cell(currentRow, 6).Value = item.Bal_Due;

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "PurchaseListJP.xlsx");
                }
            }

        }



        [HttpGet]
        public IActionResult reksolist_Print(string PurchaseRef, string StartDate, string EndDate, int RekSo_Vendor_ID)
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





            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).Where(x => x.VendorCat_ID == 4).ToList();
            ViewBag.PageTitle = "Rekso List Print";
            _oStock.ReksoList1 = _oStock.pa_select_StockParties(PurchaseRef, StartDate, EndDate, RekSo_Vendor_ID).ToList();
            _oStock.ReksoList_total = _oStock.pa_select_StockParties_total(PurchaseRef, StartDate, EndDate, RekSo_Vendor_ID).ToList();


            var demoViewPortrait = new ViewAsPdf(_oStock)
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

        [HttpGet]
        public IActionResult reksolist_Excel(string PurchaseRef, string StartDate, string EndDate, int RekSo_Vendor_ID)
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

            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).Where(x => x.VendorCat_ID == 4).ToList();

            _oStock.ReksoList1 = _oStock.pa_select_StockParties(PurchaseRef, StartDate, EndDate, RekSo_Vendor_ID).ToList();


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("reksolist");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Ref";
                worksheet.Cell(currentRow, 2).Value = "Date";
                worksheet.Cell(currentRow, 3).Value = " Vendor Name";
                worksheet.Cell(currentRow, 4).Value = "Total";
                worksheet.Cell(currentRow, 5).Value = "Paid";
                worksheet.Cell(currentRow, 6).Value = " Balance";
                worksheet.Cell(currentRow, 7).Value = "Status";


                foreach (var item in _oStock.ReksoList1)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.Ref;
                    worksheet.Cell(currentRow, 2).Value = item.Date;
                    worksheet.Cell(currentRow, 3).Value = item.Vendor_Name;
                    worksheet.Cell(currentRow, 4).Value = item.Total_Payable;

                    if (!string.IsNullOrEmpty(item.Paid) && !string.IsNullOrWhiteSpace(item.Paid))
                    {
                        worksheet.Cell(currentRow, 5).Value = Convert.ToDecimal(item.Paid);

                    }
                    else
                    {
                        worksheet.Cell(currentRow, 5).Value = item.Paid;

                    }
                    worksheet.Cell(currentRow, 6).Value = item.Balance;
                    worksheet.Cell(currentRow, 7).Value = item.PurchaseStatus;

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "reksolist.xlsx");
                }
            }
        }



        [HttpGet]
        public IActionResult SalesListJP_Print(string sale_Ref, string endDate, string startDate, int customerName)
        {





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









            _oSales.SalesMasterListJP1 = _oSales.pa_Select_SalesMaster(sale_Ref, customerName, startDate, endDate).ToList();
            _oSales.SalesMasterListJP_total = _oSales.pa_Select_SalesMaster_total(sale_Ref, customerName, startDate, endDate).ToList();



            var demoViewPortrait = new ViewAsPdf(_oSales)
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




        [HttpGet]
        public IActionResult SalesListJP_Excel(string sale_Ref, string endDate, string startDate, int customerName)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            sale_Ref = String.IsNullOrEmpty(sale_Ref) ? "" : sale_Ref;
            startDate = String.IsNullOrEmpty(startDate) ? "" : startDate;
            endDate = String.IsNullOrEmpty(endDate) ? "" : endDate;

            _oSales.SalesMasterListJP1 = _oSales.pa_Select_SalesMaster(sale_Ref, customerName, startDate, endDate).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("SalesListJP");
                var currentRow = 1;


                worksheet.Cell(currentRow, 1).Value = "Ref";
                worksheet.Cell(currentRow, 2).Value = "Date";
                worksheet.Cell(currentRow, 3).Value = "Vendor Name";
                worksheet.Cell(currentRow, 4).Value = "Total";
                worksheet.Cell(currentRow, 5).Value = "Paid";
                worksheet.Cell(currentRow, 6).Value = "Balance";



                foreach (var item in _oSales.SalesMasterListJP1)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.SaleRef;
                    worksheet.Cell(currentRow, 2).Value = item.SaleDate;
                    worksheet.Cell(currentRow, 3).Value = item.CustomerName;

                    if (!string.IsNullOrEmpty(item.Total_Amt) && !string.IsNullOrWhiteSpace(item.Total_Amt))
                    {
                        worksheet.Cell(currentRow, 4).Value = Convert.ToDecimal(item.Total_Amt);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 4).Value = item.Total_Amt;
                    }

                    if (!string.IsNullOrEmpty(item.Paid_amt) && !string.IsNullOrWhiteSpace(item.Paid_amt))
                    {
                        worksheet.Cell(currentRow, 5).Value = Convert.ToDecimal(item.Paid_amt);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 5).Value = item.Paid_amt;
                    }
                    worksheet.Cell(currentRow, 6).Value = item.Bal_due;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "SalesListJP.xlsx");
                }
            }
        }




        [HttpGet]
        public ActionResult PaperList_print(string chassis_No, string StartDate, string EndDate)
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


                ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
                _oStock.PapersListObject_print = _oStock.get_Papers_List(chassis_No, StartDate, EndDate).ToList();

                var demoViewPortrait = new ViewAsPdf(_oStock)
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

        }
        [HttpGet]
        public ActionResult PaperList_Excel(string chassis_No, string StartDate, string EndDate)
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

                chassis_No = String.IsNullOrEmpty(chassis_No) ? "" : chassis_No;
                StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
                EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;


                ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;
                _oStock.PapersListObject_print = _oStock.get_Papers_List(chassis_No, StartDate, EndDate).ToList();

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("PaperList");
                    var currentRow = 1;

                    worksheet.Cell(currentRow, 1).Value = "Chassis";
                    worksheet.Cell(currentRow, 2).Value = " Recieved Date";
                    worksheet.Cell(currentRow, 3).Value = " Purchase Ref";
                    worksheet.Cell(currentRow, 4).Value = "Status";


                    foreach (var item in _oStock.PapersListObject_print)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = item.Chassis;
                        worksheet.Cell(currentRow, 2).Value = item.Recieved_Date;
                        worksheet.Cell(currentRow, 3).Value = item.Purchase_Ref;
                        worksheet.Cell(currentRow, 4).Value = item.Status;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "PaperList.xlsx");
                    }
                }
            }
        }






        [HttpGet]
        public IActionResult SalesInvoice_JP_Print(int? SaleMaster_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion

            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oReports.SalesMasterObjJP_Print = _oReports.Get_SalesMasterJPMasterByID_Print_DAL(SaleMaster_ID);
            _oReports.SalesListJP_Print = _oReports.pa_select_salesJP_Print_DAL(SaleMaster_ID).ToList();
            _oReports.SalesGrandTotals_Print = _oReports.pa_select_salesJP_Total_Print_DAL(SaleMaster_ID);

            string Host = Request.Host.ToString();

            string TotalAmount = _oReports.pa_select_salesJP_Total_Print_DAL(SaleMaster_ID).GrandTotal.ToString();
            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0";
            }
            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());


            _oReports.SalesGrandTotals_Print.Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + " YEN "
                + (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + " YEN " + " only" : " only");



            string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/HeaderSalesInvJP?c_ID=" + c_ID,

            "http://" + Host + "/Report/FooterSalesInvJP?c_ID=" + c_ID);


            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 25, Right = 5, Top = 25 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;


        }
        [AllowAnonymous]
        public IActionResult HeaderSalesInvJP(int? c_ID)
        {
            ViewBag.ReportCompanySalInv_jp = c_ID;

            return View("~/Views/Report/" + ReportFolder + "/HeaderSalesInvJP.cshtml");
        }
        [AllowAnonymous]
        public IActionResult FooterSalesInvJP(int? c_ID)
        {
            ViewBag.ReportCompanyPur_F_JP = c_ID;
            return View("~/Views/Report/" + ReportFolder + "/FooterSalesInvJP.cshtml");
        }



        [HttpGet]
        public IActionResult AuctionList_Print(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
   string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, bool optional_cost, int Auction_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            BL_NO = String.IsNullOrEmpty(BL_NO) ? "" : BL_NO;
            BOE = String.IsNullOrEmpty(BOE) ? "" : BOE;
            loc_ID = loc_ID == null ? 0 : loc_ID;
            Stock_Status = String.IsNullOrEmpty(Stock_Status) ? "" : Stock_Status;
            VendorName = String.IsNullOrEmpty(VendorName) ? "" : VendorName;
            ModelYear = String.IsNullOrEmpty(ModelYear) ? "" : ModelYear;



            ViewBag.isOptional_Cost = optional_cost;

            _oStock.StockListGetObject = _oStock.Get_AuctionList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Auction_ID).ToList();

            _oStock.StockList_TTL1 = _oStock.Get_AuctionList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear).ToList();

            _oStock.StockListStats1 = _oStock.Get_AuctionList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear);
            _oStock.StockListStats1.isOptionalCost = optional_cost;

            var demoViewPortrait = new ViewAsPdf(_oStock)
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
        [HttpGet]
        public IActionResult AuctionReportList_Excel(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
       string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, bool optional_cost, int Auction_ID)
        {
            using (var workbook = new XLWorkbook())
            {

                #region login checking
                string UserName_Admin = HttpContext.Session.GetString("UserName");
                string UserID_Admin = HttpContext.Session.GetString("UserID");
                if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
                {
                    return RedirectToAction("Index", "Login");
                }
                #endregion

                ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
                Make_ID = Make_ID == null ? 0 : Make_ID;
                MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
                Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
                PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
                PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;
                string company_ID = HttpContext.Session.GetString("c_ID");
                int c_IDs = Convert.ToInt32(company_ID);
                c_ID = c_ID == 0 ? c_IDs : c_ID;
                PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
                SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
                BL_NO = String.IsNullOrEmpty(BL_NO) ? "" : BL_NO;
                BOE = String.IsNullOrEmpty(BOE) ? "" : BOE;
                loc_ID = loc_ID == null ? 0 : loc_ID;
                Stock_Status = String.IsNullOrEmpty(Stock_Status) ? "" : Stock_Status;
                VendorName = String.IsNullOrEmpty(VendorName) ? "" : VendorName;
                ModelYear = String.IsNullOrEmpty(ModelYear) ? "" : ModelYear;



                ViewBag.isOptional_Cost = optional_cost;

                _oStock.StockListGetObject = _oStock.Get_AuctionList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Auction_ID).ToList();

                _oStock.StockList_TTL1 = _oStock.Get_AuctionList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear).ToList();

                _oStock.StockListStats1 = _oStock.Get_AuctionList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear);
                _oStock.StockListStats1.isOptionalCost = optional_cost;



                var worksheet = workbook.Worksheets.Add("AuctionList");
                var currentRow = 1;


                worksheet.Cell(currentRow, 1).Value = "S.NO";
                worksheet.Cell(currentRow, 2).Value = "Chassis No";
                worksheet.Cell(currentRow, 3).Value = "Make";
                worksheet.Cell(currentRow, 4).Value = "Make Model";
                worksheet.Cell(currentRow, 5).Value = "Color";
                worksheet.Cell(currentRow, 6).Value = "Model Year";
                worksheet.Cell(currentRow, 7).Value = "Stock Type";

                int counter = 0;
                foreach (var item in _oStock.StockListGetObject)
                {
                    currentRow++;
                    counter++;


                    worksheet.Cell(currentRow, 1).Value = counter;
                    worksheet.Cell(currentRow, 2).Value = item.Chassis_No;
                    worksheet.Cell(currentRow, 3).Value = item.Make;
                    worksheet.Cell(currentRow, 4).Value = item.ModelDesciption;
                    worksheet.Cell(currentRow, 5).Value = item.Color;
                    worksheet.Cell(currentRow, 6).Value = item.ModelYear;
                    worksheet.Cell(currentRow, 7).Value = item.StockTypeName;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "AuctionList.xlsx");
                }
            }
        }






        //Reports Tab
        [HttpGet]
        public ActionResult SalesReport_trd(string StartDate, string EndDate, int? Customer_ID, string Sale_Ref, int Vendor_ID, int Seller_ID, int? page)
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

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Sale Report";
            ViewBag.PageTitle = "Sale Report";


            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "SalesReport";
            //---getting all customers
            ViewBag.AllCustomers = _oBasic.Get_CustomersMaster_DAL(c_ID).ToList();

            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList();

            ViewBag.SellerList = _oBasic.Get_PartnersSeller_DAL(c_ID).ToList();


            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            Customer_ID = (Customer_ID == null ? 0 : Customer_ID);
            Sale_Ref = (String.IsNullOrEmpty(Sale_Ref) || String.IsNullOrWhiteSpace(Sale_Ref) ? "" : Sale_Ref);
            Vendor_ID = Vendor_ID == 0 ? 0 : Vendor_ID;

            Seller_ID = Seller_ID == 0 ? 0 : Seller_ID;


            ViewBag.StartDate_SaleReport = StartDate;
            ViewBag.EndDate_SaleReport = EndDate;
            ViewBag.Customer_ID_SaleReport = Customer_ID;
            ViewBag.Sale_Ref_SaleReport = Sale_Ref;
            ViewBag.Vendor_ID_Sale_Ref_SaleReport = Vendor_ID;


            _oReports.SaleReport = _oReports.pa_Select_SalesReport_trd_DAL(StartDate, EndDate, Customer_ID, Sale_Ref, c_ID, Seller_ID).ToPagedList(page ?? 1, 10);

            _oReports.SaleReportTTL = _oReports.pa_Select_SalesReport_trd_TTL_DAL(StartDate, EndDate, Customer_ID, Sale_Ref, c_ID, Seller_ID);



            return View(_oReports);
        }
        [HttpGet]
        public ActionResult SalesReport_Search_trd(string StartDate, string EndDate, int? Customer_ID, string Sale_Ref, int Vendor_ID, int Seller_ID, int? page)
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

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Sale Report";
            ViewBag.PageTitle = "Sale Report";


            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "SalesReport";
            //---getting all customers
            ViewBag.AllCustomers = _oBasic.Get_CustomersMaster_DAL(c_ID).ToList();

            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList();

            ViewBag.SellerList = _oBasic.Get_PartnersSeller_DAL(c_ID).ToList();


            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            Customer_ID = (Customer_ID == null ? 0 : Customer_ID);
            Sale_Ref = (String.IsNullOrEmpty(Sale_Ref) || String.IsNullOrWhiteSpace(Sale_Ref) ? "" : Sale_Ref);
            Vendor_ID = (Vendor_ID == 0 ? 0 : Vendor_ID);

            Seller_ID = Seller_ID == 0 ? 0 : Seller_ID;



            ViewBag.StartDate_SaleReport = StartDate;
            ViewBag.EndDate_SaleReport = EndDate;
            ViewBag.Customer_ID_SaleReport = Customer_ID;
            ViewBag.Sale_Ref_SaleReport = Sale_Ref;
            ViewBag.Vendor_ID_SaleReport = Vendor_ID;

            ViewBag.Seller_ID_SaleReport = Seller_ID;



            _oReports.SaleReport = _oReports.pa_Select_SalesReport_trd_DAL(StartDate, EndDate, Customer_ID, Sale_Ref, c_ID, Seller_ID).ToPagedList(page ?? 1, 10);

            _oReports.SaleReportTTL = _oReports.pa_Select_SalesReport_trd_TTL_DAL(StartDate, EndDate, Customer_ID, Sale_Ref, c_ID, Seller_ID);



            return PartialView("_SaleReportSearch_trd", _oReports);
        }

        [HttpGet]
        public IActionResult SaleReport_Print_trd(string StartDate, string EndDate, int? Customer_ID, string Sale_Ref, int Vendor_ID, int Seller_ID)
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

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            Sale_Ref = (String.IsNullOrEmpty(Sale_Ref) || String.IsNullOrWhiteSpace(Sale_Ref) ? "" : Sale_Ref);
            Customer_ID = Customer_ID == null ? 0 : Customer_ID;
            Vendor_ID = (Vendor_ID == 0 ? 0 : Vendor_ID);
            Seller_ID = Seller_ID == 0 ? 0 : Seller_ID;


            _oReports.SaleReportPrint = _oReports.pa_Select_SalesReport_trd_DAL(StartDate, EndDate, Customer_ID, Sale_Ref, c_ID, Vendor_ID).ToList();

            _oReports.SaleReportTTLPrint = _oReports.pa_Select_SalesReport_trd_TTL_DAL(StartDate, EndDate, Customer_ID, Sale_Ref, c_ID, Vendor_ID);
            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
 "--footer-left \"  Printed At: " +
DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
" --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };
            return demoViewPortrait;

        }

        [HttpGet]
        public ActionResult StockCost(string StartPurchaseDate, string EndPurchaseDate, string Container_No, int? page)
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




            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "StockCostReport";

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "StockCost Report";
            ViewBag.PageTitle = "StockCost Report";

            StartPurchaseDate = (String.IsNullOrEmpty(StartPurchaseDate) || String.IsNullOrWhiteSpace(StartPurchaseDate) ? "" : StartPurchaseDate);
            EndPurchaseDate = (String.IsNullOrEmpty(EndPurchaseDate) || String.IsNullOrWhiteSpace(EndPurchaseDate) ? "" : EndPurchaseDate);
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;

            ViewBag.StartDate_StockCost = StartPurchaseDate;
            ViewBag.EndDate_StockCost = EndPurchaseDate;


            _oReports.StockCostList = _oReports.pa_Select_StockCostReport_DAL(StartPurchaseDate, EndPurchaseDate, Container_No, c_ID).ToPagedList(page ?? 1, 10);
            _oReports.StockCostList_ttl = _oReports.select_StockCostReport_ttl_DAL(StartPurchaseDate, EndPurchaseDate, Container_No, c_ID).ToList();



            return View(_oReports);
        }
        [HttpGet]
        public ActionResult StockCostSearch(string StartPurchaseDate, string EndPurchaseDate, string Container_No, int? page)
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




            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "StockCostReport";

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "StockCost Report";
            ViewBag.PageTitle = "StockCost Report";

            StartPurchaseDate = (String.IsNullOrEmpty(StartPurchaseDate) || String.IsNullOrWhiteSpace(StartPurchaseDate) ? "" : StartPurchaseDate);
            EndPurchaseDate = (String.IsNullOrEmpty(EndPurchaseDate) || String.IsNullOrWhiteSpace(EndPurchaseDate) ? "" : EndPurchaseDate);
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;

            ViewBag.StartDate_StockCost = StartPurchaseDate;
            ViewBag.EndDate_StockCost = EndPurchaseDate;

            _oReports.StockCostList = _oReports.pa_Select_StockCostReport_DAL(StartPurchaseDate, EndPurchaseDate, Container_No, c_ID).ToPagedList(page ?? 1, 10);
            _oReports.StockCostList_ttl = _oReports.select_StockCostReport_ttl_DAL(StartPurchaseDate, EndPurchaseDate, Container_No, c_ID).ToList();




            return PartialView("_StockCostSearch", _oReports);
        }
        [HttpGet]
        public IActionResult StockCost_Print(string StartPurchaseDate, string EndPurchaseDate, string Container_No)
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
            StartPurchaseDate = (String.IsNullOrEmpty(StartPurchaseDate) || String.IsNullOrWhiteSpace(StartPurchaseDate) ? "" : StartPurchaseDate);
            EndPurchaseDate = (String.IsNullOrEmpty(EndPurchaseDate) || String.IsNullOrWhiteSpace(EndPurchaseDate) ? "" : EndPurchaseDate);
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;

            _oReports.StockCostLists = _oReports.pa_Select_StockCostReport_DAL(StartPurchaseDate, EndPurchaseDate, Container_No, c_ID).ToList();
            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
  "--footer-left \"  Printed At: " +
DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
" --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };
            return demoViewPortrait;

        }

        [HttpGet]
        public IActionResult StockCost_Excel(string StartPurchaseDate, string EndPurchaseDate, string Container_No)
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

            StartPurchaseDate = (String.IsNullOrEmpty(StartPurchaseDate) || String.IsNullOrWhiteSpace(StartPurchaseDate) ? "" : StartPurchaseDate);
            EndPurchaseDate = (String.IsNullOrEmpty(EndPurchaseDate) || String.IsNullOrWhiteSpace(EndPurchaseDate) ? "" : EndPurchaseDate);
            Container_No = String.IsNullOrEmpty(Container_No) ? "" : Container_No;

            _oReports.StockCostLists = _oReports.pa_Select_StockCostReport_DAL(StartPurchaseDate, EndPurchaseDate, Container_No, c_ID).ToList();

            using (var workbook = new XLWorkbook())
            {

                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("StockCost");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Chassis No";
                worksheet.Cell(currentRow, 2).Value = "Chassis No";
                worksheet.Cell(currentRow, 3).Value = "ModelYear";

                worksheet.Cell(currentRow, 4).Value = "Price";
                worksheet.Cell(currentRow, 5).Value = "Price Rate";
                worksheet.Cell(currentRow, 6).Value = "Price Tax";
                worksheet.Cell(currentRow, 7).Value = "Freight";
                worksheet.Cell(currentRow, 8).Value = "Auction";
                worksheet.Cell(currentRow, 9).Value = "Rekso";
                worksheet.Cell(currentRow, 10).Value = "Recycle";
                worksheet.Cell(currentRow, 11).Value = "Loading";
                worksheet.Cell(currentRow, 12).Value = "Others";
                worksheet.Cell(currentRow, 13).Value = "JP Charges";
                worksheet.Cell(currentRow, 14).Value = "Total Cost";
                worksheet.Cell(currentRow, 15).Value = "Total Cost Other";


                foreach (var item in _oReports.StockCostLists)
                {
                    currentRow++;

                    worksheet.Cell(currentRow, 1).Value = item.Chassis_No;
                    worksheet.Cell(currentRow, 2).Value = item.PurchaseDate;
                    worksheet.Cell(currentRow, 3).Value = item.ModelYear;


                    if (!string.IsNullOrEmpty(item.Price) && !string.IsNullOrWhiteSpace(item.Price))
                    {
                        worksheet.Cell(currentRow, 4).Value = Convert.ToDecimal(item.Price);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 4).Value = item.Price;
                    }

                    worksheet.Cell(currentRow, 5).Value = item.PriceRate;
                    worksheet.Cell(currentRow, 6).Value = item.PriceTax;
                    worksheet.Cell(currentRow, 7).Value = item.Freight;
                    worksheet.Cell(currentRow, 8).Value = item.Auction;
                    worksheet.Cell(currentRow, 9).Value = item.Rekso;
                    worksheet.Cell(currentRow, 10).Value = item.Recycle;
                    worksheet.Cell(currentRow, 11).Value = item.Loading;
                    worksheet.Cell(currentRow, 12).Value = item.OtherCharges_JP;
                    worksheet.Cell(currentRow, 13).Value = item.JP_Charges;

                    if (!string.IsNullOrEmpty(item.Total_Cost) && !string.IsNullOrWhiteSpace(item.Total_Cost))
                    {
                        worksheet.Cell(currentRow, 14).Value = Convert.ToDecimal(item.Total_Cost);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 14).Value = item.Total_Cost;
                    }

                    worksheet.Cell(currentRow, 15).Value = item.Total_Cost_Others;
                }


                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "StockCost.xlsx");
                }
            }
        }




        [HttpGet]
        public IActionResult StockList_MM_Print(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
       string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            Make_ID = Make_ID == null ? 0 : Make_ID;
            MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
            Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
            PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
            PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
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

            _oStock.StockListGetObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();

            _oStock.StockList_TTL1 = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();

            _oStock.StockListStats1 = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);


            var demoViewPortrait = new ViewAsPdf(_oStock)
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

        [HttpGet]
        public IActionResult StockReportList_MM_Excel(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
  string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID)
        {
            using (var workbook = new XLWorkbook())
            {

                #region login checking
                string UserName_Admin = HttpContext.Session.GetString("UserName");
                string UserID_Admin = HttpContext.Session.GetString("UserID");
                if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
                {
                    return RedirectToAction("Index", "Login");
                }
                #endregion

                ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
                Make_ID = Make_ID == null ? 0 : Make_ID;
                MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
                Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
                PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
                PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;
                string company_ID = HttpContext.Session.GetString("c_ID");
                int c_IDs = Convert.ToInt32(company_ID);
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



                _oStock.StockListGetObject = _oStock.Get_StockList_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();

                _oStock.StockList_TTL1 = _oStock.Get_StockList_TTL_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();

                _oStock.StockListStats1 = _oStock.Get_StockList_stats_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID);

                var worksheet = workbook.Worksheets.Add("StockList");
                var currentRow = 1;


                worksheet.Cell(currentRow, 1).Value = "S.NO";
                worksheet.Cell(currentRow, 2).Value = "DATE";
                worksheet.Cell(currentRow, 3).Value = "LOT NO";
                worksheet.Cell(currentRow, 4).Value = "JB NO";
                worksheet.Cell(currentRow, 5).Value = "Make Model";
                worksheet.Cell(currentRow, 6).Value = "CHASSIS";
                worksheet.Cell(currentRow, 7).Value = "MODEL YEAR";
                worksheet.Cell(currentRow, 8).Value = "COLOR";
                worksheet.Cell(currentRow, 9).Value = "PRICE";
                worksheet.Cell(currentRow, 10).Value = "AUCTION NAME";
                worksheet.Cell(currentRow, 11).Value = "LOCATION";

                int counter = 0;
                foreach (var item in _oStock.StockListGetObject)
                {
                    currentRow++;
                    counter++;
                    worksheet.Cell(currentRow, 1).Value = counter;
                    worksheet.Cell(currentRow, 2).Value = item.PurchaseDate;
                    worksheet.Cell(currentRow, 3).Value = item.LotNumber;
                    worksheet.Cell(currentRow, 4).Value = item.ContNO;
                    worksheet.Cell(currentRow, 5).Value = item.Make;
                    worksheet.Cell(currentRow, 6).Value = item.Chassis_No;
                    worksheet.Cell(currentRow, 7).Value = item.ModelYear;
                    worksheet.Cell(currentRow, 8).Value = item.Color;


                    if (!string.IsNullOrEmpty(item.Total_Cost) && !string.IsNullOrWhiteSpace(item.Total_Cost))
                    {
                        worksheet.Cell(currentRow, 9).Value = Convert.ToDecimal(item.Total_Cost);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 9).Value = item.Total_Cost;
                    }

                    worksheet.Cell(currentRow, 10).Value = item.AuctionName;
                    worksheet.Cell(currentRow, 11).Value = item.CarLocation;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "StockList.xlsx");
                }
            }
        }



        //This method is for getting Ledger list
        [HttpGet]
        public ActionResult VENDOR_REPORT_OTHER(string StartDate, string EndDate, int? VendorID)
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

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "VendorReport";

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Vendor Report";
            ViewBag.PageTitle = "Vendor Report";
            ViewBag.AllVendors = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList();

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);

            _oReports.Ledger = _oReports.pa_Select_VendorLedger_Other_DAL(StartDate, EndDate, VendorID.ToString(), c_ID).ToList();
            _oReports.Ledger_TTL = _oReports.pa_Select_VendorLedger_Other_DAL_TTL(StartDate, EndDate, VendorID.ToString(), c_ID).ToList();


            return View(_oReports);
        }
        [HttpGet]
        public ActionResult VENDOR_REPORT_OTHER_Search(string StartDate, string EndDate, int? VendorID)
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
            //if (StartDate == null && EndDate == null)
            //{
            //    DateTime date = DateTime.Now;
            //    var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            //    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            //    StartDate = firstDayOfMonth.ToString("dd-MMM-yyyy");
            //    EndDate = lastDayOfMonth.ToString("dd-MMM-yyyy");
            //}
            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "VendorReport";

            ViewBag.AllVendors = _oBasic.Get_VendorMasterList_DAL(c_ID).ToList();

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);

            _oReports.Ledger = _oReports.pa_Select_VendorLedger_Other_DAL(StartDate, EndDate, VendorID.ToString(), c_ID).ToList();
            _oReports.Ledger_TTL = _oReports.pa_Select_VendorLedger_Other_DAL_TTL(StartDate, EndDate, VendorID.ToString(), c_ID).ToList();

            return PartialView("_VendorReport", _oReports);
        }


        [HttpGet]
        public IActionResult VendorReport_Other_Print(string StartDate, string EndDate, int? VendorID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            VendorID = VendorID == null ? 0 : VendorID;
            _oReports.Ledger_Print = _oReports.pa_Select_VendorLedger_Other_DAL(StartDate, EndDate, VendorID.ToString(), c_ID).ToList();
            _oReports.Ledger_TTL_Print = _oReports.pa_Select_VendorLedger_Other_DAL_TTL_Print(StartDate, EndDate, VendorID.ToString(), c_ID);

            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
  "--footer-left \"  Printed At: " +
DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
" --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };
            return demoViewPortrait;

        }


        [HttpGet]
        public IActionResult VendorReport_Other_Excel(string StartDate, string EndDate, int? VendorID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            VendorID = VendorID == null ? 0 : VendorID;
            _oReports.Ledger_Print = _oReports.pa_Select_VendorLedger_Other_DAL(StartDate, EndDate, VendorID.ToString(), c_ID).ToList();
            _oReports.Ledger_TTL_Print = _oReports.pa_Select_VendorLedger_Other_DAL_TTL_Print(StartDate, EndDate, VendorID.ToString(), c_ID);


            using (var workbook = new XLWorkbook())
            {

                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("VendorReport");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Date";
                worksheet.Cell(currentRow, 2).Value = "Debit";
                worksheet.Cell(currentRow, 3).Value = "Credit";
                worksheet.Cell(currentRow, 4).Value = "Description";
                worksheet.Cell(currentRow, 5).Value = "Chassis No";
                worksheet.Cell(currentRow, 6).Value = "Party Name";
                worksheet.Cell(currentRow, 7).Value = "Ref #";
                foreach (var item in _oReports.Ledger_Print)
                {
                    currentRow++;

                    worksheet.Cell(currentRow, 1).Value = item.Date;


                    if (!string.IsNullOrEmpty(item.Debit) && !string.IsNullOrWhiteSpace(item.Debit))
                    {
                        worksheet.Cell(currentRow, 2).Value = Convert.ToDecimal(item.Debit);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 2).Value = item.Debit;
                    }

                    if (!string.IsNullOrEmpty(item.Credit) && !string.IsNullOrWhiteSpace(item.Credit))
                    {
                        worksheet.Cell(currentRow, 3).Value = Convert.ToDecimal(item.Credit);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 3).Value = item.Credit;
                    }

                    worksheet.Cell(currentRow, 4).Value = item.Description;
                    worksheet.Cell(currentRow, 5).Value = item.Chassis_No;
                    worksheet.Cell(currentRow, 6).Value = item.PartyName;
                    worksheet.Cell(currentRow, 7).Value = item.trans_ref;
                }
                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "VendorReport.xlsx");
                }
            }
        }


        [HttpGet]
        public IActionResult InventoryItemCardParts_Excel(string ItemCode, int Item_ID, string traditional, string Make, string Fuel, string Transmission, string Drive,
           int ItemGroup_ID, int ItemCategory_ID, string Year, string ItemSerialNO, string StartDate, string EndDate, int Loc_ID, string EngineSpecsCode_ID)
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

            ItemCode = string.IsNullOrEmpty(ItemCode) ? "" : ItemCode;

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);

            Item_ID = Item_ID == 0 ? 0 : Item_ID;


            traditional = string.IsNullOrEmpty(traditional) ? "" : traditional;
            Make = string.IsNullOrEmpty(Make) ? "" : Make;
            Fuel = string.IsNullOrEmpty(Fuel) ? "" : Fuel;
            Transmission = string.IsNullOrEmpty(Transmission) ? "" : Transmission;
            Drive = string.IsNullOrEmpty(Drive) ? "" : Drive;

            ItemSerialNO = string.IsNullOrEmpty(ItemSerialNO) ? "" : ItemSerialNO;
            ItemGroup_ID = ItemGroup_ID == 0 ? 0 : ItemGroup_ID;
            ItemCategory_ID = ItemCategory_ID == 0 ? 0 : ItemCategory_ID;
            Loc_ID = Loc_ID == 0 ? 0 : Loc_ID;
            EngineSpecsCode_ID = string.IsNullOrEmpty(EngineSpecsCode_ID) ? "" : EngineSpecsCode_ID;
            Year = string.IsNullOrEmpty(Year) ? "" : Year;


            _oReports.ItemCardReportList = _oReports.Select_ItemCard_Report_Parts_DAL(ItemCode, Item_ID, traditional, Make, Fuel, Transmission, Drive, ItemGroup_ID, ItemCategory_ID, Year, ItemSerialNO,
            StartDate, EndDate, Loc_ID, EngineSpecsCode_ID, c_ID).ToList();
            _oReports.ItemCardReportList_TTL = _oReports.Select_ItemCard_Report_parts_TTL_DAL(ItemCode, Item_ID, traditional, Make, Fuel, Transmission, Drive, ItemGroup_ID, ItemCategory_ID, Year, ItemSerialNO,
            StartDate, EndDate, Loc_ID, EngineSpecsCode_ID, c_ID).ToList();

            using (var workbook = new XLWorkbook())
            {

                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("InventoryItemCard");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "TransDate";
                worksheet.Cell(currentRow, 2).Value = "Ref";
                worksheet.Cell(currentRow, 3).Value = "Name";
                worksheet.Cell(currentRow, 4).Value = "Quantity";
                worksheet.Cell(currentRow, 5).Value = "UnitPrice";
                worksheet.Cell(currentRow, 6).Value = "Amount/Tax";
                worksheet.Cell(currentRow, 7).Value = "Total(Pur) ";
                worksheet.Cell(currentRow, 8).Value = "Location";
                worksheet.Cell(currentRow, 9).Value = "ItemCode";
                worksheet.Cell(currentRow, 10).Value = "Quanity(Sold)";
                worksheet.Cell(currentRow, 11).Value = "UnitPrice";
                worksheet.Cell(currentRow, 12).Value = "Amount/Tax";
                worksheet.Cell(currentRow, 13).Value = "Total";

                string vat = ""; string vats = "";
                foreach (var item in _oReports.ItemCardReportList)
                {
                    currentRow++;
                    vat = item.Pur_Total_Amt + Environment.NewLine + item.VATEXP_Pur;
                    vats = item.SaleAmount + Environment.NewLine + item.VAT_Sale;
                    worksheet.Cell(currentRow, 1).Value = item.Trans_Date;
                    worksheet.Cell(currentRow, 2).Value = item.Trans_Ref;
                    worksheet.Cell(currentRow, 3).Value = item.ItemName;
                    worksheet.Cell(currentRow, 4).Value = item.Qty;


                    if (!string.IsNullOrEmpty(item.UnitPrice) && !string.IsNullOrWhiteSpace(item.UnitPrice))
                    {
                        worksheet.Cell(currentRow, 5).Value = Convert.ToDecimal(item.UnitPrice);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 5).Value = item.UnitPrice;
                    }

                    if (!string.IsNullOrEmpty(item.Pur_Amount) && !string.IsNullOrWhiteSpace(item.Pur_Amount))
                    {
                        worksheet.Cell(currentRow, 6).Value = Convert.ToDecimal(item.Pur_Amount);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 6).Value = item.Pur_Amount;
                    }

                    worksheet.Cell(currentRow, 7).Value = vats;

                    worksheet.Cell(currentRow, 8).Value = item.ItemLocationName;
                    worksheet.Cell(currentRow, 9).Value = item.ItemCode;
                    worksheet.Cell(currentRow, 10).Value = item.QtySold;

                    if (!string.IsNullOrEmpty(item.SaleUnitPrice) && !string.IsNullOrWhiteSpace(item.SaleUnitPrice))
                    {
                        worksheet.Cell(currentRow, 11).Value = Convert.ToDecimal(item.SaleUnitPrice);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 11).Value = item.SaleUnitPrice;
                    }


                    worksheet.Cell(currentRow, 12).Value = vats;

                    if (!string.IsNullOrEmpty(item.Sale_Total_Amt) && !string.IsNullOrWhiteSpace(item.Sale_Total_Amt))
                    {
                        worksheet.Cell(currentRow, 13).Value = Convert.ToDecimal(item.Sale_Total_Amt);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 13).Value = item.Sale_Total_Amt;
                    }


                }
                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "InventoryItemCard.xlsx");
                }
            }
        }





        [HttpGet]
        public IActionResult InventoryItemCardParts_Report(string ItemCode, int Item_ID, string traditional, string Make, string Fuel, string Transmission, string Drive,
            int ItemGroup_ID, int ItemCategory_ID, string Year, string ItemSerialNO, string StartDate, string EndDate, int Loc_ID, string EngineSpecsCode_ID)
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

            ItemCode = string.IsNullOrEmpty(ItemCode) ? "" : ItemCode;

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);

            Item_ID = Item_ID == 0 ? 0 : Item_ID;


            traditional = string.IsNullOrEmpty(traditional) ? "" : traditional;
            Make = string.IsNullOrEmpty(Make) ? "" : Make;
            Fuel = string.IsNullOrEmpty(Fuel) ? "" : Fuel;
            Transmission = string.IsNullOrEmpty(Transmission) ? "" : Transmission;
            Drive = string.IsNullOrEmpty(Drive) ? "" : Drive;
            Year = string.IsNullOrEmpty(Year) ? "" : Year;

            ItemSerialNO = string.IsNullOrEmpty(ItemSerialNO) ? "" : ItemSerialNO;
            ItemGroup_ID = ItemGroup_ID == 0 ? 0 : ItemGroup_ID;
            ItemCategory_ID = ItemCategory_ID == 0 ? 0 : ItemCategory_ID;
            Loc_ID = Loc_ID == 0 ? 0 : Loc_ID;
            EngineSpecsCode_ID = string.IsNullOrEmpty(EngineSpecsCode_ID) ? "" : EngineSpecsCode_ID;


            _oReports.ItemCardReportList = _oReports.Select_ItemCard_Report_Parts_DAL(ItemCode, Item_ID, traditional, Make, Fuel, Transmission, Drive, ItemGroup_ID, ItemCategory_ID, Year, ItemSerialNO,
            StartDate, EndDate, Loc_ID, EngineSpecsCode_ID, c_ID).ToList();
            _oReports.ItemCardReportList_TTL = _oReports.Select_ItemCard_Report_parts_TTL_DAL(ItemCode, Item_ID, traditional, Make, Fuel, Transmission, Drive, ItemGroup_ID, ItemCategory_ID, Year, ItemSerialNO,
            StartDate, EndDate, Loc_ID, EngineSpecsCode_ID, c_ID).ToList();
            var demoViewPortrait = new ViewAsPdf(_oReports)
            {

                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageMargins = { Left = 1, Bottom = 20, Right = 1, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
                         "--footer-left \"  Printed At: " +
         DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
         " --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""

            };
            return demoViewPortrait;


        }

        [HttpGet]
        public IActionResult Broucher_Print(int Stock_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            Stock_ID = Stock_ID == null ? 0 : Stock_ID;



            _oStock.StockObject = _oStock.Get_Select_Stock_by_ID_DAL(Stock_ID);
            _oStock.attachmentList = _oStock.GetStockMasterNew_Attachments_DAL(Stock_ID, "StockNew").ToList();
            //return View(_oStock);
            var demoViewPortrait = new ViewAsPdf(_oStock)
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

        [HttpGet]
        public IActionResult SalesInvoice_Print_trd(int? SaleMaster_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;
            ViewBag.ISQuzoon = configuration.GetSection("AppSettings").GetSection("ShowReportForQonooz").Value;


            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.SalesMasterPrintObj = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID);
            _oSales.SalesDetailPrintObj = _oSales.Get_SalesIvoiceDetailListByChassis_DAL_TRD("", SaleMaster_ID).ToList();
            _oSales.SalesDetailPrintObj_TTL = _oSales.Get_SalesInvoiceDetailPrintByID_ttl(SaleMaster_ID);
            _oSales.SalesReceiptDetailsList_Print = _oSales.Get_ReceiptDetailSaleListByID_DAL_Invoice_Print(SaleMaster_ID);
            string Host = Request.Host.ToString();

            string TotalAmount = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID).AfterVatTotal;
            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0";
            }
            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());


            _oSales.SalesDetailPrintObj_TTL.Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.SalesMasterPrintObj.Currency_ShortName
                + (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.SalesMasterPrintObj.Minor_ShortName + " only" : " only");



            string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/PrintHeaderInvoiceSalesInv?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID,

            "http://" + Host + "/Report/FooterHeaderInvoiceSalesInv?c_ID=" + c_ID);


            var demoViewPortrait = new ViewAsPdf(_oSales)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 55, Right = 5, Top = 60 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches
            };
            return demoViewPortrait;

        }

        public ActionResult InventoryReportItemLocationDetails(string ItemCode, int Item_ID, string traditional, string Make, string Fuel, string Transmission, string Drive,
       int ItemGroup_ID, int ItemCategory_ID, string Year, string ItemSerialNO, string StartDate, string EndDate, int Loc_ID, int? page)
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

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "InventoryReportItemCard";
            ViewBag.BreadCumTitle = "Inventory Report";

            ViewBag.AllItems = _oBasic.Get_ItemMasterList_DAL().ToList();
            ViewBag.ItemGroupList = _oBasic.Get_ItemGroup_DAL(c_ID).ToList();
            ViewBag.ItemCategoryList = _oBasic.Get_ItemCategory_DAL(c_ID).ToList();
            ViewBag.MakeList = _oBasic.Get_MakeModelList_DAL(c_ID).ToList();
            ItemCode = string.IsNullOrEmpty(ItemCode) ? "" : ItemCode;

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);

            Item_ID = Item_ID == 0 ? 0 : Item_ID;

            traditional = string.IsNullOrEmpty(traditional) ? "" : traditional;
            Make = string.IsNullOrEmpty(Make) ? "" : Make;
            Fuel = string.IsNullOrEmpty(Fuel) ? "" : Fuel;
            Transmission = string.IsNullOrEmpty(Transmission) ? "" : Transmission;
            Drive = string.IsNullOrEmpty(Drive) ? "" : Drive;
            Year = string.IsNullOrEmpty(Year) ? "" : Year;
            ItemSerialNO = string.IsNullOrEmpty(ItemSerialNO) ? "" : ItemSerialNO;
            ItemGroup_ID = ItemGroup_ID == 0 ? 0 : ItemGroup_ID;
            ItemCategory_ID = ItemCategory_ID == 0 ? 0 : ItemCategory_ID;
            Loc_ID = Loc_ID == 0 ? 0 : Loc_ID;



            ViewBag.ItemCode = ItemCode;
            ViewBag.Item_ID = Item_ID;
            ViewBag.traditional = traditional;
            ViewBag.Make = Make;
            ViewBag.Fuel = Fuel;
            ViewBag.Transmission = Transmission;
            ViewBag.Drive = Drive;
            ViewBag.ItemGroup_ID = ItemGroup_ID;
            ViewBag.ItemCategory_ID = ItemCategory_ID;
            ViewBag.Year = Year;
            ViewBag.ItemSerialNO = ItemSerialNO;
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = StartDate;
            ViewBag.Loc_ID = Loc_ID;


            _oReports.ItemCardReportListParts = _oReports.Select_Itemlocation_Report_Parts_DAL(ItemCode, Item_ID, traditional, Make, Fuel, Transmission, Drive, ItemGroup_ID, ItemCategory_ID, Year, ItemSerialNO,
            StartDate, EndDate, Loc_ID, c_ID).ToPagedList(page ?? 1, 10);
            _oReports.ItemCardReportList_TTL = _oReports.Select_ItemCard_Report_location_TTL_DAL(ItemCode, Item_ID, traditional, Make, Fuel, Transmission, Drive, ItemGroup_ID, ItemCategory_ID, Year, ItemSerialNO,
            StartDate, EndDate, Loc_ID, c_ID).ToList();


            return View(_oReports);
        }


        public ActionResult InventoryReportItemLocationDetailsSearch(string ItemCode, int Item_ID, string traditional, string Make, string Fuel, string Transmission, string Drive,
           int ItemGroup_ID, int ItemCategory_ID, string Year, string ItemSerialNO, string StartDate, string EndDate, int Loc_ID, int? page)
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



            ItemCode = string.IsNullOrEmpty(ItemCode) ? "" : ItemCode;

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);

            Item_ID = Item_ID == 0 ? 0 : Item_ID;

            //     ItemName = ItemName == "0" ? "" : ItemName.Split(' ')[1];
            //      ItemName = string.IsNullOrEmpty(ItemName) ? "" : ItemName.Split(' ')[1];
            traditional = string.IsNullOrEmpty(traditional) ? "" : traditional;
            Make = string.IsNullOrEmpty(Make) ? "" : Make;
            Fuel = string.IsNullOrEmpty(Fuel) ? "" : Fuel;
            Transmission = string.IsNullOrEmpty(Transmission) ? "" : Transmission;
            Drive = string.IsNullOrEmpty(Drive) ? "" : Drive;
            Year = string.IsNullOrEmpty(Year) ? "" : Year;
            ItemSerialNO = string.IsNullOrEmpty(ItemSerialNO) ? "" : ItemSerialNO;
            ItemGroup_ID = ItemGroup_ID == 0 ? 0 : ItemGroup_ID;
            ItemCategory_ID = ItemCategory_ID == 0 ? 0 : ItemCategory_ID;
            Loc_ID = Loc_ID == 0 ? 0 : Loc_ID;

            ViewBag.ItemCode = ItemCode;
            ViewBag.Item_ID = Item_ID;
            ViewBag.traditional = traditional;
            ViewBag.Make = Make;
            ViewBag.Fuel = Fuel;
            ViewBag.Transmission = Transmission;
            ViewBag.Drive = Drive;
            ViewBag.ItemGroup_ID = ItemGroup_ID;
            ViewBag.ItemCategory_ID = ItemCategory_ID;
            ViewBag.Year = Year;
            ViewBag.ItemSerialNO = ItemSerialNO;
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = StartDate;
            ViewBag.Loc_ID = Loc_ID;

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "InventoryReportItemCard";
            ViewBag.BreadCumTitle = "Inventory Report";

            ViewBag.AllItems = _oBasic.Get_ItemMasterList_DAL().ToList();
            ViewBag.ItemGroupList = _oBasic.Get_ItemGroup_DAL(c_ID).ToList();
            ViewBag.ItemCategoryList = _oBasic.Get_ItemCategory_DAL(c_ID).ToList();
            ViewBag.MakeList = _oBasic.Get_MakeModelList_DAL(c_ID).ToList();



            _oReports.ItemCardReportListParts = _oReports.Select_Itemlocation_Report_Parts_DAL(ItemCode, Item_ID, traditional, Make, Fuel, Transmission, Drive, ItemGroup_ID, ItemCategory_ID, Year, ItemSerialNO,
            StartDate, EndDate, Loc_ID, c_ID).ToPagedList(page ?? 1, 10);
            _oReports.ItemCardReportList_TTL = _oReports.Select_ItemCard_Report_location_TTL_DAL(ItemCode, Item_ID, traditional, Make, Fuel, Transmission, Drive, ItemGroup_ID, ItemCategory_ID, Year, ItemSerialNO,
            StartDate, EndDate, Loc_ID, c_ID).ToList();


            return PartialView("_InventoryReportItemLocationDetailsSearch", _oReports);
        }

        [HttpGet]
        public IActionResult GetLocationDetails(string ItemCode)
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
                _oInventory.LocDetailList = _oInventory.GetLocationDetails_DAL(ItemCode).ToList();

                return PartialView("_LocDetailList", _oInventory);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }
        }

        [HttpGet]
        public IActionResult InventoryTransfer_Print(int? Transfer_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion

            Transfer_ID = (Transfer_ID == null ? 0 : Transfer_ID);
            ViewBag.ReportCompanyTransfer = c_ID;
            _oInventory.InventoryTransferMasterObj = _oInventory.Get_InventoryTransfer_MasterByID_DAL(Transfer_ID);

            //---Inventory detial invoice list for Inventory invoice type Bychassis
            _oInventory.InventoryTransferDetailList = _oInventory.Get_InventoryTransfer_DetailListBy_DAL("", Transfer_ID).ToList();

            var demoViewPortrait = new ViewAsPdf(_oInventory)
            {

                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
            return demoViewPortrait;
        }



        [HttpGet]
        public IActionResult SalesInvoice_Print_Thermal_trd(int? SaleMaster_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;




            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.SalesMasterPrintObj = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID);
            _oSales.SalesDetailPrintObj = _oSales.Get_SalesIvoiceDetailListByChassis_DAL_TRD("", SaleMaster_ID).ToList();
            _oSales.SalesDetailPrintObj_TTL = _oSales.Get_SalesInvoiceDetailPrintByID_ttl(SaleMaster_ID);
            _oSales.SalesReceiptDetailsList_Print = _oSales.Get_ReceiptDetailSaleListByID_DAL_Invoice_Print(SaleMaster_ID);
            string Host = Request.Host.ToString();

            string TotalAmount = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID).AfterVatTotal;
            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0";
            }
            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());

            ViewBag.Qunooz = configuration.GetSection("AppSettings").GetSection("CompanyReportFolderCode").Value;



            _oSales.SalesDetailPrintObj_TTL.Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.SalesMasterPrintObj.Currency_ShortName
                + (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.SalesMasterPrintObj.Minor_ShortName + " only" : " only");


            if (ViewBag.Qunooz == "QZ")
            {

                string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

                "http://" + Host + "/Report/PrintHeaderInvoiceSalesInvThermal?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID,

                "http://" + Host + "/Report/FooterHeaderInvoiceSalesInv?c_ID=" + c_ID);

                var demoViewPortrait = new ViewAsPdf(_oSales)
                {

                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageMargins = { Left = 5, Bottom = 55, Right = 5, Top = 60 },
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                    CustomSwitches = customSwitches
                };
                return demoViewPortrait;
            }
            else
            {
                var demoViewPortrait = new ViewAsPdf(_oSales)
                {

                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Bottom = 5, Right = 1, Top = 1 },

                };
                return demoViewPortrait;

            }
            //return View(_oSales);





        }
        [HttpGet]
        public IActionResult PurchaseInvoicePrint_trd(int? PurchaseMaster_ID)
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



            _oReports.PurchaseMasterPrint = _oPurchase.Get_PurchaseMaster_OtherByID_DAL(PurchaseMaster_ID);
            _oReports.PurchaseDetailPrint = _oPurchase.Get_PurchaseDetailOtherListByID_DAL("", PurchaseMaster_ID).ToList();




            string Host = Request.Host.ToString();


            //return View(_oReports);

            string customSwitches = string.Format("--header-spacing \"0\" --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/HEADERPurchaseInvoice?c_ID=" + c_ID,

              "http://" + Host + "/Report/FooterHeaderInvoiceSalesInv?c_ID=" + c_ID);




            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 48, Right = 5, Top = 25 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;



        }
        [HttpGet]
        public IActionResult SalesDeliveryOrder_Print(int? DOMaster_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;
            DOMaster_ID = (DOMaster_ID == null ? 0 : DOMaster_ID);
            _oSales.SalesDOMasterPrintObj = _oSales.Get_SalesDeliveryMasterPrintByID_DO_DAL(DOMaster_ID);
            _oSales.SalesDODetailPrintObj = _oSales.Get_SalesDeliveryOrderDetailListByID_DAL("", DOMaster_ID).ToList();
            _oSales.SalesDODetailPrintObj_TTL = _oSales.Get_SalesDeliveryOrderDetailPrintByID_DO_ttl(DOMaster_ID);

            string Host = Request.Host.ToString();

            string TotalAmount = _oSales.Get_SalesDeliveryMasterPrintByID_DO_DAL(DOMaster_ID).AfterVatTotal;
            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0";
            }
            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());


            _oSales.SalesDODetailPrintObj_TTL.Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.SalesDOMasterPrintObj.Currency_ShortName
                + (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.SalesDOMasterPrintObj.Minor_ShortName + " only" : " only");



            string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/PrintHeaderInvoiceSalesInv_DO?c_ID=" + c_ID + "&DOMaster_ID=" + DOMaster_ID,

            "http://" + Host + "/Report/FooterHeaderInvoiceSalesInv?c_ID=" + c_ID);


            var demoViewPortrait = new ViewAsPdf(_oSales)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 48, Right = 5, Top = 70 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;


        }
        [AllowAnonymous]
        public IActionResult PrintHeaderInvoiceSalesInv_DO(int? c_ID, int? DOMaster_ID)
        {
            ViewBag.ReportCompanySalInv = c_ID;
            DOMaster_ID = (DOMaster_ID == null ? 0 : DOMaster_ID);
            _oSales.SalesDOMasterPrintObj = _oSales.Get_SalesDeliveryMasterPrintByID_DO_DAL(DOMaster_ID);
            _oSales.SalesDODetailPrintObj = _oSales.Get_SalesDeliveryOrderDetailListByID_DAL("", DOMaster_ID).ToList();
            _oSales.SalesDODetailPrintObj_TTL = _oSales.Get_SalesDeliveryOrderDetailPrintByID_DO_ttl(DOMaster_ID);
            return View("~/Views/Report/" + ReportFolder + "/PrintHeaderInvoiceSalesInv_DO.cshtml", _oSales);
        }
        [HttpGet]
        public IActionResult PurchaseInvoicePrint_GRV(int? GRVMaster_ID)
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



            _oReports.PurchaseMasterPrint_GRV = _oPurchase.Get_PurchaseMaster_GRV_DAL(GRVMaster_ID);
            _oReports.PurchaseDetailPrint_GRV = _oPurchase.Get_ReceivedDetailListByID_DAL("", GRVMaster_ID).ToList();




            string Host = Request.Host.ToString();


            //return View(_oReports);

            string customSwitches = string.Format("--header-spacing \"0\" --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/HEADERPurchaseInvoice_GRV?c_ID=" + c_ID,

              "http://" + Host + "/Report/FooterHeaderInvoiceSalesInv?c_ID=" + c_ID);




            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 48, Right = 5, Top = 25 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;



        }
        [AllowAnonymous]
        public IActionResult HEADERPurchaseInvoice_GRV(int? c_ID)
        {
            ViewBag.ReportCompanyPur = c_ID;
            return View("~/Views/Report/" + ReportFolder + "/HEADERPurchaseInvoice_GRV.cshtml");
        }

        [HttpGet]
        public IActionResult SalesInvoiceReturn_Print_trd(int? SaleMaster_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;
            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.SalesMasterPrintObj = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID);
            _oSales.SalesDetailPrintObj = _oSales.Get_SalesDetailListByID_DAL_Return("", SaleMaster_ID).ToList();
            _oSales.SalesDetailPrintObj_TTL = _oSales.Get_SalesInvoiceDetailPrintByID_ttl(SaleMaster_ID);

            string Host = Request.Host.ToString();

            string TotalAmount = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID).AfterVatTotal;
            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0";
            }
            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());


            _oSales.SalesDetailPrintObj_TTL.Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.SalesMasterPrintObj.Currency_ShortName
                + (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.SalesMasterPrintObj.Minor_ShortName + " only" : " only");



            string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/PrintHeaderInvoiceSalesInv_Return?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID,

            "http://" + Host + "/Report/FooterHeaderInvoiceSalesInv?c_ID=" + c_ID);


            var demoViewPortrait = new ViewAsPdf(_oSales)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 48, Right = 5, Top = 65 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;


        }
        [AllowAnonymous]
        public IActionResult PrintHeaderInvoiceSalesInv_Return(int? c_ID, int? SaleMaster_ID)
        {
            ViewBag.ReportCompanySalInv = c_ID;
            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.SalesMasterPrintObj = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID);
            _oSales.SalesDetailPrintObj = _oSales.Get_SalesDetailListByID_DAL_Return("", SaleMaster_ID).ToList();
            _oSales.SalesDetailPrintObj_TTL = _oSales.Get_SalesInvoiceDetailPrintByID_ttl(SaleMaster_ID);
            return View("~/Views/Report/" + ReportFolder + "/PrintHeaderInvoiceSalesInv_Return.cshtml", _oSales);
        }

        [HttpGet]
        public IActionResult Dismental_Print(int? Dismental_ID)
        {





            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion

            Dismental_ID = (Dismental_ID == null ? 0 : Dismental_ID);
            ViewBag.ReportCompanyTransfer = c_ID;
            _oInventory.DismentalMasterObj_Print = _oInventory.Get_DismentalMasterByID_DAL(Dismental_ID);

            //---Inventory detial invoice list for Inventory invoice type Bychassis
            _oInventory.DismentalDetailList_Print = _oInventory.Get_Dismental_Material_IN_DetailListBy_DAL("", Dismental_ID).ToList();






            var demoViewPortrait = new ViewAsPdf(_oInventory)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,





            };
            return demoViewPortrait;


        }

        [HttpGet]
        public IActionResult Assembly_Print(int? Assembly_ID)
        {





            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion

            Assembly_ID = (Assembly_ID == null ? 0 : Assembly_ID);
            ViewBag.ReportCompanyTransfer = c_ID;
            _oInventory.AssemblyMasterObj_Print = _oInventory.Get_AssemblyMasterByID_DAL(Assembly_ID);

            //---Inventory detial invoice list for Inventory invoice type Bychassis
            _oInventory.AssemblyDetailList_Print = _oInventory.Get_Assembly_Material_IN_DetailListBy_DAL("", Assembly_ID).ToList();






            var demoViewPortrait = new ViewAsPdf(_oInventory)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,





            };
            return demoViewPortrait;


        }
        [HttpGet]
        public IActionResult ProductFormula_Print(int? Formula_ID)
        {





            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion

            Formula_ID = (Formula_ID == null ? 0 : Formula_ID);
            ViewBag.ReportCompanyTransfer = c_ID;
            _oInventory.FormulaMasterObj_Print = _oInventory.Get_ProductFormulaMasterByID_DAL(Formula_ID);

            //---Inventory detial invoice list for Inventory invoice type Bychassis
            _oInventory.FormulaDetailList_Print = _oInventory.Get_ProductFormulaDetailListBy_DAL("", Formula_ID).ToList();






            var demoViewPortrait = new ViewAsPdf(_oInventory)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,





            };
            return demoViewPortrait;


        }
        [HttpGet]
        public IActionResult SalesInvoice_Print_SP_trd(int? SaleMaster_ID)
        {





            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;
            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.SalesMasterPrintObj = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID);
            _oSales.SalesDetailPrintObj = _oSales.Get_SalesIvoiceDetailListByChassis_DAL_TRD("", SaleMaster_ID).ToList();
            _oSales.SalesDetailPrintObj_TTL = _oSales.Get_SalesInvoiceDetailPrintByID_ttl(SaleMaster_ID);
            _oSales.SalesReceiptDetailsList_Print = _oSales.Get_ReceiptDetailSaleListByID_DAL_Invoice_Print(SaleMaster_ID);
            string Host = Request.Host.ToString();

            string TotalAmount = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID).AfterVatTotal;
            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0";
            }
            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());


            _oSales.SalesDetailPrintObj_TTL.Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.SalesMasterPrintObj.Currency_ShortName
                + (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.SalesMasterPrintObj.Minor_ShortName + " only" : " only");



            string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/PrintHeaderInvoiceSalesInv_sp?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID,
            //for spare parts top PrintHeaderInvoiceSalesInv_sp else PrintHeaderInvoiceSalesInv
            "http://" + Host + "/Report/FooterHeaderInvoiceSalesInv?c_ID=" + c_ID);


            var demoViewPortrait = new ViewAsPdf(_oSales)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 48, Right = 5, Top = 90 },
                //for spare parts top 90 else 60
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;


        }
        [AllowAnonymous]
        public IActionResult PrintHeaderInvoiceSalesInv_SP(int? c_ID, int? SaleMaster_ID)
        {
            ViewBag.ReportCompanySalInv = c_ID;
            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.SalesMasterPrintObj = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID);
            _oSales.SalesDetailPrintObj = _oSales.Get_SalesInvoiceDetailPrintByID_DAL(SaleMaster_ID).ToList();
            _oSales.SalesDetailPrintObj_TTL = _oSales.Get_SalesInvoiceDetailPrintByID_ttl(SaleMaster_ID);
            return View("~/Views/Report/" + ReportFolder + "/PrintHeaderInvoiceSalesInv_SP.cshtml", _oSales);
        }

        [HttpGet]
        public IActionResult SalesInvoice_Print_SP_trd_Booking(int? SaleMaster_ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;
            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.SalesMasterPrintObj = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID);
            _oSales.SalesDetailPrintObj = _oSales.Get_SalesInvoiceDetailPrintByID_DAL(SaleMaster_ID).ToList();
            _oSales.SalesDetailPrintObj_TTL = _oSales.Get_SalesInvoiceDetailPrintByID_ttl(SaleMaster_ID);
            _oSales.SalesReceiptDetailsList_Print = _oSales.Get_ReceiptDetailSaleListByID_DAL_Invoice_Print(SaleMaster_ID);
            string Host = Request.Host.ToString();

            string TotalAmount = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID).AfterVatTotal;
            if (string.IsNullOrEmpty(TotalAmount))
            {
                TotalAmount = "0";
            }
            TotalAmount = Regex.Replace(TotalAmount, @",", "");
            NumericWordsConverter converter = new NumericWordsConverter();
            Regex regex = new Regex(@"[^0]+");
            Match match = regex.Match(TotalAmount.Split(".")[1].ToString());





            _oSales.SalesDetailPrintObj_TTL.Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + _oSales.SalesMasterPrintObj.Currency_ShortName
                + (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + _oSales.SalesMasterPrintObj.Minor_ShortName + " only" : " only");

            ViewBag.CompanyReportCode = configuration.GetSection("AppSettings").GetSection("CompanyReportFolderCode").Value;
            if (ViewBag.CompanyReportCode == "AM" || ViewBag.CompanyReportCode == "FM" )
            {
                string customSwitches = string.Format("--header-spacing \"0\" --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",


                    "http://" + Host + "/Report/PrintHeaderInvoiceSaleBooking?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID,


        "http://" + Host + "/Report/FooterHeaderInvoiceSalesInv?c_ID=" + c_ID);



                var demoViewPortrait = new ViewAsPdf(_oSales)
                {



                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageMargins = { Left = 5, Bottom = 48, Right = 5, Top = 60 },
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                    CustomSwitches = customSwitches

          


                };
                return demoViewPortrait;
            }

            else
            {
                string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",



        "http://" + Host + "/Report/PrintHeaderInvoiceSalesInv_SP_Booking?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID,

           
         "http://" + Host + "/Report/FooterHeaderInvoiceSalesInv?c_ID=" + c_ID);


                var demoViewPortrait = new ViewAsPdf(_oSales)
                {



                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageMargins = { Left = 5, Bottom = 48, Right = 5, Top = 90 },
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                    CustomSwitches = customSwitches




                };
                return demoViewPortrait;

            }


         




        }
        [AllowAnonymous]
        public IActionResult PrintHeaderInvoiceSalesInv_SP_Booking(int? c_ID, int? SaleMaster_ID)
        {
            ViewBag.ReportCompanySalInv = c_ID;
            SaleMaster_ID = (SaleMaster_ID == null ? 0 : SaleMaster_ID);
            _oSales.SalesMasterPrintObj = _oSales.Get_SalesInvoiceMasterPrintByID_DAL(SaleMaster_ID);
            _oSales.SalesDetailPrintObj = _oSales.Get_SalesInvoiceDetailPrintByID_DAL(SaleMaster_ID).ToList();
            _oSales.SalesDetailPrintObj_TTL = _oSales.Get_SalesInvoiceDetailPrintByID_ttl(SaleMaster_ID);
            return View("~/Views/Report/" + ReportFolder + "/PrintHeaderInvoiceSalesInv_SP_Booking.cshtml", _oSales);
        }
        [HttpGet]
        public IActionResult PurchaseReturn_Print(int? PurchaseMaster_ID)
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



            _oReports.PurchaseMasterPrint = _oReports.GetDataForPurchaseMaster(PurchaseMaster_ID);
            _oReports.PurchaseDetailPrint = _oReports.GetDataForPurchasePrintDetail(PurchaseMaster_ID).ToList();
            _oReports.PurchaseStockSummaryPrint = _oReports.GetDataForStockSummaryPrintDetail(PurchaseMaster_ID).ToList();



            string Host = Request.Host.ToString();


            //return View(_oReports);

            string customSwitches = string.Format("--header-spacing \"0\" --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/HEADERPurchaseInvoice_Return?c_ID=" + c_ID,

              "http://" + Host + "/Report/FooterHeaderInvoiceSalesInv?c_ID=" + c_ID);




            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 48, Right = 5, Top = 25 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;


        }
        [AllowAnonymous]
        public IActionResult HEADERPurchaseInvoice_Return(int? c_ID)
        {
            ViewBag.ReportCompanyPur = c_ID;
            return View("~/Views/Report/" + ReportFolder + "/HEADERPurchaseInvoice_Return.cshtml");
        }

        //Dismental_Report_Print Report Nafeel

        public ActionResult Dismental_Report_Print(string Ref, string StartDate, string EndDate)
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

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "InventoryReportItemCard";
            ViewBag.BreadCumTitle = "Inventory Report";

            Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;


            ViewBag.Ref_Dismental = Ref;
            ViewBag.StartDate_Dismental = StartDate;
            ViewBag.EndDate_Dismental = EndDate;

            ViewBag.c_ID_Dismental = c_ID;


            _oInventory.DismentalMasterList_Print = _oInventory.Get_DismentalMaster_DAL(Ref, StartDate, EndDate, c_ID);


            var demoViewPortrait = new ViewAsPdf(_oInventory)
            {

                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageMargins = { Left = 1, Bottom = 20, Right = 1, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--footer-left \"  Printed At: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
        " --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""

            };
            return demoViewPortrait;
        }


        //Nafeel Assembly_Print_List
        [HttpGet]
        public IActionResult Assembly_Print_List(string Ref, string StartDate, string EndDate, int c_ID)
        {

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
         
            //---purchase Inventory master invoice list for page
            ViewBag.RecordsPerPage = 10;

            _oInventory.Get_AssembleMaster = _oInventory.Get_AssemblyMaster_DAL(Ref, StartDate, EndDate, c_ID);

            var demoViewPortrait = new ViewAsPdf(_oInventory)
            {

                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageMargins = { Left = 1, Bottom = 20, Right = 1, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--footer-left \"  Printed At: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
        " --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""

            };
            return demoViewPortrait;
        }



        //AssemblyList_Excel New Work Nafeel
        [HttpGet]
        public IActionResult AssemblyList_Excel(string Ref, string StartDate, string EndDate, int c_ID)
        {

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

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            Ref = String.IsNullOrEmpty(Ref) ? "" : Ref;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;

            _oInventory.Get_AssembleMaster = _oInventory.Get_AssemblyMaster_DAL(Ref, StartDate, EndDate, c_ID);

            using (var workbook = new XLWorkbook())
            {
                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("Get_AssembleMaster");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Ref";
                worksheet.Cell(currentRow, 2).Value = "P_Date";
                worksheet.Cell(currentRow, 3).Value = "CustomerRef";
                worksheet.Cell(currentRow, 4).Value = "Supervisor";



                foreach (var item in _oInventory.Get_AssembleMaster)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.Ref;
                    worksheet.Cell(currentRow, 2).Value = item.P_Date;
                    worksheet.Cell(currentRow, 3).Value = item.CustomerRef;
                    worksheet.Cell(currentRow, 4).Value = item.Supervisor;
                }


                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "AssemblyListExcel.xlsx");
                }
            }
        }


        //DismentalList_Excel New Work Nafeel
        [HttpGet]
        public IActionResult DismentalList_Excel(string Ref, string StartDate, string EndDate, int c_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Inventory";
            ViewBag.BreadCumAction = "DismentalList";

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


            _oInventory.DismentalMasterList_Print = _oInventory.Get_DismentalMaster_DAL(Ref, StartDate, EndDate, c_ID);


            using (var workbook = new XLWorkbook())
            {
                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("GetDismentalMaster");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Ref";
                worksheet.Cell(currentRow, 2).Value = "Date";
                worksheet.Cell(currentRow, 3).Value = "Customer Ref";
                worksheet.Cell(currentRow, 4).Value = "Chassis";
                worksheet.Cell(currentRow, 5).Value = "Supervisor";




                foreach (var item in _oInventory.DismentalMasterList_Print)
                {
                    currentRow++;

                    worksheet.Cell(currentRow, 1).Value = item.Ref;
                    worksheet.Cell(currentRow, 2).Value = item.P_Date;
                    worksheet.Cell(currentRow, 3).Value = item.CustomerRef;
                    worksheet.Cell(currentRow, 4).Value = item.Dismental_Chassis;
                    worksheet.Cell(currentRow, 5).Value = item.Supervisor;
                }

                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "GetDismentalMasterExcel.xlsx");

                }
            }
        }


        //New Purchases_Print_Received_List Nafeel
        #region Purchases_Print_ReceivedList
        [HttpGet]
        public IActionResult PurchasesReceived_Print_List(string GRVRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID, int? page)
        {
            //---Dumping static fields
            // DumpStaticFieldsPurchase();


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



            #endregion veiwbags area

            ViewBag.GRVRef_GRVOther = GRVRef;
            ViewBag.Vendor_ID_GRVOther = Vendor_ID;
            ViewBag.From_Date_GRVOther = From_Date;
            ViewBag.To_Date_GRVOther = To_Date;
            ViewBag.Status_ID_GRVOther = Status_ID;
            ViewBag.c_ID_GRVOther = c_ID;



            ViewBag.RecordsPerPage = 10;
            //---GRV master other  list for page
            _oPurchase.GRVOtherMasterList_Print = _oPurchase.Get_GoodsReceivedMaster_DAL(GRVRef, Vendor_ID, From_Date, To_Date, Status_ID, c_ID);

            var demoViewPortrait = new ViewAsPdf(_oPurchase)
            {

                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageMargins = { Left = 1, Bottom = 20, Right = 1, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--footer-left \"  Printed At: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" + " --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""

            };
            return demoViewPortrait;
        }

        [HttpGet]
        public IActionResult Purchases_Print_Excel(string GRVRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID, int? page)
        {

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

            GRVRef = String.IsNullOrEmpty(GRVRef) ? "" : GRVRef;
            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;
            From_Date = String.IsNullOrEmpty(From_Date) ? "" : From_Date;
            To_Date = String.IsNullOrEmpty(To_Date) ? "" : To_Date;
            Status_ID = Status_ID == null ? 0 : Status_ID;
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            c_ID = c_ID == null || c_ID == 0 ? c_IDs : c_ID;



            _oPurchase.GRVOtherMasterList_Print = _oPurchase.Get_GoodsReceivedMaster_DAL(GRVRef, Vendor_ID, From_Date, To_Date, Status_ID, c_ID);

            using (var workbook = new XLWorkbook())
            {
                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("GRVOtherMasterList_Print");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "GRVRef";
                worksheet.Cell(currentRow, 2).Value = "Vendor_Name";
                worksheet.Cell(currentRow, 3).Value = "GRVDate";
                worksheet.Cell(currentRow, 4).Value = "GRVStatus";
                worksheet.Cell(currentRow, 5).Value = "VAT_Rate";
                worksheet.Cell(currentRow, 6).Value = "Total_Amount";


                foreach (var item in _oPurchase.GRVOtherMasterList_Print)
                {
                    currentRow++;

                    worksheet.Cell(currentRow, 1).Value = item.GRVRef;
                    worksheet.Cell(currentRow, 2).Value = item.Vendor_Name;
                    worksheet.Cell(currentRow, 3).Value = item.GRVDate;

                    if (!string.IsNullOrEmpty(item.Total) && !string.IsNullOrWhiteSpace(item.Total))
                    {
                        worksheet.Cell(currentRow, 4).Value = Convert.ToDecimal(item.Total);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 4).Value = item.Total;
                    }

                    worksheet.Cell(currentRow, 5).Value = item.VAT_Rate;

                    if (!string.IsNullOrEmpty(item.Total_Amount) && !string.IsNullOrWhiteSpace(item.Total_Amount))
                    {
                        worksheet.Cell(currentRow, 6).Value = Convert.ToDecimal(item.Total_Amount);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 6).Value = item.Total_Amount;
                    }

                }

                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "PurchasesPrintExcel.xlsx");
                }
            }
        }



        #endregion

        //New Sales_Return_Print_List Nafeel
        #region Sales_Return_Print_List
        [HttpGet]
        public IActionResult Sales_Return_Print_List(string SaleRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string ItemId, int c_ID, int? page)
        {
            //---dumping static fields
            //DumpStaticFieldsSales();

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

            //---purchase sales master invoice list for page
            ViewBag.RecordsPerPage = 10;

            _oSales.Get_SalesMaster_Print = _oSales.Get_SalesMaster_DAL_Return(SaleRef, StartDate, EndDate, Customer_Name, ItemId, Status_ID, c_ID).ToPagedList(page ?? 1, 10);

            var demoViewPortrait = new ViewAsPdf(_oSales)
            {

                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageMargins = { Left = 1, Bottom = 20, Right = 1, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--footer-left \"  Printed At: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" + " --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""

            };
            return demoViewPortrait;
        }


        [HttpGet]
        public IActionResult Sale_ReturnList_Excel(string SaleRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string ItemId, int c_ID)
        {

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

            //---purchase sales master invoice list for page
            ViewBag.RecordsPerPage = 10;

            _oSales.DOOtherMasterList_Print = _oSales.Get_SalesDeliveryOrderMaster_DAL(SaleRef, StartDate, EndDate, Customer_Name, ItemId, Status_ID, c_ID);

            using (var workbook = new XLWorkbook())
            {
                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("DOOtherMasterList");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "DORef";
                worksheet.Cell(currentRow, 2).Value = "DODate";
                worksheet.Cell(currentRow, 3).Value = "CustomerRef";
                worksheet.Cell(currentRow, 4).Value = "Total_Amt";



                foreach (var item in _oSales.DOOtherMasterList_Print)
                {
                    currentRow++;

                    worksheet.Cell(currentRow, 1).Value = item.DORef;
                    worksheet.Cell(currentRow, 2).Value = item.DODate;
                    worksheet.Cell(currentRow, 3).Value = item.Customer_Name;

                    if (!string.IsNullOrEmpty(item.Total_Amt) && !string.IsNullOrWhiteSpace(item.Total_Amt))
                    {
                        worksheet.Cell(currentRow, 4).Value = Convert.ToDecimal(item.Total_Amt);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 4).Value = item.Total_Amt;
                    }


                }

                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "SaleReturnListExcel.xlsx");

                }
            }
        }

        #endregion

        //New SalesDOPrintList Nafeel
        #region SalesDOPrintList
        [HttpGet]
        public IActionResult SalesDO_Print_List(string SaleRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string ItemId, int c_ID)
        {
            //---dumping static fields
            //  DumpStaticFieldsSales();

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

            //---purchase sales master invoice list for page
            ViewBag.RecordsPerPage = 10;

            _oSales.DOOtherMasterList_Print = _oSales.Get_SalesDeliveryOrderMaster_DAL(SaleRef, StartDate, EndDate, Customer_Name, ItemId, Status_ID, c_ID);

            var demoViewPortrait = new ViewAsPdf(_oSales)
            {

                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageMargins = { Left = 1, Bottom = 20, Right = 1, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--footer-left \"  Printed At: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" + " --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""

            };
            return demoViewPortrait;
        }

        [HttpGet]
        public IActionResult SalesDO_List_Excel(string SaleRef, string StartDate, string EndDate, string Customer_Name, int Status_ID, string ItemId, int c_ID)
        {

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

            _oSales.DOOtherMasterList_Print = _oSales.Get_SalesDeliveryOrderMaster_DAL(SaleRef, StartDate, EndDate, Customer_Name, ItemId, Status_ID, c_ID);

            using (var workbook = new XLWorkbook())
            {
                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("DOOtherMasterList");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "DORef";
                worksheet.Cell(currentRow, 2).Value = "DODate";
                worksheet.Cell(currentRow, 3).Value = "CustomerRef";
                worksheet.Cell(currentRow, 4).Value = "Total_Amt";



                foreach (var item in _oSales.DOOtherMasterList_Print)
                {
                    currentRow++;

                    worksheet.Cell(currentRow, 1).Value = item.DORef;
                    worksheet.Cell(currentRow, 2).Value = item.Date;
                    worksheet.Cell(currentRow, 3).Value = item.Customer_Name;

                    if (!string.IsNullOrEmpty(item.Total_Amt) && !string.IsNullOrEmpty(item.Total_Amt))
                    {
                        worksheet.Cell(currentRow, 4).Value = Convert.ToDecimal(item.Total_Amt);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 4).Value = item.Total_Amt;
                    }


                }

                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "SalesDOListExcel.xlsx");

                }
            }
        }

        #endregion



        [HttpGet]
        public IActionResult Paper1(int? papers_ID)
        {





            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;
            papers_ID = (papers_ID == null ? 0 : papers_ID);
            _oSales.PaperAfterSalesMasterObj = _oSales.Get_Pa_Select_paperAfterSaleMaster_jp_ByID_print(papers_ID);
            _oSales.PaperAfterSalesDetailList = _oSales.Select_AfterSaleDetailList_DAL(papers_ID).ToList();
         
            string Host = Request.Host.ToString();


     
           


  

            //string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            //"http://" + Host + "/Report/PrintHeaderInvoiceSalesInv?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID,

            //"http://" + Host + "/Report/FooterHeaderInvoiceSalesInv?c_ID=" + c_ID);


            var demoViewPortrait = new ViewAsPdf(_oSales)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4




            };
            return demoViewPortrait;


        }
        public IActionResult Paper2(int? papers_ID)
        {





            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;
            papers_ID = (papers_ID == null ? 0 : papers_ID);
            _oSales.PaperAfterSalesMasterObj = _oSales.Get_Pa_Select_paperAfterSaleMaster_jp_ByID_print(papers_ID);
            _oSales.PaperAfterSalesDetailList = _oSales.Select_AfterSaleDetailList_DAL(papers_ID).ToList();

            string Host = Request.Host.ToString();








            //string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            //"http://" + Host + "/Report/PrintHeaderInvoiceSalesInv?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID,

            //"http://" + Host + "/Report/FooterHeaderInvoiceSalesInv?c_ID=" + c_ID);


            var demoViewPortrait = new ViewAsPdf(_oSales)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4




            };
            return demoViewPortrait;


        }
        public IActionResult Paper3(int? papers_ID)
        {





            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion
            ViewBag.IsPurchaseCost_JP_Enabled = configuration.GetSection("AppSettings").GetSection("IsPurchaseCost_JP_Enabled").Value;
            papers_ID = (papers_ID == null ? 0 : papers_ID);
            _oSales.PaperAfterSalesMasterObj = _oSales.Get_Pa_Select_paperAfterSaleMaster_jp_ByID_print(papers_ID);
            _oSales.PaperAfterSalesDetailList = _oSales.Select_AfterSaleDetailList_DAL(papers_ID).ToList();

            string Host = Request.Host.ToString();








            //string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            //"http://" + Host + "/Report/PrintHeaderInvoiceSalesInv?c_ID=" + c_ID + "&SaleMaster_ID=" + SaleMaster_ID,

            //"http://" + Host + "/Report/FooterHeaderInvoiceSalesInv?c_ID=" + c_ID);


            var demoViewPortrait = new ViewAsPdf(_oSales)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4




            };
            return demoViewPortrait;


        }
        [HttpGet]
        public IActionResult ShippingInfo_JP_Print(int ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion

            ID = (ID == 0 ? 0 : ID);
            _oReports.ShippingMasterObjJP_Print = _oStock.get_shipping_info_by_ID_Print(ID);
            _oReports.ShippingListJP_Print = _oStock.pa_Select_Shipping_Info_details_by_Print_ID(ID).ToList();


            string Host = Request.Host.ToString();

            //string TotalAmount = _oReports.pa_select_salesJP_Total_Print_DAL(SaleMaster_ID).GrandTotal.ToString();
            //if (string.IsNullOrEmpty(TotalAmount))
            //{
            //    TotalAmount = "0";
            //}
            //TotalAmount = Regex.Replace(TotalAmount, @",", "");
            //NumericWordsConverter converter = new NumericWordsConverter();
            //Regex regex = new Regex(@"[^0]+");
            //Match match = regex.Match(TotalAmount.Split(".")[1].ToString());


            //_oReports.SalesGrandTotals_Print.Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + " YEN "
            //    + (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + " YEN " + " only" : " only");



            string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/HeaderShippingJP?c_ID=" + c_ID,

            "http://" + Host + "/Report/FooterSalesInvJP?c_ID=" + c_ID);


            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 25, Right = 5, Top = 25 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;


        }

        [HttpGet]
        public IActionResult Vanning_JP_Print(int ID)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion

            ID = (ID == 0 ? 0 : ID);
            _oReports.VanningMasterObjJP_Print = _oStock.get_Vanning_Master_by_ID_Print(ID);
            _oReports.VanningListJP_Print = _oStock.get_Vanning_Details_by_ID_Print(ID).ToList();


            string Host = Request.Host.ToString();


            string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/HeaderVanningJP?c_ID=" + c_ID,

            "http://" + Host + "/Report/FooterSalesInvJP?c_ID=" + c_ID);


            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 25, Right = 5, Top = 25 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;


        }
        [AllowAnonymous]
        public IActionResult HeaderShippingJP(int? c_ID)
        {
            ViewBag.ReportCompanySalInv_jp = c_ID;

            return View("~/Views/Report/" + ReportFolder + "/HeaderShippingJP.cshtml");
        }
        [AllowAnonymous]
        public IActionResult HeaderVanningJP(int? c_ID)
        {
            ViewBag.ReportCompanySalInv_jp = c_ID;

            return View("~/Views/Report/" + ReportFolder + "/HeaderVanningJP.cshtml");
        }
        [HttpGet]
        public IActionResult PurchaseInvoice_JP_Print(int PurchaseMaster_ID)
        {





            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            string company_ID = HttpContext.Session.GetString("c_ID");
            int? c_ID = Convert.ToInt32(company_ID);
            #endregion

            PurchaseMaster_ID = (PurchaseMaster_ID == 0 ? 0 : PurchaseMaster_ID);
            _oStock.PurchaseMasterObj = _oStock.Get_PurchaseMasterJPMasterByID_Print(PurchaseMaster_ID);
            _oStock.PurchaseDetailList = _oStock.Get_PurchaseMasterJPDetailListByID_Print(PurchaseMaster_ID).ToList();
            _oStock.PurchaseDetailGrandTotal = _oStock.Get_purchaseDetailsTotal(PurchaseMaster_ID).FirstOrDefault();

            string Host = Request.Host.ToString();

            //string TotalAmount = _oStock.Get_purchaseDetailsTotal(PurchaseMaster_ID)..ToString();
            //if (string.IsNullOrEmpty(TotalAmount))
            //{
            //    TotalAmount = "0";
            //}
            //TotalAmount = Regex.Replace(TotalAmount, @",", "");
            //NumericWordsConverter converter = new NumericWordsConverter();
            //Regex regex = new Regex(@"[^0]+");
            //Match match = regex.Match(TotalAmount.Split(".")[1].ToString());


            //_oReports.SalesGrandTotals_Print.Total_Amt_inWords = converter.ToWords(Convert.ToInt64(TotalAmount.Split(".")[0])) + " " + " YEN "
            //    + (match.Success == true ? " and " + converter.ToWords(Convert.ToInt32(TotalAmount.Split(".")[1])) + " " + " YEN " + " only" : " only");



            string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/HeaderPurchaseJP?c_ID=" + c_ID,

            "http://" + Host + "/Report/FooterSalesInvJP?c_ID=" + c_ID);


            var demoViewPortrait = new ViewAsPdf(_oStock)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 25, Right = 5, Top = 25 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = customSwitches




            };
            return demoViewPortrait;


        }
        [AllowAnonymous]
        public IActionResult HeaderPurchaseJP(int? c_ID)
        {
            ViewBag.ReportCompanySalInv_jp = c_ID;

            return View("~/Views/Report/" + ReportFolder + "/HeaderPurchaseJP.cshtml");
        }
        [HttpGet]
        public ActionResult PaidTaxReport(string StartDate, string EndDate, int? page)
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


            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
           


            ViewBag.StartDate_PaidReport = StartDate;
            ViewBag.EndDate_PaidReport = EndDate;


            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "PaidTaxReport";

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Paid Tax Report";
            ViewBag.PageTitle = "Paid Tax Report";

   

            _oReports.PaidTax = _oAccounts.Get_PaidTax_Report_jp(StartDate, EndDate).ToPagedList(page ?? 1, 10);
            _oReports.PaidTax_TTL = _oAccounts.Get_PaidTax_Report_ttl_jp(StartDate, EndDate).ToList();
            return View(_oReports);

        }
        [HttpGet]
        public ActionResult PaidTaxReport_Search(string StartDate, string EndDate, int? page)
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

            //if (StartDate == null && EndDate == null)
            //{
            //    DateTime date = DateTime.Now;
            //    var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            //    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            //    StartDate = firstDayOfMonth.ToString("dd-MMM-yyyy");
            //    EndDate = lastDayOfMonth.ToString("dd-MMM-yyyy");
            //}
            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);



            ViewBag.StartDate_PaidReport = StartDate;
            ViewBag.EndDate_PaidReport = EndDate;


            //---getting all customers
            ViewBag.AllCustomers = _oBasic.Get_CustomersMaster_DAL(c_ID).ToList();

            _oReports.PaidTax = _oAccounts.Get_PaidTax_Report_jp(StartDate, EndDate).ToPagedList(page ?? 1, 10);
            _oReports.PaidTax_TTL = _oAccounts.Get_PaidTax_Report_ttl_jp(StartDate, EndDate).ToList();
            return PartialView("_PaidTaxReport", _oReports);

        }


        [HttpGet]
        public ActionResult ReceivedTaxReport(string StartDate, string EndDate, int? page)
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


            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);



            ViewBag.StartDate_ReceivedReport = StartDate;
            ViewBag.EndDate_ReceivedReport = EndDate;


            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "ReceivedTaxReport";

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Received Tax Report";
            ViewBag.PageTitle = "Received Tax Report";



            _oReports.ReceivedTax = _oAccounts.Get_ReceivedTax_Report_jp(StartDate, EndDate).ToPagedList(page ?? 1, 10);
            _oReports.ReceivedTax_TTL = _oAccounts.Get_ReceivedTax_Report_ttl_jp(StartDate, EndDate).ToList();
            return View(_oReports);

        }
        [HttpGet]
        public ActionResult ReceivedTaxReport_Search(string StartDate, string EndDate, int? page)
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

            //if (StartDate == null && EndDate == null)
            //{
            //    DateTime date = DateTime.Now;
            //    var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            //    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            //    StartDate = firstDayOfMonth.ToString("dd-MMM-yyyy");
            //    EndDate = lastDayOfMonth.ToString("dd-MMM-yyyy");
            //}
            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);



            ViewBag.StartDate_ReceivedReport = StartDate;
            ViewBag.EndDate_ReceivedReport = EndDate;


            //---getting all customers
            ViewBag.AllCustomers = _oBasic.Get_CustomersMaster_DAL(c_ID).ToList();

            _oReports.ReceivedTax = _oAccounts.Get_ReceivedTax_Report_jp(StartDate, EndDate).ToPagedList(page ?? 1, 10);
            _oReports.ReceivedTax_TTL = _oAccounts.Get_ReceivedTax_Report_ttl_jp(StartDate, EndDate).ToList();
            return PartialView("_ReceivedTaxReport", _oReports);

        }
     
        
        [HttpGet]
        public ActionResult RecycleReport(string StartDate, string EndDate,string ChassisNo, int? page)
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


            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            ChassisNo = (String.IsNullOrEmpty(ChassisNo) || String.IsNullOrWhiteSpace(ChassisNo) ? "" : ChassisNo);


            ViewBag.StartDate_ReceivedReport = StartDate;
            ViewBag.EndDate_ReceivedReport = EndDate;
            ViewBag.ChassisNo_RecycleReport = ChassisNo;

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "RecycleReport";

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Received Tax Report";
            ViewBag.PageTitle = "Received Tax Report";



            _oReports.Recycle = _oAccounts.Get_Recycle_Report_jp(StartDate, EndDate, ChassisNo).ToPagedList(page ?? 1, 10);
            _oReports.Recycle_TTL = _oAccounts.Get_Recycle_Report_ttl_jp(StartDate, EndDate, ChassisNo).ToList();
            return View(_oReports);

        }
       
        [HttpGet]
        public ActionResult RecycleReport_Search(string StartDate, string EndDate, string ChassisNo, int? page)
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

            //if (StartDate == null && EndDate == null)
            //{
            //    DateTime date = DateTime.Now;
            //    var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            //    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            //    StartDate = firstDayOfMonth.ToString("dd-MMM-yyyy");
            //    EndDate = lastDayOfMonth.ToString("dd-MMM-yyyy");
            //}
            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            ChassisNo = (String.IsNullOrEmpty(ChassisNo) || String.IsNullOrWhiteSpace(ChassisNo) ? "" : ChassisNo);



            ViewBag.StartDate_RecycleReport = StartDate;
            ViewBag.EndDate_RecycleReport = EndDate;
            ViewBag.ChassisNo_RecycleReport = ChassisNo;


            //---getting all customers
            ViewBag.AllCustomers = _oBasic.Get_CustomersMaster_DAL(c_ID).ToList();

            _oReports.Recycle = _oAccounts.Get_Recycle_Report_jp(StartDate, EndDate, ChassisNo).ToPagedList(page ?? 1, 10);
            _oReports.Recycle_TTL = _oAccounts.Get_Recycle_Report_ttl_jp(StartDate, EndDate, ChassisNo).ToList();
            return PartialView("_RecycleReport", _oReports);

        }

        [HttpGet]
        public ActionResult PaidTaxReport_Print(string StartDate, string EndDate)
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


            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);



            ViewBag.StartDate_PaidReport = StartDate;
            ViewBag.EndDate_PaidReport = EndDate;
            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "PaidTaxReport";
            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Paid Tax Report";
            ViewBag.PageTitle = "Paid Tax Report";



            _oAccounts.PaidTax_PR = _oAccounts.Get_PaidTax_Report_jp(StartDate, EndDate).ToList();
            _oAccounts.PaidTax_TTL_PR = _oAccounts.Get_PaidTax_Report_ttl_jp(StartDate, EndDate).ToList();

            string Host = Request.Host.ToString();

            string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/HeaderPrintAccount?c_ID=" + c_ID,

            "http://" + Host + "/Report/FooterPrintAccount?c_ID=" + c_ID);


            var demoViewPortrait = new ViewAsPdf(_oAccounts)
            {
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 25, Right = 5, Top = 25 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                /* CustomSwitches = customSwitches*/
            };
            return demoViewPortrait;
        }
       
        
        [HttpGet]
        public IActionResult PaidTaxReport_Excel(string StartDate, string EndDate)
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


            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);



            ViewBag.StartDate_PaidReport = StartDate;
            ViewBag.EndDate_PaidReport = EndDate;
            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "PaidTaxReport";
            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Paid Tax Report";
            ViewBag.PageTitle = "Paid Tax Report";



            _oAccounts.PaidTax_PR = _oAccounts.Get_PaidTax_Report_jp(StartDate, EndDate).ToList();
            _oAccounts.PaidTax_TTL_PR = _oAccounts.Get_PaidTax_Report_ttl_jp(StartDate, EndDate).ToList();


            using (var workbook = new XLWorkbook())
            {
                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("PaidTax_PR");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Chassis_No";
                worksheet.Cell(currentRow, 2).Value = "Date";
                worksheet.Cell(currentRow, 3).Value = "PriceTax";
                worksheet.Cell(currentRow, 4).Value = "AuctionFeeTax";
                worksheet.Cell(currentRow, 5).Value = "ReksoFeeTax";

                worksheet.Cell(currentRow, 6).Value = "TaxAmount";
                worksheet.Cell(currentRow, 7).Value = "Inspection_ChargesTax";
                worksheet.Cell(currentRow, 8).Value = "Berth_Carry_ChargesTax";
                worksheet.Cell(currentRow, 9).Value = "ExtraChargesFeeTax";

                worksheet.Cell(currentRow, 10).Value = "TaxAmount";
                worksheet.Cell(currentRow, 11).Value = "TaxAmountOthers";
                worksheet.Cell(currentRow, 12).Value = "Total";



                foreach (var item in _oAccounts.PaidTax_PR)
                {
                    currentRow++;

                    worksheet.Cell(currentRow, 1).Value = item.Chassis_No;
                    worksheet.Cell(currentRow, 2).Value = item.PurchaseDate;



                    if (!string.IsNullOrEmpty(item.PriceTax) && !string.IsNullOrWhiteSpace(item.PriceTax))
                    {
                        worksheet.Cell(currentRow, 3).Value = Convert.ToDecimal(item.PriceTax);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 3).Value = item.PriceTax;
                    }

                    if (!string.IsNullOrEmpty(item.AuctionFeeTax) && !string.IsNullOrWhiteSpace(item.AuctionFeeTax))
                    {
                        worksheet.Cell(currentRow, 4).Value = Convert.ToDecimal(item.AuctionFeeTax);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 4).Value = item.AuctionFeeTax;
                    }

                    if (!string.IsNullOrEmpty(item.ReksoFeeTax) && !string.IsNullOrWhiteSpace(item.ReksoFeeTax))
                    {
                        worksheet.Cell(currentRow, 5).Value = Convert.ToDecimal(item.ReksoFeeTax);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 5).Value = item.ReksoFeeTax;
                    }


                    if (!string.IsNullOrEmpty(item.TaxAmount) && !string.IsNullOrWhiteSpace(item.TaxAmount))
                    {
                        worksheet.Cell(currentRow, 6).Value = Convert.ToDecimal(item.TaxAmount);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 6).Value = item.TaxAmount;
                    }

                    if (!string.IsNullOrEmpty(item.Inspection_ChargesTax) && !string.IsNullOrWhiteSpace(item.Inspection_ChargesTax))
                    {
                        worksheet.Cell(currentRow, 7).Value = Convert.ToDecimal(item.Inspection_ChargesTax);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 7).Value = item.Inspection_ChargesTax;
                    }

                    if (!string.IsNullOrEmpty(item.Berth_Carry_ChargesTax) && !string.IsNullOrWhiteSpace(item.Berth_Carry_ChargesTax))
                    {
                        worksheet.Cell(currentRow, 8).Value = Convert.ToDecimal(item.Berth_Carry_ChargesTax);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 8).Value = item.Berth_Carry_ChargesTax;
                    }

                    if (!string.IsNullOrEmpty(item.ExtraChargesFeeTax) && !string.IsNullOrWhiteSpace(item.ExtraChargesFeeTax))
                    {
                        worksheet.Cell(currentRow, 9).Value = Convert.ToDecimal(item.ExtraChargesFeeTax);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 9).Value = item.ExtraChargesFeeTax;
                    }


                    if (!string.IsNullOrEmpty(item.TaxAmount) && !string.IsNullOrWhiteSpace(item.TaxAmount))
                    {
                        worksheet.Cell(currentRow, 10).Value = Convert.ToDecimal(item.TaxAmount);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 10).Value = item.TaxAmount;
                    }


                    if (!string.IsNullOrEmpty(item.TaxAmountOthers) && !string.IsNullOrWhiteSpace(item.TaxAmountOthers))
                    {
                        worksheet.Cell(currentRow, 11).Value = Convert.ToDecimal(item.TaxAmountOthers);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 11).Value = item.TaxAmountOthers;
                    }


                    if (!string.IsNullOrEmpty(item.Total) && !string.IsNullOrWhiteSpace(item.Total))
                    {
                        worksheet.Cell(currentRow, 12).Value = Convert.ToDecimal(item.Total);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 12).Value = item.Total;
                    }

                }

                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "PaidTaxReportExcel.xlsx");

                }
            }
        }










        //New ReceivedTaxReport_Print 
        //New ReceivedTaxReport_Excel Nafeel
        [HttpGet]
        public ActionResult ReceivedTaxReport_Print(string StartDate, string EndDate)
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


            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);



            ViewBag.StartDate_ReceivedReport = StartDate;
            ViewBag.EndDate_ReceivedReport = EndDate;


            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "ReceivedTaxReport";

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Received Tax Report";
            ViewBag.PageTitle = "Received Tax Report";




            _oAccounts.ReceivedTax_PR = _oAccounts.Get_ReceivedTax_Report_jp(StartDate, EndDate).ToList();
            _oAccounts.ReceivedTax_TTL_PR = _oAccounts.Get_ReceivedTax_Report_ttl_jp(StartDate, EndDate).ToList();

            string Host = Request.Host.ToString();

            string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/HeaderPrintAccount?c_ID=" + c_ID,

            "http://" + Host + "/Report/FooterPrintAccount?c_ID=" + c_ID);


            var demoViewPortrait = new ViewAsPdf(_oAccounts)
            {
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 25, Right = 5, Top = 25 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                /* CustomSwitches = customSwitches*/
            };
            return demoViewPortrait;

        }


        [HttpGet]
        public IActionResult ReceivedTaxReport_Excel(string StartDate, string EndDate)
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


            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);



            ViewBag.StartDate_PaidReport = StartDate;
            ViewBag.EndDate_PaidReport = EndDate;
            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "PaidTaxReport";
            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Paid Tax Report";
            ViewBag.PageTitle = "Paid Tax Report";



            _oAccounts.ReceivedTax_PR = _oAccounts.Get_ReceivedTax_Report_jp(StartDate, EndDate).ToList();
            _oAccounts.ReceivedTax_TTL_PR = _oAccounts.Get_ReceivedTax_Report_ttl_jp(StartDate, EndDate).ToList();


            using (var workbook = new XLWorkbook())
            {
                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("ReceivedTax_PR");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Chassis_No";
                worksheet.Cell(currentRow, 2).Value = "Date";
                worksheet.Cell(currentRow, 3).Value = "PriceTax";
                worksheet.Cell(currentRow, 4).Value = "AuctionFeeTax";
                worksheet.Cell(currentRow, 5).Value = "OfficeChargesTax";
                worksheet.Cell(currentRow, 6).Value = "Total";


                foreach (var item in _oAccounts.ReceivedTax_PR)
                {
                    currentRow++;

                    worksheet.Cell(currentRow, 1).Value = item.Chassis_No;
                    worksheet.Cell(currentRow, 2).Value = item.SaleDate;

                    if (!string.IsNullOrEmpty(item.PriceTax) && !string.IsNullOrWhiteSpace(item.PriceTax))
                    {
                        worksheet.Cell(currentRow, 3).Value = Convert.ToDecimal(item.PriceTax);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 3).Value = item.PriceTax;
                    }


                    if (!string.IsNullOrEmpty(item.AuctionFeeTax) && !string.IsNullOrWhiteSpace(item.AuctionFeeTax))
                    {
                        worksheet.Cell(currentRow, 4).Value = Convert.ToDecimal(item.AuctionFeeTax);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 4).Value = item.AuctionFeeTax;
                    }


                    if (!string.IsNullOrEmpty(item.OfficeChargesTax) && !string.IsNullOrWhiteSpace(item.OfficeChargesTax))
                    {
                        worksheet.Cell(currentRow, 5).Value = Convert.ToDecimal(item.OfficeChargesTax);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 5).Value = item.OfficeChargesTax;
                    }



                    if (!string.IsNullOrEmpty(item.Total) && !string.IsNullOrWhiteSpace(item.Total))
                    {
                        worksheet.Cell(currentRow, 6).Value = Convert.ToDecimal(item.Total);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 6).Value = item.Total;
                    }




                }

                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "ReceivedTaxReportExcel.xlsx");

                }
            }
        }







        //New RecycleReport_PR 
        //New RecycleReport_Excel Nafeel
        [HttpGet]
        public ActionResult RecycleReport_PR(string StartDate, string EndDate, string ChassisNo)
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


            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            ChassisNo = (String.IsNullOrEmpty(ChassisNo) || String.IsNullOrWhiteSpace(ChassisNo) ? "" : ChassisNo);


            ViewBag.StartDate_ReceivedReport = StartDate;
            ViewBag.EndDate_ReceivedReport = EndDate;


            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "RecycleReport";

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Received Tax Report";
            ViewBag.PageTitle = "Received Tax Report";

            _oReports.Recycle_PR = _oAccounts.Get_Recycle_Report_jp(StartDate, EndDate,ChassisNo).ToList();
            _oReports.Recycle_TTL_PR = _oAccounts.Get_Recycle_Report_ttl_jp(StartDate, EndDate,ChassisNo).ToList();

            string Host = Request.Host.ToString();

            string customSwitches = string.Format("--header-spacing \"0\"  --footer-spacing \"0\" --header-html {0} --footer-html {1}   ",

            "http://" + Host + "/Report/HeaderPrintAccount?c_ID=" + c_ID,

            "http://" + Host + "/Report/FooterPrintAccount?c_ID=" + c_ID);


            var demoViewPortrait = new ViewAsPdf(_oReports)
            {
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 25, Right = 5, Top = 25 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,

            };
            return demoViewPortrait;

        }


        [HttpGet]
        public IActionResult RecycleReport_Excel(string StartDate, string EndDate, string ChassisNo)
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


            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            ChassisNo = (String.IsNullOrEmpty(ChassisNo) || String.IsNullOrWhiteSpace(ChassisNo) ? "" : ChassisNo);


            ViewBag.StartDate_PaidReport = StartDate;
            ViewBag.EndDate_PaidReport = EndDate;
            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "PaidTaxReport";
            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Paid Tax Report";
            ViewBag.PageTitle = "Paid Tax Report";



            _oReports.Recycle_PR = _oAccounts.Get_Recycle_Report_jp(StartDate, EndDate,ChassisNo).ToList();
            _oReports.Recycle_TTL_PR = _oAccounts.Get_Recycle_Report_ttl_jp(StartDate, EndDate, ChassisNo).ToList();


            using (var workbook = new XLWorkbook())
            {
                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("Recycle_PR");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Chassis_No";
                worksheet.Cell(currentRow, 2).Value = "Purchase Date";
                worksheet.Cell(currentRow, 3).Value = "Sale Date";
                worksheet.Cell(currentRow, 4).Value = "Ref";
                worksheet.Cell(currentRow, 5).Value = "RecycleFee_Out";
                worksheet.Cell(currentRow, 6).Value = "RecycleFee_In";


                foreach (var item in _oReports.Recycle_PR)
                {
                    var Date = new string[4];
                    if (item.Date != "" && item.Date != null)
                    {
                        Date = item.Date.Split('/');

                    }

                    currentRow++;

                    worksheet.Cell(currentRow, 1).Value = item.Chassis_No;
                    worksheet.Cell(currentRow, 2).Value = @Date[0];
                    worksheet.Cell(currentRow, 3).Value = @Date[1];
                    worksheet.Cell(currentRow, 4).Value = item.Ref;
                    worksheet.Cell(currentRow, 5).Value = item.RecycleFee_Out;
                    worksheet.Cell(currentRow, 6).Value = item.RecycleFee_In;
                }




                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "RecycleReportExcel.xlsx");

                }
            }
        }


        [HttpGet]
        public IActionResult StockReportList_Complete_Excel(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_Exterior_ID, string PurchaseStartDate, string PurchaseEndDate,
       string BL_NO, string BOE, string PurchaseRef, string SaleRef, int c_ID, string Stock_Status, int loc_ID, string VendorName, string ModelYear, string Container_No, bool optional_cost, int StockType_ID)
        {
            using (var workbook = new XLWorkbook())
            {

                #region login checking
                string UserName_Admin = HttpContext.Session.GetString("UserName");
                string UserID_Admin = HttpContext.Session.GetString("UserID");
                if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
                {
                    return RedirectToAction("Index", "Login");
                }
                #endregion

                ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
                Make_ID = Make_ID == null ? 0 : Make_ID;
                MakeModel_description_ID = MakeModel_description_ID == null ? 0 : MakeModel_description_ID;
                Color_Exterior_ID = Color_Exterior_ID == null ? 0 : Color_Exterior_ID;
                PurchaseStartDate = String.IsNullOrEmpty(PurchaseStartDate) ? "" : PurchaseStartDate;
                PurchaseEndDate = String.IsNullOrEmpty(PurchaseEndDate) ? "" : PurchaseEndDate;
                string company_ID = HttpContext.Session.GetString("c_ID");
                int c_IDs = Convert.ToInt32(company_ID);
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

                ViewBag.isOptional_Cost = optional_cost;

                _oStock.StockListGetObject = _oStock.Get_StockList_Complete_DAL(ChassisNo, Make_ID, MakeModel_description_ID, Color_Exterior_ID, PurchaseStartDate, PurchaseEndDate, BL_NO, BOE, PurchaseRef, SaleRef, Stock_Status, loc_ID, c_ID, VendorName, ModelYear, Container_No, StockType_ID).ToList();





                var worksheet = workbook.Worksheets.Add("StockList");
                var currentRow = 1;


                worksheet.Cell(currentRow, 1).Value = "S.NO";
                worksheet.Cell(currentRow, 2).Value = "Lot No";
                worksheet.Cell(currentRow, 3).Value = "Chassis No";
                worksheet.Cell(currentRow, 4).Value = "Make";
                worksheet.Cell(currentRow, 5).Value = "Model";
                worksheet.Cell(currentRow, 6).Value = "Model Year";
                worksheet.Cell(currentRow, 7).Value = "Color";
                worksheet.Cell(currentRow, 8).Value = "Stock Type";
                worksheet.Cell(currentRow, 9).Value = "Vendor";
                worksheet.Cell(currentRow, 10).Value = "Purchase Date";
                worksheet.Cell(currentRow, 11).Value = "Purchase Ref";
                worksheet.Cell(currentRow, 12).Value = "Transmission";
                worksheet.Cell(currentRow, 13).Value = "Location";
                worksheet.Cell(currentRow, 14).Value = "From Location";
                worksheet.Cell(currentRow, 15).Value = "Going Location";
                worksheet.Cell(currentRow, 16).Value = "Rekso Name";
                worksheet.Cell(currentRow, 17).Value = "Price";
                worksheet.Cell(currentRow, 15).Value = "PriceTax";
                worksheet.Cell(currentRow, 18).Value = "Shipping";
                worksheet.Cell(currentRow, 19).Value = "Vanning";
                worksheet.Cell(currentRow, 20).Value = "Inspection";
                worksheet.Cell(currentRow, 21).Value = "Auction Fee";
                worksheet.Cell(currentRow, 22).Value = "Auction Fee Tax";
                worksheet.Cell(currentRow, 23).Value = "Plat Number Fee";
                worksheet.Cell(currentRow, 24).Value = "Rekso Fee";
                worksheet.Cell(currentRow, 25).Value = "Rekso Fee Tax";
                worksheet.Cell(currentRow, 26).Value = "Recycle Fee";
                worksheet.Cell(currentRow, 27).Value = "Total Exp";
                worksheet.Cell(currentRow, 28).Value = "Total Cost";
                worksheet.Cell(currentRow, 29).Value = "Paper Received";
                worksheet.Cell(currentRow, 30).Value = "Status";








                int counter = 0;
                foreach (var item in _oStock.StockListGetObject)
                {
                    currentRow++;
                    counter++;

                    worksheet.Cell(currentRow, 1).Value = counter;
                    worksheet.Cell(currentRow, 2).Value = item.LotNumber;
                    worksheet.Cell(currentRow, 3).Value = item.Chassis_No;
                    worksheet.Cell(currentRow, 4).Value = item.Make;
                    worksheet.Cell(currentRow, 5).Value = item.ModelDesciption;
                    worksheet.Cell(currentRow, 6).Value = item.ModelYear;
                    worksheet.Cell(currentRow, 7).Value = item.Color;
                    worksheet.Cell(currentRow, 8).Value = item.StockTypeName;
                    worksheet.Cell(currentRow, 9).Value = item.Vendor_Name;
                    worksheet.Cell(currentRow, 10).Value = item.PurchaseDate;
                    worksheet.Cell(currentRow, 11).Value = item.PurchaseRef;
                    worksheet.Cell(currentRow, 12).Value = item.Transmission;
                    worksheet.Cell(currentRow, 13).Value = item.CarLocation;
                    worksheet.Cell(currentRow, 14).Value = item.FromLoc;
                    worksheet.Cell(currentRow, 15).Value = item.GoingLoc;
                    worksheet.Cell(currentRow, 16).Value = item.ReksoName;
                    worksheet.Cell(currentRow, 17).Value = item.PriceOrignal;


                    if (!string.IsNullOrEmpty(item.PriceTax) && !string.IsNullOrWhiteSpace(item.PriceTax))
                    {
                        worksheet.Cell(currentRow, 15).Value = Convert.ToDecimal(item.PriceTax);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 15).Value = item.PriceTax;
                    }

                    if (!string.IsNullOrEmpty(item.Shipping_Charges) && !string.IsNullOrWhiteSpace(item.Shipping_Charges))
                    {
                        worksheet.Cell(currentRow, 18).Value = Convert.ToDecimal(item.Shipping_Charges);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 18).Value = item.Shipping_Charges;
                    }

                    worksheet.Cell(currentRow, 19).Value = item.Vanning_Charges;

                    if (!string.IsNullOrEmpty(item.Inspection_Charges) && !string.IsNullOrWhiteSpace(item.Inspection_Charges))
                    {
                        worksheet.Cell(currentRow, 20).Value = Convert.ToDecimal(item.Inspection_Charges);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 20).Value = item.Inspection_Charges;
                    }

                    if (!string.IsNullOrEmpty(item.AuctionFee) && !string.IsNullOrWhiteSpace(item.AuctionFee))
                    {
                        worksheet.Cell(currentRow, 21).Value = Convert.ToDecimal(item.AuctionFee);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 21).Value = item.AuctionFee;
                    }



                    if (!string.IsNullOrEmpty(item.AuctionFeeTax) && !string.IsNullOrWhiteSpace(item.AuctionFeeTax))
                    {
                        worksheet.Cell(currentRow, 22).Value = Convert.ToDecimal(item.AuctionFeeTax);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 22).Value = item.AuctionFeeTax;
                    }

                    if (!string.IsNullOrEmpty(item.PlatNumberFee) && !string.IsNullOrWhiteSpace(item.PlatNumberFee))
                    {
                        worksheet.Cell(currentRow, 23).Value = Convert.ToDecimal(item.PlatNumberFee);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 23).Value = item.PlatNumberFee;
                    }


                    if (!string.IsNullOrEmpty(item.ReksoFee) && !string.IsNullOrWhiteSpace(item.ReksoFee))
                    {
                        worksheet.Cell(currentRow, 24).Value = Convert.ToDecimal(item.ReksoFee);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 24).Value = item.ReksoFee;
                    }


                    if (!string.IsNullOrEmpty(item.ReksoFeeTax) && !string.IsNullOrWhiteSpace(item.ReksoFeeTax))
                    {
                        worksheet.Cell(currentRow, 25).Value = Convert.ToDecimal(item.ReksoFeeTax);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 25).Value = item.ReksoFeeTax;
                    }


                    if (!string.IsNullOrEmpty(item.RecycleFee) && !string.IsNullOrWhiteSpace(item.RecycleFee))
                    {
                        worksheet.Cell(currentRow, 26).Value = Convert.ToDecimal(item.RecycleFee);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 26).Value = item.RecycleFee;
                    }



                    if (!string.IsNullOrEmpty(item.Total_Expense) && !string.IsNullOrWhiteSpace(item.Total_Expense))
                    {
                        worksheet.Cell(currentRow, 27).Value = Convert.ToDecimal(item.Total_Expense);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 27).Value = item.Total_Expense;
                    }


                    if (!string.IsNullOrEmpty(item.Total_Cost) && !string.IsNullOrWhiteSpace(item.Total_Cost))
                    {
                        worksheet.Cell(currentRow, 28).Value = Convert.ToDecimal(item.Total_Cost);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 28).Value = item.Total_Cost;
                    }

                    worksheet.Cell(currentRow, 29).Value = item.PaperReceived;
                    worksheet.Cell(currentRow, 30).Value = item.Stock_Status;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "StockList.xlsx");
                }
            }

        }









        //InventoryPrintForm Nafeel Work 
        [HttpGet]
        public ActionResult InventoryPrintForm()
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
            #region viewbag area



            #endregion



            //---Make list for page
            _oBasic.InventryMasterObj = _oBasic.Get_InventryMasterList_DAL();

            var demoViewPortrait = new ViewAsPdf(_oBasic)
            {
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--footer-left \"  Printed At: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" + " --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""
            };
            return demoViewPortrait;

        }

    


        [HttpGet]
        public IActionResult InventoryPrintFormExcel(string StartDate, string EndDate)
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


            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);



            ViewBag.StartDate_PaidReport = StartDate;
            ViewBag.EndDate_PaidReport = EndDate;
            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "InventoryReport";
            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Inventory Report";
            ViewBag.PageTitle = "Inventory Report";




            //---Make list for page
            _oBasic.InventryMasterObj = _oBasic.Get_InventryMasterList_DAL();

            using (var workbook = new XLWorkbook())
            {
                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("InventryMasterList");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Item_ID";
                worksheet.Cell(currentRow, 2).Value = "ItemCode";
                worksheet.Cell(currentRow, 3).Value = "ItemName";
                worksheet.Cell(currentRow, 4).Value = "ItemDescription";
                worksheet.Cell(currentRow, 5).Value = "Created_at";
                worksheet.Cell(currentRow, 6).Value = "Created_by";

                worksheet.Cell(currentRow, 7).Value = "Modified_at";
                worksheet.Cell(currentRow, 8).Value = "Modified_by";
                worksheet.Cell(currentRow, 9).Value = "Purchase_Qty";
                worksheet.Cell(currentRow, 10).Value = "Sold_Qty";
                worksheet.Cell(currentRow, 11).Value = "InHand_Qty";
                worksheet.Cell(currentRow, 12).Value = "IsSerializable";

                worksheet.Cell(currentRow, 13).Value = "BarCode";
                worksheet.Cell(currentRow, 14).Value = "Unit_Price";
                worksheet.Cell(currentRow, 15).Value = "Sale_Price";
                worksheet.Cell(currentRow, 16).Value = "CostMethod";
                worksheet.Cell(currentRow, 17).Value = "Comment";
                worksheet.Cell(currentRow, 18).Value = "UOM";

                worksheet.Cell(currentRow, 19).Value = "Group_ID";
                worksheet.Cell(currentRow, 20).Value = "Category_ID";
                worksheet.Cell(currentRow, 21).Value = "GroupName";
                worksheet.Cell(currentRow, 22).Value = "CategoryName";
                worksheet.Cell(currentRow, 23).Value = "Traditional";
                worksheet.Cell(currentRow, 24).Value = "FUEL_TYPE";

                worksheet.Cell(currentRow, 25).Value = "Transmission";
                worksheet.Cell(currentRow, 26).Value = "Drive";
                worksheet.Cell(currentRow, 27).Value = "StartYear";
                worksheet.Cell(currentRow, 28).Value = "EndYear";
                worksheet.Cell(currentRow, 29).Value = "EngineSpecsCode";
                worksheet.Cell(currentRow, 30).Value = "Year";

                worksheet.Cell(currentRow, 31).Value = "Received_Qty";
                worksheet.Cell(currentRow, 32).Value = "Delivered_Qty";
                worksheet.Cell(currentRow, 33).Value = "Return_Qty";
                worksheet.Cell(currentRow, 34).Value = "OpeningBal";

                foreach (var item in _oBasic.InventryMasterObj)
                {

                    currentRow++;

                    worksheet.Cell(currentRow, 1).Value = item.Item_ID;
                    worksheet.Cell(currentRow, 2).Value = item.ItemCode;
                    worksheet.Cell(currentRow, 3).Value = item.ItemName;
                    worksheet.Cell(currentRow, 4).Value = item.ItemDescription;
                    worksheet.Cell(currentRow, 5).Value = item.Created_at; 
                    worksheet.Cell(currentRow, 6).Value = item.Created_by;

                    worksheet.Cell(currentRow, 7).Value = item.Modified_at;
                    worksheet.Cell(currentRow, 8).Value = item.Modified_by;
                    worksheet.Cell(currentRow, 9).Value = item.Purchase_Qty;


                    worksheet.Cell(currentRow, 10).Value = item.Sold_Qty;
                    worksheet.Cell(currentRow, 11).Value = item.InHand_Qty;
                    worksheet.Cell(currentRow, 12).Value = item.IsSerializable;

                    worksheet.Cell(currentRow, 13).Value = item.BarCode;

                    if (!string.IsNullOrEmpty(item.UnitPrice) && !string.IsNullOrWhiteSpace(item.UnitPrice))
                    {
                        worksheet.Cell(currentRow, 14).Value = Convert.ToDecimal(item.UnitPrice);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 14).Value = item.UnitPrice;
                    }


                    if (!string.IsNullOrEmpty(item.SalePrice) && !string.IsNullOrWhiteSpace(item.SalePrice))
                    {
                        worksheet.Cell(currentRow, 15).Value = Convert.ToDecimal(item.SalePrice);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 15).Value = item.SalePrice;
                    }


                    worksheet.Cell(currentRow, 16).Value = item.CostMethod;
                    worksheet.Cell(currentRow, 17).Value = item.Comment;
                    worksheet.Cell(currentRow, 18).Value = item.UOM;

                    worksheet.Cell(currentRow, 19).Value = item.Group_ID;
                    worksheet.Cell(currentRow, 20).Value = item.Category_ID;
                    worksheet.Cell(currentRow, 21).Value = item.GroupName;
                    worksheet.Cell(currentRow, 22).Value = item.CategoryName;
                    worksheet.Cell(currentRow, 23).Value = item.Traditional;
                    worksheet.Cell(currentRow, 24).Value = item.FUEL_TYPE;

                    worksheet.Cell(currentRow, 25).Value = item.Transmission;
                    worksheet.Cell(currentRow, 26).Value = item.Drive;
                    worksheet.Cell(currentRow, 27).Value = item.StartYear;
                    worksheet.Cell(currentRow, 28).Value = item.EndYear;
                    worksheet.Cell(currentRow, 29).Value = item.EngineSpecsCode;
                    worksheet.Cell(currentRow, 30).Value = item.Year;

                    worksheet.Cell(currentRow, 31).Value = item.Received_Qty;
                    worksheet.Cell(currentRow, 32).Value = item.Delivered_Qty;
                    worksheet.Cell(currentRow, 33).Value = item.Return_Qty;

                    if (!string.IsNullOrEmpty(item.OpeningBal) && !string.IsNullOrWhiteSpace(item.OpeningBal))
                    {
                        worksheet.Cell(currentRow, 34).Value = Convert.ToDecimal(item.OpeningBal);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 34).Value = item.OpeningBal;
                    }

                }

                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "InventoryPrintFormExcel.xlsx");

                }
            }




        }



        //BY YASEEN 1-12-2022

        #region BL Report
        [HttpGet]

        public ActionResult BLReport(string StartDate, string EndDate, string BLNO_Ref, string chassis, int? page)
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

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "BL Report";
            ViewBag.PageTitle = "BL Report";
            

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "BLReport";



            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            BLNO_Ref = (String.IsNullOrEmpty(BLNO_Ref) || String.IsNullOrWhiteSpace(BLNO_Ref) ? "" : BLNO_Ref);
            chassis = (String.IsNullOrEmpty(chassis) || String.IsNullOrWhiteSpace(chassis) ? "" : chassis);



            ViewBag.StartDate_BLReport = StartDate;
            ViewBag.EndDate_BLReport = EndDate;
            ViewBag.BLNO_Ref_BLReport = BLNO_Ref;
            ViewBag.chassis_BLReport = chassis;

            _oReports.BLReport = _oReports.pa_Select_BLReport_DAL(StartDate, EndDate, BLNO_Ref, c_ID, chassis).ToPagedList(page ?? 1, 10);




            return View(_oReports);
        }
        [HttpGet]
        public ActionResult BLReport_Search(string StartDate, string EndDate, string BLNO_Ref, string chassis, int? page)
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

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "BL Report";
            ViewBag.PageTitle = "BL Report";


            //Reporting
            TempData["StartBL"] = StartDate;
            TempData["EndBL"] = EndDate;

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "BLReport";



            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            BLNO_Ref = (String.IsNullOrEmpty(BLNO_Ref) || String.IsNullOrWhiteSpace(BLNO_Ref) ? "" : BLNO_Ref);
            chassis = (String.IsNullOrEmpty(chassis) || String.IsNullOrWhiteSpace(chassis) ? "" : chassis);



            ViewBag.StartDate_BLReport = StartDate;
            ViewBag.EndDate_BLReport = EndDate;
            ViewBag.BLNO_Ref_BLReport = BLNO_Ref;
            ViewBag.chassis_BLReport = chassis;

            _oReports.BLReport = _oReports.pa_Select_BLReport_DAL(StartDate, EndDate, BLNO_Ref, c_ID, chassis).ToPagedList(page ?? 1, 10);




            return PartialView("_BLReportSearch", _oReports);
        }

        [HttpGet]
        public IActionResult BLReport_Print(string StartDate, string EndDate, string BLNO_Ref, string chassis)
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

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);

            chassis = (String.IsNullOrEmpty(chassis) || String.IsNullOrWhiteSpace(chassis) ? "" : chassis);
            BLNO_Ref = (String.IsNullOrEmpty(BLNO_Ref) || String.IsNullOrWhiteSpace(BLNO_Ref) ? "" : BLNO_Ref);


            _oReports.BLReportPrint = _oReports.pa_Select_BLReport_DAL(StartDate, EndDate, BLNO_Ref, c_ID, chassis).ToList();

            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
 "--footer-left \"  Printed At: " +
DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
" --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };
            return demoViewPortrait;

        }

        [HttpGet]
        public IActionResult BLReport_Excel(string StartDate, string EndDate, string BLNO_Ref, string chassis)
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
            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);

            chassis = (String.IsNullOrEmpty(chassis) || String.IsNullOrWhiteSpace(chassis) ? "" : chassis);
            BLNO_Ref = (String.IsNullOrEmpty(BLNO_Ref) || String.IsNullOrWhiteSpace(BLNO_Ref) ? "" : BLNO_Ref);


            _oReports.BLReportPrint = _oReports.pa_Select_BLReport_DAL(StartDate, EndDate, BLNO_Ref, c_ID, chassis).ToList();

            using (var workbook = new XLWorkbook())
            {

                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("BLReportPrint");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Date";
                worksheet.Cell(currentRow, 2).Value = "BLNO_Ref";
                worksheet.Cell(currentRow, 3).Value = "Chassis_no";
                worksheet.Cell(currentRow, 4).Value = "Make";
                worksheet.Cell(currentRow, 5).Value = "Model_description";
                worksheet.Cell(currentRow, 6).Value = "Year";
                worksheet.Cell(currentRow, 7).Value = "BL Expense";
                worksheet.Cell(currentRow, 8).Value = "Total_Expense";
                worksheet.Cell(currentRow, 9).Value = "Total Cost";

                foreach (var item in _oReports.BLReportPrint)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.Date;
                    worksheet.Cell(currentRow, 2).Value = item.BLNO_Ref;
                    worksheet.Cell(currentRow, 3).Value = item.Chassis_no;
                    worksheet.Cell(currentRow, 4).Value = item.make;
                    worksheet.Cell(currentRow, 5).Value = item.model_description;
                    worksheet.Cell(currentRow, 6).Value = item.Year;
                    worksheet.Cell(currentRow, 6).Value = item.Bl_Exp;
                    worksheet.Cell(currentRow, 7).Value = item.Total_Expense;
                    worksheet.Cell(currentRow, 8).Value = item.Total_Cost;

                    if (!string.IsNullOrEmpty(item.Total_Expense) && !string.IsNullOrWhiteSpace(item.Total_Expense))
                    {
                        worksheet.Cell(currentRow, 9).Value = Convert.ToDecimal(item.Total_Expense);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 9).Value = item.Total_Expense;
                    }


                    if (!string.IsNullOrEmpty(item.Total_Cost) && !string.IsNullOrWhiteSpace(item.Total_Cost))
                    {
                        worksheet.Cell(currentRow, 10).Value = Convert.ToDecimal(item.Total_Cost);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 10).Value = item.Total_Cost;
                    }

                }

                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "BLReport.xlsx");
                }
            }
        }


        #endregion

        //BY YASEEN 1-12-2022


        //BY YASEEN 1-13-2022

        #region Purchase Report


        [HttpGet]
        public ActionResult PurchaseReport(string StartDate, string EndDate, string Pur_Ref, string chassis, int? page)
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

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Purchase Report";
            ViewBag.PageTitle = "Purchase Report";


            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "PurchaseReport";



            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            Pur_Ref = (String.IsNullOrEmpty(Pur_Ref) || String.IsNullOrWhiteSpace(Pur_Ref) ? "" : Pur_Ref);
            chassis = (String.IsNullOrEmpty(chassis) || String.IsNullOrWhiteSpace(chassis) ? "" : chassis);



            ViewBag.StartDate_PurchaseReport = StartDate;
            ViewBag.EndDate_PurchaseReport = EndDate;
            ViewBag.Pur_Ref_PurchaseReport = Pur_Ref;
            ViewBag.chassis_PurchaseReport = chassis;

            _oReports.PurchaseReportList = _oReports.pa_Select_PurchaseReport_DAL(StartDate, EndDate, Pur_Ref, c_ID, chassis).ToPagedList(page ?? 1, 10);


            return View(_oReports);
        }
        [HttpGet]
        public ActionResult PurchaseReport_Search(string StartDate, string EndDate, string Pur_Ref, string chassis, int? page)
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

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Purchase Report";
            ViewBag.PageTitle = "Purchase Report";


            //Reporting
            TempData["StartPur"] = StartDate;
            TempData["EndPur"] = EndDate;

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "PurchaseReport";



            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
            Pur_Ref = (String.IsNullOrEmpty(Pur_Ref) || String.IsNullOrWhiteSpace(Pur_Ref) ? "" : Pur_Ref);
            chassis = (String.IsNullOrEmpty(chassis) || String.IsNullOrWhiteSpace(chassis) ? "" : chassis);



            ViewBag.StartDate_PurchaseReport = StartDate;
            ViewBag.EndDate_PurchaseReport = EndDate;
            ViewBag.Pur_Ref_PurchaseReport = Pur_Ref;
            ViewBag.chassis_PurchaseReport = chassis;

            _oReports.PurchaseReportList = _oReports.pa_Select_PurchaseReport_DAL(StartDate, EndDate, Pur_Ref, c_ID, chassis).ToPagedList(page ?? 1, 10);

            return PartialView("_PurchaseReportSearch", _oReports);
        }


        [HttpGet]
        public IActionResult PurchaseReport_Print(string StartDate, string EndDate, string Pur_Ref, string chassis)
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

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);

            chassis = (String.IsNullOrEmpty(chassis) || String.IsNullOrWhiteSpace(chassis) ? "" : chassis);
            Pur_Ref = (String.IsNullOrEmpty(Pur_Ref) || String.IsNullOrWhiteSpace(Pur_Ref) ? "" : Pur_Ref);


            _oReports.PurchaseReportPrint = _oReports.pa_Select_PurchaseReport_DAL(StartDate, EndDate, Pur_Ref, c_ID, chassis).ToList();
          
            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
 "--footer-left \"  Printed At: " +
DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
" --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };
            return demoViewPortrait;

        }

        [HttpGet]
        public IActionResult PurchaseReport_Excel(string StartDate, string EndDate, string Pur_Ref, string chassis)
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
            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);

            chassis = (String.IsNullOrEmpty(chassis) || String.IsNullOrWhiteSpace(chassis) ? "" : chassis);
            Pur_Ref = (String.IsNullOrEmpty(Pur_Ref) || String.IsNullOrWhiteSpace(Pur_Ref) ? "" : Pur_Ref);

            _oReports.PurchaseReportPrint = _oReports.pa_Select_PurchaseReport_DAL(StartDate, EndDate, Pur_Ref, c_ID, chassis).ToList();
            using (var workbook = new XLWorkbook())
            {

                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("PurchaseReportPrint");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Date";
                worksheet.Cell(currentRow, 2).Value = "Pur_Ref";
                worksheet.Cell(currentRow, 3).Value = "Chassis_no";
                worksheet.Cell(currentRow, 4).Value = "Make";
                worksheet.Cell(currentRow, 5).Value = "Model_description";
                worksheet.Cell(currentRow, 6).Value = "Year";
                worksheet.Cell(currentRow, 7).Value = "Color";
                worksheet.Cell(currentRow, 8).Value = "Purchase_Amount";
                
                foreach (var item in _oReports.PurchaseReportPrint)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.Date;
                    worksheet.Cell(currentRow, 2).Value = item.Pur_Ref;
                    worksheet.Cell(currentRow, 3).Value = item.Chassis_no;
                    worksheet.Cell(currentRow, 4).Value = item.make;
                    worksheet.Cell(currentRow, 5).Value = item.model_description;
                    worksheet.Cell(currentRow, 6).Value = item.Year;
                    worksheet.Cell(currentRow, 6).Value = item.color;
                    worksheet.Cell(currentRow, 7).Value = item.Purchase_Amount;
                    
                    if (!string.IsNullOrEmpty(item.Purchase_Amount) && !string.IsNullOrWhiteSpace(item.Purchase_Amount))
                    {
                        worksheet.Cell(currentRow, 7).Value = Convert.ToDecimal(item.Purchase_Amount);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 7).Value = item.Purchase_Amount;
                    }
                }

                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "PurchaseReport.xlsx");
                }
            }
        }


        #endregion

        //BY YASEEN 1-13-2022






        //BY YASEEN 1-26-2022

        //This method is for getting Ledger list

        #region  Saller Report 

        [HttpGet]
        public ActionResult SallerReport(string StartDate, string EndDate ,int? SallerID, int? page)
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

            ViewBag.StartDate_SallerReport = StartDate;
            ViewBag.EndDate_SallerReport = EndDate;
           // ViewBag.Ref_SallerReport = Ref;
            ViewBag.SallerID_SallerReport = SallerID;

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "SallerReport";

            ViewBag.SectionHeaderTitle = "Reports";
            ViewBag.SectionSub_HeaderTitle = "Saller Report";
            ViewBag.PageTitle = "Saller Report";

            ViewBag.AllSallers = _oReports.Get_SellersMasterList_DAL(c_ID).Where(z => z.PartnerType == "SR").ToList();



            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);

            _oReports.LedgerPagedList = _oReports.pa_Select_SallarLedger_DAL(StartDate, EndDate,SallerID.ToString(), c_ID).ToPagedList(page ?? 1, 10);
            
            _oReports.Ledger_TTL = _oReports.pa_Select_SallerLedger_DAL_TTL(StartDate, EndDate, SallerID.ToString(), c_ID).ToList();


            return View(_oReports);
        }
        [HttpGet]
        public ActionResult SallerReport_Search(string StartDate, string EndDate, int? SallerID, int? page)
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
           


            ViewBag.StartDate_SallerReport = StartDate;
            ViewBag.EndDate_SallerReport = EndDate;
            //ViewBag.Ref_SallerReport = Ref;
            ViewBag.SallerID_SallerReport = SallerID;

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "SallerReport";


            //for Reporting purpuse            
            TempData["Start2"] = StartDate;
            TempData["End2"] = EndDate;
            TempData["VendorId"] = SallerID;

            ViewBag.AllSallers = _oReports.Get_SellersMasterList_DAL(c_ID).Where(z => z.PartnerType == "SR").ToList();

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);

            var partyName = _oReports.LedgerPagedList = _oReports.pa_Select_SallarLedger_DAL(StartDate, EndDate, SallerID.ToString(), c_ID).ToPagedList(page ?? 1, 10);

            if (partyName.Count() > 0)
            {
                TempData["PartyName"] = partyName[0].PartyName;

            }
            
            _oReports.Ledger_TTL = _oReports.pa_Select_SallerLedger_DAL_TTL(StartDate, EndDate, SallerID.ToString(), c_ID).ToList();

            return PartialView("_SallerReportSearch", _oReports);
        }


        #endregion


        #region Vat Report
        [HttpGet]
        public ActionResult VAT_IN(string StartDate, string EndDate, int? page)
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

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "VAT_IN";
            ViewBag.PageTitle = "VAT IN";




            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);
    

      
            ViewBag.StartDate_VAT_INReport = StartDate;
            ViewBag.EndDate_VAT_INReport = EndDate;
   

            _oReports.VAT_INReportReport = _oReports.pa_Select_VAT_IN_DAL(StartDate, EndDate, c_IDs).ToPagedList(page ?? 1, 10);
           
    
            return View(_oReports);
        }
        [HttpGet]
        public ActionResult VAT_IN_Search(string StartDate, string EndDate, int? page)
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

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "VAT_IN";




            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);



            ViewBag.StartDate_VAT_INReport = StartDate;
            ViewBag.EndDate_VAT_INReport = EndDate;


            _oReports.VAT_INReportReport = _oReports.pa_Select_VAT_IN_DAL(StartDate, EndDate, c_IDs).ToPagedList(page ?? 1, 10);
            

           
            return PartialView("_VAT_IN", _oReports);

        }
        [HttpGet]
        public ActionResult VAT_Out(string StartDate, string EndDate,  int? page)
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

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "VAT_Out";
            ViewBag.PageTitle = "VAT OUT";




            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);



            ViewBag.StartDate_VAT_INReport = StartDate;
            ViewBag.EndDate_VAT_INReport = EndDate;


            _oReports.VAT_OUTReportReport = _oReports.pa_Select_VAT_OUT_DAL(StartDate, EndDate, c_IDs).ToPagedList(page ?? 1, 10);
            

          
            return View(_oReports);
        }

        [HttpGet]
        public ActionResult VAT_OUT_Search(string StartDate, string EndDate, int? page)
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

            ViewBag.BreadCumController = "Report";
            ViewBag.BreadCumAction = "VAT_Out";




            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);



            ViewBag.StartDate_VAT_INReport = StartDate;
            ViewBag.EndDate_VAT_INReport = EndDate;


            _oReports.VAT_OUTReportReport = _oReports.pa_Select_VAT_OUT_DAL(StartDate, EndDate, c_IDs).ToPagedList(page ?? 1, 10);
            

            return PartialView("_VAT_Out", _oReports);

        }


        [HttpGet]
        public IActionResult VAT_IN_Print(string StartDate, string EndDate)
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

            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);



            _oReports.VAT_INReportPrint = _oReports.pa_Select_VAT_IN_DAL(StartDate, EndDate, c_IDs).ToList();
          
            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
 "--footer-left \"  Printed At: " +
DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
" --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };
            return demoViewPortrait;

        }

        [HttpGet]
        public IActionResult VAT_IN_Excel(string StartDate, string EndDate)
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
            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);


            _oReports.VAT_INReportPrint = _oReports.pa_Select_VAT_IN_DAL(StartDate, EndDate, c_IDs).ToList();

            using (var workbook = new XLWorkbook())
            {

                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("VAT_INReportPrint");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Date";
                worksheet.Cell(currentRow, 2).Value = "Ref";
                worksheet.Cell(currentRow, 3).Value = "Amount";
                worksheet.Cell(currentRow, 4).Value = "PartyName";
                worksheet.Cell(currentRow, 5).Value = "TransType";
                worksheet.Cell(currentRow, 6).Value = "VATRate";
                worksheet.Cell(currentRow, 7).Value = "Collected";
                worksheet.Cell(currentRow, 8).Value = "Return";

                foreach (var item in _oReports.VAT_INReportPrint)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.Date;
                    worksheet.Cell(currentRow, 2).Value = item.Ref;
                    worksheet.Cell(currentRow, 3).Value = item.Value;
                    worksheet.Cell(currentRow, 4).Value = item.Party_Name;
                    worksheet.Cell(currentRow, 5).Value = item.TransType;
                    worksheet.Cell(currentRow, 6).Value = item.VAT_Rate;
                    worksheet.Cell(currentRow, 6).Value = item.Collected;
                    worksheet.Cell(currentRow, 7).Value = item.Return;

                    if (!string.IsNullOrEmpty(item.Value) && !string.IsNullOrWhiteSpace(item.Value))
                    {
                        worksheet.Cell(currentRow, 3).Value = Convert.ToDecimal(item.Value);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 3).Value = item.Value;
                    }
                }

                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "VAT_INReport.xlsx");
                }
            }
        }

        [HttpGet]
        public IActionResult VAT_OUT_Print(string StartDate, string EndDate)
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
            ViewBag.PageTitle = "VAT OUT Print";
            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);



            _oReports.VAT_OUTReportPrint = _oReports.pa_Select_VAT_OUT_DAL(StartDate, EndDate, c_IDs).ToList();

            var demoViewPortrait = new ViewAsPdf(_oReports)
            {



                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 10 },
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches =
 "--footer-left \"  Printed At: " +
DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "                                          Page: [page]-[toPage]\"" +
" --footer-line --footer-font-size \"10\" --footer-spacing 2 --footer-font-name \"Segoe UI\""



            };
            return demoViewPortrait;

        }

        [HttpGet]
        public IActionResult VAT_OUT_Excel(string StartDate, string EndDate)
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
            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);


            _oReports.VAT_OUTReportPrint = _oReports.pa_Select_VAT_OUT_DAL(StartDate, EndDate, c_IDs).ToList();

            using (var workbook = new XLWorkbook())
            {

                //Created Worksheet
                var worksheet = workbook.Worksheets.Add("VAT_OUTReportPrint");

                //First Row
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Date";
                worksheet.Cell(currentRow, 2).Value = "Ref";
                worksheet.Cell(currentRow, 3).Value = "Amount";
                worksheet.Cell(currentRow, 4).Value = "PartyName";
                worksheet.Cell(currentRow, 5).Value = "TransType";
                worksheet.Cell(currentRow, 6).Value = "VATRate";
                worksheet.Cell(currentRow, 7).Value = "Paid";
                worksheet.Cell(currentRow, 8).Value = "Return";

                foreach (var item in _oReports.VAT_OUTReportPrint)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.Date;
                    worksheet.Cell(currentRow, 2).Value = item.Ref;
                    worksheet.Cell(currentRow, 3).Value = item.Value;
                    worksheet.Cell(currentRow, 4).Value = item.Party_Name;
                    worksheet.Cell(currentRow, 5).Value = item.TransType;
                    worksheet.Cell(currentRow, 6).Value = item.VAT_Rate;
                    worksheet.Cell(currentRow, 6).Value = item.Paid;
                    worksheet.Cell(currentRow, 7).Value = item.Return;

                    if (!string.IsNullOrEmpty(item.Value) && !string.IsNullOrWhiteSpace(item.Value))
                    {
                        worksheet.Cell(currentRow, 3).Value = Convert.ToDecimal(item.Value);
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 3).Value = item.Value;
                    }
                }

                //Download option
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "VAT_OUTReport.xlsx");
                }
            }
        }

        #endregion
    }
}

