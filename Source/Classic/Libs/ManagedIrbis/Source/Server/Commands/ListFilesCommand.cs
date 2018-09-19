// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ListFilesCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AM;
using AM.IO;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Direct;
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
    public class ListFilesCommand
        : ServerCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ListFilesCommand
            (
                [NotNull] WorkData data
            )
            : base(data)
        {
        }

        #endregion

        #region Private members

        [CanBeNull]
        private string _ResolveSpecification
            (
                [NotNull] FileSpecification specification
            )
        {
            Code.NotNull(specification, "specification");

            IrbisServerEngine engine = Data.Engine.ThrowIfNull();
            string fileName = specification.FileName;
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            string result;
            string database = specification.Database;
            int path = (int)specification.Path;
            if (path == 0)
            {
                result = Path.Combine(engine.SystemPath, fileName);
            }
            else if (path == 1)
            {
                result = Path.Combine(engine.DataPath, fileName);
            }
            else
            {
                if (string.IsNullOrEmpty(database))
                {
                    return null;
                }

                string parPath = Path.Combine(engine.DataPath, database + ".par");
                if (!File.Exists(parPath))
                {
                    result = null;
                }
                else
                {
                    Dictionary<int, string> dictionary;
                    using (StreamReader reader
                        = TextReaderUtility.OpenRead(parPath, IrbisEncoding.Ansi))
                    {
                        dictionary = ParFile.ReadDictionary(reader);
                    }

                    if (!dictionary.ContainsKey(path))
                    {
                        result = null;
                    }
                    else
                    {
                        result = Path.Combine
                            (
                                Path.Combine(engine.SystemPath, dictionary[path]),
                                fileName
                            );
                    }
                }
            }

            return result;
        }

        [NotNull]
        private string[] _ListFiles
            (
                [CanBeNull] string template
            )
        {
            if (string.IsNullOrEmpty(template))
            {
                return StringUtility.EmptyArray;
            }

            string directory = Path.GetDirectoryName(template);
            if (string.IsNullOrEmpty(directory))
            {
                return StringUtility.EmptyArray;
            }

            string pattern = Path.GetFileName(template);
            if (string.IsNullOrEmpty(pattern))
            {
                return StringUtility.EmptyArray;
            }

            string[] result = Directory.GetFiles
                (
                    directory,
                    pattern,
                    SearchOption.TopDirectoryOnly
                );
            result = result
                .Select(fname => Path.GetFileName(fname))
                .OrderBy(_ => _)
                .ToArray();

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
                string[] lines = request.RemainingAnsiStrings();
                ServerResponse response = Data.Response.ThrowIfNull();
                foreach (string line in lines)
                {
                    FileSpecification specification = FileSpecification.Parse(line);
                    string template = _ResolveSpecification(specification);
                    string[] files = _ListFiles(template);
                    string text = string.Join(IrbisText.IrbisDelimiter, files);
                    response.WriteAnsiString(text).NewLine();
                }
                SendResponse();
            }
            catch (IrbisException exception)
            {
                SendError(exception.ErrorCode);
            }
            catch (Exception exception)
            {
                Log.TraceException("ListFilesMfnCommand::Execute", exception);
                SendError(-8888);
            }

            engine.OnAfterExecute(Data);
        }

        #endregion
    }
}
