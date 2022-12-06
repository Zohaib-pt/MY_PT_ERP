using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class Pa_Garage_Details
    {

        public int ID { get; set; }
        public int Garage_Details_ID { get; set; }
        public int? Garage_Master_ID { get; set; }
        public int? Stock_ID { get; set; }
        public string Chassis { get; set; }
        public string Description { get; set; }        
        public string Amount { get; set; }

    }
}
