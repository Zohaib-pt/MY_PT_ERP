using DAL.Models;
using DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using X.PagedList;




namespace DAL.oClasses
{
    public class OStock : IOStock
    {


        private readonly IConnectionStringManager _conMgr;
        private readonly ISqlHelper dbLayer;
        SqlConnection con;

            public OStock(IConnectionStringManager conMgr, ISqlHelper sqlHelp)
        {

            _conMgr = conMgr;
            dbLayer = sqlHelp;
            //---Getting Connection String
            Constr = _conMgr.GetConnectionString();
        }
        //---This variable is for saving connection string.


        //--by Rafay - Start Date 16/jan/2021
        public IEnumerable<pa_Select_PaperList_Result> ShakeenList { get; set; }

        //Faraz New work
        public IEnumerable<pa_Vanning_Master> VanningListPagedObject1 { get; set; }

        //New faraz work
        public IEnumerable<pa_Shipping_Info> Shipping_infoListPagedObject1 { get; set; }

        // PurchaseListJP
        public IEnumerable<Pa_PurchaseMaster_DAL> PurchaseMasterList1 { get; set; }


        //Reksolist
        public IEnumerable<pa_select_StockParties_Result> ReksoList1 { get; set; }

        //


        //paperlist for report

        public IEnumerable<pa_Select_PaperList_Result> PapersListObject_print { get; set; }


        public IEnumerable<Pa_Attachments_DAL> attachmentList { get; set; }
        //paperlist for search

