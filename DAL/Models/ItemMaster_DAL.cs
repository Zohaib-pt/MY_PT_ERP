namespace DAL.Models
{
    public class ItemMaster_DAL
    {


        public int? Item_ID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }


    }

    public class ItemCard_DAL
    {


        public int Item_ID { get; set; }
        public string ItemCode { get; set; }
        public string itemSerialNO { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string SaleDescription { get; set; }

        public string SNo { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string Trans_Date { get; set; }
        public string Trans_Ref { get; set; }

        public string Qty { get; set; }
        public string RecQty { get; set; }

        public string UnitPrice { get; set; }

        public string Pur_Amount { get; set; }



        public string Sold_SerialNo { get; set; }
        public string QtySold { get; set; }

        public string QtyDelvr { get; set; }

        public string Total_Cost { get; set; }

        public string SaleUnitPrice { get; set; }
        public string SaleAmount { get; set; }

        public string Sale_Total_Amt { get; set; }
        public string Pur_Total_Amt { get; set; }
        public string VATEXP_Pur { get; set; }
        public string VAT_Sale { get; set; }

        public string Value { get; set; }
        public string Qty_Bal { get; set; }
        
        public string QTY_BAL { get; set; }
        public string AMOUNT_BAL { get; set; }

        public string Loc_ID { get; set; }
        public string ItemLocationName { get; set; }





    }
}
