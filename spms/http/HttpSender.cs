using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using spms.http.entity;
using spms.util;

namespace spms.http
{
    //负责发送http请求的发送者对象
    public class HttpSender
    {
        public static string URLBASE = CommUtil.GetPlatformUrl();
        
        

        

        //post方式，参数为json串
        public static string POSTByJsonStr(string url, string jsonStr)
        {
            try
            {
                Console.WriteLine("====================================发数据啦" + jsonStr);
                HttpWebRequest request = WebRequest.Create(URLBASE + url) as HttpWebRequest; //创建请求
                CookieContainer cookieContainer = new CookieContainer();
                request.Timeout = 10 * 1000; //10s超时
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                //request.AllowReadStreamBuffering = true;
                request.MaximumResponseHeadersLength = 1024;
                request.Method = "POST"; //请求方式为post
                request.AllowAutoRedirect = true;
                request.MaximumResponseHeadersLength = 1024;
                request.ContentType = "application/json";

                // string jsonstring = json.ToString();//获得参数的json字符串
                byte[] jsonbyte = Encoding.UTF8.GetBytes(jsonStr);
                Stream postStream = request.GetRequestStream();
                postStream.Write(jsonbyte, 0, jsonbyte.Length);
                postStream.Close();
                //发送请求并获取相应回应数据       
                HttpWebResponse res;
                try
                {
                    res = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    res = (HttpWebResponse)ex.Response;
                }

                StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                string content = sr.ReadToEnd(); //获得响应字符串
                Console.WriteLine("====================================response:" + content);
                return content;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
            
        }


        //url为请求的网址，param参数为需要查询的条件（服务端接收的参数，没有则为null）
        //返回该次请求的响应
        public static string GET(string url, Dictionary<String, String> param)
        {
            if (param != null) //有参数的情况下，拼接url
            {
                url = url + "?";
                foreach (var item in param)
                {
                    url = url + item.Key + "=" + item.Value + "&";
                }

                url = url.Substring(0, url.Length - 1);
            }

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest; //创建请求
            request.Method = "GET"; //请求方法为GET
            HttpWebResponse res; //定义返回的response
            try
            {
                res = (HttpWebResponse) request.GetResponse(); //此处发送了请求并获得响应
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse) ex.Response;
            }
             
            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
            string content = sr.ReadToEnd(); //响应转化为String字符串
            return content;
        }


        
    }
}