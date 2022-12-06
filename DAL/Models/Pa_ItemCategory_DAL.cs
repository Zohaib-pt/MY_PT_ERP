namespace DAL.Models
{
    public class Pa_ItemCategory_DAL
    {
        public int ID { get; set; }
        public int Category_ID { get; set; }
        public string Category_Code { get; set; }
        public string Category_Name { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }
    }
}
