using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
	public class Pa_CommisionRates_DAL
	{

		public int ID { get; set; }
		public int CommissionRates_id { get; set; }
		public int Category_ID { get; set; }
		public string Category_Name { get; set; }
		public decimal Rate { get; set; }
		public string Created_by { get; set; }
		public string Created_at { get; set; }
		public string Modified_by { get; set; }
		public string Modified_at { get; set; }
		public int Status_ID { get; set; }

		public string Status { get; set; }


	}
}