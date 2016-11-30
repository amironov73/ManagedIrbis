// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TableDefinition.cs -- parameters for table command
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Signature for Table command.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class TableDefinition
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string DatabaseName { get; set; }

        /// <summary>
        /// Table name.
        /// </summary>
        [CanBeNull]
        public string Table { get; set; }

        /// <summary>
        /// Table headers.
        /// </summary>
        // ReSharper disable ConvertToAutoProperty
        public List<string> Headers
        {
            get { return _headers; }
        }
        // ReSharper restore ConvertToAutoProperty

        /// <summary>
        /// Mode.
        /// </summary>
        public string Mod { get; set; }

        /// <summary>
        /// Search query.
        /// </summary>
        public string SearchQuery { get; set; }

        /// <summary>
        /// Minimal MFN.
        /// </summary>
        public int MinMfn { get; set; }

        /// <summary>
        /// Maximal MFN.
        /// </summary>
        public int MaxMfn { get; set; }

        /// <summary>
        /// Optional sequential query.
        /// </summary>
        public string SequentialQuery { get; set; }

        /// <summary>
        /// List of MFN.
        /// </summary>
        // ReSharper disable ConvertToAutoProperty
        public List<int> MfnList
        {
            get { return _mfnList; }
        }
        // ReSharper restore ConvertToAutoProperty

        #endregion

        #region Construction

        #endregion

        #region Private members

        private readonly List<string> _headers
            = new List<string>();

        private readonly List<int> _mfnList
            = new List<int>();

        #endregion

        #region Public methods

        #endregion
    }
}
