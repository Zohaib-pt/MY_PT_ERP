using DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using X.PagedList;

namespace DAL.oClasses
{
    public class OPurchases : IOPurchases
    {
        private readonly IConnectionStringManager _conMgr;
        private readonly ISqlHelper dbLayer;
        private IConfiguration configuration;
        SqlConnection con;

        public OPurchases(IConnectionStringManager conMgr, ISqlHelper sqlHelp, IConfiguration iConfig)
        {

            _conMgr = conMgr;
            dbLayer = sqlHelp;
            //---Getting Connection String
            Constr = _conMgr.GetConnectionString();
            configuration = iConfig;
        }
        public string Constr { get; set; }


        //Faraz Work
        public IEnumerable<Pa_PurchaseMaster_DAL> purchaseOtherMasterList1 { get; set; }




        public Pa_PurchaseDetails_DAL purchaseObject { get; set; }
        public IEnumerable<Pa_PurchaseDetails_DAL> purchaseDetailList { get; set; }
        public Pa_PurchaseMaster_DAL purchaseMasterObj { get; set; }
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


        //Get Model List

        //--getting purchase ref
        public Pa_PurchaseMaster_DAL Get_PurchaseMasterRef_DAL()
        {

            string sp = "Pa_AutoPurchaseMasterRef";

            Pa_PurchaseMaster_DAL pm = new Pa_PurchaseMaster_DAL();
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

                                pm.PurchaseRef = dr["PurchaseRef"].ToString();

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


            return pm;
        }

        //Insert purchase detail
        public string InsertPurchaseDetail_DAL(int? Make_ID, int? Model_ID, int? Color_ID, string mileage, int Make_category_ID, string Engine_Type, int StockType_ID, string Drive, string Used_New, string Fuel_Type, string BL_NO, string SellingPrice, string OtherCost, string OthersSpecs, double? Quantity, int? Currency_ID,
             double? Currency_Rate, string Unit_Price, string Amount, string Amount_Others, string Ref, string Temp_ID, int? Create_by, int PurchaseMaster_ID,
             string Chassis, string ModelYear, int? color_interior_ID, int? loc, int? c_ID, string PriceTax, string AuctionFee,
             string ReksoFee, string RecycleFee, string Vanning_Charges, string FreightCharges,
             string FreightRate, string Cont_NO, string AuctionName, string Other_Charges, string JP_Charges,string Door,string Hs_Code)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Make_ID",Make_ID),
                new SqlParameter("@Model_ID",Model_ID),
                new SqlParameter("@Color_ID",Color_ID),

                new SqlParameter("@OthersSpecs",OthersSpecs),
                new SqlParameter("@Quantity",Quantity),
                 new SqlParameter("@Currency_ID",Currency_ID),
                 new SqlParameter("@Unit_Price",Unit_Price),
                 new SqlParameter("@Currency_Rate",Currency_Rate),
                 new SqlParameter("@PriceTax",PriceTax),
                new SqlParameter("@AuctionFee",AuctionFee),
                new SqlParameter("@ReksoFee",ReksoFee),
                new SqlParameter("@RecycleFee",RecycleFee),
                new SqlParameter("@Vanning_Charges",Vanning_Charges),
                new SqlParameter("@FreightCharges",FreightCharges),
                new SqlParameter("@FreightRate",FreightRate),

                 new SqlParameter("@Amount",Amount),
                 new SqlParameter("@Amount_Others",Amount_Others),
                 new SqlParameter("@Ref",Ref),
                 new SqlParameter("@Temp_ID",Temp_ID),
                 new SqlParameter("@Create_by",Create_by),
                 new SqlParameter("@Chassis_No",Chassis),
                 new SqlParameter("@ModelYear",ModelYear),
                 new SqlParameter("@color_interior_ID",color_interior_ID),
                 new SqlParameter("@LocID",loc),
                 new SqlParameter("@mileage",mileage),
                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@PurchaseMaster_ID",PurchaseMaster_ID),

                 new SqlParameter("@Make_category_ID",Make_category_ID),
                new SqlParameter("@Engine_Type",Engine_Type),
                new SqlParameter("@StockType_ID",StockType_ID),
                 new SqlParameter("@Drive",Drive),
                new SqlParameter("@Used_New",Used_New),
                new SqlParameter("@Fuel_Type",Fuel_Type),
                new SqlParameter("@BL_NO",BL_NO),
                new SqlParameter("@SellingPrice",SellingPrice),
                new SqlParameter("@Cont_NO",Cont_NO),
                new SqlParameter("@AuctionName",AuctionName),
                new SqlParameter("@Other_Charges",Other_Charges),
                 new SqlParameter("@JP_Charges",JP_Charges),
                new SqlParameter("@OtherCost",OtherCost),


