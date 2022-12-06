using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
   public class TaxReport
    {
        public string Chassis_No { get; set; }
        public string PurchaseDate { get; set; }
        public string SaleDate { get; set; }
        public string PriceTax { get; set; }
        public string AuctionFeeTax { get; set; }

        public string ReksoFeeTax { get; set; }
        public string Tax_Amount { get; set; }
        public string Inspection_ChargesTax { get; set; }
        public string Berth_Carry_ChargesTax { get; set; }
        public string ExtraChargesFeeTax { get; set; }
        public string TaxAmount { get; set; }
        public string TaxAmountOthers { get; set; }
        public string OfficeChargesTax { get; set; }
        public string Total { get; set; }
        public string PriceTax_ttl { get; set; }
        public string AuctionFeeTax_ttl { get; set; }

        public string ReksoFeeTax_ttl { get; set; }
        public string Tax_Amount_ttl { get; set; }
        public string Inspection_ChargesTax_ttl { get; set; }
        public string Berth_Carry_ChargesTax_ttl { get; set; }
        public string ExtraChargesFeeTax_ttl { get; set; }
        public string TaxAmount_ttl { get; set; }
        public string TaxAmountOthers_ttl { get; set; }
        public string OfficeChargesTax_ttl { get; set; }
        public string Total_ttl { get; set; }
    }
}
