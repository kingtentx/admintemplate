using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using King.Data.ExtModel;

namespace King.Data
{
    /// <summary>
    /// 微信素材
    /// </summary>
    public class Wx_Media : ExtCreateModel, ICreateByModel, IModifyModel
    {
        [Key]
        public long MeId { get; set; }       
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string FileName { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string Introduction { get; set; }
        /// <summary>
        /// 新增的永久素材的media_id
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string MediaId { get; set; }
        /// <summary>
        /// Type: news=1 image=2 voice=3 video=4 
        /// </summary>
        public int MediaType { get; set; }
        /// <summary>
        /// 新增的图片素材的图片URL（仅新增图片素材时会返回该字段）
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string WxUrl { get; set; }
        /// <summary>
        /// 本地服务器URL路径
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string Url { get; set; }
        /// <summary>
        /// 封面
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string CoverUrl { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>
        [StringLength(ModelUnits.Len_10)]
        public string ExtensionName { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// 图片宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 图片高度
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 标签ID
        /// </summary>       
        public int TagId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("MeId")]
        public virtual List<Wx_Article> Articles { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string CreateBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDelete { get ; set; }
    }
}
