using DAL.Models;
using System;
using System.Collections.Generic;
using X.PagedList;


namespace DAL.oClasses
{
    public interface IOStock
    {
        public StockDetails StockObject { get; set; }
        public IEnumerable<StockDetails> StockListObject { get; set; }

        public StockDetails StockListStats { get; set; }

        public IPagedList<StockDetails> StockListPagedObject { get; set; }
        public IEnumerable<StockDetails> StockList_TTL { get; set; }

        //-- by Rafay Start Date 16/jan/2021 

        public IEnumerable<pa_Select_PaperList_Result> ShakeenList { get; set; }
        public List<pa_Select_PaperList_Result> get_Shaken_List(string chassis, string StartDate, string EndDate);


        //-- by Rafay Start Date 16/jan/2021 



        //by Rafay - start / 11-jan-2021 "Purchase Master Footer"
        public PurchaseDetailGrandTotals PurchaseDetailGrandTotal { get; set; }

        public Pa_PurchaseMaster_DAL PurchaseMasters { get; set; }

        public List<Pa_PurchaseMaster_DAL> PurchaseMaster(int ID);

        public List<PurchaseDetailGrandTotals> Get_purchaseDetailsTotal(int PurchaseMaster_ID);


        //Faraz New work

        //vanning list
        public IEnumerable<pa_Vanning_Master> VanningListPagedObject1 { get; set; }


        //shippinginfolist
        public IEnumerable<pa_Shipping_Info> Shipping_infoListPagedObject1 { get; set; }

        // PurchaseListJP
        public IEnumerable<Pa_PurchaseMaster_DAL> PurchaseMasterList1 { get; set; }

        //Reksolist
        public IEnumerable<pa_select_StockParties_Result> ReksoList1 { get; set; }

        //paperlist for report

        public IEnumerable<pa_Select_PaperList_Result> PapersListObject_print { get; set; }


        public IEnumerable<Pa_Attachments_DAL> attachmentList { get; set; }
        //paperlist for search

        public IPagedList<pa_Select_PaperList_Result> PapersListObject_search { get; set; }

        //


        //by Rafay - end / 11-jan-2021

        //by Rafay - start 11/jan/2021 "Rekso List"

        public IPagedList<pa_select_StockParties_Result> ReksoList { get; set; }

        public Pa_Auction_Master AuctionMasterObj { get; set; }






        

        public IEnumerable<Pa_Auction_Detsils> AuctionDetailList { get; set; }


        public IEnumerable<pa_select_StockParties_Result> pa_select_StockParties(string purchaseRef, string startDate, string endDate, int RekSo_Vendor_ID);

        //by Rafay - end 11/jan/2021


        public StockDetails Get_Select_Stock_by_ID_DAL(int? Stock_ID);

        public IEnumerable<Pa_PaymentDetails_DAL> objStock_Exp_Details { get; set; }
        public pa_Vanning_Master VanningMasterObj { get; set; }
        public IEnumerable<pa_Vanning_Details> VanningDetailList { get; set; }
        public pa_Vanning_Details VanningObject { get; set; }
        public IPagedList<pa_Vanning_Master> VanningListPagedObject { get; set; }
        public pa_Shipping_Info Shipping_infoMasterObj { get; set; }
        public pa_Shipping_Info_details Shipping_infoObject { get; set; }
        public IEnumerable<pa_Shipping_Info_details> Shipping_infoDetailList { get; set; }
        public IPagedList<pa_Shipping_Info> Shipping_infoListPagedObject { get; set; }
        public Papers PapersMasterObj { get; set; }
        public IEnumerable<pa_Select_PaperList_Result> PapersListObject { get; set; }
        public JP_PurchaseDetails_DAL PurchaseDetailObj { get; set; }
        public IEnumerable<JP_PurchaseDetails_DAL> PurchaseDetailList { get; set; }
        public Pa_PurchaseMaster_DAL PurchaseMasterObj { get; set; }
        public IPagedList<Pa_PurchaseMaster_DAL> PurchaseMasterList { get; set; }
        public Pa_PurchaseMaster_DAL PurchaseMasterRef { get; set; }
        public IEnumerable<StockDetails> StockListGetObject { get; set; }
        public StockDetails StockListStats1 { get; set; }
        public IEnumerable<StockDetails> StockList_TTL1 { get; set; }
        //purchaselistjp total
        public IEnumerable<Pa_PurchaseMaster_DAL> purchaseMasterTotal { get; set; }


