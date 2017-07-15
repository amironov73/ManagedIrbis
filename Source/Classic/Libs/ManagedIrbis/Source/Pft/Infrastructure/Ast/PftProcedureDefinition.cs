// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftProcedureDefinition.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

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
    public sealed class PftProcedureDefinition
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Procedure itself.
        /// </summary>
        [CanBeNull]
        public PftProcedure Procedure { get; set; }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {
                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    if (!ReferenceEquals(Procedure, null))
                    {
                        nodes.AddRange(Procedure.Body);
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

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax
        {
            get { return true; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftProcedureDefinition()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftProcedureDefinition
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

        #endregion

        #region Public methods

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            PftProcedureDefinition result
                = (PftProcedureDefinition) base.Clone();

            if (!ReferenceEquals(Procedure, null))
            {
                result.Procedure = (PftProcedure) Procedure.Clone();
            }

            return result;
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            bool flag = reader.ReadBoolean();
            if (flag)
            {
                Procedure = new PftProcedure();
                Procedure.Deserialize(reader);
            }
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            // Do something?

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = "Procedure"
            };

            if (!ReferenceEquals(Procedure, null))
            {
                result.Value = Procedure.Name;

                PftNodeInfo body = new PftNodeInfo
                {
                    Name = "Body"
                };
                result.Children.Add(body);

                foreach (PftNode node in Procedure.Body)
                {
                    body.Children.Add(node.GetNodeInfo());
                }
            }

            return result;
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            if (ReferenceEquals(Procedure, null))
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                Procedure.Serialize(writer);
            }
        }

        #endregion
    }
}
