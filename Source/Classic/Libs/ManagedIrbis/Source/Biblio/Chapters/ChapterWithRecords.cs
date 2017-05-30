// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ChapterWithRecords.cs -- 
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

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ChapterWithRecords
        : BiblioChapter
    {
        #region Properties

        /// <summary>
        /// Records.
        /// </summary>
        [NotNull]
        public RecordCollection Records { get; private set; }

        /// <summary>
        /// Duplicates.
        /// </summary>
        [NotNull]
        public RecordCollection Duplicates { get; private set; }

        /// <summary>
        /// Filter.
        /// </summary>
        [CanBeNull]
        [JsonProperty("filter")]
        public BiblioFilter Filter { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChapterWithRecords()
        {
            Records = new RecordCollection();
            Duplicates = new RecordCollection();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
