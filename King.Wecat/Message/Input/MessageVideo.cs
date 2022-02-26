using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace King.Wecat.Models
{
    public class MessageVideo : MessageBase
    {       
        /// <summary>
        /// 消息媒体id，可以调用获取临时素材接口拉取数据
        /// </summary>
        public string MediaId { get; set; }        
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public long MsgId { get; set; }
        /// <summary>
        /// 语音识别结果，UTF8编码
        /// </summary>
        public string ThumbMediaId { get; set; }

        public override void LoadXml(string data)
        {
            XElement element = XElement.Parse(data);
            ToUserName = element.Element(nameof(ToUserName)).Value;
            FromUserName = element.Element(nameof(FromUserName)).Value;
            CreateTime = long.Parse(element.Element(nameof(CreateTime)).Value);
            MsgType = element.Element(nameof(MsgType)).Value;        
            MediaId = element.Element(nameof(MediaId)).Value;         
            MsgId = long.Parse(element.Element(nameof(MsgId)).Value);

            ThumbMediaId = element.Element(nameof(ThumbMediaId)).Value;
        }
    }
}
