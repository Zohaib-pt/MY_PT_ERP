using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
   public class pa_Select_SalesDashboardList
    {
        public string Make { get; set; }

        public string chassis { get; set; }
        public string Model_Desc { get; set; }

        public string Model_Year { get; set; }

        public int Qty { get; set; }

        public string Color_Interior_Name { get; set; }
        public int Color_Interior_ID { get; set; }

        public string Color_Exterior_Name { get; set; }
        public int Color_Exterior_ID { get; set; }

        public decimal Total_Cost{ get; set; }

        public string Production_Date { get; set; }

        public int C_id { get; set; }

        public int Stock_Id { get; set; }
    }
}
