using System;
using System.Linq;
using System.Collections.Generic;

namespace JobClass
{
    using Quartz;
    using Utility;
    using Newtonsoft.Json;

    /// <summary>
    /// 发标和活动推送
    /// </summary>
    public class SetPushMessageJob : IJob
    {
        /// <summary>
        /// 作业接口
        /// </summary>
        public virtual void Execute(IJobExecutionContext context)
        {
            LogManage.Add(string.Format("{0}执行定时任务", DateTime.Now));
        }
    }
}