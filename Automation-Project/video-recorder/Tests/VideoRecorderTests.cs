using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topq.Auto.VideoRecorder;

namespace video_recorder.Tests
{
    [TestFixture]
    class VideoRecorderTests
    {
        private VideoRecorder recorder;
        
        [SetUp]
        public void Setup()
        {
            recorder = new VideoRecorder();
        }

        [Test]
        public void TestSaveCompressedVideo()
        {
            recorder.StartRecording();
            Thread.Sleep(1000 * 10);
            string fileName = recorder.SaveRecording();
            Console.WriteLine(fileName);
        }

        [Test]
        public void TestDumpCompressedVideo()
        {
            recorder.StartRecording();
            Thread.Sleep(1000 * 10);
            recorder.DumpRecording();
        }
    }
}
