namespace DAL.Models
{
    public class pa_Vanning_Details
    {
        public int ID { get; set; }
        public int? vanning_Master_ID { get; set; }
        public int? Stock_ID { get; set; }
        public string Chassis_no { get; set; }
        public string trans_ref { get; set; }
        public string Amount { get; set; }
        public string Tax_Amount { get; set; }
        public string Total_Amount { get; set; }
        public string temp_ID { get; set; }
        public string Created_at { get; set; }
        public int? Created_By { get; set; }
        public string Last_Updated_at { get; set; }
        public int? Last_Updated_by { get; set; }
        public int? transaction_status { get; set; }

        //  c.color, m.make, tblmake_details.model_description, 
        public string color { get; set; }
        public string make { get; set; }
        public string model_description { get; set; }

        public int? make_ID { get; set; }
        public int? model_description_ID { get; set; }


        public string result { get; set; }

        public string Inspection_Charges { get; set; }
        public string Inspection_ChargesTax { get; set; }


    }
}
