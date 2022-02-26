using King.Api.Models;
using King.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace King.Api
{
    public class Utils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Claims"></param>
        /// <returns></returns>
        public static LoginUser LoginUser(ClaimsPrincipal Claims)
        {
            var identity = (ClaimsIdentity)Claims.Identity;

            LoginUser user = new LoginUser()
            {
                UserId = Convert.ToInt32(identity.FindFirst(ClaimTypes.Sid)?.Value),
                UserName = identity.FindFirst(ClaimTypes.Name)?.Value
            };

            return user;
        }

        /// <summary>
        /// 分类递归
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tree"></param>
        /// <returns></returns>
        public static List<TreeModel> CreateChildTree(List<Category> list, TreeModel tree)
        {
            int parentId = tree.Id;//根节点ID

            List<TreeModel> nodeList = new List<TreeModel>();

            var children = list.Where(t => t.ParentId == parentId);
            foreach (var chl in children)
            {
                TreeModel model = new TreeModel();
                model.Id = chl.CategoryId;
                model.Pid = chl.ParentId;
                model.Name = chl.CategoryName;
                model.Sort = chl.Sort;

                var nodes = CreateChildTree(list, model);
                model.Children = nodes.Count() > 0 ? nodes : null;
                nodeList.Add(model);
            }
            return nodeList;
        }


        /// <summary>
        /// 身份证验证
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static bool IsIdCard(string idCard)
        {
            switch (idCard.Length)
            {
                case 15:
                    return Regex.IsMatch(idCard, @"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$");
                case 18:
                    return Regex.IsMatch(idCard, @"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z])$", RegexOptions.IgnoreCase);
                default:
                    return false;
            }
        }
    }
}
