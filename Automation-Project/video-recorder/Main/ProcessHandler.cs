using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace video_recorder
{
    class ProcessHandler
    {
        private string fileName;
        private string arguments;

        private Process runningProcess;

        private StringBuilder runningProcessStdout;
        private StringBuilder runningProcessStderror;
        private StreamWriter runningProcesssWriter;
        
        public ProcessHandler(string fileName, string arguments)
        {
            this.fileName = fileName;
            this.arguments = arguments;
        }


        /// <summary>
        /// Start the process with the arguments specified in the constructor
        /// </summary>        
        internal ProcessOutput StartInBlockingMode()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = fileName;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = arguments;

            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = false;
            startInfo.RedirectStandardOutput = true;

            ProcessOutput output = null;
            // Start the process with the info we specified.
            // Call WaitForExit and then the using-statement will close.
            using (Process exeProcess = Process.Start(startInfo))
            {                
                exeProcess.WaitForExit();
                output = new ProcessOutput(exeProcess.StandardOutput.ReadToEnd(), exeProcess.StandardError.ReadToEnd(), exeProcess.ExitCode);
            }
            
            return output;
        }

        internal void StartInNonBlockingMode()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = fileName;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = arguments;

            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;

            runningProcess = Process.Start(startInfo);
            
            runningProcessStdout = new StringBuilder();            
            runningProcess.OutputDataReceived += new DataReceivedEventHandler(proc_OutputDataReceived);
            runningProcess.BeginOutputReadLine();

            runningProcessStderror = new StringBuilder();
            runningProcess.ErrorDataReceived += new DataReceivedEventHandler(proc_ErrorDataReceived);
            runningProcess.BeginErrorReadLine();

            runningProcesssWriter = runningProcess.StandardInput;

        }

        private void proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            runningProcessStdout.Append(e.Data);
        }

        private void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            runningProcessStderror.Append(e.Data);
        }

        internal bool IsProcessAlive()
        {
            if (null == runningProcess)
            {
                return false;
            }
            return runningProcess.HasExited;
        }

        internal void SendToProcess(string input)
        {
            if (null == runningProcess || null == runningProcesssWriter)
            {
                throw new InvalidOperationException("No process was started");
            }
            runningProcesssWriter.WriteLine(input);


        }


        internal ProcessOutput WaitForProcessToEnd()
        {
            if (null == runningProcess)
            {
                throw new InvalidOperationException("No process was started");
            }
            if (!runningProcess.HasExited)
            {                
                runningProcess.WaitForExit();
            }

            ProcessOutput output = new ProcessOutput(runningProcessStdout.ToString(), runningProcessStderror.ToString(), runningProcess.ExitCode);         
            return output;
        }

        internal class ProcessOutput
        {
            internal ProcessOutput(string stdout, string stderror, int errorCode)
            {
                this.stdout = stdout;
                this.stderror = stderror;
                this.errorCode = errorCode;
            }
            
            private readonly string stdout;

            internal string Stdout
            {
                get { return stdout; }
                
            }
            private readonly string stderror;

            internal string Stderror
            {
                get { return stderror; }
                
            }
            private readonly int errorCode;

            internal int ErrorCode
            {
                get { return errorCode; }                
            }


        }

    }
}
