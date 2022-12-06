namespace DAL.Models
{
    public class JournalVoucher_DAL
    {
        public int? JVMaster_ID { get; set; }
        public string JVMaster_ref { get; set; }
        public string Date { get; set; }

        public int? c_ID { get; set; }


        public int? JVDetails_ID { get; set; }

        public int? Account_ID { get; set; }
        public double? DR_Amount { get; set; }
        public double? CR_Amount { get; set; }
        public string Description { get; set; }
        public int? Party_ID { get; set; }

        public string temp_ID { get; set; }
        public string AccountName { get; set; }




        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }








    }
}
