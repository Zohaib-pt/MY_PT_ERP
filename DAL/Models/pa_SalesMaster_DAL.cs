namespace DAL.Models
{
    public class pa_SalesMaster_DAL
    {


        public int? SaleMaster_ID { get; set; }
        public int? SaleMasterRef_ID { get; set; }
        public string SaleRef { get; set; }
        public string ReturnRef { get; set; }
        public string Customer_ID { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Name2 { get; set; }
        public string Customer_Contact { get; set; }
        public string Branch { get; set; }

        public string SaleDate { get; set; }
        public string Amount { get; set; }
        public string VATExp { get; set; }
        public string Void { get; set; }
        public string VATRate { get; set; }
        public string Other_charges { get; set; }
        public string Discount { get; set; }
        public string Total_Amt { get; set; }
        public string Paid_amt { get; set; }
        public string Bal_due { get; set; }
        public string SaleStatus_ID { get; set; }
        public string transaction_Status { get; set; }
        public string Saletype { get; set; }
        public string Sale_trans_type { get; set; }
        public string SaleRef_sno { get; set; }
        public string C_id { get; set; }
        public string Manualbook_ref { get; set; }
        public string Customer_Ref { get; set; }
        public string Terms { get; set; }
        public string Temp_ID { get; set; }
        public string Invoice_To { get; set; }
        public string Invoice_address { get; set; }
        public string CustomerTRN { get; set; }
        public string Remarks { get; set; }
        public string Seller_ID { get; set; }
        public string Agent_ID { get; set; }

        public string Showroom_ID { get; set; }
        public string Total_Amt_inWords { get; set; }
        public string Performa_Validity { get; set; }

        public string ExportTo_Destination_ID { get; set; }
        public string Port_of_Exit_ID { get; set; }
        public string Trans_Ref_Other { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }
        public string IsDeleted { get; set; }

        public string shipping_co_ID { get; set; }
        public string ExporterCo_ID { get; set; }
        public string Bank_to_Transfer_payment_ID { get; set; }

        //----Following fields that not exist in actual table
        public string SaleStatus { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public string AfterVatTotal { get; set; }
        public string ExportCo { get; set; }
        public string ShipCo { get; set; }
        public string Destinations { get; set; }

        public string PortOfExit { get; set; }

        public string Total_Detail_Amount { get; set; }
        public string Total_Master_Amount { get; set; }
        public string Total_VATExp { get; set; }
        public string Total_Paid { get; set; }
        public string Total_Bal_Due { get; set; }

        public string ApprovalStatus { get; set; }
        public string Date { get; set; }

        public string Company_TRN { get; set; }

        public int Currency_ID { get; set; }
        public int ApprovedBy { get; set; }

        public string Currency_ShortName { get; set; }
        public string Minor_ShortName { get; set; }
        public string Curr_Rate { get; set; }
        public string Unit_Price { get; set; }
        public string Make { get; set; }
        public string ModelDesciption { get; set; }
        public string Color { get; set; }
        public string ModelYear { get; set; }
        public string Chassis_No { get; set; }
        public string Engine_No { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string IBAN { get; set; }
        public string SwiftCode { get; set; }
        public string BankAddress { get; set; }
        public string TotalBeforeVAT { get; set; }
        public string Quantity { get; set; }
        public string ItemDesc { get; set; }
        public string SaleInvoiceType { get; set; }
        public string PortTO { get; set; }
        public string PortFrom { get; set; }
        public string Destination { get; set; }
        public string imagepath { get; set; }
        public string imgname { get; set; }
        public string QRName { get; set; }

        public string ShippingCharges { get; set; }     
        public string Other_Status { get; set; }

        public string OtherCost { get; set; }

        public string Tip { get; set; }
        public string Commision { get; set; }
        public string Wallet { get; set; }
        public  int CarCount { get; set; }




    }

}
