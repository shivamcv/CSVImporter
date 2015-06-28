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

            return temp.ToString();
        }
    }

    public class TableField
    {
        public string CsvColumnHeader { get; set; }
        public string SqlColumnHeader { get; set; }
        public string Tag { get; set; }
    }
}
