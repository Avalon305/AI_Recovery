using spms.util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace spms.view.converter
{
    class StatusConverter:IValueConverter
    {
        bool language = LanguageUtils.IsChainese();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString().Equals("在线") || value.ToString().Equals("Online")) { 
                return new SolidColorBrush(Colors.GreenYellow);
            }
            else
            {
                return new SolidColorBrush(Colors.Red); 
             }
          

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int revalue = (int)value;

            return DateTime.Now.Year - revalue;
        }
    }
}
