using spms.util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace spms.view.converter
{
    class LanguageConverter : IMultiValueConverter
    {
        bool language = LanguageUtils.IsChainese();
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (language)
            {
                return values[1];
            }
            else
            {
                return values[0];
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
