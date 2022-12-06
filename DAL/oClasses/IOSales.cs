using DAL.Models;
using System.Collections.Generic;
using System.Data;
using X.PagedList;

namespace DAL.oClasses
{
    public interface IOSales
    {
        //---properties area



        //--- by Rafay 14/jan/2021 Sales Footer

        public pa_select_saledetails_Total_Result SaleDetailGrandTotals { get; set; }

        public pa_select_saledetails_Total_Result pa_select_saledetails_Total(string saleRef);

        //--- by Rafay 14/jan/2021 

        //New WOrk Faraz
        public IEnumerable<pa_SalesMaster_DAL> SalesMasterIPagedList1_1 { get; set; }


        public pa_SalesDetails SalesDetailObj { get; set; }
        public IEnumerable<pa_SalesDetails> SalesDetailList { get; set; }

        public IEnumerable<pa_SalesDetails> SalesDetailList_trd_online { get; set; }

        public IEnumerable<pa_SalesDetails> SalesDetailList_Specs { get; set; }

        public IEnumerable<pa_ReceiptDetails_DAL> SalesReceiptDetailsList { get; set; }

        public IEnumerable<pa_SalesDetails> SalesDetailList2 { get; set; }

        public string Update_vehicle_display_master_DAL(int? vehicle_display_master_ID, string Delivery_DateTime,
           string Showroom_Loc_ID, string delivered_by_EmpID, int? Modified_by, int c_ID);
        public pa_SalesMaster_DAL SaleMasterObj { get; set; }

        public pa_SalesMaster_DAL SaleMasterObjNEW { get; set; }
        public pa_SalesMaster_DAL SaleMasterBalanceObj { get; set; }
        public IPagedList<pa_SalesMaster_DAL> SalesMasterList { get; set; }
        public IEnumerable<pa_SalesMaster_DAL> SalesMasterListTOTAL { get; set; }
        public IPagedList<pa_Select_SalesDashboardList> SalesDashboardIPagedList { get; set; }
        public IPagedList<pa_SalesMaster_DAL> SalesMasterIPagedList { get; set; }
        public IPagedList<pa_vehicle_display_master> VehicleDisplayMasterList { get; set; }
        public IEnumerable<pa_vehicle_display_details> VehicleDisplayDetailList { get; set; }
        public pa_vehicle_display_master VehicleDisplayMasterObj { get; set; }

        public IEnumerable<pa_SalesMaster_DAL> SalesMaster_BookingList { get; set; }
        public IEnumerable<pa_SalesMaster_DAL> SalesMaster_PerformaInvoiceList { get; set; }

        public IPagedList<pa_SalesMaster_DAL> SalesMaster_InvoiceReturnList { get; set; }
        public IEnumerable<Pa_Attachments_DAL> attachmentList { get; set; }
        public SaleDetailJP SalesDetailObjJP { get; set; }
        public IEnumerable<SaleDetailJP> SalesDetailListJP { get; set; }
        public pa_SalesMaster_DAL SalesMasterObjJP { get; set; }
        public pa_SalesMaster_DAL SalesMasterRefJP { get; set; }
        public IPagedList<pa_SalesMaster_DAL> SalesMasterListJP { get; set; }
        public pa_SalesMaster_DAL GetSalesObj_Stock { get; set; }

        public IEnumerable<pa_ReceiptDetails_DAL> Get_Sales_ReceiptDetails_DAL(int? SaleMaster_ID);

        public pa_SalesMaster_DAL SalesMasterPrintObj { get; set; }
        public IEnumerable<pa_SalesMaster_DAL> PerformaPrintObj { get; set; }
        public IEnumerable<pa_SalesMaster_DAL> PerformaPrintObj_Specs { get; set; }
        public IEnumerable<pa_SalesDetails> SalesDetailPrintObj { get; set; }
        public pa_SalesDetails SalesDetailPrintObj_TTL { get; set; }
        public pa_SalesMaster_DAL SalesAggrMasterPrintObj { get; set; }

        public IEnumerable<pa_SalesDetails> SalesAggrDetailPrintObj { get; set; }
        public IEnumerable<pa_SalesDetails> SalesAggrDetailPrintObj_Summary { get; set; }

