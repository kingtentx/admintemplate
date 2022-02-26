using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using King.AdminSite.Config;
using King.AdminSite.Models;
using King.Data;
using King.Helper;
using King.Interface;
using King.Utils.Enums;

namespace King.AdminSite.Controllers
{
    [Authorize]
    public class AttachmentsController : AdminBaseController
    {
        private IWebHostEnvironment _hostingEnv;  
        private IPermission _permission;
        private IBllService<Attachments> _attachService;


        public AttachmentsController(IWebHostEnvironment hostingEnv, IPermission permission, IBllService<Attachments> attachService)
        {
            _hostingEnv = hostingEnv;
            _permission = permission;         
            _attachService = attachService;
        }
        public IActionResult Index()
        {
          

            if (!_permission.CheckPermission(LoginUser, MenuCode.Content_Attachments, PermissionType.View))
            {
                return Content("无访问权限");
            }

            ViewData[PageCode.PAGE_Button_Add] = _permission.CheckPermission(LoginUser, MenuCode.Content_Attachments, PermissionType.Add);
            ViewData[PageCode.PAGE_Button_Delete] = _permission.CheckPermission(LoginUser, MenuCode.Content_Attachments, PermissionType.Delete);

            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult FileUpload()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetList(int pageIndex = 1, int pageSize = 10)
        {
           
            AjaxResultList result = new AjaxResultList() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            if (!_permission.CheckPermission(LoginUser, MenuCode.Content_Attachments, PermissionType.View))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            var filename = HttpContext.Request.Query["filename"];

            var where = LambdaHelper.True<Attachments>();

            if (!string.IsNullOrWhiteSpace(filename))
                where = where.And(p => p.FileName.Contains(filename));

            var query = _attachService.GetList(where, p => p.Id, pageIndex, pageSize);

            result.Code = (int)ResultCode.Success;
            result.Msg = "成攻";
            result.Count = query.Count;
            result.Data = query.List;

            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(long? id)
        {
          
            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            if (!_permission.CheckPermission(LoginUser, MenuCode.Content_Attachments, PermissionType.Delete))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }
            var model = await _attachService.GetOneAsync(id.Value);
            var i = _attachService.Delete(model.Id);
            if (i)
            {
                var start = model.Url.IndexOf("/upload");
                var serverPath = model.Url.Substring(start, model.Url.Length - start);
                System.IO.File.Delete(_hostingEnv.WebRootPath + serverPath);//删除文件
                log.Info("删除文件：" + model.FileName);
                result.Code = (int)ResultCode.Success;
                result.Msg = "删除成功！";
            }

            return Json(result);
        }
    }
}