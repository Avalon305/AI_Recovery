using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NuitrackScanProgress.util
{
    enum Result {
        SUCCESS,
        FAILED
    }
    class ConfigUtil
    {
        private static readonly byte[] PASSWORD = { 0x12, 0x23, 0xee, 0xff, 0xa2, 0x7c, 0x89, 0x0e, 0x63, 0x6B, 0x2D, 0x7A, 0x68, 0x75, 0x78, 0x67 };

        /// <summary>
        /// 获取配置文件的指定值，无加密
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns></returns>
        public static string Get(string key,string defaultVal)
        {
          string val =   ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(val))
            {
                return defaultVal;
            }
            return val;
        }

        /// <summary>
        /// 10个随机数+内容
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Encrypt(string content)
        {
            string random = RandomUtil.GenerateRandomNumber(10);
            var ip = AesUtil.Encrypt(Encoding.GetEncoding("GBK").GetBytes(random + content), PASSWORD);
            return ProtocolUtil.BytesToString(ip);
        }
        /// <summary>
        /// 获取配置文件的指定值，无加密,无值时候返回null
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string Get(string key)
        {
            return Get(key,null);
        }
        /// <summary>
        /// 获取配置文件的指定值，有加密
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns></returns>
        public static string GetEncrypt(string key, string defaultVal)
        {
            string v1 = Get(key);
            if (v1 == null)
            {
                return defaultVal;
            }
            Result state = Result.SUCCESS;
            string val = Decrypt(ref state, v1);
            if (state == Result.SUCCESS)
            {
                return val;
            }
            return defaultVal;
        }
        /// <summary>
        /// 获取配置文件的指定值，有加密,无值或解密失败时候返回null
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetEncrypt(string key)
        {
            return GetEncrypt(key, null);
        }



         
        private static string Decrypt(ref Result state, string content)
        {
            state = Result.SUCCESS;
            if (content.Length % 2 != 0)
            {
                state = Result.FAILED;
                return content;
            }
            try
            {
                byte[] jie = AesUtil.Decrypt(ProtocolUtil.StringToBcd(content), PASSWORD);
                string jiemi = Encoding.GetEncoding("GBK").GetString(jie);
                if (jiemi.Length <= 10)
                {
                    state = Result.FAILED;
                    return content;
                }
                return jiemi.Substring(10, jiemi.Length - 10);
            }
            catch (Exception ex)
            {
                state = Result.FAILED;
                return content;
            }

        }
    }

    class RandomUtil
    {
        private static char[] constant =
     {
        '0','1','2','3','4','5','6','7','8','9',
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
      };
        public static string GenerateRandomNumber(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }
    }
}
