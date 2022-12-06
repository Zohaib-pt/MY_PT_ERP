using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.oClasses
{
    public interface ISqlHelper
    {
        private void Initialize()
        {

        }


        public String ReadConnectionString();

        public bool OpenConnection();



        public bool CloseConnection();




        public int ExecuteNonQuery_IUD(String Querys);

        public DataSet DataSet_return(String Querys);


        public DataTable DataTable_return(String Querys);


        public DataSet SP_DataTable_return(String ProcName, params SqlParameter[] commandParameters);

        public DataSet SP_DataTable_return_UploadBulk(String ProcName, params SqlParameter[] commandParameters);

        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {

        }

        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {

        }

        public static SqlParameter[] GetSdParameter(SortedDictionary<string, string> sortedDictionary)
        {
            SqlParameter[] paramArray = new SqlParameter[] { };
            return paramArray;
        }

        public static void AddParameter(ref SqlParameter[] paramArray, string parameterName, object parameterValue)
        {

        }

        public static void AddParameter(ref SqlParameter[] paramArray, string parameterName, object parameterValue, object parameterNull)
        {

        }


        public static void AddParameter(ref SqlParameter[] paramArray, string parameterName, SqlDbType dbType, object parameterValue)
        {

        }


        public static void AddParameter(ref SqlParameter[] paramArray, string parameterName, SqlDbType dbType, ParameterDirection direction, object parameterValue)
        {

        }



        public static void AddParameter(ref SqlParameter[] paramArray, params SqlParameter[] newParameters)
        {
        }



        public DataSet SelectList(String SP_NAME, SortedDictionary<string, string> sd);

        public DataTable SelectQuery(String query);




    }
}
