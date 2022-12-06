namespace DAL.Models
{
    public class Pa_Ledger_DAL
    {
        public int ID { get; set; }
        public string Date { get; set; }

        public string ACCOUNT { get; set; }

        public string Debit { get; set; }
        public string Credit { get; set; }
        public string Entry_Type { get; set; }
        public string Description { get; set; }
        public string trans_ref { get; set; }
        public string trans_ref_ID { get; set; }

        public string Dr_Cr { get; set; }
        public string Alt_AccountID { get; set; }

        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string last_updated_at { get; set; }
        public string Last_udpated_by { get; set; }
        public string c_ID { get; set; }
        public string subHeadAccount_ID { get; set; }
        public string Party_ID { get; set; }
        public string Stock_ID { get; set; }
        public string vendor_ID { get; set; }
        public string customer_ID { get; set; }
        public string VoucherType { get; set; }
        public string trans_created_at { get; set; }

        public string Link { get; set; }

        public string TotalDebit { get; set; }
        public string TotalCredit { get; set; }

        public string TotalBalance { get; set; }
        public string Opening_Balance { get; set; }
        public string Closing_Balance { get; set; }
        public int Account_ID { get; set; }


    }
}
