using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace King.Wecat.Models
{
    public class Rp_MessageText : MessageBase
    {
        /// <summary>
        /// 回复的消息内容（换行：在content中能够换行，微信客户端就支持换行显示）
        /// </summary>
        public string Content { get; set; }

        public override string ToXml()
        {
            XElement element = new XElement("xml");
            element.SetElementValue(nameof(ToUserName), $"<![CDATA[{ToUserName}]]>");
            element.SetElementValue(nameof(FromUserName), $"<![CDATA[{FromUserName}]]>");
            element.SetElementValue(nameof(CreateTime), CreateTime.ToString());
            element.SetElementValue(nameof(MsgType), $"<![CDATA[{MsgType}]]>");
            element.SetElementValue(nameof(Content), $"<![CDATA[{Content}]]>");

            return System.Web.HttpUtility.HtmlDecode(element.ToString());
        }
      
    }
}
