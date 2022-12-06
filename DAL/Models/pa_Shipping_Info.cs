using System;

namespace DAL.Models
{
    public class pa_Shipping_Info
    {
        public int ID { get; set; }
        public int Shipping_info_ID { get; set; }

        public string Shipping_Info_Ref { get; set; }
        public string trans_ref { get; set; }
        public string trans_Date { get; set; }
        public string Invoice_ref { get; set; }
        public string Invoice_Date { get; set; }
        public string Booking_ref { get; set; }
        public Nullable<int> Shipper_ID { get; set; }
        public string Shipper_Name { get; set; }
        public string Vessel_Name { get; set; }
        public string Vessel_Name_VoyNo { get; set; }
        public Nullable<int> Port_of_Loading_ID { get; set; }
        public string ETD { get; set; }
        public Nullable<int> Port_of_Discharge_ID { get; set; }
        public Nullable<int> Final_Destination_ID { get; set; }
        public string ETA { get; set; }
        public string Notify_Party { get; set; }
        public string Berth { get; set; }
        public string Berth_in_date { get; set; }
        public string Berth_Carry_Charges { get; set; }
        public string BL_no { get; set; }
        public string Inspection_Type { get; set; }
        public string Inspection_Charges { get; set; }
        public string Paid { get; set; }
        public string Balance { get; set; }
        public string Total_Amount { get; set; }
        public string PaymentStatus { get; set; }
        public string temp_ID { get; set; }
        public string created_at { get; set; }
        public string PortFrom { get; set; }
        public string PortTo { get; set; }
        public string Destination { get; set; }
        public string Shipper { get; set; }
        public string SaleRef { get; set; }
        public string last_updated_at { get; set; }
        public string ContainerType { get; set; }
        public string Container_Count { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<int> last_updated_by { get; set; }
        public Nullable<int> transaction_status { get; set; }
    }
}
