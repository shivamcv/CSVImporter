using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CSV_Importer.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
        }

        private RelayCommand gotoSave;

        public RelayCommand GotoSave
        {
            get
            {
                return gotoSave ?? (gotoSave = new RelayCommand(() =>
                    {
                        MainWindow.Tabs.SelectedIndex = 1;
                    }));
            }
        }
        
    }
}