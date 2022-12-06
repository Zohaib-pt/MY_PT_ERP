namespace DAL.Models
{
    public class Pa_DeliveryOrder_Master
    {
        public int ID { get; set; }
        public int? DeliveryMaster_ID { get; set; }
        public string DeliveryRef { get; set; }
        public string CarTakenDate { get; set; }
        public string CarTakenPerson { get; set; }
        public string CarTakenContact { get; set; }
        public string DeliveryDate { get; set; }
        public bool CarTaken { get; set; }
        public int? c_ID { get; set; }

        public string SVRef { get; set; }

        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }


        public string isDeleted { get; set; }
        public string chassis_No { get; set; }
        public string CarLocation { get; set; }
        public string CustomerName { get; set; }
        public string RVNO { get; set; }
        public string Vehicle { get; set; }
        public string color { get; set; }
        public string Notes { get; set; }
    }
}
