using System.Windows;
using CSV_Importer.ViewModel;

namespace CSV_Importer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }
    }
}