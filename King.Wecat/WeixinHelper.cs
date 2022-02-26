using King.Helper;
using King.Wecat.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace King.Wecat
{
    public class WeixinHelper
    {
        private static ILog log = LogManager.GetLogger("King", typeof(WeixinHelper));

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="jsapi_ticket"></param>
        /// <param name="noncestr"></param>
        /// <param name="timestamp"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetSignature(string jsapi_ticket, string noncestr, string timestamp, string url)
        {
            //string tmpStr = "jsapi_ticket=" + jsapi_ticket + "&noncestr=" + noncestr + "&timestamp=" + timestamp + "&url=" + url;

            string[] ArrTmp = { jsapi_ticket, timestamp, noncestr, url };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("&", ArrTmp);
            return StringHelper.ToSHA1(tmpStr);
        }

        /// <summary>
        /// 验证微信签名
        ///  * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool CheckSignature(string signature, string timestamp, string nonce, string token)
        {
            string[] ArrTmp = { timestamp, nonce, token };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = StringHelper.ToSHA1(tmpStr);
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 上传永久素材 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="serverPath"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<(bool Success, MediaResult Media)> HttpUploadFile(string url, string serverPath, MediaInput input)
        {
            try
            {
                // 设置参数 
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线 
                request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
                byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
                byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

                //string fileName = Path.GetFileName(path);

                //请求头部信息 
                StringBuilder sbHeader = new StringBuilder($"Content-Disposition:form-data;name=\"media\";filename=\"{input.FileName}\"\r\nContent-Type:application/octet-stream\r\n\r\n");
                byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

                FileStream fs = new FileStream(serverPath, FileMode.Open, FileAccess.Read);
                byte[] bArr = new byte[fs.Length];
                fs.Read(bArr, 0, bArr.Length);
                fs.Close();

                Stream postStream = await request.GetRequestStreamAsync();
                postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                postStream.Write(bArr, 0, bArr.Length);

                if (input.IsVideo)
                {
                    postStream.Write(Encoding.UTF8.GetBytes("--" + boundary + "\r\n"));
                    postStream.Write(Encoding.UTF8.GetBytes("Content-Disposition: form-data; name=\"description\";\r\n\r\n"));
                    postStream.Write(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { title = input.Title, introduction = input.Introduction })));
                }

                postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                postStream.Close();

                //发送请求并获取相应回应数据 
                HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求 
                Stream instream = response.GetResponseStream();
                StreamReader sr = new StreamReader(instream, Encoding.UTF8);
                //返回结果网页（html）代码 
                string content = sr.ReadToEnd();
                //content = {"errcode":45009,"errmsg":"reach max api daily quota limit hints: [EiBCQH3MRa-Qk3u8a!]"}
                var result = JsonConvert.DeserializeObject<MediaResult>(content);

                if (result.MediaId != null || result.Url != null)
                {
                    log.Info(content);
                    return (true, result);
                }
                else
                {
                    log.Error(content);
                    return (false, new MediaResult());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return (false, new MediaResult());
            }
        }


        /// <summary>
        /// 上传永久素材 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="serverPath"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<(bool Success, MediaResult Media)> HttpUploadFile(string url, Stream stream, MediaInput input)
        {
            try
            {
                // 设置参数 
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线 
                request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
                byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
                byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

                //string fileName = Path.GetFileName(path);

                //请求头部信息 
                StringBuilder sbHeader = new StringBuilder($"Content-Disposition:form-data;name=\"media\";filename=\"{input.FileName}\"\r\nContent-Type:application/octet-stream\r\n\r\n");
                byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

                byte[] bArr = new byte[stream.Length];
                stream.Read(bArr, 0, bArr.Length);
                stream.Close();

                Stream postStream = await request.GetRequestStreamAsync();
                postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                postStream.Write(bArr, 0, bArr.Length);

                if (input.IsVideo)
                {
                    postStream.Write(Encoding.UTF8.GetBytes("--" + boundary + "\r\n"));
                    postStream.Write(Encoding.UTF8.GetBytes("Content-Disposition: form-data; name=\"description\";\r\n\r\n"));
                    postStream.Write(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { title = input.Title, introduction = input.Introduction })));
                }

                postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                postStream.Close();

                //发送请求并获取相应回应数据 
                HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求 
                Stream instream = response.GetResponseStream();
                StreamReader sr = new StreamReader(instream, Encoding.UTF8);
                //返回结果网页（html）代码 
                string content = sr.ReadToEnd();
                //content = {"errcode":45009,"errmsg":"reach max api daily quota limit hints: [EiBCQH3MRa-Qk3u8a!]"}
                var result = JsonConvert.DeserializeObject<MediaResult>(content);

                if (result.MediaId != null || result.Url != null)
                {
                    log.Info(content);
                    return (true, result);
                }
                else
                {
                    log.Error(content);
                    return (false, new MediaResult());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return (false, new MediaResult());
            }
        }

        /// <summary>
        ///  POST请求,发送数据 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static async Task<string> PostUrl(string url, string postData)
        {
            byte[] data = Encoding.UTF8.GetBytes(postData);

            // 设置参数 
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            Stream outstream = request.GetRequestStream();
            await outstream.WriteAsync(data, 0, data.Length);
            outstream.Close();

            //发送请求并获取相应回应数据 
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求 
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            //返回结果网页（html）代码 
            string content = await sr.ReadToEndAsync();
            return content;
        }
    }
}
