using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using King.Data.ExtModel;

namespace King.Data
{
    /// <summary>
    /// 微信用户发送记录
    /// </summary>
    public class User_Reply : ExtCreateModel
    {
        [Key]
        public long ReplyId { get; set; }
        /// <summary>
        /// 回复人
        /// </summary>
        public string Openid { get; set; }

        /// <summary>
        /// 回复类型
        /// </summary>
        [StringLength(ModelUnits.Len_50)]
        public string MsgType { get; set; }
        /// <summary>
        /// 可以调用获取临时素材接口拉取数据
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string MediaId { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string Content { get; set; }

        /// <summary>
        /// 图片链接（由系统生成）
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string PicUrl { get; set; }

        /// <summary>
        /// 语音格式，如amr，speex等 ]
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string Format { get; set; }
        [StringLength(ModelUnits.Len_100)]
        public string Recognition { get; set; }
        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string ThumbMediaId { get; set; }

        /// <summary>
        /// 地理位置维度
        /// </summary>
        public double Location_X { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public double Location_Y { get; set; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string Label { get; set; }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public int Scale { get; set; }
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public long MsgId { get; set; }
    }
}
