using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using King.AdminSite.Models;
using King.Data;
using King.Interface;

namespace King.AdminSite.Controllers
{

    [Authorize]
    public class UploadController : AdminBaseController
    {
        private IWebHostEnvironment _hostingEnv;
        private readonly IConfiguration _config;
        private IMapper _mapper;

        private string serverHost;
        private string imgUploadPath;
        private string[] imgTypes;
        private long imgMaxSize;

        private string fileUploadPath;
        private string[] fileTypes;
        private long fileMaxSize;

        private IBllService<PictureGallery> _pictureService;
        private IBllService<Attachments> _attachService;

        public UploadController(IWebHostEnvironment hostingEnv, IConfiguration configuration,
            IMapper mapper,
            IBllService<PictureGallery> pictureService,
            IBllService<Attachments> attachService
            )
        {
            _hostingEnv = hostingEnv;
            _config = configuration;
            _mapper = mapper;

            serverHost = _config["App:ServerHost"];
            imgUploadPath = $"{_config["UploadConfig:Image:Path"]}/{DateTime.Now:yyyyMM}/{DateTime.Now:dd}/";
            imgTypes = _config["UploadConfig:Image:ExtName"].Split('|');
            imgMaxSize = Convert.ToInt64(_config["UploadConfig:Image:Size"]);

            fileUploadPath = $"{_config["UploadConfig:File:Path"]}/{DateTime.Now:yyyyMM}/{DateTime.Now:dd}/";
            fileTypes = _config["UploadConfig:File:ExtName"].Split('|');
            fileMaxSize = Convert.ToInt64(_config["UploadConfig:File:Size"]);

            _pictureService = pictureService;
            _attachService = attachService;
        }

        #region 上传图片

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="file">文件</param>       
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SSIUploadImage()
        {
            var file = Request.Form.Files[0];
            if (file == null)
            {
                return BadRequest("请选择上传文件");
            }
            var oldFileName = file.FileName;//原文件名
            var extName = Path.GetExtension(file.FileName).ToLower(); //文件扩展名
            var size = file.Length;

            if (!imgTypes.Contains(extName))    //"图片格式";
            {
                return BadRequest("文件格式错误");
            }

            if (size > imgMaxSize * 1024 * 1024)    //"图片大小";
            {
                return BadRequest($"大小不能超过{imgMaxSize}M");
            }

            var saveName = DateTime.Now.Ticks.ToString() + extName;//保存的文件名

            using (var stream = file.OpenReadStream())
            {
                var strmd5 = GetMD5Value(stream);
                //if (md5 == strmd5)//校验MD5值
                //{
                //    return BadRequest($"md5值校验失败！");
                //}

                if (await Save(stream, imgUploadPath, saveName))
                {
                    var uploadpath = imgUploadPath + saveName;//返回文件的相对路径
                    Image image = Image.FromStream(stream);

                    var model = new PictureGalleryModel()
                    {
                        ImageName = oldFileName,
                        Url = serverHost + uploadpath,
                        ExtensionName = extName,
                        MD5 = strmd5,
                        Size = size,
                        Width = image.Width,
                        Height = image.Height,
                        CreateBy = LoginUser.UserName,
                        CreateTime = DateTime.Now
                    };

                    var dto = _mapper.Map<PictureGallery>(model);
                    await _pictureService.AddAsync(dto);

                    return Json(model);

                }
                else
                {
                    return BadRequest("上传失败");
                }

            }
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="file">文件</param>       
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file, string md5)
        {

            if (file == null)
            {
                return BadRequest("请选择上传文件");
            }
            var oldFileName = file.FileName;//原文件名
            var extName = Path.GetExtension(file.FileName).ToLower(); //文件扩展名
            var size = file.Length;

            if (!imgTypes.Contains(extName))    //"图片格式";
            {
                return BadRequest("文件格式错误");
            }

            if (size > imgMaxSize * 1024 * 1024)    //"图片大小";
            {
                return BadRequest($"大小不能超过{imgMaxSize}M");
            }

            var saveName = DateTime.Now.Ticks.ToString() + extName;//保存的文件名

            using (var stream = file.OpenReadStream())
            {
                var strmd5 = GetMD5Value(stream);
                //if (md5 == strmd5)//校验MD5值
                //{
                //    return BadRequest($"md5值校验失败！");
                //}

                if (await Save(stream, imgUploadPath, saveName))
                {
                    var uploadpath = imgUploadPath + saveName;//返回文件的相对路径
                    Image image = Image.FromStream(stream);

                    var model = new PictureGalleryModel()
                    {
                        ImageName = oldFileName,
                        Url = serverHost + uploadpath,
                        ExtensionName = extName,
                        MD5 = strmd5,
                        Size = size,
                        Width = image.Width,
                        Height = image.Height,
                        CreateBy = LoginUser.UserName,
                        CreateTime = DateTime.Now
                    };

                    var dto = _mapper.Map<PictureGallery>(model);
                    await _pictureService.AddAsync(dto);

                    return Json(model);

                }
                else
                {
                    return BadRequest("上传失败");
                }

            }
        }

