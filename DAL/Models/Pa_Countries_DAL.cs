namespace DAL.Models
{
    public class Pa_Countries_DAL
    {
        public int? Country_ID { get; set; }
        public string CountryName { get; set; }
        public int? loctype_ID { get; set; }
        public string LocTypeName { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }

        public string LocType { get; set; }

    }
}
