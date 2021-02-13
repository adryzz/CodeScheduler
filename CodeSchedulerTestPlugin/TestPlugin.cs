using CodeScheduler.Plugins;
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
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void OnEvent(EventType type, EventData data)
        {
            throw new NotImplementedException();
        }
    }
}
