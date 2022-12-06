namespace DAL.Models
{
    public class pa_vehicle_display_master
    {
        public int ID { get; set; }
        public int? vehicle_display_master_ID { get; set; }
        public string vehicle_Ref { get; set; }
        public string Delivery_DateTime { get; set; }
        public string Showroom_Loc_ID { get; set; }
        public string delivered_by_EmpID { get; set; }
        public string Showroom_LocationName { get; set; }
        public string Ref { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }
    }
}
