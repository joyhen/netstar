using System.IO;
using System.Net;
using System.Text;

namespace NetStar.API
{
    internal class APIRequest
    {
        /// <summary>
        /// 发送HTTP GET请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="param">请求的参数</param>
        /// <returns>请求结果</returns>
        public static string request(string url, string param)
        {
            string strURL = url + '?' + param;

            System.Net.HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "GET";
            // 添加header
            request.Headers.Add("apikey", APIKey.BaiduApiKey);

            var response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream s = response.GetResponseStream();

            string StrDate = "";
            string strValue = "";
            StreamReader Reader = new StreamReader(s, Encoding.UTF8);
            while ((StrDate = Reader.ReadLine()) != null)
            {
                strValue += StrDate + "\r\n";
            }
            return strValue;
        }
    }
}