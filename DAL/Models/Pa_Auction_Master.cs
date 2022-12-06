namespace DAL.Models
{
    public class Pa_Auction_Master
    {
        public int ID { get; set; }
        public int? Auction_ID { get; set; }
        public string Auction_Date { get; set; }
        public string Ref { get; set; }
        public string Remarks { get; set; }

        public string Created_By { get; set; }
        public string Created_At { get; set; }
        public string Last_Updated_By { get; set; }
        public string Last_Updated_At { get; set; }

        public string c_ID { get; set; }
    }
}
