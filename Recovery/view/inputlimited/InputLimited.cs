using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Recovery.view.inputlimited
{
    /// <summary>
    /// 输入框输入限制
    /// </summary>
    static class InputLimited
    {
        public static void OnlyInputNumbers(TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");

            e.Handled = re.IsMatch(e.Text);
        }
        //身份证正则
        public static bool IsIDcard(String e)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(e, @"(^\d{17}(?:\d|x)$)|(^\d{15}$)");
        }
        //手机号正则
        public static bool IsHandset(String str_handset)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, "^((13[0-9])|(14[579])|(15([0-3]|[5-9]))|16[6]|17[0135678]|(18[0-9])|19[8,9])\\d{8}$");
            //return true;
        }
    }
}
