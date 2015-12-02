using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace video_recorder
{
    internal class FfmpegWrapper
    {
        private const string CAPTURE_VIDEO_WITH_COMPRESSION = " -loglevel panic -f dshow -framerate {0} -i video=\"screen-capture-recorder\" {1}";

        private const string CAPUTRE_VIDEO_RAW = " -loglevel panic -f dshow -framerate {0} -i video=\"screen-capture-recorder\" -vcodec libx264 -crf 0 -preset ultrafast {1}";

        private const string COMPRESS_VIDEO = " -loglevel panic -i {0} -vcodec libx264 {1}";

        private const string GET_VIDEO_DETAILS = " -i {0}";

        private const string VIDEO_DETAILS_REGEX = @"Duration:\s(\d{2}:\d{2}:\d{2}\.\d{2})";

        /// <summary>
        /// UNUSED        
        /// 0 - Start duration to begin trimmed media (e.g 00:09:50.00)
        /// 1 - Source file
        /// 2 - Length of the new movie
        /// 3 - Destination file
        /// 
        /// </summary>
        private const string TRIM_COMPRESSED_MOVIE = " -loglevel panic -ss {0} -i {1} -t {2} {3}";
        // private const string TRIM_COMPRESSED_MOVIE = " -loglevel panic -ss {0} -i {1} -t {2} -c:v copy -c:a copy {3}";

        
        /// <summary>
        /// 0 - Source file
        /// 1 - Start time in seconds (e.g 30)
        /// 2 - End time in seconds (e.g 50)
        /// 3 - Destination file name
        /// </summary>
        private const string TRIM_MOVIE = " -loglevel panic -i {0} -vf trim={1}:{2} {3}";

        private readonly string ffmpegPath;

        private ProcessHandler ffmpegProcess;

        internal FfmpegWrapper(string ffmpegBinPath = @"c:/Program Files (x86)/ffmpeg/bin/")
        {
            this.ffmpegPath = ffmpegBinPath;
            if (!File.Exists(ffmpegPath + @"ffmpeg.exe"))
            {
                throw new FileNotFoundException(ffmpegPath + "ffmpeg.exe was not found. Please install it or specify the correct folder");
            }
        }

        /// <summary>
        /// Stat capturing the video.
        /// 
        /// This is a none blocking operation.
        /// </summary>
        /// <param name="frameRate">The framerate to capture the video</param>
        /// <param name="fileName">Name of the output file. should end with .mp4</param>
        internal void CaptureCompressedVideo(string fileName, float frameRate = 24)
        {
            if (null == fileName)
            {
                throw new ArgumentNullException("Arguments can't be null");
            }
            File.Delete(fileName);
            ffmpegProcess = new ProcessHandler(ffmpegPath + @"ffmpeg.exe", String.Format(CAPTURE_VIDEO_WITH_COMPRESSION, frameRate, fileName));
            ffmpegProcess.StartInNonBlockingMode();


        }

        /// <summary>
        /// Capture video without compressing. you usually would like to compress the video after saving it.
        /// </summary>
        /// <param name="frameRate"></param>
        /// <param name="fileName">Should end with .mkv</param>
        internal void CaptureRawVideo(string fileName, float frameRate = 24)
        {
            if (null == fileName)
            {
                throw new ArgumentNullException("Arguments can't be null");
            }
            File.Delete(fileName);
            ffmpegProcess = new ProcessHandler(ffmpegPath + @"ffmpeg.exe", String.Format(CAPUTRE_VIDEO_RAW, frameRate, fileName));
            ffmpegProcess.StartInNonBlockingMode();


        }

        internal void EndVideoCapture()
        {
            if (null == ffmpegProcess)
            {
                throw new InvalidOperationException("There is no video to end");
            }
            
            ffmpegProcess.SendToProcess("q");
            
            ProcessHandler.ProcessOutput output = ffmpegProcess.WaitForProcessToEnd();
            if (output.Stderror.Contains("Could not find video device with name"))
            {
                throw new InvalidProgramException("'on screen capture recorder to video free' is not installed on current machine. Please install from: http://sourceforge.net/projects/screencapturer/files/ ");
            }            
            
        }

        internal void CompressVideo(string sourceFileName, string destinationFileName)
        {
            if (!File.Exists(sourceFileName)) 
            {
                throw new FileNotFoundException("File " + sourceFileName + " is not exist");
            }
            if (String.IsNullOrEmpty(destinationFileName))
            {
                throw new InvalidOperationException("Destination can't be empty");
            }
            File.Delete(destinationFileName);
            if (File.Exists(destinationFileName))
            {
                throw new InvalidProgramException("Failed to delete destination file.");
            }
            ProcessHandler compressProcess = new ProcessHandler(ffmpegPath + @"ffmpeg.exe",String.Format(COMPRESS_VIDEO,sourceFileName, destinationFileName));
            ProcessHandler.ProcessOutput processOutput = compressProcess.StartInBlockingMode();

            if (processOutput.ErrorCode != 0)
            {
                throw new InvalidProgramException("Failed to compress movie");
            }

        }

        internal VideoDetails FetchVideoDetails(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("File " + fileName + " is not exist");
            }
            ProcessHandler compressProcess = new ProcessHandler(ffmpegPath + @"ffprobe.exe", String.Format(GET_VIDEO_DETAILS, fileName));
            ProcessHandler.ProcessOutput processOutput = compressProcess.StartInBlockingMode();
            if (processOutput.ErrorCode != 0)
            {
                throw new InvalidProgramException("Failure while trying to fetch video details: " + processOutput.Stderror);
            }
            Regex rgx = new Regex(VIDEO_DETAILS_REGEX);
            MatchCollection matches = rgx.Matches(processOutput.Stderror);
            if (matches.Count == 0)
            {
                throw new InvalidProgramException("Failed to fetch duration");
            }
            return new VideoDetails(TimeSpan.Parse(matches[0].Groups[1].Value));            
        }


        internal void TrimMovie(string sourceFileName, string destinationFileName, int timeToKeepInSeconds)
        {
            TimeSpan movieLength = FetchVideoDetails(sourceFileName).Duration;
            File.Delete(destinationFileName);
            ProcessHandler trimProcess = new ProcessHandler(ffmpegPath + @"ffmpeg.exe", String.Format(TRIM_COMPRESSED_MOVIE, movieLength - TimeSpan.FromSeconds(timeToKeepInSeconds), sourceFileName, TimeSpan.FromSeconds(timeToKeepInSeconds), destinationFileName));
            ProcessHandler.ProcessOutput processOutput = trimProcess.StartInBlockingMode();
            if (processOutput.ErrorCode != 0)
            {
                throw new InvalidProgramException("Failed to trim movie");
            }

        }


        internal class VideoDetails
        {
            private readonly TimeSpan duration;

            internal VideoDetails(TimeSpan duration)
            {
                this.duration = duration;
            }

            internal TimeSpan Duration
            {
                get { return duration; }
                
            }

            public override string ToString()
            {
                return "Duration: " + duration;
            }




        }







    }
}
