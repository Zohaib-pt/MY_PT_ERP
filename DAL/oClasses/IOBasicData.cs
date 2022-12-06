using DAL.Models;
using System.Collections.Generic;

namespace DAL.oClasses
{
    public interface IOBasicData
    {
        //---In this interface only the siganture of OBasicData class defined

        #region Methods Signature section
        public IEnumerable<Make_DAL> Get_MakeList_DAL(int c_ID);

        public string Insert_Make_DAL(string Make, string newFileName, string filepath, int? Created_by, int c_ID);

        public string Delete_Make_DAL(int? Make_ID);

        public string Update_Make_DAL(int? Make_ID, string Make, string newFileName, string filepath, int? Modified_by);



        //Yaseen
        public List<AlertNotification_DAL> Get_AlterNotiList_Dal(int UserId);
        //------------------------------------------------------------------------------------
        public IEnumerable<Pa_ModelDesc_DAL> Get_ModelList_DAL(int c_ID);
        public IEnumerable<Pa_MainMenu_DAL> Get_MainMenu_DAL(int UserId);
        public IEnumerable<Pa_MenuLevel1_DAL> Get_MenuLevel_1_DAL(int UserId);
        public IEnumerable<Pa_MenuLevel2_DAL> Get_MenuLevel_2_DAL(int UserId);
        public IEnumerable<Pa_Colors_DAL> Get_ColorsList_DAL();
        public IEnumerable<Pa_VehicleCategory_DAL> Get_VehicleCategoryList_DAL(int c_ID);
        public IEnumerable<Pa_Countries_DAL> Get_CountriesList_DAL(int c_ID);
        public IEnumerable<Pa_CurrencyMaster_DAL> Get_CurrencyMasterList_DAL(int c_ID);
        public IEnumerable<Pa_Partners_DAL> Get_VendorMasterList_DAL(int c_ID);
        public IEnumerable<Pa_Status_DAL> Get_StatusList_byType_DAL(string StatusType);
        public IEnumerable<Pa_Status_DAL> get_SalesTrans_Status_DAL();
        public IEnumerable<Pa_MakeCountries_DAL> Get_MakeCountryList_DAL(int c_ID);
        public IEnumerable<ItemMaster_DAL> Get_ItemMasterList_DAL();
        public IEnumerable<Pa_Attachments_DAL> attachmentList { get; set; }
        public IEnumerable<Pa_Partners_DAL> PartnersShippingList { get; set; }
        public IEnumerable<Pa_Partners_DAL> PartnersExportList { get; set; }
        public IEnumerable<BasicBanks_DAL> BasicBankList { get; set; }
        public IEnumerable<Pa_CurrencyMaster_DAL> currencyMasteList { get; set; }
        public Pa_WordDocument WordRef { get; set; }

        public Pa_WordDocument WordListById { get; set; }
        public string Insert_EngineType_DAL(string Engine_Power, int? Created_by, int c_ID);
        public string Update_EngineType_DAL(int? EngineType_ID, string Engine_Power, int? Modified_by);

        public string Delete_EngineType_DAL(int? EngineType_ID);
        public IEnumerable<EngineType_DAL> Get_EngineTypeList_DAL(int c_ID);
        public IEnumerable<StockType_DAL> Get_StockTypeList_DAL();
        public string Insert_MakeCountry_DAL(string CountryCode, string CountryName, int? Created_by, int c_ID);
        public string Update_MakeCountry_DAL(int? MakeCountry_ID, string CountryCode, string CountryName, int? Modified_by);
        public string Delete_MakeCountry_DAL(int? MakeCountry_ID);



        public string Insert_ModelDesc_DAL(int? Make_ID, string ModelDesciption, int? VehCategory_ID, int? MakeCountry_ID, int? Created_by, int c_ID, string Makecode , string Door, string HS_CODE, string DRIVE, string FUEL_TYPE, string EngineType);

        //Update Model desc
        public string Update_ModelDesc_DAL(int? ModelDesc_ID, int? Make_ID, string ModelDesciption, int? VehCategory_ID, int? MakeCountry_ID, int? Modified_by, string Makecode , string Door, string HS_CODE, string DRIVE, string FUEL_TYPE, string EngineType);
        public string Delete_ModelDesc_DAL(int? ModelDesc_ID);

