using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.Api.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>       
        public string Telphone { get; set; }
        
        /// <summary>
        /// 用户的唯一标识
        /// </summary>       
        public string Openid { get; set; }

        /// <summary>
        ///  用户昵称
        /// </summary>       
        public string Nickname { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>       
        public int Sex { get; set; }
      
        /// <summary>
        ///  用户个人资料填写的省份
        /// </summary>       
        public string Province { get; set; }

        /// <summary>
        /// 普通用户个人资料填写的城市
        /// </summary>      
        public string City { get; set; }

        /// <summary>
        /// 国家，如中国
        /// </summary>       
        public string Country { get; set; }

        /// <summary>
        /// 用户头像，   
        /// </summary>      
        public string Headimgurl { get; set; }

        /// <summary>
        /// 用户是否订阅该公众号标识
        /// 值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
        /// </summary>       
        public int Subscribe { get; set; }

        /// <summary>
        /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        /// </summary>     
        public long SubscribeTime { get; set; }

        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。
        /// </summary>        
        public string Unionid { get; set; }      

        /// <summary>
        /// 用户被打上的标签ID列表
        /// </summary>       
        public virtual string TagidList { get; set; }      

        
    }
}
