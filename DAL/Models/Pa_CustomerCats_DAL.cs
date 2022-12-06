using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class Pa_CustomerCats_DAL
    {
        public int? CustomerCat_ID { get; set; }
        public string CustomerCatName { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }


    }
}
