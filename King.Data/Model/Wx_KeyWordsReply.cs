using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using King.Data.ExtModel;

namespace King.Data
{
    public class Wx_KeyWordsReply : ExtCreateModel, IActiveModel
    {
        [Key]
        public int KeyId { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        [StringLength(60)]
        public string KeyName { get; set; }
        /// <summary>
        /// 回复类型 0-text 1-news 2-image 3-voice 4-video
        /// </summary>
        [StringLength(ModelUnits.Len_20)]
        public string MsgType { get; set; }
        /// <summary>
        /// 回复方式  0:回复全部  1: 随机回复一条
        /// </summary>
        public int ReplyType { get; set; }
        /// <summary>
        /// MediaId
        /// </summary>
        public long MeId { get; set; }
        /// <summary>
        /// 回复文字
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string Content { get; set; }
        /// <summary>
        /// 封面图片
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string CoverUrl { get; set; }
        /// <summary>
        /// 图文素材
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string News_MediaId { get; set; }
        /// <summary>
        /// 图片素材
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string Image_MediaId { get; set; }
        /// <summary>
        /// 语音素材
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string Voice_MediaId { get; set; }
        /// <summary>
        /// 视频素材
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string Video_MediaId { get; set; }
        /// <summary>
        /// 是否关注回复
        /// </summary>
        public bool IsSubscribe { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string Remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("KeyId")]
        public virtual ICollection<Wx_Keywords> WxKeyWordsList { get; set; }
    }
}
