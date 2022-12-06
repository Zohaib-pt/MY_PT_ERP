namespace DAL.Models
{
    public class Pa_Year_DAL
    {
        public int ID { get; set; }
        public int? ModelYear_ID { get; set; }

        public string ModelYear { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }
    }
}
