using System;

namespace DAL.Models
{
    public class RolesMenus
    {
        public int ID { get; set; }
        public int RolesMenu_ID { get; set; }
        public Nullable<int> RoleCategory_ID { get; set; }
        public string Menu_DashBoard_View { get; set; }
        public string Menu_Stock_View { get; set; }
        public string Menu_Sale_View { get; set; }
        public string Menu_Purchases_View { get; set; }
        public string Menu_Delivery_View { get; set; }
        public string Menu_Reports_View { get; set; }
        public string Menu_Accounts_View { get; set; }
        public string Menu_BasicData_View { get; set; }
        public string Menu_Admin_View { get; set; }

        public string Menu_HR_View { get; set; }

        public string DashBoard_Add { get; set; }
        public string DashBoard_Update { get; set; }
        public string DashBoard_Delete { get; set; }
        public string Stock_Add { get; set; }
        public string Stock_Update { get; set; }
        public string Stock_Delete { get; set; }
        public string Purchase_Add { get; set; }
        public string Purchase_Update { get; set; }
        public string Purchase_Delete { get; set; }
        public string Vanning_Add { get; set; }
        public string Vanning_Uddate { get; set; }
        public string Vanning_Delete { get; set; }
        public string Shipping_Add { get; set; }
        public string Shipping_Update { get; set; }
        public string Shipping_Delete { get; set; }
        public string Rekso_Add { get; set; }
        public string Rekso_Update { get; set; }
        public string Rekso_Delete { get; set; }
        public string Papers_Add { get; set; }
        public string Papers_Update { get; set; }
        public string Papers_Delete { get; set; }
        public string Sale_Add { get; set; }
        public string Sale_Update { get; set; }
        public string Sale_Delete { get; set; }
        public string Payments_Add { get; set; }
        public string Payments_Update { get; set; }
        public string Payments_Delete { get; set; }
        public string Receipts_Add { get; set; }
        public string Receipts_Update { get; set; }
        public string Receipts_Delete { get; set; }
        public string Reports_Add { get; set; }
        public string Reports_Update { get; set; }
        public string Reports_Delete { get; set; }
        public string BasicData_Add { get; set; }
        public string BasicData_Update { get; set; }
        public string BasicData_Delete { get; set; }
        public string Admin_Add { get; set; }
        public string Admin_Update { get; set; }
        public string Admin_Delete { get; set; }

        public Nullable<System.DateTime> Create_at { get; set; }
        public Nullable<int> Created_by { get; set; }
        public Nullable<System.DateTime> Modified_at { get; set; }
        public Nullable<int> Modified_by { get; set; }
        public Nullable<int> isDeleted { get; set; }
    }
}
