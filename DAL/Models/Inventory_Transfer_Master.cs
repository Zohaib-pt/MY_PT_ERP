namespace DAL.Models
{
    public class Inventory_Transfer_Master
    {
        public int Transfer_ID { get; set; }
        public string transferDate { get; set; }

        public string Ref { get; set; }

        public string OtherDetails { get; set; }


        public int Loc_ID { get; set; }


        public string created_by { get; set; }


        public string created_at { get; set; }


        public string Last_Updated_at { get; set; }


        public string Last_Update_by { get; set; }

        public string ItemLocationName { get; set; }
    }
}
