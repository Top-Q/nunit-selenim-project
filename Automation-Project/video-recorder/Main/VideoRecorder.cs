using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topq.Auto.VideoRecorder
{
    public class VideoRecorder
    {
        private readonly string ffmpegPath;
        
        private int secondsToSave = 10;

        private bool reserveCpuMode = true;


        public VideoRecorder(string ffmpegPath)
        {
            this.ffmpegPath = ffmpegPath;
        }

        public void startRecording()
        {
        }

        public void dumpRecording()
        {
        }

        public void saveReocrding()
        {
        }

        public int SecondsToSave
        {
            get { return secondsToSave; }
            set { secondsToSave = value; }
        }

        public string FfmpegPath
        {
            get { return ffmpegPath; }
        }

        public bool ReserveCpuMode
        {
            get { return reserveCpuMode; }
            set { reserveCpuMode = value; }
        }


    }
}