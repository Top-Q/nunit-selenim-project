using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using video_recorder;

namespace Topq.Auto.VideoRecorder
{
    public class VideoRecorder
    {

        private FfmpegWrapper ffmpeg;
        
        private int secondsToSave = 10;

        private float framePerSecond = 7.5f;

        private bool reserveCpuMode = false;

        private string videoCaptureFileName;

        private string fileUid;


        public VideoRecorder(string ffmpegBinPath = @"c:/Program Files (x86)/ffmpeg/bin/")
        {
            ffmpeg = new FfmpegWrapper(ffmpegBinPath);            
            fileUid = DateTime.Now.Ticks.ToString();
            SetVideoCaptureFileName();
        }

        private void SetVideoCaptureFileName()
        {
            string fileNamePrefix = Path.GetTempPath() + "tempVideoCaptureFile" + fileUid +".";
            if (reserveCpuMode)
            {
                videoCaptureFileName = fileNamePrefix + "mkv";
            } 
            else 
            {
                videoCaptureFileName = fileNamePrefix + "mp4";
            }


        }

        public void StartRecording()
        {
            ffmpeg.CaptureRawVideo(videoCaptureFileName, FramePerSecond);
        }

        private void StopRecording()
        {
            ffmpeg.EndVideoCapture();
        }

        public void DumpRecording()
        {
            StopRecording();
            File.Delete(videoCaptureFileName);
        }

        public string SaveRecording()
        {
            StopRecording();
            string tempVideoFileName = Path.GetTempPath() + "trimmedVideoCapture" + fileUid + (ReserveCpuMode ? ".mp4" : ".mkv");            
            ffmpeg.TrimMovie(videoCaptureFileName, tempVideoFileName, SecondsToSave);
            File.Delete(videoCaptureFileName);
            return tempVideoFileName;
        }

        public int SecondsToSave
        {
            get { return secondsToSave; }
            set { secondsToSave = value; }
        }

        public bool ReserveCpuMode
        {
            get { return reserveCpuMode; }
            set { reserveCpuMode = value; SetVideoCaptureFileName(); }
        }

        public float FramePerSecond
        {
            get { return framePerSecond; }
            set { framePerSecond = value; }
        }



    }
}