// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReadFileCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using AM;
using AM.Logging;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ReadFileCommand
        : ServerCommand
    {
        #region Constants

        /// <summary>
        /// Preamble for binary files.
        /// </summary>
        public static byte[] Preamble =
        {
            73, 82, 66, 73, 83, 95, 66, 73, 78, 65, 82, 89, 95, 68,
            65, 84, 65
        };

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReadFileCommand
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
                ServerResponse response = Data.Response.ThrowIfNull();
                string[] lines = request.RemainingAnsiStrings();
                foreach (string line in lines)
                {
                    try
                    {
                        FileSpecification specification = FileSpecification.Parse(line);
                        string filename = engine.ResolveFile(specification);
                        if (string.IsNullOrEmpty(filename))
                        {
                            response.NewLine();
                        }
                        else
                        {
                            byte[] content = File.ReadAllBytes(filename);
                            if (specification.BinaryFile)
                            {
                                response.Memory.Write(Preamble, 0, Preamble.Length);
                                response.Memory.Write(content, 0, content.Length);
                            }
                            else
                            {
                                IrbisText.WindowsToIrbis(content);
                                response.Memory.Write(content, 0, content.Length);
                                response.NewLine();
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.TraceException("ReadFileCommand::Execute", exception);
                        response.NewLine();
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
                Log.TraceException("ReadFileCommand::Execute", exception);
                SendError(-8888);
            }

            engine.OnAfterExecute(Data);
        }

        #endregion
    }
}
