using DAL.Models;
using System.Collections.Generic;

namespace DAL.oClasses
{
    public interface IOAdmin
    {
        public IEnumerable<Roles_Categories> roleCategorieslist { get; set; }
        public IEnumerable<AdminRights> adminrights { get; set; }
        public IEnumerable<AdminRoles> adminroles { get; set; }
        public RolesMenus rolesmenus { get; set; }
        public IEnumerable<AppSettings_DAL> AppSettingList { get; set; }

        public IEnumerable<Pa_AdminUsers_DAL> AdminUsersList { get; set; }
        public IEnumerable<AdminRights> Get_AdminRightsByAdminUser_ID_DAL(int? RoleCategory_ID);
        public string Delete_AdminRoleByID_DAL(int? RoleIDDelete);
        public string Insert_AdminRoleForSepcificUser_DAL(int MenuID, int? Role_ID, string View, string Add, string Edit, string Delete, string Print, string Excel, string IsAdmin, string MenuType);
        public IEnumerable<AdminRights> GetMenuList_DAL(int MenuType);
        public string Insert_Roles_Category_DAL(string Role_Name);
        public string Delete_RolesCategory_DAL(int? RoleIDDelete);
        public string Insert_Roles_Menus_DAL(string RoleCategory_ID, bool Menu_DashBoard_View, bool Menu_Stock_View, bool Menu_Sale_View, bool Menu_Purchases_View, bool Menu_Delivery_View,
         bool Menu_Reports_View, bool Menu_Accounts_View, bool Menu_BasicData_View, bool Menu_Admin_View, bool Menu_HR_View);
        public string Insert_Roles_Forms_DAL(string RoleCategory_ID, bool DashBoard_Add, bool DashBoard_Update, bool DashBoard_Delete, bool Stock_Add, bool Stock_Update, bool Stock_Delete, bool Purchase_Add, bool Purchase_Update, bool Purchase_Delete,
bool Vanning_Add, bool Vanning_Uddate, bool Vanning_Delete, bool Shipping_Add, bool Shipping_Update, bool Shipping_Delete, bool Rekso_Add, bool Rekso_Update, bool Rekso_Delete, bool Papers_Add, bool Papers_Update, bool Papers_Delete, bool Sale_Add, bool Sale_Update,
bool Sale_Delete, bool Payments_Add, bool Payments_Update, bool Payments_Delete, bool Receipts_Add, bool Receipts_Update, bool Receipts_Delete, bool Reports_Add, bool Reports_Update, bool Reports_Delete, bool BasicData_Add, bool BasicData_Update, bool BasicData_Delete, bool Admin_Add, bool Admin_Update, bool Admin_Delete);

        public RolesMenus Get_RolesMenus_ByID_DAL(int? RoleCategory_ID);
        public RolesMenus Get_AdminRolesByAdminUserID_DAL(int? User_ID);
        public IEnumerable<Pa_AdminUsers_DAL> Get_AdminUsers_List_DAL(int c_ID);
        public string Insert_AdminUser_DAL(string UserName, string FullName, string Designation, string Email, string User_Mobile_No, string User_OfficeNo, string password, int? c_ID, string newFileName, string filepath);
        public bool Get_AdminUsers_Email_DAL(string Email);
        public bool Get_AdminUsers_UserName_DAL(string UserName);
        public bool Get_AdminUsers_Email_ForUpdate_DAL(int? AdminUser_IDUdpate, string Email);
        public string Update_AdminUsers_DAL(int? AdminUser_IDUdpate, string EmailUpdate, string User_Mobile_NoUpdate, string User_OfficeNoUpdate, string FullNameUpdate, string DesignationUpdate, int? c_IDUpdate, string newFileName, string filepath);
        public string Delete_AdminUsers_DAL(int? id);
        public IEnumerable<AppSettings_DAL> GetAppSettingsList_DAL();
        public string UpdateGlobalSetting_DAL(int? IDUpdate, string VALUEUpdate);

        public string Delete_AdminRights_DAL(int? id);
        public string Update_AdminRoleForSepcificUser_DAL(int ID, string View, string Add,
       string Edit, string Delete, string Print, string Excel, string IsAdmin);
        public IEnumerable<Roles_Categories> GetRolesCategoryList_DAL();
        public IEnumerable<AdminRoles> Get_AdminRolesByAdminUser_ID_DAL(int? AdminUser_ID);
        public string Insert_AdminRoleUser_DAL(string AdmRole, int? AdminUser_IDAdd);
        public string Delete_AdminRoles_DAL(int? id);
    }
}
