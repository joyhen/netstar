using System;

namespace NetStar.Model.Entity
{
    /// <summary>
    /// 系统用户表
    /// </summary>
    [Serializable]
    public class sys_user : NetStar.Config.IBaseEntity
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string userpwd { get; set; }

        /// <summary>
        /// 真实名
        /// </summary>
        public string realname { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        public int? departmentid { get; set; }

        /// <summary>
        /// 性别，1男，0女
        /// </summary>
        public bool? gender { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string cellphone { get; set; }

        /// <summary>
        /// 座机
        /// </summary>
        public string telephone { get; set; }

        /// <summary>
        /// qq号
        /// </summary>
        public string qq { get; set; }

        /// <summary>
        /// 是否删除，1删除，0正常
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
    }
}