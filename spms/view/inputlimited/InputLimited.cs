using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace spms.view.inputlimited
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
    }
}
