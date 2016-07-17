using System.ComponentModel;

namespace NetStar.Config
{
    public class EnumCenter
    {
        /// <summary>
        /// 系统绑定的域名
        /// <remarks>主域</remarks>
        /// </summary>
        public enum SiteUrl
        {
            [Description("netstar.com")]
            netstar,
            [Description("kkp.com")]
            kkp
        }

        /// <summary>
        /// 数据库类别
        /// </summary>
        public enum DBType
        {
            /// <summary>
            /// SQLserver数据库
            /// </summary>
            MSSQL,
            /// <summary>
            /// 推送消息库
            /// </summary>
            MYSQL,
        }

        /// <summary>
        /// 系统用户类别
        /// <remarks>非系统用户角色</remarks>
        /// </summary>
        public enum AdminType
        {
            /// <summary>
            /// 系统用户
            /// </summary>
            User = 1,
            /// <summary>
            /// 超级管理员
            /// <remarks>拥有系统所有功能</remarks>
            /// </summary>
            Admin = 2,
            /// <summary>
            /// 系统后门用户
            /// <remarks>以防万一，你懂的</remarks>
            /// </summary>
            HiddenBug = 4
        }

        //...

    }
}