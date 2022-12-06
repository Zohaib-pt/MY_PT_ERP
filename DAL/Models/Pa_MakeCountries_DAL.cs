namespace DAL.Models
{
    public class Pa_MakeCountries_DAL
    {
        public int? MakeCountry_ID { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }
    }
}
