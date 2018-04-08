using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace AutoUpdater.Utils
{
    class LanguageUtils
    {
        public static int language;
        public static void SetLanguage(int f)
        {
            string language;
            if (IsChainese(f))
            {
                language = "zh-cn.xaml";
            }
            else
            {
                language = "en-us.xaml";
            }

            List<ResourceDictionary> dictionaryList = new List<ResourceDictionary>();
            foreach (ResourceDictionary dictionary in Application.Current.Resources.MergedDictionaries)
            {
                dictionaryList.Add(dictionary);
            }

            string requestedCulture = @"resources\" + language;
            ResourceDictionary resourceDictionary =
                dictionaryList.FirstOrDefault(d => d.Source.OriginalString.Equals(requestedCulture));
            Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            
        }

        public static bool IsChainese(int f)
        {
            if (f == 0)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 根据当前语言返回字符串
        /// </summary>
        /// <param name="chainese">中文</param>
        /// <param name="english">英文</param>
        /// <returns></returns>
        public static string ConvertLanguage(int f, string chainese, string english)
        {
            return IsChainese(f) ? chainese : english;
        }
    }
}