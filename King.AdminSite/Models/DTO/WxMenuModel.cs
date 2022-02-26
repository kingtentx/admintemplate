﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.AdminSite.Models
{
    public class WxMenuModel
    {
        public int id { get; set; }
        public int pid { get; set; }

        public string name { get; set; }

        public string type { get; set; }

        public string value { get; set; }

        public int sort { get; set; }       

        public int level { get; set; }

        public IList<WxMenuModel> sub_button { get; set; }
    }
}
