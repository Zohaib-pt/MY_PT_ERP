namespace DAL.Models
{
    public class Pa_PaymentDetails_DAL
    {


        public int? PaymentDetails_ID { get; set; }
        public int? PaymentMaster_ID { get; set; }

        public int ID { get; set; }

        public string DR_accountID { get; set; }
        public string CR_accountID { get; set; }
        public string DR_AccountName { get; set; }
        public string CR_AcountName { get; set; }
        public string PaymentDate { get; set; }
        public string Currency_Rate { get; set; }
        public string Currency_ID { get; set; }
        public string Amount_Others { get; set; }
        public string Total_Amount { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }
        public string VAT_Rate { get; set; }
        public string VAT_Exp { get; set; }
        public string Party_ID { get; set; }
        public string cheque_no { get; set; }
        public string cheque_date { get; set; }
        public string cheque_bank_name { get; set; }
        public string old_amount { get; set; }
        public string stock_ID { get; set; }
        public string Chassis_No { get; set; }
        public string Temp_ID { get; set; }
        public string Paymenttype { get; set; }
        public string trans_ref { get; set; }
        public string link_trans_ref_ID { get; set; }
        public string c_ID { get; set; }
        public string Purchase_ID { get; set; }
        public string curr_name { get; set; }
        public string isDeleted { get; set; }

        //---Own fields added that not exist in database table
        public string AmountTotal { get; set; }
        public string OtherAmountTotal { get; set; }
        public string VatExpTotal { get; set; }
        public string Grand_Total { get; set; }
        public string Status { get; set; }

        //-- by Rafay Start Date : 14/01/2021
        public int TransRefPay_ID { get; set; }

        //-- by Rafay End Date : 14/01/2021

        public string ExtraChargesFee { get; set; }
        public string ExtraChargesFeeTax { get; set; }
        public string TaxAmount { get; set; }
        public string TaxAmountOthers { get; set; }

        public int transaction_status { get; set; }
        
        public string BLNO { get; set; }
        public int PDC_Payable_Acc_ID { get; set; }


    }
}
