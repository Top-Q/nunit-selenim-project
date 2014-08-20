using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace integ_framework_infra.Config
{
    public class Sut
    {
        private static volatile Sut instance;
        private static object syncRoot = new Object();
        private IniHandler iniHandler;

        private Sut()
        {
            string sutFile = Path.Combine(Configuration.Instance.GetRootFolder(), Configuration.Instance.GetProperty(FrameworkOptions.sut));
            iniHandler = new IniHandler(sutFile);

        }

        public string GetProperty(string section, string property)
        {
            return iniHandler.GetProperty(section,property);
        }



        public static Sut Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Sut();
                    }
                }

                return instance;
            }
        }


    }
}