        /// <summary>
        /// 上传base64格式图片
        /// </summary>
        /// <param name="strImgbase64"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadImageFromBase64(string strImgbase64, string md5, string fileName)
        {

            if (strImgbase64.Length == 0)
            {
                return BadRequest("请选择上传文件");
            }

            Regex reg = new Regex(@"(?<=\/)[^\/]+(?=\;)");
            var extName = "." + reg.Match(strImgbase64).ToString(); //文件扩展名

            if (!imgTypes.Contains(extName))    //"图片格式";
            {
                return BadRequest("图片格式错误");
            }

            var size = ImageSize(strImgbase64);
            if (size > imgMaxSize * 1024 * 1024)    //"图片大小";
            {
                return BadRequest($"大小不能超过{imgMaxSize}M");
            }

            var saveName = DateTime.Now.Ticks.ToString() + extName;//保存的文件名      
            var strmd5 = GetMD5Value(strImgbase64);
            //if (md5 == strmd5)//校验MD5值
            //{
            //}

            var result = SaveImageFromBase64(strImgbase64, imgUploadPath, saveName);
            if (result.Success)
            {
                var uploadpath = imgUploadPath + saveName;//返回文件的相对路径
                Image image = result.Image;

                var model = new PictureGalleryModel()
                {
                    ImageName = string.IsNullOrWhiteSpace(fileName) ? saveName : fileName,
                    Url = serverHost + uploadpath,
                    ExtensionName = extName,
                    MD5 = strmd5,
                    Size = size,
                    Width = image.Width,
                    Height = image.Height,
                    CreateBy = LoginUser.UserName,
                    CreateTime = DateTime.Now
                };

                var dto = _mapper.Map<PictureGallery>(model);
                await _pictureService.AddAsync(dto);

                return Json(model);
            }
            else
            {
                return BadRequest("上传失败");
            }
        }

        /// <summary>
        /// 解码保存Base64图片
        /// </summary>
        /// <param name="strBase64">base64</param>
        /// <param name="path">保存的路径</param>
        /// <param name="saveName">文件名（不包含扩展名）</param>
        /// <returns></returns>
        private (Image Image, bool Success) SaveImageFromBase64(string strBase64, string path, string saveName)
        {
            if (strBase64.Length == 0)
                return (null, false);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            try
            {
                int i = strBase64.IndexOf(',') + 1;
                string base64 = strBase64.Substring(i);
                byte[] b = Convert.FromBase64String(base64);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                img.Save(path + saveName);

                return (img, true);
            }
            catch (Exception ex)
            {
                log.Error("SaveImageFromBase64 异常" + ex.Message);
                return (null, false);
            }
        }


        /// <summary>
        /// 通过图片base64流判断图片等于多少字节 image图片流
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private long ImageSize(string image)
        {
            int i = image.IndexOf(',') + 1; //1.把头部的data:image/png;base64,（注意有逗号）去掉。
            string str = image.Substring(i);

            int equalIndex = str.IndexOf("=");//2.找到等号，把等号也去掉
            if (str.IndexOf("=") > 0)
            {
                str = str.Substring(0, equalIndex);
            }
            int strLength = str.Length;//3.原来的字符流大小，单位为字节
            int size = strLength - (strLength / 8) * 2;//4.计算后得到的文件流大小，单位为字节
            return (long)size;
        }
        #endregion


