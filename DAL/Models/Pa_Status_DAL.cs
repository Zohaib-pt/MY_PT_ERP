namespace DAL.Models
{
    public class Pa_Status_DAL
    {

        public int ID { get; set; }
        public int? Status_ID { get; set; }
        public string Status { get; set; }
        public string StatusType { get; set; }

        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }
    }
}
