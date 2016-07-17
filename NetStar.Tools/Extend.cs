using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace NetStar.Tools
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class Extend
    {
        /// <summary>
        /// 时间戳转时间
        /// </summary>
        public static DateTime ConvertTicks2Time(this long timeTicks)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            //说明下，时间格式为13位后面补加4个"0"，如果时间格式为10位则后面补加7个"0"  
            long standardTicks = long.Parse(timeTicks + (timeTicks.ToString().Length == 13 ? "0000" : "0000000"));

            DateTime dtResult = dtStart.Add(new TimeSpan(standardTicks)); //得到转换后的时间  
            return dtResult;
        }

        /// <summary>
        /// 扩展比较
        /// </summary>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// 对象数据异步写入缓存
        /// </summary>
        public static T AsyncInsertCache<T>(this T cacheData, string cacheKey, int cacheTime = 60)
        {
            if (cacheData == null) return default(T);

            var context = System.Web.HttpContext.Current;

            ThreadPool.QueueUserWorkItem(state =>
            {
                CacheUtil.AddAsync(context, cacheKey, cacheData, cacheTime);
            });

            return cacheData;
        }

        /// <summary>
        /// 分割字符串
        /// <remarks>英文逗号","分割</remarks>
        /// </summary>
        public static List<string> StringToList(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return null;

            var arr = str.Replace("，", ",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return arr.ToList();
        }

        //...

    }
}
