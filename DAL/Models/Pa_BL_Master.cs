using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
   public class Pa_BL_Master
    {
        public int ID { get; set; }
        public int? BLNO_ID  { get; set; }
        public string BLNO_Date { get; set; }

        public string ShipName { get; set; }
        public string ShipLeavingDate { get; set; }
        public string Arrival_Date { get; set; }
        public string Clearing_Charges { get; set; }
        public string Custom_Duty { get; set; }
        public string Other_Charage { get; set; }
        public string Total_BL_Charges { get; set; }

        public string Ref { get; set; }
        public string Remarks { get; set; }

        public string Created_By { get; set; }
        public string Created_At { get; set; }
        public string Last_Updated_By { get; set; }
        public string Last_Updated_At { get; set; }

        public string c_ID { get; set; }
        public int Currency_ID { get; set; }
        public string Curr_Rate { get; set; }
        public string Total_With_Rate { get; set; }

        public int Vendor_ID {  get; set; }

        public string BlNO { get; set; }

        public string Paid_AED { get; set; }

        public string Balance_AED { get; set; }


        public string BLStatus_ID { get; set; }
        public string BLStatus { get; set; }







    }
}
