using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSV_Importer.HelperClasses
{
 public   class ErrorReporting
    {
      public static void ReportError(Exception ex, string msg ="", bool ShowError = false)
     {
         if (ShowError)
             MessageBox.Show(string.IsNullOrEmpty(msg)? ex.Message : msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
     }
    }
}