        public pa_SalesDetails SalesAggrDetailPrintObj_TTL { get; set; }
        public IEnumerable<pa_SalesMaster_DAL> SalesMasterIPagedList1 { get; set; }
        public IEnumerable<pa_SalesMaster_DAL> SalesMasterListTOTAL1 { get; set; }
        public IEnumerable<Pa_DeliveryOrder_Master> DeliveryReport_Print { get; set; }
        public IEnumerable<pa_SalesMaster_DAL> SalesMasterIPagedListnew { get; set; }

        public IEnumerable<pa_SalesMaster_DAL> SalesMasterListTOTALnew { get; set; }
        public IEnumerable<pa_SalesMaster_DAL> SalesMasterListJP1 { get; set; }


        public IEnumerable<pa_SalesMaster_DAL> SalesMasterListJP_total { get; set; }


        public IEnumerable<pa_ReceiptDetails_DAL> SalesReceiptDetailsList_Print { get; set; }

        public IPagedList<pa_SalesMaster_DAL> SalesMasterByDateIPagedList { get; set; }
        public IEnumerable<pa_SalesDetails> VoidDetailList { get; set; }
        public IEnumerable<pa_SalesDetails> DiscountDetailList { get; set; }
        public IEnumerable<pa_SalesDetails> DiscountDetailList_TTL { get; set; }
        public IEnumerable<Pa_SalesDeliveryOrder_Dtl> DODetailList { get; set; }
        public Pa_SalesDeliveryOrder_Master DOMasterObj { get; set; }
        public IPagedList<Pa_SalesDeliveryOrder_Master> DOOtherMasterList { get; set; }
        public IEnumerable<pa_SalesDetails> SaleDetailList { get; set; }

        public IEnumerable<pa_SalesDetails> SalesReturnDetailList { get; set; }
        public pa_SalesMaster_DAL SalesReturnMasterObj { get; set; }
        public IPagedList<pa_SalesMaster_DAL> SalesReturnMasterList { get; set; }
        public IEnumerable<Pa_ItemLocations_DAL> LocDescList { get; set; }
        public Pa_SalesDeliveryOrder_Master SalesDOMasterPrintObj { get; set; }
        public IEnumerable<Pa_SalesDeliveryOrder_Dtl> SalesDODetailPrintObj { get; set; }

        public Pa_SalesDeliveryOrder_Dtl SalesDODetailPrintObj_TTL { get; set; }
        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMaster_Print { get; set; }
        public IEnumerable<Pa_SalesDeliveryOrder_Master> DOOtherMasterList_Print { get; set; }
        public IEnumerable<PaperAfterSalesDetail_DAL> PaperAfterSalesDetailList { get; set; }
        public PaperAfterSalesMaster_DAL PaperAfterSalesMasterObj { get; set; }
        public IPagedList<PaperAfterSalesMaster_DAL> PaperAfterSalesMaster_jpMasterList { get; set; }

        public IEnumerable<pa_Select_SalesDashboardList> SalesDashboardList { get; set; }

        public IEnumerable<pa_Select_SalesDashboardList>  SelectSalesDetails { get; set; }
        //---methods area

        public IEnumerable<pa_Select_SalesDashboardList> pa_SelectSaleDetails(string Make, string Model_Desc, string Model_Year, string Ext_Color, string Production_Date, int C_id);
        public IEnumerable<pa_Select_SalesDashboardList> Pa_Select_SaleDashboard(int? Make, int? Model_Desc, int? Color, string StartDate, string EndDate, string Model_Year, int? c_id);
        public List<pa_SalesMaster_DAL> pa_Select_SalesMaster_total(string sale_Ref, int customerName, string startDate, string endDate);

        public string InsertSalesInvoiceDetail_DAL(string SalesInvoiceType, string Chassis, string ItemDesc, double? Unit_Price, double?
            VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int SaleMaster_ID, double? Discount);



        public IEnumerable<pa_SalesDetails> Get_SalesIvoiceDetailListByChassis_DAL(string Temp_ID, int? SaleMaster_ID,int c_ID);
        
        public string InsertSalesInvoiceMaster_DAL(int? Customer_ID, string Customer_Contact, string Invoice_address, string Saletype, string SaleDate, string CustomerTRN,
             string Manualbook_ref, int? Seller_ID, int? Agent_ID, int? ExportTo_Destination_ID, int? Port_of_Exit_ID, string Sale_trans_type, int? shipping_co_ID,
             int? ExporterCo_ID, int? Bank_to_Transfer_payment_ID, int? Showroom_ID, string Temp_ID, int? c_ID, int? Created_by, string Remarks, int Currency_ID, string Curr_Rate,string Customer_Name);

