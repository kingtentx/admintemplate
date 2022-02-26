using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.AdminSite.Models
{
    public class AlbumModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int AlbumId { get; set; }     
        /// <summary>
        /// 图片
        /// </summary>

        public string ImageUrl { get; set; }
        /// <summary>
        /// 标题
        /// </summary>      

        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>

        public string Description { get; set; }
        /// <summary>
        /// 作者
        /// </summary>

        public string Author { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int TagsType { get; set; }
        /// <summary>
        /// 标签ID
        /// </summary>
        public int TagsId { get; set; }     
        /// <summary>
        /// 是否公开
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>          
        public string CreateBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>           
        public string UpdateBy { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public virtual string TagsName
        {
            get
            {
                var name = string.Empty;
                if (TagsList.Count() > 0)
                {
                    name = TagsList.Where(p => p.TagsId == TagsId).FirstOrDefault().TagsName;
                }              
                return name;
            }
        }
        /// <summary>
        /// 分类列表
        /// </summary>
        public virtual List<TagsModel> TagsList { get; set; } = new List<TagsModel>();

        public virtual List<string> ImageList { get; set; } = new List<string>();
    }
}
