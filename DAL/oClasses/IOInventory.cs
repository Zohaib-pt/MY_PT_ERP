using DAL.Models;
using System.Collections.Generic;
using X.PagedList;

namespace DAL.oClasses
{
    public interface IOInventory
    {
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
        public Inventory_Transfer_Master InventoryTransferRef { get; set; }
        public pa_ProductFormulaMaster_DAL InventoryFormulaRef { get; set; }
        public Pa_Dismental_Master InventoryDismentalRef { get; set; }
        public IEnumerable<Inventory_Transfer_Master> InventoryTransferMasterList { get; set; }
        public IEnumerable<Pa_PurchaseDetails_DAL> LocDetailList { get; set; }
        public IEnumerable<Pa_Dismental_Dtl> FormulaDetailList { get; set; }

        //New inventry work
        public IEnumerable<Pa_Production_Master> ProductionMasterList1 { get; set; }

        //productformula


        public IEnumerable<pa_ProductFormulaMaster_DAL> ProductFormulaMasterList1 { get; set; }

        //

        public Pa_Dismental_Master DismentalMasterObj { get; set; }
        public IPagedList<Pa_Dismental_Master> DismentalMasterList { get; set; }
        public Pa_Dismental_Dtl DismentalDetailObj { get; set; }
        public IEnumerable<Pa_Dismental_Dtl> DismentalDetailList { get; set; }

        public Pa_Assembly_Master AssemblyMasterObj { get; set; }
        public IPagedList<Pa_Assembly_Master> AssemblyMasterList { get; set; }
        public Pa_Assembly_Dtl AssemblyDetailObj { get; set; }
        public IEnumerable<Pa_Assembly_Dtl> AssemblyDetailList { get; set; }

        public Pa_Assembly_Master InventoryAssemblyRef { get; set; }
        public Pa_Assembly_Master AssemblyStockMasterObj { get; set; }
        public Pa_Dismental_Master DismentalMasterObj_Print { get; set; }
        public IEnumerable<Pa_Dismental_Dtl> DismentalDetailList_Print { get; set; }
        public Pa_Assembly_Master AssemblyMasterObj_Print { get; set; }
        public IEnumerable<Pa_Assembly_Dtl> AssemblyDetailList_Print { get; set; }
        public pa_ProductFormulaMaster_DAL FormulaMasterObj_Print { get; set; }
        public IEnumerable<pa_ProductFormulaDetails> FormulaDetailList_Print { get; set; }


        public IEnumerable<Pa_Dismental_Master> DismentalMasterList_Print { get; set; }



        public IEnumerable<Pa_Assembly_Master> Get_AssembleMaster { get; set; }
        #region ProductFormula
        public pa_ProductFormulaMaster_DAL Get_ProductFormulaMasterByID_DAL(int? Formula_ID);
        public IEnumerable<pa_ProductFormulaDetails> Get_ProductFormulaDetailListBy_DAL(string Temp_ID, int? Formula_ID);

        public string InsertProductFormulaDetail_DAL(int? HdFormula_ID, string ItemID, string UOM, double? UnitPrice, double?
        Amount, string Temp_ID, string MajorQty, string MinorQty);
        public string UpdateProductFormulaDetail_DAL(int? ProductFormula_IN_ID, string ItemID, string UOM, double? UnitPrice, double?
        Amount, string Temp_ID, string MajorQty, string MinorQty);

        public string InsertProductFormulaMaster_DAL(string P_Date, string Ref, string FormulaName, string Production_Details, string ItemID,
     string Temp_ID, int? c_ID, int? Created_by);
        public string UpdateProductFormulaMaster_DAL(string P_Date, string Ref, string FormulaName, string Production_Details, string ItemID, int? Formula_ID, string isActive,
          string Temp_ID, int? c_ID, int? Modified_by);


        public string DeleteProductFormulaDetail_DAL(int? ProductFormula_IN_ID);