        //vanninglist total


        public IEnumerable<pa_Vanning_Master> VanningListTotal { get; set; }



        //shipping_infoList
        public IEnumerable<pa_Shipping_Info> Shipping_infoListTotal { get; set; }


        //reksolist

        public IEnumerable<pa_select_StockParties_Result> ReksoList_total { get; set; }



        //New faraz work purchasejp


        //New Faraz Work

        //purchaselistjp
        public IEnumerable<Pa_PurchaseMaster_DAL> Get_PurchaseMasterList_Total(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, string ChassisNo, int? c_ID);


        //vanninglist

        public List<pa_Vanning_Master> get_Vanning_List_Total(string trans_ref, string StartDate, string EndDate, string Shipper_Name);

        //shippinginfolist


        public List<pa_Shipping_Info> get_shipping_info_List_Total(string trans_ref, string StartDate, string EndDate, string Shipper_Name);


        //reksolist

        public IEnumerable<pa_select_StockParties_Result> pa_select_StockParties_total(string purchaseRef, string startDate, string endDate, int RekSo_Vendor_ID);



        public string Insert_Stock_DAL(string CHASSIS_NO, int loc_ID, int Showroom_ID, string vehicle_CC,
        int StockType, int User_ID, string BL_NO, string ShipName, string Arrival_Date, string Leave_Date, int Vendor_ID, string PurchaseDate,
         string PurchaseRef, string Comments, string ModelYear, int color_exterior_ID, string Transmission, string Door, int make_ID,
        int MakeModel_description_ID, int Make_category_ID, string OPTIONS, string HS_CODE, string DRIVE, string FUEL_TYPE, string EngineType, string USED_NEW,
        int make_country_ID, string Engine_No, string GCC_Specs, string mileage, string WEIGHT, string seat, int Color_Interior_ID,
        string intGrade, string intRemarks, string extGrade, string extRemarks, string Option_Code, string KeyNo,
        string PriceOrignal, string PriceRate,
        string FreightCharges, string FreightRate, string AuctionFee, string PlatNumberFee, string ReksoFee, string RecycleFee,
        string CC_Exp,string Exp_Custom_Duty,string ME_Exp, string Paper_Exp, string Transport_Exp,
        string Comm_Exp, string AgentComm_Exp, string OtherCharges_Exp, string VAT_Exp,
        string ManufacturingDate, string Selling_Price, string Master_BOE, int? c_ID, string Vanning_Charges, string OtherCharges, string JP_Charges, string PriceTax);


        public string Update_StockStatus_DAL(int Stock_ID, int Status_ID, int User_ID, int c_ID);
        public string Update_Stock_DAL(string CHASSIS_NO, int loc_ID, int Showroom_ID, string vehicle_CC,
           int StockType, int User_ID, string BL_NO, string ShipName, string Arrival_Date, string Leave_Date, int Vendor_ID, string PurchaseDate,
           string PurchaseRef, string Comments, string ModelYear, int color_exterior_ID, string Transmission, string Door, int make_ID,
           int MakeModel_description_ID, int Make_category_ID, string OPTIONS, string HS_CODE, string DRIVE, string FUEL_TYPE, string EngineType, string USED_NEW,
           int make_country_ID, string Engine_No, string GCC_Specs, string mileage, string WEIGHT, string seat, int Color_Interior_ID,
           string intGrade, string intRemarks, string extGrade, string extRemarks, string Option_Code, string KeyNo,
           string PriceOrignal, string PriceRate,
           string FreightCharges, string FreightRate, string AuctionFee, string PlatNumberFee, string ReksoFee, string RecycleFee,
           string CC_Exp, string Exp_Custom_Duty, string ME_Exp, string Paper_Exp, string Transport_Exp,
           string Comm_Exp, string AgentComm_Exp, string OtherCharges_Exp,
           string VAT_Exp, string ManufacturingDate, string Selling_Price, string Master_BOE, int? Stock_ID, string AD_Link, string Vanning_Charges, string OtherCharges, string JP_Charges, string PriceTax, string LotNumber);

        public IEnumerable<StockDetails> Get_StockList_DAL(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
               string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID);

