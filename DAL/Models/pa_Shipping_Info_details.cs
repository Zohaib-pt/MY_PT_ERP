using System;

namespace DAL.Models
{


    public class pa_Shipping_Info_details
    {
        public int ID { get; set; }
        public int Shipping_info_ID { get; set; }
        public Nullable<int> stock_ID { get; set; }

        public string Chassis_no { get; set; }
        public string make { get; set; }
        public Nullable<int> make_ID { get; set; }
        public string model_description { get; set; }
        public string color { get; set; }
        public string stock_ref { get; set; }

        public string Inspection_Charges { get; set; }
        public string Berth_Carry_Charges { get; set; }

        public string Berth_Carry_ChargesTax { get; set; }
        public string Inspection_ChargesTax { get; set; }

        public string TotalCharges { get; set; }

        public string temp_ID { get; set; }
        public string created_at { get; set; }
        public string last_updated_at { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<int> last_updated_by { get; set; }
        public Nullable<int> transaction_status { get; set; }

    }
}
