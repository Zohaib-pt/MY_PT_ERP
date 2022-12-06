using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class Pa_Dismental_Dtl
    {
        public int Dismental_Dtl_ID { get; set; }
        public int Dismental_ID { get; set; }
        public int Item_ID { get; set; }
        public string Quantity { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string Cost { get; set; }



    }
}
