using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace King.Wecat.Enum
{
    public enum Weixin_Event
    {
        [Description("取消关注")]
        UnSubscribe = 0,

        [Description("关注")]
        Subscribe = 1,

        [Description("查看")]
        View = 2,

        [Description("点击")]
        Click = 3,
    }
}
