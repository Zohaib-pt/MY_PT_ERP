using DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using X.PagedList;

namespace DAL.oClasses
{
    public class OInventory : IOInventory
    {
        private readonly IConnectionStringManager _conMgr;
        private readonly ISqlHelper dbLayer;
        SqlConnection con;

        public OInventory(IConnectionStringManager conMgr, ISqlHelper sqlHelp)
        {

            _conMgr = conMgr;
            dbLayer = sqlHelp;
            //---Getting Connection String
            Constr = _conMgr.GetConnectionString();
        }
        public string Constr { get; set; }

        public pa_ProductFormulaDetails ProductFormulaDetailObj { get; set; }
        public IEnumerable<pa_ProductFormulaDetails> ProductFormulaDetailList { get; set; }
        public pa_ProductFormulaMaster_DAL ProductFormulaMasterObj { get; set; }
        public IPagedList<pa_ProductFormulaMaster_DAL> ProductFormulaMasterList { get; set; }


        public Pa_Production_Material_Out ProductionDetailObj { get; set; }
        public IEnumerable<Pa_Production_Material_Out> ProductionDetailList { get; set; }
        public Pa_Production_Master ProductionMasterObj { get; set; }
        public IPagedList<Pa_Production_Master> ProductionMasterList { get; set; }


        public InventoryTransferDetails InventoryTransferDetailObj { get; set; }
        public IEnumerable<InventoryTransferDetails> InventoryTransferDetailList { get; set; }
        public Inventory_Transfer_Master InventoryTransferMasterObj { get; set; }
        public IEnumerable<Inventory_Transfer_Master> InventoryTransferMasterList { get; set; }
        public IEnumerable<Pa_PurchaseDetails_DAL> LocDetailList { get; set; }
        //New inventry work
        public Inventory_Transfer_Master InventoryTransferRef { get; set; }
        public IEnumerable<Pa_Production_Master> ProductionMasterList1 { get; set; }

        //productformula

        public pa_ProductFormulaMaster_DAL InventoryFormulaRef { get; set; }
        public IEnumerable<pa_ProductFormulaMaster_DAL> ProductFormulaMasterList1 { get; set; }

        public Pa_Dismental_Master DismentalMasterObj { get; set; }
        public IPagedList<Pa_Dismental_Master> DismentalMasterList { get; set; }
        public Pa_Dismental_Dtl DismentalDetailObj { get; set; }
        public IEnumerable<Pa_Dismental_Dtl> DismentalDetailList { get; set; }
        //
        public IEnumerable<Pa_Dismental_Dtl> FormulaDetailList { get; set; }
        public Pa_Dismental_Master InventoryDismentalRef { get; set; }




        public Pa_Assembly_Master AssemblyMasterObj { get; set; }
        public Pa_Assembly_Master AssemblyStockMasterObj { get; set; }
        public IPagedList<Pa_Assembly_Master> AssemblyMasterList { get; set; }
        public Pa_Assembly_Dtl AssemblyDetailObj { get; set; }
        public IEnumerable<Pa_Assembly_Dtl> AssemblyDetailList { get; set; }
       
        public Pa_Assembly_Master InventoryAssemblyRef { get; set; }

        public Pa_Dismental_Master DismentalMasterObj_Print { get; set; }
        public IEnumerable<Pa_Dismental_Dtl> DismentalDetailList_Print { get; set; }
        public Pa_Assembly_Master AssemblyMasterObj_Print { get; set; }
        public IEnumerable<Pa_Assembly_Dtl> AssemblyDetailList_Print { get; set; }

        public pa_ProductFormulaMaster_DAL FormulaMasterObj_Print { get; set; }
        public IEnumerable<pa_ProductFormulaDetails> FormulaDetailList_Print { get; set; }




        public IEnumerable<Pa_Dismental_Master> DismentalMasterList_Print { get; set; }



        public IEnumerable<Pa_Assembly_Master> Get_AssembleMaster { get; set; }
        #region ProductFormula
        public pa_ProductFormulaMaster_DAL Get_ProductFormulaMasterByID_DAL(int? Formula_ID)
        {

            string sp = "Select_ProductFormula_By_ID";

            pa_ProductFormulaMaster_DAL pm = new pa_ProductFormulaMaster_DAL();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Formula_ID", Formula_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.Formula_ID = Convert.ToInt32(dr["Formula_ID"]);

                                pm.Ref = dr["Ref"].ToString();
                                pm.P_Date = dr["P_Date"].ToString();
                                pm.FormulaName = dr["FormulaName"].ToString();
                                pm.Production_Details = dr["Production_Details"].ToString();
                                pm.transaction_Status = dr["transaction_Status"].ToString();


                                pm.c_ID = dr["c_ID"].ToString();
                                pm.ItemID = dr["ItemID"].ToString();
                                pm.isActive = dr["isActive"].ToString();






                                pm.Created_At = dr["Created_At"].ToString();
                                pm.Created_By = dr["Created_By"].ToString();
                                pm.Last_Updated_At = dr["Last_Updated_At"].ToString();
                                pm.Last_Updated_By = dr["Last_Updated_By"].ToString();

                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }


