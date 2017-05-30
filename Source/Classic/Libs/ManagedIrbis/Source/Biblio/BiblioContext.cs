// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BiblioContext.cs -- 
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
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class BiblioContext
    {
        #region Properties

        /// <summary>
        /// Document.
        /// </summary>
        [NotNull]
        public BiblioDocument Document { get; private set; }

        /// <summary>
        /// Provider.
        /// </summary>
        [NotNull]
        public IrbisProvider Provider { get; private set; }

        /// <summary>
        /// All the gathered records.
        /// </summary>
        [NotNull]
        public RecordCollection Records { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BiblioContext
            (
                [NotNull] BiblioDocument document,
                [NotNull] IrbisProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            Document = document;
            Provider = provider;
            Records = new RecordCollection();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Find the record with specified MFN.
        /// </summary>
        [CanBeNull]
        public MarcRecord FindRecord
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            MarcRecord result = Records
                .FirstOrDefault(record => record.Mfn == mfn);

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
