// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FormatRecordCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM;
using AM.Logging;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class FormatCommand
        : ServerCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FormatCommand
            (
                [NotNull] WorkData data
            )
            : base(data)
        {
        }

        #endregion

        #region Private members

        [NotNull]
        private static MarcRecord _ParseRecord
            (
                [NotNull] string[] lines
            )
        {
            MarcRecord result = new MarcRecord();

            string[] parts = lines[0].Split('#');
            result.Mfn = FastNumber.ParseInt32(parts[0]);
            if (parts.Length != 1)
            {
                result.Status = (RecordStatus) FastNumber.ParseInt32(parts[1]);
            }

            parts = lines[1].Split('#');
            result.Version = FastNumber.ParseInt32(parts[1]);

            for (int i = 2; i < lines.Length; i++)
            {
                string line = lines[i];
                if (!string.IsNullOrEmpty(line))
                {
                    RecordField field = RecordField.Parse(line);
                    result.Fields.Add(field);
                }
            }

            return result;
        }

        #endregion

        #region ServerCommand members

        /// <inheritdoc cref="ServerCommand.Execute" />
        public override void Execute()
        {
            IrbisServerEngine engine = Data.Engine.ThrowIfNull();
            engine.OnBeforeExecute(Data);

            try
            {
                ServerContext context = engine.RequireContext(Data);
                Data.Context = context;
                UpdateContext();

                ClientRequest request = Data.Request.ThrowIfNull();
                ServerResponse response = Data.Response.ThrowIfNull();
                string database = request.RequireAnsiString();
                string format = request.RequireAutoString();
                int count = request.GetInt32();
                response.WriteInt32(0).NewLine();
                using (LocalProvider provider = engine.GetProvider(database))
                {
                    if (count < 0)
                    {
                        string text = request.RemainingUtfText();
                        string[] lines = IrbisText.IrbisToWindows(text)
                            .ThrowIfNull().SplitLines();
                        MarcRecord record = _ParseRecord(lines);
                        text = provider.FormatRecord(record, format);
                        text = IrbisText.WindowsToIrbis(text);
                        response.WriteUtfString(text).NewLine();
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            int mfn = request.GetInt32();
                            MarcRecord record = provider.ReadRecord(mfn);
                            if (ReferenceEquals(record, null))
                            {
                                // TODO выяснить, что пишется на самом деле
                                response.WriteUtfString("ERROR").NewLine();
                            }
                            else
                            {
                                string text = provider.FormatRecord(record, format);
                                text = IrbisText.WindowsToIrbis(text);
                                response.WriteUtfString(text).NewLine();
                            }
                        }
                    }
                }

                SendResponse();
            }
            catch (IrbisException exception)
            {
                SendError(exception.ErrorCode);
            }
            catch (Exception exception)
            {
                Log.TraceException("FormatRecordCommand::Execute", exception);
                SendError(-8888);
            }

            engine.OnAfterExecute(Data);
        }

        #endregion
    }
}
