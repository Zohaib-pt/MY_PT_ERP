namespace DAL.Models
{
    public class DashboardStats
    {
        public string Today_Sale { get; set; }
        public string Total_Payables { get; set; }
        public string Total_Recievables { get; set; }
        public string UnSold_Count { get; set; }
        public string UnSold_Value { get; set; }

        public string Stock_Sold_Count { get; set; }
        public string DAYSS { get; set; }
        public string AMT { get; set; }
        public string Sale { get; set; }
        public string Item_Name { get; set; }
        public string Avg_Currency { get; set; }
        public string Total_Payments_YEN { get; set; }
        public string Total_Payments_AED { get; set; }
        public string Investment_Acc_Name { get; set; }

        //By Yaseen
        public string Total_Cash { get; set; }
        public string All_Bank_Total { get; set; }

        public string BankName { get; set; }
        public string BankBalance { get; set; }
        public string TotalAssets { get; set; }



    }
}
