using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Importer.ViewModel
{
   public class RunViewModel:ViewModelBase
    {
       public RunViewModel()
       {

       }

       private string selectedCSV;

       public string SelectedCSV
       {
           get { return selectedCSV; }
           set { selectedCSV = value;
           RaisePropertyChanged("SelectedCSV");
           }
       }

       private string cSVDirectory;

       public string CSVDirectory
       {
           get { return cSVDirectory; }
           set { cSVDirectory = value;
           RaisePropertyChanged("CSVDirectory");
           }
       }

       private string amibrokerDB;

       public string AmibrokerDB
       {
           get { return amibrokerDB; }
           set { amibrokerDB = value;
           RaisePropertyChanged("AmibrokerDB");
           }
       }

       private string amibrokerPath;

       public string AmibrokerPath
       {
           get { return amibrokerPath; }
           set { amibrokerPath = value;
           RaisePropertyChanged("AmibrokerPath");
           }
       }

       private int timeDelay;

       public int TimeDelay
       {
           get { return timeDelay; }
           set { timeDelay = value;
           RaisePropertyChanged("TimeDelay");
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


        #region command
       private RelayCommand run;

       public RelayCommand Run
       {
           get
           {
               return run ?? (run = new RelayCommand(() =>
                   {

                   }));
           }
       }

       private RelayCommand selectAmibrokerPath;

       public RelayCommand SelectAmibrokerPath
       {
           get
           {
               return selectAmibrokerPath ?? (selectAmibrokerPath = new RelayCommand(() =>
                   {

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

                   }));
           }
       }

       private RelayCommand selectCSVDirectory;

       public RelayCommand SelectCSVDirectory
       {
           get { return selectCSVDirectory??(selectCSVDirectory = new RelayCommand(()=>
               {

               })); }
       }

       private RelayCommand selectCSV;

       public RelayCommand SelectCSV
       {
           get
           {
               return selectCSV ?? (selectCSV = new RelayCommand(() =>
                   {

                   }));
           }
       }
        
        
        #endregion
    }
}
