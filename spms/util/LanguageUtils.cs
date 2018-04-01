using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using spms.constant;

namespace spms.util
{
    class LanguageUtils
    {
        public static void SetLanguage(int index)
        {
            var dataCodeCache = DataCodeCache.GetInstance();
            string language = "zh-cn.xaml";
            switch (dataCodeCache.GetCodeDValue(DataCodeTypeEnum.Language, index.ToString()))
            {
                case "中文":
                    language = "zh-cn.xaml";
                    break;
                case "英语":
                    language = "en-us.xaml";
                    break;
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

    }
}
