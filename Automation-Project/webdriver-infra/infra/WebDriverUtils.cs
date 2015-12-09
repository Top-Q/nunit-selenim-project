using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using integ_framework_infra.Infra.Report;
using System.IO;

namespace Webdriver_infra.Infra
{
    public static class WebDriverUtils
    {
        public static void AddScreenshot(IWebDriver driver)
        {
            AddScreenshot(driver, "Screenshot");
        }

        public static void AddScreenshot(IWebDriver driver,string description)
        {
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();

            //Use it as you want now
            string screenshot = ss.AsBase64EncodedString;
            byte[] screenshotAsByteArray = ss.AsByteArray;
            string screenshotFileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".png";
            ss.SaveAsFile(screenshotFileName, ImageFormat.Png); //use any of the built in image formating
            ReportManager.Instance.ReportImage(description, screenshotFileName);
            File.Delete(screenshotFileName);

        }

    }
}
