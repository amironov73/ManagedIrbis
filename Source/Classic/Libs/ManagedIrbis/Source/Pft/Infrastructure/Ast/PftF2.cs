// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftF2.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM.Collections;
using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftF2
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Format.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftNode> Format { get; private set; }

        /// <summary>
        /// Number.
        /// </summary>
        [CanBeNull]
        public PftNumeric Number { get; set; }

        /// <inheritdoc />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {

                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    if (!ReferenceEquals(Number, null))
                    {
                        nodes.Add(Number);
                    }
                    nodes.AddRange(Format);
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }
            protected set
            {
                // Nothing to do here
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftF2()
        {
            Format = new NonNullCollection<PftNode>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftF2
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.F2);

            Format = new NonNullCollection<PftNode>();
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

        #endregion

        #region Public methods

        /// <summary>
        /// Format the number according specified format.
        /// </summary>
        public static void FormatNumber
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                double number,
                [CanBeNull] string format
            )
        {
            Code.NotNull(context, "context");

            string output;

            if (string.IsNullOrEmpty(format))
            {
                output = number.ToString
                    (
                        CultureInfo.InvariantCulture
                    );
            }
            else
            {
                output = number.ToString
                    (
                        format,
                        CultureInfo.InvariantCulture
                    );
            }

            context.Write(node, output);
        }

        #endregion

        #region PftNode members

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (!ReferenceEquals(Number, null))
            {
                Number.Execute(context);
                double number = Number.Value;
                string format = context.Evaluate(Format);

                FormatNumber
                    (
                        context,
                        this,
                        number,
                        format
                    );
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc/>
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = "F2"
            };

            if (!ReferenceEquals(Number, null))
            {
                result.Children.Add(Number.GetNodeInfo());
            }

            if (Format.Count != 0)
            {
                PftNodeInfo format = new PftNodeInfo
                {
                    Name = "Format"
                };
                result.Children.Add(format);
                foreach (PftNode node in Format)
                {
                    format.Children.Add(node.GetNodeInfo());
                }
            }

            return result;
        }

        #endregion
    }
}