        public IEnumerable<StockDetails> Get_StockList_TTL_DAL(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
        string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID);

        public string DeleteStockMaster_DAL(int? Stock_ID, int? User_ID);
        public IEnumerable<Pa_PaymentDetails_DAL> get_veh_expense_by_Stock_ID(int? Stock_ID);

        public StockDetails Get_StockList_stats_DAL(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
        string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID);

        public IEnumerable<StockDetails> get_Stock_Status_DAL();

        public List<pa_Vanning_Master> get_Vanning_List(string trans_ref, string StartDate, string EndDate, string Shipper_Name);

        public string Insert_Vanning_Details_DAL(string Vanning_Master_ID, string Chassis_No, string Amount, string Tax_Amount, string InspectionCharges, string InsepectionChargesTax, string TotalAmount,
            string htemp_ID, int user_ID);


        public string Update_Vanning_Details_DAL(string Chassis_No, string Amount, string Tax_Amount, string InspectionCharges, string InsepectionChargesTax, string TotalAmount, int user_ID, int ID, string Temp_ID);


        public string Insert_Vanning_Master_DAL(string Vendor_ID, string Date, string trans_ref, string Comments, string Amount, string Tax_Amount,
            string temp_ID, int user_ID, int Sale_ID_For_Inserting);
        public string Update_Vanning_Master_DAL(string Vendor_ID, string Date, string trans_ref, string Comments, string Amount, string Tax_Amount,
            string temp_ID, int user_ID, string vanning_Master_ID);


        public pa_Vanning_Master get_Vanning_Master_by_tempID_ID(string temp_ID = "", string ID = "0");

        public List<pa_Vanning_Details> get_Vanning_Details_by_TempID_ID(string temp_ID = "00", string ID = "0");


        //method for getting Sale_Ref from "pa_Sale" table for dropdown list in shippin info page
        public IEnumerable<pa_SalesMaster_DAL> Get_pa_Sale_SaleRef_DAL();



        public pa_Vanning_Details GetVanningDetailByID_DAL(int? ID);


        //Mehtod for getting record on the basis of Dropdown "Sale_refDrop" in shippinginfo.cshtml page
        public List<pa_Vanning_Details> get_pa_Vanning_details_by_Sale_ID_new(string Type, int? Sale_ID, string temp_ID);
        public string GetOldTempIDFromVanningDetail_DAL(int? VanningMaster_ID);
        // public string Cancel_Vanning_DAL(int? ID, string UserId);
        public string Cancel_Vanning_DAL(int? Vanning_Master_ID, int Status_ID, int C_id, int Modified_by);
        public string Delete_Vanning_DAL(int? ID);
        public string DeleteVanningDetail_DAL(int? VanningDetails_ID);



        #region Shipping_Info_DAL

        public List<pa_Shipping_Info> get_shipping_info_List(string trans_ref, string StartDate, string EndDate, string Shipper_Name);

        public string Insert_Shipping_Info_details_DAL(string Chassis_No, string htemp_ID, string Shipping_info_ID, string Berth_Carry_Charges,
           string Berth_Carry_ChargesTax, int user_ID);


        public string Update_Shipping_Info_details_DAL(string Chassis_No, string Berth_Carry_Charges, string Berth_Carry_ChargesTax, int user_ID, int ID);

        public string Insert_Shipping_Info_DAL(string Invoice_ref, string Booking_ref, string Shipper_ID, string Vessel_Name, string Vessel_Name_VoyNo,
            int Port_of_Loading_ID, string ETD, int Port_of_Discharge_ID, int Final_Destination_ID, string ETA, string Notify_Party, string Berth,
            string Berth_in_date, string BL_no, string Inspection_Type, string temp_ID, int user_ID, int Sale_ID_For_Inserting, string ContainerType, string Container_Count);


        public string Update_Shipping_Info_DAL(string Invoice_ref, string Invoice_Date, string Booking_ref, string Shipper_ID, string Vessel_Name, string Vessel_Name_VoyNo,
            int Port_of_Loading_ID, string ETD, int Port_of_Discharge_ID, int Final_Destination_ID, string ETA, string Notify_Party, string Berth,
            string Berth_in_date, string BL_no, string Inspection_Type, string temp_ID, int user_ID, int shipinfoId, string ContainerType, string Container_Count);


