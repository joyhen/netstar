using System;

namespace NetStar.Model.Entity
{
    /// <summary>
    /// 工作记录
    /// </summary>
    [Serializable]
    public class biz_workmark : NetStar.Config.IBaseEntity
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 客户名
        /// </summary>
        public string custom { get; set; }

        /// <summary>
        /// 网址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 问题描述
        /// </summary>
        public string question { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public string attachment { get; set; }

        /// <summary>
        /// 问题类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 问题严重级别
        /// </summary>
        public string leave { get; set; }

        /// <summary>
        /// 添加人
        /// </summary>
        public string adduser { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime addtime { get; set; }

        /// <summary>
        /// 指派（处理人问题的人）
        /// </summary>
        public string calluser { get; set; }

        /// <summary>
        /// 问题状态，0未指派，1正在处理，2已解决
        /// </summary>
        public int? status { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? endtime { get; set; }

        /// <summary>
        /// 结束后还有问题，重新修改的操作记录
        /// </summary>
        public string fixitem { get; set; }
    }
}