using System;

namespace NetStar.Config
{
    public class KeyCenter
    {
        /// <summary>
        /// 应用程序根目录
        /// </summary>
        public static readonly string KeyBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 用户标识
        /// </summary>
        public const string KeyCustomerID = "张三有限公司";
        /// <summary>
        /// 用户授权码
        /// </summary>
        public const string KeyCustomerLicenseCode = "";

        /*----------------------------------------------------------------------------*/

        //Tools.Encrypt.DESEncrypt(str);

        /// <summary>
        /// Your system has expired
        /// </summary>
        public static readonly string KeySiteMsg = "D0406E2C0D757DCD3EEDE7C16EE6411DB6D550752BF36E1A";
        /// <summary>
        /// Current domain is not authorized
        /// </summary>
        public static readonly string KeySiteUrl = "1132421005CAA0F3B03712B8318ECD2E";

        /*----------------------------------------------------------------------------*/

        /// <summary>
        /// mysql数据库连接字符串键
        /// </summary>
        public const string KeyMySQLConnection = "zyj_1Connection";
        /// <summary>
        /// sqlserver数据库连接字符串键
        /// </summary>
        public const string KeyMSSQLConnection = "zyj_2Connection";

        /*----------------------------------------------------------------------------*/

        /// <summary>
        /// 默认的缓存时间 3个小时
        /// </summary>
        public const int KeyCacheMinutes = 60 * 3;
        /// <summary>
        /// 枚举对象缓存键
        /// </summary>
        public const string KeyEnumKVCache = "nt_EnumKVCache_{0}";
        /// <summary>
        /// 北京时间缓存键
        /// </summary>
        public const string KeyBeiJinTimeCache = "nt_BeiJinTimeCache";


    }
}