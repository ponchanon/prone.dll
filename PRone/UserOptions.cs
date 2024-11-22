using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PRone
{
    public class UserOption
    {
        public Boolean RectangularDatatable(DataTable dataCollection)
        {
            bool sucess = false;
            try
            {
                int rCount = Convert.ToInt32(dataCollection.Rows.Count.ToString());
                int cCount = Convert.ToInt32(dataCollection.Columns.Count.ToString());
                for (int i = 0; i < rCount; i++)
                {
                    if (dataCollection.Rows[i][cCount - 1].ToString().Trim() == "")
                    {
                        dataCollection.Rows[i].BeginEdit();
                        dataCollection.Rows[i].Delete();
                        dataCollection.Rows[i].EndEdit();
                    }
                }
                dataCollection.AcceptChanges();
                for (int i = 0; i < cCount; i++)
                {
                    dataCollection.Columns[i].ColumnName = dataCollection.Rows[0][i].ToString();
                }
                dataCollection.Rows[0].BeginEdit();
                dataCollection.Rows[0].Delete();
                dataCollection.Rows[0].EndEdit();
                dataCollection.AcceptChanges();
                sucess = true;
            }
            catch (Exception ex)
            {
                sucess = false;
            }
            return sucess;
        }

        public DataTable ConvertListToDataTable(List<string[]> list)
        {
            DataTable table = new DataTable();
            int columns = 0;
            foreach (var array in list)
            {
                if (array.Length > columns)
                {
                    columns = array.Length;
                }
            }

            foreach (var array in list)
            {
                if (list.IndexOf(array) == 0)
                {
                    foreach (var cl in array)
                    {
                        table.Columns.Add(cl.Replace("\"", "").Trim());
                    }
                }
                else
                    table.Rows.Add(array);
            }
            return table;
        }
    }
}
