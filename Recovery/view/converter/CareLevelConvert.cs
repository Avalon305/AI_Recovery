using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Recovery.constant;
using Recovery.util;

namespace Recovery.view.converter
{
    [ValueConversion(typeof(String), typeof(String))]
    class CareLevelConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string level = value.ToString();
            return DataCodeCache.GetInstance().GetCodeDValue(DataCodeTypeEnum.CareLevel, level);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}
