using DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using X.PagedList;

namespace DAL.oClasses
{
    public class OAccounts : IOAccounts
    {

        private readonly IConnectionStringManager _conMgr;
        private readonly ISqlHelper dbLayer;
        SqlConnection con;

        public OAccounts(IConnectionStringManager conMgr, ISqlHelper sqlHelp)
        {

            _conMgr = conMgr;
            dbLayer = sqlHelp;
            //---Getting Connection String
            Constr = _conMgr.GetConnectionString();
        }
        public string Constr { get; set; }

        //by Rafay Start Date 13/01/2021
        public string Insert_pa_trans_ref_payment_DAL(int? Created_by, string htemp_ID, string Trans_Ref, string Total_Amount, string Currency_Rate)
        {
            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {
                    new SqlParameter("@Created_by",Created_by),
                    new SqlParameter("@htemp_ID",htemp_ID),
                     new SqlParameter("@Trans_Ref",Trans_Ref),
                     new SqlParameter("@Total_Amount_Other",Total_Amount),
                     new SqlParameter("@Currency_Rate",Currency_Rate)

            };


            var response = dbLayer.SP_DataTable_return("pa_Insert_pa_Trans_ref_payment", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }

        //by Rafay End Date 13/01/2021


        //by Rafay Paymnet Vendor 14/01/2021

        public IEnumerable<pa_PaymentMaster_DAL> Get_Ref_Tran_Payment(string PaymentDetail_ID)
        {
            List<pa_PaymentMaster_DAL> RD = new List<pa_PaymentMaster_DAL>();
            string sp = "pa_GET_pa_Trans_ref_payment";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PaymentDetail_ID", PaymentDetail_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_PaymentMaster_DAL o = new pa_PaymentMaster_DAL();


                                o.Total_Amount = dr["Total_Amount"].ToString();
                                o.PurchaseDate = dr["PurchaseDate"].ToString();
                                o.Type = dr["Type"].ToString();
                                o.Ref = dr["Ref"].ToString();
                                o.Paid = dr["Paid"].ToString();
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

        //by Rafay Paymnet Vendor 14/01/2021



        public IEnumerable<Pa_TrailBalance_DAL> TrailBalanceList { get; set; }
        public IEnumerable<Pa_TrailBalance_DAL> TrailBalanceList_ttl { get; set; }
        public pa_PaymentMaster_DAL paymentMasterObject { get; set; }
        public IEnumerable<pa_PaymentMaster_DAL> paymentMasterList { get; set; }

        public IPagedList<pa_PaymentMaster_DAL> paymentMasterListIPaged { get; set; }
        public IEnumerable<pa_PaymentMaster_DAL> PaymentMasterListTTL_obj { get; set; }

        public IEnumerable<Approvals_DAL> ApprovalList { get; set; }


        public Pa_PaymentDetails_DAL paymentDetailObject { get; set; }
        public IEnumerable<Pa_PaymentDetails_DAL> paymentDetailList { get; set; }

        public ChassisValidation_DAL ChassisValidationObj { get; set; }

        public pa_ReceiptDetails_DAL receiptDetailObject { get; set; }
        public IEnumerable<pa_ReceiptDetails_DAL> receiptDetailList { get; set; }
        public pa_ReceiptMaster_DAL receiptMasterObject { get; set; }
        public IEnumerable<pa_ReceiptMaster_DAL> receiptMasterList { get; set; }

        public IEnumerable<pa_ReceiptMaster_DAL> receiptMasterList_TTL { get; set; }


        //new faraz work
        public IEnumerable<Pa_Ledger_DAL> LedgerMasterIPagedList_1 { get; set; }

        //new work
        public IEnumerable<Pa_Ledger_DAL> LedgerMasterIPagedList_TTL1 { get; set; }

        //------------------------------

        public IPagedList<pa_ReceiptMaster_DAL> receiptMasterIPagedList { get; set; }
        //Get ledger list
        public IEnumerable<Pa_Ledger_DAL> objLedgerMasterList { get; set; }
        public IEnumerable<Pa_Ledger_DAL> LedgerMasterList { get; set; }
        public IPagedList<Pa_Ledger_DAL> LedgerMasterIPagedList { get; set; }
        public IEnumerable<Pa_Ledger_DAL> LedgerMasterIPagedList_TTL { get; set; }


        public Accounts_DAL ChartofAccountObject { get; set; }
        public IEnumerable<Accounts_DAL> ChartofAccountList { get; set; }

        public IEnumerable<Approvals_DAL> ApprovalObjList { get; set; }

        public IEnumerable<Pa_Attachments_DAL> attachmentList { get; set; }
        public IEnumerable<pa_PaymentMaster_DAL> PaymentBalanceList { get; set; }
        public IEnumerable<pa_ReceiptMaster_DAL> receiptmasterlist { get; set; }
        public IEnumerable<pa_ReceiptMaster_DAL> receiptMasterIPagedList1 { get; set; }
        public IEnumerable<pa_PaymentMaster_DAL> paymentMasterListIPaged1 { get; set; }
        public IEnumerable<pa_ReceiptMaster_DAL> receiptMasterList_TTL1 { get; set; }

        public IEnumerable<pa_PaymentMaster_DAL> PaymentMasterListTTL_obj1 { get; set; }


        public IEnumerable<TaxReport> PaidTax_PR { get; set; }
        public IEnumerable<TaxReport> PaidTax_TTL_PR { get; set; }


        public IEnumerable<TaxReport> ReceivedTax_PR { get; set; }
        public IEnumerable<TaxReport> ReceivedTax_TTL_PR { get; set; }
        #region BalanceSheet objects


        public IEnumerable<Balance_Sheet> BalanceSheet_obj { get; set; }
        public IEnumerable<Balance_Sheet> BalSh_byCurrentAssets_obj { get; set; }
        public IEnumerable<Balance_Sheet> BalSh_byCurrentLaibilities_obj { get; set; }
        public IEnumerable<Balance_Sheet> BalSh_byFixedAssets_obj { get; set; }
        public IEnumerable<Balance_Sheet> BalSh_byLongTermLaibilities_obj { get; set; }
        public IEnumerable<Balance_Sheet> BalSh_byDrawings_obj { get; set; }
        public IEnumerable<Balance_Sheet> BalSh_byCapitalInvestments_obj { get; set; }
        public IEnumerable<Balance_Sheet> Get_NetIncome_onPrint_obj { get; set; }

        public IEnumerable<Pa_Ledger_DAL> LedgerMasterIPagedList1 { get; set; }
        #endregion


        #region IncomeStatement

        public IEnumerable<IncomeStatement> IncomeStatment_INC_obj { get; set; }
        public Pa_Ledger_DAL AccountMaster_obj { get; set; }

        public IEnumerable<IncomeStatement> IncomeStatment_Exp_obj { get; set; }
        public IEnumerable<IncomeStatement> IncomeStatment_CGS_obj { get; set; }
        public IEnumerable<IncomeStatement> IncomeStatment_Exp_Fin_obj { get; set; }
        public IEnumerable<IncomeStatement> IncomeStatment_disRet_obj { get; set; }
        public IEnumerable<IncomeStatement> IncomeStateMent_obj { get; set; }

        #endregion



        #region Chart_of_Accounts Start

        public string Insert_Accounts(string AccountName, string Acc_ShortName, int HeadAccounts_ID, int subHeadAccount_ID, int Sys_AC_type_ID,
            string Opening_Balance, string Opening_Bal_Date, int User_ID, int c_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@AccountName",AccountName),
                  new SqlParameter("@Acc_ShortName",Acc_ShortName),
                   new SqlParameter("@HeadAccounts_ID",HeadAccounts_ID),
                    new SqlParameter("@subHeadAccount_ID",subHeadAccount_ID),
                   new SqlParameter("@Sys_AC_type_ID",Sys_AC_type_ID),
                    new SqlParameter("@Opening_Balance",Opening_Balance),
                     new SqlParameter("@Opening_Bal_Date",Opening_Bal_Date),
                     new SqlParameter("@User_ID",User_ID),
                     new SqlParameter("@c_ID",c_ID)


            };


                var response = dbLayer.SP_DataTable_return("Insert_Accounts", paramArray).Tables[0];
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

        public string Update_Accounts(string AccountName, string Acc_ShortName, int HeadAccounts_ID, int subHeadAccount_ID, int Sys_AC_type_ID,
         string Opening_Balance, string Opening_Bal_Date, int User_ID, int Account_ID, int c_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@AccountName",AccountName),
                new SqlParameter("@Acc_ShortName",Acc_ShortName),
                new SqlParameter("@HeadAccounts_ID",HeadAccounts_ID),
                new SqlParameter("@subHeadAccount_ID",subHeadAccount_ID),
                new SqlParameter("@Sys_AC_type_ID",Sys_AC_type_ID),
                new SqlParameter("@Opening_Balance",Opening_Balance),
                new SqlParameter("@Opening_Bal_Date",Opening_Bal_Date),
                new SqlParameter("@User_ID",User_ID),
                new SqlParameter("@Account_ID",Account_ID),
                new SqlParameter("@c_ID",c_ID)


            };

                var response = dbLayer.SP_DataTable_return("Update_Accounts", paramArray).Tables[0];
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

        public IEnumerable<Accounts_DAL> Get_select_AccountsList_DAL(string AccountName, int? HeadAccounts_ID, int? subHeadAccount_ID,
            int Sys_AC_type_ID, int c_ID)
        {

            //        @AccountName varchar(100),
            //@HeadAccounts_ID int,
            //@subHeadAccount_ID int,
            //@Sys_AC_type_ID int


            string sp = "select_AccountsList";
            List<Accounts_DAL> itemlist = new List<Accounts_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AccountName", AccountName);
                    cmd.Parameters.AddWithValue("@HeadAccounts_ID", HeadAccounts_ID);
                    cmd.Parameters.AddWithValue("@subHeadAccount_ID", subHeadAccount_ID);
                    cmd.Parameters.AddWithValue("@Sys_AC_type_ID", Sys_AC_type_ID);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Accounts_DAL item = new Accounts_DAL();


                        item.Account_ID = Convert.ToInt32(rdr["Account_ID"]);

                        item.AccountName = rdr["AccountName"].ToString();
                        item.Account_No = rdr["Account_No"].ToString();
                        item.Acc_ShortName = rdr["Acc_ShortName"].ToString();
                        item.HeadAccounts_ID = Convert.ToInt32(rdr["HeadAccounts_ID"]);
                        item.HeadAccount = rdr["HeadAccount"].ToString();
                        item.subHeadAccount_ID = Convert.ToInt32(rdr["subHeadAccount_ID"]);
                        item.sub_HeadAccount = rdr["sub_HeadAccount"].ToString();
                        item.Sys_AC_type_ID = Convert.ToInt32(rdr["Sys_AC_type_ID"]);
                        item.Sys_Ac_TypeName = rdr["Sys_Ac_TypeName"].ToString();
                        item.Opening_Balance = rdr["Opening_Balance"].ToString();
                        item.Opening_Bal_Date = rdr["Opening_Bal_Date"].ToString();

                        item.Created_byName = rdr["Created_byName"].ToString();
                        item.Modified_byName = rdr["Modified_byName"].ToString();

                        item.Created_at = rdr["Created_at"].ToString();
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


        public IEnumerable<Accounts_DAL> get_AllAccountList_DAL(int c_ID)
        {

            //        @AccountName varchar(100),
            //@HeadAccounts_ID int,
            //@subHeadAccount_ID int,
            //@Sys_AC_type_ID int


            string sp = "get_AllAccountList";
            List<Accounts_DAL> itemlist = new List<Accounts_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    //cmd.Parameters.AddWithValue("@HeadAccounts_ID", HeadAccounts_ID);
                    //cmd.Parameters.AddWithValue("@subHeadAccount_ID", subHeadAccount_ID);
                    //cmd.Parameters.AddWithValue("@Sys_AC_type_ID", Sys_AC_type_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Accounts_DAL item = new Accounts_DAL();


                        item.Account_ID = Convert.ToInt32(rdr["Account_ID"]);
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


        public IEnumerable<Accounts_DAL> get_HeadAccountList_DAL(int c_ID)
        {

            //        @AccountName varchar(100),
            //@HeadAccounts_ID int,
            //@subHeadAccount_ID int,
            //@Sys_AC_type_ID int


            string sp = "get_HeadAccountList";
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


                        item.HeadAccounts_ID = Convert.ToInt32(rdr["ID"]);
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


        public Accounts_DAL Get_select_Account_By_ID_DAL(int? Account_ID)
        {

            string sp = "select_Account_By_ID";

            Accounts_DAL pm = new Accounts_DAL();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Account_ID", Account_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                //            SELECT ID, Account_ID, Account_No, AccountName, Acc_ShortName, HeadAccounts_ID, subHeadAccount_ID,
                                //Sys_AC_type_ID, Opening_Balance, Opening_Bal_Date, Created_at, Created_by, Modified_at, Modified_by
                                //FROM Accounts

                                pm.Account_ID = Convert.ToInt32(dr["Account_ID"]);
                                pm.HeadAccounts_ID = Convert.ToInt32(dr["HeadAccounts_ID"]);
                                pm.subHeadAccount_ID = Convert.ToInt32(dr["subHeadAccount_ID"]);
                                pm.Sys_AC_type_ID = Convert.ToInt32(dr["Sys_AC_type_ID"]);
                                pm.Account_No = dr["Account_No"].ToString();
                                pm.Opening_Balance = dr["Opening_Balance"].ToString();
                                pm.Opening_Bal_Date = dr["Opening_Bal_Date"].ToString();
                                pm.Created_at = dr["Created_at"].ToString();
                                pm.Modified_at = dr["Modified_at"].ToString();
                                pm.Created_byName = dr["Created_byName"].ToString();
                                pm.Modified_byName = dr["Modified_byName"].ToString();



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


        public IEnumerable<Approvals_DAL> Get_ApprovalList_DAL(int c_ID)
        {


            string sp = "Select_ApprovalList";
            List<Approvals_DAL> itemlist = new List<Approvals_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();
                    //Ref, , , c_ID, transaction_status, PartyName, ID, Status, Link

                    while (rdr.Read())
                    {
                        Approvals_DAL item = new Approvals_DAL();




                        item.Ref = rdr["Ref"].ToString();
                        item.Trans_Date = rdr["Date"].ToString();
                        item.PartyName = rdr["PartyName"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();
                        item.Status = rdr["Status"].ToString();
                        item.Link = rdr["Link"].ToString();
                        item.ID = Convert.ToInt32(rdr["ID"]);


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



        //---insert into payment detial general
        public string InsertPaymentDetail_General_DAL(int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
            string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount,
            int? stock_ID, string Temp_ID, int? c_ID, int? Created_by, int PaymentMaster_ID, double TaxAmount, 
            double ExtraCharges, double ExtraChargesTax, double TaxAmountOthers,string BLNO, int PDC_Payable_Acc_ID)

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
                              new SqlParameter("@stock_ID",stock_ID),
                               new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@c_ID",c_ID),
                                 new SqlParameter("@Created_by",Created_by),
                                  new SqlParameter("@PaymentMaster_ID",PaymentMaster_ID),
                                   new SqlParameter("@ExtraChargesFee",ExtraCharges),
                                   new SqlParameter("@TaxAmountOthers",TaxAmountOthers),
                                   new SqlParameter("@TaxAmount",TaxAmount),
                                   new SqlParameter("@ExtraChargesFeeTax",ExtraChargesTax),
                                     new SqlParameter("@BLNO",BLNO),
                                     new SqlParameter("@PDC_Payable_Acc_ID",PDC_Payable_Acc_ID)

            };


                var response = dbLayer.SP_DataTable_return("InsertPaymentDetail", paramArray).Tables[0];
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



        //---Update into payment detial general
        public string UpdatePaymentDetail_General_DAL(int? PaymentDetails_ID, int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
            string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, 
            double? Total_Amount, int? stock_ID, string Temp_ID, int? c_ID, int? Modified_by, 
            double ExtraCharges, double ExtraChargesTax, double TaxAmount, double TaxAmountOthers,string BLNO, int PDC_Payable_Acc_ID)

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
                new SqlParameter("@stock_ID",stock_ID),
                new SqlParameter("@Temp_ID",Temp_ID),
                new SqlParameter("@c_ID",c_ID),
                new SqlParameter("@Modified_by",Modified_by),
                new SqlParameter("@ExtraChargesFee",ExtraCharges),
               new SqlParameter("@TaxAmountOthers",TaxAmountOthers),
                 new SqlParameter("@TaxAmount",TaxAmount),
                new SqlParameter("@ExtraChargesFeeTax",ExtraChargesTax),
                new SqlParameter("@BLNO",BLNO),
                new SqlParameter("@PDC_Payable_Acc_ID",PDC_Payable_Acc_ID)




            };


                var response = dbLayer.SP_DataTable_return("UpdatePaymentDetail", paramArray).Tables[0];
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



        //--getting payment master by id
        public pa_PaymentMaster_DAL Get_PaymentMasterByID_DAL(int? PaymentMaster_ID)
        {

            string sp = "Select_PaymentMasterByID";

            pa_PaymentMaster_DAL pm = new pa_PaymentMaster_DAL();
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
                                pm.PaymentMaster_ID = Convert.ToInt32(dr["PaymentMaster_ID"]);
                                pm.PaymentMaster_ref = dr["PaymentMaster_ref"].ToString();
                                pm.Party_ID = dr["Party_ID"].ToString();
                                pm.party_type = dr["party_type"].ToString();
                                pm.NameText = dr["NameText"].ToString();
                                pm.Date = dr["Date"].ToString();
                                pm.Explanation = dr["Explanation"].ToString();
                                pm.PaymentStatus = dr["PaymentStatus"].ToString();

                                pm.create_at = dr["create_at"].ToString();
                                pm.create_by = dr["create_by"].ToString();
                                pm.last_updated_at = dr["last_updated_at"].ToString();
                                pm.last_updated_by = dr["last_updated_by"].ToString();

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

        //Get payment detail list by ID's
        public IEnumerable<Pa_PaymentDetails_DAL> Get_PaymentDetailListByID_DAL(string Temp_ID, int? PaymentMaster_ID)
        {



            string sp = "Select_PaymentDetailGenralListByID";
            List<Pa_PaymentDetails_DAL> itemlist = new List<Pa_PaymentDetails_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@PaymentMaster_ID", PaymentMaster_ID);

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
                        item.Chassis_No = rdr["Chassis_No"].ToString();
                        item.AmountTotal = rdr["AmountTotal"].ToString();
                        item.VatExpTotal = rdr["VatExpTotal"].ToString();
                        item.OtherAmountTotal = rdr["OtherAmountTotal"].ToString();
                        item.TaxAmount = rdr["TaxAmount"].ToString();
                        item.TaxAmountOthers = rdr["TaxAmountOthers"].ToString();
                        item.Grand_Total = rdr["Grand_Total"].ToString();
                        item.curr_name = rdr["Currency_ShortName"].ToString();
                        item.ExtraChargesFee = rdr["ExtraChargesFee"].ToString();
                        item.ExtraChargesFeeTax = rdr["ExtraChargesFeeTax"].ToString();

                        item.transaction_status = Convert.ToInt32(rdr["transaction_status"].ToString());

                        item.BLNO = rdr["BLNO"].ToString();
                        item.PDC_Payable_Acc_ID = Convert.ToInt32(rdr["PDC_Payable_Acc_ID"].ToString());

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

        // Get Old TempID From PaymentDetail_DAL by paymentMaster id
        public string GetOldTempIDFromPaymentDetail_DAL(int? PaymentMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@PaymentMaster_ID",PaymentMaster_ID)

            };


                var response = dbLayer.SP_DataTable_return("SelectPaymentDetailOldTempID", paramArray).Tables[0];
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

        // Get Old TempID From receipt by ReceiptMaster_ID id
        public string GetOldTempIDFromReceiptDetail_DAL(int? ReceiptMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@ReceiptMaster_ID",ReceiptMaster_ID)

            };


                var response = dbLayer.SP_DataTable_return("SelectReceiptDetailOldTempID", paramArray).Tables[0];
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


        //Delete PaymentDetailGeneral
        public string DeletePaymentDetail_DAL(int? PaymentDetails_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@PaymentDetails_ID",PaymentDetails_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_PaymentDetail", paramArray).Tables[0];
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


        public string DeletePaymentDetail_byMasterID_DAL(int? PaymentMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@PaymentMaster_ID",PaymentMaster_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_PaymentDetail_BYMASTERID", paramArray).Tables[0];
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
        //Insert payment general master
        public string Insert_PaymentGeneral_DAL(int? Vendor_ID, int? party_type, string Explanation, string PaymentType, string Date, string NameText, string Temp_ID, int? c_ID, int? Created_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Vendor_ID",Vendor_ID),
                new SqlParameter("@party_type",party_type),

                new SqlParameter("@Explanation",Explanation),
                new SqlParameter("@PaymentType",PaymentType),

                new SqlParameter("@Date",Date),
                new SqlParameter("@NameText",NameText),


                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Created_by",Created_by)




            };


                var response = dbLayer.SP_DataTable_return("Insert_PaymentMaster_General", paramArray).Tables[0];
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

        //Update payment general master
        public string Update_PaymentGeneral_DAL(int? PaymentMaster_ID, int? Vendor_ID, int? party_type, string Explanation, string Date, string NameText, string Temp_ID, int? c_ID, int? Modified_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@PaymentMaster_ID",PaymentMaster_ID),
                new SqlParameter("@Vendor_ID",Vendor_ID),
                       new SqlParameter("@party_type",party_type),
                new SqlParameter("@Explanation",Explanation),

                new SqlParameter("@Date",Date),
                  new SqlParameter("@NameText",NameText),
                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Modified_by",Modified_by)

            };


                var response = dbLayer.SP_DataTable_return("Update_PaymentMaster_General", paramArray).Tables[0];
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

        //this method is for validating chassis no and getting stock id
        public ChassisValidation_DAL ValidateChassisNo_DAL(string ChassisNo, int c_ID)
        {


            string sp = "Get_Chassis_Validation";
            ChassisValidation_DAL ChsValid = new ChassisValidation_DAL();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChassisNo", ChassisNo);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    SqlDataReader record = cmd.ExecuteReader();


                    if (record.Read())
                    {

                        ChsValid.Stock_ID = record["Stock_ID"].ToString();
                        ChsValid.Chassis_No = record["Chassis_No"].ToString();
                        ChsValid.Make = record["Make"].ToString();
                        ChsValid.ModelDesciption = record["ModelDesciption"].ToString();
                        ChsValid.ModelYear = record["ModelYear"].ToString();
                        ChsValid.Color = record["Color"].ToString();
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

        public ChassisValidation_DAL ValidateBLNO_DAL(string BLNO, int c_ID)
        {


            string sp = "Get_BL_Validation";
            ChassisValidation_DAL ChsValid = new ChassisValidation_DAL();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BLNO", BLNO);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    SqlDataReader record = cmd.ExecuteReader();


                    if (record.Read())
                    {

                        ChsValid.Stock_ID = record["Stock_ID"].ToString();
                        ChsValid.Chassis_No = record["Chassis_No"].ToString();
                        ChsValid.Make = record["Make"].ToString();
                        ChsValid.ModelDesciption = record["ModelDesciption"].ToString();
                        ChsValid.ModelYear = record["ModelYear"].ToString();
                        ChsValid.Color = record["Color"].ToString();
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

        //this method is for validating chassis no and getting stock id
        public ChassisValidation_DAL ValidateChassisNo_UNSOLD_DAL(string ChassisNo, int c_ID)
        {


            string sp = "Get_Chassis_Validation_UNSOLD";
            ChassisValidation_DAL ChsValid = new ChassisValidation_DAL();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChassisNo", ChassisNo);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    SqlDataReader record = cmd.ExecuteReader();


                    if (record.Read())
                    {

                        ChsValid.Stock_ID = record["Stock_ID"].ToString();
                        ChsValid.Chassis_No = record["Chassis_No"].ToString();
                        ChsValid.Make = record["Make"].ToString();
                        ChsValid.ModelDesciption = record["ModelDesciption"].ToString();
                        ChsValid.ModelYear = record["ModelYear"].ToString();
                        ChsValid.Color = record["Color_Int"].ToString();
                        ChsValid.Selling_Price = record["Selling_Price"].ToString();
                        //ChsValid.Total_Price = record["TotalPrice"].ToString();


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


        //Get new payment master  List
        public IEnumerable<pa_PaymentMaster_DAL> Get_PaymentMasterList_DAL(string PaymentRef, int Party_ID_Name, string StartDate,
            string EndDate, string Chassis_No, string Paid_To, string PurchaseRef, string VoucherType, int c_ID, string Cheque_no)
        {


            string sp = "Select_PaymentMaster_List";
            List<pa_PaymentMaster_DAL> itemlist = new List<pa_PaymentMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PaymentRef", PaymentRef);
                    cmd.Parameters.AddWithValue("@Party_ID_Name", Party_ID_Name);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                    cmd.Parameters.AddWithValue("@Paid_To", Paid_To);
                    cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                    cmd.Parameters.AddWithValue("@VoucherType", VoucherType);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    cmd.Parameters.AddWithValue("@cheque_no", Cheque_no);

                    //@PaymentRef varchar(100),       
                    //@Party_ID_Name int,
                    //@StartDate varchar(100),       
                    //@EndDate varchar(100),
                    //@Chassis_No varchar(100),
                    //@Paid_To varchar(100),
                    //@PurchaseRef varchar(50),


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_PaymentMaster_DAL item = new pa_PaymentMaster_DAL();


                        item.PaymentMaster_ID = Convert.ToInt32(rdr["PaymentMaster_ID"]);
                        item.PaymentMaster_ref = rdr["PaymentMaster_ref"].ToString();
                        item.Date = rdr["DATE"].ToString();
                        item.Party_ID_Name = rdr["Party_ID_Name"].ToString();
                        item.Paymenttype = rdr["Paymenttype"].ToString();
                        item.Explanation = rdr["Explanation"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();
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

        public IEnumerable<pa_PaymentMaster_DAL> Get_PaymentMasterList_TTL_DAL(string PaymentRef, int Party_ID_Name, string StartDate,
          string EndDate, string Chassis_No, string Paid_To,
          string PurchaseRef, string VoucherType, int c_ID, string Cheque_no)
        {


            string sp = "Select_PaymentMaster_List_TTL";
            List<pa_PaymentMaster_DAL> itemlist = new List<pa_PaymentMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PaymentRef", PaymentRef);
                    cmd.Parameters.AddWithValue("@Party_ID_Name", Party_ID_Name);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                    cmd.Parameters.AddWithValue("@Paid_To", Paid_To);
                    cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                    cmd.Parameters.AddWithValue("@VoucherType", VoucherType);
                    cmd.Parameters.AddWithValue("@Cheque_no", Cheque_no);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    //@PaymentRef varchar(100),       
                    //@Party_ID_Name int,
                    //@StartDate varchar(100),       
                    //@EndDate varchar(100),
                    //@Chassis_No varchar(100),
                    //@Paid_To varchar(100),
                    //@PurchaseRef varchar(50),


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_PaymentMaster_DAL item = new pa_PaymentMaster_DAL();

                        item.Amount = rdr["Amount"].ToString();
                        item.VAT_Exp = rdr["VAT_Exp"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();

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

        //Get new payment master  List
        public IEnumerable<pa_ReceiptMaster_DAL> Get_ReceiptMaster_List_DAL(string ReceiptRef,
            int Party_ID, string PartyNameText, string SaleRef, string Chassis_no, string StartDate, string EndDate, string Cheque_no, int c_ID)
        {


            string sp = "Select_ReceiptMaster_List";
            List<pa_ReceiptMaster_DAL> itemlist = new List<pa_ReceiptMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ReceiptRef", ReceiptRef);
                    cmd.Parameters.AddWithValue("@Party_ID", Party_ID);
                    cmd.Parameters.AddWithValue("@PartyNameText", PartyNameText);
                    cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                    cmd.Parameters.AddWithValue("@Chassis_no", Chassis_no);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    cmd.Parameters.AddWithValue("@Cheque_no", Cheque_no);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_ReceiptMaster_DAL item = new pa_ReceiptMaster_DAL();

                        item.ReceiptMaster_ID = Convert.ToInt32(rdr["ReceiptMaster_ID"]);
                        item.ReceiptMaster_ref = rdr["ReceiptMaster_ref"].ToString();
                        item.ReceiptDate = rdr["DATE"].ToString();
                        item.Party_ID_Name = rdr["Party_ID_Name"].ToString();
                        item.Explanation = rdr["Explanation"].ToString();
                        item.Receipttype = rdr["Receipttype"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();
                        item.transaction_status = rdr["transaction_status"].ToString();

                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();
                        item.AfterVATTotal = rdr["AfterVATTotal"].ToString();




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


        public IEnumerable<pa_ReceiptMaster_DAL> Get_ReceiptMaster_List_TTL_DAL(string ReceiptRef,
            int Party_ID, string PartyNameText, string SaleRef, string Chassis_no, string StartDate, string EndDate, string Cheque_no, int c_ID)
        {


            string sp = "Select_ReceiptMaster_List_TTL";
            List<pa_ReceiptMaster_DAL> itemlist = new List<pa_ReceiptMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ReceiptRef", ReceiptRef);
                    cmd.Parameters.AddWithValue("@Party_ID", Party_ID);
                    cmd.Parameters.AddWithValue("@PartyNameText", PartyNameText);
                    cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                    cmd.Parameters.AddWithValue("@Chassis_no", Chassis_no);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Cheque_no", Cheque_no);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_ReceiptMaster_DAL item = new pa_ReceiptMaster_DAL();


                        item.Total_Amount = rdr["Total_Amount"].ToString();
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


        //--getting receipt master gen by id
        public pa_ReceiptMaster_DAL Get_ReceiptMasterGeneralByID_DAL(int? ReceiptMaster_ID)
        {

            string sp = "Select_ReceiptMasterGeneralByID";

            pa_ReceiptMaster_DAL pm = new pa_ReceiptMaster_DAL();

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
                                pm.ReceiptMaster_ID = Convert.ToInt32(dr["ReceiptMaster_ID"]);
                                pm.ApprovedBy = Convert.ToInt32(dr["ApprovedBy"]);
                                pm.ReceiptMaster_ref = dr["ReceiptMaster_ref"].ToString();
                                pm.Party_ID = dr["Party_ID"].ToString();
                                pm.party_type = dr["party_type"].ToString();
                                pm.NameText = dr["NameText"].ToString();
                                pm.ReceiptDate = dr["ReceiptDate"].ToString();
                                pm.Explanation = dr["Explanation"].ToString();

                                pm.ReceiptStatus = dr["ReceiptStatus"].ToString();

                                //pm.Created_at = dr["create_at"].ToString();
                                //pm.Created_by = dr["Created_by"].ToString();
                                //pm.Modified_at = dr["Modified_at"].ToString();
                                //pm.Modified_by = dr["Modified_by"].ToString();

                                pm.Created_at = dr["create_at"].ToString();
                                pm.Created_by = dr["create_by"].ToString();
                                pm.Modified_at = dr["last_updated_at"].ToString();
                                pm.Modified_by = dr["last_updated_by"].ToString();

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

        //--getting receipt master Sale by id
        public pa_ReceiptMaster_DAL Get_ReceiptMasterSaleByID_DAL(int? ReceiptMaster_ID)
        {

            string sp = "Select_ReceiptMasterSaleByID";

            pa_ReceiptMaster_DAL pm = new pa_ReceiptMaster_DAL();

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
                                pm.ReceiptMaster_ID = Convert.ToInt32(dr["ReceiptMaster_ID"]);
                                pm.ApprovedBy = Convert.ToInt32(dr["ApprovedBy"]);
                                pm.ReceiptMaster_ref = dr["ReceiptMaster_ref"].ToString();
                                pm.Party_ID = dr["Party_ID"].ToString();
                                pm.NameText = dr["NameText"].ToString();
                                pm.ReceiptDate = dr["ReceiptDate"].ToString();
                                pm.Explanation = dr["Explanation"].ToString();
                                pm.ReceiptStatus = dr["ReceiptStatus"].ToString();
                                pm.Created_at = dr["create_at"].ToString();
                                pm.Created_by = dr["create_by"].ToString();
                                pm.Modified_at = dr["last_updated_at"].ToString();
                                pm.Modified_by = dr["last_updated_by"].ToString();

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

        //Get receipt general detail list by ID's
        public IEnumerable<pa_ReceiptDetails_DAL> Get_ReceiptDetailGeneralListByID_DAL(string Temp_ID, int? ReceiptMaster_ID)
        {



            string sp = "Select_ReceiptDetailGenralListByID";
            List<pa_ReceiptDetails_DAL> itemlist = new List<pa_ReceiptDetails_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@ReceiptMaster_ID", ReceiptMaster_ID);

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

                        item.Currency_ShortName = rdr["Currency_ShortName"].ToString();



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

                        item.Grand_Amount = rdr["Grand_Amount"].ToString();
                        item.Grand_VAT_Exp = rdr["Grand_VAT_Exp"].ToString();

                        item.Grand_Total_Amount = rdr["Grand_Total_Amount"].ToString();
                        item.PDC_Recieving_Acc_ID = Convert.ToInt32(rdr["PDC_Recieving_Acc_ID"].ToString());




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

        //string StartDate, string EndDate, int? AccountID, string Trans_Ref
        public IEnumerable<Pa_Ledger_DAL> Get_LedgerList_DAL(string StartDate, int AccountID, string EndDate,
            string Trans_Ref, int c_ID)
        {


            string sp = "pa_Select_Ledger";
            List<Pa_Ledger_DAL> itemlist = new List<Pa_Ledger_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //these are the parameters
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@AccountID", AccountID);
                    cmd.Parameters.AddWithValue("@Trans_Ref", Trans_Ref);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    //cmd.Parameters.AddWithValue("@HeadAccount_ID", HeadAccount_ID);
                    
                    


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Ledger_DAL item = new Pa_Ledger_DAL();

                        //Convert.ToInt32(rdr["PurchaseDetails_ID"]);
                        item.Date = rdr["Date"].ToString();
                        item.Debit = rdr["Debit"].ToString();
                        item.Credit = rdr["Credit"].ToString();
                        item.Description = rdr["Description"].ToString();
                        item.trans_ref = rdr["trans_ref"].ToString();
                        item.ACCOUNT = rdr["ACCOUNT"].ToString();
                        //item.AccountID = rdr["AccountID"].ToString();
                        item.trans_ref_ID = rdr["trans_ref_ID"].ToString();
                        item.Link = rdr["Link"].ToString();

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



        public IEnumerable<Pa_Ledger_DAL> Get_LedgerList_DAL_TTL(string StartDate, int AccountID,
         string EndDate,string Trans_Ref, int c_ID)
        {


            string sp = "pa_Select_Ledger_TTL";
            List<Pa_Ledger_DAL> itemlist = new List<Pa_Ledger_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //these are the parameters
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@AccountID", AccountID);
                    cmd.Parameters.AddWithValue("@Trans_Ref", Trans_Ref);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    //cmd.Parameters.AddWithValue("@HeadAccount_ID", HeadAccount_ID);



                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Ledger_DAL item = new Pa_Ledger_DAL();

                        //Convert.ToInt32(rdr["PurchaseDetails_ID"]);

                        item.TotalDebit = rdr["TotalDebit"].ToString();
                        item.TotalCredit = rdr["TotalCredit"].ToString();
                        item.TotalBalance = rdr["TotalBalance"].ToString();
                        item.Opening_Balance = rdr["Opening_Balance"].ToString();
                        item.Closing_Balance = rdr["Closing_Balance"].ToString();


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

        //trail balance
        public IEnumerable<Pa_TrailBalance_DAL> Get_TrailBalanceList_DAL(string StartDate, String EndDate, int c_ID)
        {


            string sp = "pa_TrailBalance";
            List<Pa_TrailBalance_DAL> itemlist = new List<Pa_TrailBalance_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //these are the parameters
                    cmd.Parameters.AddWithValue("@start_Date", StartDate);
                    cmd.Parameters.AddWithValue("@end_Date", EndDate);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);



                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_TrailBalance_DAL item = new Pa_TrailBalance_DAL();



                        item.Debit = rdr["DR"].ToString();
                        item.Credit = rdr["CR"].ToString();
                        item.ACCOUNT = rdr["ACCOUNT"].ToString();
                        item.Link = rdr["Link"].ToString();


                        //now

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

        //---insert into receipt detail general
        public string InsertReceiptDetail_General_DAL(int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
            double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date,
            string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int DV_ID, int ReceiptMaster_ID, int PDC_Recieving_Acc_ID)

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
                                 new SqlParameter("@DV_ID",DV_ID),
                                 new SqlParameter("@ReceiptMaster_ID",ReceiptMaster_ID),
                                 new SqlParameter("@PDC_Recieving_Acc_ID",PDC_Recieving_Acc_ID)



            };


                var response = dbLayer.SP_DataTable_return("InsertReceiptDetailGen", paramArray).Tables[0];
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



        //---insert into receipt detail general
        public string InsertReceiptDetailDeposit_General_DAL(int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
            double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date,
            string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int DV_ID, int ReceiptMaster_ID)

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
                                 new SqlParameter("@DV_ID",DV_ID),
                                 new SqlParameter("@ReceiptMaster_ID",ReceiptMaster_ID)



            };


                var response = dbLayer.SP_DataTable_return("InsertReceiptDetailGen_Deposit", paramArray).Tables[0];
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

        //Insert receipt general master
        public string Insert_ReceiptMasterGeneral_DAL(int? Party_ID, int? party_type, string NameText, string Explanation, string Receipttype, string ReceiptDate, string Temp_ID, int? c_ID, int? Created_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Party_ID",Party_ID),
                      new SqlParameter("@party_type",party_type),
                new SqlParameter("@Explanation",Explanation),
                new SqlParameter("@NameText",NameText),
                 new SqlParameter("@Receipttype",Receipttype),
                new SqlParameter("@ReceiptDate",ReceiptDate),
                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Created_by",Created_by)




            };


                var response = dbLayer.SP_DataTable_return("Insert_ReceiptMaster_General", paramArray).Tables[0];
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

        //Update receipt general master
        public string Update_ReceiptMasterGeneral_DAL(int? ReceiptMaster_ID, int? Party_ID, int? party_type, string NameText, string Explanation, string ReceiptDate, string Temp_ID, int? c_ID, int? Modified_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@ReceiptMaster_ID",ReceiptMaster_ID),
                new SqlParameter("@Party_ID",Party_ID),
                new SqlParameter("@party_type",party_type),
                new SqlParameter("@Explanation",Explanation),
                new SqlParameter("@NameText",NameText),

                new SqlParameter("@ReceiptDate",ReceiptDate),
                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Modified_by",Modified_by)




            };


                var response = dbLayer.SP_DataTable_return("Update_ReceiptMaster_General", paramArray).Tables[0];
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


        //---Update receipt detail general
        public string UpdateReceiptDetail_General_DAL(int? ReceiptDetail_ID, int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
            double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date,
            string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by,int PDC_Recieving_Acc_ID)

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
                                 new SqlParameter("@PDC_Recieving_Acc_ID",PDC_Recieving_Acc_ID)



            };


                var response = dbLayer.SP_DataTable_return("UpdateReceiptDetailGen", paramArray).Tables[0];
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


        //Delete receipt master 
        public string DeleteReceiptMaster_DAL(int? ReceiptMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@ReceiptMaster_ID",ReceiptMaster_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_ReceiptMaster", paramArray).Tables[0];
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


        //Delete payment master 
        public string DeletePaymentMaster_DAL(int? PaymentMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@PaymentMaster_ID",PaymentMaster_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_PaymentMaster", paramArray).Tables[0];
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

        //Delete receipt detail
        public string DeleteReceiptDetail_DAL(int? ReceiptDetail_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@ReceiptDetail_ID",ReceiptDetail_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_ReceiptDetail", paramArray).Tables[0];
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
        public IEnumerable<pa_ReceiptDetails_DAL> Get_ReceiptDetailSaleListByID_DAL(string Temp_ID, int? ReceiptMaster_ID)
        {



            string sp = "Select_ReceiptDetail_SaleListByID";
            List<pa_ReceiptDetails_DAL> itemlist = new List<pa_ReceiptDetails_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@ReceiptMaster_ID", ReceiptMaster_ID);

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

                        item.Currency_ShortName = rdr["Currency_ShortName"].ToString();

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

                        item.Grand_Amount = rdr["Grand_Amount"].ToString();
                        item.Grand_VAT_Exp = rdr["Grand_VAT_Exp"].ToString();

                        item.Grand_Total_Amount = rdr["Grand_Total_Amount"].ToString();
                        item.PDC_Recieving_Acc_ID = Convert.ToInt32(rdr["PDC_Recieving_Acc_ID"].ToString());



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

        //---insert into receipt detail Sale
        public string InsertReceiptDetail_Sale_DAL(int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
            double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date,
            string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Created_by, int ReceiptMaster_ID, int PDC_Recieving_Acc_ID)

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
                                 new SqlParameter("@ReceiptMaster_ID",ReceiptMaster_ID),
                                 new SqlParameter("@PDC_Recieving_Acc_ID",PDC_Recieving_Acc_ID)



            };


                var response = dbLayer.SP_DataTable_return("InsertReceiptDetailSale", paramArray).Tables[0];
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
        public string UpdateReceiptDetail_Sale_DAL(int? ReceiptDetail_ID, int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
            double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date,
            string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, string Temp_ID, int? c_ID, int? Modified_by, int PDC_Recieving_Acc_ID)

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
                                 new SqlParameter("@PDC_Recieving_Acc_ID",PDC_Recieving_Acc_ID)



            };


                var response = dbLayer.SP_DataTable_return("UpdateReceiptDetailSale", paramArray).Tables[0];
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


        //Insert receipt  master voucher type of Sale
        public string Insert_ReceiptMasterSale_DAL(int? Party_ID, string NameText, string Explanation, string Receipttype, string ReceiptDate, string Temp_ID, int? c_ID, int? Created_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Vendor_ID",Party_ID),
                new SqlParameter("@Explanation",Explanation),
                new SqlParameter("@NameText",NameText),
                 new SqlParameter("@Receipttype",Receipttype),
                new SqlParameter("@ReceiptDate",ReceiptDate),
                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Created_by",Created_by)




            };


                var response = dbLayer.SP_DataTable_return("Insert_ReceiptMaster_Sale", paramArray).Tables[0];
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

        //Update receipt  master voucher of type Sale
        public string Update_ReceiptMasterSale_DAL(int? ReceiptMaster_ID, int? Party_ID, string NameText, string Explanation, string ReceiptDate, string Temp_ID, int? c_ID, int? Modified_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@ReceiptMaster_ID",ReceiptMaster_ID),
                new SqlParameter("@Vendor_ID",Party_ID),
                new SqlParameter("@Explanation",Explanation),
                new SqlParameter("@NameText",NameText),

                new SqlParameter("@ReceiptDate",ReceiptDate),
                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Modified_by",Modified_by)




            };


                var response = dbLayer.SP_DataTable_return("Update_ReceiptMaster_Sale", paramArray).Tables[0];
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


        //--getting payment master of type vendor by id
        public pa_PaymentMaster_DAL Get_PaymentMasterVendorByID_DAL(int? PaymentMaster_ID)
        {

            string sp = "Select_PaymentMaster_VendorByID";

            pa_PaymentMaster_DAL pm = new pa_PaymentMaster_DAL();
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
                                pm.PaymentMaster_ID = Convert.ToInt32(dr["PaymentMaster_ID"]);
                                pm.PaymentMaster_ref = dr["PaymentMaster_ref"].ToString();
                                pm.Party_ID = dr["Party_ID"].ToString();
                                pm.party_type = dr["party_type"].ToString();
                                pm.NameText = dr["NameText"].ToString();
                                pm.Date = dr["Date"].ToString();
                                pm.Explanation = dr["Explanation"].ToString();
                                pm.PaymentStatus = dr["PaymentStatus"].ToString();

                                pm.create_at = dr["create_at"].ToString();
                                pm.create_by = dr["create_by"].ToString();
                                pm.last_updated_at = dr["last_updated_at"].ToString();
                                pm.last_updated_by = dr["last_updated_by"].ToString();

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

        //Get payment detail list by ID's
        public IEnumerable<Pa_PaymentDetails_DAL> Get_PaymentDetailVendorListByID_DAL(string Temp_ID, int? PaymentMaster_ID)
        {



            string sp = "Select_PaymentDetail_VendorListByID";
            List<Pa_PaymentDetails_DAL> itemlist = new List<Pa_PaymentDetails_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@PaymentMaster_ID", PaymentMaster_ID);

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
                        item.Chassis_No = rdr["Chassis_No"].ToString();
                        item.trans_ref = rdr["trans_ref"].ToString();
                        item.CR_AcountName = rdr["CR_AcountName"].ToString();
                        item.DR_AccountName = rdr["DR_AccountName"].ToString();

                        item.AmountTotal = rdr["AmountTotal"].ToString();
                        item.VatExpTotal = rdr["VatExpTotal"].ToString();
                        item.OtherAmountTotal = rdr["OtherAmountTotal"].ToString();
                        item.Grand_Total = rdr["Grand_Total"].ToString();
                        item.curr_name = rdr["Currency_ShortName"].ToString();
                        item.ExtraChargesFee = rdr["ExtraChargesFee"].ToString();
                        item.ExtraChargesFeeTax = rdr["ExtraChargesFeeTax"].ToString();
                        item.transaction_status = Convert.ToInt32(rdr["transaction_status"].ToString());
                        item.PDC_Payable_Acc_ID = Convert.ToInt32(rdr["PDC_Payable_Acc_ID"].ToString());


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



        //---insert into payment detial vendor
        public string InsertPaymentDetail_Vendor_DAL(int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
            string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount, int? stock_ID, string Temp_ID, int? c_ID, int? Created_by, int PaymentMaster_ID, double TaxAmount, double ExtraCharges, double ExtraChargesTax, int PDC_Payable_Acc_ID)

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
                new SqlParameter("@stock_ID",stock_ID),
                new SqlParameter("@Temp_ID",Temp_ID),
                new SqlParameter("@c_ID",c_ID),
                new SqlParameter("@Created_by",Created_by),
                new SqlParameter("@PaymentMaster_ID",PaymentMaster_ID),
                new SqlParameter("@ExtraChargesFee",ExtraCharges),
                new SqlParameter("@ExtraChargesFeeTax",ExtraChargesTax),
                new SqlParameter("@PDC_Payable_Acc_ID",PDC_Payable_Acc_ID)

            };


                var response = dbLayer.SP_DataTable_return("InsertPaymentDetail_Vendor", paramArray).Tables[0];
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


        //---Update into payment detial vendor
        public string UpdatePaymentDetail_Vendor_DAL(int? PaymentDetails_ID, int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
            string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount, int? stock_ID, string Temp_ID, int? c_ID, int? Modified_by, double TaxAmount, double ExtraCharges, double ExtraChargesTax, int PDC_Payable_Acc_ID)

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
                              new SqlParameter("@stock_ID",stock_ID),
                              new SqlParameter("@Temp_ID",Temp_ID),

                                new SqlParameter("@c_ID",c_ID),
                                 new SqlParameter("@Modified_by",Modified_by),
                                 new SqlParameter("@ExtraChargesFee",ExtraCharges),
                                 new SqlParameter("@ExtraChargesFeeTax",ExtraChargesTax),
                                 new SqlParameter("@PDC_Payable_Acc_ID",PDC_Payable_Acc_ID)


            };


                var response = dbLayer.SP_DataTable_return("UpdatePaymentDetail_Vendor", paramArray).Tables[0];
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


        //Insert payment general master of type vendor
        public string Insert_PaymentMasterVendor_DAL(int? Vendor_ID, int? party_type, string Explanation, string PaymentType, string Date, string NameText, string Temp_ID, int? c_ID, int? Created_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Vendor_ID",Vendor_ID),
                new SqlParameter("@party_type",party_type),
                new SqlParameter("@Explanation",Explanation),
                new SqlParameter("@PaymentType",PaymentType),

                new SqlParameter("@Date",Date),
                 new SqlParameter("@NameText",NameText),
                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Created_by",Created_by)




            };


                var response = dbLayer.SP_DataTable_return("Insert_PaymentMaster_Vendor", paramArray).Tables[0];
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


        //Update payment general master of type vendor
        public string Update_PaymentVendor_DAL(int? PaymentMaster_ID, int? Vendor_ID, int? party_type, string Explanation, string Date, string NameText, string Temp_ID, int? c_ID, int? Modified_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                      new SqlParameter("@PaymentMaster_ID",PaymentMaster_ID),
                new SqlParameter("@Vendor_ID",Vendor_ID),
                new SqlParameter("@party_type",party_type),
                new SqlParameter("@Explanation",Explanation),

                new SqlParameter("@Date",Date),
                  new SqlParameter("@NameText",NameText),
                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@Modified_by",Modified_by)

            };


                var response = dbLayer.SP_DataTable_return("Update_PaymentMaster_Vendor", paramArray).Tables[0];
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

        //change payment Master status
        public string ChangePaymentMasterStatus_DAL(int? PaymentMaster_ID, int? Status_ID, int? c_ID, string Trans_Type, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@PaymentMaster_ID",PaymentMaster_ID),
                  new SqlParameter("@Status_ID",Status_ID),
                   new SqlParameter("@c_ID",c_ID),
                    new SqlParameter("@Trans_Type",Trans_Type),
                   new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Update_PaymentStatus", paramArray).Tables[0];
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

        //change receipt Master status
        public string ChangeReceiptMasterStatus_DAL(int? ReceiptMaster_ID, int? Status_ID, int? c_ID, string Trans_Type, int? Modified_by,string QRName)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@ReceiptMaster_ID",ReceiptMaster_ID),
                  new SqlParameter("@Status_ID",Status_ID),
                   new SqlParameter("@c_ID",c_ID),
                    new SqlParameter("@Trans_Type",Trans_Type),
                    new SqlParameter("@QRName",QRName),
                   new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Update_ReceiptStatus", paramArray).Tables[0];
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

        //Get payment master attachment list
        public IEnumerable<Pa_Attachments_DAL> GetPaymentsMaster_Attachments_DAL(int? PaymentMaster_ID, string Type)
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
                    cmd.Parameters.AddWithValue("@Master_ID", PaymentMaster_ID);
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

        //Insert payment master attachment
        public string Insert_AttachmentPayment_DAL(int? PaymentMaster_ID, string Document_Type, string Type, string newFileName, string filepath, int? Created_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Master_ID",PaymentMaster_ID),
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

        //---delete payment attachment
        public string Delete_Attachment_Payment(int? Attachment_ID)
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

        //Get receipt master attachment list
        public IEnumerable<Pa_Attachments_DAL> GetReceiptMaster_Attachments_DAL(int? ReceiptMaster_ID, string Type)
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
                    cmd.Parameters.AddWithValue("@Master_ID", ReceiptMaster_ID);
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

        //Insert receipt master attachment
        public string Insert_AttachmentReceipt_DAL(int? ReceiptMaster_ID, string Document_Type, string Type, string newFileName, string filepath, int? Created_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Master_ID",ReceiptMaster_ID),
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
        public DataSet GetDataForPrintPaymentMaster(int? PaymentMaster_ID)
        {
            using (con = new SqlConnection(Constr))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Select_PaymentMaster_Print_ByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PaymentMaster_ID", PaymentMaster_ID);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                return ds;
            }
        }
        public DataSet GetDataForPrintReceiptMaster(int? ReceiptMaster_ID)
        {
            using (con = new SqlConnection(Constr))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Select_ReceiptMaster_Print_ByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReceiptMaster_ID", ReceiptMaster_ID);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                return ds;
            }
        }

        #region Journal Voucher

        //property
        public JournalVoucher_DAL JournalVoucherDetailObject { get; set; }
        public IEnumerable<JournalVoucher_DAL> JournalVoucherDetailList { get; set; }
        public IEnumerable<JournalVoucher_DAL> JournalVoucherMasterList { get; set; }
        public JournalVoucher_DAL JournalVoucherMasterObject { get; set; }
        public IEnumerable<Pa_PaymentDetails_DAL> Get_Trans_Ref { get ; set; }



        public IPagedList<PDCReport_DAL> PDCReportPageList { get; set; }



        //method
        public IEnumerable<JournalVoucher_DAL> Get_JournalVoucher_master_DAL()
        {
            string sp = "select_JVMasterList";
            List<JournalVoucher_DAL> itemlist = new List<JournalVoucher_DAL>();

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
                        JournalVoucher_DAL item = new JournalVoucher_DAL();


                        item.JVMaster_ID = Convert.ToInt32(rdr["JVMaster_ID"].ToString());
                        item.JVMaster_ref = rdr["JVMaster_ref"].ToString();
                        item.Date = rdr["Date"].ToString();


                        item.Created_at = rdr["Created_at"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Created_by = rdr["Created_by_Name"].ToString();
                        item.Modified_by = rdr["Modified_by_Name"].ToString();


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
        public IEnumerable<JournalVoucher_DAL> Get_JournalVoucher_DetailListByID_DAL(int? JVMaster_ID, string temp_ID)
        {



            string sp = "select_JVDetailList";
            List<JournalVoucher_DAL> itemlist = new List<JournalVoucher_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@JVMaster_ID", JVMaster_ID);
                    cmd.Parameters.AddWithValue("@temp_ID", temp_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        JournalVoucher_DAL item = new JournalVoucher_DAL();



                        item.JVDetails_ID = Convert.ToInt32(rdr["JVDetails_ID"].ToString());
                        item.Date = rdr["Date"].ToString();
                        item.Account_ID = Convert.ToInt32(rdr["AccountID"].ToString());
                        item.AccountName = rdr["AccountName"].ToString();

                        item.DR_Amount = Convert.ToDouble(rdr["DR_Amount"].ToString());
                        item.Party_ID = Convert.ToInt32(rdr["Party_ID"].ToString());

                        item.CR_Amount = Convert.ToDouble(rdr["CR_Amount"].ToString());

                        item.temp_ID = rdr["temp_ID"].ToString();
                        item.Description = rdr["Description"].ToString();
                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by_Name"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by_Name"].ToString();








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
        public string Insert_JournalVoucherMaster_DAL(string date, int? c_ID, string temp_ID, int? Created_by)
        {

            string msg = "";



            try
            {


                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@date",date),
                new SqlParameter("@c_ID" ,c_ID),



                new SqlParameter("@temp_ID",temp_ID),
                new SqlParameter("@User_ID",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Insert_JVmaster", paramArray).Tables[0];
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


        public string Insert_JournalVoucherDetail_DAL(string Date, int? Account_ID, double? DR_Amount, double? CR_Amount, string Description, int? Party_ID, string temp_ID, int? Created_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Date",Date),
                new SqlParameter("@Account_ID" ,Account_ID),
                new SqlParameter("@DR_Amount" ,DR_Amount),

                new SqlParameter("@CR_Amount" ,CR_Amount),
                new SqlParameter("@Description" ,Description),
                new SqlParameter("@Party_ID" ,Party_ID),


                     new SqlParameter("@temp_ID",temp_ID),
                new SqlParameter("@User_ID",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Insert_JournalVoucherDetails", paramArray).Tables[0];
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
        public JournalVoucher_DAL Get_JournalVoucherMaster_Id(int? JVMaster_ID)
        {
            string sp = "select_JVMasterList_ById";
            JournalVoucher_DAL item = new JournalVoucher_DAL();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@JVMaster_ID", JVMaster_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {



                        item.JVMaster_ID = Convert.ToInt32(rdr["JVMaster_ID"].ToString());
                        item.JVMaster_ref = rdr["JVMaster_ref"].ToString();
                        item.Date = rdr["Date"].ToString();
                        item.Created_at = rdr["Created_at"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Created_by = rdr["Created_by_Name"].ToString();
                        item.Modified_by = rdr["Modified_by_Name"].ToString();




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


        public string Update_JournalVoucher_DAL(string date, int? JVMaster_ID, int? Modified_by)
        {

            string msg = "";

            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@JVMaster_ID",JVMaster_ID),

                new SqlParameter("@date",date ),



                new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Update_JVMaster", paramArray).Tables[0];
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


        public string Update_JournalVoucher_detail_DAL(int? JVDetails_ID, string Date, int? Account_ID, double? DR_Amount, double? CR_Amount, string Description, int? Party_ID, int? Modified_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {



                new SqlParameter("@Date" ,Date ),
                new SqlParameter("@Account_ID",Account_ID ),
                new SqlParameter("@DR_Amount",DR_Amount ),
                new SqlParameter("@CR_Amount",CR_Amount ),
                new SqlParameter("@Description",Description ),
                new SqlParameter("@Party_ID",Party_ID ),
                new SqlParameter("@User_ID",Modified_by),
                 new SqlParameter("@JVDetails_ID",JVDetails_ID )


            };


                var response = dbLayer.SP_DataTable_return("Update_JournalVoucher_details", paramArray).Tables[0];
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


        public string GetOldTempIDFromJVDetail_DAL(int? JVMaster_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@JVMaster_ID",JVMaster_ID)

            };


                var response = dbLayer.SP_DataTable_return("SelectJVDetailOldTempID", paramArray).Tables[0];
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

        #region Financial Statements



        public List<IncomeStatement> IncomeStatment_INC_DAL(string StartDate, string EndDate, int? c_ID)
        {
            List<IncomeStatement> RD = new List<IncomeStatement>();
            string sp = "pa_IncomeStatment_INC";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@start_Date", StartDate);
                        cmd.Parameters.AddWithValue("@end_Date", EndDate);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                IncomeStatement o = new IncomeStatement();
                                o.Acount_ID= Convert.ToInt32(dr["AccountID"].ToString());
                                o.ACCOUNT = dr["ACCOUNT"].ToString();
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

        public List<IncomeStatement> IncomeStatment_Exp_DAL(string StartDate, string EndDate, int? c_ID)
        {
            List<IncomeStatement> RD = new List<IncomeStatement>();
            string sp = "pa_IncomeStatment_Exp";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@start_Date", StartDate);
                        cmd.Parameters.AddWithValue("@end_Date", EndDate);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                IncomeStatement o = new IncomeStatement();
                                o.Acount_ID =  Convert.ToInt32(dr["AccountID"].ToString());
                                o.ACCOUNT = dr["ACCOUNT"].ToString();
                                o.Balance = dr["Balance"].ToString();
                                o.sub_HeadAccount = dr["sub_HeadAccount"].ToString();

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

        public List<IncomeStatement> IncomeStatment_CGS_DAL(string StartDate, string EndDate, int? c_ID)
        {
            List<IncomeStatement> RD = new List<IncomeStatement>();
            string sp = "pa_IncomeStatment_CGS";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@start_Date", StartDate);
                        cmd.Parameters.AddWithValue("@end_Date", EndDate);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                IncomeStatement o = new IncomeStatement();
                                o.Acount_ID = Convert.ToInt32(dr["AccountID"].ToString());
                                o.ACCOUNT = dr["ACCOUNT"].ToString();
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

        public List<IncomeStatement> IncomeStatment_disRet(string StartDate, string EndDate, int? c_ID)
        {
            List<IncomeStatement> RD = new List<IncomeStatement>();
            string sp = "pa_IncomeStatment_disRet";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@start_Date", StartDate);
                        cmd.Parameters.AddWithValue("@end_Date", EndDate);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                IncomeStatement o = new IncomeStatement();
                                o.ACCOUNT = dr["ACCOUNT"].ToString();
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

        public List<IncomeStatement> IncomeStatment_Exp_Fin_DAL(string StartDate, string EndDate, int? c_ID)
        {
            List<IncomeStatement> RD = new List<IncomeStatement>();
            string sp = "pa_IncomeStatment_Exp_Fin";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@start_Date", StartDate);
                        cmd.Parameters.AddWithValue("@end_Date", EndDate);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                IncomeStatement o = new IncomeStatement();
                                o.ACCOUNT = dr["ACCOUNT"].ToString();
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

        public List<IncomeStatement> IncomeStateMent_DAL(string StartDate, string EndDate, int? c_ID)
        {
            List<IncomeStatement> RD = new List<IncomeStatement>();
            string sp = "pa_IncomeStateMent";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@start_Date", StartDate);
                        cmd.Parameters.AddWithValue("@end_Date", EndDate);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                IncomeStatement o = new IncomeStatement();

                                o.Total_Income = dr["Total_Income"].ToString();
                                o.Net_Sale_Service_Income = dr["Net_Sale_Service_Income"].ToString();
                                o.cost_of_GoodSold = dr["cost_of_GoodSold"].ToString();
                                o.Gross_Profit = dr["Gross_Profit"].ToString();
                                o.total_Expense = dr["total_Expense"].ToString();
                                o.Operating_Profit = dr["Operating_Profit"].ToString();
                                o.Financial_Expense = dr["Financial_Expense"].ToString();
                                o.Net_PL = dr["Net_PL"].ToString();
                                o.C_Code = dr["C_Code"].ToString();


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

        public List<Balance_Sheet> Balance_Sheet_DAL(string StartDate, string EndDate, int? c_ID)
        {
            List<Balance_Sheet> RD = new List<Balance_Sheet>();
            string sp = "pa_Balance_Sheet";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@start_Date", StartDate);
                        cmd.Parameters.AddWithValue("@end_Date", EndDate);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Balance_Sheet o = new Balance_Sheet();
                                o.TTL_Assets = dr["TTL_Assets"].ToString();
                                o.TTL_Owner_Equity = dr["TTL_Owner_Equity"].ToString();
                                o.Diff = dr["Diff"].ToString();
                                o.TTL_Current_Assets = dr["TTL_Current_Assets"].ToString();
                                o.TTL_Fixed_Assets = dr["TTL_Fixed_Assets"].ToString();
                                o.TTL_Current_Laibilities = dr["TTL_Current_Laibilities"].ToString();
                                o.TTL_Longterm_Laibilities = dr["TTL_Longterm_Laibilities"].ToString();
                                o.TTL_Laibilities = dr["TTL_Laibilities"].ToString();

                                o.TTL_Drawings = dr["TTL_Drawings"].ToString();
                                o.Owner_Equity = dr["Owner_Equity"].ToString();
                                o.OwnerEquity_less_Drawing = dr["OwnerEquity_less_Drawing"].ToString();
                                o.Net_Profit = dr["Net_Profit"].ToString();
                                o.C_Code = dr["C_Code"].ToString();
                                //Period
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

        public List<Balance_Sheet> BalSh_byCurrentAssets_DAL(string StartDate, string EndDate, int? c_ID)
        {
            List<Balance_Sheet> RD = new List<Balance_Sheet>();
            string sp = "pa_BalSh_bySubAccHeads";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@start_Date", StartDate);
                        cmd.Parameters.AddWithValue("@end_Date", EndDate);
                        cmd.Parameters.AddWithValue("@SubAccountsHead_ID", 1);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Balance_Sheet o = new Balance_Sheet();
                                o.Account = dr["Account"].ToString();
                                o.Amount = dr["Amount"].ToString();
                                o.LINK = dr["LINK"].ToString();
                                o.TTL_Assets = dr["TTL_Assets"].ToString();

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

        public List<Balance_Sheet> BalSh_byDrawings_DAL(string StartDate, string EndDate, int? c_ID)
        {
            List<Balance_Sheet> RD = new List<Balance_Sheet>();
            string sp = "pa_BalSh_bySubAccHeads";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@start_Date", StartDate);
                        cmd.Parameters.AddWithValue("@end_Date", EndDate);
                        cmd.Parameters.AddWithValue("@SubAccountsHead_ID", 16);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Balance_Sheet o = new Balance_Sheet();
                                o.Account = dr["Account"].ToString();
                                o.Amount = dr["Amount"].ToString();
                                o.LINK = dr["LINK"].ToString();
                                o.TTL_Assets = dr["TTL_Assets"].ToString();

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

        public List<Balance_Sheet> BalSh_byCurrentLaibilities_DAL(string StartDate, string EndDate, int? c_ID)
        {
            List<Balance_Sheet> RD = new List<Balance_Sheet>();
            string sp = "pa_BalSh_bySubAccHeads_Laibilities";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@start_Date", StartDate);
                        cmd.Parameters.AddWithValue("@end_Date", EndDate);
                        cmd.Parameters.AddWithValue("@SubAccountsHead_ID", 3);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Balance_Sheet o = new Balance_Sheet();
                                o.Account = dr["Account"].ToString();
                                o.Amount = dr["Amount"].ToString();
                                o.LINK = dr["LINK"].ToString();
                                o.TTL_Assets = dr["TTL_Assets"].ToString();

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

        public List<Balance_Sheet> BalSh_byLongTermLaibilities_DAL(string StartDate, string EndDate, int? c_ID)
        {
            List<Balance_Sheet> RD = new List<Balance_Sheet>();
            string sp = "pa_BalSh_bySubAccHeads";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@start_Date", StartDate);
                        cmd.Parameters.AddWithValue("@end_Date", EndDate);
                        cmd.Parameters.AddWithValue("@SubAccountsHead_ID", 4);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Balance_Sheet o = new Balance_Sheet();
                                o.Account = dr["Account"].ToString();
                                o.Amount = dr["Amount"].ToString();
                                o.LINK = dr["LINK"].ToString();
                                o.TTL_Assets = dr["TTL_Assets"].ToString();

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

        public List<Balance_Sheet> BalSh_byFixedAssets_DAL(string StartDate, string EndDate, int? c_ID)
        {
            List<Balance_Sheet> RD = new List<Balance_Sheet>();
            string sp = "pa_BalSh_bySubAccHeads";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@start_Date", StartDate);
                        cmd.Parameters.AddWithValue("@end_Date", EndDate);
                        cmd.Parameters.AddWithValue("@SubAccountsHead_ID", 2);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Balance_Sheet o = new Balance_Sheet();
                                o.Account = dr["Account"].ToString();
                                o.Amount = dr["Amount"].ToString();
                                o.LINK = dr["LINK"].ToString();
                                o.TTL_Assets = dr["TTL_Assets"].ToString();

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

        public List<Balance_Sheet> BalSh_byCapitalInvestments_DAL(string StartDate, string EndDate, int? c_ID)
        {
            List<Balance_Sheet> RD = new List<Balance_Sheet>();
            string sp = "pa_BalSh_bySubAccHeads_Capital";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@start_Date", StartDate);
                        cmd.Parameters.AddWithValue("@end_Date", EndDate);
                        cmd.Parameters.AddWithValue("@SubAccountsHead_ID", 11);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Balance_Sheet o = new Balance_Sheet();
                                o.Account = dr["Account"].ToString();
                                o.Amount = dr["Amount"].ToString();
                                o.LINK = dr["LINK"].ToString();
                                o.TTL_Assets = dr["TTL_Assets"].ToString();

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

        public List<Balance_Sheet> Get_NetIncome_onPrint_DAL(string StartDate, string EndDate, int? c_ID)
        {
            List<Balance_Sheet> RD = new List<Balance_Sheet>();
            string sp = "pa_Get_NetIncome_onPrint";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@start_Date", StartDate);
                        cmd.Parameters.AddWithValue("@end_Date", EndDate);
                        cmd.Parameters.AddWithValue("@c_ID", c_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Balance_Sheet o = new Balance_Sheet();
                                o.Net_PL = dr["Net_PL"].ToString();


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

        public List<pa_PaymentMaster_DAL> pa_GetParty_Balance(string Vendor_ID = "0", int PaymentMaster_ID = 0)
        {
            List<pa_PaymentMaster_DAL> RD = new List<pa_PaymentMaster_DAL>();
            string sp = "pa_GetParty_Balance";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Vendor_ID", Vendor_ID);
                        cmd.Parameters.AddWithValue("@PaymentMaster_ID", PaymentMaster_ID);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_PaymentMaster_DAL o = new pa_PaymentMaster_DAL();
                                o.Party_ID = dr["Party_ID"].ToString();

                                o.Total_Amount = dr["Total_Amount"].ToString();
                                o.PurchaseDate = dr["PurchaseDate"].ToString();
                                o.Type = dr["Type"].ToString();
                                o.Ref = dr["Ref"].ToString();
                                o.Paid = dr["Paid"].ToString();
                                o.Balance = dr["Balance"].ToString();
                                o.PaymentMaster_ID = Convert.ToInt32(dr["PaymentMaster_ID"]);

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
        public List<pa_ReceiptMaster_DAL> pa_GetParty_Balance_Receipt(string CustomerID = "0", int ReceiptMaster_ID =0 , int C_id = 0)
        {
            List<pa_ReceiptMaster_DAL> RD = new List<pa_ReceiptMaster_DAL>();
            string sp = "pa_GetParty_Balance_Reciept";

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                        cmd.Parameters.AddWithValue("@ReceiptMaster_ID", ReceiptMaster_ID);
                       
                        cmd.Parameters.AddWithValue("@C_id", C_id);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pa_ReceiptMaster_DAL o = new pa_ReceiptMaster_DAL();
                                o.Party_ID = dr["Party_ID"].ToString();                                
                                o.Ref = dr["Ref"].ToString();
                                o.Sale_Date = dr["SaleDate"].ToString();
                                o.Total_Amount = dr["Total_Amount"].ToString();
                                o.Type = dr["Type"].ToString();
                                o.Paid = dr["Paid"].ToString();
                                o.Balance = dr["Balance"].ToString();
                                o.ReceiptMaster_ID = Convert.ToInt32(dr["ReceiptMaster_ID"].ToString());                                
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
        public IEnumerable<Pa_Partners_DAL> Get_tblaccounts_VendorName(int party_type, int c_ID)
        {
            string sp = "pa_Get_tblaccounts_VendorName";
            List<Pa_Partners_DAL> Closed_Auction_Offers = new List<Pa_Partners_DAL>();

            if (party_type == 0 || party_type < 0)
            {
                party_type = 0;
            }


            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@party_type", party_type);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Partners_DAL item = new Pa_Partners_DAL();

                        item.Partner_ID = Convert.ToInt32(rdr["Party_ID"]);



                        item.PartnerName = rdr["PartyName"].ToString();



                        Closed_Auction_Offers.Add(item);

                    }

                    con.Close();

                }

                return Closed_Auction_Offers;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        public pa_Vanning_Details ValidateChassisNo_JP(string ChassisNo)
        {


            string sp = "pa_Get_StockMain_ChassisNoJP";
            pa_Vanning_Details ChsValid = new pa_Vanning_Details();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChassisNo", ChassisNo);
                    SqlDataReader record = cmd.ExecuteReader();


                    if (record.Read())
                    {

                        ChsValid.Stock_ID = Convert.ToInt32(record["stock_ID"]);
                        ChsValid.Chassis_no = record["Chassis_No"].ToString();
                        ChsValid.result = "Chassis Exists";
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
        public IEnumerable<pa_PaymentMaster_DAL> Get_PaymentMasterList_DAL1(string PaymentRef, int Party_ID_Name, string StartDate,
   string EndDate, string Chassis_No, string Paid_To, string PurchaseRef, string VoucherType, int c_ID)
        {


            string sp = "Select_PaymentMaster_List";
            List<pa_PaymentMaster_DAL> itemlist = new List<pa_PaymentMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PaymentRef", PaymentRef);
                    cmd.Parameters.AddWithValue("@Party_ID_Name", Party_ID_Name);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                    cmd.Parameters.AddWithValue("@Paid_To", Paid_To);
                    cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                    cmd.Parameters.AddWithValue("@VoucherType", VoucherType);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);



                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_PaymentMaster_DAL item = new pa_PaymentMaster_DAL();


                        item.PaymentMaster_ID = Convert.ToInt32(rdr["PaymentMaster_ID"]);
                        item.PaymentMaster_ref = rdr["PaymentMaster_ref"].ToString();
                        item.Date = rdr["DATE"].ToString();
                        item.Party_ID_Name = rdr["Party_ID_Name"].ToString();
                        item.Paymenttype = rdr["Paymenttype"].ToString();
                        item.Explanation = rdr["Explanation"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();
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

        public IEnumerable<pa_PaymentMaster_DAL> Get_PaymentMasterList_TTL_DAL1(string PaymentRef, int Party_ID_Name, string StartDate,
         string EndDate, string Chassis_No, string Paid_To,
         string PurchaseRef, string VoucherType, int c_ID)
        {


            string sp = "Select_PaymentMaster_List_TTL";
            List<pa_PaymentMaster_DAL> itemlist = new List<pa_PaymentMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PaymentRef", PaymentRef);
                    cmd.Parameters.AddWithValue("@Party_ID_Name", Party_ID_Name);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@Chassis_No", Chassis_No);
                    cmd.Parameters.AddWithValue("@Paid_To", Paid_To);
                    cmd.Parameters.AddWithValue("@PurchaseRef", PurchaseRef);
                    cmd.Parameters.AddWithValue("@VoucherType", VoucherType);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_PaymentMaster_DAL item = new pa_PaymentMaster_DAL();

                        item.Amount = rdr["Amount"].ToString();
                        item.VAT_Exp = rdr["VAT_Exp"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();

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
        public IEnumerable<pa_ReceiptMaster_DAL> Get_ReceiptMaster_List_DAL1(string ReceiptRef,
            int Party_ID, string PartyNameText, string SaleRef, string Chassis_no, string StartDate, string EndDate, string Cheque_no, int c_ID)
        {


            string sp = "Select_ReceiptMaster_List";
            List<pa_ReceiptMaster_DAL> itemlist = new List<pa_ReceiptMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ReceiptRef", ReceiptRef);
                    cmd.Parameters.AddWithValue("@Party_ID", Party_ID);
                    cmd.Parameters.AddWithValue("@PartyNameText", PartyNameText);
                    cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                    cmd.Parameters.AddWithValue("@Chassis_no", Chassis_no);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    cmd.Parameters.AddWithValue("@Cheque_no", Cheque_no);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_ReceiptMaster_DAL item = new pa_ReceiptMaster_DAL();

                        item.ReceiptMaster_ID = Convert.ToInt32(rdr["ReceiptMaster_ID"]);
                        item.ReceiptMaster_ref = rdr["ReceiptMaster_ref"].ToString();
                        item.ReceiptDate = rdr["DATE"].ToString();
                        item.Party_ID_Name = rdr["Party_ID_Name"].ToString();
                        item.Explanation = rdr["Explanation"].ToString();
                        item.Receipttype = rdr["Receipttype"].ToString();
                        item.Total_Amount = rdr["Total_Amount"].ToString();
                        item.transaction_status = rdr["transaction_status"].ToString();

                        item.Created_at = rdr["Created_at"].ToString();
                        item.Created_by = rdr["Created_by"].ToString();
                        item.Modified_at = rdr["Modified_at"].ToString();
                        item.Modified_by = rdr["Modified_by"].ToString();
                        item.AfterVATTotal = rdr["AfterVATTotal"].ToString();




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

        public IEnumerable<pa_ReceiptMaster_DAL> Get_ReceiptMaster_List_TTL_DAL1(string ReceiptRef,
            int Party_ID, string PartyNameText, string SaleRef, string Chassis_no, string StartDate, string EndDate, int c_ID)
        {


            string sp = "Select_ReceiptMaster_List_TTL";
            List<pa_ReceiptMaster_DAL> itemlist = new List<pa_ReceiptMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ReceiptRef", ReceiptRef);
                    cmd.Parameters.AddWithValue("@Party_ID", Party_ID);
                    cmd.Parameters.AddWithValue("@PartyNameText", PartyNameText);
                    cmd.Parameters.AddWithValue("@SaleRef", SaleRef);
                    cmd.Parameters.AddWithValue("@Chassis_no", Chassis_no);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_ReceiptMaster_DAL item = new pa_ReceiptMaster_DAL();


                        item.Total_Amount = rdr["Total_Amount"].ToString();
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







        //new faraz work


        public IEnumerable<Pa_Ledger_DAL> Get_LedgerList_DAL_NEW(string StartDate, int AccountID, string EndDate,
           string Trans_Ref, int c_ID)
        {


            string sp = "pa_Select_Ledger";
            List<Pa_Ledger_DAL> itemlist = new List<Pa_Ledger_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //these are the parameters
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@AccountID", AccountID);
                    cmd.Parameters.AddWithValue("@Trans_Ref", Trans_Ref);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Ledger_DAL item = new Pa_Ledger_DAL();

                        //Convert.ToInt32(rdr["PurchaseDetails_ID"]);
                        item.Date = rdr["Date"].ToString();
                        item.Debit = rdr["Debit"].ToString();
                        item.Credit = rdr["Credit"].ToString();
                        item.Description = rdr["Description"].ToString();
                        item.trans_ref = rdr["trans_ref"].ToString();
                        item.ACCOUNT = rdr["ACCOUNT"].ToString();
                        //item.AccountID = rdr["AccountID"].ToString();

                        item.trans_ref_ID = rdr["trans_ref_ID"].ToString();
                        item.Link = rdr["Link"].ToString();

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

        public IEnumerable<Pa_Ledger_DAL> Get_LedgerList_DAL_TTL1(string StartDate, int AccountID, string EndDate,
        string Trans_Ref, int c_ID)
        {


            string sp = "pa_Select_Ledger_TTL";
            List<Pa_Ledger_DAL> itemlist = new List<Pa_Ledger_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //these are the parameters
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@AccountID", AccountID);
                    cmd.Parameters.AddWithValue("@Trans_Ref", Trans_Ref);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Ledger_DAL item = new Pa_Ledger_DAL();

                        //Convert.ToInt32(rdr["PurchaseDetails_ID"]);

                        item.TotalDebit = rdr["TotalDebit"].ToString();
                        item.TotalCredit = rdr["TotalCredit"].ToString();
                        item.TotalBalance = rdr["TotalBalance"].ToString();
                        item.Opening_Balance = rdr["Opening_Balance"].ToString();
                        item.Closing_Balance = rdr["Closing_Balance"].ToString();

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


        //WOrk By Rafay
        public IEnumerable<PartyType_DAL> Get_Party_Type_By_Name()
        {
            string sp = "Get_All_Party";
            List<PartyType_DAL> itemlist = new List<PartyType_DAL>();

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
                        PartyType_DAL item = new PartyType_DAL();


                        item.PartyTypeID = Convert.ToInt32(rdr["Party_Type_ID"]);
                        item.PartyType = rdr["Party_Type"].ToString();



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
        public IEnumerable<TaxReport> Get_PaidTax_Report_jp(string StartDate, string EndDate)
        {


            string sp = "select_PaidTax_Report_jp";
            List<TaxReport> itemlist = new List<TaxReport>();

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
                        TaxReport item = new TaxReport();


                        item.Chassis_No = rdr["Chassis_No"].ToString();
                        item.PurchaseDate = rdr["PurchaseDate"].ToString();
                        item.PriceTax = rdr["PriceTax"].ToString();
                        item.AuctionFeeTax = rdr["AuctionFeeTax"].ToString();
                        item.ReksoFeeTax = rdr["ReksoFeeTax"].ToString();
                        item.Tax_Amount = rdr["VanningAmountTax"].ToString();
                        item.Inspection_ChargesTax = rdr["Inspection_ChargesTax"].ToString();

                        item.Berth_Carry_ChargesTax = rdr["Berth_Carry_ChargesTax"].ToString();
                        item.ExtraChargesFeeTax = rdr["PaymentTransferRefTax"].ToString();
                        item.TaxAmount = rdr["TaxAmount"].ToString();
                        item.TaxAmountOthers = rdr["TaxAmountOthers"].ToString();
                        item.Total = rdr["Total"].ToString();




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

        public IEnumerable<TaxReport> Get_PaidTax_Report_ttl_jp(string StartDate, string EndDate)
        {


            string sp = "select_PaidTax_Report_ttl_jp";
            List<TaxReport> itemlist = new List<TaxReport>();

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
                        TaxReport item = new TaxReport();


                        item.PriceTax_ttl = rdr["PriceTax"].ToString();
                        item.AuctionFeeTax_ttl = rdr["AuctionFeeTax"].ToString();
                        item.ReksoFeeTax_ttl = rdr["ReksoFeeTax"].ToString();
                        item.Tax_Amount_ttl = rdr["VanningAmountTax"].ToString();
                        item.Inspection_ChargesTax_ttl = rdr["Inspection_ChargesTax"].ToString();

                        item.Berth_Carry_ChargesTax_ttl = rdr["Berth_Carry_ChargesTax"].ToString();
                        item.ExtraChargesFeeTax_ttl = rdr["PaymentTransferRefTax"].ToString();
                        item.TaxAmount_ttl = rdr["TaxAmount"].ToString();
                        item.TaxAmountOthers_ttl = rdr["TaxAmountOthers"].ToString();
                        item.Total_ttl = rdr["Total"].ToString();




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

        public IEnumerable<TaxReport> Get_ReceivedTax_Report_jp(string StartDate, string EndDate)
        {


            string sp = "select_ReceivedTax_Report_jp";
            List<TaxReport> itemlist = new List<TaxReport>();

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
                        TaxReport item = new TaxReport();


                        item.Chassis_No = rdr["Chassis_No"].ToString();
                        item.SaleDate = rdr["SaleDate"].ToString();
                        item.PriceTax = rdr["PriceTax"].ToString();
                        item.AuctionFeeTax = rdr["AuctionFeeTax"].ToString();
                        item.OfficeChargesTax = rdr["OfficeChargesTax"].ToString();

                        item.Total = rdr["Total"].ToString();




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

        public IEnumerable<TaxReport> Get_ReceivedTax_Report_ttl_jp(string StartDate, string EndDate)
        {


            string sp = "select_ReceivedTax_Report_ttl_jp";
            List<TaxReport> itemlist = new List<TaxReport>();

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
                        TaxReport item = new TaxReport();


                        item.PriceTax_ttl = rdr["PriceTax"].ToString();
                        item.AuctionFeeTax_ttl = rdr["AuctionFeeTax"].ToString();
                        item.OfficeChargesTax_ttl = rdr["OfficeChargesTax"].ToString();
                        item.Total_ttl = rdr["Total"].ToString();




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

        public IEnumerable<RecycleReport> Get_Recycle_Report_jp(string StartDate, string EndDate, string ChassisNo)
        {


            string sp = "select_Recycle_Report_jp";
            List<RecycleReport> itemlist = new List<RecycleReport>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@ChassisNo", ChassisNo);



                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        RecycleReport item = new RecycleReport();


                        item.Chassis_No = rdr["Chassis_No"].ToString();
                        item.Date = rdr["Date"].ToString();
                        item.Ref = rdr["Ref"].ToString();
                        item.RecycleFee_Out = rdr["RecycleFee_Out"].ToString();
                        item.RecycleFee_In = rdr["RecycleFee_In"].ToString();






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

        public IEnumerable<RecycleReport> Get_Recycle_Report_ttl_jp(string StartDate, string EndDate, string ChassisNo)
        {


            string sp = "select_Recycle_Report_ttl_jp";
            List<RecycleReport> itemlist = new List<RecycleReport>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@ChassisNo", ChassisNo);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        RecycleReport item = new RecycleReport();


                        item.RecycleFee_Out_ttl = rdr["RecycleFee_Out"].ToString();
                        item.RecycleFee_In_ttl = rdr["RecycleFee_In"].ToString();
                        item.Balance = rdr["balance"].ToString();





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
        //Get_TrailBalanceList_TBL
        public IEnumerable<Pa_TrailBalance_DAL> Get_TrailBalanceList_TTL(string StartDate, String EndDate, int c_ID)
        {


            string sp = "pa_TrailBalance_ttl";
            List<Pa_TrailBalance_DAL> itemlist = new List<Pa_TrailBalance_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //these are the parameters
                    cmd.Parameters.AddWithValue("@start_Date", StartDate);
                    cmd.Parameters.AddWithValue("@end_Date", EndDate);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);



                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_TrailBalance_DAL item = new Pa_TrailBalance_DAL();


                        item.Debit = rdr["DR"].ToString();

                        item.Credit = rdr["CR"].ToString();



                        //now

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



        public IEnumerable<PDCReport_DAL> Get_PDCReport_DAL(string StartDate, string EndDate,string cheque_no, string Ref_ID, int c_ID)
        {
            string sp = "select_PDC";
            List<PDCReport_DAL> itemlist = new List<PDCReport_DAL>();
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@start_Date", StartDate);
                    cmd.Parameters.AddWithValue("@end_Date", EndDate);
                    cmd.Parameters.AddWithValue("@cheque_no", cheque_no);
                    cmd.Parameters.AddWithValue("@Ref_ID", Ref_ID);
                    cmd.Parameters.AddWithValue("@c_ID ", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PDCReport_DAL item = new PDCReport_DAL();
                        item.PaymentType = rdr["TYpe"].ToString();
                        item.cheque_date = rdr["cheque_date"].ToString();
                        item.cheque_no = rdr["cheque_no"].ToString();
                        item.bank_name = rdr["cheque_bank_name"].ToString();
                        item.ID = rdr["ID"].ToString();
                        item.Ref_ID = rdr["Ref_ID"].ToString();
                        item.Description = rdr["Description"].ToString();
                        item.Date = rdr["Date"].ToString();
                        item.Master_ID = Convert.ToInt32(rdr["Master_ID"]);
                        item.IsChequeCleared = Convert.ToInt32(rdr["IsChequeCleared"]);
                       
                        
                        //now	
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



        public string Post_PDCReport_DAL(int? Pv_id, int user_ID, int C_id)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@PV_ID",Pv_id),
                new SqlParameter("@userID",user_ID),
                new SqlParameter("@C_ID",C_id)


            };

                var response = dbLayer.SP_DataTable_return("Update_Post_PV_Ledger", paramArray).Tables[0];
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


        //By Yaseen 12-31-2021
        public string DeleteAccounts_DAL(int Account_ID, int c_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                    new SqlParameter("@ID",Account_ID),
                    new SqlParameter("@c_ID",c_ID)


                };


           

                var res = dbLayer.SP_DataTable_return("Delete_Accounts", paramArray).Tables[0];
                msg = res.Rows[0][0].ToString();
                return msg;
            }

            catch (Exception ex)
            {
                string erroMsg = ex.Message;
                msg = erroMsg;
                return msg;
            }
        }
        //By Yaseen 12-31-2021


        public Pa_Ledger_DAL Get_LedgerList_DAL(int AccountID ,int c_ID)
        {


            string sp = "pa_GETAcountByID_Ledger";
            Pa_Ledger_DAL item = new Pa_Ledger_DAL();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //these are the parameters
               
                    cmd.Parameters.AddWithValue("@AccountID", AccountID);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                      

                        //Convert.ToInt32(rdr["PurchaseDetails_ID"]);
                        item.Date = rdr["Date"].ToString();
                        item.Debit = rdr["Debit"].ToString();
                        item.Credit = rdr["Credit"].ToString();
                        item.Description = rdr["Description"].ToString();
                        item.trans_ref = rdr["trans_ref"].ToString();
                        item.ACCOUNT = rdr["ACCOUNT"].ToString();
                        item.Account_ID = Convert.ToInt32(rdr["Account_ID"].ToString());
                        item.trans_ref_ID = rdr["trans_ref_ID"].ToString();
                        item.Link = rdr["Link"].ToString();

                      

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

        public string Update_Post_PDC_DAL(string date, int Post_Master_ID, int Post_user_ID, string Post_vType,int C_ID)
        {
            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@Master_ID",Post_Master_ID),
                new SqlParameter("@Date",date),
                new SqlParameter("@user_ID",Post_user_ID),
                new SqlParameter("@Type",Post_vType),
                      
                new SqlParameter("@c_ID",C_ID)


            };

                var response = dbLayer.SP_DataTable_return("POST_PDC", paramArray).Tables[0];
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

        
        public pa_ReceiptMaster_DAL ReceiptMasterObject { get; set; }
        //--getting ReceiptMaster of type vendor by id
        public pa_ReceiptMaster_DAL Get_ReceiptMasterCustomerByID_DAL(int? ReceiptMaster_ID)
        {

            string sp = "Select_ReceiptMaster_CustomerByID";

            pa_ReceiptMaster_DAL pm = new pa_ReceiptMaster_DAL();
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
                                pm.ReceiptMaster_ID = Convert.ToInt32(dr["ReceiptMaster_ID"]);
                                pm.ReceiptMaster_ref = dr["ReceiptMaster_ref"].ToString();
                                pm.Party_ID = dr["Party_ID"].ToString();
                                pm.party_type = dr["party_type"].ToString();
                                pm.NameText = dr["NameText"].ToString();
                                pm.ReceiptDate = dr["Date"].ToString();
                                pm.Explanation = dr["Explanation"].ToString();
                                pm.ReceiptStatus = dr["ReceiptStatus"].ToString();

                                pm.Created_at = dr["create_at"].ToString();
                                pm.Currency_ID = dr["create_by"].ToString();
                                pm.Modified_at = dr["last_updated_at"].ToString();
                                pm.Modified_by = dr["last_updated_by"].ToString();

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


        public string Insert_pa_trans_ref_Receipt_DAL(int? Created_by, string htemp_ID, string Trans_Ref, string Total_Amount, string Currency_Rate)
        {
            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {
                    new SqlParameter("@Created_by",Created_by),
                    new SqlParameter("@htemp_ID",htemp_ID),
                     new SqlParameter("@Trans_Ref",Trans_Ref),
                     new SqlParameter("@Total_Amount_Other",Total_Amount),
                     new SqlParameter("@Currency_Rate",Currency_Rate)

            };


            var response = dbLayer.SP_DataTable_return("pa_Insert_pa_Trans_ref_Receipt", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }


    }
}
