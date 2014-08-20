using integ_framework_infra.Infra;
using integ_framework_infra.Infra.Report;
using NUnit.Core.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mysite.Infra
{
    [NUnitAddin(Name= "NUnitHook", Description = "NUnit Hook")]
    public class NUnitHook : NunitRegistrar
    {
 
    }
}