        public IEnumerable<Pa_CarLocations_DAL> Get_CarLocation_DAL(int c_ID);

        //by Raffay - start / 8-jan-2020
        public string Insert_CarLocation_DAL(string CarLocation, int? Created_by, string Location_Type_Name, int c_ID);
        //by Raffay - End / 8-jan-2020


        public string Delete_CarLocation_DAL(int? CarLocation_ID);

        public string Update_CarLocation_DAL(int? CarLocation_ID, string CarLocation, int? Modified_by);

        public IEnumerable<Pa_PortOfExit_DAL> Get_PortOfExit_DAL(int c_ID);
        public string Insert_PortOfExit_DAL(string PortOfExit, int? Created_by, int c_ID);
        public string Delete_PortOfExit_DAL(int? PortOfExit_ID);

        public string Update_PortOfExit_DAL(int? PortOfExit_ID, string PortOfExit, int? Modified_by);




        public IEnumerable<Pa_Destinations_DAL> Get_Destinations_DAL(int c_ID);
        public string Insert_Destinations_DAL(string Destinations, int? Created_by, int c_ID);
        public string Delete_Destinations_DAL(int? Destinations_ID);

        public string Update_Destinations_DAL(int? Destinations_ID, string Destinations, int? Modified_by);

        public string Delete_Year_DAL(int? ModelYear_ID);
        //delete Global Location
        public string Delete_GlobalLocation_DAL(int? Country_ID);
        public string Update_Year_DAL(int? ModelYear_ID, string ModelYear, int? Modified_by);

        public string Delete_Color_DAL(int? Color_ID);

        public string Delete_Category_DAL(int? VehCategory_ID);
        //end here
        public string Update_Color_DAL(int? Color_ID, string ColorName, int? Modified_by);

        public string Insert_Color_DAL(string ColorName, int? Created_by, int c_ID);

        public string Insert_Category_DAL(string VehCategoryName, int? Created_by, int c_ID);
        public string Insert_GlobalLocation_DAL(string CountryName, int? Loc_TypeID, int? Created_by, int c_ID);

        public string Update_Category_DAL(int? VehCategory_ID, string VehCategoryName, int? Modified_by);

        public IEnumerable<Pa_Colors_DAL> Get_ColorList_DAL(int c_ID);
        public IEnumerable<Pa_VehicleCategory_DAL> Get_VehicleCategory_DAL(int c_ID);

        public string Update_Country_DAL(int? Country_ID, string CountryName, int? Loc_TypeID, int? Modified_by);
        public string Insert_YEAR_DAL(string ModelYear, int? Created_by, int c_ID);
        public IEnumerable<Pa_Year_DAL> Get_Year_List_DAL(int c_ID);

        public IEnumerable<Pa_VendorCategory_DAL> Get_VendorCategory_DAL();
        public IEnumerable<Pa_Partners_DAL> Get_Partners_DAL(int c_ID);
        public string Delete_Partners_DAL(int? Partner_ID);
        public string Insert_Partners_DAL(string PartnerName, string ContactNumber, string MobileNo, string ContactName, string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN, string BalanceType, string OpeningBalanceDate, string OpeningBalance,
 int? VendorCat_ID, string PartnerType, string Remarks, int? Created_by, int c_ID,string SellerType);

        public string Update_Partners_DAL(int? Partner_ID, string PartnerName, string ContactNumber, string MobileNo, string ContactName, string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN, string BalanceType, string OpeningBalanceDate, string OpeningBalance,
 int? VendorCat_ID, string Remarks, int? Modified_by,string SellerType);

        public IEnumerable<Pa_Partners_DAL> Get_PartnersGarrage_DAL(int c_ID);
        public IEnumerable<Pa_Partners_DAL> Get_PartnersSeller_DAL(int c_ID);
        public IEnumerable<Pa_Partners_DAL> Get_PartnersAgent_DAL(int c_ID);
        public IEnumerable<Pa_CustomerCategory_DAL> Get_CustomerCategory_DAL();
        public IEnumerable<Pa_CustomersMaster_DAL> Get_CustomersMaster_DAL(int c_ID);
        public string Delete_CustomersMaster_DAL(int? Customer_ID);
        public string Insert_CustomersMaster_DAL(string CustomerName, string ContactNumber, string MobileNo, string ContactName, string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN,
            string OpeningBalance, string OpeningBalanceDate, string BalanceType,
            int? CustomerCat_ID, string Remarks, int? Created_by, int c_ID);

