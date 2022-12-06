using DAL.Models;
using System.Collections.Generic;
using X.PagedList;

namespace DAL.oClasses
{
    public interface IOPurchases
    {

        //Faraz New WOrk Here

        public IEnumerable<Pa_PurchaseMaster_DAL> purchaseOtherMasterList1 { get; set; }

        //Faraz work
        public IEnumerable<Pa_PurchaseMaster_DAL> Get_PurchaseMasterList_Other_DAL1(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID);


        //---properties
        public Pa_PurchaseDetails_DAL purchaseObject { get; set; }
        public Pa_PurchaseMaster_DAL purchaseMasterObj { get; set; }
        public IEnumerable<Pa_PurchaseDetails_DAL> purchaseDetailList { get; set; }
        public IEnumerable<Pa_PurchaseMaster_DAL> purchaseMasterList { get; set; }
        public IPagedList<Pa_PurchaseMaster_DAL> purchaseMasterIPagedList { get; set; }
        public IPagedList<Pa_PurchaseMaster_DAL> purchaseOtherMasterList { get; set; }
        public IPagedList<Pa_PurchaseMaster_DAL> purchaseReturnMasterList { get; set; }

        public IEnumerable<Pa_Attachments_DAL> attachmentList { get; set; }
        public IEnumerable<importHolder> ImportHolderList { get; set; }

        public IEnumerable<StockReport> StockInPoList { get; set; }
        public IEnumerable<Pa_PurchaseMaster_DAL> purchaseMasterIPagedList1 { get; set; }
        public IEnumerable<Pa_PurchaseMaster_DAL> purchaseMasterTotal { get; set; }
        public IEnumerable<Pa_GoodsReceived_Dtl> GRVDetailList { get; set; }
        public Pa_GoodsReceived_Master GRVMasterObj { get; set; }
        public IPagedList<Pa_GoodsReceived_Master> GRVOtherMasterList { get; set; }
        public IEnumerable<Pa_PurchaseDetails_DAL> PurchaseDetailList { get; set; }

        public IEnumerable<Pa_GoodsReceived_Master> GRVOtherMasterList_Print { get; set; }
        //---methods signature
        public Pa_PurchaseMaster_DAL Get_PurchaseMasterRef_DAL();

        public IEnumerable<Pa_PurchaseMaster_DAL> Get_PurchaseMasterList_TTL(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID,string ChassisNo, int? c_ID);
        public string InsertPurchaseDetail_DAL(int? Make_ID, int? Model_ID, int? Color_ID, string mileage, int Make_category_ID, string Engine_Type, int StockType_ID, string Drive, string Used_New, string Fuel_Type, string BL_NO, string SellingPrice, string OtherCost, string OthersSpecs, double? Quantity, int? Currency_ID,
                double? Currency_Rate, string Unit_Price, string Amount, string Amount_Others, string Ref, string Temp_ID, int? Create_by, int PurchaseMaster_ID, string Chassis, string ModelYear, int? color_interior_ID, int? loc, int? c_ID, string PriceTax, string AuctionFee, string ReksoFee, string RecycleFee, string Vanning_Charges, string FreightCharges, string FreightRate, string Cont_NO, string AuctionName, string Other_Charges, string JP_Charges,string Door,string Hs_Code);

        public string UpdatePurchaseDetail_DAL(int? PurchaseDetails_ID, int? Make_ID, int? Model_ID, int? Color_ID, string mileage, int Make_category_ID, string Engine_Type, int StockType_ID, string Drive, string Used_New, string Fuel_Type, string BL_NO, string SellingPrice, string OtherCost, string OthersSpecs, int? Quantity, int? Currency_ID,
               double? Currency_Rate, string Unit_Price, string Amount, string Amount_Others, string Ref, int? Modified_by, string Chassis, string ModelYear, int? color_interior_ID, int? loc, int? c_ID, int? StockID, string PriceTax, string AuctionFee, string ReksoFee, string RecycleFee, string Vanning_Charges, string FreightCharges, string FreightRate, string Cont_NO, string AuctionName, string Other_Charges, string JP_Charges, string Door, string Hs_Code);




        public IEnumerable<Pa_PurchaseDetails_DAL> Get_PurchaseDetailListByID_DAL(string Temp_ID, int? PurchaseMaster_ID);

