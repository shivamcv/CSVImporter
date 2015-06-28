﻿using System.Windows;
using GalaSoft.MvvmLight.Threading;

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
                    HelperClasses.ErrorReporting.ReportError(ee.Exception);
                };
        }
    }
}
