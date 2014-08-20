using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using Webdriver_infra.Infra;
//using NUnit;

namespace mysite.Webdriver_infra
{
    public class WebdriverFactory
    {
        public enum DriverType
        {
            InternetExplorer, Firefox, Chrome
        }

        public static IWebDriver CreateDriver(DriverType driverType, Dictionary<string,object> config)
        {
            
            ChromeOptions chromeOptoions = new ChromeOptions();
            chromeOptoions.AddArgument("--allow-running-insecure-content");
            //TODO: Create instansiation for all webdriver drivers.
            IWebDriver driver = null;
            switch (driverType)
            {
                case DriverType.Firefox:
                    driver =  new FirefoxDriver();
                    break;
                case DriverType.Chrome:
                    driver = new ChromeDriver(chromeOptoions);
                    break;
                case DriverType.InternetExplorer:
                    driver =  new InternetExplorerDriver();
                    break;
                default:
                    driver = new FirefoxDriver();
                    break;
            }
            return new EventReportingDriver(driver);

        }
    }
}
