
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using X.PagedList;

namespace DAL.oClasses
{
    public interface IOAccounts
    {
        #region properties area
        public pa_PaymentMaster_DAL paymentMasterObject { get; set; }
        public IEnumerable<pa_PaymentMaster_DAL> paymentMasterList { get; set; }

        //new faraz work
        public IEnumerable<Pa_TrailBalance_DAL> TrailBalanceList { get; set; }
        public IEnumerable<Pa_TrailBalance_DAL> TrailBalanceList_ttl { get; set; }
        //New faraz work
        public IEnumerable<Pa_Ledger_DAL> LedgerMasterIPagedList_1 { get; set; }


        //---

        //New faraz work

        //--
        public IPagedList<pa_PaymentMaster_DAL> paymentMasterListIPaged { get; set; }

        public IEnumerable<pa_PaymentMaster_DAL> PaymentMasterListTTL_obj { get; set; }

        public Pa_PaymentDetails_DAL paymentDetailObject { get; set; }
        public IEnumerable<Pa_PaymentDetails_DAL> paymentDetailList { get; set; }

        public ChassisValidation_DAL ChassisValidationObj { get; set; }
        public pa_ReceiptDetails_DAL receiptDetailObject { get; set; }
        public IEnumerable<pa_ReceiptDetails_DAL> receiptDetailList { get; set; }
        public pa_ReceiptMaster_DAL receiptMasterObject { get; set; }
        public IEnumerable<pa_ReceiptMaster_DAL> receiptMasterList { get; set; }

        public IEnumerable<pa_ReceiptMaster_DAL> receiptMasterList_TTL { get; set; }
        public IPagedList<pa_ReceiptMaster_DAL> receiptMasterIPagedList { get; set; }

        public IEnumerable<Pa_Ledger_DAL> LedgerMasterList { get; set; }
        public IPagedList<Pa_Ledger_DAL> LedgerMasterIPagedList { get; set; }
        public IEnumerable<Pa_Ledger_DAL> LedgerMasterIPagedList_TTL { get; set; }

        //  public IEnumerable<Pa_TrailBalance_DAL> TrailBalanceList { get; set; }

        public Accounts_DAL ChartofAccountObject { get; set; }
        public IEnumerable<Accounts_DAL> ChartofAccountList { get; set; }

        public IEnumerable<Approvals_DAL> ApprovalObjList { get; set; }
        public IEnumerable<Pa_Attachments_DAL> attachmentList { get; set; }
        public IEnumerable<pa_PaymentMaster_DAL> PaymentBalanceList { get; set; }
        public IEnumerable<pa_ReceiptMaster_DAL> receiptmasterlist { get; set; }
        public IEnumerable<pa_ReceiptMaster_DAL> receiptMasterIPagedList1 { get; set; }
        public IEnumerable<pa_PaymentMaster_DAL> paymentMasterListIPaged1 { get; set; }
        public IEnumerable<pa_ReceiptMaster_DAL> receiptMasterList_TTL1 { get; set; }

        public IEnumerable<pa_PaymentMaster_DAL> PaymentMasterListTTL_obj1 { get; set; }
        public IEnumerable<Pa_Ledger_DAL> LedgerMasterIPagedList1 { get; set; }
        #endregion

        #region BalanceSheet objects


        public IEnumerable<Balance_Sheet> BalanceSheet_obj { get; set; }
        public IEnumerable<Balance_Sheet> BalSh_byCurrentAssets_obj { get; set; }
        public IEnumerable<Balance_Sheet> BalSh_byCurrentLaibilities_obj { get; set; }
        public IEnumerable<Balance_Sheet> BalSh_byFixedAssets_obj { get; set; }
        public IEnumerable<Balance_Sheet> BalSh_byLongTermLaibilities_obj { get; set; }
        public IEnumerable<Balance_Sheet> BalSh_byDrawings_obj { get; set; }
        public IEnumerable<Balance_Sheet> BalSh_byCapitalInvestments_obj { get; set; }
        public IEnumerable<Balance_Sheet> Get_NetIncome_onPrint_obj { get; set; }



