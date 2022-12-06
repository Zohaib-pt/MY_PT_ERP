namespace DAL.Models
{
    public class InventoryTransferDetails
    {
        public int ID { get; set; }
        public int invoiceID { get; set; }

        public string BarCode { get; set; }

        public int ItemId { get; set; }


        public string UM_ID { get; set; }


        public string MajorQty { get; set; }


        public string MinorQty { get; set; }


        public string ItemDesc { get; set; }


        public string Unit_Price { get; set; }

        public string Amount { get; set; }

        public string job_ref { get; set; }


        public int OldLoc_ID { get; set; }


        public int NewLoc_ID { get; set; }


        public string transaction_Status { get; set; }


        public int transfer_ID { get; set; }

        public int Temp_ID { get; set; }

        public int c_id { get; set; }

        public string comments { get; set; }
        public string quantity { get; set; }
        public string NewItemLocationName { get; set; }
        public string OldItemLocationName { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }




    }
}
