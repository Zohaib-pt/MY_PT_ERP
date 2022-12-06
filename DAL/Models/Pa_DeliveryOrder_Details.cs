namespace DAL.Models
{
    public class Pa_DeliveryOrder_Details
    {
        public int ID { get; set; }
        public int? DeliveryDetails_ID { get; set; }
        public int? DeliveryMaster_ID { get; set; }
        public string Chassis_No { get; set; }
        public string CarTakenDate { get; set; }
        public string CarTakenPerson { get; set; }
        public string CarTakenContact { get; set; }
        public string DilveryOrder_Issued { get; set; }
        public string stock_ID { get; set; }
        public string Ref { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

    }
}