        public pa_Shipping_Info get_shipping_info_by_tempID_ID(string temp_ID = "", string ID = "0");

        public List<pa_Shipping_Info_details> get_shipping_info_details_by_tempID_ID(string temp_ID = "", string ID = "0");


        //method for getting Sale_Ref from "pa_Sale" table for dropdown list in shippin info page
        public IEnumerable<pa_SalesMaster_DAL> Get_pa_Sale_SaleRef_DAL_Shipping();






        //Mehtod for getting record on the basis of Dropdown "Sale_refDrop" in shippinginfo.cshtml page

        public List<pa_Shipping_Info_details> get_shipping_info_details_by_tempID_ID_new(string Type, int? Sale_ID);
        public List<pa_Shipping_Info_details> pa_Select_Shipping_Info_details_by_TempID_ID(string Type, int? Sale_ID, string temp_ID);
        public string GetOldTempIDFromShipping_infDetail_DAL(int? Shipping_info_ID);
        public pa_Shipping_Info_details GetShipping_InfoDetailByID_DAL(int? ID);
        // public string Cancel_Shipping_Info_DAL(int? ID, string UserId);
        public string Cancel_Shipping_Info_DAL(int? Shipping_info_ID, int Status_ID, int C_ID, int Modified_by);
        public string Delete_Shipping_Info_DAL(int? ID);
        public string DeleteShipping_InfoDetail_DAL(int? Shipping_InfoDetails_ID);
        public IEnumerable<Pa_Countries_DAL> Get_PortFrom();
        public IEnumerable<Pa_Countries_DAL> Get_PortTo();
        #endregion

        #region PAPERHSC
        public string Insert_Paper(string CHASSIS_No, string RecievedDate, string paperType, string isRecieved, string _ref, DateTime Created_at, int Created_by, string Modified_at, int modified_by, int isDelete,
           string Vehicle_CC, string Engine_Type, string Registration, DateTime RegistrationDate, DateTime ManufactureDate, string EngineType, string lenght, string width, string height, string Weight, string G_weight, string SEAT, string Drive);


        public string Update_Paper(int paperid, string CHASSIS_No, string RecievedDate, string paperType, string isRecieved, string _ref, DateTime Created_at, int Created_by, DateTime Modified_at, int modified_by, int isDelete,
                string Vehicle_CC, string Registration, DateTime RegistrationDate, DateTime ManufactureDate, string EngineType, string lenght, string width, string height, string gweight, string weight, string SEAT, string Drive);


        public List<pa_Select_PaperList_Result> get_Papers_List(string ChassisNo, string StartDate, string EndDate);
        public Papers Paper_Details(int? ID);
        public string Delete_Paper_DAL(string id);
        public IEnumerable<Wordss> Get_Words();
        public IEnumerable<Cities> Get_Cities();
        #endregion

        #region PurchaseJP
        public string InsertPurchaseMasterJPDetail_DAL(string Chassis, int Make, int Model, int Color, int loc, string Ref, string Lot
        , decimal? Price, int PriceRate, decimal PriceTax, decimal RecycleFee,
         decimal auctionfee, decimal auctionfeetax, decimal NumberPlate, decimal TotalPrice,
         string Year, int GoingToLocation, int VendorName, decimal ReksoFee,
         decimal ReksoFeeTax, int StockType, string Transmission, int hdPurchaseMaster_ID, string Temp_ID, int? c_ID, int? Created_by);

        //---Update into sales booking detial general
        public string UpdatePurchaseMasterJPDetail_DAL(int? PurchaseDetailID, string Chassis, int? Make, int? Model, int? Color, int? loc, string Lot,
            decimal? Price, decimal? PriceRate, decimal? PriceTax, decimal? RecycleFee,
            decimal? auctionfee, decimal? auctionfeetax, decimal? NumberPlate, decimal? TotalPrice, string Year,
            int? GoingToLocation, int? VendorName,
            decimal? ReksoFee, decimal? ReksoFeeTax, int? StockType, string Transmission, string Temp_ID, int? c_ID, int? Modified_by);


        //Insert Sales booking Master
        public string InsertPurchaseMasterJPMaster_DAL(int? Vendor, string Ref, string PurchaseRef, string Date, string PaymentDueDate, string SupplierContact, string SupplierAddress, string Purchasetype, string Temp_ID, int? c_ID, int? Created_by);


