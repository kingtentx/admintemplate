using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace King.Wecat.Models
{
    public class Rp_MessageVoice : MessageBase
    {
        public Voice Item { get; set; }


        public override string ToXml()
        {
            XElement element = new XElement("xml");
            element.SetElementValue(nameof(ToUserName), $"<![CDATA[{ToUserName}]]>");
            element.SetElementValue(nameof(FromUserName), $"<![CDATA[{FromUserName}]]>");
            element.SetElementValue(nameof(CreateTime), CreateTime.ToString());
            element.SetElementValue(nameof(MsgType), $"<![CDATA[{MsgType}]]>");

            XElement xml = new XElement(nameof(Voice));
            xml.SetElementValue(nameof(Item.MediaId), $"<![CDATA[{Item.MediaId}]]>");
            element.Add(xml);

            return System.Web.HttpUtility.HtmlDecode(element.ToString());
        }        
    }

    public class Voice
    {
        /// <summary>
        /// 通过素材管理中的接口上传多媒体文件，得到的id。   
        /// </summary>
        public string MediaId { get; set; }
    }
}
