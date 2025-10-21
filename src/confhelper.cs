using System;
using System.IO;
using System.Reflection;
using System.Web.Script.Serialization;

namespace LxpGUI {
    class ConfigHelper {
        static readonly JavaScriptSerializer serializer = new JavaScriptSerializer();
        public static Config Config;
        static readonly string configFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.json");

        public static void ReadConfig(){
            if(!File.Exists(configFilePath)){
                Config = new Config();
                WriteConfig();
            }

            Config = serializer.Deserialize<Config>(File.ReadAllText(configFilePath));
        }

        public static void WriteConfig(){
            File.WriteAllText(configFilePath, serializer.Serialize(Config));
        }
    }

    class Config {
        public string email = string.Empty;
        public string password = string.Empty;
    }
}