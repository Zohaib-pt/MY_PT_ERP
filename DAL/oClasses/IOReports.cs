using DAL.Models;
using System.Collections.Generic;
using X.PagedList;

namespace DAL.oClasses
{
    public interface IOReports
    {





        public IEnumerable<pa_tblLedgers> Ledger { get; set; }
        public IPagedList<pa_tblLedgers> LedgerPagedList { get; set; }
        public IEnumerable<pa_tblLedgers> Ledger_TTL { get; set; }
        public IEnumerable<pa_tblLedgers> payableSummary { get; set; }
        public IEnumerable<pa_tblLedgers> ReceiveAbleSummary { get; set; }

        public IPagedList<pa_tblLedgers> CustomerLedger { get; set; }
        public pa_tblLedgers CustomerLedgerMasterObj { get; set; }
        public pa_tblLedgers VendorLedgerMasterObj { get; set; }
        public IEnumerable<pa_tblLedgers> CustomerLedger_TTL { get; set; }
        public IPagedList<SaleReport> SaleReport { get; set; }



        public IPagedList<BLReport> BLReport { get; set; }

        public SaleReport SaleReportTTL { get; set; }
        public IEnumerable<StockReport> StockReportList { get; set; }
        public IPagedList<StockReport> StockCostList { get; set; }
        public IEnumerable<StockReport> StockCostLists { get; set; }

        public IEnumerable<StockReport> StockCostList_ttl { get; set; }

        public IEnumerable<StockReport> VendorReport_byChassis { get; set; }
        public IEnumerable<StockReport> VendorReport_byChassis_TTL { get; set; }
        public IEnumerable<SaleReport> SaleReportPrint { get; set; }
        public IEnumerable<BLReport> BLReportPrint { get; set; }

        public IEnumerable<PurchaseReport> PurchaseReportPrint { get; set; }

        public SaleReport SaleReportTTLPrint { get; set; }

        public IEnumerable<pa_tblLedgers> Ledger_Print { get; set; }
        public pa_tblLedgers Ledger_TTL_Print { get; set; }

        public IEnumerable<pa_tblLedgers> CustomerLedger_Print { get; set; }
        public IEnumerable<pa_PaymentMaster_DAL> PaymentVoucher_Print { get; set; }
        public IEnumerable<pa_ReceiptMaster_DAL> ReceiptVoucher_Print { get; set; }
        public IEnumerable<Deposits_DAL> DepositVoucher_Print { get; set; }
        public IEnumerable<Deposits_DAL> DepositVoucherCheque_Print { get; set; }
        public pa_tblLedgers CustomerLedger_TTL_Print { get; set; }
        public Pa_PurchaseMaster_DAL PurchaseMasterPrint { get; set; }
        public IEnumerable<Pa_PurchaseDetails_DAL> PurchaseDetailPrint { get; set; }
        public IEnumerable<Pa_PurchaseDetails_DAL> PurchaseStockSummaryPrint { get; set; }
        public IEnumerable<SaleDetailJP> SalesListJP_Print { get; set; }
        public pa_select_saledetails_Total_Result SalesGrandTotals_Print { get; set; }
        public pa_SalesMaster_DAL SalesMasterObjJP_Print { get; set; }
        public Pa_GoodsReceived_Master PurchaseMasterPrint_GRV { get; set; }
        public IEnumerable<Pa_GoodsReceived_Dtl> PurchaseDetailPrint_GRV { get; set; }

        public IPagedList<TaxReport> PaidTax { get; set; }
        public IEnumerable<TaxReport> PaidTax_TTL { get; set; }
        public IPagedList<TaxReport> ReceivedTax { get; set; }
        public IEnumerable<TaxReport> ReceivedTax_TTL { get; set; }
        public IPagedList<RecycleReport> Recycle { get; set; }
        public IEnumerable<RecycleReport> Recycle_TTL { get; set; }
        //Tax Report Get_PaidTax_Report_jp_PR Nafeel Work



