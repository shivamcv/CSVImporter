using System;
using System.Collections.Generic;
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
        public List<TableField> Fields { get; set; }
    }

    public class TableField
    {
        public string CsvColumnHeader { get; set; }
        public string SqlColumnHeader { get; set; }
        public string Tag { get; set; }
    }
}
