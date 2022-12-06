using System;

namespace DAL.Models
{
    public class Pa_AdminUsers_DAL
    {
        public int ID { get; set; }
        public int? AdminUser_ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Full_Name { get; set; }
        public int? c_ID { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }


        public string Designation { get; set; }
        public string Mobile { get; set; }
        public string Office_number { get; set; }
        public string Residence_Number { get; set; }

        public string Staff_No { get; set; }
        public string ImageName { get; set; }
        public Nullable<bool> isActive { get; set; }

        public int Role_id { get; set; }

    }
}
