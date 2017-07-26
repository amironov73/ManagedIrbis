// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftRef.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftRef
        : PftNode
    {
        #region Properties

        /// <summary>
        /// MFN.
        /// </summary>
        [CanBeNull]
        public PftNumeric Mfn { get; set; }

        /// <summary>
        /// Format.
        /// </summary>
        [NotNull]
        public PftNodeCollection Format { get; private set; }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {

                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    if (!ReferenceEquals(Mfn, null))
                    {
                        nodes.Add(Mfn);
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
        public PftRef()
        {
            Format = new PftNodeCollection(this);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftRef
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Ref);

            Format = new PftNodeCollection(this);
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
            PftRef result = (PftRef)base.Clone();

            if (!ReferenceEquals(Mfn, null))
            {
                result.Mfn = (PftNumeric)Mfn.Clone();
            }

            result.Format = Format.CloneNodes(result).ThrowIfNull();

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

            Mfn = (PftNumeric) PftSerializer.DeserializeNullable(reader);
            PftSerializer.Deserialize(reader, Format);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (!ReferenceEquals(Mfn, null))
            {
                context.Evaluate(Mfn);
                int mfn = (int)Mfn.Value;
                MarcRecord record = context.Provider.ReadRecord(mfn);
                if (!ReferenceEquals(record, null))
                {
                    using (new PftContextSaver(context, true))
                    {
                        context.Record = record;
                        context.Execute(Format);
                    }
                }
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = SimplifyTypeName(GetType().Name)
            };

            if (!ReferenceEquals(Mfn, null))
            {
                PftNodeInfo mfnInfo = new PftNodeInfo
                {
                    Node=Mfn,
                    Name = "Mfn"
                };
                result.Children.Add(mfnInfo);
                mfnInfo.Children.Add(Mfn.GetNodeInfo());
            }

            PftNodeInfo formatInfo = new PftNodeInfo
            {
                Name="Format"
            };
            foreach (PftNode node in Format)
            {
                formatInfo.Children.Add(node.GetNodeInfo());
            }
            result.Children.Add(formatInfo);

            return result;
        }

        /// <inheritdoc cref="PftNode.Optimize" />
        public override PftNode Optimize()
        {
            if (!ReferenceEquals(Mfn, null))
            {
                Mfn = (PftNumeric) Mfn.Optimize();
            }
            Format.Optimize();

            if (Format.Count == 0)
            {
                return null;
            }

            return this;
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer
                .SingleSpace()
                .Write("ref(");
            if (!ReferenceEquals(Mfn, null))
            {
                Mfn.PrettyPrint(printer);
            }
            printer.Write(", ")
                .WriteNodes(Format)
                .Write(')');
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            PftSerializer.SerializeNullable(writer, Mfn);
            PftSerializer.Serialize(writer, Format);
        }

        #endregion
    }
}
