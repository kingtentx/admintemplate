using AutoMapper;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using King.AdminSite.Models;
using King.Data;
using King.Helper;
using King.Interface;
using King.Utils;
using King.Utils.Enums;
using King.Wecat;
using King.Wecat.Models;

namespace King.AdminSite.Controllers
{
    [Authorize]
    public class WxUserController : AdminBaseController
    {
        private IWebHostEnvironment _hostingEnv;
        private IPermission _permission;
        private WeixinUtils _weixin;
        private IMapper _mapper;
        private IBllService<User> _userService;
        private IBllService<User_Reply> _replyService;

        public WxUserController(IWebHostEnvironment hostingEnv, WeixinUtils weixin, IMapper mapper, IPermission permission, IBllService<User> userService, IBllService<User_Reply> replyService)
        {
            _hostingEnv = hostingEnv;
            _weixin = weixin;
            _mapper = mapper;
            _userService = userService;
            _replyService = replyService;
            _permission = permission;
        }
        public IActionResult Index()
        {
            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_User, PermissionType.View))
            {
                return Content("无访问权限");
            }

            ViewData[PageCode.PAGE_Button_Edit] = _permission.CheckPermission(LoginUser, MenuCode.Wecat_User, PermissionType.Edit);

            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetList(int pageIndex = 1, int pageSize = 10)
        {
            AjaxResultList result = new AjaxResultList() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_User, PermissionType.View))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            var name = HttpContext.Request.Query["openid"].ToString().Trim();

            var where = LambdaHelper.True<User>();

            if (!string.IsNullOrWhiteSpace(name))
                where = where.And(p => p.Openid == name);

            var query = await _userService.GetListAsync(where, p => p.Id, pageIndex, pageSize);

            var data = _mapper.Map(query.List, new List<WxUserModel>());

            result.Code = (int)ResultCode.Success;
            result.Msg = "成攻";
            result.Count = query.Count;
            result.Data = data;

            return Json(result);

        }

        #region 同步用户
        [HttpGet]
        public async Task<JsonResult> SyncUserInfo()
        {
            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_User, PermissionType.Edit))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            var url = WeixinApi.GetUserList(_weixin.GetToken());
            var data = Common.GetDownloadString(url);
            //log.Info(data);

            var list = JsonConvert.DeserializeObject<WxUsersResult>(data);

            string next_openid = list.NextOpenid;
            decimal pages = Math.Ceiling((decimal)list.Total / (decimal)list.Count); //取总页数   

            for (int i = 0; i < pages; i++)
            {
                next_openid = await SyncUser(i == 0 ? list : null, next_openid);
            }

            if (list.Total > 0)
            {
                result.Code = (int)ResultCode.Success;
                result.Msg = $"更新用户数据{list.Total}条！";
            }

            return Json(result);
        }

        /// <summary>
        /// 同步用户信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="next_openid"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        private async Task<string> SyncUser(WxUsersResult list, string next_openid)
        {
            var userList = list;

            if (list == null)
            {
                var url = WeixinApi.GetUserList(_weixin.GetToken(), next_openid);
                var data = Common.GetDownloadString(url);
                userList = JsonConvert.DeserializeObject<WxUsersResult>(data);
            }

            foreach (var id in userList.Data.Openid)
            {
                var wxUser = _weixin.GetUser(id);
                var user = _mapper.Map<User>(wxUser);
                user.TagidList = wxUser.TagidList.Length > 0 ? string.Join(",", wxUser.TagidList) : "";

                var model = _userService.GetModel(p => p.Openid == id).AsNoTracking().FirstOrDefault();
                if (model == null)
                {
                    user.CreateTime = DateTime.Now;
                    await _userService.AddAsync(user);
                }
                else
                {
                    user.Id = model.Id;
                    user.Telphone = model.Telphone;
                    await _userService.UpdateAsync(user);
                }
            }

            return userList.NextOpenid;
        }
        #endregion

        public IActionResult ReplyList()
        {
            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_User, PermissionType.View))
            {
                return Content("无访问权限");
            }
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetReplyList(int pageIndex = 1, int pageSize = 10)
        {
            AjaxResultList result = new AjaxResultList() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_UserReply, PermissionType.View))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }


            var query = await _replyService.GetListAsync(p => true, p => p.ReplyId, pageIndex, pageSize);

            result.Code = (int)ResultCode.Success;
            result.Msg = "成攻";
            result.Count = query.Count;
            result.Data = query.List;

            return Json(result);

        }


        public async Task<IActionResult> DownloadSucai(long id)
        {
            var model = await _replyService.GetOneAsync(id);
            //微信临时素材3天有效期,用小时计算
            var hours = DateTime.Now.Subtract(model.CreateTime.Value).TotalHours;
            if (Math.Floor(hours) < 3 * 24)
            {
                var fileName = "";
                switch (model.MsgType)
                {
                    case MsgType.Image:
                        fileName = model.MediaId + ".jpg";
                        break;
                    case MsgType.Video:
                        fileName = model.MediaId + ".mp4";
                        break;
                    case MsgType.ShortVideo:
                        fileName = model.MediaId + ".mp4";
                        break;
                    case MsgType.Voice:
                        fileName = model.MediaId + ".amr";
                        break;
                }

                var url = WeixinApi.GetTempMedia(_weixin.GetToken(), model.MediaId);
                var respone = Common.GetWebResponse(url);
                return File(respone.GetResponseStream(), respone.ContentType, fileName);
            }
            else
            {
                return Redirect("/WxUser/ReplyList?code=" + (int)ResultCode.Fail);
            }

        }


        #region 弃用
        /// <summary>
        /// 更新单个用户信息
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetUserInfo(string openid)
        {
            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_UserReply, PermissionType.Edit))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            var url = WeixinApi.GetUserInfo(_weixin.GetToken(), openid);
            var data = Common.GetDownloadString(url);
            //log.Info(data);

            var wxUser = JsonConvert.DeserializeObject<WxUserModel>(data);

            var user = _mapper.Map<User>(wxUser);
            var model = _userService.GetModel(p => p.Openid == openid).AsNoTracking().FirstOrDefault();
            user.Id = model.Id;
            user.Telphone = model.Telphone;
            user.CreateTime = model.CreateTime;
            user.UnsubscribeTime = model.UnsubscribeTime;
            user.TagidList = wxUser.TagidList.Length > 0 ? string.Join(",", wxUser.TagidList) : "";

            if (await _userService.UpdateAsync(user))
            {
                result.Code = (int)ResultCode.Success;
                result.Msg = $"更新成功！";
            }

            return Json(result);
        }
        #endregion
    }

}