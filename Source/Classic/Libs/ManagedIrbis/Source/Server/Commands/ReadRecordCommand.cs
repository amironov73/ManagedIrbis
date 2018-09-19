// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReadRecordCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Text;

using AM;
using AM.Logging;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Direct;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ReadRecordCommand
        : ServerCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReadRecordCommand
            (
                [NotNull] WorkData data
            )
            : base(data)
        {
        }

        #endregion

        #region Private members

        /// <summary>
        /// Кодирование записи в клиентское представление.
        /// </summary>
        public static string EncodeRecord
            (
                MarcRecord record
            )
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat
                (
                    "{0}#{1}",
                    record.Mfn.ToInvariantString(),
                    ((int)record.Status).ToInvariantString()
                );
            result.Append("\r\n");
            result.AppendFormat
                (
                    "0#{0}",
                    record.Version.ToInvariantString()
                );
            result.Append("\r\n");

            foreach (RecordField field in record.Fields)
            {
                result.AppendFormat
                    (
                        "{0}#",
                        field.Tag.ToInvariantString()
                    );
                result.Append(field.Value);

                foreach (SubField subField in field.SubFields)
                {
                    result.AppendFormat
                        (
                            "{0}{1}{2}",
                            SubField.Delimiter,
                            subField.Code,
                            subField.Value
                        );
                }

                result.Append("\r\n");
            }

            return result.ToString();
        }

        #endregion

        #region ServerCommand members

        /// <inheritdoc cref="ServerCommand.Execute" />
        public override void Execute()
        {
            // TODO перейти на RawRecord, если не требуется форматирование

            // В случае физически удаленной записи возвращается 2 строки:
            // 1-я строка - ZERO
            // 2-я строка – UTF-8(ЗАПИСЬ ФИЗИЧЕСКИ УДАЛЕНА)

            IrbisServerEngine engine = Data.Engine.ThrowIfNull();
            engine.OnBeforeExecute(Data);

            try
            {
                ServerContext context = engine.RequireContext(Data);
                Data.Context = context;
                UpdateContext();

                ClientRequest request = Data.Request.ThrowIfNull();
                string database = request.RequireAnsiString();
                int mfn = request.GetInt32();
                int needLock = request.GetInt32();
                string format = request.GetAutoString();
                string formatted = null;

                MarcRecord record;
                using (LocalProvider provider = engine.GetProvider(database))
                {
                    record = provider.ReadRecord(mfn);
                    if (!string.IsNullOrEmpty(format)
                        && !ReferenceEquals(record, null))
                    {
                        formatted = provider.FormatRecord(record, format);
                        formatted = IrbisText.WindowsToIrbis(formatted);
                    }
                }

                if (needLock != 0)
                {
                    using (DirectAccess64 direct = engine.GetDatabase(database))
                    {
                        direct.Xrf.LockRecord(mfn, true);
                    }
                }

                ServerResponse response = Data.Response.ThrowIfNull();
                response.WriteInt32(0).NewLine();
                if (!ReferenceEquals(record, null))
                {
                    string recordText = EncodeRecord(record);
                    response.WriteUtfString(recordText).NewLine();
                }

                if (!string.IsNullOrEmpty(formatted))
                {
                    response.WriteUtfString("#").NewLine();
                    response.WriteInt32(0).NewLine();
                    response.WriteUtfString(formatted).NewLine();
                }

                SendResponse();
            }
            catch (IrbisException exception)
            {
                SendError(exception.ErrorCode);
            }
            catch (Exception exception)
            {
                Log.TraceException("ReadRecordCommand::Execute", exception);
                SendError(-8888);
            }

            engine.OnAfterExecute(Data);
        }

        #endregion
    }
}
