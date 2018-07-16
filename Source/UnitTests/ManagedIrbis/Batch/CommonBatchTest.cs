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
    public class CommonBatchTest
        : Common.CommonUnitTest
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
            byte[][] rawRequest = { new byte[0], new byte[0] };
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
        protected Mock<IIrbisConnection> GetMock()
        {
            Mock<IIrbisConnection> result = new Mock<IIrbisConnection>();
            IIrbisConnection connection = result.Object;

            // CommandFactory
            result.SetupGet(c => c.CommandFactory)
                .Returns(new CommandFactory(connection));

            // Database
            result.SetupGet(c => c.Database)
                .Returns("IBIS");

            // ExecuteCommand
            result.Setup(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()))
                .Returns((FormatCommand command)
                    => ExecuteFormatCommand(command, false));

            // FormatRecords
            result.Setup(c => c.FormatRecords(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<IEnumerable<int>>()))
                .Returns
                    (
                        (string db, string format, IEnumerable<int> mfns) =>
                        {
                            List<string> list = new List<string>();
                            foreach (int mfn in mfns)
                            {
                                string text = string.Format
                                    (
                                        "{0} {1} {2}",
                                        format,
                                        db,
                                        mfn
                                    );
                                list.Add(text);
                            }

                            return list.ToArray();
                        }
                    );

            // GetMaxMfn
            result.Setup(c => c.GetMaxMfn(It.IsAny<string>()))
                .Returns(4);
            result.Setup(c => c.GetMaxMfn())
                .Returns(4);

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

            // Search
            result.Setup(c => c.Search(It.IsAny<string>()))
                .Returns(new[] { 1, 2, 3 });

            return result;
        }
    }
}
