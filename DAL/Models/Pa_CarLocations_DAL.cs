namespace DAL.Models
{
    public class Pa_CarLocations_DAL
    {
        public int? CarLocation_ID { get; set; }
        public string CarLocation { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }

        public string LocType { get; set; }

    }
}
