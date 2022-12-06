namespace DAL.Models
{
    public class Pa_Partners_DAL
    {

        public int ID { get; set; }
        public int Partner_ID { get; set; }
        public int Party_Type_ID { get; set; }
        public string Partner_Ref { get; set; } //----- this field to be serial for each partyType
        public string PartnerName { get; set; }
        public string Party_Type { get; set; }
        public string ContactNo { get; set; }
        public string MobileNo { get; set; }
        public string ContactName { get; set; }
        public string ContactAddress { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string EmiratesID { get; set; }
        public string TradeLicenseNo { get; set; }
        public string TRN { get; set; }

        public int? VendorCat_ID { get; set; }
        public string VendorCatName { get; set; }

        public string PartnerType { get; set; }  //----- Here use the type (Garage,Transporter,Shipper, Vendor)
        public string Partner_Sno { get; set; }
        public string Remarks { get; set; }

        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }

        public string BalanceType { get; set; }

        public string OpeningBalanceDate { get; set; }
        public string OpeningBalance { get; set; }
        public string SellerType { get; set; }




    }
}
