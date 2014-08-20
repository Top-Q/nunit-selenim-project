using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using integ_framework_infra.Config;
using integ_framework_infra.Infra.Report.Reporters;
using integ_framework_infra.Infra.Report;


namespace mysite.Webdriver_infra
{
    public class Web
    {
        protected IReportDispatcher report = ReportManager.Instance;
        protected IWebDriver driver;
        protected string url;
        private Webdriver_infra.WebdriverFactory.DriverType driverType;


        public Web()
        {
            FirefoxProfileManager allProfiles = new FirefoxProfileManager();
            //FirefoxProfile profile = allProfiles.GetProfile(
            //TODO: Read the browser type from an external configuration file.
            string driverTypeProp = Sut.Instance.GetProperty("webdriver", "browser");
            if (driverTypeProp.ToLower().Equals("chrome"))
            {
                driver = WebdriverFactory.CreateDriver(Webdriver_infra.WebdriverFactory.DriverType.Chrome, null);
                driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
            }
            else if (driverTypeProp.ToLower().Equals("iexplore") || driverTypeProp.ToLower().Equals("explorer") || driverTypeProp.ToLower().Equals("explore") || driverTypeProp.ToLower().Equals("internetexplorer"))
            {
                driver = WebdriverFactory.CreateDriver(Webdriver_infra.WebdriverFactory.DriverType.InternetExplorer, null);
            }
            else if (driverTypeProp.ToLower().Equals("firefox"))
            {
                driver = WebdriverFactory.CreateDriver(Webdriver_infra.WebdriverFactory.DriverType.Firefox, null);
            }

            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
            driver.Navigate().GoToUrl(Url);
        }

        public virtual string Url
        {
            get { return url; }
            set { url = value; }
        }

        public virtual mysite.Webdriver_infra.WebdriverFactory.DriverType DriverType
        {
            get { return driverType; }
            set { driverType = value; }
        }

        public void Close()
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }

        public void CloseBrowserAndSaveCookies()
        {
            var cookies = driver.Manage().Cookies.AllCookies;
            if (driver != null)
            {
                driver.Quit();
            }
            driver = WebdriverFactory.CreateDriver(Webdriver_infra.WebdriverFactory.DriverType.InternetExplorer, null);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            driver.Navigate().GoToUrl(Url);
            foreach (Cookie cookie in cookies)
            {
                driver.Manage().Cookies.AddCookie(cookie);
            }
            driver.Navigate().GoToUrl(Url);
        }
    }
}
