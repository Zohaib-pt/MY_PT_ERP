using DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using X.PagedList;

namespace DAL.oClasses
{
    public class oReports : IOReports
    {
        private readonly IConnectionStringManager _conMgr;
        private readonly ISqlHelper dbLayer;
        SqlConnection con;

        public oReports(IConnectionStringManager conMgr, ISqlHelper sqlHelp)
        {

            _conMgr = conMgr;
            dbLayer = sqlHelp;
            //---Getting Connection String
            Constr = _conMgr.GetConnectionString();
        }
        public string Constr { get; set; }

        public IEnumerable<pa_tblLedgers> Ledger { get; set; }
        public IPagedList<pa_tblLedgers> LedgerPagedList { get; set; }
        public IEnumerable<pa_tblLedgers> Ledger_TTL { get; set; }
        public IEnumerable<pa_tblLedgers> payableSummary { get; set; }
        public IEnumerable<pa_tblLedgers> ReceiveAbleSummary { get; set; }

        public IPagedList<pa_tblLedgers> CustomerLedger { get; set; }
        public pa_tblLedgers CustomerLedgerMasterObj { get; set; }
        public IEnumerable<pa_tblLedgers> CustomerLedger_TTL { get; set; }
        public IPagedList<SaleReport> SaleReport { get; set; }
        public SaleReport SaleReportTTL { get; set; }
        public IPagedList<StockReport> StockCostList { get; set; }
        public IEnumerable<StockReport> StockCostLists { get; set; }
        public IEnumerable<StockReport> StockCostList_ttl { get; set; }

        public IEnumerable<StockReport> StockReportList { get; set; }

        public Pa_PurchaseMaster_DAL PurchaseMasterPrint { get; set; }
        public IEnumerable<Pa_PurchaseDetails_DAL> PurchaseDetailPrint { get; set; }
        public IEnumerable<Pa_PurchaseDetails_DAL> PurchaseStockSummaryPrint { get; set; }
        public IEnumerable<SaleReport> SaleReportPrint { get; set; }

        public SaleReport SaleReportTTLPrint { get; set; }


        public IEnumerable<pa_tblLedgers> Ledger_Print { get; set; }
        public pa_tblLedgers Ledger_TTL_Print { get; set; }
        public IEnumerable<pa_tblLedgers> CustomerLedger_Print { get; set; }
        public pa_tblLedgers CustomerLedger_TTL_Print { get; set; }
        public IEnumerable<pa_PaymentMaster_DAL> PaymentVoucher_Print { get; set; }
        public IEnumerable<pa_ReceiptMaster_DAL> ReceiptVoucher_Print { get; set; }
        public IEnumerable<Deposits_DAL> DepositVoucherCheque_Print { get; set; }
        public IEnumerable<Deposits_DAL> DepositVoucher_Print { get; set; }

        public IEnumerable<SaleDetailJP> SalesListJP_Print { get; set; }
        public pa_select_saledetails_Total_Result SalesGrandTotals_Print { get; set; }
        public pa_SalesMaster_DAL SalesMasterObjJP_Print { get; set; }
        public Pa_GoodsReceived_Master PurchaseMasterPrint_GRV { get; set; }
        public IEnumerable<Pa_GoodsReceived_Dtl> PurchaseDetailPrint_GRV { get; set; }
        public IPagedList<TaxReport> PaidTax { get; set; }
        public IEnumerable<TaxReport> PaidTax_TTL { get; set; }
        public IPagedList<TaxReport> ReceivedTax { get; set; }
        public IEnumerable<TaxReport> ReceivedTax_TTL { get; set; }
        public IPagedList<RecycleReport> Recycle { get; set; }
        public IEnumerable<RecycleReport> Recycle_TTL { get; set; }
        //Tax Report Get_PaidTax_Report_jp_PR Nafeel Work

   





        //Get_Recycle_Report_jp_PR Nafeel Work