        public string Update_CustomersMaster_DAL(int? Customer_ID, string CustomerName, string ContactNumber, string MobileNo, string ContactName, string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN,
            string OpeningBalance,string OpeningBalanceDate,string BalanceType,
            int? CustomerCat_ID, string Remarks, int? Modified_by);
        public IEnumerable<Pa_ShowRoom_DAL> Get_ShowRoom_DAL(int c_ID);
        public string Insert_ShowRoom_DAL(string ShowRoomCode, string ShowRoomName, string showroom_no, string TRN, int? Created_by, int c_ID);
        public string Update_ShowRoom_DAL(int? ShowRoom_ID, string ShowRoomCode, string ShowRoomName, string showroom_no, string TRN, int? Modified_by);


        public IEnumerable<Accounts_DAL> Select_PV_OnAccounts_DAL(int c_ID);

        public IEnumerable<Accounts_DAL> Select_RV_OnAccounts_DAL(int c_ID);


        public IEnumerable<Accounts_DAL> Select_PV_PayVia_DAL(int c_ID);
        public IEnumerable<AdminRights> Get_AdminRolesByAdminUserID_DAL(int? User_ID);
        public IEnumerable<Pa_Attachments_DAL> GetCustomerMaster_Attachments_DAL(int? Customer_ID, string Type);
        public string InsertAttachments_CustomerMaster_DAL(int? Customer_ID, string Document_Type, string Type, string newFileName, string filepath, int? Created_by);
        public IEnumerable<Pa_Attachments_DAL> GetVendorMaster_Attachments_DAL(int? Partner_ID, string Type);
        public string InsertAttachments_VendorMaster_DAL(int? Partner_ID, string Document_Type, string Type, string newFileName, string filepath, int? Created_by);
        public IEnumerable<Pa_Attachments_DAL> GetShowroom_Attachments_DAL(int? ShowRoom_ID, string Type);
        public string InsertAttachments_Showroom_DAL(int? ShowRoom_ID, string Document_Type, string Type, string newFileName, string filepath, int? Created_by);
        public IEnumerable<Pa_Partners_DAL> Get_ShippingCompaniesList_DAL(int c_ID);
        public IEnumerable<Pa_Partners_DAL> Get_ExportCompaniesList_DAL(int c_ID);
        public IEnumerable<BasicBanks_DAL> Get_BasicBanksList_DAL(int c_ID);

        public string Insert_BasicBank_DAL(string BankName, string AccountName, string AccountNumber, string IBAN, string branch, string SwiftCode, string Address, string ContactNumber, int? Created_by, int c_ID);
          public string Delete_BasicBank_DAL(int? Bank_ID);
        public string Update_BasicBank_DAL(int? Bank_ID, string BankName, string AccountName, string AccountNumber, string IBAN, string branch, string SwiftCode, string Address, string ContactNumber, int? Modified_by);
          public string InsertCurrencyMaster_DAL(string Currency_Name, string Currency_ShortName, double? Curr_Rate, string Minor_ShortName, int? Created_by, int c_ID);

        public string UpdateCurrencyMaster_DAL(int? Currency_ID, string Currency_Name, string Currency_ShortName, double? Curr_Rate, string Minor_ShortName, int? Modified_at); public string DeleteCurrencyMaster_DAL(int? Currency_ID);
        public Pa_CustomersMaster_DAL GetCustomerDetail_DAL(int? Customer_ID);
        public Pa_Partners_DAL GetVendorDetail_DAL(int? Vendor_ID);

        public IEnumerable<Pa_CustomersMaster_DAL> Get_CustomersList_DAL(int c_ID);
        public IEnumerable<Pa_Partners_DAL> get_PartyList_DAL(int c_ID);
        public IEnumerable<Pa_Partners_DAL> get_PartyType_DAL();
        public IEnumerable<Pa_Countries_DAL> Get_LocType_DAL();
        #endregion

