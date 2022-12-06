using DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.oClasses
{
    public class OBasicData : IOBasicData
    {

        private readonly IConnectionStringManager _conMgr;
        private readonly ISqlHelper dbLayer;
        SqlConnection con;

        public OBasicData(IConnectionStringManager conMgr, ISqlHelper sqlHelp)
        {

            _conMgr = conMgr;
            dbLayer = sqlHelp;
            //---Getting Connection String
            Constr = _conMgr.GetConnectionString();
        }
        public string Constr { get; set; }



       // public IEnumerable<AlterNotifiacation_DAL> AlterNotiList { get; set; }
        public IEnumerable<Pa_ItemLocations_DAL> ItemLocationList { get; set; }
        public IEnumerable<Pa_Inventry_DAL> InventryMasterList { get; set; }
        public IEnumerable<Make_DAL> makeList { get; set; }
        public Make_DAL makeobject { get; set; }
        public IEnumerable<Pa_ModelDesc_DAL> ModelDescList { get; set; }
        public IEnumerable<Pa_MainMenu_DAL> MainMenuList { get; set; }
        public IEnumerable<Pa_MenuLevel1_DAL> MenuLevel1List { get; set; }
        public IEnumerable<Pa_MenuLevel2_DAL> MenuLevel2List { get; set; }
        public IEnumerable<Pa_PurchaseDetails_DAL> purchaseDetailList { get; set; }
        public IEnumerable<EngineType_DAL> EngineTypeList { get; set; }
        public IEnumerable<StockType_DAL> StockTypeList { get; set; }
        public IEnumerable<Pa_MakeCountries_DAL> Make_countriesList { get; set; }
        public IEnumerable<Pa_CarLocations_DAL> CarLocationList { get; set; }
        public IEnumerable<Pa_PortOfExit_DAL> PortOfExitList { get; set; }
        public IEnumerable<Pa_Destinations_DAL> DestinationsList { get; set; }
        public IEnumerable<Pa_Year_DAL> YearList { get; set; }
        public IEnumerable<Pa_Countries_DAL> CountryList { get; set; }
        public IEnumerable<Pa_VehicleCategory_DAL> CategoryList { get; set; }
        public IEnumerable<Pa_Colors_DAL> ColorList { get; set; }
        public IEnumerable<Pa_CustomerCats_DAL> CustomerCatList { get; set; }
        public IEnumerable<Pa_Partners_DAL> PartnersList { get; set; }
        public IEnumerable<Pa_Partners_DAL> PartnersGarrageList { get; set; }
        public IEnumerable<Pa_Partners_DAL> PartnersSellerList { get; set; }
        public IEnumerable<Pa_Partners_DAL> PartnersAgentList { get; set; }
        public IEnumerable<Pa_CustomersMaster_DAL> CustomerMasterList { get; set; }
        public IEnumerable<Pa_ShowRoom_DAL> ShowRoomList { get; set; }
        public IEnumerable<AdminRights> rolesmenus { get; set; }
        public IEnumerable<Pa_Attachments_DAL> attachmentList { get; set; }
        public IEnumerable<Pa_Partners_DAL> PartnersShippingList { get; set; }
        public IEnumerable<Pa_Partners_DAL> PartnersExportList { get; set; }
        public IEnumerable<BasicBanks_DAL> BasicBankList { get; set; }

        public IEnumerable<Pa_CurrencyMaster_DAL> currencyMasteList { get; set; }
        public Pa_CustomersMaster_DAL GetCustomerObj { get; set; }
        public Pa_Partners_DAL GetVendorObj { get; set; }
        public Pa_Inventry_DAL GetItemObj { get; set; }
        public IEnumerable<Pa_ItemGroups_DAL> ItemGroupList { get; set; }
        public IEnumerable<Pa_ItemCategory_DAL> ItemCategoryList { get; set; }
        public IEnumerable<Pa_Inventry_DAL> MultipleMakeList { get; set; }
        public IEnumerable<Pa_Inventry_DAL> MultipleYearList { get; set; }
        public Pa_ModelDesc_DAL GetCodeObj { get; set; }
        public IEnumerable<Pa_Inventry_DAL> InventryMasterObj { get; set; }
        public IEnumerable<Pa_WordDocument> WordDocumentList { get; set; }
        public Pa_WordDocument WordRef { get; set; }
        public Pa_WordDocument WordListById { get; set; }
        //Get make list
        public IEnumerable<Make_DAL> Get_MakeList_DAL(int c_ID)
        {
            string sp = "pa_Select_MakeList";
            List<Make_DAL> itemlist = new List<Make_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Make_DAL item = new Make_DAL();

                        item.Make_ID = Convert.ToInt32(rdr["Make_ID"]);
                        item.Make = rdr["Make"].ToString();
                        item.ImagePath = rdr["ImagePath"].ToString();
                        item.ImageName = rdr["ImageName"].ToString();
                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public IEnumerable<AccountsHead_DAL> Get_select_HeadAccounts_DAL()
        {
            string sp = "select_HeadAccounts";
            List<AccountsHead_DAL> itemlist = new List<AccountsHead_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        AccountsHead_DAL item = new AccountsHead_DAL();
                        item.ID = Convert.ToInt32(rdr["ID"]);
                        item.HeadAccount = rdr["HeadAccount"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        //Delete receipt master 
        public string Delete_Inventory_DAL(int Item_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Item_ID",Item_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_InventoryItem", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public IEnumerable<subHeadAccounts_DAL> Get_select_sub_HeadAccount_DAL()
        {
            string sp = "select_sub_HeadAccount";
            List<subHeadAccounts_DAL> itemlist = new List<subHeadAccounts_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        subHeadAccounts_DAL item = new subHeadAccounts_DAL();
                        item.sub_headAccount_ID = Convert.ToInt32(rdr["sub_headAccount_ID"]);
                        item.sub_HeadAccount = rdr["sub_HeadAccount"].ToString();
                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public IEnumerable<sys_Ac_Type_DAL> Get_Select_Sys_Ac_Type_DAL()
        {
            string sp = "Select_Sys_Ac_Type";
            List<sys_Ac_Type_DAL> itemlist = new List<sys_Ac_Type_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        sys_Ac_Type_DAL item = new sys_Ac_Type_DAL();
                        item.sys_Ac_Type_ID = Convert.ToInt32(rdr["sys_Ac_Type_ID"]);
                        item.sys_Ac_TypeName = rdr["sys_Ac_TypeName"].ToString();
                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }


        //Insert New Tasks
        public string Insert_Make_DAL(string Make, string newFileName, string filepath, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Make",Make),
                new SqlParameter("@imagepath",filepath),
                new SqlParameter("@imgname",newFileName),
                 new SqlParameter("@Created_by",Created_by),
                 new SqlParameter("@c_ID",c_ID)



            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_Make", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Delete_Make_DAL(int? Make_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Make_ID",Make_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_Make", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        //Update New Tasks
        public string Update_Make_DAL(int? Make_ID, string Make, string newFileName, string filepath, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@Make_ID",Make_ID),
                new SqlParameter("@Make",Make),
                new SqlParameter("@imagepath",filepath),
                new SqlParameter("@imgname",newFileName),
                 new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_Make", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }



        //Get Model List
        public IEnumerable<Pa_ModelDesc_DAL> Get_ModelList_DAL(int c_ID)
        {


            string sp = "pa_Select_ModelList";
            List<Pa_ModelDesc_DAL> itemlist = new List<Pa_ModelDesc_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_ModelDesc_DAL item = new Pa_ModelDesc_DAL();

                        item.ModelDesc_ID = Convert.ToInt32(rdr["ModelDesc_ID"]);
                        item.ModelDesciption = rdr["ModelDesciption"].ToString();
                        item.Make_ID = Convert.ToInt32(rdr["Make_ID"]);
                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();
                        item.MakeCountry_ID = rdr["MakeCountry_ID"].ToString();
                        
                        item.VehCategory_ID = Convert.ToInt32(rdr["VehCategory_ID"]);
                        item.Make = rdr["Make"].ToString();
                        item.CountryName = rdr["CountryName"].ToString();

                        item.VehCategoryName = rdr["VehCategoryName"].ToString();
                        item.Makecode = rdr["Makecode"].ToString();


                        item.Door = rdr["Door"].ToString();
                        item.Drive = rdr["Drive"].ToString();
                        item.EngineType = Convert.ToInt32(rdr["Engine_Type"]);
                        item.FuelType = rdr["Fuel_Type"].ToString();
                        item.Hs_Code = rdr["Hs_Code"].ToString();




                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        //Get main menu
        public IEnumerable<pa_Shipping_Info> Get_Notification_ETA_DAL()
        {
            string sp = "select_Notification_ETA_Alert";
            List<pa_Shipping_Info> itemlist = new List<pa_Shipping_Info>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                  

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_Shipping_Info item = new pa_Shipping_Info();

                        item.Shipping_Info_Ref = rdr["Shipping_Info_Ref"].ToString();
                        item.ETA = rdr["ETA"].ToString();
                        item.Shipping_info_ID = Convert.ToInt32(rdr["Shipping_info_ID"]);
             
                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public IEnumerable<Pa_MainMenu_DAL> Get_MainMenu_DAL(int UserId)
        {
            string sp = "pa_Select_MainMenuList";
            List<Pa_MainMenu_DAL> itemlist = new List<Pa_MainMenu_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", UserId);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_MainMenu_DAL item = new Pa_MainMenu_DAL();

                        item.MainMenu_ID = Convert.ToInt32(rdr["MainMenu_ID"]);
                        item.MainMenuName = rdr["MainMenuName"].ToString();
                        item.IsExpandable = rdr["IsExpandable"].ToString();
                        item.IsVisible = rdr["IsVisible"].ToString();
                        item.Icon = rdr["icon"].ToString();
                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        //Get menu level 1
        public IEnumerable<Pa_MenuLevel1_DAL> Get_MenuLevel_1_DAL(int UserId)
        {
            string sp = "pa_Select_MenuLevel_1_List";
            List<Pa_MenuLevel1_DAL> itemlist = new List<Pa_MenuLevel1_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", UserId);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_MenuLevel1_DAL item = new Pa_MenuLevel1_DAL();

                        item.MainMenu_ID = Convert.ToInt32(rdr["MainMenu_ID"]);
                        item.MenuLevel1_ID = Convert.ToInt32(rdr["MenuLevel1_ID"]);
                        item.MenuLevel1Name = rdr["MenuLevel1Name"].ToString();
                        item.IsExpandable = rdr["IsExpandable"].ToString();
                        item.IsVisible = rdr["IsVisible"].ToString();
                        item.Controller = rdr["Controller"].ToString();
                        item.Action = rdr["Action"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }


        //Get menu level 2
        public IEnumerable<Pa_MenuLevel2_DAL> Get_MenuLevel_2_DAL(int UserId)
        {
            string sp = "pa_Select_MenuLevel_2_List";
            List<Pa_MenuLevel2_DAL> itemlist = new List<Pa_MenuLevel2_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", UserId);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_MenuLevel2_DAL item = new Pa_MenuLevel2_DAL();

                        item.MenuLevel1_ID = Convert.ToInt32(rdr["MenuLevel1_ID"]);
                        item.MenuLevel2_ID = Convert.ToInt32(rdr["MenuLevel2_ID"]);
                        item.MenuLevel2Name = rdr["MenuLevel2Name"].ToString();
                        item.IsVisible = rdr["IsVisible"].ToString();
                        item.Controller = rdr["Controller"].ToString();
                        item.Action = rdr["Action"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }


        //Get Colors List
        public IEnumerable<Pa_Colors_DAL> Get_ColorsList_DAL()
        {


            string sp = "pa_Select_ColorsList";
            List<Pa_Colors_DAL> itemlist = new List<Pa_Colors_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Colors_DAL item = new Pa_Colors_DAL();

                        item.Color_ID = Convert.ToInt32(rdr["Color_ID"]);
                        item.ColorName = rdr["ColorName"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        //Get Vehicle Category List
        public IEnumerable<Pa_VehicleCategory_DAL> Get_VehicleCategoryList_DAL(int c_ID)
        {


            string sp = "pa_Select_VehicleCategoryList";
            List<Pa_VehicleCategory_DAL> itemlist = new List<Pa_VehicleCategory_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_VehicleCategory_DAL item = new Pa_VehicleCategory_DAL();

                        item.VehCategory_ID = Convert.ToInt32(rdr["VehCategory_ID"]);
                        item.VehCategoryName = rdr["VehCategoryName"].ToString();
                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }



        //Get countries list
        public IEnumerable<Pa_Countries_DAL> Get_CountriesList_DAL(int c_ID)
        {


            string sp = "pa_Select_GlobalCountries";
            List<Pa_Countries_DAL> itemlist = new List<Pa_Countries_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Countries_DAL item = new Pa_Countries_DAL();

                        item.Country_ID = Convert.ToInt32(rdr["Country_ID"]);
                        item.CountryName = rdr["CountryName"].ToString();
                        item.loctype_ID = Convert.ToInt32(rdr["Loc_TypeID"].ToString());
                        item.LocTypeName = rdr["LocTypeName"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        //Get Make Coutries list
        public IEnumerable<Pa_MakeCountries_DAL> Get_MakeCountryList_DAL(int c_ID)
        {


            string sp = "pa_Select_VehicleMakeCountry";
            List<Pa_MakeCountries_DAL> itemlist = new List<Pa_MakeCountries_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_MakeCountries_DAL item = new Pa_MakeCountries_DAL();

                        item.MakeCountry_ID = Convert.ToInt32(rdr["MakeCountry_ID"]);
                        item.CountryName = rdr["CountryName"].ToString();
                        item.CountryCode = rdr["CountryCode"].ToString();
                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }



        //Get Currency Master List 
        public IEnumerable<Pa_CurrencyMaster_DAL> Get_CurrencyMasterList_DAL(int c_ID)
        {


            string sp = "Pa_Select_CurrencyMasterList";
            List<Pa_CurrencyMaster_DAL> itemlist = new List<Pa_CurrencyMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_CurrencyMaster_DAL item = new Pa_CurrencyMaster_DAL();

                        item.Currency_ID = Convert.ToInt32(rdr["Currency_ID"]);
                        item.Currency_Name = rdr["Currency_Name"].ToString();
                        item.Currency_ShortName = rdr["Currency_ShortName"].ToString();
                        item.Curr_Rate = rdr["Curr_Rate"].ToString();
                        item.Minor_ShortName = rdr["Minor_ShortName"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        //Get vendor master
        public IEnumerable<Pa_Partners_DAL> Get_VendorMasterList_DAL(int c_ID)
        {


            string sp = "Pa_Select_VendorMasterList";
            List<Pa_Partners_DAL> itemlist = new List<Pa_Partners_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Partners_DAL item = new Pa_Partners_DAL();

                        item.Partner_ID = Convert.ToInt32(rdr["Vendor_ID"]);
                        item.PartnerName = rdr["Vendor_Name"].ToString();
                        item.VendorCat_ID = Convert.ToInt32(rdr["VendorCat_ID"]);
                        item.PartnerType = rdr["PartnerType"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        public IEnumerable<Pa_Partners_DAL> get_PartyList_DAL(int c_ID)
        {


            string sp = "get_PartyList";
            List<Pa_Partners_DAL> itemlist = new List<Pa_Partners_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Partners_DAL item = new Pa_Partners_DAL();

                        item.Partner_ID = Convert.ToInt32(rdr["Party_ID"]);
                        item.PartnerName = rdr["PartyName"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        public IEnumerable<Pa_Partners_DAL> get_PartyType_DAL()
        {


            string sp = "get_Party_Type";
            List<Pa_Partners_DAL> itemlist = new List<Pa_Partners_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Partners_DAL item = new Pa_Partners_DAL();

                        item.Party_Type_ID = Convert.ToInt32(rdr["Party_Type_ID"]);
                        item.Party_Type = rdr["Party_Type"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        //Get Purchase Status
        public IEnumerable<Pa_Status_DAL> Get_StatusList_byType_DAL(string StatusType)
        {


            string sp = "Select_Status_byType";
            List<Pa_Status_DAL> itemlist = new List<Pa_Status_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StatusType", StatusType);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Status_DAL item = new Pa_Status_DAL();

                        item.Status_ID = Convert.ToInt32(rdr["Status_ID"]);
                        item.Status = rdr["Status"].ToString();



                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public IEnumerable<Pa_Status_DAL> get_SalesTrans_Status_DAL()
        {


            string sp = "get_SalesTrans_Status";
            List<Pa_Status_DAL> itemlist = new List<Pa_Status_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Status_DAL item = new Pa_Status_DAL();

                        item.Status_ID = Convert.ToInt32(rdr["Status_ID"]);
                        item.Status = rdr["Status"].ToString();



                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        //Get item master list
        public IEnumerable<ItemMaster_DAL> Get_ItemMasterList_DAL()
        {


            string sp = "select_ItemMasters";
            List<ItemMaster_DAL> itemlist = new List<ItemMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        ItemMaster_DAL item = new ItemMaster_DAL();

                        item.Item_ID = Convert.ToInt32(rdr["Item_ID"]);
                        item.ItemCode = rdr["ItemCode"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }




        //Get EngineTypeList
        public IEnumerable<EngineType_DAL> Get_EngineTypeList_DAL(int c_ID)
        {


            string sp = "Pa_Select_EngineTypeList";
            List<EngineType_DAL> itemlist = new List<EngineType_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        EngineType_DAL item = new EngineType_DAL();

                        item.EngineType_ID = Convert.ToInt32(rdr["EngineType_ID"]);
                        item.Engine_Power = rdr["Engine_Power"].ToString();
                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();



                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public IEnumerable<StockType_DAL> Get_StockTypeList_DAL()
        {


            string sp = "Select_StockType";
            List<StockType_DAL> itemlist = new List<StockType_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        StockType_DAL item = new StockType_DAL();

                        item.StockType_ID = Convert.ToInt32(rdr["StockType_ID"]);
                        item.StockTypeName = rdr["StockTypeName"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        //Insert engine type
        public string Insert_EngineType_DAL(string Engine_Power, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Engine_Power",Engine_Power),
                new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Insert_EngineType", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        //Update Engine type
        public string Update_EngineType_DAL(int? EngineType_ID, string Engine_Power, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@EngineType_ID",EngineType_ID),
                new SqlParameter("@Engine_Power",Engine_Power),

                 new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_EngineType", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        //Delete Engine type
        public string Delete_EngineType_DAL(int? EngineType_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@EngineType_ID",EngineType_ID)


            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_EngineType", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }



        //Insert Make Country
        public string Insert_MakeCountry_DAL(string CountryCode, string CountryName, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@CountryCode",CountryCode),
                 new SqlParameter("@CountryName",CountryName),
                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Insert_MakeCountry", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        //Update  Make Country
        public string Update_MakeCountry_DAL(int? MakeCountry_ID, string CountryCode, string CountryName, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@MakeCountry_ID",MakeCountry_ID),

                  new SqlParameter("@CountryCode",CountryCode),
                 new SqlParameter("@CountryName",CountryName),
                   new SqlParameter("@Modified_by",Modified_by)




            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_MakeCountry", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        //Delete  Make Country
        public string Delete_MakeCountry_DAL(int? MakeCountry_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@MakeCountry_ID",MakeCountry_ID)


            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_MakeCountry", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        //Insert Model desc
        public string Insert_ModelDesc_DAL(int? Make_ID, string ModelDesciption, int? VehCategory_ID, int? MakeCountry_ID, int? Created_by, int c_ID, string Makecode, string Door, string HS_CODE, string DRIVE, string FUEL_TYPE, string EngineType)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Make_ID",Make_ID),
                  new SqlParameter("@ModelDesciption",ModelDesciption),
                    new SqlParameter("@VehCategory_ID",VehCategory_ID),
                      new SqlParameter("@MakeCountry_ID",MakeCountry_ID),
                      new SqlParameter("@Created_by",Created_by),
                       new SqlParameter("@c_ID",c_ID),
                      new SqlParameter("@Makecode",Makecode),




                new SqlParameter("@Door",Door),
                  new SqlParameter("@Hs_Code",HS_CODE),
                    new SqlParameter("@Drive",DRIVE),
                      new SqlParameter("@Fuel_Type",FUEL_TYPE),
                      new SqlParameter("@Engine_Type",EngineType)
                      

            };


                var response = dbLayer.SP_DataTable_return("Pa_Insert_ModelDesc", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        //Update Model desc
        public string Update_ModelDesc_DAL(int? ModelDesc_ID, int? Make_ID, string ModelDesciption, int? VehCategory_ID, int? MakeCountry_ID, int? Modified_by, string Makecode, string Door, string HS_CODE, string DRIVE, string FUEL_TYPE, string EngineType)
        {

            string msg = "";
            try
            {
                SqlParameter[] paramArray = new SqlParameter[]
            {
                     new SqlParameter("@ModelDesc_ID",ModelDesc_ID),
                     new SqlParameter("@Make_ID",Make_ID),
                    new SqlParameter("@ModelDesciption",ModelDesciption),
                     new SqlParameter("@VehCategory_ID",VehCategory_ID),
                      new SqlParameter("@MakeCountry_ID",MakeCountry_ID),
                       new SqlParameter("@Modified_by",Modified_by),
                        new SqlParameter("@Makecode",Makecode),



                    new SqlParameter("@Door",Door),
                  new SqlParameter("@Hs_Code",HS_CODE),
                    new SqlParameter("@Drive",DRIVE),
                      new SqlParameter("@Fuel_Type",FUEL_TYPE),
                      new SqlParameter("@Engine_Type",EngineType)

            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_ModelDesc", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        //Delete  model desc
        public string Delete_ModelDesc_DAL(int? ModelDesc_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@ModelDesc_ID",ModelDesc_ID)


            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_ModelDesc", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }


        public IEnumerable<Pa_CarLocations_DAL> Get_CarLocation_DAL(int c_ID)
        {
            string sp = "pa_Select_CarLocationList";
            List<Pa_CarLocations_DAL> itemlist = new List<Pa_CarLocations_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_CarLocations_DAL item = new Pa_CarLocations_DAL();

                        item.CarLocation_ID = Convert.ToInt32(rdr["CarLocation_ID"]);
                        item.CarLocation = rdr["CarLocation"].ToString();
                        item.LocType = rdr["LocType"].ToString();
                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        //Insert New Tasks
        public string Insert_CarLocation_DAL(string CarLocation, int? Created_by, string Location_Type_Name, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@CarLocation",CarLocation),

                 new SqlParameter("@Created_by",Created_by),
                 new SqlParameter("@c_ID",c_ID),
                 new SqlParameter("@LocType",Location_Type_Name)





            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_CarLocation", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }



        public string Delete_CarLocation_DAL(int? CarLocation_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@CarLocation_ID",CarLocation_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_CarLocation", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        //Update New Tasks
        public string Update_CarLocation_DAL(int? CarLocation_ID, string CarLocation, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@CarLocation_ID",CarLocation_ID),
                new SqlParameter("@CarLocation",CarLocation),

                 new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_CarLocation", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }




        public IEnumerable<Pa_PortOfExit_DAL> Get_PortOfExit_DAL(int c_ID)
        {
            string sp = "pa_Select_PortOfExitList";
            List<Pa_PortOfExit_DAL> itemlist = new List<Pa_PortOfExit_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_PortOfExit_DAL item = new Pa_PortOfExit_DAL();

                        item.PortOfExit_ID = Convert.ToInt32(rdr["PortOfExit_ID"]);
                        item.PortOfExit = rdr["PortOfExit"].ToString();

                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        //Insert New Tasks
        public string Insert_PortOfExit_DAL(string PortOfExit, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@PortOfExit",PortOfExit),
                new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_PortOfExit", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Delete_PortOfExit_DAL(int? PortOfExit_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@PortOfExit_ID",PortOfExit_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_PortOfExit", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        //Update New Tasks
        public string Update_PortOfExit_DAL(int? PortOfExit_ID, string PortOfExit, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@PortOfExit_ID",PortOfExit_ID),
                new SqlParameter("@PortOfExit",PortOfExit),

                 new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_PortOfExit", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }







        public IEnumerable<Pa_Destinations_DAL> Get_Destinations_DAL(int c_ID)
        {
            string sp = "pa_Select_DestinationsList";
            List<Pa_Destinations_DAL> itemlist = new List<Pa_Destinations_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Destinations_DAL item = new Pa_Destinations_DAL();

                        item.Destinations_ID = Convert.ToInt32(rdr["Destinations_ID"]);
                        item.Destinations = rdr["Destinations"].ToString();

                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        //Insert New Tasks
        public string Insert_Destinations_DAL(string Destinations, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Destinations",Destinations),
                new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_Destinations", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Delete_Destinations_DAL(int? Destinations_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Destinations_ID",Destinations_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_Destinations", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        //Update New Tasks
        public string Update_Destinations_DAL(int? Destinations_ID, string Destinations, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@Destinations_ID",Destinations_ID),
                new SqlParameter("@Destinations",Destinations),

                 new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_Destinations", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }




        public IEnumerable<Pa_Colors_DAL> Get_ColorList_DAL(int c_ID)
        {
            //pa_Select_ColorList

            string sp = "pa_Select_ColorList";
            List<Pa_Colors_DAL> itemlist = new List<Pa_Colors_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Colors_DAL item = new Pa_Colors_DAL();

                        item.Color_ID = Convert.ToInt32(rdr["Color_ID"]);
                        item.ColorName = rdr["ColorName"].ToString();

                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }


        public string Insert_Color_DAL(string ColorName, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@ColorName",ColorName),
                new SqlParameter("@c_ID",c_ID),
                 new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_Color", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Update_Color_DAL(int? Color_ID, string ColorName, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                 new SqlParameter("@Color_ID",Color_ID),
                 new SqlParameter("@ColorName",ColorName),
                 new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_Color", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Delete_Color_DAL(int? Color_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Color_ID",Color_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_Color", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public IEnumerable<Pa_VehicleCategory_DAL> Get_VehicleCategory_DAL(int c_ID)
        {


            string sp = "pa_Select_VehicleCategoryList";
            List<Pa_VehicleCategory_DAL> itemlist = new List<Pa_VehicleCategory_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    //while (rdr.Read())
                    //{
                    //    Pa_VehicleCategory_DAL item = new Pa_VehicleCategory_DAL();

                    //    item.VehCategory_ID = Convert.ToInt32(rdr["VehCategory_ID"]);
                    //    item.VehCategoryName = rdr["VehCategoryName"].ToString();


                    //    itemlist.Add(item);

                    //}
                    //SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_VehicleCategory_DAL item = new Pa_VehicleCategory_DAL();

                        item.VehCategory_ID = Convert.ToInt32(rdr["VehCategory_ID"]);
                        item.VehCategoryName = rdr["VehCategoryName"].ToString();

                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();

                        itemlist.Add(item);

                    }


                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public string Insert_Category_DAL(string VehCategoryName, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@VehCategoryName",VehCategoryName),
                new SqlParameter("@c_ID",c_ID),
                 new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_Category", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Update_Category_DAL(int? VehCategory_ID, string VehCategoryName, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                 new SqlParameter("@VehCategory_ID",VehCategory_ID),
                 new SqlParameter("@VehCategoryName",VehCategoryName),
                 new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_Category", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Delete_Category_DAL(int? VehCategory_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@VehCategory_ID",VehCategory_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_Category", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Insert_GlobalLocation_DAL(string CountryName, int? Loc_TypeID, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@CountryName",CountryName),
                  new SqlParameter("@Loc_TypeID",Loc_TypeID),
                  new SqlParameter("@c_ID",c_ID),
                 new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_GlobalLocation", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Update_Country_DAL(int? Country_ID, string CountryName, int? Loc_TypeID, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                 new SqlParameter("@Country_ID",Country_ID),
                 new SqlParameter("@CountryName",CountryName),
                 new SqlParameter("@Loc_TypeID",Loc_TypeID),
                 new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_Country", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Delete_GlobalLocation_DAL(int? Country_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Country_ID",Country_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_Country", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public IEnumerable<Pa_Year_DAL> Get_Year_List_DAL(int c_ID)
        {
            string sp = "pa_Select_YearList";
            List<Pa_Year_DAL> itemlist = new List<Pa_Year_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Year_DAL item = new Pa_Year_DAL();
                        //HERE DATA READ IS WORKING
                        item.ModelYear_ID = Convert.ToInt32(rdr["ModelYear_ID"]);
                        item.ModelYear = rdr["ModelYear"].ToString();

                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public string Insert_YEAR_DAL(string ModelYear, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@ModelYear",ModelYear),
                new SqlParameter("@c_ID",c_ID),
                 new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_year", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Update_Year_DAL(int? ModelYear_ID, string ModelYear, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                 new SqlParameter("@ModelYear_ID",ModelYear_ID),
                 new SqlParameter("@ModelYear",ModelYear),
                 new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_Year", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Delete_Year_DAL(int? ModelYear_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@ModelYear_ID",ModelYear_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_Year", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        //Get VendorCategory list
        public IEnumerable<Pa_VendorCategory_DAL> Get_VendorCategory_DAL()
        {
            string sp = "pa_Select_VendorCategoryList";
            List<Pa_VendorCategory_DAL> itemlist = new List<Pa_VendorCategory_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_VendorCategory_DAL item = new Pa_VendorCategory_DAL();

                        item.VendorCat_ID = Convert.ToInt32(rdr["VendorCat_ID"]);
                        item.VendorCatName = rdr["VendorCatName"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }




        public IEnumerable<Pa_Partners_DAL> Get_Partners_DAL(int c_ID)
        {
            string sp = "pa_Select_PartnersList";
            List<Pa_Partners_DAL> itemlist = new List<Pa_Partners_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Partners_DAL item = new Pa_Partners_DAL();
                        item.Partner_ID = Convert.ToInt32(rdr["Partner_ID"]);
                        item.Partner_Ref = rdr["Partner_Ref"].ToString();
                        item.PartnerName = rdr["PartnerName"].ToString();
                        item.ContactNo = rdr["ContactNumber"].ToString();
                        item.MobileNo = rdr["MobileNo"].ToString();
                        item.ContactName = rdr["ContactName"].ToString();
                        item.ContactAddress = rdr["ContactAddress"].ToString();
                        item.Email = rdr["Email"].ToString();
                        item.Fax = rdr["Fax"].ToString();
                        item.EmiratesID = rdr["EmiratesID"].ToString();
                        item.TradeLicenseNo = rdr["TradeLicenseNo"].ToString();
                        item.TRN = rdr["TRN"].ToString();




                        item.BalanceType = rdr["BalanceType"].ToString();
                        item.OpeningBalanceDate = rdr["OpeningBalanceDate"].ToString();
                        item.OpeningBalance = rdr["OpeningBalance"].ToString();

                        item.VendorCat_ID = Convert.ToInt32(rdr["VendorCat_ID"]);
                        item.VendorCatName = rdr["VendorCatName"].ToString();
                        item.PartnerType = rdr["PartnerType"].ToString();
                        item.Partner_Sno = rdr["Partner_Sno"].ToString();
                        item.Remarks = rdr["Remarks"].ToString();
                        item.Created_at = rdr["Created_ats"].ToString();
                        item.Created_by = rdr["Created_bys"].ToString();
                        item.Modified_at = rdr["Modified_ats"].ToString();
                        item.Modified_by = rdr["Modified_bys"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        //Insert New Tasks
        public string Insert_Partners_DAL(string PartnerName, string ContactNumber, string MobileNo, string ContactName, string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN, string BalanceType, string OpeningBalanceDate, string OpeningBalance,
 int? VendorCat_ID, string PartnerType, string Remarks, int? Created_by, int c_ID ,string SellerType)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@PartnerName",PartnerName ),
                new SqlParameter("@ContactNumber" ,ContactNumber ),
                new SqlParameter("@MobileNo ",MobileNo ),
                new SqlParameter("@ContactName" ,ContactName ),
                new SqlParameter("@ContactAddress",ContactAddress),
                new SqlParameter("@Email ",Email ),
                new SqlParameter("@Fax ",Fax ),
                new SqlParameter("@EmiratesID" ,EmiratesID ),
                new SqlParameter("@TradeLicenseNo",TradeLicenseNo),
                new SqlParameter("@TRN ",TRN ),
                new SqlParameter("@VendorCat_ID ",VendorCat_ID ),
                new SqlParameter("@PartnerType ",PartnerType ),

                new SqlParameter("@BalanceType",BalanceType),
                new SqlParameter("@OpeningBalanceDate",OpeningBalanceDate),
                new SqlParameter("@OpeningBalance",OpeningBalance),
                
                new SqlParameter("@Remarks",Remarks),
                new SqlParameter("@c_ID",c_ID),
                new SqlParameter("@Created_by",Created_by),
                new SqlParameter("@SellerType",SellerType)



            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_Partners", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Delete_Partners_DAL(int? Partner_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Partner_ID",Partner_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_Partners", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        //Update New Tasks
        public string Update_Partners_DAL(int? Partner_ID, string PartnerName, string ContactNumber, string MobileNo, string ContactName, string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN, string BalanceType, string OpeningBalanceDate, string OpeningBalance,
 int? VendorCat_ID, string Remarks, int? Modified_by,string SellerType)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Partner_ID",Partner_ID),

                new SqlParameter("@PartnerName",PartnerName ),
                new SqlParameter("@ContactNumber" ,ContactNumber ),
                new SqlParameter("@MobileNo ",MobileNo ),
                new SqlParameter("@ContactName" ,ContactName ),
                new SqlParameter("@ContactAddress",ContactAddress),
                new SqlParameter("@Email ",Email ),
                new SqlParameter("@Fax ",Fax ),
                new SqlParameter("@EmiratesID" ,EmiratesID ),
                new SqlParameter("@TradeLicenseNo",TradeLicenseNo),



                new SqlParameter("@TRN ",TRN ),
                new SqlParameter("@VendorCat_ID ",VendorCat_ID ),


                new SqlParameter("@OpeningBalance",OpeningBalance),
                new SqlParameter("@OpeningBalanceDate",OpeningBalanceDate),
                new SqlParameter("@BalanceType",BalanceType),

                new SqlParameter("@Remarks",Remarks),

                new SqlParameter("@Modified_by",Modified_by),
                new SqlParameter("@SellerType",SellerType),



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_Partners", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public IEnumerable<Pa_Partners_DAL> Get_PartnersGarrage_DAL(int c_ID)
        {
            string sp = "pa_Select_PartnersGarrageList";
            List<Pa_Partners_DAL> itemlist = new List<Pa_Partners_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Partners_DAL item = new Pa_Partners_DAL();
                        item.Partner_ID = Convert.ToInt32(rdr["Partner_ID"]);
                        item.Partner_Ref = rdr["Partner_Ref"].ToString();
                        item.PartnerName = rdr["PartnerName"].ToString();
                        item.ContactNo = rdr["ContactNumber"].ToString();
                        item.MobileNo = rdr["MobileNo"].ToString();
                        item.ContactName = rdr["ContactName"].ToString();
                        item.ContactAddress = rdr["ContactAddress"].ToString();
                        item.Email = rdr["Email"].ToString();
                        item.Fax = rdr["Fax"].ToString();
                        item.EmiratesID = rdr["EmiratesID"].ToString();
                        item.TradeLicenseNo = rdr["TradeLicenseNo"].ToString();



                        item.TRN = rdr["TRN"].ToString();


                        item.BalanceType = rdr["BalanceType"].ToString();
                        item.OpeningBalanceDate = rdr["OpeningBalanceDate"].ToString();
                        item.OpeningBalance = rdr["OpeningBalance"].ToString();



                        item.VendorCat_ID = Convert.ToInt32(rdr["VendorCat_ID"]);
                        item.PartnerType = rdr["PartnerType"].ToString();
                        item.Partner_Sno = rdr["Partner_Sno"].ToString();
                        item.Remarks = rdr["Remarks"].ToString();
                        item.Created_at = rdr["Created_ats"].ToString();
                        item.Created_by = rdr["Created_bys"].ToString();
                        item.Modified_at = rdr["Modified_ats"].ToString();
                        item.Modified_by = rdr["Modified_bys"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        public IEnumerable<Pa_Partners_DAL> Get_PartnersSeller_DAL(int c_ID)
        {
            string sp = "pa_Select_PartnersSellerList";
            List<Pa_Partners_DAL> itemlist = new List<Pa_Partners_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Partners_DAL item = new Pa_Partners_DAL();
                        item.Partner_ID = Convert.ToInt32(rdr["Partner_ID"]);
                        item.Partner_Ref = rdr["Partner_Ref"].ToString();
                        item.PartnerName = rdr["PartnerName"].ToString();
                        item.ContactNo = rdr["ContactNumber"].ToString();
                        item.MobileNo = rdr["MobileNo"].ToString();
                        item.ContactName = rdr["ContactName"].ToString();
                        item.ContactAddress = rdr["ContactAddress"].ToString();
                        item.Email = rdr["Email"].ToString();
                        item.Fax = rdr["Fax"].ToString();
                        item.EmiratesID = rdr["EmiratesID"].ToString();
                        item.TradeLicenseNo = rdr["TradeLicenseNo"].ToString();
                        item.TRN = rdr["TRN"].ToString();
                        item.VendorCat_ID = Convert.ToInt32(rdr["VendorCat_ID"]);
                        item.PartnerType = rdr["PartnerType"].ToString();
                        item.Partner_Sno = rdr["Partner_Sno"].ToString();
                        item.Remarks = rdr["Remarks"].ToString();
                        item.Created_at = rdr["Created_ats"].ToString();
                        item.Created_by = rdr["Created_bys"].ToString();
                        item.Modified_at = rdr["Modified_ats"].ToString();
                        item.Modified_by = rdr["Modified_bys"].ToString();
                        item.SellerType = rdr["SellerType"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        public IEnumerable<Pa_Partners_DAL> Get_PartnersAgent_DAL(int c_ID)
        {
            string sp = "pa_Select_PartnersAgentList";
            List<Pa_Partners_DAL> itemlist = new List<Pa_Partners_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Partners_DAL item = new Pa_Partners_DAL();
                        item.Partner_ID = Convert.ToInt32(rdr["Partner_ID"]);
                        item.Partner_Ref = rdr["Partner_Ref"].ToString();
                        item.PartnerName = rdr["PartnerName"].ToString();
                        item.ContactNo = rdr["ContactNumber"].ToString();
                        item.MobileNo = rdr["MobileNo"].ToString();
                        item.ContactName = rdr["ContactName"].ToString();
                        item.ContactAddress = rdr["ContactAddress"].ToString();
                        item.Email = rdr["Email"].ToString();
                        item.Fax = rdr["Fax"].ToString();
                        item.EmiratesID = rdr["EmiratesID"].ToString();
                        item.TradeLicenseNo = rdr["TradeLicenseNo"].ToString();
                        item.TRN = rdr["TRN"].ToString();
                        item.VendorCat_ID = Convert.ToInt32(rdr["VendorCat_ID"]);
                        item.PartnerType = rdr["PartnerType"].ToString();
                        item.Partner_Sno = rdr["Partner_Sno"].ToString();
                        item.Remarks = rdr["Remarks"].ToString();
                        item.Created_at = rdr["Created_ats"].ToString();
                        item.Created_by = rdr["Created_bys"].ToString();
                        item.Modified_at = rdr["Modified_ats"].ToString();
                        item.Modified_by = rdr["Modified_bys"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public IEnumerable<Pa_CustomerCategory_DAL> Get_CustomerCategory_DAL()
        {
            string sp = "pa_Select_CustomerCategoryList";
            List<Pa_CustomerCategory_DAL> itemlist = new List<Pa_CustomerCategory_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_CustomerCategory_DAL item = new Pa_CustomerCategory_DAL();

                        item.CustomerCat_ID = Convert.ToInt32(rdr["CustomerCat_ID"]);
                        item.CustomerCatName = rdr["CustomerCatName"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }


        public IEnumerable<Pa_CustomersMaster_DAL> Get_CustomersList_DAL(int c_ID)
        {
            string sp = "get_CustomersList";
            List<Pa_CustomersMaster_DAL> itemlist = new List<Pa_CustomersMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_CustomersMaster_DAL item = new Pa_CustomersMaster_DAL();
                        item.Customer_ID = Convert.ToInt32(rdr["Customer_ID"]);
                        item.CustomerName = rdr["CustomerName"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public IEnumerable<Pa_CustomersMaster_DAL> Get_CustomersMaster_DAL(int c_ID)
        {
            string sp = "pa_Select_CustomersMasterList";
            List<Pa_CustomersMaster_DAL> itemlist = new List<Pa_CustomersMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_CustomersMaster_DAL item = new Pa_CustomersMaster_DAL();
                        item.Customer_ID = Convert.ToInt32(rdr["Customer_ID"]);
                        item.Customer_Ref = rdr["Customer_Ref"].ToString();
                        item.CustomerName = rdr["CustomerName"].ToString();
                        item.ContactNumber = rdr["ContactNumber"].ToString();
                        item.MobileNo = rdr["MobileNo"].ToString();
                        item.ContactName = rdr["ContactName"].ToString();
                        item.ContactAddress = rdr["ContactAddress"].ToString();
                        item.Email = rdr["Email"].ToString();
                        item.Fax = rdr["Fax"].ToString();
                        item.EmiratesID = rdr["EmiratesID"].ToString();
                        item.TradeLicenseNo = rdr["TradeLicenseNo"].ToString();
                        item.TRN = rdr["TRN"].ToString();

                        item.OpeningBalance = rdr["OpeningBalance"].ToString();
                        item.OpeningBalanceDate = rdr["OpeningBalanceDate"].ToString();
                        item.BalanceType = rdr["BalanceType"].ToString();

                        item.CustomerCat_ID = rdr["CustomerCat_ID"].ToString();
                        item.Remarks = rdr["Remarks"].ToString();
                        item.Created_at = rdr["Created_ats"].ToString();
                        item.Created_by = rdr["Created_bys"].ToString();
                        item.Modified_at = rdr["Modified_ats"].ToString();
                        item.Modified_by = rdr["Modified_bys"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        //Insert New Tasks
        public string Insert_CustomersMaster_DAL(string CustomerName, string ContactNumber, string MobileNo, string ContactName, 
            string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo,
            string TRN, string OpeningBalance,string OpeningBalanceDate,string BalanceType, int? CustomerCat_ID, string Remarks, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@CustomerName",CustomerName ),
                new SqlParameter("@ContactNumber" ,ContactNumber ),
                new SqlParameter("@MobileNo ",MobileNo ),
                new SqlParameter("@ContactName" ,ContactName ),
                new SqlParameter("@ContactAddress",ContactAddress),
                new SqlParameter("@Email ",Email),
                new SqlParameter("@Fax ",Fax),
                new SqlParameter("@EmiratesID" ,EmiratesID),
                new SqlParameter("@TradeLicenseNo",TradeLicenseNo),
                new SqlParameter("@TRN",TRN),

                new SqlParameter("@OpeningBalance",OpeningBalance),
                new SqlParameter("@OpeningBalanceDate",OpeningBalanceDate),
                new SqlParameter("@BalanceType",BalanceType),

                new SqlParameter("@CustomerCat_ID ",CustomerCat_ID),
                new SqlParameter("@c_ID ",c_ID),


                new SqlParameter("@Remarks",Remarks),
                new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_CustomersMaster", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();
                if (msg.Trim().Contains("Violation of UNIQUE KEY"))
                {
                    msg = "Customer Already Exist!";
                }

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Delete_CustomersMaster_DAL(int? Customer_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Customer_ID",Customer_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_CustomersMaster", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        //Update New Tasks
        public string Update_CustomersMaster_DAL(int? Customer_ID, string CustomerName, string ContactNumber, string MobileNo, 
            string ContactName, string ContactAddress, string Email, string Fax, string EmiratesID, string TradeLicenseNo, string TRN,
            string OpeningBalance, string OpeningBalanceDate, string BalanceType,
            int? CustomerCat_ID, string Remarks, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Customer_ID",Customer_ID),

                new SqlParameter("@CustomerName",CustomerName ),
                new SqlParameter("@ContactNumber" ,ContactNumber ),
                new SqlParameter("@MobileNo ",MobileNo ),
                new SqlParameter("@ContactName" ,ContactName ),
                new SqlParameter("@ContactAddress",ContactAddress),
                new SqlParameter("@Email ",Email ),
                new SqlParameter("@Fax ",Fax ),
                new SqlParameter("@EmiratesID" ,EmiratesID ),
                new SqlParameter("@TradeLicenseNo",TradeLicenseNo),
                new SqlParameter("@TRN",TRN),
                  new SqlParameter("@OpeningBalance",OpeningBalance),
                new SqlParameter("@OpeningBalanceDate",OpeningBalanceDate),
                new SqlParameter("@BalanceType",BalanceType),
                new SqlParameter("@CustomerCat_ID",CustomerCat_ID ),

                new SqlParameter("@Remarks",Remarks),

                new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_CustomersMaster", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public IEnumerable<Pa_ShowRoom_DAL> Get_ShowRoom_DAL(int c_ID)
        {
            string sp = "pa_Select_ShowRoomList";
            List<Pa_ShowRoom_DAL> itemlist = new List<Pa_ShowRoom_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_ShowRoom_DAL item = new Pa_ShowRoom_DAL();

                        item.ShowRoom_ID = Convert.ToInt32(rdr["ShowRoom_ID"]);
                        item.ShowRoomCode = rdr["ShowRoomCode"].ToString();
                        item.ShowRoomName = rdr["ShowRoomName"].ToString();
                        item.showroom_no = rdr["showroom_no"].ToString();
                        item.TRN = rdr["TRN"].ToString();

                        //item.Created_at = rdr["Created_at"].ToString();
                        //item.Created_by = rdr["Created_by"].ToString();
                        //item.Modified_at = rdr["Modified_at"].ToString();
                        //item.Modified_by = rdr["Modified_by"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        //Insert New Tasks
        public string Insert_ShowRoom_DAL(string ShowRoomCode, string ShowRoomName, string showroom_no, string TRN, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@ShowRoomCode",ShowRoomCode),
                new SqlParameter("@ShowRoomName",ShowRoomName),
                new SqlParameter("@showroom_no",showroom_no),
                new SqlParameter("@c_ID",c_ID),

                new SqlParameter("@TRN",TRN)





            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_ShowRoom", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }



        //Update New Tasks
        public string Update_ShowRoom_DAL(int? ShowRoom_ID, string ShowRoomCode, string ShowRoomName, string showroom_no, string TRN, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@ShowRoom_ID",ShowRoom_ID),
                new SqlParameter("@ShowRoomCode",ShowRoomCode),
                new SqlParameter("@ShowRoomName",ShowRoomName),
                new SqlParameter("@showroom_no",showroom_no),
                new SqlParameter("@TRN",TRN),





            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_ShowRoom", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        //Get accounts list
        public IEnumerable<Accounts_DAL> Select_PV_OnAccounts_DAL(int c_ID)
        {
            string sp = "Select_PV_OnAccounts";
            List<Accounts_DAL> itemlist = new List<Accounts_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Accounts_DAL item = new Accounts_DAL();
                        item.Sys_AC_type_ID = Convert.ToInt32(rdr["Sys_AC_type_ID"]);

                        item.Account_ID = Convert.ToInt32(rdr["account_ID"]);

                        item.AccountName = rdr["AccountName"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public IEnumerable<Accounts_DAL> Select_Expense_Accounts_DAL(int c_ID)
        {
            string sp = "Select_Expense_Accounts";
            List<Accounts_DAL> itemlist = new List<Accounts_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Accounts_DAL item = new Accounts_DAL();
                        item.Sys_AC_type_ID = Convert.ToInt32(rdr["Sys_AC_type_ID"]);

                        item.Account_ID = Convert.ToInt32(rdr["account_ID"]);

                        item.AccountName = rdr["AccountName"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public IEnumerable<Accounts_DAL> Select_RV_OnAccounts_DAL(int c_ID)
        {
            string sp = "Select_RV_OnAccounts";
            List<Accounts_DAL> itemlist = new List<Accounts_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Accounts_DAL item = new Accounts_DAL();

                        item.Account_ID = Convert.ToInt32(rdr["account_ID"]);

                        item.AccountName = rdr["AccountName"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        //Get PV_PayVia list
        public IEnumerable<Accounts_DAL> Select_PV_PayVia_DAL(int c_ID)
        {
            string sp = "Select_PV_PayVia";
            List<Accounts_DAL> itemlist = new List<Accounts_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Accounts_DAL item = new Accounts_DAL();

                        item.Account_ID = Convert.ToInt32(rdr["account_ID"]);

                        item.AccountName = rdr["AccountName"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public IEnumerable<AdminRights> Get_AdminRolesByAdminUserID_DAL(int? User_ID)
        {

            string sp = "pa_select_AdminRoles_ByAdminID";


            List<AdminRights> itemlist = new List<AdminRights>();
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@User_ID", User_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        AdminRights item = new AdminRights();
                        if (!string.IsNullOrEmpty(rdr["MView"].ToString())) { item.View = Convert.ToBoolean(rdr["MView"].ToString()); }
                        else { item.View = false; }
                        if (!string.IsNullOrEmpty(rdr["MAdd"].ToString())) { item.Add = Convert.ToBoolean(rdr["MAdd"].ToString()); }
                        else { item.Add = false; }
                        if (!string.IsNullOrEmpty(rdr["MEdit"].ToString())) { item.Edit = Convert.ToBoolean(rdr["MEdit"].ToString()); }
                        else { item.Edit = false; }
                        if (!string.IsNullOrEmpty(rdr["MDelete"].ToString())) { item.Delete = Convert.ToBoolean(rdr["MDelete"].ToString()); }
                        else { item.Delete = false; }
                        if (!string.IsNullOrEmpty(rdr["MPrint"].ToString())) { item.Print = Convert.ToBoolean(rdr["MPrint"].ToString()); }
                        else { item.Print = false; }
                        if (!string.IsNullOrEmpty(rdr["ExportToExcel"].ToString())) { item.Excel = Convert.ToBoolean(rdr["ExportToExcel"].ToString()); }
                        else { item.Excel = false; }
                        if (!string.IsNullOrEmpty(rdr["IsAdmin"].ToString())) { item.IsAdmin = Convert.ToBoolean(rdr["IsAdmin"].ToString()); }
                        else { item.IsAdmin = false; }
                        item.RoleName = rdr["UserName"].ToString();
                        item.Menu = rdr["MainMenuName"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }



            }
            catch
            {

            }

            return itemlist;

        }


        //Get customer master attachments
        public IEnumerable<Pa_Attachments_DAL> GetCustomerMaster_Attachments_DAL(int? Customer_ID, string Type)
        {


            string sp = "Select_Attachments_List";
            List<Pa_Attachments_DAL> itemlist = new List<Pa_Attachments_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Master_ID", Customer_ID);
                    cmd.Parameters.AddWithValue("@Type", Type);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Attachments_DAL item = new Pa_Attachments_DAL();

                        item.Attachment_ID = Convert.ToInt32(rdr["Attachment_ID"]);
                        item.Master_ID = Convert.ToInt32(rdr["Master_ID"]);
                        item.File_Name = rdr["File_Name"].ToString();
                        item.File_Path = rdr["File_Path"].ToString();
                        item.Document_Type = rdr["Document_Type"].ToString();
                        item.Type = rdr["Type"].ToString();
                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        //Insert customer master attachment
        public string InsertAttachments_CustomerMaster_DAL(int? Customer_ID, string Document_Type, string Type, string newFileName, string filepath, int? Created_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Master_ID",Customer_ID),
                new SqlParameter("@Document_Type",Document_Type),
                new SqlParameter("@Type",Type),
                  new SqlParameter("@newFileName",newFileName),
                    new SqlParameter("@filepath",filepath),
                 new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Insert_Attachments_VouchersDocuments", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }


        //Get vendor master attachments
        public IEnumerable<Pa_Attachments_DAL> GetVendorMaster_Attachments_DAL(int? Partner_ID, string Type)
        {


            string sp = "Select_Attachments_List";
            List<Pa_Attachments_DAL> itemlist = new List<Pa_Attachments_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Master_ID", Partner_ID);
                    cmd.Parameters.AddWithValue("@Type", Type);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Attachments_DAL item = new Pa_Attachments_DAL();

                        item.Attachment_ID = Convert.ToInt32(rdr["Attachment_ID"]);
                        item.Master_ID = Convert.ToInt32(rdr["Master_ID"]);
                        item.File_Name = rdr["File_Name"].ToString();
                        item.File_Path = rdr["File_Path"].ToString();
                        item.Document_Type = rdr["Document_Type"].ToString();
                        item.Type = rdr["Type"].ToString();
                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        //Insert vendor master attachment
        public string InsertAttachments_VendorMaster_DAL(int? Partner_ID, string Document_Type, string Type, string newFileName, string filepath, int? Created_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Master_ID",Partner_ID),
                new SqlParameter("@Document_Type",Document_Type),
                new SqlParameter("@Type",Type),
                  new SqlParameter("@newFileName",newFileName),
                    new SqlParameter("@filepath",filepath),
                 new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Insert_Attachments_VouchersDocuments", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }




        //Get showroom attachments
        public IEnumerable<Pa_Attachments_DAL> GetShowroom_Attachments_DAL(int? ShowRoom_ID, string Type)
        {


            string sp = "Select_Attachments_List";
            List<Pa_Attachments_DAL> itemlist = new List<Pa_Attachments_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Master_ID", ShowRoom_ID);
                    cmd.Parameters.AddWithValue("@Type", Type);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Attachments_DAL item = new Pa_Attachments_DAL();

                        item.Attachment_ID = Convert.ToInt32(rdr["Attachment_ID"]);
                        item.Master_ID = Convert.ToInt32(rdr["Master_ID"]);
                        item.File_Name = rdr["File_Name"].ToString();
                        item.File_Path = rdr["File_Path"].ToString();
                        item.Document_Type = rdr["Document_Type"].ToString();
                        item.Type = rdr["Type"].ToString();
                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        //Insert showroom attachment
        public string InsertAttachments_Showroom_DAL(int? ShowRoom_ID, string Document_Type, string Type, string newFileName, string filepath, int? Created_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Master_ID",ShowRoom_ID),
                new SqlParameter("@Document_Type",Document_Type),
                new SqlParameter("@Type",Type),
                  new SqlParameter("@newFileName",newFileName),
                    new SqlParameter("@filepath",filepath),
                 new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Insert_Attachments_VouchersDocuments", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        //---get shipping companies list
        public IEnumerable<Pa_Partners_DAL> Get_ShippingCompaniesList_DAL(int c_ID)
        {
            string sp = "pa_Select_PartnersShippingList";
            List<Pa_Partners_DAL> itemlist = new List<Pa_Partners_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Partners_DAL item = new Pa_Partners_DAL();
                        item.Partner_ID = Convert.ToInt32(rdr["Partner_ID"]);
                        item.Partner_Ref = rdr["Partner_Ref"].ToString();
                        item.PartnerName = rdr["PartnerName"].ToString();
                        item.ContactNo = rdr["ContactNumber"].ToString();
                        item.MobileNo = rdr["MobileNo"].ToString();
                        item.ContactName = rdr["ContactName"].ToString();
                        item.ContactAddress = rdr["ContactAddress"].ToString();
                        item.Email = rdr["Email"].ToString();
                        item.Fax = rdr["Fax"].ToString();
                        item.EmiratesID = rdr["EmiratesID"].ToString();
                        item.TradeLicenseNo = rdr["TradeLicenseNo"].ToString();



                        item.TRN = rdr["TRN"].ToString();


                        item.BalanceType = rdr["BalanceType"].ToString();
                        item.OpeningBalanceDate = rdr["OpeningBalanceDate"].ToString();
                        item.OpeningBalance = rdr["OpeningBalance"].ToString();


                        item.VendorCat_ID = Convert.ToInt32(rdr["VendorCat_ID"]);
                        item.PartnerType = rdr["PartnerType"].ToString();
                        item.Partner_Sno = rdr["Partner_Sno"].ToString();
                        item.Remarks = rdr["Remarks"].ToString();
                        item.Created_at = rdr["Created_ats"].ToString();
                        item.Created_by = rdr["Created_bys"].ToString();
                        item.Modified_at = rdr["Modified_ats"].ToString();
                        item.Modified_by = rdr["Modified_bys"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        //---get export companies list from partner table
        public IEnumerable<Pa_Partners_DAL> Get_ExportCompaniesList_DAL(int c_ID)
        {
            string sp = "Select_Partners_ExportCompanies";
            List<Pa_Partners_DAL> itemlist = new List<Pa_Partners_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Partners_DAL item = new Pa_Partners_DAL();
                        item.Partner_ID = Convert.ToInt32(rdr["Partner_ID"]);
                        item.Partner_Ref = rdr["Partner_Ref"].ToString();
                        item.PartnerName = rdr["PartnerName"].ToString();
                        item.ContactNo = rdr["ContactNumber"].ToString();
                        item.MobileNo = rdr["MobileNo"].ToString();
                        item.ContactName = rdr["ContactName"].ToString();
                        item.ContactAddress = rdr["ContactAddress"].ToString();
                        item.Email = rdr["Email"].ToString();
                        item.Fax = rdr["Fax"].ToString();
                        item.EmiratesID = rdr["EmiratesID"].ToString();
                        item.TradeLicenseNo = rdr["TradeLicenseNo"].ToString();
                        item.TRN = rdr["TRN"].ToString();
                        item.VendorCat_ID = Convert.ToInt32(rdr["VendorCat_ID"]);
                        item.PartnerType = rdr["PartnerType"].ToString();
                        item.Partner_Sno = rdr["Partner_Sno"].ToString();
                        item.Remarks = rdr["Remarks"].ToString();
                        item.Created_at = rdr["Created_ats"].ToString();
                        item.Created_by = rdr["Created_bys"].ToString();
                        item.Modified_at = rdr["Modified_ats"].ToString();
                        item.Modified_by = rdr["Modified_bys"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }



        //---get Basic Banks List


      
        public string Delete_BasicBank_DAL(int? Bank_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Bank_ID",Bank_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_BasicBank", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

     
        public string InsertCurrencyMaster_DAL(string Currency_Name, string Currency_ShortName, double? Curr_Rate, string Minor_ShortName, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Currency_Name",Currency_Name),
                  new SqlParameter("@Currency_ShortName",Currency_ShortName),
                    new SqlParameter("@Curr_Rate",Curr_Rate),
                     new SqlParameter("@Minor_ShortName",Minor_ShortName),
                     new SqlParameter("@c_ID",c_ID),
                      new SqlParameter("@Created_by",Created_by)


            };


                var response = dbLayer.SP_DataTable_return("Insert_CurrencyMaster", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }


        public string UpdateCurrencyMaster_DAL(int? Currency_ID, string Currency_Name, string Currency_ShortName, double? Curr_Rate, string Minor_ShortName, int? Modified_at)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@Currency_ID",Currency_ID),
                new SqlParameter("@Currency_Name",Currency_Name),
                  new SqlParameter("@Currency_ShortName",Currency_ShortName),
                    new SqlParameter("@Curr_Rate",Curr_Rate),
                    new SqlParameter("@Minor_ShortName",Minor_ShortName),
                      new SqlParameter("@Modified_at",Modified_at)


            };


                var response = dbLayer.SP_DataTable_return("Update_CurrencyMaster", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string DeleteCurrencyMaster_DAL(int? Currency_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Currency_ID",Currency_ID)


            };


                var response = dbLayer.SP_DataTable_return("Delete_CurrencyMaster", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public Pa_CustomersMaster_DAL GetCustomerDetail_DAL(int? Customer_ID)
        {


            string sp = "GetCustomerDetail";
            Pa_CustomersMaster_DAL Cust = new Pa_CustomersMaster_DAL();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Customer_ID", Customer_ID);
                    SqlDataReader record = cmd.ExecuteReader();


                    if (record.Read())
                    {

                        Cust.CustomerName = record["CustomerName"].ToString();
                        Cust.Customer_ID = Convert.ToInt32(record["Customer_ID"].ToString());
                        Cust.ContactNumber = record["ContactNumber"].ToString();
                        Cust.ContactAddress = record["ContactAddress"].ToString();
                        Cust.TRN = record["TRN"].ToString();

                    }

                    con.Close();

                }



            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }

            return Cust;
        }
        public Pa_Partners_DAL GetVendorDetail_DAL(int? Vendor_ID)
        {


            string sp = "GetVendorDetail";
            Pa_Partners_DAL Cust = new Pa_Partners_DAL();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                    SqlDataReader record = cmd.ExecuteReader();


                    if (record.Read())
                    {

                        Cust.PartnerName = record["PartnerName"].ToString();
                        Cust.Partner_ID = Convert.ToInt32(record["Partner_ID"].ToString());
                        Cust.ContactNo = record["ContactNumber"].ToString();
                        Cust.ContactAddress = record["ContactAddress"].ToString();
   

                    }

                    con.Close();

                }



            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }

            return Cust;
        }
        public IEnumerable<Pa_Countries_DAL> Get_LocType_DAL()
        {


            string sp = "pa_Select_LocationType";
            List<Pa_Countries_DAL> itemlist = new List<Pa_Countries_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Countries_DAL item = new Pa_Countries_DAL();

                        item.loctype_ID = Convert.ToInt32(rdr["loctype_ID"]);
                        item.LocTypeName = rdr["LocTypeName"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        //Faraz New Work




        public IEnumerable<Pa_Inventry_DAL> Get_InventryMasterList_DAL()
        {
            //pa_Select_InventryList

            string sp = "pa_Select_InventryMasterList";
            List<Pa_Inventry_DAL> itemlist = new List<Pa_Inventry_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Inventry_DAL item = new Pa_Inventry_DAL();

                        item.Item_ID = Convert.ToInt32(rdr["Item_ID"]);
                        item.ItemCode = rdr["ItemCode"].ToString();
                        item.ItemName = rdr["ItemName"].ToString();

                        item.ItemDescription = rdr["ItemDescription"].ToString();

                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();

                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();

                        item.Purchase_Qty = rdr["Purchase_Qty"].ToString();
                        item.Sold_Qty = rdr["Sold_Qty"].ToString();
                        item.InHand_Qty = rdr["InHand_Qty"].ToString();


                        item.IsSerializable = rdr["IsSerializable"].ToString();
                        item.BarCode = rdr["BarCode"].ToString();
                        item.UnitPrice = rdr["Unit_Price"].ToString();
                        item.SalePrice = rdr["Sale_Price"].ToString();
                        item.CostMethod = rdr["CostMethod"].ToString();
                        item.Comment = rdr["Comment"].ToString();
                        item.UOM = rdr["UOM"].ToString();
                        item.Group_ID = Convert.ToInt32(rdr["Group_ID"]);
                        item.Category_ID = Convert.ToInt32(rdr["Category_ID"]);
                        item.GroupName = rdr["Group_Name"].ToString();
                        item.CategoryName = rdr["Category_Name"].ToString();
                        item.Traditional = rdr["Traditional"].ToString();
                        item.FUEL_TYPE = rdr["FUEL_TYPE"].ToString();
                        item.Transmission = rdr["Transmission"].ToString();
                        item.Drive = rdr["Drive"].ToString();
                        item.StartYear = rdr["StartYear"].ToString();
                        item.EndYear = rdr["EndYear"].ToString();
                        item.EngineSpecsCode = rdr["EngineSpecsCode"].ToString();
                        item.Year = rdr["Years"].ToString();
                        item.Received_Qty = rdr["Received_Qty"].ToString();
                        item.Delivered_Qty = rdr["Delivered_Qty"].ToString();
                        item.Return_Qty = rdr["Return_Qty"].ToString();
                        item.OpeningBal = rdr["OpeningBal"].ToString();
                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public string Insert_InventryMaster_DAL(string ItemCode, string ItemName, string ItemDescription, string IsSerializable, string BarCode, string UnitPrice, string SalePrice, string CostMethod, string Comment, string UOM, int Group_ID, int Category_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@ItemCode",ItemCode),
                 new SqlParameter("@ItemName",ItemName),
                 new SqlParameter("@ItemDescription",ItemDescription),
                 new SqlParameter("@IsSerializable",IsSerializable),
                 new SqlParameter("@BarCode",BarCode),
                 new SqlParameter("@Unit_Price",UnitPrice),
                 new SqlParameter("@Sale_Price",SalePrice),
                 new SqlParameter("@Cost_Method",CostMethod),
                  new SqlParameter("@Comments",Comment),
                  new SqlParameter("@Group_ID",Group_ID),
                  new SqlParameter("@Category_ID",Category_ID),
                  new SqlParameter("@UOM",UOM)




            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_InventryMater", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Update_InventryMaster_DAL(int? Item_ID, string ItemCode, string ItemName, string ItemDescription, string IsSerializable, string BarCode, string UnitPrice, string SalePrice, string CostMethod, string Comment, string UOM, int Group_ID, int Category_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Item_ID",Item_ID),
                new SqlParameter("@ItemCode",ItemCode),
                 new SqlParameter("@ItemName",ItemName),
                 new SqlParameter("@ItemDescription",ItemDescription),
                 new SqlParameter("@IsSerializable",IsSerializable),
                 new SqlParameter("@BarCode",BarCode),
                 new SqlParameter("@Unit_Price",UnitPrice),
                 new SqlParameter("@Sale_Price",SalePrice),
                 new SqlParameter("@Cost_Method",CostMethod),
                  new SqlParameter("@Comments",Comment),
                     new SqlParameter("@Group_ID",Group_ID),
                  new SqlParameter("@Category_ID",Category_ID),
                                    new SqlParameter("@UOM",UOM)




            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_InventryMater", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public IEnumerable<Pa_Inventry_DAL> Get_UOM_DAL(int c_ID)
        {


            string sp = "pa_Select_UOM";
            List<Pa_Inventry_DAL> itemlist = new List<Pa_Inventry_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Inventry_DAL item = new Pa_Inventry_DAL();

                        item.UOM_ID = Convert.ToInt32(rdr["UOM_ID"]);
                        item.UOM = rdr["UOM"].ToString();



                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        public Pa_Inventry_DAL GetItemDetail_DAL(string Item)
        {


            string sp = "GetItemDetail";
            Pa_Inventry_DAL Cust = new Pa_Inventry_DAL();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Item", Item);
                    SqlDataReader record = cmd.ExecuteReader();


                    if (record.Read())
                    {

                        Cust.ItemDescription = record["ItemDescription"].ToString();
                        Cust.ItemCode = record["ItemCode"].ToString();
                        Cust.UnitPrice = record["Unit_Price"].ToString();
                        Cust.SalePrice = record["Sale_Price"].ToString();
                        Cust.UOM = record["UOM"].ToString();
                        Cust.location_ID = Convert.ToInt32(record["location_ID"]);


                    }

                    con.Close();

                }



            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }

            return Cust;
        }
        public IEnumerable<Pa_UOM_DAL> UOMList { get; set; }
        public IEnumerable<Pa_UOM_DAL> Get_UOMList_DAL(int c_ID)
        {
            //pa_Select_UOMList

            string sp = "pa_Select_UOMList";
            List<Pa_UOM_DAL> itemlist = new List<Pa_UOM_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_UOM_DAL item = new Pa_UOM_DAL();

                        item.UOM_ID = Convert.ToInt32(rdr["UOM_ID"]);
                        item.UOM = rdr["UOM"].ToString();

                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }


        public string Insert_UOM_DAL(string UOM, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@UOM",UOM),
                new SqlParameter("@c_ID",c_ID),
                 new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_UOM", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Update_UOM_DAL(int? UOM_ID, string UOM, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                 new SqlParameter("@UOM_ID",UOM_ID),
                 new SqlParameter("@UOM",UOM),
                 new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_UOM", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Delete_UOM_DAL(int? UOM_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@UOM_ID",UOM_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_UOM", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }


        public IEnumerable<Pa_ItemGroups_DAL> Get_ItemGroupList_DAL(int c_ID)
        {
            //pa_Select_ItemGroupList

            string sp = "pa_Select_GroupList";
            List<Pa_ItemGroups_DAL> itemlist = new List<Pa_ItemGroups_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_ItemGroups_DAL item = new Pa_ItemGroups_DAL();

                        item.Group_ID = Convert.ToInt32(rdr["Group_ID"]);
                        item.Group_Name = rdr["Group_Name"].ToString();
                        item.Group_Code = rdr["Group_Code"].ToString();

                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }


        public string Insert_ItemGroup_DAL(string Group_Code, string Group_Name, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Group_Name",Group_Name),
                new SqlParameter("@Group_Code",Group_Code),
                new SqlParameter("@c_ID",c_ID),
                 new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_Group", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Update_ItemGroup_DAL(int? Group_ID, string Group_Code, string Group_Name, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                 new SqlParameter("@Group_ID",Group_ID),

                new SqlParameter("@Group_Name",Group_Name),
                new SqlParameter("@Group_Code",Group_Code),
                 new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_Group", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Delete_ItemGroup_DAL(int? Group_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Group_ID",Group_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_Group", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public IEnumerable<Pa_ItemCategory_DAL> Get_ItemCategoryList_DAL(int c_ID)
        {
            //pa_Select_ItemCategoryList

            string sp = "pa_Select_CategoryList";
            List<Pa_ItemCategory_DAL> itemlist = new List<Pa_ItemCategory_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_ItemCategory_DAL item = new Pa_ItemCategory_DAL();

                        item.Category_ID = Convert.ToInt32(rdr["Category_ID"]);
                        item.Category_Name = rdr["Category_Name"].ToString();
                        item.Category_Code = rdr["Category_Code"].ToString();

                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }


        public string Insert_ItemCategory_DAL(string Category_Code, string Category_Name, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Category_Name",Category_Name),
                new SqlParameter("@Category_Code",Category_Code),
                new SqlParameter("@c_ID",c_ID),
                 new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_ItemCategory", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Update_ItemCategory_DAL(int? Category_ID, string Category_Code, string Category_Name, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                 new SqlParameter("@Category_ID",Category_ID),
                 new SqlParameter("@Category_Name",Category_Name),
                                 new SqlParameter("@Category_Code",Category_Code),
                 new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_ItemCategory", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Delete_ItemCategory_DAL(int? Category_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Category_ID",Category_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_ItemCategory", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }



        public IEnumerable<Pa_ItemGroups_DAL> Get_ItemGroup_DAL(int c_ID)
        {


            string sp = "pa_Select_ItemGroup";
            List<Pa_ItemGroups_DAL> itemlist = new List<Pa_ItemGroups_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_ItemGroups_DAL item = new Pa_ItemGroups_DAL();

                        item.Group_ID = Convert.ToInt32(rdr["Group_ID"]);
                        item.Group_Name = rdr["Group_Name"].ToString();



                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public IEnumerable<Pa_ItemCategory_DAL> Get_ItemCategory_DAL(int c_ID)
        {


            string sp = "pa_Select_ItemCategory";
            List<Pa_ItemCategory_DAL> itemlist = new List<Pa_ItemCategory_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_ItemCategory_DAL item = new Pa_ItemCategory_DAL();

                        item.Category_ID = Convert.ToInt32(rdr["Category_ID"]);
                        item.Category_Name = rdr["Category_Name"].ToString();



                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        public string Insert_InventryPartsMaster_DAL(string ItemCode, string ItemName, string ItemDescription, string IsSerializable, string BarCode, string UnitPrice, string SalePrice, string CostMethod, string Comment, string UOM, int Group_ID, int Category_ID, string Traditional, string FUEL_TYPE, string Transmission, string Drive, string StartYear, string EndYear, string OpeningBal)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@ItemCode",ItemCode),
                 new SqlParameter("@ItemName",ItemName),
                 new SqlParameter("@ItemDescription",ItemDescription),
                 new SqlParameter("@IsSerializable",IsSerializable),
                 new SqlParameter("@BarCode",BarCode),
                 new SqlParameter("@Unit_Price",UnitPrice),
                 new SqlParameter("@Sale_Price",SalePrice),
                 new SqlParameter("@Cost_Method",CostMethod),
                  new SqlParameter("@Comments",Comment),
                  new SqlParameter("@Group_ID",Group_ID),
                  new SqlParameter("@Category_ID",Category_ID),
                  new SqlParameter("@Traditional",Traditional),
                  new SqlParameter("@FUEL_TYPE",FUEL_TYPE),
                  new SqlParameter("@Transmission",Transmission),
                  new SqlParameter("@Drive",Drive),
                  new SqlParameter("@StartYear",StartYear),
                  new SqlParameter("@EndYear",EndYear),
                  new SqlParameter("@OpeningBal", OpeningBal),
                  new SqlParameter("@UOM",UOM)




            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_InventryPartsMater", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        #region MultipleMakes
        public string Insert_InventryPartsMultipleMake_DAL(int Item_ID, int make_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Item_ID",Item_ID),
                 new SqlParameter("@make_ID",make_ID)





            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_MultipleMake", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public IEnumerable<Pa_Inventry_DAL> Get_MultipleMakeList_DAL(int Item_ID)
        {
            //pa_Select_InventryList

            string sp = "pa_Select_MultipleMake";
            List<Pa_Inventry_DAL> itemlist = new List<Pa_Inventry_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Item_ID", Item_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Inventry_DAL item = new Pa_Inventry_DAL();

                        item.Item_ID = Convert.ToInt32(rdr["Item_ID"]);
                        item.make_ID = Convert.ToInt32(rdr["make_ID"]);

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public string Delete_MultipleMake_DAL(int? Item_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Item_ID",Item_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_MultipleMake", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Update_InventryPartsMultipleMake_DAL(int? Item_ID, int make_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Item_ID",Item_ID),
                new SqlParameter("@make_ID",make_ID)





            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_InventryPartsMater", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        #endregion

        #region MultipleEngineSpecsCode


        public IEnumerable<EngineSpecsCode_DAL> EngineSpecsCodeList { get; set; }

        public IEnumerable<Pa_Inventry_DAL> MultipleEngineSpecsCodeList { get; set; }

        public IEnumerable<EngineSpecsCode_DAL> Get_EngineSpecsCodeList_DAL(int c_ID)
        {
            //pa_Select_InventryList

            string sp = "Select_EngineSpecsCodeList";
            List<EngineSpecsCode_DAL> itemlist = new List<EngineSpecsCode_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID",c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        EngineSpecsCode_DAL item = new EngineSpecsCode_DAL();

                        item.EngineSpecsCode_ID = Convert.ToInt32(rdr["EngineSpecsCode_ID"]);
                        item.EngineSpecsCode = rdr["EngineSpecsCode"].ToString();
                        item.SpecsDescription = rdr["SpecsDescription"].ToString();
                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public IEnumerable<Pa_Inventry_DAL> Get_MultipleEngineSpecsCodeList_DAL(int Item_ID)
        {
            //pa_Select_InventryList

            string sp = "Select_MultipleEngineSpecsCode";
            List<Pa_Inventry_DAL> itemlist = new List<Pa_Inventry_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Item_ID", Item_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Inventry_DAL item = new Pa_Inventry_DAL();

                        item.Item_ID = Convert.ToInt32(rdr["Item_ID"]);
                        item.enginespecscode_ID = Convert.ToInt32(rdr["EngineSpecsCode_ID"]);
                        item.EngineSpecsCode = rdr["EngineSpecsCode"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public string Insert_MultipleEngineSpecsCode_DAL(int Item_ID, int EngineSpecsCode_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Item_ID",Item_ID),
                 new SqlParameter("@EngineSpecsCode_ID",EngineSpecsCode_ID)
            };


                var response = dbLayer.SP_DataTable_return("Insert_MultipleEngineSpecsCode", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Insert_EngineSpecsCode_DAL(string EngineSpecsCode, string SpecsDescription,int Created_by,int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@EngineSpecsCode",EngineSpecsCode),
                 new SqlParameter("@SpecsDescription",SpecsDescription),
                 new SqlParameter("@Created_by",Created_by),
                 new SqlParameter("@c_ID",c_ID)

            };


                var response = dbLayer.SP_DataTable_return("Insert_EngineSpecsCode", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Update_EngineSpecsCode_DAL(int EngineSpecsCode_ID,string EngineSpecsCode, string SpecsDescription, int Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@EngineSpecsCode_ID",EngineSpecsCode_ID),
                new SqlParameter("@EngineSpecsCode",EngineSpecsCode),
                new SqlParameter("@SpecsDescription",SpecsDescription),
                new SqlParameter("@Modified_by",Modified_by)

            };


                var response = dbLayer.SP_DataTable_return("Update_EngineSpecsCode", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Delete_EngineSpecsCode_DAL(int? Item_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@EngineSpecsCode_ID",Item_ID)
            };

                var response = dbLayer.SP_DataTable_return("Delete_EngineSpecsCode", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Delete_MultipleEngineSpecsCode_DAL(int Item_ID)
        {

            string msg = "";
            try
            {
                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Item_ID",Item_ID)
            };

                var response = dbLayer.SP_DataTable_return("Delete_MultipleEngineSpecsCode", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();
                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                msg = ErrorMessage;
                return msg;
            }

        }



        #endregion


        public string Insert_InventryPartsMultipleYear_DAL(int Item_ID, string Year)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Item_ID",Item_ID),
                 new SqlParameter("@Year",Year)





            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_MultipleYear", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }



        public string Update_InventryPartsMaster_DAL(int? Item_ID, string ItemCode, string ItemName, string ItemDescription, string IsSerializable, string BarCode, string UnitPrice, string SalePrice, string CostMethod, string Comment, string UOM, int Group_ID, int Category_ID, string Traditional, string FUEL_TYPE, string Transmission, string Drive, string StartYear, string EndYear, string OpeningBal)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Item_ID",Item_ID),
                new SqlParameter("@ItemCode",ItemCode),
                 new SqlParameter("@ItemName",ItemName),
                 new SqlParameter("@ItemDescription",ItemDescription),
                 new SqlParameter("@IsSerializable",IsSerializable),
                 new SqlParameter("@BarCode",BarCode),
                 new SqlParameter("@Unit_Price",UnitPrice),
                 new SqlParameter("@Sale_Price",SalePrice),
                 new SqlParameter("@Cost_Method",CostMethod),
                  new SqlParameter("@Comments",Comment),
                     new SqlParameter("@Group_ID",Group_ID),
                  new SqlParameter("@Category_ID",Category_ID),
                       new SqlParameter("@Traditional",Traditional),
                  new SqlParameter("@FUEL_TYPE",FUEL_TYPE),
                  new SqlParameter("@Transmission",Transmission),
                  new SqlParameter("@Drive",Drive),
                       new SqlParameter("@StartYear",StartYear),
                  new SqlParameter("@EndYear",EndYear),
                              new SqlParameter("@OpeningBal", OpeningBal),
                                    new SqlParameter("@UOM",UOM)




            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_InventryPartsMater", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
      
        public IEnumerable<Make_DAL> Get_MakeModelList_DAL(int c_ID)
        {
            string sp = "pa_Select_MakeModelList";
            List<Make_DAL> itemlist = new List<Make_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Make_DAL item = new Make_DAL();

                        item.Make_ID = Convert.ToInt32(rdr["Make_ID"]);
                        item.Make = rdr["Make"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
       



        public IEnumerable<Pa_Inventry_DAL> Get_MultipleYearList_DAL(int Item_ID)
        {
            //pa_Select_InventryList

            string sp = "pa_Select_MultipleYear";
            List<Pa_Inventry_DAL> itemlist = new List<Pa_Inventry_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Item_ID", Item_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Inventry_DAL item = new Pa_Inventry_DAL();

                        item.Item_ID = Convert.ToInt32(rdr["Item_ID"]);
                        item.Year = (rdr["Year"].ToString());

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        
       
        public string Delete_MultipleYear_DAL(int? Item_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Item_ID",Item_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_MultipleYear", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public IEnumerable<Pa_ItemLocations_DAL> Get_ItemLocationList_DAL(int c_ID)
        {
            //pa_Select_ItemLocationList

            string sp = "pa_Select_ItemLocationList";
            List<Pa_ItemLocations_DAL> itemlist = new List<Pa_ItemLocations_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_ItemLocations_DAL item = new Pa_ItemLocations_DAL();

                        item.ItemLocation_ID = Convert.ToInt32(rdr["ItemLocation_ID"]);
                        item.ItemLocationName = rdr["ItemLocationName"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }


        public string Insert_ItemLocation_DAL(string ItemLocationName, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@ItemLocationName",ItemLocationName),
                new SqlParameter("@c_ID",c_ID)
                 



            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_ItemLocation", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Update_ItemLocation_DAL(int? ItemLocation_ID, string ItemLocationName, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                 new SqlParameter("@ItemLocation_ID",ItemLocation_ID),
                 new SqlParameter("@ItemLocationName",ItemLocationName)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_ItemLocation", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Delete_ItemLocation_DAL(int? ItemLocation_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@ItemLocation_ID",ItemLocation_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_ItemLocation", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }


        public IEnumerable<Pa_CustomerCats_DAL> Get_CustomerCatsList_DAL()
        {


            string sp = "pa_Select_CustomerCatsList";
            List<Pa_CustomerCats_DAL> itemlist = new List<Pa_CustomerCats_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_CustomerCats_DAL item = new Pa_CustomerCats_DAL();

                        item.CustomerCat_ID = Convert.ToInt32(rdr["CustomerCat_ID"]);
                        item.CustomerCatName = rdr["CustomerCatName"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        public IEnumerable<Pa_CustomerCats_DAL> Get_CustomerCatList_DAL(int c_ID)
        {
            //pa_Select_CustomerCatList

            string sp = "pa_Select_CustomerCatList";
            List<Pa_CustomerCats_DAL> itemlist = new List<Pa_CustomerCats_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_CustomerCats_DAL item = new Pa_CustomerCats_DAL();

                        item.CustomerCat_ID = Convert.ToInt32(rdr["CustomerCat_ID"]);
                        item.CustomerCatName = rdr["CustomerCatName"].ToString();

                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }


        public string Insert_CustomerCat_DAL(string CustomerCatName, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@CustomerCatName",CustomerCatName),
                new SqlParameter("@c_ID",c_ID),
                 new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_CustomerCat", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Update_CustomerCat_DAL(int? CustomerCat_ID, string CustomerCatName, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                 new SqlParameter("@CustomerCat_ID",CustomerCat_ID),
                 new SqlParameter("@CustomerCatName",CustomerCatName),
                 new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_CustomerCat", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Delete_CustomerCat_DAL(int? CustomerCat_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@CustomerCat_ID",CustomerCat_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_CustomerCat", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public string Delete_EngineSpecs_DAL(int? EngineSpecsCode_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@EngineSpecsCode_ID",EngineSpecsCode_ID)


            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_EngineSpecsCode", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public Pa_Inventry_DAL GetBarcodeDetail_DAL(string Barcode)
        {


            string sp = "GetBarcodeDetail";
            Pa_Inventry_DAL Cust = new Pa_Inventry_DAL();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Barcode", Barcode);
                    SqlDataReader record = cmd.ExecuteReader();


                    if (record.Read())
                    {

                        Cust.ItemDescription = record["ItemDescription"].ToString();
                        Cust.ItemCode = record["ItemCode"].ToString();
                        Cust.UnitPrice = record["Unit_Price"].ToString();
                        Cust.SalePrice = record["Sale_Price"].ToString();
                        Cust.UOM = record["UOM"].ToString();
                        Cust.Item_ID =  Convert.ToInt32(record["Item_ID"]);


                    }

                    con.Close();

                }



            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }

            return Cust;
        }
        public Pa_ModelDesc_DAL GetCodeDetail_DAL(string Code)
        {


            string sp = "GetCodeDetail";
            Pa_ModelDesc_DAL Cust = new Pa_ModelDesc_DAL();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Code", Code);
                    SqlDataReader record = cmd.ExecuteReader();


                    if (record.Read())
                    {

                        Cust.Make_ID = Convert.ToInt32(record["Make_ID"]);
                        Cust.ModelDesc_ID = Convert.ToInt32(record["ModelDesc_ID"]);
          


                    }

                    con.Close();

                }



            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }

            return Cust;
        }

        public Pa_ModelDesc_DAL GetDetailsByCode_DAL(string Code)
        {


            string sp = "sp_DetailsbyMakeCode";
            Pa_ModelDesc_DAL Cust = new Pa_ModelDesc_DAL();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Code", Code);
                    SqlDataReader record = cmd.ExecuteReader();


                    if (record.Read())
                    {

                        Cust.Make_ID = Convert.ToInt32(record["Make_ID"]);
                        Cust.Make = record["Make"].ToString();
                        Cust.ModelDesc_ID = Convert.ToInt32(record["ModelDesc_ID"]);

                        Cust.ModelDesciption = record["ModelDesciption"].ToString();
                        Cust.VehCategory_ID = Convert.ToInt32(record["VehCategory_ID"]);
                        Cust.VehCategoryName = record["VehCategoryName"].ToString();
                        Cust.MakeCountry_ID = record["MakeCountry_ID"].ToString();
                        Cust.CountryName = record["VehicalCountry"].ToString();
                        Cust.Drive = record["Drive"].ToString();
                        Cust.Door = record["Door"].ToString();
                        Cust.EngineType = Convert.ToInt32(record["Engine_Type"]);
                        Cust.EngineTypeName = record["EngineType"].ToString();
                        Cust.FuelType = record["Fuel_Type"].ToString();
                        Cust.Hs_Code = record["Hs_Code"].ToString();
                    }

                    con.Close();

                }



            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }

            return Cust;
        }
        //---get Basic Banks List
        public IEnumerable<BasicBanks_DAL> Get_BasicBanksList_DAL(int c_ID)
        {
            string sp = "Select_BasicBanksList";
            List<BasicBanks_DAL> itemlist = new List<BasicBanks_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        BasicBanks_DAL item = new BasicBanks_DAL();

                        item.Bank_ID = Convert.ToInt32(rdr["Bank_ID"]);
                        item.BankName = rdr["BankName"].ToString();
                        item.AccountName = rdr["AccountName"].ToString();
                        item.AccountNumber = rdr["AccountNumber"].ToString();
                        item.IBAN = rdr["IBAN"].ToString();
                        item.branch = rdr["branch"].ToString();
                        item.SwiftCode = rdr["SwiftCode"].ToString();
                        item.Address = rdr["Address"].ToString();
                        item.ContactNumber = rdr["ContactNumber"].ToString();
                        item.created_at = rdr["Created_ats"].ToString();
                        item.created_by = rdr["Created_bys"].ToString();
                        item.modified_at = rdr["Modified_ats"].ToString();
                        item.modified_by = rdr["Modified_bys"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        //Insert New Tasks
        public string Insert_BasicBank_DAL(string BankName, string AccountName, string AccountNumber, string IBAN, string branch, string SwiftCode, string Address, string ContactNumber, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@BankName",BankName  ),
                new SqlParameter("@AccountName" ,AccountName  ),
                new SqlParameter("@AccountNumber",AccountNumber  ),
                new SqlParameter("@IBAN",IBAN),
                new SqlParameter("@branch" ,branch),
                new SqlParameter("@SwiftCode",SwiftCode),
                new SqlParameter("@Address",Address ),
                new SqlParameter("@ContactNumber",ContactNumber ),
                new SqlParameter("@c_ID", c_ID ),
                new SqlParameter("@Created_by",Created_by)

            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_BasicBank", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        //Update New Tasks
        public string Update_BasicBank_DAL(int? Bank_ID, string BankName, string AccountName, string AccountNumber, string IBAN, string branch, string SwiftCode, string Address, string ContactNumber, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Bank_ID",Bank_ID),
                new SqlParameter("@BankName",BankName  ),
                new SqlParameter("@AccountName" ,AccountName  ),
                new SqlParameter("@AccountNumber",AccountNumber),
                new SqlParameter("@IBAN" , IBAN),
                new SqlParameter("@branch" ,branch),
                new SqlParameter("@SwiftCode",SwiftCode),
                new SqlParameter("@Address",Address ),
                new SqlParameter("@ContactNumber",ContactNumber ),
                new SqlParameter("@Modified_by",Modified_by)

            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_BasicBank", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }



        //New CityRegistrations_DAL_NEW Nafeel OBasicData
        public IEnumerable<CityRegistration> CityRegistrations_DAL_NEW(int ID)
        {
            string sp = "pa_Insert_CityRegistration";
            List<CityRegistration> itemlist = new List<CityRegistration>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        CityRegistration item = new CityRegistration();

                        item.CityName = rdr["CityName"].ToString();
                        item.CityNameEnglish = rdr["CityNameEnglish"].ToString();

                        itemlist.Add(item);
                    }
                    con.Close();
                }

                return itemlist;
            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }
        }
        public IEnumerable<CityRegistration> CityRegistrationsObj { get; set; }
        public string Update_CityReg_DAL(int? ID, string CityName, string CityNameEnglish)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
               new SqlParameter("@ID",ID),
               new SqlParameter("@CityName",CityName),
               new SqlParameter("@CityNameEnglish",CityNameEnglish)
            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_CityReg", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }
        }
        public string Insert_CityReg_DAL(string CityName, string CityNameEnglish)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                    new SqlParameter("@CityName",CityName),
                    new SqlParameter("@CityNameEnglish",CityNameEnglish)
            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_CityReg", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public string DeleteCityRegExit(int? ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@ID",ID)
            };

                var response = dbLayer.SP_DataTable_return("Pa_Delete_CityReg", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }
        }
        public IEnumerable<CityRegistration> Get_CityReg_DAL(int ID)
        {
            string sp = "Pa_Select_CityReg";
            List<CityRegistration> itemlist = new List<CityRegistration>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        CityRegistration item = new CityRegistration();

                        item.CityName = rdr["CityName"].ToString();
                        item.CityNameEnglish = rdr["CityNameEnglish"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;
            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }
        }
        public IEnumerable<CityRegistration> Get_CityRegistration_DAL_List()
        {
            string sp = "Pa_Select_CityReg";
            List<CityRegistration> itemlist = new List<CityRegistration>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;



                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        CityRegistration item = new CityRegistration();
                        item.ID = Convert.ToInt32(rdr["ID"]);
                        item.CityName = rdr["City_Name"].ToString();
                        item.CityNameEnglish = rdr["City_Name_English"].ToString();

                        itemlist.Add(item);
                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }
        }
        public IEnumerable<CityRegistration> Get_CityReg_Obj { get; set; }
        public IEnumerable<CityRegistration> CustomerRegistrationList { get; set; }
        public IEnumerable<CityRegistration> Get_CityRegistrationsObj { get; set; }





        //New Words Nafeel OBasicData
        public IEnumerable<Words> Get_WordsDAL_List()
        {
            string sp = "Pa_Select_Words";
            List<Words> itemlist = new List<Words>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Words item = new Words();

                        item.ID = Convert.ToInt32(rdr["ID"]);
                        item.WordName = rdr["Words"].ToString();
                        itemlist.Add(item);
                    }
                    con.Close();
                }
                return itemlist;
            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }
        }
        public string InsertWordsDAL(string Words)
        {
            string msg = "";
            try
            {
                SqlParameter[] paramArray = new SqlParameter[] {
                    new SqlParameter("@Words",Words)

            };
                var response = dbLayer.SP_DataTable_return("Pa_insert_WordsReg", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                msg = ErrorMessage;
                return msg;
            }
        }
        public string DeleteWordsExit(int? ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@ID",ID)
            };

                var response = dbLayer.SP_DataTable_return("Pa_Delete_Words", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }
        }
        public string UpdateWordsDAL(int? ID, string Words)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Words", Words)

            };

                var response = dbLayer.SP_DataTable_return("Pa_Update_Words", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                msg = ErrorMessage;
                return msg;
            }
        }
        public IEnumerable<Words> WordsRegistrationList { get; set; }
        
        public Pa_WordDocument WordLoadRef()
        {



            string sp = "Get_WordRef";

            Pa_WordDocument si = new Pa_WordDocument();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                si.WordRef = dr["Ref"].ToString();
                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }


            return si;
        }

        public IEnumerable<Pa_WordDocument> Get_WordDocument_List_DAL(int c_ID,int WordDocument_ID, int Vendor_ID, int Customer_ID,string WordTitle)
        {
            string sp = "pa_select_WordDocument_List";
            List<Pa_WordDocument> AdminUsersList = new List<Pa_WordDocument>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    cmd.Parameters.AddWithValue("@WordDocument_ID", WordDocument_ID);
                    cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                    cmd.Parameters.AddWithValue("@Customer_ID", Customer_ID);
                    cmd.Parameters.AddWithValue("@WordTitle", WordTitle);
                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_WordDocument item = new Pa_WordDocument();

                        item.ID = Convert.ToInt32(rdr["ID"]);
                        item.WordDocument_ID = Convert.ToInt32(rdr["WordDocument_ID"]);
                        item.WordRef = rdr["WordRef"].ToString();


                        item.FileName = rdr["FileName"].ToString();
         
                        item.c_ID = Convert.ToInt32(rdr["c_ID"]);
                        item.ApprovedBy = Convert.ToInt32(rdr["ApprovedBy"]);
                        item.ApprovedBy_Name = rdr["UserName"].ToString();
                        item.PDFName = rdr["PDFName"].ToString();
                        item.CustomerName = rdr["CustomerName"].ToString();
                        item.VendorName = rdr["VendorName"].ToString();
                        item.Approved_at = rdr["Approved_at"].ToString();
                        item.Created_at = rdr["Created_at"].ToString();
                        item.Createby_name = rdr["Createdby_Name"].ToString();
                        item.WordTitle = rdr["WordTitle"].ToString();
                        item.imgname = rdr["imgname"].ToString();
                        AdminUsersList.Add(item);

                    }

                    con.Close();

                }

                return AdminUsersList;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }
        }

        public Pa_WordDocument Get_WordList(int c_ID, int WordDocument_ID)
        {



            string sp = "pa_select_WordDocument_List";
            Pa_WordDocument item = new Pa_WordDocument();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    cmd.Parameters.AddWithValue("@WordDocument_ID", WordDocument_ID);
                    cmd.Parameters.AddWithValue("@Vendor_ID", 0);
                    cmd.Parameters.AddWithValue("@Customer_ID", 0);
                    cmd.Parameters.AddWithValue("@WordTitle", "");
                    
                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {



                        item.ID = Convert.ToInt32(rdr["ID"]);
                        item.WordDocument_ID = Convert.ToInt32(rdr["WordDocument_ID"]);
                        item.WordRef = rdr["WordRef"].ToString();


                        item.FileName = rdr["FileName"].ToString();

                        item.c_ID = Convert.ToInt32(rdr["c_ID"]);
                        item.ApprovedBy = Convert.ToInt32(rdr["ApprovedBy"]);
                        item.PDFName = rdr["PDFName"].ToString();
                        item.Filepath = rdr["Filepath"].ToString();
               




                    }

                    con.Close();

                }

                return item;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        public Pa_WordDocument Get_WordList_I(int c_ID, int WordDocument_ID)
        {



            string sp = "pa_select_WordDocument_List";
            Pa_WordDocument item = new Pa_WordDocument();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    cmd.Parameters.AddWithValue("@WordDocument_ID", WordDocument_ID);
                    cmd.Parameters.AddWithValue("@Vendor_ID", 0);
                    cmd.Parameters.AddWithValue("@Customer_ID", 0);
                    cmd.Parameters.AddWithValue("@WordTitle", "");
                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {



                        item.ID = Convert.ToInt32(rdr["ID"]);
                        item.WordDocument_ID = Convert.ToInt32(rdr["WordDocument_ID"]);
                        item.WordRef = rdr["WordRef"].ToString();


                        item.FileName = rdr["FileName"].ToString();

                        item.c_ID = Convert.ToInt32(rdr["c_ID"]);
                        item.ApprovedBy = Convert.ToInt32(rdr["ApprovedBy"]);
                        item.PDFName = rdr["PDFName"].ToString();
                        item.Filepath = rdr["Filepath"].ToString();
                        item.imgname = rdr["imgname"].ToString();





                    }

                    con.Close();

                }

                return item;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        public string Insert_WordDocument_DAL(string WordRef, int? c_ID, string FileName, string Filepath, int Vendor_ID, int Customer_ID,string WordTitle,int Create_by)
        {

            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {

                     new SqlParameter("@WordRef",WordRef),

                    new SqlParameter("@FileName",FileName),

                      new SqlParameter("@Filepath",Filepath),
                      new SqlParameter("@Vendor_ID",Vendor_ID),
                      new SqlParameter("@Customer_ID",Customer_ID),
                      new SqlParameter("@WordTitle",WordTitle),
                      new SqlParameter("@Create_by",Create_by),

                        new SqlParameter("@c_ID",c_ID),









            };


            var response = dbLayer.SP_DataTable_return("pa_Insert_WordDocument", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }

        public string Delete_WordDocument_DAL(int? id)
        {

            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {

                    new SqlParameter("@id",id),








            };


            var response = dbLayer.SP_DataTable_return("pa_delete_WordDocument", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }
        public string ChangeMasterStatus_DAL(int? WordDocument_ID, int? Status_ID, int? c_ID, string Trans_Type, int? Modified_by, string QRName,string PDFName)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@WordDocument_ID",WordDocument_ID),
                  new SqlParameter("@Status_ID",Status_ID),
                   new SqlParameter("@c_ID",c_ID),
                    new SqlParameter("@Trans_Type",Trans_Type),
                    new SqlParameter("@QRName",QRName),
                    new SqlParameter("@PDFName",PDFName),
                   new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_Status_Word", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        //By Yaseen 
        public List<AlertNotification_DAL> Get_AlterNotiList_Dal(int UserId)
        {


            string sp = "select_Conflicts";
            List <AlertNotification_DAL> itemlist = new List<AlertNotification_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
               //     cmd.Parameters.AddWithValue("@UserId", UserId);

                    SqlDataReader rdr = cmd.ExecuteReader();


                  
                   

                   

                    while (rdr.Read())
                    {

                        AlertNotification_DAL item = new AlertNotification_DAL();

                        item.ID = Convert.ToInt32(rdr["REF"]);
                        item.AlertDescription = rdr["Description"].ToString();
                        item.alert_count = Convert.ToInt32(rdr["Cnt_Alterts"]);
                                    
                        itemlist.Add(item);
                    }
                  
                   


                    con.Close();

                }


                if (itemlist.Count==0)
                {

                    AlertNotification_DAL item = new AlertNotification_DAL();

                    item.ID =0;
                    item.AlertDescription = "";
                    item.alert_count = 0;

                    itemlist.Add(item);
                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }



        }

        #region PayVia For SaleInvoice_trd
        public IEnumerable<Accounts_DAL> Select_PV_OnAccounts_Trd(int c_ID)
        {
            string sp = "Select_PV_OnAccounts_Trd";
            List<Accounts_DAL> itemlist = new List<Accounts_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Accounts_DAL item = new Accounts_DAL();

                        item.Account_ID = Convert.ToInt32(rdr["account_ID"]);

                        item.AccountName = rdr["AccountName"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }


        #endregion

        #region CommisionRates

        public IEnumerable<Pa_CommisionRates_DAL> CommisionRateobjList { get; set; }
        public IEnumerable<Pa_CommisionRates_DAL> Get_CommisionRatesList_DAL()
        {
            string sp = "pa_Select_CommissionRatesList";
            List<Pa_CommisionRates_DAL> itemlist = new List<Pa_CommisionRates_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_CommisionRates_DAL item = new Pa_CommisionRates_DAL();

                        item.ID = Convert.ToInt32(rdr["ID"]);
                        item.CommissionRates_id = Convert.ToInt32(rdr["CommissionRates_id"]);
                        item.Category_ID = Convert.ToInt32(rdr["Category_ID"]);
                        item.Category_Name = rdr["Category_Name"].ToString();
                        
                        item.Rate = Convert.ToInt32(rdr["Rate"]);
                        item.Status_ID = Convert.ToInt32(rdr["IsActive"]);
                        
                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();
                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }
        }

        public string Insert_CommisionRates_DAL(int Category_ID, decimal Rate, int Status, int? Created_by)
        {
            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Category_id",Category_ID),
                new SqlParameter("@Rate",Rate),
                new SqlParameter("@IsActive",Status),
                 new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_insert_CommissionRates", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Update_CommisionRates_DAL(int? CommissionRates_ID, int Category_ID, decimal Rate, int Status, int? Modified_by)
        {
            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
              
                    
                    new SqlParameter("@CommissionRates_ID",CommissionRates_ID),
                     new SqlParameter("@Category_id",Category_ID),               
                      new SqlParameter("@Rate",Rate),
                       new SqlParameter("@IsActive",Status),
                      new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_CommissionRates", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string Delete_CommisionRates_DAL(int? CommissionRates_ID)
        {
            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@CommissionRates_ID",CommissionRates_ID)
            };

                var response = dbLayer.SP_DataTable_return("Pa_Delete_CommissionRates", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }
        }

        public IEnumerable<Pa_Status_DAL> get_SalesOther_Status_DAL()
        {

            string sp = "get_SalesOther_Status";
            List<Pa_Status_DAL> itemlist = new List<Pa_Status_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Status_DAL item = new Pa_Status_DAL();

                        //item.Status_ID = Convert.ToInt32(rdr["SaleMaster_ID"].ToString());

                        item.Status = rdr["Other_Status"].ToString();



                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }

        }

        #endregion


        public IEnumerable<Accounts_DAL> Select_PV_OnAccounts_SaleReturn_Trd(int c_ID)
        {
            string sp = "Select_PV_OnAccounts__SaleReturn_Trd";
            List<Accounts_DAL> itemlist = new List<Accounts_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Accounts_DAL item = new Accounts_DAL();

                        item.Account_ID = Convert.ToInt32(rdr["account_ID"]);

                        item.AccountName = rdr["AccountName"].ToString();

                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }


    }
}
