using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CSV_Importer.View
{
    /// <summary>
    /// Interaction logic for DateTimeConversion.xaml
    /// </summary>
    public partial class DateTimeConversion : UserControl, INotifyPropertyChanged
    {
        public DateTimeConversion()
        {
            InitializeComponent();
            DataContext = this;
        }

        private string dateTimeValue;

        public string DateTimeValue
        {
            get { return dateTimeValue; }
            set { dateTimeValue = value;
            RaisePropertyChanged("DateTimeValue");
            conversion();
            }
        }

    

        private string dateTimeFormat;

        public string DateTimeFormat
        {
            get { return dateTimeFormat; }
            set { dateTimeFormat = value;
            RaisePropertyChanged("DateTimeFormat");
            conversion();
            }
        }

        private string output;

        public string Output
        {
            get { return output; }
            set { output = value;
            RaisePropertyChanged("Output");
            }
        }
        

        private void RaisePropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        private void conversion()
        {
            if (!string.IsNullOrEmpty(DateTimeFormat) && !string.IsNullOrEmpty(dateTimeValue))
            {
                DateTime temp;
                CultureInfo enUS = new CultureInfo("en-US");

                if (DateTime.TryParseExact(DateTimeValue, DateTimeFormat, enUS, DateTimeStyles.None, out temp))
                    Output = string.Format("year:{0} month:{1} day:{2} hour:{3} minute:{4} second{5}", temp.Year, temp.Month, temp.Day, temp.Hour, temp.Minute, temp.Second);
                else
                    Output = "Error in parsing";
            }
            else
                Output = string.Empty;
               
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
