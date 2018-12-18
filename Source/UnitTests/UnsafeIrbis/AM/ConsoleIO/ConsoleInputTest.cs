using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UnsafeAM.ConsoleIO;

namespace UnitTests.UnsafeIrbis.AM.ConsoleIO
{
    [TestClass]
    public class ConsoleInputTest
    {
        private static IConsoleDriver _saveDriver;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            _saveDriver = ConsoleInput.SetDriver(new NullConsole());
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            ConsoleInput.SetDriver(_saveDriver);
        }

        [TestMethod]
        public void ConsoleInput_BackgroundColor_1()
        {
            ConsoleInput.BackgroundColor = ConsoleColor.Black;
            Assert.AreEqual(ConsoleColor.Black, ConsoleInput.BackgroundColor);
        }

        [TestMethod]
        public void ConsoleInput_ForegroundColor_1()
        {
            ConsoleInput.ForegroundColor = ConsoleColor.Black;
            Assert.AreEqual(ConsoleColor.Black, ConsoleInput.ForegroundColor);
        }

        [TestMethod]
        public void ConsoleInput_KeyAvailable_1()
        {
            Assert.IsFalse(ConsoleInput.KeyAvailable);
        }

        [TestMethod]
        public void ConsoleInput_Title_1()
        {
            ConsoleInput.Title = "Title";
            Assert.AreEqual("Title", ConsoleInput.Title);
        }

        [TestMethod]
        public void ConsoleInput_Clear_1()
        {
            ConsoleInput.Clear();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ConsoleInput_Read_1()
        {
            Assert.AreEqual(-1, ConsoleInput.Read());
        }

        [TestMethod]
        public void ConsoleInput_ReadKey_1()
        {
            ConsoleKeyInfo info = ConsoleInput.ReadKey(false);
            Assert.AreEqual((ConsoleKey)0, info.Key);
        }

        [TestMethod]
        public void ConsoleInput_ReadLine_1()
        {
            Assert.AreEqual(null, ConsoleInput.ReadLine());
        }

        [TestMethod]
        public void ConsoleInput_Write_1()
        {
            ConsoleInput.Write("Text");
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ConsoleInput_WriteLine_1()
        {
            ConsoleInput.WriteLine();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ConsoleInput_WriteLine_2()
        {
            ConsoleInput.WriteLine("Text");
            Assert.IsTrue(true);
        }
    }
}
