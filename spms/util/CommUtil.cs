using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.util
{
    class CommUtil
    {
        /// <summary>
        /// 获取用户照片全路径
        /// </summary>
        /// <param name="picName"></param>
        /// <returns></returns>
        public static string GetUserPic(string picName)
        {
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string path =ConfigurationManager.AppSettings["PicPath"];

            return basePath + path + picName;
        }
    }
}
