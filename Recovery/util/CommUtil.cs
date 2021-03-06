﻿using Dapper;
using Recovery.dao;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Recovery.util
{
    class CommUtil
    {

     
        private static SetterDAO setterDAO = new SetterDAO();

        /// <summary>
        /// 获取当前版本号
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentVersion()
        {
            return  FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location).ProductVersion;
        }

        /// <summary>
        /// 获取web平台路径
        /// </summary>
        /// <param name="picName"></param>
        /// <returns></returns>
        public static string GetPlatformUrl()
        {
            string platformUrl = ConfigurationManager.AppSettings["PlatformUrl"];
            return platformUrl;
        }
        /// <summary>
        /// 大数据上传间隔，单位为秒
        /// </summary>
        /// <param name="picName"></param>
        /// <returns></returns>
        public static int? GetBigDataRate()
        {
            string BeatRate = ConfigurationManager.AppSettings["BigDataRate"];
            int heartBeatRate = Convert.ToInt16(BeatRate)*1000;
            return heartBeatRate;
        }
        /// <summary>
        /// 心跳，单位为秒
        /// </summary>
        /// <param name="picName"></param>
        /// <returns></returns>
        public static int GetHeartBeatRate()
        {
            string BeatRate = ConfigurationManager.AppSettings["HeartBeatRate"];
            int heartBeatRate = Convert.ToInt16(BeatRate) * 1000;
            return heartBeatRate;
        }
        /// <summary>
        /// 获取用户照片全路径
        /// </summary>
        /// <param name="picName"></param>
        /// <returns></returns>
        public static string GetUserPic(string picName)
        {

            var setter = setterDAO.getSetter();

            if (setter != null)
            {
                return setter.Set_PhotoLocation + picName;
            }
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string path = ConfigurationManager.AppSettings["PicPath"];

            return basePath + path + picName;
        }
        /// <summary>
        /// 获得用户照片temp全路径
        /// </summary>
        /// <param name="picName"></param>
        /// <returns></returns>
        public static string GetUserPicTemp(string picName)
        {
        
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string path = ConfigurationManager.AppSettings["PicPathTemp"];

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
        //重构
        public static string GetUserPic()
        {
            var setter = setterDAO.getSetter();

            if (setter != null)
            {
                return setter.Set_PhotoLocation ;
            }

            string basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string path = ConfigurationManager.AppSettings["PicPath"];

            return basePath + path;
        }
        //重构
        public static string GetUserPicTemp()
        {

            string basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string path = ConfigurationManager.AppSettings["PicPathTemp"];

            return basePath + path;
        }
        //mac地址的list 但是都相同??
        public static List<string> GetMacByWMI()
        {
            List<string> macs = new List<string>();
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
                        macs.Add(mac);
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
        public static List<string> GetMacByIPConfig()
{
            List<string> macs = new List<string>();
            ProcessStartInfo startInfo = new ProcessStartInfo("ipconfig", "/all");
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            Process p = Process.Start(startInfo);
            //截取输出流
            StreamReader reader = p.StandardOutput;
            string line = reader.ReadLine();

            while (!reader.EndOfStream)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    line = line.Trim();

                    if (line.StartsWith("物理地址"))
                    {
                        macs.Add(line);
                    }
                }

                line = reader.ReadLine();
            }

            //等待程序执行完退出进程
            p.WaitForExit();
            p.Close();
            reader.Close();

            return macs;
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

        /// <summary>
        /// 读取客户设置
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public static string GetSettingString(string settingName)
        {
            try
            {
                string settingString = ConfigurationManager.AppSettings[settingName].ToString();
                return settingString;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="valueName"></param>
        public static void UpdateSettingString(string settingName, string valueName)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
               
            if (ConfigurationManager.AppSettings[settingName] != null)
            {
                config.AppSettings.Settings.Remove(settingName);
            }
            config.AppSettings.Settings.Add(settingName, valueName);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 执行cmd命令
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strcmd">命令</param>
        /// <returns>返回执行结果</returns>
        public static string RunCmd(string strPath, string strcmd)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.WorkingDirectory = strPath;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;

            p.Start();
            p.StandardInput.WriteLine(strcmd);
            p.StandardInput.WriteLine("exit");
            return p.StandardError.ReadToEnd();
        }

        /// <summary>
        /// 备份图片
        /// </summary>
        /// <param name="saveDirPath"></param>
        public static void CopyDirectory(string saveDirPath)
        {
            try
            {
                entity.Setter setter = new entity.Setter();
                setter = new SetterDAO().getSetter();
                Console.WriteLine("目标路径："+saveDirPath);
                string sourceDirPath = setter.Set_PhotoLocation;
                Console.WriteLine("原路径：" + sourceDirPath);
                if (!Directory.Exists(saveDirPath))
                {
                    Directory.CreateDirectory(saveDirPath);
                }
                string[] files = Directory.GetFiles(sourceDirPath.Substring(0, sourceDirPath.Length - 1));
                //MessageBox.Show(files.Length.ToString());
                foreach (string file in files)
                {
                    string pFilePath = saveDirPath + Path.GetFileName(file);
                    if (File.Exists(pFilePath))
                        continue;
                    File.Copy(file, pFilePath, true);
                }
            }
            catch (Exception ee)
            {

            }
        }

        /// <summary>
        /// 字符串转int
        /// </summary>
        /// <param name="intStr">要进行转换的字符串</param>
        /// <param name="defaultValue">默认值，默认：0</param>
        /// <returns></returns>
        public static int ParseInt(string intStr, int defaultValue = 0)
        {
            int parseInt;
            if (int.TryParse(intStr, out parseInt))
                return parseInt;
            return defaultValue;
        }
    }
}
