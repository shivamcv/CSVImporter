
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace CSV_Importer.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
            }
            else
            {
            }

            SimpleIoc.Default.Register<ViewModel.SettingViewModel>(true);
            SimpleIoc.Default.Register<ViewModel.Connection>();
            SimpleIoc.Default.Register<SaveViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public SettingViewModel Settings
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingViewModel>();
            }
        }
        public Connection ConnectionVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Connection>();
            }
        }


        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public SaveViewModel SaveVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SaveViewModel>();
            }
        }

        public static void Cleanup()
        {
        }
    }
}