                new SqlParameter("@Door",Door),
                new SqlParameter("@Hs_Code",Hs_Code)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Insert_PurchaseDetail", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();
                if (msg.Trim().Contains("Violation of UNIQUE KEY"))
                {
                    msg = "Already Exist!";
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


        //Insert purchase detail
        public string UpdatePurchaseDetail_DAL(int? PurchaseDetails_ID, int? Make_ID, int? Model_ID, int? Color_ID, string mileage, int Make_category_ID, string Engine_Type, int StockType_ID, string Drive, string Used_New, string Fuel_Type, string BL_NO, string SellingPrice, string OtherCost, string OthersSpecs, int? Quantity, int? Currency_ID,
           double? Currency_Rate, string Unit_Price, string Amount, string Amount_Others, string Ref, int? Modified_by, string Chassis, string ModelYear,
           int? color_interior_ID, int? loc, int? c_ID, int? StockID, string PriceTax, string AuctionFee, string ReksoFee, string RecycleFee,
           string Vanning_Charges, string FreightCharges, string FreightRate, string Cont_NO, string AuctionName, string Other_Charges, string JP_Charges,string Door , string Hs_Code)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                 new SqlParameter("@PurchaseDetails_ID",PurchaseDetails_ID),
                 new SqlParameter("@Make_ID",Make_ID),
                 new SqlParameter("@Model_ID",Model_ID),
                 new SqlParameter("@Color_ID",Color_ID),

                 new SqlParameter("@OthersSpecs",OthersSpecs),
                 new SqlParameter("@Quantity",Quantity),
                 new SqlParameter("@Currency_ID",Currency_ID),
                 new SqlParameter("@Currency_Rate",Currency_Rate),
                 new SqlParameter("@Unit_Price",Unit_Price),
                 new SqlParameter("@Amount",Amount),
                 new SqlParameter("@Amount_Others",Amount_Others),
                 new SqlParameter("@Ref",Ref),
                 new SqlParameter("@Modified_by",Modified_by),
                 new SqlParameter("@Stock_ID",StockID),
                 new SqlParameter("@Chassis_No",Chassis),
                 new SqlParameter("@ModelYear",ModelYear),
                 new SqlParameter("@color_interior_ID",color_interior_ID),
                 new SqlParameter("@LocID",loc),
                 new SqlParameter("@mileage",mileage),
                 new SqlParameter("@c_ID",c_ID),
                 new SqlParameter("@Make_category_ID",Make_category_ID),
                new SqlParameter("@Engine_Type",Engine_Type),
                new SqlParameter("@StockType_ID",StockType_ID),
                 new SqlParameter("@Drive",Drive),
                new SqlParameter("@Used_New",Used_New),
                new SqlParameter("@Fuel_Type",Fuel_Type),
                new SqlParameter("@BL_NO",BL_NO),
                new SqlParameter("@SellingPrice",SellingPrice),
                new SqlParameter("@OtherCost",OtherCost),
                new SqlParameter("@PriceTax",PriceTax),
                new SqlParameter("@AuctionFee",AuctionFee),
                new SqlParameter("@ReksoFee",ReksoFee),
                new SqlParameter("@RecycleFee",RecycleFee),
                new SqlParameter("@Vanning_Charges",Vanning_Charges),
                new SqlParameter("@FreightCharges",FreightCharges),
                new SqlParameter("@Cont_NO",Cont_NO),
                new SqlParameter("@AuctionName",AuctionName),
                new SqlParameter("@Other_Charges",Other_Charges),
                new SqlParameter("@FreightRate",FreightRate),
                new SqlParameter("@JP_Charges",JP_Charges),



                new SqlParameter("@Door",Door),
                new SqlParameter("@Hs_Code",Hs_Code)


            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_PurchaseDetail", paramArray).Tables[0];
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



        //Insert purchase Master
        public string InsertPurchaseMaster_DAL(int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string PurchaseType, string PurchaseDate, int? VAT_Rate, double? Discount, string Temp_ID, int? c_ID, int? Created_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Vendor_ID",Vendor_ID),
                new SqlParameter("@Vendor_PruchaseTo",Vendor_PruchaseTo),
                new SqlParameter("@Vendor_Address",Vendor_Address),
                  new SqlParameter("@PurchaseType",PurchaseType),
                 new SqlParameter("@PurchaseDate",PurchaseDate),
                   new SqlParameter("@VAT_Rate",VAT_Rate),
                new SqlParameter("@Discount",Discount),
                 new SqlParameter("@Temp_ID",Temp_ID),
                  new SqlParameter("@c_ID",c_ID),
                 new SqlParameter("@Created_by",Created_by)




            };


                var response = dbLayer.SP_DataTable_return("Pa_Insert_PurchaseMaster", paramArray).Tables[0];
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


        //--getting purchase master by id
        public Pa_PurchaseMaster_DAL Get_PurchaseMasterByID_DAL(int? PurchaseMaster_ID)
        {

            string sp = "Pa_Select_PurchaseMasterByID";

            Pa_PurchaseMaster_DAL pm = new Pa_PurchaseMaster_DAL();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PurchaseMaster_ID", PurchaseMaster_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.PurchaseMaster_ID = Convert.ToInt32(dr["PurchaseMaster_ID"]);
                                pm.PurchaseRef = dr["PurchaseRef"].ToString();
                                pm.Vendor_ID = Convert.ToInt32(dr["Vendor_ID"]);
                                pm.PurchaseStatus_ID = dr["PurchaseStatus_ID"].ToString();
                                pm.PurchaseStatus = dr["PurchaseStatus"].ToString();
                                pm.Vendor_PruchaseTo = dr["Vendor_PruchaseTo"].ToString();
                                pm.Vendor_Address = dr["Vendor_Address"].ToString();
                                pm.PurchaseDate = dr["PurchaseDate"].ToString();
                                pm.Discount = dr["Discount"].ToString();
                                pm.VAT_Rate = dr["VAT_Rate"].ToString();
                                pm.Total = dr["SubTotal"].ToString();
                                pm.Created_at = dr["Created_at"].ToString();
                                pm.Created_by = dr["Created_by"].ToString();
                                pm.Modified_at = dr["Modified_at"].ToString();
                                pm.Modified_by = dr["Modified_by"].ToString();

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


            return pm;
        }



        //Get new purchase master  List
        public IEnumerable<Pa_PurchaseMaster_DAL> Get_PurchaseMasterList_DAL(string PurchaseRef, 
            int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, string ChassisNo, int? c_ID)
        {



            string sp = "Pa_Select_PurchaseMasterList";



            List<Pa_PurchaseMaster_DAL> itemlist = new List<Pa_PurchaseMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                    cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                    cmd.Parameters.AddWithValue("@From_Date", From_Date);
                    cmd.Parameters.AddWithValue("@To_Date", To_Date);
                    cmd.Parameters.AddWithValue("@Status_ID", Status_ID);
                    cmd.Parameters.AddWithValue("@ChassisNo", ChassisNo);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_PurchaseMaster_DAL item = new Pa_PurchaseMaster_DAL();


                        item.PurchaseMaster_ID = Convert.ToInt32(rdr["PurchaseMaster_ID"]);
                        item.Vendor_Name = rdr["Vendor_Name"].ToString();
                        item.Vendor_ID = Convert.ToInt32(rdr["Vendor_ID"].ToString());

                        item.PurchaseRef = rdr["PurchaseRef"].ToString();
                        item.PurchaseDate = rdr["PurchaseDate"].ToString();
                        item.PurchaseStatus = rdr["PurchaseStatus"].ToString();
                        item.Total = rdr["Total"].ToString();
                        item.VAT_Exp = rdr["VAT_Exp"].ToString();
                        item.Discount = rdr["Discount"].ToString();

                        item.Total_Amount = rdr["Total_Amount"].ToString();

                        item.Paid = rdr["Paid"].ToString();
                        item.Bal_Due = rdr["Bal_Due"].ToString();
                        item.CountCars = rdr["CountCars"].ToString();


                        // item.TotalVAT_Exp = rdr["TotalVAT_Exp"].ToString();
                        //item.BeforeVATTotal = rdr["BeforeVATTotal"].ToString();
                        //item.AfterVATTotal = rdr["AfterVATTotal"].ToString();
                        //item.TotalPaid = rdr["TotalPaid"].ToString();
                        //item.TotalBal_due = rdr["TotalBal_due"].ToString();
                        //item.TotalDiscount = rdr["TotalDiscount"].ToString();


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


        //Get new purchase master  List
        public IEnumerable<Pa_PurchaseMaster_DAL> Get_PurchaseMasterReturnList_DAL(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID)
        {


            string sp = "Pa_Select_PurchaseMaster_Return_List";
            List<Pa_PurchaseMaster_DAL> itemlist = new List<Pa_PurchaseMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                    cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                    cmd.Parameters.AddWithValue("@From_Date", From_Date);
                    cmd.Parameters.AddWithValue("@To_Date", To_Date);
                    cmd.Parameters.AddWithValue("@Status_ID", Status_ID);
                    cmd.Parameters.AddWithValue("@c_id", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_PurchaseMaster_DAL item = new Pa_PurchaseMaster_DAL();


                        item.PurchaseMaster_ID = Convert.ToInt32(rdr["PurchaseMaster_ID"]);
                        item.Vendor_Name = rdr["Vendor_Name"].ToString();
                        item.PurchaseRef = rdr["PurchaseRef"].ToString();
                        item.PurchaseDate = rdr["PurchaseDate"].ToString();
                        item.PurchaseStatus = rdr["PurchaseStatus"].ToString();
                        item.Total = rdr["Total"].ToString();
                        item.VAT_Exp = rdr["VAT_Exp"].ToString();

                        item.Total_Amount = rdr["Total"].ToString();
                        item.Paid = rdr["Paid"].ToString();
                        item.Bal_Due = rdr["Balance"].ToString();
                        
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


        //Delete Purchase Detail
        public string DeletePurchaseDetail_DAL(int? PurchaseDetails_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@PurchaseDetails_ID",PurchaseDetails_ID)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_PurchaseDetail", paramArray).Tables[0];
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



        //Delete Purchase Master
        public string DeletePurchaseMaster_DAL(int? PurchaseMaster_ID, int USER_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@PurchaseMaster_ID",PurchaseMaster_ID),
                new SqlParameter("@User_ID",PurchaseMaster_ID)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_PurchaseMaster", paramArray).Tables[0];
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


        public string DeletePurchaseMasterOther_DAL(int? PurchaseMaster_ID, int USER_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@PurchaseMaster_ID",PurchaseMaster_ID),
                  new SqlParameter("@User_ID",USER_ID),




            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_PurchaseMasterOther", paramArray).Tables[0];
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
        //change Purchase Master status
        public string ChangePurchaseMasterStatus_DAL(int? PurchaseMaster_ID, int? Status_ID, int? c_ID, string Trans_Type, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@PurchaseMaster_ID",PurchaseMaster_ID),
                  new SqlParameter("@Status_ID",Status_ID),
                   new SqlParameter("@c_ID",c_ID),
                    new SqlParameter("@Trans_Type",Trans_Type),
                   new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_PurchaseStatus", paramArray).Tables[0];
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


        //Update purchase Master
        public string UpdatePurchaseMaster_DAL(int? PurchaseMaster_ID, int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string PurchaseDate, int? VAT_Rate, double? Discount, string Temp_ID, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@PurchaseMaster_ID",PurchaseMaster_ID),
                new SqlParameter("@Vendor_ID",Vendor_ID),
                new SqlParameter("@Vendor_PruchaseTo",Vendor_PruchaseTo),
                new SqlParameter("@Vendor_Address",Vendor_Address),
                 new SqlParameter("@PurchaseDate",PurchaseDate),
                  new SqlParameter("@VAT_Rate",VAT_Rate),
                  new SqlParameter("@Discount",Discount),
                 new SqlParameter("@Temp_ID",Temp_ID),
                 new SqlParameter("@Modified_by",Modified_by)




            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_PurchaseMaster", paramArray).Tables[0];
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


        //--getting purchase detail by id
        public Pa_PurchaseDetails_DAL GetPuchaseDetailByID_DAL(int? PurchaseDetails_ID)
        {

            string sp = "Pa_Select_PurchaseDetailByID";

            Pa_PurchaseDetails_DAL pm = new Pa_PurchaseDetails_DAL();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PurchaseDetails_ID", PurchaseDetails_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.PurchaseDetails_ID = Convert.ToInt32(dr["PurchaseDetails_ID"]);
                                pm.Make_ID = dr["Make_ID"].ToString();
                                pm.Model_ID = dr["Model_ID"].ToString();
                                pm.ModelDesciption = dr["ModelDesciption"].ToString();
                                pm.Color_ID = dr["Color_ID"].ToString();
                                pm.OthersSpecs = dr["OthersSpecs"].ToString();
                                pm.Quantity = dr["Quantity"].ToString();
                                pm.Currency_ID = dr["Currency_ID"].ToString();
                                pm.Currency_Rate = dr["Currency_Rate"].ToString();
                                pm.Unit_Price = dr["Unit_Price"].ToString();
                                pm.Amount = dr["Amount"].ToString();
                                pm.Amount_Others = dr["Amount_Others"].ToString();
                                pm.Ref = dr["Ref"].ToString();
                                pm.Chassis_No = dr["Chassis_No"].ToString();
                                pm.ModelYear = dr["ModelYear"].ToString();
                                pm.Color_Interior_ID = Convert.ToInt32(dr["Color_Interior_ID"]);
                                pm.locID = Convert.ToInt32(dr["Loc_ID"]);
                                pm.StockID = Convert.ToInt32(dr["stock_ID"]);
                                pm.mileage = dr["mileage"].ToString();
                                pm.Make_category_ID = Convert.ToInt32(dr["Make_category_ID"]);
                                pm.Engine_Type = dr["Engine_Type"].ToString();
                                pm.Fuel_Type = dr["Fuel_Type"].ToString();
                                pm.Used_New = dr["Used_New"].ToString();
                                pm.Drive = dr["Drive"].ToString();

                                pm.StockType_ID = Convert.ToInt32(dr["StockType_ID"]);
                                pm.BL_NO = dr["BL_NO"].ToString();
                                pm.Selling_Price = dr["Selling_Price"].ToString();
                                pm.OtherCost = dr["OtherCost"].ToString();

                                pm.PriceTax = dr["PriceTax"].ToString();
                                pm.AuctionFee = dr["AuctionFee"].ToString();
                                pm.ReksoFee = dr["ReksoFee"].ToString();
                                pm.RecycleFee = dr["RecycleFee"].ToString();
                                pm.LoadingCharges = dr["LoadingCharges"].ToString();
                                pm.FreightCharges = dr["FreightCharges"].ToString();
                                pm.FreightRate = dr["FreightRate"].ToString();
                                pm.ContainerNo = dr["ContainerNo"].ToString();
                                pm.AuctionName = dr["AuctionName"].ToString();
                                pm.OtherCharges = dr["OtherCharges"].ToString();

                                pm.JP_Charges = dr["JP_Charges"].ToString();




                                pm.Door = dr["Door"].ToString();
                                pm.Hs_Code = dr["Hs_Code"].ToString();



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


            return pm;
        }



        // Get Old TempID From PurchaseDetail_DAL by PurchaseMaster_ID
        public string GetOldTempIDFromPurchaseDetail_DAL(int? PurchaseMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@PurchaseMaster_ID",PurchaseMaster_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_SelectPurchaseDetailOldTempID", paramArray).Tables[0];
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


        //--getting purchase master other by id
        public Pa_PurchaseMaster_DAL Get_PurchaseMaster_OtherByID_DAL(int? PurchaseMaster_ID)
        {

            string sp = "Pa_Select_PurchaseMaster_OtherByID";

            Pa_PurchaseMaster_DAL pm = new Pa_PurchaseMaster_DAL();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PurchaseMaster_ID", PurchaseMaster_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.PurchaseMaster_ID = Convert.ToInt32(dr["PurchaseMaster_ID"]);
                                pm.PurchaseRef = dr["PurchaseRef"].ToString();
                                pm.Vendor_ID = Convert.ToInt32(dr["Vendor_ID"]);
                                pm.PurchaseStatus_ID = dr["PurchaseStatus_ID"].ToString();
                                pm.PurchaseStatus = dr["PurchaseStatus"].ToString();
                                pm.Vendor_PruchaseTo = dr["Vendor_PruchaseTo"].ToString();
                                pm.Vendor_Address = dr["Vendor_Address"].ToString();
                                pm.PurchaseDate = dr["PurchaseDate"].ToString();
                                pm.Discount = dr["Discount"].ToString();
                                pm.VAT_Rate = dr["VAT_Rate"].ToString();
                                pm.Total = dr["SubTotal"].ToString();
                                pm.Created_at = dr["Created_at"].ToString();
                                pm.Created_by = dr["Created_by"].ToString();
                                pm.Modified_at = dr["Modified_at"].ToString();
                                pm.Modified_by = dr["Modified_by"].ToString();
                                pm.Vendor_Name = dr["PartnerName"].ToString();

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


            return pm;
        }




        //Insert purchase detail others
        public string InsertPurchaseDetail_Other_DAL(int? item_ID, string item_Description, double? Quantity, string UOM, int? Currency_ID, double? Currency_Rate, double? Unit_Price, int? VAT_Rate,
           double? VAT_Exp, double? Discount, double? Amount, double? TtlAmount, double? Amount_Others, string Temp_ID, int? c_ID, int? Created_by, int PurchaseMaster_ID, int Location_ID, string Serial, string Barcode)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@item_ID",item_ID),
                new SqlParameter("@item_Description",item_Description),
                new SqlParameter("@Quantity",Quantity),
                 new SqlParameter("@UOM",UOM),

                 new SqlParameter("@Currency_ID",Currency_ID),
                 new SqlParameter("@Currency_Rate",Currency_Rate),
                 new SqlParameter("@Unit_Price",Unit_Price),

                    new SqlParameter("@VAT_Rate",VAT_Rate),
                       new SqlParameter("@VAT_Exp",VAT_Exp),
                          new SqlParameter("@Discount",Discount),
                             new SqlParameter("@Amount",Amount),
                                new SqlParameter("@TtlAmount",TtlAmount),


                 new SqlParameter("@Amount_Others",Amount_Others),

                  new SqlParameter("@Temp_ID",Temp_ID),
                   new SqlParameter("@Created_by",Created_by),
                   new SqlParameter("@PurchaseMaster_ID",PurchaseMaster_ID),
                   new SqlParameter("@Serial",Serial),
                   new SqlParameter("@Barcode",Barcode),
                   new SqlParameter("@Location_ID",Location_ID),
                   new SqlParameter("@c_ID",c_ID)




            };


                var response = dbLayer.SP_DataTable_return("Pa_Insert_PurchaseDetail_Others", paramArray).Tables[0];
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




        //Get purchase master attachment list
        public IEnumerable<Pa_Attachments_DAL> GetPurchaseMasterNew_Attachments_DAL(int? PurchaseMaster_ID, string Type)
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
                    cmd.Parameters.AddWithValue("@Master_ID", PurchaseMaster_ID);
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

        //Insert purchase master attachment
        public string Insert_PurchaseMasterAttachment_DAL(int? PurchaseMaster_ID, string Document_Type, string Type, string newFileName, string filepath, int? Created_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Master_ID",PurchaseMaster_ID),
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


        //Get other purchase detail List
        public IEnumerable<Pa_PurchaseDetails_DAL> Get_PurchaseDetailOtherListByID_DAL(string Temp_ID, int? PurchaseMaster_ID)
        {



            string sp = "Pa_Select_PurchaseDetail_OtherListByID";
            List<Pa_PurchaseDetails_DAL> itemlist = new List<Pa_PurchaseDetails_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@PurchaseMaster_ID", PurchaseMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_PurchaseDetails_DAL item = new Pa_PurchaseDetails_DAL();

                        item.PurchaseDetails_ID = Convert.ToInt32(rdr["PurchaseDetails_ID"]);
                        item.PurchaseMaster_ID = Convert.ToInt32(rdr["PurchaseMaster_ID"]);
                        item.item_ID = rdr["item_ID"].ToString();
                        item.ItemCode = rdr["ItemCode"].ToString();
                        item.ItemName = rdr["ItemName"].ToString();
                        item.item_Description = rdr["item_Description"].ToString();
                        item.Quantity = rdr["Quantity"].ToString();
                        item.UOM = rdr["UOM"].ToString();
                        item.Unit_Price = rdr["Unit_Price"].ToString();
                        item.Currency_ID = rdr["Currency_ID"].ToString();
                        item.Currency_Rate = rdr["Currency_Rate"].ToString();
                        item.Discount = rdr["Discount"].ToString();
                        item.VAT_Rate = rdr["VAT_Rate"].ToString();
                        item.VAT_Exp = rdr["VAT_Exp"].ToString();

                        item.Amount = rdr["Amount"].ToString();
                        item.TtlAmount = rdr["TtlAmount"].ToString();
                        item.Amount_Others = rdr["Amount_Others"].ToString();


                        item.SubTotal = rdr["SubTotal"].ToString();
                        item.Vat_ExpTotal = rdr["Vat_ExpTotal"].ToString();
                        item.DiscountTotal = rdr["DiscountTotal"].ToString();
                        item.Grand_Total = rdr["Grand_Total"].ToString();
                        item.location_ID = rdr["location_ID"].ToString();
                        item.Serial = rdr["SerialNo"].ToString();
                        item.Barcode = rdr["Barcode"].ToString();
                        item.LocationName = rdr["ItemLocationName"].ToString();




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



        //Uptate purchase detail others
        public string UpdatePurchaseDetail_Other_DAL(int? PurchaseDetails_ID, int? item_ID, string item_Description, double? Quantity, string UOM, int? Currency_ID, double? Currency_Rate, double? Unit_Price, int? VAT_Rate,
           double? VAT_Exp, double? Discount, double? Amount, double? TtlAmount, double? Amount_Others, string Temp_ID, int? c_ID, int? Modified_by, int Location_ID, string Serial, string Barcode)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@PurchaseDetails_ID",PurchaseDetails_ID),
                new SqlParameter("@item_ID",item_ID),
                new SqlParameter("@item_Description",item_Description),
                new SqlParameter("@Quantity",Quantity),
                 new SqlParameter("@UOM",UOM),

                 new SqlParameter("@Currency_ID",Currency_ID),
                 new SqlParameter("@Currency_Rate",Currency_Rate),
                 new SqlParameter("@Unit_Price",Unit_Price),

                    new SqlParameter("@VAT_Rate",VAT_Rate),
                       new SqlParameter("@VAT_Exp",VAT_Exp),
                          new SqlParameter("@Discount",Discount),
                             new SqlParameter("@Amount",Amount),
                                new SqlParameter("@TtlAmount",TtlAmount),


                 new SqlParameter("@Amount_Others",Amount_Others),

                  new SqlParameter("@Temp_ID",Temp_ID),


                   new SqlParameter("@Modified_by",Modified_by),
                                 new SqlParameter("@Serial",Serial),
                                 new SqlParameter("@Barcode",Barcode),

                   new SqlParameter("@Location_ID",Location_ID),
                   new SqlParameter("@c_ID",c_ID)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_PurchaseDetail_Other", paramArray).Tables[0];
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


        //Update Other purchase Master
        public string UpdateOtherPurchaseMaster_DAL(int? PurchaseMaster_ID, int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string PurchaseDate, string Temp_ID, int? c_ID, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@PurchaseMaster_ID",PurchaseMaster_ID),
                new SqlParameter("@Vendor_ID",Vendor_ID),
                new SqlParameter("@Vendor_PruchaseTo",Vendor_PruchaseTo),
                new SqlParameter("@Vendor_Address",Vendor_Address),
                 new SqlParameter("@PurchaseDate",PurchaseDate),

                 new SqlParameter("@Temp_ID",Temp_ID),
                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Modified_by",Modified_by)




            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_PurchaseMaster_Other", paramArray).Tables[0];
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


        //Insert Other purchase Master
        public string Insert_OtherPurchaseMaster_DAL(int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string PurchaseType, string PurchaseDate, string Temp_ID, int? c_ID, int? Created_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Vendor_ID",Vendor_ID),
                new SqlParameter("@Vendor_PruchaseTo",Vendor_PruchaseTo),
                new SqlParameter("@Vendor_Address",Vendor_Address),
                new SqlParameter("@PurchaseType",PurchaseType),
                 new SqlParameter("@PurchaseDate",PurchaseDate),

                 new SqlParameter("@Temp_ID",Temp_ID),
                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Created_by",Created_by)




            };


                var response = dbLayer.SP_DataTable_return("Pa_Insert_PurchaseMaster_Other", paramArray).Tables[0];
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


        //Get new purchase master  List
        public IEnumerable<Pa_PurchaseMaster_DAL> Get_PurchaseMasterList_Other_DAL(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID)
        {


            string sp = "Pa_Select_PurchaseMaster_Other_List";
            List<Pa_PurchaseMaster_DAL> itemlist = new List<Pa_PurchaseMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                    cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                    cmd.Parameters.AddWithValue("@From_Date", From_Date);
                    cmd.Parameters.AddWithValue("@To_Date", To_Date);
                    cmd.Parameters.AddWithValue("@Status_ID", Status_ID);
                    cmd.Parameters.AddWithValue("@c_id", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_PurchaseMaster_DAL item = new Pa_PurchaseMaster_DAL();


                        item.PurchaseMaster_ID = Convert.ToInt32(rdr["PurchaseMaster_ID"]);
                        item.Vendor_Name = rdr["Vendor_Name"].ToString();
                        item.PurchaseRef = rdr["PurchaseRef"].ToString();
                        item.PurchaseDate = rdr["PurchaseDate"].ToString();
                        item.PurchaseStatus = rdr["PurchaseStatus"].ToString();
                        item.Total = rdr["Total"].ToString();
                        item.VAT_Exp = rdr["VAT_Exp"].ToString();

                        item.Total_Amount = rdr["Total_Amount"].ToString();
                        item.Paid = rdr["Paid"].ToString();
                        item.Bal_Due = rdr["Bal_Due"].ToString();

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



        //Check If purchase Ref Exist In PurchaseMaster table
        public string CheckIfRefExistInPurchaseMaster_DAL(string PurchaseRef)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@PurchaseRef",PurchaseRef)



            };


                var response = dbLayer.SP_DataTable_return("Check_If_PurchaseMasterRefExists", paramArray).Tables[0];
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




        //Insert  purchase return detail List
        public string Insert_PurhaseReturnDetail_DAL(string Temp_ID, string PurchaseRef)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {


                               new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@PurchaseRef",PurchaseRef)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Get_PurchaseReturnDetails", paramArray).Tables[0];
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





        //--getting purchase master return other by id
        public Pa_PurchaseMaster_DAL Get_PurchaseMaster_Return_ByID_DAL(int? PurchaseMaster_ID)
        {

            string sp = "Pa_Select_PurchaseMaster_Return_ByID";

            Pa_PurchaseMaster_DAL pm = new Pa_PurchaseMaster_DAL();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PurchaseMaster_ID", PurchaseMaster_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.PurchaseMaster_ID = Convert.ToInt32(dr["PurchaseMaster_ID"]);
                                pm.PurchaseRef = dr["PurchaseRef"].ToString();
                                pm.Vendor_ID = Convert.ToInt32(dr["Vendor_ID"]);
                                pm.PurchaseStatus_ID = dr["PurchaseStatus_ID"].ToString();
                                pm.PurchaseStatus = dr["PurchaseStatus"].ToString();
                                pm.Vendor_PruchaseTo = dr["Vendor_PruchaseTo"].ToString();
                                pm.Vendor_Address = dr["Vendor_Address"].ToString();
                                pm.PurchaseDate = dr["PurchaseDate"].ToString();

                                pm.Created_at = dr["Created_at"].ToString();
                                pm.Created_by = dr["Created_by"].ToString();
                                pm.Modified_at = dr["Modified_at"].ToString();
                                pm.Modified_by = dr["Modified_by"].ToString();
                                pm.OtherREF = dr["CancelRef"].ToString();

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


            return pm;
        }


        //Get other purchase detail List
        public IEnumerable<Pa_PurchaseDetails_DAL> Get_PurchaseDetailReturnListByID_DAL(string Temp_ID, int? PurchaseMaster_ID)
        {



            string sp = "Pa_Select_PurchaseDetail_ReturnListByID";
            List<Pa_PurchaseDetails_DAL> itemlist = new List<Pa_PurchaseDetails_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@PurchaseMaster_ID", PurchaseMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_PurchaseDetails_DAL item = new Pa_PurchaseDetails_DAL();

                        item.PurchaseDetails_ID = Convert.ToInt32(rdr["PurchaseDetails_ID"]);
                        item.PurchaseMaster_ID = Convert.ToInt32(rdr["PurchaseMaster_ID"]);
                        item.item_ID = rdr["item_ID"].ToString();
                        item.item_Description = rdr["item_Description"].ToString();
                        item.OthersSpecs = rdr["OthersSpecs"].ToString();
                        item.Make = rdr["Make"].ToString();
                        item.ModelDesciption = rdr["ModelDesciption"].ToString();
                        item.ColorName = rdr["ColorName"].ToString();
                        item.Quantity = rdr["Quantity"].ToString();
                        item.UOM = rdr["UOM"].ToString();
                        item.Unit_Price = rdr["Unit_Price"].ToString();
                        item.Currency_ID = rdr["Currency_ID"].ToString();
                        item.Currency_Rate = rdr["Currency_Rate"].ToString();
                        item.VAT_Exp = rdr["VAT_Exp"].ToString();
                        item.VAT_Rate = rdr["VAT_Rate"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.Amount_Others = rdr["Amount_Others"].ToString();
                        item.Discount = rdr["Discount"].ToString();
                        item.SubTotal = rdr["SubTotal"].ToString();
                        item.Vat_ExpTotal = rdr["Vat_ExpTotal"].ToString();
                        item.DiscountTotal = rdr["DiscountTotal"].ToString();
                        item.Grand_Total = rdr["Grand_Total"].ToString();
                        

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


        //Insert Return purchase Master
        public string Insert_PurchaseMaster_Return_DAL(int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string PurchaseType, string PurchaseDate, string Temp_ID, int? c_ID, int? Created_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Vendor_ID",Vendor_ID),
                new SqlParameter("@Vendor_PruchaseTo",Vendor_PruchaseTo),
                new SqlParameter("@Vendor_Address",Vendor_Address),
                new SqlParameter("@PurchaseType",PurchaseType),
                 new SqlParameter("@PurchaseDate",PurchaseDate),

                 new SqlParameter("@Temp_ID",Temp_ID),
                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Created_by",Created_by)




            };


                var response = dbLayer.SP_DataTable_return("Pa_Insert_PurchaseMaster_Return", paramArray).Tables[0];
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


        //Update return purchase Master
        public string Update_Return_PurchaseMaster_DAL(int? PurchaseMaster_ID, int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string PurchaseDate, string Temp_ID, int? c_ID, int? Modified_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@PurchaseMaster_ID",PurchaseMaster_ID),
                new SqlParameter("@Vendor_ID",Vendor_ID),
                new SqlParameter("@Vendor_PruchaseTo",Vendor_PruchaseTo),
                new SqlParameter("@Vendor_Address",Vendor_Address),
                 new SqlParameter("@PurchaseDate",PurchaseDate),

                 new SqlParameter("@Temp_ID",Temp_ID),
                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Modified_by",Modified_by)




            };


                var response = dbLayer.SP_DataTable_return("Update_PurchaseMaster_Return", paramArray).Tables[0];
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


        public IEnumerable<Pa_PurchaseDetails_DAL> Get_PurchaseDetailListByID_DAL(string Temp_ID, int? PurchaseMaster_ID)
        {



            string sp = "Pa_Select_PurchaseDetailListByID";
            List<Pa_PurchaseDetails_DAL> itemlist = new List<Pa_PurchaseDetails_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@PurchaseMaster_ID", PurchaseMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_PurchaseDetails_DAL item = new Pa_PurchaseDetails_DAL();

                        item.PurchaseDetails_ID = Convert.ToInt32(rdr["PurchaseDetails_ID"]);
                        item.PurchaseMaster_ID = Convert.ToInt32(rdr["PurchaseMaster_ID"]);
                        item.Make_ID = rdr["Make_ID"].ToString();
                        item.Model_ID = rdr["Model_ID"].ToString();
                        item.Color_ID = rdr["Color_ID"].ToString();
                        item.OthersSpecs = rdr["OthersSpecs"].ToString();
                        item.Quantity = rdr["Quantity"].ToString();
                        item.Currency_ID = rdr["Currency_ID"].ToString();
                        item.Currency_Rate = rdr["Currency_Rate"].ToString();
                        item.Unit_Price = rdr["Unit_Price"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.Amount_Others = rdr["Amount_Others"].ToString();

                        item.Make = rdr["Make"].ToString();
                        item.ColorName = rdr["ColorName"].ToString();
                        item.ModelDesciption = rdr["ModelDesciption"].ToString();
                        item.Currency_Name = rdr["Currency_Name"].ToString();

                        item.SubTotal = rdr["SubTotal"].ToString();
                        item.Vat_ExpTotal = rdr["Vat_ExpTotal"].ToString();
                        item.DiscountTotal = rdr["DiscountTotal"].ToString();
                        item.Grand_Total = rdr["Grand_Total"].ToString();
                        item.Chassis_No = rdr["Chassis_No"].ToString();
                        item.ModelYear = rdr["ModelYear"].ToString();
                        item.Color_Interior_ID = Convert.ToInt32(rdr["Color_Interior_ID"]);
                        item.locID = Convert.ToInt32(rdr["Loc_ID"]);
                        item.mileage = rdr["mileage"].ToString();


                        

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


        public string Delete_Attachment_PurchaseNew(int? Attachment_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Attachment_ID",Attachment_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_Attachment", paramArray).Tables[0];
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


        public string Clear_ImportHolder()
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
            };


                var response = dbLayer.SP_DataTable_return("Clear_ImportHolder", paramArray).Tables[0];
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

        public string InsertImportHolder_DAL(string MAKENAME, string MAKEMODELNAME, string CHASSIS_NO, string MODEL,
            string COLORNAME, string COLOR_INT, string PRICE, string PRICE_RATE, string FREIGHT, string FR_RATE,
string SELLING_PRICE, string TRANSMISSION, string DOOR, string DRIVE, string ENGINE_NO, string WEIGHT, string HS_CODE,
string ENGINE_POWER, string MILEAGE, string VEHICLE_CC, string USED_NEW, string OPTIONS, string FUEL_TYPE, string MAKECATEGORYNAME, string VENDORNAME,
string PURCHASEDATE, string PURCHASE_REF, string BL_NO, string SHIP_NAME, string SHIPDATE, string LEAVE_DATE,
string ENTRY_DATE, string BOE, string LOCATION, string MAKECOUNTRYNAME, string AVAILABILITY,
string STOCK_TYPE, string SHOWROOM, string COMMENTS, string OPTION_CODE, string KEYNO, string PRODUCTION_DATE,
string EXP_TRANSPORT, string EXP_AGENT_COMMISSION, string EXP_CUSTOM_DUTY, string EXP_OTHERS, string EXP_CC,
string EXP_MAINT, string EXP_COMM_OTHERS, string EXP_PAPER, string OTHER_REF, string TAX10, string AUCTIONFEE, string REKSO, string RECYCLE, string LOADING,
string AUCTIONNAME, string CONT_NO, string OTHERS_JP, string JP_CHARGES, int C_ID, int USER_ID)

        {

            //---validation for some fields

            //AUCTION_FEE = String.IsNullOrEmpty(AUCTION_FEE) || String.IsNullOrWhiteSpace(AUCTION_FEE) ? "0" : AUCTION_FEE;
            //RECYLE_FEE = String.IsNullOrEmpty(RECYLE_FEE) || String.IsNullOrWhiteSpace(RECYLE_FEE) ? "0" : RECYLE_FEE;
            //LA_PRICE = String.IsNullOrEmpty(LA_PRICE) || String.IsNullOrWhiteSpace(LA_PRICE) ? "0" : LA_PRICE;
            //RUKUSO = String.IsNullOrEmpty(RUKUSO) || String.IsNullOrWhiteSpace(RUKUSO) ? "0" : RUKUSO;
            //GST = String.IsNullOrEmpty(GST) || String.IsNullOrWhiteSpace(GST) ? "0" : GST;
            EXP_TRANSPORT = String.IsNullOrEmpty(EXP_TRANSPORT) || String.IsNullOrWhiteSpace(EXP_TRANSPORT) ? "0" : EXP_TRANSPORT;
            EXP_AGENT_COMMISSION = String.IsNullOrEmpty(EXP_AGENT_COMMISSION) || String.IsNullOrWhiteSpace(EXP_AGENT_COMMISSION) ? "0" : EXP_AGENT_COMMISSION;
            EXP_CUSTOM_DUTY = String.IsNullOrEmpty(EXP_CUSTOM_DUTY) || String.IsNullOrWhiteSpace(EXP_CUSTOM_DUTY) ? "0" : EXP_CUSTOM_DUTY;
            EXP_OTHERS = String.IsNullOrEmpty(EXP_OTHERS) || String.IsNullOrWhiteSpace(EXP_OTHERS) ? "0" : EXP_OTHERS;
            EXP_CC = String.IsNullOrEmpty(EXP_CC) || String.IsNullOrWhiteSpace(EXP_CC) ? "0" : EXP_CC;
            EXP_MAINT = String.IsNullOrEmpty(EXP_MAINT) || String.IsNullOrWhiteSpace(EXP_MAINT) ? "0" : EXP_MAINT;
            EXP_COMM_OTHERS = String.IsNullOrEmpty(EXP_COMM_OTHERS) || String.IsNullOrWhiteSpace(EXP_COMM_OTHERS) ? "0" : EXP_COMM_OTHERS;
            EXP_PAPER = String.IsNullOrEmpty(EXP_PAPER) || String.IsNullOrWhiteSpace(EXP_PAPER) ? "0" : EXP_PAPER;

            TAX10 = String.IsNullOrEmpty(TAX10) || String.IsNullOrWhiteSpace(TAX10) ? "0" : TAX10;
            AUCTIONFEE = String.IsNullOrEmpty(AUCTIONFEE) || String.IsNullOrWhiteSpace(AUCTIONFEE) ? "0" : AUCTIONFEE;
            REKSO = String.IsNullOrEmpty(REKSO) || String.IsNullOrWhiteSpace(REKSO) ? "0" : REKSO;
            RECYCLE = String.IsNullOrEmpty(RECYCLE) || String.IsNullOrWhiteSpace(RECYCLE) ? "0" : RECYCLE;
            LOADING = String.IsNullOrEmpty(LOADING) || String.IsNullOrWhiteSpace(LOADING) ? "0" : LOADING;

            OTHERS_JP = String.IsNullOrEmpty(OTHERS_JP) || String.IsNullOrWhiteSpace(OTHERS_JP) ? "0" : OTHERS_JP;
            JP_CHARGES = String.IsNullOrEmpty(JP_CHARGES) || String.IsNullOrWhiteSpace(JP_CHARGES) ? "0" : JP_CHARGES;





            string msg = "";
            try
            {




                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@MAKENAME",MAKENAME),
                new SqlParameter("@MAKEMODELNAME",MAKEMODELNAME),
                 new SqlParameter("@CHASSIS_NO",CHASSIS_NO),
                  new SqlParameter("@MODEL",MODEL),

                   new SqlParameter("@COLORNAME",COLORNAME),
                      new SqlParameter("@COLOR_INT",COLOR_INT),
                    new SqlParameter("@PRICE",PRICE),
                     new SqlParameter("@PRICE_RATE",PRICE_RATE),
                      new SqlParameter("@FREIGHT",FREIGHT),
                       new SqlParameter("@FR_RATE",FR_RATE),

                        new SqlParameter("@SELLING_PRICE",SELLING_PRICE),
                         new SqlParameter("@TRANSMISSION",TRANSMISSION),
                          new SqlParameter("@DOOR",DOOR),
                           new SqlParameter("@DRIVE",DRIVE),
                            new SqlParameter("@ENGINE_NO",ENGINE_NO),
                             new SqlParameter("@WEIGHT",WEIGHT),
                              new SqlParameter("@HS_CODE",HS_CODE),
                               new SqlParameter("@ENGINE_POWER",ENGINE_POWER),
                              new SqlParameter("@MILEAGE",MILEAGE),
                               new SqlParameter("@VEHICLE_CC",VEHICLE_CC),
                                new SqlParameter("@USED_NEW",USED_NEW),
                                 new SqlParameter("@OPTIONS",OPTIONS),
                                  new SqlParameter("@FUEL_TYPE",FUEL_TYPE),
                                   new SqlParameter("@MAKECATEGORYNAME",MAKECATEGORYNAME),
                                    new SqlParameter("@VENDORNAME",VENDORNAME),
                                     new SqlParameter("@PURCHASEDATE",PURCHASEDATE),
                                      new SqlParameter("@PURCHASE_REF",PURCHASE_REF),
                                       new SqlParameter("@BL_NO",BL_NO),
                                       new SqlParameter("@SHIP_NAME",SHIP_NAME),
                                       new SqlParameter("@SHIPDATE",SHIPDATE),
                                       new SqlParameter("@LEAVE_DATE",LEAVE_DATE),
                                       new SqlParameter("@ENTRY_DATE",ENTRY_DATE),
                                       new SqlParameter("@BOE",BOE),
                                        new SqlParameter("@LOCATION",LOCATION),
                                       new SqlParameter("@MAKECOUNTRYNAME",MAKECOUNTRYNAME),
                                       new SqlParameter("@AVAILABILITY",AVAILABILITY),
                                       new SqlParameter("@STOCK_TYPE",STOCK_TYPE),
                                       new SqlParameter("@SHOWROOM",SHOWROOM),
                                       new SqlParameter("@COMMENTS",COMMENTS),
                                        new SqlParameter("@OPTION_CODE",OPTION_CODE),
                                         new SqlParameter("@KEYNO",KEYNO),
                                         new SqlParameter("@PRODUCTION_DATE",PRODUCTION_DATE),
                                          new SqlParameter("@EXP_TRANSPORT",EXP_TRANSPORT),
                                                new SqlParameter("@EXP_AGENT_COMMISSION",EXP_AGENT_COMMISSION),
                                                 new SqlParameter("@EXP_CUSTOM_DUTY",EXP_CUSTOM_DUTY),
                                                  new SqlParameter("@EXP_OTHERS",EXP_OTHERS),
                                                   new SqlParameter("@EXP_CC",EXP_CC),
                                                    new SqlParameter("@EXP_MAINT",EXP_MAINT),
                                                     new SqlParameter("@EXP_COMM_OTHERS",EXP_COMM_OTHERS),
                                                      new SqlParameter("@EXP_PAPER",EXP_PAPER),
                                                       new SqlParameter("@OTHER_REF",OTHER_REF),
                                                   new SqlParameter("@TAX10",TAX10),
                                                          new SqlParameter("@AUCTIONFEE",AUCTIONFEE),
                                                            new SqlParameter("@REKSO",REKSO),
                                                     new SqlParameter("@RECYCLE",RECYCLE),
                                                      new SqlParameter("@LOADING",LOADING),

                                                         new SqlParameter("@AUCTIONNAME",AUCTIONNAME),
                                                            new SqlParameter("@CONT_NO",CONT_NO),
                                                               new SqlParameter("@OTHERS_JP",OTHERS_JP),
                                                                  new SqlParameter("@JP_CHARGES",JP_CHARGES),
                                                       new SqlParameter("@C_ID",C_ID),
                                                       new SqlParameter("@USER_ID",USER_ID)

            };


                var response = dbLayer.SP_DataTable_return("Insert_ImportHolder_BulkUploadStock", paramArray).Tables[0];
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




        public IEnumerable<importHolder> Get_StockPuchaseBulkList_DAL()
        {



            string sp = "Select_ImportHolderBulkStockList";
            List<importHolder> itemlist = new List<importHolder>();

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
                        importHolder item = new importHolder();


                        item.MAKENAME = rdr["MAKENAME"].ToString();
                        item.MAKEMODELNAME = rdr["MAKEMODELNAME"].ToString();
                        item.CHASSIS_NO = rdr["CHASSIS_NO"].ToString();
                        item.MODEL = rdr["MODEL"].ToString();
                        item.COLORNAME = rdr["COLORNAME"].ToString();
                        item.COLOR_INT = rdr["COLOR_INT"].ToString();
                        item.PRICE = rdr["PRICE"].ToString();

                        item.PRICE_RATE = rdr["PRICE_RATE"].ToString();
                        item.FREIGHT = rdr["FREIGHT"].ToString();
                        item.FR_RATE = rdr["FR_RATE"].ToString();
                        item.SELLING_PRICE = rdr["SELLING_PRICE"].ToString();
                        item.TRANSMISSION = rdr["TRANSMISSION"].ToString();
                        item.DOOR = rdr["DOOR"].ToString();
                        item.DRIVE = rdr["DRIVE"].ToString();
                        item.ENGINE_NO = rdr["ENGINE_NO"].ToString();
                        item.WEIGHT = rdr["WEIGHT"].ToString();
                        item.HS_CODE = rdr["HS_CODE"].ToString();
                        item.ENGINE_POWER = rdr["ENGINE_POWER"].ToString();
                        item.MILEAGE = rdr["MILEAGE"].ToString();
                        item.VEHICLE_CC = rdr["VEHICLE_CC"].ToString();
                        item.USED_NEW = rdr["USED_NEW"].ToString();
                        item.OPTIONS = rdr["OPTIONS"].ToString();
                        item.FUEL_TYPE = rdr["FUEL_TYPE"].ToString();
                        item.MAKECATEGORYNAME = rdr["MAKECATEGORYNAME"].ToString();
                        item.VENDORNAME = rdr["VENDORNAME"].ToString();
                        item.PURCHASEDATE = rdr["PURCHASEDATE"].ToString();
                        item.PURCHASE_REF = rdr["PURCHASE_REF"].ToString();
                        item.BL_NO = rdr["BL_NO"].ToString();
                        item.SHIP_NAME = rdr["SHIP_NAME"].ToString();
                        item.SHIPDATE = rdr["SHIPDATE"].ToString();
                        item.LEAVE_DATE = rdr["LEAVE_DATE"].ToString();
                        item.ENTRY_DATE = rdr["ENTRY_DATE"].ToString();
                        item.BOE = rdr["BOE"].ToString();
                        item.LOCATION = rdr["LOCATION"].ToString();
                        item.MAKECOUNTRYNAME = rdr["MAKECOUNTRYNAME"].ToString();
                        item.AVAILABILITY = rdr["AVAILABILITY"].ToString();
                        item.STOCK_TYPE = rdr["STOCK_TYPE"].ToString();
                        item.SHOWROOM = rdr["SHOWROOM"].ToString();
                        item.COMMENTS = rdr["COMMENTS"].ToString();
                        item.OPTION_CODE = rdr["OPTION_CODE"].ToString();
                        item.KEYNO = rdr["KEYNO"].ToString();

                        item.PRODUCTION_DATE = rdr["PRODUCTION_DATE"].ToString();

                        item.EXP_TRANSPORT = rdr["EXP_TRANSPORT"].ToString();
                        item.EXP_AGENT_COMMISSION = rdr["EXP_AGENT_COMMISSION"].ToString();
                        item.EXP_CUSTOM_DUTY = rdr["EXP_CUSTOM_DUTY"].ToString();
                        item.EXP_OTHERS = rdr["EXP_OTHERS"].ToString();
                        item.EXP_CC = rdr["EXP_CC"].ToString();
                        item.EXP_MAINT = rdr["EXP_MAINT"].ToString();
                        item.EXP_COMM_OTHERS = rdr["EXP_COMM_OTHERS"].ToString();
                        item.EXP_PAPER = rdr["EXP_PAPER"].ToString();

                        item.OTHER_REF = rdr["OTHER_REF"].ToString();
                        item.TAX10 = rdr["TAX10"].ToString();
                        item.AUCTIONFEE = rdr["AUCTIONFEE"].ToString();
                        item.REKSO = rdr["REKSO"].ToString();
                        item.RECYCLE = rdr["RECYCLE"].ToString();
                        item.LOADING = rdr["LOADING"].ToString();

                        item.AUCTIONNAME = rdr["AUCTIONNAME"].ToString();
                        item.CONT_NO = rdr["CONT_NO"].ToString();
                        item.OTHERS_JP = rdr["OTHERS_JP"].ToString();
                        item.JP_CHARGES = rdr["JP_CHARGES"].ToString();



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




        public string InsertImportFailure_DAL(string CHASSIS_NO, string Message)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@CHASSIS_NO",CHASSIS_NO),
                 new SqlParameter("@Message",Message)



            };


                var response = dbLayer.SP_DataTable_return("Insert_ImportFailure", paramArray).Tables[0];
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


        public string InsertBulkFromImportHolder_DAL(string Temp_ID, int User_ID,int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@Created_By_User_ID",User_ID),
                     new SqlParameter("@c_ID",c_ID)
            };


                var response = dbLayer.SP_DataTable_return_UploadBulk("Push_ImportData", paramArray).Tables[0];
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



        public IEnumerable<StockReport> get_Stock_in_PO_byPurchaseRef_DAL(string PurchaseRef)
        {
            List<StockReport> RD = new List<StockReport>();
            string sp = "get_Stock_in_PO_byPurchaseRef";



            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                StockReport o = new StockReport();



                                o.Chassis_No = dr["Chassis_No"].ToString();

                                o.PurchaseRef = dr["PurchaseRef"].ToString();

                                o.Total_Cost = dr["Total_Cost"].ToString();


                                o.Color = dr["Color"].ToString();
                                o.ModelDesciption = dr["ModelDesciption"].ToString();
                                o.Make = dr["Make"].ToString();


                                RD.Add(o);













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


            return RD;


        }

        //public IEnumerable<Pa_PurchaseMaster_DAL> Get_PurchaseMasterList_DAL(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID)
        //{


        //    string sp = "Pa_Select_PurchaseMasterList";
        //    List<Pa_PurchaseMaster_DAL> itemlist = new List<Pa_PurchaseMaster_DAL>();

        //    try
        //    {
        //        using (con = new SqlConnection(Constr))
        //        {
        //            con.Open();
        //            var cmd = new SqlCommand(sp, con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
        //            cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
        //            cmd.Parameters.AddWithValue("@From_Date", From_Date);
        //            cmd.Parameters.AddWithValue("@To_Date", To_Date);
        //            cmd.Parameters.AddWithValue("@Status_ID", Status_ID);
        //            cmd.Parameters.AddWithValue("@c_ID", c_ID);


        //            SqlDataReader rdr = cmd.ExecuteReader();


        //            while (rdr.Read())
        //            {
        //                Pa_PurchaseMaster_DAL item = new Pa_PurchaseMaster_DAL();


        //                item.PurchaseMaster_ID = Convert.ToInt32(rdr["PurchaseMaster_ID"]);
        //                item.Vendor_Name = rdr["Vendor_Name"].ToString();
        //                item.PurchaseRef = rdr["PurchaseRef"].ToString();
        //                item.PurchaseDate = rdr["PurchaseDate"].ToString();
        //                item.PurchaseStatus = rdr["PurchaseStatus"].ToString();
        //                item.Total = rdr["Total"].ToString();
        //                item.VAT_Exp = rdr["VAT_Exp"].ToString();
        //                item.Discount = rdr["Discount"].ToString();

        //                item.Total_Amount = rdr["Total_Amount"].ToString();
        //                item.Paid = rdr["Paid"].ToString();
        //                item.Bal_Due = rdr["Bal_Due"].ToString();

        //              //  item.TotalVAT_Exp = rdr["TotalVAT_Exp"].ToString();
        //                //item.BeforeVATTotal = rdr["BeforeVATTotal"].ToString();
        //                //item.AfterVATTotal = rdr["AfterVATTotal"].ToString();
        //                //item.TotalPaid = rdr["TotalPaid"].ToString();
        //                //item.TotalBal_due = rdr["TotalBal_due"].ToString();
        //                //item.TotalDiscount = rdr["TotalDiscount"].ToString();


        //                //item.Created_at = rdr["Created_at"].ToString();
        //                //item.Created_by = rdr["Created_by"].ToString();
        //                //item.Modified_at = rdr["Modified_at"].ToString();
        //                //item.Modified_by = rdr["Modified_by"].ToString();

        //                itemlist.Add(item);

        //            }

        //            con.Close();

        //        }

        //        return itemlist;

        //    }
        //    catch (Exception ex)
        //    {
        //        string ErrorString = ex.Message;
        //        return null;
        //    }


        //}

        public IEnumerable<Pa_PurchaseMaster_DAL> Get_PurchaseMasterList_Other_DAL1(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID)
        {


            string sp = "Pa_Select_PurchaseMaster_Other_List";
            List<Pa_PurchaseMaster_DAL> itemlist = new List<Pa_PurchaseMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                    cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                    cmd.Parameters.AddWithValue("@From_Date", From_Date);
                    cmd.Parameters.AddWithValue("@To_Date", To_Date);
                    cmd.Parameters.AddWithValue("@Status_ID", Status_ID);
                    cmd.Parameters.AddWithValue("@c_id", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_PurchaseMaster_DAL item = new Pa_PurchaseMaster_DAL();


                        item.PurchaseMaster_ID = Convert.ToInt32(rdr["PurchaseMaster_ID"]);
                        item.Vendor_Name = rdr["Vendor_Name"].ToString();
                        item.PurchaseRef = rdr["PurchaseRef"].ToString();
                        item.PurchaseDate = rdr["PurchaseDate"].ToString();
                        item.PurchaseStatus = rdr["PurchaseStatus"].ToString();
                        item.Total = rdr["Total"].ToString();
                        item.VAT_Exp = rdr["VAT_Exp"].ToString();

                        item.Total_Amount = rdr["Total_Amount"].ToString();
                        item.Paid = rdr["Paid"].ToString();
                        item.Bal_Due = rdr["Bal_Due"].ToString();

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


        //Get new purchase master  List
        public IEnumerable<Pa_PurchaseMaster_DAL> Get_PurchaseMasterList_TTL(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID,string ChassisNo, int? c_ID)
        {


            string sp = "Pa_Select_PurchaseMasterList_TTL";
            List<Pa_PurchaseMaster_DAL> itemlist = new List<Pa_PurchaseMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                    cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                    cmd.Parameters.AddWithValue("@From_Date", From_Date);
                    cmd.Parameters.AddWithValue("@To_Date", To_Date);
                    cmd.Parameters.AddWithValue("@Status_ID", Status_ID);
                    cmd.Parameters.AddWithValue("@ChassisNo", ChassisNo);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_PurchaseMaster_DAL item = new Pa_PurchaseMaster_DAL();



                        //item.AfterVATTotal = rdr["AfterVATTotal"].ToString();
                        item.Total = rdr["Total"].ToString();
                        item.TotalVAT_Exp = rdr["TotalVAT_Exp"].ToString();
                        item.TotalDiscount = rdr["TotalDiscount"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();

                        item.TotalPaid = rdr["TotalPaid"].ToString();
                        item.TotalBal_due = rdr["TotalBal_due"].ToString();





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

        #region GoodsReceived

        public Pa_GoodsReceived_Master Get_ReceivedMaster_ByID_DAL(int? GRVMaster_ID)
        {

            string sp = "Pa_Select_GRVMaster_OtherByID";

            Pa_GoodsReceived_Master pm = new Pa_GoodsReceived_Master();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@GRVMaster_ID", GRVMaster_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.GRVMaster_ID = Convert.ToInt32(dr["GRVMaster_ID"]);
                                pm.GRVRef = dr["GRVRef"].ToString();
                                pm.Vendor_ID = Convert.ToInt32(dr["Vendor_ID"]);
                                pm.GRVStatus_ID = dr["GRVStatus_ID"].ToString();
                                pm.GRVStatus = dr["GRVStatus"].ToString();
                                pm.Vendor_PruchaseTo = dr["Vendor_PruchaseTo"].ToString();
                                pm.Vendor_Address = dr["Vendor_Address"].ToString();
                                pm.GRVDate = dr["GRVDate"].ToString();
                                pm.Discount = dr["Discount"].ToString();
                                pm.VAT_Rate = dr["VAT_Rate"].ToString();
                                pm.Total = dr["SubTotal"].ToString();
                                pm.Created_at = dr["Created_at"].ToString();
                                pm.Created_by = dr["Created_by"].ToString();
                                pm.Modified_at = dr["Modified_at"].ToString();
                                pm.Modified_by = dr["Modified_by"].ToString();
                                pm.Vendor_Name = dr["PartnerName"].ToString();
                                pm.PurchaseMaster_ID = Convert.ToInt32(dr["PurchaseMaster_Id"]);

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


            return pm;
        }
        public IEnumerable<Pa_GoodsReceived_Dtl> Get_ReceivedDetailListByID_DAL(string Temp_ID, int? GRVMaster_ID)
        {



            string sp = "Pa_Select_GRVDetail_OtherListByID";
            List<Pa_GoodsReceived_Dtl> itemlist = new List<Pa_GoodsReceived_Dtl>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@GRVMaster_ID", GRVMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_GoodsReceived_Dtl item = new Pa_GoodsReceived_Dtl();

                        item.GRVDetails_ID = Convert.ToInt32(rdr["GRVDetails_ID"]);
                        item.GRVMaster_ID = Convert.ToInt32(rdr["GRVMaster_ID"]);
                        item.item_ID = rdr["item_ID"].ToString();
                        item.ItemCode = rdr["ItemCode"].ToString();
                        item.ItemName = rdr["ItemName"].ToString();
                        item.item_Description = rdr["item_Description"].ToString();
                        item.Quantity = rdr["Quantity"].ToString();
                        item.UOM = rdr["UOM"].ToString();
                        item.Unit_Price = rdr["Unit_Price"].ToString();
                        item.Currency_ID = rdr["Currency_ID"].ToString();
                        item.Currency_Rate = rdr["Currency_Rate"].ToString();
                        item.Discount = rdr["Discount"].ToString();
                        item.VAT_Rate = rdr["VAT_Rate"].ToString();
                        item.VAT_Exp = rdr["VAT_Exp"].ToString();

                        item.Amount = rdr["Amount"].ToString();
                        item.TtlAmount = rdr["TtlAmount"].ToString();
                        item.Amount_Others = rdr["Amount_Others"].ToString();


                        item.SubTotal = rdr["SubTotal"].ToString();
                        item.Vat_ExpTotal = rdr["Vat_ExpTotal"].ToString();
                        item.DiscountTotal = rdr["DiscountTotal"].ToString();
                        item.Grand_Total = rdr["Grand_Total"].ToString();
                        item.location_ID = rdr["location_ID"].ToString();
                        item.PurchaseQTY = rdr["PurchaseQTY"].ToString();
                        item.LocationName = rdr["ItemLocationName"].ToString();




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
        public string InsertGoodsReceivedDetail_DAL(int? item_ID, string item_Description, double? Quantity, string UOM, int? Currency_ID, double? Currency_Rate, double? Unit_Price, int? VAT_Rate,
          double? VAT_Exp, double? Discount, double? Amount, double? TtlAmount, double? Amount_Others, string Temp_ID, int? c_ID, int? Created_by, int GRVMaster_ID, int Location_ID, int PurchaseDetails_ID, int PurchaseMaster_Id)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@item_ID",item_ID),
                new SqlParameter("@item_Description",item_Description),
                new SqlParameter("@Quantity",Quantity),
                 new SqlParameter("@UOM",UOM),

                 new SqlParameter("@Currency_ID",Currency_ID),
                 new SqlParameter("@Currency_Rate",Currency_Rate),
                 new SqlParameter("@Unit_Price",Unit_Price),

                    new SqlParameter("@VAT_Rate",VAT_Rate),
                       new SqlParameter("@VAT_Exp",VAT_Exp),
                          new SqlParameter("@Discount",Discount),
                             new SqlParameter("@Amount",Amount),
                                new SqlParameter("@TtlAmount",TtlAmount),


                 new SqlParameter("@Amount_Others",Amount_Others),

                  new SqlParameter("@Temp_ID",Temp_ID),
                   new SqlParameter("@Created_by",Created_by),
                   new SqlParameter("@GRVMaster_ID",GRVMaster_ID),
                   new SqlParameter("@PurchaseDetails_ID",PurchaseDetails_ID),
                   new SqlParameter("@PurchaseMaster_Id",PurchaseMaster_Id),
         
                   new SqlParameter("@Location_ID",Location_ID)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Insert_GRVDetail_Others", paramArray).Tables[0];
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





        public string UpdateGoodsReceivedDetail_DAL(int? GRVDetails_ID, int? item_ID, string item_Description, double? Quantity, string UOM, int? Currency_ID, double? Currency_Rate, double? Unit_Price, int? VAT_Rate,
           double? VAT_Exp, double? Discount, double? Amount, double? TtlAmount, double? Amount_Others, string Temp_ID, int? c_ID, int? Modified_by, int Location_ID, string Serial, string Barcode)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@GRVDetails_ID",GRVDetails_ID),
                new SqlParameter("@item_ID",item_ID),
                new SqlParameter("@item_Description",item_Description),
                new SqlParameter("@Quantity",Quantity),
                 new SqlParameter("@UOM",UOM),

                 new SqlParameter("@Currency_ID",Currency_ID),
                 new SqlParameter("@Currency_Rate",Currency_Rate),
                 new SqlParameter("@Unit_Price",Unit_Price),

                    new SqlParameter("@VAT_Rate",VAT_Rate),
                       new SqlParameter("@VAT_Exp",VAT_Exp),
                          new SqlParameter("@Discount",Discount),
                             new SqlParameter("@Amount",Amount),
                                new SqlParameter("@TtlAmount",TtlAmount),


                 new SqlParameter("@Amount_Others",Amount_Others),

                  new SqlParameter("@Temp_ID",Temp_ID),


                   new SqlParameter("@Modified_by",Modified_by),
                          

                   new SqlParameter("@Location_ID",Location_ID)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_GRVDetail_Other", paramArray).Tables[0];
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
        public string InsertGoodsReceivedMaster_DAL(int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string GRVType, string GRVDate, string Temp_ID, int? c_ID, int? Created_by, int PurchaseMaster_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Vendor_ID",Vendor_ID),
                new SqlParameter("@Vendor_PruchaseTo",Vendor_PruchaseTo),
                new SqlParameter("@Vendor_Address",Vendor_Address),
              
                 new SqlParameter("@GRVDate",GRVDate),

                 new SqlParameter("@Temp_ID",Temp_ID),
                 new SqlParameter("@c_ID",c_ID),
                 new SqlParameter("@PurchaseMaster_ID",PurchaseMaster_ID),

                 new SqlParameter("@Created_by",Created_by)




            };


                var response = dbLayer.SP_DataTable_return("Pa_Insert_GRVMaster_Other", paramArray).Tables[0];
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

        public string UpdateGoodsReceivedMaster_DAL(int? GRVMaster_ID, int? Vendor_ID, string Vendor_PruchaseTo, string Vendor_Address, string GRVDate, string Temp_ID, int? c_ID, int? Modified_by, int PurchaseMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@GRVMaster_ID",GRVMaster_ID),
                new SqlParameter("@Vendor_ID",Vendor_ID),
                new SqlParameter("@Vendor_PruchaseTo",Vendor_PruchaseTo),
                new SqlParameter("@Vendor_Address",Vendor_Address),
                 new SqlParameter("@GRVDate",GRVDate),

                 new SqlParameter("@Temp_ID",Temp_ID),
                 new SqlParameter("@c_ID",c_ID),
                      new SqlParameter("@PurchaseMaster_ID",PurchaseMaster_ID),
                 new SqlParameter("@Modified_by",Modified_by)




            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_GRVMaster_Other", paramArray).Tables[0];
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


        public string DeleteGoodsReceivedMaster_DAL(int? GRVMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@GRVMaster_ID",GRVMaster_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_GRV_Master", paramArray).Tables[0];
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
        public string DeleteGoodsReceived_Material_In_DAL(int? GRVDetails_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@GRVDetails_ID",GRVDetails_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_GRV_Dtl", paramArray).Tables[0];
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

        public IEnumerable<Pa_GoodsReceived_Master> Get_GoodsReceivedMaster_DAL(string GRVRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, int? c_ID)
        {


            string sp = "Pa_Select_GRVMaster_Other_List";
            List<Pa_GoodsReceived_Master> itemlist = new List<Pa_GoodsReceived_Master>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GRVRef", GRVRef);
                    cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                    cmd.Parameters.AddWithValue("@From_Date", From_Date);
                    cmd.Parameters.AddWithValue("@To_Date", To_Date);
                    cmd.Parameters.AddWithValue("@Status_ID", Status_ID);
                    cmd.Parameters.AddWithValue("@c_id", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_GoodsReceived_Master item = new Pa_GoodsReceived_Master();


                        item.GRVMaster_ID = Convert.ToInt32(rdr["GRVMaster_ID"]);
                        item.Vendor_Name = rdr["Vendor_Name"].ToString();
                        item.GRVRef = rdr["GRVRef"].ToString();
                        item.GRVDate = rdr["GRVDate"].ToString();
                        item.GRVStatus = rdr["GRVStatus"].ToString();
                        item.Total = rdr["Total"].ToString();
                        item.VAT_Exp = rdr["VAT_Exp"].ToString();

                        item.Total_Amount = rdr["Total_Amount"].ToString();
                        item.Paid = rdr["Paid"].ToString();
                        item.Bal_Due = rdr["Bal_Due"].ToString();
                        item.PurchaseRef = rdr["PurchaseRef"].ToString();

         

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




        public string GetOldTempIDFromGoodsReceivedDetail_DAL(int? GRVMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@GRVMaster_ID",GRVMaster_ID)

            };


                var response = dbLayer.SP_DataTable_return("SelectgrvDetailOldTempID", paramArray).Tables[0];
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

        public IEnumerable<Pa_PurchaseDetails_DAL> GetGRVRefDetails_Other_DAL()
        {


            string sp = "GetPurchaseRef";
            List<Pa_PurchaseDetails_DAL> itemlist = new List<Pa_PurchaseDetails_DAL>();

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
                        Pa_PurchaseDetails_DAL item = new Pa_PurchaseDetails_DAL();

                        item.PurchaseMaster_ID = Convert.ToInt32( rdr["PurchaseMaster_ID"]);
                        item.PurchaseRef = rdr["PurchaseRef"].ToString();



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

        public IEnumerable<Pa_PurchaseDetails_DAL> Get_PurchaseRefDetails_DAL(int PurchaseMaster_ID)
        {



            string sp = "Pa_Select_PurchaseDetail_ByPurchaseRef";
            List<Pa_PurchaseDetails_DAL> itemlist = new List<Pa_PurchaseDetails_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
            
                    cmd.Parameters.AddWithValue("@PurchaseMaster_ID", PurchaseMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_PurchaseDetails_DAL item = new Pa_PurchaseDetails_DAL();

                        item.PurchaseDetails_ID = Convert.ToInt32(rdr["PurchaseDetails_ID"]);
                        item.PurchaseMaster_ID = Convert.ToInt32(rdr["PurchaseMaster_ID"]);
                        item.item_ID = rdr["item_ID"].ToString();
                        item.ItemCode = rdr["ItemCode"].ToString();
                        item.ItemName = rdr["ItemName"].ToString();
                        item.item_Description = rdr["item_Description"].ToString();
                        item.Quantity = rdr["Quantity"].ToString();
                        item.UOM = rdr["UOM"].ToString();
                        item.Unit_Price = rdr["Unit_Price"].ToString();
                        item.Currency_ID = rdr["Currency_ID"].ToString();
                        item.Currency_Rate = rdr["Currency_Rate"].ToString();
                        item.Discount = rdr["Discount"].ToString();
                        item.VAT_Rate = rdr["VAT_Rate"].ToString();
                        item.VAT_Exp = rdr["VAT_Exp"].ToString();

                        item.Amount = rdr["Amount"].ToString();
                        item.TtlAmount = rdr["TtlAmount"].ToString();
                        item.Amount_Others = rdr["Amount_Others"].ToString();


                        item.SubTotal = rdr["SubTotal"].ToString();
                        item.Vat_ExpTotal = rdr["Vat_ExpTotal"].ToString();
                        item.DiscountTotal = rdr["DiscountTotal"].ToString();
                        item.Grand_Total = rdr["Grand_Total"].ToString();
                        item.location_ID = rdr["location_ID"].ToString();
                        item.Currency_Name = rdr["Currency_Name"].ToString();
                        item.PURQty = rdr["PURQty"].ToString();
                        item.GRVQty = rdr["GRVQty"].ToString();
              




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
        public Pa_GoodsReceived_Master Get_PurchaseMaster_GRV_DAL(int? GRVMaster_ID)
        {

            string sp = "Pa_Select_PurchaseMaster_GRV";

            Pa_GoodsReceived_Master pm = new Pa_GoodsReceived_Master();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@GRVMaster_ID", GRVMaster_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.GRVMaster_ID = Convert.ToInt32(dr["GRVMaster_ID"]);
                                pm.GRVRef = dr["GRVRef"].ToString();
                                pm.Vendor_ID = Convert.ToInt32(dr["Vendor_ID"]);
                    
                                pm.Vendor_PruchaseTo = dr["Vendor_PruchaseTo"].ToString();
                                pm.Vendor_Address = dr["Vendor_Address"].ToString();
                                pm.GRVDate = dr["GRVDate"].ToString();
                      
                                pm.Total = dr["SubTotal"].ToString();
                                pm.Created_at = dr["Created_at"].ToString();
                                pm.Created_by = dr["Created_by"].ToString();
                                pm.Modified_at = dr["Modified_at"].ToString();
                                pm.Modified_by = dr["Modified_by"].ToString();
                                pm.Vendor_Name = dr["PartnerName"].ToString();
                                pm.PurchaseRef = dr["PurchaseRef"].ToString();

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


            return pm;
        }
        #endregion

    }
}



















