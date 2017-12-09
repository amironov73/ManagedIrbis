using System.IO;
using System.Text;

using AM.Runtime;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class MessageFileTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetFileName()
        {
            return Path.Combine
                (
                    Irbis64RootPath,
                    MessageFile.DefaultName
                );
        }

        [TestMethod]
        public void MessageFile_Construction_1()
        {
            MessageFile file = new MessageFile();
            Assert.IsNull(file.Name);
            Assert.AreEqual(0, file.LineCount);
        }

        [TestMethod]
        public void MessageFile_ReadLocalFile_1()
        {
            string fileName = _GetFileName();
            Encoding encoding = IrbisEncoding.Ansi;
            MessageFile file = MessageFile.ReadLocalFile(fileName, encoding);
            Assert.AreEqual(1422, file.LineCount);
            Assert.AreSame(fileName, file.Name);
            Assert.AreEqual("Ассоциация ЭБНИТ", file.GetMessage(1));
            Assert.AreEqual("MISSING @11111", file.GetMessage(11111));
        }

        private void _TestSerialization
            (
                [NotNull] MessageFile first
            )
        {
            byte[] bytes = first.SaveToMemory();
            MessageFile second = bytes.RestoreObjectFromMemory<MessageFile>();
            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.LineCount, second.LineCount);
        }

        [TestMethod]
        public void MessageFile_Serialization_1()
        {
            MessageFile file = new MessageFile();
            _TestSerialization(file);

            string fileName = _GetFileName();
            Encoding encoding = IrbisEncoding.Ansi;
            file = MessageFile.ReadLocalFile(fileName, encoding);
            _TestSerialization(file);
        }
    }
}
