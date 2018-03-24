using System;
using System.IO;
using AM;
using AM.Runtime;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Menus;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable MustUseReturnValue

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisTreeFileTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void IrbisTreeFile_Construction_1()
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
        public void IrbisTreeFile_ParseStream_1()
        {
            TextReader reader = TextReader.Null;
            IrbisTreeFile tree = IrbisTreeFile.ParseStream(reader);
            Assert.AreEqual(0, tree.Roots.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void IrbisTreeFile_ParseStream_2()
        {
            TextReader reader = new StringReader(IrbisTreeFile.Indent + "HELLO");
            IrbisTreeFile.ParseStream(reader);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void IrbisTreeFile_ParseStream_3()
        {
            string text = "Hello\n\t\tWorld";
            TextReader reader = new StringReader(text);
            IrbisTreeFile.ParseStream(reader);
        }

        [TestMethod]
        public void IrbisTreeFile_ParseLocalFile_1()
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
        public void IrbisTreeFile_ParseLocalFile_2()
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
        public void IrbisTreeFile_Save_1()
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
            tree1.AddRoot("1 - First");
            IrbisTreeFile.Item root2 = tree1.AddRoot("2 - Second");
            root2.AddChild("2.1 - Second first");
            IrbisTreeFile.Item child = root2.AddChild("2.2 - Second second");
            child.AddChild("2.2.1 - Second second first");
            root2.AddChild("2.3 - Second third");
            IrbisTreeFile.Item root3 = tree1.AddRoot("3 - Third");
            child = root3.AddChild("3.1 - Third first");
            child.AddChild("3.1.1 - Third first first");
            tree1.AddRoot("4 - Fourth");

            return tree1;
        }

        private void _TestSerialization
            (
                [NotNull] IrbisTreeFile first
            )
        {
            byte[] bytes = first.SaveToMemory();

            IrbisTreeFile second = bytes.RestoreObjectFromMemory<IrbisTreeFile>();

            Assert.AreEqual(first.FileName, second.FileName);
            Assert.AreEqual(first.Roots.Count, second.Roots.Count);
        }

        [TestMethod]
        public void IrbisTreeFile_Serialization_1()
        {
            IrbisTreeFile tree = new IrbisTreeFile();
            _TestSerialization(tree);

            tree = _CreateTree();
            _TestSerialization(tree);
        }

        [TestMethod]
        public void IrbisTreeFile_Verify_1()
        {
            IrbisTreeFile tree = _CreateTree();
            Assert.IsTrue(tree.Verify(false));
            Assert.IsTrue(tree.Verify(true));

            tree = new IrbisTreeFile();
            Assert.IsFalse(tree.Verify(false));
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void IrbisTreeFile_Verify_2()
        {
            IrbisTreeFile tree = new IrbisTreeFile();
            IrbisTreeFile.Item item = new IrbisTreeFile.Item();
            tree.Roots.Add(item);
            tree.Verify(true);
        }

        [TestMethod]
        public void IrbisTreeFile_Verify_3()
        {
            IrbisTreeFile tree = new IrbisTreeFile();
            IrbisTreeFile.Item item = new IrbisTreeFile.Item();
            tree.Roots.Add(item);
            Assert.IsFalse(tree.Verify(false));
        }

        [TestMethod]
        public void IrbisTreeFile_SaveToLocalFile_1()
        {
            IrbisTreeFile tree = _CreateTree();
            string fileName = Path.GetTempFileName();
            tree.SaveToLocalFile(fileName, IrbisEncoding.Ansi);
            int length = File.ReadAllText(fileName, IrbisEncoding.Ansi).DosToUnix().Length;
            Assert.AreEqual(180, length);
        }

        [TestMethod]
        public void IrbisTreeFile_ToMenu_1()
        {
            IrbisTreeFile tree = _CreateTree();
            MenuFile menu = tree.ToMenu();
            Assert.AreEqual(10, menu.Entries.Count);
        }

        [TestMethod]
        public void IrbisTreeFile_Walk_1()
        {
            IrbisTreeFile tree = _CreateTree();
            int count = 0;
            Action<IrbisTreeFile.Item> action = item => count++;
            tree.Walk(action);
            Assert.AreEqual(10, count);
        }

        [TestMethod]
        public void IrbisTreeFile_Delimiter_1()
        {
            string saveDelimiter = IrbisTreeFile.Item.Delimiter;
            IrbisTreeFile.Item.Delimiter = "!";
            Assert.AreEqual("!", IrbisTreeFile.Item.Delimiter);
            IrbisTreeFile.Item.Delimiter = saveDelimiter;
        }
    }
}
