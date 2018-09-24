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
using System.Net;
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

using ManagedIrbis.Menus;

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

        /// <summary>
        /// Признак того, что client_m.mnu зашифрован.
        /// </summary>
        public static byte[] EncryptionMark =
        {
            0x49, 0x72, 0x62, 0x69, 0x73, 0x36, 0x34, 0x5F,
            0x43, 0x72, 0x79, 0x70, 0x74, 0x65, 0x64
        };

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

        /// <summary>
        /// Загрузка client_m.mnu с учетом того,
        /// что тот может быть зашифрован.
        /// </summary>
        [NotNull]
        public static UserInfo[] LoadClientList
            (
                [NotNull] string fileName,
                [NotNull] MenuFile clientIni
            )
        {
            Code.FileExists(fileName, "fileName");

            byte[] rawContent = FileUtility.ReadAllBytes(fileName);
            if (!ArrayUtility.Coincide(rawContent, 0, EncryptionMark,
                0, EncryptionMark.Length))
            {
                return UserInfo.ParseFile(fileName, clientIni);
            }

            int shift = EncryptionMark.Length;
            int length = rawContent.Length - shift;
            for (int i = 0; i < length; i++)
            {
                byte mask = i % 2 == 0 ? (byte)0xE6 : (byte)0x14;
                rawContent[i + shift] = (byte) (rawContent[i + shift] ^ mask);
            }

            string decryptedText = IrbisEncoding.Ansi.GetString(rawContent,
                shift, length);
            using (StringReader reader = new StringReader(decryptedText))
            {
                return UserInfo.ParseStream(reader, clientIni);
            }
        }

        #endregion
    }
}
