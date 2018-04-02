using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace spms.view.converter
{
    [ValueConversion(typeof(DateTime), typeof(int))]
    class AgeConverter :IValueConverter
    {
 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime revalue = System.Convert.ToDateTime(value);
            int age = DateTime.Now.Year - revalue.Year;
            
            return age;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int revalue = (int)value;

            return DateTime.Now.Year -revalue;
        }
    }
}
