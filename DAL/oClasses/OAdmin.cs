using DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.oClasses
{
    public class OAdmin : IOAdmin
    {

        private readonly IConnectionStringManager _conMgr;
        private readonly ISqlHelper dbLayer;

        SqlConnection con;

        public OAdmin(IConnectionStringManager conMgr, ISqlHelper sqlHelp)
        {

            _conMgr = conMgr;
            //---Getting Connection String
            dbLayer = sqlHelp;
            Constr = _conMgr.GetConnectionString();
        }


        //---This variable is for saving connection string.
        public string Constr { get; set; }

        public IEnumerable<Pa_AdminUsers_DAL> AdminUsersList { get; set; }
        public IEnumerable<Roles_Categories> roleCategorieslist { get; set; }
        public IEnumerable<AdminRights> adminrights { get; set; }
        public IEnumerable<AdminRoles> adminroles { get; set; }
        public IEnumerable<AppSettings_DAL> AppSettingList { get; set; }

        public RolesMenus rolesmenus { get; set; }
        //method for getting admin roles against AdminUser_ID
        public IEnumerable<AdminRights> Get_AdminRightsByAdminUser_ID_DAL(int? RoleCategory_ID)
        {
            string sp = "pa_select_AdminRightsByAdminID";
            List<AdminRights> adminroles = new List<AdminRights>();
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RoleCategory_ID", RoleCategory_ID);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AdminRights item = new AdminRights();

                        item.ID = Convert.ToInt32(rdr["Id"]);
                        if (!string.IsNullOrEmpty(rdr["MView"].ToString())) { item.View = Convert.ToBoolean(rdr["MView"].ToString()); }
                        else { item.View = false; }
                        if (!string.IsNullOrEmpty(rdr["MAdd"].ToString())) { item.Add = Convert.ToBoolean(rdr["MAdd"].ToString()); }
                        else { item.Add = false; }
                        if (!string.IsNullOrEmpty(rdr["MEdit"].ToString())) { item.Edit = Convert.ToBoolean(rdr["MEdit"].ToString()); }
                        else { item.Edit = false; }
                        if (!string.IsNullOrEmpty(rdr["MDelete"].ToString())) { item.Delete = Convert.ToBoolean(rdr["MDelete"].ToString()); }
                        else { item.Delete = false; }
                        if (!string.IsNullOrEmpty(rdr["MPrint"].ToString())) { item.Print = Convert.ToBoolean(rdr["MPrint"].ToString()); }
                        else { item.Print = false; }
                        if (!string.IsNullOrEmpty(rdr["ExportToExcel"].ToString())) { item.Excel = Convert.ToBoolean(rdr["ExportToExcel"].ToString()); }
                        else { item.Excel = false; }
                        if (!string.IsNullOrEmpty(rdr["IsAdmin"].ToString())) { item.IsAdmin = Convert.ToBoolean(rdr["IsAdmin"].ToString()); }
                        else { item.IsAdmin = false; }
                        item.RoleName = rdr["RoleName"].ToString();
                        item.Menu = rdr["MainMenuName"].ToString();

                        adminroles.Add(item);

                    }
                    con.Close();
                }

                return adminroles;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }




        //Deleting AdminRole
        public string Delete_AdminRoleByID_DAL(int? RoleIDDelete)
        {

            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {

                    new SqlParameter("@Role_ID",RoleIDDelete),


            };


            var response = dbLayer.SP_DataTable_return("pa_Delete_AdminRole_ByID", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }



        //method for adding role to a specific user
        public string Insert_AdminRoleForSepcificUser_DAL(int MenuID, int? Role_ID, string View, string Add,
            string Edit, string Delete, string Print, string Excel, string IsAdmin, string MenuType)
        {

            string msg = "";
            bool mView;
            if (View == "on")
            {
                mView = true;
            }
            else
            {
                mView = false;
            }

            bool mAdd;
            if (Add == "on")
            {
                mAdd = true;
            }
            else
            {
                mAdd = false;
            }

            bool mEdit;
            if (Edit == "on")
            {
                mEdit = true;
            }
            else
            {
                mEdit = false;
            }

            bool mDelete;
            if (Delete == "on")
            {
                mDelete = true;
            }
            else
            {
                mDelete = false;
            }

            bool mPrint;
            if (Print == "on")
            {
                mPrint = true;
            }
            else
            {
                mPrint = false;
            }

            bool mExcel;
            if (Excel == "on")
            {
                mExcel = true;
            }
            else
            {
                mExcel = false;
            }

            bool mIsAdmin;
            if (IsAdmin == "on")
            {
                mIsAdmin = true;
            }
            else
            {
                mIsAdmin = false;
            }
            SqlParameter[] paramArray = new SqlParameter[] {

                    new SqlParameter("@MenuID",MenuID),
                      new SqlParameter("@Role_ID",Role_ID),
                      new SqlParameter("@View",mView),
                      new SqlParameter("@Add",mAdd),
                      new SqlParameter("@Edit",mEdit),
                      new SqlParameter("@Delete",mDelete),
                      new SqlParameter("@Print",mPrint),
                      new SqlParameter("@Excel",mExcel),
                      new SqlParameter("@MenuType",MenuType),
                      new SqlParameter("@IsAdmin",mIsAdmin)


            };


            var response = dbLayer.SP_DataTable_return("pa_Insert_AdminRights_ByAdminRole_ID", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }

        public string Update_AdminRoleForSepcificUser_DAL(int ID, string View, string Add,
         string Edit, string Delete, string Print, string Excel, string IsAdmin)
        {

            string msg = "";
            bool mView;
            if (View == "on")
            {
                mView = true;
            }
            else
            {
                mView = false;
            }

            bool mAdd;
            if (Add == "on")
            {
                mAdd = true;
            }
            else
            {
                mAdd = false;
            }

            bool mEdit;
            if (Edit == "on")
            {
                mEdit = true;
            }
            else
            {
                mEdit = false;
            }

            bool mDelete;
            if (Delete == "on")
            {
                mDelete = true;
            }
            else
            {
                mDelete = false;
            }

            bool mPrint;
            if (Print == "on")
            {
                mPrint = true;
            }
            else
            {
                mPrint = false;
            }

            bool mExcel;
            if (Excel == "on")
            {
                mExcel = true;
            }
            else
            {
                mExcel = false;
            }

            bool mIsAdmin;
            if (IsAdmin == "on")
            {
                mIsAdmin = true;
            }
            else
            {
                mIsAdmin = false;
            }
            SqlParameter[] paramArray = new SqlParameter[] {

                    new SqlParameter("@ID",ID),

                      new SqlParameter("@View",mView),
                      new SqlParameter("@Add",mAdd),
                      new SqlParameter("@Edit",mEdit),
                      new SqlParameter("@Delete",mDelete),
                      new SqlParameter("@Print",mPrint),
                      new SqlParameter("@Excel",mExcel),

                      new SqlParameter("@IsAdmin",mIsAdmin)


            };


            var response = dbLayer.SP_DataTable_return("pa_Update_AdminRights", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }


        //method for getting list of Roles_Categories
        public IEnumerable<AdminRights> GetMenuList_DAL(int MenuType)
        {

            string sp = "pa_select_Menu_List";
            List<AdminRights> Rolescategorieslist = new List<AdminRights>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MenuType", MenuType);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        AdminRights item = new AdminRights();

                        item.MenuID = Convert.ToInt32(rdr["MenuID"]);
                        item.Menu = rdr["Menu"].ToString();


                        Rolescategorieslist.Add(item);

                    }

                    con.Close();

                }

                return Rolescategorieslist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }



        //method for adding a role to Roles_Category table
        public string Insert_Roles_Category_DAL(string Role_Name)
        {

            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {

                    new SqlParameter("@Role_Name",Role_Name)



            };


            var response = dbLayer.SP_DataTable_return("pa_Insert_Roles_Category", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }



        //Deleting Roles category
        public string Delete_RolesCategory_DAL(int? RoleIDDelete)
        {

            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {

                    new SqlParameter("@RoleCategory_ID",RoleIDDelete),


            };


            var response = dbLayer.SP_DataTable_return("pa_Delete_Roles_Category_ByID", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }




        //method for inserting to different menus
        public string Insert_Roles_Menus_DAL(string RoleCategory_ID, bool Menu_DashBoard_View, bool Menu_Stock_View, bool Menu_Sale_View, bool Menu_Purchases_View, bool Menu_Delivery_View,
               bool Menu_Reports_View, bool Menu_Accounts_View, bool Menu_BasicData_View, bool Menu_Admin_View, bool Menu_HR_View)
        {

            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {

                  new SqlParameter("@RoleCategory_ID",RoleCategory_ID),

                    new SqlParameter("@Menu_DashBoard_View",Menu_DashBoard_View),
                      new SqlParameter("@Menu_Stock_View",Menu_Stock_View),
                        new SqlParameter("@Menu_Sale_View",Menu_Sale_View),
                          new SqlParameter("@Menu_Purchases_View",Menu_Purchases_View),
                            new SqlParameter("@Menu_Delivery_View",Menu_Delivery_View),
                              new SqlParameter("@Menu_Reports_View",Menu_Reports_View),
                                new SqlParameter("@Menu_Accounts_View",Menu_Accounts_View),
                                new SqlParameter("@Menu_BasicData_View",Menu_BasicData_View),
                                new SqlParameter("@Menu_Admin_View",Menu_Admin_View),
                                 new SqlParameter("@Menu_HR_View",Menu_HR_View)


            };


            var response = dbLayer.SP_DataTable_return("pa_Insert_RolesMenus", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }





        //method for inserting to different forms buttons
        public string Insert_Roles_Forms_DAL(string RoleCategory_ID, bool DashBoard_Add, bool DashBoard_Update, bool DashBoard_Delete, bool Stock_Add, bool Stock_Update, bool Stock_Delete, bool Purchase_Add, bool Purchase_Update, bool Purchase_Delete,
  bool Vanning_Add, bool Vanning_Uddate, bool Vanning_Delete, bool Shipping_Add, bool Shipping_Update, bool Shipping_Delete, bool Rekso_Add, bool Rekso_Update, bool Rekso_Delete, bool Papers_Add, bool Papers_Update, bool Papers_Delete, bool Sale_Add, bool Sale_Update,
  bool Sale_Delete, bool Payments_Add, bool Payments_Update, bool Payments_Delete, bool Receipts_Add, bool Receipts_Update, bool Receipts_Delete, bool Reports_Add, bool Reports_Update, bool Reports_Delete, bool BasicData_Add, bool BasicData_Update, bool BasicData_Delete, bool Admin_Add, bool Admin_Update, bool Admin_Delete)
        {

            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {
                  new SqlParameter("@RoleCategory_ID",RoleCategory_ID),


            new SqlParameter("@DashBoard_Add",DashBoard_Add),
            new SqlParameter("@DashBoard_Update",DashBoard_Update),
            new SqlParameter("@DashBoard_Delete",DashBoard_Delete),

            new SqlParameter("@Stock_Add",Stock_Add),
            new SqlParameter("@Stock_Update",Stock_Update),
            new SqlParameter("@Stock_Delete",RoleCategory_ID),

            new SqlParameter("@Purchase_Add",Purchase_Add),
            new SqlParameter("@Purchase_Update",Purchase_Update),
            new SqlParameter("@Purchase_Delete",Purchase_Delete),

            new SqlParameter("@Vanning_Add",Vanning_Add),
            new SqlParameter("@Vanning_Uddate",Vanning_Uddate),
            new SqlParameter("@Vanning_Delete",Vanning_Delete),

            new SqlParameter("@Shipping_Add",Shipping_Add),
            new SqlParameter("@Shipping_Update",Shipping_Update),
            new SqlParameter("@Shipping_Delete",Shipping_Delete),

            new SqlParameter("@Rekso_Add",Rekso_Add),
            new SqlParameter("@Rekso_Update",Rekso_Update),
            new SqlParameter("@Rekso_Delete",Rekso_Delete),

            new SqlParameter("@Papers_Add",Papers_Add),
            new SqlParameter("@Papers_Update",Papers_Update),
            new SqlParameter("@Papers_Delete",Papers_Delete),

            new SqlParameter("@Sale_Add",Sale_Add),
            new SqlParameter("@Sale_Update",Sale_Update),
            new SqlParameter("@Sale_Delete",Sale_Delete),

            new SqlParameter("@Payments_Add",Payments_Add),
            new SqlParameter("@Payments_Update",Payments_Update),
            new SqlParameter("@Payments_Delete",Payments_Delete),

            new SqlParameter("@Receipts_Add",Receipts_Add),
            new SqlParameter("@Receipts_Update",Receipts_Update),
            new SqlParameter("@Receipts_Delete",Receipts_Delete),

            new SqlParameter("@Reports_Add",Reports_Add),
            new SqlParameter("@Reports_Update",Reports_Update),
            new SqlParameter("@Reports_Delete",Reports_Delete),

            new SqlParameter("@BasicData_Add",BasicData_Add),
            new SqlParameter("@BasicData_Update",BasicData_Update),
            new SqlParameter("@BasicData_Delete",BasicData_Delete),

            new SqlParameter("@Admin_Add",Admin_Add),
            new SqlParameter("@Admin_Update",Admin_Update),
            new SqlParameter("@Admin_Delete",Admin_Delete)



            };


            var response = dbLayer.SP_DataTable_return("pa_Insert_RolesForms", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }




        //following method is for getting all roles by RolesCategoryID from RolesMenu and RolesForms tables
        public RolesMenus Get_RolesMenus_ByID_DAL(int? RoleCategory_ID)
        {

            string sp = "pa_select_RolesMenu_ByID";

            RolesMenus info = new RolesMenus();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RoleCategory_ID", RoleCategory_ID);

                    SqlDataReader record = cmd.ExecuteReader();

                    while (record.Read())
                    {


                        info.Menu_DashBoard_View = record["Menu_DashBoard_View"].ToString();

                        info.Menu_Stock_View = record["Menu_Stock_View"].ToString();

                        info.Menu_Sale_View = record["Menu_Sale_View"].ToString();

                        info.Menu_Purchases_View = record["Menu_Purchases_View"].ToString();

                        info.Menu_Delivery_View = record["Menu_Delivery_View"].ToString();

                        info.Menu_Reports_View = record["Menu_Reports_View"].ToString();

                        info.Menu_Accounts_View = record["Menu_Accounts_View"].ToString();

                        info.Menu_BasicData_View = record["Menu_BasicData_View"].ToString();

                        info.Menu_Admin_View = record["Menu_Admin_View"].ToString();
                        info.Menu_HR_View = record["Menu_HR_View"].ToString();

                        info.DashBoard_Add = record["DashBoard_Add"].ToString();
                        info.DashBoard_Update = record["DashBoard_Update"].ToString();
                        info.DashBoard_Delete = record["DashBoard_Delete"].ToString();

                        info.Stock_Add = record["Stock_Add"].ToString();
                        info.Stock_Update = record["Stock_Update"].ToString();
                        info.Stock_Delete = record["Stock_Delete"].ToString();

                        info.Purchase_Add = record["Purchase_Add"].ToString();
                        info.Purchase_Update = record["Purchase_Update"].ToString();
                        info.Purchase_Delete = record["Purchase_Delete"].ToString();

                        info.Vanning_Add = record["Vanning_Add"].ToString();
                        info.Vanning_Uddate = record["Vanning_Uddate"].ToString();
                        info.Vanning_Delete = record["Vanning_Delete"].ToString();

                        info.Shipping_Add = record["Shipping_Add"].ToString();
                        info.Shipping_Update = record["Shipping_Update"].ToString();
                        info.Shipping_Delete = record["Shipping_Delete"].ToString();

                        info.Rekso_Add = record["Rekso_Add"].ToString();
                        info.Rekso_Update = record["Rekso_Update"].ToString();
                        info.Rekso_Delete = record["Rekso_Delete"].ToString();

                        info.Papers_Add = record["Papers_Add"].ToString();
                        info.Papers_Update = record["Papers_Update"].ToString();
                        info.Papers_Delete = record["Papers_Delete"].ToString();

                        info.Sale_Add = record["Sale_Add"].ToString();
                        info.Sale_Update = record["Sale_Update"].ToString();
                        info.Sale_Delete = record["Sale_Delete"].ToString();

                        info.Payments_Add = record["Payments_Add"].ToString();
                        info.Payments_Update = record["Payments_Update"].ToString();
                        info.Payments_Delete = record["Payments_Delete"].ToString();

                        info.Receipts_Add = record["Receipts_Add"].ToString();
                        info.Receipts_Update = record["Receipts_Update"].ToString();
                        info.Receipts_Delete = record["Receipts_Delete"].ToString();

                        info.Reports_Add = record["Reports_Add"].ToString();
                        info.Reports_Update = record["Reports_Update"].ToString();
                        info.Reports_Delete = record["Reports_Delete"].ToString();

                        info.BasicData_Add = record["BasicData_Add"].ToString();
                        info.BasicData_Update = record["BasicData_Update"].ToString();
                        info.BasicData_Delete = record["BasicData_Delete"].ToString();

                        info.Admin_Add = record["Admin_Add"].ToString();
                        info.Admin_Update = record["Admin_Update"].ToString();
                        info.Admin_Delete = record["Admin_Delete"].ToString();



                    }

                    con.Close();

                }



            }
            catch
            {

            }

            return info;

        }



        //following method is for getting all roles from AdminRoles table by Admin UserID
        public RolesMenus Get_AdminRolesByAdminUserID_DAL(int? User_ID)
        {

            string sp = "pa_select_AdminRoles_ByAdminID";

            RolesMenus info = new RolesMenus();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@User_ID", User_ID);

                    SqlDataReader record = cmd.ExecuteReader();


                    while (record.Read())
                    {

                        info.Menu_DashBoard_View = record["Menu_DashBoard_View"].ToString();

                        info.Menu_Stock_View = record["Menu_Stock_View"].ToString();

                        info.Menu_Sale_View = record["Menu_Sale_View"].ToString();

                        info.Menu_Purchases_View = record["Menu_Purchases_View"].ToString();

                        info.Menu_Delivery_View = record["Menu_Delivery_View"].ToString();

                        info.Menu_Reports_View = record["Menu_Reports_View"].ToString();

                        info.Menu_Accounts_View = record["Menu_Accounts_View"].ToString();

                        info.Menu_BasicData_View = record["Menu_BasicData_View"].ToString();

                        info.Menu_Admin_View = record["Menu_Admin_View"].ToString();
                        info.Menu_HR_View = record["Menu_HR_View"].ToString();

                        info.DashBoard_Add = record["DashBoard_Add"].ToString();
                        info.DashBoard_Update = record["DashBoard_Update"].ToString();
                        info.DashBoard_Delete = record["DashBoard_Delete"].ToString();

                        info.Stock_Add = record["Stock_Add"].ToString();
                        info.Stock_Update = record["Stock_Update"].ToString();
                        info.Stock_Delete = record["Stock_Delete"].ToString();

                        info.Purchase_Add = record["Purchase_Add"].ToString();
                        info.Purchase_Update = record["Purchase_Update"].ToString();
                        info.Purchase_Delete = record["Purchase_Delete"].ToString();

                        info.Vanning_Add = record["Vanning_Add"].ToString();
                        info.Vanning_Uddate = record["Vanning_Uddate"].ToString();
                        info.Vanning_Delete = record["Vanning_Delete"].ToString();

                        info.Shipping_Add = record["Shipping_Add"].ToString();
                        info.Shipping_Update = record["Shipping_Update"].ToString();
                        info.Shipping_Delete = record["Shipping_Delete"].ToString();

                        info.Rekso_Add = record["Rekso_Add"].ToString();
                        info.Rekso_Update = record["Rekso_Update"].ToString();
                        info.Rekso_Delete = record["Rekso_Delete"].ToString();

                        info.Papers_Add = record["Papers_Add"].ToString();
                        info.Papers_Update = record["Papers_Update"].ToString();
                        info.Papers_Delete = record["Papers_Delete"].ToString();

                        info.Sale_Add = record["Sale_Add"].ToString();
                        info.Sale_Update = record["Sale_Update"].ToString();
                        info.Sale_Delete = record["Sale_Delete"].ToString();

                        info.Payments_Add = record["Payments_Add"].ToString();
                        info.Payments_Update = record["Payments_Update"].ToString();
                        info.Payments_Delete = record["Payments_Delete"].ToString();

                        info.Receipts_Add = record["Receipts_Add"].ToString();
                        info.Receipts_Update = record["Receipts_Update"].ToString();
                        info.Receipts_Delete = record["Receipts_Delete"].ToString();

                        info.Reports_Add = record["Reports_Add"].ToString();
                        info.Reports_Update = record["Reports_Update"].ToString();
                        info.Reports_Delete = record["Reports_Delete"].ToString();

                        info.BasicData_Add = record["BasicData_Add"].ToString();
                        info.BasicData_Update = record["BasicData_Update"].ToString();
                        info.BasicData_Delete = record["BasicData_Delete"].ToString();

                        info.Admin_Add = record["Admin_Add"].ToString();
                        info.Admin_Update = record["Admin_Update"].ToString();
                        info.Admin_Delete = record["Admin_Delete"].ToString();

                    }

                    con.Close();

                }



            }
            catch
            {

            }

            return info;

        }



        //Method for getting AdminUsers from database from AdminUsers table
        public IEnumerable<Pa_AdminUsers_DAL> Get_AdminUsers_List_DAL(int c_ID)
        {
            string sp = "pa_select_AdminUsers_List";
            List<Pa_AdminUsers_DAL> AdminUsersList = new List<Pa_AdminUsers_DAL>();

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
                        Pa_AdminUsers_DAL item = new Pa_AdminUsers_DAL();

                        item.ID = Convert.ToInt32(rdr["ID"]);
                        item.AdminUser_ID = Convert.ToInt32(rdr["AdminUser_ID"]);
                        item.UserName = rdr["username"].ToString();
                        item.Full_Name = rdr["Full_Name"].ToString();

                        item.Designation = rdr["Designation"].ToString();


                        item.Email = rdr["Email"].ToString();
                        item.Mobile = rdr["Mobile"].ToString();
                        item.Office_number = rdr["Office_number"].ToString();
                        item.ImageName = rdr["imgname"].ToString();
                        item.c_ID = Convert.ToInt32(rdr["c_ID"]);

                        AdminUsersList.Add(item);

                    }

                    con.Close();

                }

                return AdminUsersList;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }
        }




        //method for inserting new admin user in AdminUsersTable table in database
        public string Insert_AdminUser_DAL(string UserName, string FullName, string Designation, string Email, string User_Mobile_No, string User_OfficeNo, string password, int? c_ID, string newFileName, string filepath)
        {

            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {

                     new SqlParameter("@UserName",UserName),

                    new SqlParameter("@FullName",FullName),

                      new SqlParameter("@Designation",Designation),
                       new SqlParameter("@Email",Email),
                        new SqlParameter("@User_Mobile_No",User_Mobile_No),
                        new SqlParameter("@User_OfficeNo",User_OfficeNo),
                        new SqlParameter("@password",password),
                        new SqlParameter("@c_ID",c_ID),

              new SqlParameter("@imagepath",filepath),
                new SqlParameter("@imgname",newFileName),







            };


            var response = dbLayer.SP_DataTable_return("pa_Insert_AdminUsers", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }



        //this method will check if Email already exists in Adminusers table. if email exists then will ask the user "Email id already exists" 
        public bool Get_AdminUsers_Email_DAL(string Email)
        {

            bool result = false;
            string sp = "pa_select_AdminUsers_Email";


            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", Email);
                    SqlDataReader record = cmd.ExecuteReader();


                    if (record.Read())
                    {
                        result = true;
                    }

                    con.Close();

                }



            }
            catch
            {
                result = false;
            }

            return result;
        }



        //this method will check if user name already exists in AdminUserrs table.
        public bool Get_AdminUsers_UserName_DAL(string UserName)
        {
            //List<Members> RD = new List<Members>();
            bool result = false;
            string sp = "pa_select_Password_UserName";
            //pa_ReceiptDetails o = new pa_ReceiptDetails();
            //string sp = "";


            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    SqlDataReader record = cmd.ExecuteReader();


                    if (record.Read())
                    {
                        result = true;
                    }

                    con.Close();

                }



            }
            catch
            {
                result = false;
            }

            return result;
        }



        //this method will check if Email already exists in Admin Users table except that email
        public bool Get_AdminUsers_Email_ForUpdate_DAL(int? AdminUser_IDUdpate, string Email)
        {

            bool result = false;
            string sp = "pa_select_AdminUsers_EmailForUpdate";


            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AdminUser_ID", AdminUser_IDUdpate);
                    cmd.Parameters.AddWithValue("@Email", Email);


                    SqlDataReader record = cmd.ExecuteReader();


                    if (record.Read())
                    {
                        result = true;
                    }

                    con.Close();

                }



            }
            catch
            {
                result = false;
            }

            return result;
        }



        //method for updating new admin user in Password table in database
        public string Update_AdminUsers_DAL(int? AdminUser_IDUdpate, string EmailUpdate, string User_Mobile_NoUpdate, string User_OfficeNoUpdate, string FullNameUpdate, string DesignationUpdate, int? c_IDUpdate, string newFileName, string filepath)
        {

            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {

                     new SqlParameter("@AdminUser_ID",AdminUser_IDUdpate),
                    new SqlParameter("@EmailUpdate",EmailUpdate),
                     new SqlParameter("@User_Mobile_NoUpdate",User_Mobile_NoUpdate),
                      new SqlParameter("@User_OfficeNoUpdate",User_OfficeNoUpdate),
                                      new SqlParameter("@imagepath",filepath),
                new SqlParameter("@imgname",newFileName),
                        new SqlParameter("@FullNameUpdate",FullNameUpdate),
                        new SqlParameter("@DesignationUpdate",DesignationUpdate),
                       
                        new SqlParameter("@c_IDUpdate",c_IDUpdate),








            };


            var response = dbLayer.SP_DataTable_return("pa_Update_AdminUsers", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }




        public string Delete_AdminUsers_DAL(int? id)
        {

            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {

                    new SqlParameter("@id",id),








            };


            var response = dbLayer.SP_DataTable_return("pa_delete_AdminUsers", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }




        public IEnumerable<AppSettings_DAL> GetAppSettingsList_DAL()
        {

            string sp = "Select_AppSettingList";
            List<AppSettings_DAL> Rolescategorieslist = new List<AppSettings_DAL>();

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
                        AppSettings_DAL item = new AppSettings_DAL();

                        item.ID = Convert.ToInt32(rdr["ID"]);
                        item.NAME = rdr["NAME"].ToString();
                        item.VALUE = rdr["VALUE"].ToString();

                        Rolescategorieslist.Add(item);

                    }

                    con.Close();

                }

                return Rolescategorieslist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }



        public string UpdateGlobalSetting_DAL(int? IDUpdate, string VALUEUpdate)
        {

            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {

                    new SqlParameter("@IDUpdate",IDUpdate),
                     new SqlParameter("@VALUEUpdate",VALUEUpdate)



            };


            var response = dbLayer.SP_DataTable_return("Update_AppSetting", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }

        public string Delete_AdminRights_DAL(int? id)
        {

            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {

                    new SqlParameter("@id",id),








            };


            var response = dbLayer.SP_DataTable_return("pa_delete_AdminRights", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }
        public IEnumerable<Roles_Categories> GetRolesCategoryList_DAL()
        {

            string sp = "pa_select_RolesCategory_List";
            List<Roles_Categories> Rolescategorieslist = new List<Roles_Categories>();

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
                        Roles_Categories item = new Roles_Categories();

                        item.RoleCategory_ID = Convert.ToInt32(rdr["RoleCategory_ID"]);
                        item.Role_Name = rdr["Role_Name"].ToString();
                        item.DateCreate = rdr["DateCreate"].ToString();

                        Rolescategorieslist.Add(item);

                    }

                    con.Close();

                }

                return Rolescategorieslist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        public IEnumerable<AdminRoles> Get_AdminRolesByAdminUser_ID_DAL(int? AdminUser_ID)
        {
            string sp = "pa_select_AdminRolesByAdminID";
            List<AdminRoles> adminroles = new List<AdminRoles>();
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AdminUser_ID", AdminUser_ID);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AdminRoles item = new AdminRoles();

                        item.AdminUser_ID = Convert.ToInt32(rdr["AdminUser_ID"]);
                        item.Admin_Role_ID = Convert.ToInt32(rdr["Admin_Role_ID"]);
                        item.Role_Name = rdr["Role_Name"].ToString();
                        item.RoleCategory_ID = Convert.ToInt32(rdr["RoleCategory_ID"]);
                        item.AdminUserName = rdr["AdminUserName"].ToString();

                        adminroles.Add(item);

                    }
                    con.Close();
                }

                return adminroles;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        public string Insert_AdminRoleUser_DAL(string AdmRole, int? AdminUser_IDAdd)
        {

            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {

                    new SqlParameter("@RoleCategory_ID",AdmRole),
                      new SqlParameter("@AdminUser_ID",AdminUser_IDAdd)


            };


            var response = dbLayer.SP_DataTable_return("pa_Insert_AdminRoles_ByAdminUserID", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }
        public string Delete_AdminRoles_DAL(int? id)
        {

            string msg = "";

            SqlParameter[] paramArray = new SqlParameter[] {

                    new SqlParameter("@id",id),








            };


            var response = dbLayer.SP_DataTable_return("pa_delete_AdminRoles", paramArray).Tables[0];
            msg = response.Rows[0][0].ToString();

            return msg;
        }
    }
}
