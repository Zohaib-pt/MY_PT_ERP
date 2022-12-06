using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public  class AlertNotification_DAL
    {
       
        public int ID { get; set; }
        public string type { get; set; }
        public string Ref { get; set; }
        public string Description { get; set; }
        public string Voucher_Amt { get; set; }
        public string Ledger_Amt { get; set; }
        public string Fix { get; set; }
        public string AlertDescription { get; set; }
        
        public int alert_count { get; set; }
        
        public DateTime Create_At { get; set; }


        public string Link { get; set; }




    }
}
