using King.Data.ExtModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace King.Data
{
   public class Wx_Keywords : ExtCreateModel
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 规则ID
        /// </summary>
        public int KeyId { get; set; }
        /// <summary>
        /// 匹配类型  0:全匹配  1:半匹配
        /// </summary>
        public int KeyType { get; set; }
        /// <summary>
        /// 关键词
        /// </summary>    
        [StringLength(ModelUnits.Len_250)]
        public string KeyWords { get; set; }
    }
}
