namespace DAL.Models
{
    public class Pa_EmpDocument_DAL
    {
        public int ID { get; set; }
        public int? empDoc_ID { get; set; }
        public int? emp_ID { get; set; }
        public string empDoc_Type { get; set; }
        public string empDoc_FileName { get; set; }
        public string empDoc_filePath { get; set; }
        public string empDoc_ExpiryDate { get; set; }
        public bool empDoc_altert_isRead { get; set; }
        public int? empDoc_altert_completed_by { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }
    }
}
