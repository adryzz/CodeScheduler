using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using CodeScheduler.Logging;
using CodeScheduler.Plugins;

namespace CodeScheduler
{
    class Program
    {
        public static string PluginFolder = "Plugins";
        static PluginManager Manager = new PluginManager();
        static Configuration Config = new Configuration();
        static void Main(string[] args)
        {
            Logger.Initialize();
            Logger.Log(LogSeverity.Info, "Main Executable", "Application Started");
            Logger.Log(LogSeverity.Debug, "Main Executable", "Loading configuration...");
            if (File.Exists("config.json"))
            {
                try
                {
                    Logger.Log(LogSeverity.Debug, "Main Executable", "Found configuration file. Loading...");
                    Config = Configuration.FromFile("config.json");
                }
                catch (Exception ex)
                {
                    Logger.Log(LogSeverity.Debug, "Main Executable", $"Exception of type {ex.GetType()}: {ex.Message}");
                    Logger.Log(LogSeverity.Trace, "Main Executable", ex.StackTrace);
                    Logger.Log(LogSeverity.Fatal, "Main Executable", "Could not load configuration file.");
                    Environment.Exit(-1);
                }
            }
            else
            {
                try
                {
                    Logger.Log(LogSeverity.Debug, "Main Executable", "No configuration file found. Creating it...");
                    Config.Save("config.json");
                }
                catch (Exception ex)
                {
                    Logger.Log(LogSeverity.Debug, "Main Executable", $"Exception of type {ex.GetType()}: {ex.Message}");
                    Logger.Log(LogSeverity.Trace, "Main Executable", ex.StackTrace);
                    Logger.Log(LogSeverity.Fatal, "Main Executable", "Could not create configuration file.");
                    Environment.Exit(-1);
                }
            }

            ProcessConfiguration();

            Manager.Plugins = EnumeratePlugins();
            Manager.LoadAllPlugins();
            InitializePlugins();
            Logger.Log(LogSeverity.Debug, "Main Executable", "Registering events...");

            Application.Run();
        }

        private static void ProcessConfiguration()
        {
            if (Config.Debug)
            {
                Utils.CreateConsole();
            }
            Logger.Verbosity = Config.LogVerbosity;
        }

        private static List<Plugin> EnumeratePlugins()
        {
            List<Plugin> plugins = new List<Plugin>();

            Logger.Log(LogSeverity.Info, "Main Executable", "Loading plugins...");
            if (!Directory.Exists(Utils.GetAbsolutePath(PluginFolder)))
            {
                Logger.Log(LogSeverity.Warning, "Main Executable", "The plugin folder isn't present.");
                Logger.Log(LogSeverity.Info, "Main Executable", "Creating new plugin folder...");
                try
                {
                    Directory.CreateDirectory(Utils.GetAbsolutePath(PluginFolder));
                    Logger.Log(LogSeverity.Info, "Main Executable", "Successfully created plugin directory.");
                }
                catch(Exception ex)
                {
                    Logger.Log(LogSeverity.Debug, "Main Executable", $"Exception of type {ex.GetType()}: {ex.Message}");
                    Logger.Log(LogSeverity.Trace, "Main Executable", ex.StackTrace);
                    Logger.Log(LogSeverity.Fatal, "Main Executable", "Could not create plugin directory.");
                    Environment.Exit(-1);
                }
            }

            try
            {
                //check for .json files
                List<string> json = Directory.EnumerateFiles(Utils.GetAbsolutePath(PluginFolder), "*.json").ToList();
                Logger.Log(LogSeverity.Info, "Main Executable", $"Found {json.Count} plugin(s).");
                if (json.Count > 0)
                {
                    foreach(string s in json)
                    {
                        Plugin p = Newtonsoft.Json.JsonConvert.DeserializeObject<Plugin>(File.ReadAllText(s));
                        if (p == null)
                        {
                            Logger.Log(LogSeverity.Warning, "Main Executable", $"Plugin {Path.GetFileName(s)} does not exist.");
                        }
                        else if (!File.Exists(Utils.GetAbsolutePath(PluginFolder, p.AssemblyName)))
                        {
                            Logger.Log(LogSeverity.Warning, "Main Executable", $"Plugin {Path.GetFileName(s)} does not exist.");
                        }
                        else
                        {
                            plugins.Add(p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Debug, "Main Executable", $"Exception of type {ex.GetType()}: {ex.Message}");
                Logger.Log(LogSeverity.Trace, "Main Executable", ex.StackTrace);
                Logger.Log(LogSeverity.Fatal, "Main Executable", "Could not load plugins.");
                Environment.Exit(-1);
            }
            return plugins;
        }

        private static void InitializePlugins()
        {
            foreach(Plugin p in Manager.Plugins)
            {
                if (p.Loaded)
                {
                    Logger.Log(LogSeverity.Debug, $"PluginLoader [{Path.GetFileName(p.AssemblyName)}]", "Initializing plugin...");
                    p.Instance.Initialize();
                    Logger.Log(LogSeverity.Debug, $"PluginLoader [{Path.GetFileName(p.AssemblyName)}]", "Plugin Initialized.");
                }
            }
        }
    }
}