        public IEnumerable<pa_ProductFormulaMaster_DAL> Get_ProductFormulaMasterInvoiceList_DAL(string Ref, string StartDate, string EndDate
            , int c_ID);

        public string GetOldTempIDFromProductFormulaDetail_DAL(int? Formula_ID);


        public pa_ProductFormulaMaster_DAL FormulaLoadRef();
        #endregion

        #region production
        public Pa_Production_Master Get_ProductionMasterByID_DAL(int? Production_ID);
        public IEnumerable<Pa_Production_Material_In> Get_Production_Material_IN_DetailListBy_DAL(string Temp_ID, int? Production_ID);

        public IEnumerable<Pa_Production_Material_Out> Get_Production_Material_OUT_DetailListBy_DAL(string Temp_ID, int? Production_ID);
        public string InsertProduction_Material_In_Detail_DAL(int? HdProduction_ID, string ItemID, string MajorQty, string
        MinorQty, string UOM, string Quantity, string Temp_ID);

        public string InsertProduction_Material_Out_Detail_DAL(int? HdProduction_ID, string ItemID, double? DirectCost, double?
        InDirectCost, string UOM, string Quantity, string Temp_ID, int formula_ID);
        public string UpdateProduction_Material_In_DAL(int? Material_IN_ID, string ItemID, string UOM, string Quantity, double? UnitPrice, double? Amount);

        public string UpdateProduction_Material_Out_DAL(int? Material_OUT_ID, string ItemID, double? DirectCost, double?
        InDirectCost, string UOM, string Quantity, int formula_ID, double? UnitPrice, double? Amount);
        public string InsertProductionMaster_DAL(string P_Date, string Ref, string CustomerRef, string Supervisor,
     string Production_Details, string Created_By, int? c_id, string Temp_ID);
        public string UpdateProductionMaster_DAL(int Production_ID, string P_Date, string Ref, string CustomerRef, string Supervisor,
     string Production_Details, string Created_By, int? c_id, string Temp_ID);

        public string DeleteProductionMaster_DAL(int? Production_ID);
        public string DeleteProduction_Material_In_DAL(int? Material_IN_ID);

        public string DeleteProduction_Material_Out_DAL(int? Material_OUT_ID);


        public IEnumerable<Pa_Production_Master> Get_ProductionMaster_DAL(string Ref, string StartDate, string EndDate
            , int c_ID);

        public string GetOldTempIDFromProductionDetail_DAL(int? Production_ID);

        public IEnumerable<pa_ProductFormulaMaster_DAL> Get_Formula_DAL();
        #endregion


        #region InventoryTransfer
        public Inventory_Transfer_Master Get_InventoryTransfer_MasterByID_DAL(int? Transfer_ID);
        public IEnumerable<InventoryTransferDetails> Get_InventoryTransfer_DetailListBy_DAL(string Temp_ID, int? Transfer_ID);


        public string InsertInventory_Transfer_Detail_DAL(int? HdTransfer_ID, int ItemId, string UM_ID, string
        MajorQty, string MinorQty, string ItemDesc, string Unit_Price, string Amount, string job_ref, int OldLoc_ID, int NewLoc_ID, string Temp_ID, int c_id, string comments, string quantity);

        public string UpdateInventory_Transfer_Detail_DAL(int? ID, int? Transfer_ID, int ItemId, string ItemDesc, string Unit_Price, string Amount, string job_ref, int OldLoc_ID, int NewLoc_ID, string comments, string quantity);


        public string InsertInventoryTransferMaster_DAL(string transferDate, string Ref, string OtherDetails, int? Loc_ID,
     string created_by, string Temp_ID);
        public string UpdateInventory_TransferMaster_DAL(int Transfer_ID, string transferDate, string Ref, string OtherDetails, int? Loc_ID,
      string Last_Update_by, string Temp_ID);

        public string DeleteInventory_TransferMaster_DAL(int? transfer_ID);
        public string DeleteInventory_Transfer_Details(int? ID);



        public IEnumerable<Inventory_Transfer_Master> Get_Inventory_TransferMaster_DAL();

