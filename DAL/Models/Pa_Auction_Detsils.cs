namespace DAL.Models
{
    public class Pa_Auction_Detsils
    {
        public int ID { get; set; }
        public int Detail_ID { get; set; }

        public int? Auction_ID { get; set; }
        public int? Stock_ID { get; set; }
        public string Chassis { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }

    }
}
