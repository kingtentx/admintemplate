using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using King.Helper;
using King.Wecat.Models;

namespace King.Wecat
{
    public class SendMessage
    {
        #region 回复消息
        /// <summary>
        /// 文本
        /// </summary>
        /// <param name="fromUserName">openid</param>
        /// <param name="toUserName">开发者微信号</param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string Rp_Text(string fromUserName, string toUserName, string content)
        {
            var rp_msg = new Rp_MessageText()
            {
                ToUserName = fromUserName,
                FromUserName = toUserName,
                MsgType = MsgType.Text,
                CreateTime = StringHelper.GetTimeStamp(),
                Content = content
            };
            return rp_msg.ToXml();
        }

        /// <summary>
        /// 图片
        /// </summary>
        /// <param name="fromUserName">openid</param>
        /// <param name="toUserName">开发者微信号</param>
        /// <param name="image"></param>
        /// <returns></returns>
        public string Rp_Image(string fromUserName, string toUserName, Image image)
        {
            var rp_msg = new Rp_MessageImage()
            {
                ToUserName = fromUserName,
                FromUserName = toUserName,
                MsgType = MsgType.Image,
                CreateTime = StringHelper.GetTimeStamp(),
                Item = image,
            };
            return rp_msg.ToXml();
        }

        /// <summary>
        /// 音频
        /// </summary>
        /// <param name="fromUserName">openid</param>
        /// <param name="toUserName">开发者微信号</param>
        /// <param name="voice"></param>
        /// <returns></returns>
        public string Rp_Voice(string fromUserName, string toUserName, Voice voice)
        {
            var rp_msg = new Rp_MessageVoice()
            {
                ToUserName = fromUserName,
                FromUserName = toUserName,
                MsgType = MsgType.Voice,
                CreateTime = StringHelper.GetTimeStamp(),
                Item = voice,
            };
            return rp_msg.ToXml();
        }

        /// <summary>
        /// 视频
        /// </summary>
        /// <param name="fromUserName">openid</param>
        /// <param name="toUserName">开发者微信号</param>
        /// <param name="video"></param>
        /// <returns></returns>
        public string Rp_Video(string fromUserName, string toUserName, Video video)
        {
            var rp_msg = new Rp_MessageVideo()
            {
                ToUserName = fromUserName,
                FromUserName = toUserName,
                MsgType = MsgType.Video,
                CreateTime = StringHelper.GetTimeStamp(),
                Item = video,
            };
            return rp_msg.ToXml();
        }

        /// <summary>
        /// 图文
        /// </summary>
        /// <param name="fromUserName">openid</param>
        /// <param name="toUserName">开发者微信号</param>
        /// <param name="articles"></param>
        /// <returns></returns>
        public string Rp_News(string fromUserName, string toUserName, List<Article> articles)
        {
            var rp_msg = new Rp_MessageNews()
            {
                ToUserName = fromUserName,
                FromUserName = toUserName,
                MsgType = MsgType.News,
                CreateTime = StringHelper.GetTimeStamp(),
                ArticleCount = articles.Count,
                Articles = articles
            };
            return rp_msg.ToXml();
        }


        /// <summary>
        /// 音乐
        /// </summary>
        /// <param name="fromUserName">openid</param>
        /// <param name="toUserName">开发者微信号</param>
        /// <param name="music"></param>
        /// <returns></returns>
        public string Rp_Music(string fromUserName, string toUserName, Music music)
        {
            var rp_msg = new Rp_MessageMusic()
            {
                ToUserName = fromUserName,
                FromUserName = toUserName,
                MsgType = MsgType.Image,
                CreateTime = StringHelper.GetTimeStamp(),
                Item = music,
            };
            return rp_msg.ToXml();
        }
        #endregion

    }
}
