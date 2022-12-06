namespace DAL.Models
{
    public class pa_tblLedgers
    {

        public string Date { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public string ENTRY_TYPE { get; set; }
        public int ID { get; set; }
        public string Description { get; set; }
        public string trans_ref { get; set; }
        public string ACCOUNT { get; set; }
        public string trans_ref_ID { get; set; }
        public string vendor_ID { get; set; }
        public string trans_created_at { get; set; }
        public string Chassis_No { get; set; }
        public string Alt_ACCOUNT { get; set; }
        public string PartyName { get; set; }

        public string Link { get; set; }
        public string Party_ID { get; set; }

        public string Balance { get; set; }

        public string customer_ID { get; set; }
        public string TotalDebit { get; set; }
        public string TotalCredit { get; set; }
        public string TotalBalance { get; set; }
        public string Opening_Balance { get; set; }
        public string Closing_Balance { get; set; }



        public string Order_Ref { get; set; }

        public int SaleMaster_ID { get; set; }


    }

    public class SaleReport
    {
        public string Date { get; set; }

        public string SaleRef { get; set; }
        public string Chassis_no { get; set; }
        public string make { get; set; }

        public string model_description { get; set; }
        public string color { get; set; }

        public string Sale_Value { get; set; }
        public string Total_Expense { get; set; }

        public string VATExp { get; set; }
        public string Total_Amt { get; set; }

        public string Total_Sold_Cost { get; set; }
      
        public string Total_Cost { get; set; }

        public string ProfitLost { get; set; }
        public string Customer_Name { get; set; }
        public string Remarks { get; set; }

        public int SaleMaster_ID { get; set; }
        public string Manualbook_ref { get; set; }

        public string COD { get; set; }

        public string Tip { get; set; }

        public string ShipingFee { get; set; }
        public string SellerName { get; set; }
        public int Customer_ID { get; set; }
        public int Stock_id { get; set; }


        







    }

    public class StockReport
    {




        public string Stock_ID { get; set; }
        public string Chassis_No { get; set; }
        public string PurchaseDate { get; set; }
        public string PurchaseRef { get; set; }
        public string Make { get; set; }
        public string ModelDesciption { get; set; }
        public string Color { get; set; }
        public string StockType { get; set; }
        public string Transmission { get; set; }
        public string Total_Cost { get; set; }


        public string TotalPrice { get; set; }
        public string Vendor_Name { get; set; }
        public string ModelYear { get; set; }
        public string CarLocation { get; set; }



        public string Total_Expense { get; set; }
        public string Stock_Status { get; set; }

        public string Paid { get; set; }
        public string Balance { get; set; }
        public string Price { get; set; }
        public string PriceTax { get; set; }
        public string Auction { get; set; }
        public string Rekso { get; set; }
        public string Recycle { get; set; }
        public string Loading { get; set; }

        public string OtherCharges_JP { get; set; }
        public string JP_Charges { get; set; }

        public string Freight { get; set; }
        public string PriceRate { get; set; }
        public string Total_Cost_Others { get; set; }




    }
}











