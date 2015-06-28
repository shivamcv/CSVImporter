using System.Windows;
using CSV_Importer.ViewModel;
using System.Windows.Controls;

namespace CSV_Importer
{
    public partial class MainWindow : Window
    {
        public static TabControl Tabs;
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            Tabs = tabs;
        }
    }
}