        #endregion


        #region IncomeStatement

        public IEnumerable<IncomeStatement> IncomeStatment_INC_obj { get; set; }
        public IEnumerable<IncomeStatement> IncomeStatment_Exp_obj { get; set; }
        public IEnumerable<IncomeStatement> IncomeStatment_CGS_obj { get; set; }
        public IEnumerable<IncomeStatement> IncomeStatment_Exp_Fin_obj { get; set; }
        public IEnumerable<IncomeStatement> IncomeStatment_disRet_obj { get; set; }
        public IEnumerable<IncomeStatement> IncomeStateMent_obj { get; set; }

        #endregion


        #region methods signature area

        public IEnumerable<Approvals_DAL> Get_ApprovalList_DAL(int c_ID);

        public IEnumerable<Accounts_DAL> get_AllAccountList_DAL(int c_ID);

        public IEnumerable<Accounts_DAL> get_HeadAccountList_DAL(int c_ID);


        public string InsertPaymentDetail_General_DAL(int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount,
int? stock_ID, string Temp_ID, int? c_ID, int? Created_by, int PaymentMaster_ID, double TaxAmount, 
double ExtraCharges, double ExtraChargesTax, double TaxAmountOthers,string BLNO, int PDC_Payable_Acc_ID);

        //new faraz  

        public IEnumerable<Pa_TrailBalance_DAL> Get_TrailBalanceList_DAL(string StartDate, String EndDate, int c_ID);

        //new work
        public IEnumerable<Pa_Ledger_DAL> LedgerMasterIPagedList_TTL1 { get; set; }

        public IEnumerable<Pa_Ledger_DAL> Get_LedgerList_DAL_NEW(string StartDate, int AccountID, String EndDate, string Trans_Ref, int c_ID);
        //---

        //by Rafay Start Date 14/jan/2021
        public IEnumerable<pa_PaymentMaster_DAL> Get_Ref_Tran_Payment(string PaymentDetail_ID);

        public IEnumerable<Pa_PaymentDetails_DAL> Get_Trans_Ref { get; set; }
        public IEnumerable<TaxReport> PaidTax_PR { get; set; }
        public IEnumerable<TaxReport> PaidTax_TTL_PR { get; set; }


        public IEnumerable<TaxReport> ReceivedTax_PR { get; set; }
        public IEnumerable<TaxReport> ReceivedTax_TTL_PR { get; set; }

        //by Rafay End Date 14/jan/2021




        public string UpdatePaymentDetail_General_DAL(int? PaymentDetails_ID, int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
            string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, 
            double? VAT_Exp, double? Total_Amount, int? stock_ID, string Temp_ID, int? c_ID, int? Modified_by, 
            double ExtraCharges, double ExtraChargesTax, double TaxAmount, double TaxAmountOthers,string BLNO, int PDC_Payable_Acc_ID);

        public pa_PaymentMaster_DAL Get_PaymentMasterByID_DAL(int? PaymentMaster_ID);

        public IEnumerable<Pa_PaymentDetails_DAL> Get_PaymentDetailListByID_DAL(string Temp_ID, int? PaymentMaster_ID);
        public string GetOldTempIDFromPaymentDetail_DAL(int? PaymentMaster_ID);
        public string DeletePaymentDetail_DAL(int? PaymentDetails_ID);
        public string Insert_PaymentGeneral_DAL(int? Vendor_ID, int? party_type, string Explanation, string PaymentType, string Date, string NameText, string Temp_ID, int? c_ID, int? Created_by);




