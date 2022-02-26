using AutoMapper;
using log4net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using King.Data;
using King.Helper;
using King.Interface;
using King.Wecat;
using King.Wecat.Models;

namespace King.AdminSite.WeCat
{
    public class ResponseMessage
    {
        private ILog log = LogManager.GetLogger(Startup.logRepository.Name, typeof(ResponseMessage));
        private IMapper _mapper;
        private WeixinUtils _weixin;
        private WeixinEvent _event;
        private IBllService<User> _userService;
        private IBllService<User_Reply> _replyService;
        private IBllService<Wx_MenuViewLog> _wxViewService;

        public ResponseMessage(WeixinUtils weixin, IMapper mapper, WeixinEvent weixinEventt,
            IBllService<User> userService,
            IBllService<User_Reply> replyService,
            IBllService<Wx_MenuViewLog> wxViewService
            )
        {
            _mapper = mapper;
            _weixin = weixin;
            _event = weixinEventt;
            _userService = userService;
            _replyService = replyService;
            _wxViewService = wxViewService;
        }

        public async Task<string> Message(string weixinXML)
        {
            log.Info("接收微信消息：" + weixinXML);
            string result = string.Empty;

            var baseMsg = new MessageBase();
            baseMsg.LoadXml(weixinXML);

            switch (baseMsg.MsgType)
            {
                case MsgType.Event:
                    result = await this.Event(weixinXML);
                    break;
                case MsgType.Text:
                    var text = new MessageText();
                    text.LoadXml(weixinXML);
                    result = await _event.EventText(text);
                    break;
                case MsgType.Image:
                    var img = new MessageImage();
                    img.LoadXml(weixinXML);
                    result = await _event.EventImage(img);
                    break;
                case MsgType.Voice:
                    var voice = new MessageVoice();
                    voice.LoadXml(weixinXML);
                    result = await _event.EventVoice(voice);
                    break;
                case MsgType.Video:
                    var video = new MessageVideo();
                    video.LoadXml(weixinXML);
                    result = await _event.EventVideo(video);
                    break;
                case MsgType.ShortVideo:
                    var s_video = new MessageVideo();
                    s_video.LoadXml(weixinXML);
                    result = await _event.EventVideo(s_video);
                    break;
                case MsgType.Location:
                    var local = new MessageLocation();
                    local.LoadXml(weixinXML);
                    result = await _event.EventLocation(local);
                    break;
                default:
                    break;
            }

            log.Info("输出微信消息：" + result);
            return result;
        }

        #region 基础事件
        public async Task<string> Event(string data)
        {
            var result = string.Empty;
            var msgPush = new MessagePush();
            msgPush.LoadXml(data);

            var msgbase = _mapper.Map<MessageBase>(msgPush);

            User user = new User();

            switch (msgPush.Event)
            {
                case EventType.CLICK:
                    result = await _event.EventKey(msgbase, msgPush.EventKey);
                    break;
                case EventType.Subscribe:
                    #region 关注
                    log.Info("用户关注：openid:" + msgPush.FromUserName);
                    var wxUser = _weixin.GetUser(msgPush.FromUserName);
                    //MapperConfiguration config = new MapperConfiguration(fig => fig.CreateMap<WxUserModel, User>());
                    //var _mapper = config.CreateMapper();
                    user = _mapper.Map<User>(wxUser);

                    var model = _userService.GetModel(p => p.Openid == msgPush.FromUserName).AsNoTracking().FirstOrDefault();
                    if (model == null)
                    {
                        user.TagidList = wxUser.TagidList.Length > 0 ? string.Join(",", wxUser.TagidList) : "";
                        user.CreateTime = DateTime.Now;
                        await _userService.AddAsync(user);
                    }
                    else
                    {
                        user.Id = model.Id;
                        user.Telphone = model.Telphone;
                        user.CreateTime = model.CreateTime;
                        user.UnsubscribeTime = model.UnsubscribeTime;
                        user.TagidList = wxUser.TagidList.Length > 0 ? string.Join(",", wxUser.TagidList) : "";
                        await _userService.UpdateAsync(user);
                    }

                    result =await _event.SubscribeEvent(msgbase);

                    #endregion
                    break;
                case EventType.UnSubscribe:
                    log.Info("用户取消关注：openid:" + msgPush.FromUserName);
                    user = _userService.GetModel(p => p.Openid == msgPush.FromUserName).AsNoTracking().FirstOrDefault();
                    if (user != null)
                    {
                        user.Subscribe = 0;
                        user.UnsubscribeTime = DateTime.Now;
                        await _userService.UpdateAsync(user);
                    }
                    break;
                default:    //默认view事件，此处可统计点击率
                    log.Info($"用户openid:{msgPush.FromUserName},浏览{msgPush.EventKey}");
                    var viewlog = new Wx_MenuViewLog { Openid = msgPush.FromUserName, Url = msgPush.EventKey, CreateTime = DateTime.Now };
                    await _wxViewService.AddAsync(viewlog);
                    break;
            }

            return result;
        }
        #endregion

    }
}
