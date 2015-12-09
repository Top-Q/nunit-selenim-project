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

        private int secondsToSave = 20;

        private float framePerSecond = 7.5f;

        private bool reserveCpuMode = false;

        private string videoCaptureFileName;


        public VideoRecorder(string ffmpegBinPath = @"c:/Program Files (x86)/ffmpeg/bin/")
        {
            ffmpeg = new FfmpegWrapper(ffmpegBinPath);
            SetVideoCaptureFileName();
        }

        private void SetVideoCaptureFileName()
        {
            string fileNamePrefix = Path.GetTempPath() + "tempVideoCaptureFile.";
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
            if (reserveCpuMode)
            {
                ffmpeg.CaptureRawVideo(videoCaptureFileName, FramePerSecond);
            }
            else
            {
                ffmpeg.CaptureCompressedVideo(videoCaptureFileName, FramePerSecond);
            }

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
            string tempVideoFileName = Path.GetTempPath() + "trimmedVideoCapture" + (ReserveCpuMode ? ".mkv" : ".mp4");
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