        public string Update_PaymentGeneral_DAL(int? PaymentMaster_ID, int? party_type, int? Vendor_ID, string Explanation, string Date, string NameText, string Temp_ID, int? c_ID, int? Modified_by);
        public ChassisValidation_DAL ValidateChassisNo_DAL(string ChassisNo, int c_ID);
        public ChassisValidation_DAL ValidateChassisNo_UNSOLD_DAL(string ChassisNo, int c_ID);
        public IEnumerable<pa_PaymentMaster_DAL> Get_PaymentMasterList_DAL(string PaymentRef, int Party_ID_Name, string StartDate,
             string EndDate, string Chassis_No, string Paid_To, string PurchaseRef, string VoucherType, int c_ID, string Cheque_no);

        public IEnumerable<pa_PaymentMaster_DAL> Get_PaymentMasterList_TTL_DAL(string PaymentRef, int Party_ID_Name, string StartDate,
          string EndDate, string Chassis_No, string Paid_To, string PurchaseRef, string VoucherType, int c_ID, string Cheque_no);

        public IEnumerable<pa_ReceiptMaster_DAL> Get_ReceiptMaster_List_DAL(string ReceiptRef,
            int Party_ID, string PartyNameText, string SaleRef, string Chassis_no, string StartDate, string EndDate, string Cheque_no, int c_ID);

        public IEnumerable<pa_ReceiptMaster_DAL> Get_ReceiptMaster_List_TTL_DAL(string ReceiptRef,
            int Party_ID, string PartyNameText, string SaleRef, string Chassis_no, string StartDate, string EndDate, string Cheque_no, int c_ID);

        public pa_ReceiptMaster_DAL Get_ReceiptMasterGeneralByID_DAL(int? ReceiptMaster_ID);

        public IEnumerable<pa_ReceiptDetails_DAL> Get_ReceiptDetailGeneralListByID_DAL(string Temp_ID, int? ReceiptMaster_ID);

        public IEnumerable<Pa_Ledger_DAL> Get_LedgerList_DAL(string StartDate, int AccountID, String EndDate, string Trans_Ref, int c_ID);
        public IEnumerable<Pa_Ledger_DAL> Get_LedgerList_DAL_TTL(string StartDate, int AccountID, String EndDate, string Trans_Ref, int c_ID);


        public string InsertReceiptDetail_General_DAL(int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
            double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date,
            string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int DV_ID, int ReceiptMaster_ID,int PDC_Recieving_Acc_ID);

        public string InsertReceiptDetailDeposit_General_DAL(int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
    double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date,
    string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int DV_ID, int ReceiptMaster_ID);
        public string Insert_ReceiptMasterGeneral_DAL(int? Party_ID, int? party_type, string NameText, string Explanation, string Receipttype, string ReceiptDate, string Temp_ID, int? c_ID, int? Created_by);
        public string Update_ReceiptMasterGeneral_DAL(int? ReceiptMaster_ID, int? Party_ID, int? party_type, string NameText, string Explanation, string ReceiptDate, string Temp_ID, int? c_ID, int? Modified_by);

        public string DeleteReceiptMaster_DAL(int? ReceiptMaster_ID);

        public string DeletePaymentMaster_DAL(int? PaymentMaster_ID);
        public string UpdateReceiptDetail_General_DAL(int? ReceiptDetail_ID, int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
            double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date,
            string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by, int PDC_Recieving_Acc_ID);

        public string DeleteReceiptDetail_DAL(int? ReceiptDetail_ID);
        public string GetOldTempIDFromReceiptDetail_DAL(int? ReceiptMaster_ID);
        public pa_ReceiptMaster_DAL Get_ReceiptMasterSaleByID_DAL(int? ReceiptMaster_ID);
        public IEnumerable<pa_ReceiptDetails_DAL> Get_ReceiptDetailSaleListByID_DAL(string Temp_ID, int? ReceiptMaster_ID);
        public string InsertReceiptDetail_Sale_DAL(int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
          double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date,
          string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int ReceiptMaster_ID, int PDC_Recieving_Acc_ID);

