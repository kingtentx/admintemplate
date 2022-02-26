using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace King.Wecat.Models
{
    public class Rp_MessageVideo : MessageBase
    {
        public Video Item { get; set; }


        public override string ToXml()
        {
            XElement element = new XElement("xml");
            element.SetElementValue(nameof(ToUserName), $"<![CDATA[{ToUserName}]]>");
            element.SetElementValue(nameof(FromUserName), $"<![CDATA[{FromUserName}]]>");
            element.SetElementValue(nameof(CreateTime), CreateTime.ToString());
            element.SetElementValue(nameof(MsgType), $"<![CDATA[{MsgType}]]>");

            XElement xml = new XElement(nameof(Video));
            xml.SetElementValue(nameof(Item.Title), $"<![CDATA[{Item.Title}]]>");
            xml.SetElementValue(nameof(Item.Description), $"<![CDATA[{Item.Description}]]>");
            xml.SetElementValue(nameof(Item.MediaId), $"<![CDATA[{Item.MediaId}]]>");
            element.Add(xml);

            return System.Web.HttpUtility.HtmlDecode(element.ToString());
        }
    }

    /// <summary>
    /// 视频
    /// </summary>
    public class Video
    {
        /// <summary>
        /// 通过素材管理中的接口上传多媒体文件，得到的id。   
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 视频消息的标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// /视频消息的描述
        /// </summary>
        public string Description { get; set; }
    }
}
