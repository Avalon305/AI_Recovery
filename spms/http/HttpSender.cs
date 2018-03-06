using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace spms.http
{
    //负责发送http请求的发送者对象
    class HttpSender
    {
        //发送地址
        private string Uri;
        //发送的json数据体
        private string Data;
        //有参构造
        public HttpSender(string UriTarget, string DataBody) {
            this.Uri = UriTarget;
            this.Data = DataBody;
        }
        //私有化空构造
        private HttpSender()
        {
        }
        //发送数据至平台的具体方法
        public string sendDataToWebPlatform() {
            return "";
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
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;//创建请求
            request.Method = "GET"; //请求方法为GET
            HttpWebResponse res; //定义返回的response
            try
            {
                res = (HttpWebResponse)request.GetResponse(); //此处发送了请求并获得响应
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
            string content = sr.ReadToEnd(); //响应转化为String字符串
            return content;
        }
        //url为请求的网址，param为需要传递的参数
        //返回服务端的响应
        public static string POST(string url, Dictionary<String, String> param)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest; //创建请求
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            //request.AllowReadStreamBuffering = true;
            request.MaximumResponseHeadersLength = 1024;
            request.Method = "POST"; //请求方式为post
            request.AllowAutoRedirect = true;
            request.MaximumResponseHeadersLength = 1024;
            request.ContentType = "application/json";
            JObject json = new JObject();
            if (param.Count != 0) //将参数添加到json对象中
            {
                foreach (var item in param)
                {
                    json.Add(item.Key, item.Value);
                }
            }
            string jsonstring = json.ToString();//获得参数的json字符串
            byte[] jsonbyte = Encoding.UTF8.GetBytes(jsonstring);
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
            return content;
        }
    }
}
