using System.Collections.Generic;

namespace King.AdminSite.Models
{
    public class MenuTreeModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string Href { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 是否展开
        /// </summary>
        public bool Spread { get; set; } = false;
        /// <summary>
        /// 权限代码
        /// </summary>
        public string PermissionKey { get; set; }

        public List<MenuTreeModel> Children { get; set; }

        public string[] Buttons { get; set; }
    }
}
