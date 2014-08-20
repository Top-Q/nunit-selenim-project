using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mysite.Webdriver_infra.Infra;

namespace my_site_tests.Infra.PageObjects
{
    public class GoogleSearchPage : AbstractPage
    {
        public GoogleSearchPage(IWebDriver driver) : base(driver)
        {
        }

        protected override void AssertInPage()
        {
        }

        public void SendKeysToSearchTb(string text)
        {
            report.Report("About to send keys "+text+" for search text box");
            IWebElement searchTb = driver.FindElement(By.Name("q"));
            searchTb.Clear();
            searchTb.SendKeys(text);
        }



        public void ClickOnSearchBtn()
        {
            report.Report("About to click on seach button");
            IWebElement searchBtn = driver.FindElement(By.Name("btnK"));
            searchBtn.Click();
        }
    }
}
