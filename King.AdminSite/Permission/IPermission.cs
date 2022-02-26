using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using King.Utils.Enums;
using King.AdminSite.Models;


namespace King.AdminSite
{
    public interface IPermission
    {

        /// <summary>
        /// 检查菜单权限
        /// </summary>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool CheckPermission(LoginUser user, string code, PermissionType type);

        /// <summary>
        /// 获取角色菜单
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<ModuleModel> GetRoleMenus(LoginUser user);

        /// <summary>
        /// 获取左侧菜单
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<MenuTreeModel> GetLeftMenus(LoginUser user);

        List<MenuTreeModel> GetMenu();
    }
}