        public string GetOldTempIDFromSalesDetail_DAL(int? SaleMaster_ID);
        public pa_SalesMaster_DAL Get_SalesInvoiceMasterByID_DAL(int? SaleMaster_ID,int c_ID);
        public pa_SalesMaster_DAL Get_SalesInvoiceMasterBalance(int? SaleMaster_ID,int c_ID);


        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMasterInvoiceList_DAL(string SaleRef, string ManualRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int Status_ID,int c_ID, int Make, int Model_Desc, string Model_Year);

        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMasterInvoiceList_TTL_DAL(string SaleRef, string ManualRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int Status_ID,
        int c_ID, int Make, int Model_Desc, string Model_Year);

        public IEnumerable<pa_SalesMaster_DAL> Get_SalesInvoiceReturnList_DAL(string SaleRef, string StartDate, string EndDate, string Customer_Name, int c_ID);
        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMasterBooking_List_DAL(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int Status_ID,
         int c_ID);
        public IEnumerable<pa_SalesMaster_DAL> Get_PerformaInvoice_List_DAL(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int c_ID);


        public IEnumerable<Pa_CarLocations_DAL> Get_CarLocation_DAL(int c_ID);
        public pa_vehicle_display_master Get_MaxRef();
        public IEnumerable<Pa_EmpMaster_DAL> Get_Emp_Name_DAL(int c_ID);
        public IEnumerable<pa_vehicle_display_master> Get_vehicle_display_master_DAL(string Ref, string StartDate, string EndDate,
            string Chassis, int c_ID);
        public string Insert_vehicle_display_master_DAL(string Delivery_DateTime, string Showroom_Loc_ID, string delivered_by_EmpID, string temp_ID, int? Created_by, int c_ID);
        public string Delete_vehicle_display_master_DAL(int? vehicle_display_master_ID);



        public IEnumerable<pa_SalesDetails> Get_SaleSummaryDescription_DAL(int SaleMaster_ID, int c_ID);



        public IEnumerable<pa_vehicle_display_details> Get_vehicle_display_DetailListByID_DAL(int? vehicle_display_master_ID, string temp_ID);

        public string Insert_vehicle_display_detail_DAL(int? vehicle_display_master_ID, string chassis_no, string return_Date,
            string temp_ID, int? Created_by, int c_ID);
        public string Update_vehicle_display_detail_DAL(int? vehicle_display_master_ID, int? vehicle_display_details_ID, string chassis_no, string return_Date, int? Modified_by);
        public string Delete_vehicle_display_detail_DAL(int? vehicle_display_details_ID);
        public pa_vehicle_display_master Get_vehicle_display_master_DAL_Id(int? vehicle_display_master_ID);

        public string UpdateSalesInvoiceMaster_DAL(int? SaleMaster_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string CustomerTRN,
        string Manualbook_ref, int? Seller_ID, int? Agent_ID, int? ExportTo_Destination_ID, int? Port_of_Exit_ID, string Sale_trans_type, int? shipping_co_ID,
        int? ExporterCo_ID, int? Bank_to_Transfer_payment_ID, int? Showroom_ID, string Temp_ID, int? c_ID, int? Modified_by, string Remarks, int Currency_ID, string Curr_Rate,string Customer_Name);

        public string DeleteSalesMaster_DAL(int? SaleMaster_ID);
        public string DeleteSalesDetail_DAL(int SalesDetails_ID,int c_ID);
        ////add Perameter C_id by waiz for deleting Performa Invoice chassis Data/////
        public string DeleteSalesDetail_Performa_DAL(int? SalesDetails_ID,int? c_id);
        public string DeleteSalesDetail_Specs_DAL(int? SalesDetails_ID);
        public string UpdateSalesInvoiceDetail_DAL(int? SalesDetails_ID, string Chassis, string ItemDesc, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by, double? Discount);
        public string InsertSalesBookingDetail_DAL(string Chassis, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int hdSaleMaster_ID, double? Discount);

        public string UpdateSalesBookingDetail_DAL(int? SalesDetails_ID, string Chassis, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by, double? Discount);

