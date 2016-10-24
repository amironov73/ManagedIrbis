/* UniforE.cs --
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
using AM.Collections;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Diagnostics
{
    /// <summary>
    /// Node info.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftNodeInfo
    {
        #region Properties

        /// <summary>
        /// Children.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftNodeInfo> Children { get; private set; }

            /// <summary>
        /// Name.
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        /// <summary>
        /// Node itself.
        /// </summary>
        [CanBeNull]
        public PftNode Node { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        [CanBeNull]
        public string Value { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNodeInfo()
        {
            Children = new NonNullCollection<PftNodeInfo>();
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Value))
            {
                return Name.ToVisibleString();
            }

            return string.Format
                (
                    "{0}: {1}",
                    Name.ToVisibleString(),
                    Value
                );
        }

        #endregion
    }
}
