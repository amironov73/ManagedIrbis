// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisFileNotFoundException.cs -- file not found on the server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// File not found on the server.
    /// </summary>
    public sealed class IrbisFileNotFoundException
        : IrbisException
    {
        #region Properties

        /// <summary>
        /// File name.
        /// </summary>
        [NotNull]
        public string FileName { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisFileNotFoundException
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            FileName = fileName;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisFileNotFoundException
            (
                [NotNull] FileSpecification specification
            )
        {
            Code.NotNull(specification, "specification");

            FileName = specification.ToString();
        }

        #endregion

        #region MyRegion

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format("File not found: {0}", FileName);
        }

        #endregion
    }
}
