using System;
using System.IO;
using System.Net;
using System.Text;

namespace NetStar.API
{
    using NetStar.Model.DTO;
    using Newtonsoft.Json;

    internal class bjtime
    {
        public long stime { get; set; }
    }

    public class Baidu
    {
        /// <summary>
        /// 获取北京时间戳
        /// </summary>
        public static long GetBeiJinTimeTicks()
        {
            string url = "http://apis.baidu.com/3023/time/time";
            string param = "";
            string result = APIRequest.request(url, param); //{"stime":1442357387}

            return JsonConvert.DeserializeObject<bjtime>(result).stime;
        }
        /// <summary>
        /// 获取北京时间
        /// </summary>
        public static DateTime GetBeiJinTime()
        {
            return ConvertTicks2Time(GetBeiJinTimeTicks());
        }

        /// <summary>
        /// 时间戳转时间
        /// </summary>
        private static DateTime ConvertTicks2Time(long timeTicks)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            //说明下，时间格式为13位后面补加4个"0"，如果时间格式为10位则后面补加7个"0"  
            long standardTicks = long.Parse(timeTicks + (timeTicks.ToString().Length == 13 ? "0000" : "0000000"));

            DateTime dtResult = dtStart.Add(new TimeSpan(standardTicks)); //得到转换后的时间  
            return dtResult;
        }

        /// <summary>
        /// IP查询接口
        /// </summary>
        /// <remarks>
        /* 
            {
                "error": "0",
                "msg": "",
                "data": {
                    "ip": "14.17.34.189",
                    "country": "China",
                    "province": "广东省",
                    "city": "广州市",
                    "operator": "电信",
                    "lat": "22.54605355",
                    "lng": "114.02597366"
                }
            }
         */
        /// </remarks>
        public static BaiduIpInfo GetIpInfo(string ip)
        {
            string url = "http://apis.baidu.com/chazhao/ipsearch/ipsearch";
            string param = string.Format("ip={0}", ip);
            string result = APIRequest.request(url, param);

            return JsonConvert.DeserializeObject<BaiduData>(result).data;
        }

        //...

    }
}