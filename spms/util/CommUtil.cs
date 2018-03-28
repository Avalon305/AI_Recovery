using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            string path = ConfigurationManager.AppSettings["PicPath"];

            return basePath + path + picName;
        }
        /// <summary>
        /// 获取Excel和pdf的路径全路径
        /// </summary>
        /// <param name="picName"></param>
        /// <returns></returns>
        public static string GetDocPath(string docName)
        {
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string path = ConfigurationManager.AppSettings["DocPath"];

            return basePath + path + docName;
        }

        public static string GetUserPic()
        {
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string path = ConfigurationManager.AppSettings["PicPath"];

            return basePath + path;
        }

        //mac地址的list 但是都相同??
        public static List<entity.Setter> GetMacByWMI()
        {
            entity.Setter setter = new entity.Setter();
            List<entity.Setter> macs = new List<entity.Setter>();
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        mac = mo["MacAddress"].ToString();
                        setter.Set_Unique_Id = mac;
                        macs.Add(setter);
                    }
                }
                moc = null;
                mc = null;
                return macs;
            }
            catch
            {
                return null;
            }
        }
        //单个mac地址
        public static string GetMacAddress()
        {
            try
            {
                string strMac = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        strMac = mo["MacAddress"].ToString();
                    }
                }
                moc = null;
                mc = null;
                return strMac;
            }
            catch
            {
                return "unknown";
            }
        }
    }
}
