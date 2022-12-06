namespace DAL.Models
{
    public class StockDetails
    {
        public int stock_ID { get; set; }
        public int Auction_ID { get; set; }
        public string stock_ref { get; set; }
        public string Stock_Status { get; set; }
        public string Chassis_No { get; set; }
        public string Make { get; set; }
        public int Make_ID { get; set; }
        public string Color { get; set; }
        public string ModelDesciption { get; set; }
        public string PurchaseRef { get; set; }
        public string Transmission { get; set; }
        public string Fuel_Type { get; set; }
        //public string Engine_Power { get; set; }
        public string Engine_Type { get; set; }
        public string Engine_No { get; set; }
        public string Door { get; set; }
        public string ModelYear { get; set; }
        public string Drive { get; set; }
        public string Used_New { get; set; }
        public string CarLocation { get; set; }
        public int Loc_ID { get; set; }
        public string ShowRoomName { get; set; }
        public int Showroom_ID { get; set; }

        public int StockType_ID { get; set; }
        public string StockTypeName { get; set; }
        public string Color_Int { get; set; }
        public int Color_Exterior_ID { get; set; }
        public int Color_Interior_ID { get; set; }
        public int MakeModel_description_ID { get; set; }
        public int Vendor_ID { get; set; }
        public string Vendor_Name { get; set; }
        public string PurchaseDate { get; set; }
        public string StatusCancel_Date { get; set; }

        public string Arrival_Date { get; set; }
        public string ShipName { get; set; }
        public string Leave_Date { get; set; }

        public string ManufacturingDate { get; set; }
        public string BL_NO { get; set; }
        public string ContNO { get; set; }
        public string Comments { get; set; }

        public string PriceOrignal { get; set; }
        public string AuctionName { get; set; }
        public string PriceRate { get; set; }
        public string PriceTax { get; set; }
        public string Shipping_Charges { get; set; }
        public string Vanning_Charges { get; set; }
        public string Inspection_Charges { get; set; }
        public string AuctionFee { get; set; }


        public string AuctionFeeTax { get; set; }
        public string PlatNumberFee { get; set; }
        public string ReksoFee { get; set; }

        public string ReksoFeeTax { get; set; }
        public string FreightCharges { get; set; }

        public string LoadingCharges { get; set; }

        public string JP_Charges { get; set; }

        


        public string FreightRate { get; set; }
        public string TotalPrice { get; set; }
        public string CC_Exp { get; set; }

       

       public string Exp_Custom_Duty { get; set; }


        public string ME_Exp { get; set; }
        public string Paper_Exp { get; set; }
        public string Transport_Exp { get; set; }
        public string Comm_Exp { get; set; }
        public string AgentComm_Exp { get; set; }
        public string OtherCharges_Exp { get; set; }
        public string OtherCharges { get; set; }
        public string VAT_Exp { get; set; }
        public string Total_Expense { get; set; }
        public string Total_Cost { get; set; }

       

        public string Created_at { get; set; }
        public string Modified_at { get; set; }
        public int Created_by { get; set; }
        public string CreatedBy_UserName { get; set; }

        public int Modified_by { get; set; }
        public string Modified_by_UserName { get; set; }

        public bool CarTaken { get; set; }

        public string CarTakenDate { get; set; }
        public string CarTakenPerson { get; set; }
        public string CarTakenContact { get; set; }

        public bool DilveryOrder_Issued { get; set; }
        public string DeliverOrder_Date { get; set; }

        public int Make_category_ID { get; set; }

        public string Weight { get; set; }

        public int Make_country_ID { get; set; }

        public string HS_Code { get; set; }

        public string Selling_Price { get; set; }

        public string Options { get; set; }

        public string StockStatus_UNSOLD { get; set; }
        public string StockStatus_SOLD { get; set; }
        public string StockStatus_BOOKING { get; set; }
        public string StockStatus_CXX { get; set; }
        public string StockStatus_TOTALSTOCK { get; set; }
        public int? Status_ID { get; set; }
        public string Total_All_Cost { get; set; }

        public string Master_BOE { get; set; }


        public string LotNumber { get; set; }
        public string CountAttach { get; set; }
        public string SalePrice { get; set; }



        public string RecycleFee { get; set; }
        public string TotalJp { get; set; }
        public string ReksoName { get; set; }
        public string FromLoc { get; set; }
        public string GoingLoc { get; set; }

        public bool isOptionalCost { get; set; }


        public string InteriorColor { get; set; }
        public string Engine_Power { get; set; }

        public string ProuctionDate { get; set; }
        public string mileage { get; set; }


        public string G_weight { get; set; }
        public string AD_Link { get; set; }
        public string PaperReceived { get; set; }
        public string Registration { get; set; }
        public string FirstRegistrationDate { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }

        public string cc { get; set; }
        public string TotalPrice_Other { get; set; }
        public string TotalExpense { get; set; }
        



    }
}
