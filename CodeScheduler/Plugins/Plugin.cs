using Newtonsoft.Json;
using System;
using System.Reflection;

namespace CodeScheduler.Plugins
{
    [Serializable()]
    public class Plugin
    {
        public string AssemblyName = "";
        public string AssemblyDescription = "";
        public Version AssemblyVersion = new Version();
        public string AssemblyRepositoryURL = "";
        public string AssemblyWikiURL = "";
        public string AssemblyLicense = "";
        public string ConfigName = "";
        public bool Enabled = true;

        [JsonIgnore()]
        public Assembly Assembly;
        [JsonIgnore()]
        public Type PluginType;
        [JsonIgnore()]
        public bool Loaded = false;
        [JsonIgnore()]
        public int UnhandledExceptions = 0;
        [JsonIgnore()]
        public IPlugin Instance;

        public void Save()
        {
            System.IO.File.WriteAllText(Utils.GetAbsolutePath(Program.PluginFolder, ConfigName), JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }

    public interface IPlugin : IDisposable
    {
        void Initialize();

        void OnEvent(EventType type, EventData data);
    }

    public enum EventType
    {
        Startup,
        Shutdown
    }

    /// <summary>
    /// A class that defines data for an event. Useful for reflection.
    /// </summary>
    public class EventData
    {
        public Type DataType;
        public object Data;
    }
}