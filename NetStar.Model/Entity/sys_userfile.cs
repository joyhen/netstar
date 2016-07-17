using System;

namespace NetStar.Model.Entity
{
    /// <summary>
    /// 用户文件表
    /// </summary>
    [Serializable]
    public class sys_userfile : NetStar.Config.IBaseEntity
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 文件id
        /// </summary>
        public int? fileid { get; set; }

        /// <summary>
        /// 动作id集合，逗号分隔
        /// </summary>
        public string actinos { get; set; }

        /// <summary>
        /// 添加人
        /// </summary>
        public string adduser { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime addtime { get; set; }
    }
}