using System;
using System.Globalization;
using System.Windows.Data;

namespace spms.view.converter
{
    [ValueConversion(typeof(int), typeof(String))]
    class SexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int reValue = System.Convert.ToInt32(value);
            string sex;
            if (reValue == 1)
            {
                sex = "男";
            }
            else
            {
                sex = "女";
            }
            return sex;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string sexString= value.ToString();
            int sex;
            if (sexString.Equals("男"))
            {
                sex = 1;
            }
            else
            {
                sex = 0;
            }
            return sex;
        }
    }
}
