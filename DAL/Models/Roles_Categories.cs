using System;

namespace DAL.Models
{
    public class Roles_Categories
    {
        public int ID { get; set; }
        public int RoleCategory_ID { get; set; }
        public string Role_Name { get; set; }
        public string Role_Description { get; set; }
        public Nullable<System.DateTime> Created_at { get; set; }
        public Nullable<int> Created_by { get; set; }
        public Nullable<System.DateTime> Modified_at { get; set; }
        public Nullable<int> Modified_by { get; set; }
        public Nullable<int> isDeleted { get; set; }

        public string DateCreate { get; set; }
    }
}
