using DynDns.Settings;
using System;
using System.IO;
using System.Text.Json;

namespace DynDns.Infrastruct
{
    public class SettingsTools
    {
        private const string defaultAppSettingsFile = "appsettings.json";
        private const string devAppSettingsFile = "appSettings.dev.json";

        public static AppSettings LoadSettings()
        {
            
            string jsonText = string.Empty;
            string jsonTextFileName = string.Empty;

            if (File.Exists(devAppSettingsFile))
            {
                jsonTextFileName = devAppSettingsFile;
            }
            else if (File.Exists(defaultAppSettingsFile))
            {
                jsonTextFileName = defaultAppSettingsFile;
            }
            else
            {
                throw new Exception("Application settings file not found.");
            }

            jsonText = File.ReadAllText(jsonTextFileName);
            AppSettings? appSettings = JsonSerializer.Deserialize<AppSettings>(jsonText);

            if(appSettings == null)
                throw new Exception($"Error deserializing AppSettings file '{jsonTextFileName}'.");

            return appSettings;

        }

    }

}

