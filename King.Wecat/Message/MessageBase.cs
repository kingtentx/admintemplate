using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace King.Wecat.Models
{
    public class MessageBase
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public long CreateTime { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string MsgType { get; set; }       


        public virtual string ToXml()
        {
            XElement element = new XElement("xml");
            element.SetElementValue(nameof(ToUserName), $"<![CDATA[{ToUserName}]]>");
            element.SetElementValue(nameof(FromUserName), $"<![CDATA[{FromUserName}]]>");
            element.SetElementValue(nameof(CreateTime), CreateTime.ToString());
            element.SetElementValue(nameof(MsgType), $"<![CDATA[{MsgType}]]>");         

            return System.Web.HttpUtility.HtmlDecode(element.ToString());
        }

        public virtual void LoadXml(string data)
        {
            XElement element = XElement.Parse(data);
            ToUserName = element.Element(nameof(ToUserName)).Value;
            FromUserName = element.Element(nameof(FromUserName)).Value;
            CreateTime = long.Parse(element.Element(nameof(CreateTime)).Value);
            MsgType = element.Element(nameof(MsgType)).Value;          
        }

    }
}
