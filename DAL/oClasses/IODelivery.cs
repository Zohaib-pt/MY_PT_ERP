using DAL.Models;
using System.Collections.Generic;
using System.Data;
using X.PagedList;

namespace DAL.oClasses
{
    public interface IODelivery
    {


        //New Work Faraz
        public IEnumerable<Deposits_DAL> DepositsDetailListIPageList1 { get; set; }

        public IEnumerable<Deposits_DAL> DepositsDetailListIPageList1_1 { get; set; }

        public IEnumerable<Pa_DeliveryOrder_Master> Get_DeliveryOrder_master_DAL(string DeliveryRef, string StartDate, string EndDate,
            string Chassis, int? Customer_ID, int c_ID);
        public IEnumerable<Pa_DeliveryOrder_Details> Get_DeliveryOrder_DetailListByID_DAL(int? Delivery_ID, string temp_ID , int C_id);
        public string Insert_DeliveryOrderMaster_DAL(string CarTakenDate, string CarTakenPerson, string CarTakenContact, string CarTaken,
            string temp_ID, int? Created_by, int c_ID);
        public string Insert_DeliveryOrderDetail_DAL(string Chassis_No, int? DeliveryMaster_ID, string temp_ID, int? Created_by, int c_ID);
        public string Update_DeliveryOrderMaster_DAL(int? DeliveryMaster_ID, string CarTakenDate, string CarTakenPerson, string CarTakenContact, string CarTaken, string DeliveryOrder_Date, int? Modified_by);

        public string Delete_Delivery_details_DAL(int? DeliveryDetails_ID);

        public string Delete_Delete_master_DAL(int? DeliveryMaster_ID);




        public Pa_DeliveryOrder_Master Get_DeliveryOrderMaster_Id(int? DeliveryMaster_ID);
        public IPagedList<Pa_DeliveryOrder_Master> DeliveryOrderMasterList { get; set; }
        public IEnumerable<Pa_DeliveryOrder_Details> DeliveryOrderDetailList { get; set; }
        public Pa_DeliveryOrder_Master DeliveryOrderMasterObj { get; set; }


        public ChassisValidation_DAL ValidateChassisNoForDeposit_DAL(string ChassisNo);

        //New Faraz Work
        public IEnumerable<Deposits_DAL> Get_DepositMaster_List_DAL1(string DV_Ref, string Customer_Name, string StartDate, string EndDate, string Chassis_No, int? c_ID);



        public string InsertDepositVoucherDetail_DAL(int? Stock_ID, double? Car_Sale_Value, double? Deposit, double? Fix_Deductable_Charges, double? PaperExpense_Changes,
         double? VAT_Security_Deposit, double? Other_Charges, string External_Trans_Ref, string ShippingDate, string Submit_Date, string Temp_ID, int? c_ID, int? Created_by, int DV_ID);
        public IEnumerable<Deposits_DAL> Get_DepositVoucherReceivedListByID_DAL(string Temp_ID, int? DVdetails_ID);
        public Deposits_DAL Get_DepositVoucherMasterByID_DAL(int? DV_ID);
        public string GetOldTempIDFromDepositDetail_DAL(int? DV_ID);
        public string InsertDepositVoucherMaster_DAL(int? Customer_ID, string Customer_Name, string Customer_Contact, string refund_Customer, string refund_Contact, int? export_to_Destination_ID,
           int? port_ID, string Date_Return, string Date_Taken, string Temp_ID, int? c_ID, int? Created_by);
        public string Update_DepositVoucherMaster_DAL(int? DV_ID, int? Customer_ID, string Customer_Name, string Customer_Contact, string refund_Customer, string refund_Contact, int? export_to_Destination_ID,
           int? port_ID, string Date_Return, string Date_Taken, string Temp_ID, int? c_ID, int? Modified_by);
        public string UpdateDepositVoucherDetail_DAL(int? DVdetails_ID, int? Stock_ID, double? Car_Sale_Value, double? Deposit, double? Fix_Deductable_Charges, double? PaperExpense_Changes,
        double? VAT_Security_Deposit, double? Other_Charges, string External_Trans_Ref, string ShippingDate, string Submit_Date, string Temp_ID, int? c_ID, int? Modified_by);
        public IEnumerable<pa_ReceiptDetails_DAL> Get_ReceiptDetailDepositVoucherlListByID_DAL(string Temp_ID, int? DV_ID);
        public string DeleteDepositVoucherDetail_DAL(int? DVdetails_ID);
        public IEnumerable<Deposits_DAL> Get_DepositMaster_List_DAL(string DV_Ref, string Customer_Name, string StartDate, string EndDate, string Chassis_No, string Cheque_no, int? c_ID);

        public IEnumerable<Pa_PaymentDetails_DAL> Get_PaymentDetailListByID_DAL(string Temp_ID, int? DV_ID);
        public string InsertDepositReturnPaymentDetail_DAL(int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
         string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int DV_ID);

        public string UpdateDepositReturnPaymentDetail_DAL(int? PaymentDetails_ID, int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
          string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by);

        public string Insert_DepositReturnMaster_DAL(int? DV_ID, int? Customer_ID, string refund_Customer, string refund_Contact, string Date_Return, string Temp_ID, int? c_ID, int? Modified_by);
        public DataSet GetDataForPrintDtl(int? DV_ID);
        public DataSet GetDataForPrintDtl_Claim(int? DV_ID);
        public IEnumerable<Pa_Attachments_DAL> GetDVMaster_Attachments_DAL(int? DV_ID, string Type);


        //Insert sales invoice master attachment
        public string Insert_Attachment_DV_DAL(int? DV_ID, string Document_Type, string Type, string newFileName, string filepath, int? Created_by);

        public IEnumerable<Deposits_DAL> Get_DepositMaster_List_DAL1_1(string DV_Ref, string Customer_Name, string StartDate, string EndDate, string Chassis_No, string Cheque_no, int? c_ID);
        public string Delete_Attachment(int? Attachment_ID);
        public ChassisValidation_DAL ChassisValidationObj { get; set; }
        public IEnumerable<Deposits_DAL> DepositsDetailList { get; set; }
        public IPagedList<Deposits_DAL> DepositsDetailListIPageList { get; set; }
        public Deposits_DAL DepositMasterObj { get; set; }
        public IEnumerable<pa_ReceiptDetails_DAL> receiptDetailList { get; set; }
        public IEnumerable<Pa_PaymentDetails_DAL> paymentDetailList { get; set; }
        public IEnumerable<Pa_Attachments_DAL> attachmentList { get; set; }

    }
}
