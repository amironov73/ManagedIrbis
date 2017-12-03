using System;
using System.IO;
using AM.Runtime;
using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

// ReSharper disable UseObjectOrCollectionInitializer

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IlfFileTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private IlfFile.Entry _GetEntry()
        {
            IlfFile.Entry result = new IlfFile.Entry
            {
                Position = 123,
                Date = new DateTime(2017, 12, 3, 13, 57, 0),
                Deleted = false,
                Name = "Entry",
                Number = 234,
                Flags = 0,
                DataLength = 345,
                Description = "Description",
                Data = "Data"
            };

            return result;
        }

        [NotNull]
        private IlfFile _GetFile()
        {
            IlfFile result = new IlfFile
            {
                Unknown1 = 123,
                SlotCount = 234,
                EntryCount = 345,
                WriteCount = 456,
                DeleteCount = 567
            };
            result.Entries.Add(_GetEntry());

            return result;
        }

        [TestMethod]
        public void IlfFile_Entry_Construction_1()
        {
            IlfFile.Entry entry = new IlfFile.Entry();
            Assert.AreEqual(0, entry.Position);
            Assert.AreEqual(DateTime.MinValue, entry.Date);
            Assert.IsFalse(entry.Deleted);
            Assert.IsNull(entry.Name);
            Assert.AreEqual(0, entry.Number);
            Assert.AreEqual((short)0, entry.Flags);
            Assert.AreEqual(0, entry.DataLength);
            Assert.IsNull(entry.Description);
            Assert.IsNull(entry.Data);
        }

        [TestMethod]
        public void IlfFile_Entry_Properties_1()
        {
            IlfFile.Entry entry = new IlfFile.Entry();
            entry.Position = 123;
            Assert.AreEqual(123, entry.Position);
            DateTime date = new DateTime(2017, 12, 3, 13, 48, 0);
            entry.Date = date;
            Assert.AreEqual(date, entry.Date);
            entry.Deleted = true;
            Assert.IsTrue(entry.Deleted);
            string name = "name";
            entry.Name = name;
            Assert.AreSame(name, entry.Name);
            entry.Number = 234;
            Assert.AreEqual(234, entry.Number);
            entry.Flags = 345;
            Assert.AreEqual((short)345, entry.Flags);
            entry.DataLength = 456;
            Assert.AreEqual(456, entry.DataLength);
            string description = "description";
            entry.Description = description;
            Assert.AreSame(description, entry.Description);
            string data = "data";
            entry.Data = data;
            Assert.AreSame(data, entry.Data);
        }

        private void _CompareEntries
            (
                [NotNull] IlfFile.Entry first,
                [NotNull] IlfFile.Entry second
            )
        {
            Assert.AreEqual(first.Position, second.Position);
            Assert.AreEqual(first.Date, second.Date);
            Assert.AreEqual(first.Deleted, second.Deleted);
            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Number, second.Number);
            Assert.AreEqual(first.Flags, second.Flags);
            Assert.AreEqual(first.DataLength, second.DataLength);
            Assert.AreEqual(first.Description, second.Description);
            Assert.AreEqual(first.Data, second.Data);
        }

        private void _TestSerialization
            (
                [NotNull] IlfFile.Entry first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IlfFile.Entry second = bytes.RestoreObjectFromMemory<IlfFile.Entry>();
            _CompareEntries(first, second);
        }

        [TestMethod]
        public void IlfFile_Entry_Serialization_1()
        {
            IlfFile.Entry entry = new IlfFile.Entry();
            _TestSerialization(entry);

            entry = _GetEntry();
            _TestSerialization(entry);
        }

        [TestMethod]
        public void IlfFile_Entry_Verify_1()
        {
            IlfFile.Entry entry = new IlfFile.Entry();
            Assert.IsFalse(entry.Verify(false));

            entry = _GetEntry();
            Assert.IsTrue(entry.Verify(false));
        }

        [TestMethod]
        public void IlfFile_Construction_1()
        {
            IlfFile file = new IlfFile();
            Assert.AreEqual(0, file.Unknown1);
            Assert.AreEqual(0, file.SlotCount);
            Assert.AreEqual(0, file.EntryCount);
            Assert.AreEqual(0, file.WriteCount);
            Assert.AreEqual(0, file.DeleteCount);
            Assert.IsNotNull(file.Entries);
            Assert.AreEqual(0, file.Entries.Count);
        }

        [TestMethod]
        public void IlfFile_Properties_1()
        {
            IlfFile file = new IlfFile();
            file.Unknown1 = 123;
            Assert.AreEqual(123, file.Unknown1);
            file.SlotCount = 234;
            Assert.AreEqual(234, file.SlotCount);
            file.EntryCount = 345;
            Assert.AreEqual(345, file.EntryCount);
            file.WriteCount = 456;
            Assert.AreEqual(456, file.WriteCount);
            file.DeleteCount = 567;
            Assert.AreEqual(567, file.DeleteCount);
        }

        [TestMethod]
        public void IlfFile_ReadLocalFile_1()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "MARS_WSS.ILF"
                );

            IlfFile library = IlfFile.ReadLocalFile
                (
                    fileName,
                    IrbisEncoding.Ansi
                );

            Assert.AreEqual(190, library.Entries.Count);
            Assert.AreEqual(library.EntryCount, library.Entries.Count);

            for (int i = 0; i < library.EntryCount; i++)
            {
                IlfFile.Entry entry = library.Entries[i];
                Assert.AreEqual(i + 1, entry.Number);
                Assert.IsNotNull(entry.Name);
                Assert.IsNotNull(entry.Data);
            }

            Assert.IsNotNull(library.GetFile("PR"));
            Assert.IsNotNull(library.GetFile("pr"));
            Assert.IsNull(library.GetFile("ZR"));

            Assert.IsTrue
                (
                    library.Verify(false)
                );
        }

        private void _TestSerialization
            (
                [NotNull] IlfFile first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IlfFile second = bytes.RestoreObjectFromMemory<IlfFile>();
            for (int i = 0; i < first.Entries.Count; i++)
            {
                _CompareEntries(first.Entries[i], second.Entries[i]);
            }
        }

        [TestMethod]
        public void IlfFile_Serialization_1()
        {
            IlfFile file = new IlfFile();
            _TestSerialization(file);

            file = _GetFile();
            _TestSerialization(file);
        }

        [TestMethod]
        public void IlfFile_Verify_1()
        {
            IlfFile file = new IlfFile();
            Assert.IsTrue(file.Verify(false));

            file = _GetFile();
            Assert.IsTrue(file.Verify(false));
        }
    }
}
