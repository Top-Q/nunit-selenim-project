using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Script.Serialization;

namespace integ_framework_infra.Infra.Report.Reporters.HtmlTestReporter.Model.Test
{
    public class TestDetails
    {
        public string name { get; set; }
        public string description { get; set; }
        public string timestamp { get; set; }
        public long duration { get; set; }
        public Dictionary<string, string> parameters;
        public Dictionary<string, string> properties;
        public List<ReportElement> reportElements;


        public TestDetails()
        {
        }

        public TestDetails(string name)
        {
            this.name = name;
        }

        
        public void AddReportElement(ReportElement reportElement)
        {
            if (null == reportElements)
            {
                reportElements = new List<ReportElement>();
            }
            reportElements.Add(reportElement);

        }

        
        public void AddProperty(string key,string value)
        {
            if (null == properties)
            {
                properties = new Dictionary<string,string>();
            }
            properties.Add(key, value);
        }

        
        public void AddParameter(string key, string value)
        {
            if (null == parameters)
            {
                parameters = new Dictionary<string, string>();
            }
            parameters.Add(key, value);
        }


    }
}
