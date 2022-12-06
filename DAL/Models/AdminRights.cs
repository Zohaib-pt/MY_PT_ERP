namespace DAL.Models
{
    public class AdminRights
    {
        public int ID { get; set; }
        public int MenuID { get; set; }
        public int UserId { get; set; }
        public bool View { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool Print { get; set; }
        public bool Excel { get; set; }
        public bool IsAdmin { get; set; }
        public string Menu { get; set; }
        public string RoleName { get; set; }
        public string MenuType { get; set; }








    }
}
