namespace DAL.Models
{
    public class Pa_CurrencyMaster_DAL
    {

        public int ID { get; set; }

        public int? Currency_ID { get; set; }
        public string Currency_Name { get; set; }
        public string Currency_ShortName { get; set; }
        public string Curr_Rate { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }
        public string Minor_ShortName { get; set; }
        public string IsDeleted { get; set; }

    }
}
