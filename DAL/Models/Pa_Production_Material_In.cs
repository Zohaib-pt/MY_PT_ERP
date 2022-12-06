namespace DAL.Models
{
    public class Pa_Production_Material_In
    {

        public int ID { get; set; }

        public int Material_IN_ID { get; set; }

        public int Production_ID { get; set; }


        public int ItemID { get; set; }

        public string MajorQty { get; set; }

        public string MinorQty { get; set; }

        public string UOM { get; set; }

        public string UnitPrice { get; set; }

        public string Amount { get; set; }


        public string transaction_Status { get; set; }


        public string created_At { get; set; }


        public string Last_Updated_At { get; set; }





    }
}
