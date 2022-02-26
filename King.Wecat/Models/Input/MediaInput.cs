using System;
using System.Collections.Generic;
using System.Text;

namespace King.Wecat.Models
{
    public class MediaInput
    {        
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 视频素材的标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 视频素材的描述
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 是否视频
        /// </summary>
        public bool IsVideo { get; set; } = false;
    }
}
