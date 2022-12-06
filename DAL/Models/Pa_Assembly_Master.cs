using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
   public class Pa_Assembly_Master
    {

        public int ID { get; set; }

        public int Assembly_ID { get; set; }

        public string P_Date { get; set; }


        public string Ref { get; set; }

        public string CustomerRef { get; set; }

        public string Supervisor { get; set; }

        public string Assembly_Chassis { get; set; }

        public string Created_By { get; set; }

        public string Created_At { get; set; }


        public string Last_Updated_By { get; set; }


        public string Last_Updated_At { get; set; }


        public string transaction_Status { get; set; }


        public int c_ID { get; set; }
        public int ItemID { get; set; }
        public string CHASSIS_NO { get; set; }
        public string make_ID { get; set; }
        public string MakeModel_description_ID { get; set; }
        public string ModelYear { get; set; }
        public string color_exterior_ID { get; set; }
        public string color_interior_ID { get; set; }
        public string Assembly_Type { get; set; }
    }
}
