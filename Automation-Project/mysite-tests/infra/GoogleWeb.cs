using OpenQA.Selenium;
using integ_framework_infra.Config;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mysite.Webdriver_infra;
using my_site_tests.Infra.PageObjects;

namespace mysite.automation.PageObjects
{
    public class GoogleWeb : Web 
    {

        public GoogleSearchPage GetSearchPage()
        {
            report.Report("About to get Google Search Page");
            return new GoogleSearchPage(driver);
        }


        public override string Url 
        {
            get { return Sut.Instance.GetProperty("domains", "domain.google"); }
            set { url = value; }
        }

        public IWebDriver Driver
        {
            get { return driver; }
        }


    }
}
