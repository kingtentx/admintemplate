using System;
using System.Collections.Generic;
using System.Text;

namespace King.Wecat
{
    public class EventType
    {
        /// <summary>
        /// 取消订阅
        /// </summary>
        public const string UnSubscribe = "unsubscribe";
        /// <summary>
        /// 订阅
        /// </summary>
        public const string Subscribe = "subscribe";
        /// <summary>
        /// 扫码
        /// </summary>
        public const string SCAN = "SCAN";
        /// <summary>
        /// 上报地理位置事件
        /// </summary>
        public const string LOCATION = "LOCATION";
        /// <summary>
        /// 点击菜单拉取消息时的事件
        /// </summary>
        public const string CLICK = "CLICK";
        /// <summary>
        /// 点击菜单跳转链接时的事件
        /// </summary>
        public const string VIEW = "VIEW";
    }
}
