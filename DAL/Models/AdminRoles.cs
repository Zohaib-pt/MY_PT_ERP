using System;

namespace DAL.Models
{
    public class AdminRoles
    {
        public int ID { get; set; }
        public int Admin_Role_ID { get; set; }
        public Nullable<int> AdminUser_ID { get; set; }
        public string Role_Name { get; set; }
        public Nullable<int> RoleCategory_ID { get; set; }
        public Nullable<System.DateTime> Created_at { get; set; }
        public Nullable<int> Created_by { get; set; }
        public Nullable<System.DateTime> Modified_at { get; set; }
        public Nullable<int> Modified_by { get; set; }
        public Nullable<int> isDeleted { get; set; }

        public string AdminUserName { get; set; }
    }
}
