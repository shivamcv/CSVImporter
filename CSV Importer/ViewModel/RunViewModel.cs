using CSV_Importer.Model;
using CSV_Importer.Properties;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;


namespace CSV_Importer.ViewModel
{
   public class RunViewModel:ViewModelBase
    {
        public Model.TableSetting TableData { get; set; }

       public RunViewModel()
       {
           var temp = Settings.Default.configFiles;

           if (!string.IsNullOrEmpty(temp))
               CsvHistory = new ObservableCollection<string>(temp.Split('|'));
           else
               CsvHistory = new ObservableCollection<string>();

       }

       private ObservableCollection<string> csvHistory;

       public ObservableCollection<string> CsvHistory
       {
           get { return csvHistory; }
           set { csvHistory = value;
           RaisePropertyChanged("CsvHistory");
           }
       }
       
       private string selectedCSV;

       public string SelectedCSV
       {
           get { return selectedCSV; }
           set {
               if (SelectedCSV == value || value == null) return;
               selectedCSV = value;

               loadCSV();
           RaisePropertyChanged("SelectedCSV");
           }
       }

       private int batchSize = 100;

       public int BatchSize
       {
           get { return batchSize; }
           set { batchSize = value; }
       }
       

       private void loadCSV()
       {
           if(File.Exists(SelectedCSV))
           {
               try
               {
                   TableData = HelperClasses.XMLHelper.readDataContractXml<Model.TableSetting>(SelectedCSV);
                   ContentInfo = TableData.ToString();

                   if(!CsvHistory.Contains(SelectedCSV))
                   {
                       CsvHistory.Add(SelectedCSV);
                       Settings.Default.configFiles = string.Join("|", csvHistory);
                       Settings.Default.Save();
                   }

               }
               catch (Exception ex)
               {
                   HelperClasses.ErrorReporting.ReportError(ex, "Couldn't Load data!!", true);
                   ContentInfo = string.Empty;
               }
           }
           else
               ContentInfo = string.Empty;

       }

      

       private string contentInfo;

       public string ContentInfo
       {
           get { return contentInfo; }
           set { contentInfo = value;
           RaisePropertyChanged("ContentInfo");
           }
       }
       
       
       private string log;

       public string Log
       {
           get { return log; }
           set { log = value;
           RaisePropertyChanged("Log");
           }
       }

       private string runStop= "Run";

       public string RunStop
       {
           get { return runStop; }
           set { runStop = value;
           RaisePropertyChanged("RunStop");
           }
       }

       private bool isTaskRunning;

       public bool IsTaskRunning
       {
           get { return isTaskRunning; }
           set { isTaskRunning = value;
           RaisePropertyChanged("IsTaskRunning");
           }
       }

       private string currentTask;

       public string CurrentTask
       {
           get { return currentTask; }
           set { currentTask = value;
           RaisePropertyChanged("CurrentTask");
           }
       }
       

        #region command
       CancellationTokenSource cts;

       private RelayCommand run;
       public RelayCommand Run
       {
           get
           {
               return run ?? (run = new RelayCommand(() =>
                   {
                       if (!IsTaskRunning)
                       {
                           cts =  new CancellationTokenSource();
                           Task t = new Task(async (obj) =>
                           {
                               var cancelToken = (CancellationToken)obj;
                               while (!cancelToken.IsCancellationRequested)
                               {
                                   CurrentTask = "Started";
                                   await DataTransfer.Load(TableData,ProgressChanged, AddLog,Completed, Onerror,cancelToken,BatchSize);
                                   CurrentTask = "Waiting..";
                                   await Task.Delay(TableData.Delay);
                               }

                               RunStop = "Run";
                               IsTaskRunning = false;
                               CurrentTask = "";

                           }, cts.Token);

                           IsTaskRunning = true;
                           t.Start();
                           RunStop = "Stop";
                       }
                       else
                       {
                           RunStop = "Run";
                           cts.Cancel();
                           IsTaskRunning = false;
                           CurrentTask = "";
                       }

                   }, () =>
                       {
                           if (TableData != null && 
                              //!string.IsNullOrEmpty(TableData.AmibrokerDb) &&
                              //!string.IsNullOrEmpty(TableData.AmibrokerExe) &&
                              !string.IsNullOrEmpty(TableData.ConnectionString) &&
                              !string.IsNullOrEmpty(TableData.CSVFolder) &&
                              !string.IsNullOrEmpty(TableData.ProviderConnectionString) &&
                              !string.IsNullOrEmpty(TableData.TableName) &&
                              TableData.Fields.Any())
                               
                               return true;

                           return false;
                       }
                   ));
           }
       }

       private void ProgressChanged(string obj)
       {
           CurrentTask = obj;
       }

       void Completed()
       {

       }

       void Onerror()
       {
           if (cts != null && !cts.IsCancellationRequested)
           {
               cts.Cancel();
               MessageBox.Show("Process halted due to too many errors!!");
           }
       }


       
       


       private void AddLog(string p)
       {
           
           Log += string.Format("\n[{0}] {1}", DateTime.Now.ToString("HH:mm:ss"), p);
       }

       private RelayCommand edit;

       public RelayCommand Edit
       {
           get
           {
               return edit ?? (edit = new RelayCommand(() =>
                   {
                      var vm = SimpleIoc.Default.GetInstance<SaveViewModel>();

                     
                      vm.AmibrokerDB = TableData.AmibrokerDb;
                      vm.AmibrokerPath = TableData.AmibrokerExe;
                      vm.ConnectionString = TableData.ConnectionString;
                      vm.CSVDirectory = TableData.CSVFolder;
                      vm.ProviderConnectionString = TableData.ProviderConnectionString;
                      vm.TableName = TableData.TableName;
                      vm.TableFields =new ObservableCollection<TableField>();
                      vm.TimeDelay = TableData.Delay;
                      vm.IsFirstRowHeader = TableData.IsFirstRowHeader;
                      vm.Delimiter = TableData.Delimiter;

                      foreach (var item in TableData.Fields)
                      {
                          vm.TableFields.Add(new TableField() 
                          {
                              SqlColumnHeader = item.SqlColumnHeader,
                                CsvColumnHeader = null,
                                ColumnInfo = item.ColumnInfo,
                                DatatypeName = item.DatatypeName,
                                _Tag = item._Tag,
                          });
                      }

                      SimpleIoc.Default.GetInstance<MainViewModel>().GotoSave.Execute(null);
                   }, 
                   ()=>
                   {
                       if (TableData != null)
                           return true;
                       return false;
                   }
                   ));
           }
       }
       
     
       private RelayCommand selectCSV;

       public RelayCommand SelectCSV
       {
           get
           {
               return selectCSV ?? (selectCSV = new RelayCommand(() =>
                   {
                       var dig = new OpenFileDialog();
                       dig.Filter = "XML Files (*.xml)|*.xml";

                       if (dig.ShowDialog() == true)
                       {
                           SelectedCSV  = dig.FileName;

                       }
                   }));
           }
       }
        
        
        #endregion

    }
}
