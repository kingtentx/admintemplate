using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace King.Helper
{
    public class StringHelper
    {
        public static string TextToHtml(string content)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(content);
            stringBuilder.Replace("&", "&amp;");
            stringBuilder.Replace("<", "&lt;");
            stringBuilder.Replace(">", "&gt;");
            stringBuilder.Replace("\"", "&quot;");
            stringBuilder.Replace(" ", "&nbsp;");
            stringBuilder.Replace("\t", "&nbsp;&nbsp;");
            stringBuilder.Replace("\r", "");
            stringBuilder.Replace("\n", "<br />");
            return stringBuilder.ToString();
        }
        public static string HtmlToText(string content)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(content);
            stringBuilder.Replace("<br />", "\n");
            stringBuilder.Replace("<br/>", "\n");
            stringBuilder.Replace("&nbsp;&nbsp;", "\t");
            stringBuilder.Replace("&nbsp;", " ");
            stringBuilder.Replace("&#39;", "'");
            stringBuilder.Replace("&quot;", "\"");
            stringBuilder.Replace("&gt;", ">");
            stringBuilder.Replace("&lt;", "<");
            stringBuilder.Replace("&amp;", "&");
            return stringBuilder.ToString();
        }

        public static string WipeScript(string html)
        {
            return StringHelper.WipeScript(html, false);
        }
        public static string WipeScript(string html, bool newWindow)
        {
            return StringHelper.WipeScript(html, newWindow, false);
        }
        public static string WipeScript(string html, bool newWindow, bool replceHref)
        {
            Regex regex = new Regex("<style[\\s\\S]+</style *>", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex("<script[\\s\\S]+</script *>", RegexOptions.IgnoreCase);
            Regex regex3 = new Regex(" href *= *[\\s\\S]*script *:", RegexOptions.IgnoreCase);
            Regex regex4 = new Regex("[\\s]*on[a-zA-Z]*[\\s]=[\\s]*", RegexOptions.IgnoreCase);
            Regex regex5 = new Regex("<iframe[\\s\\S]+</iframe *>", RegexOptions.IgnoreCase);
            Regex regex6 = new Regex("<frameset[\\s\\S]+</frameset *>", RegexOptions.IgnoreCase);
            Regex regex7 = new Regex("<form[\\s\\S]+</form *>", RegexOptions.IgnoreCase);
            Regex regex8 = new Regex("<meta[\\s\\S]+</meta *>", RegexOptions.IgnoreCase);
            Regex regex9 = new Regex("<meta[\\s\\S]+/{1}>{1}", RegexOptions.IgnoreCase);
            Regex regex10 = new Regex("display[\\s]*:[\\s]*[a-zA-Z]*[-]*[a-zA-Z]*[\\s]*;*", RegexOptions.IgnoreCase);
            Regex regex11 = new Regex("visibility[\\s]*:[\\s]*[a-zA-Z]*[\\s]*;*", RegexOptions.IgnoreCase);
            Regex regex12 = new Regex("position[\\s]*:[\\s]*[a-zA-Z]*[-]*[a-zA-Z]*[\\s]*;*", RegexOptions.IgnoreCase);
            Regex regex13 = new Regex("z-index[\\s]*:[\\s]*[-]*\\d+[\\s]*;*", RegexOptions.IgnoreCase);
            Regex regex14 = new Regex("float[\\s]*:[\\s]*[a-zA-Z]*[\\s]*;*", RegexOptions.IgnoreCase);
            Regex regex15 = new Regex("behavior[\\s]*:([\\s]*[a-zA-Z]*[(]*[\\s\\S]+[)]*[\\s]*)*;*", RegexOptions.IgnoreCase);
            Regex regex16 = new Regex(" class[\\s]*=[\"]{0,1}([\\s]*([a-zA-Z0-9]|[-]|[_])*[\\s]*)+[\"]{0,1}", RegexOptions.IgnoreCase);
            html = regex.Replace(html, "");
            html = regex2.Replace(html, "");
            html = regex3.Replace(html, "");
            html = regex4.Replace(html, " _disibledevent=");
            html = regex5.Replace(html, "");
            html = regex6.Replace(html, "");
            html = regex7.Replace(html, "");
            html = regex8.Replace(html, "");
            html = regex9.Replace(html, "");
            html = regex10.Replace(html, "");
            html = regex11.Replace(html, "");
            html = regex12.Replace(html, "");
            html = regex13.Replace(html, "");
            html = regex14.Replace(html, "");
            html = regex15.Replace(html, "");
            html = regex16.Replace(html, "");
            if (newWindow)
            {
                Regex regex17 = new Regex("[\\s]*target[\\s]*=[\\s]*[\"]([_blank]|[_parent]|[_search]|[_self]|[_top])*[\\s]*[\"]", RegexOptions.IgnoreCase);
                html = regex17.Replace(html, " ");
                html = Regex.Replace(html, "(<a[\\s]+)", "$1 target=\"_blank\" ", RegexOptions.IgnoreCase);
            }
            return html;
        }
        public static int GetLength(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            return System.Text.Encoding.Default.GetBytes(str).Length;
        }
        public static string SqlEncode(string sql)
        {
            if (sql == null || sql.Length == 0)
            {
                return string.Empty;
            }
            return sql.Trim().Replace("'", "''");
        }
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, "[-|;|,|\\/|\\(|\\)|\\[|\\]|\\}|\\{|%|@|\\*|!|\\']");
        }

        /// <summary>  
        /// 删除SQL注入特殊字符  
        /// 对输入参数sql为Null的判断  
        /// </summary>  
        public static string StripSQLInjection(string sql)
        {
            if (!string.IsNullOrEmpty(sql))
            {
                //过滤 ' --  
                string pattern1 = @"(\%27)|(\')|(\-\-)";

                //防止执行 ' or  
                string pattern2 = @"((\%27)|(\'))\s*((\%6F)|o|(\%4F))((\%72)|r|(\%52))";

                //防止执行sql server 内部存储过程或扩展存储过程  
                string pattern3 = @"\s+exec(\s|\+)+(s|x)p\w+";

                sql = Regex.Replace(sql, pattern1, string.Empty, RegexOptions.IgnoreCase);
                sql = Regex.Replace(sql, pattern2, string.Empty, RegexOptions.IgnoreCase);
                sql = Regex.Replace(sql, pattern3, string.Empty, RegexOptions.IgnoreCase);
            }
            return sql;
        }

        /// <summary>  
        ///  判断是否有非法字符 
        /// </summary>  
        /// <param name="strString"></param>  
        /// <returns>返回TRUE表示有非法字符，返回FALSE表示没有非法字符。</returns>  
        public static bool CheckBadStr(string strString)
        {
            bool outValue = false;
            if (strString != null && strString.Length > 0)
            {
                string[] bidStrlist = new string[9];
                bidStrlist[0] = "'";
                bidStrlist[1] = ";";
                bidStrlist[2] = ":";
                bidStrlist[3] = "%";
                bidStrlist[4] = "@";
                bidStrlist[5] = "&";
                bidStrlist[6] = "#";
                bidStrlist[7] = "\"";
                bidStrlist[8] = "net user";
                bidStrlist[9] = "exec";
                bidStrlist[10] = "net localgroup";
                bidStrlist[11] = "select";
                bidStrlist[12] = "asc";
                bidStrlist[13] = "char";
                bidStrlist[14] = "mid";
                bidStrlist[15] = "insert";
                bidStrlist[19] = "order";
                bidStrlist[20] = "exec";
                bidStrlist[21] = "delete";
                bidStrlist[22] = "drop";
                bidStrlist[23] = "truncate";
                bidStrlist[24] = "xp_cmdshell";
                bidStrlist[25] = "<";
                bidStrlist[26] = ">";
                string tempStr = strString.ToLower();
                for (int i = 0; i < bidStrlist.Length; i++)
                {
                    if (tempStr.IndexOf(bidStrlist[i]) != -1)             
                    {
                        outValue = true;
                        break;
                    }
                }
            }
            return outValue;
        }

        public static string SqlSafe(string Parameter)
        {
            Parameter = Parameter.ToLower();
            Parameter = Parameter.Replace("'", "");
            Parameter = Parameter.Replace(">", ">");
            Parameter = Parameter.Replace("<", "<");
            Parameter = Parameter.Replace("\n", "<br>");
            Parameter = Parameter.Replace("\0", "·");
            return Parameter;
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CutString(string str, int length)
        {
            return StringHelper.CutString(str, 0, length, string.Empty);
        }
        public static string CutString(string str, int length, string def)
        {
            return StringHelper.CutString(str, 0, length, def);
        }
        public static string CutString(string str, int startIndex, int length, string def)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length *= -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex -= length;
                    }
                }
                if (startIndex > str.Length)
                {
                    return "";
                }
            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                if (length + startIndex <= 0)
                {
                    return "";
                }
                length += startIndex;
                startIndex = 0;
            }
            if (str.Length - startIndex <= length)
            {
                length = str.Length - startIndex;
                def = string.Empty;
            }
            string result;
            try
            {
                result = str.Substring(startIndex, length) + def;
            }
            catch
            {
                result = str + def;
            }
            return result;
        }

        public static string CutStr(string str, int len)
        {
            if (str == null || str.Length == 0 || len <= 0)
            {
                return string.Empty;
            }

            int l = str.Length;

            #region 计算长度
            int clen = 0;
            while (clen < len && clen < l)
            {
                //每遇到一个中文，则将目标长度减一。
                if ((int)str[clen] > 128) { len--; }
                clen++;
            }
            #endregion

            if (clen < l)
            {
                return str.Substring(0, clen) + "...";
            }
            else
            {
                return str;
            }
        }

        public static string RemoveHtml(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }
            string pattern = "<[^>]*>";
            return Regex.Replace(content, pattern, string.Empty, RegexOptions.IgnoreCase).Trim();
        }
        public static bool IsDateTime(string value)
        {
            try
            {
                System.DateTime.Parse(value);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool IsTime(string timeval)
        {
            return !string.IsNullOrEmpty(timeval) && Regex.IsMatch(timeval, "^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }
        public static bool IsInt(string str)
        {
            return !string.IsNullOrEmpty(str) && Regex.IsMatch(str, "^[0-9]*$");
        }
        public static bool IsNumeric(string str)
        {
            return str != null && str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, "^[-]?[0-9]*[.]?[0-9]*$") && (str.Length < 10 || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'));
        }
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            for (int i = 0; i < strNumber.Length; i++)
            {
                string str = strNumber[i];
                if (!StringHelper.IsNumeric(str))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool IsDouble(string expression)
        {
            return expression != null && Regex.IsMatch(expression, "^([0-9])[0-9]*(\\.\\w*)?$");
        }
        public static bool IsUrl(string strUrl)
        {
            return !string.IsNullOrEmpty(strUrl) && strUrl.IndexOf("http://") == 0;
        }
        public static bool IsEmail(string strEmail)
        {
            return !string.IsNullOrEmpty(strEmail) && Regex.IsMatch(strEmail, "^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");
        }

        public static bool IsTelphone(string strTelphone)
        {
            return !string.IsNullOrEmpty(strTelphone) && Regex.IsMatch(strTelphone, "^1(3([0-35-9]\\d|4[1-8])|4[14-9]\\d|5([0125689]\\d|7[1-79])|66\\d|7[2-35-8]\\d|8\\d{2}|9[89]\\d)\\d{7}$");
        }
        public static bool IsIPAddress(string ipAddress)
        {
            return !string.IsNullOrEmpty(ipAddress) && Regex.IsMatch(ipAddress, "^((2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.){3}(2[0-4]\\d|25[0-5]|[01]?\\d\\d?)$");
        }
        public static int ToInt(string str, int def)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().Length >= 11 || !Regex.IsMatch(str.Trim(), "^([-]|[0-9])[0-9]*(\\.\\w*)?$"))
            {
                return def;
            }
            int result;
            if (int.TryParse(str, out result))
            {
                return result;
            }
            return def;
        }
        public static decimal ToDecimal(string str, decimal def)
        {
            decimal result;
            try
            {
                result = decimal.Parse(str);
            }
            catch
            {
                result = def;
            }
            return result;
        }
        public static long ToInt64(string str, long def)
        {
            long result;
            if (long.TryParse(str, out result))
            {
                return result;
            }
            return def;
        }
        public static float ToFloat(string strValue, float defValue)
        {
            if (strValue == null || strValue.Length > 10)
            {
                return defValue;
            }
            float result = defValue;
            if (strValue != null)
            {
                bool flag = Regex.IsMatch(strValue, "^([-]|[0-9])[0-9]*(\\.\\w*)?$");
                if (flag)
                {
                    float.TryParse(strValue, out result);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetFileName(string url)
        {
            if (url == null)
            {
                url = string.Empty;
            }
            if (url.IndexOf("?") != -1)
            {
                string text = url.Substring(0, url.IndexOf("?"));
                string[] array = text.Split(new char[]
                {
                    '/'
                });
                return array[array.Length - 1];
            }
            return System.IO.Path.GetFileName(url);
        }
        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <param name="isLower"></param>
        /// <returns></returns>
        public static string RandCode(int length, bool isLower = false)
        {
            char[] array = new char[]
            {
                'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x','0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'
            };
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            System.Random random = new System.Random(System.DateTime.Now.Millisecond);
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(array[random.Next(0, array.Length)].ToString());
            }
            if (isLower)
            {
                return stringBuilder.ToString().ToLower();
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 添加URL参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AddUrlParameter(string url, string key, string value)
        {
            int num = url.LastIndexOf("#");
            string str = string.Empty;
            if (num > -1)
            {
                str = url.Substring(num);
                url = url.Substring(0, num);
            }
            int num2 = url.IndexOf("?");
            if (num2 < 0)
            {
                string text = url;
                url = string.Concat(new string[]
                {
                    text,
                    "?",
                    key,
                    "=",
                    value
                });
            }
            else
            {
                Regex regex = new Regex("(?<=[&\\?])" + key + "=[^\\s&#]*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                if (regex.IsMatch(url))
                {
                    url = regex.Replace(url, key + "=" + value);
                }
                else
                {
                    string text2 = url;
                    url = string.Concat(new string[]
                    {
                        text2,
                        "&",
                        key,
                        "=",
                        value
                    });
                }
            }
            return url + str;
        }
        /// <summary>
        /// 移除URL参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RemoveUrlParameter(string url, string key)
        {
            Regex regex = new Regex("[&\\?]" + key + "=[^\\s&#]*&?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return regex.Replace(url, new MatchEvaluator(StringHelper.PutAwayGarbageFromUrl));
        }
        private static string PutAwayGarbageFromUrl(Match match)
        {
            string value = match.Value;
            if (value.EndsWith("&"))
            {
                return value.Substring(0, 1);
            }
            return string.Empty;
        }
        public static string SeoUrlEncode(string str)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(str);
            stringBuilder.Replace("/", "=2F");
            stringBuilder.Replace("-", "=2D");
            stringBuilder.Replace("_", "=5F");
            stringBuilder.Replace("#", "=23");
            stringBuilder.Replace("%", "=25");
            stringBuilder.Replace("&", "=26");
            stringBuilder.Replace("+", "=2B");
            stringBuilder.Replace(":", "=3A");
            stringBuilder.Replace("\"", "=22");
            stringBuilder.Replace("<", "=3C");
            stringBuilder.Replace(">", "=3E");
            stringBuilder.Replace("|", "=7C");
            stringBuilder.Replace("?", "=3F");
            stringBuilder.Replace("*", "=27");
            stringBuilder.Replace(",", "=2C");
            stringBuilder.Replace(".", "=3D");
            return stringBuilder.ToString();
        }
        public static string SeoUrlDecode(string str)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(str);
            stringBuilder.Replace("=2F", "/");
            stringBuilder.Replace("=2D", "-");
            stringBuilder.Replace("=5F", "_");
            stringBuilder.Replace("=23", "#");
            stringBuilder.Replace("=25", "%");
            stringBuilder.Replace("=26", "&");
            stringBuilder.Replace("=2B", "+");
            stringBuilder.Replace("=3A", ":");
            stringBuilder.Replace("=22", "\"");
            stringBuilder.Replace("=3C", "<");
            stringBuilder.Replace("=3E", ">");
            stringBuilder.Replace("=7C", "|");
            stringBuilder.Replace("=3F", "?");
            stringBuilder.Replace("=27", "*");
            stringBuilder.Replace("=2C", ",");
            stringBuilder.Replace("=3D", ".");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 字符串转Unicode 
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>Unicode编码后的字符串</returns>
        private static string StringToUnicode(string str)
        {
            //source = RemoveEmoji(source);
            byte[] bytes = Encoding.Unicode.GetBytes(str);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Unicode转字符串
        /// </summary>
        /// <param name="str">经过Unicode编码的字符串</param>
        /// <returns>正常字符串</returns>
        private static string UnicodeToString(string str)
        {
            //source = RemoveEmoji(source);
            return new System.Text.RegularExpressions.Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                         str, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
        /// <summary>
        /// 过滤emoji表情
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string RemoveEmoji(string str)
        {
            string result = Regex.Replace(str, @"\p{Cs}", "");
            return result;
        }
        /// <summary> 
        /// 获取时间戳 
        /// </summary> 
        /// <returns></returns> 
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
        /// <summary>
        ///  MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMD5(string str)
        {
            if (str == null)
            {
                str = string.Empty;
            }
            //return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes_out = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            string result = BitConverter.ToString(bytes_out).Replace("-", "");
            return result;
        }
        /// <summary>
        ///  SHA1加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToSHA1(string str)
        {
            if (str == null)
            {
                str = string.Empty;
            }
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_out = sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            string result = BitConverter.ToString(bytes_out).Replace("-", "");
            return result;
        }

        /// <summary>
        ///  SHA256加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToSHA256(string str)
        {
            if (str == null)
            {
                str = string.Empty;
            }

            SHA256 sha1 = new SHA256CryptoServiceProvider();
            byte[] bytes_out = sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            string result = BitConverter.ToString(bytes_out).Replace("-", "");
            return result.ToLower();
        }
    }
}
