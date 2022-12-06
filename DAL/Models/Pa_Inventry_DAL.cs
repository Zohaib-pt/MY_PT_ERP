namespace DAL.Models
{
    public class Pa_Inventry_DAL
    {
        //New Faraz Work


        public int Item_ID { get; set; }
        public int make_ID { get; set; }

        public string ItemCode { get; set; }
        public string Year { get; set; }


        public string ItemName { get; set; }

        public string ItemDescription { get; set; }

        public string Created_at { get; set; }

        public string Created_by { get; set; }


        public string Modified_at { get; set; }

        public string Modified_by { get; set; }
        public string IsSerializable { get; set; }
        public string BarCode { get; set; }


        public string UnitPrice { get; set; }
        public string OpeningBal { get; set; }

        public string SalePrice { get; set; }


        public string CostMethod { get; set; }


        public string Comment { get; set; }

        public string Purchase_Qty { get; set; }
        public string Sold_Qty { get; set; }
        public string InHand_Qty { get; set; }
        public int UOM_ID { get; set; }
        public string UOM { get; set; }
        public int Group_ID { get; set; }
        public int Category_ID { get; set; }
        public string GroupName { get; set; }
        public string CategoryName { get; set; }
        public string Traditional { get; set; }
        public string FUEL_TYPE { get; set; }
        public string Transmission { get; set; }
        public string Drive { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }

        public int enginespecscode_ID { get; set; }

        public string EngineSpecsCode { get; set; }
 

        public string SpecsDescription { get; set; }
        public string Received_Qty { get; set; }
        public string Delivered_Qty { get; set; }
        public int location_ID { get; set; }
        public string Return_Qty { get; set; }


    }
}
