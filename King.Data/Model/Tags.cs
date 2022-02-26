using King.Data.ExtModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace King.Data
{
    public class Tags : ExtFullModifyModel, IActiveModel, ISortModel, IModifyModel
    {
        /// <summary>
        /// 标签ID
        /// </summary>
        [Key]
        public int TagsId { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        [StringLength(ModelUnits.Len_50)]
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
    }
}
