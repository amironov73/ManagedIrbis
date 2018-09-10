// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ServerUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ServerUtility
    {
        #region Constants

        /// <summary>
        /// Inclusion begin sign.
        /// </summary>
        public const char InclusionStart = '\x1C';

        /// <summary>
        /// Inclusion end sign.
        /// </summary>
        public const char InclusionEnd = '\x1D';

        #endregion

        #region Public methods

        /// <summary>
        /// Expand inclusion.
        /// </summary>
        [NotNull]
        public static string ExpandInclusion
            (
                [NotNull] string text,
                [NotNull] string extension,
                [NotNull] string[] pathArray
            )
        {
            Code.NotNull(text, "text");
            Code.NotNull(extension, "extension");
            Code.NotNull(pathArray, "pathArray");

            if (!text.Contains(InclusionStart))
            {
                return text;
            }

            if (pathArray.Length == 0)
            {
                throw new IrbisException();
            }

            StringBuilder result = new StringBuilder(text.Length * 2);
            TextNavigator navigator = new TextNavigator(text);

            while (!navigator.IsEOF)
            {
                string prefix = navigator.ReadUntil(InclusionStart);
                result.Append(prefix);
                if (navigator.ReadChar() != InclusionStart)
                {
                    break;
                }
                string fileName = navigator.ReadUntil(InclusionEnd);
                if (string.IsNullOrEmpty(fileName)
                    || navigator.ReadChar() != InclusionEnd)
                {
                    break;
                }

                string fullPath = FindFileOnPath
                    (
                        fileName,
                        extension,
                        pathArray
                    );
                string fileContent = FileUtility.ReadAllText
                    (
                        fullPath,
                        IrbisEncoding.Ansi
                    );
                fileContent = ExpandInclusion
                    (
                        fileContent,
                        extension,
                        pathArray
                    );
                result.Append(fileContent);
            }

            string remaining = navigator.GetRemainingText();
            result.Append(remaining);

            return result.ToString();
        }

        /// <summary>
        /// Find file on path.
        /// </summary>
        [NotNull]
        public static string FindFileOnPath
            (
                [NotNull] string fileName,
                [NotNull] string extension,
                [NotNull] string[] pathArray
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(extension, "extension");
            Code.NotNull(pathArray, "pathArray");

            if (!fileName.Contains('.'))
            {
                if (!extension.StartsWith("."))
                {
                    fileName += '.';
                }
                fileName += extension;
            }

            foreach (string path in pathArray)
            {
                string fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }

            throw new IrbisException();
        }

        /// <summary>
        /// Get server version.
        /// </summary>
        [NotNull]
        public static IrbisVersion GetServerVersion()
        {
            IrbisVersion result = new IrbisVersion
            {
                Version = "64.2012.1",
                Organization = "Open source version",
                MaxClients = int.MaxValue
            };

            return result;
        }

        #endregion
    }
}