        public IEnumerable<RecycleReport> Recycle_PR { get; set; }
        public IEnumerable<RecycleReport> Recycle_TTL_PR { get; set; }
        public IEnumerable<pa_tblLedgers> pa_Select_Payable_Summary_DAL(int c_ID);

        public IEnumerable<pa_tblLedgers> pa_Select_CustomerLedger_DAL(string StartDate, string EndDate, string Customer_ID, int c_ID);
        public IEnumerable<pa_tblLedgers> pa_Select_Recievable_Summary_DAL(int c_ID);

        public IEnumerable<SaleReport> pa_Select_SalesReport_DAL(string StartDate, string EndDate, int? Customer_ID, string Sale_Ref, int c_ID, int Vendor_ID, string cartype, string chassis, string Pur_Ref, int? Make, int? Model_Desc, string Model_Year);


        //By Yaseen    1-12-2022
        public IEnumerable<BLReport> pa_Select_BLReport_DAL(string StartDate, string EndDate, string BLNO_Ref, int c_ID, string chassis);
        //By Yaseen    1-12-2022

        //By Yaseen    1-13-2022
        public IEnumerable<PurchaseReport> pa_Select_PurchaseReport_DAL(string StartDate, string EndDate, string Pur_Ref, int c_ID, string chassis);
        public IPagedList<PurchaseReport> PurchaseReportList { get; set; }
       
        //By Yaseen    1-13-2022

        public SaleReport pa_Select_SalesReport_TTL_DAL(string StartDate, string EndDate, int? Customer_ID, string Sale_Ref, int c_ID, int Vendor_ID, string cartype, string chassis, string Pur_Ref, int? Make, int? Model_Desc, string Model_Year);


        public IEnumerable<StockReport> pa_Select_StockReport_DAL(string StartPurchaseDate, string EndPurchaseDate, string Chassis_No, string PurchaseRef, int? Make_ID, int? MakeModel_description_ID, int? Color_ID, int c_ID);
        public IEnumerable<pa_tblLedgers> pa_Select_CustomerLedger_DAL_TTL(string StartDate, string EndDate, string Customer_ID, int c_ID);
        public IEnumerable<StockReport> pa_Select_StockCostReport_DAL(string StartPurchaseDate, string EndPurchaseDate, string Container_No,
           int c_ID);
        public IEnumerable<StockReport> select_StockCostReport_ttl_DAL(string StartPurchaseDate, string EndPurchaseDate, string Container_No,
           int c_ID);

        public IEnumerable<Pa_PurchaseMaster_DAL> GetDataForPurchasePrintMaster(int? PurchaseMaster_ID);
        public IEnumerable<Pa_PurchaseDetails_DAL> GetDataForPurchasePrintDetail(int? PurchaseMaster_ID);
        public IEnumerable<Pa_PurchaseDetails_DAL> GetDataForStockSummaryPrintDetail(int? PurchaseMaster_ID);
        public SaleReport pa_Select_SalesReport_TTL_DAL_Print(string StartDate, string EndDate, int? Customer_ID, string Sale_Ref, int c_ID, string Pur_Ref, int? Make, int? Model_Desc, string Model_Year);


        #region Vendor Report 

        public IEnumerable<pa_tblLedgers> pa_Select_VendorLedger_DAL(string StartDate, string EndDate, string Vendor_ID, int? c_ID);
        public IEnumerable<pa_tblLedgers> pa_Select_VendorLedger_DAL_TTL(string StartDate, string EndDate, string Vendor_ID, int? c_ID);
        public pa_tblLedgers pa_Select_VendorLedger_DAL_TTL_Print(string StartDate, string EndDate, string Vendor_ID, int? c_ID);
        public IEnumerable<StockReport> select_VendorReport_Chassis_Wise_DAL(string StartDate, string EndDate, int Vendor_ID, int? c_ID);
        public IEnumerable<StockReport> select_VendorReport_Chassis_Wise_TTL_DAL(string StartDate, string EndDate, int Vendor_ID, int? c_ID);


