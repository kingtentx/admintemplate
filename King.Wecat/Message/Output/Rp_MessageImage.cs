using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace King.Wecat.Models
{
    public class Rp_MessageImage : MessageBase
    {
        public Image Item { get; set; }


        public override string ToXml()
        {
            XElement element = new XElement("xml");
            element.SetElementValue(nameof(ToUserName), $"<![CDATA[{ToUserName}]]>");
            element.SetElementValue(nameof(FromUserName), $"<![CDATA[{FromUserName}]]>");
            element.SetElementValue(nameof(CreateTime), CreateTime.ToString());
            element.SetElementValue(nameof(MsgType), $"<![CDATA[{MsgType}]]>");

            XElement xml = new XElement(nameof(Image));
            xml.SetElementValue(nameof(Item.MediaId), $"<![CDATA[{Item.MediaId}]]>");
            element.Add(xml);

            return System.Web.HttpUtility.HtmlDecode(element.ToString());
        }

        //public override void LoadXml(string data)
        //{
        //    XElement element = XElement.Parse(data);
        //    ToUserName = element.Element(nameof(ToUserName)).Value;
        //    FromUserName = element.Element(nameof(FromUserName)).Value;
        //    CreateTime = long.Parse(element.Element(nameof(CreateTime)).Value);
        //    MsgType = element.Element(nameof(MsgType)).Value;
        //    Image img = new Image()
        //    {
        //        MediaId = element.Element(nameof(Image)).Element(nameof(Img.MediaId)).Value
        //    };
        //    Img = img;
        //}
    }

    public class Image
    {
        /// <summary>
        /// 通过素材管理中的接口上传多媒体文件，得到的id。   
        /// </summary>
        public string MediaId { get; set; }
    }
}
