/* RecordTransformContext.cs --
 * Ars Magna project, http://arsmagna.ru
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

namespace ManagedClient.Transform
{
    /// <summary>
    /// Context of <see cref="IrbisRecord"/> transformation.
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
        public IrbisRecord Source { get { return _source; } }

        /// <summary>
        /// Target record.
        /// </summary>
        public IrbisRecord Target { get { return _target; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordTransformContext
            (
                [NotNull] IrbisRecord source
            )
        {
            Code.NotNull(source, "source");

            _source = source;
            _target = new IrbisRecord();
        }

        #endregion

        #region Private member

        private IrbisRecord _source;

        private IrbisRecord _target;

        #endregion

        #region Public methods

        #endregion
    }
}
