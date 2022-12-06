namespace DAL.Models
{
    public class EngineType_DAL
    {
        public int ID { get; set; }
        public int? EngineType_ID { get; set; }
        public string Engine_Power { get; set; }

        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }

    }

    public class EngineSpecsCode_DAL
    {
        
        public int EngineSpecsCode_ID { get; set; }
        public string EngineSpecsCode { get; set; }
        public string SpecsDescription { get; set; }

        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }

        public int c_ID { get; set; }

    }

    public class StockType_DAL
    {
        public int StockType_ID { get; set; }
        public string StockTypeName { get; set; }

    }
}
