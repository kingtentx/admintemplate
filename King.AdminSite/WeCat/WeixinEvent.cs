using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using King.AdminSite.Models;
using King.Data;
using King.Helper;
using King.Interface;
using King.Wecat;
using King.Wecat.Models;
using Microsoft.EntityFrameworkCore;

namespace King.AdminSite.WeCat
{
    public class WeixinEvent
    {
        private SendMessage send = new SendMessage();
        private IBllService<User_Reply> _replyService;
        private IBllService<Wx_KeyWordsReply> _autoreplyService;
        private IBllService<Wx_Article> _wxarticleService;
        private IBllService<Wx_Keywords> _keywordsService;

        public WeixinEvent(IBllService<User_Reply> replyService,
             IBllService<Wx_KeyWordsReply> autoreplyService,
             IBllService<Wx_Article> wxarticleService,
             IBllService<Wx_Keywords> keywordsService
            )
        {
            _replyService = replyService;
            _autoreplyService = autoreplyService;
            _wxarticleService = wxarticleService;
            _keywordsService = keywordsService;
        }
        /// <summary>
        /// 用户关注事件
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task<string> SubscribeEvent(MessageBase msg)
        {
            string result = string.Empty;
            var model = await _autoreplyService.GetOneAsync(p => p.IsSubscribe == true && p.IsActive == true);
            if (model != null)
            {
                var mediaType = Convert.ToInt32(model.MsgType);
                switch (mediaType)
                {
                    case (int)MediaType.Text:
                        result = send.Rp_Text(msg.FromUserName, msg.ToUserName, model.Content);
                        break;
                    case (int)MediaType.Image:
                        result = send.Rp_Image(msg.FromUserName, msg.ToUserName, new Image() { MediaId = model.Image_MediaId });
                        break;
                    case (int)MediaType.Video:
                        result = send.Rp_Video(msg.FromUserName, msg.ToUserName, new Video() { MediaId = model.Video_MediaId, Title = "", Description = "" });
                        break;
                    case (int)MediaType.Voice:
                        result = send.Rp_Voice(msg.FromUserName, msg.ToUserName, new Voice() { MediaId = model.Voice_MediaId });
                        break;
                }
            }
            return result;
        }

        #region 菜单CLICK事件
        /// <summary>
        /// CLICK事件
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<string> EventKey(MessageBase msg, string keys)
        {
            string result;

            //string content;
            //switch (keys)
            //{
            //    case "MENU_ZQZL":
            //        List<Wecat.Models.Article> list = new List<Wecat.Models.Article>();

            //        var article = new Wecat.Models.Article()
            //        {
            //            Title = "微品生活，享受美好生活~",
            //            Description = "商品全场九折",
            //            PicUrl = "http://www.izaoban.com/group1/M00/00/79/wKgAGFw_-euAE3DwAAYyyPibBts345_375x200.png",
            //            Url = "http://www.vphonor.com?openid=" + msg.FromUserName,
            //        };

            //        var article2 = new Wecat.Models.Article()
            //        {
            //            Title = "微品甄选，全场五折，全场五折，全场五折",
            //            Description = "全场五折，",
            //            PicUrl = "http://007h5.vpclub.cn/upload/image/202005/08/637245628739301407.png",
            //            Url = "https://viph5.vpclub.cn?openid=" + msg.FromUserName,
            //        };
            //        list.Add(article);
            //        list.Add(article2);

            //        result = send.Rp_News(msg.FromUserName, msg.ToUserName, list);
            //        break;
            //    default:
            //        content = "您点击的内容是：" + keys;
            //        result = send.Rp_Text(msg.FromUserName, msg.ToUserName, content);
            //        break;
            //}

            result = await ReplyContent(msg.FromUserName, msg.ToUserName, keys, true);

            return result;
        }
        #endregion      

