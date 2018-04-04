using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace spms.view.converter
{
    [ValueConversion(typeof(String), typeof(String))]
    class IDCardConverter : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            if (value.ToString().Length >= 4)
            {
                String idCard = value.ToString();
               // Console.WriteLine("123:    " + idCard);
                String idCard_Public = idCard.Substring(0, value.ToString().Length - 4);
                //Console.WriteLine("0~14:    " + idCard_Public);
                return idCard_Public + "****";
            }
            else
            {
                return "***";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Console.WriteLine("返回值: "+ value.ToString());
            return value.ToString();
        }
    }

}
