using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Script.Serialization;

namespace integ_framework_infra.Infra.Report.Reporters.HtmlTestReporter.Model.Execution
{
    public abstract class Node
    {
        private string statusValue;
        
        public string name { get; set; }
        
        public string status { get {return statusValue;}
            
            set 
            {
                if (statusValue == null){
                    statusValue = value;
                    if (parent != null)
                    {
                        parent.status = value;
                    }
                    return;
                }
                Status currentStatus = (Status)Enum.Parse(typeof(Status), statusValue);
                Status newStatus = (Status)Enum.Parse(typeof(Status), value);
                if (currentStatus < newStatus)
                {
                    statusValue = value;
                    if (parent != null)
                    {
                        parent.status = value;
                    }
                    

                }
            } 
        
        }
        public string type { get; set; }

        [ScriptIgnoreAttribute]
        public NodeWithChildren parent { get; set; }


        public Node()
        {
        }

        public Node(string name)
        {
            this.name = name;
        }


    }
}
