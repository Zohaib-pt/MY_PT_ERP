namespace DAL.Models
{
    public class pa_PaymentMaster_DAL
    {

        public int? PaymentMaster_ID { get; set; }
        public string PaymentMaster_ref { get; set; }
        public string Party_ID { get; set; }
        public string party_type { get; set; }
        public string Party_ID_Name { get; set; }


        public string NameText { get; set; }
        public string Date { get; set; }
        public string Explanation { get; set; }
        public string Amount { get; set; }
        public string VAT_Rate { get; set; }
        public string VAT_Exp { get; set; }
        public string Total_Amount { get; set; }
        public string Paymenttype { get; set; }
        public string PaymentRef_Sno { get; set; }
        public string Recievedby_Name { get; set; }
        public string transaction_status { get; set; }
        public string trans_ref_ID { get; set; }
        public string c_ID { get; set; }
        public string approval_status { get; set; }
        public string PaymentStatus { get; set; }
        public string Verifiedby_userID { get; set; }
        public string preparedby_userID { get; set; }
        public string approvedby_userID { get; set; }
        public string approved_at { get; set; }
        public string Temp_ID { get; set; }

        public string create_at { get; set; }
        public string create_by { get; set; }
        public string last_updated_at { get; set; }
        public string last_updated_by { get; set; }

        public string IsDeleted { get; set; }
        public string PurchaseDate { get; set; }
        public string Type { get; set; }
        public string Ref { get; set; }
        public string Paid { get; set; }
        public string Balance { get; set; }
        public int PaymentDetail_ID { get; set; }
        public string AfterVATTotal { get; set; }
        public string PartyName { get; set; }
        public string ContactNumber { get; set; }
        public string DR_accountName { get; set; }
        public string Description { get; set; }
        public string cheque_no { get; set; }
        public string cheque_date { get; set; }
        public string Chassis_No { get; set; }
        public string Currency_ShortName { get; set; }
        public string Minor_ShortName { get; set; }
        public string Total_Amt_inWords { get; set; }

    }
}