        public string InsertSalesBookingMaster_DAL(int? Customer_ID, string Customer_Contact, string Invoice_address, string Saletype, string SaleDate, string CustomerTRN, string Manualbook_ref,
            int? Seller_ID, int? Agent_ID, int? ExportTo_Destination_ID, int? Port_of_Exit_ID, string Sale_trans_type, int? shipping_co_ID, int? ExporterCo_ID, int? Bank_to_Transfer_payment_ID, int? Showroom_ID, string Temp_ID, int? c_ID, int? Created_by, string Remarks, int Currency_ID, string Currency_Rate);


        public string UpdateSalesBookingMaster_DAL(int? SaleMaster_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string CustomerTRN, string Manualbook_ref, int? Seller_ID, int? Agent_ID,
            int? ExportTo_Destination_ID, int? Port_of_Exit_ID, string Sale_trans_type, int? shipping_co_ID, int? ExporterCo_ID, int? Bank_to_Transfer_payment_ID, int? Showroom_ID, string Temp_ID, int? c_ID, int? Modified_by, string Remarks, int Currency_ID, string Currency_Rate);
        public pa_SalesMaster_DAL Get_SalesBookingMasterByID_DAL(int? SaleMaster_ID);
        public IEnumerable<pa_SalesDetails> Get_SalesBookingDetailListByID_DAL(string Temp_ID, int? SaleMaster_ID);
        public string Convert_SalesBooking_by_Chassis_DAL(int? SaleMaster_ID, string SelectedChassis, int? Created_by);
        public string convert_Salebooking_By_SalesMasterID_DAL(int? SaleMaster_ID);
        public pa_SalesMaster_DAL Get_SalesPerformaMasterByID_DAL(int? SaleMaster_ID);
        public IEnumerable<pa_SalesDetails> Get_SalesPerformaDetailListByChassis_DAL(string Temp_ID, int? SaleMaster_ID);
        public IEnumerable<pa_SalesDetails> Get_SalesPerformaDetailListByChassis_Specs_DAL(string Temp_ID, int? SaleMaster_ID);


        public string InsertSalesPerformaDetail_DAL(string SalesInvoiceType, string Chassis, string ItemDesc, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int hdSaleMaster_ID);
        public string InsertSalesPerformaDetail_Specs_DAL(string SalesInvoiceType, string Chassis, string ItemDesc, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, double? Quantity, int hdSaleMaster_ID);

        public string InsertSalesPerformaMaster_DAL(int? Customer_ID, string Customer_Contact, string Invoice_address, string Saletype, string SaleDate, string CustomerTRN,
            string Manualbook_ref, int? Seller_ID, int? Agent_ID, int? ExportTo_Destination_ID, int? Port_of_Exit_ID, string Sale_trans_type, int? shipping_co_ID, int? ExporterCo_ID, int? Bank_to_Transfer_payment_ID, int? Showroom_ID, string Performa_Validity, string Temp_ID, int? c_ID, int? Created_by, int Currency_ID, string Currency_Rate);
        public string UpdateSalesPerformaMaster_DAL(int? SaleMaster_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string CustomerTRN, string Manualbook_ref, int? Seller_ID,
            int? Agent_ID, int? ExportTo_Destination_ID, int? Port_of_Exit_ID, string Sale_trans_type, int? shipping_co_ID, int? ExporterCo_ID,
            int? Bank_to_Transfer_payment_ID, int? Showroom_ID, string Performa_Validity, string Temp_ID, int? c_ID, int? Modified_by, int Currency_ID, string Currency_Rate);
        public string UpdateSalesPerformaDetail_DAL(int? SalesDetails_ID, string Chassis, string ItemDesc, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by);
        public string UpdateSalesPerformaDetail_Specs_DAL(int? SalesDetails_ID, string Chassis, string ItemDesc, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by, double? Quantity);

        public string InsertSalesDetails_CancelReturn(string Temp_ID, string SaleRef);
        public string CheckIfRefExistInSalesMaster_DAL(string SaleRef);
        public string ChangeSalesMasterStatus_DAL(int? SaleMaster_ID, int? Status_ID, int? c_ID, string Trans_Type, int? Modified_by, string QRName);
        public IEnumerable<pa_SalesDetails> Get_SalesCancelDetailListByID_DAL(string Temp_ID, int? SaleMaster_ID);
        public string InsertSalesMaster_CancelReturn_DAL(int? Customer_ID, string Customer_Contact, string Invoice_address, string Saletype, string SaleDate, string Temp_ID, int? c_ID, int? Created_by);

