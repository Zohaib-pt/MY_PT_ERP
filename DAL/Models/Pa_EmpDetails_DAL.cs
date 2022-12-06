namespace DAL.Models
{
    public class Pa_EmpDetails_DAL
    {

        public int ID { get; set; }
        public int? Emp_Details_ID { get; set; }
        public int? emp_ID { get; set; }
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
    }
}
