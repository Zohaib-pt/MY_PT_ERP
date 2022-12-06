namespace DAL.Models
{
    public class Deposits_DAL
    {


        public int DV_ID { get; set; }
        public string DV_Ref { get; set; }
        public int DVdetails_ID { get; set; }

        public string temp_ID { get; set; }
        public string Date_Taken { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string Customer_ID { get; set; }
        public string CUSTOMER_CONTACT { get; set; }


        public string Date_Return { get; set; }
        public string refund_Customer { get; set; }
        public string refund_Contact { get; set; }

        public string Car_Sale_Value { get; set; }
        public string Deposit { get; set; }
        public string Fix_Deductable_Charges { get; set; }
        public string PaperExpense_Changes { get; set; }
        public string VAT_Security_Deposit { get; set; }
        public string Other_Charges { get; set; }
        public string Total_Amount { get; set; }


        public string External_Trans_Ref { get; set; }
        public string Submission_Date { get; set; }
        public string ShipDate { get; set; }


        public int Stock_ID { get; set; }
        public string Chassis_No { get; set; }

        public string Others { get; set; }
        public string Total_Collected { get; set; }
        public string Amount_Recieved { get; set; }
        public string Amount_Return { get; set; }


        public int export_to_Destination_ID { get; set; }
        public int port_ID { get; set; }
        public int transaction_status { get; set; }

        public int c_id { get; set; }

        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Created_byName { get; set; }


        public string Modified_at { get; set; }
        public string Modified_by { get; set; }
        public string Modified_byName { get; set; }

        public string ModelDesciption { get; set; }
        public string Make { get; set; }
        public string ModelYear { get; set; }
        public string Color { get; set; }
        public string AfterTotal { get; set; }
        public string AMOUNT_INWORDS { get; set; }
        public string REFUND_DATE { get; set; }
        public string DEPOSIT_EXP_DATE { get; set; }
        public string CAR_SHIP_DATE { get; set; }
        public string DEDUCTED_AMOUNT { get; set; }
        public string cheque_Date { get; set; }
        public string Cheque_no { get; set; }
        public string Cheque_Bank_Name { get; set; }
        public string PAPER_MADE_DATE { get; set; }


    }



}
