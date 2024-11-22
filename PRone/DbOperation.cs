using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.IO;

namespace PRone
{
    public class DbOperation
    {
        //string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.10.217.59)(PORT=1521)) (CONNECT_DATA=(SERVER = DEDICATED)(SERVICE_NAME = CNDB)));User Id=cn_tool;Password=cn_tool;Integrated Security=no;";
        //string connectionString = "Data Source=10.10.217.60/cndb;User Id=cn_tool;Password=cn_tool;Integrated Security=no;";
        public DataTable Select(string query, string connectionString)
        {
            DataSet ds = new DataSet();
            DataTable dtError = new DataTable();
            string qString = query.Trim();
            try
            {
                OracleConnection connection = new OracleConnection(connectionString);
                if (qString != "")
                {
                    OracleDataAdapter oda = new OracleDataAdapter(query, connectionString);
                    oda.Fill(ds);
                }
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                string[] array = ex.Message.ToString().Split('^');
                dtError.Columns.Add(new DataColumn(ex.Message.ToString()));
                return dtError;
            }
            
            //return ds.Tables[0];
        }

        public Boolean InUpDelBoolean(string query, string connectionString)
        {
            bool isSuccess = false;
            string connSt = "";
            OracleConnection connection = new OracleConnection(connectionString);
            OracleCommand command;
            try
            {
                string[] qString = query.Trim().Split(';');
                foreach (string insertquery in qString)
                {
                    if (insertquery.Trim() != "")
                    {
                        command = new OracleCommand(insertquery.Trim());
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                isSuccess = false;
            }
            return isSuccess;
        }

        public string InUpDel(string query, string connectionString)
        {
            string isSuccess = "false";
            string connSt = "";
            OracleConnection connection = new OracleConnection(connectionString);
            OracleCommand command;
            try
            {
                string[] qString = query.Trim().Split(';');
                foreach (string insertquery in qString)
                {
                    if (insertquery.Trim() != "")
                    {
                        command = new OracleCommand(insertquery.Trim());
                        command.Connection = connection;
                        connection.Open();
                        isSuccess = command.ExecuteNonQuery().ToString();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                isSuccess = ex.Message;
            }
            return isSuccess;
        }

        public int InUpDel(DataTable sourceTable,string tableName, string sourceColumns, string destinationColumns, string connectionString)
        {
            int totalRows = 0;
            if (!sourceTable.Columns[0].ColumnName.Contains("ERROR"))
            {
                string[] queryList = new string[0];
                for (int rowNum = 0; rowNum< sourceTable.Rows.Count; rowNum++)
                {
                    string columnString = "";
                    foreach (string cName in sourceColumns.Trim().Split(','))
                    {
                        string tempColumnString = "";
                        if (cName.ToString().StartsWith("@"))
                        {
                            tempColumnString = cName.ToString().Replace("@", "");
                        }
                        else
                        {
                            tempColumnString = (sourceTable.Rows[rowNum][cName].ToString() == "") ? "null" : (sourceTable.Rows[rowNum][cName].ToString());
                        }
                        
                        columnString += tempColumnString + ((Array.LastIndexOf(sourceColumns.Trim().Split(','), cName) == sourceColumns.Trim().Split(',').Length - 1) ? "" : ",");
                    }
                    Array.Resize(ref queryList, queryList.Length + 1);
                    queryList[queryList.Length - 1] = columnString;
                }
                StreamWriter sw = new StreamWriter("error.log", true);
                try
                {
                    //string oFee = dop.InUpDel("DELETE FROM " + tableName + " WHERE DATES = TO_DATE('" + dataTable.Rows[1]["Result Time"] + "','yyyy-mm-dd HH24:MI') " + deleteColumn, connectionString);
                    for (int i = 0; i<queryList.Length; i++)
                    {
                        string oracleFee = InUpDel("INSERT INTO " + tableName + " (" + destinationColumns + ") VALUES (" + queryList[i] + ") ", connectionString);
                        if (oracleFee.StartsWith("ORA"))
                        {
                            sw.WriteLine(DateTime.Now.ToString("yyMMddHHmmss\t1\t") + oracleFee.Replace("\n", "") + "\n\t\t\t\t" + "INSERT INTO " + tableName + " (" + destinationColumns + ")\n\t\t\t\tVALUES (" + queryList[i] + ")\n");
                            if (oracleFee.StartsWith("ORA-00001:")) //unique constant error in oracle
                            { break; }                                
                        }
                        else totalRows += 1;
                    }  
                }
                catch(Exception ex)
                {
                    sw.WriteLine(DateTime.Now.ToString("yyMMddHHmmss\t2\t") + ex.Message);
                }
                sw.Close();
                sw.Dispose();
            }
            else
            {
                totalRows = 0;
            }
            return totalRows;
        }
    }
}
