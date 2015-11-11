using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using video_recorder.ProcessHandler;
namespace video_recorder
{
    [TestFixture]
    class ProcessHandlerTests
    {
        
        [Test]
        public void TestRunInBlocking()
        {
            ProcessHandler p = new ProcessHandler(@"C:\Windows\System32\tree.com", @"c:\temp");
            ProcessHandler.ProcessOutput output = p.StartInBlockingMode();
            Assert.IsNotNull(output);
        }

        [Test]
        public void TestRunInNonBlocking()
        {
            ProcessHandler p = new ProcessHandler(@"C:\Windows\System32\tree.com", @"c:\");
            p.StartInNonBlockingMode();
            Thread.Sleep(100);
            ProcessHandler.ProcessOutput output = p.WaitForProcessToEnd();
            Assert.IsNotNull(output);
        }

    }
}
