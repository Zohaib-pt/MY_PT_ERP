using DAL.Models;
using System.Collections.Generic;

namespace DAL.oClasses
{
    public interface IOHR
    {
        public IEnumerable<Pa_EmpMaster_DAL> EmpMasterList { get; set; }
        public IEnumerable<Pa_EmpDocument_DAL> EmpDocumentList { get; set; }
        public Pa_EmpMaster_DAL EmpMasterObj { get; set; }
        public IEnumerable<Pa_EmpMaster_DAL> Get_Emp_Main_master_DAL(int c_ID);
        public IEnumerable<Pa_EmpDocument_DAL> Get_EmpDocument_DetailListByID_DAL(int? emp_ID, string temp_ID);
        public string Emp_Insert_EmpMainDetail_DAL(string emp_Name, string Birth_Date, string Joining_Date, string Nationality, bool IsActive, string Desgination, string Basic_Salary, string Accomodation, string Transport,
            string Sp_Allowance, string Total_Salary, string ProfileImageName, string ProfileImagePath, string Temp_ID, string c_ID, int? Created_by);


        public string Emp_Insert_Documents_DAL(string empDoc_Type, string empDoc_FileName, string empDoc_filePath, string empDoc_ExpiryDate, string Temp_ID, int? Created_by);
        public string GetOldTempIDFromEmpDocDetail_DAL(int? Emp_master_ID);
        public Pa_EmpMaster_DAL Get_EmployeeMasterByID_DAL(int? Emp_master_ID);
        public string Emp_Update_EmpMainDetail_DAL(int? Emp_master_ID, string emp_Name, string Birth_Date, string Joining_Date, string Nationality, bool IsActive,
       string Desgination, string Basic_Salary, string Accomodation, string Transport, string Sp_Allowance, string Total_Salary, string ProfileImageName, string ProfileImagePath, string Temp_ID, int? c_ID, int? Modified_by);

        public string DeleteEmployee_DAL(int? emp_ID);
        public string DeleteEmployee_Document_DAL(int? empDoc_ID);


        #region PAYROLL 
        public IEnumerable<AccountingPeroid_DAL> Get_AccountingPeroid_DAL(int c_ID);      
        public string InsertPayRoll_DAL(string pDate, int? Acc_Period_ID, int? c_id, int? created_by);
        public IEnumerable<PayRoll_DAL> Get_PayRollList_DAL(int? c_ID , string? start_date,string? end_date, string? Acc_Period_ID);
              
        public IEnumerable<PayRoll_DAL> PayMasterList { get; set; }

      

        public IEnumerable<PaySlipVouchar_DAL> Get_PaySlipVoucharByID_DAL(int? payRoll_Master_ID);

        public IEnumerable<PaySlipVouchar_DAL> PaySlipVoucharList { get; set; }
        public IEnumerable<PaySlipVouchar_DAL> PaySlipVoucharList_Print { get; set; }
        public string InsertIssuPaymentVouchar_DAL(int emp_PayrollDetails_ID, int? pay_Account_ID);





        #endregion
        public IEnumerable<AlertNotification_DAL> AlertNotificationList { get; set; }
        public IEnumerable<AlertNotification_DAL> Get_AlertNotificationList_DAL(int UserID);

    }
}
