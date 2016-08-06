using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.IO;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class TextReaderUtilityTest
    {
        private readonly string _nl
            = Environment.NewLine;

        [TestMethod]
        public void TestTextReaderUtility_RequireLine()
        {
            string testText = "first line"
                              + _nl
                              + "second line";
            int counter = 0;
            StringReader reader = new StringReader(testText);
            reader.RequireLine();
            counter++;
            reader.RequireLine();
            counter++;
            try
            {
                reader.RequireLine();
            }
            catch (ArsMagnaException)
            {
                counter++;
            }

            Assert.AreEqual(3, counter);
        }
    }
}
