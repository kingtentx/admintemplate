using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.AdminSite.Config
{
    public class WecatConfig
    {
        /// <summary>
        /// 微信appid
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 微信secret
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// 微信token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 微信加密key
        /// </summary>
        public string EncodingAESKey { get; set; }
    }
}
