/* PathUtility.cs -- path manipulation routines
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.IO
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
        /// Appends trailing backslash (if none exists) to given path.
        /// </summary>
        /// <param name="path">Path to convert.</param>
        /// <returns>Converted path.</returns>
        /// <remarks>Path need NOT to be existent.</remarks>
        public static string AppendBackslash(string path)
        {
            path = ConvertSlashes(path);
            if (!path.EndsWith(_backslash))
            {
                path = path + _backslash;
            }
            return path;
        }

        /// <summary>
        /// Converts ordinary slashes to backslashes.
        /// </summary>
        /// <param name="path">Path to convert.</param>
        /// <returns>Converted path.</returns>
        /// <remarks>Path need NOT to be existent.</remarks>
        public static string ConvertSlashes(string path)
        {
            Code.NotNull(path, "path");

            return path.Replace
                (
                Path.AltDirectorySeparatorChar,
                Path.DirectorySeparatorChar
                );
        }

        /// <summary>
        /// Maps the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string MapPath
            (
                string path
            )
        {
            Code.NotNull(path, "path");

            string appDirectory
                = Path.GetDirectoryName
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
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string StripExtension
            (
                string path
            )
        {
            Code.NotNull(path, "path");

            string extension = Path.GetExtension(path);
            if (!string.IsNullOrEmpty(extension))
            {
                path = path.Substring
                    (
                        0, 
                        path.Length - extension.Length
                    );
            }

            return path;
        }

        /// <summary>
        /// Removes trailing backslash (if exists) from the path.
        /// </summary>
        /// <param name="path">Path to convert.</param>
        /// <returns>Converted path.</returns>
        /// <remarks>Path need NOT to be existent.</remarks>
        public static string StripTrailingBackslash(string path)
        {
            Code.NotNull(path, "path");

            path = ConvertSlashes(path);
            while (path.EndsWith(_backslash))
            {
                path = path.Substring
                    (
                        0, 
                        path.Length - _backslash.Length
                    );
            }
            return path;
        }

        #endregion
    }
}