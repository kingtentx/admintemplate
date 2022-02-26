using System;
using System.Collections.Generic;
using System.Text;

namespace King.Wecat
{
    public static class WeixinApi
    {
        /// <summary>
        /// AccessToken
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public static string GetAccessToken(string appId, string appSecret)
        {
            return $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appId}&secret={appSecret}";
        }

        /// <summary>
        /// Ticket
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static string GetTicket(string access_token)
        {
            return $"https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={access_token}&type=jsapi";
        }

        #region user

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static string GetUserInfo(string access_token, string openid)
        {
            return $"https://api.weixin.qq.com/cgi-bin/user/info?access_token={access_token}&openid={openid}&lang=zh_CN";
        }

        /// <summary>
        /// 获取用户openid集合
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static string GetUserList(string access_token, string openid = "")
        {
            string api = $"https://api.weixin.qq.com/cgi-bin/user/get?access_token={access_token}";
            if (!string.IsNullOrWhiteSpace(openid))
            {
                api += $"&next_openid={openid}";
            }
            return api;
        }

        #endregion

        #region 菜单
        /// <summary>
        /// 获取微信菜单
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static string GetMenu(string access_token)
        {
            return $"https://api.weixin.qq.com/cgi-bin/menu/get?access_token={access_token}";
        }

        /// <summary>
        ///创建微信菜单
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static string CreateMenu(string access_token)
        {
            return $"https://api.weixin.qq.com/cgi-bin/menu/create?access_token={access_token}";
        }
        #endregion

        /// <summary>
        /// 新增临时素材 
        /// 图片（image）: 2M，支持PNG\JPEG\JPG\GIF格式
        /// 语音（voice）：2M，播放长度不超过60s，支持AMR\MP3格式
        /// 视频（video）：10MB，支持MP4格式
        /// 缩略图（thumb）：64KB，支持JPG格式
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）</param>
        /// <returns>media</returns>
        public static string CreateTempMedia(string access_token, string type)
        {
            return $"https https://api.weixin.qq.com/cgi-bin/media/upload?access_token={access_token}&type={type}";
        }
        /// <summary>
        /// 获取临时素材
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public static string GetTempMedia(string access_token, string media_id)
        {
            return $"https://api.weixin.qq.com/cgi-bin/media/get?access_token={access_token}&media_id={media_id}";
        }


        /// <summary>
        /// 新增永久图片素材 大小必须在1MB以下
        /// 调用示例（使用curl命令，用FORM表单方式上传一个图片）： curl -F media=@test.jpg 
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static string Uploadimg(string access_token)
        {
            return $"https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token={access_token} ";
        }

        /// <summary>
        /// 新增永久图文素材 
        /// </summary>
        /// <param name="access_token"></param>      
        /// <returns> { "media_id":MEDIA_ID }</returns>
        public static string AddNews(string access_token)
        {
            return $"https://api.weixin.qq.com/cgi-bin/material/add_news?access_token={access_token}";
        }
       
        /// <summary>
        /// 修改永久图文素材 
        /// </summary>
        /// <param name="access_token"></param>      
        /// <returns> { "media_id":MEDIA_ID }</returns>
        public static string UpdateNews(string access_token)
        {
            return $"https://api.weixin.qq.com/cgi-bin/material/update_news?access_token={access_token}";
        }
        /// <summary>
        /// 新增永久素材 
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）</param>
        /// <returns> { "media_id":MEDIA_ID, "url":URL }</returns>
        public static string AddMaterial(string access_token, string type)
        {
            return $"https://api.weixin.qq.com/cgi-bin/material/add_material?access_token={access_token}&type={type}";
        }

        /// <summary>
        /// 删除永久素材
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static string DeleteMaterial(string access_token)
        {
            return $"https://api.weixin.qq.com/cgi-bin/material/del_material?access_token={access_token}";
        }
       
    }
}
