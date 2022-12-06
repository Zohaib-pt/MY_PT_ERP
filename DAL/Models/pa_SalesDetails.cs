namespace DAL.Models
{
    public class pa_SalesDetails
    {


        public int? SalesDetails_ID { get; set; }
        public int? SaleMaster_ID { get; set; }
        public string SaleRef { get; set; }
        public string Stock_ID { get; set; }
        public string Chassis_No { get; set; }


        public string Amount { get; set; }
        public string VATExp { get; set; }
        public string VATRate { get; set; }
        public string Discount { get; set; }

        public string Total_Discount { get; set; }
        public string Total_Amount { get; set; }
        public string Total_Amt_inWords { get; set; }
        public string ModelYear { get; set; }


        public string Other_charges { get; set; }
        public string transaction_Status { get; set; }
        public string Temp_ID { get; set; }
        public string c_id { get; set; }
        public string Location_ID { get; set; }
        public string BarCode { get; set; }
        public string Serial { get; set; }
        public int Item_ID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public string Description_Veh { get; set; }
        public string Quantity { get; set; }
        public string Unit_Price { get; set; }
        public string UM_ID { get; set; }
        public string SaleInvoiceType { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }
        public string IsDeleted { get; set; }


        //---My own added fields that not exists in actual table in database
        public string Make { get; set; }
        public string ModelDesciption { get; set; }
        public string Color { get; set; }
        public string TotalUnitPriceByChassis { get; set; }
        public string TotalUnitPriceByService { get; set; }

        public string Grand_UnitPrice { get; set; }
        public string Grand_Amount { get; set; }
        public string Grand_TotalAmount { get; set; }
        public string Grand_VATExp { get; set; }
        public string Engine_No { get; set; }
        public string INVOICENO { get; set; }
        public string INOVOICEDATE { get; set; }
        public string ItemQty { get; set; }
        public string ItemRate { get; set; }
        public string Total { get; set; }
        public string username { get; set; }
        public string usercode { get; set; }
        public string DiscountCode { get; set; }
        public string SaleQty { get; set; }
        public string DOQty { get; set; }
        public string ReturnQty { get; set; }
        public string LocationName { get; set; }

        public string OtherCost { get; set; }
        public string ShippingCharges { get; set; }




        public string Tip { get; set; }
        public int Seller_ID { get; set; }




    }
}
