using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.AdminSite.Config
{
    public class RedisConfig
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; } = false;
        /// <summary>
        /// 连接
        /// </summary>
        public string ConnectionStrings { get; set; }
        /// <summary>
        /// 实例名
        /// </summary>
        public string InstanceName { get; set; }
        /// <summary>
        /// 默认存储库
        /// </summary>
        public int DefaultDB { get; set; } = 0;

    }
}
