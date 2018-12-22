// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PathUtility.cs -- path manipulation routines
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Text;

using UnsafeAM.Runtime;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.IO
{
    /// <summary>
    /// Path manipulation routines.
    /// </summary>
    [PublicAPI]
    public static class PathUtility
    {
        #region Private members

        private static string _backslash
            = new string(Path.DirectorySeparatorChar, 1);

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
            Code.NotNullNorEmpty(path, nameof(path));

            string result = ConvertSlashes(path);
            if (!result.EndsWith(_backslash))
            {
                result = result + _backslash;
            }

            return result;
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
            Code.NotNull(path, nameof(path));

            string result = path.Replace
                (
                    Path.AltDirectorySeparatorChar,
                    Path.DirectorySeparatorChar
                );

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
            Code.NotNullNorEmpty(absolutePath, nameof(absolutePath));
            Code.NotNullNorEmpty(baseDirectory, nameof(baseDirectory));

            // absolutePath = Path.GetFullPath(absolutePath);
            // baseDirectory = Path.GetFullPath(baseDirectory);

            string mainSeparator = char.ToString(Path.DirectorySeparatorChar);
            string altSeparator = char.ToString(Path.AltDirectorySeparatorChar);

            string[] separators =
            {
                mainSeparator,
                altSeparator
            };

            string[] absoluteParts = absolutePath.Split
                (
                    separators,
                    StringSplitOptions.RemoveEmptyEntries
                );
            string[] baseParts = baseDirectory.Split
                (
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

        /// <summary>
        /// Maps the path relative to the executable name.
        /// </summary>
        [NotNull]
        public static string MapPath
            (
                [NotNull] string path
            )
        {
            Code.NotNull(path, nameof(path));

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

        /// <summary>
        /// Strips extension from given path.
        /// </summary>
        [NotNull]
        public static string StripExtension
            (
                [NotNull] string path
            )
        {
            Code.NotNull(path, nameof(path));

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
            Code.NotNull(path, nameof(path));

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