        public string UpdateReceiptDetail_Sale_DAL(int? ReceiptDetail_ID, int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
           double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date,
           string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by, int PDC_Recieving_Acc_ID);
        public string Insert_ReceiptMasterSale_DAL(int? Party_ID, string NameText, string Explanation, string Receipttype, string ReceiptDate, string Temp_ID, int? c_ID, int? Created_by);
        public string Update_ReceiptMasterSale_DAL(int? ReceiptMaster_ID, int? Party_ID, string NameText, string Explanation, string ReceiptDate, string Temp_ID, int? c_ID, int? Modified_by);

        public pa_PaymentMaster_DAL Get_PaymentMasterVendorByID_DAL(int? PaymentMaster_ID);

        public IEnumerable<Pa_PaymentDetails_DAL> Get_PaymentDetailVendorListByID_DAL(string Temp_ID, int? PaymentMaster_ID);

        public string InsertPaymentDetail_Vendor_DAL(int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
          string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount, int? stock_ID, string Temp_ID, int? c_ID, int? Created_by, int PaymentMaster_ID, double TaxAmount, double ExtraCharges, double ExtraChargesTax, int PDC_Payable_Acc_ID);
        public string UpdatePaymentDetail_Vendor_DAL(int? PaymentDetails_ID, int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
          string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount, int? stock_ID, string Temp_ID, int? c_ID, int? Modified_by, double TaxAmount, double ExtraCharges, double ExtraChargesTax, int PDC_Payable_Acc_ID);
        public string Insert_PaymentMasterVendor_DAL(int? Vendor_ID, int? party_type, string Explanation, string PaymentType, string Date, string NameText, string Temp_ID, int? c_ID, int? Created_by);

        public string Update_PaymentVendor_DAL(int? PaymentMaster_ID, int? party_type, int? Vendor_ID, string Explanation, string Date, string NameText, string Temp_ID, int? c_ID, int? Modified_by);


        public string Insert_Accounts(string AccountName, string Acc_ShortName, int HeadAccounts_ID, int subHeadAccount_ID, int Sys_AC_type_ID,
        string Opening_Balance, string Opening_Bal_Date, int User_ID, int c_ID);

        public string Update_Accounts(string AccountName, string Acc_ShortName, int HeadAccounts_ID, int subHeadAccount_ID, int Sys_AC_type_ID,
        string Opening_Balance, string Opening_Bal_Date, int User_ID, int Account_ID, int c_ID);
        
        
        //By Yaseen 12-31-2021

        public string DeleteAccounts_DAL(int Account_ID, int c_ID);

        //By Yaseen 12-31-2021
        public IEnumerable<Accounts_DAL> Get_select_AccountsList_DAL(string AccountName, int? HeadAccounts_ID, int? subHeadAccount_ID, int Sys_AC_type_ID, int c_ID);
        public Accounts_DAL Get_select_Account_By_ID_DAL(int? Account_ID);
        public string ChangePaymentMasterStatus_DAL(int? PaymentMaster_ID, int? Status_ID, int? c_ID, string Trans_Type, int? Modified_by);

        public string ChangeReceiptMasterStatus_DAL(int? ReceiptMaster_ID, int? Status_ID, int? c_ID, string Trans_Type, int? Modified_by, string QRName);
        public IEnumerable<Pa_Attachments_DAL> GetPaymentsMaster_Attachments_DAL(int? PaymentMaster_ID, string Type);
        public string Insert_AttachmentPayment_DAL(int? PaymentMaster_ID, string Document_Type, string Type, string newFileName, string filepath, int? Created_by);
        public string Delete_Attachment_Payment(int? Attachment_ID);
        public IEnumerable<Pa_Attachments_DAL> GetReceiptMaster_Attachments_DAL(int? ReceiptMaster_ID, string Type);
        public string Insert_AttachmentReceipt_DAL(int? ReceiptMaster_ID, string Document_Type, string Type, string newFileName, string filepath, int? Created_by);

