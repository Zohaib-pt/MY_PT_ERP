using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
   public class Pa_Garage_Master
    {
        public int ID { get; set; }
        public int? GarageMaster_ID  { get; set; }
        public string Date_Send { get; set; }
        public int? GarageVendor_ID { get; set; }
        public string GarageVendor_Name { get; set; }
        public string GarageVendorTo { get; set; }
        public string GarageVendor_Address { get; set; }
        public string Total_Amount { get; set; }
        public string RefInv { get; set; }
        public string Remarks { get; set; }
        public string Created_By { get; set; }
        public string Created_At { get; set; }
        public string Last_Updated_By { get; set; }
        public string Last_Updated_At { get; set; }
        public string c_ID { get; set; }


        public string Paid { get; set; }
        public string Balance { get; set; }



    }
}
