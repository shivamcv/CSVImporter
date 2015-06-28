using CSV_Importer.Model;
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

       private void loadCSV()
       {
           if(File.Exists(SelectedCSV))
           {
               try
               {
                   TableData = HelperClasses.XMLHelper.readXml<Model.TableSetting>(SelectedCSV);
                   ContentInfo = TableData.ToString();

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
                                   AddLog("Opening Connection");
                                   await Task.Delay(500);
                               }
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
                       }

                   }, () =>
                       {
                           if (TableData != null && !string.IsNullOrEmpty(TableData.AmibrokerDb) &&
                              !string.IsNullOrEmpty(TableData.AmibrokerExe) &&
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
                      vm.TableFields =TableData.Fields;
                      vm.TimeDelay = TableData.Delay;
                      vm.IsFirstRowHeader = TableData.IsFirstRowHeader;

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
