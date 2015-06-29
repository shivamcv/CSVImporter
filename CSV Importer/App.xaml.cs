using System.Windows;
using GalaSoft.MvvmLight.Threading;
using System.IO;
using GalaSoft.MvvmLight.Ioc;

namespace CSV_Importer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();

            
        }

        public App()
        {
            DispatcherUnhandledException += (ss, ee) =>
                {
                    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "Log.txt"), SimpleIoc.Default.GetInstance<ViewModel.RunViewModel>().Log);
                    HelperClasses.ErrorReporting.ReportError(ee.Exception);
                };
        }
    }
}
