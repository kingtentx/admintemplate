using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.Api.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AjaxResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class AjaxResultList
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Count { get; set; } = 0;
        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }
    }

    /// <summary>
    /// 状态码
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 成功1000
        /// </summary>
        Success = 1000,
        /// <summary>
        /// 无权限访问1001
        /// </summary>
        Nopermit = 1001,
        /// <summary>
        /// 访问次数达到上限1002
        /// </summary>
        Limited = 1002,
        /// <summary>
        /// 无该记录
        /// </summary>
        NULL = 1003,
        /// <summary>
        /// 服务器处理失败1004
        /// </summary>
        ServerError = 1004,
        /// <summary>
        /// 失败1005
        /// </summary>
        Fail = 1005,
        /// <summary>
        /// 参数不全1006
        /// </summary>
        ParmsError = 1006,
    }
}
