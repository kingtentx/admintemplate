using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using King.Data;
using King.Helper;
using King.AdminSite.Models;

namespace King.AdminSite
{
    public class PageUtils : ControllerBase
    {
        /// <summary>
        /// 导航递归
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tree"></param>
        /// <returns></returns>
        public static List<TreeModel> NavigationTree(List<Navigation> list, TreeModel tree)
        {
            int parentId = tree.Id;//根节点ID

            List<TreeModel> nodeList = new List<TreeModel>();

            var children = list.Where(t => t.ParentId == parentId);
            foreach (var chl in children)
            {
                TreeModel model = new TreeModel();
                model.Id = chl.NavigationId;
                model.Name = chl.NavigationName;
                model.Sort = chl.Sort;

                var nodes = NavigationTree(list, model);
                model.Children = nodes.Count() > 0 ? nodes : null;
                nodeList.Add(model);
            }
            return nodeList;
        }
    }

}
