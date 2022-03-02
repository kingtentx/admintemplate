using log4net;
using System;

namespace King.Jobs
{
    public class TestTask
    {
        private readonly ILog log = LogManager.GetLogger("King", typeof(TestTask));

        public void Run()
        {          
            log.Info("执行任务--TestTask");           
        }

    }
}
