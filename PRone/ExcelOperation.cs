using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Data.OleDb;
using System.Runtime.InteropServices;

namespace PRone
{
    public class ExcelOperation
    {
        public Boolean CreateFile(string fileName, string sheetName, int rowLocationStart, int columnLocationStart, int rowLocationEnd, int columnLocationEnd, DataTable dataCollection)
        {
            bool sucess = false;
            try
            {
                #region Create File with default property
                object misValue = Missing.Value;
                Excel.Application xlAp = new Excel.Application();
                Excel.Workbook xlWb = xlAp.Workbooks.Add(misValue);
                Excel.Worksheet xlWs = (Excel.Worksheet)xlWb.Worksheets.Add(misValue);
                xlWs.Name = sheetName;

                Excel.Worksheet xlWsDel = (Excel.Worksheet)xlWb.Worksheets.get_Item("Sheet1");
                xlWsDel.Delete();
                xlWsDel = (Excel.Worksheet)xlWb.Worksheets.get_Item("Sheet2");
                xlWsDel.Delete();
                xlWsDel = (Excel.Worksheet)xlWb.Worksheets.get_Item("Sheet3");
                xlWsDel.Delete();
                #endregion

                #region Load data from Oracle to Excel File
                for (int c = columnLocationStart; c <= columnLocationEnd ; c++)
                {
                    for (int r = rowLocationStart; r <= rowLocationEnd; r++)
                    {
                        xlWs.Cells[r + 1, c + 1] = "";
                    }
                }

                for (int c = 0; c < dataCollection.Columns.Count; c++)
                {
                    xlWs.Cells[rowLocationStart + 1, columnLocationStart + c + 1] = dataCollection.Columns[c].ColumnName.Replace("_", " ");
                    for (int r = 0; r < dataCollection.Rows.Count; r++)
                    {
                        xlWs.Cells[rowLocationStart + r + 2, columnLocationStart + c + 1] = dataCollection.Rows[r][c].ToString();
                    }
                }
                #endregion

                #region Save Excel file
                xlWs.Cells.get_Range("A1", "AZ999").EntireColumn.AutoFit();
                xlWs.Cells.get_Range("A1", "AZ1").Font.Bold = true;
                if (File.Exists(@fileName))
                {
                    File.Delete(@fileName);
                    xlWb.SaveAs(@fileName);
                }
                //xlWs.Cells.get_Range("A1", "A99").Interior.Color = Color.Blue;
                else
                {
                    xlWb.SaveAs(@fileName);
                }
                xlWb.Close();
                xlAp.Quit();
                #endregion

                sucess = true;
            }
            catch (Exception ex)
            {
                sucess = false;
            }
            return sucess;         
        }

        public Boolean DeleteFile(string fileName)
        {
            return true;
        }

        public Boolean CreateSheet(string fileName, string sheetName, int rowLocationStart, int columnLocationStart, int rowLocationEnd, int columnLocationEnd, DataTable dataCollection)
        {
            bool sucess = false;
            try
            {
                #region Create Sheet with default property
                object misValue = Missing.Value;
                Excel.Application xlAp = new Excel.Application();
                Excel.Workbook xlWb = xlAp.Workbooks.Open(fileName, misValue, false);
                Excel.Worksheet xlWs = (Excel.Worksheet)xlWb.Worksheets.Add(misValue);
                xlWs.Name = sheetName;
                #endregion

                #region Load data from Oracle to Excel File
                for (int c = columnLocationStart; c <= columnLocationEnd; c++)
                {
                    for (int r = rowLocationStart; r <= rowLocationEnd; r++)
                    {
                        xlWs.Cells[r + 1, c + 1] = "";
                    }
                }

                for (int c = 0; c < dataCollection.Columns.Count; c++)
                {
                    xlWs.Cells[rowLocationStart + 1, columnLocationStart + c + 1] = dataCollection.Columns[c].ColumnName.Replace("_", " ");
                    for (int r = 0; r < dataCollection.Rows.Count; r++)
                    {
                        xlWs.Cells[rowLocationStart + r + 2, columnLocationStart + c + 1] = dataCollection.Rows[r][c].ToString();
                    }
                }
                #endregion

                #region Save Excel file
                xlWs.Cells.get_Range("A1", "AZ999").EntireColumn.AutoFit();
                xlWs.Cells.get_Range("A1", "AZ1").Font.Bold = true;
                xlWb.Save();
                xlWb.Close();
                xlAp.Quit();
                #endregion
            }
            catch (Exception ex)
            {
                sucess = false;
            }
            return sucess;         
        }

        public Boolean DeleteSheet(string fileName, string sheetName)
        {
            bool sucess = false;
            try
            {
                #region Create Sheet with default property
                object misValue = Missing.Value;
                Excel.Application xlAp = new Excel.Application();
                Excel.Workbook xlWb = xlAp.Workbooks.Open(fileName, false, false, misValue, misValue, misValue, true, misValue, misValue, true, misValue, misValue, misValue, misValue, misValue);

                Excel.Worksheet xlWs = (Excel.Worksheet)xlWb.Worksheets.get_Item(sheetName);
                xlWs.Delete();
                #endregion

                #region Save Excel file
                xlWb.Save();
                //xlWb.Close();
                xlAp.Quit();
                #endregion
            }
            catch (Exception ex)
            {
                sucess = false;
            }
            return sucess;
        }

