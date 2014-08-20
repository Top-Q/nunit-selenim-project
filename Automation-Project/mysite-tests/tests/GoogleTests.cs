using my_site_tests.Infra.PageObjects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_site_tests.Tests
{
    [TestFixture]
    public class GoogleTests : AbstractGoogleTestCase
    {
        [Test]
        public void TestGoogleSearchPage()
        {
            report.Step("About to enter check cheese search results");
            GoogleSearchPage googleSearchPage = web.GetSearchPage();
            googleSearchPage.SendKeysToSearchTb("Cheeseasas");
            googleSearchPage.ClickOnSearchBtn();
            

        }
    }
}
