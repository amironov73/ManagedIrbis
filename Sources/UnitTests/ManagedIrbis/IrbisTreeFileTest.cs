using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisTreeFileTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void TestIrbisTreeFileConstruction()
        {
            IrbisTreeFile tree = new IrbisTreeFile();

            Assert.AreEqual(0, tree.Roots.Count);

            IrbisTreeFile.Item root = tree.AddRoot("1 - First root");
            Assert.AreEqual(1, tree.Roots.Count);
            Assert.AreEqual("1", root.Prefix);
            Assert.AreEqual("First root", root.Suffix);
            Assert.AreEqual("1 - First root", root.Value);
            Assert.AreEqual(0, root.Children.Count);

            IrbisTreeFile.Item child = root.AddChild("1.1 - First - child");
            Assert.AreEqual(1, root.Children.Count);
            Assert.AreEqual(0, child.Children.Count);
            Assert.AreEqual("1.1 - First - child", child.Value);
            Assert.AreEqual("1.1", child.Prefix);
            Assert.AreEqual("First - child", child.Suffix);

            _TestSerialization(tree);
        }

        [TestMethod]
        public void TestIrbisTreeFileParseLocalFile1()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "II.TRE"
                );

            IrbisTreeFile tree = IrbisTreeFile.ReadLocalFile
                (
                    fileName,
                    IrbisEncoding.Ansi
                );
            Assert.AreEqual(4, tree.Roots.Count);
            Assert.AreEqual("1", tree.Roots[0].Prefix);
            Assert.AreEqual
                (
                    "БИБЛИОТЕЧНОЕ ДЕЛО. БИБЛИОТЕКОВЕДЕНИЕ",
                    tree.Roots[0].Suffix
                );

            _TestSerialization(tree);
        }

        [TestMethod]
        public void TestIrbisTreeFileParseLocalFile2()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "test1.tre"
                );

            IrbisTreeFile tree = IrbisTreeFile.ReadLocalFile
                (
                    fileName,
                    IrbisEncoding.Ansi
                );

            Assert.AreEqual(4, tree.Roots.Count);
            Assert.AreEqual(0, tree.Roots[0].Children.Count);
            Assert.AreEqual(3, tree.Roots[1].Children.Count);
            Assert.AreEqual(0, tree.Roots[1].Children[0].Children.Count);
            Assert.AreEqual(1, tree.Roots[1].Children[1].Children.Count);
            Assert.AreEqual(0, tree.Roots[1].Children[1].Children[0].Children.Count);
            Assert.AreEqual(1, tree.Roots[2].Children.Count);
            Assert.AreEqual(0, tree.Roots[3].Children.Count);


            _TestSerialization(tree);
        }

        [TestMethod]
        public void TestIrbisTreeFileSave()
        {
            var tree1 = _CreateTree();

            StringWriter writer = new StringWriter();
            tree1.Save(writer);
            string actual = writer.ToString();
            string expected = string.Format
                (
                    "1 - First{0}"
                    + "2 - Second{0}"
                    + "\t2.1 - Second first{0}"
                    + "\t2.2 - Second second{0}"
                    + "\t\t2.2.1 - Second second first{0}"
                    + "\t2.3 - Second third{0}"
                    + "3 - Third{0}"
                    + "\t3.1 - Third first{0}"
                    + "\t\t3.1.1 - Third first first{0}"
                    + "4 - Fourth{0}",
                    Environment.NewLine
                );
            Assert.AreEqual(expected, actual);

            StringReader reader = new StringReader(actual);
            IrbisTreeFile tree2 = IrbisTreeFile.ParseStream(reader);

            Assert.AreEqual(tree1.Roots.Count, tree2.Roots.Count);
        }

        private IrbisTreeFile _CreateTree()
        {
            IrbisTreeFile tree1 = new IrbisTreeFile();
            IrbisTreeFile.Item root1 = tree1.AddRoot("1 - First");
            IrbisTreeFile.Item root2 = tree1.AddRoot("2 - Second");
            root2.AddChild("2.1 - Second first");
            IrbisTreeFile.Item child = root2.AddChild("2.2 - Second second");
            child.AddChild("2.2.1 - Second second first");
            child = root2.AddChild("2.3 - Second third");
            IrbisTreeFile.Item root3 = tree1.AddRoot("3 - Third");
            child = root3.AddChild("3.1 - Third first");
            child.AddChild("3.1.1 - Third first first");
            IrbisTreeFile.Item root4 = tree1.AddRoot("4 - Fourth");
            
            return tree1;
        }

        private void _TestSerialization
            (
                IrbisTreeFile first
            )
        {
            byte[] bytes = first.SaveToMemory();

            IrbisTreeFile second = bytes
                .RestoreObjectFromMemory<IrbisTreeFile>();

            Assert.AreEqual(first.FileName, second.FileName);
            Assert.AreEqual(first.Roots.Count, second.Roots.Count);
        }

        [TestMethod]
        public void TestIrbisTreeFileSerialization()
        {
            IrbisTreeFile tree = new IrbisTreeFile();
            _TestSerialization(tree);
        }

        [TestMethod]
        public void TestIrbisTreeFileVerify()
        {
            IrbisTreeFile tree = _CreateTree();
            
            Assert.IsTrue(tree.Verify(false));

            tree = new IrbisTreeFile();

            Assert.IsFalse(tree.Verify(false));
        }
    }
}
