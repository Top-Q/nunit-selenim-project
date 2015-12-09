

using integ_framework_infra.Infra.Report.Reporters.HtmlTestReporter.Model.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Script.Serialization;

using integ_framework_infra.Infra.Report.Reporters.HtmlTestReporter;
using System.Diagnostics;
using integ_framework_infra.Infra.Report.Reporters.HtmlTestReporter.Model;
using integ_framework_infra.Infra.Utils;

using System.IO;
using integ_framework_infra.Infra.Report.Reporters.HtmlTestReporter.Model.Test;
using System.IO.Compression;


namespace integ_framework_infra.Infra.Report.Reporters.HtmlTestReproters
{
    public class HtmlTestReporter : IReporter
    {
        private const string templateFolder = @"integ-framework-infra\Infra\Report\Reporters\HtmlTestReporter\Resources";
        private const string htmlArchiveFile = @"Resources\difido-reports-common.jar";
        private const string executionModelFileName = "execution.js";
        private const string testModelFileName = "test.js";
        private const string testHtmlFileName = "test.html";
        private Execution execution;
        private Test currentTest;
        private int index;
        private TestDetails testDetails;
        private string testFolder;
        private Machine machine;


        private string outputFolder;

        public HtmlTestReporter()
        {
        }

        private void ExecutionToFile()
        {
            string json = "var execution=" + new JavaScriptSerializer().Serialize(execution) + ";";
            System.IO.StreamWriter file = new System.IO.StreamWriter(outputFolder + @"\" + executionModelFileName);
            file.WriteLine(json);
            file.Close();

        }

        private void CreateTestFolderIfNotExists()
        {
            testFolder = outputFolder + @"\tests\test_" + index;
            if (!Directory.Exists(testFolder))
            {
                Directory.CreateDirectory(testFolder);
                System.IO.File.Copy(outputFolder + @"\" + testHtmlFileName, testFolder + @"\" + testHtmlFileName, true);
            }

        }

        private void TestToFile()
        {
            CreateTestFolderIfNotExists();
            string json = "var test=" + new JavaScriptSerializer().Serialize(testDetails) + ";";
            System.IO.StreamWriter file = new System.IO.StreamWriter(testFolder + @"\" + testModelFileName);
            file.WriteLine(json);
            file.Close();

        }


        public virtual void Init(string outputFolder)
        {
            this.outputFolder = outputFolder + @"\current";
            CreateReportsFolder();
            InitHtmls();
            machine = new Machine(System.Environment.MachineName);
            execution = new Execution();
            execution.AddMachine(machine);


        }

        public virtual void StartTest(ReporterTestInfo testInfo)
        {

            currentTest = new Test(testInfo.TestName);
            currentTest.timestamp = DateTime.Now.ToString("HH:mm:ss");
            currentTest.index = index;
            string scenarioName = testInfo.FullyQualifiedTestClassName.Split('.')[testInfo.FullyQualifiedTestClassName.Split('.').Length - 2];
            Scenario scenario;
            if (machine.IsChildWithNameExists(scenarioName))
            {
                scenario = (Scenario)machine.GetChildWithName(scenarioName);
            }
            else
            {
                scenario = new Scenario(scenarioName);
                machine.AddChild(scenario);
            }
            scenario.AddChild(currentTest);
            ExecutionToFile();
            testDetails = new TestDetails(testInfo.TestName);
            testDetails.description = testInfo.FullyQualifiedTestClassName;
            testDetails.timestamp = DateTime.Now.ToString();
        }

        public virtual void EndTest(ReporterTestInfo testInfo)
        {

            currentTest.status = testInfo.Status.ToString();
            currentTest.duration = testInfo.DurationTime;
            testDetails.duration = testInfo.DurationTime;
            ExecutionToFile();
            TestToFile();
            index++;
        }

        public void StartSuite(string suiteName)
        {
        }

        public void EndSuite(string suiteName)
        {
        }



        public void Report(string title, string message, ReporterTestInfo.TestStatus status, ReportElementType type)
        {
            ReportElement element = new ReportElement();
            if (null == testDetails)
            {
                Console.WriteLine("HTML reporter was not initiliazed propertly. No reports would be created.");
                return;
            }
            testDetails.AddReportElement(element);
            element.title = title;
            element.message = message;
            element.time = DateTime.Now.ToString("HH:mm:ss");
            element.status = status.ToString();
            element.type = type.ToString();

            if (type == ReportElementType.lnk || type == ReportElementType.img)
            {
                if (File.Exists(message))
                {
                    //This is a link to a file. Let's copy it to the report folder
                    CreateTestFolderIfNotExists();
                    try
                    {
                        string fileName = Path.GetFileName(message);
                        string fileDestination = testFolder + @"\" + fileName;
                        System.IO.File.Copy(message, fileDestination, true);
                        //We need that the link would be to the file in the report folder
                        element.message = fileName;
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine("Failed adding file to the report due to " + e.Message);
                    }


                }
            }

            TestToFile();
        }

        private void CreateReportsFolder()
        {
            try
            {
                System.IO.Directory.CreateDirectory(outputFolder);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to create reports output folder", e);
            }
        }

        private string GetRootFolder()
        {
            return Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        }

        private void InitHtmls()
        {
            string reportTemplatesFolder = GetRootFolder() + @"\" + templateFolder;
            try
            {
                //Deleting old reports
                Directory.Delete(outputFolder, true);

                //Re creating the reports output folder
                Directory.CreateDirectory(outputFolder);

                //Creating temp folder
                string tempFolder = FileUtils.CreateTempFolder();
                try
                {
                    //Extracting the JAR file with the HTML reports to the temp folder
                    ZipFile.ExtractToDirectory(htmlArchiveFile, tempFolder);

                    //Copying only the HTML files to the reports output folder
                    FileUtils.DirectoryCopy(tempFolder + @"\il.co.topq.difido.view", outputFolder, true);
                }
                finally
                {
                    //Deleting the temporary folder
                    Directory.Delete(tempFolder, true);
                }

            }
            catch (IOException e)
            {
                //Failed to create or copy files.
                string str = e.ToString();
            }
            //TODO: Extract the file name automatically
            //System.IO.File.WriteAllText(outputFolder + @"\reportng.css", AssemblyUtils.GetResource("index.html"));

        }





    }
}
