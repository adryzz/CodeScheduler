using Newtonsoft.Json;
using System;
using System.Reflection;

namespace CodeScheduler.Plugins
{
    [Serializable()]
    public class Plugin
    {
        public string AssemblyName = "";
        public Version AssemblyVersion = new Version();
        public string AssemblyRepositoryURL = "";
        public bool Enabled = true;

        [JsonIgnore()]
        public Assembly Assembly;
        [JsonIgnore()]
        public Type PluginType;
        [JsonIgnore()]
        public bool Loaded = false;

        public IPlugin Instance;
    }

    public interface IPlugin : IDisposable
    {
        void Initialize();

        void OnEvent(EventType type, EventData data);
    }

    public enum EventType
    {

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