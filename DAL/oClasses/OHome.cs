using DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.oClasses
{
    public class OHome : IOHome
    {

        private readonly IConnectionStringManager _conMgr;
        private readonly ISqlHelper dbLayer;
        SqlConnection con;

        public OHome(IConnectionStringManager conMgr, ISqlHelper sqlHelp)
        {

            _conMgr = conMgr;
            dbLayer = sqlHelp;
            //---Getting Connection String
            Constr = _conMgr.GetConnectionString();
        }
        public string Constr { get; set; }

        public IEnumerable<DashboardStats> DashboardListObject { get; set; }

        public IEnumerable<DashboardStats> AllBankBalanceListObject { get; set; }



        //following method is for checking if user exist
        public bool Check_If_User_Exist_DAL(string Email, String password)
        {
            bool result = false;
            string sp = "Pa_Select_Users_EmailPassword";



            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@password", password);
                    SqlDataReader record = cmd.ExecuteReader();


                    if (record.Read())
                    {
                        result = true;
                    }

                    con.Close();

                }



            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                result = false;
            }

            return result;

        }

        public bool Change_User_Password(int AdminUser_ID, string password)
        {
            bool result = false;
            string sp = "pa_Change_user_password";



            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AdminUser_ID", AdminUser_ID);
                    cmd.Parameters.AddWithValue("@Password", password);
                    SqlDataReader record = cmd.ExecuteReader();


                    if (record.Read())
                    {
                        result = true;
                    }

                    con.Close();

                }



            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                result = false;
            }

            return result;
        }


        //following method is for getting user id and user name for session after login
        public Pa_AdminUsers_DAL Get_Users_Info_DAL(string Email, String password)
        {

            string sp = "Pa_Select_AdminUser_Info";

            Pa_AdminUsers_DAL User = new Pa_AdminUsers_DAL();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@password", password);
                    SqlDataReader record = cmd.ExecuteReader();


                    while (record.Read())
                    {
                        User.AdminUser_ID = Convert.ToInt32(record["AdminUser_ID"]);
                        User.UserName = record["UserName"].ToString();
                        User.c_ID = Convert.ToInt32(record["c_ID"]);
                        User.Role_id = Convert.ToInt32(record["Role_ID"]);
                    }

                    con.Close();

                }



            }
            catch
            {

            }

            return User;

        }


        public IEnumerable<DashboardStats> Get_Inv_LAST_7DAYS_SALE(int c_ID)
        {


            string sp = "LAST_7DAYS_SALE_TRD";

            List<DashboardStats> itemlist = new List<DashboardStats>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_id", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        DashboardStats item = new DashboardStats();


                        item.DAYSS = rdr["DAYSS"].ToString();
                        item.AMT = rdr["AMT"].ToString();


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


        public IEnumerable<DashboardStats> get_All_bank_balance(int c_ID)
        {


            string sp = "get_All_bank_balance";

            List<DashboardStats> itemlist = new List<DashboardStats>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_id", c_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        DashboardStats item = new DashboardStats();


                        item.BankBalance = rdr["Balance"].ToString();
                        item.BankName = rdr["AccountName"].ToString();


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


        public DashboardStats Get_Dashboard_Stats(int c_ID)
        {

            string sp = "get_DashBoard";



            DashboardStats Dashboard = new DashboardStats();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader record = cmd.ExecuteReader();


                    while (record.Read())
                    {
                        Dashboard.Today_Sale = record["Today_Sale"].ToString();
                        Dashboard.Total_Payables = record["Total_Payables"].ToString();
                        Dashboard.Total_Recievables = record["Total_Recievables"].ToString();
                        Dashboard.UnSold_Count = record["UnSold_Count"].ToString();
                        Dashboard.UnSold_Value = record["UnSold_Value"].ToString();
                        Dashboard.Stock_Sold_Count = record["Stock_Sold_Count"].ToString();
                        Dashboard.Total_Cash= record["Total_Cash"].ToString();
                        Dashboard.All_Bank_Total = record["AllBankTotal"].ToString();
                        Dashboard.TotalAssets = record["TotalAssets"].ToString();
                


                    }

                    con.Close();

                }



            }
            catch
            {

            }

            return Dashboard;

        }

        public IEnumerable<DashboardStats> TopSaleCategory_trd()
        {
            string StartDate = "12-mar-2021";
            string EndDate = "12-mar-2021";

            string sp = "TopSaleValueItems_trd";

            List<DashboardStats> itemlist = new List<DashboardStats>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Start_Date", StartDate);
                    cmd.Parameters.AddWithValue("@End_Date", EndDate);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        DashboardStats item = new DashboardStats();


                        item.Sale = rdr["Sale"].ToString();
                        item.Item_Name = rdr["ItemName"].ToString();


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

        public DashboardStats DashboardObject { get; set; }
        public DashboardStats DashboardCurrencyObject { get; set; }
        public DashboardStats Get_Dashboard_Currency(int c_ID)
        {

            string sp = "get_DashBoard_Currency";



            DashboardStats Dashboard = new DashboardStats();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader record = cmd.ExecuteReader();


                    while (record.Read())
                    {
                        Dashboard.Avg_Currency = record["Curr_Avg"].ToString();
                        Dashboard.Total_Payments_YEN = record["TotalPayments"].ToString();
                        Dashboard.Total_Payments_AED = record["inDhs"].ToString();
                        Dashboard.Investment_Acc_Name = record["Investment_Acc_Name"].ToString();
                


                    }

                    con.Close();

                }



            }
            catch
            {

            }

            return Dashboard;

        }


    }
}
