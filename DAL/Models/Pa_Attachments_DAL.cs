namespace DAL.Models
{
    public class Pa_Attachments_DAL
    {


        public int? Attachment_ID { get; set; }
        public int? Master_ID { get; set; }
        public string File_Name { get; set; }
        public string File_Path { get; set; }

        public string Document_Type { get; set; }


        public string Type { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }

    }
}
