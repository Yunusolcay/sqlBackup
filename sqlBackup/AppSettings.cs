using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace sqlBackup
{
    public static class AppSettings
    {
        private static JToken _appSettingsJSON;
        public static RootConfig config;

        public static void InitializeAppSettings()
        {
            var JSON = System.IO.File.ReadAllText("appsettings.json");
            _appSettingsJSON = JObject.Parse(JSON);

            config = Newtonsoft.Json.JsonConvert.DeserializeObject<RootConfig>(JSON);
        }

        //public static T GetConfiguration<T>(string ConfigurationKey)
        //{
        //    string value = (string)_appSettingsJSON.SelectToken(string.Format("Configuration.{0}", ConfigurationKey));
        //    return (T)Convert.ChangeType(value, typeof(T));
        //}

        //public static object GetDatabaseLists(string ConfigurationKey)
        //{
        //    string value =  (string)_appSettingsJSON.SelectToken(string.Format("Configuration.{0}", ConfigurationKey));
        //    List<databaseList> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<databaseList>>(value);
        //    return list;
        //}

        public class DatabaseList
        {
            public string email1 { get; set; }
            public string email2 { get; set; }
            public string email3 { get; set; }
            public int partMB { get; set; }
            public string saat { get; set; }
            public string databaseName { get; set; }
        }

        public class Configuration
        {
            public string backupFolder { get; set; }
            public string backupLocation { get; set; }
            public string databaseServer { get; set; }
            public string databaseUserID { get; set; }
            public string databasePassword { get; set; }
            public List<DatabaseList> databaseList { get; set; }
        }

        public class RootConfig
        {
            public Configuration Configuration { get; set; }
        }





    }
}
