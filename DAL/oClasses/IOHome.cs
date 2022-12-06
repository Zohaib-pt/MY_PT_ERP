
using DAL.Models;
using System;
using System.Collections.Generic;

namespace DAL.oClasses
{
    public interface IOHome
    {
        public DashboardStats DashboardObject { get; set; }
        public DashboardStats DashboardCurrencyObject { get; set; }
        public DashboardStats Get_Dashboard_Currency(int c_ID);
        public IEnumerable<DashboardStats> DashboardListObject { get; set; }
        public bool Check_If_User_Exist_DAL(string Email, String password);
        public Pa_AdminUsers_DAL Get_Users_Info_DAL(string Email, String password);
        public bool Change_User_Password(int AdminUser_ID, string password);
        public DashboardStats Get_Dashboard_Stats(int c_ID);

        public IEnumerable<DashboardStats> AllBankBalanceListObject { get; set; }

        public IEnumerable<DashboardStats> Get_Inv_LAST_7DAYS_SALE(int c_ID);

        public IEnumerable<DashboardStats> TopSaleCategory_trd();


        public IEnumerable<DashboardStats> get_All_bank_balance(int c_ID);




    }
}
