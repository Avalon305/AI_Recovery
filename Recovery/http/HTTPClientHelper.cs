using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.http
{
    public class HTTPClientHelper
    {
        private static readonly HttpClient HttpClient;
        //宝德龙厂区服务器地址
        //public static readonly string URL_BD = "http://222.133.43.210:28888/cloud/bigDataRecivedHandler/msg";
        public static readonly string URL_BD = "http://10.22.70.195:8888/cloud/bigDataRecivedHandler/msg";
        static HTTPClientHelper()
        {
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.None };
            HttpClient = new HttpClient(handler);
        }

        public static string HttpPost(string postDataStr)   
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL_BD);
                request.Method = "POST";
                request.ContentType = "application/json";
                //request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
                //request.CookieContainer = cookie;
                Stream myRequestStream = request.GetRequestStream();
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
                myStreamWriter.Write(postDataStr);
                myStreamWriter.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //response.Cookies = cookie.GetCookies(response.ResponseUri);
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return e.Message;
            }
        }

    }
}
