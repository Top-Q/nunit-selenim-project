using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace integ_framework_infra.Infra.Report.Reporters.HtmlTestReporter.Model.Execution
{
    public class Scenario : NodeWithChildren
    {
        public Scenario()
        {
            type = "scenario";
            status = "success";
        }

        public Scenario(string name)
            : base(name)
        {
            type = "scenario";
            status = "success";
        }

        
    }
}
