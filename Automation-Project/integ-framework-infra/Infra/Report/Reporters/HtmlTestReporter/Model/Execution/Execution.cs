using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace integ_framework_infra.Infra.Report.Reporters.HtmlTestReporter.Model.Execution
{
    public class Execution
    {
        public List<Machine> machines { get; set; }

        public void AddMachine(Machine machine)
        {
            if (machines == null)
            {
                machines = new List<Machine>();
            }
            machines.Add(machine);
        }
    }
}
