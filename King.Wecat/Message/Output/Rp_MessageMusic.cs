using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace King.Wecat.Models
{
    public class Rp_MessageMusic : MessageBase
    {
        public Music Item { get; set; }


        public override string ToXml()
        {
            XElement element = new XElement("xml");
            element.SetElementValue(nameof(ToUserName), $"<![CDATA[{ToUserName}]]>");
            element.SetElementValue(nameof(FromUserName), $"<![CDATA[{FromUserName}]]>");
            element.SetElementValue(nameof(CreateTime), CreateTime.ToString());
            element.SetElementValue(nameof(MsgType), $"<![CDATA[{MsgType}]]>");

            XElement xml = new XElement(nameof(Music));
            xml.SetElementValue(nameof(Item.Title), $"<![CDATA[{Item.Title}]]>");
            xml.SetElementValue(nameof(Item.Description), $"<![CDATA[{Item.Description}]]>");
            xml.SetElementValue(nameof(Item.MusicUrl), $"<![CDATA[{Item.MusicUrl}]]>");
            xml.SetElementValue(nameof(Item.HQMusicUrl), $"<![CDATA[{Item.HQMusicUrl}]]>");
            xml.SetElementValue(nameof(Item.ThumbMediaId), $"<![CDATA[{Item.ThumbMediaId}]]>");
            element.Add(xml);

            return System.Web.HttpUtility.HtmlDecode(element.ToString());
        }
    }

    /// <summary>
    /// 视频
    /// </summary>
    public class Music
    {
        /// <summary>
        /// 音乐标题 [可空]
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 音乐描述 [可空]
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 音乐链接
        /// </summary>
        public string MusicUrl { get; set; }
        /// <summary>
        /// 高质量音乐链接，WIFI环境优先使用该链接播放音乐
        /// </summary>
        public string HQMusicUrl { get; set; }
        /// <summary>
        /// 缩略图的媒体id，通过素材管理中的接口上传多媒体文件，得到的id
        /// </summary>
        public string ThumbMediaId { get; set; }
    }
}
