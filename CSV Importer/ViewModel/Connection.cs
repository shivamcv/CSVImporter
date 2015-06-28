using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace CSV_Importer.ViewModel
{
   public class Connection:ViewModelBase
    {
       Window window;
        public Connection()
       {
           Task.Factory.StartNew(() =>
               {
                   CurrentCursor = Cursors.Wait;
                   Servers = SqlDataSourceEnumerator.Instance.GetDataSources().Rows
                                                                               .Cast<DataRow>()
                                                                               .Select(x=>x["ServerName"] + "\\" + x["InstanceName"]);
                   CurrentCursor = Cursors.Arrow;
               });
       }

        public EntityConnectionStringBuilder loadConnectionString()
        {
            window = new View.Connection();
            window.ShowDialog();
           return esb;
        }

        private IEnumerable<string> servers;

        public IEnumerable<string> Servers
        {
            get { return servers; }
            set { servers = value;
            RaisePropertyChanged("Servers");
            }
        }

        private string selectedServer;

        public string SelectedServer
        {
            get { return selectedServer; }
            set { selectedServer = value;
                    RaisePropertyChanged("SelectedServer");
                 }
        }

        private string selectedDatabase;

        public string SelectedDatabase
        {
            get { return selectedDatabase; }
            set { selectedDatabase = value;
            RaisePropertyChanged("SelectedDatabase");
            }
        }
        

        private RelayCommand<RoutedEventArgs> passwordChanged;

        public RelayCommand<RoutedEventArgs> PasswordChanged
        {
            get { return  passwordChanged?? (passwordChanged = new RelayCommand<RoutedEventArgs>((e)=>
                {
                   
                        Password = ((PasswordBox)e.Source).Password;
                   
                })); }
           
        }
        

        public string Username { get; set; }
        public string Password { get; set; }

        private bool isWindowsAuthentication;

        public bool IsWindowsAuthentication
        {
            get { return isWindowsAuthentication; }
            set { isWindowsAuthentication = value;
            RaisePropertyChanged("IsWindowsAuthentication");
            }
        }


        private RelayCommand loadDatabase;

        public RelayCommand LoadDatabase
        {
            get
            {
                return loadDatabase ?? (loadDatabase = new RelayCommand(() =>
                    {
                        try
                        {
                        SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder();
                        conn.DataSource = SelectedServer;
                        conn.UserID = Username??"";
                        conn.Password = Password??"";
                        conn.IntegratedSecurity = IsWindowsAuthentication;
                        
                        SqlConnection Db = new SqlConnection();
                        Db.ConnectionString = conn.ConnectionString;
                      
                            SqlDataAdapter da = new SqlDataAdapter("exec sp_databases ", Db);
                            DataTable dtDB = new DataTable();
                            da.Fill(dtDB);

                            Databases = dtDB.Rows.Cast<DataRow>()
                                                .Select(p => p["DATABASE_NAME"].ToString());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }));
            }
        }

        private IEnumerable<String> databases;

        public IEnumerable<String> Databases
        {
            get { return databases; }
            set { databases = value;
                 RaisePropertyChanged("Databases");
            }
        }

        private Cursor currentCursor;

        public Cursor CurrentCursor
        {
            get { return currentCursor; }
            set
            {
                currentCursor = value;
                RaisePropertyChanged("CurrentCursor");
            }
        }

        EntityConnectionStringBuilder esb;
        private RelayCommand connect;

        public RelayCommand Connect
        {
            get
            {
                return connect ?? (connect = new RelayCommand(() =>
                    {
                        try
                        {
                            SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder();
                            conn.DataSource = SelectedServer;
                            conn.UserID = Username ?? "";
                            conn.Password = Password ?? "";
                            conn.IntegratedSecurity = IsWindowsAuthentication;
                            conn.InitialCatalog = SelectedDatabase;
                            conn.MultipleActiveResultSets = true;

                            esb = new EntityConnectionStringBuilder();
                           
                            esb.Metadata = @"res://*/Data.TriggerPointModel.csdl|res://*/Data.TriggerPointModel.ssdl|res://*/Data.TriggerPointModel.msl";
                            esb.Provider = "System.Data.SqlClient";
                            esb.ProviderConnectionString = conn.ConnectionString;
                           

                            SqlConnection Db = new SqlConnection();
                            Db.ConnectionString =  conn.ConnectionString;
                                
                            Db.Open();
                            
                            updateConfigFile(conn.ConnectionString);
                            window.DialogResult = true;
                            window.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }, () =>
                    {
                        if (string.IsNullOrEmpty(selectedDatabase) || string.IsNullOrEmpty(selectedServer))
                            return false;
                                 
                        return true;
                    }));
            }
        }

        public void updateConfigFile(string con)
        {
            
            //updating config file
            XmlDocument XmlDoc = new XmlDocument();
            //Loading the Config file
            XmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlElement xElement in XmlDoc.DocumentElement)
            {
                if (xElement.Name == "connectionStrings")
                {
                    //setting the coonection string
                  // xElement.FirstChild.Attributes[1].Value = con;
                  xElement.ChildNodes[0].Attributes[1].Value = "metadata=res://*/StockModel.csdl|res://*/StockModel.ssdl|res://*/StockModel.msl;provider=System.Data.SqlClient;provider connection string='" + con + ";Persist Security Info=True;MultipleActiveResultSets=True'";
                    //                    xElement.ChildNodes[1].Attributes[1].Value = "metadata=res://*/StockModel.csdl|res://*/StockModel.ssdl|res://*/StockModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;" + con + ";Persist Security Info=True;MultipleActiveResultSets=True&quot;";

                   
                }
            }
            //writing the connection string in config file
            XmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);


        }

      
    }
}
