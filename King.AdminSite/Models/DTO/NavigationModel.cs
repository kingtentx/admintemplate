﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using King.Utils.Enums;

namespace King.AdminSite.Models
{
    public class NavigationModel
    {
        /// <summary>
        /// ID
        /// </summary>       
        public int Id { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>       
        public int Pid { get; set; }
       
        /// <summary>
        /// 名称
        /// </summary>       
        public string NavigationName { get; set; }
        /// <summary>
        /// 新名称
        /// </summary>      
        public string RewriteName { get; set; }      
        /// <summary>
        /// 描述
        /// </summary>      
        public string Description { get; set; }
        /// <summary>
        /// 指定跳转链接
        /// </summary>     
        public string LinkUrl { get; set; }
        /// <summary>
        /// 排序越大越靠后
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否主页
        /// </summary>
        public bool IsHomePage { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
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

        public bool IsDelete { get; set; }
    }
}
