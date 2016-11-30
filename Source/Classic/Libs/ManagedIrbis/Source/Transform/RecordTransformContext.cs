// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordTransformContext.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Transform
{
    /// <summary>
    /// Context of <see cref="MarcRecord"/> transformation.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RecordTransformContext
    {
        #region Properties

        /// <summary>
        /// Source record.
        /// </summary>
        [NotNull]
        public MarcRecord Source { get { return _source; } }

        /// <summary>
        /// Target record.
        /// </summary>
        public MarcRecord Target { get { return _target; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordTransformContext
            (
                [NotNull] MarcRecord source
            )
        {
            Code.NotNull(source, "source");

            _source = source;
            _target = new MarcRecord();
        }

        #endregion

        #region Private member

        private MarcRecord _source;

        private MarcRecord _target;

        #endregion

        #region Public methods

        #endregion
    }
}
