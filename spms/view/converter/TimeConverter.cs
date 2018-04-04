using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace spms.view.converter
{
    [ValueConversion(typeof(int), typeof(double))]
    class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 0;
            }
            else
            {
                //object转float
                double temp = System.Convert.ToSingle(value.ToString());
                int hours = (int)(temp / 60);
                int minute = (int)(temp - hours * 60);
                int second = (int)(temp % 1 * 60);
                DateTime time = new DateTime();
                time.AddHours(hours);
                time.AddMinutes(minute);
                time.AddSeconds(second);
                return time;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 0;
            }
            else
            {
                DateTime time = System.Convert.ToDateTime(value.ToString());
                return (int)(time.Hour*60+time.Minute+time.Second/60);
            }
        }
    }
}
