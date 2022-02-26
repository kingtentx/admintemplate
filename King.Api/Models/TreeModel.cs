using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.Api.Models
{
    public class TreeModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Pid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TreeModel> Children { get; set; }

    }
}