        public string UpdateSalesMaster_CancelReturn_DAL(int? SaleMaster_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string Temp_ID, int? c_ID, int? Modified_by);
        public pa_SalesMaster_DAL Get_SalesCancelReturnMasterByID_DAL(int? SaleMaster_ID);
        public IEnumerable<Pa_Attachments_DAL> GetSalesInvoiceMaster_Attachments_DAL(int? SaleMaster_ID, string Type);
        public string Insert_Attachment_SalesInvoice_DAL(int? SaleMaster_ID, string Document_Type, string Type, string newFileName, string filepath, int? Created_by);
        public string Delete_Attachment(int? Attachment_ID);


        public pa_SalesMaster_DAL Get_SalesInvoiceMasterPrintByID_DAL(int? SaleMaster_ID);
        public IEnumerable<pa_SalesDetails> Get_SalesInvoiceDetailPrintByID_DAL(int? SaleMaster_ID);
        public DataSet GetDataForPrintDtl(int? SaleMaster_ID);
        public DataSet select_vehicle_display_Print_By_ID(int? vehicle_display_master_ID);
        public string GetOldTempIDFromDisplayDetail_DAL(int? vehicle_display_master_ID);
        public DataSet select_Delivery_Print_By_ID(int? DeliveryMaster_ID);






        #region SalesJP

        //---insert into sales booking detial general
        public string InsertSalesMasterJPDetail_DAL(string Chassis, string Ref,
             string Price, string PriceRate, string PriceTax, string RecycleFee,
            string auctionfee, string auctionfeetax, string NumberPlate, string OfficeCharges, string OfficeChargesTax, int hdSalesMaster_ID, string Temp_ID, int? c_ID, int? Created_by);

        //---Update into sales booking detial general
        public string UpdateSalesMasterJPDetail_DAL(int? SaleDetailID, string Chassis,
             string Price, string PriceRate, string PriceTax, string RecycleFee,
            string auctionfee, string auctionfeetax, string NumberPlate, string OfficeCharges, string OfficeChargesTax, string Temp_ID, int? c_ID, int? Modified_by);


        //Insert Sales booking Master
        public string InsertSalesMasterJPMaster_DAL(int CustomerName, string Ref, string SaleRef, string Date, string ManualRef, string SaleDate, string CustomerContact, string CustomerAddress, string SalesType, string Saletype, string Temp_ID, int? c_ID, int? Created_by, int Bank_to_Transfer_payment_ID);


        //Udpate Sale booking Master
        public string UpdateSalesMasterJPMaster_DAL(string SaleRef, int? Sale_ID, string Date, string Customer, string Ref, int? ID, string CustomerContact, string CustomerAddress, string SalesType, string txtmanualRefUpdate, string Temp_ID, int? c_ID, int? Modified_by, int Bank_to_Transfer_payment_ID);

        //--getting Sale booking master by id
        public pa_SalesMaster_DAL Get_SalesMasterJPMasterByID_DAL(int? SalesMaster_ID);


        //Get Sale booking detail list by id
        public IEnumerable<SaleDetailJP> Get_SalesMasterJPDetailListByID_DAL(string Temp_ID, int? SalesMaster_ID);

        public string GetOldTempIDFromSaleDetail_DAL(int? SalesMaster_ID);
        public string DeleteSalesMasterJP_DAL(int? SalesMaster_ID);





        //public string Cancel_SaleJP(int? ID, string UserId,int Status_ID);
        public string Cancel_SaleJP(int? SaleMasterID, int? Status_ID, int? c_ID, string Trans_Type, int? Modified_by);

        public string DeleteSaleDetail_DAL(int? StockDetails_ID);
        public List<pa_SalesMaster_DAL> pa_Select_SalesMaster(string sale_Ref, int customerName, string startDate, string endDate);





        public pa_SalesMaster_DAL SaleLoadRef();

        #endregion

        public string InsertReceiptDetail_Sale_DAL_Invoice(int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
      double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date,
      string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int hdSaleMaster_ID, int hdCustomer_ID,string ReceiptDate);

