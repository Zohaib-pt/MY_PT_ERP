namespace DAL.Models
{
    public class pa_vehicle_display_details
    {
        public int ID { get; set; }
        public int? vehicle_display_master_ID { get; set; }
        public int? vehicle_display_details_ID { get; set; }
        public string chassis_no { get; set; }
        public string return_Date { get; set; }
        public string Curr_Car_Location { get; set; }
        public string VehicleDesc { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }
    }
}
