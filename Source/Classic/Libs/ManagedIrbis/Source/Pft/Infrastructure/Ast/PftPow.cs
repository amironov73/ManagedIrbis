// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftPow.cs --
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
    public sealed class PftPow
        : PftNumeric
    {
        #region Properties

        /// <summary>
        /// X.
        /// </summary>
        [CanBeNull]
        public PftNumeric X { get; set; }

        /// <summary>
        /// Y.
        /// </summary>
        [CanBeNull]
        public PftNumeric Y { get; set; }

        /// <inheritdoc />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {
                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    if (!ReferenceEquals(X, null))
                    {
                        nodes.Add(X);
                    }
                    if (!ReferenceEquals(Y, null))
                    {
                        nodes.Add(Y);
                    }
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
        public PftPow()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftPow
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Pow);
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

        #endregion

        #region Public methods

        #endregion

        #region ICloneable members

        /// <inheritdoc />
        public override object Clone()
        {
            PftPow result = (PftPow)base.Clone();

            if (!ReferenceEquals(X, null))
            {
                result.X = (PftNumeric) X.Clone();
            }

            if (!ReferenceEquals(Y, null))
            {
                result.Y = (PftNumeric)Y.Clone();
            }

            return result;
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

            if (!ReferenceEquals(X, null)
                && !ReferenceEquals(Y, null))
            {
                X.Execute(context);
                Y.Execute(context);
                Value = Math.Pow(X.Value, Y.Value);
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc/>
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = SimplifyTypeName(GetType().Name)
            };

            if (!ReferenceEquals(X, null))
            {
                PftNodeInfo x = new PftNodeInfo
                {
                    Node = X,
                    Name = "X"
                };
                result.Children.Add(x);
                x.Children.Add(X.GetNodeInfo());
            }

            if (!ReferenceEquals(Y, null))
            {
                PftNodeInfo y = new PftNodeInfo
                {
                    Node = Y,
                    Name = "Y"
                };
                result.Children.Add(y);
                y.Children.Add(Y.GetNodeInfo());
            }

            return result;
        }

        #endregion
    }
}
