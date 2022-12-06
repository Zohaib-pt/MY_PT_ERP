using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
  public  class PDCReport_DAL
    {

        public string PaymentType { get; set; }

        public string cheque_date { get; set; }
        public string cheque_no { get; set; }
        public string bank_name { get; set; }
        public string ID { get; set; }
        public string Ref_ID { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }

        public int Master_ID { get; set; }
        public int user_ID { get; set; }
        public int IsChequeCleared   { get; set; }

        
    }
}
