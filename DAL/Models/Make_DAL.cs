namespace DAL.Models
{
    public class Make_DAL
    {
        public int ID { get; set; }
        public int? Make_ID { get; set; }
        public string Make { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }

        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }

    }
}
