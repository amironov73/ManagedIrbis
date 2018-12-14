using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UnsafeAM.IO;

namespace UnitTests.UnsafeAM.IO
{
    [TestClass]
    public class FileUtilityTest
    {
        [TestMethod]
        public void FileUtility_FindFileInPath_1()
        {
            string path = Environment.GetEnvironmentVariable("PATH");
            string found = FileUtility.FindFileInPath
                (
                    "cmd.exe",
                    path,
                    ';'
                );
            Assert.IsNotNull(found);
        }
    }
}
