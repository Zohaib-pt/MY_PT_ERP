namespace DAL.Models
{
    public class Pa_Production_Material_Out
    {
        public int ID { get; set; }

        public int Material_OUT_ID { get; set; }

        public int Production_ID { get; set; }


        public string ItemID { get; set; }

        public string DirectCost { get; set; }

        public string InDirectCost { get; set; }

        public string MajorQty { get; set; }

        public string MinorQty { get; set; }

        public string UOM { get; set; }


        public string UnitPrice { get; set; }


        public string Amount { get; set; }


        public string transaction_Status { get; set; }
        public string Quantity { get; set; }
        public string FormulaName { get; set; }

        public int formula_ID { get; set; }



    }
}
