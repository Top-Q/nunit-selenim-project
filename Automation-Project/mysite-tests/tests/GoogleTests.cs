using my_site_tests.Infra.PageObjects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace my_site_tests.Tests
{
    [TestFixture]
    public class GoogleTests : AbstractGoogleTestCase
    {
        [Test]
        public void TestGoogleSearchPageSuccess()
        {
            report.Step("About to enter check cheese search results");
            Thread.Sleep(5000);
            GoogleSearchPage googleSearchPage = web.GetSearchPage();
            googleSearchPage.SendKeysToSearchTb("Cheeseasas");
            googleSearchPage.ClickOnSearchBtn();
        }

        [Test]
        public void TestGoogleSearchPageFailure()
        {
            report.Step("About to enter check cheese search results");
            Thread.Sleep(10000);
            GoogleSearchPage googleSearchPage = web.GetSearchPage();
            googleSearchPage.SendKeysToSearchTb("Cheeseasas");
            googleSearchPage.ClickOnSearchBtn();
            Assert.NotNull(null, "Failing the test by purpose");
        }
    }
}
