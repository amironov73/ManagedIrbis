using System.IO;

using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Fst;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Fst
{
    [TestClass]
    public class FstFileTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private FstFile _GetFile()
        {
            FstFile result = new FstFile
            {
                FileName = "FST file"
            };

            result.Lines.Add(new FstLine
            {
                LineNumber = 1,
                Tag = 610,
                Method = FstIndexMethod.Method0,
                Format = "(v2 /)"
            });
            result.Lines.Add(new FstLine
            {
                LineNumber = 2,
                Tag = 700,
                Method = FstIndexMethod.Method0,
                Format = "\"^A\"v1^A"
            });
            result.Lines.Add(new FstLine
            {
                LineNumber = 3,
                Tag = 200,
                Method = FstIndexMethod.Method0,
                Format = "\"^A\"v1^T"
            });
            result.Lines.Add(new FstLine
            {
                LineNumber = 4,
                Tag = 210,
                Method = FstIndexMethod.Method0,
                Format = "\"^D\"v1^B"
            });
            result.Lines.Add(new FstLine
            {
                LineNumber = 5,
                Tag = 10,
                Method = FstIndexMethod.Method0,
                Format = "\"^A\"v1^D"
            });

            return result;
        }

        [TestMethod]
        [Description("Состояние объекта сразу после создания")]
        public void FstFile_Construction_1()
        {
            FstFile fst = new FstFile();
            Assert.IsNull(fst.FileName);
            Assert.IsNotNull(fst.Lines);
            Assert.AreEqual(0, fst.Lines.Count);
        }

        [TestMethod]
        public void FstFile_ConcatenateFormat_1()
        {
            FstFile fst = _GetFile();
            string actual = fst.ConcatenateFormat().DosToUnix();
            Assert.AreEqual("mpl,\'610\',/,(v2 /),\'\a\'mpl,\'700\',/," +
                "\"^A\"v1^A,\'\a\'mpl,\'200\',/,\"^A\"v1^T,\'\a\'mpl," +
                "\'210\',/,\"^D\"v1^B,\'\a\'mpl,\'10\',/,\"^A\"v1^D,\'\a\'", actual);
        }

        private void _TestSerialization
            (
                [NotNull] FstFile first
            )
        {
            byte[] bytes = first.SaveToMemory();

            FstFile second = bytes
                .RestoreObjectFromMemory<FstFile>();

            Assert.AreEqual(first.FileName, second.FileName);
            Assert.AreEqual(first.Lines.Count, second.Lines.Count);
            for (int i = 0; i < first.Lines.Count; i++)
            {
                Assert.AreEqual(first.Lines[i].LineNumber, second.Lines[i].LineNumber);
                Assert.AreEqual(first.Lines[i].Tag, second.Lines[i].Tag);
                Assert.AreEqual(first.Lines[i].Method, second.Lines[i].Method);
                Assert.AreEqual(first.Lines[i].Format, second.Lines[i].Format);
            }
        }

        [TestMethod]
        [Description("Ручная сериализация")]
        public void FstFile_Serialization_1()
        {
            FstFile fst = new FstFile();
            _TestSerialization(fst);

            fst = _GetFile();
            _TestSerialization(fst);
        }

        [TestMethod]
        [Description("Парсинг файла с диска")]
        public void FstFile_ParseLocalFile_1()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "QueryToRec.fst"
                );
            FstFile fst = FstFile.ParseLocalFile(fileName, IrbisEncoding.Ansi);
            Assert.AreEqual(5, fst.Lines.Count);
            _TestSerialization(fst);
        }

        [TestMethod]
        public void FstFile_Verify_1()
        {
            FstFile fst = new FstFile();
            Assert.IsFalse(fst.Verify(false));

            fst = _GetFile();
            Assert.IsTrue(fst.Verify(false));
        }

        [TestMethod]
        public void FstFile_ToXml_1()
        {
            FstFile fst = new FstFile();
            Assert.AreEqual("<fst />", XmlUtility.SerializeShort(fst));

            fst = _GetFile();
            Assert.AreEqual("<fst fileName=\"FST file\"><line tag=\"610\" method=\"Method0\"><format>(v2 /)</format></line><line tag=\"700\" method=\"Method0\"><format>\"^A\"v1^A</format></line><line tag=\"200\" method=\"Method0\"><format>\"^A\"v1^T</format></line><line tag=\"210\" method=\"Method0\"><format>\"^D\"v1^B</format></line><line tag=\"10\" method=\"Method0\"><format>\"^A\"v1^D</format></line></fst>", XmlUtility.SerializeShort(fst));
        }

        [TestMethod]
        public void FstFile_ToJson_1()
        {
            FstFile fst = new FstFile();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(fst));

            fst = _GetFile();
            Assert.AreEqual("{\'fileName\':\'FST file\',\'lines\':[{\'tag\':610,\'method\':0,\'format\':\'(v2 /)\'},{\'tag\':700,\'method\':0,\'format\':\'\"^A\"v1^A\'},{\'tag\':200,\'method\':0,\'format\':\'\"^A\"v1^T\'},{\'tag\':210,\'method\':0,\'format\':\'\"^D\"v1^B\'},{\'tag\':10,\'method\':0,\'format\':\'\"^A\"v1^D\'}]}", JsonUtility.SerializeShort(fst));
        }

        [TestMethod]
        public void FstFile_ToString_1()
        {
            FstFile fst = new FstFile();
            Assert.AreEqual("(null)", fst.ToString());

            fst = _GetFile();
            Assert.AreEqual("FST file", fst.ToString());
        }
    }
}
