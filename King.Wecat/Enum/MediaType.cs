using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace King.Wecat
{
    /// <summary>
    /// 素材类型
    /// </summary>
    public enum MediaType
    {
        [Description("文本")]
        Text = 0,

        [Description("图文")]
        News = 1,

        [Description("图片")]
        Image = 2,

        [Description("语音")]
        Voice = 3,

        [Description("视频")]
        Video = 4,
    }
}
