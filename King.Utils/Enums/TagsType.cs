using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace King.Utils.Enums
{
    /// <summary>
    /// Tags类型
    /// </summary>
    public enum TagsType
    {       
        /// <summary>
        /// 图文列表
        /// </summary>
        [Description("图文列表")]
        ImageTextList = 1,
        /// <summary>
        /// 图片列表
        /// </summary>
        [Description("图片列表")]
        ImageList = 2,

        /// <summary>
        /// 图文列表
        /// </summary>
        [Description("岗位招聘")]
        TextList = 3,
    }
}
