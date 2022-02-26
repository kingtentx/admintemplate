using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.AdminSite.Models
{
    public class TagsModel
    {
        /// <summary>
        /// 标签ID
        /// </summary>     
        public int TagsId { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>       
        public string TagsName { get; set; }
        /// <summary>
        /// 标签类型
        /// </summary>
        public int TagsType { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }

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

        public virtual string TypeName
        {
            get
            {
                var name = string.Empty;
                if (TagsType > 0)
                {
                    name = Helper.EnumHelper.GetDescription((Utils.Enums.TagsType)TagsType);
                }
                return name;
            }
        }
    }
}
