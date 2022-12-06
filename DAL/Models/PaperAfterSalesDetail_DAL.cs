using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
   public class PaperAfterSalesDetail_DAL
    {
        public int ID { get; set; }
        public int papers_ID { get; set; }
        public int Stock_ID { get; set; }
        public int SaleMaster_ID { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }
        public string Chassis_No { get; set; }
        public string Registration { get; set; }
        public string CustomerName { get; set; }
        public string Amount { get; set; }
        public string Price { get; set; }
    }
}
