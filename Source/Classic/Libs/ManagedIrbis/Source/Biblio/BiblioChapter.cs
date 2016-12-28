// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BiblioChapter.cs -- 
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
    public class BiblioChapter
    {
        #region Properties

        /// <summary>
        /// Children chapters.
        /// </summary>
        [NotNull]
        [JsonProperty("children")]
        public NonNullCollection<BiblioChapter> Children { get; private set; }

        /// <summary>
        /// Gathered records.
        /// </summary>
        [NotNull]
        [JsonProperty("records")]
        public NonNullCollection<MarcRecord> Records { get; private set; }

            /// <summary>
        /// Title of the chapter.
        /// </summary>
        [NotNull]
        [JsonProperty("title")]
        public string Title { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// 
        /// </summary>
        public BiblioChapter()
        {
            Children = new NonNullCollection<BiblioChapter>();
            Records = new NonNullCollection<MarcRecord>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Gather records.
        /// </summary>
        public virtual void GatherRecords
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");
        }

        #endregion

        #region Object members

        #endregion
    }
}