        #region 接收的消息处理
        /// <summary>
        /// 接收文本消息
        /// </summary>
        /// <param name="baseXml"></param>
        /// <param name="msgType"></param>
        /// <returns></returns>
        public async Task<string> EventText(MessageText msg)
        {
            //string content = "您发送的内容是：" + msg.Content;
            //result = send.Rp_Text(msg.FromUserName, msg.ToUserName, content);

            string result;
            var reply_text = new User_Reply()
            {
                Openid = msg.FromUserName,
                MsgType = msg.MsgType,
                MsgId = msg.MsgId,
                Content = msg.Content,
                CreateTime = DateTime.Now
            };
            await _replyService.AddAsync(reply_text);
            result = await ReplyContent(msg.FromUserName, msg.ToUserName, msg.Content);
            return result;
        }

        /// <summary>
        /// 接收图片消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task<string> EventImage(MessageImage msg)
        {
            string result = string.Empty;

            var reply_pic = new User_Reply()
            {
                Openid = msg.FromUserName,
                MsgType = msg.MsgType,
                MsgId = msg.MsgId,
                PicUrl = msg.PicUrl,
                MediaId = msg.MediaId,
                CreateTime = DateTime.Now
            };
            await _replyService.AddAsync(reply_pic);


            return result;
        }

        /// <summary>
        /// 接收语音消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task<string> EventVoice(MessageVoice msg)
        {
            string result = string.Empty;

            var reply_voice = new User_Reply()
            {
                Openid = msg.FromUserName,
                MsgType = msg.MsgType,
                MsgId = msg.MsgId,
                MediaId = msg.MediaId,
                Format = msg.Format,
                Recognition = msg.Recognition,
                CreateTime = DateTime.Now
            };
            await _replyService.AddAsync(reply_voice);


            return result;
        }

        /// <summary>
        /// 接收视频消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task<string> EventVideo(MessageVideo msg)
        {
            string result = string.Empty;

            var reply_video = new User_Reply()
            {
                Openid = msg.FromUserName,
                MsgType = msg.MsgType,
                MsgId = msg.MsgId,
                MediaId = msg.MediaId,
                ThumbMediaId = msg.ThumbMediaId,
                CreateTime = DateTime.Now
            };
            await _replyService.AddAsync(reply_video);


            return result;
        }

        /// <summary>
        /// 接收地理位置消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task<string> EventLocation(MessageLocation msg)
        {
            string result = string.Empty;

            var reply_local = new User_Reply()
            {
                Openid = msg.FromUserName,
                MsgType = msg.MsgType,
                MsgId = msg.MsgId,
                Location_X = msg.Location_X,
                Location_Y = msg.Location_Y,
                Scale = msg.Scale,
                Label = msg.Label,
                CreateTime = DateTime.Now
            };
            await _replyService.AddAsync(reply_local);


            return result;
        }
        #endregion