        public DataSet GetDataForPrintPaymentMaster(int? PaymentMaster_ID);
        public DataSet GetDataForPrintReceiptMaster(int? ReceiptMaster_ID);

        #endregion

        #region Journal Voucher

        //property
        public JournalVoucher_DAL JournalVoucherDetailObject { get; set; }
        public IEnumerable<JournalVoucher_DAL> JournalVoucherDetailList { get; set; }
        public JournalVoucher_DAL JournalVoucherMasterObject { get; set; }
        public IEnumerable<JournalVoucher_DAL> JournalVoucherMasterList { get; set; }




        //method
        public IEnumerable<JournalVoucher_DAL> Get_JournalVoucher_master_DAL();
        public IEnumerable<JournalVoucher_DAL> Get_JournalVoucher_DetailListByID_DAL(int? JVMaster_ID, string temp_ID);
        public string Insert_JournalVoucherMaster_DAL(string date, int? c_ID, string temp_ID, int? Created_by);
        public string Insert_JournalVoucherDetail_DAL(string Date, int? Account_ID, double? DR_Amount, double? CR_Amount, string Description, int? Party_ID, string temp_ID, int? Created_by);

        public JournalVoucher_DAL Get_JournalVoucherMaster_Id(int? JVMaster_ID);
        public string Update_JournalVoucher_DAL(string date, int? JVMaster_ID, int? Modified_by);


        public string Update_JournalVoucher_detail_DAL(int? JVDetails_ID, string Date, int? Account_ID, double? DR_Amount, double? CR_Amount, string Description, int? Party_ID, int? Modified_by);

        public string GetOldTempIDFromJVDetail_DAL(int? JVMaster_ID);

        #endregion

        #region  Financial Statements


        public List<IncomeStatement> IncomeStatment_INC_DAL(string StartDate, string EndDate, int? c_ID);

        public List<IncomeStatement> IncomeStatment_Exp_DAL(string StartDate, string EndDate, int? c_ID);

        public List<IncomeStatement> IncomeStatment_CGS_DAL(string StartDate, string EndDate, int? c_ID);

        public List<IncomeStatement> IncomeStatment_disRet(string StartDate, string EndDate, int? c_ID);

        public List<IncomeStatement> IncomeStatment_Exp_Fin_DAL(string StartDate, string EndDate, int? c_ID);

        public List<IncomeStatement> IncomeStateMent_DAL(string StartDate, string EndDate, int? c_ID);

        public List<Balance_Sheet> Balance_Sheet_DAL(string StartDate, string EndDate, int? c_ID);

        public List<Balance_Sheet> BalSh_byCurrentAssets_DAL(string StartDate, string EndDate, int? c_ID);

        public List<Balance_Sheet> BalSh_byDrawings_DAL(string StartDate, string EndDate, int? c_ID);

        public List<Balance_Sheet> BalSh_byCurrentLaibilities_DAL(string StartDate, string EndDate, int? c_ID);

        public List<Balance_Sheet> BalSh_byLongTermLaibilities_DAL(string StartDate, string EndDate, int? c_ID);

        public List<Balance_Sheet> BalSh_byFixedAssets_DAL(string StartDate, string EndDate, int? c_ID);

        public List<Balance_Sheet> BalSh_byCapitalInvestments_DAL(string StartDate, string EndDate, int? c_ID);

        public List<Balance_Sheet> Get_NetIncome_onPrint_DAL(string StartDate, string EndDate, int? c_ID);

        #endregion

        public List<pa_PaymentMaster_DAL> pa_GetParty_Balance(string Vendor_ID = "0", int PaymentMaster_ID = 0);
        public List<pa_ReceiptMaster_DAL> pa_GetParty_Balance_Receipt(string CustomerID = "0" ,int ReceiptMaster_ID = 0, int C_id = 0);
        public IEnumerable<Pa_Partners_DAL> Get_tblaccounts_VendorName(int party_type, int c_ID);
        public pa_Vanning_Details ValidateChassisNo_JP(string ChassisNo);