        #region Properties declaring section
        public IEnumerable<Make_DAL> makeList { get; set; }
        public Make_DAL makeobject { get; set; }
        
        
        //By Yaseen
       // public IEnumerable<AlterNotifiacation_DAL> AlterNotiList { get; set; }
        //By Yaseen
        public IEnumerable<Pa_ModelDesc_DAL> ModelDescList { get; set; }
        public IEnumerable<Pa_MainMenu_DAL> MainMenuList { get; set; }

        public IEnumerable<Pa_MenuLevel1_DAL> MenuLevel1List { get; set; }
        public IEnumerable<Pa_MenuLevel2_DAL> MenuLevel2List { get; set; }
        public IEnumerable<Pa_PurchaseDetails_DAL> purchaseDetailList { get; set; }
        public IEnumerable<EngineType_DAL> EngineTypeList { get; set; }
        public IEnumerable<Pa_MakeCountries_DAL> Make_countriesList { get; set; }
        public IEnumerable<Pa_CarLocations_DAL> CarLocationList { get; set; }
        public IEnumerable<Pa_PortOfExit_DAL> PortOfExitList { get; set; }

        public IEnumerable<Pa_Destinations_DAL> DestinationsList { get; set; }
        public IEnumerable<Pa_Year_DAL> YearList { get; set; }
        public IEnumerable<Pa_Countries_DAL> CountryList { get; set; }
        public IEnumerable<Pa_VehicleCategory_DAL> CategoryList { get; set; }
        public IEnumerable<Pa_Colors_DAL> ColorList { get; set; }
        public IEnumerable<Pa_ItemLocations_DAL> ItemLocationList { get; set; }
        public IEnumerable<Pa_ItemGroups_DAL> ItemGroupList { get; set; }
        public IEnumerable<Pa_ItemCategory_DAL> ItemCategoryList { get; set; }

        public IEnumerable<Pa_Partners_DAL> PartnersList { get; set; }
        public IEnumerable<Pa_Partners_DAL> PartnersGarrageList { get; set; }
        public IEnumerable<Pa_Partners_DAL> PartnersSellerList { get; set; }
        public IEnumerable<Pa_Partners_DAL> PartnersAgentList { get; set; }

        public IEnumerable<Pa_CustomersMaster_DAL> CustomerMasterList { get; set; }
        public Pa_CustomersMaster_DAL GetCustomerObj { get; set; }
        public Pa_Partners_DAL GetVendorObj { get; set; }
        public IEnumerable<Pa_ShowRoom_DAL> ShowRoomList { get; set; }
        public IEnumerable<AdminRights> rolesmenus { get; set; }
        public IEnumerable<Pa_WordDocument> WordDocumentList { get; set; }


        public IEnumerable<Words> Get_WordsDAL_List();
        public IEnumerable<Words> WordsRegistrationList { get; set; }
        public string InsertWordsDAL(string Words);
        public string DeleteWordsExit(int? ID);
        public string UpdateWordsDAL(int? ID, string Words);




        public IEnumerable<sys_Ac_Type_DAL> Get_Select_Sys_Ac_Type_DAL();
        public IEnumerable<subHeadAccounts_DAL> Get_select_sub_HeadAccount_DAL();
        public IEnumerable<AccountsHead_DAL> Get_select_HeadAccounts_DAL();

        public Pa_Inventry_DAL GetItemObj { get; set; }

        #endregion


        //Faraz New Work
        //list,Add and update work
        public IEnumerable<Pa_CustomerCats_DAL> CustomerCatList { get; set; }
        public IEnumerable<Pa_Inventry_DAL> InventryMasterList { get; set; }
        public IEnumerable<Pa_Inventry_DAL> MultipleMakeList { get; set; }
        public IEnumerable<Pa_Inventry_DAL> MultipleYearList { get; set; }
        public Pa_ModelDesc_DAL GetCodeObj { get; set; }
        public IEnumerable<Pa_Inventry_DAL> InventryMasterObj { get; set; }

        public IEnumerable<Pa_Inventry_DAL> Get_InventryMasterList_DAL();
        public IEnumerable<Pa_Inventry_DAL> Get_MultipleMakeList_DAL(int Item_ID);
        public IEnumerable<Pa_Inventry_DAL> Get_MultipleYearList_DAL(int Item_ID);

        public string Insert_InventryMaster_DAL(string ItemCode, string ItemName, string ItemDescription, string IsSerializable, string BarCode, string UnitPrice, string SalePrice, string CostMethod, string Comment, string UOM, int Group_ID, int Category_ID);