        //---Update receipt detail Sale
        public string UpdateReceiptDetail_Sale_DAL_Invoice(int? ReceiptDetail_ID, int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
            double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date,
            string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by, string ReceiptDate);
        
        
        //Get receipt of type sale detail list by ID's
        public IEnumerable<pa_ReceiptDetails_DAL> Get_ReceiptDetailSaleListByID_DAL_Invoice(string Temp_ID, int SaletMaster_ID);
        public string DeleteReceiptDetail_DAL_Invoice(int? ReceiptDetail_ID);
        public pa_SalesMaster_DAL GetSalesDetail_DAL(int? Stock_ID);
        public pa_SalesDetails Get_SalesInvoiceDetailPrintByID_ttl(int? SaleMaster_ID);
        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMasterInvoiceList_DAL1(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int Status_ID,
int c_ID);

        public pa_vehicle_display_master Get_vehicle_display_master_DAL_Id1(int? vehicle_display_master_ID);
        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMasterInvoiceList_TTL_DAL1(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int Status_ID,
int c_ID);
        public IEnumerable<pa_SalesMaster_DAL> Get_PerformaInvoice_List_DAL1(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int c_ID);

        //New Faraz Work
        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMasterBooking_List_DAL1(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int Status_ID,
        int c_ID);





        public IEnumerable<pa_SalesMaster_DAL> Get_PerformaInvoiceDetailPrintByID_DAL(int? SaleMaster_ID);
        public IEnumerable<pa_SalesMaster_DAL> Get_PerformaInvoiceDetailPrintByID_Specs_DAL(int? SaleMaster_ID);
        public IEnumerable<Pa_DeliveryOrder_Master> Get_DeliveryNotePrintByID_DAL(int? DeliveryMaster_ID);
        #region SalesInvoice_TRD
        public pa_SalesMaster_DAL Get_SalesInvoiceMasterByID_DAL_TRD(int? SaleMaster_ID);
        public IEnumerable<pa_SalesDetails> Get_SalesIvoiceDetailListByChassis_DAL_TRD(string Temp_ID, int? SaleMaster_ID);

        public string InsertSalesInvoiceDetail_DAL_TRD(string SalesInvoiceType, int Item_ID, string ItemDesc, double? Unit_Price, double?
        VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int SaleMaster_ID, string Quantity, string UM_ID, int Location_ID, string Serial, string Barcode, int Seller_ID);
        public string UpdateSalesInvoiceDetail_DAL_TRD(int? SalesDetails_ID, int Item_ID, string ItemDesc, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by
            , string Quantity, string UM_ID, int Location_ID, string Serial, string Barcode, int Seller_ID);

        public string InsertSalesInvoiceMaster_DAL_TRD(int? Customer_ID, string Customer_Contact, string Invoice_address, string Saletype, string SaleDate, string CustomerTRN,
        string Manualbook_ref,  string Sale_trans_type,
     string Temp_ID, int? c_ID, int? Created_by, string Remarks, string Customer_Name,string OtherCost,string Tip,string Commision, string Wallet, string ShippingCharges);
        public string UpdateSalesInvoiceMaster_DAL_TRD(int? SaleMaster_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string CustomerTRN,
        string Manualbook_ref, string Sale_trans_type,
  string Temp_ID, int? c_ID, int? Modified_by, string Remarks, string Customer_Name, string OtherCost, string Tip, string Commision, string Wallet,string ShippingCharges);

        public string DeleteSalesMaster_DAL_TRD(int? SaleMaster_ID);
        public string DeleteSalesDetail_DAL_TRD(int? SalesDetails_ID);
        public string SalesID_DAL_TRD();
        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMasterInvoiceList_DAL_TRD(string SaleRef, string StartDate, string EndDate, string Customer_Name, string ItemId, string MNumber, int Status_ID,
    int c_ID, string OrderRef,string OrderStatus);

        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMasterInvoiceList_TTL_DAL_TRD(string SaleRef, string StartDate, string EndDate, string Customer_Name, string ItemId, string MNumber, int Status_ID,
          int c_ID, string OrderRef, string OrderStatus);

        #endregion

        public IEnumerable<pa_ReceiptDetails_DAL> Get_ReceiptDetailSaleListByID_DAL_Invoice_Print(int? SaletMaster_ID);