            return pm;
        }
        public IEnumerable<pa_ProductFormulaDetails> Get_ProductFormulaDetailListBy_DAL(string Temp_ID, int? Formula_ID)
        {



            string sp = "Select_productFormula_IN_by_ID";
            List<pa_ProductFormulaDetails> itemlist = new List<pa_ProductFormulaDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);
                    cmd.Parameters.AddWithValue("@Formula_ID", Formula_ID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_ProductFormulaDetails item = new pa_ProductFormulaDetails();

                        item.ID = Convert.ToInt32(rdr["ID"]);

                        item.ProductFormula_IN_ID = Convert.ToInt32(rdr["ProductFormula_IN_ID"]);
                        item.Formula_ID = Convert.ToInt32(rdr["Formula_ID"]);

                        item.ItemID = rdr["ItemID"].ToString();
                        item.ItemCode = rdr["ItemCode"].ToString();
                        item.MajorQty = rdr["MajorQty"].ToString();
                        item.MinorQty = rdr["MinorQty"].ToString();
                        item.Quantity = rdr["Quantity"].ToString();
                        item.UOM = rdr["UOM"].ToString();

                        item.UnitPrice = rdr["UnitPrice"].ToString();
                        item.Amount = rdr["Amount"].ToString();


                        item.transaction_Status = rdr["transaction_Status"].ToString();
                        item.ItemName = rdr["ItemName"].ToString();




                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public string InsertProductFormulaDetail_DAL(int? HdFormula_ID, string ItemID, string UOM, double? UnitPrice, double?
        Amount, string Temp_ID, string MajorQty, string MinorQty)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {



                           new SqlParameter("@ItemID",ItemID),
                            new SqlParameter("@MajorQty",MajorQty),
                            new SqlParameter("@MinorQty",MinorQty),
                             new SqlParameter("@UOM",UOM),
                              new SqlParameter("@UnitPrice",UnitPrice),

                               new SqlParameter("@Temp_ID",Temp_ID),
                               new SqlParameter("@HdFormula_ID",HdFormula_ID),
                                new SqlParameter("@Amount",Amount)




            };


                var response = dbLayer.SP_DataTable_return("INSERT_productFormula_IN", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();
                if (msg.Trim().Contains("Violation of UNIQUE KEY"))
                {
                    msg = "Already Exist!";
                }
                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public string UpdateProductFormulaDetail_DAL(int? ProductFormula_IN_ID, string ItemID, string UOM, double? UnitPrice, double?
        Amount, string Temp_ID, string MajorQty, string MinorQty)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
          new SqlParameter("@ProductFormula_IN_ID",ProductFormula_IN_ID),
                           new SqlParameter("@ItemID",ItemID),
                               new SqlParameter("@MajorQty",MajorQty),
                            new SqlParameter("@MinorQty",MinorQty),
                             new SqlParameter("@UOM",UOM),
                              new SqlParameter("@UnitPrice",UnitPrice),


                                new SqlParameter("@Amount",Amount),


            };


                var response = dbLayer.SP_DataTable_return("update_productFormula_IN", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string InsertProductFormulaMaster_DAL(string P_Date, string Ref, string FormulaName, string Production_Details, string ItemID,
     string Temp_ID, int? c_ID, int? Created_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                new SqlParameter("@P_Date",P_Date),
                new SqlParameter("@Ref",Ref),
                new SqlParameter("@FormulaName",FormulaName),
                 new SqlParameter("@Production_Details",Production_Details),
                  new SqlParameter("@ItemID",ItemID),
                new SqlParameter("@UserID",Created_by),

                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),







            };


                var response = dbLayer.SP_DataTable_return("Insert_ProductFormula", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public string UpdateProductFormulaMaster_DAL(string P_Date, string Ref, string FormulaName, string Production_Details, string ItemID, int? Formula_ID, string isActive,
          string Temp_ID, int? c_ID, int? Modified_by)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@Formula_ID",Formula_ID),
                new SqlParameter("@P_Date",P_Date),
                new SqlParameter("@FormulaName",FormulaName),
                new SqlParameter("@Production_Details",Production_Details),

                  new SqlParameter("@ItemID",ItemID),

                    new SqlParameter("@isActive",isActive),



                 new SqlParameter("@Temp_ID",Temp_ID),


                 new SqlParameter("@c_ID",c_ID),

                 new SqlParameter("@UserID",Modified_by)




            };


                var response = dbLayer.SP_DataTable_return("Update_ProductFormula", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }


        public string DeleteProductFormulaDetail_DAL(int? ProductFormula_IN_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@ProductFormula_IN_ID",ProductFormula_IN_ID)



            };


                var response = dbLayer.SP_DataTable_return("delete_productFormula_IN", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public IEnumerable<pa_ProductFormulaMaster_DAL> Get_ProductFormulaMasterInvoiceList_DAL(string Ref, string StartDate, string EndDate
    , int c_ID)

        {

            string sp = "Select_ProductFormula_List";
            List<pa_ProductFormulaMaster_DAL> itemlist = new List<pa_ProductFormulaMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Ref", Ref);
                    cmd.Parameters.AddWithValue("@Start_Date", StartDate);
                    cmd.Parameters.AddWithValue("@End_Date", EndDate);

                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader dr = cmd.ExecuteReader();


                    while (dr.Read())
                    {
                        pa_ProductFormulaMaster_DAL pm = new pa_ProductFormulaMaster_DAL();
                        pm.Formula_ID = Convert.ToInt32(dr["Formula_ID"]);

                        pm.Ref = dr["Ref"].ToString();
                        pm.P_Date = dr["P_Date"].ToString();
                        pm.FormulaName = dr["FormulaName"].ToString();
                        pm.Production_Details = dr["Production_Details"].ToString();
                        pm.transaction_Status = dr["transaction_Status"].ToString();
                        pm.ItemName = dr["ItemName"].ToString();
                        pm.c_ID = dr["c_ID"].ToString();
                        pm.ItemID = dr["ItemID"].ToString();
                        pm.isActive = dr["isActive"].ToString();






                        pm.Created_At = dr["Created_At"].ToString();
                        pm.Created_By = dr["Created_By"].ToString();
                        pm.Last_Updated_At = dr["Last_Updated_At"].ToString();
                        pm.Last_Updated_By = dr["Last_Updated_By"].ToString();


                        itemlist.Add(pm);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public string GetOldTempIDFromProductFormulaDetail_DAL(int? Formula_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Formula_ID",Formula_ID)

            };


                var response = dbLayer.SP_DataTable_return("SelectProductFormulaDetailOldTempID", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }


        public pa_ProductFormulaMaster_DAL FormulaLoadRef()
        {



            string sp = "Get_FormulaRef";

            pa_ProductFormulaMaster_DAL si = new pa_ProductFormulaMaster_DAL();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                si.Ref = dr["Ref"].ToString();
                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }


            return si;
        }

        #endregion

        #region production
        public Pa_Production_Master Get_ProductionMasterByID_DAL(int? Production_ID)
        {

            string sp = "Select_Production_Master_ID";

            Pa_Production_Master pm = new Pa_Production_Master();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Production_ID", Production_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.Production_ID = Convert.ToInt32(dr["Production_ID"]);

                                pm.P_Date = dr["P_Date"].ToString();
                                pm.Ref = dr["Ref"].ToString();

                                pm.CustomerRef = dr["CustomerRef"].ToString();
                                pm.Supervisor = dr["Supervisor"].ToString();
                                pm.Production_Details = dr["Production_Details"].ToString();

                                pm.Created_By = dr["Created_By"].ToString();

                                pm.Created_At = dr["Created_At"].ToString();
                                pm.Last_Updated_By = dr["Last_Updated_By"].ToString();


                                pm.Last_Updated_At = dr["Last_Updated_At"].ToString();

                                pm.transaction_Status = dr["transaction_Status"].ToString();
                                pm.c_ID = Convert.ToInt32(dr["c_ID"]);


                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }


            return pm;
        }
        public IEnumerable<Pa_Production_Material_In> Get_Production_Material_IN_DetailListBy_DAL(string Temp_ID, int? Production_ID)
        {



            string sp = "Select_Production_Material_IN_by_ID";
            List<Pa_Production_Material_In> itemlist = new List<Pa_Production_Material_In>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@production_ID", Production_ID);
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Production_Material_In item = new Pa_Production_Material_In();

                        item.ID = Convert.ToInt32(rdr["ID"]);

                        item.Material_IN_ID = Convert.ToInt32(rdr["Material_IN_ID"]);
                        item.Production_ID = Convert.ToInt32(rdr["Production_ID"]);

                        item.ItemID = Convert.ToInt32(rdr["ItemID"]);
                        item.MajorQty = rdr["MajorQty"].ToString();
                        item.MinorQty = rdr["MinorQty"].ToString();

                        item.UOM = rdr["UOM"].ToString();

                        item.UnitPrice = rdr["UnitPrice"].ToString();
                        item.Amount = rdr["Amount"].ToString();


                        item.transaction_Status = rdr["transaction_Status"].ToString();
                        item.created_At = rdr["created_At"].ToString();
                        item.Last_Updated_At = rdr["Last_Updated_At"].ToString();



                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public IEnumerable<Pa_Production_Material_Out> Get_Production_Material_OUT_DetailListBy_DAL(string Temp_ID, int? Production_ID)
        {



            string sp = "Select_Production_Material_OUT_by_ID";
            List<Pa_Production_Material_Out> itemlist = new List<Pa_Production_Material_Out>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@production_ID", Production_ID);
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Production_Material_Out item = new Pa_Production_Material_Out();

                        item.ID = Convert.ToInt32(rdr["ID"]);

                        item.Material_OUT_ID = Convert.ToInt32(rdr["Material_OUT_ID"]);
                        item.Production_ID = Convert.ToInt32(rdr["Production_ID"]);

                        item.ItemID = rdr["ItemID"].ToString();
                        item.DirectCost = rdr["DirectCost"].ToString();
                        item.InDirectCost = rdr["InDirectCost"].ToString();

                        item.MajorQty = rdr["MajorQty"].ToString();

                        item.MinorQty = rdr["MinorQty"].ToString();
                        item.Amount = rdr["Amount"].ToString();


                        item.transaction_Status = rdr["transaction_Status"].ToString();
                        item.UOM = rdr["UOM"].ToString();
                        item.UnitPrice = rdr["UnitPrice"].ToString();

                        item.FormulaName = rdr["FormulaName"].ToString();
                        item.formula_ID = Convert.ToInt32(rdr["formula_ID"]);
                        item.Quantity = rdr["Quantity"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        public string InsertProduction_Material_In_Detail_DAL(int? HdProduction_ID, string ItemID, string MajorQty, string
        MinorQty, string UOM, string Quantity, string Temp_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {


                          new SqlParameter("@Production_ID",HdProduction_ID),
                           new SqlParameter("@ItemID",ItemID),
                            new SqlParameter("@MajorQty",MajorQty),
                             new SqlParameter("@MinorQty",MinorQty),
                              new SqlParameter("@UOM",UOM),

                               new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@Qty_Out",Quantity)




            };


                var response = dbLayer.SP_DataTable_return("INSERT_Production_Material_IN", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();
                if (msg.Trim().Contains("Violation of UNIQUE KEY"))
                {
                    msg = "Already Exist!";
                }
                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string InsertProduction_Material_Out_Detail_DAL(int? HdProduction_ID, string ItemID, double? DirectCost, double?
        InDirectCost, string UOM, string Quantity, string Temp_ID, int formula_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {


                          new SqlParameter("@Production_ID",HdProduction_ID),
                           new SqlParameter("@ItemID",ItemID),
                           new SqlParameter("@DirectCost",DirectCost),
                           new SqlParameter("@InDirectCost",InDirectCost),
                            new SqlParameter("@Quantity",Quantity),
                              new SqlParameter("@UOM",UOM),
                                new SqlParameter("@Temp_ID",Temp_ID),
                                new SqlParameter("@formula_ID",formula_ID)




            };


                var response = dbLayer.SP_DataTable_return("INSERT_Production_Material_OUT", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();
                if (msg.Trim().Contains("Violation of UNIQUE KEY"))
                {
                    msg = "Already Exist!";
                }
                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public string UpdateProduction_Material_In_DAL(int? Material_IN_ID, string ItemID, string UOM, string Quantity, double? UnitPrice, double? Amount)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@Material_IN_ID",Material_IN_ID),

                            new SqlParameter("@ItemID",ItemID),
                           new SqlParameter("@Quantity",Quantity),

                             new SqlParameter("@UOM",UOM),
                              new SqlParameter("@UnitPrice",UnitPrice),
                               new SqlParameter("@Amount",Amount)





            };


                var response = dbLayer.SP_DataTable_return("UPDATE_Production_Material_IN", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string UpdateProduction_Material_Out_DAL(int? Material_OUT_ID, string ItemID, double? DirectCost, double?
        InDirectCost, string UOM, string Quantity, int formula_ID, double? UnitPrice, double? Amount)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@Material_OUT_ID",Material_OUT_ID),

                            new SqlParameter("@ItemID",ItemID),
                           new SqlParameter("@DirectCost",DirectCost),
                            new SqlParameter("@InDirectCost",InDirectCost),
                             new SqlParameter("@Quantity",Quantity),

                               new SqlParameter("@UOM",UOM),
                              new SqlParameter("@UnitPrice",UnitPrice),
                              new SqlParameter("@Amount",Amount),

                              new SqlParameter("@formula_ID",formula_ID)




            };


                var response = dbLayer.SP_DataTable_return("UPDATE_Production_Material_OUT", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public string InsertProductionMaster_DAL(string P_Date, string Ref, string CustomerRef, string Supervisor,
     string Production_Details, string Created_By, int? c_id, string Temp_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {


                new SqlParameter("@P_Date",P_Date),
                new SqlParameter("@Ref",Ref),
                 new SqlParameter("@CustomerRef",CustomerRef),
                  new SqlParameter("@Supervisor",Supervisor),
                new SqlParameter("@Production_Details",Production_Details),

                 new SqlParameter("@UserID",Created_By),

                 new SqlParameter("@c_id",c_id),
                 new SqlParameter("@Temp_ID",Temp_ID)








            };


                var response = dbLayer.SP_DataTable_return("Insert_Production_Master", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public string UpdateProductionMaster_DAL(int Production_ID, string P_Date, string Ref, string CustomerRef, string Supervisor,
     string Production_Details, string Created_By, int? c_id, string Temp_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@Production_ID",Production_ID),
              new SqlParameter("@P_Date",P_Date),
                new SqlParameter("@Ref",Ref),
                 new SqlParameter("@CustomerRef",CustomerRef),
                  new SqlParameter("@Supervisor",Supervisor),
                new SqlParameter("@Production_Details",Production_Details),

                 new SqlParameter("@UserID",Created_By),

                 new SqlParameter("@Temp_ID",Temp_ID)




            };


                var response = dbLayer.SP_DataTable_return("Update_Production_Master", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string DeleteProductionMaster_DAL(int? Production_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Production_ID",Production_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_Production_Master", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public string DeleteProduction_Material_In_DAL(int? Material_IN_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Material_IN_ID",Material_IN_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_Production_Material_IN", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string DeleteProduction_Material_Out_DAL(int? Material_OUT_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Material_OUT_ID",Material_OUT_ID)



            };


                var response = dbLayer.SP_DataTable_return("delete_Production_Material_OUT", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }


        public IEnumerable<Pa_Production_Master> Get_ProductionMaster_DAL(string Ref, string StartDate, string EndDate
            , int c_ID)

        {

            string sp = "Select_Production_Master_List";
            List<Pa_Production_Master> itemlist = new List<Pa_Production_Master>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Ref", Ref);
                    cmd.Parameters.AddWithValue("@Start_Date", StartDate);
                    cmd.Parameters.AddWithValue("@End_Date", EndDate);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader dr = cmd.ExecuteReader();


                    while (dr.Read())
                    {
                        Pa_Production_Master pm = new Pa_Production_Master();
                        pm.Production_ID = Convert.ToInt32(dr["Production_ID"]);

                        pm.P_Date = dr["P_Date"].ToString();
                        pm.Ref = dr["Ref"].ToString();

                        pm.CustomerRef = dr["CustomerRef"].ToString();
                        pm.Supervisor = dr["Supervisor"].ToString();
                        pm.Production_Details = dr["Production_Details"].ToString();

                        pm.Created_By = dr["Created_By"].ToString();

                        pm.Created_At = dr["Created_At"].ToString();
                        pm.Last_Updated_By = dr["Last_Updated_By"].ToString();


                        pm.Last_Updated_At = dr["Last_Updated_At"].ToString();

                        pm.transaction_Status = dr["transaction_Status"].ToString();
                        pm.c_ID = Convert.ToInt32(dr["c_ID"]);

                        itemlist.Add(pm);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public string GetOldTempIDFromProductionDetail_DAL(int? Production_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Production_ID",Production_ID)

            };


                var response = dbLayer.SP_DataTable_return("SelectProducionDetailOldTempID", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public IEnumerable<pa_ProductFormulaMaster_DAL> Get_Formula_DAL()
        {


            string sp = "select_productFormula";
            List<pa_ProductFormulaMaster_DAL> itemlist = new List<pa_ProductFormulaMaster_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        pa_ProductFormulaMaster_DAL item = new pa_ProductFormulaMaster_DAL();

                        item.Formula_ID = Convert.ToInt32(rdr["Formula_ID"]);
                        item.FormulaName = rdr["FormulaName"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }
        #endregion

        #region InventoryTransfer
        public Inventory_Transfer_Master Get_InventoryTransfer_MasterByID_DAL(int? Transfer_ID)
        {

            string sp = "select_InventoryTransferMaster";

            Inventory_Transfer_Master pm = new Inventory_Transfer_Master();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Transfer_ID", Transfer_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.Transfer_ID = Convert.ToInt32(dr["Transfer_ID"]);

                                pm.transferDate = dr["transferDate"].ToString();
                                pm.Ref = dr["Ref"].ToString();

                                pm.OtherDetails = dr["OtherDetails"].ToString();
                                pm.Loc_ID = Convert.ToInt32(dr["Loc_ID"]);
                                pm.created_by = dr["created_by"].ToString();

                                pm.created_at = dr["created_at"].ToString();

                                pm.Last_Updated_at = dr["Last_Updated_at"].ToString();
                                pm.Last_Update_by = dr["Last_Update_by"].ToString();





                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }


            return pm;
        }
        public IEnumerable<InventoryTransferDetails> Get_InventoryTransfer_DetailListBy_DAL(string Temp_ID, int? Transfer_ID)
        {



            string sp = "select_InventoryTransferDetails_by_ID";
            List<InventoryTransferDetails> itemlist = new List<InventoryTransferDetails>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Transfer_ID", Transfer_ID);
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        InventoryTransferDetails item = new InventoryTransferDetails();

                        item.ID = Convert.ToInt32(rdr["ID"]);

                        item.invoiceID = Convert.ToInt32(rdr["invoiceID"]);


                        item.ItemId = Convert.ToInt32(rdr["ItemId"]);


                        item.ItemDesc = rdr["ItemDesc"].ToString();
                        item.Unit_Price = rdr["Unit_Price"].ToString();


                        item.Amount = rdr["Amount"].ToString();
                        item.job_ref = rdr["job_ref"].ToString();
                        item.OldLoc_ID = Convert.ToInt32(rdr["OldLoc_ID"]);
                        item.NewLoc_ID = Convert.ToInt32(rdr["NewLoc_ID"]);
                        item.transaction_Status = rdr["transaction_Status"].ToString();


                        item.comments = rdr["comments"].ToString();
                        item.quantity = rdr["quantity"].ToString();
                        item.NewItemLocationName = rdr["NewItemLocationName"].ToString();
                        item.OldItemLocationName = rdr["OldItemLocationName"].ToString();
                        item.ItemName = rdr["ItemName"].ToString();
                        item.ItemCode = rdr["ItemCode"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }


        public string InsertInventory_Transfer_Detail_DAL(int? HdTransfer_ID, int ItemId, string UM_ID, string
        MajorQty, string MinorQty, string ItemDesc, string Unit_Price, string Amount, string job_ref, int OldLoc_ID, int NewLoc_ID, string Temp_ID, int c_id, string comments, string quantity)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                              new SqlParameter("@Transfer_ID",HdTransfer_ID),

                               new SqlParameter("@ItemId",ItemId),
                                new SqlParameter("@UM_ID",UM_ID),
                                new SqlParameter("@quantity",quantity),
                                new SqlParameter("@MajorQty",MajorQty),
                                 new SqlParameter("@MinorQty",MinorQty),


                                   new SqlParameter("@ItemDesc",ItemDesc),
                                    new SqlParameter("@Unit_Price",Unit_Price),
                                    new SqlParameter("@Amount",Amount),
                                    new SqlParameter("@Job_Ref",job_ref),
                                    new SqlParameter("@OldLoc_ID",OldLoc_ID),
                                     new SqlParameter("@NewLoc_ID",NewLoc_ID),

                                    new SqlParameter("@c_id",c_id),
                                    new SqlParameter("@Temp_ID",Temp_ID),
                         
                                    new SqlParameter("@comments",comments)



                };


                var response = dbLayer.SP_DataTable_return("Insert_InventoryTransferDetails", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();
                if (msg.Trim().Contains("Violation of UNIQUE KEY"))
                {
                    msg = "Already Exist!";
                }
                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string UpdateInventory_Transfer_Detail_DAL(int? ID, int? Transfer_ID, int ItemId, string ItemDesc, string Unit_Price, string Amount, string job_ref, int OldLoc_ID, int NewLoc_ID, string comments, string quantity)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                              new SqlParameter("@ID",ID),
                              new SqlParameter("@Transfer_ID",Transfer_ID),

                               new SqlParameter("@ItemId",ItemId),
                         new SqlParameter("@quantity",quantity),



                                   new SqlParameter("@ItemDesc",ItemDesc),
                                    new SqlParameter("@Unit_Price",Unit_Price),
                                    new SqlParameter("@Amount",Amount),
                                    new SqlParameter("@Job_Ref",job_ref),
                                    new SqlParameter("@OldLoc_ID",OldLoc_ID),
                                     new SqlParameter("@NewLoc_ID",NewLoc_ID),

                                    new SqlParameter("@comments",comments)



                };










                var response = dbLayer.SP_DataTable_return("Update_InventoryTransferDetails", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }


        public string InsertInventoryTransferMaster_DAL(string transferDate, string Ref, string OtherDetails, int? Loc_ID,
     string created_by, string Temp_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {


                    new SqlParameter("@transferDate",transferDate),
                    new SqlParameter("@ref",Ref),
                     new SqlParameter("@OtherDetails",OtherDetails),
                      new SqlParameter("@Loc_ID",Loc_ID),
                    new SqlParameter("user_ID",created_by),

                      new SqlParameter("@Temp_ID",Temp_ID)









                };


                var response = dbLayer.SP_DataTable_return("Insert_InventoryTransferMaster", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public string UpdateInventory_TransferMaster_DAL(int Transfer_ID, string transferDate, string Ref, string OtherDetails, int? Loc_ID,
      string Last_Update_by, string Temp_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {

                    new SqlParameter("@transfer_ID",Transfer_ID),
                new SqlParameter("@transferDate",transferDate),
                    new SqlParameter("@ref",Ref),
                     new SqlParameter("@OtherDetails",OtherDetails),
                      new SqlParameter("@Loc_ID",Loc_ID),
                  new SqlParameter("@Temp_ID",Temp_ID),
                     new SqlParameter("@user_ID",Last_Update_by)




                };


                var response = dbLayer.SP_DataTable_return("Update_InventoryTransferMaster", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string DeleteInventory_TransferMaster_DAL(int? transfer_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                    new SqlParameter("@transfer_ID",transfer_ID)



                };


                var response = dbLayer.SP_DataTable_return("Delete_InventoryTransferMaster", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public string DeleteInventory_Transfer_Details(int? ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                    new SqlParameter("@ID",ID)



                };


                var response = dbLayer.SP_DataTable_return("delete_InventoryTransferDetails", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }



        public IEnumerable<Inventory_Transfer_Master> Get_Inventory_TransferMaster_DAL()

        {

            string sp = "select_InventoryTransfer_List";
            List<Inventory_Transfer_Master> itemlist = new List<Inventory_Transfer_Master>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    SqlDataReader dr = cmd.ExecuteReader();


                    while (dr.Read())
                    {
                        Inventory_Transfer_Master pm = new Inventory_Transfer_Master();
                        pm.Transfer_ID = Convert.ToInt32(dr["Transfer_ID"]);

                        pm.transferDate = dr["transferDate"].ToString();

                        pm.Ref = dr["ref"].ToString();
                        pm.OtherDetails = dr["OtherDetails"].ToString();
         


                        pm.created_by = dr["created_by"].ToString();
                        pm.created_at = dr["created_at"].ToString();
                        pm.Last_Updated_at = dr["Last_Updated_at"].ToString();


                        pm.Last_Update_by = dr["Last_Update_by"].ToString();
                        pm.ItemLocationName = dr["ItemLocationName"].ToString();



                        itemlist.Add(pm);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public string GetOldTempIDFromInventory_TransferDetail_DAL(int? transfer_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@transfer_ID",transfer_ID)

            };


                var response = dbLayer.SP_DataTable_return("SelectInventoryTransferDetailOldTempID", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public Inventory_Transfer_Master TransferLoadRef()
        {



            string sp = "Get_TransferRef";

            Inventory_Transfer_Master si = new Inventory_Transfer_Master();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                si.Ref = dr["TransRef"].ToString();
                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }


            return si;
        }



        #endregion



        public IEnumerable<Pa_PurchaseDetails_DAL> GetLocationDetails_DAL(string ItemCode)
        {


            string sp = "GetLocationDetails";
            List<Pa_PurchaseDetails_DAL> itemlist = new List<Pa_PurchaseDetails_DAL>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemCode", ItemCode);



                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_PurchaseDetails_DAL item = new Pa_PurchaseDetails_DAL();

                        item.ItemName = rdr["ItemName"].ToString();
                        item.LocationName = rdr["ItemLocationName"].ToString();
                        item.Quantity = rdr["QTY"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

     

        #region Dismental
        public Pa_Dismental_Master Get_DismentalMasterByID_DAL(int? Dismental_ID)
        {

            string sp = "Select_Dismental_Master_ID";

            Pa_Dismental_Master pm = new Pa_Dismental_Master();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Dismental_ID", Dismental_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.Dismental_ID = Convert.ToInt32(dr["Dismental_ID"]);

                                pm.P_Date = dr["P_Date"].ToString();
                                pm.Ref = dr["Ref"].ToString();

                                pm.CustomerRef = dr["CustomerRef"].ToString();
                                pm.Supervisor = dr["Supervisor"].ToString();
                                pm.Dismental_Chassis = dr["Dismental_Chassis"].ToString();

                                pm.Created_By = dr["Created_By"].ToString();

                                pm.Created_At = dr["Created_At"].ToString();
                                pm.Last_Updated_By = dr["Last_Updated_By"].ToString();


                                pm.Last_Updated_At = dr["Last_Updated_At"].ToString();

                                pm.transaction_Status = dr["transaction_Status"].ToString();
                                pm.c_ID = Convert.ToInt32(dr["c_ID"]);


                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }


            return pm;
        }
        public IEnumerable<Pa_Dismental_Dtl> Get_Dismental_Material_IN_DetailListBy_DAL(string Temp_ID, int? Dismental_ID)
        {



            string sp = "Select_Dismental_Dtl_by_ID";
            List<Pa_Dismental_Dtl> itemlist = new List<Pa_Dismental_Dtl>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Dismental_ID", Dismental_ID);
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Dismental_Dtl item = new Pa_Dismental_Dtl();

                        item.Dismental_Dtl_ID = Convert.ToInt32(rdr["Dismental_Dtl_ID"]);

                        item.Dismental_ID = Convert.ToInt32(rdr["Dismental_ID"]);
                        item.Item_ID = Convert.ToInt32(rdr["Item_ID"]);

                        item.Quantity = rdr["Quantity"].ToString();
                        item.Cost = rdr["Cost"].ToString();
                        item.ItemName = rdr["ItemName"].ToString();
                        item.ItemCode = rdr["ItemCode"].ToString();
                   



                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

    public string InsertDismental_Material_In_Detail_DAL(int? HdDismental_ID, int Item_ID, string Cost, string Quantity, string Temp_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {


                          new SqlParameter("@Dismental_ID",HdDismental_ID),
                           new SqlParameter("@ItemID",Item_ID),
                            new SqlParameter("@Quantity",Quantity),
                             new SqlParameter("@Cost",Cost),
                             new SqlParameter("@TempId",Temp_ID)
                         




            };


                var response = dbLayer.SP_DataTable_return("INSERT_Dismental_Dtl", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();
                if (msg.Trim().Contains("Violation of UNIQUE KEY"))
                {
                    msg = "Already Exist!";
                }
                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }


        public string UpdateDismental_Material_In_DAL(int? Dismental_Dtl_ID, string ItemID, string Cost, string Quantity)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@Dismental_Dtl_ID",Dismental_Dtl_ID),

                            new SqlParameter("@ItemID",ItemID),
                           new SqlParameter("@Quantity",Quantity),

                             new SqlParameter("@Cost",Cost)
                       





            };


                var response = dbLayer.SP_DataTable_return("UPDATE_Dismental_Dtl", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

  
        public string InsertDismentalMaster_DAL(string P_Date, string Ref, string CustomerRef, string Supervisor,
     string Dismental_Chassis, string Created_By, int? c_id, string Temp_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {


                new SqlParameter("@P_Date",P_Date),
                new SqlParameter("@Ref",Ref),
                 new SqlParameter("@CustomerRef",CustomerRef),
                  new SqlParameter("@Supervisor",Supervisor),
                new SqlParameter("@Dismental_Chassis",Dismental_Chassis),

                 new SqlParameter("@UserID",Created_By),

                 new SqlParameter("@c_id",c_id),
                 new SqlParameter("@Temp_ID",Temp_ID)








            };


                var response = dbLayer.SP_DataTable_return("Insert_Dismental_Master", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public string UpdateDismentalMaster_DAL(int Dismental_ID, string P_Date, string Ref, string CustomerRef, string Supervisor,
     string Dismental_Chassis, string Created_By, int? c_id, string Temp_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@Dismental_ID",Dismental_ID),
              new SqlParameter("@P_Date",P_Date),
                new SqlParameter("@Ref",Ref),
                 new SqlParameter("@CustomerRef",CustomerRef),
                  new SqlParameter("@Supervisor",Supervisor),
                new SqlParameter("@Dismental_Chassis",Dismental_Chassis),

                 new SqlParameter("@UserID",Created_By),

                 new SqlParameter("@Temp_ID",Temp_ID)




            };


                var response = dbLayer.SP_DataTable_return("Update_Dismental_Master", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string DeleteDismentalMaster_DAL(int? Dismental_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Dismental_ID",Dismental_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_Dismental_Master", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public string DeleteDismental_Material_In_DAL(int? Dismental_Dtl_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Dismental_Dtl_ID",Dismental_Dtl_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_Dismental_Dtl", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

    


        public IEnumerable<Pa_Dismental_Master> Get_DismentalMaster_DAL(string Ref, string StartDate, string EndDate
            , int c_ID)

        {

            string sp = "Select_Dismental_Master_List";
            List<Pa_Dismental_Master> itemlist = new List<Pa_Dismental_Master>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Ref", Ref);
                    cmd.Parameters.AddWithValue("@Start_Date", StartDate);
                    cmd.Parameters.AddWithValue("@End_Date", EndDate);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader dr = cmd.ExecuteReader();


                    while (dr.Read())
                    {
                        Pa_Dismental_Master pm = new Pa_Dismental_Master();
                        pm.Dismental_ID = Convert.ToInt32(dr["Dismental_ID"]);

                        pm.P_Date = dr["P_Date"].ToString();
                        pm.Ref = dr["Ref"].ToString();

                        pm.CustomerRef = dr["CustomerRef"].ToString();
                        pm.Supervisor = dr["Supervisor"].ToString();
                        pm.Dismental_Chassis = dr["Dismental_Chassis"].ToString();

                        pm.Created_By = dr["Created_By"].ToString();

                        pm.Created_At = dr["Created_At"].ToString();
                        pm.Last_Updated_By = dr["Last_Updated_By"].ToString();


                        pm.Last_Updated_At = dr["Last_Updated_At"].ToString();

                        pm.transaction_Status = dr["transaction_Status"].ToString();
                        pm.c_ID = Convert.ToInt32(dr["c_ID"]);

                        itemlist.Add(pm);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public string GetOldTempIDFromDismentalDetail_DAL(int? Dismental_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Dismental_ID",Dismental_ID)

            };


                var response = dbLayer.SP_DataTable_return("SelectDismentalDetailOldTempID", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public IEnumerable<Pa_Dismental_Dtl> GetFormulaDetails_DAL(int FormulaId)
        {


            string sp = "GetFormulaDetails";
            List<Pa_Dismental_Dtl> itemlist = new List<Pa_Dismental_Dtl>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FormulaId", FormulaId);



                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Dismental_Dtl item = new Pa_Dismental_Dtl();

                        item.Item_ID = Convert.ToInt32(rdr["ItemID"].ToString());
                        item.ItemName = rdr["ItemName"].ToString();
                        item.ItemCode = rdr["ItemCode"].ToString();


                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public Pa_Dismental_Master DismentalLoadRef()
        {



            string sp = "Get_DismentalRef";

            Pa_Dismental_Master si = new Pa_Dismental_Master();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                si.Ref = dr["Ref"].ToString();
                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }


            return si;
        }
        #endregion

        #region Assembly

        public Pa_Assembly_Master Get_AssemblyMasterByID_DAL(int? Assembly_ID)
        {

            string sp = "Select_Assembly_Master_ID";

            Pa_Assembly_Master pm = new Pa_Assembly_Master();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Assembly_ID", Assembly_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.Assembly_ID = Convert.ToInt32(dr["Assembly_ID"]);

                                pm.P_Date = dr["P_Date"].ToString();
                                pm.Ref = dr["Ref"].ToString();

                                pm.CustomerRef = dr["CustomerRef"].ToString();
                                pm.Supervisor = dr["Supervisor"].ToString();
                                pm.Assembly_Chassis = dr["Assembly_Chassis"].ToString();

                                pm.Created_By = dr["Created_By"].ToString();

                                pm.Created_At = dr["Created_At"].ToString();
                                pm.Last_Updated_By = dr["Last_Updated_By"].ToString();


                                pm.Last_Updated_At = dr["Last_Updated_At"].ToString();

                                pm.transaction_Status = dr["transaction_Status"].ToString();
                                pm.c_ID = Convert.ToInt32(dr["c_ID"]);
                                pm.ItemID = Convert.ToInt32(dr["ItemID"]);
                                pm.Assembly_Type = dr["Assembly_Type"].ToString();


                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }


            return pm;
        }
        public IEnumerable<Pa_Assembly_Dtl> Get_Assembly_Material_IN_DetailListBy_DAL(string Temp_ID, int? Assembly_ID)
        {



            string sp = "Select_Assembly_Dtl_by_ID";
            List<Pa_Assembly_Dtl> itemlist = new List<Pa_Assembly_Dtl>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Assembly_ID", Assembly_ID);
                    cmd.Parameters.AddWithValue("@Temp_ID", Temp_ID);


                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Pa_Assembly_Dtl item = new Pa_Assembly_Dtl();

                        item.Assembly_Dtl_ID = Convert.ToInt32(rdr["Assembly_Dtl_ID"]);

                        item.Assembly_ID = Convert.ToInt32(rdr["Assembly_ID"]);
                        item.Item_ID = Convert.ToInt32(rdr["Item_ID"]);

                        item.Quantity = rdr["Quantity"].ToString();
                        item.Cost = rdr["Cost"].ToString();
                        item.ItemName = rdr["ItemName"].ToString();
                        item.ItemCode = rdr["ItemCode"].ToString();
                        item.Remarks = rdr["Remarks"].ToString();




                        itemlist.Add(item);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public string InsertAssembly_Material_In_Detail_DAL(int? HdAssembly_ID, int Item_ID, string Cost, string Quantity,string Remarks, string Temp_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {


                          new SqlParameter("@Assembly_ID",HdAssembly_ID),
                           new SqlParameter("@ItemID",Item_ID),
                            new SqlParameter("@Quantity",Quantity),
                             new SqlParameter("@Cost",Cost),
                             new SqlParameter("@Remarks",Remarks),
                             new SqlParameter("@TempId",Temp_ID)





            };


                var response = dbLayer.SP_DataTable_return("INSERT_Assembly_Dtl", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();
                if (msg.Trim().Contains("Violation of UNIQUE KEY"))
                {
                    msg = "Already Exist!";
                }
                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }


        public string UpdateAssembly_Material_In_DAL(int? Assembly_Dtl_ID, string ItemID, string Cost, string Quantity)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@Assembly_Dtl_ID",Assembly_Dtl_ID),

                            new SqlParameter("@ItemID",ItemID),
                           new SqlParameter("@Quantity",Quantity),

                             new SqlParameter("@Cost",Cost)






            };


                var response = dbLayer.SP_DataTable_return("UPDATE_Assembly_Dtl", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }


        public string InsertAssemblyMaster_DAL(string P_Date, string Ref, string CustomerRef, string Supervisor,
     string Assembly_Chassis, string Created_By, int? c_id, string Temp_ID, int ItemID, string Assembly_Type)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {


                new SqlParameter("@P_Date",P_Date),
                new SqlParameter("@Ref",Ref),
                 new SqlParameter("@CustomerRef",CustomerRef),
                  new SqlParameter("@Supervisor",Supervisor),
                new SqlParameter("@Assembly_Chassis",Assembly_Chassis),

                 new SqlParameter("@UserID",Created_By),

                 new SqlParameter("@c_id",c_id),
                 new SqlParameter("@ItemID",ItemID),
                 new SqlParameter("@Assembly_Type",Assembly_Type),
                 new SqlParameter("@Temp_ID",Temp_ID)








            };


                var response = dbLayer.SP_DataTable_return("Insert_Assembly_Master", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public string UpdateAssemblyMaster_DAL(int Assembly_ID, string P_Date, string Ref, string CustomerRef, string Supervisor,
     string Assembly_Chassis, string Created_By, int? c_id, string Temp_ID, int ItemID, string Assembly_Type)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                     new SqlParameter("@Assembly_ID",Assembly_ID),
              new SqlParameter("@P_Date",P_Date),
                new SqlParameter("@Ref",Ref),
                 new SqlParameter("@CustomerRef",CustomerRef),
                  new SqlParameter("@Supervisor",Supervisor),
                new SqlParameter("@Assembly_Chassis",Assembly_Chassis),

                 new SqlParameter("@UserID",Created_By),
                  new SqlParameter("@ItemID",ItemID),
                 new SqlParameter("@Assembly_Type",Assembly_Type),
                 new SqlParameter("@Temp_ID",Temp_ID)




            };


                var response = dbLayer.SP_DataTable_return("Update_Assembly_Master", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        public string DeleteAssemblyMaster_DAL(int? Assembly_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Assembly_ID",Assembly_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_Assembly_Master", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public string DeleteAssembly_Material_In_DAL(int? Assembly_Dtl_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Assembly_Dtl_ID",Assembly_Dtl_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_Assembly_Dtl", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }




        public IEnumerable<Pa_Assembly_Master> Get_AssemblyMaster_DAL(string Ref, string StartDate, string EndDate
            , int c_ID)

        {

            string sp = "Select_Assembly_Master_List";
            List<Pa_Assembly_Master> itemlist = new List<Pa_Assembly_Master>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand(sp, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Ref", Ref);
                    cmd.Parameters.AddWithValue("@Start_Date", StartDate);
                    cmd.Parameters.AddWithValue("@End_Date", EndDate);
                    cmd.Parameters.AddWithValue("@c_ID", c_ID);

                    SqlDataReader dr = cmd.ExecuteReader();


                    while (dr.Read())
                    {
                        Pa_Assembly_Master pm = new Pa_Assembly_Master();
                        pm.Assembly_ID = Convert.ToInt32(dr["Assembly_ID"]);

                        pm.P_Date = dr["P_Date"].ToString();
                        pm.Ref = dr["Ref"].ToString();

                        pm.CustomerRef = dr["CustomerRef"].ToString();
                        pm.Supervisor = dr["Supervisor"].ToString();
                        pm.Assembly_Chassis = dr["Assembly_Chassis"].ToString();

                        pm.Created_By = dr["Created_By"].ToString();

                        pm.Created_At = dr["Created_At"].ToString();
                        pm.Last_Updated_By = dr["Last_Updated_By"].ToString();


                        pm.Last_Updated_At = dr["Last_Updated_At"].ToString();

                        pm.transaction_Status = dr["transaction_Status"].ToString();
                        pm.c_ID = Convert.ToInt32(dr["c_ID"]);

                        itemlist.Add(pm);

                    }

                    con.Close();

                }

                return itemlist;

            }
            catch (Exception ex)
            {
                string ErrorString = ex.Message;
                return null;
            }


        }

        public string GetOldTempIDFromAssemblyDetail_DAL(int? Assembly_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Assembly_ID",Assembly_ID)

            };


                var response = dbLayer.SP_DataTable_return("SelectAssemblyDetailOldTempID", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }

        

        public Pa_Assembly_Master AssemblyLoadRef()
        {



            string sp = "Get_AssemblyRef";

            Pa_Assembly_Master si = new Pa_Assembly_Master();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                si.Ref = dr["Ref"].ToString();
                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }


            return si;
        }
        public string InsertAssemblyStockMaster_DAL(string MasterId,string CHASSIS_NO, int make_ID, int MakeModel_description_ID, int ModelYear, int color_exterior_ID, int color_interior_ID)

        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {


                new SqlParameter("@MasterId",MasterId),
                new SqlParameter("@CHASSIS_NO",CHASSIS_NO),
                 new SqlParameter("@make_ID",make_ID),
                  new SqlParameter("@MakeModel_description_ID",MakeModel_description_ID),
                new SqlParameter("@ModelYear",ModelYear),

                 new SqlParameter("@color_exterior_ID",color_exterior_ID),

                 new SqlParameter("@color_interior_ID",color_interior_ID)








            };


                var response = dbLayer.SP_DataTable_return("Insert_Assembly_MaterialOut", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public string DeleteAssembly_Material_Out_DAL(int? Assembly_Dtl_ID)
        {

            string msg = "";
            try
            {

                SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Assembly_Dtl_ID",Assembly_Dtl_ID)



            };


                var response = dbLayer.SP_DataTable_return("Delete_Assembly_Out_Dtl", paramArray).Tables[0];
                msg = response.Rows[0][0].ToString();

                return msg;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                //throw;
                msg = ErrorMessage;
                return msg;
            }

        }
        public Pa_Assembly_Master Get_AssemblyStockMasterByID_DAL(int? Assembly_ID)
        {

            string sp = "Select_Assembly_Out_Master_ID";

            Pa_Assembly_Master pm = new Pa_Assembly_Master();
            //string sp = "";
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Assembly_ID", Assembly_ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                pm.Assembly_ID = Convert.ToInt32(dr["MasterId"]);

                                pm.CHASSIS_NO = dr["CHASSIS_NO"].ToString();
                                pm.make_ID = dr["make_ID"].ToString();

                                pm.MakeModel_description_ID = dr["MakeModel_description_ID"].ToString();
                                pm.ModelYear = dr["ModelYear"].ToString();
                                pm.color_exterior_ID = dr["color_exterior_ID"].ToString();

                                pm.color_interior_ID = dr["color_interior_ID"].ToString();



                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }


            return pm;
        }
        #endregion

    }
}
