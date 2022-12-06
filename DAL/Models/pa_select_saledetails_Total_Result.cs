using System;

namespace DAL.Models
{
    public class pa_select_saledetails_Total_Result
    {
        public Nullable<decimal> TotalPrice { get; set; }
        public decimal TotalRecyclePrice { get; set; }
        public Nullable<decimal> TotalAuctionPrice { get; set; }
        public decimal TotalPlatNaumberPrice { get; set; }
        public Nullable<decimal> TotalTax { get; set; }
        public decimal GrandTotal { get; set; }
        public string Total_Amt_inWords { get; set; }
    }
}
