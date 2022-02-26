using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace King.Wecat
{
    public enum WeixinType
    {
        [Description("事件")]
        Event = 0,

        [Description("文本")]
        Text = 1,

        [Description("图片")]        
        Image = 2,

        [Description("语音")]
        Voice = 3,

        [Description("视频")]
        Video = 4,

        [Description("小视频")]
        ShortVideo = 5,

        [Description("地理位置")]
        Location = 6,

        [Description("链接")]
        Link = 7,

        [Description("图文")]
        News = 8,
    }
}
