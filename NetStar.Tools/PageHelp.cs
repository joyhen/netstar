using System;
using System.Web;

namespace NetStar.Tools
{
    public class PageHelp
    {
        /// <summary>
        /// 页面输出提示
        /// </summary>
        /// <param name="pageMsg">页面提示信息</param>
        /// <param name="go2Url">跳转地址</param>
        public static void PageMessage(HttpResponse response, string pageMsg, string go2Url)
        {
            PageMessage(response, pageMsg ?? "未知错误退出", go2Url, 0);
        }

        /// <summary>
        /// 页面输出提示
        /// </summary>
        /// <param name="pageMsg">页面提示信息</param>
        /// <param name="backStep">倒退步数</param>
        public static void PageMessage(HttpResponse response, string pageMsg, int backStep)
        {
            PageMessage(response, pageMsg ?? "未知错误退出", "", Math.Max(backStep, 1));
        }

        public static void PageMessage(HttpResponse response, string pageMsg, string go2Url, int BackStep)
        {
            int Seconds = 2; //倒计时
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>\r\n");
            sb.Append("<html xmlns='http://www.w3.org/1999/xhtml'>\r\n");
            sb.Append("<head>\r\n");
            sb.Append("<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />\r\n");
            sb.Append("<head>\r\n");
            sb.Append("<title>系统提示</title>\r\n");
            sb.Append("<style>\r\n");
            sb.Append("body {padding:0; margin:0; }\r\n");
            sb.Append("#infoBox{padding:0; margin:0; position:absolute; top:40%; width:100%; text-align:center;}\r\n");
            sb.Append("#info{padding:0; margin:0;position:relative; top:-40%; right:0; border:0px #B4E0F7 solid; text-align:center;}\r\n");
            sb.Append(".a_goto{ color:#ff0000; text-decoration:underline;}");
            sb.Append("</style>\r\n");
            sb.Append("<script language=\"javascript\">\r\n");
            sb.Append("var seconds=" + Seconds + ";\r\n");
            sb.Append("for(i=1;i<=seconds;i++)\r\n");
            sb.Append("{window.setTimeout(\"update(\" + i + \")\", i * 1000);}\r\n");
            sb.Append("function update(num)\r\n");
            sb.Append("{\r\n");
            sb.Append("if(num == seconds)\r\n");
            if (BackStep > 0)
                sb.Append("{ history.go(" + (0 - BackStep) + "); }\r\n");
            else
            {
                if (go2Url != "")
                    sb.Append("{ top.location.href='" + go2Url + "'; }\r\n");
                else
                    sb.Append("{window.close();}\r\n");
            }
            sb.Append("else\r\n");
            sb.Append("{ }\r\n");
            sb.Append("}\r\n");
            sb.Append("</script>\r\n");
            sb.Append("<base target='_self' />\r\n");
            sb.Append("</head>\r\n");
            sb.Append("<body>\r\n");
            sb.Append("<div id='infoBox'>\r\n");
            sb.Append("<div id='info'>\r\n");
            sb.Append("<div style='text-align:center;margin:0 auto;width:320px;padding-top:4px;line-height:26px;height:26px;font-weight:bold;color:#fff;font-size:14px;border:1px #1e71b1 solid;background:#1e71b1;'>提示信息</div>\r\n");
            sb.Append("<div style='text-align:center;padding:20px 0 20px 0;margin:0 auto;width:320px;font-size:12px;background:#F5FBFF;border-right:1px #1e71b1 solid;border-bottom:1px #1e71b1 solid;border-left:1px #1e71b1 solid;'>\r\n");
            sb.Append(pageMsg + "<br /><br />\r\n");
            if (BackStep > 0)
                sb.Append("        <a class=\"a_goto\" href=\"javascript:history.go(" + (0 - BackStep) + ")\">如果您的浏览器没有自动跳转，请点击这里</a>\r\n");
            else
                sb.Append("        <a class=\"a_goto\" href=\"" + go2Url + "\">如果您的浏览器没有自动跳转，请点击这里</a>\r\n");
            sb.Append("    </div>\r\n");
            sb.Append("</div>\r\n");
            sb.Append("</div>\r\n");
            sb.Append("</body>\r\n");
            sb.Append("</html>\r\n");
            response.Write(sb.ToString());
            response.End();
        }
    }
}