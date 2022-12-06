using System;

namespace DAL.Models
{

    public class pa_PapersInfo
    {

        public int id { get; set; }
        public int paper_ID { get; set; }
        public int Stock_ID { get; set; }

        public string Recieved_Date { get; set; }
        public string PaperType { get; set; }
        public bool IsRecieved { get; set; }
        public string Ref { get; set; }
        public string Created_at { get; set; }
        public int Created_by { get; set; }
        public int isDeleted { get; set; }
        public int Modified_by { get; set; }

        public string Modified_at { get; set; }

    }

    public partial class pa_Select_PaperList_Result
    {
        public int paper_ID { get; set; }
        public string Recieved_Date { get; set; }
        public string Chassis { get; set; }
        public string Purchase_Ref { get; set; }
        public string Status { get; set; }
        public string accountId { get; set; }

        public string AdjustedRef { get; set; }
        public int Stock_ID { get; set; }
        public string PurchaseDate { get; set; }
        public string PlatNumberFee { get; set; }
        public string Cancel_Submit_Date { get; set; }
        public string RefundAMount { get; set; }
        public string Refund_Request_Date { get; set; }
        public string Make { get; set; }
        public string ModelDesciption { get; set; }
        public string ModelYear { get; set; }
        public string Ref { get; set; }

    }

    public partial class pa_Select_EPaperList_Result
    {
        public int paper_ID { get; set; }
        public string Submit_Date { get; set; }
        public string Chassis { get; set; }
        public string SaleType { get; set; }


    }
    public class Papers
    {
        public int Paper_ID { get; set; }
        public string CHASSIS_NO { get; set; }

        public string Recieved_Date { get; set; }
        public bool IsRecieved { get; set; }
        public string Ref { get; set; }
        public int Created_by { get; set; }
        public DateTime Created_at { get; set; }
        public string PaperType { get; set; }
        public int isDeleted { get; set; }
        public int Modified_by { get; set; }
        public DateTime Modified_at { get; set; }

        public string FUEL_TYPE { get; set; }
        public string weight { get; set; }
        public string gweight { get; set; }
        public string make_id { get; set; }
        public string model { get; set; }
        public string SEAT { get; set; }
        public string Registration { get; set; }
        public DateTime FirstRegistrationDate { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public string MonthYear { get; set; }




        public string Lenght { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string Vehicle_CC { get; set; }
        public string Drive { get; set; }


    }
    public class Wordss
    {
        public int ID { get; set; }
        public string Words { get; set; }

    }
    public class Cities
    {
        public int ID { get; set; }

        public string City_Name { get; set; }
    }
}