        //Udpate Purchase booking Master
        public string UpdatePurchaseMasterJPMaster_DAL(int? PurchaseMaster_ID, string PurchaseRef, string Date, int? Vendor, string Ref, string SupplierContact, string SupplierAddress, string Temp_ID, int? c_ID, int? Modified_by);

        //--getting Purchase booking master by id
        public Pa_PurchaseMaster_DAL Get_PurchaseMasterJPMasterByID_DAL(int? PurchaseMaster_ID);


        //Get Purchase booking detail list by id
        public IEnumerable<JP_PurchaseDetails_DAL> Get_PurchaseMasterJPDetailListByID_DAL(string Temp_ID, int? PurchaseMaster_ID);
        public string GetOldTempIDFromPurchaseDetail_DAL(int? PurchaseMaster_ID);
        public string DeletePurchaseMaster_DAL(int? PurchaseMaster_ID, int? User_ID);

        public string Cancel_PurchaseJP(int? PurchaseMaster_ID, int? Status_ID, int? c_ID, string Trans_Type, int? Modified_by);
        public string DeletePurchaseDetail_DAL(int? StockDetails_ID);
        public IEnumerable<Pa_PurchaseMaster_DAL> pa_Select_PurchaseMaster(string PurchaseRef, string From_Date, string To_Date, int Vendor_ID, int Status_ID, string ChassisNo, int c_ID);

        public IEnumerable<Pa_Countries_DAL> GoingLocation();
        public Pa_PurchaseMaster_DAL LoadRef();
        #endregion

        public IEnumerable<StockDetails> Get_StockList_DAL1(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID, string VendorName, string ModelYear);

        public IEnumerable<StockDetails> Get_StockList_TTL_DAL1(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
     string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID);


        public StockDetails Get_StockList_stats_DAL1(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
        string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID);
        public IEnumerable<Pa_Attachments_DAL> GetStockMasterNew_Attachments_DAL(int? StockMaster_ID, string Type);

        public string Insert_StockMasterAttachment_DAL(int? StockMaster_ID, string Document_Type, string Type, string newFileName, string filepath, int? Created_by);
        public string Delete_Attachment_StockNew(int? Attachment_ID);


        public Pa_Auction_Master Get_AuctionMasterByID_DAL(int? Auction_ID);

        public IEnumerable<Pa_Auction_Detsils> Get_AuctionDetailListBy_DAL(string Temp_ID, int? Auction_ID);
        
        public string InsertAuctionDetail_DAL(int? HdAuction_ID, int? Stock_ID
        , string Temp_ID);
       
        public string InsertAuctionMaster_DAL(string Auction_Date, string Ref, string Remarks,
     string Temp_ID, int? c_ID, int? Created_by);

        public string UpdateAuctionMaster_DAL(string Auction_Date, string Ref, string Remarks, int? Auction_ID,
          string Temp_ID, int? c_ID, int? Modified_by);

        public string DeleteAuctionDetail_DAL(int? Detail_ID);
        
        public IEnumerable<Pa_Auction_Master> Get_AuctionMasterInvoiceList_DAL(string Chassis, string StartDate, string EndDate
    , int c_ID);

        public string GetOldTempIDFromAuctionDetail_DAL(int? Auction_ID);
        
        public IEnumerable<StockDetails> StockList { get; set; }
        public IEnumerable<StockDetails> Get_AllStock_DAL();


        public IEnumerable<StockDetails> Get_AuctionList_DAL(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
        string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID, string VendorName, string ModelYear, int Auction_ID);
        public IEnumerable<StockDetails> Get_AuctionList_TTL_DAL(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
         string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID, string VendorName, string ModelYear);


        public StockDetails Get_AuctionList_stats_DAL(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
           string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID, string VendorName, string ModelYear);

