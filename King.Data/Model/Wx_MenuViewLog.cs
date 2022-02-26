using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using King.Data.ExtModel;

namespace King.Data
{
    public class Wx_MenuViewLog : ExtCreateModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public long Id { get; set; }
        [StringLength(ModelUnits.Len_250)]
        public string Url { get; set; }
        [StringLength(ModelUnits.Len_50)]
        public string Openid { get; set; }

    }
}
