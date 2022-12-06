using System;

namespace DAL.Models
{
    public class pa_ReceiptMaster_DAL
    {

        public int? ReceiptMaster_ID { get; set; }
        public int ApprovedBy { get; set; }
        public string ReceiptMaster_ref { get; set; }
        public string Party_ID { get; set; }
        public string party_type { get; set; }
        public string Party_ID_Name { get; set; }

        public string NameText { get; set; }
        public string ReceiptDate { get; set; }
        public string Explanation { get; set; }
        public string Amount { get; set; }
        public string Tax_Amount { get; set; }
        public string Total_Amount { get; set; }
        public string Receipttype { get; set; }
        public string ReceiptMaster_ref_sno { get; set; }
        public string Currency_ID { get; set; }
        public string Currency_Rate { get; set; }



        public string Recievedby_Name { get; set; }
        public string transaction_status { get; set; }
        public string trans_ref_ID { get; set; }
        public string c_ID { get; set; }
        public string approval_status { get; set; }
        public string ReceiptStatus { get; set; }


        public string recievedby_userID { get; set; }
        public string preparedby_userID { get; set; }
        public string approvedby_userID { get; set; }
        public string approved_at { get; set; }
        public string Temp_ID { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }
        public string IsDeleted { get; set; }
        public string Sale_Date { get; set; }
        public string Type { get; set; }
        public string Ref { get; set; }
        public string Paid { get; set; }
        public string Balance { get; set; }
        public Nullable<int> Sale_ID { get; set; }

        public string AfterVATTotal { get; set; }

        public string PartyName { get; set; }
        public string Description { get; set; }
        public string Cheque_no { get; set; }
        public string cheque_Date { get; set; }
        public string Cheque_Bank_Name { get; set; }
        public string Currency_ShortName { get; set; }
        public string Minor_ShortName { get; set; }
        public string Total_Amt_inWords { get; set; }
        public string QRName { get; set; }
        public string imgname { get; set; }
       
        public string Total_Sale { get; set; }



    }
}
