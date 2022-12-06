namespace DAL.Models
{
    public class SaleDetailJP
    {
        public int Sale_ID { get; set; }
        public int SalesDetails_ID { get; set; }
        public string Sale_Ref { get; set; }
        public string Chassis_no { get; set; }
        public decimal Price { get; set; }
        public decimal PriceTax { get; set; }
        public decimal PriceRate { get; set; }
        public decimal RecycleFee { get; set; }
        public decimal PlateFee { get; set; }
        public decimal AuctionFee { get; set; }
        public decimal AuctionFeeTax { get; set; }
        public decimal Amount { get; set; }
        public string Make { get; set; }
        public string MakeModelDescription { get; set; }
        public string Color { get; set; }
        public string SaleType { get; set; }
        public string Year { get; set; }
        public int stock_ID { get; set; }

        public decimal OfficeCharges { get; set; }
        public decimal OfficeChargesTax { get; set; }
        public decimal Total_Amount { get; set; }
    }
}
