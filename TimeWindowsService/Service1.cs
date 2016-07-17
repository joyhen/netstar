using System;
using System.ServiceProcess;

namespace Service
{
    using Quartz;
    using Quartz.Impl;
    using Utility;
    //using Common.Logging;

    public partial class Service1 : ServiceBase
    {
        //private readonly ILog logger = LogManager.GetLogger(this.GetType());
        private IScheduler scheduler;

        public Service1()
        {
            InitializeComponent();

            //初始化日志和调度业务工厂
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            //scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler = schedulerFactory.GetScheduler();
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        protected override void OnStart(string[] args)
        {
            try
            {
                LogManage.Add("服务启动============================================>s");

                if (!scheduler.IsStarted)
                {
                    //int _minutes = 1; //ConfigHelp.GetConfigValueInt("ExePushMessageJobTime", 2);

                    IJobDetail job = JobBuilder.Create<JobClass.SetPushMessageJob>()
                                     .WithIdentity("job1", "group1")
                                     .Build();

                    ITrigger trigger = TriggerBuilder.Create()
                                           .WithDailyTimeIntervalSchedule(s =>
                                               //s.WithIntervalInHours(24)          //每隔x小时执行一次
                                               //s.WithIntervalInMinutes(_minutes)    //每隔x分钟执行一次
                                           s.WithIntervalInSeconds(5)         //每隔x秒钟执行一次
                                           .OnEveryDay()                        //每天都执行
                                           .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
                                            //.EndingDailyAt()
                                         )
                                       .Build();
                    scheduler.ScheduleJob(job, trigger);

                    scheduler.Start();                                          //启动计划任务
                    LogManage.Add("作业任务加载成功====================================>s");
                }
            }
            catch (Exception ex)
            {
                LogManage.Add("服务启动异常：" + ex.Message);
            }
        }

        /// <summary>
        /// 服务暂停处理
        /// </summary>
        protected override void OnPause()
        {
            scheduler.PauseAll();
            LogManage.Add("服务暂停====================================>e\r\n");
            base.OnPause();
        }

        /// <summary>
        /// 继续已经挂起的线程
        /// </summary>
        protected override void OnContinue()
        {
            scheduler.ResumeAll();
            LogManage.Add("服务继续====================================>e\r\n");
            base.OnContinue();
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        protected override void OnStop()
        {
            if (!scheduler.IsShutdown)
            {
                scheduler.Shutdown();
            }
            LogManage.Add("服务停止====================================>e\r\n");
        }

        //mark:关于cron表达式详解：
        //http://www.cnblogs.com/linjiqin/archive/2013/07/08/3178452.html
        //http://www.tuicool.com/articles/QbAJjqe

    }
}