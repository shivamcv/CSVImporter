using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Importer.Model
{
  public  class TableSetting
    {
        public string ProviderConnectionString { get; set; }
        public string ConnectionString { get; set; }
        public string TableName { get; set; }
        public bool IsFirstRowHeader { get; set; }
        public ObservableCollection<TableField> Fields { get; set; }
        public string CSVFolder { get; set; }
        public int Delay { get; set; }
        public string AmibrokerDb { get; set; }
        public string AmibrokerExe { get; set; }
        public string Delimiter { get; set; }
        public override string ToString()
        {
            var temp = new StringBuilder();

            SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder();

            conn.ConnectionString = ProviderConnectionString;

            temp.Append(string.Format("Server : {0} \nDatabase : {1}", conn.DataSource, conn.InitialCatalog));
            temp.Append(string.Format("Table Name: {0} \n", TableName));
            temp.Append(string.Format("Ignore First Row: {0} \n", IsFirstRowHeader));
            temp.Append(string.Format("Csv Folder: {0} \n", CSVFolder));
            temp.Append(string.Format("Amibroker Folder: {0} \n", AmibrokerDb));
            temp.Append(string.Format("Amibroker EXE: {0} \n", AmibrokerExe));
            temp.Append(string.Format("Time Delay: {0} milliseconds \n", Delay));
            temp.Append(string.Format("Delimiter: {0}  \n", Delimiter));
            temp.Append(string.Format("=============================== \n", Delay));

            foreach (var item in Fields.Where(p=>p.CsvColumnHeader!=null && p.CsvColumnHeader.ColumnNumber!=-1))
            {
                temp.Append(string.Format("{0} - {1} - {2}\n", item.SqlColumnHeader,item.CsvColumnHeader.Name, item._Tag));
            }

            temp.Append(string.Format("=============================== \n", Delay));

            return temp.ToString();
        }
    }

    public class TableField
    {
        public CSVHeader CsvColumnHeader { get; set; }
        public string SqlColumnHeader { get; set; }
        string t;
        public string _Tag {
            get { 
                return t; }
            set { 
                t = value; }
        }
        public Dictionary<string,string> ColumnInfo { get; set; }
        public string DatatypeName { get; set; }
        public string _Value { get; set; }
        public int ColumnNumber { get; set; }
        public TableField()
        {
            ColumnInfo = new Dictionary<string, string>();
        }
    }
}