        public IEnumerable<Pa_Auction_Master> AuctionMasterList { get; set; }
        public IEnumerable<Pa_Auction_Master> Get_AuctionList_DAL();


        
        public pa_Shipping_Info get_shipping_info_by_ID_Print(int ID);
        public List<pa_Shipping_Info_details> pa_Select_Shipping_Info_details_by_Print_ID(int ID);
        public pa_Vanning_Master get_Vanning_Master_by_ID_Print(int ID);
        public List<pa_Vanning_Details> get_Vanning_Details_by_ID_Print(int ID);
        public Pa_PurchaseMaster_DAL Get_PurchaseMasterJPMasterByID_Print(int? PurchaseMaster_ID);
        public IEnumerable<JP_PurchaseDetails_DAL> Get_PurchaseMasterJPDetailListByID_Print(int? PurchaseMaster_ID);
        public IEnumerable<StockDetails> Get_StockList_Complete_DAL(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
     string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID);
        public string Update_Shipping_Alert_DAL(int shipinfoId);
        public string Insert_Shaken_DAL(string stock_ID, string platNumber_fee, string refundAmount, string cancel_Date, string refund_date, string recieved_Date, string Account_Debit_ID,
string Status, string AdjustedPurchase_ref, string modiefied_by, string created_by);



        // By Yaseen   1-7-2022


        #region  Bill of Lading


       
        public IPagedList<Pa_BL_Master> BLMasterPlist { get; set; }
        public Pa_BL_Master BLMasterObj { get; set; }
        public Pa_BL_Master Get_BLMasterByID_DAL(int? BL_ID);
        public IEnumerable<Pa_BL_Detsils> BLDetailList { get; set; }

        public IEnumerable<Pa_BL_Master> BLMasterList { get; set; }
        public IEnumerable<Pa_BL_Master> Get_BlList_DAL(string BLNO ,string StartDate ,string EndDate, int c_ID);

        public IEnumerable<Pa_BL_Detsils> Get_BLDetailListBy_DAL(string Temp_ID, int? BL_ID);

        public string InsertBLDetail_DAL(int? HdBL_ID, int? Stock_ID
                , string Temp_ID);

        public string InsertBLMaster_DAL(string ShipName, string ShipLeavingDate, string Arrival_Date, string Clearing_Charges, string Custom_Duty, string Other_Charage, string BLNO_Date, string Remarks,string Total_BL_Charges,
      string Temp_ID, int? c_ID, int? Created_by ,int Currency_ID, string Currency_Rate ,string Total_With_Rate, int? Vendor_ID,string BlNo);

        public string UpdateBLMaster_DAL(string ShipName, string ShipLeavingDate, string Arrival_Date, string Clearing_Charges, string Custom_Duty, string Other_Charage, string BLNO_Date, string Remarks, string Total_BL_Charges, int? BL_ID,
                  string Temp_ID, int? c_ID, int? Modified_by, int Currency_ID, string Currency_Rate , string Total_With_Rate ,int? Vendor_ID, string BlNo);

        public string DeleteBLDetail_DAL(int? BLDetail_ID);

        public string GetOldTempIDFromBLDetail_DAL(int? BL_ID);

        //By Yaseen 1-24-2022
        public string ChangeBlnoStatus_DAL(int? Blno_ID, int? Status_ID, int? c_ID, int? Modified_by);


        #endregion

        // By Yaseen   1-7-2022


        //By Yaseen 1-14-2022
        #region WorkShop


        public Pa_Garage_Master GarageMasterObj { get; set; }
        public Pa_Garage_Master Get_GarageMasterByID_DAL(int? Garage_Master_ID);
        public IEnumerable<Pa_Garage_Details> GarageDetailList { get; set; }

        public IEnumerable<Pa_Garage_Master> GarageMasterList { get; set; }
        public IEnumerable<Pa_Garage_Master> Get_GarageList_DAL();

        public IEnumerable<Pa_Garage_Details> Get_GarageDetailListBy_DAL(string Temp_ID, int? Garage_Master_ID);

        public string InsertGarageDetail_DAL(int? HdGarage_Master_ID, int? Stock_ID, string Temp_ID, string Description, string Amount);

        public string InsertGarageMaster_DAL(string Date_Send, string Remarks, int? GarageVendor_ID,
       string Temp_ID, int? c_ID, int? Created_by);

        public string UpdateGarageMaster_DAL(string Date_Send, string Remarks, int?  GarageVendor_ID,int Garage_Master_ID,
       string Temp_ID, int? c_ID,int? Modified_by);

        public string DeleteGarageDetail_DAL(int? Garage_Detail_ID);

        public string GetOldTempIDFromGarageDetail_DAL(int? Garage_Master_ID);

        #endregion
        //By Yaseen 1-14-2022
    }
}