        //by Rafay Start Date : 13/01/2021 

        public string Insert_pa_trans_ref_payment_DAL(int? Created_by, string htemp_ID, string Trans_Ref, string Total_Amount, string Currency_Rate);
        public string DeletePaymentDetail_byMasterID_DAL(int? PaymentMaster_ID);
        //by Rafay End Date : 13/01/2021 



        public IEnumerable<pa_PaymentMaster_DAL> Get_PaymentMasterList_DAL1(string PaymentRef, int Party_ID_Name, string StartDate,
string EndDate, string Chassis_No, string Paid_To, string PurchaseRef, string VoucherType, int c_ID);

        public IEnumerable<pa_PaymentMaster_DAL> Get_PaymentMasterList_TTL_DAL1(string PaymentRef, int Party_ID_Name, string StartDate,
         string EndDate, string Chassis_No, string Paid_To,
         string PurchaseRef, string VoucherType, int c_ID);
        public IEnumerable<pa_ReceiptMaster_DAL> Get_ReceiptMaster_List_DAL1(string ReceiptRef,
            int Party_ID, string PartyNameText, string SaleRef, string Chassis_no, string StartDate, string EndDate, string Cheque_no, int c_ID);

        public IEnumerable<pa_ReceiptMaster_DAL> Get_ReceiptMaster_List_TTL_DAL1(string ReceiptRef,
            int Party_ID, string PartyNameText, string SaleRef, string Chassis_no, string StartDate, string EndDate, int c_ID);
        public IEnumerable<Pa_Ledger_DAL> Get_LedgerList_DAL_TTL1(string StartDate, int AccountID, string EndDate,
       string Trans_Ref, int c_ID);

        public IEnumerable<PartyType_DAL> Get_Party_Type_By_Name();
        public IEnumerable<TaxReport> Get_PaidTax_Report_jp(string StartDate, string EndDate);

        public IEnumerable<TaxReport> Get_PaidTax_Report_ttl_jp(string StartDate, string EndDate);

        public IEnumerable<TaxReport> Get_ReceivedTax_Report_jp(string StartDate, string EndDate);

        public IEnumerable<TaxReport> Get_ReceivedTax_Report_ttl_jp(string StartDate, string EndDate);

        public IEnumerable<RecycleReport> Get_Recycle_Report_jp(string StartDate, string EndDate, string ChassisNo);

        public IEnumerable<RecycleReport> Get_Recycle_Report_ttl_jp(string StartDate, string EndDate, string ChassisNo);
        public IEnumerable<Pa_TrailBalance_DAL> Get_TrailBalanceList_TTL(string StartDate, String EndDate, int c_ID);



        // By Yaseen   31-12-2021
        #region PDC Report
        public IEnumerable<PDCReport_DAL> Get_PDCReport_DAL(string StartDate, string EndDate, string cheque_no, string Ref_ID, int c_ID);
        
        public IPagedList<PDCReport_DAL> PDCReportPageList { get; set; }

        public string Post_PDCReport_DAL(int? Pv_id, int user_ID, int C_id);

        #endregion
        // By Yaseen   31-12-2021


        public ChassisValidation_DAL ValidateBLNO_DAL(string BLNO, int c_ID);

        public Pa_Ledger_DAL AccountMaster_obj { get; set; }
        public Pa_Ledger_DAL Get_LedgerList_DAL(int AccountID, int c_ID);

        public string Update_Post_PDC_DAL(string date, int Post_Master_ID, int Post_user_ID, string Post_vType,int C_ID);
        public pa_ReceiptMaster_DAL Get_ReceiptMasterCustomerByID_DAL(int? ReceiptMaster_ID);
        public pa_ReceiptMaster_DAL ReceiptMasterObject { get; set; }
        public string Insert_pa_trans_ref_Receipt_DAL(int? Created_by, string htemp_ID, string Trans_Ref, string Total_Amount, string Currency_Rate);
    }
}
