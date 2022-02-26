using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace King.Wecat.Models
{
    public class MessageLocation : MessageBase
    {      
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public long MsgId { get; set; }
        /// <summary>
        /// 地理位置维度
        /// </summary>
        public double Location_X { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public double Location_Y { get; set; }
        /// <summary>
        ///地图缩放大小
        /// </summary>
        public int Scale { get; set; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { get; set; }       


        public override void LoadXml(string data)
        {
            XElement element = XElement.Parse(data);
            ToUserName = element.Element(nameof(ToUserName)).Value;
            FromUserName = element.Element(nameof(FromUserName)).Value;
            CreateTime = long.Parse(element.Element(nameof(CreateTime)).Value);
            MsgType = element.Element(nameof(MsgType)).Value; 
            MsgId = long.Parse(element.Element(nameof(MsgId)).Value);

            Location_X = double.Parse(element.Element(nameof(Location_X)).Value);
            Location_Y = double.Parse(element.Element(nameof(Location_Y)).Value);
            Scale = int.Parse(element.Element(nameof(Scale)).Value);
            Label = element.Element(nameof(Label)).Value;
        }
    }
}
