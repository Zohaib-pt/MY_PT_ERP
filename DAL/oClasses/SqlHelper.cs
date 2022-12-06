using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;


namespace DAL.oClasses
{
    public class SqlHelper : ISqlHelper
    {

        private readonly IConnectionStringManager _conMgr;
        SqlConnection con;

        public SqlHelper(IConnectionStringManager conMgr)
        {

            _conMgr = conMgr;
            //---Getting Connection String
            Constr = _conMgr.GetConnectionString();
        }
        public string Constr { get; set; }


        public SqlConnection connection;

        #region Initiallize
        public SqlHelper()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            Constr = ReadConnectionString();

            connection = new SqlConnection(Constr);
        }

        public String ReadConnectionString()
        {
            return Constr;
        }

        #endregion


        #region DB ConnectionOpen
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (SqlException)
            {

            }
            return false;

        }
        #endregion

        #region DB Connection Close
        //Close connection
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (SqlException)
            {

                return false;
            }
        }


        #endregion



        #region ExecuteNonQuery for insert/Update and Delete
        //For Insert/Update/Delete
        public int ExecuteNonQuery_IUD(String Querys)
        {
            int result = 0;
            //open connection
            if (OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                SqlCommand cmd = new SqlCommand(Querys, connection);

                //Execute command
                result = cmd.ExecuteNonQuery();

                //close connection
                CloseConnection();
            }

            return result;
        }
        #endregion

        #region Dataset for select result and return as Dataset
        //for select result and return as Dataset
        public DataSet DataSet_return(String Querys)
        {
            DataSet ds = new DataSet();

            try
            {
                using (con = new SqlConnection(Constr))
                {

                    con.Open();

                    SqlCommand cmdSel = new SqlCommand(Querys, con);

                    SqlDataAdapter da = new SqlDataAdapter(cmdSel);
                    da.Fill(ds);
                    con.Close();
                    return ds;

                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                return null;
            }



        }
        #endregion

        #region DataTable for select result and return as DataTable
        //for select result and return as DataTable
        public DataTable DataTable_return(String Querys)
        {
            DataTable dt = new DataTable();
            //open connection
            if (OpenConnection() == true)
            {
                //for Select Query               

                SqlCommand cmdSel = new SqlCommand(Querys, connection);

                SqlDataAdapter da = new SqlDataAdapter(cmdSel);
                da.Fill(dt);
                //close connection
                CloseConnection();
            }
            return dt;
        }
        #endregion


        #region DataTable for select result and return as DataTable
        //for select result and return as DataTable
        public DataSet SP_DataTable_return(String ProcName, params SqlParameter[] commandParameters)
        {
            try
            {

                DataSet ds = new DataSet();


                using (con = new SqlConnection(Constr))
                {

                    con.Open();

                    SqlCommand cmdSel = new SqlCommand(ProcName, con);
                    cmdSel.CommandType = CommandType.StoredProcedure;
                    // Assign the provided values to these parameters based on parameter order
                    AssignParameterValues(commandParameters, commandParameters);
                    AttachParameters(cmdSel, commandParameters);
                    SqlDataAdapter da = new SqlDataAdapter(cmdSel);

                    da.Fill(ds);
                    con.Close();
                    return ds;

                }



            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;

                return null;
            }

        }

        public DataSet SP_DataTable_return_UploadBulk(String ProcName, params SqlParameter[] commandParameters)
        {
            try
            {

                DataSet ds = new DataSet();


                using (con = new SqlConnection(Constr))
                {

                    con.Open();

                    SqlCommand cmdSel = new SqlCommand(ProcName, con);
                    cmdSel.CommandType = CommandType.StoredProcedure;
                    // Assign the provided values to these parameters based on parameter order
                    AssignParameterValues(commandParameters, commandParameters);
                    AttachParameters(cmdSel, commandParameters);
                    SqlDataAdapter da = new SqlDataAdapter(cmdSel);


                  
                    da.SelectCommand.CommandTimeout = 240;


                    da.Fill(ds);
                    con.Close();
                    return ds;

                }



            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;

                return null;
            }

        }
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                // Do nothing if we get no data
                return;
            }

            // We must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // Iterate through the SqlParameters, assigning the values from the corresponding position in the 
            // value array
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if (paramInstance.Value == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }
        #endregion



        #region Methods Parameter

        /// <summary>
        /// This method Sorted-Dictionary key values to an array of SqlParameters
        /// </summary>
        public static SqlParameter[] GetSdParameter(SortedDictionary<string, string> sortedDictionary)
        {
            SqlParameter[] paramArray = new SqlParameter[] { };

            foreach (string key in sortedDictionary.Keys)
            {
                AddParameter(ref paramArray, new SqlParameter(key, sortedDictionary[key]));
            }

            return paramArray;
        }


        public static void AddParameter(ref SqlParameter[] paramArray, string parameterName, object parameterValue)
        {
            SqlParameter parameter = new SqlParameter(parameterName, parameterValue);

            AddParameter(ref paramArray, parameter);
        }


        public static void AddParameter(ref SqlParameter[] paramArray, string parameterName, object parameterValue, object parameterNull)
        {
            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = parameterName;

            if (parameterValue.ToString() == parameterNull.ToString())
                parameter.Value = DBNull.Value;
            else
                parameter.Value = parameterValue;

            AddParameter(ref paramArray, parameter);
        }

        public static void AddParameter(ref SqlParameter[] paramArray, string parameterName, SqlDbType dbType, object parameterValue)
        {
            SqlParameter parameter = new SqlParameter(parameterName, dbType);
            parameter.Value = parameterValue;

            AddParameter(ref paramArray, parameter);
        }

        public static void AddParameter(ref SqlParameter[] paramArray, string parameterName, SqlDbType dbType, ParameterDirection direction, object parameterValue)
        {
            SqlParameter parameter = new SqlParameter(parameterName, dbType);
            parameter.Value = parameterValue;
            parameter.Direction = direction;

            AddParameter(ref paramArray, parameter);
        }

        ////public static void AddParameter(ref SqlParameter[] paramArray, string parameterName, SqlDbType dbType, int size, object parameterValue)
        ////{
        ////    SqlParameter parameter = new SqlParameter(parameterName, dbType, size);
        ////    parameter.Value = parameterValue;

        ////    AddParameter(ref paramArray, parameter);
        ////}

        ////public static void AddParameter(ref SqlParameter[] paramArray, string parameterName, SqlDbType dbType, int size, ParameterDirection direction, object parameterValue)
        ////{
        ////    SqlParameter parameter = new SqlParameter(parameterName, dbType, size);
        ////    parameter.Value = parameterValue;
        ////    parameter.Direction = direction;

        ////    AddParameter(ref paramArray, parameter);
        ////}

        public static void AddParameter(ref SqlParameter[] paramArray, params SqlParameter[] newParameters)
        {
            SqlParameter[] newArray = Array.CreateInstance(typeof(SqlParameter), paramArray.Length + newParameters.Length) as SqlParameter[];
            paramArray.CopyTo(newArray, 0);
            newParameters.CopyTo(newArray, paramArray.Length);

            paramArray = newArray;
        }

        #endregion

        #region ALL Business method here
        public DataSet SelectList(String SP_NAME, SortedDictionary<string, string> sd)
        {
            try
            {
                return SP_DataTable_return(SP_NAME, GetSdParameter(sd));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataTable SelectQuery(String query)
        {
            try
            {
                return DataTable_return(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        #endregion


    }
}
