using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace PRone
{
    public class PostgresOperation
    {
        public DataTable Select(string query, string connectionString)
        {
            string qString = query.Trim();
            DataTable dtResult = new DataTable();
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionString);
            try
            {
                npgsqlConnection.Open();
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(qString, npgsqlConnection);

                NpgsqlDataReader dr = npgsqlCommand.ExecuteReader();
                //dtResult.Load((IDataReader)dr,LoadOption.OverwriteChanges);
                DataSet ds = new DataSet();
                ds.EnforceConstraints = false;
                ds.Tables.Add(dtResult);
                dtResult.Load(dr,LoadOption.OverwriteChanges);
                ds.Tables.Remove(dtResult);
            }
            catch (Exception ex)
            {
                dtResult = null;
                //Console.WriteLine($" {ex.Message}");
            }
            finally
            {
                if (npgsqlConnection != null && npgsqlConnection.State == ConnectionState.Open)
                {
                    npgsqlConnection.Close();
                }                
            }
            return dtResult;
        }

        public Boolean InUpDelBoolean(string query, string connectionString)
        {
            bool isSuccess = false;
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionString);
            string qString = query.Trim();
            try
            {
                npgsqlConnection.Open();
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(qString, npgsqlConnection);
                npgsqlCommand.ExecuteNonQuery();
                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            finally
            {
                if (npgsqlConnection != null && npgsqlConnection.State == ConnectionState.Open)
                {
                    npgsqlConnection.Close();
                }
            }
            return isSuccess;
        }

        public int InUpDelImpacted(string query, string connectionString)
        {
            int impactedRows = 0;
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionString);
            string qString = query.Trim();
            try
            {
                npgsqlConnection.Open();
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(qString, npgsqlConnection);
                impactedRows = npgsqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                impactedRows = 0;
            }
            finally
            {
                if (npgsqlConnection != null && npgsqlConnection.State == ConnectionState.Open)
                {
                    npgsqlConnection.Close();
                }
            }
            return impactedRows;
        }
    }
}
