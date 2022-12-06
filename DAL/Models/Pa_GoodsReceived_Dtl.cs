using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class Pa_GoodsReceived_Dtl
    {

        public int ID { get; set; }
        public int? GRVDetails_ID { get; set; }
        public int? GRVMaster_ID { get; set; }
        public string item_ID { get; set; } //-- use in Other GRV(PO)
        public string ItemCode { get; set; } //-- use in Other GRV(PO)
        public string ItemName { get; set; } //-- use in Other GRV(PO)
        public string item_Description { get; set; }  //-- use in Other Pruchase(PO)
        public string Make_ID { get; set; }

        public string Model_ID { get; set; }
        public string Color_ID { get; set; }

        public string OthersSpecs { get; set; }  //---use only in GRVs(PR)
        public string Quantity { get; set; }
        public string ReceivedQuantity { get; set; }
        public string LocationName { get; set; }
        public string Currency_ID { get; set; }
        public string Currency_Rate { get; set; }
        public string Unit_Price { get; set; }
        public string Amount { get; set; }
        public string Amount_Others { get; set; }
        public string TtlAmount { get; set; }
        public string VAT_Rate { get; set; }
        public string VAT_Exp { get; set; }


        public string Discount { get; set; }

        public int? transaction_status { get; set; }

        public string Temp_ID { get; set; }
        public string GRVRef { get; set; }
        public string Vendor_ID { get; set; }
        public string Serial { get; set; }
        public string Barcode { get; set; }
        public string location_ID { get; set; } //----- use only in Other GRV(PO), and Location is Inventory Location(Don't Show)
        public string UOM { get; set; }  //----- use only in Other GRV(PO)


        public string Ref { get; set; }
        public string Job { get; set; }
        public string Make { get; set; }
        public string ColorName { get; set; }
        public string ModelDesciption { get; set; }
        public string Currency_Name { get; set; }

        public string SubTotal { get; set; }
        public string Vat_ExpTotal { get; set; }
        public string DiscountTotal { get; set; }
        public string Grand_Total { get; set; }


        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }
        public string Chassis_No { get; set; }
        public string Engine_No { get; set; }


        public string ModelYear { get; set; }

        public int Color_Interior_ID { get; set; }
        public int locID { get; set; }

        public int StockID { get; set; }
        public string mileage { get; set; }

        public int Make_category_ID { get; set; }

        public string Engine_Type { get; set; }


        public int StockType_ID { get; set; }





        public string Drive { get; set; }


        public string Used_New { get; set; }


        public string Fuel_Type { get; set; }


        public string BL_NO { get; set; }
        public string ContainerNo { get; set; }
        public string AuctionName { get; set; }

        public string Selling_Price { get; set; }

        public string OtherCost { get; set; }


        public string PriceTax { get; set; }
        public string AuctionFee { get; set; }

        public string LoadingCharges { get; set; }

        public string ReksoFee { get; set; }
        public string RecycleFee { get; set; }
        public string Vanning_Charges { get; set; }
        public string FreightCharges { get; set; }
        public string OtherCharges { get; set; }

        public string JP_Charges { get; set; }


        public string FreightRate { get; set; }
        public string PurchaseQTY { get; set; }
    }
}
