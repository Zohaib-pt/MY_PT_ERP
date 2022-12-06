using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
     public  class PaySlipVouchar_DAL
    {


        public int emp_No { get; set; }
        public string emp_name { get; set; }
        public string Period_Desc { get; set; }
        public int Created_By { get; set; }
        public string Bal_Recieved { get; set; }
        public string Salary_Paid { get; set; }

        public int emp_PayRollDetail_ID { get; set; }



    }
}
