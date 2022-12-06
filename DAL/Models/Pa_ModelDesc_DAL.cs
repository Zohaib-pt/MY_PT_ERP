namespace DAL.Models
{
    public class Pa_ModelDesc_DAL
    {
        public int ID { get; set; }
        public int? ModelDesc_ID { get; set; }
        public string ModelDesciption { get; set; }
        public int? Make_ID { get; set; }
        public int? VehCategory_ID { get; set; }
        public string MakeCountry_ID { get; set; }
        public string Make { get; set; }
        public string CountryName { get; set; }
        public string VehCategoryName { get; set; }



        public string  Hs_Code { get; set; }
        public string Door { get; set; }
        public int? EngineType { get; set; }
        public string EngineTypeName { get; set; }
        public string FuelType { get; set; }
        public string Drive { get; set; }



        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }
        public string Makecode { get; set; }



    }
}