        public string ChangeSalesMasterStatus_trd_DAL(int? SaleMaster_ID, int? Status_ID, int? c_ID, string Trans_Type, int? Modified_by);

        #region Dashboard Graph
        public DataSet getLAST_7DAYS_SALE_TRD();
        public DataSet GetTopSaleValueItems_trd(string StartDate, string EndDate);

        public DataSet Sale_By_Month_trd(string Year, int c_ID);

        public DataSet TopPayables_trd(string StartDate, string EndDate);
        public DataSet TopSaleCategory_trd(string StartDate, string EndDate);
        public DataSet TopItemByCost_trd(string StartDate, string EndDate);
        public DataSet get_StockLocation_Summary(int c_ID);

        public DataSet get_All_Accounts_Balance(int c_ID);
        public DataSet get_MakeModel_Summary(int c_ID);
        public DataSet getLAST_7DAYS_SALE();
        public DataSet getSale_By_Month();
        #endregion
        public IEnumerable<pa_SalesMaster_DAL> Get_SalesSummaryByDate_DAL(string StartDate, string EndDate);
        public IEnumerable<pa_SalesDetails> GetVoidDetails_DAL(string ItemCode);

        public IEnumerable<pa_SalesDetails> GetDiscountDetails_DAL(string SaleDate);

        public IEnumerable<pa_SalesDetails> GetDiscountDetails_DAL_TTL(string SaleDate);
        public IEnumerable<Pa_ItemLocations_DAL> Get_LocList_DAL(string Item_ID);
        public string ChangeSalesMasterStatus_Performa_DAL(int? SaleMaster_ID, int? Status_ID, int? c_ID, string Trans_Type, int? Modified_by,string QRName);
        #region SalesDeliveryOrder

        public Pa_SalesDeliveryOrder_Master Get_SalesDeliveryOrderMaster_ByID_DAL(int? DOMaster_ID);
        public IEnumerable<Pa_SalesDeliveryOrder_Dtl> Get_SalesDeliveryOrderDetailListByID_DAL(string Temp_ID, int? DOMaster_ID);
        public string InsertSalesDeliveryOrderDetail_DAL(int Item_ID, string ItemDesc, double? Unit_Price, double?
VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int DOMaster_ID, int SaleMaster_ID, string Quantity, string UM_ID, int Location_ID,int SaleDetails_ID);





        public string InsertSalesDeliveryOrderMaster_DAL(int? Customer_ID, string Customer_Contact, string Invoice_address, string DODate, string CustomerTRN,
 string Manualbook_ref, int? Seller_ID,
 string Temp_ID, int? c_ID, int? Created_by, string Remarks, string Customer_Name, int SaleMaster_ID);

        public string UpdateSalesDeliveryOrderMaster_DAL(int DOMaster_ID, int? SaleMaster_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string DODate, string CustomerTRN,
string Manualbook_ref, int? Seller_ID,
string Temp_ID, int? c_ID, int? Modified_by, string Remarks, string Customer_Name);


        public string DeleteSalesDeliveryOrderMaster_DAL(int? DOMaster_ID);
        public string DeleteSalesDeliveryOrder_Material_In_DAL(int? DODetails_ID);

        public IEnumerable<Pa_SalesDeliveryOrder_Master> Get_SalesDeliveryOrderMaster_DAL(string DORef, string StartDate, string EndDate, string Customer_Name, string ItemId, int Status_ID,
int c_ID);




        public string GetOldTempIDFromSalesDeliveryOrderDetail_DAL(int? DOMaster_ID);

        public IEnumerable<pa_SalesDetails> GetDORefDetails_Other_DAL();


        public IEnumerable<pa_SalesDetails> Get_SalesDetailListByRef_DAL(int? DOMaster_ID);

        public Pa_SalesDeliveryOrder_Master Get_SalesDeliveryMasterPrintByID_DO_DAL(int? DOMaster_ID);
        public Pa_SalesDeliveryOrder_Dtl Get_SalesDeliveryOrderDetailPrintByID_DO_ttl(int? DOMaster_ID);

        #endregion


        #region SalesInventoryReturn

