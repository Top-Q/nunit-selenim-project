using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace integ_framework_infra.Infra.Report.Reporters.HtmlTestReporter.Model.Execution
{
    public class Test : Node
    {
        public int index { get; set; }
        public long duration { get; set; }
        public string timestamp { get; set; }

        public Test()
        {
            type = "test";
            status = "success";
        }

        public Test(string name) : base(name)
        {
            type = "test";
            status = "success";
            
        }


    }
}
