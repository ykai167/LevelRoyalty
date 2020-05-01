﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Tools
{
    public static class ConfigHelper
    {
        static AppSettings appSettings = new AppSettings();
        public static AppSettings AppSettings { get { return appSettings; } }
    }

    public class AppSettings
    {
        public string this[string key]
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings[key];
            }
        }
    }
}
