using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class AccountingPeroid_DAL
    {
        public int ID { get; set; }
        public int Accounting_Period_ID { get; set; }
        //public DateTime Start_From { get; set; } 
        //public DateTime End_Till { get; set; }
        public string Year { get; set; }
        public string Period_Des { get; set; }
        //public Boolean isActive { get; set; }
        //public int c_ID { get; set; }

    }
}
