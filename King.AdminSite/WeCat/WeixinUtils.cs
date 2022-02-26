using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using King.Utils;
using King.Helper;
using King.Interface;
using King.Data;
using King.AdminSite.Models;
using King.Wecat;
using King.AdminSite.Config;
using Microsoft.Extensions.Options;

namespace King.AdminSite
{
    public class WeixinUtils
    {
        private ILog log = LogManager.GetLogger(Startup.logRepository.Name, typeof(WeixinUtils));
        private ICacheService _cache;
        private WecatConfig _wecatConfig;     


        public WeixinUtils(IOptions<WecatConfig> wecatConfig, ICacheService cache)
        {
            _wecatConfig = wecatConfig.Value;
            _cache = cache;
        }

        #region  token
        /// <summary>
        /// 获取调用的Token[参考微信息开发者文档]
        /// </summary>
        /// <returns></returns>       
        public string GetToken()
        {
            var cache_key = CacheKey.WeiXin_Token + _wecatConfig.AppId;
            string token;
            if (_cache.Exists(cache_key))
            {
                token = _cache.Get(cache_key).ToString();
            }
            else
            {
                //var url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appId, appSecret);
                //{"access_token":"ACCESS_TOKEN","expires_in":7200}
                //{"errcode":40013,"errmsg":"invalid appid"}

                var url = WeixinApi.GetAccessToken(_wecatConfig.AppId, _wecatConfig.AppSecret);
                var data = Common.GetDownloadString(url);              
                var obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                if (!obj.TryGetValue("access_token", out _))
                {
                    log.Error("获取access_token异常:" + data);
                    return "";
                }
                token = obj["access_token"];

                _cache.Add(cache_key, token, TimeSpan.FromSeconds(7100));
            }

            return token;
        }
        #endregion

        /// <summary>
        /// 获取jsapi_ticket
        /// </summary>
        /// <returns></returns>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
        public string GetTicket()
        {
            //{"errcode":40001,"errmsg":"invalid credential, access_token is invalid or not latest hint: [hf2oPA0480vr18]"}
            //{"errcode":0,"errmsg":"ok","ticket":"bxLdikRXVbTPdHSM05e5u5sUoXNKdvsdshFKA","expires_in":7200}

            var url = WeixinApi.GetTicket(GetToken());
            var data = Common.GetDownloadString(url);
            var obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
            if (Convert.ToInt32(obj["errcode"]) > 0)
            {
                log.Error("获取GetTicket异常:" + data);
                return "";
            }

            return obj["ticket"];
        }
        

        #region 用户
        /// <summary>
        /// 用户
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public WxUserModel GetUser(string openid)
        {
            //var url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", GetAccessToken(), openid);
            var url = WeixinApi.GetUserInfo(GetToken(), openid);
            var data = Common.GetDownloadString(url);
            log.Info("GetUser:" + data);
            var user = JsonConvert.DeserializeObject<WxUserModel>(data);
           
            return user;

        }
        #endregion


    }

}
