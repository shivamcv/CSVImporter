using CSV_Importer.HelperClasses;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Importer.ViewModel
{
   public class SaveViewModel:ViewModelBase
    {
       public SaveViewModel()
       {
           TableFields = new ObservableCollection<Model.TableField>();
       }

        #region Properties
           private IEnumerable<string> tableList;
           public IEnumerable<string> TableList
           {
               get { return tableList; }
               set { tableList = value;
               RaisePropertyChanged("TableList");
               }
           }

           private ObservableCollection<Model.TableField> tableFields;

           public ObservableCollection<Model.TableField> TableFields
           {
               get { return tableFields; }
               set { tableFields = value;
               RaisePropertyChanged("TableFields");
               }
           }

           private IEnumerable<Model.CSVHeader> csvHeaders;
           public IEnumerable<Model.CSVHeader> CSVHeaders
           {
               get { return csvHeaders; }
               set { csvHeaders = value;
               RaisePropertyChanged("CSVHeaders");
               }
           }
           

           private string tableName;
           public string TableName
           {
               get { return tableName; }
               set { tableName = value;

                   using (var conn = new SqlConnection(ProviderConnectionString))
                   {
                       conn.Open();
                       var cmd = new SqlCommand("SELECT * from "+ value , conn);
                       var rdr = cmd.ExecuteReader();

                         var schemaTable = rdr.GetSchemaTable();

                       TableFields.Clear();
                         foreach (DataRow myField in schemaTable.Rows)
                         {
                             var temp = new Model.TableField();
                             foreach (DataColumn myProperty in schemaTable.Columns)
                             {
                                 if (myProperty.ColumnName == "ColumnName")
                                     temp.SqlColumnHeader = myField[myProperty].ToString();

                                 if(myProperty.ColumnName == "DataTypeName")
                                     temp.DatatypeName = myField[myProperty].ToString();

                                 temp.ColumnInfo.Add(myProperty.ColumnName, myField[myProperty].ToString());
                             }
                             TableFields.Add(temp);
                         }
                       
                       rdr.Close();
                       conn.Close();
                   }
                        RaisePropertyChanged("TableName");
               }
           }

           private string delimiter = ",";
           public string Delimiter
           {
               get { return delimiter; }
               set
               {
                   delimiter = value;
                   FillCSVheaders();
                   RaisePropertyChanged("Delimiter");
               }
           }

           private string connectionDetails;
           public string ConnectionDetails
           {
               get { return connectionDetails; }
               set { connectionDetails = value;
               RaisePropertyChanged("ConnectionDetails");
               }
           }

           private string csvFilePath;

           public string CsvFilePath
           {
               get { return csvFilePath; }
               set { csvFilePath = value;
               RaisePropertyChanged("CsvFilePath");
               }
           }
           

           private string csvFilename;
           public string CSVFilename
           {
               get { return csvFilename; }
               set { csvFilename = value;
               RaisePropertyChanged("CSVFilename");
               }
           }

           private bool isFirstRowHeader;

           public bool IsFirstRowHeader
           {
               get { return isFirstRowHeader; }
               set { isFirstRowHeader = value;
               RaisePropertyChanged("IsFirstRowHeader");
               }
           }
           
           private string providerConnectionString;

           public string ProviderConnectionString
           {
               get { return providerConnectionString; }
               set {
                   if (string.IsNullOrEmpty(value) || providerConnectionString == value) return;
                   providerConnectionString = value;
                   InitialiseConnection();
               }
           }
           
           public string ConnectionString { get; set; }

           private string cSVDirectory;

           public string CSVDirectory
           {
               get { return cSVDirectory; }
               set
               {
                   cSVDirectory = value;
                   RaisePropertyChanged("CSVDirectory");
               }
           }

           private string amibrokerDB;

           public string AmibrokerDB
           {
               get { return amibrokerDB; }
               set
               {
                   amibrokerDB = value;
                   RaisePropertyChanged("AmibrokerDB");
               }
           }

           private string amibrokerPath;

           public string AmibrokerPath
           {
               get { return amibrokerPath; }
               set
               {
                   amibrokerPath = value;
                   RaisePropertyChanged("AmibrokerPath");
               }
           }

           private int timeDelay;

           public int TimeDelay
           {
               get { return timeDelay; }
               set
               {
                   timeDelay = value;
                   RaisePropertyChanged("TimeDelay");
               }
           }

        #endregion

        #region Commands
           private RelayCommand connectDatabase;
           public RelayCommand ConnectDatabase
           {
               get
               {
                   return connectDatabase ?? (connectDatabase = new RelayCommand(() =>
                       {
                          var temp = SimpleIoc.Default.GetInstance<Connection>().loadConnectionString();

                          if (temp == null) return;

                          ConnectionString = temp.ConnectionString;
                          ProviderConnectionString =temp.ProviderConnectionString;
                       }));
               }
           }

           private void InitialiseConnection()
           {
               SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder();
               conn.ConnectionString = ProviderConnectionString;
               ConnectionDetails = string.Format("Server : {0} \n Database : {1}", conn.DataSource, conn.InitialCatalog);

               using (var db = new SqlConnection(conn.ConnectionString))
               {
                   db.Open();
                   var cmd = new SqlCommand("SELECT * FROM information_schema.tables", db);
                   var reader = cmd.ExecuteReader();
                   var tempList = new List<string>();
                   while (reader.Read())
                   {
                       tempList.Add(reader[2].ToString());
                   }
                   reader.Close();
                   TableList = tempList;
                   getDatatypes();
                   db.Close();
               }
           }

           private void getDatatypes()
           {
               using (var conn = new SqlConnection(ProviderConnectionString))
               {
                       conn.Open();
                       var dataTypes = new List<string>();
                       foreach (var value in TableList)
                       {
                           var cmd = new SqlCommand("SELECT * from " + value, conn);
                           var rdr = cmd.ExecuteReader();

                           var schemaTable = rdr.GetSchemaTable();
                      
                           foreach (DataRow myField in schemaTable.Rows)
                           {
                               foreach (DataColumn myProperty in schemaTable.Columns)
                               {
                                   if (myProperty.ColumnName == "DataTypeName" && !dataTypes.Contains(myField[myProperty].ToString()))
                                      dataTypes.Add(myField[myProperty].ToString());
                               }
                           }
                            rdr.Close();
                       }
                   conn.Close();
               }
           }

           private RelayCommand saveCommand;
           public RelayCommand SaveCommand
           {
               get
               {
                   return saveCommand ?? (saveCommand = new RelayCommand(() =>
                       {
                           var dig = new SaveFileDialog();
                           dig.DefaultExt = "xml";
                           dig.Filter = "XML Files (*.xml)|*.xml";

                           if(dig.ShowDialog() == true)
                           {
                               var temp = new Model.TableSetting();
                               temp.ConnectionString = ConnectionString;
                               temp.Fields = TableFields;
                               temp.ProviderConnectionString = ProviderConnectionString;
                               temp.TableName = TableName;
                               temp.IsFirstRowHeader = IsFirstRowHeader;
                               temp.CSVFolder = CSVDirectory;
                               temp.AmibrokerExe = AmibrokerPath;
                               temp.AmibrokerDb = AmibrokerDB;
                               temp.Delay = timeDelay;
                               temp.Delimiter = Delimiter;
                               XMLHelper.writeDataContractXml(temp, dig.FileName);
                           }
                           Reset();
                       }));
               }
             
           }

           private void Reset()
           {
               TableFields.Clear();

           }

           private RelayCommand selectCSV;

           public RelayCommand SelectCSV
           {
               get
               {
                   return selectCSV ?? (selectCSV = new RelayCommand(() =>
                       {
                           var dig = new OpenFileDialog();

                           dig.Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

                           if(dig.ShowDialog() == true)
                           {
                               CsvFilePath = dig.FileName;
                               CSVFilename = Path.GetFileName(dig.FileName);

                               FillCSVheaders();
                               if (string.IsNullOrEmpty(CSVDirectory))
                                   CSVDirectory = Path.GetDirectoryName(dig.FileName);
                           }
                       }));
               }
           }

           private void FillCSVheaders()
           {
              try
              {
                  var temp = File.ReadAllLines(CsvFilePath).FirstOrDefault();

                  if(temp != null)
                  {
                     var t=  temp.Split(new string[]{Delimiter},StringSplitOptions.RemoveEmptyEntries).ToList();

                     var h = new List<Model.CSVHeader>();

                     for (int i = 0; i < t.Count(); i++)
                     {
                         h.Add(new Model.CSVHeader(i,t[i]));
                     }
                     h.Add(new Model.CSVHeader(101, "CSVFileName"));
                     h.Add(new Model.CSVHeader(102, "CurrentDateTime"));
                     h.Add(new Model.CSVHeader(-1, "Empty"));
                     CSVHeaders = h;
                  }
              }
               catch(Exception ex)
              {
                  ErrorReporting.ReportError(ex);
              }
           }

           private RelayCommand selectAmibrokerPath;

           public RelayCommand SelectAmibrokerPath
           {
               get
               {
                   return selectAmibrokerPath ?? (selectAmibrokerPath = new RelayCommand(() =>
                   {
                       var dig = new OpenFileDialog();
                       dig.Filter = "Executable Files (*.exe)|*.exe";

                       if (dig.ShowDialog() == true)
                       {
                           AmibrokerPath = dig.FileName;
                       }
                   }));
               }
           }

           private RelayCommand selectAmibrokerDB;

           public RelayCommand SelectAmibrokerDB
           {
               get
               {
                   return selectAmibrokerDB ?? (selectAmibrokerDB = new RelayCommand(() =>
                   {
                       var dig = new System.Windows.Forms.FolderBrowserDialog();

                       if (dig.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                       {
                           AmibrokerDB = dig.SelectedPath;
                       }
                   }));
               }
           }

           private RelayCommand selectCSVDirectory;

           public RelayCommand SelectCSVDirectory
           {
               get
               {
                   return selectCSVDirectory ?? (selectCSVDirectory = new RelayCommand(() =>
                   {
                       var dig = new System.Windows.Forms.FolderBrowserDialog();

                       if (dig.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                       {
                           CSVDirectory = dig.SelectedPath;
                       }
                   }));
               }
           }

        
        #endregion
    }
}
