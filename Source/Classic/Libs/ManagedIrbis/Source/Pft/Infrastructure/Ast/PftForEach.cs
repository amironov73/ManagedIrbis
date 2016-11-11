/* PftForEach.cs --
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
    public sealed class PftForEach
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Variable reference.
        /// </summary>
        [CanBeNull]
        public PftVariableReference Variable { get; set; }

        /// <summary>
        /// Sequence.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftNode> Sequence { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftForEach()
        {
            Sequence = new NonNullCollection<PftNode>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftForEach
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Sequence = new NonNullCollection<PftNode>();
        }

        #endregion

        #region Private members

        private string[] GetSequence
            (
                [NotNull] PftContext context
            )
        {
            List<string> result = new List<string>();

            foreach (PftNode node in Sequence)
            {
                string text = context.Evaluate(node);
                if (!string.IsNullOrEmpty(text))
                {
                    string[] lines = text.SplitLines()
                        .NonEmptyLines()
                        .ToArray();
                    result.AddRange(lines);
                }
            }

            return result.ToArray();
        }

        #endregion

        #region Public methods

        #endregion

        #region PftNode members

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            PftVariableReference variable = Variable
                .ThrowIfNull("variable");
            string name = variable.Name
                .ThrowIfNull("variable.Name");

            string[] items = GetSequence(context);
            foreach (string item in items)
            {
                context.Variables.SetVariable(name, item);

                context.Execute(Children);
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc/>
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Name = "ForEach"
            };

            if (!ReferenceEquals(Variable, null))
            {
                result.Children.Add(Variable.GetNodeInfo());
            }

            PftNodeInfo sequence = new PftNodeInfo
            {
                Name = "Sequence"
            };
            result.Children.Add(sequence);
            foreach (PftNode node in Sequence)
            {
                sequence.Children.Add(node.GetNodeInfo());
            }

            PftNodeInfo body = new PftNodeInfo
            {
                Name = "Body"
            };
            result.Children.Add(body);
            foreach (PftNode node in Children)
            {
                body.Children.Add(node.GetNodeInfo());
            }

            return result;
        }

        #endregion
    }
}
