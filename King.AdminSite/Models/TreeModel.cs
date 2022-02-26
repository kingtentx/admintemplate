using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.AdminSite.Models
{
    public class TreeModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
     
        public int ParentId { get; set; }

        public int Sort { get; set; }

        public List<TreeModel> Children { get; set; }

    }
}
