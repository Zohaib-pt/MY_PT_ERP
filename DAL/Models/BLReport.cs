using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
   public class BLReport
    {

        public string Date { get; set; }

        public string BLNO_Ref { get; set; }
        public string Chassis_no { get; set; }
        public string make { get; set; }

        public string model_description { get; set; }

        public string  Year { get; set; }


        public string Bl_Exp { get; set; }

        public string Total_Expense { get; set; }
        public string Total_Cost { get; set; }


        public int  stock_id { get; set; }






    }
}
