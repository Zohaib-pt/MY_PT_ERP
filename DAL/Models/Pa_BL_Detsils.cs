using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class Pa_BL_Detsils
    {

        public int ID { get; set; }
        public int BLNO_Details_ID { get; set; }
        public int? BLNO_ID { get; set; }
        public int? Stock_ID { get; set; }
        public string Chassis { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }

    }
}
