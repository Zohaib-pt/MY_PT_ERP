using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using X.PagedList;



namespace DAL.oClasses
{
    public class OSales : IOSales
    {
        private readonly IConnectionStringManager _conMgr;
        private readonly ISqlHelper dbLayer;
        SqlConnection con;

        public OSales(IConnectionStringManager conMgr, ISqlHelper sqlHelp)
        {

            _conMgr = conMgr;
            dbLayer = sqlHelp;
            //---Getting Connection String
            Constr = _conMgr.GetConnectionString();
        }
        public string Constr { get; set; }


        //--- by Rafay 14/jan/2021 Sales Footer

        public pa_select_saledetails_Total_Result SaleDetailGrandTotals { get; set; }
        public pa_select_saledetails_Total_Result pa_select_saledetails_Total(string SalesDetails_ID)
        {
            string sp = "pa_select_saledetails_Total";

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
                        cmd.Parameters.AddWithValue("@SaleMaster_ID", SalesDetails_ID);


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

        //--- by Rafay 14/jan/2021 


        //New Faraz Work
        public IEnumerable<pa_SalesMaster_DAL> SalesMasterIPagedList1_1 { get; set; }


        //---Model class properties
        public pa_SalesDetails SalesDetailObj { get; set; }
        public IEnumerable<pa_SalesDetails> SalesDetailList { get; set; }

        public IEnumerable<pa_SalesDetails> SalesDetailList_trd_online { get; set; }



        public IEnumerable<pa_SalesDetails> SalesDetailList_Specs { get; set; }


        public IEnumerable<pa_ReceiptDetails_DAL> SalesReceiptDetailsList { get; set; }
        public IEnumerable<pa_SalesDetails> SalesDetailList2 { get; set; }

        public pa_SalesMaster_DAL SaleMasterObj { get; set; }

        public pa_SalesMaster_DAL SaleMasterObjNEW { get; set; }
        public pa_SalesMaster_DAL SaleMasterBalanceObj { get; set; }
        public IPagedList<pa_SalesMaster_DAL> SalesMasterList { get; set; }
        public IEnumerable<pa_SalesMaster_DAL> SalesMasterListTOTAL { get; set; }
        public IPagedList<pa_SalesMaster_DAL> SalesMasterIPagedList { get; set; }

        public IEnumerable<pa_SalesMaster_DAL> SalesMaster_BookingList { get; set; }
        public IEnumerable<pa_SalesMaster_DAL> SalesMaster_PerformaInvoiceList { get; set; }
        public IPagedList<pa_SalesMaster_DAL> SalesMaster_InvoiceReturnList { get; set; }
        public IEnumerable<Pa_Attachments_DAL> attachmentList { get; set; }

        public IEnumerable<pa_SalesMaster_DAL> PerformaPrintObj { get; set; }

        public IEnumerable<pa_SalesMaster_DAL> PerformaPrintObj_Specs { get; set; }
        public SaleDetailJP SalesDetailObjJP { get; set; }
        public IEnumerable<SaleDetailJP> SalesDetailListJP { get; set; }
        public pa_SalesMaster_DAL SalesMasterObjJP { get; set; }
        public pa_SalesMaster_DAL SalesMasterRefJP { get; set; }
        public IPagedList<pa_SalesMaster_DAL> SalesMasterListJP { get; set; }

        public pa_SalesMaster_DAL GetSalesObj_Stock { get; set; }

        public pa_SalesMaster_DAL SalesMasterPrintObj { get; set; }
        public IEnumerable<pa_SalesDetails> SalesDetailPrintObj { get; set; }

        public pa_SalesDetails SalesDetailPrintObj_TTL { get; set; }
        public pa_SalesMaster_DAL SalesAggrMasterPrintObj { get; set; }
        public IEnumerable<pa_SalesDetails> SalesAggrDetailPrintObj { get; set; }

        public IEnumerable<pa_SalesDetails> SalesAggrDetailPrintObj_Summary { get; set; }

        public pa_SalesDetails SalesAggrDetailPrintObj_TTL { get; set; }
        public IEnumerable<pa_SalesMaster_DAL> SalesMasterIPagedList1 { get; set; }
        public IEnumerable<pa_SalesMaster_DAL> SalesMasterListTOTAL1 { get; set; }
        public IEnumerable<Pa_DeliveryOrder_Master> DeliveryReport_Print { get; set; }
        public IEnumerable<pa_SalesMaster_DAL> SalesMasterIPagedListnew { get; set; }

        public IEnumerable<pa_SalesMaster_DAL> SalesMasterListTOTALnew { get; set; }
        public IEnumerable<pa_SalesMaster_DAL> SalesMasterListJP1 { get; set; }
        public IEnumerable<pa_SalesMaster_DAL> SalesMasterListJP_total { get; set; }

        public IEnumerable<pa_ReceiptDetails_DAL> SalesReceiptDetailsList_Print { get; set; }
        public IPagedList<pa_SalesMaster_DAL> SalesMasterByDateIPagedList { get; set; }
        public IEnumerable<pa_SalesDetails> VoidDetailList { get; set; }
        public IEnumerable<pa_SalesDetails> DiscountDetailList { get; set; }
        public IEnumerable<pa_SalesDetails> DiscountDetailList_TTL { get; set; }

        public IEnumerable<Pa_SalesDeliveryOrder_Dtl> DODetailList { get; set; }
        public Pa_SalesDeliveryOrder_Master DOMasterObj { get; set; }
        public IPagedList<Pa_SalesDeliveryOrder_Master> DOOtherMasterList { get; set; }
        public IEnumerable<pa_SalesDetails> SaleDetailList { get; set; }



        public IEnumerable<pa_SalesDetails> SalesReturnDetailList { get; set; }
        public pa_SalesMaster_DAL SalesReturnMasterObj { get; set; }
        public IPagedList<pa_SalesMaster_DAL> SalesReturnMasterList { get; set; }
        public IEnumerable<Pa_ItemLocations_DAL> LocDescList { get; set; }


        public Pa_SalesDeliveryOrder_Master SalesDOMasterPrintObj { get; set; }
        public IEnumerable<Pa_SalesDeliveryOrder_Dtl> SalesDODetailPrintObj { get; set; }
        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMaster_Print { get; set; }
        public IEnumerable<Pa_SalesDeliveryOrder_Master> DOOtherMasterList_Print { get; set; }

        public Pa_SalesDeliveryOrder_Dtl SalesDODetailPrintObj_TTL { get; set; }

        public IEnumerable<PaperAfterSalesDetail_DAL> PaperAfterSalesDetailList { get; set; }
        public PaperAfterSalesMaster_DAL PaperAfterSalesMasterObj { get; set; }
        public IPagedList<PaperAfterSalesMaster_DAL> PaperAfterSalesMaster_jpMasterList { get; set; }
        //---insert into sales invoice detial general
        public string InsertSalesInvoiceDetail_DAL(string SalesInvoiceType, string Chassis, string ItemDesc, double? Unit_Price, double?
            VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int SaleMaster_ID, double? Discount)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
               new SqlParameter("@SalesInvoiceType",SalesInvoiceType),
                         new SqlParameter("@Chassis",Chassis),
                          new SqlParameter("@ItemDesc",ItemDesc),
                           new SqlParameter("@Unit_Price",Unit_Price),
                            new SqlParameter("@VATRate",VATRate),
                             new SqlParameter("@VATExp",VATExp),
                              new SqlParameter("@Total_Amount",Total_Amount),

                               new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@c_ID",c_ID),
                                 new SqlParameter("@Created_by",Created_by),
                                  new SqlParameter("@SaleMaster_ID",SaleMaster_ID),
                                  new SqlParameter("@Discount",Discount),
                                  new SqlParameter("@U_ItemDesc",ItemDesc)



            };


                var response = dbLayer.SP_DataTable_return("InsertSalesDetailSaleInvoice", paramArray).Tables[0];
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