        public Boolean UpdateSheet(string fileName, string sheetName, int rowLocationStart, int columnLocationStart, int rowLocationEnd, int columnLocationEnd, DataTable dataCollection)
        {
            bool sucess = false;
            try
            {
                #region Create Sheet with default property
                object misValue = Missing.Value;
                Excel.Application xlAp = new Excel.Application();
                Excel.Workbook xlWb = xlAp.Workbooks.Open(fileName, false, false,misValue,misValue,misValue,true,misValue,misValue,true,misValue,misValue,misValue,misValue,misValue);
                Excel.Worksheet xlWs = (Excel.Worksheet)xlWb.Worksheets.get_Item(sheetName);
                xlWs.Name = sheetName;
                #endregion

                #region Load data from Oracle to Excel File
                rowLocationStart = rowLocationStart + dataCollection.Rows.Count;
                for (int c = columnLocationStart; c <= columnLocationEnd; c++)
                {
                    for (int r = rowLocationStart; r <= rowLocationEnd; r++)
                    {
                        xlWs.Cells[r + 1, c + 1] = "";
                    }
                }

                //for (int c = 0; c < dataCollection.Columns.Count; c++)
                //{
                //    xlWs.Cells[rowLocation + 1, columnLocation + c + 1] = dataCollection.Columns[c].ColumnName.Replace("_", " ");
                //    for (int r = 0; r < dataCollection.Rows.Count; r++)
                //    {
                //        xlWs.Cells[r + 1, columnLocationStart + c + 1] = dataCollection.Rows[r][c].ToString();
                //    }
                //}
                #endregion

                #region Save Excel file
                xlWs.Cells.get_Range("A1", "AZ999").EntireColumn.AutoFit();
                xlWs.Cells.get_Range("A1", "AZ1").Font.Bold = true;
                //xlWb.UpdateLink(misValue,misValue);
                xlWb.Save();
                //xlWb.Close(misValue,misValue,misValue);
                xlAp.Quit();
                #endregion
            }
            catch (Exception ex)
            {
                sucess = false;
            }
            return sucess;
        }

        public DataTable ReadSheet(string fileName, string sheetName)
        {
            //string strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";";
            //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + fileName + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";

            //if (sheetName == "")
            //{
            //    object misValue = Missing.Value;
            //    Excel.Application xlAp = new Excel.Application();
            //    Excel.Workbook xlWb = xlAp.Workbooks.Open(fileName, false, false, misValue, misValue, misValue, true, misValue, misValue, true, misValue, misValue, misValue, misValue, misValue);
            //    Excel.Worksheet xlWs = (Excel.Worksheet)xlWb.Worksheets.get_Item(1);
            //    sheetName = xlWs.Name;
            //    xlWb.Save();
            //    xlWb.Close(misValue, misValue, misValue);
            //    xlWb.Close(true, misValue, misValue);
            //    Marshal.ReleaseComObject(xlWs);
            //    xlAp.Quit();
            //}
            OleDbConnection conn = null;
            OleDbCommand cmd = null;
            OleDbDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                conn = new OleDbConnection(strConn);
                conn.Open();
                dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                cmd = new OleDbCommand("SELECT * FROM [" + sheetName + "$]", conn);
                cmd.CommandType = CommandType.Text;
                da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception exc)
            {
                //MessageBox.Show("1. " + exc.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                conn.Dispose();
                cmd.Dispose();
                da.Dispose();
            }
            return dt;
        }

        public List<string> ListSheetInExcel(string fileName)
        {
            OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
            String strExtendedProperties = String.Empty;
            sbConnection.DataSource = fileName;
            if (Path.GetExtension(fileName).Equals(".xls"))//for 97-03 Excel file
            {
                sbConnection.Provider = "Microsoft.Jet.OLEDB.4.0";
                strExtendedProperties = "Excel 8.0;HDR=Yes;IMEX=1";//HDR=ColumnHeader,IMEX=InterMixed
            }
            else if (Path.GetExtension(fileName).Equals(".xlsx"))  //for 2007 Excel file
            {
                sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
                strExtendedProperties = "Excel 12.0;HDR=Yes;IMEX=1";
            }
            sbConnection.Add("Extended Properties", strExtendedProperties);
            List<string> listSheet = new List<string>();
            using (OleDbConnection conn = new OleDbConnection(sbConnection.ToString()))
            {
                conn.Open();
                DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //List<string> listSheet = new List<string>();
                foreach (DataRow drSheet in dtSheet.Rows)
                {
                    if (drSheet["TABLE_NAME"].ToString().Trim().EndsWith("$") || drSheet["TABLE_NAME"].ToString().Trim().EndsWith("$'"))//checks whether row contains '_xlnm#_FilterDatabase' or sheet name(i.e. sheet name always ends with $ sign)
                    {
                        listSheet.Add(drSheet["TABLE_NAME"].ToString());
                    }
                }
            }
            return listSheet;
        }
    }
}
