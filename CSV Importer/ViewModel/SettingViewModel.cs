using CSV_Importer.Properties;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CSV_Importer.ViewModel
{
    public class SettingViewModel : ViewModelBase
    {
        private Color primaryColor;

        public Color PrimaryColor
        {
            get { return primaryColor; }
            set
            {
                primaryColor = value;
                RaisePropertyChanged("PrimaryColor");
            }
        }

        private Color secondaryColor;

        public Color SecondaryColor
        {
            get { return secondaryColor; }
            set
            {
                secondaryColor = value;
                RaisePropertyChanged("SecondaryColor");
            }
        }

        private Color bgColor;

        public Color BgColor
        {
            get { return bgColor; }
            set
            {
                bgColor = value;
                RaisePropertyChanged("BgColor");
            }
        }

        public SettingViewModel()
        {
            PrimaryColor = ConvertToMediaColor(Settings.Default.PrimaryColor);
            SecondaryColor = ConvertToMediaColor(Settings.Default.SecondaryColor);
            BgColor = ConvertToMediaColor(Settings.Default.BgColor);

        }

        private RelayCommand restoreDefault;

        public RelayCommand RestoreDefault
        {
            get
            {
                return restoreDefault ?? (restoreDefault = new RelayCommand(() =>
                {
                    PrimaryColor = Color.FromRgb(35, 39, 48);
                    SecondaryColor = Color.FromRgb(0, 97, 152);
                    BgColor = Colors.White;
                    Save.Execute(null);
                }));
            }
        }

        private RelayCommand save;

        public RelayCommand Save
        {
            get
            {
                return save ?? (save = new RelayCommand(() =>
                {
                    Settings.Default.PrimaryColor = ConvertToDrawingColor(PrimaryColor);
                    Settings.Default.SecondaryColor = ConvertToDrawingColor(SecondaryColor);
                    Settings.Default.BgColor = ConvertToDrawingColor(BgColor);
                    Settings.Default.Save();
                }));
            }
        }


        private Color ConvertToMediaColor(System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private System.Drawing.Color ConvertToDrawingColor(Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }

}


 
