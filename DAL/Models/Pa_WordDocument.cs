using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
   public class Pa_WordDocument
    {
        public int ID { get; set; }
        public int ApprovedBy { get; set; }

        public string ApprovedBy_Name { get; set; }
        public int? WordDocument_ID { get; set; }
        public string WordRef { get; set; }
        public int? c_ID { get; set; }
        public string FileName { get; set; }
        public string PDFName { get; set; }
        public string Filepath { get; set; }
        public string CustomerName { get; set; }
        public string VendorName { get; set; }
        public string Approved_at { get; set; }
        public string Created_at { get; set; }

        public string Createby_name { get; set; }
        public string WordTitle { get; set; }
        public string imgname { get; set; }
    }
}
