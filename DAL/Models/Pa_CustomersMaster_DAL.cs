namespace DAL.Models
{
    public class Pa_CustomersMaster_DAL
    {
        public int ID { get; set; }
        public int? Customer_ID { get; set; }
        public string Customer_Ref { get; set; }
        public string CustomerName { get; set; }
        public string ContactNumber { get; set; }
        public string MobileNo { get; set; }
        public string ContactName { get; set; }
        public string ContactAddress { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string EmiratesID { get; set; }
        public string TradeLicenseNo { get; set; }
        public string TRN { get; set; }

        public string CustomerCat_ID { get; set; }

        public string OpeningBalance { get; set; }
        public string OpeningBalanceDate { get; set; }

        public string BalanceType { get; set; }
        public string Remarks { get; set; }

        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }





    }
}



