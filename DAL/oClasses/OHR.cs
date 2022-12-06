using DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
namespace DAL.oClasses
{
    public class OHR : IOHR
    {


        private readonly IConnectionStringManager _conMgr;
        private readonly ISqlHelper dbLayer;
        SqlConnection con;

        public OHR(IConnectionStringManager conMgr, ISqlHelper sqlHelp)
        {

            _conMgr = conMgr;
            //---Getting Connection String
            dbLayer = sqlHelp;
            Constr = _conMgr.GetConnectionString();
        }


        //---This variable is for saving connection string.
        public string Constr { get; set; }


        public IEnumerable<Pa_EmpMaster_DAL> EmpMasterList { get; set; }
        public IEnumerable<Pa_EmpDocument_DAL> EmpDocumentList { get; set; }
        public Pa_EmpMaster_DAL EmpMasterObj { get; set; }

         
        public IEnumerable<PayRoll_DAL> PayMasterList { get; set; }
        public IEnumerable<PaySlipVouchar_DAL> PaySlipVoucharList { get; set; }
        public IEnumerable<PaySlipVouchar_DAL> PaySlipVoucharList_Print { get;set; }

        public IEnumerable<Pa_EmpMaster_DAL> Get_Emp_Main_master_DAL(int c_ID)
        {
            string sp = "select_Emp_Main_master";
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


                        item.emp_ID = Convert.ToInt32(rdr["emp_ID"].ToString());
                        item.emp_No = rdr["emp_No"].ToString();
                        item.emp_Name = rdr["emp_Name"].ToString();
                        item.Joining_Date = rdr["Joining_Date"].ToString();
                        item.Nationality = rdr["Nationality"].ToString();
                        item.IsActive = Convert.ToBoolean(rdr["IsActive"].ToString());

                        item.Emp_Details_ID = Convert.ToInt32(rdr["Emp_Details_ID"].ToString());
                        item.Desgination = rdr["Desgination"].ToString();
                        item.Basic_Salary = rdr["Basic_Salary"].ToString();
                        item.Accomodation = rdr["Accomodation"].ToString();
                        item.Transport = rdr["Transport"].ToString();
                        item.Sp_Allowance = rdr["Sp_Allowance"].ToString();
                        item.Total_Salary = rdr["Total_Salary"].ToString();
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
        public IEnumerable<Pa_EmpDocument_DAL> Get_EmpDocument_DetailListByID_DAL(int? emp_ID, string temp_ID)
        {



            string sp = "select_Emp_Documents_ID";
            List<Pa_EmpDocument_DAL> itemlist = new List<Pa_EmpDocument_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@emp_ID", emp_ID);
                    cmd.Parameters.AddWithValue("@temp_ID", temp_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_EmpDocument_DAL item = new Pa_EmpDocument_DAL();


                        item.emp_ID = Convert.ToInt32(rdr["emp_ID"].ToString());
                        item.empDoc_ID = Convert.ToInt32(rdr["empDoc_ID"].ToString());
                        item.empDoc_Type = rdr["empDoc_Type"].ToString();
                        item.empDoc_FileName = rdr["empDoc_FileName"].ToString();
                        item.empDoc_filePath = rdr["empDoc_filePath"].ToString();
                        item.empDoc_ExpiryDate = rdr["empDoc_ExpiryDate"].ToString();





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
        public string Emp_Insert_EmpMainDetail_DAL(string emp_Name, string Birth_Date, string Joining_Date, string Nationality, bool IsActive, string Desgination, string Basic_Salary, string Accomodation, string Transport,
            string Sp_Allowance, string Total_Salary, string ProfileImageName, string ProfileImagePath, string Temp_ID, string c_ID, int? Created_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@emp_Name",emp_Name),
                new SqlParameter("@Birth_Date" ,Birth_Date),

                new SqlParameter("@Joining_Date ",Joining_Date),
                new SqlParameter("@Nationality ",Nationality),
               new SqlParameter("@IsActive ",IsActive),
                new SqlParameter("@Desgination ",Desgination),
                new SqlParameter("@Basic_Salary ",Basic_Salary),
                new SqlParameter("@Accomodation ",Accomodation),
                new SqlParameter("@Transport ",Transport ),
                new SqlParameter("@Sp_Allowance ",Sp_Allowance),
                new SqlParameter("@Total_Salary ",Total_Salary),

                 new SqlParameter("@ProfileImageName ",ProfileImageName),
                  new SqlParameter("@ProfileImagePath ",ProfileImagePath),

                new SqlParameter("@c_ID ",c_ID),
                new SqlParameter("@Temp_ID ",Temp_ID),
                new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Emp_Insert_EmpMainDetails", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }


        public string Emp_Update_EmpMainDetail_DAL(int? Emp_master_ID, string emp_Name, string Birth_Date, string Joining_Date, string Nationality, bool IsActive,
            string Desgination, string Basic_Salary, string Accomodation, string Transport, string Sp_Allowance, string Total_Salary, string ProfileImageName, string ProfileImagePath, string Temp_ID, int? c_ID, int? Modified_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@Emp_master_ID",Emp_master_ID),
                new SqlParameter("@emp_Name",emp_Name),
                new SqlParameter("@Birth_Date" ,Birth_Date),

                new SqlParameter("@Joining_Date ",Joining_Date),
                new SqlParameter("@Nationality ",Nationality),
               new SqlParameter("@IsActive ",IsActive),
                new SqlParameter("@Desgination ",Desgination),
                new SqlParameter("@Basic_Salary ",Basic_Salary),
                new SqlParameter("@Accomodation ",Accomodation),
                new SqlParameter("@Transport ",Transport ),
                new SqlParameter("@Sp_Allowance ",Sp_Allowance),
                new SqlParameter("@Total_Salary ",Total_Salary),
                    new SqlParameter("@ProfileImageName ",ProfileImageName),
                  new SqlParameter("@ProfileImagePath ",ProfileImagePath),

                new SqlParameter("@c_ID ",c_ID),
                new SqlParameter("@Temp_ID ",Temp_ID),
                new SqlParameter("@Modified_by",Modified_by)



            };


                var response = dbLayer.SP_DataTable_return("Emp_Update_EmpMainDetails", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }





        public string Emp_Insert_Documents_DAL(string empDoc_Type, string empDoc_FileName, string empDoc_filePath, string empDoc_ExpiryDate, string Temp_ID, int? Created_by)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@empDoc_Type",empDoc_Type),
                new SqlParameter("@empDoc_FileName" ,empDoc_FileName),
                new SqlParameter("@empDoc_filePath" ,empDoc_filePath),
                new SqlParameter("@empDoc_ExpiryDate ",empDoc_ExpiryDate),
                     new SqlParameter("@Temp_ID ",Temp_ID),
                new SqlParameter("@Created_by",Created_by)



            };


                var response = dbLayer.SP_DataTable_return("Emp_Insert_Documents", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }


        // Get Old TempID From Emp_Documents by Emp_master_ID 
        public string GetOldTempIDFromEmpDocDetail_DAL(int? Emp_master_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Emp_master_ID",Emp_master_ID)

            };


                var response = dbLayer.SP_DataTable_return("Select_Emp_Documents_OldTempID", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }


        //--getting employee master by id
        public Pa_EmpMaster_DAL Get_EmployeeMasterByID_DAL(int? Emp_master_ID)
        {

            string sp = "Select_EmployeeMasterByID";

            Pa_EmpMaster_DAL pm = new Pa_EmpMaster_DAL();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Emp_master_ID", Emp_master_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.emp_ID = Convert.ToInt32(dr["emp_ID"]);
                                pm.emp_No = dr["emp_No"].ToString();
                                pm.emp_Name = dr["emp_Name"].ToString();
                                pm.Joining_Date = dr["Joining_Date"].ToString();
                                pm.Birth_Date = dr["Birth_Date"].ToString();
                                pm.Nationality = dr["Nationality"].ToString();
                                pm.Desgination = dr["Desgination"].ToString();
                                pm.IsActive = Convert.ToBoolean(dr["IsActive"]);
                                pm.Basic_Salary = dr["Basic_Salary"].ToString();
                                pm.Accomodation = dr["Accomodation"].ToString();
                                pm.Transport = dr["Transport"].ToString();
                                pm.Sp_Allowance = dr["Sp_Allowance"].ToString();
                                pm.Total_Salary = dr["Total_Salary"].ToString();

                                pm.ProfileImageName = dr["ProfileImageName"].ToString();
                                pm.ProfileImagePath = dr["ProfileImagePath"].ToString();





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

        //---Delete employee 
        public string DeleteEmployee_DAL(int? emp_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@emp_ID",emp_ID)

            };


                var response = dbLayer.SP_DataTable_return("emp_Delete_Employee", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        //---Delete employee documents
        public string DeleteEmployee_Document_DAL(int? empDoc_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@empDoc_ID",empDoc_ID)

            };


                var response = dbLayer.SP_DataTable_return("Delete_Employee_Document", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        #region PAYROLL 
        public IEnumerable<AccountingPeroid_DAL> Get_AccountingPeroid_DAL(int c_ID)
        {
            string sp = "select_AccountingPeroid_list";
            List<AccountingPeroid_DAL> itemlist = new List<AccountingPeroid_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@C_id", c_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        AccountingPeroid_DAL item = new AccountingPeroid_DAL();
                        
                        item.Accounting_Period_ID = Convert.ToInt32(rdr["Acc_Period_ID"].ToString());
                    
                        item.Period_Des = rdr["Period_Desc"].ToString();
                        
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

        public string InsertPayRoll_DAL(string pDate, int? Acc_Period_ID, int? c_id, int? created_by)
        {
            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@pDate",pDate),
                new SqlParameter("@Acc_Period_ID",Acc_Period_ID),
                new SqlParameter("@c_Id",c_id),
                 new SqlParameter("@user_ID",created_by)

            };

                var response = dbLayer.SP_DataTable_return("Insert_PR_Master", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();
                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }
        }

        public string PayRollList_DAL(int? c_id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PayRoll_DAL> Get_PayRollList_DAL(int? c_ID, string? start_date, string? end_date, string? Acc_Period_ID)
        {
          
            
            string sp = "Select_PR_master";
            List<PayRoll_DAL> itemlist = new List<PayRoll_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);
                    cmd.Parameters.AddWithValue("@start_Date", start_date);
                    cmd.Parameters.AddWithValue("@End_Date", end_date);
                    cmd.Parameters.AddWithValue("@Acc_Period_ID", Acc_Period_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        PayRoll_DAL item = new PayRoll_DAL();


                        item.PayRol_Id = Convert.ToInt32(rdr["payRoll_ID"].ToString());
                        item.pDate = rdr["pDate"].ToString();
                        item.pariod_Des = rdr["Period_Desc"].ToString();
                        item.c_CODE = rdr["C_CODE"].ToString();
                        item.User_id = Convert.ToInt32(rdr["User_ID"].ToString());
                        item.AcoountName = rdr["userid"].ToString();
                        item.AccountPeroid_id = Convert.ToInt32(rdr["Acc_Period_ID"].ToString());

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

        public IEnumerable<PaySlipVouchar_DAL> Get_PaySlipVoucharByID_DAL(int? payRoll_Master_ID)
        {
            string sp = "Select_PayRollReport";
            List<PaySlipVouchar_DAL> itemlist = new List<PaySlipVouchar_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;           
                    cmd.Parameters.AddWithValue("@payRoll_Master_ID", payRoll_Master_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        PaySlipVouchar_DAL item = new PaySlipVouchar_DAL();


                        item.emp_No = Convert.ToInt32(rdr["emp_No"].ToString());
                        item.emp_name = rdr["emp_Name"].ToString();
                        item.Period_Desc = rdr["Period_Desc"].ToString();
                        //item.Created_By = Convert.ToInt32(rdr[""].ToString());
                        item.emp_PayRollDetail_ID = Convert.ToInt32(rdr["emp_PayrollDetails_ID"].ToString()); 
                        item.Bal_Recieved = rdr["Bal_Recieved"].ToString();
                        item.Salary_Paid = rdr["Salary_Paid"].ToString();
                        
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

        public string InsertIssuPaymentVouchar_DAL(int emp_PayrollDetails_ID, int? pay_Account_ID)
        {
            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@emp_PayrollDetails_ID",emp_PayrollDetails_ID),
                new SqlParameter("@pay_Account_ID",pay_Account_ID),
             

            };

                var response = dbLayer.SP_DataTable_return("Insert_PRoll_Activity_PostSalary", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();
                return msg;

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


        public IEnumerable<AlertNotification_DAL> AlertNotificationList { get; set; }
        public IEnumerable<AlertNotification_DAL> Get_AlertNotificationList_DAL(int UserID)
        {

            string sp = "pa_Select_AlterNotiList";
            List<AlertNotification_DAL> itemlist = new List<AlertNotification_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    

                    cmd.Parameters.AddWithValue("@UserID", UserID);
            



                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AlertNotification_DAL item = new AlertNotification_DAL();
                      
                        item.type = rdr["type"].ToString();
                        item.Ref = rdr["Ref"].ToString();
                        item.Description = rdr["Description"].ToString();
                        item.Fix = rdr["Fix"].ToString();
                        item.Ledger_Amt = rdr["Ledger_Amt"].ToString();
                        item.Voucher_Amt = rdr["Voucher_Amt"].ToString();
                        item.Link = rdr["LINK"].ToString();

               


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