        public string Update_InventryMaster_DAL(int? Item_ID, string ItemCode, string ItemName, string ItemDescription, string IsSerializable, string BarCode, string UnitPrice, string SalePrice, string CostMethod, string Comment, string UOM, int Group_ID, int Category_ID);


        public string Delete_Inventory_DAL(int Item_ID);
        public IEnumerable<Pa_Inventry_DAL> Get_UOM_DAL(int c_ID);

        public Pa_Inventry_DAL GetItemDetail_DAL(string Item);
        public IEnumerable<Pa_UOM_DAL> UOMList { get; set; }

        public IEnumerable<Pa_UOM_DAL> Get_UOMList_DAL(int c_ID);


        public string Insert_UOM_DAL(string UOM, int? Created_by, int c_ID);

        public string Update_UOM_DAL(int? UOM_ID, string UOM, int? Modified_by);

        public string Delete_UOM_DAL(int? UOM_ID);


        public IEnumerable<Pa_ItemGroups_DAL> Get_ItemGroupList_DAL(int c_ID);


        public string Insert_ItemGroup_DAL(string Group_Code, string Group_Name, int? Created_by, int c_ID);

        public string Update_ItemGroup_DAL(int? Group_ID, string Group_Code, string Group_Name, int? Modified_by);

        public string Delete_ItemGroup_DAL(int? Group_ID);

        public IEnumerable<Pa_ItemCategory_DAL> Get_ItemCategoryList_DAL(int c_ID);


        public string Insert_ItemCategory_DAL(string Category_Code, string Category_Name, int? Created_by, int c_ID);

        public string Update_ItemCategory_DAL(int? Category_ID, string Category_Code, string Category_Name, int? Modified_by);

        public string Delete_ItemCategory_DAL(int? Category_ID);
        public IEnumerable<Pa_ItemGroups_DAL> Get_ItemGroup_DAL(int c_ID);

        public IEnumerable<Pa_ItemCategory_DAL> Get_ItemCategory_DAL(int c_ID);
        public string Insert_InventryPartsMaster_DAL(string ItemCode, string ItemName, string ItemDescription, string IsSerializable, string BarCode, string UnitPrice, string SalePrice, string CostMethod, string Comment, string UOM, int Group_ID, int Category_ID, string Traditional, string FUEL_TYPE, string Transmission, string Drive, string StartYear, string EndYear, string OpeningBal);

        public string Update_InventryPartsMaster_DAL(int? Item_ID, string ItemCode, string ItemName, string ItemDescription, string IsSerializable, string BarCode, string UnitPrice, string SalePrice, string CostMethod, string Comment, string UOM, int Group_ID, int Category_ID, string Traditional, string FUEL_TYPE, string Transmission, string Drive, string StartYear, string EndYear, string OpeningBal);
        public IEnumerable<Make_DAL> Get_MakeModelList_DAL(int c_ID);


        public string Insert_InventryPartsMultipleMake_DAL(int Item_ID, int make_ID);
        
        public string Insert_InventryPartsMultipleYear_DAL(int Item_ID, string Year);

      

        public string Delete_MultipleMake_DAL(int? Item_ID);
        
        public string Delete_MultipleYear_DAL(int? Item_ID);
        
        public IEnumerable<Pa_ItemLocations_DAL> Get_ItemLocationList_DAL(int c_ID);


        public string Insert_ItemLocation_DAL(string ItemLocationName, int? Created_by, int c_ID);

        public string Update_ItemLocation_DAL(int? ItemLocation_ID, string ItemLocationName, int? Modified_by);

        public string Delete_ItemLocation_DAL(int? ItemLocation_ID);



        public IEnumerable<Pa_CustomerCats_DAL> Get_CustomerCatsList_DAL();
        public IEnumerable<Pa_CustomerCats_DAL> Get_CustomerCatList_DAL(int c_ID);


        public string Insert_CustomerCat_DAL(string CustomerCatName, int? Created_by, int c_ID);

        public string Update_CustomerCat_DAL(int? CustomerCat_ID, string CustomerCatName, int? Modified_by);

