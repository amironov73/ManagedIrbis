// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SafeSaveStream.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.IO
{
    /// <summary>
    /// Не перезаписывает имеющийся файл в случае неудачи.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SafeSaveStream
        : FileStream
    {
        #region Properties

        /// <summary>
        /// Original filename.
        /// </summary>
        [NotNull]
        public string OriginalFileName { get; private set; }

        /// <summary>
        /// Temporary filename.
        /// </summary>
        [NotNull]
        public string TemporaryFileName { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SafeSaveStream
            (
                [NotNull] string fileName
            )
            : base
                (
                    _GetTemporaryFileName(fileName),
                    FileMode.CreateNew,
                    FileAccess.Write
                )
        {
            OriginalFileName = fileName;
            TemporaryFileName = _GetTemporaryFileName(fileName);
        }

        #endregion

        #region Private members

        private static string _GetTemporaryFileName
            (
                [NotNull] string originalFileName
            )
        {
            Code.NotNullNorEmpty(originalFileName, "originalFileName");

            string extension = Path.GetExtension(originalFileName);
            string fileName 
                = Path.GetFileNameWithoutExtension(originalFileName);
            string directory = Path.GetDirectoryName(originalFileName);
            string result = fileName + "_temp";
            if (!string.IsNullOrEmpty(extension))
            {
                result = Path.Combine(result, extension);
            }
            if (!string.IsNullOrEmpty(directory))
            {
                result = Path.Combine(directory, result);
            }

            return result;
        }

        #endregion

        #region Public methods

        #endregion

        #region FileStream members

        /// <inheritdoc cref="Stream.Dispose(bool)"/>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (File.Exists(OriginalFileName))
            {
                File.Delete(OriginalFileName);
            }
            File.Move(TemporaryFileName, OriginalFileName);
        }

        #endregion

        #region Object members

        #endregion
    }
}
