using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace PRone
{
    class XMLxlsOperation
    {
        DataTable dt = new DataTable();
        string testString = "";
        bool first_row = true;

        public void getsubnode(XmlNode vnode, DataTable dTable)
        {
            while (vnode.HasChildNodes == true)
            {
                foreach (XmlNode vchildnode in vnode)
                {
                    if (vchildnode.Name == "Row")
                    {
                        if (first_row)
                            first_row = false;
                        insertDataRow();
                    }
                    if (vchildnode.Name != "WorksheetOptions")
                        getsubnode(vchildnode, dTable);
                }
                return;
            }
            while (vnode.HasChildNodes == false)
            {
                string stringtest = "";
                stringtest = vnode.InnerText + ",";
                if (stringtest != ",")
                {
                    testString += stringtest;
                }
                return;
            }
        }

        public void insertDataRow()
        {
            if (dt.Rows.Count != 0 || testString.Length != 0)
            {
                testString = (testString.Length == 0) ? "" : testString.Substring(0, testString.Length - 1);
                if (testString.Split(',').Length > dt.Columns.Count)
                {
                    for (int i = dt.Columns.Count; i < testString.Split(',').Length; i++)
                    {
                        dt.Columns.Add();
                    }
                }
                dt.Rows.Add(testString.Split(','));
                testString = "";
            }
        }

        public DataTable ReadSheet(string fileName)
        {            
            try
            {
                first_row = true;
                XmlDocument test = new XmlDocument();
                XmlNode test2;
                test.Load(fileName);
                test2 = test.DocumentElement;
                getsubnode(test2, dt);
                insertDataRow();
            }
            catch (Exception exc)
            {
                //MessageBox.Show("1. " + exc.Message);
            }
            finally
            {
                
            }
            return dt;
        }
    }
}
