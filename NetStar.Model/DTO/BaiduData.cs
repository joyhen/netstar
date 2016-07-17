using System;

namespace NetStar.Model.DTO
{
    public class BaiduData
    {
        public string error { get; set; }
        public string msg { get; set; }
        public BaiduIpInfo data { get; set; }
    }
}