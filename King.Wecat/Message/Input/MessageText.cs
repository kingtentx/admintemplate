using System.Web;
using System.Xml.Linq;

namespace King.Wecat.Models
{
    public class MessageText : MessageBase
    {
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public long MsgId { get; set; }
        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content { get; set; }

        public override void LoadXml(string data)
        {
            XElement element = XElement.Parse(data);
            ToUserName = element.Element(nameof(ToUserName)).Value;
            FromUserName = element.Element(nameof(FromUserName)).Value;
            CreateTime = long.Parse(element.Element(nameof(CreateTime)).Value);
            MsgType = element.Element(nameof(MsgType)).Value;
            MsgId = long.Parse(element.Element(nameof(MsgId)).Value);

            Content = element.Element(nameof(Content)).Value;
        }
    }
}
