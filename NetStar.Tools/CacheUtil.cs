using System;
using System.Web;
using System.Web.Caching;

namespace NetStar.Tools
{
    /// <summary>
    /// 单机缓存简单操作
    /// </summary>
    /// <remarks>
    /// 1.轻量级数据本地化缓存使用
    /// 2.注意，缓存时间取的服务器时间，不是标准北京时间（轻量级数据存储，缓存丢失允许）
    /// </remarks>
    public class CacheUtil
    {
        //private static CacheUtil Instance;
        //private static object Locker = new object();

        //public CacheUtil() { }
        //public static CacheUtil GetInstance()
        //{
        //    if (Instance == null)
        //    {
        //        lock (Locker)
        //        {
        //            if (Instance == null)
        //                Instance = new CacheUtil();
        //        }
        //    }
        //    return Instance;
        //}

        /// <summary>
        /// 读取缓存
        /// </summary>
        public static object Get(string key)
        {
            return HttpContext.Current.Cache[key];
        }
        /// <summary>
        /// 读取缓存
        /// </summary>
        public static T Get<T>(string key)
        {
            var cache = HttpContext.Current.Cache[key];
            try
            {
                return (T)cache;
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 写入缓存
        /// </summary>
        public static bool Add(string key, object value, int cacheMinutes = 60)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            //var callBack = new CacheItemRemovedCallback(onRemove);

            HttpContext.Current.Cache.Insert(
                key,
                value,
                null,
                DateTime.Now.AddMinutes(cacheMinutes),
                Cache.NoSlidingExpiration,
                CacheItemPriority.High,
                null
            );

            return true;
        }

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="context">HttpContext对象，多线程下可能为空</param>
        public static bool AddAsync(HttpContext context, string key, object value, int cacheMinutes = 60)
        {
            if (string.IsNullOrWhiteSpace(key) || value == null)
            {
                return false;
            }

            context.Cache.Insert(
                key,
                value,
                null,
                DateTime.Now.AddMinutes(cacheMinutes),
                Cache.NoSlidingExpiration,
                CacheItemPriority.High,
                null
            );

            return true;
        }

        /// <summary>
        /// 删除缓存对象
        /// </summary>
        public static void Remove(string strKey)
        {
            HttpContext.Current.Cache.Remove(strKey);
        }

        /// <summary>
        /// 清除所有缓存对象
        /// </summary>
        public static void Clear()
        {
            var enu = HttpContext.Current.Cache.GetEnumerator();
            while (enu.MoveNext())
            {
                Remove(enu.Key.ToString());
            }
        }

        /// <summary>
        /// 移除缓存触发
        /// </summary>
        private static void onRemove(string key, object value, CacheItemRemovedReason reason)
        {
            //....
        }

        //...

    }
}