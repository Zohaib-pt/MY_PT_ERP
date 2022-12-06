namespace DAL.Models
{
    public class Accounts_DAL
    {

        public int? Account_ID { get; set; }
        public string AccountName { get; set; }
        public string Account_No { get; set; }
        public string Acc_ShortName { get; set; }

        public int HeadAccounts_ID { get; set; }
        public string HeadAccount { get; set; }
        public int subHeadAccount_ID { get; set; }
        public string sub_HeadAccount { get; set; }

        public string Sys_Ac_TypeName { get; set; }
        public int Sys_AC_type_ID { get; set; }
        public string Opening_Balance { get; set; }
        public string Opening_Bal_Date { get; set; }
        public string other_refrence_ID { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Created_byName { get; set; }


        public string Modified_at { get; set; }
        public string Modified_by { get; set; }
        public string Modified_byName { get; set; }

        public string IsDeleted { get; set; }
    }


    public class sys_Ac_Type_DAL
    {
        public int sys_Ac_Type_ID { get; set; }
        public string sys_Ac_TypeName { get; set; }
    }

    public class subHeadAccounts_DAL
    {
        public int sub_headAccount_ID { get; set; }
        public string sub_HeadAccount { get; set; }
    }

    public class AccountsHead_DAL
    {
        public int ID { get; set; }
        public string HeadAccount { get; set; }
    }

}
