using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
   public class Pa_Assembly_Dtl
    {
        public int Assembly_Dtl_ID { get; set; }
        public int Assembly_ID { get; set; }
        public int Item_ID { get; set; }
        public string Quantity { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string Remarks { get; set; }
        public string Cost { get; set; }
    }
}