        public string Delete_CustomerCat_DAL(int? CustomerCat_ID);
        public string Delete_EngineSpecs_DAL(int? EngineSpecsCode_ID);
        public Pa_Inventry_DAL GetBarcodeDetail_DAL(string Barcode);
        public Pa_ModelDesc_DAL GetCodeDetail_DAL(string Code);

        public Pa_ModelDesc_DAL GetDetailsByCode_DAL(string Code);

        #region MultipleEngineSpecsCode

        public IEnumerable<EngineSpecsCode_DAL> EngineSpecsCodeList { get; set; }

        public IEnumerable<Pa_Inventry_DAL> MultipleEngineSpecsCodeList { get; set; }

        public IEnumerable<EngineSpecsCode_DAL> Get_EngineSpecsCodeList_DAL(int c_ID);

        public IEnumerable<Pa_Inventry_DAL> Get_MultipleEngineSpecsCodeList_DAL(int Item_ID);

        public string Delete_MultipleEngineSpecsCode_DAL(int Item_ID);

        public string Insert_MultipleEngineSpecsCode_DAL(int Item_ID, int EngineSpecsCode_ID);


        public string Insert_EngineSpecsCode_DAL(string EngineSpecsCode, string SpecsDescription, int Created_by, int c_ID);
       
        public string Update_EngineSpecsCode_DAL(int EngineSpecsCode_ID, string EngineSpecsCode, string SpecsDescription, int Modified_by);
       
        public string Delete_EngineSpecsCode_DAL(int? Item_ID);

        public IEnumerable<pa_Shipping_Info> Get_Notification_ETA_DAL();



        public IEnumerable<CityRegistration> Get_CityReg_DAL(int ID);
        public IEnumerable<CityRegistration> CityRegistrationsObj { get; set; }
        public string Update_CityReg_DAL(int? ID, string CityName, string CityNameEnglish);
        public string Insert_CityReg_DAL(string CityName, string CityNameEnglish);
        public string DeleteCityRegExit(int? ID);
        public IEnumerable<CityRegistration> CityRegistrations_DAL_NEW(int ID);
        public IEnumerable<CityRegistration> Get_CityRegistration_DAL_List();
        public IEnumerable<CityRegistration> CustomerRegistrationList { get; set; }
        public IEnumerable<CityRegistration> Get_CityReg_Obj { get; set; }
        public IEnumerable<CityRegistration> Get_CityRegistrationsObj { get; set; }
        #endregion
        public Pa_WordDocument WordLoadRef();
        public IEnumerable<Pa_WordDocument> Get_WordDocument_List_DAL(int c_ID, int WordDocument_ID, int Vendor_ID, int Customer_ID, string WordTitle);
        public Pa_WordDocument Get_WordList(int c_ID, int WordDocument_ID);
        public Pa_WordDocument Get_WordList_I(int c_ID, int WordDocument_ID);
        public string Insert_WordDocument_DAL(string WordRef, int? c_ID, string FileName, string Filepath, int Vendor_ID, int Customer_ID, string WordTitle,int Create_by);

        public string Delete_WordDocument_DAL(int? id);
        public string ChangeMasterStatus_DAL(int? WordDocument_ID, int? Status_ID, int? c_ID, string Trans_Type, int? Modified_by, string QRName, string PDFName);
        #region PayVia For SaleInvoice_trd
        public IEnumerable<Accounts_DAL> Select_PV_OnAccounts_Trd(int c_ID);

        #endregion


        #region

        public IEnumerable<Pa_CommisionRates_DAL> CommisionRateobjList { get; set; }
        public IEnumerable<Pa_CommisionRates_DAL> Get_CommisionRatesList_DAL();
        public string Insert_CommisionRates_DAL(int Category_ID, decimal Rate, int Status, int? Created_by);
        public string Update_CommisionRates_DAL(int? CommissionRates_ID, int Category_ID, decimal Rate, int Status, int? Modified_by);

        public string Delete_CommisionRates_DAL(int? CommissionRates_ID);
        #endregion
        public IEnumerable<Pa_Status_DAL> get_SalesOther_Status_DAL();
        public IEnumerable<Accounts_DAL> Select_PV_OnAccounts_SaleReturn_Trd(int c_ID);

        public IEnumerable<Accounts_DAL> Select_Expense_Accounts_DAL(int c_ID);


    }

}