        public IEnumerable<RecycleReport> Recycle_PR { get; set; }
        public IEnumerable<RecycleReport> Recycle_TTL_PR { get; set; }
        public IEnumerable<pa_tblLedgers> pa_Select_Payable_Summary_DAL(int c_ID)
        {
            List<pa_tblLedgers> RD = new List<pa_tblLedgers>();
            string sp = "pa_Select_Payable_Summary";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_tblLedgers o = new pa_tblLedgers();

                                o.Party_ID = dr["Party_ID"].ToString();
                                o.PartyName = dr["PartyName"].ToString();

                                o.Debit = dr["Debit"].ToString();
                                o.Credit = dr["Credit"].ToString();
                                o.Balance = dr["Balance"].ToString();

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


        public IEnumerable<pa_tblLedgers> pa_Select_CustomerLedger_DAL(string StartDate, string EndDate, string Customer_ID, int c_ID)
        {
            List<pa_tblLedgers> RD = new List<pa_tblLedgers>();
            string sp = "pa_Select_CustomerLedger";
            string itemdesc = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Customer_ID", Customer_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_tblLedgers o = new pa_tblLedgers();

                                o.customer_ID = dr["customer_ID"].ToString();
                                o.Date = dr["Date"].ToString();
                                o.trans_ref = dr["trans_ref"].ToString();
                                o.Debit = dr["Debit"].ToString();
                                o.Credit = dr["Credit"].ToString();


                                o.Description = dr["Description"].ToString();
                                o.trans_created_at = dr["trans_created_at"].ToString();
                                o.Chassis_No = dr["Chassis_No"].ToString();
                                o.ACCOUNT = dr["ACCOUNT"].ToString();
                                o.Alt_ACCOUNT = dr["Alt_ACCOUNT"].ToString();
                                o.PartyName = dr["PartyName"].ToString();
                                o.Link = dr["Link"].ToString();
                               // itemdesc = dr["ItemDesc"].ToString();
                                //if (o.Description == "" && itemdesc != null)
                                //{
                                //    o.Description = itemdesc;
                                //}
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
        public IEnumerable<pa_tblLedgers> pa_Select_CustomerLedger_DAL_TTL(string StartDate, string EndDate, string Customer_ID, int c_ID)
        {
            List<pa_tblLedgers> RD = new List<pa_tblLedgers>();
            string sp = "pa_Select_CustomerLedger_TTL";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Customer_ID", Customer_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_tblLedgers o = new pa_tblLedgers();

                                o.TotalDebit = dr["TotalDebit"].ToString();
                                o.TotalCredit = dr["TotalCredit"].ToString();
                                o.TotalBalance = dr["TotalBalance"].ToString();
                                o.Opening_Balance = dr["Opening_Balance"].ToString();
                                o.Closing_Balance = dr["Closing_Balance"].ToString();


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


        #region Vendor Reports

        public IEnumerable<StockReport> VendorReport_byChassis { get; set; }
        public IEnumerable<StockReport> VendorReport_byChassis_TTL { get; set; }
        public IEnumerable<pa_tblLedgers> pa_Select_VendorLedger_DAL(string StartDate, string EndDate, string Vendor_ID, int? c_ID)
        {
            List<pa_tblLedgers> RD = new List<pa_tblLedgers>();
            string sp = "pa_Select_VendorLedger";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_tblLedgers o = new pa_tblLedgers();

                                o.vendor_ID = dr["vendor_ID"].ToString();
                                o.Date = dr["Date"].ToString();
                                o.trans_ref = dr["trans_ref"].ToString();
                                o.Debit = dr["Debit"].ToString();
                                o.Credit = dr["Credit"].ToString();
                                o.Description = dr["Description"].ToString();
                                o.trans_created_at = dr["trans_created_at"].ToString();
                                o.Chassis_No = dr["Chassis_No"].ToString();
                                o.ACCOUNT = dr["ACCOUNT"].ToString();
                                o.Alt_ACCOUNT = dr["Alt_ACCOUNT"].ToString();
                                o.PartyName = dr["PartyName"].ToString();
                                o.Link = dr["Link"].ToString();

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


        public IEnumerable<pa_tblLedgers> pa_Select_VendorLedger_DAL_TTL(string StartDate, string EndDate, string Vendor_ID, int? c_ID)
        {
            List<pa_tblLedgers> RD = new List<pa_tblLedgers>();
            string sp = "pa_Select_VendorLedger_TTL";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_tblLedgers o = new pa_tblLedgers();

                                o.TotalDebit = dr["TotalDebit"].ToString();
                                o.TotalCredit = dr["TotalCredit"].ToString();
                                o.TotalBalance = dr["TotalBalance"].ToString();
                                o.Opening_Balance = dr["Opening_Balance"].ToString();
                                o.Closing_Balance = dr["Closing_Balance"].ToString();

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



        public IEnumerable<StockReport> select_VendorReport_Chassis_Wise_DAL(string StartDate, string EndDate, int Vendor_ID, int? c_ID)
        {
            List<StockReport> RD = new List<StockReport>();
            string sp = "select_VendorReport_Chassis_Wise";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                StockReport o = new StockReport();


                                o.Chassis_No = dr["Chassis_No"].ToString();
                                o.Vendor_Name = dr["Vendor_Name"].ToString();
                                o.PurchaseRef = dr["PurchaseRef"].ToString();
                                o.PurchaseDate = dr["PurchaseDate"].ToString();
                                o.Paid = dr["Paid"].ToString();
                                o.Balance = dr["Balance"].ToString();
                                o.TotalPrice = dr["TotalPrice"].ToString();



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

        public IEnumerable<StockReport> select_VendorReport_Chassis_Wise_TTL_DAL(string StartDate, string EndDate, int Vendor_ID, int? c_ID)
        {
            List<StockReport> RD = new List<StockReport>();
            string sp = "select_VendorReport_Chassis_Wise_TTL";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                StockReport o = new StockReport();


                                o.Paid = dr["Paid"].ToString();
                                o.Balance = dr["Balance"].ToString();
                                o.TotalPrice = dr["TotalPrice"].ToString();



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


        #endregion Vendor Reports



        public IEnumerable<pa_tblLedgers> pa_Select_Recievable_Summary_DAL(int c_ID)
        {
            List<pa_tblLedgers> RD = new List<pa_tblLedgers>();
            string sp = "pa_Select_Recievable_Summary";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_tblLedgers o = new pa_tblLedgers();

                                o.Party_ID = dr["Party_ID"].ToString();
                                o.PartyName = dr["PartyName"].ToString();

                                o.Debit = dr["Debit"].ToString();
                                o.Credit = dr["Credit"].ToString();
                                o.Balance = dr["Balance"].ToString();

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


        public IEnumerable<SaleReport> pa_Select_SalesReport_DAL(string StartDate, string EndDate, int? Customer_ID, string Sale_Ref, int c_ID, int Vendor_ID, string cartype, string chassis, string Pur_Ref, int? Make, int? Model_Desc, string Model_Year)
        {
            
            List<SaleReport> RD = new List<SaleReport>();
            string sp = "Select_SalesReport";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@CustomerID", Customer_ID);
                        cmd.Parameters.AddWithValue("@SaleRef", Sale_Ref);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);
                        cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                        cmd.Parameters.AddWithValue("@cartype", cartype);
                        cmd.Parameters.AddWithValue("@chassis", chassis);
                        cmd.Parameters.AddWithValue("@PurRef", Pur_Ref);
                        cmd.Parameters.AddWithValue("@Make", Make);
                        cmd.Parameters.AddWithValue("@Model", Model_Desc);
                        cmd.Parameters.AddWithValue("@Model_Year", Model_Year);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                SaleReport o = new SaleReport();


                                o.Date = dr["Date"].ToString();
                                o.SaleRef = dr["Sale_Ref"].ToString();
                                o.Chassis_no = dr["Chassis_no"].ToString();
                                o.make = dr["make"].ToString();
                                o.model_description = dr["model_description"].ToString();
                                o.color = dr["Color"].ToString();
                                o.Sale_Value = dr["Sale_Value"].ToString();
                                o.Total_Expense = dr["Total_Expense"].ToString();
                                o.Total_Cost = dr["Total_Cost"].ToString();
                                o.ProfitLost = dr["ProfitLost"].ToString();
                                o.Customer_Name = dr["Customer_Name"].ToString();
                                o.SaleMaster_ID = Convert.ToInt32(dr["SaleMaster_ID"].ToString());
                                o.Customer_ID = Convert.ToInt32(dr["Customer_ID"].ToString());
                                o.Stock_id = Convert.ToInt32(dr["stock_ID"].ToString());
                             





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

        public SaleReport pa_Select_SalesReport_TTL_DAL(string StartDate, string EndDate, int? Customer_ID, string Sale_Ref, int c_ID, int Vendor_ID, string cartype, string chassis, string Pur_Ref, int? Make, int? Model_Desc, string Model_Year)
        {
            SaleReport o = new SaleReport();
            string sp = "Select_SalesReport_ttl";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@CustomerID", Customer_ID);
                        cmd.Parameters.AddWithValue("@SaleRef", Sale_Ref);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);
                        cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                        cmd.Parameters.AddWithValue("@cartype", cartype);
                        cmd.Parameters.AddWithValue("@chassis", chassis);
                        cmd.Parameters.AddWithValue("@PurRef", Pur_Ref);
                        cmd.Parameters.AddWithValue("@Make", Make);
                        cmd.Parameters.AddWithValue("@Model", Model_Desc);
                        cmd.Parameters.AddWithValue("@Model_Year", Model_Year);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {



                                o.Total_Expense = dr["Total_Expense"].ToString();
                                o.Total_Cost = dr["Total_Cost"].ToString();
                                o.ProfitLost = dr["ProfitLost"].ToString();
                                o.Sale_Value = dr["Sale_Value"].ToString();



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


            return o;


        }
        public IEnumerable<StockReport> pa_Select_StockReport_DAL(string StartPurchaseDate, string EndPurchaseDate, string Chassis_No, string PurchaseRef, int? Make_ID,
            int? MakeModel_description_ID, int? Color_ID, int c_ID)
        {
            List<StockReport> RD = new List<StockReport>();
            string sp = "select_StockReport";



            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PurchaseStartDate", StartPurchaseDate);
                        cmd.Parameters.AddWithValue("@PurchaseEndDate", EndPurchaseDate);

                        cmd.Parameters.AddWithValue("@ChassisNo", Chassis_No);
                        cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                        cmd.Parameters.AddWithValue("@Make_ID", Make_ID);
                        cmd.Parameters.AddWithValue("@MakeModel_description_ID", MakeModel_description_ID);
                        cmd.Parameters.AddWithValue("@Color_ID", Color_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                StockReport o = new StockReport();


                                o.Stock_ID = dr["stock_ID"].ToString();
                                o.Chassis_No = dr["Chassis_No"].ToString();
                                o.PurchaseDate = dr["PurchaseDate"].ToString();
                                o.PurchaseRef = dr["PurchaseRef"].ToString();

                                o.Total_Cost = dr["Total_Cost"].ToString();
                                o.ModelYear = dr["ModelYear"].ToString();
                                o.Vendor_Name = dr["Vendor_Name"].ToString();
                                o.Stock_Status = dr["Stock_Status"].ToString();
                                o.CarLocation = dr["CarLocation"].ToString();
                                o.Transmission = dr["Transmission"].ToString();
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



        public IEnumerable<Pa_PurchaseDetails_DAL> GetDataForStockSummaryPrintDetail(int? PurchaseMaster_ID)
        {

            string sp = "Pa_Select_StockSummary_PrintById";

            List<Pa_PurchaseDetails_DAL> ABC = new List<Pa_PurchaseDetails_DAL>();
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
                                Pa_PurchaseDetails_DAL pm = new Pa_PurchaseDetails_DAL();
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
                                pm.ColorName = dr["ColorName"].ToString();
                                pm.Make = dr["Make"].ToString();
                                pm.Engine_No = dr["Engine_No"].ToString();
                                pm.PurchaseRef = dr["PurchaseRef"].ToString();

                                ABC.Add(pm);
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


            return ABC;
        }
        public IEnumerable<Pa_PurchaseDetails_DAL> GetDataForPurchasePrintDetail(int? PurchaseMaster_ID)
        {

            string sp = "Pa_Select_PurchaseDetailByID_PrintById";

            List<Pa_PurchaseDetails_DAL> ABC = new List<Pa_PurchaseDetails_DAL>();
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
                                Pa_PurchaseDetails_DAL pm = new Pa_PurchaseDetails_DAL();
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
                                pm.ColorName = dr["ColorName"].ToString();
                                pm.Make = dr["Make"].ToString();

                                ABC.Add(pm);
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


            return ABC;
        }
        public IEnumerable<Pa_PurchaseMaster_DAL> GetDataForPurchasePrintMaster(int? PurchaseMaster_ID)
        {

            string sp = "Pa_Select_Purchase_Master_PrintById";

            List<Pa_PurchaseMaster_DAL> MAS = new List<Pa_PurchaseMaster_DAL>();
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
                                Pa_PurchaseMaster_DAL pm = new Pa_PurchaseMaster_DAL();
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
                                pm.Total = dr["Total"].ToString();
                                pm.Created_at = dr["Created_at"].ToString();
                                pm.Created_by = dr["Created_by"].ToString();
                                pm.Modified_at = dr["Modified_at"].ToString();
                                pm.Modified_by = dr["Modified_by"].ToString();
                                pm.Approve_by = dr["Approve_by"].ToString();
                                pm.Approved_at = dr["Approved_at"].ToString();
                                pm.Vendor_Name = dr["Vendor_Name"].ToString();
                                pm.cheque_no = dr["cheque_no"].ToString();
                                pm.cheque_date = dr["cheque_date"].ToString();
                                pm.cheque_bank_name = dr["cheque_bank_name"].ToString();
                                MAS.Add(pm);
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


            return MAS;
        }
        public SaleReport pa_Select_SalesReport_TTL_DAL_Print(string StartDate, string EndDate, int? Customer_ID, string Sale_Ref, int c_ID, string Pur_Ref, int? Make, int? Model_Desc, string Model_Year)
        {
            SaleReport o = new SaleReport();
            string sp = "Select_SalesReport_ttl";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@CustomerID", Customer_ID);
                        cmd.Parameters.AddWithValue("@SaleRef", Sale_Ref);
                        cmd.Parameters.AddWithValue("@PurRef", Pur_Ref);
                        cmd.Parameters.AddWithValue("@Make", Make);
                        cmd.Parameters.AddWithValue("@Model", Model_Desc);
                        cmd.Parameters.AddWithValue("@Model_Year", Model_Year);


                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {



                                o.Total_Expense = dr["Total_Expense"].ToString();
                                o.Total_Cost = dr["Total_Cost"].ToString();
                                o.ProfitLost = dr["ProfitLost"].ToString();
                                o.Sale_Value = dr["Sale_Value"].ToString();



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


            return o;


        }
        public pa_tblLedgers pa_Select_VendorLedger_DAL_TTL_Print(string StartDate, string EndDate, string Vendor_ID, int? c_ID)
        {
            pa_tblLedgers o = new pa_tblLedgers();
            string sp = "pa_Select_VendorLedger_TTL";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {


                                o.TotalDebit = dr["TotalDebit"].ToString();
                                o.TotalCredit = dr["TotalCredit"].ToString();
                                o.TotalBalance = dr["TotalBalance"].ToString();
                                o.Opening_Balance = dr["Opening_Balance"].ToString();
                                o.Closing_Balance = dr["Closing_Balance"].ToString();





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


            return o;


        }

        public pa_tblLedgers pa_Select_CustomerLedger_DAL_TTL1(string StartDate, string EndDate, string Customer_ID, int c_ID)
        {
            string sp = "pa_Select_CustomerLedger_TTL";
            pa_tblLedgers ptb = new pa_tblLedgers();



            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(sp, con);



                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Customer_ID", Customer_ID);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    SqlDataReader dr = cmd.ExecuteReader();




                    while (dr.Read())
                    {




                        ptb.TotalDebit = dr["TotalDebit"].ToString();
                        ptb.TotalCredit = dr["TotalCredit"].ToString();
                        ptb.TotalBalance = dr["TotalBalance"].ToString();
                        ptb.Opening_Balance = dr["Opening_Balance"].ToString();
                        ptb.Closing_Balance = dr["Closing_Balance"].ToString();





                    }
                    con.Close();
                }
                return ptb;
            }




            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }
        }

        public IEnumerable<pa_PaymentMaster_DAL> GetDataForPaymentMaster_Print(int? PaymentMaster_ID)
        {

            string sp = "Select_PaymentMaster_Print_ByID";

            List<pa_PaymentMaster_DAL> MAS = new List<pa_PaymentMaster_DAL>();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PaymentMaster_ID", PaymentMaster_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_PaymentMaster_DAL pm = new pa_PaymentMaster_DAL();
                                pm.PaymentMaster_ID = Convert.ToInt32(dr["PaymentMaster_ID"]);
                                pm.PaymentMaster_ref = dr["PaymentMaster_ref"].ToString();
                                pm.Date = dr["Date"].ToString();
                                pm.PartyName = dr["PartyName"].ToString();
                                pm.ContactNumber = dr["ContactNumber"].ToString();
                                pm.DR_accountName = dr["DR_accountName"].ToString();
                                pm.Description = dr["Description"].ToString();
                                pm.cheque_no = dr["cheque_no"].ToString();
                                pm.cheque_date = dr["cheque_date"].ToString();
                                pm.Amount = dr["Amount"].ToString();
                                pm.Chassis_No = dr["Chassis_No"].ToString();
                                pm.Currency_ShortName = dr["Currency_ShortName"].ToString();
                                pm.Minor_ShortName = dr["Minor_ShortName"].ToString();
                                pm.Total_Amount = dr["Total_Amount"].ToString();
                                pm.create_by = dr["create_by"].ToString();
                                pm.approved_at = dr["Approved_byName"].ToString();
                                pm.Total_Amt_inWords = dr["Total_Amt_inWords"].ToString();

                                MAS.Add(pm);
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


            return MAS;
        }


        public IEnumerable<pa_ReceiptMaster_DAL> GetDataForReceiptMaster_Print(int? ReceiptMaster_ID)
        {

            string sp = "Select_ReceiptMaster_Print_ByID";

            List<pa_ReceiptMaster_DAL> MAS = new List<pa_ReceiptMaster_DAL>();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ReceiptMaster_ID", ReceiptMaster_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_ReceiptMaster_DAL pm = new pa_ReceiptMaster_DAL();
                                pm.ReceiptMaster_ID = Convert.ToInt32(dr["ReceiptMaster_ID"]);
                                pm.ReceiptMaster_ref = dr["ReceiptMaster_ref"].ToString();
                                pm.ReceiptDate = dr["ReceiptDate"].ToString();
                                pm.PartyName = dr["PartyName"].ToString();
                                pm.Description = dr["Description"].ToString();
                                pm.Cheque_no = dr["Cheque_no"].ToString();
                                pm.cheque_Date = dr["cheque_Date"].ToString();
                                pm.Cheque_Bank_Name = dr["Cheque_Bank_Name"].ToString();
                                pm.Currency_ShortName = dr["Currency_ShortName"].ToString();
                                pm.Minor_ShortName = dr["Minor_ShortName"].ToString();
                                pm.Amount = dr["Amount"].ToString();
                                pm.Total_Amount = dr["Total_Amount"].ToString();
                                pm.Created_by = dr["create_by"].ToString();
                                pm.Total_Amt_inWords = dr["Total_Amt_inWords"].ToString();
                                pm.imgname = dr["imgname"].ToString();
                                pm.QRName = dr["QRName"].ToString();
                                pm.Sale_ID = Convert.ToInt32(dr["Sales_ID"].ToString());
                        
                              

                              


                                MAS.Add(pm);
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


            return MAS;
        }

        public Pa_PurchaseMaster_DAL GetDataForPurchaseMaster(int? PurchaseMaster_ID)
        {

            string sp = "Pa_Select_Purchase_Master_PrintById";

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
                                pm.Total = dr["Total"].ToString();
                                pm.Created_at = dr["Created_at"].ToString();
                                pm.Created_by = dr["Created_by"].ToString();
                                pm.Modified_at = dr["Modified_at"].ToString();
                                pm.Modified_by = dr["Modified_by"].ToString();
                                pm.Approve_by = dr["Approve_by"].ToString();
                                pm.Approved_at = dr["Approved_at"].ToString();
                                pm.Vendor_Name = dr["Vendor_Name"].ToString();
                                pm.cheque_no = dr["cheque_no"].ToString();
                                pm.cheque_date = dr["cheque_date"].ToString();
                                pm.cheque_bank_name = dr["cheque_bank_name"].ToString();

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

        //New Faraz Work

        public IEnumerable<pa_Expense> ExpenseReport { get; set; }


        public pa_Expense ExpenseReport_TTL { get; set; }


        //Direct
        public IPagedList<pa_Expense> ExpenseDirectReport { get; set; }
        public IEnumerable<pa_Expense> ExpenseReport_Direct { get; set; }
        public IPagedList<pa_Expense> ExpenseLedger { get; set; }

        public pa_Expense ExpenseReport_TTL_Direct { get; set; }
        public pa_Shipping_Info ShippingMasterObjJP_Print { get; set; }
        public IEnumerable<pa_Shipping_Info_details> ShippingListJP_Print { get; set; }
        public pa_Vanning_Master VanningMasterObjJP_Print { get; set; }
        public IEnumerable<pa_Vanning_Details> VanningListJP_Print { get; set; }
        //

        //New Faraz Work
        public IEnumerable<pa_Expense> pa_Select_Expense__InDirect_DAL(string StartDate, string EndDate, int? DR_accountID, string Chassis_No, int? c_ID)
        {
            List<pa_Expense> RD = new List<pa_Expense>();
            string sp = "select_ExpenseReport_InDirect";



            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@DR_accountID", DR_accountID);
                        cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);


                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_Expense o = new pa_Expense();

                                o.ExpenseAccount = dr["ExpenseAccount"].ToString();
                                o.PayVia = dr["PayVia"].ToString();
                                o.stock_ID = Convert.ToInt32(dr["stock_ID"]);

                                o.Chassis_No = dr["Chassis_No"].ToString();
                                o.PaymentDate = dr["PaymentDate"].ToString();
                                o.Currency_Rate = dr["Currency_Rate"].ToString();
                                o.Amount = dr["Amount"].ToString();
                                o.VAT_Exp = dr["VAT_Exp"].ToString();
                                o.Total_Amount = dr["Total_Amount"].ToString();
                                o.Description = dr["Description"].ToString();

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
        //ttl

        public pa_Expense pa_Select_Expense__InDirect_TTL_DAL(string StartDate, string EndDate, int? DR_accountID, string Chassis_No, int? c_ID)
        {

            string sp = "select_ExpenseReport_InDirect_TTL";
            pa_Expense o = new pa_Expense();



            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(sp, con);



                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@DR_accountID", DR_accountID);
                    cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    SqlDataReader dr = cmd.ExecuteReader();


                    while (dr.Read())
                    {


                        o.Amount = dr["Amount"].ToString();
                        o.VAT_Exp = dr["VAT_Exp"].ToString();
                        o.Total_Amount = dr["Total_Amount"].ToString();



                    }
                    con.Close();
                }
                return o;
            }




            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }

        }

        //New Work

        public IEnumerable<pa_Expense> pa_Select_Expense__Direct_DAL(string StartDate, string EndDate, int? DR_accountID, string Chassis_No, int? c_ID)
        {
            List<pa_Expense> RD = new List<pa_Expense>();
            string sp = "select_ExpenseReport_Direct";



            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@DR_accountID", DR_accountID);
                        cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);


                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_Expense o = new pa_Expense();

                                o.ExpenseAccount = dr["ExpenseAccount"].ToString();

                                o.PayVia = dr["PayVia"].ToString();
                                o.stock_ID = Convert.ToInt32(dr["stock_ID"]);

                                o.Chassis_No = dr["Chassis_No"].ToString();
                                o.PaymentDate = dr["PaymentDate"].ToString();
                                o.Currency_Rate = dr["Currency_Rate"].ToString();
                                o.Amount = dr["Amount"].ToString();
                                o.VAT_Exp = dr["VAT_Exp"].ToString();
                                o.Total_Amount = dr["Total_Amount"].ToString();

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
        //ttl

        public pa_Expense pa_Select_Expense__Direct_TTL_DAL(string StartDate, string EndDate, int? DR_accountID, string Chassis_No, int? c_ID)
        {

            string sp = "select_ExpenseReport_Direct_TTL";
            pa_Expense o = new pa_Expense();



            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(sp, con);



                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@DR_accountID", DR_accountID);
                    cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    SqlDataReader dr = cmd.ExecuteReader();


                    while (dr.Read())
                    {


                        o.Amount = dr["Amount"].ToString();
                        o.VAT_Exp = dr["VAT_Exp"].ToString();
                        o.Total_Amount = dr["Total_Amount"].ToString();



                    }
                    con.Close();
                }
                return o;
            }




            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }

        }



        //By Jahangir 17 Jan 2021 10:06 am
        #region ITEM CARD REPORTS / INVENTORY STOCK REPORT
        public IEnumerable<ItemCard_DAL> ItemCardReportList { get; set; }
        public IPagedList<ItemCard_DAL> ItemCardReportListParts { get; set; }
        public IEnumerable<ItemCard_DAL> ItemCardReportList_TTL { get; set; }
      
        
        public IEnumerable<ItemCard_DAL> Select_ItemCard_Report_DAL(string ItemCode, string ItemSerialNO,
            string StartDate, string EndDate, int Loc_ID, int c_ID)
        {
            List<ItemCard_DAL> RD = new List<ItemCard_DAL>();
            string sp = "Select_ItemCard_Report";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ItemCode", ItemCode);
                        cmd.Parameters.AddWithValue("@ItemSerialNO ", ItemSerialNO);
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Loc_ID", Loc_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);





                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                ItemCard_DAL o = new ItemCard_DAL();

                                o.SNo = dr["SNo"].ToString();
                                o.ItemCode = dr["ItemCode"].ToString();
                                o.ItemDescription = dr["ItemDescription"].ToString();

                                o.SaleDescription = dr["SaleDescription"].ToString();
                                o.Trans_Date = dr["Trans_Date"].ToString();
                                o.Trans_Ref = dr["Trans_Ref"].ToString();

                                o.itemSerialNO = dr["itemSerialNO"].ToString();
                                o.Qty = dr["Qty"].ToString();
                                o.UnitPrice = dr["UnitPrice"].ToString();
                                o.Pur_Amount = dr["Pur_Amount"].ToString();
                                o.VATEXP_Pur = dr["VATEXP_Pur"].ToString();
                                o.Pur_Total_Amt = dr["Pur_Total_Amt"].ToString();

                                o.Sold_SerialNo = dr["Sold_SerialNo"].ToString();
                                o.SaleUnitPrice = dr["SaleUnitPrice"].ToString();
                                o.QtySold = dr["QtySold"].ToString();
                                o.SaleAmount = dr["SaleAmount"].ToString();
                                o.VAT_Sale = dr["VAT_Sale"].ToString();
                                o.Sale_Total_Amt = dr["Sale_Total_Amt"].ToString();

                                o.ItemLocationName = dr["ItemLocationName"].ToString();
                                o.Value = dr["Value"].ToString();
                                o.Qty_Bal = dr["Qty_Bal"].ToString();
                                o.ItemName = dr["ItemName"].ToString();


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

        public IEnumerable<ItemCard_DAL> Select_ItemCard_Report_Parts_DAL(string ItemCode, int Item_ID, string traditional, string Make,
            string Fuel, string Transmission, string Drive, int ItemGroup_ID, int ItemCategory_ID, string Year, string ItemSerialNO,
            string StartDate, string EndDate, int Loc_ID, string EngineSpecsCode_ID, int c_ID)
        {
            List<ItemCard_DAL> RD = new List<ItemCard_DAL>();
            string sp = "Select_ItemCard_Report_parts";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ItemCode", ItemCode);
                        cmd.Parameters.AddWithValue("@Item_ID", Item_ID);
                        cmd.Parameters.AddWithValue("@traditional", traditional);
                        cmd.Parameters.AddWithValue("@Make", Make);
                        cmd.Parameters.AddWithValue("@Fuel", Fuel);
                        cmd.Parameters.AddWithValue("@Transmission", Transmission);
                        cmd.Parameters.AddWithValue("@Drive", Drive);
                        cmd.Parameters.AddWithValue("@ItemGroup_ID", ItemGroup_ID);
                        cmd.Parameters.AddWithValue("@ItemCategory_ID" , ItemCategory_ID);
                        cmd.Parameters.AddWithValue("@Year", Year);
                        cmd.Parameters.AddWithValue("@ItemSerialNO", ItemSerialNO);
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Loc_ID", Loc_ID);
                        cmd.Parameters.AddWithValue("@EngineSpecsCode_ID", EngineSpecsCode_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);



                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                ItemCard_DAL o = new ItemCard_DAL();

                                o.SNo = dr["SNo"].ToString();
                                o.ItemCode = dr["ItemCode"].ToString();
                                o.ItemDescription = dr["ItemDescription"].ToString();

                                o.SaleDescription = dr["SaleDescription"].ToString();
                                o.Trans_Date = dr["Trans_Date"].ToString();
                                o.Trans_Ref = dr["Trans_Ref"].ToString();

                                o.itemSerialNO = dr["itemSerialNO"].ToString();
                                o.Qty = dr["Qty"].ToString();
                                o.UnitPrice = dr["UnitPrice"].ToString();
                                o.Pur_Amount = dr["Pur_Amount"].ToString();
                                o.VATEXP_Pur = dr["VATEXP_Pur"].ToString();
                                o.Pur_Total_Amt = dr["Pur_Total_Amt"].ToString();

                                o.Sold_SerialNo = dr["Sold_SerialNo"].ToString();
                                o.SaleUnitPrice = dr["SaleUnitPrice"].ToString();
                                o.QtySold = dr["QtySold"].ToString();
                                o.SaleAmount = dr["SaleAmount"].ToString();
                                o.VAT_Sale = dr["VAT_Sale"].ToString();
                                o.Sale_Total_Amt = dr["Sale_Total_Amt"].ToString();

                                o.ItemLocationName = dr["ItemLocationName"].ToString();
                                o.Value = dr["Value"].ToString();
                                o.Qty_Bal = dr["Qty_Bal"].ToString();
                                o.ItemName = dr["ItemName"].ToString();


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

        public IEnumerable<ItemCard_DAL> Select_ItemCard_Report_parts_TTL_DAL(string ItemCode, int Item_ID, string traditional, string Make,
            string Fuel, string Transmission, string Drive, int ItemGroup_ID, int ItemCategory_ID, string Year, string ItemSerialNO,
            string StartDate, string EndDate, int Loc_ID, string EngineSpecsCode_ID, int c_ID)
        {
            List<ItemCard_DAL> RD = new List<ItemCard_DAL>();
            string sp = "Select_ItemCard_Report_parts_TTL";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ItemCode", ItemCode);
                        cmd.Parameters.AddWithValue("@Item_ID", Item_ID);
                        cmd.Parameters.AddWithValue("@traditional", traditional);
                        cmd.Parameters.AddWithValue("@Make", Make);
                        cmd.Parameters.AddWithValue("@Fuel", Fuel);
                        cmd.Parameters.AddWithValue("@Transmission", Transmission);
                        cmd.Parameters.AddWithValue("@Drive", Drive);
                        cmd.Parameters.AddWithValue("@ItemGroup_ID", ItemGroup_ID);
                        cmd.Parameters.AddWithValue("@ItemCategory_ID", ItemCategory_ID);
                        cmd.Parameters.AddWithValue("@Year", Year);
                        cmd.Parameters.AddWithValue("@ItemSerialNO", ItemSerialNO);
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Loc_ID", Loc_ID);
                        cmd.Parameters.AddWithValue("@EngineSpecsCode_ID", EngineSpecsCode_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);




                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                ItemCard_DAL o = new ItemCard_DAL();

                                o.Qty = dr["Qty"].ToString();

                                o.Pur_Amount = dr["Pur_Amount"].ToString();
                                o.VATEXP_Pur = dr["VATEXP_Pur"].ToString();
                                o.Pur_Total_Amt = dr["Pur_Total_Amt"].ToString();

                                o.QtySold = dr["QtySold"].ToString();
                                o.SaleAmount = dr["SaleAmount"].ToString();
                                o.VAT_Sale = dr["VAT_Sale"].ToString();
                                o.Sale_Total_Amt = dr["Sale_Total_Amt"].ToString();

                                o.AMOUNT_BAL = dr["AMOUNT_BAL"].ToString();
                                o.QTY_BAL = dr["QTY_BAL"].ToString();


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

        public IEnumerable<ItemCard_DAL> Select_ItemCard_Report_TTL_DAL(string ItemCode, string ItemSerialNO,
          string StartDate, string EndDate, int Loc_ID, int c_ID)
        {
            List<ItemCard_DAL> RD = new List<ItemCard_DAL>();
            string sp = "Select_ItemCard_Report_TTL";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ItemCode", ItemCode);
                        cmd.Parameters.AddWithValue("@ItemSerialNO ", ItemSerialNO);
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Loc_ID", Loc_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);




                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                ItemCard_DAL o = new ItemCard_DAL();

                                o.Qty = dr["Qty"].ToString();

                                o.Pur_Amount = dr["Pur_Amount"].ToString();
                                o.VATEXP_Pur = dr["VATEXP_Pur"].ToString();
                                o.Pur_Total_Amt = dr["Pur_Total_Amt"].ToString();

                                o.QtySold = dr["QtySold"].ToString();
                                o.SaleAmount = dr["SaleAmount"].ToString();
                                o.VAT_Sale = dr["VAT_Sale"].ToString();
                                o.Sale_Total_Amt = dr["Sale_Total_Amt"].ToString();

                              

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



        #endregion


        public IEnumerable<Deposits_DAL> GetDataForDpositMaster_Print(int? DV_ID)
        {

            string sp = "get_dvDetails_print";

            List<Deposits_DAL> MAS = new List<Deposits_DAL>();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DV_ID", DV_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Deposits_DAL pm = new Deposits_DAL();

                                pm.DV_Ref = dr["DV_Ref"].ToString();
                                pm.Date_Taken = dr["Date_Taken"].ToString();
                                pm.CUSTOMER_NAME = dr["CUSTOMER_NAME"].ToString();
                                pm.Chassis_No = dr["Chassis_No"].ToString();
                                pm.ModelDesciption = dr["ModelDesciption"].ToString();
                                pm.Make = dr["Make"].ToString();
                                pm.ModelYear = dr["ModelYear"].ToString();
                                pm.Color = dr["Color"].ToString();

                                pm.AfterTotal = dr["AfterTotal"].ToString();
                                pm.AMOUNT_INWORDS = dr["AMOUNT_INWORDS"].ToString();
                                pm.refund_Customer = dr["refund_Customer"].ToString();
                                pm.refund_Contact = dr["refund_Contact"].ToString();
                                pm.REFUND_DATE = dr["REFUND_DATE"].ToString();
                                pm.DEPOSIT_EXP_DATE = dr["DEPOSIT_EXP_DATE"].ToString();
                                pm.CAR_SHIP_DATE = dr["CAR_SHIP_DATE"].ToString();
                                pm.DEDUCTED_AMOUNT = dr["DEDUCTED_AMOUNT"].ToString();
                                pm.Amount_Return = dr["Amount_Return"].ToString();

                                pm.Total_Amount = dr["Total_Amount"].ToString();
                                pm.PAPER_MADE_DATE = dr["PAPER_MADE_DATE"].ToString();


                                MAS.Add(pm);
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


            return MAS;
        }



        public IEnumerable<Deposits_DAL> GetDataForDpositMasterCheque_Print(int? DV_ID)
        {

            string sp = "get_dvDetailsCheque_print";

            List<Deposits_DAL> MAS = new List<Deposits_DAL>();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DV_ID", DV_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Deposits_DAL pm = new Deposits_DAL();

                                pm.cheque_Date = dr["cheque_Date"].ToString();
                                pm.Cheque_no = dr["Cheque_no"].ToString();
                                pm.Cheque_Bank_Name = dr["Cheque_Bank_Name"].ToString();



                                MAS.Add(pm);
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


            return MAS;
        }

        public pa_SalesMaster_DAL Get_SalesMasterJPMasterByID_Print_DAL(int? SalesMaster_ID)
        {

            string sp = "Pa_Select_SaleMasterByID_JP_Print";

            pa_SalesMaster_DAL pm = new pa_SalesMaster_DAL();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {


                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SalesMaster_ID", SalesMaster_ID);

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        pm.SaleMaster_ID = Convert.ToInt32(dr["SaleMaster_ID"]);
                        pm.Customer_ID = dr["Customer_ID"].ToString();
                        pm.SaleRef = dr["SaleRef"].ToString();
                        pm.SaleDate = dr["SaleDate"].ToString();
                        pm.Invoice_address = dr["Invoice_address"].ToString();
                        pm.Customer_Contact = dr["Customer_Contact"].ToString();
                        pm.Manualbook_ref = dr["Manualbook_ref"].ToString();
                        pm.Saletype = dr["Sale_trans_type"].ToString();
                        pm.CustomerName = dr["CustomerName"].ToString();

                        pm.Created_at = dr["Created_at"].ToString();
                        pm.Created_by = dr["Created_by"].ToString();
                        pm.Modified_at = dr["Modified_at"].ToString();
                        pm.Modified_by = dr["Modified_by"].ToString();
                        pm.BankName = dr["BankName"].ToString();
                        pm.Branch = dr["branch"].ToString();
                        pm.AccountName = dr["AccountName"].ToString();
                        pm.AccountNumber = dr["AccountNumber"].ToString();
                        pm.IBAN = dr["IBAN"].ToString();
                        pm.SwiftCode = dr["SwiftCode"].ToString();
                        pm.BankAddress = dr["Address"].ToString();


                    }

                }
                con.Close();

            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }


            return pm;
        }

        public IEnumerable<SaleDetailJP> pa_select_salesJP_Print_DAL(int? SalesMaster_ID)
        {



            string sp = "pa_select_salesJP_Print";
            List<SaleDetailJP> itemlist = new List<SaleDetailJP>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SalesMaster_ID", SalesMaster_ID);

                    SqlDataReader dr = cmd.ExecuteReader();


                    while (dr.Read())
                    {
                        SaleDetailJP pm = new SaleDetailJP();
                        pm.Sale_ID = Convert.ToInt32(dr["SaleMaster_ID"].ToString());
                        pm.Sale_Ref = dr["SaleRef"].ToString();
                        pm.SalesDetails_ID = Convert.ToInt32(dr["SalesDetails_ID"].ToString());

                        pm.Chassis_no = Convert.ToString(dr["Chassis_no"].ToString());
                        pm.Price = Convert.ToDecimal(dr["Price"].ToString());
                        pm.PriceTax = Convert.ToDecimal(dr["PriceTax"].ToString());
                        pm.PriceRate = Convert.ToDecimal(dr["PriceRate"].ToString());
                        pm.RecycleFee = Convert.ToDecimal(dr["RecycleFee"].ToString());
                        //pm.RecycleFeeTax = Convert.ToDecimal(dr["RecycleFeeTax"].ToString());
                        pm.PlateFee = Convert.ToDecimal(dr["PlateFee"].ToString());
                        pm.AuctionFee = Convert.ToDecimal(dr["AuctionFee"].ToString());
                        pm.AuctionFeeTax = Convert.ToDecimal(dr["AuctionFeeTax"].ToString());
                        //pm.PlatNaumberTax = Convert.ToDecimal(dr["PlatNaumberTax"].ToString());
                        pm.Amount = Convert.ToDecimal(dr["Amount"].ToString());

                        pm.Make = dr["Make"].ToString();
                        pm.Color = dr["Color"].ToString();
                        pm.MakeModelDescription = dr["ModelDesciption"].ToString();
                        pm.Year = dr["ModelYear"].ToString();

                        pm.OfficeCharges = Convert.ToDecimal(dr["OfficeCharges"].ToString());

                        pm.OfficeChargesTax = Convert.ToDecimal(dr["OfficeChargesTax"].ToString());
                        pm.SaleType = Convert.ToString(dr["Sale_Type"].ToString());



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

        public pa_select_saledetails_Total_Result pa_select_salesJP_Total_Print_DAL(int? SalesMaster_ID)
        {
            string sp = "pa_select_salesJP_Total_Print";

            pa_select_saledetails_Total_Result pm = new pa_select_saledetails_Total_Result();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SalesMaster_ID", SalesMaster_ID);


                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {

                                pm.TotalPrice = Convert.ToDecimal(dr["TotalPrice"]);
                                pm.TotalRecyclePrice = Convert.ToDecimal(dr["TotalRecyclePrice"]);
                                pm.TotalAuctionPrice = Convert.ToDecimal(dr["TotalAuctionPrice"]);
                                pm.TotalPlatNaumberPrice = Convert.ToDecimal(dr["TotalPlatNaumberPrice"]);
                                pm.TotalPlatNaumberPrice = Convert.ToDecimal(dr["TotalPlatNaumberPrice"]);
                                pm.TotalTax = Convert.ToDecimal(dr["TotalTax"]);
                                pm.GrandTotal = Convert.ToDecimal(dr["GrandTotal"]);

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

        public IEnumerable<SaleReport> pa_Select_SalesReport_trd_DAL(string StartDate, string EndDate, int? Customer_ID, string Sale_Ref, int c_ID, int Seller_ID)
        {
            List<SaleReport> RD = new List<SaleReport>();
            string sp = "Select_SalesReport_trd";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@CustomerID", Customer_ID);
                        cmd.Parameters.AddWithValue("@SaleRef", Sale_Ref);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);
                        cmd.Parameters.AddWithValue("@Seller_ID", Seller_ID);


                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                SaleReport o = new SaleReport();

//                                SELECT CONVERT(varchar(30), SaleDate, 105) AS Date, SaleRef,
//Convert(varchar(30), Amount, 1) as SaleAmount,
//Convert(varchar(30), VATExp, 1) as VATExp,
//Convert(varchar(30), Total_Amt, 1) as Total_Amt,
//Convert(varchar(30), Total_Sold_Cost, 1) as Total_Sold_Cost,   
//Convert(varchar(30), Total_Expense, 1) as Total_Expense,
//Convert(varchar(30), isNull(Amount, 0) - isnull(Total_Sold_Cost, 0) - isnull(Total_Expense, 0), 1) AS ProfitLost,
//    Remarks, SaleMaster_ID


                                o.Date = dr["Date"].ToString();
                                o.SaleRef = dr["SaleRef"].ToString();

                                o.Sale_Value = dr["Sale_Value"].ToString();
                                o.VATExp = dr["VATExp"].ToString();
                                o.Total_Amt = dr["Total_Amt"].ToString();

                                o.Total_Expense = dr["Total_Expense"].ToString();
                                o.Total_Sold_Cost = dr["Total_Sold_Cost"].ToString();
                                o.ProfitLost = dr["ProfitLost"].ToString();
                                                              
                                o.Manualbook_ref = dr["Manualbook_ref"].ToString();
                                o.ShipingFee = dr["ShippingCharges"].ToString();
                                o.COD = dr["OtherCost"].ToString();
                                o.Customer_Name = dr["Remarks"].ToString();
                                o.Tip = dr["Tip"].ToString();
                                o.SellerName = dr["SellerName"].ToString();
                              

                                o.SaleMaster_ID = Convert.ToInt32(dr["SaleMaster_ID"].ToString());






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

        public SaleReport pa_Select_SalesReport_trd_TTL_DAL(string StartDate, string EndDate, int? Customer_ID, string Sale_Ref, int c_ID,int Seller_ID)
        {
            SaleReport o = new SaleReport();
            string sp = "Select_SalesReport_trd_ttl";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@CustomerID", Customer_ID);
                        cmd.Parameters.AddWithValue("@SaleRef", Sale_Ref);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);
                        cmd.Parameters.AddWithValue("@Seller_ID", Seller_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {



                                o.Sale_Value = dr["Sale_Value"].ToString();
                                o.VATExp = dr["VATExp"].ToString();
                                o.Total_Amt = dr["Total_Amt"].ToString();
                                o.Total_Expense = dr["Total_Expense"].ToString();
                                o.Total_Sold_Cost = dr["Total_Sold_Cost"].ToString();
                                o.ProfitLost = dr["ProfitLost"].ToString();
                                o.Tip = dr["Total_Tip"].ToString();
                                o.ShipingFee = dr["Total_ShippingCharges"].ToString();
                                o.COD = dr["Total_OtherCost"].ToString();


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


            return o;


        }

        public IEnumerable<StockReport> pa_Select_StockCostReport_DAL(string StartPurchaseDate, string EndPurchaseDate, string Container_No,
         int c_ID)
        {
            List<StockReport> RD = new List<StockReport>();
            string sp = "select_StockCostReport";



            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PurchaseStartDate", StartPurchaseDate);
                        cmd.Parameters.AddWithValue("@PurchaseEndDate", EndPurchaseDate);
                        cmd.Parameters.AddWithValue("@Container_No", Container_No);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                StockReport o = new StockReport();


                                o.Stock_ID = dr["stock_ID"].ToString();
                                o.Chassis_No = dr["Chassis_No"].ToString();
                                o.PurchaseDate = dr["PurchaseDate"].ToString();

                                o.Total_Cost = dr["Total_Cost"].ToString();
                                o.ModelYear = dr["ModelYear"].ToString();

                                o.Stock_Status = dr["Stock_Status"].ToString();


                                o.ModelDesciption = dr["ModelDesciption"].ToString();

                                o.Price = dr["Price"].ToString();
                                o.PriceTax = dr["PriceTax"].ToString();
                                o.Auction = dr["AuctionFee"].ToString();
                                o.Rekso = dr["ReksoFee"].ToString();
                                o.Recycle = dr["RecycleFee"].ToString();
                                o.Freight = dr["FreightCharges"].ToString();
                                o.Loading = dr["LoadingCharges"].ToString();

                                o.OtherCharges_JP = dr["OtherCharges_JP"].ToString();
                                o.JP_Charges = dr["JP_Charges"].ToString();

                                o.PriceRate = dr["PriceRate"].ToString();
                                o.Total_Cost_Others = dr["Total_Cost_Others"].ToString();



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

        public IEnumerable<StockReport> select_StockCostReport_ttl_DAL(string StartPurchaseDate, string EndPurchaseDate, string Container_No,
         int c_ID)
        {
            List<StockReport> RD = new List<StockReport>();
            string sp = "select_StockCostReport_ttl";



            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PurchaseStartDate", StartPurchaseDate);
                        cmd.Parameters.AddWithValue("@PurchaseEndDate", EndPurchaseDate);
                        cmd.Parameters.AddWithValue("@Container_No", Container_No);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                StockReport o = new StockReport();


                                o.Price = dr["Price"].ToString();
                                o.PriceTax = dr["PriceTax"].ToString();
                                o.Auction = dr["AuctionFee"].ToString();
                                o.Rekso = dr["ReksoFee"].ToString();
                                o.Recycle = dr["RecycleFee"].ToString();
                                o.Freight = dr["FreightCharges"].ToString();
                                o.Loading = dr["LoadingCharges"].ToString();
                                o.OtherCharges_JP = dr["OtherCharges_JP"].ToString();
                                o.JP_Charges = dr["JP_Charges"].ToString();
                                o.Total_Cost = dr["Total_Cost"].ToString();
                                o.Total_Cost_Others = dr["Total_Cost_Others"].ToString();

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


        public IEnumerable<pa_tblLedgers> pa_Select_VendorLedger_Other_DAL(string StartDate, string EndDate, string Vendor_ID, int? c_ID)
        {
            List<pa_tblLedgers> RD = new List<pa_tblLedgers>();
            string sp = "pa_Select_VendorLedger_Other";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_tblLedgers o = new pa_tblLedgers();

                                o.vendor_ID = dr["vendor_ID"].ToString();
                                o.Date = dr["Date"].ToString();
                                o.trans_ref = dr["trans_ref"].ToString();
                                o.Debit = dr["Debit"].ToString();
                                o.Credit = dr["Credit"].ToString();
                                o.Description = dr["Description"].ToString();
                                o.trans_created_at = dr["trans_created_at"].ToString();
                                o.Chassis_No = dr["Chassis_No"].ToString();
                                o.ACCOUNT = dr["ACCOUNT"].ToString();
                                o.Alt_ACCOUNT = dr["Alt_ACCOUNT"].ToString();
                                o.PartyName = dr["PartyName"].ToString();
                                o.Link = dr["Link"].ToString();

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


        public IEnumerable<pa_tblLedgers> pa_Select_VendorLedger_Other_DAL_TTL(string StartDate, string EndDate, string Vendor_ID, int? c_ID)
        {
            List<pa_tblLedgers> RD = new List<pa_tblLedgers>();
            string sp = "pa_Select_VendorLedger_Other_TTL";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_tblLedgers o = new pa_tblLedgers();

                                o.TotalDebit = dr["TotalDebit"].ToString();
                                o.TotalCredit = dr["TotalCredit"].ToString();
                                o.TotalBalance = dr["TotalBalance"].ToString();


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
        public pa_tblLedgers pa_Select_VendorLedger_Other_DAL_TTL_Print(string StartDate, string EndDate, string Vendor_ID, int? c_ID)
        {
            pa_tblLedgers o = new pa_tblLedgers();
            string sp = "pa_Select_VendorLedger_Other_TTL";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {


                                o.TotalDebit = dr["TotalDebit"].ToString();
                                o.TotalCredit = dr["TotalCredit"].ToString();
                                o.TotalBalance = dr["TotalBalance"].ToString();






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


            return o;


        }
        public IEnumerable<ItemCard_DAL> Select_Itemlocation_Report_Parts_DAL(string ItemCode, int Item_ID, string traditional, string Make,
    string Fuel, string Transmission, string Drive, int ItemGroup_ID, int ItemCategory_ID, string Year, string ItemSerialNO,
    string StartDate, string EndDate, int Loc_ID, int c_ID)
        {
            List<ItemCard_DAL> RD = new List<ItemCard_DAL>();
            string sp = "Select_ItemCard_Report_parts_Location";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ItemCode", ItemCode);
                        cmd.Parameters.AddWithValue("@Item_ID", Item_ID);
                        cmd.Parameters.AddWithValue("@traditional", traditional);
                        cmd.Parameters.AddWithValue("@Make", Make);
                        cmd.Parameters.AddWithValue("@Fuel", Fuel);
                        cmd.Parameters.AddWithValue("@Transmission", Transmission);
                        cmd.Parameters.AddWithValue("@Drive", Drive);
                        cmd.Parameters.AddWithValue("@ItemGroup_ID", ItemGroup_ID);
                        cmd.Parameters.AddWithValue("@ItemCategory_ID", ItemCategory_ID);
                        cmd.Parameters.AddWithValue("@Year", Year);
                        cmd.Parameters.AddWithValue("@ItemSerialNO", ItemSerialNO);
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Loc_ID", Loc_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);



                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                ItemCard_DAL o = new ItemCard_DAL();

                                o.SNo = dr["SNo"].ToString();
                                o.ItemCode = dr["ItemCode"].ToString();
                                o.ItemDescription = dr["ItemDescription"].ToString();

                                o.SaleDescription = dr["SaleDescription"].ToString();
                                o.Trans_Date = dr["Trans_Date"].ToString();
                                o.Trans_Ref = dr["Trans_Ref"].ToString();

                                o.itemSerialNO = dr["itemSerialNO"].ToString();
                                o.Qty = dr["Qty"].ToString();
                                o.UnitPrice = dr["UnitPrice"].ToString();
                                o.Pur_Amount = dr["Pur_Amount"].ToString();
                                o.VATEXP_Pur = dr["VATEXP_Pur"].ToString();
                                o.Pur_Total_Amt = dr["Pur_Total_Amt"].ToString();

                                o.Sold_SerialNo = dr["Sold_SerialNo"].ToString();
                                o.SaleUnitPrice = dr["SaleUnitPrice"].ToString();
                                o.QtySold = dr["QtySold"].ToString();
                                o.SaleAmount = dr["SaleAmount"].ToString();
                                o.VAT_Sale = dr["VAT_Sale"].ToString();
                                o.Sale_Total_Amt = dr["Sale_Total_Amt"].ToString();

                                o.ItemLocationName = dr["ItemLocationName"].ToString();
                                o.Value = dr["Value"].ToString();
                                o.Qty_Bal = dr["Qty_Bal"].ToString();
                                o.ItemName = dr["ItemName"].ToString();


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

        public IEnumerable<ItemCard_DAL> Select_ItemCard_Report_location_TTL_DAL(string ItemCode, int Item_ID, string traditional, string Make,
            string Fuel, string Transmission, string Drive, int ItemGroup_ID, int ItemCategory_ID, string Year, string ItemSerialNO,
            string StartDate, string EndDate, int Loc_ID, int c_ID)
        {
            List<ItemCard_DAL> RD = new List<ItemCard_DAL>();
            string sp = "Select_ItemCard_Report_location_TTL";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ItemCode", ItemCode);
                        cmd.Parameters.AddWithValue("@Item_ID", Item_ID);
                        cmd.Parameters.AddWithValue("@traditional", traditional);
                        cmd.Parameters.AddWithValue("@Make", Make);
                        cmd.Parameters.AddWithValue("@Fuel", Fuel);
                        cmd.Parameters.AddWithValue("@Transmission", Transmission);
                        cmd.Parameters.AddWithValue("@Drive", Drive);
                        cmd.Parameters.AddWithValue("@ItemGroup_ID", ItemGroup_ID);
                        cmd.Parameters.AddWithValue("@ItemCategory_ID", ItemCategory_ID);
                        cmd.Parameters.AddWithValue("@Year", Year);
                        cmd.Parameters.AddWithValue("@ItemSerialNO", ItemSerialNO);
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Loc_ID", Loc_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);




                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                ItemCard_DAL o = new ItemCard_DAL();

                                o.Qty = dr["Qty"].ToString();

                                o.Pur_Amount = dr["Pur_Amount"].ToString();
                                o.VATEXP_Pur = dr["VATEXP_Pur"].ToString();
                                o.Pur_Total_Amt = dr["Pur_Total_Amt"].ToString();

                                o.QtySold = dr["QtySold"].ToString();
                                o.SaleAmount = dr["SaleAmount"].ToString();
                                o.VAT_Sale = dr["VAT_Sale"].ToString();
                                o.Sale_Total_Amt = dr["Sale_Total_Amt"].ToString();

                                o.AMOUNT_BAL = dr["AMOUNT_BAL"].ToString();
                                o.QTY_BAL = dr["QTY_BAL"].ToString();


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


        //By Yaseen 1-12-2022
        #region BL Report


        public IPagedList<BLReport> BLReport { get; set; }

        public IEnumerable<BLReport> BLReportPrint { get; set; }

        public IEnumerable<BLReport> pa_Select_BLReport_DAL(string StartDate, string EndDate, string BLNO_Ref, int c_ID, string chassis)
        {

            List<BLReport> bl = new List<BLReport>();
            string sp = "Select_BLReport";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);


                        cmd.Parameters.AddWithValue("@BLNORef", BLNO_Ref);
                        cmd.Parameters.AddWithValue("@chassis", chassis);

                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                BLReport o = new BLReport();


                                o.Date = dr["Date"].ToString();
                                o.BLNO_Ref = dr["BLNO_Ref"].ToString();
                                o.Chassis_no = dr["Chassis_no"].ToString();

                                o.make = dr["make"].ToString();
                                o.model_description = dr["model_description"].ToString();
                                o.Year = dr["ModelYear"].ToString();
                                o.Bl_Exp = dr["Bl_Expense"].ToString();
                                o.stock_id = Convert.ToInt32(dr["stock_ID"].ToString());
                                o.Total_Expense = dr["Total_Expense"].ToString();
                                o.Total_Cost = dr["Total_Cost"].ToString();


                                bl.Add(o);



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


            return bl;

        }

        #endregion
        //By Yaseen 1-12-2022

        //By Yaseen 1-13-2022
        #region Purchase Report
        public IPagedList<PurchaseReport> PurchaseReportList { get; set; }
        public IEnumerable<PurchaseReport> PurchaseReportPrint { get ; set; }
        public IEnumerable<pa_VAT> VAT_INReportPrint { get ; set ; }
        public IEnumerable<pa_VAT> VAT_OUTReportPrint { get; set; }
        public pa_VAT VAT_INReport_TTL { get ; set ; }
       // public IEnumerable<pa_VAT> VAT_OUTReportReport { get ; set ; }
        public pa_VAT VAT_OUTReport_TTL { get ; set ; }
        public IPagedList<pa_VAT> VAT_INReportReport { get ; set ; }

        public IPagedList<pa_VAT> VAT_OUTReportReport { get; set; }

        public IEnumerable<PurchaseReport> pa_Select_PurchaseReport_DAL(string StartDate, string EndDate, string Pur_Ref, int c_ID, string chassis)
        {
            List<PurchaseReport> pr = new List<PurchaseReport>();
            string sp = "Select_PurchaseReport";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Pur_Ref", Pur_Ref);
                        cmd.Parameters.AddWithValue("@chassis", chassis);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                PurchaseReport o = new PurchaseReport();


                                o.Date = dr["Date"].ToString();
                                o.Pur_Ref = dr["Pur_Ref"].ToString();
                                o.Chassis_no = dr["Chassis_no"].ToString();

                                o.make = dr["make"].ToString();
                                o.model_description = dr["model_description"].ToString();
                                o.Year = dr["ModelYear"].ToString();
                                o.color = dr["Color"].ToString();
                                o.Purchase_Amount = dr["Purchase_Amount"].ToString();

                                pr.Add(o);



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


            return pr;

        }

        #endregion

        //By Yaseen 1-13-2022






        //By Yaseen 1-26-2022

        #region  Saller Report

       public IEnumerable<Pa_Partners_DAL> Get_SellersMasterList_DAL(int c_ID)
        {


            string sp = "Pa_Select_SellerMasterList";
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

                        item.Partner_ID = Convert.ToInt32(rdr["Seller_ID"]);
                        item.PartnerName = rdr["Seller_Name"].ToString();           
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

        public IEnumerable<pa_tblLedgers> pa_Select_SallarLedger_DAL(string StartDate, string EndDate, string Saller_ID, int? c_ID)
        {
            List<pa_tblLedgers> RD = new List<pa_tblLedgers>();
            string sp = "pa_Select_SellerLedger";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Seller_ID", Saller_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_tblLedgers o = new pa_tblLedgers();

                                o.vendor_ID = dr["Party_ID"].ToString();
                                o.Date = dr["Date"].ToString();
                                o.trans_ref = dr["trans_ref"].ToString();
                                o.Debit = dr["Debit"].ToString();
                                o.Credit = dr["Credit"].ToString();
                                o.Description = dr["Description"].ToString();
                                o.trans_created_at = dr["trans_created_at"].ToString();                        
                                o.ACCOUNT = dr["ACCOUNT"].ToString();
                                o.Alt_ACCOUNT = dr["Alt_ACCOUNT"].ToString();
                                o.PartyName = dr["PartyName"].ToString();
                                o.Link = dr["Link"].ToString();
                                o.SaleMaster_ID = Convert.ToInt32(dr["SaleMaster_ID"].ToString());
                                o.Order_Ref = dr["Manualbook_ref"].ToString();
                             

                               
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


        public IEnumerable<pa_tblLedgers> pa_Select_SallerLedger_DAL_TTL(string StartDate, string EndDate, string Saller_ID, int? c_ID)
        {
            List<pa_tblLedgers> RD = new List<pa_tblLedgers>();
            string sp = "pa_Select_SallerLedger_TTL";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Seller_ID", Saller_ID);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_tblLedgers o = new pa_tblLedgers();

                                o.TotalDebit = dr["TotalDebit"].ToString();
                                o.TotalCredit = dr["TotalCredit"].ToString();
                                o.TotalBalance = dr["TotalBalance"].ToString();
                                o.Opening_Balance = dr["Opening_Balance"].ToString();
                                o.Closing_Balance = dr["Closing_Balance"].ToString();

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

        public IEnumerable<pa_VAT> pa_Select_VAT_IN_DAL(string StartDate, string EndDate, int? c_ID)
        {
            List<pa_VAT> RD = new List<pa_VAT>();
            string sp = "select_VAT_Report_IN";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);                       
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_VAT o = new pa_VAT();

                                o.Date = dr["Date"].ToString();
                                o.Ref = dr["Ref"].ToString();
                                o.Party_Name = dr["Party_Name"].ToString();
                                o.TransType = dr["Type"].ToString();
                                o.Value = dr["Value"].ToString();
                                o.VAT_Rate = dr["Returned"].ToString();
                                o.Collected = dr["Collected"].ToString();
                                o.Return = dr["Returned"].ToString();
                                

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

        public pa_VAT pa_Select_VAT_IN_TTL_DAL(string StartDate, string EndDate, int? c_ID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<pa_VAT> pa_Select_VAT_OUT_DAL(string StartDate, string EndDate, int? c_ID)
        {
            List<pa_VAT> RD = new List<pa_VAT>();
            string sp = "select_VAT_Report_OUT";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_VAT o = new pa_VAT();

                                o.Date = dr["Date"].ToString();
                                o.Ref = dr["Ref"].ToString();
                                o.Party_Name = dr["Party_Name"].ToString();
                                o.TransType = dr["Type"].ToString();
                                o.Value = dr["Value"].ToString();
                                 o.VAT_Rate = dr["Returned"].ToString();
                                o.Paid = dr["Paid"].ToString();
                                o.Return = dr["Returned"].ToString();


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

        public pa_VAT pa_Select_VAT_OUT_TTL_DAL(string StartDate, string EndDate, int? c_ID)
        {
            throw new NotImplementedException();
        }


        #endregion

        //By Yaseen 1-26-2022


        public pa_tblLedgers pa_Select_CustomerReportByID_DAL(int? Customer_ID)
        {

            string sp = "Pa_CustomerReportByID";

            pa_tblLedgers o = new pa_tblLedgers();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Customer_ID", Customer_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                           


                                o.customer_ID = dr["customer_ID"].ToString();
                                o.Date = dr["Date"].ToString();
                                o.trans_ref = dr["trans_ref"].ToString();
                                o.Debit = dr["Debit"].ToString();
                                o.Credit = dr["Credit"].ToString();


                                o.Description = dr["Description"].ToString();
                                o.trans_created_at = dr["trans_created_at"].ToString();
                                o.Chassis_No = dr["Chassis_No"].ToString();
                                o.ACCOUNT = dr["ACCOUNT"].ToString();
                                o.Alt_ACCOUNT = dr["Alt_ACCOUNT"].ToString();
                                o.PartyName = dr["PartyName"].ToString();
                                o.Link = dr["Link"].ToString();
                                // itemdesc = dr["ItemDesc"].ToString();
                                //if (o.Description == "" && itemdesc != null)
                                //{
                                //    o.Description = itemdesc;
                                //}
                               

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

            return o;
        }
        public pa_tblLedgers VendorLedgerMasterObj { get; set; }


        public pa_tblLedgers pa_Select_VendorReportByID_DAL(int? VendorID)
        {

            string sp = "Pa_VendorIDReportByID";

            pa_tblLedgers o = new pa_tblLedgers();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@VendorID", VendorID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {



                                o.vendor_ID = dr["vendor_ID"].ToString();
                                o.Date = dr["Date"].ToString();
                                o.trans_ref = dr["trans_ref"].ToString();
                                o.Debit = dr["Debit"].ToString();
                                o.Credit = dr["Credit"].ToString();
                                o.Description = dr["Description"].ToString();
                                o.trans_created_at = dr["trans_created_at"].ToString();
                                o.Chassis_No = dr["Chassis_No"].ToString();
                                o.ACCOUNT = dr["ACCOUNT"].ToString();
                                o.Alt_ACCOUNT = dr["Alt_ACCOUNT"].ToString();
                                o.PartyName = dr["PartyName"].ToString();
                                o.Link = dr["Link"].ToString();


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

            return o;
        }


        public IEnumerable<pa_SalesMaster_DAL> Sale_Info_Print { get; set; }

        public IEnumerable<pa_SalesMaster_DAL> GetDataForSaleInfo_Print(int? SaleMaster_ID)
        {

            string sp = "get_Sale_Info";

            List<pa_SalesMaster_DAL> MAS = new List<pa_SalesMaster_DAL>();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SaleMaster_ID", SaleMaster_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_SalesMaster_DAL pm = new pa_SalesMaster_DAL();
                           
                                
                                pm.SaleDate = dr["SaleDate"].ToString();
                                pm.SaleRef = dr["SaleRef"].ToString();
                                pm.Amount = dr["Amount"].ToString();
                                pm.VATExp = dr["VATExp"].ToString();
                                pm.Total_Amt = dr["Total_Amount"].ToString();
                                pm.Paid_amt = dr["Paid_Amt"].ToString();
                                pm.Amount = dr["Amount"].ToString();
                                pm.Bal_due = dr["Bal_Due"].ToString();




                                MAS.Add(pm);
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


            return MAS;
        }


     

    }
}
