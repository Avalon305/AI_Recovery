using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using spms.constant;
using spms.util;

namespace spms.view.converter
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
