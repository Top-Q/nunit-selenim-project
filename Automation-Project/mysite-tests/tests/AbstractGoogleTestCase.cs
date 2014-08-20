using integ_framework_infra.Infra.Report;
using integ_framework_infra.Infra.Report.Reporters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mysite.automation.PageObjects;

namespace my_site_tests.Tests
{
    [TestFixture]
    public abstract class AbstractGoogleTestCase
    {
        protected static IReportDispatcher report = ReportManager.Instance;
        protected GoogleWeb web;

        [SetUp]
        public void setUp()
        {
            web = new GoogleWeb();
        }

        [TearDown]
        public void tearDown()
        {
            if (web != null)
            {
                web.Close();
            }
        }
    }
}