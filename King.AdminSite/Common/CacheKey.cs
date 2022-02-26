namespace King.AdminSite
{
    /// <summary>
    /// 通用常量
    /// </summary>
    public class CacheKey
    {
        public const int Expiration_Min_5 = 5;

        public const int Expiration_Min_30 = 30;

        public const int Expiration_Min_60 = 60;

        public const int Expiration_Hour_24 = 24;

        #region 缓存

        /// <summary>
        /// 验证码
        /// </summary>
        public const string ValidateCode = "ValidateCode_";
        /// <summary>
        /// 导航缓存键
        /// </summary>
        public const string Navigation = "Navigation";
        /// <summary>
        /// 网站信息
        /// </summary>
        public const string SiteInfo = "SiteInfo";
        /// <summary>
        /// 网站底部信息
        /// </summary>
        public const string FooterInfo = "FooterInfo";
        /// <summary>
        /// 当前页面布局配置
        /// </summary>
        public const string PageLayout = "PageLayout_";

        /// <summary>
        /// 菜单缓存键
        /// </summary>
        public const string Permission_Menu = "Permission_Menu_";

        /// <summary>
        /// 微信配置
        /// </summary>
        public const string WeiXin_Config = "WeiXin_Config";
        /// <summary>
        /// 微信appid键值
        /// </summary>
        public const string WeiXin_Token = "WeiXin_Token_";
        /// <summary>
        /// 微信菜单
        /// </summary>
        public const string WeiXin_Menu = "WeiXin_Menu";
        /// <summary>
        /// 微信菜单
        /// </summary>
        public const string WeiXin_AccessToken = "WeiXin_AccessToken_";
        /// <summary>
        /// Jsapi_Ticket 
        /// </summary>
        public const string WeiXin_JsapiTicket = "WeiXin_JsapiTicket_";

        #endregion

    }
}