        public pa_SalesMaster_DAL Get_SalesMaster_ByID_DAL_Return(int? SaleMaster_ID);
        public IEnumerable<pa_SalesDetails> Get_SalesDetailListByID_DAL_Return(string Temp_ID, int? SaleMaster_ID);
        public string InsertSalesDetail_DAL_Return(int Item_ID, string ItemDesc, double? Unit_Price, double?
        VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int SaleMaster_ID, int SaleMasterRef_ID, string Quantity, string UM_ID, int Location_ID, int SaleDetails_ID);





        public string InsertSalesMaster_DAL_Return(int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string CustomerTRN,
        string Manualbook_ref, int? Seller_ID,
        string Temp_ID, int? c_ID, int? Created_by, string Remarks, string Customer_Name, string SaleType, int SaleMasterRef_ID);

        public string UpdateSalesMaster_DAL_Return(int SaleMaster_ID, int? SaleMasterRef_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string CustomerTRN,
        string Manualbook_ref, int? Seller_ID,
        string Temp_ID, int? c_ID, int? Modified_by, string Remarks, string Customer_Name);


        public string DeleteSalesMaster_DAL_Return(int? SaleMaster_ID);
        public string DeleteSalesDetail_DAL_Return(int? SalesDetails_ID);

        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMaster_DAL_Return(string SaleRef, string StartDate, string EndDate, string Customer_Name, string ItemId, int Status_ID,
        int c_ID);




        public string GetOldTempIDFromSalesDetail_DAL_Return(int? SaleMaster_ID);

        public IEnumerable<pa_SalesDetails> GetSaleRefDetails_Other_DAL_Return();


        public IEnumerable<pa_SalesDetails> Get_SalesDetailListByRef_DAL_Return(int? SaleMaster_ID);
        #endregion
        #region PaperAfertSalesJp

        //Insert  purchase return detail List
        public string Insert_paperAfterSaleDetails_jp(string Temp_ID, string SaleRef, int hdpapers_ID);

        public string Insert_paperAfterSaleDetails_Chassis_jp(string Temp_ID, string Chassis, int hdpapers_ID);

        public string CheckIfRefExistInSalesMaster_jp_DAL(string SaleRef);
        public string CheckIfRefExistInChassis_DAL(string Chassis);


        //--getting purchase master return other by id
        public PaperAfterSalesMaster_DAL Get_Pa_Select_paperAfterSaleMaster_jp_ByID(int? papers_ID);
        public PaperAfterSalesMaster_DAL Get_Pa_Select_paperAfterSaleMaster_jp_ByID_print(int? papers_ID);


        //Get other purchase detail List
        public IEnumerable<PaperAfterSalesDetail_DAL> Get_paperAfterSaleDetails_DAL(string Temp_ID, int? papers_ID);


        //Insert Return purchase Master
        public string Insert_paperAfterSaleMaster_Return_DAL(string DateCreated, string DateSubmission, string DateValid, string Remarks, string Temp_ID, int? Created_by);


        //Update return purchase Master
        public string Update_paperAfterSaleMaster_DAL(int? papers_ID, string DateCreated, string DateSubmission, string DateValid, string Remarks, string Temp_ID, int? Modified_by);
        public IEnumerable<PaperAfterSalesMaster_DAL> Get_PaperAfterSalesMasterList_DAL(string Chassis_No, string EndDate, string StartDate, string SaleRef);
        public string GetOldTempIDFrom_DAL(int? papers_ID);
        public IEnumerable<PaperAfterSalesDetail_DAL> Select_AfterSaleDetailList_DAL(int? papers_ID);
        public string DeleteaperAfterSaleDetails_DAL(int? papers_ID);
        #endregion

        //BY Yaseen 1-24-2022
        #region Payment Details
        public IEnumerable<Pa_PaymentDetails_DAL> PaymentDetailList { get; set; }
        public IEnumerable<Pa_PaymentDetails_DAL> Select_PaymentList_PVSV_ByID_DAL(string Temp_ID, int? SaleMaster_ID);
        public string InsertPaymentDetail_PVSV_DAL(int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
        string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int hdSaleMaster_ID);

        public string UpdateSalesPayment_PVSV_DAL(int? PaymentDetails_ID, int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
          string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by);


        public string DeletePaymentDetail_DAL(int? PaymentDetails_ID);
        #endregion



        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMaster_DAL_Return_trd(string SaleRef, string StartDate, string EndDate, string Customer_Name, string ItemId, int Status_ID,
       int c_ID);
    }


}
