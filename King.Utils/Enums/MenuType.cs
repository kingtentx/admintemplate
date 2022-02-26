using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace King.Utils.Enums
{
    /// <summary>
    /// 菜单类型
    /// </summary>
    public enum MenuType
    {
        /// <summary>
        /// 模块
        /// </summary>
        [Description("模块")]
        Module = 1,

        /// <summary>
        /// 菜单
        /// </summary>
        [Description("菜单")]
        Menu = 2,

        /// <summary>
        /// 按钮
        /// </summary>
        [Description("按钮")]
        Button = 3
    }

}
