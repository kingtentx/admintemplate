using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace King.Utils
{
    public class Common
    {
        /// <summary>
        /// Get请求获取url地址输出内容,Encoding.UTF8编码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetDownloadString(string url)
        {
            var client = new System.Net.WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            return client.DownloadString(url);
        }

        /// <summary>   
        /// Get请求获取url地址输出内容   
        /// </summary> 
        /// <param name="url">url</param>   
        /// <param name="encoding">返回内容编码方式，例如：Encoding.UTF8</param>   
        public static string GetWeb(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";
            webRequest.Timeout = 30 * 1000;
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            StreamReader sr = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
            return sr.ReadToEnd();
        }

        /// <summary>
        /// Get请求获取url地址输出流
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Stream GetStream(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";
            webRequest.Timeout = 30 * 1000;
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            Stream sr = webResponse.GetResponseStream();
            return sr;
        }

        /// <summary>
        /// Get请求获取url地址输出
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpWebResponse GetWebResponse(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";
            webRequest.Timeout = 30 * 1000;
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            return webResponse;
        }

        /// <summary>
        /// Post请求获取url地址输出内容
        /// </summary>
        public static string PostWeb(string url, string postContent)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.Timeout = 30 * 1000;
            request.ContentType = "application/x-www-form-urlencoded";

            Encoding encoding = Encoding.GetEncoding("utf-8");
            if (!string.IsNullOrEmpty(postContent))
            {
                Stream stream = request.GetRequestStream();
                byte[] dataMenu = encoding.GetBytes(postContent);
                stream.Write(dataMenu, 0, dataMenu.Length);
                stream.Flush();
                stream.Close();
            }
            WebResponse response = request.GetResponse();
            Stream inStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(inStream);
            return sr.ReadToEnd();
        }


        /// <summary>
        /// 文件保存到服务器
        /// </summary>
        /// <param name="file"></param>
        /// <param name="serverPath"></param>
        /// <param name="saveName"></param>
        /// <returns></returns>
        public static async Task<bool> Save(Stream stream, string serverPath, string saveName)
        {
            try
            {
                if (!Directory.Exists(serverPath))
                {
                    Directory.CreateDirectory(serverPath);
                }

                await Task.Run(() =>
                {
                    using (FileStream fs = new FileStream(serverPath + saveName, FileMode.Create))
                    {
                        stream.Position = 0;
                        stream.CopyTo(fs);
                        fs.Close();
                    }
                });
                return true;

            }
            catch
            {

                return false;
            }
        }

        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="tmpDirectory">临时上传目录</param>        
        /// <param name="path">上传目录</param>
        /// <param name="saveFileName">保存之后新文件名</param>
        /// <returns></returns>
        public static async Task<bool> FileMerge(string tmpDirectory, string serverPath, string saveName)
        {
            try
            {
                var tmpPath = serverPath + tmpDirectory;//获得临时目录下面的所有文件

                var files = Directory.GetFiles(tmpPath);

                using (var fs = new FileStream(serverPath + saveName, FileMode.Create))
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
            catch 
            {
                return false;
            }

        }
    }
}
