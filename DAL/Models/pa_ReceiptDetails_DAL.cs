namespace DAL.Models
{
    public class pa_ReceiptDetails_DAL
    {
        public int? ReceiptDetail_ID { get; set; }
        public int? ReceiptMaster_ID { get; set; }
        public string Party_ID { get; set; }
        public string Stock_ID { get; set; }
        public string trans_ref { get; set; }
        public string Receipttype { get; set; }
        public string link_trans_ref_ID { get; set; }
        public string CR_accountID { get; set; }
        public string DR_accountID { get; set; }
        public string ReceiptDate { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string VAT_Exp { get; set; }
        public string VAT_Rate { get; set; }
        public string Total_Amount { get; set; }
        public string Total_Amount_OtherCurrency { get; set; }
        public string Currency_ID { get; set; }

        public string Currency_ShortName { get; set; }
        public string Currency_Rate { get; set; }
        public string other_curr_amount { get; set; }
        public string Sales_ID { get; set; }
        public string Cheque_no { get; set; }
        public string cheque_Date { get; set; }
        public string Cheque_Bank_Name { get; set; }
        public string transaction_status { get; set; }
        public string c_ID { get; set; }
        public string Temp_ID { get; set; }
        public string IsDeleted { get; set; }


        //---Following fields not exists in actual table
        public string Grand_Amount { get; set; }

        public string Grand_VAT_Exp { get; set; }
        public string Grand_Amount_Others { get; set; }
        public string Grand_Total_Amount { get; set; }

        public string DR_AcountName { get; set; }

        public string CR_AccountName { get; set; }
        public int PDC_Recieving_Acc_ID { get; set; }



    }
}
