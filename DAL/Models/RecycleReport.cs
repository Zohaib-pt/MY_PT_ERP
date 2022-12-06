using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
  public  class RecycleReport
    {
        public string Chassis_No { get; set; }
        public string Date { get; set; }
        public string Ref { get; set; }
        public string RecycleFee_Out { get; set; }

        public string RecycleFee_In { get; set; }
        public string RecycleFee_Out_ttl { get; set; }

        public string RecycleFee_In_ttl { get; set; }
        public string Balance { get; set; }
    }
}
