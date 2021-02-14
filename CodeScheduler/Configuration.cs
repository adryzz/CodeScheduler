using System;
using System.IO;
using Newtonsoft.Json;
using CodeScheduler.Logging;

namespace CodeScheduler
{
    public  class Configuration
    {
        public bool Debug = false;
        public LogSeverity LogVerbosity = LogSeverity.Info;
        public bool RunEventsOnNewThread = false;

        public void Save(string fileName)
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(fileName, json);
        }

        public static Configuration FromFile(string fileName)
        {
            string json = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<Configuration>(json);
        }
    }
}