        public string GetOldTempIDFromInventory_TransferDetail_DAL(int? transfer_ID);
        public Inventory_Transfer_Master TransferLoadRef();
        public IEnumerable<Pa_PurchaseDetails_DAL> GetLocationDetails_DAL(string ItemCode);
        #endregion


        #region Dismental
        public Pa_Dismental_Master Get_DismentalMasterByID_DAL(int? Dismental_ID);
        public IEnumerable<Pa_Dismental_Dtl> Get_Dismental_Material_IN_DetailListBy_DAL(string Temp_ID, int? Dismental_ID);

        public string InsertDismental_Material_In_Detail_DAL(int? HdDismental_ID, int Item_ID, string Cost, string Quantity, string Temp_ID);


        public string UpdateDismental_Material_In_DAL(int? Dismental_Dtl_ID, string ItemID, string Cost, string Quantity);

        public string InsertDismentalMaster_DAL(string P_Date, string Ref, string CustomerRef, string Supervisor,
     string Dismental_Chassis, string Created_By, int? c_id, string Temp_ID);
        public string UpdateDismentalMaster_DAL(int Dismental_ID, string P_Date, string Ref, string CustomerRef, string Supervisor,
     string Dismental_Chassis, string Created_By, int? c_id, string Temp_ID);

        public string DeleteDismentalMaster_DAL(int? Dismental_ID);
        public string DeleteDismental_Material_In_DAL(int? Dismental_Dtl_ID);




        public IEnumerable<Pa_Dismental_Master> Get_DismentalMaster_DAL(string Ref, string StartDate, string EndDate
            , int c_ID);

        public string GetOldTempIDFromDismentalDetail_DAL(int? Dismental_ID);
        public IEnumerable<Pa_Dismental_Dtl> GetFormulaDetails_DAL(int FormulaId);
        public Pa_Dismental_Master DismentalLoadRef();

        #endregion


        #region Assembly

        public Pa_Assembly_Master Get_AssemblyMasterByID_DAL(int? Assembly_ID);
        public IEnumerable<Pa_Assembly_Dtl> Get_Assembly_Material_IN_DetailListBy_DAL(string Temp_ID, int? Assembly_ID);

        public string InsertAssembly_Material_In_Detail_DAL(int? HdAssembly_ID, int Item_ID, string Cost, string Quantity, string Remarks, string Temp_ID);


        public string UpdateAssembly_Material_In_DAL(int? Assembly_Dtl_ID, string ItemID, string Cost, string Quantity);


        public string InsertAssemblyMaster_DAL(string P_Date, string Ref, string CustomerRef, string Supervisor,
     string Assembly_Chassis, string Created_By, int? c_id, string Temp_ID, int ItemID, string Assembly_Type);
        public string InsertAssemblyStockMaster_DAL(string MasterId, string CHASSIS_NO, int make_ID, int MakeModel_description_ID, int ModelYear, int color_exterior_ID, int color_interior_ID);

        public string UpdateAssemblyMaster_DAL(int Assembly_ID, string P_Date, string Ref, string CustomerRef, string Supervisor,
     string Assembly_Chassis, string Created_By, int? c_id, string Temp_ID, int ItemID, string Assembly_Type);

        public string DeleteAssemblyMaster_DAL(int? Assembly_ID);
        public string DeleteAssembly_Material_In_DAL(int? Assembly_Dtl_ID);

        public string DeleteAssembly_Material_Out_DAL(int? Assembly_Dtl_ID);


        public IEnumerable<Pa_Assembly_Master> Get_AssemblyMaster_DAL(string Ref, string StartDate, string EndDate
            , int c_ID);

        public string GetOldTempIDFromAssemblyDetail_DAL(int? Assembly_ID);



        public Pa_Assembly_Master AssemblyLoadRef();
        public Pa_Assembly_Master Get_AssemblyStockMasterByID_DAL(int? Assembly_ID);
        #endregion


      




    }
}
