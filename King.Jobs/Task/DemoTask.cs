using log4net;
using System;

namespace King.Jobs
{
    public class DemoTask
    {
        private readonly ILog log = LogManager.GetLogger("King", typeof(DemoTask));

        public void Run()
        {           
            log.Info("执行任务--DemoTask");           
        }

    }
}
