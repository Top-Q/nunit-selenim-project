using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


namespace integ_framework_infra.Infra.Utils
{
    public static class AssemblyUtils
    {
        public static string GetResource(string resourceName)
        {
            string result = null;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
            return result;

        }
    }
}
