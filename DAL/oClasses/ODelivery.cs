using DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using X.PagedList;

namespace DAL.oClasses
{
    public class ODelivery : IODelivery
    {
        private readonly IConnectionStringManager _conMgr;
        private readonly ISqlHelper dbLayer;
        SqlConnection con;

        public ODelivery(IConnectionStringManager conMgr, ISqlHelper sqlHelp)
        {

            _conMgr = conMgr;
            //---Getting Connection String
            dbLayer = sqlHelp;
            Constr = _conMgr.GetConnectionString();
        }


        //New Faraz Work
        public IEnumerable<Deposits_DAL> DepositsDetailListIPageList1 { get; set; }

        public IEnumerable<Deposits_DAL> DepositsDetailListIPageList1_1 { get; set; }

        //---This variable is for saving connection string.
        public string Constr { get; set; }

        public ChassisValidation_DAL ChassisValidationObj { get; set; }
        public IEnumerable<Deposits_DAL> DepositsDetailList { get; set; }
        public IPagedList<Deposits_DAL> DepositsDetailListIPageList { get; set; }
        public Deposits_DAL DepositMasterObj { get; set; }
        public IEnumerable<pa_ReceiptDetails_DAL> receiptDetailList { get; set; }
        public IEnumerable<Pa_PaymentDetails_DAL> paymentDetailList { get; set; }

        public IEnumerable<Pa_Attachments_DAL> attachmentList { get; set; }





        //this method is for validating chassis no 
        public ChassisValidation_DAL ValidateChassisNoForDeposit_DAL(string ChassisNo)
        {


            string sp = "get_Valid_Chassis_For_DV";
            ChassisValidation_DAL ChsValid = new ChassisValidation_DAL();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Chassis_No", ChassisNo);
                    SqlDataReader record = cmd.ExecuteReader();


                    if (record.Read())
                    {

                        ChsValid.Stock_ID = record["stock_ID"].ToString();
                        ChsValid.CustomerName = record["CustomerName"].ToString();
                        ChsValid.ContactNumber = record["ContactNumber"].ToString();
                        ChsValid.ExportTo_Destination_ID = record["ExportTo_Destination_ID"].ToString();
                        ChsValid.Port_of_Exit_ID = record["Port_of_Exit_ID"].ToString();
                        ChsValid.Customer_ID = record["Customer_ID"].ToString();
                        ChsValid.Message = "Chassis Exists";
                        //result = true;




                    }

                    con.Close();

                }



            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }

