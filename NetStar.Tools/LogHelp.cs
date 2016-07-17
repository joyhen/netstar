using System;
using System.Threading;

namespace NetStar.Tools
{
    public class LogHelp
    {
        /// <summary>  
        /// 日志锁定  
        /// </summary>  
        private readonly static Object Lok = new Object();

        /// <summary>  
        /// 记录日志
        /// </summary>  
        public static void Log(string Txt)
        {
            if (string.IsNullOrWhiteSpace(Txt)) return;

            try
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    string logContent = string.Format("{0}\t【{1}】", DateTime.Now.ToString(@"HH:mm:ss"), Txt);
                    WriteLog(logContent);
                });
            }
            catch
            {
            }
        }

        public static void Log(Exception ex)
        {
            if (ex == null) return;

            try
            {
                //System.Threading.Tasks.Task.Factory.StartNew(() => { });
                ThreadPool.QueueUserWorkItem(state =>
                {
                    string logContent = string.Format(
                        "{0}\t【{1}】\r\n{2}",
                        DateTime.Now.ToString(@"HH:mm:ss"),
                        ex.Message,
                        ex.StackTrace);
                    WriteLog(logContent);
                });
            }
            catch
            {
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logContent">日志内容</param>
        private static void WriteLog(string logContent)
        {
            lock (Lok)
            {
                logContent = string.Format("{0}\r\n----------------------------------------------------\r\n", logContent);
                string logFol = "Log"; //db层的异常文件夹
                string logDir = AppDomain.CurrentDomain.BaseDirectory;
                string logPath = string.Format(@"{0}\{1}", logDir, logFol);
                string logFile = string.Format(@"{0}\{1}.log", logPath, DateTime.Now.ToString(@"yy-MM-dd"));

                System.IO.Directory.CreateDirectory(logPath);
                System.IO.File.AppendAllText(logFile, logContent);
            }
        }

        //...

    }
}