        public IPagedList<pa_Select_PaperList_Result> PapersListObject_search { get; set; }
        public List<pa_Select_PaperList_Result> get_Shaken_List(string chassis, string StartDate, string EndDate)
        {
            string sp = "pa_Select_platnumber_list";

            List<pa_Select_PaperList_Result> main = new List<pa_Select_PaperList_Result>();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@chassis_no", chassis);
                        cmd.Parameters.AddWithValue("@start_date", StartDate);
                        cmd.Parameters.AddWithValue("@end_date", EndDate);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {



                                //     s.stock_ref, c.color, m.make, tblmake_details.model_description, sd.Chassis_no, sd.stock_ID, sd.ID, sd.temp_ID, sd.Shipping_info_ID

                                pa_Select_PaperList_Result si = new pa_Select_PaperList_Result();

                                si.Purchase_Ref = dr["PurchaseRef"].ToString();
                                si.Stock_ID = Convert.ToInt16(dr["Stock_ID"].ToString());
                                si.PurchaseDate = dr["PurchaseDate"].ToString();
                                si.PlatNumberFee = dr["PlatNumberFee"].ToString();
                                si.Cancel_Submit_Date = dr["Cancel_Submit_Date"].ToString();
                                si.RefundAMount = dr["RefundAMount"].ToString();
                                si.Refund_Request_Date = dr["Refund_Request_Date"].ToString();
                                si.Status = dr["Status"].ToString();
                                si.Chassis = dr["Chassis_No"].ToString();
                                si.Recieved_Date = dr["Recieved_date"].ToString();
                                si.accountId = dr["Account_Debit_ID"].ToString();
                                si.AdjustedRef = dr["Adjusted_Purchase_ref"].ToString();
                                si.Make = dr["Make"].ToString();
                                si.ModelDesciption = dr["ModelDesciption"].ToString();
                                si.ModelYear = dr["ModelYear"].ToString();
                                si.Ref = dr["ref"].ToString();



                                main.Add(si);
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


            return main;
        }
        //--by Rafay - End Date 16/jan/2021


        //by Rafay - start 11/jan/2021

        public Pa_PurchaseMaster_DAL PurchaseMasters { get; set; }
        public PurchaseDetailGrandTotals PurchaseDetailGrandTotal { get; set; }
        public List<PurchaseDetailGrandTotals> Get_purchaseDetailsTotal(int PurchaseMaster_ID)
        {
            List<PurchaseDetailGrandTotals> pm = new List<PurchaseDetailGrandTotals>();
            string sp = "select_purchasedetails_Total_jp";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PurchaseMaster_ID", PurchaseMaster_ID);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                PurchaseDetailGrandTotals PM = new PurchaseDetailGrandTotals();
                                PM.TotalPriceOrignal = dr["TotalPriceOrignal"].ToString();
                                PM.TotalPriceTax = dr["TotalPriceTax"].ToString();
                                PM.TotalAuctionFee = dr["TotalAuctionFee"].ToString();
                                PM.TotalAuctionFeeTax = dr["TotalAuctionFeeTax"].ToString();
                                PM.TotalPlatNumberFee = dr["TotalPlatNumberFee"].ToString();
                                PM.TotalRecycleFee = dr["TotalRecycleFee"].ToString();
                                PM.TotalReksoFeeTax = dr["TotalReksoFeeTax"].ToString();
                                PM.TotalAuctionPayable = dr["TotalAuctionPayable"].ToString();
                                PM.TotalReksoPayable = dr["TotalReksoPayable"].ToString();
                                PM.TotalAuctionPayableWithTax = dr["TotalAuctionPayableWithTax"].ToString();
                                PM.TotalReksoPayableWithTax = dr["TotalReksoPayableWithTax"].ToString();
                                PM.TotalTaxOnly = dr["TotalTaxOnly"].ToString();
                                PM.GrandTotalPayable = dr["GrandTotalPayable"].ToString();
                                PM.GrandTotalPayableWithTax = dr["GrandTotalPayableWithTax"].ToString();
                                PM.Paid_Auction = dr["Paid_Auction"].ToString();
                                PM.Paid_Rikso = dr["Paid_Rikso"].ToString();
                                PM.Bal_Due = dr["Bal_Due"].ToString();
                                PM.PlatNumberFee_Refund = dr["PlatNumberFee_Refund"].ToString();

                                pm.Add(PM);
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
        public List<Pa_PurchaseMaster_DAL> PurchaseMaster(int ID)
        {
            List<Pa_PurchaseMaster_DAL> PM = new List<Pa_PurchaseMaster_DAL>();
            string sp = "pa_Select_PurchaseMaster_by_ID";
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PurchaseMaster_ID", ID);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Pa_PurchaseMaster_DAL pm = new Pa_PurchaseMaster_DAL();


                                pm.Balance = Convert.ToInt32(dr["Balance"]).ToString();



                                PM.Add(pm);
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


            return PM;
        }

        //by Rafay - end 11/jan/2021

        //by Rafay - Start 11/jan/2021 Rekso List

        public IPagedList<pa_select_StockParties_Result> ReksoList { get; set; }

        public IEnumerable<pa_select_StockParties_Result> pa_select_StockParties(string purchaseRef, string startDate, string endDate, int RekSo_Vendor_ID)
        {
            List<pa_select_StockParties_Result> pm = new List<pa_select_StockParties_Result>();
            string sp = "pa_select_StockParties";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PurchaseRef", purchaseRef);
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);
                        cmd.Parameters.AddWithValue("@RekSo_Vendor_ID", RekSo_Vendor_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_select_StockParties_Result PM = new pa_select_StockParties_Result();
                                PM.Ref = dr["Ref"].ToString();
                                PM.PurchaseRef = dr["PurchaseRef"].ToString();
                                PM.PurchaseMaster_ID = Convert.ToInt32(dr["PurchaseMaster_ID"]);
                                PM.Date = dr["Date"].ToString();
                                PM.Vendor_Name = dr["Vendor_Name"].ToString();
                                PM.PurchaseStatus = dr["PurchaseStatus"].ToString();
                                PM.Total_Payable = dr["Total_Payable"].ToString();
                                PM.Paid = dr["Paid"].ToString();
                                PM.Balance = dr["Balance"].ToString();
                                PM.Car_Count =Convert.ToInt32(dr["Rikso_Cnt"].ToString());


                                pm.Add(PM);
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

        //by Rafay - end 11/jan/2021 




        public string Constr { get; set; }
        public StockDetails StockObject { get; set; }

        public IEnumerable<StockDetails> StockListObject { get; set; }

        public StockDetails StockListStats { get; set; }

        public IEnumerable<StockDetails> StockList_TTL { get; set; }


        public IPagedList<StockDetails> StockListPagedObject { get; set; }

        public IEnumerable<Pa_PaymentDetails_DAL> objStock_Exp_Details { get; set; }
        public pa_Vanning_Master VanningMasterObj { get; set; }
        public pa_Vanning_Details VanningObject { get; set; }
        public IEnumerable<pa_Vanning_Details> VanningDetailList { get; set; }
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
        public Pa_PurchaseMaster_DAL PurchaseMasterRef { get; set; }
        public IPagedList<Pa_PurchaseMaster_DAL> PurchaseMasterList { get; set; }
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
        public Pa_Auction_Master AuctionMasterObj { get; set; }
        public IEnumerable<Pa_Auction_Detsils> AuctionDetailList { get; set; }
        public IEnumerable<Pa_Auction_Master> AuctionMasterList { get; set; }

        public StockDetails Get_Select_Stock_by_ID_DAL(int? Stock_ID)
        {

            string sp = "Select_Stock_by_ID";

            StockDetails pm = new StockDetails();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Stock_ID", Stock_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.stock_ID = Convert.ToInt32(dr["Stock_ID"]);

                                pm.PurchaseRef = dr["PurchaseRef"].ToString();
                                pm.Vendor_ID = Convert.ToInt32(dr["Vendor_ID"]);


                                pm.Make_ID = Convert.ToInt32(dr["Make_ID"]);

                                pm.Color_Interior_ID = Convert.ToInt32(dr["Color_Interior_ID"]);
                                pm.Color_Exterior_ID = Convert.ToInt32(dr["Color_Exterior_ID"]);


                                pm.MakeModel_description_ID = Convert.ToInt32(dr["MakeModel_description_ID"]);
                                pm.Chassis_No = dr["Chassis_No"].ToString();
                                pm.Transmission = dr["Transmission"].ToString();
                                pm.Door = dr["Door"].ToString();
                                pm.Drive = dr["Drive"].ToString();
                                pm.Fuel_Type = dr["Fuel_Type"].ToString();
                                //pm.Engine_Power = dr["Engine_Power"].ToString();
                                pm.Used_New = dr["Used_New"].ToString();
                                pm.Engine_No = dr["Engine_No"].ToString();
                                pm.stock_ref = dr["stock_ref"].ToString();
                                pm.ModelYear = dr["ModelYear"].ToString();
                                pm.PurchaseDate = dr["PurchaseDate"].ToString();
                                pm.StatusCancel_Date = dr["StatusCancel_Date"].ToString();
                                pm.Arrival_Date = dr["Arrival_Date"].ToString();
                                pm.Leave_Date = dr["Leave_Date"].ToString();


                                pm.BL_NO = dr["BL_NO"].ToString();
                                pm.ShipName = dr["ShipName"].ToString();

                                pm.Loc_ID = Convert.ToInt32(dr["Loc_ID"]);
                                pm.Showroom_ID = Convert.ToInt32(dr["Showroom_ID"]);

                                pm.CreatedBy_UserName = dr["CreatedBy_UserName"].ToString();
                                pm.Modified_by_UserName = dr["Modified_by_UserName"].ToString();

                                pm.Stock_Status = dr["Stock_Status"].ToString();
                                pm.PriceOrignal = dr["PriceOrignal"].ToString();
                                pm.PriceRate = dr["PriceRate"].ToString();
                                pm.FreightCharges = dr["FreightCharges"].ToString();
                                pm.FreightRate = dr["FreightRate"].ToString();

                                pm.TotalPrice = dr["TotalPrice"].ToString();
                                pm.CC_Exp = dr["CC_Exp"].ToString();

                                pm.Exp_Custom_Duty = dr["Exp_Custom_Duty"].ToString();

                                pm.ME_Exp = dr["ME_Exp"].ToString();
                                pm.Paper_Exp = dr["Paper_Exp"].ToString();
                                pm.Transport_Exp = dr["Transport_Exp"].ToString();
                                pm.Comm_Exp = dr["Comm_Exp"].ToString();
                                pm.Transport_Exp = dr["Transport_Exp"].ToString();
                                pm.AgentComm_Exp = dr["AgentComm_Exp"].ToString();
                                pm.OtherCharges_Exp = dr["OtherCharges_Exp"].ToString();

                                pm.Total_Expense = dr["Total_Expense"].ToString();
                                pm.Total_Cost = dr["Total_Cost"].ToString();

                                pm.VAT_Exp = dr["VAT_Exp"].ToString();

                                // Japan Fields 
                                pm.LotNumber = dr["LotNumber"].ToString();
                                pm.AuctionFee = dr["AuctionFee"].ToString();
                                pm.AuctionFeeTax = dr["AuctionFeeTax"].ToString();
                                pm.PlatNumberFee = dr["PlatNumberFee"].ToString();
                                pm.ReksoFee = dr["ReksoFee"].ToString();
                                pm.ReksoFeeTax = dr["ReksoFeeTax"].ToString();
                                pm.RecycleFee = dr["RecycleFee"].ToString();
                                pm.LoadingCharges = dr["LoadingCharges"].ToString();





                                pm.CarTaken = Convert.ToBoolean(dr["CarTaken"].ToString());
                                pm.CarTakenDate = dr["CarTakenDate"].ToString();
                                pm.CarTakenPerson = dr["CarTakenPerson"].ToString();
                                pm.CarTakenContact = dr["CarTakenContact"].ToString();
                                pm.DilveryOrder_Issued = Convert.ToBoolean(dr["DilveryOrder_Issued"].ToString());
                                pm.DeliverOrder_Date = dr["DeliverOrder_Date"].ToString();
                                pm.Comments = dr["Comments"].ToString();
                                pm.Make_category_ID = Convert.ToInt32(dr["Make_category_ID"].ToString());
                                pm.Weight = dr["Weight"].ToString();
                                pm.Make_country_ID = Convert.ToInt32(dr["Make_country_ID"].ToString());
                                pm.Engine_Type = dr["Engine_Type"].ToString();
                                pm.HS_Code = dr["Hs_Code"].ToString();
                                pm.Selling_Price = dr["Selling_Price"].ToString();
                                pm.Options = dr["Options"].ToString();
                                pm.StockType_ID = Convert.ToInt32(dr["StockType"].ToString());

                                pm.ManufacturingDate = dr["ManufacturingDate"].ToString();


                                pm.Master_BOE = dr["Master_BOE"].ToString();
                                pm.FromLoc = dr["FromLoc"].ToString();
                                pm.GoingLoc = dr["GoingLoc"].ToString();
                                pm.ReksoName = dr["ReksoName"].ToString();

                                pm.Shipping_Charges = dr["Shipping_Charges"].ToString();
                                pm.Vanning_Charges = dr["Vanning_Charges"].ToString();
                                pm.Inspection_Charges = dr["Inspection_Charges"].ToString();
                                pm.mileage = dr["mileage"].ToString();

                                pm.G_weight = dr["G_weight"].ToString();
                                pm.Registration = dr["Registration"].ToString();
                                pm.FirstRegistrationDate = dr["FirstRegistrationDate"].ToString();
                                pm.Length = dr["Length"].ToString();
                                pm.Width = dr["Width"].ToString();
                                pm.Height = dr["Height"].ToString();

                                pm.cc = dr["cc"].ToString();
                                pm.PriceTax = dr["PriceTax"].ToString();
                                pm.LoadingCharges = dr["LoadingCharges"].ToString();
                                pm.OtherCharges = dr["OtherCharges"].ToString();
                                pm.AuctionName = dr["AuctionName"].ToString();
                                pm.ContNO = dr["ContainerNo"].ToString();

                                pm.JP_Charges = dr["JP_Charges"].ToString();
                                pm.Make = dr["Make"].ToString();
                                pm.ModelDesciption = dr["ModelDesciption"].ToString();
                                pm.Color = dr["Color"].ToString();
                                pm.InteriorColor = dr["Color_Int"].ToString();
                                pm.Engine_Power = dr["Engine_Power"].ToString();
                                pm.AD_Link = dr["AD_Link"].ToString();
                                pm.TotalPrice_Other = dr["TotalPrice_Other"].ToString();





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



        public string Insert_Stock_DAL(string CHASSIS_NO, int loc_ID, int Showroom_ID, string vehicle_CC,
            int StockType, int User_ID, string BL_NO, string ShipName, string Arrival_Date, string Leave_Date, int Vendor_ID, string PurchaseDate,
            string PurchaseRef, string Comments, string ModelYear, int color_exterior_ID, string Transmission, string Door, int make_ID,
            int MakeModel_description_ID, int Make_category_ID, string OPTIONS, string HS_CODE, string DRIVE, string FUEL_TYPE, string EngineType, string USED_NEW,
            int make_country_ID, string Engine_No, string GCC_Specs, string mileage, string WEIGHT, string seat, int Color_Interior_ID,
            string intGrade, string intRemarks, string extGrade, string extRemarks, string Option_Code, string KeyNo,
            string PriceOrignal, string PriceRate,
            string FreightCharges, string FreightRate, string AuctionFee, string PlatNumberFee, string ReksoFee, string RecycleFee,
            string CC_Exp,string Exp_Custom_Duty, string ME_Exp, string Paper_Exp, string Transport_Exp,
            string Comm_Exp, string AgentComm_Exp, string OtherCharges_Exp, string VAT_Exp,
            string ManufacturingDate, string Selling_Price, string Master_BOE, int? c_ID, string Vanning_Charges, string OtherCharges, string JP_Charges, string PriceTax)
        {

            string msg = "";
            try
            {




                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@CHASSIS_NO",CHASSIS_NO),
                new SqlParameter("@loc_ID",loc_ID),
                new SqlParameter("@Showroom_ID",Showroom_ID),
                  new SqlParameter("@vehicle_CC",vehicle_CC),
                 new SqlParameter("@StockType",StockType),
                   new SqlParameter("@User_ID",User_ID),
                new SqlParameter("@BL_NO",BL_NO),
                 new SqlParameter("@ShipName",ShipName),
                  new SqlParameter("@Arrival_Date",Arrival_Date),
                     new SqlParameter("@Leave_Date",Leave_Date),
                    new SqlParameter("@Vendor_ID",Vendor_ID),
                    new SqlParameter("@PurchaseDate",PurchaseDate),
                          new SqlParameter("@PurchaseRef",PurchaseRef),
                             new SqlParameter("@Comments",Comments),
                               new SqlParameter("@ModelYear",ModelYear),
                                   new SqlParameter("@color_exterior_ID",color_exterior_ID),
                                      new SqlParameter("@TRANSMISSION",Transmission),
                                         new SqlParameter("@DOOR",Door),
                                            new SqlParameter("@make_ID",make_ID),
                                             new SqlParameter("@MakeModel_description_ID",MakeModel_description_ID),
                                             new SqlParameter("@Make_category_ID",Make_category_ID),
                                              new SqlParameter("@OPTIONS",OPTIONS),
                                              new SqlParameter("@HS_CODE",HS_CODE),
                                              new SqlParameter("@DRIVE",DRIVE),
                                             new SqlParameter("@FUEL_TYPE",FUEL_TYPE),
                                              new SqlParameter("@ENGINE_POWER",EngineType),
                                              new SqlParameter("@USED_NEW",USED_NEW),
                                                new SqlParameter("@make_country_ID",make_country_ID),
                                                  new SqlParameter("@Engine_No",Engine_No),
                                                     new SqlParameter("@GCC_Specs",GCC_Specs),
                                                new SqlParameter("@mileage",mileage),
                                                new SqlParameter("@WEIGHT",WEIGHT),
                                                new SqlParameter("@seat",seat),
                                                new SqlParameter("@Color_Interior_ID",Color_Interior_ID),
                                                new SqlParameter("@intGrade",intGrade),
                                                 new SqlParameter("@intRemarks",intRemarks),
                                                  new SqlParameter("@extGrade",extGrade),
                                                   new SqlParameter("@extRemarks",extRemarks),
                                                     new SqlParameter("@Option_Code",Option_Code),
                                                       new SqlParameter("@KeyNo",KeyNo),
                                                         new SqlParameter("@PriceOrignal",PriceOrignal),
                                                           new SqlParameter("@PriceRate",PriceRate),
                                                             new SqlParameter("@FreightCharges",FreightCharges),
                                                             new SqlParameter("@FreightRate",FreightRate),
                                                             new SqlParameter("@AuctionFee",AuctionFee),
                                                             new SqlParameter("@PlatNumberFee",PlatNumberFee),
                                                             new SqlParameter("@ReksoFee",ReksoFee),
                                                             new SqlParameter("@RecycleFee",RecycleFee),
                                                              new SqlParameter("@CC_Exp",CC_Exp),
                                                               new SqlParameter("@Exp_Custom_Duty",Exp_Custom_Duty),
                                                               new SqlParameter("@ME_Exp",ME_Exp),
                                                                new SqlParameter("@Paper_Exp",Paper_Exp),
                                                                 new SqlParameter("@Transport_Exp",Transport_Exp),
                                                                  new SqlParameter("@Comm_Exp",Comm_Exp),
                                                                   new SqlParameter("@AgentComm_Exp",AgentComm_Exp),
                                                                    new SqlParameter("@OtherCharges_Exp",OtherCharges_Exp),
                                                                     new SqlParameter("@VAT_Exp",VAT_Exp),
                                                                      new SqlParameter("@ManufacturingDate",ManufacturingDate),
                                                                       new SqlParameter("@Selling_Price",Selling_Price),
                                                                       new SqlParameter("@Master_BOE",Master_BOE),
                                                                        new SqlParameter("@Vanning_Charges",Vanning_Charges),
                                                                    new SqlParameter("@OtherCharges",OtherCharges),
                                                                    new SqlParameter("@JP_Charges",JP_Charges),
                                                                    new SqlParameter("@PriceTax", PriceTax),
                                                                        new SqlParameter("@c_ID",c_ID)

            };


                var response = dbLayer.SP_DataTable_return("Insert_Stock", paramArray).Tables[0];
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
           string VAT_Exp, string ManufacturingDate, string Selling_Price, string Master_BOE, int? Stock_ID, string AD_Link, string Vanning_Charges, string OtherCharges, string JP_Charges, string PriceTax, string LotNumber)
        {

            string msg = "";
            try
            {




                SqlParameter[] paramArray = new SqlParameter[] {
                  new SqlParameter("@CHASSIS_NO",CHASSIS_NO),
                new SqlParameter("@loc_ID",loc_ID),
                new SqlParameter("@Showroom_ID",Showroom_ID),
                  new SqlParameter("@vehicle_CC",vehicle_CC),
                 new SqlParameter("@StockType",StockType),
                   new SqlParameter("@User_ID",User_ID),
                new SqlParameter("@BL_NO",BL_NO),
                 new SqlParameter("@ShipName",ShipName),
                  new SqlParameter("@Arrival_Date",Arrival_Date),
                     new SqlParameter("@Leave_Date",Leave_Date),
                    new SqlParameter("@Vendor_ID",Vendor_ID),
                    new SqlParameter("@PurchaseDate",PurchaseDate),
                          new SqlParameter("@PurchaseRef",PurchaseRef),
                             new SqlParameter("@Comments",Comments),
                               new SqlParameter("@ModelYear",ModelYear),
                                   new SqlParameter("@color_exterior_ID",color_exterior_ID),
                                      new SqlParameter("@TRANSMISSION",Transmission),
                                         new SqlParameter("@DOOR",Door),
                                            new SqlParameter("@make_ID",make_ID),
                                             new SqlParameter("@MakeModel_description_ID",MakeModel_description_ID),
                                               new SqlParameter("@Make_category_ID",Make_category_ID),
                                              new SqlParameter("@OPTIONS",OPTIONS),
                                              new SqlParameter("@HS_CODE",HS_CODE),
                                              new SqlParameter("@DRIVE",DRIVE),
                                             new SqlParameter("@FUEL_TYPE",FUEL_TYPE),
                                              new SqlParameter("@ENGINE_POWER",EngineType),
                                              new SqlParameter("@USED_NEW",USED_NEW),
                                                new SqlParameter("@make_country_ID",make_country_ID),
                                                  new SqlParameter("@Engine_No",Engine_No),
                                                     new SqlParameter("@GCC_Specs",GCC_Specs),
                                                              new SqlParameter("@mileage",mileage),
                                                new SqlParameter("@WEIGHT",WEIGHT),
                                                new SqlParameter("@seat",seat),
                                                new SqlParameter("@Color_Interior_ID",Color_Interior_ID),
                                                new SqlParameter("@intGrade",intGrade),
                                                 new SqlParameter("@intRemarks",intRemarks),
                                                  new SqlParameter("@extGrade",extGrade),
                                                   new SqlParameter("@extRemarks",extRemarks),
                                                     new SqlParameter("@Option_Code",Option_Code),
                                                       new SqlParameter("@KeyNo",KeyNo),
                                                         new SqlParameter("@PriceOrignal",PriceOrignal),
                                                           new SqlParameter("@PriceRate",PriceRate),
                                                             new SqlParameter("@FreightCharges",FreightCharges),
                                                             new SqlParameter("@FreightRate",FreightRate),
                                                             new SqlParameter("@AuctionFee",AuctionFee),
                                                             new SqlParameter("@PlatNumberFee",PlatNumberFee),
                                                             new SqlParameter("@ReksoFee",ReksoFee),
                                                             new SqlParameter("@RecycleFee",RecycleFee),
                                                              new SqlParameter("@CC_Exp",CC_Exp),
                                                              new SqlParameter("@Exp_Custom_Duty",Exp_Custom_Duty),
                                                               new SqlParameter("@ME_Exp",ME_Exp),
                                                                new SqlParameter("@Paper_Exp",Paper_Exp),
                                                                 new SqlParameter("@Transport_Exp",Transport_Exp),
                                                                  new SqlParameter("@Comm_Exp",Comm_Exp),
                                                                   new SqlParameter("@AgentComm_Exp",AgentComm_Exp),
                                                                    new SqlParameter("@OtherCharges_Exp",OtherCharges_Exp),
                                                                     new SqlParameter("@VAT_Exp",VAT_Exp),
                                                                     new SqlParameter("@ManufacturingDate",ManufacturingDate),
                                                                     new SqlParameter("@Selling_Price",Selling_Price),
                                                                     new SqlParameter("@Master_BOE",Master_BOE),
                                                                    new SqlParameter("@Stock_ID",Stock_ID),
                                                                    new SqlParameter("@Vanning_Charges",Vanning_Charges),
                                                                    new SqlParameter("@OtherCharges",OtherCharges),
                                                                    new SqlParameter("@JP_Charges",JP_Charges),
                                                                    new SqlParameter("@PriceTax", PriceTax),
                                                                    new SqlParameter("@AD_Link",AD_Link),
                                                                    new SqlParameter("@LotNumber",LotNumber)

                    //@Selling_Price varchar(30),@Master_BOE varchar(50),
            };


                var response = dbLayer.SP_DataTable_return("Update_Stock", paramArray).Tables[0];
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

        public IEnumerable<StockDetails> Get_StockList_DAL(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
                string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID, string VendorName, string ModelYear, string Container_No,int StockType_ID)
        {

            string sp = "select_StockList";

            List<StockDetails> itemlist = new List<StockDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ChassisNo", ChassisNo);
                        cmd.Parameters.AddWithValue("@Make_ID", Make_ID);
                        cmd.Parameters.AddWithValue("@MakeModel_description_ID", MakeModel_description_ID);
                        cmd.Parameters.AddWithValue("@Color_ID", Color_ID);
                        cmd.Parameters.AddWithValue("@PurchaseStartDate", PurchaseStartDate);
                        cmd.Parameters.AddWithValue("@PurchaseEndDate", PurchaseEndDate);
                        cmd.Parameters.AddWithValue("@BL_NO", BL_NO);
                        cmd.Parameters.AddWithValue("@BOE", BOE);
                        cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                        cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                        cmd.Parameters.AddWithValue("@Stock_Status", Stock_Status);
                        cmd.Parameters.AddWithValue("@loc_ID", loc_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);
                        cmd.Parameters.AddWithValue("@VendorName", VendorName);
                        cmd.Parameters.AddWithValue("@ModelYear", ModelYear);
                        cmd.Parameters.AddWithValue("@Container_No", Container_No);
                        cmd.Parameters.AddWithValue("@StockType_ID", StockType_ID);


                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                //stock_ID, Chassis_No,  Make, ModelDesciption, Color, StockType,Transmission, CarLocation, Total_Cost,PurchaseRef,PurchaseDate, Stock_Status,Vendor_Name,ModelYear

                                StockDetails item = new StockDetails();
                                item.stock_ID = Convert.ToInt32(dr["Stock_ID"]);
                                item.Chassis_No = dr["Chassis_No"].ToString();
                                item.Vendor_Name = dr["Vendor_Name"].ToString();
                                item.Make = dr["Make"].ToString();
                                item.ModelDesciption = dr["ModelDesciption"].ToString();
                                item.ModelYear = dr["ModelYear"].ToString();
                                item.Color = dr["Color"].ToString();
                                item.StockTypeName = dr["StockTypeName"].ToString();
                                item.Transmission = dr["Transmission"].ToString();
                                item.CarLocation = dr["CarLocation"].ToString();
                                item.Total_Cost = dr["Total_Cost"].ToString();
                                item.PurchaseRef = dr["PurchaseRef"].ToString();
                                item.PurchaseDate = dr["PurchaseDate"].ToString();
                                item.Stock_Status = dr["Stock_Status"].ToString();
                                item.InteriorColor = dr["InteriorColor"].ToString();
                                item.ProuctionDate = dr["ProuctionDate"].ToString();
                                item.mileage = dr["mileage"].ToString();
                                item.PriceOrignal = dr["Price"].ToString();
                                item.AuctionName = dr["AuctionName"].ToString();
                                item.ContNO = dr["ContainerNo"].ToString();
                                item.LotNumber = dr["LotNumber"].ToString();
                                item.CountAttach = dr["CountAttach"].ToString();
                                item.Selling_Price = dr["Selling_Price"].ToString();
                                item.AD_Link = dr["AD_Link"].ToString();
                                item.PaperReceived = dr["PaperReceived"].ToString();
                                item.TotalExpense   = dr["Total_Expense"].ToString();


                                itemlist.Add(item);

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


            return itemlist;
        }
        public IEnumerable<StockDetails> Get_StockList_TTL_DAL(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
         string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID)
        {

            string sp = "select_StockList_TTL";

            List<StockDetails> itemlist = new List<StockDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ChassisNo", ChassisNo);
                        cmd.Parameters.AddWithValue("@Make_ID", Make_ID);
                        cmd.Parameters.AddWithValue("@MakeModel_description_ID", MakeModel_description_ID);
                        cmd.Parameters.AddWithValue("@Color_ID", Color_ID);
                        cmd.Parameters.AddWithValue("@PurchaseStartDate", PurchaseStartDate);
                        cmd.Parameters.AddWithValue("@PurchaseEndDate", PurchaseEndDate);
                        cmd.Parameters.AddWithValue("@BL_NO", BL_NO);
                        cmd.Parameters.AddWithValue("@BOE", BOE);
                        cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                        cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                        cmd.Parameters.AddWithValue("@Stock_Status", Stock_Status);
                        cmd.Parameters.AddWithValue("@loc_ID", loc_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);
                        cmd.Parameters.AddWithValue("@VendorName", VendorName);
                        cmd.Parameters.AddWithValue("@ModelYear", ModelYear);
                        cmd.Parameters.AddWithValue("@Container_No", Container_No);
                        cmd.Parameters.AddWithValue("@StockType_ID", StockType_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                //stock_ID, Chassis_No,  Make, ModelDesciption, Color, StockType,Transmission, CarLocation, Total_Cost,PurchaseRef,PurchaseDate, Stock_Status,Vendor_Name,ModelYear

                                StockDetails item = new StockDetails();

                                item.Total_All_Cost = dr["Total_All_Cost"].ToString();
                                item.PriceOrignal = dr["PriceOrignal"].ToString();


                                itemlist.Add(item);

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


            return itemlist;
        }

        public string DeleteStockMaster_DAL(int? Stock_ID, int? User_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Stock_ID",Stock_ID),
                new SqlParameter("@User_ID",User_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_StockMaster", paramArray).Tables[0];
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
        public StockDetails Get_StockList_stats_DAL(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
           string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID)
        {

            string sp = "select_StockList_stats";

            StockDetails pm = new StockDetails();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ChassisNo", ChassisNo);
                        cmd.Parameters.AddWithValue("@Make_ID", Make_ID);
                        cmd.Parameters.AddWithValue("@MakeModel_description_ID", MakeModel_description_ID);
                        cmd.Parameters.AddWithValue("@Color_ID", Color_ID);
                        cmd.Parameters.AddWithValue("@PurchaseStartDate", PurchaseStartDate);
                        cmd.Parameters.AddWithValue("@PurchaseEndDate", PurchaseEndDate);
                        cmd.Parameters.AddWithValue("@BL_NO", BL_NO);
                        cmd.Parameters.AddWithValue("@BOE", BOE);
                        cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                        cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                        cmd.Parameters.AddWithValue("@Stock_Status", Stock_Status);
                        cmd.Parameters.AddWithValue("@loc_ID", loc_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);
                        cmd.Parameters.AddWithValue("@VendorName", VendorName);
                        cmd.Parameters.AddWithValue("@ModelYear", ModelYear);
                        cmd.Parameters.AddWithValue("@Container_No", Container_No);
                        cmd.Parameters.AddWithValue("@StockType_ID", StockType_ID);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {

                                pm.StockStatus_UNSOLD = dr["UNSOLD"].ToString();
                                pm.StockStatus_SOLD = dr["SOLD"].ToString();
                                pm.StockStatus_BOOKING = dr["BOOKING"].ToString();
                                pm.StockStatus_CXX = dr["CXX"].ToString();
                                pm.StockStatus_TOTALSTOCK = dr["TOTAL_STOCK"].ToString();


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

        //public string DeleteStockMaster_DAL(int? Stock_ID, int? User_ID)
        //{

        //    string msg = "";
        //    try
        //    {

        //        SqlParameter[] paramArray = new SqlParameter[] {
        //        new SqlParameter("@Stock_ID",Stock_ID),
        //        new SqlParameter("@User_ID",User_ID)



        //    };


        //        var response = dbLayer.SP_DataTable_return("Delete_StockMaster", paramArray).Tables[0];
        //        msg = response.Rows[0][0].ToString();

        //        return msg;

        //    }
        //    catch (Exception ex)
        //    {
        //        string ErrorMessage = ex.Message;
        //        //throw;
        //        msg = ErrorMessage;
        //        return msg;
        //    }

        //}




        public IEnumerable<Pa_PaymentDetails_DAL> get_veh_expense_by_Stock_ID(int? Stock_ID)
        {

            string sp = "get_veh_expense_by_Stock_ID";

            List<Pa_PaymentDetails_DAL> itemlist = new List<Pa_PaymentDetails_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Stock_ID", Stock_ID);


                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                //stock_ID, Chassis_No,  Make, ModelDesciption, Color, StockType,Transmission, CarLocation, Total_Cost,PurchaseRef,PurchaseDate, Stock_Status,Vendor_Name,ModelYear

                                Pa_PaymentDetails_DAL item = new Pa_PaymentDetails_DAL();

                                item.DR_AccountName = dr["Account"].ToString();
                                item.Total_Amount = dr["Total_Amount"].ToString();
                                item.Description = dr["Description"].ToString();
                                item.PaymentDate = dr["PaymentDate"].ToString();
                                item.CR_AcountName = dr["PayVia"].ToString();
                                item.Status = dr["ApprovalStatus"].ToString();

                                itemlist.Add(item);

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


            return itemlist;
        }




        public string Update_StockStatus_DAL(int Stock_ID, int Status_ID, int User_ID, int c_ID)
        {


            string msg = "";
            try
            {




                SqlParameter[] paramArray = new SqlParameter[] {
                  new SqlParameter("@Status_ID",Status_ID),
                new SqlParameter("@Stock_ID",Stock_ID),
                new SqlParameter("@c_ID",c_ID),
                new SqlParameter("@Modified_by",User_ID) };


                var response = dbLayer.SP_DataTable_return("Update_StockStatus", paramArray).Tables[0];
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
        public IEnumerable<StockDetails> get_Stock_Status_DAL()
        {
            string sp = "get_Stock_Status";
            List<StockDetails> itemlist = new List<StockDetails>();

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
                        StockDetails item = new StockDetails();

                        item.Status_ID = Convert.ToInt32(rdr["Status_ID"]);
                        item.Stock_Status = rdr["Status"].ToString();



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

        #region Vanning_DAL
        public List<pa_Vanning_Master> get_Vanning_List(string trans_ref, string StartDate, string EndDate, string Shipper_Name)
        {



            string sp = "pa_Select_VanningMaster_List";

            List<pa_Vanning_Master> main = new List<pa_Vanning_Master>();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@VanningRef", trans_ref);
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@vendor_name", Shipper_Name);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {


                                //     s.stock_ref, c.color, m.make, tblmake_details.model_description, sd.Chassis_no, sd.stock_ID, sd.ID, sd.temp_ID, sd.Shipping_info_ID

                                pa_Vanning_Master si = new pa_Vanning_Master();

                                si.vanning_ref = dr["vanning_ref"].ToString();
                                si.Date = dr["Date"].ToString();
                                si.Total_Amount = Convert.ToInt32(dr["Total_Amount"]).ToString();
                                si.vanning_Master_ID = Convert.ToInt32(dr["vanning_Master_ID"].ToString());
                                si.Paid = dr["Paid"].ToString();
                                si.Balance = Convert.ToInt32(dr["Balance"]).ToString();
                                si.Purchased_Status = dr["PurchaseStatus"].ToString();
                                si.Vendor_Name = dr["Vendor_Name"].ToString();
                                si.transaction_status = Convert.ToInt32(dr["transaction_status"].ToString());



                                main.Add(si);
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


            return main;
        }

        public string Insert_Vanning_Details_DAL(string Vanning_Master_ID, string Chassis_No, string Amount, string Tax_Amount, string InspectionCharges, string InsepectionChargesTax, string TotalAmount,
            string htemp_ID, int user_ID)
        {

            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Vanning_Master_ID",Vanning_Master_ID),
                 new SqlParameter("@Chassis_No",Chassis_No),
                    new SqlParameter("@Amount",Amount),
                       new SqlParameter("@Tax_Amount",Tax_Amount),

                        new SqlParameter("@InspectionCharges",InspectionCharges),
                       new SqlParameter("@InsepectionChargesTax",InsepectionChargesTax),
                        new SqlParameter("@TotalAmount",TotalAmount),

                        new SqlParameter("@temp_ID",htemp_ID),
                         new SqlParameter("@user_ID",user_ID)
            };

            var response = dbLayer.SP_DataTable_return("pa_Insert_Vanning_Details", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }


        public string Update_Vanning_Details_DAL(string Chassis_No, string Amount, string Tax_Amount, string InspectionCharges, string InsepectionChargesTax, string TotalAmount, int user_ID, int ID, string Temp_ID)
        {

            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {

                 new SqlParameter("@Chassis_No",Chassis_No),
                    new SqlParameter("@Amount",Amount),
                       new SqlParameter("@Tax_Amount",Tax_Amount),

                       new SqlParameter("@InspectionCharges",InspectionCharges),
                       new SqlParameter("@InsepectionChargesTax",InsepectionChargesTax),
                        new SqlParameter("@TotalAmount",TotalAmount),


                         new SqlParameter("@user_ID",user_ID),
                         new SqlParameter("@ID",ID)

            };

            var response = dbLayer.SP_DataTable_return("pa_Update_Vanning_Details", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }


        public string Insert_Vanning_Master_DAL(string Vendor_ID, string Date, string trans_ref, string Comments, string Amount, string Tax_Amount,
            string temp_ID, int user_ID, int Sale_ID_For_Inserting)
        {


            string msg = "";



            //the Type variable is going is parameter into the procedure "pa_Insert_Shipping_Info" and there is a condition for this variable in the procedure.
            string Type = " ";

            //if sale id will greater than "0" then "Type" value will be "BYSALEREF"
            if (Sale_ID_For_Inserting > 0)
            {
                Type = "BYSALEREF";
            }



            SqlParameter[] paramArray = new SqlParameter[] {

               new SqlParameter("@Vendor_ID",Vendor_ID),
                new SqlParameter("@Date",Date),
                 new SqlParameter("@trans_ref",trans_ref),
                  new SqlParameter("@Comments",Comments),
                   new SqlParameter("@Amount",Amount),
                     new SqlParameter("@Tax_Amount",Tax_Amount),
                       new SqlParameter("@temp_ID",temp_ID),
                           new SqlParameter("@user_ID",user_ID),

                           //following two lines added by Nooruddin on 29-02-2020 because there made a modification in the procedure "pa_insert_vanning_master"
                             new SqlParameter("@Sale_ID",Sale_ID_For_Inserting),
                              new SqlParameter("@Type",Type)

            };


            var response = dbLayer.SP_DataTable_return("pa_Insert_Vanning_Master", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;



        }

        public string Update_Vanning_Master_DAL(string Vendor_ID, string Date, string trans_ref, string Comments, string Amount, string Tax_Amount,
            string temp_ID, int user_ID, string vanning_Master_ID)
        {


            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {

               new SqlParameter("@Vendor_ID",Vendor_ID),
                new SqlParameter("@Date",Date),
                 new SqlParameter("@trans_ref",trans_ref),
                  new SqlParameter("@Comments",Comments),
                   new SqlParameter("@Amount",Amount),
                     new SqlParameter("@Tax_Amount",Tax_Amount),
                       new SqlParameter("@temp_ID",temp_ID),
                           new SqlParameter("@user_ID",user_ID),

                            new SqlParameter("@vanning_Master_ID",vanning_Master_ID)


            };


            var response = dbLayer.SP_DataTable_return("pa_Update_Vanning_Master", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;



        }


        public pa_Vanning_Master get_Vanning_Master_by_tempID_ID(string temp_ID = "", string ID = "0")
        {



            string sp = "pa_Select_Vanning_Master_by_tempID_ID";

            pa_Vanning_Master pm = new pa_Vanning_Master();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@temp_ID", temp_ID);
                        cmd.Parameters.AddWithValue("@ID", ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                //            SELECT ID, vanning_Master_ID, trans_ref, Vendor_ID, Date, Comments, Amount, Tax_Amount, Total_Amount, temp_ID, 
                                //Created_at, Created_By, Last_Updated_at,
                                //                          Last_Updated_by, transaction_status, isDeleted
                                //FROM pa_Vanning_Master


                                pm.vanning_Master_ID = Convert.ToInt32(dr["vanning_Master_ID"].ToString());

                                pm.vanning_ref = dr["vanning_ref"].ToString();

                                pm.trans_ref = dr["trans_ref"].ToString();
                                pm.Vendor_ID = Convert.ToInt16(dr["Vendor_ID"]);
                                pm.Date = dr["Date"].ToString();
                                pm.Comments = dr["Comments"].ToString();
                                pm.Amount = Convert.ToInt32(dr["Amount"]).ToString();
                                pm.Tax_Amount = Convert.ToInt32(dr["Tax_Amount"]).ToString();
                                pm.Total_Amount = Convert.ToInt32(dr["Total_Amount"]).ToString();
                                pm.temp_ID = dr["temp_ID"].ToString();
                                pm.Balance = Convert.ToInt32(dr["Balance"]).ToString();
                                pm.Paid = Convert.ToInt32(dr["Paid"]).ToString();





                                //PM.Add(pm);
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

        public List<pa_Vanning_Details> get_Vanning_Details_by_TempID_ID(string temp_ID = "00", string ID = "0")
        {
            List<pa_Vanning_Details> RD = new List<pa_Vanning_Details>();
            string sp = "pa_Select_Vanning_Details_by_tempID_ID";
            pa_Vanning_Details o = new pa_Vanning_Details();
            //string sp = "";

            string Type = "BYCHASSIS";
            int Sale_ID = 0;

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@temp_ID", temp_ID);
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@Type", Type);
                        cmd.Parameters.AddWithValue("@Sale_ID", Sale_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {

                                pa_Vanning_Details td = new pa_Vanning_Details();

                                //td.ID, td.Vanning_Master_ID, td.Stock_ID, td.Chassis_No, td.Amount, td.Tax_Amount, td.Total_Amount, td.temp_ID,
                                //td.Created_at, td.Created_By, td.Last_Updated_at, 
                                //     td.Last_Updated_by, td.transaction_status, s.stock_ref, c.color, m.make, tblmake_details.model_description, 
                                //td.From_Location_ID, td.To_Location_ID, td.Date_From, 
                                //    td.Date_To, tblCar_Location.CAR_LOCATION AS From_Location_Name, tblCar_Location_1.CAR_LOCATION AS To_Location_Name

                                td.ID = Convert.ToInt32(dr["ID"].ToString());
                                td.vanning_Master_ID = Convert.ToInt32(dr["vanning_Master_ID"]);
                                //td.Stock_ID = Convert.ToInt32(dr["Stock_ID"]);
                                td.Chassis_no = dr["Chassis_no"].ToString();
                                td.Amount = dr["Amount"].ToString();
                                td.Tax_Amount = dr["Tax_Amount"].ToString();
                                td.Total_Amount = dr["Total_Amount"].ToString();

                                td.Inspection_Charges = dr["Inspection_Charges"].ToString();
                                td.Inspection_ChargesTax = dr["Inspection_ChargesTax"].ToString();



                                td.make = dr["make"].ToString();
                                td.color = dr["color"].ToString();
                                td.model_description = dr["model_description"].ToString();

                                td.make_ID = Convert.ToInt32(dr["make_ID"]);
                                td.model_description_ID = Convert.ToInt32(dr["model_description_ID"]);

                                td.temp_ID = dr["temp_ID"].ToString();


                                RD.Add(td);


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


        //method for getting Sale_Ref from "pa_Sale" table for dropdown list in shippin info page
        public IEnumerable<pa_SalesMaster_DAL> Get_pa_Sale_SaleRef_DAL()
        {
            string sp = "pa_select_Sale_ref_ID";
            List<pa_SalesMaster_DAL> SaleRefList = new List<pa_SalesMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@atype", "VN");

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesMaster_DAL item = new pa_SalesMaster_DAL();

                        item.SaleMaster_ID = Convert.ToInt32(rdr["Sale_ID"]);
                        item.SaleRef = rdr["Sale_Ref"].ToString();


                        SaleRefList.Add(item);

                    }

                    con.Close();

                }

                return SaleRefList;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }






        //Mehtod for getting record on the basis of Dropdown "Sale_refDrop" in shippinginfo.cshtml page

        public List<pa_Vanning_Details> get_pa_Vanning_details_by_Sale_ID_new(string Type, int? Sale_ID, string temp_ID)
        {



            //string sp = "pa_Select_Shipping_Info_details_by_TempID_ID";
            string sp = "pa_Select_Vanning_Details_by_tempID_ID";


            //declaring dummy variables for procedure paramaters

            int ID = 0;



            List<pa_Vanning_Details> main = new List<pa_Vanning_Details>();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sale_ID", Sale_ID);
                        cmd.Parameters.AddWithValue("@Type", Type);
                        cmd.Parameters.AddWithValue("@temp_ID", temp_ID);
                        cmd.Parameters.AddWithValue("@ID", ID);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {



                                //     s.stock_ref, c.color, m.make, tblmake_details.model_description, sd.Chassis_no, sd.stock_ID, sd.ID, sd.temp_ID, sd.Shipping_info_ID

                                pa_Vanning_Details td = new pa_Vanning_Details();

                                td.ID = Convert.ToInt32(dr["ID"].ToString());
                                td.vanning_Master_ID = Convert.ToInt32(dr["vanning_Master_ID"]);
                                td.Stock_ID = Convert.ToInt32(dr["Stock_ID"]);
                                td.Chassis_no = dr["Chassis_no"].ToString();
                                td.Amount = dr["Amount"].ToString();
                                td.Tax_Amount = dr["Tax_Amount"].ToString();
                                td.Total_Amount = dr["Total_Amount"].ToString();
                                td.make = dr["make"].ToString();
                                td.model_description = dr["model_description"].ToString();

                                td.make_ID = Convert.ToInt32(dr["make_ID"]);
                                td.model_description_ID = Convert.ToInt32(dr["model_description_ID"]);
                                td.color = dr["color"].ToString();

                                td.temp_ID = dr["temp_ID"].ToString();






                                main.Add(td);
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


            return main;
        }
        public string GetOldTempIDFromVanningDetail_DAL(int? VanningMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@VanningMaster_ID",VanningMaster_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_SelectVanningDetailOldTempID", paramArray).Tables[0];
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
        public pa_Vanning_Details GetVanningDetailByID_DAL(int? ID)
        {

            string sp = "Pa_Select_VanningDetailByID";

            pa_Vanning_Details td = new pa_Vanning_Details();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                td.ID = Convert.ToInt32(dr["ID"].ToString());
                                td.vanning_Master_ID = Convert.ToInt32(dr["vanning_Master_ID"]);
                                td.Stock_ID = Convert.ToInt32(dr["Stock_ID"]);
                                td.Chassis_no = dr["Chassis_no"].ToString();
                                td.Amount = dr["Amount"].ToString();
                                td.Tax_Amount = dr["Tax_Amount"].ToString();
                                td.Total_Amount = dr["Total_Amount"].ToString();
                                td.make = dr["make"].ToString();
                                td.model_description = dr["model_description"].ToString();
                                td.Inspection_Charges = dr["Inspection_Charges"].ToString();
                                td.Inspection_ChargesTax = dr["Inspection_ChargesTax"].ToString();
                                td.make_ID = Convert.ToInt32(dr["make_ID"]);
                                td.model_description_ID = Convert.ToInt32(dr["model_description_ID"]);
                                td.color = dr["color"].ToString();

                                td.temp_ID = dr["temp_ID"].ToString();


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


            return td;
        }
        public string Cancel_Vanning_DAL(int? Vanning_Master_ID, int Status_ID,  int C_id ,int Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Vanning_Master_ID",Vanning_Master_ID),
                new SqlParameter("@Status_ID",Status_ID),
                new SqlParameter("@last_updated_at",DateTime.Now),
                new SqlParameter("@Modified_by",Modified_by),
                new SqlParameter("@C_id",C_id)



            };


                var response = dbLayer.SP_DataTable_return("pa_Cancel_VanningVoucher", paramArray).Tables[0];
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
        public string Delete_Vanning_DAL(int? ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Vanning_Master_ID",ID),




            };


                var response = dbLayer.SP_DataTable_return("pa_Delete_VanningList", paramArray).Tables[0];
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
        public string DeleteVanningDetail_DAL(int? VanningDetails_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@VanningDetails_ID",VanningDetails_ID)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_VanningDetail", paramArray).Tables[0];
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




        #region Shipping_Info_DAL

        public List<pa_Shipping_Info> get_shipping_info_List(string trans_ref, string StartDate, string EndDate, string Shipper_Name)
        {



            string sp = "pa_Select_Shipping_Info_List";

            List<pa_Shipping_Info> main = new List<pa_Shipping_Info>();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@trans_ref", trans_ref);
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Shipper_Name", Shipper_Name);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {



                                //     s.stock_ref, c.color, m.make, tblmake_details.model_description, sd.Chassis_no, sd.stock_ID, sd.ID, sd.temp_ID, sd.Shipping_info_ID

                                pa_Shipping_Info si = new pa_Shipping_Info();

                                si.Shipping_info_ID = Convert.ToInt32(dr["Shipping_info_ID"].ToString());
                                si.trans_ref = dr["trans_ref"].ToString();
                                si.Shipping_Info_Ref = dr["Shipping_info_Ref"].ToString();
                                si.Shipper_Name = dr["Shipper_Name"].ToString();
                                si.Invoice_Date = dr["Invoice_Date"].ToString();
                                si.Invoice_ref = dr["Invoice_ref"].ToString();
                                si.trans_Date = dr["trans_Date"].ToString();

                                si.Total_Amount = dr["Total_Amount"].ToString();
                                si.Paid = dr["Paid"].ToString();
                                si.Balance = dr["Balance"].ToString();
                                si.transaction_status = Convert.ToInt16(dr["transaction_status"].ToString());
                                si.PaymentStatus = dr["PaymentStatus"].ToString();




                                main.Add(si);
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


            return main;
        }

        public string Insert_Shipping_Info_details_DAL(string Chassis_No, string htemp_ID, string Shipping_info_ID, string Berth_Carry_Charges,
           string Berth_Carry_ChargesTax, int user_ID)
        {


            //@Chassis_No varchar(100),
            //@temp_ID varchar(100),
            //@Shipping_info_ID int,
            //@user_ID int

            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Chassis_No",Chassis_No),
                   new SqlParameter("@temp_ID",htemp_ID),
                    new SqlParameter("@Shipping_info_ID",Shipping_info_ID),
                    new SqlParameter("@Berth_Carry_Charges",Berth_Carry_Charges),

                     new SqlParameter("@Berth_Carry_ChargesTax",Berth_Carry_ChargesTax),


                     new SqlParameter("@user_ID",user_ID)

            };



            //            @Chassis_No varchar(100),
            //@temp_ID varchar(100),
            //@Shipping_info_ID int,
            //@Berth_Carry_Charges varchar(30),
            //@Inspection_Charges varchar(30),
            //@user_ID int

            var response = dbLayer.SP_DataTable_return("pa_Insert_Shipping_Info_details", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }


        public string Update_Shipping_Info_details_DAL(string Chassis_No, string Berth_Carry_Charges, string Berth_Carry_ChargesTax, int user_ID, int ID)
        {


            //@Chassis_No varchar(100),
            //@temp_ID varchar(100),
            //@Shipping_info_ID int,
            //@user_ID int,
            //@ID int




            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {

               new SqlParameter("@Chassis_No",Chassis_No),

                   new SqlParameter("@Berth_Carry_Charges",Berth_Carry_Charges),

                     new SqlParameter("@Berth_Carry_ChargesTax",Berth_Carry_ChargesTax),


                     new SqlParameter("@user_ID",user_ID),
                     new SqlParameter("@ID",ID)
            };





            var response = dbLayer.SP_DataTable_return("pa_Update_Shipping_Info_details", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }

        public string Insert_Shipping_Info_DAL(string Invoice_ref, string Booking_ref, string Shipper_ID, string Vessel_Name, string Vessel_Name_VoyNo,
            int Port_of_Loading_ID, string ETD, int Port_of_Discharge_ID, int Final_Destination_ID, string ETA, string Notify_Party, string Berth,
            string Berth_in_date, string BL_no, string Inspection_Type, string temp_ID, int user_ID, int Sale_ID_For_Inserting, string ContainerType,string Container_Count)
        {

            //@Invoice_ref varchar(100),
            //@Booking_ref varchar(100),
            //@Shipper_ID int,
            //@Vessel_Name varchar(100),
            //@Vessel_Name_VoyNo varchar(100),
            //@Port_of_Loading_ID int,
            //@ETD varchar(50),
            //@Port_of_Discharge_ID int,
            //@Final_Destination_ID int,
            //@ETA varchar(50),
            //@Notify_Party varchar(2000),
            //@Berth varchar(100),
            //@Berth_in_date varchar(50),
            //@Berth_Carry_Charges varchar(30),
            //@BL_no varchar(30),
            //@Inspection_Type varchar(30),
            //@Inspection_Charges varchar(30),
            //@temp_ID varchar(100),
            //@user_ID int



            string msg = "";


            //the Type variable is going is parameter into the procedure "pa_Insert_Shipping_Info" and there is a condition for this variable in the procedure.
            string Type = " ";

            //if sale id will greater than "0" then "Type" value will be "BYSALEREF"
            if (Sale_ID_For_Inserting > 0)
            {
                Type = "BYSALEREF";
            }

            SqlParameter[] paramArray = new SqlParameter[] {

                    new SqlParameter("@Invoice_ref",Invoice_ref),
                    new SqlParameter("@Booking_ref",Booking_ref),
                    new SqlParameter("@Shipper_ID",Shipper_ID),
                    new SqlParameter("@Vessel_Name",Vessel_Name),
                    new SqlParameter("@Vessel_Name_VoyNo",Vessel_Name),
                    new SqlParameter("@Port_of_Loading_ID",Port_of_Loading_ID),
                    new SqlParameter("@ETD",ETD),
                    new SqlParameter("@Port_of_Discharge_ID",Port_of_Discharge_ID),
                    new SqlParameter("@Final_Destination_ID",Final_Destination_ID),
                    new SqlParameter("@ETA",ETA),
                    new SqlParameter("@Notify_Party",Notify_Party),
                    new SqlParameter("@Berth",Berth),
                     new SqlParameter("@Berth_in_date",Berth_in_date),
                      new SqlParameter("@BL_no",BL_no),
                        new SqlParameter("@Inspection_Type",Inspection_Type),
                          new SqlParameter("@temp_ID",temp_ID),
                              new SqlParameter("@user_ID",user_ID),

                              new SqlParameter("@Sale_ID",Sale_ID_For_Inserting),
                              new SqlParameter("@ContainerType",ContainerType),
                              new SqlParameter("@Container_Count",Container_Count),
                              new SqlParameter("@Type",Type)

            };


            var response = dbLayer.SP_DataTable_return("pa_Insert_Shipping_Info", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;



        }


        public string Update_Shipping_Info_DAL(string Invoice_ref, string Invoice_Date, string Booking_ref, string Shipper_ID, string Vessel_Name, string Vessel_Name_VoyNo,
            int Port_of_Loading_ID, string ETD, int Port_of_Discharge_ID, int Final_Destination_ID, string ETA, string Notify_Party, string Berth,
            string Berth_in_date, string BL_no, string Inspection_Type, string temp_ID, int user_ID, int shipinfoId,string ContainerType,string Container_Count)
        {


   



            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {

                    new SqlParameter("@Invoice_ref",Invoice_ref),
                    new SqlParameter("@Invoice_Date",Invoice_Date),
                    new SqlParameter("@Booking_ref",Booking_ref),
                    new SqlParameter("@Shipper_ID",Shipper_ID),
                    new SqlParameter("@Vessel_Name",Vessel_Name),
                    new SqlParameter("@Vessel_Name_VoyNo",Vessel_Name),
                    new SqlParameter("@Port_of_Loading_ID",Port_of_Loading_ID),
                    new SqlParameter("@ETD",ETD),
                    new SqlParameter("@Port_of_Discharge_ID",Port_of_Discharge_ID),
                    new SqlParameter("@Final_Destination_ID",Final_Destination_ID),
                    new SqlParameter("@ETA",ETA),
                    new SqlParameter("@Notify_Party",Notify_Party),
                    new SqlParameter("@Berth",Berth),
                     new SqlParameter("@Berth_in_date",Berth_in_date),
                      new SqlParameter("@BL_no",BL_no),
                        new SqlParameter("@Inspection_Type",Inspection_Type),
                         new SqlParameter("@temp_ID",temp_ID),
                              new SqlParameter("@user_ID",user_ID),
                              new SqlParameter("@ContainerType",ContainerType),
                              new SqlParameter("@Container_Count",Container_Count),
                               new SqlParameter("@shipinfoId",shipinfoId)

            };


            var response = dbLayer.SP_DataTable_return("pa_Update_Shipping_Info", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;



        }


        public pa_Shipping_Info get_shipping_info_by_tempID_ID(string temp_ID = "", string ID = "0")
        {



            string sp = "pa_Select_Shipping_Info_by_TempID_ID";

            pa_Shipping_Info si = new pa_Shipping_Info();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@temp_ID", temp_ID);
                        cmd.Parameters.AddWithValue("@ID", ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {

                                //       ID, Shipping_info_ID, Invoice_ref, Booking_ref, Shipper_ID, Shipper_Name, Vessel_Name, Vessel_Name_VoyNo, Port_of_Loading_ID, ETD, Port_of_Discharge_ID, 
                                //Final_Destination_ID, ETA, Notify_Party, Berth, Berth_in_date, Berth_Carry_Charges, BL_no, Inspection_Type, Inspection_Charges, temp_ID, 
                                //created_at, 
                                //last_updated_at, created_by, last_updated_by, transaction_status

                                si.Shipping_info_ID = Convert.ToInt32(dr["Shipping_info_ID"].ToString());
                                si.trans_ref = dr["trans_ref"].ToString();
                                si.Shipping_Info_Ref = dr["Shipping_info_Ref"].ToString();
                                si.trans_Date = dr["trans_Date"].ToString();
                                si.Invoice_ref = dr["Invoice_ref"].ToString();
                                si.Invoice_Date = dr["Invoice_Date"].ToString();
                                si.Booking_ref = dr["Booking_ref"].ToString();
                                si.Shipper_ID = Convert.ToInt32(dr["Shipper_ID"].ToString());
                                si.Shipper_Name = dr["Shipper_Name"].ToString();
                                si.Vessel_Name = dr["Vessel_Name"].ToString();
                                si.Vessel_Name_VoyNo = dr["Vessel_Name_VoyNo"].ToString();
                                si.Port_of_Loading_ID = Convert.ToInt32(dr["Port_of_Loading_ID"].ToString());
                                si.Port_of_Discharge_ID = Convert.ToInt32(dr["Port_of_Discharge_ID"].ToString());
                                si.Final_Destination_ID = Convert.ToInt32(dr["Final_Destination_ID"].ToString());
                                si.ETD = dr["ETD"].ToString();
                                si.ETA = dr["ETA"].ToString();
                                si.Notify_Party = dr["Notify_Party"].ToString();
                                si.Berth = dr["Berth"].ToString();
                                si.Berth_in_date = dr["Berth_in_date"].ToString();
                                si.Berth_Carry_Charges = dr["Berth_Carry_Charges"].ToString();
                                si.BL_no = dr["BL_no"].ToString();
                                si.Inspection_Type = dr["Inspection_Type"].ToString();
                                si.Inspection_Charges = dr["Inspection_Charges"].ToString();
                                si.temp_ID = dr["temp_ID"].ToString();
                                si.created_at = dr["created_at"].ToString();
                                si.last_updated_at = dr["last_updated_at"].ToString();
                                si.Balance = Convert.ToInt32(dr["Balance"]).ToString();
                                si.Paid = Convert.ToInt32(dr["Paid"]).ToString();
                                si.ContainerType = dr["ContainerType"].ToString();
                                si.Container_Count = dr["Container_Count"].ToString();



                                //PM.Add(pm);
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

        public List<pa_Shipping_Info_details> get_shipping_info_details_by_tempID_ID(string temp_ID = "", string ID = "0")
        {



            //string sp = "pa_Select_Shipping_Info_details_by_TempID_ID";
            string sp = "pa_Select_Shipping_Info_details_by_TempID_ID_new";
            string Type = "BYCHASSIS";
            int Sale_ID = 0;

            List<pa_Shipping_Info_details> main = new List<pa_Shipping_Info_details>();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@temp_ID", temp_ID);
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@Type", Type);
                        cmd.Parameters.AddWithValue("@Sale_ID", Sale_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {



                                //     s.stock_ref, c.color, m.make, tblmake_details.model_description, sd.Chassis_no, sd.stock_ID, sd.ID, sd.temp_ID, sd.Shipping_info_ID

                                pa_Shipping_Info_details si = new pa_Shipping_Info_details();

                                si.Shipping_info_ID = Convert.ToInt32(dr["Shipping_info_ID"].ToString());
                                si.make = dr["make"].ToString();

                                si.make_ID = Convert.ToInt32(dr["make_ID"].ToString());

                                si.model_description = dr["model_description"].ToString();
                                si.color = dr["color"].ToString();
                                si.stock_ref = dr["stock_ref"].ToString();
                                si.Chassis_no = dr["Chassis_no"].ToString();
                                si.Berth_Carry_Charges = dr["Berth_Carry_Charges"].ToString();
                                si.Inspection_Charges = dr["Inspection_Charges"].ToString();

                                si.Berth_Carry_ChargesTax = dr["Berth_Carry_ChargesTax"].ToString();
                                si.Inspection_ChargesTax = dr["Inspection_ChargesTax"].ToString();
                                si.TotalCharges = dr["TotalCharges"].ToString();


                                si.ID = Convert.ToInt32(dr["ID"].ToString());

                                si.temp_ID = dr["temp_ID"].ToString();




                                main.Add(si);
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


            return main;
        }


        //method for getting Sale_Ref from "pa_Sale" table for dropdown list in shippin info page
        public IEnumerable<pa_SalesMaster_DAL> Get_pa_Sale_SaleRef_DAL_Shipping()
        {
            string sp = "pa_select_Sale_ref_ID";
            List<pa_SalesMaster_DAL> SaleRefList = new List<pa_SalesMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@atype", "SH");


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesMaster_DAL item = new pa_SalesMaster_DAL();

                        item.SaleMaster_ID = Convert.ToInt32(rdr["Sale_ID"]);
                        item.SaleRef = rdr["Sale_Ref"].ToString();


                        SaleRefList.Add(item);

                    }

                    con.Close();

                }

                return SaleRefList;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }






        //Mehtod for getting record on the basis of Dropdown "Sale_refDrop" in shippinginfo.cshtml page

        public List<pa_Shipping_Info_details> get_shipping_info_details_by_tempID_ID_new(string Type, int? Sale_ID)
        {



            //string sp = "pa_Select_Shipping_Info_details_by_TempID_ID";
            string sp = "pa_Select_Shipping_Info_details_by_TempID_ID_new";


            //declaring dummy variables for procedure paramaters
            string temp_ID = "";
            int ID = 0;



            List<pa_Shipping_Info_details> main = new List<pa_Shipping_Info_details>();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sale_ID", Sale_ID);
                        cmd.Parameters.AddWithValue("@Type", Type);
                        cmd.Parameters.AddWithValue("@temp_ID", temp_ID);
                        cmd.Parameters.AddWithValue("@ID", ID);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {



                                //     s.stock_ref, c.color, m.make, tblmake_details.model_description, sd.Chassis_no, sd.stock_ID, sd.ID, sd.temp_ID, sd.Shipping_info_ID

                                pa_Shipping_Info_details si = new pa_Shipping_Info_details();

                                si.Shipping_info_ID = Convert.ToInt32(dr["Shipping_info_ID"].ToString());
                                si.make = dr["make"].ToString();
                                si.make_ID = Convert.ToInt32(dr["make_ID"].ToString());
                                si.model_description = dr["model_description"].ToString();
                                si.color = dr["color"].ToString();
                                si.stock_ref = dr["stock_ref"].ToString();
                                si.Chassis_no = dr["Chassis_no"].ToString();
                                si.stock_ID = Convert.ToInt32(dr["stock_ID"].ToString());
                                si.ID = Convert.ToInt32(dr["ID"].ToString());

                                si.temp_ID = dr["temp_ID"].ToString();




                                main.Add(si);
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


            return main;
        }



        public List<pa_Shipping_Info_details> pa_Select_Shipping_Info_details_by_TempID_ID(string Type, int? Sale_ID, string temp_ID)
        {



            //string sp = "pa_Select_Shipping_Info_details_by_TempID_ID";
            string sp = "pa_Select_Shipping_Info_details_by_TempID_ID";


            //declaring dummy variables for procedure paramaters

            int ID = 0;



            List<pa_Shipping_Info_details> main = new List<pa_Shipping_Info_details>();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sale_ID", Sale_ID);
                        cmd.Parameters.AddWithValue("@Type", Type);
                        cmd.Parameters.AddWithValue("@temp_ID", temp_ID);
                        cmd.Parameters.AddWithValue("@ID", ID);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {



                                //     s.stock_ref, c.color, m.make, tblmake_details.model_description, sd.Chassis_no, sd.stock_ID, sd.ID, sd.temp_ID, sd.Shipping_info_ID

                                pa_Shipping_Info_details si = new pa_Shipping_Info_details();

                                si.Shipping_info_ID = Convert.ToInt32(dr["Shipping_info_ID"].ToString());
                                si.make = dr["make"].ToString();
                                si.make_ID = Convert.ToInt32(dr["make_ID"].ToString());
                                si.model_description = dr["model_description"].ToString();
                                si.color = dr["color"].ToString();
                                si.stock_ref = dr["stock_ref"].ToString();
                                si.Chassis_no = dr["Chassis_no"].ToString();
                                si.stock_ID = Convert.ToInt32(dr["stock_ID"].ToString());
                                si.ID = Convert.ToInt32(dr["ID"].ToString());

                                si.temp_ID = dr["temp_ID"].ToString();




                                main.Add(si);
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


            return main;
        }


        public string GetOldTempIDFromShipping_infDetail_DAL(int? Shipping_info_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Shipping_info_ID",Shipping_info_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_SelectShipping_infoDetailOldTempID", paramArray).Tables[0];
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
        public pa_Shipping_Info_details GetShipping_InfoDetailByID_DAL(int? ID)
        {

            string sp = "pa_Select_Shipping_Info_Detail_List";

            pa_Shipping_Info_details si = new pa_Shipping_Info_details();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                si.ID = Convert.ToInt32(dr["ID"].ToString());
                                si.Shipping_info_ID = Convert.ToInt32(dr["Shipping_info_ID"].ToString());
                                si.Chassis_no = dr["Chassis_no"].ToString();
                                si.Inspection_Charges = dr["Inspection_Charges"].ToString();
                                si.Berth_Carry_Charges = dr["Berth_Carry_Charges"].ToString();
                                si.make = dr["make"].ToString();
                                si.model_description = dr["model_description"].ToString();
                                si.make_ID = Convert.ToInt32(dr["Make_ID"].ToString());
                                si.Berth_Carry_ChargesTax = dr["Berth_Carry_ChargesTax"].ToString();
                                si.TotalCharges = dr["TotalCharges"].ToString();


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
        public string Cancel_Shipping_Info_DAL(int? Shipping_info_ID, int Status_ID,int C_ID ,int Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Shipping_info_ID",Shipping_info_ID),
                new SqlParameter("@last_updated_at",DateTime.Now),
                new SqlParameter("@Status_ID",Status_ID),
                new SqlParameter("@c_ID",C_ID),
                new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("pa_Cancel_Shipping_infoVoucher", paramArray).Tables[0];
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
        public string Delete_Shipping_Info_DAL(int? ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Shipping_info_ID",ID),




            };


                var response = dbLayer.SP_DataTable_return("pa_DeleteShippingInfo", paramArray).Tables[0];
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
        public string DeleteShipping_InfoDetail_DAL(int? Shipping_InfoDetails_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@ID",Shipping_InfoDetails_ID)



            };


                var response = dbLayer.SP_DataTable_return("pa_Delete_Shipping_Info_details", paramArray).Tables[0];
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
        public IEnumerable<Pa_Countries_DAL> Get_PortFrom()
        {
            string sp = "pa_Select_PortFrom";
            List<Pa_Countries_DAL> SaleRefList = new List<Pa_Countries_DAL>();

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

                        item.Country_ID = Convert.ToInt32(rdr["Country_ID"]);
                        item.CountryName = rdr["CountryName"].ToString();


                        SaleRefList.Add(item);

                    }

                    con.Close();

                }

                return SaleRefList;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        public IEnumerable<Pa_Countries_DAL> Get_PortTo()
        {
            string sp = "pa_Select_PortTo";
            List<Pa_Countries_DAL> SaleRefList = new List<Pa_Countries_DAL>();

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

                        item.Country_ID = Convert.ToInt32(rdr["Country_ID"]);
                        item.CountryName = rdr["CountryName"].ToString();


                        SaleRefList.Add(item);

                    }

                    con.Close();

                }

                return SaleRefList;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        #endregion

        #region PAPERHSC
        public string Insert_Paper(string CHASSIS_No, string RecievedDate, string paperType, string isRecieved, string _ref, DateTime Created_at, int Created_by, string Modified_at, int modified_by, int isDelete,
           string Vehicle_CC, string Engine_Type, string Registration, DateTime RegistrationDate, DateTime ManufactureDate, string EngineType, string lenght, string width, string height, string Weight, string G_weight, string SEAT, string Drive)
        {
            string result = "DATA NOT SAVED SUCCESSFULLY";
            bool isRecieveds;
            if (isRecieved == "on")
            {
                isRecieveds = true;
            }
            else
            {
                isRecieveds = false;
            }
            if (EngineType == null)
            {
                EngineType = "";
            }
            if (SEAT == null)
            {
                SEAT = "";
            }
            if (Weight == null)
            {
                Weight = "";
            }



            if (lenght == null)
            {
                lenght = "";
            }
            if (width == null)
            {
                width = "";
            }
            if (height == null)
            {
                height = "";
            }
            if (Vehicle_CC == null)
            {
                Vehicle_CC = "";
            }
            if (Drive == null)
            {
                Drive = "";
            }
            if (G_weight == null)
            {
                G_weight = "";
            }
            if (_ref == null)
            {
                _ref = "";
            }
            if (paperType == null)
            {
                paperType = "";
            }
            if (Engine_Type == null)
            {
                Engine_Type = "";
            }
            try
            {


                string sp = "pa_Insert_Paper";
                //string UserId = Session["userID"].ToString();
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    if (CHASSIS_No != "" && CHASSIS_No != null)
                    {
                        using (SqlCommand cmd = new SqlCommand(sp, con))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            //cmd.Parameters.AddWithValue("@paper_ID", 1202);
                            cmd.Parameters.AddWithValue("@CHASSIS_NO", CHASSIS_No);
                            cmd.Parameters.AddWithValue("@Recieved_Date", Convert.ToDateTime(RecievedDate.ToString()));
                            cmd.Parameters.AddWithValue("@IsRecieved", isRecieveds);
                            cmd.Parameters.AddWithValue("@Ref", _ref);
                            cmd.Parameters.AddWithValue("@Created_by", Created_by);
                            cmd.Parameters.AddWithValue("@Created_at", Created_at);
                            cmd.Parameters.AddWithValue("@PaperType", paperType);
                            cmd.Parameters.AddWithValue("@isDeleted", isDelete);
                            cmd.Parameters.AddWithValue("@Modified_by", modified_by);
                            cmd.Parameters.AddWithValue("@Modified_at", Modified_at);
                            cmd.Parameters.AddWithValue("@FUEL_TYPE", EngineType);
                            cmd.Parameters.AddWithValue("@Weight", Weight);
                            cmd.Parameters.AddWithValue("@G_weight", G_weight);

                            cmd.Parameters.AddWithValue("@SEAT", SEAT);
                            var sp_param = new SqlParameter("@Registration", SqlDbType.NVarChar);
                            sp_param.Value = Registration;
                            cmd.Parameters.Add(sp_param);
                            cmd.Parameters.AddWithValue("@FirstRegistrationDate", RegistrationDate);
                            cmd.Parameters.AddWithValue("@ManufacturingDate", ManufactureDate);
                            cmd.Parameters.AddWithValue("@Lenght", lenght);
                            cmd.Parameters.AddWithValue("@width", width);
                            cmd.Parameters.AddWithValue("@height", height);
                            cmd.Parameters.AddWithValue("@Vcc", Vehicle_CC);
                            cmd.Parameters.AddWithValue("@Engine_Type", Engine_Type);

                            cmd.Parameters.AddWithValue("@Drive", Drive);
                            var res = cmd.ExecuteNonQuery();
                            if (res > 0)
                            {
                                result = "DATA SAVED SUCCESSFULLY";

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

            return result;
        }


        public string Update_Paper(int paperid, string CHASSIS_No, string RecievedDate, string paperType, string isRecieved, string _ref, DateTime Created_at, int Created_by, DateTime Modified_at, int modified_by, int isDelete,
                string Vehicle_CC, string Registration, DateTime RegistrationDate, DateTime ManufactureDate, string EngineType, string lenght, string width, string height, string gweight, string weight, string SEAT, string Drive)
        {
            string result = "DATA NOT SAVED SUCCESSFULLY";
            bool isRecieveds;
            if (isRecieved == "on")
            {
                isRecieveds = true;
            }
            else
            {
                isRecieveds = false;
            }
            if (EngineType == null)
            {
                EngineType = "";
            }
            if (SEAT == null)
            {
                SEAT = "";
            }
            if (weight == null)
            {
                weight = "";
            }



            if (lenght == null)
            {
                lenght = "";
            }
            if (width == null)
            {
                width = "";
            }
            if (height == null)
            {
                height = "";
            }
            if (Vehicle_CC == null)
            {
                Vehicle_CC = "";
            }
            if (Drive == null)
            {
                Drive = "";
            }
            if (gweight == null)
            {
                gweight = "";
            }
            if (_ref == null)
            {
                _ref = "";
            }
            if (paperType == null)
            {
                paperType = "";
            }
            if (EngineType == null)
            {
                EngineType = "";
            }
            try
            {


                string sp = "pa_UpdatePaper";
                //string UserId = Session["userID"].ToString();
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    if (CHASSIS_No != "" && CHASSIS_No != null)
                    {
                        using (SqlCommand cmd = new SqlCommand(sp, con))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@paper_ID", paperid);
                            cmd.Parameters.AddWithValue("@CHASSIS_NO", CHASSIS_No);
                            cmd.Parameters.AddWithValue("@Recieved_Date", Convert.ToDateTime(RecievedDate.ToString()));
                            cmd.Parameters.AddWithValue("@IsRecieved", isRecieveds);
                            cmd.Parameters.AddWithValue("@Ref", _ref);
                            cmd.Parameters.AddWithValue("@Created_by", Created_by);
                            cmd.Parameters.AddWithValue("@Created_at", Created_at);
                            cmd.Parameters.AddWithValue("@PaperType", paperType);
                            cmd.Parameters.AddWithValue("@isDeleted", isDelete);
                            cmd.Parameters.AddWithValue("@Modified_by", modified_by);
                            cmd.Parameters.AddWithValue("@Modified_at", Modified_at);
                            cmd.Parameters.AddWithValue("@FUEL_TYPE", EngineType);
                            cmd.Parameters.AddWithValue("@weight", weight);

                            cmd.Parameters.AddWithValue("@SEAT", SEAT);
                            var sp_param = new SqlParameter("@Registration", SqlDbType.NVarChar);
                            sp_param.Value = Registration;
                            cmd.Parameters.Add(sp_param);
                            cmd.Parameters.AddWithValue("@FirstRegistrationDate", RegistrationDate);
                            cmd.Parameters.AddWithValue("@ManufacturingDate", ManufactureDate);
                            cmd.Parameters.AddWithValue("@Lenght", lenght);
                            cmd.Parameters.AddWithValue("@width", width);
                            cmd.Parameters.AddWithValue("@height", height);
                            cmd.Parameters.AddWithValue("@Vcc", Vehicle_CC);
                            cmd.Parameters.AddWithValue("@Drive", Drive);
                            cmd.Parameters.AddWithValue("@gweight", gweight);
                            var res = cmd.ExecuteNonQuery();
                            if (res > 0)
                            {
                                result = "DATA SAVED SUCCESSFULLY";

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

            return result;
        }


        public List<pa_Select_PaperList_Result> get_Papers_List(string ChassisNo, string StartDate, string EndDate)
        {

            string sp = "pa_Select_Papers_List";

            List<pa_Select_PaperList_Result> main = new List<pa_Select_PaperList_Result>();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@chassis_no", ChassisNo);
                        cmd.Parameters.AddWithValue("@start_date", StartDate);
                        cmd.Parameters.AddWithValue("@end_date", EndDate);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {



                                //     s.stock_ref, c.color, m.make, tblmake_details.model_description, sd.Chassis_no, sd.stock_ID, sd.ID, sd.temp_ID, sd.Shipping_info_ID

                                pa_Select_PaperList_Result pi = new pa_Select_PaperList_Result();

                                pi.Chassis = dr["Chassis"].ToString();
                                pi.Purchase_Ref = dr["Purchase_Ref"].ToString();
                                pi.Recieved_Date = dr["Recieved_Date"].ToString();
                                pi.Status = dr["Status"].ToString();
                                pi.paper_ID = Convert.ToInt16(dr["paperID"].ToString());




                                main.Add(pi);
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


            return main;
        }
        public Papers Paper_Details(int? ID)
        {

            string sp = "pa_Select_PaperListBy_ID";

            Papers pm = new Papers();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@paperID", ID);

                    SqlDataReader dr = cmd.ExecuteReader();


                    while (dr.Read())
                    {

                        pm.Paper_ID = Convert.ToInt32(dr["paperID"]);
                        pm.CHASSIS_NO = dr["CHASSIS_NO"].ToString();
                        pm.model = dr["MODEL"].ToString();
                        pm.make_id = dr["make_ID"].ToString();
                        pm.Recieved_Date = dr["Recieved_Date"].ToString();
                        pm.IsRecieved = Convert.ToBoolean(dr["Status"]);
                        pm.Registration = dr["Registration"].ToString();
                        if (!string.IsNullOrEmpty(dr["FirstRegistrationDate"].ToString()))
                        {
                            pm.FirstRegistrationDate = Convert.ToDateTime(dr["FirstRegistrationDate"]);
                        }
                        else
                        {
                            pm.FirstRegistrationDate = DateTime.Now;
                        }
                        if (!string.IsNullOrEmpty(dr["ManufacturingDate"].ToString()))
                        {
                            pm.ManufacturingDate = Convert.ToDateTime(dr["ManufacturingDate"]);
                        }
                        else
                        {
                            pm.ManufacturingDate = DateTime.Now;
                        }

                        pm.Vehicle_CC = dr["VH_CC"].ToString();
                        pm.PaperType = dr["PType"].ToString();
                        pm.Drive = dr["DRIVE"].ToString();
                        pm.FUEL_TYPE = dr["FUEL_TYPE"].ToString();
                        pm.Lenght = dr["Lenght"].ToString();
                        pm.width = dr["Width"].ToString();
                        pm.height = dr["Height"].ToString();
                        pm.weight = dr["WEIGHT"].ToString();
                        pm.gweight = dr["gweight"].ToString();
                        pm.SEAT = dr["seat"].ToString();
                        pm.Ref = dr["ref"].ToString();





                    }

                    con.Close();

                }



            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return pm;

        }

        public string Delete_Paper_DAL(string id)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@paper_ID",id),




            };


                var response = dbLayer.SP_DataTable_return("pa_DeletePaper", paramArray).Tables[0];
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
        public IEnumerable<Wordss> Get_Words()
        {
            string sp = "Select_Words";
            List<Wordss> SaleRefList = new List<Wordss>();

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
                        Wordss item = new Wordss();

                        item.ID = Convert.ToInt32(rdr["ID"]);
                        item.Words = rdr["Words"].ToString();


                        SaleRefList.Add(item);

                    }

                    con.Close();

                }

                return SaleRefList;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        public IEnumerable<Cities> Get_Cities()
        {
            string sp = "Select_Cities";
            List<Cities> SaleRefList = new List<Cities>();

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
                        Cities item = new Cities();

                        item.ID = Convert.ToInt32(rdr["ID"]);
                        item.City_Name = rdr["City_Name"].ToString();


                        SaleRefList.Add(item);

                    }

                    con.Close();

                }

                return SaleRefList;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        #endregion

        #region PurchaseJP

        //---insert into sales booking detial general
        public string InsertPurchaseMasterJPDetail_DAL(string Chassis, int Make, int Model, int Color, int loc, string Ref, string Lot
           , decimal? Price, int PriceRate, decimal PriceTax, decimal RecycleFee,
            decimal auctionfee, decimal auctionfeetax, decimal NumberPlate, decimal TotalPrice,
            string Year, int GoingToLocation, int VendorName, decimal ReksoFee,
            decimal ReksoFeeTax, int StockType, string Transmission, int hdPurchaseMaster_ID, string Temp_ID, int? c_ID, int? Created_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                         new SqlParameter("@Chassis_No", Chassis),
               new SqlParameter("@LotNumber", Lot),
               new SqlParameter("@MakeModelDescription_ID", Model),
               new SqlParameter("@PurchaseRef", Ref),
               new SqlParameter("@LocID", loc),
               new SqlParameter("@Make_ID", Make),
               new SqlParameter("@Color_ID", Color),
               new SqlParameter("@ColorInterior_ID", 0),
                //cmd.Parameters.AddWithValue("@VendorID",Vendor),
      
               new SqlParameter("@PriceOrignal", Price),
               new SqlParameter("@PriceRate", PriceRate),
               new SqlParameter("@PriceTax", PriceTax),
               new SqlParameter("@RecycleFee", RecycleFee),
                //cmd.Parameters.AddWithValue("@RecycleFeeTax",RecycleFeeTax),
               new SqlParameter("@AuctionFee", auctionfee),
               new SqlParameter("@AuctionFeeTax", auctionfeetax),
               new SqlParameter("@PlatNumberFee", NumberPlate),
                //cmd.Parameters.AddWithValue("@PlatNaumberTax",NumberPlateTax),
               new SqlParameter("@Total", TotalPrice),

               new SqlParameter("@temp_ID", Temp_ID),

               new SqlParameter("@Year", Year),
               new SqlParameter("@GoingToLocation", GoingToLocation),
               new SqlParameter("@Vendor_ID", VendorName),
               new SqlParameter("@ReksoFee", ReksoFee),
               new SqlParameter("@ReksoFeeTax", ReksoFeeTax),
               new SqlParameter("@StockType", StockType),
               new SqlParameter("@Transmission", Transmission),
               new SqlParameter("@hdPurchaseMaster_ID", hdPurchaseMaster_ID),


                                new SqlParameter("@c_ID",c_ID),
                                 new SqlParameter("@Created_By",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("pa_InsertPurchaseDetail_JP", paramArray).Tables[0];
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

        //---Update into sales booking detial general
        public string UpdatePurchaseMasterJPDetail_DAL(int? PurchaseDetailID, string Chassis, int? Make, int? Model, int? Color, int? loc, string Lot,
            decimal? Price, decimal? PriceRate, decimal? PriceTax, decimal? RecycleFee,
            decimal? auctionfee, decimal? auctionfeetax, decimal? NumberPlate, decimal? TotalPrice, string Year,
            int? GoingToLocation, int? VendorName,
            decimal? ReksoFee, decimal? ReksoFeeTax, int? StockType, string Transmission, string Temp_ID, int? c_ID, int? Modified_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
               new SqlParameter("@PurchaseDetails_ID",PurchaseDetailID),
               new SqlParameter("@Chassis_No", Chassis),
               new SqlParameter("@LotNumber", Lot),
               new SqlParameter("@MakeModelDescription_ID", Model),
               new SqlParameter("@LocID", loc),
               new SqlParameter("@Make_ID", Make),
               new SqlParameter("@Color_ID", Color),
               new SqlParameter("@ColorInterior_ID", 0),
               new SqlParameter("@PriceOrignal", Price),
               new SqlParameter("@PriceRate", PriceRate),
               new SqlParameter("@PriceTax", PriceTax),
               new SqlParameter("@RecycleFee", RecycleFee),
               new SqlParameter("@AuctionFee", auctionfee),
               new SqlParameter("@AuctionFeeTax", auctionfeetax),
               new SqlParameter("@PlatNumberFee", NumberPlate),
               new SqlParameter("@Total", TotalPrice),
               new SqlParameter("@Year", Year),
               new SqlParameter("@GoingToLocation", GoingToLocation),
               new SqlParameter("@Vendor_ID", VendorName),
               new SqlParameter("@ReksoFee", ReksoFee),
               new SqlParameter("@ReksoFeeTax", ReksoFeeTax),
               new SqlParameter("@StockType", StockType),
               new SqlParameter("@Transmission", Transmission),
               new SqlParameter("@Temp_ID",Temp_ID),
               new SqlParameter("@c_ID",c_ID),
               new SqlParameter("@User_ID",Modified_by)

              //cmd.Parameters.AddWithValue("@VendorID",Vendor),
                 //cmd.Parameters.AddWithValue("@RecycleFeeTax",RecycleFeeTax),
                     //cmd.Parameters.AddWithValue("@PlatNaumberTax",NumberPlateTax),

            };


                var response = dbLayer.SP_DataTable_return("pa_UpdatePurchaseDetail_JP", paramArray).Tables[0];
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


        //Insert Sales booking Master
        public string InsertPurchaseMasterJPMaster_DAL(int? Vendor, string Ref, string PurchaseRef, string Date, string PaymentDueDate, string SupplierContact, string SupplierAddress, string Purchasetype, string Temp_ID, int? c_ID, int? Created_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {


                new SqlParameter("@OtherRef", PurchaseRef),
                new SqlParameter("@Vendor_ID", Vendor),
                new SqlParameter("@PurchaseDate", Date),
                new SqlParameter("@Vendor_Contact", SupplierContact),
                new SqlParameter("@Vendor_Address", SupplierAddress),
                new SqlParameter("@VAT_Rate", 0),
                new SqlParameter("@Discount", 0),
                new SqlParameter("@PurchaseType", Purchasetype),
                new SqlParameter("@PaymentDueDate", PaymentDueDate),
                new SqlParameter("@Temp_ID",Temp_ID),
                new SqlParameter("@c_ID",c_ID),
                new SqlParameter("@Created_by",Created_by),
                new SqlParameter("@PurchaseRef",Ref)
            };


                var response = dbLayer.SP_DataTable_return("Pa_Insert_PurchaseMaster_JP", paramArray).Tables[0];
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


        //Udpate Purchase booking Master
        public string UpdatePurchaseMasterJPMaster_DAL(int? PurchaseMaster_ID, string PurchaseRef, string Date, int? Vendor, string Ref, string SupplierContact, string SupplierAddress, string Temp_ID, int? c_ID, int? Modified_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@OtherRef", Ref),
                new SqlParameter("@Vendor_ID", Vendor),
                new SqlParameter("@PurchaseDate", Date),
                new SqlParameter("@SupplierContact", SupplierContact),
                new SqlParameter("@SupplierAddress", SupplierAddress),
                new SqlParameter("@VatRate", 0),
                new SqlParameter("@VatExp", 0),
                new SqlParameter("@User_ID",Modified_by),
                new SqlParameter("@c_ID",c_ID),
                new SqlParameter("@PurchaseMaster_ID",PurchaseMaster_ID)

            };


                var response = dbLayer.SP_DataTable_return("pa_UpdatePurchaseMaster_JP", paramArray).Tables[0];
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

        //--getting Purchase booking master by id
        public Pa_PurchaseMaster_DAL Get_PurchaseMasterJPMasterByID_DAL(int? PurchaseMaster_ID)
        {

            string sp = "Pa_Select_PurchaseMasterByID_JP";

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

                        cmd.Parameters.AddWithValue("@PurchaseMasterID", PurchaseMaster_ID);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.PurchaseMaster_ID = Convert.ToInt32(dr["PurchaseMaster_ID"]);
                                pm.Vendor_ID = Convert.ToInt32(dr["Vendor_ID"]);
                                pm.PurchaseRef = dr["PurchaseRef"].ToString();
                                pm.PurchaseDate = dr["PurchaseDate"].ToString();
                                pm.Vendor_Address = dr["Vendor_Address"].ToString();
                                pm.Vendor_Contact = dr["Vendor_Contact"].ToString();
                                pm.OtherREF = dr["OtherRef"].ToString();
                                pm.PaymentDueDate = dr["PaymentDueDate"].ToString();
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


        //Get Purchase booking detail list by id
        public IEnumerable<JP_PurchaseDetails_DAL> Get_PurchaseMasterJPDetailListByID_DAL(string Temp_ID, int? PurchaseMaster_ID)
        {



            string sp = "Pa_Select_PurchaseDetailListByID_JP";
            List<JP_PurchaseDetails_DAL> itemlist = new List<JP_PurchaseDetails_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@PurchaseMaster_ID", PurchaseMaster_ID);

                    SqlDataReader dr = cmd.ExecuteReader();


                    while (dr.Read())
                    {
                        JP_PurchaseDetails_DAL pm = new JP_PurchaseDetails_DAL();
                        pm.ID = Convert.ToInt32(dr["ID"].ToString());
                        pm.PurchaseMaster_ID = Convert.ToInt32(dr["PurchaseMaster_ID"].ToString());
                        pm.PurchaseDetails_ID = Convert.ToInt32(dr["PurchaseDetails_ID"].ToString());
                        pm.stock_ID = Convert.ToInt32(dr["stock_ID"].ToString());
                        pm.Unit_Price = Convert.ToDecimal(dr["Unit_Price"]);
                        //pm.MajorQty = Convert.ToInt32(dr["MajorQty"].ToString());
                        //pm.MinorQty = Convert.ToInt32(dr["MinorQty"].ToString());
                        //pm.transaction_status = Convert.ToInt32(dr["transaction_status"].ToString());
                        //pm.isDeleted = Convert.ToInt32(dr["isDeleted"].ToString());
                        pm.Chassis_No = Convert.ToString(dr["Chassis_No"].ToString());
                        pm.PriceOrignal = Convert.ToDecimal(dr["PriceOrignal"].ToString());
                        pm.PriceRate = Convert.ToDecimal(dr["PriceRate"].ToString());
                        pm.PriceTax = Convert.ToDecimal(dr["PriceTax"].ToString());
                        pm.RecycleFee = Convert.ToDecimal(dr["RecycleFee"].ToString());
                        //pm.RecycleFeeTax = Convert.ToDecimal(dr["RecycleFeeTax"].ToString());
                        pm.AuctionFee = Convert.ToDecimal(dr["AuctionFee"].ToString());
                        pm.AuctionFeeTax = Convert.ToDecimal(dr["AuctionFeeTax"].ToString());
                        pm.PlatNumberFee = Convert.ToDecimal(dr["PlatNumberFee"].ToString());
                        pm.PlatNaumberTax = Convert.ToDecimal(dr["PlatNaumberTax"].ToString());
                        pm.Total = Convert.ToDecimal(dr["Total"].ToString());
                        pm.Make_ID = Convert.ToInt32(dr["Make_ID"].ToString());
                        pm.Color_ID = Convert.ToInt32(dr["Color_ID"].ToString());
                        pm.MakeModelDescription_ID = Convert.ToInt32(dr["MakeModelDescription_ID"].ToString());
                        pm.locID = Convert.ToInt32(dr["loc_ID"].ToString());
                        pm.temp_ID = Convert.ToString(dr["temp_ID"].ToString());
                        pm.LotNumber = dr["LotNumber"].ToString();
                        pm.Year = Convert.ToInt32(dr["Year"].ToString());
                        pm.GoingToLocation = Convert.ToInt32(dr["GoingToLocation"].ToString());
                        pm.Vendor_ID = Convert.ToInt32(dr["Vendor_ID"].ToString());
                        pm.ReksoFee = Convert.ToDecimal(dr["ReksoFee"].ToString());
                        pm.ReksoFeeTax = Convert.ToDecimal(dr["ReksoFeeTax"].ToString());
                        pm.StockType = Convert.ToInt32(dr["StockType"].ToString());
                        pm.transmission = dr["transmission"].ToString();
                        pm.Risko_Vendor_ID = Convert.ToInt32(dr["Risko_Vendor_ID"].ToString());
                        pm.color = dr["Color"].ToString();
                        pm.ModelYear = dr["Year"].ToString();

                        pm.make = dr["Make"].ToString();




                        itemlist.Add(pm);

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

        public string GetOldTempIDFromPurchaseDetail_DAL(int? PurchaseMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@PurchaseMaster_ID",PurchaseMaster_ID)

            };


                var response = dbLayer.SP_DataTable_return("SelectPurchaseDetailOldTempID", paramArray).Tables[0];
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
        public string DeletePurchaseMaster_DAL(int? PurchaseMaster_ID, int? User_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@PurchaseMaster_ID",PurchaseMaster_ID),
                new SqlParameter("@User_ID",User_ID)



            };


                var response = dbLayer.SP_DataTable_return("pa_DeletePurchase_JP", paramArray).Tables[0];
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





        public string Cancel_PurchaseJP(int? PurchaseMaster_ID, int? Status_ID, int? c_ID, string Trans_Type, int? Modified_by)
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


                var response = dbLayer.SP_DataTable_return("pa_CanceledPurchaseJP", paramArray).Tables[0];
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

        public string DeletePurchaseDetail_DAL(int? StockDetails_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@ID",StockDetails_ID)



            };


                var response = dbLayer.SP_DataTable_return("pa_DeletePurchaseDetail", paramArray).Tables[0];
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
        public IEnumerable<Pa_PurchaseMaster_DAL> pa_Select_PurchaseMaster(string PurchaseRef, string From_Date, string To_Date, int Vendor_ID, int Status_ID, string ChassisNo, int c_ID)
        {



            string sp = "Pa_Select_PurchaseMasterList";

            List<Pa_PurchaseMaster_DAL> main = new List<Pa_PurchaseMaster_DAL>();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                        cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                        cmd.Parameters.AddWithValue("@From_Date", From_Date);
                        cmd.Parameters.AddWithValue("@To_Date", To_Date);
                        cmd.Parameters.AddWithValue("@Status_ID", Status_ID);
                        cmd.Parameters.AddWithValue("@ChassisNo", ChassisNo);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        
                        SqlDataReader dr = cmd.ExecuteReader();
                        
                            while (dr.Read())
                            {

                                //                             PM.PurchaseMaster_ID, PM.PurchaseRef, CONVERT(varchar(30), PM.PurchaseDate, 105) AS PurchaseDate, vVendors.Vendor_Name , 
                                //ISNULL(Pa_Status.Status, 'Pending') AS PurchaseStatus, CONVERT(varchar(30), ISNULL(PM.Total, 0), 1)   
                                //               AS Total, CONVERT(varchar(30), ISNULL(PM.VAT_Exp, 0), 1) AS VAT_Exp, CONVERT(varchar(30), ISNULL(PM.Total_Amount, 0), 1) AS Total_Amount,
                                //      CONVERT(varchar(30), Paid, 1) AS Paid, CONVERT(varchar(30), Bal_Due, 1) AS Bal_Due,
                                //                  CONVERT(varchar(30), PM.Created_at, 105) AS Created_at, CONVERT(varchar(30), PM.Modified_at, 105) AS Modified_at, AU.UserName AS Created_by, AUM.UserName AS Modified_by
                                //   ,@BeforeVATTotal as BeforeVATTotal , @AfterVATTotal as AfterVATTotal, @TotalVAT_Exp as TotalVAT_Exp , @TotalPaid as TotalPaid ,@TotalBal_due as TotalBal_due ,
                                //CONVERT(varchar(30), ISNULL(PM.Discount, 0), 1) as Discount,@TotalDiscount as TotalDiscount

                                Pa_PurchaseMaster_DAL si = new Pa_PurchaseMaster_DAL();

                                
                                si.PurchaseMaster_ID = Convert.ToInt32(dr["PurchaseMaster_ID"].ToString());
                                si.PurchaseRef = dr["PurchaseRef"].ToString();
                                si.PurchaseDate = dr["PurchaseDate"].ToString();
                                si.Vendor_Name = dr["Vendor_Name"].ToString();
                                si.Total_Amount = dr["Total_Amount"].ToString();

                                si.Paid = dr["Paid"].ToString();
                            //By Yaseen 
                                si.CountCars = dr["CountCars"].ToString();
                            //By Yaseen

                                si.Bal_Due = dr["Bal_Due"].ToString();
                                si.PurchaseStatus = dr["PurchaseStatus"].ToString();
                                si.PurchaseStatus_ID = dr["PurchaseStatus_ID"].ToString();

                                si.transaction_status = dr["transaction_status"].ToString();





                                main.Add(si);
                            }
                        }
                    }
                    con.Close();                
            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }


            return main;
        }
        public IEnumerable<Pa_Countries_DAL> GoingLocation()
        {
            string sp = "pa_Select_GoingLoc";
            List<Pa_Countries_DAL> SaleRefList = new List<Pa_Countries_DAL>();

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

                        item.Country_ID = Convert.ToInt32(rdr["Country_ID"]);
                        item.CountryName = rdr["CountryName"].ToString();


                        SaleRefList.Add(item);

                    }

                    con.Close();

                }

                return SaleRefList;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }




        public Pa_PurchaseMaster_DAL LoadRef()
        {



            string sp = "pa_AutoRefJP";

            Pa_PurchaseMaster_DAL si = new Pa_PurchaseMaster_DAL();
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
                                si.PurchaseRef = dr["PurchaseRef"].ToString();
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

        #endregion
        public IEnumerable<StockDetails> Get_StockList_DAL1(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
            string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID, string VendorName, string ModelYear)
        {

            string sp = "select_StockList";

            List<StockDetails> itemlist = new List<StockDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ChassisNo", ChassisNo);
                        cmd.Parameters.AddWithValue("@Make_ID", Make_ID);
                        cmd.Parameters.AddWithValue("@MakeModel_description_ID", MakeModel_description_ID);
                        cmd.Parameters.AddWithValue("@Color_ID", Color_ID);
                        cmd.Parameters.AddWithValue("@PurchaseStartDate", PurchaseStartDate);
                        cmd.Parameters.AddWithValue("@PurchaseEndDate", PurchaseEndDate);
                        cmd.Parameters.AddWithValue("@BL_NO", BL_NO);
                        cmd.Parameters.AddWithValue("@BOE", BOE);
                        cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                        cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                        cmd.Parameters.AddWithValue("@Stock_Status", Stock_Status);
                        cmd.Parameters.AddWithValue("@loc_ID", loc_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);
                        cmd.Parameters.AddWithValue("@VendorName", VendorName);
                        cmd.Parameters.AddWithValue("@ModelYear", ModelYear);


                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                //stock_ID, Chassis_No,  Make, ModelDesciption, Color, StockType,Transmission, CarLocation, Total_Cost,PurchaseRef,PurchaseDate, Stock_Status,Vendor_Name,ModelYear

                                StockDetails item = new StockDetails();
                                item.stock_ID = Convert.ToInt32(dr["Stock_ID"]);
                                item.Chassis_No = dr["Chassis_No"].ToString();
                                item.Vendor_Name = dr["Vendor_Name"].ToString();
                                item.Make = dr["Make"].ToString();
                                item.ModelDesciption = dr["ModelDesciption"].ToString();
                                item.ModelYear = dr["ModelYear"].ToString();
                                item.Color = dr["Color"].ToString();
                                item.StockTypeName = dr["StockTypeName"].ToString();
                                item.Transmission = dr["Transmission"].ToString();
                                item.CarLocation = dr["CarLocation"].ToString();
                                item.Total_Cost = dr["Total_Cost"].ToString();
                                item.PurchaseRef = dr["PurchaseRef"].ToString();
                                item.PurchaseDate = dr["PurchaseDate"].ToString();
                                item.Stock_Status = dr["Stock_Status"].ToString();
                                item.CarLocation = dr["CarLocation"].ToString();

                                itemlist.Add(item);

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


            return itemlist;
        }


        public IEnumerable<StockDetails> Get_StockList_TTL_DAL1(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
     string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID)
        {
            string sp = "select_StockList_TTL";

            List<StockDetails> itemlist = new List<StockDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ChassisNo", ChassisNo);
                        cmd.Parameters.AddWithValue("@Make_ID", Make_ID);
                        cmd.Parameters.AddWithValue("@MakeModel_description_ID", MakeModel_description_ID);
                        cmd.Parameters.AddWithValue("@Color_ID", Color_ID);
                        cmd.Parameters.AddWithValue("@PurchaseStartDate", PurchaseStartDate);
                        cmd.Parameters.AddWithValue("@PurchaseEndDate", PurchaseEndDate);
                        cmd.Parameters.AddWithValue("@BL_NO", BL_NO);
                        cmd.Parameters.AddWithValue("@BOE", BOE);
                        cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                        cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                        cmd.Parameters.AddWithValue("@Stock_Status", Stock_Status);
                        cmd.Parameters.AddWithValue("@loc_ID", loc_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);


                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                //stock_ID, Chassis_No,  Make, ModelDesciption, Color, StockType,Transmission, CarLocation, Total_Cost,PurchaseRef,PurchaseDate, Stock_Status,Vendor_Name,ModelYear

                                StockDetails item = new StockDetails();

                                item.Total_All_Cost = dr["Total_All_Cost"].ToString();


                                itemlist.Add(item);

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


            return itemlist;

        }


        public StockDetails Get_StockList_stats_DAL1(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
        string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID)
        {

            string sp = "select_StockList_stats";

            StockDetails pm = new StockDetails();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ChassisNo", ChassisNo);
                        cmd.Parameters.AddWithValue("@Make_ID", Make_ID);
                        cmd.Parameters.AddWithValue("@MakeModel_description_ID", MakeModel_description_ID);
                        cmd.Parameters.AddWithValue("@Color_ID", Color_ID);
                        cmd.Parameters.AddWithValue("@PurchaseStartDate", PurchaseStartDate);
                        cmd.Parameters.AddWithValue("@PurchaseEndDate", PurchaseEndDate);
                        cmd.Parameters.AddWithValue("@BL_NO", BL_NO);
                        cmd.Parameters.AddWithValue("@BOE", BOE);
                        cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                        cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                        cmd.Parameters.AddWithValue("@Stock_Status", Stock_Status);
                        cmd.Parameters.AddWithValue("@loc_ID", loc_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {

                                pm.StockStatus_UNSOLD = dr["UNSOLD"].ToString();
                                pm.StockStatus_SOLD = dr["SOLD"].ToString();
                                pm.StockStatus_BOOKING = dr["BOOKING"].ToString();
                                pm.StockStatus_CXX = dr["CXX"].ToString();
                                pm.StockStatus_TOTALSTOCK = dr["TOTAL_STOCK"].ToString();


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
        public List<pa_Vanning_Master> get_Vanning_List_Total(string trans_ref, string StartDate, string EndDate, string Shipper_Name)
        {



            string sp = "pa_Select_VanningMaster_List_TTL";

            List<pa_Vanning_Master> main = new List<pa_Vanning_Master>();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@VanningRef", trans_ref);
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@vendor_name", Shipper_Name);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {


                                //     s.stock_ref, c.color, m.make, tblmake_details.model_description, sd.Chassis_no, sd.stock_ID, sd.ID, sd.temp_ID, sd.Shipping_info_ID

                                pa_Vanning_Master si = new pa_Vanning_Master();


                                si.Total_Amount = dr["Total_Amount"].ToString();
                                si.Paid = dr["Paid"].ToString();
                                si.Balance = dr["Balance"].ToString();




                                main.Add(si);
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


            return main;
        }

        //shipping_infolist

        public List<pa_Shipping_Info> get_shipping_info_List_Total(string trans_ref, string StartDate, string EndDate, string Shipper_Name)
        {



            string sp = "pa_Select_Shipping_Info_List_ttl";

            List<pa_Shipping_Info> main = new List<pa_Shipping_Info>();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@trans_ref", trans_ref);
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Shipper_Name", Shipper_Name);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {



                                //     s.stock_ref, c.color, m.make, tblmake_details.model_description, sd.Chassis_no, sd.stock_ID, sd.ID, sd.temp_ID, sd.Shipping_info_ID

                                pa_Shipping_Info si = new pa_Shipping_Info();



                                si.Total_Amount = dr["Total_Amount"].ToString();
                                si.Paid = dr["Paid"].ToString();
                                si.Balance = dr["Balance"].ToString();





                                main.Add(si);
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


            return main;
        }

        //reksolist total
        public IEnumerable<pa_select_StockParties_Result> pa_select_StockParties_total(string purchaseRef, string startDate, string endDate, int RekSo_Vendor_ID)
        {
            List<pa_select_StockParties_Result> pm = new List<pa_select_StockParties_Result>();
            string sp = "pa_select_StockParties_ttl";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PurchaseRef", purchaseRef);
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);
                        cmd.Parameters.AddWithValue("@RekSo_Vendor_ID", RekSo_Vendor_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_select_StockParties_Result PM = new pa_select_StockParties_Result();

                                PM.Total_Payable = dr["Total_Payable"].ToString();
                                PM.Paid = dr["Paid"].ToString();
                                PM.Balance = dr["Balance"].ToString();


                                pm.Add(PM);
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


        //purchaselistjp total:

        //new faraz purchasejp total work

        //Get new purchase master  List
        public IEnumerable<Pa_PurchaseMaster_DAL> Get_PurchaseMasterList_Total(string PurchaseRef, int? Vendor_ID, string From_Date, string To_Date, int? Status_ID, string ChassisNo, int? c_ID)
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

                        item.Total = rdr["Total"].ToString();
                        item.VAT_Exp = rdr["TotalVAT_Exp"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();
                        item.Paid = rdr["TotalPaid"].ToString();
                        item.Bal_Due = rdr["TotalBal_due"].ToString();



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


        public IEnumerable<Pa_Attachments_DAL> GetStockMasterNew_Attachments_DAL(int? StockMaster_ID, string Type)
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
                    cmd.Parameters.AddWithValue("@Master_ID", StockMaster_ID);
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


        public string Insert_StockMasterAttachment_DAL(int? StockMaster_ID, string Document_Type, string Type, string newFileName, string filepath, int? Created_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Master_ID",StockMaster_ID),
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

        public string Delete_Attachment_StockNew(int? Attachment_ID)
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


        #region Auction
        public Pa_Auction_Master Get_AuctionMasterByID_DAL(int? Auction_ID)
        {

            string sp = "Select_Auction_By_ID";

            Pa_Auction_Master pm = new Pa_Auction_Master();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Auction_ID", Auction_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.Auction_ID = Convert.ToInt32(dr["Auction_ID"]);

                                pm.Ref = dr["Ref"].ToString();
                                pm.Auction_Date = dr["Auction_Date"].ToString();
                                pm.Remarks = dr["Remarks"].ToString();
                                pm.Created_At = dr["Created_At"].ToString();
                                pm.Created_By = dr["Created_By"].ToString();
                                pm.Last_Updated_At = dr["Last_Updated_At"].ToString();
                                pm.Last_Updated_By = dr["Last_Updated_By"].ToString();

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
        public IEnumerable<Pa_Auction_Detsils> Get_AuctionDetailListBy_DAL(string Temp_ID, int? Auction_ID)
        {



            string sp = "Select_Auction_Dtls_by_ID";
            List<Pa_Auction_Detsils> itemlist = new List<Pa_Auction_Detsils>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@Auction_ID", Auction_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Auction_Detsils item = new Pa_Auction_Detsils();

                        item.ID = Convert.ToInt32(rdr["ID"]);

                        item.Detail_ID = Convert.ToInt32(rdr["Auction_Details_ID"]);
                        item.Auction_ID = Convert.ToInt32(rdr["Auction_ID"]);
                        item.Stock_ID = Convert.ToInt32(rdr["Stock_ID"]);

                        item.Chassis = rdr["Chassis_No"].ToString();
                        item.Make = rdr["Make"].ToString();
                        item.Model = rdr["ModelDesciption"].ToString();
                        item.Year = rdr["ModelYear"].ToString();





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
        public IEnumerable<Pa_Auction_Master> Get_AuctionList_DAL()
        {



            string sp = "Select_Auction_LIST";
            List<Pa_Auction_Master> itemlist = new List<Pa_Auction_Master>();

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
                        Pa_Auction_Master item = new Pa_Auction_Master();

                        item.ID = Convert.ToInt32(rdr["ID"]);

                        item.Auction_ID = Convert.ToInt32(rdr["Auction_ID"]);
                        item.Ref = rdr["Ref"].ToString();


                        item.Remarks = rdr["Remarks"].ToString();
                        item.Auction_Date = rdr["Auction_Date"].ToString();






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
        public string InsertAuctionDetail_DAL(int? HdAuction_ID, int? Stock_ID
        , string Temp_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {




                               new SqlParameter("@Temp_ID",Temp_ID),
                               new SqlParameter("@HdAuction_ID",HdAuction_ID),
                               new SqlParameter("@Stock_ID",Stock_ID),





            };


                var response = dbLayer.SP_DataTable_return("INSERT_Auction_Dtl", paramArray).Tables[0];
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

        public string InsertAuctionMaster_DAL(string Auction_Date, string Ref, string Remarks,
     string Temp_ID, int? c_ID, int? Created_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Auction_Date",Auction_Date),
                new SqlParameter("@Ref",Ref),
                new SqlParameter("@Remarks",Remarks),
                new SqlParameter("@UserID",Created_by),

                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),







            };


                var response = dbLayer.SP_DataTable_return("Insert_Auction", paramArray).Tables[0];
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
        public string UpdateAuctionMaster_DAL(string Auction_Date, string Ref, string Remarks, int? Auction_ID,
          string Temp_ID, int? c_ID, int? Modified_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@Auction_ID",Auction_ID),
                new SqlParameter("@Auction_Date",Auction_Date),
                new SqlParameter("@Ref",Ref),
                new SqlParameter("@Remarks",Remarks),




                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@UserID",Modified_by)




            };


                var response = dbLayer.SP_DataTable_return("Update_Auction", paramArray).Tables[0];
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


        public string DeleteAuctionDetail_DAL(int? Detail_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Detail_ID",Detail_ID)



            };


                var response = dbLayer.SP_DataTable_return("delete_Auction_Detail", paramArray).Tables[0];
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

        public IEnumerable<Pa_Auction_Master> Get_AuctionMasterInvoiceList_DAL(string Chassis, string StartDate, string EndDate
    , int c_ID)

        {

            string sp = "Select_Auction_List";
            List<Pa_Auction_Master> itemlist = new List<Pa_Auction_Master>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Chassis", Chassis);
                    cmd.Parameters.AddWithValue("@Start_Date", StartDate);
                    cmd.Parameters.AddWithValue("@End_Date", EndDate);

                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader dr = cmd.ExecuteReader();


                    while (dr.Read())
                    {
                        Pa_Auction_Master pm = new Pa_Auction_Master();
                        pm.Auction_ID = Convert.ToInt32(dr["Auction_ID"]);

                        pm.Ref = dr["Ref"].ToString();
                        pm.Auction_Date = dr["Auction_Date"].ToString();







                        pm.Created_At = dr["Created_At"].ToString();
                        pm.Created_By = dr["Created_By"].ToString();
                        pm.Last_Updated_At = dr["Last_Updated_At"].ToString();
                        pm.Last_Updated_By = dr["Last_Updated_By"].ToString();


                        itemlist.Add(pm);

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
        public IEnumerable<StockDetails> StockList { get; set; }
        public IEnumerable<Pa_BL_Master> BLMasterList { get ; set ; }
        public Pa_BL_Master BLMasterObj { get ; set ; }
        public IEnumerable<Pa_BL_Detsils> BLDetailList { get ; set ; }
        public IEnumerable<Pa_Garage_Master> GarageMasterList { get ; set; }
        public Pa_Garage_Master GarageMasterObj { get ; set ; }
        public IEnumerable<Pa_Garage_Details> GarageDetailList { get ; set ; }


        public string GetOldTempIDFromAuctionDetail_DAL(int? Auction_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Auction_ID",Auction_ID)

            };


                var response = dbLayer.SP_DataTable_return("SelectAuctionDetailOldTempID", paramArray).Tables[0];
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





        public IEnumerable<StockDetails> Get_AllStock_DAL()

        {

            string sp = "SelectAllStock";
            List<StockDetails> itemlist = new List<StockDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    SqlDataReader dr = cmd.ExecuteReader();


                    while (dr.Read())
                    {
                        StockDetails pm = new StockDetails();
                        pm.stock_ID = Convert.ToInt32(dr["stock_ID"]);
                        pm.Chassis_No = dr["Chassis_No"].ToString();

                        pm.Make = dr["Make"].ToString();
                        pm.ModelDesciption = dr["ModelDesciption"].ToString();







                        pm.ModelYear = dr["ModelYear"].ToString();



                        itemlist.Add(pm);

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




        public IEnumerable<StockDetails> Get_AuctionList_DAL(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
         string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID, string VendorName, string ModelYear, int Auction_ID)
        {

            string sp = "select_AuctionList";

            List<StockDetails> itemlist = new List<StockDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ChassisNo", ChassisNo);
                        cmd.Parameters.AddWithValue("@Make_ID", Make_ID);
                        cmd.Parameters.AddWithValue("@MakeModel_description_ID", MakeModel_description_ID);
                        cmd.Parameters.AddWithValue("@Color_ID", Color_ID);
                        cmd.Parameters.AddWithValue("@PurchaseStartDate", PurchaseStartDate);
                        cmd.Parameters.AddWithValue("@PurchaseEndDate", PurchaseEndDate);
                        cmd.Parameters.AddWithValue("@BL_NO", BL_NO);
                        cmd.Parameters.AddWithValue("@BOE", BOE);
                        cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                        cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                        cmd.Parameters.AddWithValue("@Stock_Status", Stock_Status);
                        cmd.Parameters.AddWithValue("@loc_ID", loc_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);
                        cmd.Parameters.AddWithValue("@VendorName", VendorName);
                        cmd.Parameters.AddWithValue("@ModelYear", ModelYear);
                        cmd.Parameters.AddWithValue("@Auction_ID", Auction_ID);


                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                //stock_ID, Chassis_No,  Make, ModelDesciption, Color, StockType,Transmission, CarLocation, Total_Cost,PurchaseRef,PurchaseDate, Stock_Status,Vendor_Name,ModelYear

                                StockDetails item = new StockDetails();
                                item.stock_ID = Convert.ToInt32(dr["Stock_ID"]);
                                item.Auction_ID = Convert.ToInt32(dr["Auction_ID"]);
                                item.Chassis_No = dr["Chassis_No"].ToString();
                                item.Vendor_Name = dr["Vendor_Name"].ToString();
                                item.Make = dr["Make"].ToString();
                                item.ModelDesciption = dr["ModelDesciption"].ToString();
                                item.ModelYear = dr["ModelYear"].ToString();
                                item.Color = dr["Color"].ToString();
                                item.StockTypeName = dr["StockTypeName"].ToString();
                                item.Transmission = dr["Transmission"].ToString();
                                item.CarLocation = dr["CarLocation"].ToString();
                                item.Total_Cost = dr["Total_Cost"].ToString();
                                item.PurchaseRef = dr["PurchaseRef"].ToString();
                                item.PurchaseDate = dr["PurchaseDate"].ToString();
                                item.Stock_Status = dr["Stock_Status"].ToString();
                                item.InteriorColor = dr["InteriorColor"].ToString();
                                item.ProuctionDate = dr["ProuctionDate"].ToString();
                                item.mileage = dr["mileage"].ToString();
                                item.Selling_Price = dr["Selling_Price"].ToString();



                                itemlist.Add(item);

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


            return itemlist;
        }
        public IEnumerable<StockDetails> Get_AuctionList_TTL_DAL(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
         string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID, string VendorName, string ModelYear)
        {

            string sp = "select_AuctionList_TTL";

            List<StockDetails> itemlist = new List<StockDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ChassisNo", ChassisNo);
                        cmd.Parameters.AddWithValue("@Make_ID", Make_ID);
                        cmd.Parameters.AddWithValue("@MakeModel_description_ID", MakeModel_description_ID);
                        cmd.Parameters.AddWithValue("@Color_ID", Color_ID);
                        cmd.Parameters.AddWithValue("@PurchaseStartDate", PurchaseStartDate);
                        cmd.Parameters.AddWithValue("@PurchaseEndDate", PurchaseEndDate);
                        cmd.Parameters.AddWithValue("@BL_NO", BL_NO);
                        cmd.Parameters.AddWithValue("@BOE", BOE);
                        cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                        cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                        cmd.Parameters.AddWithValue("@Stock_Status", Stock_Status);
                        cmd.Parameters.AddWithValue("@loc_ID", loc_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);
                        cmd.Parameters.AddWithValue("@VendorName", VendorName);
                        cmd.Parameters.AddWithValue("@ModelYear", ModelYear);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                //stock_ID, Chassis_No,  Make, ModelDesciption, Color, StockType,Transmission, CarLocation, Total_Cost,PurchaseRef,PurchaseDate, Stock_Status,Vendor_Name,ModelYear

                                StockDetails item = new StockDetails();

                                item.Total_All_Cost = dr["Total_All_Cost"].ToString();


                                itemlist.Add(item);

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


            return itemlist;
        }


        public StockDetails Get_AuctionList_stats_DAL(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
           string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID, string VendorName, string ModelYear)
        {

            string sp = "select_StockList_stats";

            StockDetails pm = new StockDetails();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ChassisNo", ChassisNo);
                        cmd.Parameters.AddWithValue("@Make_ID", Make_ID);
                        cmd.Parameters.AddWithValue("@MakeModel_description_ID", MakeModel_description_ID);
                        cmd.Parameters.AddWithValue("@Color_ID", Color_ID);
                        cmd.Parameters.AddWithValue("@PurchaseStartDate", PurchaseStartDate);
                        cmd.Parameters.AddWithValue("@PurchaseEndDate", PurchaseEndDate);
                        cmd.Parameters.AddWithValue("@BL_NO", BL_NO);
                        cmd.Parameters.AddWithValue("@BOE", BOE);
                        cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                        cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                        cmd.Parameters.AddWithValue("@Stock_Status", Stock_Status);
                        cmd.Parameters.AddWithValue("@loc_ID", loc_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);
                        cmd.Parameters.AddWithValue("@VendorName", VendorName);
                        cmd.Parameters.AddWithValue("@ModelYear", ModelYear);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {

                                pm.StockStatus_UNSOLD = dr["UNSOLD"].ToString();
                                pm.StockStatus_SOLD = dr["SOLD"].ToString();
                                pm.StockStatus_BOOKING = dr["BOOKING"].ToString();
                                pm.StockStatus_CXX = dr["CXX"].ToString();
                                pm.StockStatus_TOTALSTOCK = dr["TOTAL_STOCK"].ToString();


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
        public pa_Shipping_Info get_shipping_info_by_ID_Print(int ID)
        {



            string sp = "pa_Select_Shipping_Info_by_ID_Print";

            pa_Shipping_Info si = new pa_Shipping_Info();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
 
                        cmd.Parameters.AddWithValue("@ID", ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {

                                //       ID, Shipping_info_ID, Invoice_ref, Booking_ref, Shipper_ID, Shipper_Name, Vessel_Name, Vessel_Name_VoyNo, Port_of_Loading_ID, ETD, Port_of_Discharge_ID, 
                                //Final_Destination_ID, ETA, Notify_Party, Berth, Berth_in_date, Berth_Carry_Charges, BL_no, Inspection_Type, Inspection_Charges, temp_ID, 
                                //created_at, 
                                //last_updated_at, created_by, last_updated_by, transaction_status

                                si.Shipping_info_ID = Convert.ToInt32(dr["Shipping_info_ID"].ToString());
                                si.trans_ref = dr["trans_ref"].ToString();
                                si.Shipping_Info_Ref = dr["Shipping_info_Ref"].ToString();
                                si.trans_Date = dr["trans_Date"].ToString();
                                si.Invoice_ref = dr["Invoice_ref"].ToString();
                                si.Invoice_Date = dr["Invoice_Date"].ToString();
                                si.Booking_ref = dr["Booking_ref"].ToString();
             
                                si.Shipper_Name = dr["Shipper_Name"].ToString();
                                si.Vessel_Name = dr["Vessel_Name"].ToString();
                                si.Vessel_Name_VoyNo = dr["Vessel_Name_VoyNo"].ToString();
          
                                si.ETD = dr["ETD"].ToString();
                                si.ETA = dr["ETA"].ToString();
                                si.Notify_Party = dr["Notify_Party"].ToString();
                                si.Berth = dr["Berth"].ToString();
                                si.Berth_in_date = dr["Berth_in_date"].ToString();
                                si.Berth_Carry_Charges = dr["Berth_Carry_Charges"].ToString();
                                si.BL_no = dr["BL_no"].ToString();
                                si.Inspection_Type = dr["Inspection_Type"].ToString();
                                si.Inspection_Charges = dr["Inspection_Charges"].ToString();
                
                                si.Balance = Convert.ToInt32(dr["Balance"]).ToString();
                                si.Paid = Convert.ToInt32(dr["Paid"]).ToString();
                                si.PortFrom = dr["PortFrom"].ToString();
                                si.PortTo = dr["PortTo"].ToString();
                                si.Destination = dr["Destination"].ToString();
                                si.Shipper = dr["Shipper"].ToString();
                                si.SaleRef = dr["SaleRef"].ToString();



                                //PM.Add(pm);
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
        public List<pa_Shipping_Info_details> pa_Select_Shipping_Info_details_by_Print_ID(int ID)
        {
            string sp = "pa_Select_Shipping_Info_details_by_Print_ID";








            List<pa_Shipping_Info_details> main = new List<pa_Shipping_Info_details>();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
  
                        cmd.Parameters.AddWithValue("@ID", ID);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {



                                //     s.stock_ref, c.color, m.make, tblmake_details.model_description, sd.Chassis_no, sd.stock_ID, sd.ID, sd.temp_ID, sd.Shipping_info_ID

                                pa_Shipping_Info_details si = new pa_Shipping_Info_details();

                                si.Shipping_info_ID = Convert.ToInt32(dr["Shipping_info_ID"].ToString());
                                si.make = dr["make"].ToString();

                                si.make_ID = Convert.ToInt32(dr["make_ID"].ToString());

                                si.model_description = dr["model_description"].ToString();
                                si.color = dr["color"].ToString();
                                si.stock_ref = dr["stock_ref"].ToString();
                                si.Chassis_no = dr["Chassis_no"].ToString();
                                si.Berth_Carry_Charges = dr["Berth_Carry_Charges"].ToString();
                                si.Inspection_Charges = dr["Inspection_Charges"].ToString();

                                si.Berth_Carry_ChargesTax = dr["Berth_Carry_ChargesTax"].ToString();
                                si.Inspection_ChargesTax = dr["Inspection_ChargesTax"].ToString();
                                si.TotalCharges = dr["TotalCharges"].ToString();




                                main.Add(si);
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


            return main;
        }
        public pa_Vanning_Master get_Vanning_Master_by_ID_Print(int ID)
        {



            string sp = "pa_Select_Vanning_Master_by_ID_Print";

            pa_Vanning_Master pm = new pa_Vanning_Master();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                
                        cmd.Parameters.AddWithValue("@ID", ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                //            SELECT ID, vanning_Master_ID, trans_ref, Vendor_ID, Date, Comments, Amount, Tax_Amount, Total_Amount, temp_ID, 
                                //Created_at, Created_By, Last_Updated_at,
                                //                          Last_Updated_by, transaction_status, isDeleted
                                //FROM pa_Vanning_Master


                                pm.vanning_Master_ID = Convert.ToInt32(dr["vanning_Master_ID"].ToString());

                                pm.vanning_ref = dr["vanning_ref"].ToString();

                                pm.trans_ref = dr["trans_ref"].ToString();
                                pm.Date = dr["Date"].ToString();
                                pm.Comments = dr["Comments"].ToString();
                                pm.Amount = Convert.ToInt32(dr["Amount"]).ToString();
                                pm.Tax_Amount = Convert.ToInt32(dr["Tax_Amount"]).ToString();
                                pm.Total_Amount = Convert.ToInt32(dr["Total_Amount"]).ToString();
                                pm.Balance = Convert.ToInt32(dr["Balance"]).ToString();
                                pm.Paid = Convert.ToInt32(dr["Paid"]).ToString();
                                pm.VendorName = dr["VendorName"].ToString();
                                pm.SaleRef = dr["SaleRef"].ToString();




                                //PM.Add(pm);
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

        public List<pa_Vanning_Details> get_Vanning_Details_by_ID_Print(int ID)
        {
            List<pa_Vanning_Details> RD = new List<pa_Vanning_Details>();
            string sp = "pa_Select_Vanning_Details_by_ID_Print";
            pa_Vanning_Details o = new pa_Vanning_Details();


            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
  
                        cmd.Parameters.AddWithValue("@ID", ID);
             

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {

                                pa_Vanning_Details td = new pa_Vanning_Details();
                  
                                td.vanning_Master_ID = Convert.ToInt32(dr["vanning_Master_ID"]);
                                //td.Stock_ID = Convert.ToInt32(dr["Stock_ID"]);
                                td.Chassis_no = dr["Chassis_no"].ToString();
                                td.Amount = dr["Amount"].ToString();
                                td.Tax_Amount = dr["Tax_Amount"].ToString();
                                td.Total_Amount = dr["Total_Amount"].ToString();

                                td.Inspection_Charges = dr["Inspection_Charges"].ToString();
                                td.Inspection_ChargesTax = dr["Inspection_ChargesTax"].ToString();



                                td.make = dr["make"].ToString();
                                td.color = dr["color"].ToString();
                                td.model_description = dr["model_description"].ToString();

                                RD.Add(td);


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
        public Pa_PurchaseMaster_DAL Get_PurchaseMasterJPMasterByID_Print(int? PurchaseMaster_ID)
        {

            string sp = "Pa_Select_PurchaseMasterByID_JP_Print";

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

                        cmd.Parameters.AddWithValue("@PurchaseMasterID", PurchaseMaster_ID);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.PurchaseMaster_ID = Convert.ToInt32(dr["PurchaseMaster_ID"]);
                                pm.Vendor_ID = Convert.ToInt32(dr["Vendor_ID"]);
                                pm.PurchaseRef = dr["PurchaseRef"].ToString();
                                pm.PurchaseDate = dr["PurchaseDate"].ToString();
                                pm.Vendor_Address = dr["Vendor_Address"].ToString();
                                pm.Vendor_Contact = dr["Vendor_Contact"].ToString();
                                pm.PaymentDueDate = dr["PaymentDueDate"].ToString();
                           
                                pm.Vendor_Name = dr["VendorName"].ToString();


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

        public static decimal Parse(string input)
        {
            if(!string.IsNullOrEmpty(input) && !string.IsNullOrWhiteSpace(input))
            {
                return decimal.Parse(Regex.Replace(input, @"[^\d.]", ""));
            }
            else
            {
                return decimal.Parse("0");
            }
          
        }
        //Get Purchase booking detail list by id
        public IEnumerable<JP_PurchaseDetails_DAL> Get_PurchaseMasterJPDetailListByID_Print(int? PurchaseMaster_ID)
        {



            string sp = "Pa_Select_PurchaseDetailListByID_JP_Print";
            List<JP_PurchaseDetails_DAL> itemlist = new List<JP_PurchaseDetails_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
       
                    cmd.Parameters.AddWithValue("@PurchaseMaster_ID", PurchaseMaster_ID);

                    SqlDataReader dr = cmd.ExecuteReader();


                    while (dr.Read())
                    {
                        JP_PurchaseDetails_DAL pm = new JP_PurchaseDetails_DAL();
                        pm.ID = Convert.ToInt32(dr["ID"].ToString());
                        pm.PurchaseMaster_ID = Convert.ToInt32(dr["PurchaseMaster_ID"].ToString());
                        pm.PurchaseDetails_ID = Convert.ToInt32(dr["PurchaseDetails_ID"].ToString());
                        pm.stock_ID = Convert.ToInt32(dr["stock_ID"].ToString());
                        pm.Unit_Price = Parse(dr["Unit_Price"].ToString());
                        //pm.MajorQty = Convert.ToInt32(dr["MajorQty"].ToString());
                        //pm.MinorQty = Convert.ToInt32(dr["MinorQty"].ToString());
                        //pm.transaction_status = Convert.ToInt32(dr["transaction_status"].ToString());
                        //pm.isDeleted = Convert.ToInt32(dr["isDeleted"].ToString());
                        pm.Chassis_No = Convert.ToString(dr["Chassis_No"].ToString());
                        pm.PriceOrignal = Parse(dr["PriceOrignal"].ToString());
                        pm.PriceRate = Parse(dr["PriceRate"].ToString());
                        pm.PriceTax = Parse(dr["PriceTax"].ToString());
                        pm.RecycleFee = Parse(dr["RecycleFee"].ToString());
                        //pm.RecycleFeeTax = Parse(dr["RecycleFeeTax"].ToString());
                        pm.AuctionFee = Parse(dr["AuctionFee"].ToString());
                        pm.AuctionFeeTax = Parse(dr["AuctionFeeTax"].ToString());
                        pm.PlatNumberFee = Parse(dr["PlatNumberFee"].ToString());
                        //pm.PlatNaumberTax = Parse(dr["PlatNaumberTax"].ToString());
                        pm.Total = Parse(dr["Total"].ToString());
                        pm.Make_ID = Convert.ToInt32(dr["Make_ID"].ToString());
                        pm.Color_ID = Convert.ToInt32(dr["Color_ID"].ToString());
                        pm.MakeModelDescription_ID = Convert.ToInt32(dr["MakeModelDescription_ID"].ToString());
                        pm.locID = Convert.ToInt32(dr["loc_ID"].ToString());
                        pm.temp_ID = Convert.ToString(dr["temp_ID"].ToString());
                        pm.LotNumber = dr["LotNumber"].ToString();
                        pm.Year = Convert.ToInt32(dr["Year"].ToString());
                        pm.GoingToLocation = Convert.ToInt32(dr["GoingToLocation"].ToString());
                        pm.Vendor_ID = Convert.ToInt32(dr["Vendor_ID"].ToString());
                        pm.ReksoFee = Parse(dr["ReksoFee"].ToString());
                        pm.ReksoFeeTax = Parse(dr["ReksoFeeTax"].ToString());
                        pm.StockType = Convert.ToInt32(dr["StockType"].ToString());
                        pm.transmission = dr["transmission"].ToString();
                        pm.Risko_Vendor_ID = Convert.ToInt32(dr["Risko_Vendor_ID"].ToString());
                        pm.color = dr["Color"].ToString();
                        pm.Year = Convert.ToInt32(dr["Year"].ToString());
                        pm.make = dr["Make"].ToString();




                        itemlist.Add(pm);

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
        public IEnumerable<StockDetails> Get_StockList_Complete_DAL(string ChassisNo, int Make_ID, int MakeModel_description_ID, int Color_ID, string PurchaseStartDate, string PurchaseEndDate,
             string BL_NO, string BOE, string PurchaseRef, string SaleRef, string Stock_Status, int loc_ID, int c_ID, string VendorName, string ModelYear, string Container_No, int StockType_ID)
        {

            string sp = "select_StockList_Complete";

            List<StockDetails> itemlist = new List<StockDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ChassisNo", ChassisNo);
                        cmd.Parameters.AddWithValue("@Make_ID", Make_ID);
                        cmd.Parameters.AddWithValue("@MakeModel_description_ID", MakeModel_description_ID);
                        cmd.Parameters.AddWithValue("@Color_ID", Color_ID);
                        cmd.Parameters.AddWithValue("@PurchaseStartDate", PurchaseStartDate);
                        cmd.Parameters.AddWithValue("@PurchaseEndDate", PurchaseEndDate);
                        cmd.Parameters.AddWithValue("@BL_NO", BL_NO);
                        cmd.Parameters.AddWithValue("@BOE", BOE);
                        cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                        cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                        cmd.Parameters.AddWithValue("@Stock_Status", Stock_Status);
                        cmd.Parameters.AddWithValue("@loc_ID", loc_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);
                        cmd.Parameters.AddWithValue("@VendorName", VendorName);
                        cmd.Parameters.AddWithValue("@ModelYear", ModelYear);
                        cmd.Parameters.AddWithValue("@Container_No", Container_No);
                        cmd.Parameters.AddWithValue("@StockType_ID", StockType_ID);


                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                //stock_ID, Chassis_No,  Make, ModelDesciption, Color, StockType,Transmission, CarLocation, Total_Cost,PurchaseRef,PurchaseDate, Stock_Status,Vendor_Name,ModelYear

                                StockDetails item = new StockDetails();
                                item.stock_ID = Convert.ToInt32(dr["Stock_ID"]);
                                item.Chassis_No = dr["Chassis_No"].ToString();
                                item.Vendor_Name = dr["Vendor_Name"].ToString();
                                item.Make = dr["Make"].ToString();
                                item.ModelDesciption = dr["ModelDesciption"].ToString();
                                item.ModelYear = dr["ModelYear"].ToString();
                                item.Color = dr["Color"].ToString();
                                item.StockTypeName = dr["StockTypeName"].ToString();
                                item.Transmission = dr["Transmission"].ToString();
                                item.CarLocation = dr["CarLocation"].ToString();
                                item.Total_Cost = dr["Total_Cost"].ToString();
                                item.PurchaseRef = dr["PurchaseRef"].ToString();
                                item.PurchaseDate = dr["PurchaseDate"].ToString();
                                item.Stock_Status = dr["Stock_Status"].ToString();
                                item.InteriorColor = dr["InteriorColor"].ToString();
                                item.ProuctionDate = dr["ProuctionDate"].ToString();
                                item.mileage = dr["mileage"].ToString();
                                item.PriceOrignal = dr["Price"].ToString();
                                item.AuctionName = dr["AuctionName"].ToString();
                                item.ContNO = dr["ContainerNo"].ToString();
                                item.LotNumber = dr["LotNumber"].ToString();
                                item.CountAttach = dr["CountAttach"].ToString();
                                item.Selling_Price = dr["Selling_Price"].ToString();
                                item.AD_Link = dr["AD_Link"].ToString();
                                item.PaperReceived = dr["PaperReceived"].ToString();
                                item.PriceTax = dr["PriceTax"].ToString();
                                item.Total_Expense = dr["Total_Expense"].ToString();
                                item.Shipping_Charges = dr["Shipping_Charges"].ToString();
                                item.Vanning_Charges = dr["Vanning_Charges"].ToString();
                                item.Inspection_Charges = dr["Inspection_Charges"].ToString();
                                item.AuctionFee = dr["AuctionFee"].ToString();
                                item.AuctionFeeTax = dr["AuctionFeeTax"].ToString();
                                item.AuctionFeeTax = dr["AuctionFeeTax"].ToString();
                                item.PlatNumberFee = dr["PlatNumberFee"].ToString();
                                item.ReksoFee = dr["ReksoFee"].ToString();
                                item.ReksoFeeTax = dr["ReksoFeeTax"].ToString();
                                item.RecycleFee = dr["RecycleFee"].ToString();
                                item.FromLoc = dr["FromLoc"].ToString();
                                item.GoingLoc = dr["GoingLoc"].ToString();
                                item.ReksoName = dr["ReksoName"].ToString();


                                itemlist.Add(item);

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


            return itemlist;
        }
        public string Update_Shipping_Alert_DAL(int shipinfoId)
        {






            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {

 
                               new SqlParameter("@shipinfoId",shipinfoId)

            };


            var response = dbLayer.SP_DataTable_return("pa_Update_Shipping_Alert", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;



        }
        public string Insert_Shaken_DAL(string stock_ID, string platNumber_fee, string refundAmount, string cancel_Date, string refund_date, string recieved_Date, string Account_Debit_ID,
     string Status, string AdjustedPurchase_ref,string modiefied_by,string created_by)
        {

            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@stock_ID",stock_ID),
                 new SqlParameter("@platNumber_fee",platNumber_fee),
                    new SqlParameter("@refundAmount",refundAmount),
                       new SqlParameter("@cancel_Date",cancel_Date),
                        new SqlParameter("@refund_date",refund_date),
                       new SqlParameter("@recieved_Date",recieved_Date),
                        new SqlParameter("@Account_Debit_ID",Account_Debit_ID),
                        new SqlParameter("@Status",Status),
                         new SqlParameter("@AdjustedPurchase_ref",AdjustedPurchase_ref),
                         new SqlParameter("@modiefied_by",modiefied_by),
                         new SqlParameter("@created_by",created_by)
                      
            };

            var response = dbLayer.SP_DataTable_return("pa_Update_platnumber", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }



        #region  Bill of Lading 

        public IPagedList<Pa_BL_Master> BLMasterPlist { get; set; }
        public IEnumerable<Pa_BL_Master> Get_BlList_DAL(string BLNO, string StartDate, string EndDate, int c_ID)
        {

            string sp = "Select_BLNO_List";
            List<Pa_BL_Master> itemlist = new List<Pa_BL_Master>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BLNO", BLNO);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@C_ID", c_ID);



                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Pa_BL_Master item = new Pa_BL_Master();

                        item.ID = Convert.ToInt32(rdr["ID"]);
                        item.BLNO_ID = Convert.ToInt32(rdr["BLNO_ID"]);
                        item.ShipName = rdr["ShipName"].ToString();
                        item.ShipLeavingDate = rdr["ShipLeavingDate"].ToString();
                        item.BLNO_Date = rdr["BLNO_Date"].ToString();
                        item.Arrival_Date = rdr["Arrival_Date"].ToString();                        
                        item.Total_BL_Charges = rdr["Total_BL_Charges"].ToString();

                        item.Clearing_Charges = rdr["Clearing_Charges"].ToString();
                        item.Custom_Duty = rdr["Custom_Duty"].ToString();
                        item.Ref = rdr["Ref"].ToString();
                        item.Remarks = rdr["Remarks"].ToString();
                        item.Currency_ID = Convert.ToInt32(rdr["Currency_ID"]);
                        item.Curr_Rate = rdr["Curr_Rate"].ToString();
                        item.Total_With_Rate = rdr["Total_With_Rate"].ToString();
                        item.Vendor_ID = Convert.ToInt32(rdr["Vendor_ID"].ToString());
                        item.BlNO = rdr["BlNO"].ToString();
                        item.Balance_AED = rdr["balance_AED"].ToString();
                        item.Paid_AED = rdr["Paid_AED"].ToString();

                        item.BLStatus = rdr["BL_Status"].ToString();
                        item.BLStatus_ID = rdr["BL_Status_ID"].ToString();


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

        public Pa_BL_Master Get_BLMasterByID_DAL(int? BL_ID)
        {
            string sp = "Select_BLNO_By_ID";

            Pa_BL_Master pm = new Pa_BL_Master();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BLNO_ID", BL_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.BLNO_ID = Convert.ToInt32(dr["BLNO_ID"]);

                                pm.Ref = dr["Ref"].ToString();
                                pm.BLNO_Date = dr["BLNO_Date"].ToString();
                                pm.ShipName = dr["ShipName"].ToString();
                                pm.ShipLeavingDate = dr["ShipLeavingDate"].ToString();
                                pm.Arrival_Date = dr["Arrival_Date"].ToString();
                                pm.Clearing_Charges = dr["Clearing_Charges"].ToString();
                                pm.Custom_Duty = dr["Custom_Duty"].ToString();

                                pm.Other_Charage = dr["Other_Charage"].ToString();
                                pm.Total_BL_Charges = dr["Total_BL_Charges"].ToString();

                                pm.Remarks = dr["Remarks"].ToString();
                                pm.Created_At = dr["Created_At"].ToString();
                                pm.Created_By = dr["Created_By"].ToString();
                                pm.Last_Updated_At = dr["Last_Updated_At"].ToString();
                                pm.Last_Updated_By = dr["Last_Updated_By"].ToString();
                                pm.Currency_ID = Convert.ToInt32(dr["Currency_ID"]);
                                pm.Curr_Rate = dr["Curr_Rate"].ToString();
                                pm.Total_With_Rate = dr["Total_With_Rate"].ToString();
                                pm.Vendor_ID = Convert.ToInt32(dr["Vendor_ID"].ToString());
                                pm.BlNO = dr["BlNO"].ToString();
                                pm.BLStatus_ID = dr["BL_Status_ID"].ToString();                                
                                pm.BLStatus = dr["BL_Status"].ToString();

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

        public IEnumerable<Pa_BL_Detsils> Get_BLDetailListBy_DAL(string Temp_ID, int? BL_ID)
        {


            string sp = "Select_BLNO_details_by_ID";
            List<Pa_BL_Detsils> itemlist = new List<Pa_BL_Detsils>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@BLNO_ID", BL_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_BL_Detsils item = new Pa_BL_Detsils();

                        item.ID = Convert.ToInt32(rdr["ID"]);

                        item.BLNO_Details_ID = Convert.ToInt32(rdr["BLNO_Details_ID"]);
                        item.BLNO_ID = Convert.ToInt32(rdr["BLNO_ID"]);                                              
                        item.Stock_ID = Convert.ToInt32(rdr["Stock_ID"]);
                        item.Chassis = rdr["Chassis_No"].ToString();
                        item.Make = rdr["Make"].ToString();
                        item.Model = rdr["ModelDesciption"].ToString();
                        item.Year = rdr["ModelYear"].ToString();

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

        public string InsertBLDetail_DAL(int? HdBL_ID, int? Stock_ID, string Temp_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {


                               new SqlParameter("@Temp_ID",Temp_ID),
                               new SqlParameter("@HdBLNo_ID",HdBL_ID),
                               new SqlParameter("@Stock_ID",Stock_ID),

            };


                var response = dbLayer.SP_DataTable_return("Insert_BLNO_Details", paramArray).Tables[0];
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

        public string InsertBLMaster_DAL(string ShipName, string ShipLeavingDate, string Arrival_Date, string Clearing_Charges, string Custom_Duty, string Other_Charage, string BLNO_Date, string Remarks, string Total_BL_Charges, string Temp_ID, int? c_ID, int? Created_by, int Currency_ID, string Currency_Rate, string Total_With_Rate, int? Vendor_ID, string BlNo)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {



                new SqlParameter("@ShipName",ShipName),
                new SqlParameter("@ShipLeavingDate",ShipLeavingDate),
                new SqlParameter("@Arrival_Date",Arrival_Date),
                                  
                    new SqlParameter("@Clearing_Charges",Clearing_Charges),
                new SqlParameter("@Custom_Duty",Custom_Duty),
                new SqlParameter("@Other_Charage",Other_Charage),
               
                    
                new SqlParameter("@BLNO_Date",BLNO_Date),
                new SqlParameter("@Total_BL_Charges",Total_BL_Charges),


                    new SqlParameter("@Remarks",Remarks),
                
                    
                 new SqlParameter("@UserID",Created_by),

                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Currency_ID",Currency_ID),


                 new SqlParameter("@Currency_Rate",Currency_Rate),
                 new SqlParameter("@Total_With_Rate",Total_With_Rate),
                  new SqlParameter("@Vendor_ID",Vendor_ID),
                 new SqlParameter("@BLNO",BlNo)


            };


                var response = dbLayer.SP_DataTable_return("Insert_BLNO_master", paramArray).Tables[0];
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

        public string UpdateBLMaster_DAL(string ShipName, string ShipLeavingDate, string Arrival_Date, string Clearing_Charges, string Custom_Duty, string Other_Charage, string BLNO_Date, string Remarks, string Total_BL_Charges, int? BL_ID, string Temp_ID, int? c_ID, int? Modified_by, int Currency_ID, string Currency_Rate,string Total_With_Rate, int? Vendor_ID, string BlNo)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {


                new SqlParameter("@ShipName",ShipName),
                new SqlParameter("@ShipLeavingDate",ShipLeavingDate),
                new SqlParameter("@Arrival_Date",Arrival_Date),
                new SqlParameter("@Clearing_Charges",Clearing_Charges),
                 new SqlParameter("@Custom_Duty",Custom_Duty),
                 new SqlParameter("@Other_Charage",Other_Charage),

                 new SqlParameter("@Total_BL_Charges",Total_BL_Charges),


                new SqlParameter("@BLNo_ID",BL_ID),
                new SqlParameter("@BLNO_Date",BLNO_Date),
                
                new SqlParameter("@Remarks",Remarks),
                 new SqlParameter("@Temp_ID",Temp_ID),
                 new SqlParameter("@c_ID",c_ID),
                 new SqlParameter("@UserID",Modified_by),

                 new SqlParameter("@Currency_ID",Currency_ID),
                 new SqlParameter("@Currency_Rate",Currency_Rate),
                 new SqlParameter("@Total_With_Rate",Total_With_Rate),
                 new SqlParameter("@Vendor_ID",Vendor_ID),
                 new SqlParameter("@BLNO",BlNo)


            };


                var response = dbLayer.SP_DataTable_return("Update_BLNO_master", paramArray).Tables[0];
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

        public string DeleteBLDetail_DAL(int? BLDetail_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@BLDetail_ID",BLDetail_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_BLNO_Details", paramArray).Tables[0];
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

        public string GetOldTempIDFromBLDetail_DAL(int? BL_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@BLNO_ID",BL_ID)

            };


                var response = dbLayer.SP_DataTable_return("SelectBLNODetailOldTempID", paramArray).Tables[0];
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



        #region WorkShop

        
        public IEnumerable<Pa_Garage_Master> Get_GarageList_DAL()
        {

            string sp = "Select_Garage_List";
            List<Pa_Garage_Master> itemlist = new List<Pa_Garage_Master>();

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
                        Pa_Garage_Master item = new Pa_Garage_Master();

                        item.ID = Convert.ToInt32(rdr["ID"]);
                        item.GarageMaster_ID = Convert.ToInt32(rdr["GarageMaster_ID"]);
                        item.GarageVendor_ID = Convert.ToInt32(rdr["GarageVendor_ID"].ToString());
                        item.GarageVendor_Name = rdr["Vendor_Name"].ToString();

                        item.Date_Send = rdr["Date_Send"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();                        
                        item.RefInv = rdr["RefInv"].ToString();
                        item.Remarks = rdr["Remarks"].ToString();
                        item.Paid = rdr["Paid"].ToString();
                        item.Balance = rdr["Balance"].ToString();

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

        public IEnumerable<Pa_Garage_Details> Get_GarageDetailListBy_DAL(string Temp_ID, int? Garage_Master_ID)
        {

            string sp = "Select_Garage_details_by_ID";
            List<Pa_Garage_Details> itemlist = new List<Pa_Garage_Details>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@Garage_Master_ID", Garage_Master_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Garage_Details item = new Pa_Garage_Details();

                        item.ID = Convert.ToInt32(rdr["ID"]);

                        item.Garage_Details_ID = Convert.ToInt32(rdr["GarageDetails_ID"]);
                        item.Garage_Master_ID = Convert.ToInt32(rdr["GarageMaster_ID"]);
                        item.Chassis = (rdr["Chassis_No"].ToString());
                        item.Stock_ID = Convert.ToInt32(rdr["Stock_ID"]);                        
                        item.Description = rdr["Description"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                       
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

        public string InsertGarageDetail_DAL(int? HdGarage_Master_ID, int? Stock_ID, string Temp_ID,string Description, string Amount)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {


                               new SqlParameter("@Temp_ID",Temp_ID),
                               new SqlParameter("@HdGarage_Master_ID",HdGarage_Master_ID),
                               new SqlParameter("@Stock_ID",Stock_ID),
                               new SqlParameter("@Description",Description),
                               new SqlParameter("@Amount",Amount)

            };


                var response = dbLayer.SP_DataTable_return("Insert_Garage_Details", paramArray).Tables[0];
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

        public string InsertGarageMaster_DAL(string Date_Send, string Remarks,int? GarageVendor_ID, string Temp_ID, int? c_ID, int? Created_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {



                new SqlParameter("@Date_Send",Date_Send),
                new SqlParameter("@Remarks",Remarks),
              
                new SqlParameter("@GarageVendor_ID",GarageVendor_ID),                               
                new SqlParameter("@UserID",Created_by),
                new SqlParameter("@Temp_ID",Temp_ID),
                new SqlParameter("@c_ID",c_ID)







            };


                var response = dbLayer.SP_DataTable_return("Insert_Garage_master", paramArray).Tables[0];
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

        public string UpdateGarageMaster_DAL(string Date_Send, string Remarks,int? GarageVendor_ID, int Garage_Master_ID, string Temp_ID, int? c_ID, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {



                new SqlParameter("@Date_Send",Date_Send),
            
                new SqlParameter("@GarageVendor_ID",GarageVendor_ID),
                new SqlParameter("@Remarks",Remarks),
                new SqlParameter("@UserID",Modified_by),
                new SqlParameter("@Temp_ID",Temp_ID),
                new SqlParameter("@c_ID",c_ID),
                new SqlParameter("@Garage_Master_ID",Garage_Master_ID)







            };


                var response = dbLayer.SP_DataTable_return("Update_Garage_master", paramArray).Tables[0];
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

        public string DeleteGarageDetail_DAL(int? Garage_Detail_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Garage_Detail_ID",Garage_Detail_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_Garage_Details", paramArray).Tables[0];
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

        public string GetOldTempIDFromGarageDetail_DAL(int? Garage_Master_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@GarageMaster_ID",Garage_Master_ID)

            };


                var response = dbLayer.SP_DataTable_return("SelectGarageDetailOldTempID", paramArray).Tables[0];
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

        public Pa_Garage_Master Get_GarageMasterByID_DAL(int? Garage_Master_ID)
        {
            string sp = "Select_Garage_By_ID";

            Pa_Garage_Master pm = new Pa_Garage_Master();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Garage_Master_ID", Garage_Master_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.GarageMaster_ID = Convert.ToInt32(dr["GarageMaster_ID"]);
                                pm.RefInv = dr["RefInv"].ToString();
                                pm.Date_Send = dr["Date_Send"].ToString();
                                pm.GarageVendor_ID = Convert.ToInt32(dr["GarageVendor_ID"].ToString());
                                pm.GarageVendor_Name = dr["Vendor_Name"].ToString();
                                pm.Total_Amount = dr["Total_Amount"].ToString();
                                pm.Remarks = dr["Remarks"].ToString();
                                pm.Created_At = dr["Created_At"].ToString();
                                pm.Created_By = dr["Created_By"].ToString();
                                pm.Last_Updated_At = dr["Last_Updated_At"].ToString();
                                pm.Last_Updated_By = dr["Last_Updated_By"].ToString();

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

        public string ChangeBlnoStatus_DAL(int? Blno_ID, int? Status_ID, int? c_ID, int? Modified_by)
        {
            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Blno_ID",Blno_ID),
                  new SqlParameter("@Status_ID",Status_ID),
                   new SqlParameter("@c_ID",c_ID),
                   
                   new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_BlnoStatus", paramArray).Tables[0];
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

    }
}
