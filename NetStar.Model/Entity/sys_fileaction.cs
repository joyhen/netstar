using System;

namespace NetStar.Model.Entity
{
    /// <summary>
    /// 文件事件表
    /// </summary>
    [Serializable]
    public class sys_fileaction : NetStar.Config.IBaseEntity
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string action { get; set; }

        /// <summary>
        /// 是否删除，1是，0正常
        /// </summary>
        public bool delete { get; set; }

        /// <summary>
        /// 添加人
        /// </summary>
        public string adduser { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime addtime { get; set; }

        /// <summary>
        /// 方法有效时间内访问，开始时间
        /// </summary>
        public DateTime? starttime { get; set; }

        /// <summary>
        /// 方法有效时间内访问，结束时间
        /// </summary>
        public DateTime? endtime { get; set; }
    }
}