// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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
using AM.Text;
using AM.Text.Output;
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

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNodeInfo
            (
                [CanBeNull] PftNode node
            )
            : this()
        {
            Node = node;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Dump the node info (include children).
        /// </summary>
        public static void Dump
            (
                [NotNull] AbstractOutput output,
                [NotNull] PftNodeInfo nodeInfo,
                int level
            )
        {
            Code.NotNull(output, "output");
            Code.NotNull(nodeInfo, "nodeInfo");

            output.Write(new string(' ', level));
            output.Write(nodeInfo.ToString());
            output.WriteLine(string.Empty);
            foreach (PftNodeInfo child in nodeInfo.Children)
            {
                Dump(output, child, level+1);
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
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