        public string InsertPurchaseMaster_DAL(int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string PurchaseType, string PurchaseDate, int? VAT_Rate, double? Discount, string Temp_ID, int? c_ID, int? Created_by);
        public IEnumerable<Pa_PurchaseMaster_DAL> Get_PurchaseMasterList_DAL(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID,string ChassisNo, int? c_ID);

        public Pa_PurchaseMaster_DAL Get_PurchaseMasterByID_DAL(int? PurchaseMaster_ID);
        public string DeletePurchaseDetail_DAL(int? PurchaseDetails_ID);
        public Pa_PurchaseDetails_DAL GetPuchaseDetailByID_DAL(int? PurchaseDetails_ID);
        public string UpdatePurchaseMaster_DAL(int? PurchaseMaster_ID, int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string PurchaseDate, int? VAT_Rate, double? Discount, string Temp_ID, int? Modified_by);
        public string GetOldTempIDFromPurchaseDetail_DAL(int? PurchaseMaster_ID);
        public Pa_PurchaseMaster_DAL Get_PurchaseMaster_OtherByID_DAL(int? PurchaseMaster_ID);

        public string InsertPurchaseDetail_Other_DAL(int? item_ID, string item_Description, double? Quantity, string UOM, int? Currency_ID, double? Currency_Rate, double? Unit_Price, int? VAT_Rate,
           double? VAT_Exp, double? Discount, double? Amount, double? TtlAmount, double? Amount_Others, string Temp_ID, int? c_ID, int? Created_by, int PurchaseMaster_ID, int Location_ID, string Serial, string Barcode);
        public string DeletePurchaseMaster_DAL(int? PurchaseMaster_ID, int USER_ID);
        public string DeletePurchaseMasterOther_DAL(int? PurchaseMaster_ID, int USER_ID);
        public string ChangePurchaseMasterStatus_DAL(int? PurchaseMaster_ID, int? Status_ID, int? c_ID, string Trans_Type, int? Modified_by);
        public IEnumerable<Pa_Attachments_DAL> GetPurchaseMasterNew_Attachments_DAL(int? PurchaseMaster_ID, string Type);
        public string Insert_PurchaseMasterAttachment_DAL(int? PurchaseMaster_ID, string Document_Type, string Type, string newFileName, string filepath, int? Created_by);

        public IEnumerable<Pa_PurchaseDetails_DAL> Get_PurchaseDetailOtherListByID_DAL(string Temp_ID, int? PurchaseMaster_ID);
        public string UpdatePurchaseDetail_Other_DAL(int? PurchaseDetails_ID, int? item_ID, string item_Description, double? Quantity, string UOM, int? Currency_ID, double? Currency_Rate, double? Unit_Price, int? VAT_Rate,
           double? VAT_Exp, double? Discount, double? Amount, double? TtlAmount, double? Amount_Others, string Temp_ID, int? c_ID, int? Modified_by, int Location_ID, string Serial, string Barcode);


        public string UpdateOtherPurchaseMaster_DAL(int? PurchaseMaster_ID, int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string PurchaseDate, string Temp_ID, int? c_ID, int? Modified_by);
        public string Insert_OtherPurchaseMaster_DAL(int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string PurchaseType, string PurchaseDate, string Temp_ID, int? c_ID, int? Created_by);
        public IEnumerable<Pa_PurchaseMaster_DAL> Get_PurchaseMasterList_Other_DAL(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID);

        public string CheckIfRefExistInPurchaseMaster_DAL(string PurchaseRef);
        public string Insert_PurhaseReturnDetail_DAL(string Temp_ID, string PurchaseRef);
        public Pa_PurchaseMaster_DAL Get_PurchaseMaster_Return_ByID_DAL(int? PurchaseMaster_ID);
        public IEnumerable<Pa_PurchaseDetails_DAL> Get_PurchaseDetailReturnListByID_DAL(string Temp_ID, int? PurchaseMaster_ID);
        public string Insert_PurchaseMaster_Return_DAL(int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string PurchaseType, string PurchaseDate, string Temp_ID, int? c_ID, int? Created_by);

        public string Update_Return_PurchaseMaster_DAL(int? PurchaseMaster_ID, int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string PurchaseDate, string Temp_ID, int? c_ID, int? Modified_by);

        public IEnumerable<Pa_PurchaseMaster_DAL> Get_PurchaseMasterReturnList_DAL(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID);

        public string Delete_Attachment_PurchaseNew(int? Attachment_ID);

        public string Clear_ImportHolder();

