// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PathUtility.cs -- path manipulation routines
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WIN81

#region Using directives

using System;
using System.IO;
using System.Text;

using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.IO
{
    /// <summary>
    /// Path manipulation routines.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class PathUtility
    {
        #region Private members

#if PORTABLE

        private static string _backslash = "/";

#else

        private static string _backslash
            = new string(Path.DirectorySeparatorChar, 1);

#endif

        #endregion

        #region Public methods

        /// <summary>
        /// Appends trailing backslash (if none exists)
        /// to given path.
        /// </summary>
        /// <param name="path">Path to convert.</param>
        /// <returns>Converted path.</returns>
        /// <remarks>Path need NOT to be existent.</remarks>
        [NotNull]
        public static string AppendBackslash
            (
                [NotNull] string path
            )
        {
            Code.NotNullNorEmpty(path, "path");

            string result = ConvertSlashes(path);
            if (!result.EndsWith(_backslash))
            {
                result = result + _backslash;
            }

            return result;
        }

        /// <summary>
        /// Combine strings as path.
        /// </summary>
        [NotNull]
        public static string Combine
            (
                [NotNull] params string[] elements
            )
        {
#if FW35 || WINMOBILE || PocketPC

            if (elements.Length == 0)
            {
                return string.Empty;
            }
            if (elements.Length == 1)
            {
                return elements[0];
            }
            if (elements.Length == 2)
            {
                return Path.Combine (elements[0], elements[1]);
            }

            string result = Path.Combine (elements[0], elements[1]);
            for (int i = 2; i < elements.Length; i++)
            {
                result = Path.Combine
                    (
                        result,
                        elements[i]
                    );
            }

            return result;

#else

            return Path.Combine(elements);

#endif
        }

        /// <summary>
        /// Converts ordinary slashes to backslashes.
        /// </summary>
        /// <param name="path">Path to convert.</param>
        /// <returns>Converted path.</returns>
        /// <remarks>Path need NOT to be existent.</remarks>
        [NotNull]
        public static string ConvertSlashes
            (
                [NotNull] string path
            )
        {
            Code.NotNull(path, "path");

#if PORTABLE

            string result = path.Replace
                (
                    '/',
                    '\\'
                );

#else

            string result = path.Replace
                (
                    Path.AltDirectorySeparatorChar,
                    Path.DirectorySeparatorChar
                );

#endif

            return result;
        }

        /// <summary>
        /// Get relative path.
        /// </summary>
        [NotNull]
        public static string GetRelativePath
            (
                [NotNull] string absolutePath,
                [NotNull] string baseDirectory
            )
        {
            Code.NotNullNorEmpty(absolutePath, "absolutePath");
            Code.NotNullNorEmpty(baseDirectory, "baseDirectory");

            // absolutePath = Path.GetFullPath(absolutePath);
            // baseDirectory = Path.GetFullPath(baseDirectory);

#if PORTABLE

            string mainSeparator = "\\";
            string altSeparator = "/";

#else

            string mainSeparator = char.ToString(Path.DirectorySeparatorChar);
            string altSeparator = char.ToString(Path.AltDirectorySeparatorChar);

#endif

            string[] separators =
            {
                mainSeparator,
                altSeparator
            };

            string[] absoluteParts = StringUtility.SplitString
                (
                    absolutePath,
                    separators,
                    StringSplitOptions.RemoveEmptyEntries
                );
            string[] baseParts = StringUtility.SplitString
                (
                    baseDirectory,
                    separators,
                    StringSplitOptions.RemoveEmptyEntries
                );
            int length = Math.Min
                (
                    absoluteParts.Length,
                    baseParts.Length
                );

            int offset = 0;
            for (int i = 0; i < length; i++)
            {
                if (StringUtility.CompareNoCase
                    (
                        absoluteParts[i],
                        baseParts[i]
                    ))
                {
                    offset++;
                }
                else
                {
                    break;
                }
            }

            if (0 == offset)
            {
                if (!absolutePath.StartsWith(mainSeparator)) // Linux
                {
                    throw new ArgumentException
                        (
                            "Paths do not have a common base!"
                        );
                }
            }

            var relativePath = new StringBuilder();

            for (int i = 0; i < baseParts.Length - offset; i++)
            {
                relativePath.Append("..");
                relativePath.Append(mainSeparator);
            }

            for (int i = offset; i < absoluteParts.Length - 1; i++)
            {
                relativePath.Append(absoluteParts[i]);
                relativePath.Append(mainSeparator);
            }

            relativePath.Append(absoluteParts[absoluteParts.Length - 1]);

            return relativePath.ToString();
        }

#if CLASSIC

        /// <summary>
        /// Maps the path relative to the executable name.
        /// </summary>
        [NotNull]
        public static string MapPath
            (
                [NotNull] string path
            )
        {
            Code.NotNull(path, "path");

            string appDirectory = Path.GetDirectoryName
                (
                    RuntimeUtility.ExecutableFileName
                );
            string result = string.IsNullOrEmpty(appDirectory)
                ? path
                : Path.Combine
                    (
                        appDirectory,
                        path
                    );

            return result;
        }

#endif

        /// <summary>
        /// Strips extension from given path.
        /// </summary>
        [NotNull]
        public static string StripExtension
            (
                [NotNull] string path
            )
        {
            Code.NotNull(path, "path");

            string extension = Path.GetExtension(path);
            string result = path;
            if (!string.IsNullOrEmpty(extension))
            {
                result = result.Substring
                    (
                        0,
                        result.Length - extension.Length
                    );
            }

            return result;
        }

        /// <summary>
        /// Removes trailing backslash (if exists) from the path.
        /// </summary>
        /// <param name="path">Path to convert.</param>
        /// <returns>Converted path.</returns>
        /// <remarks>Path need NOT to be existent.</remarks>
        [NotNull]
        public static string StripTrailingBackslash
            (
                [NotNull] string path
            )
        {
            Code.NotNull(path, "path");

            string result = ConvertSlashes(path);
            while (result.EndsWith(_backslash))
            {
                result = result.Substring
                    (
                        0,
                        result.Length - _backslash.Length
                    );
            }

            return result;
        }

        #endregion
    }
}

#endif

