namespace DAL.Models
{
    public class Pa_EmpMaster_DAL
    {

        public int ID { get; set; }
        public int? emp_ID { get; set; }
        public string emp_No { get; set; }
        public string emp_Name { get; set; }
        public string Joining_Date { get; set; }
        public string Birth_Date { get; set; }
        public string Nationality { get; set; }
        public int? c_ID { get; set; }
        public int? Emp_Details_ID { get; set; }
        public string Desgination { get; set; }
        public string Basic_Salary { get; set; }
        public string Accomodation { get; set; }
        public string Transport { get; set; }
        public string Sp_Allowance { get; set; }
        public string Total_Salary { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public bool IsActive { get; set; }
        public string isDeleted { get; set; }


        //---Following fields not exists in actual table. These fields are just for data reading
        public string ProfileImageName { get; set; }
        public string ProfileImagePath { get; set; }


    }
}