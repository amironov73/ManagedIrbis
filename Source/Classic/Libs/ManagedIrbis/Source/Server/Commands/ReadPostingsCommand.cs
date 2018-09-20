// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReadPostingsCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM;
using AM.Logging;

using JetBrains.Annotations;

using ManagedIrbis.Direct;
using ManagedIrbis.Infrastructure;
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
    public class ReadPostingsCommand
        : ServerCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReadPostingsCommand
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
                PostingParameters parameters = new PostingParameters
                {
                    Database = request.RequireAnsiString(),
                    NumberOfPostings = request.GetInt32(),
                    FirstPosting = request.GetInt32(),
                    Format = request.GetAutoString(),
                    Term = request.RequireUtfString()
                };

                // TODO shift and number
                // TODO format
                // TODO list of terms

                int returnCode = 0;
                TermLink[] links;
                using (DirectAccess64 direct = engine.GetDatabase(parameters.Database))
                {
                    links = direct.ReadLinks(parameters.Term);
                }

                if (links.Length == 0)
                {
                    returnCode = (int) IrbisReturnCode.TermNotExist;
                }

                ServerResponse response = Data.Response.ThrowIfNull();
                response.WriteInt32(returnCode).NewLine();
                foreach (TermLink link in links)
                {
                    string line = string.Format
                        (
                            "{0}#{1}#{2}#{3}",
                            link.Mfn,
                            link.Tag,
                            link.Occurrence,
                            link.Index
                        );
                    response.WriteUtfString(line).NewLine();
                }
                SendResponse();
            }
            catch (IrbisException exception)
            {
                SendError(exception.ErrorCode);
            }
            catch (Exception exception)
            {
                Log.TraceException("ReadPostingsCommand::Execute", exception);
                SendError(-8888);
            }

            engine.OnAfterExecute(Data);
        }

        #endregion
    }
}
