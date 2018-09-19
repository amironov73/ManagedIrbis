// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchCommand.cs --
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
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SearchCommand
        : ServerCommand
    {
        #region Properties

        /// <summary>
        /// Search parameters.
        /// </summary>
        public SearchParameters Parameters { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchCommand
            (
                [NotNull] WorkData data
            )
            : base(data)
        {
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
                Parameters = new SearchParameters
                {
                    Database = request.RequireAnsiString(),
                    SearchExpression = request.GetUtfString(),
                    NumberOfRecords = request.GetInt32(),
                    FirstRecord = request.GetInt32(),
                    FormatSpecification = request.GetAutoString(),
                    MinMfn = request.GetInt32(),
                    MaxMfn = request.GetInt32(),
                    SequentialSpecification = request.GetAutoString()
                };

                ServerResponse response = Data.Response.ThrowIfNull();
                using (LocalProvider provider = engine.GetProvider(Parameters.Database))
                {
                    int[] found = provider.Search(Parameters.SearchExpression);
                    response.WriteInt32(0).NewLine();
                    response.WriteInt32(found.Length).NewLine();
                    int howMany = found.Length;
                    if (Parameters.NumberOfRecords > 0
                        && Parameters.NumberOfRecords < howMany)
                    {
                        howMany = Parameters.NumberOfRecords;
                    }

                    if (Parameters.FirstRecord == 0)
                    {
                        response.WriteInt32(found.Length);
                    }
                    else
                    {
                        int shift = Parameters.FirstRecord - 1;
                        if (howMany + shift > found.Length)
                        {
                            howMany = found.Length - shift;
                        }
                        for (int i = 0; i < howMany; i++)
                        {
                            int mfn = found[i + shift];
                            response.WriteInt32(mfn);
                            if (!string.IsNullOrEmpty(Parameters.FormatSpecification))
                            {
                                response.WriteUtfString("#");
                                MarcRecord record = provider.ReadRecord(mfn);
                                if (!ReferenceEquals(record, null))
                                {
                                    string text = provider.FormatRecord
                                        (
                                            record,
                                            Parameters.FormatSpecification
                                        );
                                    text = IrbisText.WindowsToIrbis(text);
                                    response.WriteUtfString(text);
                                }
                            }

                            response.NewLine();
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
                Log.TraceException("SearchCommand::Execute", exception);
                SendError(-8888);
            }

            engine.OnAfterExecute(Data);
        }

        #endregion
    }
}