        /// <summary>
        /// 处理关键词
        /// </summary>
        /// <param name="fromUserName">接收人</param>
        /// <param name="toUserName">发送人</param>
        /// <param name="keyWords">关键词</param>
        /// <param name="IsClick">是否菜单点击的关键词</param>
        /// <returns></returns>
        private async Task<string> ReplyContent(string fromUserName, string toUserName, string keyWords, bool IsClick = false)
        {
            string result = string.Empty;

            var datalist = await _autoreplyService.GetListAsync(p => p.IsSubscribe == false && p.IsActive == true);

            if (datalist.Count == 0)
            {
                return result;
            }

            var keywords = await _keywordsService.GetListAsync(p => datalist.Select(x => x.KeyId).Contains(p.KeyId));

            if (keywords.Count == 0)
            {
                return result;
            }

            //关键词集合（建议存储在缓存中）
            var keywords_all = keywords.Where(p => p.KeyType == 0).ToList();//全匹配
            var keywords_like = keywords.Where(p => p.KeyType == 1).ToList();//半匹配  
            var model = new Wx_KeyWordsReply();//匹配关键词结果
            var search_all = new Wx_Keywords();
            if (IsClick)//点击事件
            {
                search_all = keywords_all.Where(p => p.KeyWords == keyWords).FirstOrDefault();
                if (search_all != null)
                {
                    model = datalist.Where(p => p.KeyId == search_all.KeyId).FirstOrDefault();
                }
            }
            else//自动回复
            {
                search_all = keywords_all.Where(p => p.KeyWords == keyWords).FirstOrDefault();
                if (search_all != null)//全匹配
                {
                    model = datalist.Where(p => p.KeyId == search_all.KeyId).FirstOrDefault();
                }
                else //半匹配
                {
                    var search_like = keywords_like.Where(p => p.KeyWords.Contains(keyWords)).FirstOrDefault();
                    model = datalist.Where(p => p.KeyId == search_all.KeyId).FirstOrDefault();
                }
            }
            if (model != null)
            {
                //var model = datalist.Where(p => p.KeyId == keywordsList_like.KeyId).FirstOrDefault();
                int[] typeArray = Array.ConvertAll(model.MsgType.TrimEnd(',').Split(','), int.Parse);
                if (model.ReplyType == 0)//待修改
                {
                    //var msg = string.Empty;
                    //foreach (var type in typeArray)
                    //{
                    //    switch (type)
                    //    {
                    //        case (int)MediaType.Text:
                    //            msg += send.Rp_Text(fromUserName, toUserName, model.Content);
                    //            break;
                    //        case (int)MediaType.Image:
                    //            msg += send.Rp_Image(fromUserName, toUserName, new Image() { MediaId = model.Image_MediaId });
                    //            break;
                    //        case (int)MediaType.Video:
                    //            msg += send.Rp_Video(fromUserName, toUserName, new Video() { MediaId = model.Video_MediaId, Title = "", Description = "" });
                    //            break;
                    //        case (int)MediaType.Voice:
                    //            msg += send.Rp_Voice(fromUserName, toUserName, new Voice() { MediaId = model.Voice_MediaId });
                    //            break;
                    //        case (int)MediaType.News:
                    //            var query = await _wxarticleService.GetListAsync(p => p.MeId == model.MeId);
                    //            var list = from a in query
                    //                       select new Wecat.Models.Article()
                    //                       {
                    //                           Title = a.Title,
                    //                           Description = a.Digest,
                    //                           PicUrl = a.CoverUrl,
                    //                           Url = StringHelper.AddUrlParameter(a.ContentSourceUrl, "openid", fromUserName)
                    //                       };
                    //            msg += send.Rp_News(fromUserName, toUserName, list.ToList());
                    //            break;
                    //    }
                    //}
                    //result = msg;
                }
                else //随机回复一条
                {
                    var num = new Random().Next(typeArray.Length);

                    switch (typeArray[num])
                    {
                        case (int)MediaType.Text:
                            result = send.Rp_Text(fromUserName, toUserName, model.Content);
                            break;
                        case (int)MediaType.Image:
                            result = send.Rp_Image(fromUserName, toUserName, new Image() { MediaId = model.Image_MediaId });
                            break;
                        case (int)MediaType.Video:
                            result = send.Rp_Video(fromUserName, toUserName, new Video() { MediaId = model.Video_MediaId, Title = "", Description = "" });
                            break;
                        case (int)MediaType.Voice:
                            result = send.Rp_Voice(fromUserName, toUserName, new Voice() { MediaId = model.Voice_MediaId });
                            break;
                        case (int)MediaType.News:
                            var query = await _wxarticleService.GetListAsync(p => p.MeId == model.MeId);
                            var list = from a in query
                                       select new Wecat.Models.Article()
                                       {
                                           Title = a.Title,
                                           Description = a.Digest,
                                           PicUrl = a.CoverUrl,
                                           Url = StringHelper.AddUrlParameter(a.ContentSourceUrl, "openid", fromUserName)
                                       };
                            result = send.Rp_News(fromUserName, toUserName, list.ToList());
                            break;
                    }
                }
            }
            return result;
        }


    }
}
