using integ_framework_infra.Infra.Report;
using integ_framework_infra.Infra.Report.Reporters;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webdriver_infra.Infra
{
    public class EventReportingDriver : EventFiringWebDriver
    {
        protected static IReportDispatcher report;

        public EventReportingDriver(IWebDriver parentDriver) : base(parentDriver)
        {
            report = ReportManager.Instance;
            this.ExceptionThrown += ExceptionThrownHandler;
            this.ElementClicked += ElementClickedHandler;
            this.ElementClicking += ElementClickingHandler;
            this.FindElementCompleted += FindElementCompletedHandler;
        }

        private void FindElementCompletedHandler(object sender, FindElementEventArgs e)
        {
            if (e.Element != null)
            {
                report.Report("Found element with tag: " + e.Element.TagName);
            }            
        }

        private void ElementClickingHandler(object sender, WebElementEventArgs e)
        {
        }

        private void ElementClickedHandler(object sender, WebElementEventArgs e)
        {
        }

        private void ExceptionThrownHandler(object sender, WebDriverExceptionEventArgs e)
        {
            report.Report("Exception was thrown by WebDriver", e.ThrownException.ToString());
            WebDriverUtils.AddScreenshot(this,e.ThrownException.Message);
        }

   



    }
}
