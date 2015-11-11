using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace video_recorder
{
    [TestFixture]
    class FfmpegWrapperTests
    {
        private FfmpegWrapper ffmpeg;

        [SetUp]
        public void Setup()
        {
            ffmpeg = new FfmpegWrapper();
        }

        [Test]
        public void TestCaptureCompressedVideo()
        {
            ffmpeg.CaptureCompressedVideo(@"c:\temp\capture.mp4",7.5f);
            Thread.Sleep(1000 * 60  *10);
            ffmpeg.EndVideoCapture();
        }

        [Test]
        public void TestCaptureRawVideo()
        {
            ffmpeg.CaptureRawVideo(@"c:\temp\capture.mkv", 7.5f);
            Thread.Sleep(1000 * 10);
            ffmpeg.EndVideoCapture();
        }

        [Test]
        public void TestCaptureRawVideoAndCompress()
        {
            string source = @"c:\temp\captureRaw.mkv";
            string destination = @"c:\temp\captureCompresses.mkv";
            ffmpeg.CaptureRawVideo(source, 7.5f);
            Thread.Sleep(1000 * 10);
            ffmpeg.EndVideoCapture();
            ffmpeg.CompressVideo(source, destination);

        }

        [Test]
        public void TestGetVideoDetails()
        {
            string source = @"c:\temp\captureRaw.mp4";
            ffmpeg.CaptureCompressedVideo(source, 7.5f);
            Thread.Sleep(1000 * 2);
            ffmpeg.EndVideoCapture();
            FfmpegWrapper.VideoDetails details = ffmpeg.FetchVideoDetails(source);
        }



    }
}
