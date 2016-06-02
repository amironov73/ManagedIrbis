/* IrbisUpperCaseTable.cs -- table for uppercase character conversion
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Table for uppercase character conversion.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    public sealed class IrbisUpperCaseTable
    {
        #region Constants

        /// <summary>
        /// Имя файла таблицы по умолчанию.
        /// </summary>
        public const string FileName = "ISISUCW.TAB";

        #endregion

        #region Properties
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisUpperCaseTable
            (
                Encoding encoding, 
                byte[] firstTable, 
                byte[] secondTable
            )
        {
            _encoding = encoding;
            _firstTable = firstTable;
            _secondTable = secondTable;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public IrbisUpperCaseTable
            (
                [NotNull] ManagedClient64 client,
                [NotNull] string fileName
            )
        {
            Code.NotNull(client, "client");
            Code.NotNullNorEmpty(fileName, "fileName");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="client">The client.</param>
        public IrbisUpperCaseTable
            (
                [NotNull] ManagedClient64 client
            )
            : this ( client, FileName )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private members

        private readonly Encoding _encoding;

        private readonly byte[] _firstTable;

        private readonly byte[] _secondTable;

        #endregion

        #region Public methods

        /// <summary>
        /// Converts specified character to uppercase.
        /// </summary>
        public char ToUpper
            (
                char c
            )
        {
            return c;
        }

        /// <summary>
        /// Converts spefified string to uppercase
        /// </summary>
        [NotNull]
        public string ToUpper
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            return text;
        }

        #endregion
    }
}
