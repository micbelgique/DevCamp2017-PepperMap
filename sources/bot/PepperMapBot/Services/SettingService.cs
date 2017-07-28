using System;
using System.Configuration;
using PepperMap.Infrastructure.Interfaces;

namespace PepperMapBot.Services
{
    [Serializable]
    public class SettingService : ISettingService
    {
        public string GetSetting(string key)
        {
            return ConfigurationManager.AppSettings[key] ?? string.Empty;
        }
    }
}