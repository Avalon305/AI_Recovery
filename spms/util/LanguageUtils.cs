using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using spms.constant;
using spms.dao;
using Setter = spms.entity.Setter;

namespace spms.util
{
    class LanguageUtils
    {
        public static void SetLanguage()
        {
            string language;
            if (IsChainese())
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
            ResourceDictionary resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source.OriginalString.Equals(requestedCulture));
            Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }

        public static bool IsChainese()
        {
            List<Setter> all = new SetterDAO().ListAll();
            var dataCodeCache = DataCodeCache.GetInstance();
            if (all != null && all.Count != 0)
            {
                if (dataCodeCache.GetCodeDValue(DataCodeTypeEnum.Language, all[0].Set_Language.ToString()) == "中文")
                {
                    return true;
                }
            }
            return false;
            
        }
        /// <summary>
        /// 根据当前语言返回字符串
        /// </summary>
        /// <param name="chainese">中文</param>
        /// <param name="english">英文</param>
        /// <returns></returns>
        public static string ConvertLanguage(string chainese, string english)
        {
            return IsChainese() ? chainese : english;
        }

        /// <summary>
        /// 根据资源id返回当前语言的字符串
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentLanuageByKey(string key)
        {
            return (string)Application.Current.Resources.MergedDictionaries[0][key];
        }

        /// <summary>
        /// 判断某个字符串是不是匹配某个资源id的中英文
        /// </summary>
        /// <param name="str">要比较的字符串</param>
        /// <param name="key">资源id</param>
        /// <returns></returns>
        public static bool EqualsResource(string str, string key)
        {
            List<ResourceDictionary> dictionaryList = new List<ResourceDictionary>();
            foreach (ResourceDictionary dictionary in Application.Current.Resources.MergedDictionaries)
            {
                dictionaryList.Add(dictionary);
            }
            string zh = (string)dictionaryList.FirstOrDefault(d => d.Source.OriginalString.Equals("zh-cn.xaml")).MergedDictionaries[0][key];
            string en = (string)dictionaryList.FirstOrDefault(d => d.Source.OriginalString.Equals("en-us.xaml")).MergedDictionaries[0][key];
            return false;
        }

    }
}
