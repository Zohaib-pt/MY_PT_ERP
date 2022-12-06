namespace DAL.Models
{
    public class BasicBanks_DAL
    {

        public int Bank_ID { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string IBAN { get; set; }
        public string SwiftCode { get; set; }
        public string branch { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string created_at { get; set; }
        public string created_by { get; set; }
        public string modified_at { get; set; }
        public string modified_by { get; set; }
    }
}