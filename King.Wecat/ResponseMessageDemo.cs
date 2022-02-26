using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using King.Helper;
using King.Wecat.Enum;
using King.Wecat.Models;
using Newtonsoft.Json;

namespace King.Wecat
{
    public class ResponseMessageDemo
    {
        private ILog log = LogManager.GetLogger("King", typeof(ResponseMessageDemo));
        public string Message(string weixinXML)
        {
            string result = "";

            var baseMsg = new MessageBase();
            baseMsg.LoadXml(weixinXML);
            //log.Info("处理微信消息base：" + JsonConvert.SerializeObject(baseMsg));

            switch (baseMsg.MsgType)
            {
                case MsgType.Event:
                    //log.Info("事件消息：" + baseMsg.MsgType);
                    break;
                case MsgType.Text:

                    var m = Text(weixinXML);
                    //log.Info(JsonConvert.SerializeObject(m));

                    var rp_msg = new Rp_MessageText()
                    {
                        ToUserName = baseMsg.FromUserName,
                        FromUserName = baseMsg.ToUserName,
                        MsgType = m.MsgType,
                        CreateTime = StringHelper.GetTimeStamp(),
                        Content = "您发送的内容是：" + m.Content
                    };
                    result = rp_msg.ToXml();

                    #region demo
                    //var txt = "<xml><ToUserName><![CDATA[gh_9b946264f6e2]]></ToUserName>"
                    //        + "<FromUserName><![CDATA[oc5xbweGK_AcpjjEJJ_J_qEaB1FQ]]></FromUserName>"
                    //        + "<CreateTime>" + StringHelper.GetTimeStamp() + "</CreateTime>"
                    //        + "<MsgType><![CDATA[text]]></MsgType>"
                    //        + "<Content><![CDATA[你回复的内容是：id]]></Content>"
                    //        + "</xml> ";

                    //XmlDocument xx = new XmlDocument();
                    //xx.LoadXml(weixinXML);//返回XML文档

                    //XmlNode root = xx.SelectSingleNode("xml");
                    //XmlNode _Content = root.SelectSingleNode("Content");
                    //XmlNode _FromName = root.SelectSingleNode("ToUserName");//获取开发者微信号
                    //XmlNode _ToName = root.SelectSingleNode("FromUserName");//获取接收方帐号（收到的OpenID）
                    //XmlNode _CreatTime = root.SelectSingleNode("CreateTime");
                    //XmlNode _MsgType = root.SelectSingleNode("MsgType");

                    //StringBuilder sb = new StringBuilder();
                    //sb.Append("<xml>");
                    //sb.AppendFormat("<ToUserName><![CDATA[{0}]]></ToUserName>", _ToName.InnerText);
                    //sb.AppendFormat("<FromUserName><![CDATA[{0}]]></FromUserName>", _FromName.InnerText);
                    //sb.AppendFormat("<CreateTime><![CDATA[{0}]]></CreateTime>", StringHelper.GetTimeStamp());
                    //sb.Append("<MsgType><![CDATA[text]]></MsgType>");
                    //sb.AppendFormat("<Content><![CDATA[{0}]]></Content>", "你发送的内容：" + _Content.InnerText);
                    //sb.Append("</xml>");
                    //result = sb.ToString();
                    #endregion
                    break;
                default:

                    break;
            }
            log.Info("输出的内容：" + result);
            return result;
        }

        public MessageText Text(string data)
        {
            var msg = new MessageText();
            msg.LoadXml(data);
            return msg;
        }
    }
}
