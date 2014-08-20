using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using integ_framework_infra.Infra.Report.Reporters;
using integ_framework_infra.Infra.Report;
using Webdriver_infra.Infra;
using System.Threading;

namespace mysite.Webdriver_infra.Infra
{
    public abstract class AbstractPage
    {

        protected IWebDriver driver;
        protected WebDriverWait wait;
        protected IReportDispatcher report = ReportManager.Instance;

        public AbstractPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            PageFactory.InitElements(driver, (this));
            AssertInPage();
            AddScreenshot(this.GetType().Name);            
        }

        public void AddScreenshot(string description)
        {
            WebDriverUtils.AddScreenshot(driver, description);
        }

        protected abstract void AssertInPage();

        protected void SendKeysAsChars(IWebElement element, string text)
        {
            foreach (char c in text)
            {
                element.SendKeys(c.ToString());
                Thread.Sleep(5);
            }
        }
    }
}
