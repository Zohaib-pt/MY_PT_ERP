using DAL.Models;
using DAL.oClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using X.PagedList;
using ZXing;
using ZXing.QrCode;

namespace PT_ERP.Controllers
{
    public class AccountsController : BaseController
    {


        #region static variables region
        //---Static Fields for creating new payment voucher general
        //static string PaymentMasterGeneral_TempID;
        //static int? PaymentMasterGeneralStatic_ID;
        //{
        //    get { return TempData["PaymentMasterGeneral_TempID"]?.ToString() == null ? "" : TempData["PaymentMasterGeneral_TempID"]?.ToString(); }
        //    set { TempData["PaymentMasterGeneral_TempID"] = value; }
        //}


        //--Static fields for creating receipt voucher gen


        //--Static fields for creating receipt voucher of type sale


        //---Static Fields for creating new payment voucher of type vendor


        static string JVMaster_TempID;

        static int? JVMasterStatic_ID;
        static string ReceiptURL;
        static string PaymentURL;
        static string ApprovalURL;
        static string Urls;
        #endregion

        #region interfaces variables
        private readonly IOBasicData _oBasic;
        private readonly IOAccounts _oAccounts;
        private IConfiguration configuration;
        private object _oHome;
        #endregion

        public AccountsController(IOBasicData oBasicBase, IOAccounts oAccounts, IConfiguration iConfig)
         : base(oBasicBase)
        {
            _oBasic = oBasicBase;
            _oAccounts = oAccounts;
            configuration = iConfig;
        }
        [HttpGet]
        public IActionResult Ledger(string From_Date, string To_Date, int AccountID, string Trans_Ref, int? page)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);


            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }

            #endregion
            ViewBag.SectionHeaderTitle = "Ledger";
            ViewBag.SectionSub_HeaderTitle = "Ledger";
            ViewBag.PageTitle = "Ledger";
            From_Date = (String.IsNullOrEmpty(From_Date) || String.IsNullOrWhiteSpace(From_Date) ? "" : From_Date);
            To_Date = (String.IsNullOrEmpty(To_Date) || String.IsNullOrWhiteSpace(To_Date) ? "" : To_Date);
            AccountID = (AccountID == 0 ? 0 : AccountID);
           

            Trans_Ref = (String.IsNullOrEmpty(Trans_Ref) || String.IsNullOrWhiteSpace(Trans_Ref) ? "" : Trans_Ref);
      

            ViewBag.From_Date_Ledger = From_Date;
            ViewBag.To_Date_Ledger = To_Date;
            ViewBag.AccountID_Ledger = AccountID;
           


            ViewBag.Trans_Ref_Ledger = Trans_Ref;
          

            ViewBag.allaccountlist = _oAccounts.get_AllAccountList_DAL(c_ID).ToList();
            ViewBag.HeadAccountList = _oAccounts.get_HeadAccountList_DAL(c_ID).ToList();





            ViewBag.RecordsPerPage = 10;
            ViewBag.PageTitle = "Ledger";


            _oAccounts.AccountMaster_obj = _oAccounts.Get_LedgerList_DAL(AccountID, c_ID);

            _oAccounts.LedgerMasterIPagedList = _oAccounts.Get_LedgerList_DAL(From_Date, AccountID, To_Date, Trans_Ref, c_ID).ToPagedList(page ?? 1, 10);
            _oAccounts.LedgerMasterIPagedList_TTL = _oAccounts.Get_LedgerList_DAL_TTL(From_Date, AccountID, To_Date, Trans_Ref, c_ID).ToList();


            return View(_oAccounts);


        }
        [HttpGet]
        public IActionResult Search_Ledger(string From_Date, string To_Date, int AccountID, string Trans_Ref,  int? page)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);

            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            From_Date = (String.IsNullOrEmpty(From_Date) || String.IsNullOrWhiteSpace(From_Date) ? "" : From_Date);
            To_Date = (String.IsNullOrEmpty(To_Date) || String.IsNullOrWhiteSpace(To_Date) ? "" : To_Date);
            AccountID = (AccountID == 0 ? 0 : AccountID);
         

            Trans_Ref = (String.IsNullOrEmpty(Trans_Ref) || String.IsNullOrWhiteSpace(Trans_Ref) ? "" : Trans_Ref);
            //HeadAccount_ID = (HeadAccount_ID == 0 ? 0 : HeadAccount_ID);


            ViewBag.From_Date_Ledger = From_Date;
            ViewBag.To_Date_Ledger = To_Date;
            ViewBag.AccountID_Ledger = AccountID;
         
            ViewBag.Trans_Ref_Ledger = Trans_Ref;
         

            ViewBag.allaccountlist = _oAccounts.get_AllAccountList_DAL(c_ID).ToList();
            ViewBag.HeadAccountList = _oAccounts.get_HeadAccountList_DAL(c_ID).ToList();


            ViewBag.RecordsPerPage = 10;

            _oAccounts.AccountMaster_obj = _oAccounts.Get_LedgerList_DAL(AccountID, c_ID);

            _oAccounts.LedgerMasterIPagedList = _oAccounts.Get_LedgerList_DAL(From_Date, AccountID,To_Date, Trans_Ref, c_ID).ToPagedList(page ?? 1, 10);
            _oAccounts.LedgerMasterIPagedList_TTL = _oAccounts.Get_LedgerList_DAL_TTL(From_Date, AccountID, To_Date, Trans_Ref, c_ID).ToList();


            try
            {
                ViewBag.allaccountlist = _oAccounts.get_AllAccountList_DAL(c_ID).ToList();

                ViewBag.RecordsPerPage = 10;

                _oAccounts.LedgerMasterIPagedList = _oAccounts.Get_LedgerList_DAL(From_Date, AccountID, To_Date, Trans_Ref, c_ID).ToPagedList(page ?? 1, 10);
                _oAccounts.LedgerMasterIPagedList_TTL = _oAccounts.Get_LedgerList_DAL_TTL(From_Date, AccountID, To_Date, Trans_Ref, c_ID).ToList();


                return PartialView("_LedgerPartialView", _oAccounts);
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }


        }



        //trail balance
        public IActionResult TrailBalance(string StartDate, string EndDate)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            ViewBag.SectionHeaderTitle = "Trail Balance";
            ViewBag.SectionSub_HeaderTitle = "Trail Balance";
            ViewBag.PageTitle = "Trail Balance";




            StartDate = (String.IsNullOrEmpty(StartDate) || String.IsNullOrWhiteSpace(StartDate) ? "" : StartDate);
            EndDate = (String.IsNullOrEmpty(EndDate) || String.IsNullOrWhiteSpace(EndDate) ? "" : EndDate);


            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            _oAccounts.TrailBalanceList = _oAccounts.Get_TrailBalanceList_DAL(StartDate, EndDate, c_ID);
            _oAccounts.TrailBalanceList_ttl = _oAccounts.Get_TrailBalanceList_TTL(StartDate, EndDate, c_ID);

             
            return View(_oAccounts);
        }
        //balancesheet
        [HttpGet]
        public IActionResult BalanceSheet(string From_Date, string To_Date)
        {
            ViewBag.BreadCumController = "Accounts";
            ViewBag.BreadCumAction = "BalanceSheet";

            ViewBag.SectionHeaderTitle = "Accounts";
            ViewBag.SectionSub_HeaderTitle = "Balance Sheet";
            ViewBag.PageTitle = "Balance Sheet";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);


             if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            


            if (From_Date == null && To_Date == null)
            {
                DateTime date = DateTime.Now;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                From_Date = firstDayOfMonth.ToString("dd-MMM-yyyy");
                To_Date = lastDayOfMonth.ToString("dd-MMM-yyyy");
            }




            From_Date = (String.IsNullOrEmpty(From_Date) || String.IsNullOrWhiteSpace(From_Date) ? "" : From_Date);
            To_Date = (String.IsNullOrEmpty(To_Date) || String.IsNullOrWhiteSpace(To_Date) ? "" : To_Date);

            _oAccounts.BalanceSheet_obj = _oAccounts.Balance_Sheet_DAL(From_Date, To_Date, c_ID).ToList();
            _oAccounts.BalSh_byCurrentAssets_obj = _oAccounts.BalSh_byCurrentAssets_DAL(From_Date, To_Date, c_ID).ToList();
            _oAccounts.BalSh_byCurrentLaibilities_obj = _oAccounts.BalSh_byCurrentLaibilities_DAL(From_Date, To_Date, c_ID).ToList();
            _oAccounts.BalSh_byFixedAssets_obj = _oAccounts.BalSh_byFixedAssets_DAL(From_Date, To_Date, c_ID).ToList();
            _oAccounts.BalSh_byLongTermLaibilities_obj = _oAccounts.BalSh_byLongTermLaibilities_DAL(From_Date, To_Date, c_ID).ToList();
            _oAccounts.BalSh_byCapitalInvestments_obj = _oAccounts.BalSh_byCapitalInvestments_DAL(From_Date, To_Date, c_ID).ToList();
            _oAccounts.BalSh_byDrawings_obj = _oAccounts.BalSh_byDrawings_DAL(From_Date, To_Date, c_ID).ToList();
            _oAccounts.Get_NetIncome_onPrint_obj = _oAccounts.Get_NetIncome_onPrint_DAL(From_Date, To_Date, c_ID).ToList();

            return View(_oAccounts);


        }


        [HttpGet]
        public IActionResult IncomeStatement(string From_Date, string To_Date)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            ViewBag.SectionHeaderTitle = "Income Statement";
            ViewBag.SectionSub_HeaderTitle = "Income Statement";

            ViewBag.PageTitle = "Income Statement";
            if (From_Date == null && To_Date == null)
            {
                DateTime date = DateTime.Now;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                From_Date = firstDayOfMonth.ToString("dd-MMM-yyyy");
                To_Date = lastDayOfMonth.ToString("dd-MMM-yyyy");
            }


            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);

            From_Date = (String.IsNullOrEmpty(From_Date) || String.IsNullOrWhiteSpace(From_Date) ? "" : From_Date);
            To_Date = (String.IsNullOrEmpty(To_Date) || String.IsNullOrWhiteSpace(To_Date) ? "" : To_Date);

            

            _oAccounts.IncomeStatment_INC_obj = _oAccounts.IncomeStatment_INC_DAL(From_Date, To_Date, c_ID).ToList();
            
            _oAccounts.IncomeStatment_CGS_obj = _oAccounts.IncomeStatment_CGS_DAL(From_Date, To_Date, c_ID).ToList();

            _oAccounts.IncomeStatment_Exp_obj = _oAccounts.IncomeStatment_Exp_DAL(From_Date, To_Date, c_ID).ToList();

            _oAccounts.IncomeStatment_Exp_Fin_obj = _oAccounts.IncomeStatment_Exp_Fin_DAL(From_Date, To_Date, c_ID).ToList();
            _oAccounts.IncomeStatment_disRet_obj = _oAccounts.IncomeStatment_disRet(From_Date, To_Date, c_ID).ToList();

            _oAccounts.IncomeStateMent_obj = _oAccounts.IncomeStateMent_DAL(From_Date, To_Date, c_ID).ToList();

            return View(_oAccounts);


        }


        #region PAYMENT GENERAL

        [HttpGet]
        public IActionResult NewPaymentVouchersGeneral(int? PaymentMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Accounts";
            ViewBag.BreadCumAction = "NewPaymentVouchersGeneral";

            ViewBag.SectionHeaderTitle = "Accounts";
            ViewBag.SectionSub_HeaderTitle = "Payment Vouchers (General)";
            ViewBag.PageTitle = "Payment Vouchers (General)";


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            #region ViewBag Area
            ViewBag.PartyType = _oBasic.get_PartyType_DAL().ToList();
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.get_PartyList_DAL(c_ID).ToList();
            //---Get Accounts List
            ViewBag.CR_accountIDList = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();
            //---Get pay via list
            ViewBag.DR_AccountsList = _oBasic.Select_PV_OnAccounts_DAL(c_ID).ToList();
            ViewBag.DR_AccountsListBanks = _oBasic.Select_PV_OnAccounts_DAL(c_ID).Where(x => x.Sys_AC_type_ID ==13).ToList();

          
            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;


            #endregion


            if (PaymentMaster_ID == null || PaymentMaster_ID < 1)
            {
                string PaymentMasterGeneral_TempID = null;


                //---Create payment master Ref(tem id)
                if (String.IsNullOrEmpty(PaymentMasterGeneral_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    HttpContext.Session.SetString("PaymentMasterGeneral_TempID", obj.ToString());

                }
                PaymentMasterGeneral_TempID = HttpContext.Session.GetString("PaymentMasterGeneral_TempID");
                if (string.IsNullOrEmpty(PaymentMaster_ID.ToString()))
                {
                    PaymentMaster_ID = 0;
                    HttpContext.Session.SetString("PaymentMasterGeneralStatic_ID", PaymentMaster_ID.ToString());
                }

                //---Get payment master id
                ViewBag.PaymentMaster_ID = PaymentMaster_ID;

                //--Get payment Master
                _oAccounts.paymentMasterObject = _oAccounts.Get_PaymentMasterByID_DAL(-1);

                //--Get payment detail
                _oAccounts.paymentDetailList = _oAccounts.Get_PaymentDetailListByID_DAL(PaymentMasterGeneral_TempID, -1).ToList();
            }
            else
            {

                //---Get old temp id from payment detail table that already creaded for this "PaymentMaster_ID" id. And then use this if new record added
                //    string OldTempID = _oAccounts.GetOldTempIDFromPaymentDetail_DAL(PaymentMaster_ID);

                Urls = null;

                PaymentURL = HttpContext.Request.Headers["Referer"];
                Urls = PaymentURL;

                HttpContext.Session.SetString("PaymentMasterGeneralStatic_ID", PaymentMaster_ID.ToString());

                //---Get payment master id
                ViewBag.PaymentMaster_ID = PaymentMaster_ID;

                //--Get payment Master

                _oAccounts.paymentMasterObject = _oAccounts.Get_PaymentMasterByID_DAL(PaymentMaster_ID);
                //--Get payment detail
                _oAccounts.paymentDetailList = _oAccounts.Get_PaymentDetailListByID_DAL("0", PaymentMaster_ID).ToList();
            }



            return View(_oAccounts);
        }


        //---insert into payment detial general
        [HttpPost]
        public IActionResult InsertPaymentDetail_General(int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others,
            int? CR_accountID, string Description,
            string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount, int? stock_ID, int hdPaymentMaster_ID
            , double TaxAmount, double ExtraCharges, double ExtraChargesTax,double TaxAmountOthers,string BLNO,int PDC_Payable_Acc_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            string PaymentMasterGeneral_TempID = HttpContext.Session.GetString("PaymentMasterGeneral_TempID");
            if (hdPaymentMaster_ID == 0)
            {
                Temp_ID = PaymentMasterGeneral_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            //---making stock_id 0 if null from page
            stock_ID = stock_ID == null ? 0 : stock_ID;

            if (DR_accountID == null || Amount == null || CR_accountID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            string IsJapanSpecsEnabled = configuration.GetSection("AppSettings").GetSection("IsJapanSpecsEnabled").Value;
            if (IsJapanSpecsEnabled == "true")
            {
                VAT_Exp = TaxAmount;
            }


            try
            {
                message = _oAccounts.InsertPaymentDetail_General_DAL(DR_accountID, Amount, VAT_Rate, Currency_ID, Currency_Rate, Amount_Others, CR_accountID, Description,
             trans_ref, cheque_bank_name, cheque_date, cheque_no, VAT_Exp, Total_Amount, stock_ID, Temp_ID, c_ID, Created_by, 
             hdPaymentMaster_ID, TaxAmount, ExtraCharges, ExtraChargesTax,TaxAmountOthers,BLNO, PDC_Payable_Acc_ID);




                if (message == "Saved Successfully!")
                {

                    _oAccounts.paymentDetailList = _oAccounts.Get_PaymentDetailListByID_DAL(Temp_ID, hdPaymentMaster_ID).ToList();

                    return PartialView("_PaymentDetailGeneralList_Partial", _oAccounts);
                }
                else
                {
                    return Json(new { message = "Record not saved! Please try again!" });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured! Please try again!" });
            }


        }

        //---Update  payment detail general
        [HttpPost]
        public IActionResult UpdatePaymentDetail_General(int? PaymentDetails_ID, int? DR_accountID, double? AmountUpdate, int? VAT_Rate, int? Currency_ID,
            double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
            string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, 
            double? VAT_Exp, double? Total_Amount, int? stock_ID, int hdPaymentMaster_ID, 
            double ExtraCharges, double ExtraChargesTax, double TaxAmount, double TaxAmountOthers,string BLNO,int PDC_Payable_Acc_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);



            //string OldTempID = _oAccounts.GetOldTempIDFromPaymentDetail_DAL(PaymentMasterGeneralStatic_ID);
            //if (!String.IsNullOrEmpty(PaymentMasterGeneral_TempID))
            //{
            //    Temp_ID = PaymentMasterGeneral_TempID;
            //}
            //else
            //{
            //    Temp_ID = OldTempID;
            //}
            string PaymentMasterGeneral_TempID = HttpContext.Session.GetString("PaymentMasterGeneral_TempID");
            if (hdPaymentMaster_ID == 0)
            {
                Temp_ID = PaymentMasterGeneral_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            //PaymentMasterGeneralStatic_ID = PaymentMasterGeneralStatic_ID == null ? -1 : PaymentMasterGeneralStatic_ID;

            //---making stock_id 0 if null from page
            stock_ID = stock_ID == null ? 0 : stock_ID;

            if (PaymentDetails_ID == null || DR_accountID == null || AmountUpdate == null || CR_accountID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            try
            {
                message = _oAccounts.UpdatePaymentDetail_General_DAL(PaymentDetails_ID, DR_accountID, AmountUpdate, VAT_Rate, Currency_ID, Currency_Rate, Amount_Others, CR_accountID, Description,
             trans_ref, cheque_bank_name, cheque_date, cheque_no, VAT_Exp, Total_Amount, stock_ID, Temp_ID, c_ID, 
             Modified_by, ExtraCharges, ExtraChargesTax,TaxAmount,TaxAmountOthers,BLNO, PDC_Payable_Acc_ID);
                if (message == "Updated Successfully!")
                {

                    _oAccounts.paymentDetailList = _oAccounts.Get_PaymentDetailListByID_DAL(Temp_ID, hdPaymentMaster_ID).ToList();

                    return PartialView("_PaymentDetailGeneralList_Partial", _oAccounts);

                }
                else
                {
                    return Json(new { message = "Record not saved! Please try again!" });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured! Please try again!" });
            }

        }

        //---Delete  payment detail general
        [HttpPost]
        public IActionResult DeletePaymentDetail_General(int PaymentDetails_ID, int hdPaymentMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            //Temp_ID = PaymentMasterGeneral_TempID;
            //string OldTempID = _oAccounts.GetOldTempIDFromPaymentDetail_DAL(PaymentMasterGeneralStatic_ID);
            //if (!String.IsNullOrEmpty(PaymentMasterGeneral_TempID))
            //{
            //    Temp_ID = PaymentMasterGeneral_TempID;
            //}
            //else
            //{
            //    Temp_ID = OldTempID;
            //}
            //PaymentMasterGeneralStatic_ID = PaymentMasterGeneralStatic_ID == null ? -1 : PaymentMasterGeneralStatic_ID;

            //if (PaymentDetails_ID == 0)
            //{
            //    return Json(new { message = "ID is null. Please try again!" });
            //}
            string PaymentMasterGeneral_TempID = HttpContext.Session.GetString("PaymentMasterGeneral_TempID");
            if (hdPaymentMaster_ID == 0)
            {
                Temp_ID = PaymentMasterGeneral_TempID;
            }
            else
            {
                Temp_ID = "0";
            }
            try
            {
                message = _oAccounts.DeletePaymentDetail_DAL(PaymentDetails_ID);
                if (message == "Deleted Successfully!")
                {
                    _oAccounts.paymentDetailList = _oAccounts.Get_PaymentDetailListByID_DAL(Temp_ID, hdPaymentMaster_ID).ToList();

                    return PartialView("_PaymentDetailGeneralList_Partial", _oAccounts);
                }
                else
                {
                    return Json(new { message = "Record not deleted! Please try again!" });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured! Please try again!" });
            }

        }


        //---Insert  payment master general
        [HttpPost]
        public IActionResult InsertPaymentMaster_General(int? Vendor_ID, int? party_type, string Explanation, string Date, string NameText)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            string PaymentMasterGeneral_TempID = HttpContext.Session.GetString("PaymentMasterGeneral_TempID");
            Temp_ID = PaymentMasterGeneral_TempID;
            string PaymentType = "GEN";  //---It is the ref in table for new Payment master. Send this value hard coded from here

            if (Vendor_ID == null && NameText == null)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewPaymentVouchersGeneral", "Accounts");
            }

            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;
            party_type = party_type == null ? 0 : party_type;


            try
            {
                message = _oAccounts.Insert_PaymentGeneral_DAL(Vendor_ID, party_type, Explanation, PaymentType, Date, NameText, Temp_ID, c_ID, Created_by);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("PaymentsList", "Accounts");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("NewPaymentVouchersGeneral", "Accounts");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
                return RedirectToAction("NewPaymentVouchersGeneral", "Accounts");
            }


        }

        //---udpate  payment master general
        [HttpPost]
        public IActionResult UdpatePaymentMaster_General(int? PaymentMaster_ID, int? Vendor_ID, int? party_type, string Explanation, string Date, string NameText)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            string PaymentMasterGeneral_TempID = HttpContext.Session.GetString("PaymentMasterGeneral_TempID");
            Temp_ID = PaymentMasterGeneral_TempID;
            string OldTempID = _oAccounts.GetOldTempIDFromPaymentDetail_DAL(PaymentMaster_ID);

            if (!String.IsNullOrEmpty(OldTempID))
            {
                Temp_ID = OldTempID;
            }
            else
            {
                if (String.IsNullOrEmpty(Temp_ID))
                {
                    Guid obj = Guid.NewGuid();
                    Temp_ID = obj.ToString();
                }
            }
            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();


            if (Vendor_ID == null && NameText == null)
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            if (PaymentMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again!";
                return Redirect(url);
            }



            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;
            party_type = party_type == null ? 0 : party_type;


            try
            {
                message = _oAccounts.Update_PaymentGeneral_DAL(PaymentMaster_ID, Vendor_ID, party_type, Explanation, Date, NameText, Temp_ID, c_ID, Modified_by);

                if (message.Contains("Successfully"))
                {

                    //TempData["Success"] = "Updated Successfully!";

                    return RedirectToAction("PaymentsList", "Accounts");
                }
                else
                {


                    TempData["Error"] = "An error occured. Please try again!";
                    return Redirect(url);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
                return Redirect(url);
            }



        }


        #endregion PAYMENT GENERAL


        #region Chart of Accounts

        //---Chart of Accounts List
        public IActionResult chartofaccounts(string AccountName, int HeadAccounts_ID, int subHeadAccount_ID, int Sys_AC_type_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);


            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            ViewBag.SectionHeaderTitle = "Accounts";
            ViewBag.SectionSub_HeaderTitle = "Chart of Account";

            ViewBag.PageTitle = "Chart of accounts";

            //--DDL
            ViewBag.HeadAccountList = _oBasic.Get_select_HeadAccounts_DAL().ToList();
            ViewBag.sub_HeadAccountList = _oBasic.Get_select_sub_HeadAccount_DAL().ToList();
            ViewBag.Sys_Ac_TypeList = _oBasic.Get_Select_Sys_Ac_Type_DAL().ToList();

            if (AccountName == null)
            {
                AccountName = "";
            };

            _oAccounts.ChartofAccountList = _oAccounts.Get_select_AccountsList_DAL(AccountName, HeadAccounts_ID, subHeadAccount_ID, Sys_AC_type_ID, c_ID);

            return View(_oAccounts);


        }


        //---Add/Update Accounts for Chart of Accounts
        [HttpPost]
        public IActionResult SaveAccounts(string AccountName, int HeadAccount_ID, int sub_headAccount_ID, int sys_Ac_Type_ID,
            string Acc_ShortName, string OpeningBalance, string OpeningBalanceDate)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            int company_ID = Convert.ToInt32(HttpContext.Session.GetString("c_ID"));
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Accounts", "chartofaccounts");
            }
            #endregion

            string message = "";

            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);


            //if (Vendor_ID == null || String.IsNullOrEmpty(Date))
            //{
            //    TempData["Error"] = "Please fill all required fields!";
            //    return RedirectToAction("NewPaymentVouchersGeneral", "Accounts");
            //}

            //Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;


            try
            {
                message = _oAccounts.Insert_Accounts(AccountName, Acc_ShortName, HeadAccount_ID, sub_headAccount_ID, sys_Ac_Type_ID, OpeningBalance, OpeningBalanceDate, Convert.ToInt32(UserID_Admin), company_ID);
                if (message == "Saved Successfully!")
                {


                    return RedirectToAction("chartofaccounts", "Accounts");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("chartofaccounts", "Accounts");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
                return RedirectToAction("chartofaccount", "Accounts");
            }


        }

        [HttpPost]
        public IActionResult UpdateAccounts(string AccountName, string Acc_ShortName, int HeadAccounts_ID, int subHeadAccount_ID, int Sys_AC_type_ID,
         string Opening_Balance, string Opening_Bal_Date, int Account_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            int c_ID = Convert.ToInt32(HttpContext.Session.GetString("c_ID"));
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Accounts", "chartofaccounts");
            }
            #endregion

            string message = "";

            int? Created_by = Convert.ToInt32(UserID_Admin);


            //if (Vendor_ID == null || String.IsNullOrEmpty(Date))
            //{
            //    TempData["Error"] = "Please fill all required fields!";
            //    return RedirectToAction("NewPaymentVouchersGeneral", "Accounts");
            //}

            //Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;


            try
            {
                message = _oAccounts.Update_Accounts(AccountName, Acc_ShortName, HeadAccounts_ID, subHeadAccount_ID, Sys_AC_type_ID, Opening_Balance, Opening_Bal_Date, Convert.ToInt32(UserID_Admin), Account_ID, c_ID);
                if (message == "Saved Successfully!")
                {


                    return RedirectToAction("chartofaccounts", "Accounts");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("chartofaccounts", "Accounts");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = "An error occured. Please try again!";
                return RedirectToAction("chartofaccount", "Accounts");
            }


        }

        #endregion


        [HttpGet]
        public IActionResult ApprovalList()
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);


            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            ViewBag.SectionHeaderTitle = "Approval List";
            ViewBag.SectionSub_HeaderTitle = "Approval List";
            ViewBag.PageTitle = "Approval List";
            _oAccounts.ApprovalObjList = _oAccounts.Get_ApprovalList_DAL(c_ID);

            return View(_oAccounts);


        }



        [HttpPost]
        public IActionResult DeleteAccounts(int Account_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            int c_ID = Convert.ToInt32(HttpContext.Session.GetString("c_ID"));
            #endregion
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Accounts", "chartofaccounts");
            }


            string url = Request.Headers["Referer"].ToString();



            if (Account_ID == 0)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }
            string message = "";


            try
            {
                message = _oAccounts.DeleteAccounts_DAL(Account_ID, c_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = message;
                    return RedirectToAction("chartofaccounts", "Accounts");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }


        }
        //---PAYMENT LIST 
        [HttpGet]
        public IActionResult PaymentsList(string PaymentRef, int Party_ID_Name, string StartDate, string EndDate, string PaidToNameText,
            string ChassisNo, string PurchaseRef, string VoucherType, int? c_ID, int? page, string Cheque_no)
        {

            //---Dumping static fields
            DumpStaticFieldsAccounts();

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Accounts";
            ViewBag.BreadCumAction = "PaymentsList";

            ViewBag.SectionHeaderTitle = "Accounts";
            ViewBag.SectionSub_HeaderTitle = "Payment List";
            ViewBag.PageTitle = "Payment List";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            #region Veiwbags area
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_IDs).ToList();
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;


            #endregion veiwbags area ends here
            //if(VoucherType == null)
            //{
            //    VoucherType = "GEN";
            //}
            PaymentRef = String.IsNullOrEmpty(PaymentRef) ? "" : PaymentRef;
            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            PaidToNameText = String.IsNullOrEmpty(PaidToNameText) ? "" : PaidToNameText;
            VoucherType = String.IsNullOrEmpty(VoucherType) ? "" : VoucherType;
            Cheque_no = String.IsNullOrEmpty(Cheque_no) ? "" : Cheque_no;

            Party_ID_Name = Party_ID_Name == 0 ? 0 : Party_ID_Name;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;

            c_ID = c_ID == null || c_ID == 0 ? c_IDs : c_ID;

            ViewBag.PaymentRef_Payments = PaymentRef;
            ViewBag.Party_ID_Name_Payments = Party_ID_Name;
            ViewBag.StartDate_Payments = StartDate;
            ViewBag.EndDate_Payments = EndDate;
            ViewBag.c_ID_Payments = c_ID;
            ViewBag.PaidToNameText_Payments = PaidToNameText;
            ViewBag.ChassisNo_Payments = ChassisNo;
            ViewBag.PurchaseRef_Payments = PurchaseRef;
            ViewBag.VoucherType_Payments = VoucherType;
            ViewBag.Cheque_no = Cheque_no;
            ViewBag.IsMultiCompanyEnabled = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyEnabled").Value;
            ViewBag.RecordsPerPage = 10;

            _oAccounts.paymentMasterListIPaged = _oAccounts.Get_PaymentMasterList_DAL(PaymentRef, Party_ID_Name, StartDate, EndDate,
                ChassisNo, PaidToNameText, PurchaseRef, VoucherType, Convert.ToInt32(c_ID), Cheque_no).ToList().ToPagedList(page ?? 1, 10);

            _oAccounts.PaymentMasterListTTL_obj = _oAccounts.Get_PaymentMasterList_TTL_DAL(PaymentRef, Party_ID_Name, StartDate, EndDate,
               ChassisNo, PaidToNameText, PurchaseRef, VoucherType, Convert.ToInt32(c_ID), Cheque_no).ToList();


            return View(_oAccounts);

        }


        //---PAYMENT LIST SEARCH FILTERS
        [HttpGet]
        public IActionResult GePaymentList_BySearchFitlers(string PaymentRef, int Party_ID_Name, string StartDate, string EndDate,
            string PaidToNameText, string ChassisNo, string PurchaseRef, string VoucherType, int c_ID, int? page, string Cheque_no)
        {

            //---Dumping static fields
            DumpStaticFieldsAccounts();


            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Accounts";
            ViewBag.BreadCumAction = "PaymentsList";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            //if (VoucherType == null)
            //{
            //    VoucherType = "GEN";
            //}
            #region Veiwbags area
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.Get_VendorMasterList_DAL(c_IDs).ToList();
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;



            #endregion veiwbags area ends here

            PaymentRef = String.IsNullOrEmpty(PaymentRef) ? "" : PaymentRef;
            ChassisNo = String.IsNullOrEmpty(ChassisNo) ? "" : ChassisNo;
            PurchaseRef = String.IsNullOrEmpty(PurchaseRef) ? "" : PurchaseRef;
            PaidToNameText = String.IsNullOrEmpty(PaidToNameText) ? "" : PaidToNameText;
            Cheque_no = String.IsNullOrEmpty(Cheque_no) ? "" : Cheque_no;
            //-- Fix by Jahangir
            VoucherType = String.IsNullOrEmpty(VoucherType) ? "" : VoucherType;
            //end


            Party_ID_Name = Party_ID_Name == 0 ? 0 : Party_ID_Name;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;
            c_ID = c_ID == 0 ? c_IDs : c_ID;
            ViewBag.PaymentRef_Payments = PaymentRef;
            ViewBag.Party_ID_Name_Payments = Party_ID_Name;
            ViewBag.StartDate_Payments = StartDate;
            ViewBag.EndDate_Payments = EndDate;
            ViewBag.c_ID_Payments = c_ID;
            ViewBag.PaidToNameText_Payments = PaidToNameText;
            ViewBag.ChassisNo_Payments = ChassisNo;
            ViewBag.PurchaseRef_Payments = PurchaseRef;
            ViewBag.VoucherType_Payments = VoucherType;
            ViewBag.Cheque_no = Cheque_no;
            ViewBag.IsMultiCompanyEnabled = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyEnabled").Value;
            try
            {
                //---payment master list for page

                ViewBag.RecordsPerPage = 10;

                _oAccounts.paymentMasterListIPaged = _oAccounts.Get_PaymentMasterList_DAL(PaymentRef, Party_ID_Name, StartDate, EndDate,
                    ChassisNo, PaidToNameText, PurchaseRef, VoucherType, Convert.ToInt32(c_ID), Cheque_no).ToList().ToPagedList(page ?? 1, 10);

                _oAccounts.PaymentMasterListTTL_obj = _oAccounts.Get_PaymentMasterList_TTL_DAL(PaymentRef, Party_ID_Name, StartDate, EndDate,
                ChassisNo, PaidToNameText, PurchaseRef, VoucherType, Convert.ToInt32(c_ID), Cheque_no).ToList();

                return PartialView("_Search_PaymentsList_Partial", _oAccounts);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }



        }





        //---get method of  Receipt Voucher Genenral
        [HttpGet]
        public IActionResult ReceiptVoucherGen(int? ReceiptMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "ReceiptVoucherGen";
            ViewBag.SectionHeaderTitle = "Accounts";
            ViewBag.SectionSub_HeaderTitle = "Reciept Voucher Gen";
            ViewBag.SectionSub_HeaderMainTitle = "Reciept Voucher";
            ViewBag.PageTitle = "Reciept Voucher Gen";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            #region ViewBag Area
            ViewBag.PartyType = _oBasic.get_PartyType_DAL().ToList();
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.get_PartyList_DAL(c_ID).ToList();

            //---Get Customer Master
            ViewBag.CustomerMaster = _oBasic.Get_CustomersMaster_DAL(c_ID).ToList();

            //---Get Accounts List
            ViewBag.CR_AccountsList = _oBasic.Select_RV_OnAccounts_DAL(c_ID).ToList();

            //---Get pay via list
            ViewBag.DR_AccountsList = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();
            
            
            ViewBag.DR_AccountsListRM = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();


            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;

            #endregion


            if (ReceiptMaster_ID == null || ReceiptMaster_ID < 1)
            {
                string ReceiptMasterGen_TempID = null;
                //---Create receipt master Ref(tem id)
                if (String.IsNullOrEmpty(ReceiptMasterGen_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    HttpContext.Session.SetString("ReceiptMasterGen_TempID", obj.ToString());
                }
                ReceiptMasterGen_TempID = HttpContext.Session.GetString("ReceiptMasterGen_TempID");
                if (string.IsNullOrEmpty(ReceiptMaster_ID.ToString()))
                {
                    ReceiptMaster_ID = 0;
                    HttpContext.Session.SetString("ReceiptMasterGenStatic_ID", ReceiptMaster_ID.ToString());
                }

                //---Get receipt master id
                ViewBag.ReceiptMaster_ID = ReceiptMaster_ID;

                //--Get receipt  Master
                _oAccounts.receiptMasterObject = _oAccounts.Get_ReceiptMasterGeneralByID_DAL(-1);

                //--Get receipt detail
                _oAccounts.receiptDetailList = _oAccounts.Get_ReceiptDetailGeneralListByID_DAL(ReceiptMasterGen_TempID, -1).ToList();
            }
            else
            {
                Urls = null;

                ApprovalURL = HttpContext.Request.Headers["Referer"];
                Urls = ApprovalURL;
                //---Get old temp id from receipt detail table that already creaded for this "ReceiptMaster_ID" id. And then use this if new record added
                string OldTempID = _oAccounts.GetOldTempIDFromReceiptDetail_DAL(ReceiptMaster_ID);




                HttpContext.Session.SetString("ReceiptMasterGenStatic_ID", ReceiptMaster_ID.ToString());

                //---Get receipt master id
                ViewBag.ReceiptMaster_ID = ReceiptMaster_ID;

                //--Get receipt Master
                _oAccounts.receiptMasterObject = _oAccounts.Get_ReceiptMasterGeneralByID_DAL(ReceiptMaster_ID);


                //--Get receipt detail
                _oAccounts.receiptDetailList = _oAccounts.Get_ReceiptDetailGeneralListByID_DAL(OldTempID, ReceiptMaster_ID).ToList();
            }



            return View(_oAccounts);
        }

        //---get method of  Receipt Voucher Sales
        [HttpGet]
        public IActionResult ReceiptVoucherSales(int? ReceiptMaster_ID)
        {
            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Sales";
            ViewBag.BreadCumAction = "ReceiptVoucherGen";
            ViewBag.SectionHeaderTitle = "Accounts";
            ViewBag.SectionSub_HeaderTitle = "Receipt Voucher";

            ViewBag.PageTitle = "Receipt Voucher (Sales)";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            #region ViewBag Area
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.get_PartyList_DAL(c_ID).ToList();
            //---Get Customer Master
            ViewBag.CustomerMaster = _oBasic.Get_CustomersMaster_DAL(c_ID).ToList();

            //---Get Accounts List
            ViewBag.CR_AccountsList = _oBasic.Select_RV_OnAccounts_DAL(c_ID).ToList();

            //---Get pay via list
            ViewBag.DR_AccountsList = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();
            ViewBag.DR_AccountsListRM = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();


            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;


            #endregion


            if (ReceiptMaster_ID == null || ReceiptMaster_ID < 1)
            {
                string ReceiptMasterSale_TempID = null;


                //---Create receipt master Ref(tem id)
                if (String.IsNullOrEmpty(ReceiptMasterSale_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    HttpContext.Session.SetString("ReceiptMasterSale_TempID", obj.ToString());
                }
                ReceiptMasterSale_TempID = HttpContext.Session.GetString("ReceiptMasterSale_TempID");
                HttpContext.Session.SetString("ReceiptMasterSaleStatic_ID", ReceiptMaster_ID.ToString());


                //---Get receipt master id
                ViewBag.ReceiptMaster_ID = ReceiptMaster_ID;

                //--Get receipt  Master
                _oAccounts.receiptMasterObject = _oAccounts.Get_ReceiptMasterSaleByID_DAL(-1);

                //--Get receipt detail
                _oAccounts.receiptDetailList = _oAccounts.Get_ReceiptDetailSaleListByID_DAL(ReceiptMasterSale_TempID, -1).ToList();
            }
            else
            {
                Urls = null;

                ApprovalURL = HttpContext.Request.Headers["Referer"];
                Urls = ApprovalURL;
                //---Get old temp id from receipt detail table that already creaded for this "ReceiptMaster_ID" id. And then use this if new record added
                string OldTempID = _oAccounts.GetOldTempIDFromReceiptDetail_DAL(ReceiptMaster_ID);




                HttpContext.Session.SetString("ReceiptMasterSaleStatic_ID", ReceiptMaster_ID.ToString());

                //---Get receipt master id
                ViewBag.ReceiptMaster_ID = ReceiptMaster_ID;

                //--Get receipt Master
                _oAccounts.receiptMasterObject = _oAccounts.Get_ReceiptMasterSaleByID_DAL(ReceiptMaster_ID);
                _oAccounts.ReceiptMasterObject = _oAccounts.Get_ReceiptMasterCustomerByID_DAL(ReceiptMaster_ID);

                //--Get receipt detail
                _oAccounts.receiptDetailList = _oAccounts.Get_ReceiptDetailSaleListByID_DAL(OldTempID, ReceiptMaster_ID).ToList();
            }



            return View(_oAccounts);
        }

        //---get method of  Receipt List
        [HttpGet]
        public IActionResult ReceiptList(string ReceiptRef, int Party_ID, string PartyNameText, string SaleRef, string Chassis_no, string StartDate, string EndDate, string Cheque_no, int c_ID, int? page)
        {
            //---Dumping static fields
            DumpStaticFieldsAccounts();

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Accounts";
            ViewBag.BreadCumAction = "ReceiptList";
            ViewBag.SectionHeaderTitle = "Accounts";
            ViewBag.SectionSub_HeaderTitle = "Receipt List";
            ViewBag.PageTitle = "Receipt List";
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            #region Veiwbags area
            //---Get Vendor Master
            ViewBag.CustomerMaster = _oBasic.Get_CustomersList_DAL(c_IDs).ToList();
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.IsMultiCompanyVisible = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyVisible").Value;


            #endregion veiwbags area ends here

            ReceiptRef = String.IsNullOrEmpty(ReceiptRef) ? "" : ReceiptRef;
            Party_ID = Party_ID == 0 ? 0 : Party_ID;
            PartyNameText = String.IsNullOrEmpty(PartyNameText) ? "" : PartyNameText;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            Chassis_no = String.IsNullOrEmpty(Chassis_no) ? "" : Chassis_no;
            Cheque_no = String.IsNullOrEmpty(Cheque_no) ? "" : Cheque_no;

            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;

            c_ID = c_ID == 0 ? c_IDs : c_ID;

            ViewBag.ReceiptRef_Receipt = ReceiptRef;
            ViewBag.Party_ID_Receipt = Party_ID;
            ViewBag.PartyNameText = PartyNameText;
            ViewBag.SaleRef = SaleRef;
            ViewBag.Chassis_no = Chassis_no;
            ViewBag.StartDate_Receipt = StartDate;
            ViewBag.EndDate_Receipt = EndDate;
            ViewBag.c_ID_Receipt = c_ID;
            ViewBag.Cheque_no = Cheque_no;
            ViewBag.IsMultiCompanyEnabled = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyEnabled").Value;
            ViewBag.RecordsPerPage = 10;


            //---receipt master list for page
            _oAccounts.receiptMasterIPagedList = _oAccounts.Get_ReceiptMaster_List_DAL(ReceiptRef, Party_ID, PartyNameText, SaleRef, Chassis_no, StartDate, Cheque_no, EndDate, c_ID).ToPagedList(page ?? 1, 10);

            _oAccounts.receiptMasterList_TTL = _oAccounts.Get_ReceiptMaster_List_TTL_DAL(ReceiptRef, Party_ID, PartyNameText, SaleRef, Chassis_no, StartDate, EndDate, Cheque_no, c_ID).ToList();


            return View(_oAccounts);

        }

        [HttpGet]
        public IActionResult GeReceiptList_BySearchFitlers(string ReceiptRef, int Party_ID, string PartyNameText, string SaleRef, string Chassis_no, string StartDate, string EndDate, string Cheque_no, int c_ID, int? page)
        {
            //---Dumping static fields
            DumpStaticFieldsAccounts();

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Accounts";
            ViewBag.BreadCumAction = "ReceiptList";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_IDs = Convert.ToInt32(company_ID);
            #region Veiwbags area
            //---Get Vendor Master
            ViewBag.CustomerMasterss = _oBasic.Get_CustomersList_DAL(c_IDs).ToList();
            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;


            #endregion veiwbags area ends here

            ReceiptRef = String.IsNullOrEmpty(ReceiptRef) ? "" : ReceiptRef;
            Party_ID = Party_ID == 0 ? 0 : Party_ID;
            PartyNameText = String.IsNullOrEmpty(PartyNameText) ? "" : PartyNameText;
            SaleRef = String.IsNullOrEmpty(SaleRef) ? "" : SaleRef;
            Chassis_no = String.IsNullOrEmpty(Chassis_no) ? "" : Chassis_no;
            Cheque_no = String.IsNullOrEmpty(Cheque_no) ? "" : Cheque_no;

            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = String.IsNullOrEmpty(EndDate) ? "" : EndDate;

            c_ID = c_ID == 0 ? c_IDs : c_ID;

            ViewBag.ReceiptRef_Receipt = ReceiptRef;
            ViewBag.Party_ID_Receipt = Party_ID;
            ViewBag.PartyNameText = PartyNameText;
            ViewBag.SaleRef = SaleRef;
            ViewBag.Chassis_no = Chassis_no;
            ViewBag.StartDate_Receipt = StartDate;
            ViewBag.EndDate_Receipt = EndDate;
            ViewBag.c_ID_Receipt = c_ID;
            ViewBag.Cheque_no = Cheque_no;
            ViewBag.IsMultiCompanyEnabled = configuration.GetSection("AppSettings").GetSection("IsMultiCompanyEnabled").Value;
            ViewBag.RecordsPerPage = 10;

            // return View();

            try
            {
                //---payment master list for page
                ViewBag.RecordsPerPage = 10;
                _oAccounts.receiptMasterIPagedList = _oAccounts.Get_ReceiptMaster_List_DAL(ReceiptRef, Party_ID, PartyNameText, SaleRef, Chassis_no, StartDate, EndDate, Cheque_no, c_ID).ToPagedList(page ?? 1, 10);

                _oAccounts.receiptMasterList_TTL = _oAccounts.Get_ReceiptMaster_List_TTL_DAL(ReceiptRef, Party_ID, PartyNameText, SaleRef, Chassis_no, StartDate, EndDate, Cheque_no, c_ID).ToList();

                return PartialView("_Search_Receiptist_Partial", _oAccounts);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = "An error occured in your searching. Please try again!" });
            }


        }


        //---chassis validation function
        [HttpGet]
        public JsonResult ValidateChassisNo(string ChassisNo)
        {
            string Stock_ID = "0";
            string Message = "";
            string Chassis_No = "";
            string Make = "";
            string ModelDesciption = "";
            string modelYear = "";
            string Color = "";

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);

            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function
            _oAccounts.ChassisValidationObj = _oAccounts.ValidateChassisNo_DAL(ChassisNo, c_ID);

            if (_oAccounts.ChassisValidationObj.Message == "Chassis Exists" && !String.IsNullOrEmpty(_oAccounts.ChassisValidationObj.Chassis_No))
            {
                Message = "Chassis is valid";
                Stock_ID = _oAccounts.ChassisValidationObj.Stock_ID;
                Chassis_No = _oAccounts.ChassisValidationObj.Chassis_No;
                Make = _oAccounts.ChassisValidationObj.Make;
                ModelDesciption = _oAccounts.ChassisValidationObj.ModelDesciption;
                modelYear = _oAccounts.ChassisValidationObj.ModelYear;
                Color = _oAccounts.ChassisValidationObj.Color;
            }
            else
            {
                Message = "Invalid Chassis";
                Stock_ID = null;
                Chassis_No = null;
                Make = null;
                ModelDesciption = null;
                modelYear = null;
                Color = null;
            }


            return Json(new { Message, Stock_ID, Chassis_No, Make, ModelDesciption, modelYear, Color });

        }

        public JsonResult ValidateBLNo(string BLNo)
        {
            string Stock_ID = "0";
            string Message = "";
            string Chassis_No = "";
            string Make = "";
            string ModelDesciption = "";
            string modelYear = "";
            string Color = "";

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);

            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function
            _oAccounts.ChassisValidationObj = _oAccounts.ValidateBLNO_DAL(BLNo, c_ID);

            if (_oAccounts.ChassisValidationObj.Message == "Chassis Exists" && !String.IsNullOrEmpty(_oAccounts.ChassisValidationObj.Chassis_No))
            {
                Message = "BL is valid";
                Stock_ID = _oAccounts.ChassisValidationObj.Stock_ID;
                Chassis_No = _oAccounts.ChassisValidationObj.Chassis_No;
                Make = _oAccounts.ChassisValidationObj.Make;
                ModelDesciption = _oAccounts.ChassisValidationObj.ModelDesciption;
                modelYear = _oAccounts.ChassisValidationObj.ModelYear;
                Color = _oAccounts.ChassisValidationObj.Color;
            }
            else
            {
                Message = "Invalid BL";
                Stock_ID = null;
                Chassis_No = null;
                Make = null;
                ModelDesciption = null;
                modelYear = null;
                Color = null;
            }


            return Json(new { Message, Stock_ID, Chassis_No, Make, ModelDesciption, modelYear, Color });

        }

        public JsonResult ValidateChassisNo_UNSOLD(string ChassisNo)
        {
            string Stock_ID = "0";
            string Message = "";
            string Chassis_No = "";
            string Make = "";
            string ModelDesciption = "";
            string modelYear = "";
            string Color = "";
            string sellingPrice = "";
            //string TotalPrice = "";

            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);


            //---Note: You can add more fields to this function but you cannot change the body. Because this function used in too many places.
            //---So please dont remove existing fields in this function
            _oAccounts.ChassisValidationObj = _oAccounts.ValidateChassisNo_UNSOLD_DAL(ChassisNo, c_ID);

            if (_oAccounts.ChassisValidationObj.Message == "Chassis Exists" && !String.IsNullOrEmpty(_oAccounts.ChassisValidationObj.Chassis_No))
            {
                Message = "Chassis is valid";
                Stock_ID = _oAccounts.ChassisValidationObj.Stock_ID;
                Chassis_No = _oAccounts.ChassisValidationObj.Chassis_No;
                Make = _oAccounts.ChassisValidationObj.Make;
                ModelDesciption = _oAccounts.ChassisValidationObj.ModelDesciption;
                modelYear = _oAccounts.ChassisValidationObj.ModelYear;
                Color = _oAccounts.ChassisValidationObj.Color;
                sellingPrice = _oAccounts.ChassisValidationObj.Selling_Price;
                //TotalPrice = _oAccounts.ChassisValidationObj.Total_Price;

            }
            else
            {
                Message = "Invalid Chassis";
                Stock_ID = null;
                Chassis_No = null;
                Make = null;
                ModelDesciption = null;
                modelYear = null;
                Color = null;
                sellingPrice = null;
                //TotalPrice = null;

            }


            return Json(new { Message, Stock_ID, Chassis_No, Make, ModelDesciption, modelYear, Color, sellingPrice});

        }
        [HttpGet]

        public JsonResult ValidateChassisNoJP(string ChassisNo)
        {
            int? Stock_ID = 0;
            string Message = "";

            pa_Vanning_Details chassisDetail = new pa_Vanning_Details();
            chassisDetail = _oAccounts.ValidateChassisNo_JP(ChassisNo);

            if (chassisDetail.result == "Chassis Exists")
            {
                Message = "Chassis is valid";
                Stock_ID = chassisDetail.Stock_ID;
            }
            else
            {
                Message = "Invalid Chassis";
                Stock_ID = null;
            }


            return Json(new { Message, Stock_ID });
        }


        //---insert into receipt detail general
        [HttpPost]
        public IActionResult InsertReceiptDetail_General(int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
            double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date, string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, int hdReceiptMaster_ID,int PDC_Recieving_Acc_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            string ReceiptMasterGen_TempID = HttpContext.Session.GetString("ReceiptMasterGen_TempID");
            Temp_ID = ReceiptMasterGen_TempID;
            if (hdReceiptMaster_ID == 0)
            {
                Temp_ID = ReceiptMasterGen_TempID;
            }
            else
            {
                Temp_ID = "0";
            }




            int DV_ID = 0;

            if (DR_accountID == null || Amount == null || CR_accountID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            try
            {
                message = _oAccounts.InsertReceiptDetail_General_DAL(DR_accountID, Amount, other_curr_amount, Currency_ID, Currency_Rate, CR_accountID,
                    Description, trans_ref, Cheque_Bank_Name, cheque_Date, Cheque_no, VAT_Rate, VAT_Exp, Total_Amount, Temp_ID, c_ID, Created_by, DV_ID, hdReceiptMaster_ID, PDC_Recieving_Acc_ID);
                if (message == "Saved Successfully!")
                {

                    _oAccounts.receiptDetailList = _oAccounts.Get_ReceiptDetailGeneralListByID_DAL(Temp_ID, hdReceiptMaster_ID).ToList();

                    return PartialView("_ReceiptDetailGenList_Partial", _oAccounts);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }

        //---Update into receipt detail general
        [HttpPost]
        public IActionResult UpdateReceiptDetail_General(int? ReceiptDetail_ID, int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
            double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date, string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, int hdReceiptMaster_ID, int PDC_Recieving_Acc_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            string ReceiptMasterGen_TempID = HttpContext.Session.GetString("ReceiptMasterGen_TempID");
            Temp_ID = ReceiptMasterGen_TempID;
            if (hdReceiptMaster_ID == 0)
            {
                Temp_ID = ReceiptMasterGen_TempID;
            }
            else
            {
                Temp_ID = "0";
            }


            if (ReceiptDetail_ID == null)
            {
                return Json(new { message = "Receipt detail id is null. Please try again!" });
            }

            if (DR_accountID == null || Amount == null || CR_accountID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            try
            {
                message = _oAccounts.UpdateReceiptDetail_General_DAL(ReceiptDetail_ID, DR_accountID, Amount, other_curr_amount, Currency_ID, Currency_Rate, CR_accountID,
                    Description, trans_ref, Cheque_Bank_Name, cheque_Date, Cheque_no, VAT_Rate, VAT_Exp, Total_Amount, Temp_ID, c_ID, Modified_by, PDC_Recieving_Acc_ID);
                if (message == "Updated Successfully!")
                {

                    _oAccounts.receiptDetailList = _oAccounts.Get_ReceiptDetailGeneralListByID_DAL(Temp_ID, hdReceiptMaster_ID).ToList();

                    return PartialView("_ReceiptDetailGenList_Partial", _oAccounts);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }



        //---Insert  receipt master general
        [HttpPost]
        public IActionResult InsertReceiptMaster_General(int? Party_ID, int? party_type, string NameText, string Explanation, string ReceiptDate)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            string ReceiptMasterGen_TempID = HttpContext.Session.GetString("ReceiptMasterGen_TempID");
            Temp_ID = ReceiptMasterGen_TempID;

            string Receipttype = "RVG";  //---It is the ref in table for new Payment master. Send this value hard coded from here

            if (Party_ID == null && NameText == null)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("ReceiptVoucherGen", "Accounts");
            }

            Party_ID = Party_ID == null ? 0 : Party_ID;
            party_type = party_type == null ? 0 : party_type;

            try
            {
                message = _oAccounts.Insert_ReceiptMasterGeneral_DAL(Party_ID, party_type, NameText, Explanation, Receipttype, ReceiptDate, Temp_ID, c_ID, Created_by);

                if (message.Contains("Saved Successfully"))
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("ReceiptList", "Accounts");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("ReceiptVoucherGen", "Accounts");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("ReceiptVoucherGen", "Accounts");
            }


        }

        //---udpate  receipt master general
        [HttpPost]
        public IActionResult UpdateReceiptMaster_General(int? ReceiptMaster_ID, int? Party_ID, int? party_type, string NameText, string Explanation, string ReceiptDate)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            string ReceiptMasterGen_TempID = HttpContext.Session.GetString("ReceiptMasterGen_TempID");
            Temp_ID = ReceiptMasterGen_TempID;
            string OldTempID = _oAccounts.GetOldTempIDFromReceiptDetail_DAL(ReceiptMaster_ID);

            if (!String.IsNullOrEmpty(OldTempID))
            {
                Temp_ID = OldTempID;
            }
            else
            {
                if (String.IsNullOrEmpty(Temp_ID))
                {
                    Guid obj = Guid.NewGuid();
                    Temp_ID = obj.ToString();
                }
            }
            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();


            if (Party_ID == null || String.IsNullOrEmpty(ReceiptDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            if (ReceiptMaster_ID == null)
            {
                TempData["Error"] = "Receipt ID is null. Please try again!";
                return Redirect(url);
            }



            Party_ID = Party_ID == null ? 0 : Party_ID;
            party_type = party_type == null ? 0 : party_type;


            try
            {
                message = _oAccounts.Update_ReceiptMasterGeneral_DAL(ReceiptMaster_ID, Party_ID, party_type, NameText, Explanation, ReceiptDate, Temp_ID, c_ID, Modified_by);

                if (message.Contains("Successfully"))
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("ReceiptList", "Accounts");
                }
                else
                {


                    TempData["Error"] = message;
                    return Redirect(url);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }



        }


        //---Method for deleting receipt master general
        [HttpPost]
        public IActionResult DeleteReceiptMaster_Gen(int? ReceiptMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion


            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();

            string message = "";

            if (ReceiptMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oAccounts.DeleteReceiptMaster_DAL(ReceiptMaster_ID);
                if (message == "Deleted Successfully!")
                {
                    return RedirectToAction("ReceiptList", "Accounts");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }


        //---Method for deleting receipt  detail gen
        [HttpPost]
        public IActionResult DeleteReceiptDetailGen(int? ReceiptDetail_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion
            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();

            string message = "";

            if (ReceiptDetail_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }



            try
            {
                message = _oAccounts.DeleteReceiptDetail_DAL(ReceiptDetail_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = message;
                    return Redirect(url);
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }



        //---Method for deleting payment master general
        [HttpPost]
        public IActionResult DeletePaymentMaster_Gen(int? PaymentMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion


            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();

            string message = "";

            if (PaymentMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oAccounts.DeletePaymentMaster_DAL(PaymentMaster_ID);
                if (message == "Deleted Successfully!")
                {
                    return RedirectToAction("PaymentsList", "Accounts");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }



        //---insert into receipt detail Sale
        [HttpPost]
        public IActionResult InsertReceiptDetail_Sale(int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
            double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date, string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, int hdReceiptMaster_ID, int PDC_Recieving_Acc_ID, string[] selectedIds, string[] UserselectedAmounts)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            string ReceiptMasterSale_TempID = HttpContext.Session.GetString("ReceiptMasterSale_TempID");
            Temp_ID = ReceiptMasterSale_TempID;
            if (hdReceiptMaster_ID == 0)
            {
                Temp_ID = ReceiptMasterSale_TempID;
            }
            else
            {
                Temp_ID = "0";
            }





            if (DR_accountID == null || Amount == null || CR_accountID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            try
            {
                message = _oAccounts.InsertReceiptDetail_Sale_DAL(DR_accountID, Amount, other_curr_amount, Currency_ID, Currency_Rate, CR_accountID,
                    Description, trans_ref, Cheque_Bank_Name, cheque_Date, Cheque_no, VAT_Rate, VAT_Exp, Total_Amount, Temp_ID, c_ID, Created_by, hdReceiptMaster_ID,  PDC_Recieving_Acc_ID);


                if (selectedIds != null && selectedIds.Length != 0)
                {
                    for (int i = 0; i < selectedIds.Length; i++)
                    {
                        _oAccounts.Insert_pa_trans_ref_Receipt_DAL(Created_by, Temp_ID, selectedIds[i], UserselectedAmounts[i], Currency_Rate.ToString());
                    }
                }
             

                    if (message == "Saved Successfully!")
                {

                    _oAccounts.receiptDetailList = _oAccounts.Get_ReceiptDetailSaleListByID_DAL(Temp_ID, hdReceiptMaster_ID).ToList();

                    return PartialView("_ReceiptDetail_SaleList_Partial", _oAccounts);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }

        //---Update into receipt detail Sale
        [HttpPost]
        public IActionResult UpdateReceiptDetail_Sale(int? ReceiptDetail_ID, int? DR_accountID, double? Amount, double? other_curr_amount, int? Currency_ID,
            double? Currency_Rate, int? CR_accountID, string Description, string trans_ref, string Cheque_Bank_Name, string cheque_Date, string Cheque_no, int? VAT_Rate, double? VAT_Exp, double? Total_Amount, int hdReceiptMaster_ID, int PDC_Recieving_Acc_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            string ReceiptMasterSale_TempID = HttpContext.Session.GetString("ReceiptMasterSale_TempID");
            Temp_ID = ReceiptMasterSale_TempID;
            if (hdReceiptMaster_ID == 0)
            {
                Temp_ID = ReceiptMasterSale_TempID;
            }
            else
            {
                Temp_ID = "0";
            }


            if (ReceiptDetail_ID == null)
            {
                return Json(new { message = "Receipt detail id is null. Please try again!" });
            }

            if (DR_accountID == null || Amount == null || CR_accountID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            try
            {
                message = _oAccounts.UpdateReceiptDetail_Sale_DAL(ReceiptDetail_ID, DR_accountID, Amount, other_curr_amount, Currency_ID, Currency_Rate, CR_accountID,
                    Description, trans_ref, Cheque_Bank_Name, cheque_Date, Cheque_no, VAT_Rate, VAT_Exp, Total_Amount, Temp_ID, c_ID, Modified_by, PDC_Recieving_Acc_ID);
                if (message == "Updated Successfully!")
                {

                    _oAccounts.receiptDetailList = _oAccounts.Get_ReceiptDetailSaleListByID_DAL(Temp_ID, hdReceiptMaster_ID).ToList();

                    return PartialView("_ReceiptDetail_SaleList_Partial", _oAccounts);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }



        //---Insert  receipt master Sale
        [HttpPost]
        public IActionResult InsertReceiptMaster_Sale(int? Party_ID, string NameText, string Explanation, string ReceiptDate)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            string ReceiptMasterSale_TempID = HttpContext.Session.GetString("ReceiptMasterSale_TempID");
            Temp_ID = ReceiptMasterSale_TempID;

            string Receipttype = "RVS";  //---It is the ref in table for new receipt master. Send this value hard coded from here

            if (Party_ID == null || String.IsNullOrEmpty(ReceiptDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("ReceiptVoucherSales", "Accounts");
            }

            Party_ID = Party_ID == null ? 0 : Party_ID;


            try
            {
                message = _oAccounts.Insert_ReceiptMasterSale_DAL(Party_ID, NameText, Explanation, Receipttype, ReceiptDate, Temp_ID, c_ID, Created_by);

                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("ReceiptList", "Accounts");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("ReceiptVoucherSales", "Accounts");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("ReceiptVoucherSales", "Accounts");
            }


        }

        //---udpate  receipt master Sale
        [HttpPost]
        public IActionResult UpdateReceiptMaster_Sale(int? ReceiptMaster_ID, int? Party_ID, string NameText, string Explanation, string ReceiptDate)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            string ReceiptMasterSale_TempID = HttpContext.Session.GetString("ReceiptMasterSale_TempID");
            Temp_ID = ReceiptMasterSale_TempID;
            string OldTempID = _oAccounts.GetOldTempIDFromReceiptDetail_DAL(ReceiptMaster_ID);

            if (!String.IsNullOrEmpty(OldTempID))
            {
                Temp_ID = OldTempID;
            }
            else
            {
                if (String.IsNullOrEmpty(Temp_ID))
                {
                    Guid obj = Guid.NewGuid();
                    Temp_ID = obj.ToString();
                }
            }
            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();


            if (Party_ID == null || String.IsNullOrEmpty(ReceiptDate))
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            if (ReceiptMaster_ID == null)
            {
                TempData["Error"] = "Receipt ID is null. Please try again!";
                return Redirect(url);
            }



            Party_ID = Party_ID == null ? 0 : Party_ID;


            try
            {
                message = _oAccounts.Update_ReceiptMasterSale_DAL(ReceiptMaster_ID, Party_ID, NameText, Explanation, ReceiptDate, Temp_ID, c_ID, Modified_by);

                if (message.Contains("Successfully"))
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("ReceiptList", "Accounts");
                }
                else
                {


                    TempData["Error"] = message;
                    return Redirect(url);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }



        }


        //---Method for deleting receipt master Sale
        [HttpPost]
        public IActionResult DeleteReceiptMaster_Sale(int? ReceiptMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion


            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();

            string message = "";

            if (ReceiptMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oAccounts.DeleteReceiptMaster_DAL(ReceiptMaster_ID);
                if (message == "Deleted Successfully!")
                {
                    return RedirectToAction("ReceiptList", "Accounts");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }


        //---Method for deleting receipt  detail Sale
        [HttpPost]
        public IActionResult DeleteReceiptDetailSale(int? ReceiptDetail_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion
            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();

            string message = "";

            if (ReceiptDetail_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }



            try
            {
                message = _oAccounts.DeleteReceiptDetail_DAL(ReceiptDetail_ID);
                if (message == "Deleted Successfully!")
                {
                    TempData["Success"] = message;
                    return Redirect(url);
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }



        #region PAYMENT VOUCHER - VENDOR

        //------------------------------------------------------------------------------------------------
        [HttpGet]
        public IActionResult NewPaymentVendor(int? PaymentMaster_ID)
        {

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Accounts";
            ViewBag.BreadCumAction = "NewPaymentVendor";
            ViewBag.SectionHeaderTitle = "Accounts";
            ViewBag.SectionSub_HeaderTitle = "Payment Voucher (Vendor)";
            ViewBag.PageTitle = "Payment Voucher (Vendor)";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            #region ViewBag Area
            ViewBag.PartyType = _oBasic.get_PartyType_DAL().ToList();
            //---Get Vendor Master
            ViewBag.VendorMaster = _oBasic.get_PartyList_DAL(c_ID).ToList();
            //---Get Accounts List
            ViewBag.DR_AccountsList = _oBasic.Select_PV_OnAccounts_DAL(c_ID).ToList();
            //---Get pay via list
            ViewBag.CR_accountIDList = _oBasic.Select_PV_PayVia_DAL(c_ID).ToList();
            //---Get currency list
            ViewBag.CurrencyList = _oBasic.Get_CurrencyMasterList_DAL(c_ID).ToList();

            ViewBag.DR_AccountsListBanks = _oBasic.Select_PV_OnAccounts_DAL(c_ID).Where(x => x.Sys_AC_type_ID == 13).ToList();

            ViewBag.AttachmentVisibility = configuration.GetSection("AppSettings").GetSection("AttachmentVisibility").Value;
            ViewBag.ApprovalVisibility = configuration.GetSection("AppSettings").GetSection("ApprovalVisibility").Value;
            ViewBag.IsJapanSpecsEnabled = configuration.GetSection("AppSettings").GetSection("IsJapanSpecsEnabled").Value;

            #endregion


            if (PaymentMaster_ID == null || PaymentMaster_ID < 1)
            {

                string PaymentMasterVendor_TempID = null;

                //---Create payment master Ref(tem id)
                if (String.IsNullOrEmpty(PaymentMasterVendor_TempID))
                {
                    Guid obj = Guid.NewGuid();

                    HttpContext.Session.SetString("PaymentMasterVendor_TempID", obj.ToString());
                }
                PaymentMasterVendor_TempID = HttpContext.Session.GetString("PaymentMasterVendor_TempID");
                if (string.IsNullOrEmpty(PaymentMaster_ID.ToString()))
                {
                    PaymentMaster_ID = 0;
                    HttpContext.Session.SetString("PaymentMasterVendorStatic_ID", PaymentMaster_ID.ToString());
                }

                //---Get payment master id
                ViewBag.PaymentMaster_ID = PaymentMaster_ID;

                //--Get payment Master
                _oAccounts.paymentMasterObject = _oAccounts.Get_PaymentMasterVendorByID_DAL(-1);

                //--Get payment detail
                _oAccounts.paymentDetailList = _oAccounts.Get_PaymentDetailVendorListByID_DAL(PaymentMasterVendor_TempID, -1).ToList();
            }
            else
            {
                Urls = null;

                PaymentURL = HttpContext.Request.Headers["Referer"];
                Urls = PaymentURL;

                //---Get old temp id from payment detail table that already creaded for this "PaymentMaster_ID" id. And then use this if new record added
                string OldTempID = _oAccounts.GetOldTempIDFromPaymentDetail_DAL(PaymentMaster_ID);




                HttpContext.Session.SetString("PaymentMasterVendorStatic_ID", PaymentMaster_ID.ToString());

                //---Get payment master id
                ViewBag.PaymentMaster_ID = PaymentMaster_ID;

                //--Get payment Master

                _oAccounts.paymentMasterObject = _oAccounts.Get_PaymentMasterVendorByID_DAL(PaymentMaster_ID);
                //--Get payment detail
                _oAccounts.paymentDetailList = _oAccounts.Get_PaymentDetailVendorListByID_DAL(OldTempID, PaymentMaster_ID).ToList();
            }



            return View(_oAccounts);
        }

        //---insert into payment detial Vendor
        [HttpPost]
        public IActionResult InsertPaymentDetail_Vendor(int? DR_accountID, double? Amount, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
            string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount, int? stock_ID, int hdPaymentMaster_ID, double TaxAmount, double ExtraCharges, double ExtraChargesTax, string[] selectedIds, string[] UserselectedAmounts,int PaymentDetails_ID, int PDC_Payable_Acc_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            string PaymentMasterVendor_TempID = HttpContext.Session.GetString("PaymentMasterVendor_TempID");
            Temp_ID = PaymentMasterVendor_TempID;
            if (hdPaymentMaster_ID == 0)
            {
                Temp_ID = PaymentMasterVendor_TempID;
            }
            else
            {
                Temp_ID = "0";
            }


            //---making stock_id 0 if null from page
            stock_ID = stock_ID == null ? 0 : stock_ID;

            if (DR_accountID == null && CR_accountID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            string IsJapanSpecsEnabled = configuration.GetSection("AppSettings").GetSection("IsJapanSpecsEnabled").Value;
            if (IsJapanSpecsEnabled == "true")
            {
                VAT_Exp = TaxAmount;
            }



            try
            {

                if (hdPaymentMaster_ID >0)
                {
                    message = _oAccounts.DeletePaymentDetail_byMasterID_DAL(hdPaymentMaster_ID);
                }

                    message = _oAccounts.InsertPaymentDetail_Vendor_DAL(DR_accountID, Amount, VAT_Rate, Currency_ID, Currency_Rate, Amount_Others, CR_accountID, Description,
             trans_ref, cheque_bank_name, cheque_date, cheque_no, VAT_Exp, Total_Amount, stock_ID, Temp_ID, c_ID, Created_by, hdPaymentMaster_ID, TaxAmount, ExtraCharges, ExtraChargesTax ,PDC_Payable_Acc_ID);



                if (selectedIds != null && selectedIds.Length != 0)
                {

                    for (int i = 0; i < selectedIds.Length; i++)
                    {
                        _oAccounts.Insert_pa_trans_ref_payment_DAL(Created_by, Temp_ID, selectedIds[i], UserselectedAmounts[i],Currency_Rate.ToString());
                    }

                }



                if (message.Contains("Successfully"))
                {

                    _oAccounts.paymentDetailList = _oAccounts.Get_PaymentDetailVendorListByID_DAL(Temp_ID, hdPaymentMaster_ID).ToList();




                    return PartialView("_PaymentDetailVendorList_Partial", _oAccounts);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }

        //---Get Purchasae refrences by amount 
        [HttpGet]
        public ActionResult Get_PurchaseRef_By_ID(string PaymentDetail_ID)
        {

            _oAccounts.PaymentBalanceList = _oAccounts.Get_Ref_Tran_Payment(PaymentDetail_ID);

            return PartialView("_PurchaseTranRefList", _oAccounts);
        }

        //---Update  payment detail Vendor
        [HttpPost]
        public IActionResult UpdatePaymentDetail_Vendor(int? PaymentDetails_ID, int? DR_accountID, double? AmountUpdate, int? VAT_Rate, int? Currency_ID, double? Currency_Rate, double? Amount_Others, int? CR_accountID, string Description,
             string trans_ref, string cheque_bank_name, string cheque_date, string cheque_no, double? VAT_Exp, double? Total_Amount,
             int? Stock_ID_HiddenUpdate, int hdPaymentMaster_ID, double ExtraCharges, double ExtraChargesTax, double TaxAmount,int PDC_Payable_Acc_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            string PaymentMasterVendor_TempID = HttpContext.Session.GetString("PaymentMasterVendor_TempID");
            Temp_ID = PaymentMasterVendor_TempID;
            if (hdPaymentMaster_ID == 0)
            {
                Temp_ID = PaymentMasterVendor_TempID;
            }
            else
            {
                Temp_ID = "0";
            }

            //---making stock_id 0 if null from page
            Stock_ID_HiddenUpdate = Stock_ID_HiddenUpdate == null ? 0 : Stock_ID_HiddenUpdate;

            if (PaymentDetails_ID == null || DR_accountID == null || AmountUpdate == null || CR_accountID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            string IsJapanSpecsEnabled = configuration.GetSection("AppSettings").GetSection("IsJapanSpecsEnabled").Value;
            if (IsJapanSpecsEnabled == "true")
            {
                VAT_Exp = TaxAmount;
            }
            try
            {
                message = _oAccounts.UpdatePaymentDetail_Vendor_DAL(PaymentDetails_ID, DR_accountID, AmountUpdate, VAT_Rate, Currency_ID, Currency_Rate, Amount_Others, CR_accountID, Description,
             trans_ref, cheque_bank_name, cheque_date, cheque_no, VAT_Exp, Total_Amount, Stock_ID_HiddenUpdate, Temp_ID, c_ID, Modified_by, TaxAmount, ExtraCharges, ExtraChargesTax, PDC_Payable_Acc_ID);


                //if (selectedIds != null && selectedIds.Length != 0)
                //{

                //    for (int i = 0; i < selectedIds.Length; i++)
                //    {
                //        _oAccounts.Insert_pa_trans_ref_payment_DAL(Modified_by, Temp_ID, selectedIds[i], UserselectedAmounts[i]);
                //    }

                //}





                if (message == "Updated Successfully!")
                {

                    _oAccounts.paymentDetailList = _oAccounts.Get_PaymentDetailVendorListByID_DAL(Temp_ID, hdPaymentMaster_ID).ToList();

                    return PartialView("_PaymentDetailVendorList_Partial", _oAccounts);

                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }

        }

        //---Delete  payment detail Vendor
        [HttpPost]
        public IActionResult DeletePaymentDetail_Vendor(int? PaymentDetails_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            string PaymentMasterVendor_TempID = HttpContext.Session.GetString("PaymentMasterVendor_TempID");
            int? PaymentMasterVendorStatic_ID = Convert.ToInt32(HttpContext.Session.GetString("PaymentMasterVendorStatic_ID"));
            Temp_ID = PaymentMasterVendor_TempID;
            string OldTempID = _oAccounts.GetOldTempIDFromPaymentDetail_DAL(PaymentMasterVendorStatic_ID);
            if (!String.IsNullOrEmpty(PaymentMasterVendor_TempID))
            {
                Temp_ID = PaymentMasterVendor_TempID;
            }
            else
            {
                Temp_ID = OldTempID;
            }

            if (PaymentDetails_ID == null)
            {
                return Json(new { message = "ID is null. Please try again!" });
            }

            try
            {
                message = _oAccounts.DeletePaymentDetail_DAL(PaymentDetails_ID);
                if (message == "Deleted Successfully!")
                {
                    _oAccounts.paymentDetailList = _oAccounts.Get_PaymentDetailVendorListByID_DAL(Temp_ID, PaymentMasterVendorStatic_ID).ToList();

                    return PartialView("_PaymentDetailVendorList_Partial", _oAccounts);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }

        }

        //---Insert  payment master general
        [HttpPost]
        public IActionResult InsertPaymentMaster_Vendor(int? Vendor_ID, int? party_type, string Explanation, string Date, string NameText)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            string PaymentMasterVendor_TempID = HttpContext.Session.GetString("PaymentMasterVendor_TempID");
            Temp_ID = PaymentMasterVendor_TempID;
            string PaymentType = "VEN";  //---It is the ref in table for new Payment master. Send this value hard coded from here

            if (Vendor_ID == null && NameText == null)
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewPaymentVendor", "Accounts");
            }

            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;
            party_type = party_type == null ? 0 : party_type;


            try
            {
                message = _oAccounts.Insert_PaymentMasterVendor_DAL(Vendor_ID, party_type, Explanation, PaymentType, Date, NameText, Temp_ID, c_ID, Created_by);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("PaymentsList", "Accounts");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("NewPaymentVendor", "Accounts");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("NewPaymentVendor", "Accounts");
            }


        }

        //---udpate  payment master Vendor
        [HttpPost]
        public IActionResult UdpatePaymentMaster_Vendor(int? PaymentMaster_ID, int? Vendor_ID, int? party_type, string Explanation, string Date, string NameText)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            string PaymentMasterVendor_TempID = HttpContext.Session.GetString("PaymentMasterVendor_TempID");
            Temp_ID = PaymentMasterVendor_TempID;
            string OldTempID = _oAccounts.GetOldTempIDFromPaymentDetail_DAL(PaymentMaster_ID);

            if (!String.IsNullOrEmpty(OldTempID))
            {
                Temp_ID = OldTempID;
            }
            else
            {
                if (String.IsNullOrEmpty(Temp_ID))
                {
                    Guid obj = Guid.NewGuid();
                    Temp_ID = obj.ToString();
                }
            }
            //Saving complete path(url) of page from where this ActionResult method called. This url will be used in return redirect
            string url = Request.Headers["Referer"].ToString();


            if (Vendor_ID == null && NameText == null)
            {
                TempData["Error"] = "Please fill all required fields!";
                return Redirect(url);
            }

            if (PaymentMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again!";
                return Redirect(url);
            }



            Vendor_ID = Vendor_ID == null ? 0 : Vendor_ID;

            party_type = party_type == null ? 0 : party_type;
            try
            {
                message = _oAccounts.Update_PaymentVendor_DAL(PaymentMaster_ID, Vendor_ID, party_type, Explanation, Date, NameText, Temp_ID, c_ID, Modified_by);

                if (message.Contains("Successfully"))
                {

                    //TempData["Success"] = "Updated Successfully!";

                    return RedirectToAction("PaymentsList", "Accounts");
                }
                else
                {


                    TempData["Error"] = message;
                    return Redirect(url);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }



        }

        //---Method for deleting payment master general
        [HttpPost]
        public IActionResult DeletePaymentMaster_Vendor(int? PaymentMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion


            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();

            string message = "";

            if (PaymentMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oAccounts.DeletePaymentMaster_DAL(PaymentMaster_ID);
                if (message == "Deleted Successfully!")
                {
                    return RedirectToAction("PaymentsList", "Accounts");
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }

        //---this method is for changing payment master status
        [HttpPost]
        public IActionResult ChangePaymentMasterStatus(int? PaymentMaster_ID, int? Status_ID, string Trans_Type)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion

            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();


            if (PaymentMaster_ID == null || Status_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                message = _oAccounts.ChangePaymentMasterStatus_DAL(PaymentMaster_ID, Status_ID, c_ID, Trans_Type, Modified_by);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = message;
                    return Redirect(Urls);
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }


        //---this method is for changing receipt master status
        [HttpPost]
        public IActionResult ChangeReceiptMasterStatus(int? ReceiptMaster_ID, int? Status_ID, string Trans_Type)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");

            }
            #endregion

            string message = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            //----getting calling page url
            string url = Request.Headers["Referer"].ToString();


            if (ReceiptMaster_ID == null || Status_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again";
                return Redirect(url);
            }

            try
            {
                string QRName = QRGenerator(ReceiptMaster_ID,c_ID.ToString());
                message = _oAccounts.ChangeReceiptMasterStatus_DAL(ReceiptMaster_ID, Status_ID, c_ID, Trans_Type, Modified_by, QRName);
                if (message.Contains("Successfully"))
                {
                    TempData["Success"] = message;
                    return Redirect(Urls);
                }
                else
                {
                    TempData["Error"] = message;
                    return Redirect(url);
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return Redirect(url);
            }

        }



        //---Get payment  attachment list 
        [HttpGet]
        public IActionResult GetPaymentMaster_Attachments(int? PaymentMaster_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            string Type = "Accounts_PaymentVoucher";

            try
            {
                _oAccounts.attachmentList = _oAccounts.GetPaymentsMaster_Attachments_DAL(PaymentMaster_ID, Type).ToList();

                return PartialView("_PaymentAttachment", _oAccounts);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }
        }


        #endregion 

        //Insert Attachment method of Payments other
        [HttpPost]
        public IActionResult InsertAttachments_Payments(int? PaymentMaster_ID, string Type, string Document_Type, IFormFile Attachment)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            if (PaymentMaster_ID == null || String.IsNullOrEmpty(Document_Type) || Attachment == null)
            {
                //TempData["Error"] = "Please fill all required fields!";
                return Json(new { message = "Please fill all required fields!" });
            }

            var newFileName = "";
            var filepath = "";
            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            #region saving file
            if (Attachment != null)
            {
                if (Attachment.Length > 0)
                {
                    try
                    {

                        //string Image = Path.Combine(Server.MapPath(filePath), imgname);


                        if (Attachment.Length > 0)
                        {
                            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                            //Getting FileName
                            var fileName = Path.GetFileName(Attachment.FileName);

                            //Assigning Unique Filename (Guid)
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                            //Getting file Extension
                            var fileExtension = Path.GetExtension(fileName);

                            // concatenating  FileName + FileExtension
                            newFileName = String.Concat(myUniqueFileName, fileExtension);

                            // Combines two strings into a path.

                            filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{newFileName}";

                            using (FileStream fs = System.IO.File.Create(filepath))
                            {
                                Attachment.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        return Json(new { message = ErrorMessage });
                    }

                }

            }


            #endregion

            //---saving attachment in database
            try
            {

                message = _oAccounts.Insert_AttachmentPayment_DAL(PaymentMaster_ID, Document_Type, Type, newFileName, filepath, Created_by);
                if (message == "Saved Successfully!")
                {

                    _oAccounts.attachmentList = _oAccounts.GetPaymentsMaster_Attachments_DAL(PaymentMaster_ID, Type).ToList();

                    return PartialView("_PaymentAttachment", _oAccounts);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                return Json(new { message = ErrorMessage });
            }

            //return View();
        }

        //---this method is for deleting attachment in payment list
        [HttpGet]
        public IActionResult DeleteAttachmentPayment(int? PaymentMaster_ID, int? Attachment_ID, string Type, string File_Name)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            string message = "";

            if (PaymentMaster_ID == null)
            {
                //TempData["Error"] = "Please fill all required fields!";
                return Json(new { message = "Sales master id is null" });
            }

            if (Attachment_ID == null)
            {

                return Json(new { message = "Attachment_ID is null" });
            }
            if (String.IsNullOrEmpty(Type))
            {

                return Json(new { message = "Document type is null" });
            }


            #region Deleting actual file from folder
            try
            {
                string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                string filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{File_Name}";

                if ((System.IO.File.Exists(filepath)))
                {
                    System.IO.File.Delete(filepath);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });

            }
            #endregion





            try
            {

                message = _oAccounts.Delete_Attachment_Payment(Attachment_ID);
                if (message == "Deleted Successfully!")
                {

                    _oAccounts.attachmentList = _oAccounts.GetPaymentsMaster_Attachments_DAL(PaymentMaster_ID, Type).ToList();

                    return PartialView("_PaymentAttachment", _oAccounts);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                return Json(new { message = ErrorMessage });
            }

            //return View();
        }

        //---Get payment  attachment list 
        [HttpGet]
        public IActionResult GetReceiptMaster_Attachments(int? ReceiptMaster_ID)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            string Type = "Accounts_ReceiptVoucher";

            try
            {
                _oAccounts.attachmentList = _oAccounts.GetReceiptMaster_Attachments_DAL(ReceiptMaster_ID, Type).ToList();

                return PartialView("_ReceiptAttachment", _oAccounts);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }
        }

        //Insert Attachment method of Payments other
        [HttpPost]
        public IActionResult InsertAttachments_Receipts(int? ReceiptMaster_ID, string Type, string Document_Type, IFormFile Attachment)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            if (ReceiptMaster_ID == null || String.IsNullOrEmpty(Document_Type) || Attachment == null)
            {

                return Json(new { message = "Please fill all required fields!" });
            }

            var newFileName = "";
            var filepath = "";
            string message = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);


            #region saving file
            if (Attachment != null)
            {
                if (Attachment.Length > 0)
                {
                    try
                    {

                        //string Image = Path.Combine(Server.MapPath(filePath), imgname);


                        if (Attachment.Length > 0)
                        {
                            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                            //Getting FileName
                            var fileName = Path.GetFileName(Attachment.FileName);

                            //Assigning Unique Filename (Guid)
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                            //Getting file Extension
                            var fileExtension = Path.GetExtension(fileName);

                            // concatenating  FileName + FileExtension
                            newFileName = String.Concat(myUniqueFileName, fileExtension);

                            // Combines two strings into a path.

                            filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{newFileName}";

                            using (FileStream fs = System.IO.File.Create(filepath))
                            {
                                Attachment.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        return Json(new { message = ErrorMessage });
                    }

                }

            }


            #endregion

            //---saving attachment in database
            try
            {

                message = _oAccounts.Insert_AttachmentReceipt_DAL(ReceiptMaster_ID, Document_Type, Type, newFileName, filepath, Created_by);
                if (message == "Saved Successfully!")
                {

                    _oAccounts.attachmentList = _oAccounts.GetReceiptMaster_Attachments_DAL(ReceiptMaster_ID, Type).ToList();

                    return PartialView("_ReceiptAttachment", _oAccounts);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                return Json(new { message = ErrorMessage });
            }

            //return View();
        }

        //---this method is for deleting attachment in payment list
        [HttpGet]
        public IActionResult DeleteAttachmentReceipts(int? ReceiptMaster_ID, int? Attachment_ID, string Type, string File_Name)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            string message = "";

            if (ReceiptMaster_ID == null)
            {
                //TempData["Error"] = "Please fill all required fields!";
                return Json(new { message = "Sales master id is null" });
            }

            if (Attachment_ID == null)
            {

                return Json(new { message = "Attachment_ID is null" });
            }
            if (String.IsNullOrEmpty(Type))
            {

                return Json(new { message = "Document type is null" });
            }



            #region Deleting actual file from folder
            try
            {
                string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
                string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
                string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;

                string filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{File_Name}";

                if ((System.IO.File.Exists(filepath)))
                {
                    System.IO.File.Delete(filepath);
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });

            }
            #endregion





            try
            {

                message = _oAccounts.Delete_Attachment_Payment(Attachment_ID);
                if (message == "Deleted Successfully!")
                {

                    _oAccounts.attachmentList = _oAccounts.GetReceiptMaster_Attachments_DAL(ReceiptMaster_ID, Type).ToList();

                    return PartialView("_ReceiptAttachment", _oAccounts);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                return Json(new { message = ErrorMessage });
            }

            //return View();
        }





        //----dumping static ids, and temp fields

        //----dumping static ids, and temp fields
        public void DumpStaticFieldsAccounts()
        {
            HttpContext.Session.Remove("PaymentMasterGeneral_TempID");
            HttpContext.Session.Remove("PaymentMasterGeneralStatic_ID");
            HttpContext.Session.Remove("ReceiptMasterGen_TempID");
            HttpContext.Session.Remove("ReceiptMasterGenStatic_ID");
            HttpContext.Session.Remove("ReceiptMasterSale_TempID");
            HttpContext.Session.Remove("ReceiptMasterSaleStatic_ID");
            HttpContext.Session.Remove("PaymentMasterVendor_TempID");
            HttpContext.Session.Remove("PaymentMasterVendorStatic_ID");




            JVMaster_TempID = null;
            JVMasterStatic_ID = null;
        }

        #region Journal Voucher




        //---get method of New Journal Voucher
        [HttpGet]
        public IActionResult NewJournalVoucher(int? JVMaster_ID)
        {

            //---BreadCrumbs defintion
            ViewBag.BreadCumController = "Accounts";
            ViewBag.BreadCumAction = "NewJournalVoucher";
            ViewBag.SectionHeaderTitle = "Accounts";
            ViewBag.SectionSub_HeaderTitle = "Journal Voucher";
            ViewBag.SectionSub_HeaderMainTitle = "Journal Voucher List";
            ViewBag.PageTitle = "Journal Voucher";

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion

            #region ViewBag Area

            ViewBag.DR_AccountsList = _oBasic.Select_PV_OnAccounts_DAL(c_ID).ToList();



            #endregion


            if (JVMaster_ID == null || JVMaster_ID < 1)
            {



                //---Create payment master Ref(tem id)
                if (String.IsNullOrEmpty(JVMaster_TempID))
                {
                    Guid obj = Guid.NewGuid();
                    JVMaster_TempID = obj.ToString();
                }

                JVMasterStatic_ID = JVMaster_ID;

                //---Get payment master id
                ViewBag.JVMaster_ID = JVMaster_ID;

                //--Get payment Master
                _oAccounts.JournalVoucherMasterObject = _oAccounts.Get_JournalVoucherMaster_Id(-1);

                //--Get payment detail
                _oAccounts.JournalVoucherDetailList = _oAccounts.Get_JournalVoucher_DetailListByID_DAL(-1, JVMaster_TempID).ToList();
            }
            else
            {

                //---Get old temp id from payment detail table that already creaded for this "JVMaster_ID" id. And then use this if new record added
                string OldTempID = _oAccounts.GetOldTempIDFromJVDetail_DAL(JVMaster_ID);

                if (!String.IsNullOrEmpty(OldTempID))
                {
                    JVMaster_TempID = OldTempID;
                }
                else
                {
                    if (String.IsNullOrEmpty(JVMaster_TempID))
                    {
                        Guid obj = Guid.NewGuid();
                        JVMaster_TempID = obj.ToString();
                    }
                }


                JVMasterStatic_ID = JVMaster_ID;

                //---Get payment master id
                ViewBag.JVMaster_ID = JVMaster_ID;

                //--Get payment Master

                _oAccounts.JournalVoucherMasterObject = _oAccounts.Get_JournalVoucherMaster_Id(JVMaster_ID);
                //--Get payment detail
                _oAccounts.JournalVoucherDetailList = _oAccounts.Get_JournalVoucher_DetailListByID_DAL(JVMaster_ID, JVMaster_TempID).ToList();
            }



            return View(_oAccounts);
        }
        //---insert into jv detial 
        [HttpPost]
        public IActionResult InsertJVDetail(int? Account_ID, double? DR_Amount, double? CR_Amount, string Description, int? Party_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion


            string message = "";
            string Temp_ID = "";
            string Date = DateTime.Now.ToShortDateString();
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);

            Temp_ID = JVMaster_TempID;
            JVMasterStatic_ID = JVMasterStatic_ID == null ? -1 : JVMasterStatic_ID;

            Party_ID = Party_ID == null ? 0 : Party_ID;
            DR_Amount = DR_Amount == null ? 0 : DR_Amount;
            CR_Amount = CR_Amount == null ? 0 : CR_Amount;

            if (Account_ID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            try
            {
                message = _oAccounts.Insert_JournalVoucherDetail_DAL(Date, Account_ID, DR_Amount, CR_Amount, Description, Party_ID, Temp_ID, Created_by);
                if (message == "Saved Successfully!")
                {

                    _oAccounts.JournalVoucherDetailList = _oAccounts.Get_JournalVoucher_DetailListByID_DAL(JVMasterStatic_ID, Temp_ID).ToList();

                    return PartialView("_JournalVoucherPartialView", _oAccounts);
                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }


        }

        //---Update  jv detail 
        [HttpPost]
        public IActionResult UpdateJVDetail(int? JVDetails_ID, int? Account_ID, double? DR_Amount, double? CR_Amount, string Description, int? Party_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return Json(new { message = "Session expired! Please login again!" });
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            string Date = DateTime.Now.ToShortDateString();

            Temp_ID = JVMaster_TempID;
            JVMasterStatic_ID = JVMasterStatic_ID == null ? -1 : JVMasterStatic_ID;
            Party_ID = Party_ID == null ? 0 : Party_ID;
            DR_Amount = DR_Amount == null ? 0 : DR_Amount;
            CR_Amount = CR_Amount == null ? 0 : CR_Amount;

            if (JVDetails_ID == null || Account_ID == null)
            {
                return Json(new { message = "Please fill all required fields!" });
            }

            try
            {
                message = _oAccounts.Update_JournalVoucher_detail_DAL(JVDetails_ID, Date, Account_ID, DR_Amount, CR_Amount, Description, Party_ID, Modified_by);
                if (message == "Saved Successfully!")
                {

                    _oAccounts.JournalVoucherDetailList = _oAccounts.Get_JournalVoucher_DetailListByID_DAL(JVMasterStatic_ID, Temp_ID).ToList();

                    return PartialView("_JournalVoucherPartialView", _oAccounts);

                }
                else
                {
                    return Json(new { message = message });
                }

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return Json(new { message = ErrorMessage });
            }

        }


        [HttpPost]
        public IActionResult InsertJVMaster(string date)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Created_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);
            Temp_ID = JVMaster_TempID;


            if (String.IsNullOrEmpty(date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewJournalVoucher", "Accounts");
            }




            try
            {
                message = _oAccounts.Insert_JournalVoucherMaster_DAL(date, c_ID, Temp_ID, Created_by);
                if (message == "Saved Successfully!")
                {

                    //TempData["Success"] = "Saved Successfully!";
                    return RedirectToAction("JournalVoucherList", "Accounts");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("NewJournalVoucher", "Accounts");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("NewJournalVoucher", "Accounts");
            }


        }

        //---udpate  jv master 
        [HttpPost]
        public IActionResult UdpateJVMaster(string date, int? JVMaster_ID)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
            string Temp_ID = "";
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int? c_ID = Convert.ToInt32(company_ID);





            if (String.IsNullOrEmpty(date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("NewJournalVoucher", "Accounts");
            }

            if (JVMaster_ID == null)
            {
                TempData["Error"] = "ID is null. Please try again!";
                return RedirectToAction("NewJournalVoucher", "Accounts");
            }






            try
            {
                message = _oAccounts.Update_JournalVoucher_DAL(date, JVMaster_ID, Modified_by);

                if (message == "Update Successfully!")
                {

                    //TempData["Success"] = "Updated Successfully!";

                    return RedirectToAction("JournalVoucherList", "Accounts");
                }
                else
                {


                    TempData["Error"] = message;
                    return RedirectToAction("NewJournalVoucher", "Accounts");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("NewJournalVoucher", "Accounts");
            }



        }





        //---get method of Journal Voucher List
        [HttpGet]
        public IActionResult JournalVoucherList()
        {

            ViewBag.SectionHeaderTitle = "Accounts";
            ViewBag.SectionSub_HeaderTitle = "Journal Voucher List";

            ViewBag.PageTitle = "Journal Voucher List";
            //---Dumping static fields
            DumpStaticFieldsAccounts();

            _oAccounts.JournalVoucherMasterList = _oAccounts.Get_JournalVoucher_master_DAL();
            return View(_oAccounts);
        }

        [HttpGet]
        public IActionResult PDCReport(string StartDate, string Enddate, string cheque_no, string Ref_ID, int? page)
        {

            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            int? user_id = Convert.ToInt32(UserID_Admin);
            int c_ID = Convert.ToInt32(company_ID);

            string Msg = "";

            ViewBag.SectionHeaderTitle = "Accounts";
            ViewBag.SectionSub_HeaderTitle = "PDCReport";

            ViewBag.PageTitle = "PDCReport";
            //---Dumping static fields
            DumpStaticFieldsAccounts();


            Ref_ID = String.IsNullOrEmpty(Ref_ID) ? "" : Ref_ID;
            cheque_no = String.IsNullOrEmpty(cheque_no) ? "" : cheque_no;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            Enddate = String.IsNullOrEmpty(Enddate) ? "" : Enddate;
            ViewBag.DR_AccountsListBanks = _oBasic.Select_PV_OnAccounts_DAL(c_ID).Where(x => x.Sys_AC_type_ID == 13).ToList();


            ViewBag.PDCRef_ID = Ref_ID;
            
            ViewBag.PDCStartDate = StartDate;
            ViewBag.PDCEndDate = Enddate;
            
            ViewBag.PDCcheque_no = cheque_no;


            //if (StartDate == null && Enddate == null)
            //{
            //    StartDate = "";
            //    Enddate = "";
            //}
            try
            {

                _oAccounts.PDCReportPageList = _oAccounts.Get_PDCReport_DAL(StartDate, Enddate, cheque_no, Ref_ID,c_ID).ToPagedList(page ?? 1, 10);


                
            }
            catch (Exception ex)
            {

                string ErrorMessage = ex.Message;

            }



            return View(_oAccounts);
        }

        [HttpGet]
        public IActionResult PDCReportBySearch(string StartDate, string Enddate, string cheque_no, string Ref_ID, int? page)
        {


            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion


            int? user_id = Convert.ToInt32(UserID_Admin);
            int c_ID = Convert.ToInt32(company_ID);

            ViewBag.SectionHeaderTitle = "Accounts";
            ViewBag.SectionSub_HeaderTitle = "PDCReport";

            ViewBag.PageTitle = "PDCReport";
            //---Dumping static fields
            DumpStaticFieldsAccounts();
            ViewBag.DR_AccountsListBanks = _oBasic.Select_PV_OnAccounts_DAL(c_ID).Where(x => x.Sys_AC_type_ID == 13).ToList();



            Ref_ID = String.IsNullOrEmpty(Ref_ID) ? "" : Ref_ID;
            cheque_no = String.IsNullOrEmpty(cheque_no) ? "" : cheque_no;
            StartDate = String.IsNullOrEmpty(StartDate) ? "" : StartDate;
            Enddate = String.IsNullOrEmpty(Enddate) ? "" : Enddate;



            //if (StartDate == null && Enddate == null)
            //{
            //    StartDate = "";
            //    Enddate = "";
            //}



            ViewBag.PDCRef_ID = Ref_ID;
            ViewBag.PDCStartDate = StartDate;
            ViewBag.PDCEndDate = Enddate;
            ViewBag.PDCcheque_no = cheque_no;

        


            try
            {

                _oAccounts.PDCReportPageList = _oAccounts.Get_PDCReport_DAL(StartDate, Enddate, cheque_no,Ref_ID, c_ID).ToPagedList(page ?? 1, 10);
                
                return PartialView("_PDCReportSearchPartail", _oAccounts);

            }
            catch (Exception ex)
            {

                throw;
            }


        }

        #endregion
        [HttpGet]
        public ActionResult GetPartyBalance_By_VendorID(string Vendor_ID,int PaymentMaster_ID,int PaymentDetails_ID)
        {
             ViewBag.PaymentMaster_ID = PaymentMaster_ID;

            _oAccounts.paymentMasterObject = _oAccounts.Get_PaymentMasterVendorByID_DAL(PaymentMaster_ID);
            _oAccounts.PaymentBalanceList = _oAccounts.pa_GetParty_Balance(Vendor_ID, PaymentMaster_ID).ToList();


            return PartialView("_PaymentBalance", _oAccounts);
        }
        [HttpGet]
        public ActionResult GetPartyBalanceReceipt_By_CustomerID(string CustomerID, int ReceiptMaster_ID)
        {

            string C_id = HttpContext.Session.GetString("c_ID");
            ViewBag.ReceiptMaster_ID = ReceiptMaster_ID;

            _oAccounts.ReceiptMasterObject = _oAccounts.Get_ReceiptMasterCustomerByID_DAL(ReceiptMaster_ID);
            _oAccounts.receiptmasterlist = _oAccounts.pa_GetParty_Balance_Receipt(CustomerID, ReceiptMaster_ID, Convert.ToInt32(C_id)).ToList();


            return PartialView("_ReceiptBalance", _oAccounts);
        }

        //this method is for getting vendor name according to Vandorcategory
        public JsonResult Get_VendorName_By_VendorCategory(int party_type)
        {
            #region CompanyID
            string company_ID = HttpContext.Session.GetString("c_ID");
            int c_ID = Convert.ToInt32(company_ID);
            #endregion
            List<Pa_Partners_DAL> vendor = new List<Pa_Partners_DAL>();


            if (party_type < 0 || party_type == 0)
            {
                vendor = _oAccounts.Get_tblaccounts_VendorName(0, c_ID).ToList();
            }


            vendor = _oAccounts.Get_tblaccounts_VendorName(party_type, c_ID).ToList();

            return Json(new { vendor });

        }

        public JsonResult Get_Party()
        {

            List<PartyType_DAL> item = new List<PartyType_DAL>();

            item = _oAccounts.Get_Party_Type_By_Name().ToList();

            return Json(new { item });




        }

        #region QRGENERATOR
        //public static string EncryptString(string key, string plainText)
        //{
        //    byte[] iv = new byte[16];
        //    byte[] array;

        //    using (Aes aes = Aes.Create())
        //    {
        //        aes.Key = Encoding.UTF8.GetBytes(key);
        //        aes.IV = iv;

        //        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        //        using (MemoryStream memoryStream = new MemoryStream())
        //        {
        //            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
        //            {
        //                using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
        //                {
        //                    streamWriter.Write(plainText);
        //                }

        //                array = memoryStream.ToArray();
        //            }
        //        }
        //    }

        //    return Convert.ToBase64String(array);
        //}
        public string QRGenerator(int? ReceiptMaster_ID,string c_ID)
        {
            string url = configuration.GetSection("AppSettings").GetSection("ReceiptURL").Value;
            ReceiptMaster_ID = (ReceiptMaster_ID == null ? 0 : ReceiptMaster_ID);
            var writer = new QRCodeWriter();
            //generate QR Code
            //var key = "b14ca5898a4e4133bbce2ea2315a1916";
            //var encryptedString = EncryptString(key, ReceiptMaster_ID.ToString());
            var resultBit = writer.encode(url + ReceiptMaster_ID+ "&c_id="+c_ID, BarcodeFormat.QR_CODE, 75, 75);
            //get Bitmatrix result
            var matrix = resultBit;

            //convert bitmatrix into image 
            int scale = 2;

            Bitmap result = new Bitmap(matrix.Width * scale, matrix.Height * scale);
            for (int x = 0; x < matrix.Height; x++)
                for (int y = 0; y < matrix.Width; y++)
                {
                    Color pixel = matrix[x, y] ? Color.Black : Color.White;
                    for (int i = 0; i < scale; i++)
                        for (int j = 0; j < scale; j++)
                            result.SetPixel(x * scale + i, y * scale + j, pixel);
                }
            string filepath = "";
            string imgname = "";
            #region
            int length = 4;
            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }

            //appending str_build text to image name
            imgname = str_build.ToString() + "_" + ReceiptMaster_ID + "QR.png";
            #endregion

            //get wwwroot folder location
            string RootFolder = configuration.GetSection("FilePathToImages").GetSection("RootFolder").Value;
            string CommonImages_Level_1 = configuration.GetSection("FilePathToImages").GetSection("CommonImages_Level_1").Value;
            string OtherImages_Level_2 = configuration.GetSection("FilePathToImages").GetSection("OtherImages_Level_2").Value;


            filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), RootFolder, CommonImages_Level_1, OtherImages_Level_2)).Root + $@"\{imgname}";

            //save result as png inside 'Images' folder
            result.Save(filepath);
            return imgname;
        }

        #endregion
    
        
        //PDC update
        [HttpPost]
        public IActionResult UpdatePDC(string Post_Date, int Post_Master_ID,string Post_vType)
        {
            #region login checking
            string UserName_Admin = HttpContext.Session.GetString("UserName");
            string UserID_Admin = HttpContext.Session.GetString("UserID");
            string company_ID = HttpContext.Session.GetString("c_ID");
            if (String.IsNullOrEmpty(UserName_Admin) || String.IsNullOrEmpty(UserID_Admin))
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion

            string message = "";
       
            int? Modified_by = Convert.ToInt32(UserID_Admin);
            int c_ID = Convert.ToInt32(company_ID);
            int User_ID = Convert.ToInt32(UserID_Admin);





            if (String.IsNullOrEmpty(Post_Date))
            {
                TempData["Error"] = "Please fill all required fields!";
                return RedirectToAction("PDCReport", "Accounts");
            }

            if (Post_Master_ID == 0)
            {
                TempData["Error"] = "ID is null. Please try again!";
                return RedirectToAction("PDCReport", "Accounts");
            }


            try
            {
                message = _oAccounts.Update_Post_PDC_DAL(Post_Date, Post_Master_ID, User_ID, Post_vType, c_ID);

                if (message == "Posted Successfully")
                {

                    TempData["Success"] = "Posted Successfully";

                    return RedirectToAction("PDCReport", "Accounts");
                }
                else
                {


                    TempData["Error"] = message;
                    return RedirectToAction("PDCReport", "Accounts");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                TempData["Error"] = ErrorMessage;
                return RedirectToAction("PDCReport", "Accounts");
            }



        }


    }
}