

namespace DYLS.Model.Response
{
    /// <summary>
    /// 响应结果类
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 操作信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }
    }
}
