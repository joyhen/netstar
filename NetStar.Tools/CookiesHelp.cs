using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;

namespace NetStar.Tools
{
    using Config;

    /// <summary>
    /// Cookie操作类
    /// </summary>
    /// <remarks>注意，缓存时间取的服务器时间</remarks>
    public static class CookiesHelp
    {
        /// <summary>
        /// cookies加密方式
        /// </summary>
        private static string JiaCoo(string str)
        {
            return Encrypt.DESEncrypt(str);
        }
        /// <summary>
        /// cookies加密方式
        /// </summary>
        private static string JieCoo(string str)
        {
            return Encrypt.DESDecrypt(str);
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        public static T GetCookie<T>(string cookieName)
        {
            if (string.IsNullOrWhiteSpace(cookieName)) return default(T);

            var cookies = HttpContext.Current.Request.Cookies[cookieName] ?? null;
            if (cookies != null)//&& cookies.HasKeys)
            {
                var cookiesValue = cookies.Value ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(cookiesValue))
                {
                    try
                    {
                        var value = JieCoo(cookiesValue);
                        return JsonUtils.DeserializeObject<T>(value);
                    }
                    finally
                    {
                    }
                }
            }

            return default(T);
        }
        /// <summary>
        /// 创建cookies对象
        /// </summary>
        /// <param name="cookieKey">cookies对象名</param>
        /// <param name="cookieValue">cookie对象Value值</param>
        /// <param name="iExpires">cookie对象有效时间(此处是小时)</param>
        public static void SetCookie(string cookieKey, object cookieValue, int expiresDays = 7)
        {
            if (string.IsNullOrEmpty(cookieKey) || cookieValue == null) return;

            var cookiesDomain = GetCookiesDomain();
            cookiesDomain.ForEach(x =>
            {
                HttpCookie objCookie = new HttpCookie(cookieKey);
                objCookie.Value = JiaCoo(JsonUtils.JsonSerializer(cookieValue, false));
                objCookie.Expires = DateTime.Now.AddDays(expiresDays > 0 ? expiresDays : 0);
                objCookie.Path = "/";
                objCookie.Domain = x;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            });
        }

        public static void DeleteCookies(string cookieKey)
        {
            if (string.IsNullOrEmpty(cookieKey)) return;

            var cookiesDomain = GetCookiesDomain();
            cookiesDomain.ForEach(x =>
            {
                HttpCookie objCookie = new HttpCookie(cookieKey);
                objCookie.Expires = DateTime.Now.AddYears(-1); //让其过期无效
                objCookie.Path = "/";
                objCookie.Domain = x;
                HttpContext.Current.Response.Cookies.Add(objCookie);
                //HttpContext.Current.Response.Cookies.Remove(cookieName);
            });
        }

        /// <summary>
        /// 获取Cookie作用域
        /// </summary>
        public static List<string> GetCookiesDomain()
        {
            var key = string.Format(Config.KeyCenter.KeyEnumKVCache, "SiteUrl");
            var cache = CacheUtil.Get<Dictionary<string, string>>(key);

            if (cache == null)
            {
                cache = EnumUtils.GetDescription<EnumCenter.SiteUrl>();
                CacheUtil.Add(key, cache);
            }

            return cache.Select(x => x.Value).ToList();
        }

        //...

    }
}