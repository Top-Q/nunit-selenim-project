using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace integ_framework_infra.Infra.Report.Reporters.ExcelReport
{
    public class ExcelReporter : IReporter
    {
        private string templateFolder =  @"integ-framework-infra\Infra\Report\Reporters\ExcelReport\Resources\test_results.xlsx";
        private const string reportFileName = "test_result.xlsx";

        private string outputFolder;
        private string outputFolder2;
        private int currRow = 1;
        private string failureReason;

        bool firstEntry = true;

        public void Init(string outputFolder)
        {
            this.outputFolder = outputFolder;
            this.outputFolder2 = outputFolder;

        }

        public void StartTest(ReporterTestInfo testInfo)
        {
            if (firstEntry)
            {
                try
                {
                    this.outputFolder = outputFolder + @"\ExcelReports\ExcelReport " + DateTime.Now.ToString(" MM-dd-yy.hh-mm-ss") + ".xlsx";
                    File.Copy(GetRootFolder() + @"/" + templateFolder, outputFolder);
                }
                catch
                {
                    string folderName = outputFolder2 + @"\ExcelReports";
                    System.IO.Directory.CreateDirectory(folderName);
                    this.outputFolder = outputFolder2 + @"\ExcelReports\ExcelReport" + DateTime.Now.ToString(" MM-dd-yy.hh-mm-ss") + ".xlsx";
                    File.Copy(GetRootFolder() + @"/" + templateFolder, outputFolder);
                }

                firstEntry = false;
            }
        }

        public void EndTest(ReporterTestInfo testInfo)
        {
            //Add one row to the excel report with the test info.
            if (testInfo.Status != ReporterTestInfo.TestStatus.success)
            {
                  AppendTestExcel(testInfo);    
                 currRow++;
            }     
        }


        private void AppendTestExcel(ReporterTestInfo testInfo)
        {
            // Open Template
            FileStream fsr = new FileStream(outputFolder, FileMode.Open, FileAccess.Read);

            // Load the template into a NPOI workbook
            XSSFWorkbook templateWorkbook = new XSSFWorkbook(fsr);

            // Load the sheet you are going to use as a template into NPOI
            ISheet sheet = templateWorkbook.GetSheet("Sheet1");
           // XSSFRow row;
            // Insert data into template
            var row = sheet.CreateRow(currRow);
            var cellIn = row.CreateCell(0);
            cellIn.SetCellValue(testInfo.FullyQualifiedTestClassName);
            cellIn = row.CreateCell(1);
            cellIn.SetCellValue(testInfo.TestName);
            cellIn = row.CreateCell(2);
            cellIn.SetCellValue(testInfo.Status.ToString());
            cellIn = row.CreateCell(3);
            cellIn.SetCellValue(testInfo.DurationTime);
            cellIn = row.CreateCell(4);
            cellIn.SetCellValue(failureReason);
            sheet.ForceFormulaRecalculation = true;

            // Save the NPOI workbook into a memory stream to be sent to the browser, could have saved to disk.
            fsr.Close();

            templateWorkbook.Write(new FileStream(outputFolder, FileMode.Create, FileAccess.ReadWrite));
        }

        public void StartSuite(string suiteName)
        {
            
        }

        public void EndSuite(string suiteName)
        {
            
        }

        public void Report(string title, string message, ReporterTestInfo.TestStatus status, ReportElementType type)
        {
            failureReason = title;
        }

        private string GetRootFolder()
        {
            return Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        }
    }
}
