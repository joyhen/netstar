using System;

namespace NetStar.Model.Entity
{
    /// <summary>
    /// 系统文件
    /// </summary>
    [Serializable]
    public class sys_file : NetStar.Config.IBaseEntity
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 父id
        /// </summary>
        public int? parentid { get; set; }

        /// <summary>
        /// 文件或模块名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 是否删除，1是，0正常
        /// </summary>
        public bool? delete { get; set; }

        /// <summary>
        /// 添加人
        /// </summary>
        public string adduser { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime addtime { get; set; }

        /// <summary>
        /// 文件有效时间内访问，开始时间
        /// </summary>
        public DateTime? starttime { get; set; }

        /// <summary>
        /// 文件有效时间内访问，结束时间
        /// </summary>
        public DateTime? endtime { get; set; }
    }
}