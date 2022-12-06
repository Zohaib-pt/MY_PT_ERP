namespace DAL.Models
{
    public class Balance_Sheet
    {
        public string Amount { get; set; }

        public string LINK { get; set; }
        public string Account { get; set; }

        public string TTL_Owner_Equity { get; set; }
        public string Diff { get; set; }

        public string TTL_Assets { get; set; }
        public string TTL_Current_Assets { get; set; }
        public string TTL_Fixed_Assets { get; set; }
        public string TTL_Current_Laibilities { get; set; }

        public string TTL_Longterm_Laibilities { get; set; }
        public string TTL_Laibilities { get; set; }
        public string TTL_Drawings { get; set; }
        public string Owner_Equity { get; set; }
        public string OwnerEquity_less_Drawing { get; set; }
        public string Net_Profit { get; set; }

        public string Net_PL { get; set; }
        public string C_Code { get; set; }
        public string Period { get; set; }

        //public string Total_Income { get; set; }
        //public string Net_Sale_Service_Income { get; set; }
        //public string cost_of_GoodSold { get; set; }
        //public string Gross_Profit { get; set; }
        //public string total_Expense { get; set; }
        //public string Operating_Profit { get; set; }
        //public string Financial_Expense { get; set; }
        //public string Net_PL { get; set; }
        //public string C_Code { get; set; }


    }
}
