namespace DAL.Models
{
    public class JP_PurchaseDetails_DAL
    {
        public int ID { get; set; }
        public int PurchaseDetails_ID { get; set; }
        public int PurchaseMaster_ID { get; set; }
        public string ItemId { get; set; }
        public string ItemDesc { get; set; }
        public string UM_ID { get; set; }
        public int stock_ID { get; set; }
        public decimal? Unit_Price { get; set; }
        public int? MajorQty { get; set; }
        public int? MinorQty { get; set; }
        public int transaction_status { get; set; }
        public int isDeleted { get; set; }
        public string Chassis_No { get; set; }
        public decimal PriceOrignal { get; set; }
        public decimal PriceRate { get; set; }
        public decimal PriceTax { get; set; }
        public decimal RecycleFee { get; set; }
        public decimal RecycleFeeTax { get; set; }
        public decimal AuctionFee { get; set; }
        public decimal AuctionFeeTax { get; set; }
        public decimal PlatNumberFee { get; set; }
        public decimal PlatNaumberTax { get; set; }
        public string make { get; set; }
        public string color { get; set; }
        public string model_description { get; set; }
        public int locID { get; set; }
        public string temp_ID { get; set; }
        public string CAR_LOCATION { get; set; }
        public decimal Total { get; set; }
        public string vendor_name { get; set; }
        public int Vendor_ID { get; set; }
        public string PurchaseRef { get; set; }
        public string PurchaseDate { get; set; }
        public string PaymentDueDate { get; set; }
        public string OtherRef { get; set; }
        public string PurchaseStatus { get; set; }
        public decimal Sub_Total { get; set; }
        public decimal VATrate { get; set; }
        public decimal VATexp { get; set; }
        public decimal Total_Amount { get; set; }
        public string LotNumber { get; set; }
        public int Make_ID { get; set; }
        public int Color_ID { get; set; }
        public int MakeModelDescription_ID { get; set; }
        public string SupplierContact { get; set; }
        public string SupplierAddress { get; set; }

        public string Balance { get; set; }
        public string Paid { get; set; }

        public string ModelYear { get; set; }
        public int Year { get; set; }
        public int GoingToLocation { get; set; }
        public decimal ReksoFee { get; set; }
        public decimal ReksoFeeTax { get; set; }
        public int StockType { get; set; }

        public int Risko_Vendor_ID { get; set; }

        public string transmission { get; set; }

    }
    public class PurchaseDetailGrandTotals
    {
        public string PurchaseRef { get; set; }

        public int PurchaseMaster_ID { get; set; }
        public string TotalPriceOrignal { get; set; }
        public string TotalPriceTax { get; set; }
        public string TotalAuctionFee { get; set; }
        public string TotalAuctionFeeTax { get; set; }
        public string TotalPlatNumberFee { get; set; }
        public string TotalRecycleFee { get; set; }
        public string TotalReksoFee { get; set; }

        public string TotalReksoFeeTax { get; set; }
        public string TotalAuctionPayable { get; set; }
        public string TotalAuctionPayableWithTax { get; set; }
        public string TotalReksoPayable { get; set; }

        public string TotalReksoPayableWithTax { get; set; }

        public string TotalTaxOnly { get; set; }

        public string GrandTotalPayable { get; set; }
        public string GrandTotalPayableWithTax { get; set; }

        public string Paid_Auction { get; set; }

        public string Paid_Rikso { get; set; }

        public string Bal_Due { get; set; }
        public string PlatNumberFee_Refund { get; set; }




    }
}
