using Recovery.util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Recovery.view.converter
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
            

            return null;
        }
    }
    class StatusImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString().Equals("在线") || value.ToString().Equals("Online"))
            {
                return @"/view/Images/dcu_online.png";
            }
            else
            {
                return @"/view/Images/dcu_offline.png";
            }


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            

            return null;
        }
    }
}
