/* MxRecord.cs -- 
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

#endregion

namespace ManagedIrbis.Mx
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class MxRecord
    {
        #region Properties

        /// <summary>
        /// Sequential number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

        /// <summary>
        /// MFN.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Record index (field 903, if any).
        /// </summary>
        [CanBeNull]
        public string Index { get; set; }

        /// <summary>
        /// Record itself.
        /// </summary>
        [CanBeNull]
        public MarcRecord Record { get; set; }

        /// <summary>
        /// Bibliographic description.
        /// </summary>
        [CanBeNull]
        public string Description { get; set; }

        /// <summary>
        /// For sorting.
        /// </summary>
        [CanBeNull]
        public string Order { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        public object UserData { get; set; }

        #endregion
    }
}
