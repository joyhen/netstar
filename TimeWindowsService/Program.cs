namespace Service
{
    using System;
    using System.ServiceProcess;
    using Utility;

    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            try
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                { 
                    new Service1() 
                };
                ServiceBase.Run(ServicesToRun);
            }
            catch (Exception ex)
            {
                LogManage.Add("推送服务启动错误：" + ex.Message);
            }
        }

        //...

    }
}