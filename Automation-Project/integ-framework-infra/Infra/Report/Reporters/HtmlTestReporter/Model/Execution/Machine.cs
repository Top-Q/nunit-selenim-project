using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace integ_framework_infra.Infra.Report.Reporters.HtmlTestReporter.Model.Execution
{
    public class Machine : NodeWithChildren
    {
        public Machine()
        {
            type = "machine";
        }

        public Machine(string name) : base(name)
        {
            type = "machine";
        }
        
    }
}
