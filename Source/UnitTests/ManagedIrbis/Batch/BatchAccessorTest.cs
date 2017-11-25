using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;

using Moq;

namespace UnitTests.ManagedIrbis.Batch
{
    [TestClass]
    public class BatchAccessorTest
    {
        private static void AppendIrbisLine
            (
                [NotNull] StringBuilder builder,
                [NotNull] string format,
                params object[] args
            )
        {
            builder.AppendFormat(format, args);
            builder.Append('\x001F');
        }

        private static void EncodeField
            (
                [NotNull] StringBuilder builder,
                [NotNull] RecordField field
            )
        {
            builder.AppendFormat("{0}#", field.Tag);
            builder.Append(field.Value);

            foreach (SubField subField in field.SubFields)
            {
                ProtocolText.EncodeSubField(builder, subField);
            }

            builder.Append('\x001F');
        }

        [NotNull]
        private string EncodeRecord
            (
                [NotNull] MarcRecord record
            )
        {
            StringBuilder result = new StringBuilder();

            AppendIrbisLine
                (
                    result,
                    "0#{0}",
                    record.Version
                );
            AppendIrbisLine
                (
                    result,
                    "{0}#{1}",
                    record.Mfn,
                    (int)record.Status
                );

            foreach (RecordField field in record.Fields)
            {
                EncodeField(result, field);
            }

            return result.ToString();

        }

        [NotNull]
        private ServerResponse ExecuteFormatCommand
            (
                [NotNull] FormatCommand command,
                bool emptyRecords
            )
        {
            IIrbisConnection connection = command.Connection;
            List<int> list = command.MfnList;
            int count = list.Count;
            command.FormatResult = new string[count];
            for (int i = 0; i < count; i++)
            {
                MarcRecord record = new MarcRecord
                {
                    Database = connection.Database,
                    Mfn = list[i],
                    Version = 1,
                    Status = RecordStatus.Last
                };
                if (!emptyRecords)
                {
                    record.Fields.Add(RecordField.Parse(100, "Field100"));
                    record.Fields.Add(RecordField.Parse(300, "Field200"));
                    record.Fields.Add(RecordField.Parse(300, "Field300"));
                }
                string line = EncodeRecord(record);
                command.FormatResult[i] = line;
            }

            byte[] rawAnswer = new byte[0];
            byte[] rawRequest = new byte[0];
            ServerResponse response = new ServerResponse
            (
                connection,
                rawAnswer,
                rawRequest,
                true
            );

            return response;
        }

        [NotNull]
        private Mock<IIrbisConnection> GetMock()
        {
            Mock<IIrbisConnection> result = new Mock<IIrbisConnection>();
            IIrbisConnection connection = result.Object;

            // CommandFactory
            result.SetupGet(c => c.CommandFactory)
                .Returns(new CommandFactory(connection));

            // ExecuteCommand
            result.Setup(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()))
                .Returns((FormatCommand command)
                => ExecuteFormatCommand(command, false));

            // ReadRecord
            result.Setup(c => c.ReadRecord(It.IsAny<string>(),
                    It.IsAny<int>(), It.IsAny<bool>(),
                    It.IsAny<string>()))
                .Returns
                    (
                        (string db, int mfn, bool flag, string fmt) =>
                            new MarcRecord
                            {
                                Database = db,
                                Mfn = mfn
                            }
                    );

            return result;
        }

        private int RecordToMfn
            (
                [NotNull] MarcRecord record
            )
        {
            return record.Mfn;
        }

        [TestMethod]
        public void BatchAccessor_Construction_1()
        {
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchAccessor batch = new BatchAccessor(connection);
            Assert.AreSame(connection, batch.Connection);
        }

        [TestMethod]
        public void BatchAccessor_ReadRecords_1()
        {
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchAccessor batch = new BatchAccessor(connection);
            int[] mfnList = new int[0];
            MarcRecord[] records = batch.ReadRecords("IBIS", mfnList);
            Assert.AreEqual(0, records.Length);
        }

        [TestMethod]
        public void BatchAccessor_ReadRecords_2()
        {
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchAccessor batch = new BatchAccessor(connection);
            int[] mfnList = { 1 };
            MarcRecord[] records = batch.ReadRecords("IBIS", mfnList);
            Assert.AreEqual(1, records.Length);
            Assert.AreEqual(1, records[0].Mfn);

            mock.Verify(c => c.ReadRecord(It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<bool>(),
                It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BatchAccessor_ReadRecords_3()
        {
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchAccessor batch = new BatchAccessor(connection);
            int[] mfnList = { 1, 2, 3 };
            MarcRecord[] records = batch.ReadRecords("IBIS", mfnList);
            Assert.AreEqual(3, records.Length);
            records = records.OrderBy(record => record.Mfn).ToArray();
            Assert.AreEqual(1, records[0].Mfn);
            Assert.AreEqual(2, records[1].Mfn);
            Assert.AreEqual(3, records[2].Mfn);

            mock.Verify(c => c.ReadRecord(It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<bool>(),
                It.IsAny<string>()), Times.Never);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()),
                Times.Once);
        }

        [TestMethod]
        public void BatchAccessor_ReadRecords_4()
        {
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchAccessor batch = new BatchAccessor(connection);
            int[] mfnList = new int[0];
            int[] records = batch.ReadRecords("IBIS", mfnList, RecordToMfn);
            Assert.AreEqual(0, records.Length);

            mock.Verify(c => c.ReadRecord(It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<bool>(),
                It.IsAny<string>()), Times.Never);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()),
                Times.Never);
        }

        [TestMethod]
        public void BatchAccessor_ReadRecords_5()
        {
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchAccessor batch = new BatchAccessor(connection);
            int[] mfnList = { 1 };
            int[] records = batch.ReadRecords("IBIS", mfnList, RecordToMfn);
            Assert.AreEqual(1, records.Length);
            Assert.AreEqual(1, records[0]);

            mock.Verify(c => c.ReadRecord(It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<bool>(),
                It.IsAny<string>()), Times.Once);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()),
                Times.Never);
        }

        [TestMethod]
        public void BatchAccessor_ReadRecords_6()
        {
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchAccessor batch = new BatchAccessor(connection);
            int[] mfnList = { 1, 2, 3 };
            int[] records = batch.ReadRecords("IBIS", mfnList, RecordToMfn);
            Assert.AreEqual(3, records.Length);
            Array.Sort(records);
            Assert.AreEqual(1, records[0]);
            Assert.AreEqual(2, records[1]);
            Assert.AreEqual(3, records[2]);

            mock.Verify(c => c.ReadRecord(It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<bool>(),
                It.IsAny<string>()), Times.Never);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()),
                Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void BatchAccessor_ReadRecords_7()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;

            mock.SetupGet(c => c.CommandFactory)
                .Returns(new CommandFactory(connection));
            mock.Setup(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()))
                .Returns((FormatCommand command)
                => ExecuteFormatCommand(command, true));
            BatchAccessor batch = new BatchAccessor(connection);
            int[] mfnList = { 1, 2, 3 };
            batch.ReadRecords("IBIS", mfnList);
        }
    }
}
