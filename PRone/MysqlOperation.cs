using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace PRone
{
    public class MysqlOperation
    {
        public DataTable Select(string query, string connectionString)
        {
            MySqlConnection myConnection = new MySqlConnection(connectionString);
            string qString = query.Trim();
            DataTable dtResult = new DataTable();
            try
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand(qString, myConnection);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = myCommand;
                adapter.Fill(dtResult);
                myConnection.Close();
            }
            catch (Exception ex)
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }

            return dtResult;
        }

        public Boolean InUpDelBoolean(string query, string connectionString)
        {
            bool isSuccess = false;
            MySqlConnection myConnection = new MySqlConnection(connectionString);
            string qString = query.Trim();
            try
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand(qString, myConnection);
                myCommand.ExecuteNonQuery();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
                isSuccess = false;
            }
            return isSuccess;
        }
    }
}
