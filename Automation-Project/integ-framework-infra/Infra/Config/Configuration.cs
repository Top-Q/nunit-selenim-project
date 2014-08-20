using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace integ_framework_infra.Config
{
    public class Configuration
    {
        private static volatile Configuration instance;
        private static object syncRoot = new Object();
        private const string configurationFileName = "configuration.ini";
        private const string configurationIniSection = "general";

        private IniHandler iniHandler;

        private Configuration() {
            string configurationFile = Path.Combine(GetRootFolder(), configurationFileName);
            iniHandler = new IniHandler(configurationFile);
        
        }

        public string GetProperty(FrameworkOptions option)
        {
            return iniHandler.GetProperty(configurationIniSection, option.ToString());
        }

        public string GetProperty(string section,FrameworkOptions option)
        {
            return iniHandler.GetProperty(section, option.ToString());
        }


        public string GetRootFolder()
        {
            return Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        }

        public static Configuration Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Configuration();
                    }
                }

                return instance;
            }
        }


    }
}
