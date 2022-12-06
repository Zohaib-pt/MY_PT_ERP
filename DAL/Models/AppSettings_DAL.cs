namespace DAL.Models
{
    public class AppSettings_DAL
    {
        public int? ID { get; set; }
        public string NAME { get; set; }
        public string VALUE { get; set; }

        public int? isConfigurable { get; set; }
        public string type { get; set; }
    }
}
