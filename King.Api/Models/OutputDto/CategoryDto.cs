using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.Api.Models
{
    public class CategoryDto
    {
        /// <summary>
        /// ID
        /// </summary>       
        public int Id { get; set; }
        /// <summary>
        /// ID
        /// </summary>       
        public int Pid { get; set; }
        /// <summary>
        /// 名称
        /// </summary>       
        public string CategoryName { get; set; }
        /// <summary>
        /// 排序越大越靠后
        /// </summary>
        public int Sort { get; set; }       
    }
}