            return ChsValid;
        }

        //method for inserting into deposit voucher detail
        public string InsertDepositVoucherDetail_DAL(int? Stock_ID, double? Car_Sale_Value, double? Deposit, double? Fix_Deductable_Charges, double? PaperExpense_Changes,
            double? VAT_Security_Deposit, double? Other_Charges, string External_Trans_Ref, string ShippingDate, string Submit_Date, string Temp_ID, int? c_ID, int? Created_by, int DV_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Stock_ID",Stock_ID),
                 new SqlParameter("@Car_Sale_Value",Car_Sale_Value),
                  new SqlParameter("@Deposit",Deposit),
                   new SqlParameter("@Fix_Deductable_Charges",Fix_Deductable_Charges),
                    new SqlParameter("@PaperExpense_Changes",PaperExpense_Changes),
                     new SqlParameter("@VAT_Security_Deposit",VAT_Security_Deposit),
                      new SqlParameter("@Other_Charges",Other_Charges),
                       new SqlParameter("@External_Trans_Ref",External_Trans_Ref),
                        new SqlParameter("@ShipDate",ShippingDate),
                         new SqlParameter("@Submission_Date",Submit_Date),
                         new SqlParameter("@Temp_ID",Temp_ID),
                          new SqlParameter("@c_ID",c_ID),
                           new SqlParameter("@Created_by",Created_by),
                           new SqlParameter("@DV_ID",DV_ID)




            };


                var response = dbLayer.SP_DataTable_return("Insert_DvDetails", paramArray).Tables[0];
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


        //method for update into deposit voucher detail
        public string UpdateDepositVoucherDetail_DAL(int? DVdetails_ID, int? Stock_ID, double? Car_Sale_Value, double? Deposit, double? Fix_Deductable_Charges, double? PaperExpense_Changes,
            double? VAT_Security_Deposit, double? Other_Charges, string External_Trans_Ref, string ShippingDate, string Submit_Date, string Temp_ID, int? c_ID, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                       new SqlParameter("@DVdetails_ID",DVdetails_ID),
                new SqlParameter("@Stock_ID",Stock_ID),
                 new SqlParameter("@Car_Sale_Value",Car_Sale_Value),
                  new SqlParameter("@Deposit",Deposit),
                   new SqlParameter("@Fix_Deductable_Charges",Fix_Deductable_Charges),
                    new SqlParameter("@PaperExpense_Changes",PaperExpense_Changes),
                     new SqlParameter("@VAT_Security_Deposit",VAT_Security_Deposit),
                      new SqlParameter("@Other_Charges",Other_Charges),
                       new SqlParameter("@External_Trans_Ref",External_Trans_Ref),
                        new SqlParameter("@ShipDate",ShippingDate),
                         new SqlParameter("@Submission_Date",Submit_Date),
                         new SqlParameter("@Temp_ID",Temp_ID),
                          new SqlParameter("@c_ID",c_ID),
                           new SqlParameter("@Modified_by",Modified_by)




            };


                var response = dbLayer.SP_DataTable_return("Update_DvDetails", paramArray).Tables[0];
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





        //Get deposit voucher received list by id
        public IEnumerable<Deposits_DAL> Get_DepositVoucherReceivedListByID_DAL(string Temp_ID, int? DV_ID)
        {



            string sp = "Select_DepositVoucherDetailsListByID";
            List<Deposits_DAL> itemlist = new List<Deposits_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@DV_ID", DV_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Deposits_DAL item = new Deposits_DAL();

                        item.DVdetails_ID = Convert.ToInt32(rdr["DVdetails_ID"]);
                        item.Stock_ID = Convert.ToInt32(rdr["Stock_ID"]);
                        item.Chassis_No = rdr["Chassis_No"].ToString();
                        item.Car_Sale_Value = rdr["Car_Sale_Value"].ToString();
                        item.Deposit = rdr["Deposit"].ToString();
                        item.Fix_Deductable_Charges = rdr["Fix_Deductable_Charges"].ToString();

                        item.PaperExpense_Changes = rdr["PaperExpense_Changes"].ToString();
                        item.VAT_Security_Deposit = rdr["VAT_Security_Deposit"].ToString();
                        item.Other_Charges = rdr["Other_Charges"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();
                        item.External_Trans_Ref = rdr["External_Trans_Ref"].ToString();
                        item.Submission_Date = rdr["Submission_Date"].ToString();

                        item.ShipDate = rdr["ShipDate"].ToString();


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


        //--getting payment master by id
        public Deposits_DAL Get_DepositVoucherMasterByID_DAL(int? DV_ID)
        {

            string sp = "Select_DepositVoucherMasterByID";

            Deposits_DAL pm = new Deposits_DAL();
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
                                pm.DV_ID = Convert.ToInt32(dr["DV_ID"]);
                                pm.DV_Ref = dr["DV_Ref"].ToString();
                                pm.Date_Taken = dr["Date_Taken"].ToString();
                                pm.Customer_ID = dr["Customer_ID"].ToString();
                                pm.CUSTOMER_NAME = dr["CUSTOMER_NAME"].ToString();
                                pm.CUSTOMER_CONTACT = dr["CUSTOMER_CONTACT"].ToString();
                                pm.export_to_Destination_ID = Convert.ToInt32(dr["export_to_Destination_ID"]);

                                pm.port_ID = Convert.ToInt32(dr["port_ID"]);

                                pm.refund_Contact = dr["refund_Contact"].ToString();
                                pm.refund_Customer = dr["refund_Customer"].ToString();

                                pm.Date_Return = dr["Date_Return"].ToString();


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


        // Get Old TempID from deposit detail
        public string GetOldTempIDFromDepositDetail_DAL(int? DV_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@DV_ID",DV_ID)

            };


                var response = dbLayer.SP_DataTable_return("SelectDepositDetailOldTempID", paramArray).Tables[0];
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


        // insert deposit voucher master
        public string InsertDepositVoucherMaster_DAL(int? Customer_ID, string Customer_Name, string Customer_Contact, string refund_Customer, string refund_Contact, int? export_to_Destination_ID,
            int? port_ID, string Date_Return, string Date_Taken, string Temp_ID, int? c_ID, int? Created_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Customer_ID",Customer_ID),
                 new SqlParameter("@Customer_Name",Customer_Name),
                  new SqlParameter("@Customer_Contact",Customer_Contact),
                   new SqlParameter("@refund_Customer",refund_Customer),
                     new SqlParameter("@refund_Contact",refund_Contact),
                    new SqlParameter("@export_to_Destination_ID",export_to_Destination_ID),
                     new SqlParameter("@port_ID",port_ID),
                      new SqlParameter("@Date_Return",Date_Return),
                       new SqlParameter("@Date_Taken",Date_Taken),
                        new SqlParameter("@Temp_ID",Temp_ID),
                            new SqlParameter("@c_ID",c_ID),
                                new SqlParameter("@Created_by",Created_by)

            };


                var response = dbLayer.SP_DataTable_return("Insert_DepositMaster", paramArray).Tables[0];
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



        // update deposit voucher master
        public string Update_DepositVoucherMaster_DAL(int? DV_ID, int? Customer_ID, string Customer_Name, string Customer_Contact, string refund_Customer, string refund_Contact, int? export_to_Destination_ID,
            int? port_ID, string Date_Return, string Date_Taken, string Temp_ID, int? c_ID, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                       new SqlParameter("@DV_ID",DV_ID),
                new SqlParameter("@Customer_ID",Customer_ID),
                 new SqlParameter("@Customer_Name",Customer_Name),
                  new SqlParameter("@Customer_Contact",Customer_Contact),
                   new SqlParameter("@refund_Customer",refund_Customer),
                     new SqlParameter("@refund_Contact",refund_Contact),
                    new SqlParameter("@export_to_Destination_ID",export_to_Destination_ID),
                     new SqlParameter("@port_ID",port_ID),
                      new SqlParameter("@Date_Return",Date_Return),
                       new SqlParameter("@Date_Taken",Date_Taken),
                        new SqlParameter("@Temp_ID",Temp_ID),
                            new SqlParameter("@c_ID",c_ID),
                                new SqlParameter("@Modified_by",Modified_by)

            };


                var response = dbLayer.SP_DataTable_return("Update_DepositMaster", paramArray).Tables[0];
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


        // insert/update deposit return voucher master
        public string Insert_DepositReturnMaster_DAL(int? DV_ID, int? Customer_ID, string refund_Customer, string refund_Contact, string Date_Return, string Temp_ID, int? c_ID, int? Modified_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                       new SqlParameter("@DV_ID",DV_ID),
                new SqlParameter("@Customer_ID",Customer_ID),

                   new SqlParameter("@refund_Customer",refund_Customer),
                     new SqlParameter("@refund_Contact",refund_Contact),

                      new SqlParameter("@Date_Return",Date_Return),

                        new SqlParameter("@Temp_ID",Temp_ID),
                            new SqlParameter("@c_ID",c_ID),
                                new SqlParameter("@Modified_by",Modified_by)

            };


                var response = dbLayer.SP_DataTable_return("Insert_Update_DepositMaster_Return", paramArray).Tables[0];
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





        //Get receipt general detail list by ID's
        public IEnumerable<pa_ReceiptDetails_DAL> Get_ReceiptDetailDepositVoucherlListByID_DAL(string Temp_ID, int? DV_ID)
        {

            string sp = "Select_ReceiptDetailDepositVouchListByID";
            List<pa_ReceiptDetails_DAL> itemlist = new List<pa_ReceiptDetails_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@DV_ID", DV_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_ReceiptDetails_DAL item = new pa_ReceiptDetails_DAL();

                        item.ReceiptDetail_ID = Convert.ToInt32(rdr["ReceiptDetail_ID"]);
                        item.CR_accountID = rdr["CR_accountID"].ToString();
                        item.DR_accountID = rdr["DR_accountID"].ToString();
                        item.DR_AcountName = rdr["DR_AcountName"].ToString();
                        item.CR_AccountName = rdr["CR_AccountName"].ToString();

                        item.Description = rdr["Description"].ToString();
                        item.Currency_ID = rdr["Currency_ID"].ToString();
                        item.Currency_Rate = rdr["Currency_Rate"].ToString();
                        item.Cheque_Bank_Name = rdr["Cheque_Bank_Name"].ToString();
                        item.cheque_Date = rdr["cheque_Date"].ToString();
                        item.Cheque_no = rdr["Cheque_no"].ToString();

                        item.Amount = rdr["Amount"].ToString();
                        item.VAT_Exp = rdr["VAT_Exp"].ToString();
                        item.VAT_Rate = rdr["VAT_Rate"].ToString();
                        item.other_curr_amount = rdr["other_curr_amount"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();
                        item.trans_ref = rdr["trans_ref"].ToString();


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


        // delete detail voucher of deposit
        public string DeleteDepositVoucherDetail_DAL(int? DVdetails_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@DVdetails_ID",DVdetails_ID)

            };


                var response = dbLayer.SP_DataTable_return("Delete_DepositVoucher_Detail", paramArray).Tables[0];
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


        //Get deposit master list
        public IEnumerable<Deposits_DAL> Get_DepositMaster_List_DAL(string DV_Ref, string Customer_Name, string StartDate, string EndDate, string Chassis_No, string Cheque_no, int? c_ID)
        {


            string sp = "Select_DepositMaster_List";
            List<Deposits_DAL> itemlist = new List<Deposits_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DV_Ref", DV_Ref);
                    cmd.Parameters.AddWithValue("@Party_ID_Name", Customer_Name);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                    cmd.Parameters.AddWithValue("@Cheque_no", Cheque_no);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Deposits_DAL item = new Deposits_DAL();

                        item.DV_ID = Convert.ToInt32(rdr["DV_ID"]);
                        item.DV_Ref = rdr["DV_Ref"].ToString();
                        item.Date_Taken = rdr["Date_Taken"].ToString();
                        item.Deposit = rdr["Deposit"].ToString();
                        item.Others = rdr["Others"].ToString();
                        item.Total_Collected = rdr["Total_Collected"].ToString();
                        item.Amount_Recieved = rdr["Amount_Recieved"].ToString();

                        item.Amount_Return = rdr["Amount_Return"].ToString();
                        item.CUSTOMER_NAME = rdr["CUSTOMER_NAME"].ToString();
                        item.export_to_Destination_ID = Convert.ToInt32(rdr["export_to_Destination_ID"]);
                        item.c_id = Convert.ToInt32(rdr["c_ID"]);


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


        public string Delete_Delete_master_DAL(int? DeliveryMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@DeliveryMaster_ID",DeliveryMaster_ID)

            };


                var response = dbLayer.SP_DataTable_return("Delete_Delivery_master", paramArray).Tables[0];
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

        //Get deposit return payment  detail list by ID's
        public IEnumerable<Pa_PaymentDetails_DAL> Get_PaymentDetailListByID_DAL(string Temp_ID, int? DV_ID)
        {



            string sp = "Select_DepsoitReturnPaymentListByID";
            List<Pa_PaymentDetails_DAL> itemlist = new List<Pa_PaymentDetails_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@DV_ID", DV_ID);

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


        //---insert into payment detial general of deposit return
        public string InsertDepositReturnPaymentDetail_DAL(int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
            string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int DV_ID)

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
                                 new SqlParameter("@DV_ID", DV_ID)



            };


                var response = dbLayer.SP_DataTable_return("InsertDepositReturnPayment", paramArray).Tables[0];
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

        //---Update into payment detial general of deposit return
        public string UpdateDepositReturnPaymentDetail_DAL(int? PaymentDetails_ID, int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
            string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by)

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


                var response = dbLayer.SP_DataTable_return("UpdateDepositReturnPayment", paramArray).Tables[0];
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





        public IPagedList<Pa_DeliveryOrder_Master> DeliveryOrderMasterList { get; set; }
        public IEnumerable<Pa_DeliveryOrder_Details> DeliveryOrderDetailList { get; set; }
        public Pa_DeliveryOrder_Master DeliveryOrderMasterObj { get; set; }


        public IEnumerable<Pa_DeliveryOrder_Master> Get_DeliveryOrder_master_DAL(string DeliveryRef, string StartDate, string EndDate,
            string Chassis, int? Customer_ID, int c_ID)
        {
            string sp = "select_vehDeliveryMasterList";
            List<Pa_DeliveryOrder_Master> itemlist = new List<Pa_DeliveryOrder_Master>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DeliveryRef", DeliveryRef);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Chassis", Chassis);
                    cmd.Parameters.AddWithValue("@Customer_ID", Customer_ID);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);




                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_DeliveryOrder_Master item = new Pa_DeliveryOrder_Master();


                        item.DeliveryMaster_ID = Convert.ToInt32(rdr["DeliveryMaster_ID"].ToString());
                        item.DeliveryRef = rdr["DeliveryRef"].ToString();
                        item.DeliveryDate = rdr["DeliveryDate"].ToString();


                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();


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
        public IEnumerable<Pa_DeliveryOrder_Details> Get_DeliveryOrder_DetailListByID_DAL(int? DeliveryMaster_ID, string temp_ID, int C_id)
        {



            string sp = "select_vehDeliveryList";
            List<Pa_DeliveryOrder_Details> itemlist = new List<Pa_DeliveryOrder_Details>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DeliveryMaster_ID", DeliveryMaster_ID);
                    cmd.Parameters.AddWithValue("@temp_ID", temp_ID);
                    cmd.Parameters.AddWithValue("@c_id", C_id);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_DeliveryOrder_Details item = new Pa_DeliveryOrder_Details();



                        item.DeliveryDetails_ID = Convert.ToInt32(rdr["ID"].ToString());
                        item.Chassis_No = rdr["Chassis_No"].ToString();
                        item.stock_ID = rdr["stock_ID"].ToString();
                        item.CarTakenDate = rdr["CarTakenDate"].ToString();
                        item.CarTakenPerson = rdr["CarTakenPerson"].ToString();
                        item.CarTakenContact = rdr["CarTakenContact"].ToString();
                        item.DilveryOrder_Issued = rdr["DilveryOrder_Issued"].ToString();







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
        public string Insert_DeliveryOrderMaster_DAL(string CarTakenDate, string CarTakenPerson, string CarTakenContact,
            string CarTaken, string temp_ID, int? Created_by, int c_ID)
        {

            string msg = "";
            bool CarTakens;
            if (CarTaken == "on")
            {
                CarTakens = true;
            }
            else
            {
                CarTakens = false;
            }

            try
            {


                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@CarTakenDate",CarTakenDate),
                new SqlParameter("@CarTakenPerson" ,CarTakenPerson),

                new SqlParameter("@CarTakenContact ",CarTakenContact),
                new SqlParameter("@CarTaken ",CarTakens),

                new SqlParameter("@temp_ID ",temp_ID),
                new SqlParameter("@User_ID",Created_by),
                new SqlParameter("@c_ID",c_ID)



            };


                var response = dbLayer.SP_DataTable_return("Insert_VehDeliverymaster", paramArray).Tables[0];
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

        public string Update_DeliveryOrderMaster_DAL(int? DeliveryMaster_ID, string CarTakenDate, string CarTakenPerson, string CarTakenContact, string CarTaken, string DeliveryOrder_Date, int? Modified_by)
        {

            string msg = "";
            bool CarTakens;
            if (CarTaken == "on")
            {
                CarTakens = true;
            }
            else
            {
                CarTakens = false;
            }
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@DeliveryMaster_ID",DeliveryMaster_ID),

                new SqlParameter("@CarTakenDate",CarTakenDate ),
                new SqlParameter("@CarTakenPerson" ,CarTakenPerson ),
                new SqlParameter("@CarTakenContact ",CarTakenContact ),
                new SqlParameter("@CarTaken ",CarTakens),
                new SqlParameter("@DeliveryOrder_Date ",DeliveryOrder_Date ),


                new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Update_VehDeliverymaster", paramArray).Tables[0];
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

        public string Insert_DeliveryOrderDetail_DAL(string Chassis_No, int? DeliveryMaster_ID, string temp_ID, int? Created_by, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Chassis_no",Chassis_No),
                new SqlParameter("@DeliveryMaster_ID" ,DeliveryMaster_ID),
                new SqlParameter("@temp_ID ",temp_ID),
                new SqlParameter("@User_ID",Created_by),
                new SqlParameter("@c_ID",c_ID)


            };


                var response = dbLayer.SP_DataTable_return("Insert_VehDelivery", paramArray).Tables[0];
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
        public Pa_DeliveryOrder_Master Get_DeliveryOrderMaster_Id(int? DeliveryMaster_ID)
        {
            string sp = "select_DeliveryOrderMaster_Id";
            Pa_DeliveryOrder_Master item = new Pa_DeliveryOrder_Master();

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



                        item.DeliveryMaster_ID = Convert.ToInt32(rdr["DeliveryMaster_ID"].ToString());
                        item.DeliveryRef = rdr["DeliveryRef"].ToString();
                        if (!string.IsNullOrEmpty(rdr["CarTaken"].ToString())) { item.CarTaken = Convert.ToBoolean(rdr["CarTaken"].ToString()); }
                        else { item.CarTaken = false; }
                        if (!string.IsNullOrEmpty(rdr["CarTakenDate"].ToString())) { item.CarTakenDate = rdr["CarTakenDate"].ToString(); }
                        else { item.CarTakenDate = ""; }
                        if (!string.IsNullOrEmpty(rdr["CarTakenPerson"].ToString())) { item.CarTakenPerson = rdr["CarTakenPerson"].ToString(); }
                        else { item.CarTakenPerson = ""; }
                        if (!string.IsNullOrEmpty(rdr["CarTakenContact"].ToString())) { item.CarTakenContact = rdr["CarTakenContact"].ToString(); }
                        else { item.CarTakenContact = ""; }
                        if (!string.IsNullOrEmpty(rdr["Sale_Ref"].ToString())) { item.SVRef = rdr["Sale_Ref"].ToString(); }
                        else { item.SVRef = ""; }





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

        public string Delete_Delivery_details_DAL(int? DeliveryDetails_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@DeliveryDetails_ID",DeliveryDetails_ID)

            };


                var response = dbLayer.SP_DataTable_return("Delete_Delivery_details", paramArray).Tables[0];
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

        public DataSet GetDataForPrintDtl(int? DV_ID)
        {
            using (con = new SqlConnection(Constr))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("get_dvDetails_print", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DV_ID", DV_ID);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                return ds;
            }
        }
        public DataSet GetDataForPrintDtl_Claim(int? DV_ID)
        {
            using (con = new SqlConnection(Constr))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("get_dvDetailsClaim_print", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DV_ID", DV_ID);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                return ds;
            }
        }
        public IEnumerable<Pa_Attachments_DAL> GetDVMaster_Attachments_DAL(int? DV_ID, string Type)
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
                    cmd.Parameters.AddWithValue("@Master_ID", DV_ID);
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
        public string Insert_Attachment_DV_DAL(int? DV_ID, string Document_Type, string Type, string newFileName, string filepath, int? Created_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Master_ID",DV_ID),
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

        public IEnumerable<Deposits_DAL> Get_DepositMaster_List_DAL1(string DV_Ref, string Customer_Name, string StartDate, string EndDate, string Chassis_No, int? c_ID)
        {


            string sp = "Select_DepositMaster_List";
            List<Deposits_DAL> itemlist = new List<Deposits_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DV_Ref", DV_Ref);
                    cmd.Parameters.AddWithValue("@Party_ID_Name", Customer_Name);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Deposits_DAL item = new Deposits_DAL();

                        item.DV_ID = Convert.ToInt32(rdr["DV_ID"]);
                        item.DV_Ref = rdr["DV_Ref"].ToString();
                        item.Date_Taken = rdr["Date_Taken"].ToString();
                        item.Deposit = rdr["Deposit"].ToString();
                        item.Others = rdr["Others"].ToString();
                        item.Total_Collected = rdr["Total_Collected"].ToString();
                        item.Amount_Recieved = rdr["Amount_Recieved"].ToString();

                        item.Amount_Return = rdr["Amount_Return"].ToString();
                        item.CUSTOMER_NAME = rdr["CUSTOMER_NAME"].ToString();
                        item.export_to_Destination_ID = Convert.ToInt32(rdr["export_to_Destination_ID"]);
                        item.c_id = Convert.ToInt32(rdr["c_ID"]);


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

        public IEnumerable<Deposits_DAL> Get_DepositMaster_List_DAL1_1(string DV_Ref, string Customer_Name, string StartDate, string EndDate, string Chassis_No, string Cheque_no, int? c_ID)
        {


            string sp = "Select_DepositMaster_List";
            List<Deposits_DAL> itemlist = new List<Deposits_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DV_Ref", DV_Ref);
                    cmd.Parameters.AddWithValue("@Party_ID_Name", Customer_Name);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                    cmd.Parameters.AddWithValue("@Cheque_no", Cheque_no);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Deposits_DAL item = new Deposits_DAL();

                        item.DV_ID = Convert.ToInt32(rdr["DV_ID"]);
                        item.DV_Ref = rdr["DV_Ref"].ToString();
                        item.Date_Taken = rdr["Date_Taken"].ToString();
                        item.Deposit = rdr["Deposit"].ToString();
                        item.Others = rdr["Others"].ToString();
                        item.Total_Collected = rdr["Total_Collected"].ToString();
                        item.Amount_Recieved = rdr["Amount_Recieved"].ToString();

                        item.Amount_Return = rdr["Amount_Return"].ToString();
                        item.CUSTOMER_NAME = rdr["CUSTOMER_NAME"].ToString();
                        item.export_to_Destination_ID = Convert.ToInt32(rdr["export_to_Destination_ID"]);
                        item.c_id = Convert.ToInt32(rdr["c_ID"]);


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
