using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using King.AdminSite.Models;
using King.Data;
using King.Interface;
using King.Utils.Enums;
using Microsoft.AspNetCore.Hosting;
using System.Xml.Linq;
using King.Helper;

namespace King.AdminSite
{
    public class Permission : IPermission
    {
        private ILog log = LogManager.GetLogger(Startup.logRepository.Name, typeof(Permission));
        private IWebHostEnvironment _hostingEnv;
        private IBllService<RoleMenu> _rolemenuService;
        private ICacheService _cache;

        public Permission(ICacheService cache, IWebHostEnvironment hostingEnv,
            IBllService<RoleMenu> rolemenuService)
        {
            _rolemenuService = rolemenuService;
            _cache = cache;
            _hostingEnv = hostingEnv;
        }

        #region 权限

        /// <summary>
        /// 检查菜单权限
        /// </summary>
        /// <param name="Claims"></param>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool CheckPermission(LoginUser user, string code, PermissionType type)
        {
            try
            {
                if (user.IsAdmin)
                {
                    return true;
                }
                else
                {
                    var list = this.GetRolePermission(user);
                    var key = type == PermissionType.View ? code : code + "_" + type;
                    return list.Contains(key) ? true : false;
                }
            }
            catch (Exception ex)
            {
                log.Error("检查权限异常CheckPermission" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private List<string> GetRolePermission(LoginUser user)
        {
            var list = new List<string>();

            if (user.IsAdmin)
                return list;

            if (_cache.Exists(CacheKey.Permission_Menu + user.Roles))
            {
                list = _cache.Get<List<string>>(CacheKey.Permission_Menu + user.Roles);
            }
            else
            {
                string[] arrRoles = user.Roles.Split(',');
                var roles = Array.ConvertAll(arrRoles, int.Parse);
                list = _rolemenuService.GetList(p => roles.Contains(p.RoleID)).Select(p => p.Permission).ToList();

                _cache.Add(CacheKey.Permission_Menu + user.Roles, list, TimeSpan.FromHours(CacheKey.Expiration_Min_60));
            }

            return list;
        }

        #endregion

        #region 菜单

        /// <summary>
        /// 获取当前角色的菜单模块
        /// </summary>
        /// <param name="user">角色ID</param>     
        /// <returns></returns>
        public List<ModuleModel> GetRoleMenus(LoginUser user)
        {
            List<ModuleModel> menuList = new List<ModuleModel>();

            List<string> list = new List<string>();
            if (!user.IsAdmin)
            {
                list = this.GetRolePermission(user);
            }

            foreach (var module in GetMenu())
            {
                if (module.Spread)
                {
                    continue;
                }

                var model = new ModuleModel();
                model.Name = module.Title;
                model.PermissionKey = module.PermissionKey;
                if (list.Contains(module.PermissionKey) || user.IsAdmin)
                {
                    model.IsChecked = true;
                }

                if (module.Children != null)
                {
                    List<MenuModel> menus = new List<MenuModel>();
                    foreach (var menu in module.Children)
                    {
                        var node = new MenuModel();
                        node.Name = menu.Title;
                        node.PermissionKey = menu.PermissionKey;
                        node.Url = menu.Href;
                        if (list.Contains(menu.PermissionKey) || user.IsAdmin)
                        {
                            node.IsChecked = true;
                        }

                        if (menu.Buttons != null)
                        {
                            List<ButtonModel> buttons = new List<ButtonModel>();
                            foreach (var button in menu.Buttons)
                            {
                                var btn = new ButtonModel();
                                btn.Name = EnumHelper.GetDescription<PermissionType>(button);
                                btn.PermissionKey = menu.PermissionKey + "_" + button;
                                if (list.Contains(btn.PermissionKey) || user.IsAdmin)
                                {
                                    btn.IsChecked = true;
                                }
                                buttons.Add(btn);

                            }
                            node.Buttons = buttons;
                        }
                        menus.Add(node);
                    }
                    model.Menus = menus;
                }
                menuList.Add(model);
            }

            return menuList;
        }

        /// <summary>
        /// 获取左侧菜单
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<MenuTreeModel> GetLeftMenus(LoginUser user)
        {
            if (user.IsAdmin)
            {
                return GetMenu();
            }

            List<MenuTreeModel> menuList = new List<MenuTreeModel>();
            var list = this.GetRolePermission(user);

            foreach (var module in GetMenu())
            {
                var model = new MenuTreeModel();
                if (module.Spread)
                {
                    model.Title = module.Title;
                    //model.PermissionKey = module.PermissionKey;
                    model.Href = module.Href;
                    model.Icon = module.Icon;
                    model.Spread = module.Spread;
                }
                else
                {
                    if (list.Contains(module.PermissionKey))
                    {
                        model.Title = module.Title;
                        //model.PermissionKey = module.PermissionKey;
                        model.Href = module.Href;
                        model.Icon = module.Icon;
                        model.Spread = module.Spread;

                        if (module.Children != null)
                        {
                            List<MenuTreeModel> menus = new List<MenuTreeModel>();
                            foreach (var menu in module.Children)
                            {
                                var node = new MenuTreeModel();
                                if (list.Contains(menu.PermissionKey))
                                {
                                    node.Title = menu.Title;
                                    //node.PermissionKey = menu.PermissionKey;
                                    node.Href = menu.Href;
                                    node.Icon = menu.Icon;
                                    node.Spread = menu.Spread;

                                    menus.Add(node);
                                }
                            }
                            model.Children = menus;
                        }
                        menuList.Add(model);
                    }
                }

            }
            return menuList;
        }

        #endregion

        #region 获取XML菜单数据

        public List<MenuTreeModel> GetMenu()
        {
            var obj = _hostingEnv.ContentRootPath + "//AppData//PermissionData.xml";
            XDocument doc = XDocument.Load(obj);
            XElement data = doc.Element("Menus");
            var list = CreateChildTree(data);

            return list;
        }

        private List<MenuTreeModel> CreateChildTree(XElement xml)
        {
            var nodeList = new List<MenuTreeModel>();

            foreach (var item in xml.Elements())
            {
                MenuTreeModel model = new MenuTreeModel();
                model.Title = item.Element("Title").Value;
                model.Href = item.Element("Href") == null ? "" : item.Element("Href").Value;
                model.PermissionKey = item.Element("PermissionKey") == null ? "" : item.Element("PermissionKey").Value;
                model.Icon = item.Element("Icon") == null ? "" : item.Element("Icon").Value;
                model.Spread = item.Element("Spread") == null ? false : Convert.ToBoolean(item.Element("Spread").Value);
                if (item.Element("Children") != null)
                {
                    var nodes = CreateChildTree(item.Element("Children"));
                    model.Children = nodes;
                }
                if (item.Element("Buttons") != null)
                {
                    model.Buttons = item.Element("Buttons").Value.Split(",");
                }
                nodeList.Add(model);
            }
            return nodeList;
        }

        #endregion
    }
}
