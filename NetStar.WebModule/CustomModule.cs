using System;
using System.Linq;
using System.Web;

namespace NetStar.WebModule
{
    public class CustomModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            HttpResponse response = context.Response;

            const string ajaxfile = ".ashx";
            //const string htmlfile = ".htm"; //.htm or .html
            string[] arr = context.Request.Url.ToString().Split('?');

            //对网站绑定的域名做限制

            if (arr[0].Contains(ajaxfile)/* && !arr[0].Contains(htmlfile)*/)
            {
                // 登录校验
                //.....
                Tools.PageHelp.PageMessage(response, "您尚未登录", go2Url: "/login.html");
            }
        }
    }
}