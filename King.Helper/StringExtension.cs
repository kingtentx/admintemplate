using System;
using System.Collections.Generic;
using System.Text;

namespace King.Helper
{
    public static class StringExtension
    {
        /// <summary>
        /// 截取字符串，多余部分用"..."代替
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static string CutString(this string str, int length)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length <= length)
                {
                    return str;
                }
                else
                {
                    return str.Substring(0, length) + "...";
                }
            }
            else
            {
                return "";
            }
        }
    }   
}
