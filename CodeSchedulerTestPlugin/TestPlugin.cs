using CodeScheduler.Plugins;
using CodeScheduler.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSchedulerTestPlugin
{
    public class TestPlugin : IPlugin
    {
        public void Dispose()
        {
            Logger.Log(LogSeverity.Info, "Test Plugin", "Disposed, indeed");
        }

        public void Initialize()
        {
            Logger.Log(LogSeverity.Info, "Test Plugin", "Initialized, indeed");
        }

        public void OnEvent(EventType type, EventData data)
        {
            
        }
    }
}
