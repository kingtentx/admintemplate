using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.AdminSite.Models
{
    public class ModuleModel
    {
        /// <summary>
        /// 名称
        /// </summary>       
        public string Name { get; set; }
        /// <summary>
        /// 权限代码
        /// </summary>   
        public string PermissionKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsChecked { get; set; } = false;

        public List<MenuModel> Menus { get; set; }
    }

    public class MenuModel
    {
        /// <summary>
        /// 名称
        /// </summary>       
        public string Name { get; set; }
        /// <summary>
        /// URL
        /// </summary>       
        public string Url { get; set; }
        /// <summary>
        /// 权限代码
        /// </summary>   
        public string PermissionKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsChecked { get; set; } = false;

        public List<ButtonModel> Buttons { get; set; }
    }

    public class ButtonModel
    {
        /// <summary>
        /// 名称
        /// </summary>       
        public string Name { get; set; }
        /// <summary>
        /// 权限代码
        /// </summary>   
        public string PermissionKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsChecked { get; set; } = false;

    }
}