        public string InsertImportHolder_DAL(string MAKENAME, string MAKEMODELNAME, string CHASSIS_NO, string MODEL,
            string COLORNAME, string COLOR_INT, string PRICE, string PRICE_RATE, string FREIGHT, string FR_RATE,
string SELLING_PRICE, string TRANSMISSION, string DOOR, string DRIVE, string ENGINE_NO, string WEIGHT, string HS_CODE,
string ENGINE_POWER, string MILEAGE, string VEHICLE_CC, string USED_NEW, string OPTIONS, string FUEL_TYPE, string MAKECATEGORYNAME, string VENDORNAME,
string PURCHASEDATE, string PURCHASE_REF, string BL_NO, string SHIP_NAME, string SHIPDATE, string LEAVE_DATE,
string ENTRY_DATE, string BOE, string LOCATION, string MAKECOUNTRYNAME, string AVAILABILITY,
string STOCK_TYPE, string SHOWROOM, string COMMENTS, string OPTION_CODE, string KEYNO, string PRODUCTION_DATE,
string EXP_TRANSPORT, string EXP_AGENT_COMMISSION, string EXP_CUSTOM_DUTY, string EXP_OTHERS, string EXP_CC,
string EXP_MAINT, string EXP_COMM_OTHERS, string EXP_PAPER, string OTHER_REF, string TAX10, string AUCTIONFEE, string REKSO, string RECYCLE, string LOADING,
string AUCTIONNAME, string CONT_NO, string OTHERS_JP, string JP_CHARGES, int C_ID, int USER_ID);



        public IEnumerable<importHolder> Get_StockPuchaseBulkList_DAL();
        public string InsertImportFailure_DAL(string CHASSIS_NO, string Message);
        public string InsertBulkFromImportHolder_DAL(string Temp_ID, int User_ID,int c_ID);

        public IEnumerable<StockReport> get_Stock_in_PO_byPurchaseRef_DAL(string PurchaseRef);


        //    public IEnumerable<Pa_PurchaseMaster_DAL> Get_PurchaseMasterList_DAL1(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID);

        #region GoodsReceived

        public Pa_GoodsReceived_Master Get_ReceivedMaster_ByID_DAL(int? GRVMaster_ID);
        public IEnumerable<Pa_GoodsReceived_Dtl> Get_ReceivedDetailListByID_DAL(string Temp_ID, int? GRVMaster_ID);
        public string InsertGoodsReceivedDetail_DAL(int? item_ID, string item_Description, double? Quantity, string UOM, int? Currency_ID, double? Currency_Rate, double? Unit_Price, int? VAT_Rate,
          double? VAT_Exp, double? Discount, double? Amount, double? TtlAmount, double? Amount_Others, string Temp_ID, int? c_ID, int? Created_by, int GRVMaster_ID, int Location_ID, int PurchaseDetails_ID, int PurchaseMaster_Id);





        public string UpdateGoodsReceivedDetail_DAL(int? GRVDetails_ID, int? item_ID, string item_Description, double? Quantity, string UOM, int? Currency_ID, double? Currency_Rate, double? Unit_Price, int? VAT_Rate,
           double? VAT_Exp, double? Discount, double? Amount, double? TtlAmount, double? Amount_Others, string Temp_ID, int? c_ID, int? Modified_by, int Location_ID, string Serial, string Barcode);
        public string InsertGoodsReceivedMaster_DAL(int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string GRVType, string GRVDate, string Temp_ID, int? c_ID, int? Created_by, int PurchaseMaster_ID);

        public string UpdateGoodsReceivedMaster_DAL(int? GRVMaster_ID, int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string GRVDate, string Temp_ID, int? c_ID, int? Modified_by, int PurchaseMaster_ID);


        public string DeleteGoodsReceivedMaster_DAL(int? GRVMaster_ID);
        public string DeleteGoodsReceived_Material_In_DAL(int? GRVDetails_ID);

        public IEnumerable<Pa_GoodsReceived_Master> Get_GoodsReceivedMaster_DAL(string GRVRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID);




        public string GetOldTempIDFromGoodsReceivedDetail_DAL(int? GRVMaster_ID);

        public IEnumerable<Pa_PurchaseDetails_DAL> GetGRVRefDetails_Other_DAL();
        public IEnumerable<Pa_PurchaseDetails_DAL> Get_PurchaseRefDetails_DAL(int PurchaseMaster_ID);
        public Pa_GoodsReceived_Master Get_PurchaseMaster_GRV_DAL(int? GRVMaster_ID);

        #endregion
    }
}
