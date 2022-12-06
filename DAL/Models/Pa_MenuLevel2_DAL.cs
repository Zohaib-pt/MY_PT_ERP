namespace DAL.Models
{
    public class Pa_MenuLevel2_DAL
    {
        public int ID { get; set; }
        public int? MenuLevel2_ID { get; set; }
        public int? MenuLevel1_ID { get; set; }

        public string MenuLevel2Name { get; set; }
        public string IsVisible { get; set; }
        public int? MainMenu_ID { get; set; }

        public string Controller { get; set; }
        public string Action { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }
    }
}