        #region  上传附件
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UploadAttachments(IFormFile file, string fileName, string tempDirectory, int index, int total, int totalSize = 0)
        {
            if (file == null)
            {
                return BadRequest("请选择上传文件");
            }
            var oldFileName = fileName;// file.FileName;//原文件名
            var extName = Path.GetExtension(fileName).ToLower(); //文件扩展名
            var size = file.Length;

            if (!fileTypes.Contains(extName))    //"图片格式";
            {
                return BadRequest("文件格式错误");
            }

            if (totalSize > fileMaxSize * 1024 * 1024)    //"图片大小";
            {
                return BadRequest($"大小不能超过{fileMaxSize}M");
            }

            var saveName = DateTime.Now.Ticks.ToString() + extName;//保存的文件名
            string tmp = Path.Combine(fileUploadPath, tempDirectory) + "/";//临时保存分块的目录
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var strmd5 = GetMD5Value(stream);
                    //if (md5 == strmd5)//校验MD5值
                    //{
                    //}

                    if (await Save(stream, tmp, index.ToString()))
                    {
                        var uploadpath = fileUploadPath + saveName;//返回文件的相对路径
                        bool mergeOk = false;
                        if (total == index)
                        {
                            mergeOk = await FileMerge(tmp, fileUploadPath, saveName);
                            if (mergeOk)
                            {
                                var model = new AttachmentsModel()
                                {
                                    FileName = oldFileName,
                                    Url = serverHost + uploadpath,
                                    ExtensionName = extName,
                                    MD5 = strmd5,
                                    Size = size,
                                    CreateBy = LoginUser.UserName,
                                    CreateTime = DateTime.Now
                                };

                                var dto = _mapper.Map<Attachments>(model);
                                await _attachService.AddAsync(dto);
                            }
                        }

                        Dictionary<string, object> result = new Dictionary<string, object>();
                        result.Add("number", index);
                        result.Add("mergeOk", mergeOk);
                        result.Add("filename", saveName);
                        return Json(result);
                    }
                    else
                    {
                        return BadRequest("上传失败");
                    }
                }
            }
            catch (Exception ex)
            {
                Directory.Delete(_hostingEnv.WebRootPath + tmp);//删除文件夹
                log.Error($"上传异常:{ex.Message}");
                return BadRequest("上传失败");
            }

        }
        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="tmpDirectory">临时上传目录</param>        
        /// <param name="path">上传目录</param>
        /// <param name="saveFileName">保存之后新文件名</param>
        /// <returns></returns>
        private async Task<bool> FileMerge(string tmpDirectory, string path, string saveName)
        {
            try
            {
                var tmpPath = _hostingEnv.WebRootPath + tmpDirectory;//获得临时目录下面的所有文件
                var serverPath = Path.Combine(_hostingEnv.WebRootPath + path, saveName);//最终保存的文件路径
                var files = Directory.GetFiles(tmpPath);

                using (var fs = new FileStream(serverPath, FileMode.Create))
                {
                    foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))
                    {
                        var bytes = System.IO.File.ReadAllBytes(part);
                        await fs.WriteAsync(bytes, 0, bytes.Length);
                        bytes = null;
                        System.IO.File.Delete(part);//删除分块
                    }
                    fs.Close();

                    Directory.Delete(tmpPath);//删除临时目录
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error("上传合并文件异常", ex);
                return false;
            }

        }

        #endregion

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        /// <param name="obj">类型只能为string or stream，否则将会抛出错误</param>
        /// <returns>文件的MD5值</returns>
        private string GetMD5Value(object obj)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = null;
            switch (obj)
            {
                case string str:
                    data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(str));
                    break;
                case Stream stream:
                    data = md5Hash.ComputeHash(stream);
                    break;
                case null:
                    throw new ArgumentException("参数不能为空");
                default:
                    throw new ArgumentException("参数类型错误");
            }

            return BitConverter.ToString(data).Replace("-", "");
        }

        /// <summary>
        /// 文件保存到本地
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="path"></param>
        /// <param name="saveName"></param>
        /// <returns></returns>
        private async Task<bool> Save(Stream stream, string path, string saveName)
        {
            try
            {
                var serverPath = _hostingEnv.WebRootPath + path;
                if (!Directory.Exists(serverPath))
                {
                    Directory.CreateDirectory(serverPath);
                }

                await Task.Run(async () =>
                {
                    using (FileStream fs = new FileStream(serverPath + saveName, FileMode.Create))
                    {
                        stream.Position = 0;
                        await stream.CopyToAsync(fs);
                        fs.Close();
                    }
                });
                return true;

            }
            catch (Exception ex)
            {
                log.Error("上传异常", ex);
                return false;
            }
        }
    }
}