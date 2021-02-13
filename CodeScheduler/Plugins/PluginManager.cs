using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using CodeScheduler.Logging;
using System.IO;

namespace CodeScheduler.Plugins
{
    public class PluginManager
    {
        public List<Plugin> Plugins = new List<Plugin>();

        public void LoadAllPlugins()
        {
            foreach(Plugin p in Plugins)
            {
                if (!p.Loaded)
                {
                    LoadPlugin(p);
                }
            }
        }

        private void LoadPlugin(Plugin p)
        {
            try
            {
                p.Assembly = Assembly.Load(Path.Combine(Program.PluginFolder, p.AssemblyName));
                foreach(Type t in p.Assembly.GetExportedTypes())
                {
                    if (t.IsAssignableFrom(typeof(IPlugin)))
                    {
                        p.PluginType = t;
                    }
                    else
                    {
                        Logger.Log(LogSeverity.Debug, $"PluginLoader [{Path.GetFileName(p.AssemblyName)}]", $"Type {t.FullName} does not implement IPlugin interface");
                    }
                }
                if (p.PluginType == null)
                {
                    Logger.Log(LogSeverity.Error, $"PluginLoader [{Path.GetFileName(p.AssemblyName)}]", $"No plugin code detected. The plugin will not get loaded.");
                    return;
                }
                p.Instance = (IPlugin)Activator.CreateInstance(p.PluginType);
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Debug, $"PluginLoader [{Path.GetFileName(p.AssemblyName)}]", $"Exception of type {ex.GetType()}: {ex.Message}");
                Logger.Log(LogSeverity.Trace, $"PluginLoader [{Path.GetFileName(p.AssemblyName)}]", ex.StackTrace);
                Logger.Log(LogSeverity.Error, $"PluginLoader [{Path.GetFileName(p.AssemblyName)}]", "Could not load plugin.");
            }
        }
    }
}
