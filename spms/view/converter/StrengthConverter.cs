using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace spms.view.converter
{
    class StrengthConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int reValue = System.Convert.ToInt32(value);
            if(reValue == 1 || reValue == 2)
            {
                return "非常轻松";
            }else if(reValue == 3 || reValue == 4)
            {
                return "很轻松";
            }else if(reValue == 5 || reValue == 6)
            {
                return "轻松";
            }else if(reValue == 7||reValue == 8)
            {
                return "有点儿困难";

            }
            else
            {
                return "困难";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
