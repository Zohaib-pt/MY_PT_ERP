using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
   public class Pa_GoodsReceived_Master
    {
        public int ID { get; set; }
        public int? GRVMaster_ID { get; set; }
        public int PurchaseMaster_ID { get; set; }
        public string GRVRef { get; set; }  //----- (PR for GRV , PO for Other GRV) Example PR1, PR2, PO1
        public string PurchaseRef { get; set; }  //----- (PR for GRV , PO for Other GRV) Example PR1, PR2, PO1
        public string GRVDate { get; set; }
        public int? Vendor_ID { get; set; }

        public string Vendor_Name { get; set; }
        public string Vendor_PruchaseTo { get; set; }
        public string Vendor_Address { get; set; }
        public string GRVStatus_ID { get; set; }
        public string GRVStatus { get; set; }

        public string Total { get; set; }
        public string VAT_Rate { get; set; }

        public string Paid { get; set; }
        public string Bal_Due { get; set; }
        public string CountCars { get; set; }

        public string Total_Amount { get; set; }

        public string Total_Amount_Other { get; set; }

        public string VAT_Exp { get; set; }
        public string Discount { get; set; }
        public string Approved_at { get; set; }
        public string Approve_by { get; set; }
        public string GRVType { get; set; }
        public string GRV_Sno { get; set; }  //--- -- PR - Normal GRV, PO - Other GRV


        public string Created_at { get; set; }
        public string Created_by { get; set; }
        public string Modified_at { get; set; }
        public string Modified_by { get; set; }

        public string IsDeleted { get; set; }


        //public string BeforeVATTotal { get; set; }
        //public string AfterVATTotal { get; set; }
        public string TotalVAT_Exp { get; set; }
        public string TotalPaid { get; set; }
        public string TotalBal_due { get; set; }
        public string TotalDiscount { get; set; }
        public string OtherREF { get; set; }
        public string Vendor_Contact { get; set; }
        public string PaymentDueDate { get; set; }
        public string transaction_status { get; set; }
        public string cheque_no { get; set; }
        public string cheque_date { get; set; }
        public string cheque_bank_name { get; set; }

        public string Balance { get; set; }

    }
}