        #endregion
        public pa_tblLedgers pa_Select_CustomerLedger_DAL_TTL1(string StartDate, string EndDate, string Customer_ID, int c_ID);
        public IEnumerable<pa_PaymentMaster_DAL> GetDataForPaymentMaster_Print(int? PaymentMaster_ID);
        public IEnumerable<pa_ReceiptMaster_DAL> GetDataForReceiptMaster_Print(int? ReceiptMaster_ID);
        public Pa_PurchaseMaster_DAL GetDataForPurchaseMaster(int? PurchaseMaster_ID);


        //New Faraz Work
        public IEnumerable<pa_Expense> ExpenseReport { get; set; }

        public pa_Expense ExpenseReport_TTL { get; set; }
        public IEnumerable<pa_Expense> pa_Select_Expense__InDirect_DAL(string StartDate, string EndDate, int? DR_accountID, string Chassis_No, int? c_ID);
        //ttl

        public pa_Expense pa_Select_Expense__InDirect_TTL_DAL(string StartDate, string EndDate, int? DR_accountID, string Chassis_No, int? c_ID);


        //New Work
        public IPagedList<pa_Expense> ExpenseDirectReport { get; set; }
        public IEnumerable<pa_Expense> ExpenseReport_Direct { get; set; }
        public IPagedList<pa_Expense> ExpenseLedger { get; set; }

        public pa_Expense ExpenseReport_TTL_Direct { get; set; }
        public pa_Shipping_Info ShippingMasterObjJP_Print { get; set; }
        public IEnumerable<pa_Shipping_Info_details> ShippingListJP_Print { get; set; }
        public pa_Vanning_Master VanningMasterObjJP_Print { get; set; }
        public IEnumerable<pa_Vanning_Details> VanningListJP_Print { get; set; }
        public IEnumerable<pa_Expense> pa_Select_Expense__Direct_DAL(string StartDate, string EndDate, int? DR_accountID, string Chassis_No, int? c_ID);



        //ttl

        public pa_Expense pa_Select_Expense__Direct_TTL_DAL(string StartDate, string EndDate, int? DR_accountID, string Chassis_No, int? c_ID);





        //By Jahangir 17 Jan 2021 10:06 am
        #region ITEM CARD REPORTS / INVENTORY STOCK REPORT


        public IEnumerable<ItemCard_DAL> ItemCardReportList { get; set; }
        public IPagedList<ItemCard_DAL> ItemCardReportListParts { get; set; }
        public IEnumerable<ItemCard_DAL> ItemCardReportList_TTL { get; set; }

        public IEnumerable<ItemCard_DAL> Select_ItemCard_Report_DAL(string ItemCode, string ItemSerialNO,
         string StartDate, string EndDate, int Loc_ID, int c_ID);

        public IEnumerable<ItemCard_DAL> Select_ItemCard_Report_Parts_DAL(string ItemCode, int Item_ID, string traditional, string Make,
            string Fuel, string Transmission, string Drive, int ItemGroup_ID, int ItemCategory_ID, string Year, string ItemSerialNO,
            string StartDate, string EndDate, int Loc_ID, string EngineSpecsCode_ID, int c_ID);

        public IEnumerable<ItemCard_DAL> Select_ItemCard_Report_parts_TTL_DAL(string ItemCode, int Item_ID, string traditional, string Make,
            string Fuel, string Transmission, string Drive, int ItemGroup_ID, int ItemCategory_ID, string Year, string ItemSerialNO,
            string StartDate, string EndDate, int Loc_ID, string EngineSpecsCode_ID, int c_ID);


        public IEnumerable<ItemCard_DAL> Select_ItemCard_Report_TTL_DAL(string ItemCode, string ItemSerialNO,
          string StartDate, string EndDate, int Loc_ID, int c_ID);

        #endregion


        public IEnumerable<Deposits_DAL> GetDataForDpositMaster_Print(int? DV_ID);
        public IEnumerable<Deposits_DAL> GetDataForDpositMasterCheque_Print(int? DV_ID);
        public IEnumerable<SaleDetailJP> pa_select_salesJP_Print_DAL(int? SalesMaster_ID);

