namespace DAL.Models
{
    public class Pa_MainMenu_DAL
    {

        public int ID { get; set; }
        public int? MainMenu_ID { get; set; }
        public string MainMenuName { get; set; }

        public string IsExpandable { get; set; }
        public string IsVisible { get; set; }
        public string Icon { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }
    }
}
