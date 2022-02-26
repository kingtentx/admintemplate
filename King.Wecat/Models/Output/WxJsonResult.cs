using System;
using System.Collections.Generic;
using System.Text;
using King.Wecat.Enum;

namespace King.Wecat.Models
{
    public class WxJsonResult
    {
        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public WxJsonResult() { }

        // <summary>
        /// 返回结果代码
        /// </summary>
        public ReturnCode errcode { get; set; }

        /// <summary>
        /// 返回结果消息
        /// </summary>
        public virtual string errmsg { get; set; }

        /// <summary>
        /// 返回消息代码数字（同errcode枚举值）
        /// </summary>
        public int ErrorCodeValue { get { return (int)errcode; } }

        public override string ToString()
        {
            return string.Format("WxJsonResult：{{errcode:'{0}',errcode_name:'{1}',errmsg:'{2}'}}",(int)errcode, errcode.ToString(), errmsg);
        }
    }
}
