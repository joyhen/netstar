using System;
using System.IO;
using System.Threading;
using System.Configuration;

namespace Utility
{
    public class LogManage
    {
        /// <summary>
        /// 日志锁定
        /// </summary>
        private readonly static Object Lok = new Object();

        public static void Add(Exception ex)
        {
            string log = Newtonsoft.Json.JsonConvert.SerializeObject(
                             ex,
                             Newtonsoft.Json.Formatting.Indented,
                             new Newtonsoft.Json.JsonSerializerSettings { }
                         );
            Add(log, true);
        }

        public static void Add(string content, bool isException = false)
        {
            //ThreadPool.QueueUserWorkItem((state) =>
            //{

            //});

            string logPath = LogPath;
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".txt";

            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            lock (Lok)
            {
                using (FileStream fs = new FileStream(logPath + fileName, FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        if (isException)
                        {
                            string _logTemplate = "{0}: 有异常错误======>\r\n{1}\r\n{2}\r\n";
                            content = string.Format(
                                          _logTemplate,
                                          DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                          content,
                                          new System.String('*', 100)
                                      );
                        }
                        else
                            content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + content;

                        sw.WriteLine(content);
                    }
                }
            }
        }

        private static string LogPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + "\\pushlog\\";
            }
        }
    }
}
