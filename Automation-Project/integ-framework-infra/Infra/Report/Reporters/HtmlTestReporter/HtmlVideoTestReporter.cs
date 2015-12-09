using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topq.Auto.VideoRecorder;

namespace integ_framework_infra.Infra.Report.Reporters.HtmlTestReporter
{
    /// <summary>
    /// FFMPEG Installation
    /// The video reporter is using FFMPEG for capturing and editing the videos. From this reason, FFMPEG should be installed on every machine that we want to use the video recorder on.
    /// Start by downloading the FFMEG installer for your operating system from the FFMPEG site. We are using the static 32 bit version, so, if you are using Windows, you can download it directly from here. The name of the file should be something like ffmpeg-20151208-git-ff6dd58-win32-static.7z
    /// Extract the content of the archived file to C:\Program Files (x86)\ffmpeg
    /// If you are using Windows, you will also need the third party application screen capture recorder to video windows free. Download the latest version from here and install it using the default settings.
    /// </summary>
    public class HtmlVideoTestReporter : HtmlTestReproters.HtmlTestReporter
    {
        private VideoRecorder recorder;

        private bool enabled = true;

        public override void Init(string outputFolder)
        {
            base.Init(outputFolder);
            if (enabled)
            {
                try
                {
                    recorder = new VideoRecorder();
                }
                catch
                {
                    enabled = false;
                }

            }


        }

        public override void StartTest(ReporterTestInfo testInfo)
        {
            base.StartTest(testInfo);
            if (!enabled)
            {
                return;
            }
            try
            {
                recorder.StartRecording();
            }
            catch
            {
                enabled = false;
            }

        }

        public override void EndTest(ReporterTestInfo testInfo)
        {
            if (enabled)
            {
                try
                {
                    if (testInfo.Status != ReporterTestInfo.TestStatus.success)
                    {
                        string videoFileName = recorder.SaveRecording();
                        base.Report("Test Flow Video", videoFileName, ReporterTestInfo.TestStatus.failure, ReportElementType.lnk);
                        File.Delete(videoFileName);
                    }
                    else
                    {
                        recorder.DumpRecording();
                    }

                }
                catch
                {
                    enabled = false;
                }

            }

            base.EndTest(testInfo);


        }
    }
}
