namespace DAL.Models
{
    public class Pa_VehicleCategory_DAL
    {
        public int? VehCategory_ID { get; set; }
        public string VehCategoryName { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }
    }
}