        public IEnumerable<pa_ReceiptDetails_DAL> Get_Sales_ReceiptDetails_DAL(int? SaleMaster_ID)
        {

            string sp = "get_Sales_ReceiptDetails";
            List<pa_ReceiptDetails_DAL> itemlist = new List<pa_ReceiptDetails_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleMaster_ID", SaleMaster_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_ReceiptDetails_DAL item = new pa_ReceiptDetails_DAL();

                        item.Total_Amount = rdr["Total_Amount"].ToString();
                        item.Currency_Rate = rdr["Currency_Rate"].ToString();
                        item.Total_Amount_OtherCurrency = rdr["Total_Amount_OtherCurrency"].ToString();
                        item.ReceiptDate = rdr["ReceiptDate"].ToString();
                        item.CR_AccountName = rdr["CR_AccountName"].ToString();

                        item.Receipttype = rdr["Receipttype"].ToString();
                        item.transaction_status = rdr["transaction_status"].ToString();


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


        //Get sales invoice detail list of by chassis type 
        public IEnumerable<pa_SalesDetails> Get_SalesIvoiceDetailListByChassis_DAL(string Temp_ID, int? SaleMaster_ID,int c_ID)
        {



            string sp = "Select_SaleDetailInv_List_ByChassis";
            List<pa_SalesDetails> itemlist = new List<pa_SalesDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@SaleMaster_ID", SaleMaster_ID);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesDetails item = new pa_SalesDetails();

                        item.SalesDetails_ID = Convert.ToInt32(rdr["SalesDetails_ID"]);
                        item.Unit_Price = rdr["Unit_Price"].ToString();
                        item.VATRate = rdr["VATRate"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();
                        item.ItemDesc = rdr["ItemDesc"].ToString();
                        item.SaleInvoiceType = rdr["SaleInvoiceType"].ToString();
                        item.Stock_ID = rdr["Stock_ID"].ToString();
                        item.Chassis_No = rdr["Chassis_No"].ToString();
                        item.Make = rdr["Make"].ToString();
                        item.ModelDesciption = rdr["ModelDesciption"].ToString();
                        item.Color = rdr["Color"].ToString();
                        item.Grand_UnitPrice = rdr["Grand_UnitPrice"].ToString();
                        item.Grand_Amount = rdr["Grand_Amount"].ToString();
                        item.Grand_TotalAmount = rdr["Grand_TotalAmount"].ToString();
                        item.Grand_VATExp = rdr["Grand_VATExp"].ToString();
                        item.Total_Amt_inWords = rdr["Total_Amt_inWords"].ToString();
                        item.Discount = rdr["Discount"].ToString();
                        item.Total_Discount = rdr["Total_Discount"].ToString();
                        item.ModelYear = rdr["ModelYear"].ToString();


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


        //Insert SalesInvoice Master
        public string InsertSalesInvoiceMaster_DAL(int? Customer_ID, string Customer_Contact, string Invoice_address, string Saletype, string SaleDate, string CustomerTRN,
            string Manualbook_ref, int? Seller_ID, int? Agent_ID, int? ExportTo_Destination_ID, int? Port_of_Exit_ID, string Sale_trans_type, int? shipping_co_ID,
            int? ExporterCo_ID, int? Bank_to_Transfer_payment_ID, int? Showroom_ID, string Temp_ID, int? c_ID, int? Created_by, string Remarks, int Currency_ID, string Curr_Rate, string Customer_Name)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Customer_ID",Customer_ID),
                new SqlParameter("@Customer_Contact",Customer_Contact),
                new SqlParameter("@Invoice_address",Invoice_address),
                 new SqlParameter("@Saletype",Saletype),
                  new SqlParameter("@SaleDate",SaleDate),
             new SqlParameter("@CustomerTRN",CustomerTRN),
 new SqlParameter("@Manualbook_ref",Manualbook_ref),
  new SqlParameter("@Seller_ID",Seller_ID),

    new SqlParameter("@Agent_ID",Agent_ID),
 new SqlParameter("@ExportTo_Destination_ID",ExportTo_Destination_ID),
  new SqlParameter("@Port_of_Exit_ID",Port_of_Exit_ID),
    new SqlParameter("@Sale_trans_type",Sale_trans_type),

                  new SqlParameter("@shipping_co_ID",shipping_co_ID),
            new SqlParameter("@ExporterCo_ID",ExporterCo_ID),
            new SqlParameter("@Bank_to_Transfer_payment_ID",Bank_to_Transfer_payment_ID),
               new SqlParameter("@Showroom_ID",Showroom_ID),

                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Created_by",Created_by),
                 new SqlParameter("@Remarks",Remarks),
                   new SqlParameter("@Currency_ID",Currency_ID),
                     new SqlParameter("@Curr_Rate",Curr_Rate),
                     new SqlParameter("@Customer_Name",Customer_Name)



            };


                var response = dbLayer.SP_DataTable_return("Insert_SalesMaster_SaleInvoice", paramArray).Tables[0];
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



        //Udpate SalesInvoice Master
        public string UpdateSalesInvoiceMaster_DAL(int? SaleMaster_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string CustomerTRN,
            string Manualbook_ref, int? Seller_ID, int? Agent_ID, int? ExportTo_Destination_ID, int? Port_of_Exit_ID, string Sale_trans_type, int? shipping_co_ID,
            int? ExporterCo_ID, int? Bank_to_Transfer_payment_ID, int? Showroom_ID, string Temp_ID, int? c_ID, int? Modified_by, string Remarks, int Currency_ID, string Curr_Rate,string Customer_Name)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@SaleMaster_ID",SaleMaster_ID),
                new SqlParameter("@Customer_ID",Customer_ID),
                new SqlParameter("@Customer_Contact",Customer_Contact),
                new SqlParameter("@Invoice_address",Invoice_address),

                  new SqlParameter("@SaleDate",SaleDate),

                    new SqlParameter("@CustomerTRN",CustomerTRN),
 new SqlParameter("@Manualbook_ref",Manualbook_ref),
  new SqlParameter("@Seller_ID",Seller_ID),

    new SqlParameter("@Agent_ID",Agent_ID),
 new SqlParameter("@ExportTo_Destination_ID",ExportTo_Destination_ID),
  new SqlParameter("@Port_of_Exit_ID",Port_of_Exit_ID),
    new SqlParameter("@Sale_trans_type",Sale_trans_type),

                  new SqlParameter("@shipping_co_ID",shipping_co_ID),
            new SqlParameter("@ExporterCo_ID",ExporterCo_ID),
            new SqlParameter("@Bank_to_Transfer_payment_ID",Bank_to_Transfer_payment_ID),
            new SqlParameter("@Showroom_ID",Showroom_ID),

                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Modified_by",Modified_by),
                 new SqlParameter("@Remarks",Remarks),
                   new SqlParameter("@Currency_ID",Currency_ID),
                     new SqlParameter("@Curr_Rate",Curr_Rate),
                     new SqlParameter("@Customer_Name",Customer_Name)





            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_SalesMaster_SaleInvoice", paramArray).Tables[0];
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


        // Get Old TempID From sales detail table by SaleMaster_ID 
        public string GetOldTempIDFromSalesDetail_DAL(int? SaleMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@SaleMaster_ID",SaleMaster_ID)

            };


                var response = dbLayer.SP_DataTable_return("SelectSalesDetailOldTempID", paramArray).Tables[0];
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

        //--getting sales invoice master by id
        public pa_SalesMaster_DAL Get_SalesInvoiceMasterByID_DAL(int? SaleMaster_ID,int c_ID)
        {

            string sp = "Select_SalesMasterSaleInvoice_ByID";

            pa_SalesMaster_DAL pm = new pa_SalesMaster_DAL();
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
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.SaleMaster_ID = Convert.ToInt32(dr["SaleMaster_ID"]);
                                pm.Customer_ID = dr["Customer_ID"].ToString();
                                pm.SaleRef = dr["SaleRef"].ToString();
                                pm.Customer_Contact = dr["Customer_Contact"].ToString();
                                pm.Invoice_address = dr["Invoice_address"].ToString();
                                pm.SaleDate = dr["SaleDate"].ToString();
                                pm.SaleStatus = dr["SaleStatus"].ToString();


                                pm.CustomerTRN = dr["CustomerTRN"].ToString();
                                pm.Manualbook_ref = dr["Manualbook_ref"].ToString();
                                pm.Seller_ID = dr["Seller_ID"].ToString();
                                pm.Agent_ID = dr["Agent_ID"].ToString();
                                pm.ExportTo_Destination_ID = dr["ExportTo_Destination_ID"].ToString();
                                pm.Port_of_Exit_ID = dr["Port_of_Exit_ID"].ToString();
                                pm.Sale_trans_type = dr["Sale_trans_type"].ToString();

                                pm.shipping_co_ID = dr["shipping_co_ID"].ToString();
                                pm.ExporterCo_ID = dr["ExporterCo_ID"].ToString();
                                pm.Bank_to_Transfer_payment_ID = dr["Bank_to_Transfer_payment_ID"].ToString();
                                pm.Showroom_ID = dr["Showroom_ID"].ToString();

                                pm.Created_at = dr["Created_at"].ToString();
                                pm.Created_by = dr["Created_by"].ToString();
                                pm.Modified_at = dr["Modified_at"].ToString();
                                pm.Modified_by = dr["Modified_by"].ToString();
                                pm.Remarks = dr["Remarks"].ToString();
                                pm.Currency_ID = Convert.ToInt16(dr["Currency_ID"]);
                                pm.Curr_Rate = dr["Curr_Rate"].ToString();
                                pm.ApprovedBy = Convert.ToInt32(dr["ApprovedBy"]);
                                //pm.SaleStatus_ID = dr["Status_ID"].ToString();
                                pm.CustomerName = dr["Customer_Name"].ToString();

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

        public pa_SalesMaster_DAL Get_SalesInvoiceMasterBalance(int? SaleMaster_ID,int c_ID)
        {

            string sp = "Select_SalesBalance";

            pa_SalesMaster_DAL pm = new pa_SalesMaster_DAL();
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
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.Paid_amt = dr["Paid_amt"].ToString();
                                pm.Bal_due = dr["Bal_due"].ToString();

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
        public IPagedList<pa_vehicle_display_master> VehicleDisplayMasterList { get; set; }
        public IEnumerable<pa_vehicle_display_details> VehicleDisplayDetailList { get; set; }
        public pa_vehicle_display_master VehicleDisplayMasterObj { get; set; }
        public IEnumerable<pa_Select_SalesDashboardList> SalesDashboardList { get ; set; }
        public IPagedList<pa_Select_SalesDashboardList> SalesDashboardIPagedList { get ; set; }
        public IEnumerable<pa_Select_SalesDashboardList> SelectSalesDetails { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


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
        public pa_vehicle_display_master Get_MaxRef()
        {

            string sp = "select_max_vehicleDisplay_ID";

            pa_vehicle_display_master pm = new pa_vehicle_display_master();
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

                                pm.Ref = dr["Ref"].ToString();

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

        public IEnumerable<Pa_EmpMaster_DAL> Get_Emp_Name_DAL(int c_ID)
        {
            string sp = "pa_Get_Emp_Main";
            List<Pa_EmpMaster_DAL> itemlist = new List<Pa_EmpMaster_DAL>();

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
                        Pa_EmpMaster_DAL item = new Pa_EmpMaster_DAL();

                        item.emp_ID = Convert.ToInt32(rdr["emp_ID"]);
                        item.emp_Name = rdr["emp_Name"].ToString();


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






        //Update New Tasks


        //Delete Sales Master
        public string DeleteSalesMaster_DAL(int? SaleMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@SaleMaster_ID",SaleMaster_ID)



            };


                var response = dbLayer.SP_DataTable_return("Pa_DeleteSalesMaster", paramArray).Tables[0];
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


        //Delete Sales detail
        public string DeleteSalesDetail_DAL(int SalesDetails_ID,int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@SalesDetails_ID",SalesDetails_ID),
                new SqlParameter("@c_ID",c_ID)


            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_SalesDetail", paramArray).Tables[0];
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

        //Delete Sales detail
        ////add Perameter C_id by waiz for deleting Performa Invoice chassis Data/////
        public string DeleteSalesDetail_Performa_DAL(int? SalesDetails_ID, int? c_id)
        {
            
            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@SalesDetails_ID",SalesDetails_ID),
                new SqlParameter("@c_ID",c_id)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_SalesDetail_Performa", paramArray).Tables[0];
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
        //Delete Sales detail
        public string DeleteSalesDetail_Specs_DAL(int? SalesDetails_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@SalesDetails_ID",SalesDetails_ID)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_SalesDetail_Specs", paramArray).Tables[0];
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
        //---Update into sales invoice detial general
        public string UpdateSalesInvoiceDetail_DAL(int? SalesDetails_ID, string Chassis, string ItemDesc, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by, double? Discount)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
               new SqlParameter("@SalesDetails_ID",SalesDetails_ID),
                         new SqlParameter("@Chassis_No",Chassis),
                            new SqlParameter("@ItemDesc",ItemDesc),
                           new SqlParameter("@Unit_Price",Unit_Price),
                            new SqlParameter("@VATRate",VATRate),
                             new SqlParameter("@VATExp",VATExp),
                              new SqlParameter("@Total_Amount",Total_Amount),
                               new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@c_ID",c_ID),
                                 new SqlParameter("@Modified_by",Modified_by),
                                 new SqlParameter("@Discount",Discount),
                                 



            };


                var response = dbLayer.SP_DataTable_return("Pa_UpdateSalesDetailSaleInvoice", paramArray).Tables[0];
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





        //Get sales master invoice list
        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMasterInvoiceList_DAL(string SaleRef, string ManualRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int Status_ID,int c_ID,int Make,int Model_Desc,string Model_Year)

        {

            string sp = "Select_SalesMasterInvoiceList";
            List<pa_SalesMaster_DAL> itemlist = new List<pa_SalesMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                    cmd.Parameters.AddWithValue("@ManualRef", ManualRef);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                    cmd.Parameters.AddWithValue("@Status_ID", Status_ID);

                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    cmd.Parameters.AddWithValue("@Make_Id", Make);
                    cmd.Parameters.AddWithValue("@Model_Desc_Id", Model_Desc);
                    cmd.Parameters.AddWithValue("@Model_Year", Model_Year);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesMaster_DAL item = new pa_SalesMaster_DAL();
                        //VATExp    Total_Amt    Paid_amt   Bal_due   SaleStatus   Modified_at   CustomerName   


                        item.SaleMaster_ID = Convert.ToInt32(rdr["SaleMaster_ID"]);
                        item.SaleRef = rdr["SaleRef"].ToString();
                        item.SaleDate = rdr["SaleDate"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.Discount = rdr["Discount"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amt = rdr["Total_Amt"].ToString();
                        item.Paid_amt = rdr["Paid_amt"].ToString();
                        item.Bal_due = rdr["Bal_due"].ToString();
                        item.Customer_Name = rdr["Customer_Name"].ToString();
                        item.SaleStatus = rdr["Status"].ToString();
                        item.Status = rdr["Status"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.ApprovalStatus = rdr["Approval_Status"].ToString();
                        item.Remarks = rdr["Remarks"].ToString();
                        item.Customer_ID = rdr["Customer_ID"].ToString();
                        item.CarCount= Convert.ToInt32(rdr["CarCount"].ToString());


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

        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMasterInvoiceList_TTL_DAL(string SaleRef, string ManualRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int Status_ID,
           int c_ID,int Make,int Model_Desc,string Model_Year)

        {

            string sp = "Select_SalesMasterInvoiceList_TTL";
            List<pa_SalesMaster_DAL> itemlist = new List<pa_SalesMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                    cmd.Parameters.AddWithValue("@ManualRef", ManualRef);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                    cmd.Parameters.AddWithValue("@Status_ID", Status_ID);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    cmd.Parameters.AddWithValue("@Make_Id", Make);
                    cmd.Parameters.AddWithValue("@Model_Desc_Id", Model_Desc);
                    cmd.Parameters.AddWithValue("@Model_Year", Model_Year);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesMaster_DAL item = new pa_SalesMaster_DAL();

                        item.Total_Detail_Amount = rdr["Total_Detail_Amount"].ToString();
                        item.Total_VATExp = rdr["Total_VATExp"].ToString();
                        item.Total_Master_Amount = rdr["Total_Master_Amount"].ToString();
                        item.Total_Paid = rdr["Total_Paid"].ToString();
                        item.Total_Bal_Due = rdr["Total_Bal_Due"].ToString();

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


        //Get sales Booking List
        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMasterBooking_List_DAL(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int Status_ID,
            int c_ID)

        {

            string sp = "Select_SalesMasterBooking_List";
            List<pa_SalesMaster_DAL> itemlist = new List<pa_SalesMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                    cmd.Parameters.AddWithValue("@Status_ID", Status_ID);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesMaster_DAL item = new pa_SalesMaster_DAL();
                        //VATExp    Total_Amt    Paid_amt   Bal_due   SaleStatus   Modified_at   CustomerName   


                        item.SaleMaster_ID = Convert.ToInt32(rdr["SaleMaster_ID"]);
                        item.SaleRef = rdr["SaleRef"].ToString();
                        item.SaleDate = rdr["SaleDate"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amt = rdr["Total_Amt"].ToString();
                        item.Paid_amt = rdr["Paid_amt"].ToString();
                        item.Bal_due = rdr["Bal_due"].ToString();
                        item.Customer_Name = rdr["Customer_Name"].ToString();
                        item.SaleStatus = rdr["Status"].ToString();
                        item.Status = rdr["Status"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();

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

        // Get performa Invoice (Sales) List
        public IEnumerable<pa_SalesMaster_DAL> Get_PerformaInvoice_List_DAL(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int c_ID)

        {

            string sp = "Select_PerformaInvoice_List";
            List<pa_SalesMaster_DAL> itemlist = new List<pa_SalesMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesMaster_DAL item = new pa_SalesMaster_DAL();
                        //VATExp    Total_Amt    Paid_amt   Bal_due   SaleStatus   Modified_at   CustomerName   


                        item.SaleMaster_ID = Convert.ToInt32(rdr["SaleMaster_ID"]);
                        item.SaleRef = rdr["SaleRef"].ToString();
                        item.SaleDate = rdr["SaleDate"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amt = rdr["Total_Amt"].ToString();
                        item.Customer_Name = rdr["Customer_Name"].ToString();
                        item.SaleStatus = rdr["Status"].ToString();
                        item.Status = rdr["Status"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();

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


        public IEnumerable<pa_SalesMaster_DAL> Get_SalesInvoiceReturnList_DAL(string SaleRef, string StartDate, string EndDate, string Customer_Name, int c_ID)

        {

            string sp = "Select_SalesInvoiceReturnList";
            List<pa_SalesMaster_DAL> itemlist = new List<pa_SalesMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesMaster_DAL item = new pa_SalesMaster_DAL();
                        //VATExp    Total_Amt    Paid_amt   Bal_due   SaleStatus   Modified_at   CustomerName   


                        item.SaleMaster_ID = Convert.ToInt32(rdr["SaleMaster_ID"]);
                        item.SaleRef = rdr["SaleRef"].ToString();
                        item.SaleDate = rdr["SaleDate"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amt = rdr["Total_Amt"].ToString();
                        item.Paid_amt = rdr["Return_amt"].ToString();
                        item.Bal_due = rdr["Bal_due"].ToString();
                        item.Customer_Name = rdr["Customer_Name"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
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



        //---insert into sales booking detial general
        public string InsertSalesBookingDetail_DAL(string Chassis, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int hdSaleMaster_ID, double? Discount)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                         new SqlParameter("@Chassis",Chassis),

                           new SqlParameter("@Unit_Price",Unit_Price),
                            new SqlParameter("@VATRate",VATRate),
                             new SqlParameter("@VATExp",VATExp),
                              new SqlParameter("@Total_Amount",Total_Amount),

                               new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@c_ID",c_ID),
                                 new SqlParameter("@Created_by",Created_by),
                                 new SqlParameter("@Discount",Discount),
                                 new SqlParameter("@hdSaleMaster_ID",hdSaleMaster_ID)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Insert_SalesDetail_Booking", paramArray).Tables[0];
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
        public string UpdateSalesBookingDetail_DAL(int? SalesDetails_ID, string Chassis, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by, double? Discount)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@SalesDetails_ID",SalesDetails_ID),
                         new SqlParameter("@Chassis_No",Chassis),

                           new SqlParameter("@Unit_Price",Unit_Price),
                            new SqlParameter("@VATRate",VATRate),
                             new SqlParameter("@VATExp",VATExp),
                              new SqlParameter("@Total_Amount",Total_Amount),

                               new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@c_ID",c_ID),
                                new SqlParameter("@Discount",Discount),
                                 new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_SalesDetail_Booking", paramArray).Tables[0];
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
        public string InsertSalesBookingMaster_DAL(int? Customer_ID, string Customer_Contact, string Invoice_address, string Saletype, string SaleDate, string CustomerTRN, string Manualbook_ref,
            int? Seller_ID, int? Agent_ID, int? ExportTo_Destination_ID, int? Port_of_Exit_ID, string Sale_trans_type, int? shipping_co_ID, int? ExporterCo_ID, int? Bank_to_Transfer_payment_ID, int? Showroom_ID, string Temp_ID, int? c_ID, int? Created_by, string Remarks, int Currency_ID, string Currency_Rate)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Customer_ID",Customer_ID),
                new SqlParameter("@Customer_Contact",Customer_Contact),
                new SqlParameter("@Invoice_address",Invoice_address),
                 new SqlParameter("@Saletype",Saletype),
                  new SqlParameter("@SaleDate",SaleDate),

                            new SqlParameter("@CustomerTRN",CustomerTRN),
            new SqlParameter("@Manualbook_ref",Manualbook_ref),
            new SqlParameter("@Seller_ID",Seller_ID),

            new SqlParameter("@Agent_ID",Agent_ID),
            new SqlParameter("@ExportTo_Destination_ID",ExportTo_Destination_ID),
            new SqlParameter("@Port_of_Exit_ID",Port_of_Exit_ID),
            new SqlParameter("@Sale_trans_type",Sale_trans_type),

              new SqlParameter("@shipping_co_ID",shipping_co_ID),
            new SqlParameter("@ExporterCo_ID",ExporterCo_ID),
            new SqlParameter("@Bank_to_Transfer_payment_ID",Bank_to_Transfer_payment_ID),
                new SqlParameter("@Showroom_ID",Showroom_ID),

                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),
                 new SqlParameter("@Remarks",Remarks),
                 new SqlParameter("@Currency_ID",Currency_ID),
                 new SqlParameter("@Currency_Rate",Currency_Rate),

                 new SqlParameter("@Created_by",Created_by)




            };


                var response = dbLayer.SP_DataTable_return("Pa_Insert_SalesMaster_Booking", paramArray).Tables[0];
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


        //Udpate Sales booking Master
        public string UpdateSalesBookingMaster_DAL(int? SaleMaster_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string CustomerTRN, string Manualbook_ref, int? Seller_ID, int? Agent_ID, int? ExportTo_Destination_ID,
            int? Port_of_Exit_ID, string Sale_trans_type, int? shipping_co_ID, int? ExporterCo_ID, int? Bank_to_Transfer_payment_ID, int? Showroom_ID, string Temp_ID, int? c_ID, int? Modified_by, string Remarks, int Currency_ID, string Currency_Rate)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@SaleMaster_ID",SaleMaster_ID),
                new SqlParameter("@Customer_ID",Customer_ID),
                new SqlParameter("@Customer_Contact",Customer_Contact),
                new SqlParameter("@Invoice_address",Invoice_address),

                  new SqlParameter("@SaleDate",SaleDate),


                   new SqlParameter("@CustomerTRN",CustomerTRN),
 new SqlParameter("@Manualbook_ref",Manualbook_ref),
  new SqlParameter("@Seller_ID",Seller_ID),

    new SqlParameter("@Agent_ID",Agent_ID),
 new SqlParameter("@ExportTo_Destination_ID",ExportTo_Destination_ID),
  new SqlParameter("@Port_of_Exit_ID",Port_of_Exit_ID),
    new SqlParameter("@Sale_trans_type",Sale_trans_type),

       new SqlParameter("@shipping_co_ID",shipping_co_ID),
            new SqlParameter("@ExporterCo_ID",ExporterCo_ID),
            new SqlParameter("@Bank_to_Transfer_payment_ID",Bank_to_Transfer_payment_ID),
                new SqlParameter("@Showroom_ID",Showroom_ID),
                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),
                           new SqlParameter("@Remarks",Remarks),
                 new SqlParameter("@Currency_ID",Currency_ID),
                 new SqlParameter("@Currency_Rate",Currency_Rate),
                 new SqlParameter("@Modified_by",Modified_by)




            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_SalesMaster_Booking", paramArray).Tables[0];
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

        //--getting sales booking master by id
        public pa_SalesMaster_DAL Get_SalesBookingMasterByID_DAL(int? SaleMaster_ID)
        {

            string sp = "Pa_Select_SalesMasterBooking_ByID";

            pa_SalesMaster_DAL pm = new pa_SalesMaster_DAL();
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
                                pm.SaleMaster_ID = Convert.ToInt32(dr["SaleMaster_ID"]);
                                pm.Customer_ID = dr["Customer_ID"].ToString();
                                pm.SaleRef = dr["SaleRef"].ToString();
                                pm.Customer_Contact = dr["Customer_Contact"].ToString();
                                pm.Invoice_address = dr["Invoice_address"].ToString();
                                pm.SaleDate = dr["SaleDate"].ToString();
                                pm.SaleStatus = dr["SaleStatus"].ToString();

                                pm.CustomerTRN = dr["CustomerTRN"].ToString();
                                pm.Manualbook_ref = dr["Manualbook_ref"].ToString();
                                pm.Seller_ID = dr["Seller_ID"].ToString();
                                pm.Agent_ID = dr["Agent_ID"].ToString();
                                pm.ExportTo_Destination_ID = dr["ExportTo_Destination_ID"].ToString();
                                pm.Port_of_Exit_ID = dr["Port_of_Exit_ID"].ToString();
                                pm.Sale_trans_type = dr["Sale_trans_type"].ToString();

                                pm.shipping_co_ID = dr["shipping_co_ID"].ToString();
                                pm.ExporterCo_ID = dr["ExporterCo_ID"].ToString();
                                pm.Bank_to_Transfer_payment_ID = dr["Bank_to_Transfer_payment_ID"].ToString();
                                pm.Showroom_ID = dr["Showroom_ID"].ToString();


                                pm.Created_at = dr["Created_at"].ToString();
                                pm.Created_by = dr["Created_by"].ToString();
                                pm.Modified_at = dr["Modified_at"].ToString();
                                pm.Modified_by = dr["Modified_by"].ToString();
                                pm.Remarks = dr["Remarks"].ToString();
                                pm.Currency_ID = Convert.ToInt32(dr["Currency_ID"]);
                                pm.Curr_Rate = dr["Curr_Rate"].ToString();


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


        //Get sales booking detail list by id
        public IEnumerable<pa_SalesDetails> Get_SalesBookingDetailListByID_DAL(string Temp_ID, int? SaleMaster_ID)
        {



            string sp = "Select_SaleDetailBooking_List";
            List<pa_SalesDetails> itemlist = new List<pa_SalesDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@SaleMaster_ID", SaleMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesDetails item = new pa_SalesDetails();

                        item.SalesDetails_ID = Convert.ToInt32(rdr["SalesDetails_ID"]);

                        item.Unit_Price = rdr["Unit_Price"].ToString();

                        item.Stock_ID = rdr["Stock_ID"].ToString();
                        item.Chassis_No = rdr["Chassis_No"].ToString();
                        item.Make = rdr["Make"].ToString();
                        item.ModelDesciption = rdr["ModelDesciption"].ToString();
                        item.Color = rdr["Color"].ToString();
                        item.Amount = rdr["Amount"].ToString();

                        item.VATRate = rdr["VATRate"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();

                        item.Grand_UnitPrice = rdr["Grand_UnitPrice"].ToString();
                        item.Grand_Amount = rdr["Grand_Amount"].ToString();
                        item.Grand_TotalAmount = rdr["Grand_TotalAmount"].ToString();
                        item.Grand_VATExp = rdr["Grand_VATExp"].ToString();
                        item.Total_Amt_inWords = rdr["Total_Amt_inWords"].ToString();
                        item.Discount = rdr["Discount"].ToString();


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


        //---Convert Sales Booking by Chassis_DAL
        public string Convert_SalesBooking_by_Chassis_DAL(int? SaleMaster_ID, string SelectedChassis, int? Created_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
               new SqlParameter("@Booking_SaleMaster_ID",SaleMaster_ID),
                         new SqlParameter("@chassis_no_str",SelectedChassis),

                                 new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Convert_SalesBooking_by_Chassis", paramArray).Tables[0];
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


        //---Convert Sales Booking by Chassis_DAL
        public string convert_Salebooking_By_SalesMasterID_DAL(int? SaleMaster_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
               new SqlParameter("@SaleMaster_ID",SaleMaster_ID)



            };


                var response = dbLayer.SP_DataTable_return("convert_Salebooking_By_SalesMasterID", paramArray).Tables[0];
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

        //--getting sales performa master by id
        public pa_SalesMaster_DAL Get_SalesPerformaMasterByID_DAL(int? SaleMaster_ID)
        {

            string sp = "Select_SalesMasterPerformaInvoice_ByID";

            pa_SalesMaster_DAL pm = new pa_SalesMaster_DAL();
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
                                pm.SaleMaster_ID = Convert.ToInt32(dr["SaleMaster_ID"]);
                                pm.Customer_ID = dr["Customer_ID"].ToString();
                                pm.SaleRef = dr["SaleRef"].ToString();
                                pm.Customer_Contact = dr["Customer_Contact"].ToString();
                                pm.Invoice_address = dr["Invoice_address"].ToString();
                                pm.SaleDate = dr["SaleDate"].ToString();
                                pm.SaleStatus = dr["SaleStatus"].ToString();


                                pm.CustomerTRN = dr["CustomerTRN"].ToString();
                                pm.Manualbook_ref = dr["Manualbook_ref"].ToString();
                                pm.Seller_ID = dr["Seller_ID"].ToString();
                                pm.Agent_ID = dr["Agent_ID"].ToString();
                                pm.ExportTo_Destination_ID = dr["ExportTo_Destination_ID"].ToString();
                                pm.Port_of_Exit_ID = dr["Port_of_Exit_ID"].ToString();
                                pm.Sale_trans_type = dr["Sale_trans_type"].ToString();

                                pm.shipping_co_ID = dr["shipping_co_ID"].ToString();
                                pm.ExporterCo_ID = dr["ExporterCo_ID"].ToString();
                                pm.Bank_to_Transfer_payment_ID = dr["Bank_to_Transfer_payment_ID"].ToString();


                                pm.Showroom_ID = dr["Showroom_ID"].ToString();
                                pm.Performa_Validity = dr["Performa_Validity"].ToString();

                                pm.Currency_ID = Convert.ToInt32(dr["Currency_ID"]);
                                pm.Curr_Rate = dr["Curr_Rate"].ToString();


                                pm.Created_at = dr["Created_at"].ToString();
                                pm.Created_by = dr["Created_by"].ToString();
                                pm.Modified_at = dr["Modified_at"].ToString();
                                pm.Modified_by = dr["Modified_by"].ToString();
                                pm.ApprovedBy = Convert.ToInt32(dr["ApprovedBy"]);


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

        //Get sales performa detail list of by chassis type 
        public IEnumerable<pa_SalesDetails> Get_SalesPerformaDetailListByChassis_DAL(string Temp_ID, int? SaleMaster_ID)
        {



            string sp = "Select_SaleDetailPerforma_List_ByChassis";
            List<pa_SalesDetails> itemlist = new List<pa_SalesDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@SaleMaster_ID", SaleMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesDetails item = new pa_SalesDetails();

                        item.SalesDetails_ID = Convert.ToInt32(rdr["SalesDetails_ID"]);

                        item.Unit_Price = rdr["Unit_Price"].ToString();

                        item.ItemDesc = rdr["ItemDesc"].ToString();
                        item.ItemCode = rdr["ItemId"].ToString();

                        item.VATRate = rdr["VATRate"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();

                        item.SaleInvoiceType = rdr["SaleInvoiceType"].ToString();
                        item.Stock_ID = rdr["Stock_ID"].ToString();
                        item.Chassis_No = rdr["Chassis_No"].ToString();
                        item.Make = rdr["Make"].ToString();
                        item.ModelDesciption = rdr["ModelDesciption"].ToString();
                        item.Color = rdr["Color"].ToString();

                        item.Grand_UnitPrice = rdr["Grand_UnitPrice"].ToString();
                        item.Grand_Amount = rdr["Grand_Amount"].ToString();
                        item.Grand_TotalAmount = rdr["Grand_TotalAmount"].ToString();
                        item.Grand_VATExp = rdr["Grand_VATExp"].ToString();
                        item.Total_Amt_inWords = rdr["Total_Amt_inWords"].ToString();



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

        //Get sales performa detail list of by Specs type 
        public IEnumerable<pa_SalesDetails> Get_SalesPerformaDetailListByChassis_Specs_DAL(string Temp_ID, int? SaleMaster_ID)
        {



            string sp = "Select_SaleDetailPerforma_List_BySpecs";
            List<pa_SalesDetails> itemlist = new List<pa_SalesDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@SaleMaster_ID", SaleMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesDetails item = new pa_SalesDetails();

                        item.SalesDetails_ID = Convert.ToInt32(rdr["SalesDetails_ID"]);

                        item.Unit_Price = rdr["Unit_Price"].ToString();

                        item.ItemDesc = rdr["ItemDesc"].ToString();
                        item.ItemCode = rdr["ItemId"].ToString();

                        item.VATRate = rdr["VATRate"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();

                        item.SaleInvoiceType = rdr["SaleInvoiceType"].ToString();
                        item.Stock_ID = rdr["Stock_ID"].ToString();
                        item.Chassis_No = rdr["Chassis_No"].ToString();
                        item.Make = rdr["Make"].ToString();
                        item.ModelDesciption = rdr["ModelDesciption"].ToString();
                        item.Color = rdr["Color"].ToString();

                        item.Grand_UnitPrice = rdr["Grand_UnitPrice"].ToString();
                        item.Grand_Amount = rdr["Grand_Amount"].ToString();
                        item.Grand_TotalAmount = rdr["Grand_TotalAmount"].ToString();
                        item.Grand_VATExp = rdr["Grand_VATExp"].ToString();
                        item.Total_Amt_inWords = rdr["Total_Amt_inWords"].ToString();
                        item.Quantity = rdr["Quantity"].ToString();



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

        //---insert into sales performa detial general
        public string InsertSalesPerformaDetail_DAL(string SalesInvoiceType, string Chassis, string ItemDesc, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int hdSaleMaster_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
               new SqlParameter("@SalesInvoiceType",SalesInvoiceType),
                         new SqlParameter("@Chassis",Chassis),
                          new SqlParameter("@ItemDesc",ItemDesc),
                           new SqlParameter("@Unit_Price",Unit_Price),
                             new SqlParameter("@VATRate",VATRate),
                             new SqlParameter("@VATExp",VATExp),
                              new SqlParameter("@Total_Amount",Total_Amount),

                               new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@c_ID",c_ID),
                                 new SqlParameter("@Created_by",Created_by),
                                 new SqlParameter("@hdSaleMaster_ID",hdSaleMaster_ID)



            };


                var response = dbLayer.SP_DataTable_return("InsertSalesDetail_Performa", paramArray).Tables[0];
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

        //---insert into sales performa detial general
        public string InsertSalesPerformaDetail_Specs_DAL(string SalesInvoiceType, string Chassis, string ItemDesc, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, double? Quantity, int hdSaleMaster_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
               new SqlParameter("@SalesInvoiceType",SalesInvoiceType),
                         new SqlParameter("@Chassis",Chassis),
                          new SqlParameter("@ItemDesc",ItemDesc),
                           new SqlParameter("@Unit_Price",Unit_Price),
                             new SqlParameter("@VATRate",VATRate),
                             new SqlParameter("@VATExp",VATExp),
                              new SqlParameter("@Total_Amount",Total_Amount),

                               new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@c_ID",c_ID),
                                 new SqlParameter("@Created_by",Created_by),
                                 new SqlParameter("@Quantity",Quantity),
                                 new SqlParameter("@hdSaleMaster_ID",hdSaleMaster_ID)



            };


                var response = dbLayer.SP_DataTable_return("InsertSalesDetail_Performa_Specs", paramArray).Tables[0];
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



        //Insert sale performa Master
        public string InsertSalesPerformaMaster_DAL(int? Customer_ID, string Customer_Contact, string Invoice_address, string Saletype, string SaleDate, string CustomerTRN,
            string Manualbook_ref, int? Seller_ID, int? Agent_ID, int? ExportTo_Destination_ID, int? Port_of_Exit_ID, string Sale_trans_type,
            int? shipping_co_ID, int? ExporterCo_ID, int? Bank_to_Transfer_payment_ID, int? Showroom_ID, string Performa_Validity, string Temp_ID, int? c_ID, int? Created_by, int Currency_ID, string Currency_Rate)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Customer_ID",Customer_ID),
                new SqlParameter("@Customer_Contact",Customer_Contact),
                new SqlParameter("@Invoice_address",Invoice_address),
                new SqlParameter("@Saletype",Saletype),
                new SqlParameter("@SaleDate",SaleDate),
                new SqlParameter("@CustomerTRN",CustomerTRN),
                new SqlParameter("@Manualbook_ref",Manualbook_ref),
                new SqlParameter("@Seller_ID",Seller_ID),
                new SqlParameter("@Agent_ID",Agent_ID),
                new SqlParameter("@ExportTo_Destination_ID",ExportTo_Destination_ID),
                new SqlParameter("@Port_of_Exit_ID",Port_of_Exit_ID),
                new SqlParameter("@Sale_trans_type",Sale_trans_type),
                new SqlParameter("@shipping_co_ID",shipping_co_ID),
                new SqlParameter("@ExporterCo_ID",ExporterCo_ID),
                new SqlParameter("@Bank_to_Transfer_payment_ID",Bank_to_Transfer_payment_ID),
                new SqlParameter("@Showroom_ID",Showroom_ID),
                new SqlParameter("@Performa_Validity",Performa_Validity),
                new SqlParameter("@Temp_ID",Temp_ID),
                new SqlParameter("@c_ID",c_ID),
                new SqlParameter("@Created_by",Created_by),
                new SqlParameter("@Currency_ID",Currency_ID),
                new SqlParameter("@Curr_Rate",Currency_Rate)



            };


                var response = dbLayer.SP_DataTable_return("Insert_SalesMaster_PerformaInv", paramArray).Tables[0];
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

        //Udpate Sales performa Master
        public string UpdateSalesPerformaMaster_DAL(int? SaleMaster_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string CustomerTRN,
            string Manualbook_ref, int? Seller_ID, int? Agent_ID, int? ExportTo_Destination_ID, int? Port_of_Exit_ID, string Sale_trans_type,
            int? shipping_co_ID, int? ExporterCo_ID, int? Bank_to_Transfer_payment_ID, int? Showroom_ID, string Performa_Validity, string Temp_ID, int? c_ID, int?
            Modified_by, int Currency_ID, string Currency_Rate)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@SaleMaster_ID",SaleMaster_ID),
                new SqlParameter("@Customer_ID",Customer_ID),
                new SqlParameter("@Customer_Contact",Customer_Contact),
                new SqlParameter("@Invoice_address",Invoice_address),

                  new SqlParameter("@SaleDate",SaleDate),

                   new SqlParameter("@CustomerTRN",CustomerTRN),
 new SqlParameter("@Manualbook_ref",Manualbook_ref),
  new SqlParameter("@Seller_ID",Seller_ID),

    new SqlParameter("@Agent_ID",Agent_ID),
 new SqlParameter("@ExportTo_Destination_ID",ExportTo_Destination_ID),
  new SqlParameter("@Port_of_Exit_ID",Port_of_Exit_ID),
    new SqlParameter("@Sale_trans_type",Sale_trans_type),

           new SqlParameter("@shipping_co_ID",shipping_co_ID),
            new SqlParameter("@ExporterCo_ID",ExporterCo_ID),
            new SqlParameter("@Bank_to_Transfer_payment_ID",Bank_to_Transfer_payment_ID),
                          new SqlParameter("@Showroom_ID",Showroom_ID),
            new SqlParameter("@Performa_Validity",Performa_Validity),


                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Modified_by",Modified_by),
                 new SqlParameter("@Currency_ID",Currency_ID),
                 new SqlParameter("@Curr_Rate",Currency_Rate)




            };


                var response = dbLayer.SP_DataTable_return("Update_SalesMaster_PerformaInv", paramArray).Tables[0];
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


        //---Update into sales Performa detial general
        public string UpdateSalesPerformaDetail_DAL(int? SalesDetails_ID, string Chassis, string ItemDesc, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
               new SqlParameter("@SalesDetails_ID",SalesDetails_ID),
                         new SqlParameter("@Chassis_No",Chassis),
                            new SqlParameter("@ItemDesc",ItemDesc),
                           new SqlParameter("@Unit_Price",Unit_Price),

                             new SqlParameter("@VATRate",VATRate),
 new SqlParameter("@VATExp",VATExp),
  new SqlParameter("@Total_Amount",Total_Amount),



                               new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@c_ID",c_ID),
                                 new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("UpdateSalesDetailPerformaInv", paramArray).Tables[0];
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

        //---Update into sales Performa detial general
        public string UpdateSalesPerformaDetail_Specs_DAL(int? SalesDetails_ID, string Chassis, string ItemDesc, double? Unit_Price, double? VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by, double? Quantity)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
               new SqlParameter("@SalesDetails_ID",SalesDetails_ID),
                         new SqlParameter("@Chassis_No",Chassis),
                            new SqlParameter("@ItemDesc",ItemDesc),
                           new SqlParameter("@Unit_Price",Unit_Price),

                             new SqlParameter("@VATRate",VATRate),
 new SqlParameter("@VATExp",VATExp),
  new SqlParameter("@Total_Amount",Total_Amount),



                               new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@c_ID",c_ID),
                                new SqlParameter("@Quantity",Quantity),
                                 new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("UpdateSalesDetailPerformaInv_Specs", paramArray).Tables[0];
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

        //---insert sales detail cancel/return
        public string InsertSalesDetails_CancelReturn(string Temp_ID, string SaleRef)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {


                               new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@SaleRef",SaleRef)



            };


                var response = dbLayer.SP_DataTable_return("InsertSalesDetails_CancelReturn", paramArray).Tables[0];
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


        public IEnumerable<pa_SalesDetails> Get_SaleSummaryDescription_DAL(int SaleMaster_ID, int c_ID)

        {

            string sp = "Get_SaleSummaryDescription";
            List<pa_SalesDetails> itemlist = new List<pa_SalesDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleMaster_ID", SaleMaster_ID);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesDetails item = new pa_SalesDetails();
                        //VATExp    Total_Amt    Paid_amt   Bal_due   SaleStatus   Modified_at   CustomerName   


                        item.SaleMaster_ID = Convert.ToInt32(rdr["SaleMaster_ID"]);
                        item.Quantity = rdr["QUANTITY"].ToString();
                        item.Description_Veh = rdr["Description"].ToString();
                        item.Total_Amount = rdr["Total_Amt"].ToString();
                        item.Unit_Price = rdr["UnitPrice"].ToString();
                        item.ItemDesc = rdr["ItemDesc"].ToString();
                        item.SaleInvoiceType = rdr["SaleInvoiceType"].ToString();


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





        //change Sales Master status
        public string ChangeSalesMasterStatus_trd_DAL(int? SaleMaster_ID, int? Status_ID, int? c_ID, string Trans_Type, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@SaleMaster_ID",SaleMaster_ID),
                  new SqlParameter("@Status_ID",Status_ID),
                   new SqlParameter("@c_ID",c_ID),
                    new SqlParameter("@Trans_Type",Trans_Type),
                   new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_SaleStatus_trd", paramArray).Tables[0];
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

        public string ChangeSalesMasterStatus_DAL(int? SaleMaster_ID, int? Status_ID, int? c_ID, string Trans_Type, int? Modified_by,string QRName)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@SaleMaster_ID",SaleMaster_ID),
                  new SqlParameter("@Status_ID",Status_ID),
                   new SqlParameter("@c_ID",c_ID),
                    new SqlParameter("@Trans_Type",Trans_Type),
                    new SqlParameter("@QRName",QRName),
                   new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_SaleStatus", paramArray).Tables[0];
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


        //Get sales Cancel detail list by id
        public IEnumerable<pa_SalesDetails> Get_SalesCancelDetailListByID_DAL(string Temp_ID, int? SaleMaster_ID)
        {



            string sp = "Select_SalesDetailCancel_ListBy_ID";
            List<pa_SalesDetails> itemlist = new List<pa_SalesDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@SaleMaster_ID", SaleMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesDetails item = new pa_SalesDetails();

                        item.SalesDetails_ID = Convert.ToInt32(rdr["SalesDetails_ID"]);

                        item.Unit_Price = rdr["Unit_Price"].ToString();
                        item.SaleInvoiceType = rdr["SaleInvoiceType"].ToString();
                        item.Stock_ID = rdr["Stock_ID"].ToString();
                        item.Chassis_No = rdr["Chassis_No"].ToString();
                        item.Make = rdr["Make"].ToString();
                        item.ModelDesciption = rdr["ModelDesciption"].ToString();
                        item.Color = rdr["Color"].ToString();
                        item.Amount = rdr["Amount"].ToString();

                        item.Grand_UnitPrice = rdr["Grand_UnitPrice"].ToString();
                        item.Grand_Amount = rdr["Grand_Amount"].ToString();
                        item.Grand_VATExp = rdr["Grand_VATExp"].ToString();
                        item.Grand_TotalAmount = rdr["Grand_TotalAmount"].ToString();




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


        //Insert sale cancel return Master
        public string InsertSalesMaster_CancelReturn_DAL(int? Customer_ID, string Customer_Contact, string Invoice_address, string Saletype, string SaleDate, string Temp_ID, int? c_ID, int? Created_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Customer_ID",Customer_ID),
                new SqlParameter("@Customer_Contact",Customer_Contact),
                new SqlParameter("@Invoice_address",Invoice_address),
                 new SqlParameter("@Saletype",Saletype),
                  new SqlParameter("@SaleDate",SaleDate),

                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Created_by",Created_by)




            };


                var response = dbLayer.SP_DataTable_return("Insert_SalesMaster_CancelReturn", paramArray).Tables[0];
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

        //Udpate Sales cancel return Master
        public string UpdateSalesMaster_CancelReturn_DAL(int? SaleMaster_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string Temp_ID, int? c_ID, int? Modified_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@SaleMaster_ID",SaleMaster_ID),
                new SqlParameter("@Customer_ID",Customer_ID),
                new SqlParameter("@Customer_Contact",Customer_Contact),
                new SqlParameter("@Invoice_address",Invoice_address),

                  new SqlParameter("@SaleDate",SaleDate),

                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Modified_by",Modified_by)




            };


                var response = dbLayer.SP_DataTable_return("Update_SalesMaster_CancelReturn", paramArray).Tables[0];
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


        //--getting sales cancel return master by id
        public pa_SalesMaster_DAL Get_SalesCancelReturnMasterByID_DAL(int? SaleMaster_ID)
        {

            string sp = "Select_SalesMasterCancelReturn_ByID";

            pa_SalesMaster_DAL pm = new pa_SalesMaster_DAL();
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
                                pm.SaleMaster_ID = Convert.ToInt32(dr["SaleMaster_ID"]);
                                pm.Customer_ID = dr["Customer_ID"].ToString();
                                pm.SaleRef = dr["SaleRef"].ToString();
                                pm.Customer_Contact = dr["Customer_Contact"].ToString();
                                pm.Invoice_address = dr["Invoice_address"].ToString();
                                pm.SaleDate = dr["SaleDate"].ToString();
                                pm.SaleStatus = dr["SaleStatus"].ToString();
                                pm.Created_at = dr["Created_at"].ToString();
                                pm.Created_by = dr["Created_by"].ToString();
                                pm.Modified_at = dr["Modified_at"].ToString();
                                pm.Modified_by = dr["Modified_by"].ToString();
                                pm.Trans_Ref_Other = dr["CancelRef"].ToString();

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



        //Get sales invoice master attachment list
        public IEnumerable<Pa_Attachments_DAL> GetSalesInvoiceMaster_Attachments_DAL(int? SaleMaster_ID, string Type)
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
                    cmd.Parameters.AddWithValue("@Master_ID", SaleMaster_ID);
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


        //Insert sales invoice master attachment
        public string Insert_Attachment_SalesInvoice_DAL(int? SaleMaster_ID, string Document_Type, string Type, string newFileName, string filepath, int? Created_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Master_ID",SaleMaster_ID),
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


        public string Delete_Attachment(int? Attachment_ID)
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



        public IEnumerable<pa_SalesDetails> Get_SalesInvoiceDetailPrintByID_DAL(int? SaleMaster_ID)
        {



            string sp = "Select_SalesDetailrSaleInvoicePrint_ByID";
            List<pa_SalesDetails> itemlist = new List<pa_SalesDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SaleMaster_ID", SaleMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesDetails item = new pa_SalesDetails();

                        item.SalesDetails_ID = Convert.ToInt32(rdr["SalesDetails_ID"]);

                        item.Unit_Price = rdr["Unit_Price"].ToString();

                        item.VATRate = rdr["VATRate"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();

                        item.SaleInvoiceType = rdr["SaleInvoiceType"].ToString();
                        item.Stock_ID = rdr["Stock_ID"].ToString();
                        item.Chassis_No = rdr["Chassis_No"].ToString();
                        item.Make = rdr["Make"].ToString();
                        item.ModelDesciption = rdr["ModelDesciption"].ToString();
                        item.Color = rdr["Color"].ToString();
                        item.TotalUnitPriceByChassis = rdr["TotalUnitPriceByChassis"].ToString();
                        item.Engine_No = rdr["Engine_No"].ToString();
                        item.Discount = rdr["Discount"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.Quantity = rdr["Quantity"].ToString();
                        item.Total_Amt_inWords = rdr["Total_Amt_inWords"].ToString();
                        item.ModelYear = rdr["ModelYear"].ToString();
                        item.ItemDesc = rdr["ItemDesc"].ToString();



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
        public DataSet GetDataForPrintDtl(int? SaleMaster_ID)
        {
            using (con = new SqlConnection(Constr))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Select_SalesMasterPerformaInvoice_Print_ByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SaleMaster_ID", SaleMaster_ID);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                return ds;
            }
        }

        public DataSet select_vehicle_display_Print_By_ID(int? vehicle_display_master_ID)
        {
            using (con = new SqlConnection(Constr))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("select_vehicle_display_Print_By_ID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@vehicle_display_master_ID", vehicle_display_master_ID);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                return ds;
            }
        }







        public IEnumerable<pa_vehicle_display_master> Get_vehicle_display_master_DAL(string Ref, string StartDate, string EndDate, string Chassis, int c_ID)
        {
            string sp = "select_vehicle_display_master";
            List<pa_vehicle_display_master> itemlist = new List<pa_vehicle_display_master>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.Parameters.AddWithValue("@Ref", Ref);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Chassis", Chassis);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    cmd.CommandType = CommandType.StoredProcedure;


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_vehicle_display_master item = new pa_vehicle_display_master();


                        item.vehicle_display_master_ID = Convert.ToInt32(rdr["vehicle_display_master_ID"].ToString());
                        item.Delivery_DateTime = rdr["Delivery_DateTime"].ToString();
                        item.Showroom_Loc_ID = rdr["Showroom_Loc_ID"].ToString();
                        item.delivered_by_EmpID = rdr["delivered_by_EmpID"].ToString();
                        item.Showroom_LocationName = rdr["Showroom_LocationName"].ToString();
                        item.Created_at = rdr["Created_at"].ToString();


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
        public pa_vehicle_display_master Get_vehicle_display_master_DAL_Id(int? vehicle_display_master_ID)
        {
            string sp = "select_vehicle_display_masters_Id";
            pa_vehicle_display_master item = new pa_vehicle_display_master();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vehicle_display_master_ID", vehicle_display_master_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {



                        item.vehicle_display_master_ID = Convert.ToInt32(rdr["vehicle_display_master_ID"].ToString());
                        item.Delivery_DateTime = rdr["Delivery_DateTime"].ToString();
                        item.Showroom_Loc_ID = rdr["Showroom_Loc_ID"].ToString();
                        item.delivered_by_EmpID = rdr["delivered_by_EmpID"].ToString();
                        item.Showroom_LocationName = rdr["Showroom_LocationName"].ToString();
                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();




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
        //Insert New Tasks
        public string Insert_vehicle_display_master_DAL(string Delivery_DateTime, string Showroom_Loc_ID, string delivered_by_EmpID, string temp_ID, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Delivery_DateTime",Delivery_DateTime ),
                new SqlParameter("@Showroom_Loc_ID" ,Showroom_Loc_ID ),
                new SqlParameter("@delivered_by_EmpID ",delivered_by_EmpID ),
                new SqlParameter("@temp_ID ",temp_ID ),
                new SqlParameter("@userID",Created_by),
                new SqlParameter("@c_ID",c_ID)



            };


                var response = dbLayer.SP_DataTable_return("pa_Insert_vehicle_display_master", paramArray).Tables[0];
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

        public string Delete_vehicle_display_master_DAL(int? vehicle_display_master_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@vehicle_display_master_ID",vehicle_display_master_ID)

            };


                var response = dbLayer.SP_DataTable_return("Delete_vehicle_display_master", paramArray).Tables[0];
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

        public IEnumerable<pa_vehicle_display_details> Get_vehicle_display_DetailListByID_DAL(int? vehicle_display_master_ID, string temp_ID)
        {



            string sp = "select_vehicle_display_details_ID";
            List<pa_vehicle_display_details> itemlist = new List<pa_vehicle_display_details>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@vehicle_display_master_ID", vehicle_display_master_ID);
                    cmd.Parameters.AddWithValue("@temp_ID", temp_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_vehicle_display_details item = new pa_vehicle_display_details();


                        item.vehicle_display_master_ID = Convert.ToInt32(rdr["vehicle_display_master_ID"].ToString());
                        item.vehicle_display_details_ID = Convert.ToInt32(rdr["vehicle_display_details_ID"].ToString());
                        item.chassis_no = rdr["CHASSIS_NO"].ToString();
                        item.return_Date = rdr["return_Date"].ToString();
                        item.Curr_Car_Location = rdr["Curr_Car_Location"].ToString();
                        item.VehicleDesc = rdr["VehicleDesc"].ToString();





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
        public string Insert_vehicle_display_detail_DAL(int? vehicle_display_master_ID, string chassis_no, string return_Date,
            string temp_ID, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@vehicle_display_master_ID",vehicle_display_master_ID),
                new SqlParameter("@chassis_no",chassis_no),
                new SqlParameter("@return_Date" ,return_Date),
                new SqlParameter("@temp_ID ",temp_ID),
                new SqlParameter("@userID",Created_by),
                 new SqlParameter("@c_ID ",c_ID)

            };


                var response = dbLayer.SP_DataTable_return("pa_Insert_vehicle_display_details", paramArray).Tables[0];
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
        public string Update_vehicle_display_detail_DAL(int? vehicle_display_master_ID, int? vehicle_display_details_ID, string chassis_no, string return_Date, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@vehicle_display_master_ID",vehicle_display_master_ID),

                new SqlParameter("@vehicle_display_details_ID",vehicle_display_details_ID ),
                new SqlParameter("@chassis_no" ,chassis_no ),
                new SqlParameter("@return_Date ",return_Date ),


                new SqlParameter("@userID",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("pa_Update_vehicle_display_details", paramArray).Tables[0];
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
        public string Delete_vehicle_display_detail_DAL(int? vehicle_display_details_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@vehicle_display_details_ID",vehicle_display_details_ID)

            };


                var response = dbLayer.SP_DataTable_return("Delete_vehicle_display_details", paramArray).Tables[0];
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
        public string GetOldTempIDFromDisplayDetail_DAL(int? vehicle_display_master_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@vehicle_display_master_ID",vehicle_display_master_ID)

            };


                var response = dbLayer.SP_DataTable_return("SelectDisplayDetailOldTempID", paramArray).Tables[0];
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
        public DataSet select_Delivery_Print_By_ID(int? DeliveryMaster_ID)
        {
            using (con = new SqlConnection(Constr))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SoldNotDelevered_Print", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DeliveryMaster_ID", DeliveryMaster_ID);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                return ds;
            }
        }



        #region SalesJP

        //---insert into sales booking detial general
        public string InsertSalesMasterJPDetail_DAL(string Chassis, string Ref,
             string Price, string PriceRate, string PriceTax, string RecycleFee,
            string auctionfee, string auctionfeetax, string NumberPlate, string OfficeCharges, string OfficeChargesTax, int hdSalesMaster_ID, string Temp_ID, int? c_ID, int? Created_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

               new SqlParameter("@Chassis_no", Chassis),
               new SqlParameter("@Sale_Ref", Ref),
               new SqlParameter("@Price", Price),
               new SqlParameter("@PriceRate", PriceRate),
               new SqlParameter("@PriceTax", PriceTax),
               new SqlParameter("@PriceCurrency", 1),
               new SqlParameter("@RecycleFee", RecycleFee),
                //cmd.Parameters.AddWithValue("@RecycleFeeTax",RecycleFeeTax),
               new SqlParameter("@AuctionFee", auctionfee),
               new SqlParameter("@AuctionFeeTax", auctionfeetax),
               new SqlParameter("@PlateFee", NumberPlate),
               new SqlParameter("@PlateFeeTax", 0),
               new SqlParameter("@comments", ""),
               new SqlParameter("@Location_ID", 0),
               new SqlParameter("@temp_ID", Temp_ID),
               new SqlParameter("@OfficeCharges", OfficeCharges),
               new SqlParameter("@OfficeChargesTax", OfficeChargesTax),
               new SqlParameter("@hdSalesMaster_ID", hdSalesMaster_ID),
               new SqlParameter("@c_ID",c_ID),




            };


                var response = dbLayer.SP_DataTable_return("InsertSalesDetailSaleInvoice_JP", paramArray).Tables[0];
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
        public string UpdateSalesMasterJPDetail_DAL(int? SaleDetailID, string Chassis,
             string Price, string PriceRate, string PriceTax, string RecycleFee,
            string auctionfee, string auctionfeetax, string NumberPlate, string OfficeCharges, string OfficeChargesTax, string Temp_ID, int? c_ID, int? Modified_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@SalesDetails_ID",SaleDetailID),
                                          new SqlParameter("@Chassis_no", Chassis),


               new SqlParameter("@PriceCurrency", 1),
                //cmd.Parameters.AddWithValue("@VendorID",Vendor),
  
               new SqlParameter("@Price", Price),
               new SqlParameter("@PriceRate", PriceRate),
               new SqlParameter("@PriceTax", PriceTax),
               new SqlParameter("@RecycleFee", RecycleFee),
                //cmd.Parameters.AddWithValue("@RecycleFeeTax",RecycleFeeTax),
               new SqlParameter("@AuctionFee", auctionfee),
               new SqlParameter("@AuctionFeeTax", auctionfeetax),
               new SqlParameter("@PlateFee", NumberPlate),
               new SqlParameter("@PlateFeeTax", 0),

                      new SqlParameter("@comments", ""),
                new SqlParameter("@Location_ID", 0),
                        new SqlParameter("@OfficeCharges", OfficeCharges),
               new SqlParameter("@OfficeChargesTax", OfficeChargesTax),



            };


                var response = dbLayer.SP_DataTable_return("pa_Update_SalesDetails_vehicles", paramArray).Tables[0];
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
        public string InsertSalesMasterJPMaster_DAL(int CustomerName, string Ref, string SaleRef, string Date, string ManualRef, string SaleDate, string CustomerContact, string CustomerAddress, string SalesType, string Saletype, string Temp_ID, int? c_ID, int? Created_by,int Bank_to_Transfer_payment_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {


                new SqlParameter("@Sale_Ref", Ref),
                new SqlParameter("@CustomerId", CustomerName),
                new SqlParameter("@Customer_Ref", CustomerName),
                new SqlParameter("@Date", Date),

                new SqlParameter("@Terms", 0),
                new SqlParameter("@Discount", 0),
                new SqlParameter("@Other_Charges", 0),
                new SqlParameter("@Sales_type", SalesType),
                new SqlParameter("@Invoice_Address", CustomerAddress),

                new SqlParameter("@Seller_ID", 0),
                new SqlParameter("@agent_ID", 0),

                new SqlParameter("@VatRate", 0),
                new SqlParameter("@VatExp", 0),

                new SqlParameter("@manual_ref", ManualRef),
                new SqlParameter("@CustomerTRN", 0),
                new SqlParameter("@Saletype", Saletype),





                new SqlParameter("@CustomerContact", CustomerContact),
                new SqlParameter("@CustomerAddress", CustomerAddress),


                new SqlParameter("@temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),
                 new SqlParameter("@Bank_to_Transfer_payment_ID",Bank_to_Transfer_payment_ID),

                 new SqlParameter("@User_ID",Created_by)




            };


                var response = dbLayer.SP_DataTable_return("Insert_SalesMaster_SaleInvoice_jp", paramArray).Tables[0];
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


        //Udpate Sale booking Master
        public string UpdateSalesMasterJPMaster_DAL(string SaleRef, int? Sale_ID, string Date, string Customer, string Ref, int? ID, string CustomerContact, string CustomerAddress, string SalesType, string txtmanualRefUpdate, string Temp_ID, int? c_ID, int? Modified_by,int Bank_to_Transfer_payment_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@Sale_Ref", SaleRef),
                new SqlParameter("@CustomerId", Customer),
                new SqlParameter("@Customer_Ref", Customer),
                new SqlParameter("@Date", Date),

                new SqlParameter("@Terms", 0),
                new SqlParameter("@Discount", 0),
                new SqlParameter("@Other_Charges", 0),
                new SqlParameter("@Sales_type", SalesType),

                new SqlParameter("@Invoice_Address", CustomerAddress),

                new SqlParameter("@Seller_ID", 0),
                new SqlParameter("@agent_ID", 0),

                new SqlParameter("@VatRate", 0),
                new SqlParameter("@VatExp", 0),


                new SqlParameter("@manual_ref", txtmanualRefUpdate),
                new SqlParameter("@CustomerTRN", 0),

                new SqlParameter("@approval_status", 0),



                new SqlParameter("@Sale_ID", Sale_ID),

                new SqlParameter("@CustomerContact", CustomerContact),
                new SqlParameter("@CustomerAddress", CustomerAddress),




                 new SqlParameter("@c_ID",c_ID),
                new SqlParameter("@Bank_to_Transfer_payment_ID",Bank_to_Transfer_payment_ID),
                 new SqlParameter("@User_ID",Modified_by)




            };


                var response = dbLayer.SP_DataTable_return("Update_SalesMaster_SaleInvoice_jp", paramArray).Tables[0];
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

        //--getting Sale booking master by id
        public pa_SalesMaster_DAL Get_SalesMasterJPMasterByID_DAL(int? SalesMaster_ID)
        {

            string sp = "Pa_Select_SaleMasterByID_JP";

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

                        pm.Created_at = dr["Created_at"].ToString();
                        pm.Created_by = dr["Created_by"].ToString();
                        pm.Modified_at = dr["Modified_at"].ToString();
                        pm.Modified_by = dr["Modified_by"].ToString();
                        pm.Bank_to_Transfer_payment_ID = dr["Bank_to_Transfer_payment_ID"].ToString();


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


        //Get Sale booking detail list by id
        public IEnumerable<SaleDetailJP> Get_SalesMasterJPDetailListByID_DAL(string Temp_ID, int? SalesMaster_ID)
        {



            string sp = "pa_Select_SaleDetail_ByID";
            List<SaleDetailJP> itemlist = new List<SaleDetailJP>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
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

        public string GetOldTempIDFromSaleDetail_DAL(int? SalesMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@SalesMaster_ID",SalesMaster_ID)

            };


                var response = dbLayer.SP_DataTable_return("SelectSaleDetailOldTempID", paramArray).Tables[0];
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
        public string DeleteSalesMasterJP_DAL(int? SalesMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@SaleMaster_ID",SalesMaster_ID)



            };


                var response = dbLayer.SP_DataTable_return("Pa_DeleteSalesMaster_jp", paramArray).Tables[0];
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





        public string Cancel_SaleJP(int? SaleMasterID,int? Status_ID ,int? c_ID, string Trans_Type, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@SaleMaster_ID",SaleMasterID),              
                new SqlParameter("@Status_ID",Status_ID),
                new SqlParameter("@c_ID",c_ID),
                new SqlParameter("@Trans_Type",Trans_Type),
                new SqlParameter("@Modified_by",Modified_by),



            };


               // var response = dbLayer.SP_DataTable_return("pa_Cancel_SalesVoucher", paramArray).Tables[0];
                
                 var response = dbLayer.SP_DataTable_return("Pa_Update_SaleStatus_jp", paramArray).Tables[0];
                
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

        public string DeleteSaleDetail_DAL(int? StockDetails_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@ID",StockDetails_ID)



            };


                var response = dbLayer.SP_DataTable_return("pa_DeleteSaleDetail", paramArray).Tables[0];
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
        public List<pa_SalesMaster_DAL> pa_Select_SalesMaster(string sale_Ref, int customerName, string startDate, string endDate)
        {



            string sp = "pa_Select_SalesList";

            List<pa_SalesMaster_DAL> main = new List<pa_SalesMaster_DAL>();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sale_Ref", sale_Ref);
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);
                        cmd.Parameters.AddWithValue("@CustomerName", customerName);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {



                                //     s.stock_ref, c.color, m.make, tblmake_details.model_description, sd.Chassis_no, sd.stock_ID, sd.ID, sd.temp_ID, sd.Shipping_info_ID

                                pa_SalesMaster_DAL si = new pa_SalesMaster_DAL();

                                si.SaleMaster_ID = Convert.ToInt32(dr["SaleMaster_ID"].ToString());
                                si.SaleRef = dr["SaleRef"].ToString();
                                si.SaleDate = dr["SaleDate"].ToString();
                                si.CustomerName = dr["CustomerName"].ToString();
                                si.Bal_due = dr["Bal_due"].ToString();

                                si.Paid_amt = dr["Paid_amt"].ToString();

                                si.Total_Amt = dr["Total_Amt"].ToString();
                                si.transaction_Status = dr["transaction_Status"].ToString();
                                si.SaleStatus = dr["payment_status"].ToString();







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





        public pa_SalesMaster_DAL SaleLoadRef()
        {



            string sp = "pa_AutoSaleRefJP";

            pa_SalesMaster_DAL si = new pa_SalesMaster_DAL();
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
                                si.SaleRef = dr["SaleRef"].ToString();
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


        //---insert into receipt detail Sale
        public string InsertReceiptDetail_Sale_DAL_Invoice(int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
            double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date,
            string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int hdSaleMaster_ID, int hdCustomer_ID, string ReceiptDate)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@DR_accountID",DR_accountID),
                  new SqlParameter("@Amount",Amount),
                   new SqlParameter("@other_curr_amount",other_curr_amount),
                    new SqlParameter("@Currency_ID",Currency_ID),
                   new SqlParameter("@Currency_Rate",Currency_Rate),
                     new SqlParameter("@CR_accountID",CR_accountID),


                      new SqlParameter("@Description",Description),
                       new SqlParameter("@trans_ref",trans_ref),

                         new SqlParameter("@Cheque_Bank_Name",Cheque_Bank_Name),
                          new SqlParameter("@cheque_Date",cheque_Date),
                           new SqlParameter("@Cheque_no",Cheque_no),
                            new SqlParameter("@VAT_Rate",VAT_Rate),
                             new SqlParameter("@VAT_Exp",VAT_Exp),
                              new SqlParameter("@Total_Amount",Total_Amount),
                               new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@c_ID",c_ID),
                                 new SqlParameter("@Created_by",Created_by),
                                 new SqlParameter("@link_trans_ref_ID",hdSaleMaster_ID),
                                 new SqlParameter("@hdCustomer_ID",hdCustomer_ID),
                                 new SqlParameter("@ReceiptDate",ReceiptDate)




            };


                var response = dbLayer.SP_DataTable_return("InsertReceiptDetailSaleforSaleInvoice", paramArray).Tables[0];
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

        //---Update receipt detail Sale
        public string UpdateReceiptDetail_Sale_DAL_Invoice(int? ReceiptDetail_ID, int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
            double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date,
            string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by, string ReceiptDate)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                        new SqlParameter("@ReceiptDetail_ID",ReceiptDetail_ID),
                new SqlParameter("@DR_accountID",DR_accountID),
                  new SqlParameter("@Amount",Amount),
                   new SqlParameter("@other_curr_amount",other_curr_amount),
                    new SqlParameter("@Currency_ID",Currency_ID),
                   new SqlParameter("@Currency_Rate",Currency_Rate),
                     new SqlParameter("@CR_accountID",CR_accountID),


                      new SqlParameter("@Description",Description),
                       new SqlParameter("@trans_ref",trans_ref),

                         new SqlParameter("@Cheque_Bank_Name",Cheque_Bank_Name),
                          new SqlParameter("@cheque_Date",cheque_Date),
                           new SqlParameter("@Cheque_no",Cheque_no),
                            new SqlParameter("@VAT_Rate",VAT_Rate),
                             new SqlParameter("@VAT_Exp",VAT_Exp),
                              new SqlParameter("@Total_Amount",Total_Amount),
                               new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@c_ID",c_ID),
                                 new SqlParameter("@Modified_by",Modified_by),
                                 new SqlParameter("@ReceiptDate",ReceiptDate)



            };


                var response = dbLayer.SP_DataTable_return("UpdateReceiptDetailSaleForSaleInvoice", paramArray).Tables[0];
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
        //Get receipt of type sale detail list by ID's
        public IEnumerable<pa_ReceiptDetails_DAL> Get_ReceiptDetailSaleListByID_DAL_Invoice(string Temp_ID, int SaletMaster_ID)
        {

            string sp = "Select_ReceiptDetail_SaleListByIDForSaleInvoice";
            List<pa_ReceiptDetails_DAL> itemlist = new List<pa_ReceiptDetails_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@SaletMaster_ID", SaletMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_ReceiptDetails_DAL item = new pa_ReceiptDetails_DAL();

                        item.ReceiptDetail_ID = Convert.ToInt32(rdr["ReceiptDetail_ID"]);
                        item.ReceiptMaster_ID = Convert.ToInt32(rdr["ReceiptMaster_ID"]);


                        item.CR_accountID = rdr["CR_accountID"].ToString();
                        item.DR_accountID = rdr["DR_accountID"].ToString();
                        item.DR_AcountName = rdr["DR_AcountName"].ToString();
                        item.CR_AccountName = rdr["CR_AccountName"].ToString();

                        item.Description = rdr["Description"].ToString();
                        item.Currency_ID = rdr["Currency_ID"].ToString();
                        item.Currency_Rate = rdr["Currency_Rate"].ToString();


                        item.Amount = rdr["Amount"].ToString();
                        item.VAT_Exp = rdr["VAT_Exp"].ToString();
                        item.VAT_Rate = rdr["VAT_Rate"].ToString();
                        item.other_curr_amount = rdr["other_curr_amount"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();
                        item.trans_ref = rdr["trans_ref"].ToString();
                        item.Receipttype = rdr["Receipttype"].ToString();
                        item.transaction_status = rdr["transaction_status"].ToString();

                        item.Cheque_Bank_Name = rdr["Cheque_Bank_Name"].ToString();
                        item.cheque_Date = rdr["cheque_Date"].ToString();
                        item.Cheque_no = rdr["Cheque_no"].ToString();

                        item.Grand_Amount = rdr["Grand_Amount"].ToString();
                        item.Grand_VAT_Exp = rdr["Grand_VAT_Exp"].ToString();

                        item.Grand_Total_Amount = rdr["Grand_Total_Amount"].ToString();

                        item.ReceiptDate = rdr["ReceiptDate"].ToString();



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

        //Delete receipt detail
        public string DeleteReceiptDetail_DAL_Invoice(int? ReceiptDetail_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@ReceiptDetail_ID",ReceiptDetail_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_ReceiptDetailForSaleInvoice", paramArray).Tables[0];
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


        public string Update_vehicle_display_master_DAL(int? vehicle_display_master_ID, string Delivery_DateTime,
           string Showroom_Loc_ID, string delivered_by_EmpID, int? Modified_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@vehicle_display_master_ID",vehicle_display_master_ID),

                new SqlParameter("@Delivery_DateTime",Delivery_DateTime ),
                new SqlParameter("@Showroom_Loc_ID" ,Showroom_Loc_ID ),
                new SqlParameter("@delivered_by_EmpID",delivered_by_EmpID ),


                new SqlParameter("@userID",Modified_by),
                  new SqlParameter("@c_ID",c_ID)



            };


                var response = dbLayer.SP_DataTable_return("pa_Update_vehicle_display_master", paramArray).Tables[0];
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


        public pa_SalesMaster_DAL GetSalesDetail_DAL(int? Stock_ID)
        {


            string sp = "GetSalesDetail_Stock";
            pa_SalesMaster_DAL Cust = new pa_SalesMaster_DAL();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Stock_ID", Stock_ID);
                    SqlDataReader record = cmd.ExecuteReader();


                    if (record.Read())
                    {

                        Cust.CustomerName = record["CustomerName"].ToString();
                        Cust.Customer_Contact = record["ContactNumber"].ToString();
                        Cust.SaleDate = record["SaleDate"].ToString();
                        Cust.Total_Amt = record["Total_Amount"].ToString();
                        Cust.Paid_amt = record["Paid_Amt"].ToString();
                        Cust.Bal_due = record["Bal_Due"].ToString();
                        Cust.ExportTo_Destination_ID = record["ExportTo_Destination_ID"].ToString();
                        Cust.Port_of_Exit_ID = record["Port_of_Exit_ID"].ToString();
                        Cust.PortTO = record["PortTO"].ToString();
                        Cust.PortFrom = record["PortFrom"].ToString();
                        Cust.Destination = record["Destination"].ToString();


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
        public pa_SalesMaster_DAL Get_SalesInvoiceMasterPrintByID_DAL(int? SaleMaster_ID)
        {

            string sp = "Select_SalesMasterSaleInvoicePrint_ByID";

            pa_SalesMaster_DAL pm = new pa_SalesMaster_DAL();
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
                                pm.SaleStatus_ID = dr["SaleStatus_ID"].ToString();
                                pm.SaleMaster_ID = Convert.ToInt32(dr["SaleMaster_ID"]);
                                pm.Customer_ID = dr["Customer_ID"].ToString();
                                pm.SaleRef = dr["SaleRef"].ToString();
                                pm.Customer_Contact = dr["Customer_Contact"].ToString();
                                pm.Invoice_address = dr["Invoice_address"].ToString();
                                pm.SaleDate = dr["SaleDate"].ToString();
                                pm.SaleStatus = dr["SaleStatus"].ToString();


                                pm.CustomerTRN = dr["CustomerTRN"].ToString();

                                pm.Company_TRN = dr["Company_TRN"].ToString();


                                pm.Manualbook_ref = dr["Manualbook_ref"].ToString();
                                pm.Seller_ID = dr["Seller_ID"].ToString();
                                pm.Agent_ID = dr["Agent_ID"].ToString();
                                pm.ExportTo_Destination_ID = dr["ExportTo_Destination_ID"].ToString();
                                pm.Port_of_Exit_ID = dr["Port_of_Exit_ID"].ToString();
                                pm.PortOfExit = dr["PortOfExit"].ToString();


                                pm.Sale_trans_type = dr["Sale_trans_type"].ToString();
                                pm.Currency_ShortName = dr["Currency_ShortName"].ToString();
                                pm.Minor_ShortName = dr["Minor_ShortName"].ToString();

                                pm.Created_at = dr["Created_at"].ToString();
                                pm.Created_by = dr["Created_by"].ToString();
                                pm.Modified_at = dr["Modified_at"].ToString();
                                pm.Modified_by = dr["Modified_by"].ToString();
                                pm.CustomerName = dr["CustomerName"].ToString();
                                pm.AfterVatTotal = dr["AfterVatTotal"].ToString();
                                pm.ExportCo = dr["ExportCo"].ToString();
                                pm.ShipCo = dr["ShipCo"].ToString();
                                pm.Destinations = dr["Destinations"].ToString();
                                pm.Paid_amt = dr["Paid_amt"].ToString();
                                pm.Bal_due = dr["Bal_due"].ToString();
                                pm.Remarks = dr["Remarks"].ToString();
                                pm.VATExp = dr["VATTotal"].ToString();
                                pm.Total_Amt = dr["BeforeVatTotal"].ToString();
                                pm.ReturnRef = dr["ReturnRef"].ToString();
                                pm.imagepath = dr["imagepath"].ToString();
                                pm.imgname = dr["imgname"].ToString();
                                pm.QRName = dr["QRName"].ToString();
                     

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
        public pa_SalesDetails Get_SalesInvoiceDetailPrintByID_ttl(int? SaleMaster_ID)
        {



            string sp = "Select_SalesDetailrSaleInvoicePrint_ByID";
            pa_SalesDetails item = new pa_SalesDetails();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SaleMaster_ID", SaleMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {


                        item.SalesDetails_ID = Convert.ToInt32(rdr["SalesDetails_ID"]);

                        item.Unit_Price = rdr["Unit_Price"].ToString();

                        item.VATRate = rdr["VATRate"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();

                        item.SaleInvoiceType = rdr["SaleInvoiceType"].ToString();
                        item.Stock_ID = rdr["Stock_ID"].ToString();
                        item.Chassis_No = rdr["Chassis_No"].ToString();
                        item.Make = rdr["Make"].ToString();
                        item.ModelDesciption = rdr["ModelDesciption"].ToString();
                        item.Color = rdr["Color"].ToString();
                        item.TotalUnitPriceByChassis = rdr["TotalUnitPriceByChassis"].ToString();
                        item.Engine_No = rdr["Engine_No"].ToString();
                        item.Discount = rdr["Discount"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.Quantity = rdr["Quantity"].ToString();
                        item.Total_Amt_inWords = rdr["Total_Amt_inWords"].ToString();
                        item.ModelYear = rdr["ModelYear"].ToString();




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

        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMasterInvoiceList_DAL1(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int Status_ID,
  int c_ID)

        {

            string sp = "Select_SalesMasterInvoiceList";
            List<pa_SalesMaster_DAL> itemlist = new List<pa_SalesMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                    cmd.Parameters.AddWithValue("@Status_ID", Status_ID);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesMaster_DAL item = new pa_SalesMaster_DAL();
                        //VATExp    Total_Amt    Paid_amt   Bal_due   SaleStatus   Modified_at   CustomerName   


                        item.SaleMaster_ID = Convert.ToInt32(rdr["SaleMaster_ID"]);
                        item.SaleRef = rdr["SaleRef"].ToString();
                        item.SaleDate = rdr["SaleDate"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amt = rdr["Total_Amt"].ToString();
                        item.Paid_amt = rdr["Paid_amt"].ToString();
                        item.Bal_due = rdr["Bal_due"].ToString();
                        item.Customer_Name = rdr["Customer_Name"].ToString();
                        item.SaleStatus = rdr["Status"].ToString();
                        item.Status = rdr["Status"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.ApprovalStatus = rdr["Approval_Status"].ToString();


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

        public pa_vehicle_display_master Get_vehicle_display_master_DAL_Id1(int? vehicle_display_master_ID)
        {
            string sp = "select_vehicle_display_masters_Id";
            pa_vehicle_display_master item = new pa_vehicle_display_master();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vehicle_display_master_ID", vehicle_display_master_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {



                        item.vehicle_display_master_ID = Convert.ToInt32(rdr["vehicle_display_master_ID"].ToString());
                        item.Delivery_DateTime = rdr["Delivery_DateTime"].ToString();
                        item.Showroom_Loc_ID = rdr["Showroom_Loc_ID"].ToString();
                        item.delivered_by_EmpID = rdr["delivered_by_EmpID"].ToString();
                        item.Showroom_LocationName = rdr["Showroom_LocationName"].ToString();
                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();




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
        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMasterInvoiceList_TTL_DAL1(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int Status_ID,
    int c_ID)

        {

            string sp = "Select_SalesMasterInvoiceList_TTL";
            List<pa_SalesMaster_DAL> itemlist = new List<pa_SalesMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                    cmd.Parameters.AddWithValue("@Status_ID", Status_ID);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesMaster_DAL item = new pa_SalesMaster_DAL();

                        item.Total_Detail_Amount = rdr["Total_Detail_Amount"].ToString();
                        item.Total_VATExp = rdr["Total_VATExp"].ToString();
                        item.Total_Master_Amount = rdr["Total_Master_Amount"].ToString();
                        item.Total_Paid = rdr["Total_Paid"].ToString();
                        item.Total_Bal_Due = rdr["Total_Bal_Due"].ToString();

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

        public IEnumerable<pa_SalesMaster_DAL> Get_PerformaInvoice_List_DAL1(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int c_ID)

        {

            string sp = "Select_PerformaInvoice_List";
            List<pa_SalesMaster_DAL> itemlist = new List<pa_SalesMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesMaster_DAL item = new pa_SalesMaster_DAL();
                        //VATExp    Total_Amt    Paid_amt   Bal_due   SaleStatus   Modified_at   CustomerName   


                        item.SaleMaster_ID = Convert.ToInt32(rdr["SaleMaster_ID"]);
                        item.SaleRef = rdr["SaleRef"].ToString();
                        item.SaleDate = rdr["SaleDate"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amt = rdr["Total_Amt"].ToString();
                        item.Customer_Name = rdr["Customer_Name"].ToString();
                        item.SaleStatus = rdr["Status"].ToString();
                        item.Status = rdr["Status"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();

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


        //New Faraz Work
        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMasterBooking_List_DAL1(string SaleRef, string StartDate, string EndDate, string Customer_Name, string Chassis_No, int Status_ID,
          int c_ID)

        {

            string sp = "Select_SalesMasterBooking_List";
            List<pa_SalesMaster_DAL> itemlist = new List<pa_SalesMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                    cmd.Parameters.AddWithValue("@Status_ID", Status_ID);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesMaster_DAL item = new pa_SalesMaster_DAL();
                        //VATExp    Total_Amt    Paid_amt   Bal_due   SaleStatus   Modified_at   CustomerName   


                        item.SaleMaster_ID = Convert.ToInt32(rdr["SaleMaster_ID"]);
                        item.SaleRef = rdr["SaleRef"].ToString();
                        item.SaleDate = rdr["SaleDate"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amt = rdr["Total_Amt"].ToString();
                        item.Paid_amt = rdr["Paid_amt"].ToString();
                        item.Bal_due = rdr["Bal_due"].ToString();
                        item.Customer_Name = rdr["Customer_Name"].ToString();
                        item.SaleStatus = rdr["Status"].ToString();
                        item.Status = rdr["Status"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();

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



        public IEnumerable<pa_SalesMaster_DAL> Get_PerformaInvoiceDetailPrintByID_DAL(int? SaleMaster_ID)
        {



            string sp = "Select_SalesMasterPerformaInvoice_Print_ByID";
            List<pa_SalesMaster_DAL> itemlist = new List<pa_SalesMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SaleMaster_ID", SaleMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesMaster_DAL item = new pa_SalesMaster_DAL();

                        item.SaleMaster_ID = Convert.ToInt32(rdr["SaleMaster_ID"]);

                        item.SaleRef = rdr["SaleRef"].ToString();
                        item.Customer_ID = rdr["Customer_ID"].ToString();
                        item.Customer_Contact = rdr["Customer_Contact"].ToString();
                        item.Invoice_address = rdr["Invoice_address"].ToString();
                        item.SaleDate = rdr["SaleDate"].ToString();
                        item.Created_at = rdr["Created_at"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();
                        item.SaleStatus = rdr["SaleStatus"].ToString();
                        item.CustomerTRN = rdr["CustomerTRN"].ToString();
                        item.Manualbook_ref = rdr["Manualbook_ref"].ToString();
                        item.Sale_trans_type = rdr["Sale_trans_type"].ToString();
                        item.Total_Amt = rdr["Total_Amount"].ToString();
                        item.Unit_Price = rdr["Unit_Price"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.Make = rdr["Make"].ToString();
                        item.ModelDesciption = rdr["ModelDesciption"].ToString();
                        item.Color = rdr["Color"].ToString();
                        item.ModelYear = rdr["ModelYear"].ToString();
                        item.CustomerName = rdr["CustomerName"].ToString();
                        item.Chassis_No = rdr["Chassis_No"].ToString();

                        item.VATRate = rdr["VATRate"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Engine_No = rdr["Engine_No"].ToString();
                        item.Total_Amt_inWords = rdr["Total_Amt_inWords"].ToString();
                        item.AccountName = rdr["AccountName"].ToString();
                        item.AccountNumber = rdr["AccountNumber"].ToString();
                        item.BankName = rdr["BankName"].ToString();
                        item.IBAN = rdr["IBAN"].ToString();
                        item.SwiftCode = rdr["SwiftCode"].ToString();
                        item.BankAddress = rdr["BankAddress"].ToString();
                        item.Performa_Validity = rdr["ValidityDate"].ToString();
                        item.AfterVatTotal = rdr["TotalAfterVAT"].ToString();
                        item.TotalBeforeVAT = rdr["TotalBeforeVAT"].ToString();
                        item.Quantity = rdr["Quantity"].ToString();
                        item.ItemDesc = rdr["ItemDesc"].ToString();
                        item.SaleInvoiceType = rdr["SaleInvoiceType"].ToString();
                        item.Currency_ShortName = rdr["Currency_ShortName"].ToString();
                        item.Minor_ShortName = rdr["Minor_ShortName"].ToString();
                        item.imagepath = rdr["imagepath"].ToString();
                        item.imgname = rdr["imgname"].ToString();
                        item.QRName = rdr["QRName"].ToString();



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


        public IEnumerable<pa_SalesMaster_DAL> Get_PerformaInvoiceDetailPrintByID_Specs_DAL(int? SaleMaster_ID)
        {



            string sp = "Select_SalesMasterPerformaInvoice_SPECS_Print_ByID";
            List<pa_SalesMaster_DAL> itemlist = new List<pa_SalesMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SaleMaster_ID", SaleMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesMaster_DAL item = new pa_SalesMaster_DAL();

                        item.SaleMaster_ID = Convert.ToInt32(rdr["SaleMaster_ID"]);

                        item.SaleRef = rdr["SaleRef"].ToString();
                        item.Customer_ID = rdr["Customer_ID"].ToString();
                        item.Customer_Contact = rdr["Customer_Contact"].ToString();
                        item.Invoice_address = rdr["Invoice_address"].ToString();
                        item.SaleDate = rdr["SaleDate"].ToString();
                        item.Created_at = rdr["Created_at"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();
                        item.SaleStatus = rdr["SaleStatus"].ToString();
                        item.CustomerTRN = rdr["CustomerTRN"].ToString();
                        item.Manualbook_ref = rdr["Manualbook_ref"].ToString();
                        item.Sale_trans_type = rdr["Sale_trans_type"].ToString();
                        item.Total_Amt = rdr["Total_Amount"].ToString();
                        item.Unit_Price = rdr["Unit_Price"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.Make = rdr["Make"].ToString();
                        item.ModelDesciption = rdr["ModelDesciption"].ToString();
                        item.Color = rdr["Color"].ToString();
                        item.ModelYear = rdr["ModelYear"].ToString();
                        item.CustomerName = rdr["CustomerName"].ToString();
                        item.Chassis_No = rdr["Chassis_No"].ToString();

                        item.VATRate = rdr["VATRate"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Engine_No = rdr["Engine_No"].ToString();
                        item.Total_Amt_inWords = rdr["Total_Amt_inWords"].ToString();
                        item.AccountName = rdr["AccountName"].ToString();
                        item.AccountNumber = rdr["AccountNumber"].ToString();
                        item.BankName = rdr["BankName"].ToString();
                        item.IBAN = rdr["IBAN"].ToString();
                        item.SwiftCode = rdr["SwiftCode"].ToString();
                        item.BankAddress = rdr["BankAddress"].ToString();
                        item.Performa_Validity = rdr["ValidityDate"].ToString();
                        item.AfterVatTotal = rdr["TotalAfterVAT"].ToString();
                        item.TotalBeforeVAT = rdr["TotalBeforeVAT"].ToString();
                        item.Quantity = rdr["Quantity"].ToString();
                        item.ItemDesc = rdr["ItemDesc"].ToString();
                        item.SaleInvoiceType = rdr["SaleInvoiceType"].ToString();
                        item.Currency_ShortName = rdr["Currency_ShortName"].ToString();
                        item.Minor_ShortName = rdr["Minor_ShortName"].ToString();
                        item.imgname = rdr["imgname"].ToString();
                        item.QRName = rdr["QRName"].ToString();


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



        public IEnumerable<Pa_DeliveryOrder_Master> Get_DeliveryNotePrintByID_DAL(int? DeliveryMaster_ID)
        {



            string sp = "SoldNotDelevered_Print";
            List<Pa_DeliveryOrder_Master> itemlist = new List<Pa_DeliveryOrder_Master>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DeliveryMaster_ID", DeliveryMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_DeliveryOrder_Master item = new Pa_DeliveryOrder_Master();

                        item.ID = Convert.ToInt32(rdr["id"]);

                        item.CarTakenDate = rdr["CarTakenDate"].ToString();
                        item.CarTakenPerson = rdr["CarTakenPerson"].ToString();
                        item.CarTakenContact = rdr["CarTakenContact"].ToString();

                        item.SVRef = rdr["Sale_Ref"].ToString();
                        item.chassis_No = rdr["chassis_No"].ToString();
                        item.CarLocation = rdr["CarLocation"].ToString();
                        item.CustomerName = rdr["CustomerName"].ToString();
                        item.RVNO = rdr["RVNO"].ToString();
                        item.Vehicle = rdr["Vehicle"].ToString();
                        item.color = rdr["color"].ToString();
                        item.Notes = rdr["Notes"].ToString();
                        item.DeliveryRef = rdr["DeliveryRef"].ToString();
                        item.Created_by = rdr["PreparedBy"].ToString();





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


        #region SalesInvoice_TRD
        public pa_SalesMaster_DAL Get_SalesInvoiceMasterByID_DAL_TRD(int? SaleMaster_ID)
        {

            string sp = "Select_SalesMasterSaleInvoice_ByID_TRD";

            pa_SalesMaster_DAL pm = new pa_SalesMaster_DAL();
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
                                pm.SaleMaster_ID = Convert.ToInt32(dr["SaleMaster_ID"]);
                                pm.Customer_ID = dr["Customer_ID"].ToString();
                                pm.SaleRef = dr["SaleRef"].ToString();
                                pm.Customer_Contact = dr["Customer_Contact"].ToString();
                                pm.Invoice_address = dr["Invoice_address"].ToString();
                                pm.SaleDate = dr["SaleDate"].ToString();
                                pm.SaleStatus = dr["SaleStatus"].ToString();


                                pm.CustomerTRN = dr["CustomerTRN"].ToString();
                                pm.Manualbook_ref = dr["Manualbook_ref"].ToString();
                                pm.Seller_ID = dr["Seller_ID"].ToString();


                                pm.Sale_trans_type = dr["Sale_trans_type"].ToString();



                                pm.Created_at = dr["Created_at"].ToString();
                                pm.Created_by = dr["Created_by"].ToString();
                                pm.Modified_at = dr["Modified_at"].ToString();
                                pm.Modified_by = dr["Modified_by"].ToString();
                                pm.Remarks = dr["Remarks"].ToString();
                                pm.Customer_Name = dr["Customer_Name"].ToString();
                                pm.Other_Status = dr["Other_Status"].ToString();
                                pm.OtherCost = dr["OtherCost"].ToString();
                                pm.ShippingCharges = dr["ShippingCharges"].ToString();
                                pm.Tip = dr["Tip"].ToString();
                                pm.Commision = dr["Commission"].ToString();
                                pm.Wallet = dr["Wallet"].ToString();
                                
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
        public IEnumerable<pa_SalesDetails> Get_SalesIvoiceDetailListByChassis_DAL_TRD(string Temp_ID, int? SaleMaster_ID)
        {



            string sp = "Select_SaleDetailInv_List_ByItem_TRD";
            List<pa_SalesDetails> itemlist = new List<pa_SalesDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@SaleMaster_ID", SaleMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesDetails item = new pa_SalesDetails();

                        item.SalesDetails_ID = Convert.ToInt32(rdr["SalesDetails_ID"]);

                        item.Unit_Price = rdr["Unit_Price"].ToString();

                        item.VATRate = rdr["VATRate"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();

                        item.ItemDesc = rdr["ItemDesc"].ToString();

                        item.SaleInvoiceType = rdr["SaleInvoiceType"].ToString();
                        item.Stock_ID = rdr["Stock_ID"].ToString();


                        item.Grand_UnitPrice = rdr["Grand_UnitPrice"].ToString();
                        item.Grand_Amount = rdr["Grand_Amount"].ToString();
                        item.Grand_TotalAmount = rdr["Grand_TotalAmount"].ToString();
                        item.Grand_VATExp = rdr["Grand_VATExp"].ToString();
                        item.Total_Amt_inWords = rdr["Total_Amt_inWords"].ToString();
                        item.ItemCode = rdr["ItemId"].ToString();
                        item.Item_ID = Convert.ToInt32(rdr["Item_ID"]);

                        item.Location_ID = rdr["Location_ID"].ToString();
                        item.BarCode = rdr["BarCode"].ToString();
                        item.Quantity = rdr["Quantity"].ToString();
                        item.UM_ID = rdr["UM_ID"].ToString();
                        item.ItemName = rdr["ItemName"].ToString();
                     
                        item.Serial = rdr["SerialNo"].ToString();
                        item.LocationName = rdr["ItemLocationName"].ToString();

                        item.OtherCost = rdr["OtherCost"].ToString();
                        item.ShippingCharges = rdr["Shipping_Charges"].ToString();
                        item.Tip = rdr["Tip"].ToString();
                        item.Seller_ID = Convert.ToInt32(rdr["Seller_ID"].ToString());

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

        public string InsertSalesInvoiceDetail_DAL_TRD(string SalesInvoiceType, int Item_ID, string ItemDesc, double? Unit_Price, double?
        VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int SaleMaster_ID, string Quantity, string UM_ID, int Location_ID, string Serial, string Barcode,int Seller_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
               new SqlParameter("@SalesInvoiceType",SalesInvoiceType),

                          new SqlParameter("@ItemDesc",ItemDesc),
                           new SqlParameter("@Unit_Price",Unit_Price),
                            new SqlParameter("@VATRate",VATRate),
                             new SqlParameter("@VATExp",VATExp),
                              new SqlParameter("@Total_Amount",Total_Amount),

                               new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@c_ID",c_ID),
                                 new SqlParameter("@Created_by",Created_by),
                                  new SqlParameter("@SaleMaster_ID",SaleMaster_ID),
                                  new SqlParameter("@Item_ID",Item_ID),
                                  new SqlParameter("@Quantity",Quantity),
                                  new SqlParameter("@UM_ID",UM_ID),
                                  new SqlParameter("@Serial",Serial),
                                  new SqlParameter("@Barcode",Barcode),
                                  new SqlParameter("@Location_ID",Location_ID),
                                  new SqlParameter("@Seller_ID",Seller_ID)



            };


                var response = dbLayer.SP_DataTable_return("InsertSalesDetailSaleInvoice_TRD", paramArray).Tables[0];
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
        public string UpdateSalesInvoiceDetail_DAL_TRD(int? SalesDetails_ID, int Item_ID, string ItemDesc, double? Unit_Price, 
            double? VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by
            , string Quantity, string UM_ID, int Location_ID, string Serial, string Barcode,int Seller_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
               new SqlParameter("@SalesDetails_ID",SalesDetails_ID),

                            new SqlParameter("@ItemDesc",ItemDesc),
                           new SqlParameter("@Unit_Price",Unit_Price),
                            new SqlParameter("@VATRate",VATRate),
                             new SqlParameter("@VATExp",VATExp),
                              new SqlParameter("@Total_Amount",Total_Amount),
                               new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@c_ID",c_ID),
                                 new SqlParameter("@Modified_by",Modified_by),
                                  new SqlParameter("@Item_ID",Item_ID),
                                  new SqlParameter("@Quantity",Quantity),
                                  new SqlParameter("@UM_ID",UM_ID),
                                                       new SqlParameter("@Serial",Serial),
                                  new SqlParameter("@Barcode",Barcode),
                                  new SqlParameter("@Location_ID",Location_ID),
                                  new SqlParameter("@Seller_ID",Seller_ID)


            };


                var response = dbLayer.SP_DataTable_return("Pa_UpdateSalesDetailSaleInvoice_TRD", paramArray).Tables[0];
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

        public string InsertSalesInvoiceMaster_DAL_TRD(int? Customer_ID, string Customer_Contact, string Invoice_address, string Saletype, string SaleDate, string CustomerTRN,
        string Manualbook_ref, string Sale_trans_type,
     string Temp_ID, int? c_ID, int? Created_by, string Remarks, string Customer_Name,string OtherCost,string Tip, string Commision, string Wallet, string ShippingCharges)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Customer_ID",Customer_ID),
                new SqlParameter("@Customer_Contact",Customer_Contact),
                new SqlParameter("@Invoice_address",Invoice_address),
                 new SqlParameter("@Saletype",Saletype),
                  new SqlParameter("@SaleDate",SaleDate),
             new SqlParameter("@CustomerTRN",CustomerTRN),
 new SqlParameter("@Manualbook_ref",Manualbook_ref),
  //new SqlParameter("@Seller_ID",Seller_ID),


    new SqlParameter("@Sale_trans_type",Sale_trans_type),




                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Created_by",Created_by),
                 new SqlParameter("@Customer_Name",Customer_Name),
                 new SqlParameter("@Remarks",Remarks),
                 new SqlParameter("@OtherCost",OtherCost),
                 new SqlParameter("@Tip",Tip),
                new SqlParameter("@Commission",Commision),
                 new SqlParameter("@Wallet",Wallet),
                 new SqlParameter("@ShippingCharges",ShippingCharges)





            };


                var response = dbLayer.SP_DataTable_return("Insert_SalesMaster_SaleInvoice_TRD", paramArray).Tables[0];
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
        public string UpdateSalesInvoiceMaster_DAL_TRD(int? SaleMaster_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string CustomerTRN,
        string Manualbook_ref, string Sale_trans_type,
  string Temp_ID, int? c_ID, int? Modified_by, string Remarks, string Customer_Name,string OtherCost,string Tip, string Commision, string Wallet, string ShippingCharges)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@SaleMaster_ID",SaleMaster_ID),
                new SqlParameter("@Customer_ID",Customer_ID),
                new SqlParameter("@Customer_Contact",Customer_Contact),
                new SqlParameter("@Invoice_address",Invoice_address),

                  new SqlParameter("@SaleDate",SaleDate),

                    new SqlParameter("@CustomerTRN",CustomerTRN),
 new SqlParameter("@Manualbook_ref",Manualbook_ref),
  //new SqlParameter("@Seller_ID",Seller_ID),


    new SqlParameter("@Sale_trans_type",Sale_trans_type),


                 new SqlParameter("@Temp_ID",Temp_ID),
                 new SqlParameter("@c_ID",c_ID),
                 new SqlParameter("@Modified_by",Modified_by),
                 new SqlParameter("@Customer_Name",Customer_Name),
                 new SqlParameter("@Remarks",Remarks),
                 new SqlParameter("@OtherCost",OtherCost),
                 new SqlParameter("@Tip",Tip),
                 new SqlParameter("@Commission",Commision),
                 new SqlParameter("@Wallet",Wallet),
                 new SqlParameter("@ShippingCharges",ShippingCharges)


            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_SalesMaster_SaleInvoice_TRD", paramArray).Tables[0];
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

        public string DeleteSalesMaster_DAL_TRD(int? SaleMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@SaleMaster_ID",SaleMaster_ID)



            };


                var response = dbLayer.SP_DataTable_return("Pa_DeleteSalesMaster_TRD", paramArray).Tables[0];
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
        public string SalesID_DAL_TRD()
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                     new SqlParameter("@SaleType","SI")


            };


                var response = dbLayer.SP_DataTable_return("Pa_GetSalesMasterID_TRD", paramArray).Tables[0];
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
        public string DeleteSalesDetail_DAL_TRD(int? SalesDetails_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@SalesDetails_ID",SalesDetails_ID)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_SalesDetail_TRD", paramArray).Tables[0];
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

        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMasterInvoiceList_DAL_TRD(string SaleRef, string StartDate, string EndDate, string Customer_Name, string ItemId,string MNumber, int Status_ID,
    int c_ID,string OrderRef ,string OrderStatus)

        {

            string sp = "Select_SalesMasterInvoiceList_TRD";
            List<pa_SalesMaster_DAL> itemlist = new List<pa_SalesMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    cmd.Parameters.AddWithValue("@ItemId", ItemId);
                    cmd.Parameters.AddWithValue("@Status_ID", Status_ID);
                    cmd.Parameters.AddWithValue("@MNumber", MNumber);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    cmd.Parameters.AddWithValue("@OrderRef", OrderRef);
                    cmd.Parameters.AddWithValue("@OrderStatus", OrderStatus);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesMaster_DAL item = new pa_SalesMaster_DAL();
                        //VATExp    Total_Amt    Paid_amt   Bal_due   SaleStatus   Modified_at   CustomerName   


                        item.SaleMaster_ID = Convert.ToInt32(rdr["SaleMaster_ID"]);
                        item.SaleRef = rdr["SaleRef"].ToString();
                        item.SaleDate = rdr["SaleDate"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amt = rdr["Total_Amt"].ToString();
                        item.Paid_amt = rdr["Paid_amt"].ToString();
                        item.Bal_due = rdr["Bal_due"].ToString();
                        item.Customer_Name = rdr["Customer_Name"].ToString();
                        item.SaleStatus = rdr["Status"].ToString();
                        item.Status = rdr["Status"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.ApprovalStatus = rdr["Approval_Status"].ToString();

                        item.Manualbook_ref = rdr["Manualbook_ref"].ToString();
                        item.Other_Status = rdr["Other_Status"].ToString();
                        item.Tip = rdr["Tip"].ToString();
                        item.ShippingCharges = rdr["ShippingCharges"].ToString();
                        item.Commision = rdr["Commission"].ToString();
                        item.Wallet = rdr["Wallet"].ToString();

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

        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMasterInvoiceList_TTL_DAL_TRD(string SaleRef, string StartDate, string EndDate, string Customer_Name, string ItemId, string MNumber, int Status_ID,
          int c_ID,string OrderRef,string OrderStatus)

        {

            string sp = "Select_SalesMasterInvoiceList_TTL_TRD";
            List<pa_SalesMaster_DAL> itemlist = new List<pa_SalesMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    cmd.Parameters.AddWithValue("@ItemId", ItemId);
                    cmd.Parameters.AddWithValue("@Status_ID", Status_ID);
                    cmd.Parameters.AddWithValue("@MNumber", MNumber);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    cmd.Parameters.AddWithValue("@OrderRef", OrderRef);
                    cmd.Parameters.AddWithValue("@OrderStatus", OrderStatus);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesMaster_DAL item = new pa_SalesMaster_DAL();

                        item.Total_Detail_Amount = rdr["Total_Detail_Amount"].ToString();
                        item.Total_VATExp = rdr["Total_VATExp"].ToString();
                        item.Total_Master_Amount = rdr["Total_Master_Amount"].ToString();
                        item.Total_Paid = rdr["Total_Paid"].ToString();
                        item.Total_Bal_Due = rdr["Total_Bal_Due"].ToString();
                      


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







        public List<pa_SalesMaster_DAL> pa_Select_SalesMaster_total(string sale_Ref, int customerName, string startDate, string endDate)
        {



            string sp = "pa_Select_SalesList_ttl";

            List<pa_SalesMaster_DAL> main = new List<pa_SalesMaster_DAL>();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Sale_Ref", sale_Ref);
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);
                        cmd.Parameters.AddWithValue("@CustomerName", customerName);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {



                                //     s.stock_ref, c.color, m.make, tblmake_details.model_description, sd.Chassis_no, sd.stock_ID, sd.ID, sd.temp_ID, sd.Shipping_info_ID

                                pa_SalesMaster_DAL si = new pa_SalesMaster_DAL();


                                si.Bal_due = dr["Bal_due"].ToString();

                                si.Paid_amt = dr["Paid_amt"].ToString();

                                si.Total_Amt = dr["Total_Amt"].ToString();







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

        public IEnumerable<pa_ReceiptDetails_DAL> Get_ReceiptDetailSaleListByID_DAL_Invoice_Print(int? SaletMaster_ID)
        {

            string sp = "Select_ReceiptDetail_SaleListByID_Print";
            List<pa_ReceiptDetails_DAL> itemlist = new List<pa_ReceiptDetails_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SaletMaster_ID", SaletMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_ReceiptDetails_DAL item = new pa_ReceiptDetails_DAL();

                        item.ReceiptDetail_ID = Convert.ToInt32(rdr["ReceiptDetail_ID"]);
                        item.ReceiptMaster_ID = Convert.ToInt32(rdr["ReceiptMaster_ID"]);


                        item.CR_accountID = rdr["CR_accountID"].ToString();
                        item.DR_accountID = rdr["DR_accountID"].ToString();
                        item.DR_AcountName = rdr["DR_AcountName"].ToString();
                        item.CR_AccountName = rdr["CR_AccountName"].ToString();

                        item.Description = rdr["Description"].ToString();
                        item.Currency_ID = rdr["Currency_ID"].ToString();
                        item.Currency_Rate = rdr["Currency_Rate"].ToString();


                        item.Amount = rdr["Amount"].ToString();
                        item.VAT_Exp = rdr["VAT_Exp"].ToString();
                        item.VAT_Rate = rdr["VAT_Rate"].ToString();
                        item.other_curr_amount = rdr["other_curr_amount"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();
                        item.trans_ref = rdr["trans_ref"].ToString();
                        item.Receipttype = rdr["Receipttype"].ToString();
                        item.transaction_status = rdr["transaction_status"].ToString();

                        item.Cheque_Bank_Name = rdr["Cheque_Bank_Name"].ToString();
                        item.cheque_Date = rdr["cheque_Date"].ToString();
                        item.Cheque_no = rdr["Cheque_no"].ToString();

                        item.Grand_Amount = rdr["Grand_Amount"].ToString();
                        item.Grand_VAT_Exp = rdr["Grand_VAT_Exp"].ToString();

                        item.Grand_Total_Amount = rdr["Grand_Total_Amount"].ToString();



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

        #region Dashboard Graph

        public DataSet getLAST_7DAYS_SALE_TRD()
        {
            using (con = new SqlConnection(Constr))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("LAST_7DAYS_SALE_TRD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@c_ID", 1);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                return ds;
            }
        }
        public DataSet getLAST_7DAYS_SALE()
        {
            using (con = new SqlConnection(Constr))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("LAST_7DAYS_SALE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@c_ID", 1);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                return ds;
            }
        }
        public DataSet getSale_By_Month()
        {
            using (con = new SqlConnection(Constr))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sale_By_Month", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@c_ID", 1);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                return ds;
            }
        }

        public DataSet GetTopSaleValueItems_trd(string StartDate, string EndDate)
        {
            using (con = new SqlConnection(Constr))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("TopSaleValueItems_trd", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Start_Date", StartDate);
                cmd.Parameters.AddWithValue("@End_Date", EndDate);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                return ds;
            }
        }

        public DataSet Sale_By_Month_trd(string Year, int c_ID)
        {
            using (con = new SqlConnection(Constr))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sale_By_Month_trd", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Year", Year);
                cmd.Parameters.AddWithValue("@c_ID", c_ID);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                return ds;
            }
        }

        public DataSet TopPayables_trd(string StartDate, string EndDate)
        {
            using (con = new SqlConnection(Constr))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("TopPayables_trd", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Start_Date", StartDate);
                cmd.Parameters.AddWithValue("@End_Date", EndDate);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                return ds;
            }
        }

        public DataSet TopSaleCategory_trd(string StartDate, string EndDate)
        {
            using (con = new SqlConnection(Constr))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("TopSaleCategory_trd", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Start_Date", StartDate);
                cmd.Parameters.AddWithValue("@End_Date", EndDate);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                return ds;
            }
        }

        public DataSet TopItemByCost_trd(string StartDate, string EndDate)
        {
            using (con = new SqlConnection(Constr))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("TopItemByCost_trd", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Start_Date", StartDate);
                cmd.Parameters.AddWithValue("@End_Date", EndDate);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                return ds;
            }
        }
        public DataSet get_StockLocation_Summary(int c_ID)
        {
            using (con = new SqlConnection(Constr))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("get_StockLocation_Summary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@c_ID", c_ID);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                return ds;
            }
        }


        public DataSet get_All_Accounts_Balance(int c_ID)
        {
            using (con = new SqlConnection(Constr))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("get_All_accounts_Balance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@c_ID", c_ID);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                return ds;
            }
        }

        public DataSet get_MakeModel_Summary(int c_ID)
        {
            using (con = new SqlConnection(Constr))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("get_MakeModel_Sale", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@c_ID", c_ID);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                return ds;
            }
        }
        #endregion
        public IEnumerable<pa_SalesMaster_DAL> Get_SalesSummaryByDate_DAL(string StartDate, string EndDate)

        {

            string sp = "SalesSummarybySaleDate";
            List<pa_SalesMaster_DAL> itemlist = new List<pa_SalesMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
             
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
           

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesMaster_DAL item = new pa_SalesMaster_DAL();
                   


           
                        item.SaleDate = rdr["SaleDate"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.Void = rdr["Void"].ToString();
                        item.Discount = rdr["Discount"].ToString();
           


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

        public IEnumerable<pa_SalesDetails> GetVoidDetails_DAL(string SaleDate)
        {


            string sp = "GetVoidDetails";
            List<pa_SalesDetails> itemlist = new List<pa_SalesDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleDate", SaleDate);



                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesDetails item = new pa_SalesDetails();

                        item.INVOICENO = rdr["INVOICENO"].ToString();
                        item.INOVOICEDATE = rdr["INOVOICEDATE"].ToString();
                        item.ItemName = rdr["ItemName"].ToString();
                        item.ItemQty = rdr["ItemQty"].ToString();
       
                        item.ItemRate = rdr["ItemRate"].ToString();
                        item.Total = rdr["Total"].ToString();
                        item.username = rdr["username"].ToString();
                        item.usercode = rdr["usercode"].ToString();


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


        public IEnumerable<pa_SalesDetails> GetDiscountDetails_DAL(string SaleDate)
        {


            string sp = "GetDiscountDetails";
            List<pa_SalesDetails> itemlist = new List<pa_SalesDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleDate", SaleDate);



                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesDetails item = new pa_SalesDetails();

                        item.INVOICENO = rdr["Invoice_No"].ToString();
                        item.INOVOICEDATE = rdr["SaleDate"].ToString();
 
                     
                        item.Total = rdr["Total_Amt"].ToString();
                        item.Discount = rdr["Discount"].ToString();
      
                        item.DiscountCode = rdr["DiscountCode"].ToString();


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

        public IEnumerable<pa_SalesDetails> GetDiscountDetails_DAL_TTL(string SaleDate)
        {


            string sp = "GetDiscountDetails_TTL";
            List<pa_SalesDetails> itemlist = new List<pa_SalesDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleDate", SaleDate);



                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesDetails item = new pa_SalesDetails();

                        item.Total_Discount = rdr["Total_Discount"].ToString();
               


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
        public IEnumerable<Pa_ItemLocations_DAL> Get_LocList_DAL(string Item_ID)
        {


            string sp = "Pa_Select_LocationSummary_ByItemId";
            List<Pa_ItemLocations_DAL> itemlist = new List<Pa_ItemLocations_DAL>();

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
                        Pa_ItemLocations_DAL item = new Pa_ItemLocations_DAL();

                        item.ItemLocation_ID = Convert.ToInt32(rdr["Loc_ID"]);
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
        public string ChangeSalesMasterStatus_Performa_DAL(int? SaleMaster_ID, int? Status_ID, int? c_ID, string Trans_Type, int? Modified_by, string QRName)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@SaleMaster_ID",SaleMaster_ID),
                  new SqlParameter("@Status_ID",Status_ID),
                   new SqlParameter("@c_ID",c_ID),
                    new SqlParameter("@Trans_Type",Trans_Type),
                    new SqlParameter("@QRName",QRName),
                   new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_SaleStatus_Performa", paramArray).Tables[0];
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
        #region SalesDeliveryOrder

        public Pa_SalesDeliveryOrder_Master Get_SalesDeliveryOrderMaster_ByID_DAL(int? DOMaster_ID)
        {

            string sp = "Select_SaleDOasterSaleInvoice";

            Pa_SalesDeliveryOrder_Master pm = new Pa_SalesDeliveryOrder_Master();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DOMaster_ID", DOMaster_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.DOMaster_ID = Convert.ToInt32(dr["DOMaster_ID"]);
                                pm.Customer_ID = dr["Customer_ID"].ToString();
                                pm.DORef = dr["DORef"].ToString();
                                pm.Customer_Contact = dr["Customer_Contact"].ToString();
                                pm.Invoice_address = dr["Invoice_address"].ToString();
                                pm.DODate = dr["DODate"].ToString();
                               


                                pm.CustomerTRN = dr["CustomerTRN"].ToString();
                                pm.Manualbook_ref = dr["Manualbook_ref"].ToString();
                                pm.Seller_ID = dr["Seller_ID"].ToString();


                               



                                pm.Created_at = dr["Created_at"].ToString();
                                pm.Created_by = dr["Created_by"].ToString();
                                pm.Modified_at = dr["Modified_at"].ToString();
                                pm.Modified_by = dr["Modified_by"].ToString();
                                pm.Remarks = dr["Remarks"].ToString();
                                pm.Customer_Name = dr["Customer_Name"].ToString();
                                pm.SaleMaster_ID = Convert.ToInt32(dr["SaleMaster_ID"]);

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
        public IEnumerable<Pa_SalesDeliveryOrder_Dtl> Get_SalesDeliveryOrderDetailListByID_DAL(string Temp_ID, int? DOMaster_ID)
        {



            string sp = "Select_SaleDODetailInv_List_ByItem";
            List<Pa_SalesDeliveryOrder_Dtl> itemlist = new List<Pa_SalesDeliveryOrder_Dtl>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@DOMaster_ID", DOMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_SalesDeliveryOrder_Dtl item = new Pa_SalesDeliveryOrder_Dtl();

                        item.DODetails_ID = Convert.ToInt32(rdr["DODetails_ID"]);

                        item.Unit_Price = rdr["Unit_Price"].ToString();

                        item.VATRate = rdr["VATRate"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();

                        item.ItemDesc = rdr["ItemDesc"].ToString();

               
                        item.Stock_ID = rdr["Stock_ID"].ToString();


                        item.Grand_UnitPrice = rdr["Grand_UnitPrice"].ToString();
                        item.Grand_Amount = rdr["Grand_Amount"].ToString();
                        item.Grand_TotalAmount = rdr["Grand_TotalAmount"].ToString();
                        item.Grand_VATExp = rdr["Grand_VATExp"].ToString();
                        item.Total_Amt_inWords = rdr["Total_Amt_inWords"].ToString();
                        item.ItemCode = rdr["ItemId"].ToString();
                        item.Item_ID = Convert.ToInt32(rdr["Item_ID"]);

                        item.Location_ID = rdr["Location_ID"].ToString();
     
                        item.Quantity = rdr["Quantity"].ToString();
                        item.UM_ID = rdr["UM_ID"].ToString();
                        item.ItemName = rdr["ItemName"].ToString();
                        item.SaleQty = rdr["SaleQty"].ToString();
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
        public string InsertSalesDeliveryOrderDetail_DAL(int Item_ID, string ItemDesc, double? Unit_Price, double?
VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by,int DOMaster_ID, int SaleMaster_ID, string Quantity, string UM_ID, int Location_ID,int SaleDetails_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                           new SqlParameter("@Item_ID",Item_ID),

                          new SqlParameter("@ItemDesc",ItemDesc),
                           new SqlParameter("@Unit_Price",Unit_Price),
                            new SqlParameter("@VATRate",VATRate),
                             new SqlParameter("@VATExp",VATExp),
                              new SqlParameter("@Total_Amount",Total_Amount),

                               new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@c_ID",c_ID),
                                 new SqlParameter("@Created_by",Created_by),
                                  new SqlParameter("@SaleMaster_ID",SaleMaster_ID),
                                  new SqlParameter("@DOMaster_ID",DOMaster_ID),
                                 
                                  new SqlParameter("@Quantity",Quantity),
                                  new SqlParameter("@UM_ID",UM_ID),
                                  new SqlParameter("@SaleDetails_ID",SaleDetails_ID),
                                  new SqlParameter("@Location_ID",Location_ID)



            };


                var response = dbLayer.SP_DataTable_return("InsertSalesDODetailSaleInvoice", paramArray).Tables[0];
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





        public string InsertSalesDeliveryOrderMaster_DAL(int? Customer_ID, string Customer_Contact, string Invoice_address, string DODate, string CustomerTRN,
 string Manualbook_ref, int? Seller_ID, 
 string Temp_ID, int? c_ID, int? Created_by, string Remarks, string Customer_Name, int SaleMaster_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                 new SqlParameter("@Customer_ID",Customer_ID),
                new SqlParameter("@Customer_Contact",Customer_Contact),
                new SqlParameter("@Invoice_address",Invoice_address),
                  new SqlParameter("@DODate",DODate),
             new SqlParameter("@CustomerTRN",CustomerTRN),
 new SqlParameter("@Manualbook_ref",Manualbook_ref),
  new SqlParameter("@Seller_ID",Seller_ID),

                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Created_by",Created_by),
                 new SqlParameter("@Customer_Name",Customer_Name),
                 new SqlParameter("@Remarks",Remarks),
                
                 new SqlParameter("@SaleMaster_ID",SaleMaster_ID)






            };


                var response = dbLayer.SP_DataTable_return("Pa_Insert_DOMaster", paramArray).Tables[0];
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

        public string UpdateSalesDeliveryOrderMaster_DAL(int DOMaster_ID,int? SaleMaster_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string DODate, string CustomerTRN,
string Manualbook_ref, int? Seller_ID, 
string Temp_ID, int? c_ID, int? Modified_by, string Remarks, string Customer_Name)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
  new SqlParameter("@DOMaster_ID",DOMaster_ID),
  new SqlParameter("@SaleMaster_ID",SaleMaster_ID),
                new SqlParameter("@Customer_ID",Customer_ID),
                new SqlParameter("@Customer_Contact",Customer_Contact),
                new SqlParameter("@Invoice_address",Invoice_address),

                  new SqlParameter("@DODate",DODate),

                    new SqlParameter("@CustomerTRN",CustomerTRN),
 new SqlParameter("@Manualbook_ref",Manualbook_ref),
  new SqlParameter("@Seller_ID",Seller_ID),





                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Modified_by",Modified_by),
                   new SqlParameter("@Customer_Name",Customer_Name),
                 new SqlParameter("@Remarks",Remarks)





            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_DOMaster", paramArray).Tables[0];
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


        public string DeleteSalesDeliveryOrderMaster_DAL(int? DOMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@DOMaster_ID",DOMaster_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_DO_Master", paramArray).Tables[0];
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
        public string DeleteSalesDeliveryOrder_Material_In_DAL(int? DODetails_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@DODetails_ID",DODetails_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_DO_Dtl", paramArray).Tables[0];
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

        public IEnumerable<Pa_SalesDeliveryOrder_Master> Get_SalesDeliveryOrderMaster_DAL(string DORef, string StartDate, string EndDate, string Customer_Name, string ItemId, int Status_ID,
int c_ID)
        {


            string sp = "Select_DOMasterInvoiceList";
            List<Pa_SalesDeliveryOrder_Master> itemlist = new List<Pa_SalesDeliveryOrder_Master>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DORef", DORef);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    cmd.Parameters.AddWithValue("@ItemId", ItemId);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_SalesDeliveryOrder_Master item = new Pa_SalesDeliveryOrder_Master();

                        item.DOMaster_ID = Convert.ToInt32(rdr["DOMaster_ID"]);
                        item.DORef = rdr["DORef"].ToString();
                        item.DODate = rdr["DODate"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amt = rdr["Total_Amt"].ToString();
                        item.SaleRef = rdr["SaleRef"].ToString();

                        item.Customer_Name = rdr["CustomerName"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.ApprovalStatus = rdr["Approval_Status"].ToString();



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




        public string GetOldTempIDFromSalesDeliveryOrderDetail_DAL(int? DOMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@DOMaster_ID",DOMaster_ID)

            };


                var response = dbLayer.SP_DataTable_return("SelectDODetailOldTempID", paramArray).Tables[0];
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

        public IEnumerable<pa_SalesDetails> GetDORefDetails_Other_DAL()
        {


            string sp = "GetSaleRef";
            List<pa_SalesDetails> itemlist = new List<pa_SalesDetails>();

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
                        pa_SalesDetails item = new pa_SalesDetails();

                        item.SaleMaster_ID = Convert.ToInt32(rdr["SaleMaster_ID"]);
                        item.SaleRef = rdr["SaleRef"].ToString();



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

  
        public IEnumerable<pa_SalesDetails> Get_SalesDetailListByRef_DAL(int? DOMaster_ID)
        {



            string sp = "Select_SaleDetailInv_BySaleRef";
            List<pa_SalesDetails> itemlist = new List<pa_SalesDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
             
                    cmd.Parameters.AddWithValue("@DOMaster_ID", DOMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesDetails item = new pa_SalesDetails();

                        item.SalesDetails_ID = Convert.ToInt32(rdr["SalesDetails_ID"]);
                        item.SaleMaster_ID = Convert.ToInt32(rdr["SaleMaster_ID"]);

                        item.Unit_Price = rdr["Unit_Price"].ToString();

                        item.VATRate = rdr["VATRate"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();

                        item.ItemDesc = rdr["ItemDesc"].ToString();

           
                        item.Stock_ID = rdr["Stock_ID"].ToString();


                        item.Grand_UnitPrice = rdr["Grand_UnitPrice"].ToString();
                        item.Grand_Amount = rdr["Grand_Amount"].ToString();
                        item.Grand_TotalAmount = rdr["Grand_TotalAmount"].ToString();
                        item.Grand_VATExp = rdr["Grand_VATExp"].ToString();
                        item.Total_Amt_inWords = rdr["Total_Amt_inWords"].ToString();
                        item.ItemCode = rdr["ItemId"].ToString();
                        item.Item_ID = Convert.ToInt32(rdr["Item_ID"]);

                        item.Location_ID = rdr["Location_ID"].ToString();
             
                        item.Quantity = rdr["Quantity"].ToString();
                        item.UM_ID = rdr["UM_ID"].ToString();
                        item.ItemName = rdr["ItemName"].ToString();
                        item.SaleQty = rdr["SaleQty"].ToString();
                        item.DOQty = rdr["DOQty"].ToString();





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
        public Pa_SalesDeliveryOrder_Master Get_SalesDeliveryMasterPrintByID_DO_DAL(int? DOMaster_ID)
        {

            string sp = "Select_SalesMaster_DO";

            Pa_SalesDeliveryOrder_Master pm = new Pa_SalesDeliveryOrder_Master();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DOMaster_ID", DOMaster_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {

                            while (dr.Read())
                            {

                                pm.DOMaster_ID = Convert.ToInt32(dr["DOMaster_ID"]);
                                pm.Customer_ID = dr["Customer_ID"].ToString();
                                pm.DORef = dr["DORef"].ToString();
                                pm.Customer_Contact = dr["Customer_Contact"].ToString();
                                pm.Invoice_address = dr["Invoice_address"].ToString();
                                pm.DODate = dr["DODate"].ToString();
 


                                pm.CustomerTRN = dr["CustomerTRN"].ToString();

                                pm.Company_TRN = dr["Company_TRN"].ToString();


                                pm.Manualbook_ref = dr["Manualbook_ref"].ToString();
                   

                 
                                pm.Currency_ShortName = dr["Currency_ShortName"].ToString();
                                pm.Minor_ShortName = dr["Minor_ShortName"].ToString();

                                pm.Created_at = dr["Created_at"].ToString();
                                pm.Created_by = dr["Created_by"].ToString();
                                pm.Modified_at = dr["Modified_at"].ToString();
                                pm.Modified_by = dr["Modified_by"].ToString();
                                pm.CustomerName = dr["CustomerName"].ToString();
                                pm.AfterVatTotal = dr["AfterVatTotal"].ToString();
                                pm.ExportCo = dr["ExportCo"].ToString();
                                pm.ShipCo = dr["ShipCo"].ToString();
                                pm.Destinations = dr["Destinations"].ToString();
                   
                                pm.Remarks = dr["Remarks"].ToString();
                                pm.VATExp = dr["VATTotal"].ToString();
                                pm.Total_Amt = dr["BeforeVatTotal"].ToString();
                                pm.SaleRef = dr["SaleRef"].ToString();

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
        public Pa_SalesDeliveryOrder_Dtl Get_SalesDeliveryOrderDetailPrintByID_DO_ttl(int? DOMaster_ID)
        {



            string sp = "Select_SalesDetailPrint_ByID_DO";
            Pa_SalesDeliveryOrder_Dtl item = new Pa_SalesDeliveryOrder_Dtl();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DOMaster_ID", DOMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {


                        item.DODetails_ID = Convert.ToInt32(rdr["DODetails_ID"]);

                        item.Unit_Price = rdr["Unit_Price"].ToString();

                        item.VATRate = rdr["VATRate"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();       

                        item.Amount = rdr["Amount"].ToString();
                        item.Quantity = rdr["Quantity"].ToString();
                        item.Total_Amt_inWords = rdr["Total_Amt_inWords"].ToString();
 




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
        #endregion

        #region SalesInventoryReturn

        public pa_SalesMaster_DAL Get_SalesMaster_ByID_DAL_Return(int? SaleMaster_ID)
        {

            string sp = "Select_SaleInvoice_Return";

            pa_SalesMaster_DAL pm = new pa_SalesMaster_DAL();
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
                                pm.SaleMaster_ID = Convert.ToInt32(dr["SaleMaster_ID"]);
                                pm.Customer_ID = dr["Customer_ID"].ToString();
                                pm.SaleRef = dr["SaleRef"].ToString();
                                pm.Customer_Contact = dr["Customer_Contact"].ToString();
                                pm.Invoice_address = dr["Invoice_address"].ToString();
                                pm.SaleDate = dr["SaleDate"].ToString();



                                pm.CustomerTRN = dr["CustomerTRN"].ToString();
                                pm.Manualbook_ref = dr["Manualbook_ref"].ToString();
                                pm.Seller_ID = dr["Seller_ID"].ToString();






                                pm.Created_at = dr["Created_at"].ToString();
                                pm.Created_by = dr["Created_by"].ToString();
                                pm.Modified_at = dr["Modified_at"].ToString();
                                pm.Modified_by = dr["Modified_by"].ToString();
                                pm.Remarks = dr["Remarks"].ToString();
                                pm.Customer_Name = dr["Customer_Name"].ToString();
                                pm.SaleMasterRef_ID = Convert.ToInt32(dr["SaleMasterRef_ID"]);

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
        public IEnumerable<pa_SalesDetails> Get_SalesDetailListByID_DAL_Return(string Temp_ID, int? SaleMaster_ID)
        {



            string sp = "Select_SaleDetailInv_ByItem_Return";
            List<pa_SalesDetails> itemlist = new List<pa_SalesDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@SaleMaster_ID", SaleMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesDetails item = new pa_SalesDetails();

                        item.SalesDetails_ID = Convert.ToInt32(rdr["SalesDetails_ID"]);

                        item.Unit_Price = rdr["Unit_Price"].ToString();

                        item.VATRate = rdr["VATRate"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();

                        item.ItemDesc = rdr["ItemDesc"].ToString();


                        item.Stock_ID = rdr["Stock_ID"].ToString();


                        item.Grand_UnitPrice = rdr["Grand_UnitPrice"].ToString();
                        item.Grand_Amount = rdr["Grand_Amount"].ToString();
                        item.Grand_TotalAmount = rdr["Grand_TotalAmount"].ToString();
                        item.Grand_VATExp = rdr["Grand_VATExp"].ToString();
                        item.Total_Amt_inWords = rdr["Total_Amt_inWords"].ToString();
                        item.ItemCode = rdr["ItemId"].ToString();
                        item.Item_ID = Convert.ToInt32(rdr["Item_ID"]);

                        item.Location_ID = rdr["Location_ID"].ToString();

                        item.Quantity = rdr["Quantity"].ToString();
                        item.UM_ID = rdr["UM_ID"].ToString();
                        item.ItemName = rdr["ItemName"].ToString();
                        item.SaleQty = rdr["SaleQty"].ToString();
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
        public string InsertSalesDetail_DAL_Return(int Item_ID, string ItemDesc, double? Unit_Price, double?
        VATRate, double? VATExp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int SaleMaster_ID, int SaleMasterRef_ID, string Quantity, string UM_ID, int Location_ID, int SaleDetails_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                           new SqlParameter("@Item_ID",Item_ID),

                          new SqlParameter("@ItemDesc",ItemDesc),
                           new SqlParameter("@Unit_Price",Unit_Price),
                            new SqlParameter("@VATRate",VATRate),
                             new SqlParameter("@VATExp",VATExp),
                              new SqlParameter("@Total_Amount",Total_Amount),

                               new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@c_ID",c_ID),
                                 new SqlParameter("@Created_by",Created_by),
                                  new SqlParameter("@SaleMaster_ID",SaleMaster_ID),
                                  new SqlParameter("@SaleMasterRef_ID",SaleMasterRef_ID),

                                  new SqlParameter("@Quantity",Quantity),
                                  new SqlParameter("@UM_ID",UM_ID),
                                  new SqlParameter("@SaleDetails_ID",SaleDetails_ID),
                                  new SqlParameter("@Location_ID",Location_ID)



            };


                var response = dbLayer.SP_DataTable_return("InsertSalesDetail_Return", paramArray).Tables[0];
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





        public string InsertSalesMaster_DAL_Return(int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string CustomerTRN,
        string Manualbook_ref, int? Seller_ID,
        string Temp_ID, int? c_ID, int? Created_by, string Remarks, string Customer_Name,string SaleType, int SaleMasterRef_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                 new SqlParameter("@Customer_ID",Customer_ID),
                new SqlParameter("@Customer_Contact",Customer_Contact),
                new SqlParameter("@Invoice_address",Invoice_address),
                  new SqlParameter("@SaleDate",SaleDate),
             new SqlParameter("@CustomerTRN",CustomerTRN),
 new SqlParameter("@Manualbook_ref",Manualbook_ref),
  new SqlParameter("@Seller_ID",Seller_ID),

                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Created_by",Created_by),
                 new SqlParameter("@Customer_Name",Customer_Name),
                 new SqlParameter("@Remarks",Remarks),
                 new SqlParameter("@SaleType",SaleType),

                 new SqlParameter("@SaleMasterRef_ID",SaleMasterRef_ID)






            };


                var response = dbLayer.SP_DataTable_return("Pa_Insert_SaleMaster_Return", paramArray).Tables[0];
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

        public string UpdateSalesMaster_DAL_Return(int SaleMaster_ID, int? SaleMasterRef_ID, int? Customer_ID, string Customer_Contact, string Invoice_address, string SaleDate, string CustomerTRN,
        string Manualbook_ref, int? Seller_ID,
        string Temp_ID, int? c_ID, int? Modified_by, string Remarks, string Customer_Name)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
  new SqlParameter("@SaleMaster_ID",SaleMaster_ID),
  new SqlParameter("@SaleMasterRef_ID",SaleMasterRef_ID),
                new SqlParameter("@Customer_ID",Customer_ID),
                new SqlParameter("@Customer_Contact",Customer_Contact),
                new SqlParameter("@Invoice_address",Invoice_address),

                  new SqlParameter("@SaleDate",SaleDate),

                    new SqlParameter("@CustomerTRN",CustomerTRN),
 new SqlParameter("@Manualbook_ref",Manualbook_ref),
  new SqlParameter("@Seller_ID",Seller_ID),





                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Modified_by",Modified_by),
                   new SqlParameter("@Customer_Name",Customer_Name),
                 new SqlParameter("@Remarks",Remarks)





            };


                var response = dbLayer.SP_DataTable_return("Pa_Update_SaleMaster_Return", paramArray).Tables[0];
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


        public string DeleteSalesMaster_DAL_Return(int? SaleMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@SaleMaster_ID",SaleMaster_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_SaleMaster_Return", paramArray).Tables[0];
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
        public string DeleteSalesDetail_DAL_Return(int? SalesDetails_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@SalesDetails_ID",SalesDetails_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_Dtl_Return", paramArray).Tables[0];
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

        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMaster_DAL_Return(string SaleRef, string StartDate, string EndDate, string Customer_Name, string ItemId, int Status_ID,
        int c_ID)
        {


            string sp = "Select_SaleMasterInvoiceList_Return";
            List<pa_SalesMaster_DAL> itemlist = new List<pa_SalesMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    cmd.Parameters.AddWithValue("@ItemId", ItemId);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesMaster_DAL item = new pa_SalesMaster_DAL();

                        item.SaleMaster_ID = Convert.ToInt32(rdr["SaleMaster_ID"]);
                        item.SaleRef = rdr["SaleRef"].ToString();
                        item.SaleDate = rdr["SaleDate"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amt = rdr["Total_Amt"].ToString();
                        item.ReturnRef = rdr["ReturnRef"].ToString();

                        item.Customer_Name = rdr["CustomerName"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.ApprovalStatus = rdr["Approval_Status"].ToString();



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




        public string GetOldTempIDFromSalesDetail_DAL_Return(int? SaleMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@SaleMaster_ID",SaleMaster_ID)

            };


                var response = dbLayer.SP_DataTable_return("SelectDetailOldTempID_Return", paramArray).Tables[0];
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

        public IEnumerable<pa_SalesDetails> GetSaleRefDetails_Other_DAL_Return()
        {


            string sp = "GetSaleRef";
            List<pa_SalesDetails> itemlist = new List<pa_SalesDetails>();

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
                        pa_SalesDetails item = new pa_SalesDetails();

                        item.SaleMaster_ID = Convert.ToInt32(rdr["SaleMaster_ID"]);
                        item.SaleRef = rdr["SaleRef"].ToString();



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


        public IEnumerable<pa_SalesDetails> Get_SalesDetailListByRef_DAL_Return(int? SaleMaster_ID)
        {



            string sp = "Select_SaleDetailInv_BySaleRef_Return";
            List<pa_SalesDetails> itemlist = new List<pa_SalesDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SaleMasterRef_ID", SaleMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesDetails item = new pa_SalesDetails();

                        item.SalesDetails_ID = Convert.ToInt32(rdr["SalesDetails_ID"]);
                        item.SaleMaster_ID = Convert.ToInt32(rdr["SaleMaster_ID"]);

                        item.Unit_Price = rdr["Unit_Price"].ToString();

                        item.VATRate = rdr["VATRate"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();

                        item.ItemDesc = rdr["ItemDesc"].ToString();


                        item.Stock_ID = rdr["Stock_ID"].ToString();


                        item.Grand_UnitPrice = rdr["Grand_UnitPrice"].ToString();
                        item.Grand_Amount = rdr["Grand_Amount"].ToString();
                        item.Grand_TotalAmount = rdr["Grand_TotalAmount"].ToString();
                        item.Grand_VATExp = rdr["Grand_VATExp"].ToString();
                        item.Total_Amt_inWords = rdr["Total_Amt_inWords"].ToString();
                        item.ItemCode = rdr["ItemId"].ToString();
                        item.Item_ID = Convert.ToInt32(rdr["Item_ID"]);

                        item.Location_ID = rdr["Location_ID"].ToString();

                        item.Quantity = rdr["Quantity"].ToString();
                        item.UM_ID = rdr["UM_ID"].ToString();
                        item.ItemName = rdr["ItemName"].ToString();
                        item.SaleQty = rdr["SaleQty"].ToString();
                        item.ReturnQty = rdr["ReturnQty"].ToString();





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
        #region PaperAfertSalesJp

        //Insert  purchase return detail List
        public string Insert_paperAfterSaleDetails_jp(string Temp_ID, string SaleRef,int hdpapers_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {


                               new SqlParameter("@Temp_ID",Temp_ID),
                               new SqlParameter("@hdpapers_ID",hdpapers_ID),
                                new SqlParameter("@SaleRef",SaleRef)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Get_paperAfterSaleDetails_jp", paramArray).Tables[0];
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


        public string Insert_paperAfterSaleDetails_Chassis_jp(string Temp_ID, string Chassis, int hdpapers_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {


                               new SqlParameter("@Temp_ID",Temp_ID),
                               new SqlParameter("@hdpapers_ID",hdpapers_ID),
                                new SqlParameter("@Chassis",Chassis)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Get_paperAfterSaleDetails_Chassis_jp", paramArray).Tables[0];
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

        //Check If sale Ref Exist In SalesMaster table
        public string CheckIfRefExistInSalesMaster_DAL(string SaleRef)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@SaleRef",SaleRef)



            };


                var response = dbLayer.SP_DataTable_return("Check_If_SaleRefExists", paramArray).Tables[0];
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
        public string CheckIfRefExistInSalesMaster_jp_DAL(string SaleRef)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@SaleRef",SaleRef)



            };


                var response = dbLayer.SP_DataTable_return("Check_If_SaleRefExists_jp", paramArray).Tables[0];
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
        public string CheckIfRefExistInChassis_DAL(string Chassis)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Chassis",Chassis)



            };


                var response = dbLayer.SP_DataTable_return("Check_If_ChassisExists_jp", paramArray).Tables[0];
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
        public PaperAfterSalesMaster_DAL Get_Pa_Select_paperAfterSaleMaster_jp_ByID(int? papers_ID)
        {

            string sp = "Pa_Select_paperAfterSaleMaster_jp_ByID";

            PaperAfterSalesMaster_DAL pm = new PaperAfterSalesMaster_DAL();
         
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@papers_ID", papers_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.papers_ID = Convert.ToInt32(dr["papers_ID"]);
                                pm.paperRef = dr["paperRef"].ToString();
                                pm.DateCreated = dr["DateCreated"].ToString();
                                pm.DateSubmission = dr["DateSubmission"].ToString();
                                pm.DateValid = dr["DateValid"].ToString();
                                pm.Remarks = dr["Remarks"].ToString();
                         

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

        public PaperAfterSalesMaster_DAL Get_Pa_Select_paperAfterSaleMaster_jp_ByID_print(int? papers_ID)
        {

            string sp = "Pa_Select_paperAfterSaleMaster_jp_ByID_Print";

            PaperAfterSalesMaster_DAL pm = new PaperAfterSalesMaster_DAL();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@papers_ID", papers_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.papers_ID = Convert.ToInt32(dr["papers_ID"]);
                                pm.paperRef = dr["paperRef"].ToString();
                                pm.DateCreated = dr["DateCreated"].ToString();
                                pm.DateSubmission = dr["DateSubmission"].ToString();
                                pm.DateValid = dr["DateValid"].ToString();
                                pm.Remarks = dr["Remarks"].ToString();


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
        public IEnumerable<PaperAfterSalesDetail_DAL> Get_paperAfterSaleDetails_DAL(string Temp_ID, int? papers_ID)
        {



            string sp = "Pa_Select_paperAfterSaleDetails_jp_ByID";
            List<PaperAfterSalesDetail_DAL> itemlist = new List<PaperAfterSalesDetail_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@papers_ID", papers_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        PaperAfterSalesDetail_DAL item = new PaperAfterSalesDetail_DAL();

                        item.ID = Convert.ToInt32(rdr["ID"]);
                        item.papers_ID = Convert.ToInt32(rdr["papers_ID"]);
                        item.SaleMaster_ID = Convert.ToInt32(rdr["SaleMaster_ID"]);
                        item.Stock_ID = Convert.ToInt32(rdr["Stock_ID"]);
                        item.Chassis_No = rdr["Chassis_No"].ToString();
                        item.CustomerName = rdr["CustomerName"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.Price = rdr["Price"].ToString();

                      


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
        public string Insert_paperAfterSaleMaster_Return_DAL(string DateCreated, string DateSubmission, string DateValid, string Remarks, string Temp_ID, int? Created_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@DateCreated",DateCreated),
                new SqlParameter("@DateSubmission",DateSubmission),
                new SqlParameter("@DateValid",DateValid),
                new SqlParameter("@Remarks",Remarks),
                 new SqlParameter("@Temp_ID",Temp_ID),
                 new SqlParameter("@Created_by",Created_by)




            };


                var response = dbLayer.SP_DataTable_return("Pa_Insert_paperAfterSaleMaster_jp", paramArray).Tables[0];
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
        public string Update_paperAfterSaleMaster_DAL(int? papers_ID, string DateCreated, string DateSubmission, string DateValid, string Remarks, string Temp_ID, int? Modified_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@papers_ID",papers_ID),
                new SqlParameter("@DateSubmission",DateSubmission),
                new SqlParameter("@DateValid",DateValid),
                new SqlParameter("@Remarks",Remarks),
                 new SqlParameter("@Temp_ID",Temp_ID),
                 new SqlParameter("@Modified_by",Modified_by)




            };


                var response = dbLayer.SP_DataTable_return("Update_paperAfterSaleMaster_jp", paramArray).Tables[0];
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
        public IEnumerable<PaperAfterSalesMaster_DAL> Get_PaperAfterSalesMasterList_DAL(string Chassis_No, string EndDate, string StartDate, string SaleRef)
        {


            string sp = "Pa_Select_paperAfterSaleMaster_jp_List";
            List<PaperAfterSalesMaster_DAL> itemlist = new List<PaperAfterSalesMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@SaleRef", SaleRef);

                    SqlDataReader dr = cmd.ExecuteReader();


                    while (dr.Read())
                    {
                        PaperAfterSalesMaster_DAL pm = new PaperAfterSalesMaster_DAL();


                        pm.papers_ID = Convert.ToInt32(dr["papers_ID"]);
                        pm.paperRef = dr["paperRef"].ToString();
                        pm.DateCreated = dr["DateCreated"].ToString();
                        pm.DateSubmission = dr["DateSubmission"].ToString();
                        pm.DateValid = dr["DateValid"].ToString();
                        pm.Remarks = dr["Remarks"].ToString();



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
        public string GetOldTempIDFrom_DAL(int? papers_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@papers_ID",papers_ID)

            };


                var response = dbLayer.SP_DataTable_return("Pa_SelectDetailOldTempID", paramArray).Tables[0];
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
        public IEnumerable<PaperAfterSalesDetail_DAL> Select_AfterSaleDetailList_DAL( int? papers_ID)
        {



            string sp = "Select_AfterSaleDetailList";
            List<PaperAfterSalesDetail_DAL> itemlist = new List<PaperAfterSalesDetail_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@papers_ID", papers_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        PaperAfterSalesDetail_DAL item = new PaperAfterSalesDetail_DAL();



                        item.Chassis_No = rdr["Chassis_No"].ToString();

                        item.Registration = rdr["Registration"].ToString();
                        



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
        public string DeleteaperAfterSaleDetails_DAL(int? papers_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@papers_ID",papers_ID)



            };


                var response = dbLayer.SP_DataTable_return("Pa_Delete_paperAfterSaleDetails", paramArray).Tables[0];
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


        #region SalesDashboard
        public IEnumerable<pa_Select_SalesDashboardList> Pa_Select_SaleDashboard(int? Make, int? Model_Desc, int? Color, string StartDate, string EndDate, string Model_Year, int? c_id)
        {
           
            con = new SqlConnection(Constr);
            string sp = "Select_SalesDashboardList";
            List<pa_Select_SalesDashboardList> itemlist = new List<pa_Select_SalesDashboardList>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Make", Make);
                    cmd.Parameters.AddWithValue("@Model_Desc", Model_Desc);
                    cmd.Parameters.AddWithValue("@Color", Color);
                    cmd.Parameters.AddWithValue("@Start_Date", StartDate);
                    cmd.Parameters.AddWithValue("@End_Date", EndDate);
                    cmd.Parameters.AddWithValue("@Model_year", Model_Year);
                    cmd.Parameters.AddWithValue("@c_ID", c_id);
                   
                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_Select_SalesDashboardList item = new pa_Select_SalesDashboardList();
                        //VATExp    Total_Amt    Paid_amt   Bal_due   SaleStatus   Modified_at   CustomerName   


                        item.Make = rdr["Make"].ToString();
                        item.Model_Desc = rdr["ModelDesciption"].ToString();
                        item.Model_Year = rdr["ModelYear"].ToString();
                        item.Qty = Convert.ToInt32(rdr["Qty"].ToString());
                        item.Color_Interior_Name = rdr["Color_Interior_Name"].ToString();
                        item.Color_Exterior_Name = rdr["Color_Exterior_Name"].ToString();
                        //item.Total_Cost = Convert.ToDecimal(rdr["Price"].ToString());
                        item.Production_Date = rdr["Production_Date"].ToString();

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

        public IEnumerable<pa_Select_SalesDashboardList> pa_SelectSaleDetails(string Make, string Model_Desc, string Model_Year, string Ext_Color, string Production_Date,int C_id)
        {
            con = new SqlConnection(Constr);
            string sp = "Select_SaleDashboard_Details";
            List<pa_Select_SalesDashboardList> itemlist = new List<pa_Select_SalesDashboardList>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Make", Make);
                    cmd.Parameters.AddWithValue("@Model_Desc", Model_Desc);
                    cmd.Parameters.AddWithValue("@model_Year", Model_Year);
                    cmd.Parameters.AddWithValue("@Ext_Color", Ext_Color);
                    cmd.Parameters.AddWithValue("@Production_Date", Production_Date);
                    
                    cmd.Parameters.AddWithValue("@c_id", C_id);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_Select_SalesDashboardList item = new pa_Select_SalesDashboardList();
                        //VATExp    Total_Amt    Paid_amt   Bal_due   SaleStatus   Modified_at   CustomerName   


                        item.Stock_Id = Convert.ToInt32(rdr["stock_ID"]);
                        item.chassis = rdr["Chassis_No"].ToString();
                        item.Color_Interior_Name = rdr["Color_Interior_Name"].ToString();
                        item.Color_Exterior_Name = rdr["Color_Exterior_Name"].ToString();
                        item.Total_Cost = Convert.ToDecimal(rdr["Purchase_Price"].ToString());
                        item.Production_Date = rdr["Production_Date"].ToString();
                        item.C_id = Convert.ToInt32(rdr["c_ID"]);

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


        //BY Yaseen 1-24-2022
        #region Payment Details
        public IEnumerable<Pa_PaymentDetails_DAL> PaymentDetailList { get; set; }
        public IEnumerable<Pa_PaymentDetails_DAL> Select_PaymentList_PVSV_ByID_DAL(string Temp_ID, int? SaleMaster_ID)
        {

            string sp = "Select_PaymentList_PVSV_ByID";
            List<Pa_PaymentDetails_DAL> itemlist = new List<Pa_PaymentDetails_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@SaleMaster_ID", SaleMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_PaymentDetails_DAL item = new Pa_PaymentDetails_DAL();

                        item.PaymentDetails_ID = Convert.ToInt32(rdr["PaymentDetails_ID"]);
                        item.PaymentMaster_ID = Convert.ToInt32(rdr["PaymentMaster_ID"]);
                        item.CR_accountID = rdr["CR_accountID"].ToString();
                        item.DR_accountID = rdr["DR_accountID"].ToString();
                        item.PaymentDate = rdr["PaymentDate"].ToString();
                        item.Currency_Rate = rdr["Currency_Rate"].ToString();
                        item.Currency_ID = rdr["Currency_ID"].ToString();
                        item.Amount_Others = rdr["Amount_Others"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.Description = rdr["Description"].ToString();
                        item.VAT_Rate = rdr["VAT_Rate"].ToString();
                        item.VAT_Exp = rdr["VAT_Exp"].ToString();
                        item.cheque_no = rdr["cheque_no"].ToString();
                        item.cheque_date = rdr["cheque_date"].ToString();
                        item.cheque_bank_name = rdr["cheque_bank_name"].ToString();
                        item.stock_ID = rdr["stock_ID"].ToString();
                        item.trans_ref = rdr["trans_ref"].ToString();
                        item.CR_AcountName = rdr["CR_AcountName"].ToString();
                        item.DR_AccountName = rdr["DR_AccountName"].ToString();

                        //item.AmountTotal = rdr["AmountTotal"].ToString();
                        //item.VatExpTotal = rdr["VatExpTotal"].ToString();
                        //item.OtherAmountTotal = rdr["OtherAmountTotal"].ToString();
                        //item.Grand_Total = rdr["Grand_Total"].ToString();





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

        public string InsertPaymentDetail_PVSV_DAL(int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description, string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int hdSaleMaster_ID)
        {
            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@DR_accountID",DR_accountID),
                  new SqlParameter("@Amount",Amount),
                   new SqlParameter("@VAT_Rate",VAT_Rate),
                    new SqlParameter("@Currency_ID",Currency_ID),
                   new SqlParameter("@Currency_Rate",Currency_Rate),
                    new SqlParameter("@Amount_Others",Amount_Others),
                     new SqlParameter("@CR_accountID",CR_accountID),
                      new SqlParameter("@Description",Description),
                       new SqlParameter("@trans_ref",trans_ref),

                         new SqlParameter("@cheque_bank_name",cheque_bank_name),
                          new SqlParameter("@cheque_date",cheque_date),
                           new SqlParameter("@cheque_no",cheque_no),
                            new SqlParameter("@VAT_Exp",VAT_Exp),
                             new SqlParameter("@Total_Amount",Total_Amount),

                               new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@c_ID",c_ID),
                                 new SqlParameter("@Created_by",Created_by),
                                 new SqlParameter("@link_trans_ref_ID", hdSaleMaster_ID)



            };


                var response = dbLayer.SP_DataTable_return("InsertPaymentDetails_Sales_PVSV", paramArray).Tables[0];
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

        public string UpdateSalesPayment_PVSV_DAL(int? PaymentDetails_ID, int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description, string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                    new SqlParameter("@PaymentDetails_ID",PaymentDetails_ID),
                new SqlParameter("@DR_accountID",DR_accountID),
                  new SqlParameter("@Amount",Amount),
                   new SqlParameter("@VAT_Rate",VAT_Rate),
                    new SqlParameter("@Currency_ID",Currency_ID),
                   new SqlParameter("@Currency_Rate",Currency_Rate),
                    new SqlParameter("@Amount_Others",Amount_Others),
                     new SqlParameter("@CR_accountID",CR_accountID),
                      new SqlParameter("@Description",Description),
                       new SqlParameter("@trans_ref",trans_ref),

                         new SqlParameter("@cheque_bank_name",cheque_bank_name),
                          new SqlParameter("@cheque_date",cheque_date),
                           new SqlParameter("@cheque_no",cheque_no),
                            new SqlParameter("@VAT_Exp",VAT_Exp),
                             new SqlParameter("@Total_Amount",Total_Amount),

                              new SqlParameter("@Temp_ID",Temp_ID),

                                new SqlParameter("@c_ID",c_ID),
                                 new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("UpdatePaymentDetails_Sales_PVSV", paramArray).Tables[0];
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

        public string DeletePaymentDetail_DAL(int? PaymentDetails_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@PaymentDetails_ID",PaymentDetails_ID)


            };


                var response = dbLayer.SP_DataTable_return("Delete_PaymentDetailForSaleInvoice", paramArray).Tables[0];
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



        #endregion


        public IEnumerable<pa_SalesMaster_DAL> Get_SalesMaster_DAL_Return_trd(string SaleRef, string StartDate, string EndDate, string Customer_Name, string ItemId, int Status_ID, int c_ID)
        {
            string sp = "Select_SaleMasterInvoiceList_Return_TRD";
            List<pa_SalesMaster_DAL> itemlist = new List<pa_SalesMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Customer_Name", Customer_Name);
                    cmd.Parameters.AddWithValue("@ItemId", ItemId);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_SalesMaster_DAL item = new pa_SalesMaster_DAL();

                        //item.SaleMaster_ID = Convert.ToInt32(rdr["SaleMaster_ID"]);
                        //item.SaleRef = rdr["SaleRef"].ToString();
                        //item.SaleDate = rdr["SaleDate"].ToString();
                        //item.Amount = rdr["Amount"].ToString();
                        //item.VATExp = rdr["VATExp"].ToString();
                        //item.Total_Amt = rdr["Total_Amt"].ToString();
                        item.ReturnRef = rdr["SaleRef"].ToString();

                        //item.Customer_Name = rdr["CustomerName"].ToString();
                        //item.Modified_at = rdr["Modified_at"].ToString();
                        //item.ApprovalStatus = rdr["Approval_Status"].ToString();

                        item.SaleMaster_ID = Convert.ToInt32(rdr["SaleMaster_ID"]);
                        item.SaleRef = rdr["SaleRef"].ToString();
                        item.SaleDate = rdr["SaleDate"].ToString();
                        item.Amount = rdr["Amount"].ToString();
                        item.VATExp = rdr["VATExp"].ToString();
                        item.Total_Amt = rdr["Total_Amt"].ToString();
                        item.Paid_amt = rdr["Paid_amt"].ToString();
                        item.Bal_due = rdr["Bal_due"].ToString();
                        item.Customer_Name = rdr["Customer_Name"].ToString();
                        item.SaleStatus = rdr["Status"].ToString();
                        item.Status = rdr["Status"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.ApprovalStatus = rdr["Approval_Status"].ToString();

                        item.Manualbook_ref = rdr["Manualbook_ref"].ToString();
                        item.Other_Status = rdr["Other_Status"].ToString();
                        item.Tip = rdr["Tip"].ToString();


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
