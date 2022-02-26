using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using King.Utils.Enums;

namespace King.AdminSite
{
    public class MenuCode
    {
        #region 系统菜单
        /// <summary>
        /// 系统
        /// </summary>
        public const string System = "System";
        public const string System_Admin = "System_Admin";// 管理员    
        public const string System_Role = "System_Role";// 角色       
        public const string System_LoginInfo = "System_LoginInfo";// 登录日志

        #endregion

        #region  网站管理

        public const string Content = "Content";
        public const string Content_Article = "Content_Article";//文章管理
        public const string Content_Album = "Content_Album";//相册管理      
        public const string Content_Tags = "Content_Tags";//标签管理
        public const string Content_Category = "Content_Category";//分类管理
        public const string Content_Attachments = "Content_Attachments"; //文件管理    

        #endregion

        #region  微信管理
        /// <summary>
        /// 微信管理
        /// </summary>
        public const string Wecat = "Wecat";
        public const string Wecat_Config = "Wecat_Config";//微信配置
        public const string Wecat_User = "Wecat_User";// 微信用户     
        public const string Wecat_UserReply = "Wecat_UserReply";// 微信用户回复     
        public const string Wecat_Menu = "Wecat_Menu";// 微信菜单       
        public const string Wecat_AutoReply = "Wecat_AutoReply";// 微信自动回复     
        public const string Wecat_Media = "Wecat_Media";// 微信素材


        #endregion
    }
}
