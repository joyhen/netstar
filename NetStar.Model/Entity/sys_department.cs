using System;

namespace NetStar.Model.Entity
{
    /// <summary>
    /// 部门表
    /// </summary>
    [Serializable]
    public class sys_department : NetStar.Config.IBaseEntity
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public int? parentid { get; set; }

        /// <summary>
        ///  部门名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 部门负责人
        /// </summary>
        public string head { get; set; }

        /// <summary>
        /// 是否删除，1删除，0正常
        /// </summary>
        public bool? delete { get; set; }
    }
}