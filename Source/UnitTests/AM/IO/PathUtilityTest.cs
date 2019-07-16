using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class PathUtilityTest
    {
        private readonly string _sl = "/";

        private readonly string _bs
            = new string(Path.DirectorySeparatorChar, 1);

        [TestMethod]
        public void PathUtility_AppendBackslash_1()
        {
            const string source = "Some\\Path";
            string expected = source + _bs;
            string actual = PathUtility.AppendBackslash(source);
            Assert.AreEqual(expected, actual);

            actual = PathUtility.AppendBackslash(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PathUtility_Combine_1()
        {
            Assert.AreEqual
                (
                    "1" + _bs + "2" + _bs + "3",
                    PathUtility.Combine("1", "2", "3")
                );
        }

        [TestMethod]
        public void PathUtility_ConvertSlashes_1()
        {
            string source = "Some" + _sl + "Path";
            string expected = "Some" + _bs + "Path";
            string actual = PathUtility.ConvertSlashes(source);
            Assert.AreEqual(expected, actual);

            source = "Some" + _bs + "Path";
            actual = PathUtility.ConvertSlashes(source);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PathUtility_ExpandHomePath_1()
        {
            string sourcePath = "~/Pictures";
            string expandedPath = PathUtility.ExpandHomePath(sourcePath);
            Assert.IsNotNull(expandedPath);
            Assert.IsTrue(expandedPath.EndsWith("Pictures"));
        }

        [TestMethod]
        public void PathUtility_ExpandHomePath_2()
        {
            string sourcePath = null;
            string expandedPath = PathUtility.ExpandHomePath(sourcePath);
            Assert.IsNull(expandedPath);
        }

        [TestMethod]
        public void PathUtility_ExpandHomePath_3()
        {
            string sourcePath = string.Empty;
            string expandedPath = PathUtility.ExpandHomePath(sourcePath);
            Assert.AreEqual(expandedPath, string.Empty);
        }

        [TestMethod]
        public void PathUtility_ExpandHomePath_4()
        {
            string sourcePath = "some_file.txt";
            string expandedPath = PathUtility.ExpandHomePath(sourcePath);
            Assert.AreEqual(sourcePath, expandedPath);
        }

        [TestMethod]
        public void PathUtility_GetRelativePath_1()
        {
            string firstPath = "hello" + _bs + "world";
            string secondPath = "hello" + _bs + "dotnet";
            string actual = PathUtility.GetRelativePath(secondPath, firstPath);
            string expected = ".." + _bs + "dotnet";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PathUtility_GetRelativePath_2()
        {
            string firstPath = "hello" + _bs + "world";
            string secondPath = "nothing" + _bs + "common";
            PathUtility.GetRelativePath(secondPath, firstPath);
        }

        [TestMethod]
        public void PathUtility_GetRelativePath_3()
        {
            string firstPath = _bs + "hello" + _bs + "world";
            string secondPath = _bs + "nothing" + _bs + "common";
            string actual = PathUtility.GetRelativePath(secondPath, firstPath);
            string expected = ".." + _bs + ".." + secondPath;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PathUtiltity_GetRelativePath_4()
        {
            string firstPath = "hello" + _bs + "very" + _bs + "long" + _bs + "path";
            string secondPath = "hello" + _bs + "other" + _bs + "path";
            string actual = PathUtility.GetRelativePath(secondPath, firstPath);
            string excpected = ".." + _bs + ".." + _bs + ".." + _bs + "other" + _bs + "path";
            Assert.AreEqual(excpected, actual);
        }

        [TestMethod]
        public void PathUtility_MapPath_1()
        {
            string source = "Some" + _bs + "Path";
            string actual = PathUtility.MapPath(source);
            Assert.AreNotEqual(source, actual);
        }

        [TestMethod]
        public void PathUtility_StripExtension_1()
        {
            string source = "Some" + _bs + "Path" + _bs + "FileName.ext";
            string expected = "Some" + _bs + "Path" + _bs + "FileName";
            string actual = PathUtility.StripExtension(source);
            Assert.AreEqual(expected, actual);

            actual = PathUtility.StripExtension(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PathUtility_StripTrailingBackslash_1()
        {
            string source = "Some" + _bs + "Path" + _bs;
            string expected = "Some" + _bs + "Path";
            string actual = PathUtility.StripTrailingBackslash(source);
            Assert.AreEqual(expected, actual);

            actual = PathUtility.StripTrailingBackslash(actual);
            Assert.AreEqual(expected, actual);
        }
    }
}
