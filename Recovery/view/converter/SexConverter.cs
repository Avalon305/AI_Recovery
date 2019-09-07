using System;
using System.Globalization;
using System.Windows.Data;
using Recovery.util;

namespace Recovery.view.converter
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
                sex = LanguageUtils.GetCurrentLanuageStrByKey("SearchSubjectView.M");
            }
            else
            {
                sex = LanguageUtils.GetCurrentLanuageStrByKey("SearchSubjectView.F");
            }
            return sex;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string sexString= value.ToString();
            int sex;
            if (LanguageUtils.EqualsResource(sexString, "SearchSubjectView.M"))
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
