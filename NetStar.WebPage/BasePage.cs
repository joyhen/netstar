using System;
using System.Linq;
using System.Collections.Generic;

namespace NetStar.WebPage
{
    using NetStar.API;
    using NetStar.Tools;
    using NetStar.Model;
    using NetStar.Config;

    /// <summary>
    /// 功能基类页面，后端基类页面
    /// </summary>
    public class BasePage : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            if (!CheckUserLogin())
            {
                Tools.PageHelp.PageMessage(Response, "系统尚未登录或登录超时！", "/login.aspx");
                return;
            }
            base.OnInit(e);
        }

        /// <summary>
        /// Page_Load事件，建议基类从写此方法
        /// </summary>
        public virtual void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 检查用户是否登录
        /// </summary>
        protected bool CheckUserLogin()
        {
            return true;
        }

        /// <summary>
        /// 北京时间
        /// </summary>
        protected DateTime BeiJinTime
        {
            get
            {
                var dataDic = CacheUtil.Get<Dictionary<long, long>>(KeyCenter.KeyBeiJinTimeCache)
                            ?? new Dictionary<long, long>();

                if (dataDic == null || dataDic.Count == 0)
                {
                    var bjTimeTicks = Baidu.GetBeiJinTimeTicks();

                    dataDic.Add(GetTimeStamp(), bjTimeTicks);               //当前时间戳，北京时间戳
                    CacheUtil.Add(KeyCenter.KeyBeiJinTimeCache, dataDic);   //写入缓存

                    return bjTimeTicks.ConvertTicks2Time();
                }

                var sp = GetTimeStamp() - dataDic.FirstOrDefault().Key;
                if (sp < 0) return Baidu.GetBeiJinTime();                   //手动修改了服务器本地时间
                return (dataDic.FirstOrDefault().Value + sp).ConvertTicks2Time();
            }
        }

        /// <summary>
        /// 当前时间时间戳
        /// </summary>
        private static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 从缓存获取数据（缓存有就取，没有就执行方法获取）
        /// </summary>
        /// <typeparam name="T">要获取的对象</typeparam>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="fn">缓存没有时执行的方法</param>
        public T GetDataWithCache<T>(string cacheKey, Func<T> fn)
        {
            var cachedata = CacheUtil.Get<T>(cacheKey);
            if (cachedata == null)
            {
                if (fn == null) return default(T);

                cachedata = fn.Invoke().AsyncInsertCache(cacheKey);
            }

            return cachedata;
        }

        #region common
        /// <summary>
        /// 获取AppSettings值
        /// </summary>
        protected string GetAppSettingValue(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key] ?? string.Empty;
        }
        /// <summary>
        /// 用户密码加密
        /// </summary>
        protected string EncryptAdmin(string paramStr)
        {
            return Encrypt.MD5Encrypt(Encrypt.MD5Encrypt(paramStr));
        }

        /// <summary>
        /// GET请求参数
        /// </summary>
        protected string Q(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return string.Empty;

            var query = Request.QueryString[key];
            return query ?? string.Empty;
        }
        protected int QInt(string key)
        {
            int id = 0;
            int.TryParse(Q(key), out id);
            return id;
        }
        /// <summary>
        /// POST请求参数
        /// </summary>
        protected string F(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return string.Empty;

            var query = Request.Form[key];
            return query ?? string.Empty;
        }
        protected int FInt(string key)
        {
            int id = 0;
            int.TryParse(Q(key), out id);
            return id;
        }

        /// <summary>
        /// 字符串有效性检查
        /// </summary>
        protected bool IsNotEmptyString(string str)
        {
            return (!string.IsNullOrWhiteSpace(str) && str.Trim().Length > 0);
        }

        /// <summary>
        /// 请求响应后的输出
        /// </summary>
        /// <param name="msg">错误内容</param>
        protected void Outmsg(string msg)
        {
            Outmsg(false, msg);
        }
        /// <summary>
        /// 请求响应后的输出
        /// </summary>
        /// <remarks>outmsg : "" or default(String)</remarks>
        protected void Outmsg(bool success = false, string outmsg = "", object outdata = null)
        {
            Response.Write(JsonUtils.JsonSerializer(new ResultModel
            {
                success = success,
                msg = outmsg,
                data = outdata
            }));
            Response.End();
        }
        #endregion
    }
}
