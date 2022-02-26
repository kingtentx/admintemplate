using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace King.Wecat.Models
{
    public class MessagePush : MessageBase
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// 事件KEY值
        /// </summary>
        public string EventKey { get; set; }
       

        public override void LoadXml(string data)
        {
            XElement element = XElement.Parse(data);
            ToUserName = element.Element(nameof(ToUserName)).Value;
            FromUserName = element.Element(nameof(FromUserName)).Value;
            CreateTime = long.Parse(element.Element(nameof(CreateTime)).Value);
            MsgType = element.Element(nameof(MsgType)).Value;
            Event = element.Element(nameof(Event)).Value;
            EventKey = element.Element(nameof(EventKey)).Value;

        }
    }
}
