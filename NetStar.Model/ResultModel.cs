namespace NetStar.Model
{
    /// <summary>
    /// 统一接口输出实体
    /// </summary>
    public class ResultModel
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 执行成功与否
        /// </summary>
        public bool success { set; get; }
        /// <summary>
        /// 成功或错误的提示信息
        /// </summary>
        public string msg { set; get; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object data { set; get; }
    }
}