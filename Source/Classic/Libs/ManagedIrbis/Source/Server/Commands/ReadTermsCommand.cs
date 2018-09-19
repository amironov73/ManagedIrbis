// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReadTermsCommand.cs --
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
    public class ReadTermsCommand
        : ServerCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReadTermsCommand
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
                TermParameters parameters = new TermParameters
                {
                    Database = request.RequireAnsiString(),
                    StartTerm = request.RequireUtfString(),
                    NumberOfTerms = request.GetInt32(),
                    Format = request.GetUtfString()
                };

                if (parameters.NumberOfTerms == 0)
                {
                    parameters.NumberOfTerms = IrbisConstants.MaxPostings;
                }

                TermInfo[] terms;
                int returnCode = 0;
                using (DirectAccess64 direct = engine.GetDatabase(parameters.Database))
                {
                    terms = direct.ReadTerms(parameters);
                }

                if (terms.Length != 0
                    && terms[0].Text != parameters.StartTerm)
                {
                    returnCode = (int) IrbisReturnCode.TermNotExist;
                }
                if (terms.Length < parameters.NumberOfTerms)
                {
                    returnCode = (int) IrbisReturnCode.LastTermInList;
                }

                // TODO format

                ServerResponse response = Data.Response.ThrowIfNull();
                response.WriteInt32(returnCode).NewLine();
                foreach (TermInfo term in terms)
                {
                    response.WriteUtfString(term.ToString()).NewLine();
                }
                SendResponse();
            }
            catch (IrbisException exception)
            {
                SendError(exception.ErrorCode);
            }
            catch (Exception exception)
            {
                Log.TraceException("ReadTermsCommand::Execute", exception);
                SendError(-8888);
            }

            engine.OnAfterExecute(Data);
        }

        #endregion
    }
}
