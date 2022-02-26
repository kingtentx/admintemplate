using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace King.Wecat.Models
{
    public class MediaResult
    {
        /// <summary>
        /// 永久素材的media_id
        /// </summary>
        [JsonProperty("media_id")]
        public string MediaId { get; set; }
        /// <summary>
        /// 新增的图片素材的图片URL（仅新增图片素材时会返回该字段）
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        // <summary>
        /// 返回结果代码
        /// </summary>
        [JsonProperty("errcode")]
        public virtual int Errcode { get; set; }

        /// <summary>
        /// 返回结果消息
        /// </summary>
        [JsonProperty("errmsg")]
        public virtual string Errmsg { get; set; }
        public override string ToString()
        {
            return string.Format("WxJsonResult：{{errcode:'{0}',errmsg:'{1}'}}", Errcode, Errmsg);
        }
    }
}
