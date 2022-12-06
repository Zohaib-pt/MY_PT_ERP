namespace DAL.Models
{
    public class pa_Vanning_Master
    {

        public int ID { get; set; }
        public int? vanning_Master_ID { get; set; }
        public int? Vendor_ID { get; set; }
        public string Date { get; set; }
        public string Vendor_Name { get; set; }

        public string vanning_ref { get; set; }
        public string trans_ref { get; set; }
        public string Comments { get; set; }
        public string Amount { get; set; }
        public string VendorName { get; set; }
        public string SaleRef { get; set; }
        public string Tax_Amount { get; set; }
        public string Total_Amount { get; set; }
        public string temp_ID { get; set; }
        public string Created_at { get; set; }
        public int? Created_By { get; set; }
        public string Balance { get; set; }
        public string Paid { get; set; }
        public string Purchased_Status { get; set; }
        public string Last_Updated_at { get; set; }


        public int? Last_Updated_by { get; set; }

        public int? transaction_status { get; set; }

    }
}