        public pa_select_saledetails_Total_Result pa_select_salesJP_Total_Print_DAL(int? SalesMaster_ID);
        public pa_SalesMaster_DAL Get_SalesMasterJPMasterByID_Print_DAL(int? SalesMaster_ID);

        public IEnumerable<SaleReport> pa_Select_SalesReport_trd_DAL(string StartDate, string EndDate, int? Customer_ID, string Sale_Ref, int c_ID, int Vendor_ID);

        public SaleReport pa_Select_SalesReport_trd_TTL_DAL(string StartDate, string EndDate, int? Customer_ID, string Sale_Ref, int c_ID, int Vendor_ID);

        public IEnumerable<pa_tblLedgers> pa_Select_VendorLedger_Other_DAL(string StartDate, string EndDate, string Vendor_ID, int? c_ID);
        public IEnumerable<pa_tblLedgers> pa_Select_VendorLedger_Other_DAL_TTL(string StartDate, string EndDate, string Vendor_ID, int? c_ID);
        public pa_tblLedgers pa_Select_VendorLedger_Other_DAL_TTL_Print(string StartDate, string EndDate, string Vendor_ID, int? c_ID);

        public IEnumerable<ItemCard_DAL> Select_Itemlocation_Report_Parts_DAL(string ItemCode, int Item_ID, string traditional, string Make,
string Fuel, string Transmission, string Drive, int ItemGroup_ID, int ItemCategory_ID, string Year, string ItemSerialNO,
string StartDate, string EndDate, int Loc_ID, int c_ID);

        public IEnumerable<ItemCard_DAL> Select_ItemCard_Report_location_TTL_DAL(string ItemCode, int Item_ID, string traditional, string Make,
            string Fuel, string Transmission, string Drive, int ItemGroup_ID, int ItemCategory_ID, string Year, string ItemSerialNO,
            string StartDate, string EndDate, int Loc_ID, int c_ID);

        //BY Yaseen 1-26-2022

        #region  Saller Report

        public IEnumerable<Pa_Partners_DAL> Get_SellersMasterList_DAL(int c_ID);
        public IEnumerable<pa_tblLedgers> pa_Select_SallarLedger_DAL(string StartDate, string EndDate,string Saller_ID, int? c_ID);
        public IEnumerable<pa_tblLedgers> pa_Select_SallerLedger_DAL_TTL(string StartDate, string EndDate, string Saller_ID, int? c_ID);
        #endregion



        #region Vat Report
        //public IEnumerable<pa_VAT> VAT_INReportReport { get; set; }
        public IPagedList<pa_VAT> VAT_INReportReport { get; set; }
        public pa_VAT VAT_INReport_TTL { get; set; }
        public IEnumerable<pa_VAT> pa_Select_VAT_IN_DAL(string StartDate, string EndDate,  int? c_ID);
        //ttl

        public pa_VAT pa_Select_VAT_IN_TTL_DAL(string StartDate, string EndDate,  int? c_ID);


       // public IEnumerable<pa_VAT> VAT_OUTReportReport { get; set; }

        public IPagedList<pa_VAT> VAT_OUTReportReport { get; set; }

        public pa_VAT VAT_OUTReport_TTL { get; set; }
        public IEnumerable<pa_VAT> pa_Select_VAT_OUT_DAL(string StartDate, string EndDate, int? c_ID);
        //ttl

        public pa_VAT pa_Select_VAT_OUT_TTL_DAL(string StartDate, string EndDate, int? c_ID);

        public IEnumerable<pa_VAT> VAT_INReportPrint { get; set; }
        public IEnumerable<pa_VAT> VAT_OUTReportPrint { get; set; }

        public pa_tblLedgers pa_Select_CustomerReportByID_DAL(int? Customer_ID);
        public pa_tblLedgers pa_Select_VendorReportByID_DAL(int? VendorID);
        #endregion

        public IEnumerable<pa_SalesMaster_DAL> Sale_Info_Print { get; set; }
        public IEnumerable<pa_SalesMaster_DAL> GetDataForSaleInfo_Print(int? SaleMaster_ID);

      
    }
}
