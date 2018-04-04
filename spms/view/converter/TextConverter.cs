using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using spms.util;

namespace spms.view.converter
{
    [ValueConversion(typeof(String), typeof(String))]
    class TextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return LanguageUtils.ConvertLanguage("未填写", "Unfilled");
            }
            else { 
            String text = value.ToString();
            
            if (String.IsNullOrEmpty(text.Trim()))
            {
                return LanguageUtils.ConvertLanguage("未填写", "Unfilled");
                }
            else
            {
                return text;
            }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return LanguageUtils.ConvertLanguage("未填写", "Unfilled");
            }
            else
            {
                String text = value.ToString();

                if (value.ToString().Equals("未填写") || value.ToString().Equals("Unfilled"))
                {
                    return LanguageUtils.ConvertLanguage("未填写", "Unfilled");
                }
                else
                {
                    return text;
                }
            }
        }
    }
}
