using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace King.Wecat.Models
{
    public class WxUsersResult
    {
        /// <summary>
        /// 总关注用户数
        /// </summary>
        [JsonProperty("total")]
        public int Total { get; set; }
        /// <summary>
        /// 单次最多拉取数 max:10000
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; } = 10000;

        [JsonProperty("next_openid")]
        public string NextOpenid { get; set; }

        [JsonProperty("data")]
        public OpenidData Data { get; set; }
    }

    public class OpenidData
    {
        [JsonProperty("openid")]
        public string[] Openid { get; set; }
    }
}
