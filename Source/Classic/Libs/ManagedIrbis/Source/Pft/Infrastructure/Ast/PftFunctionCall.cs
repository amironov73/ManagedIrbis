﻿/* PftFunctionCall.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftFunctionCall
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Function name.
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFunctionCall()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFunctionCall
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            Name = name;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFunctionCall
            (
                [NotNull] PftToken token
            )
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Identifier);

            Name = token.Text;
        }

        #endregion

        #region Private members

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

            string name = Name;
            if (string.IsNullOrEmpty(name))
            {
                throw new PftSyntaxException(this);
            }

            string expression = context.Evaluate(Children);
            PftFunction function = context.Functions.FindFunction(name);
            if (!ReferenceEquals(function, null))
            {
                function(context, this, expression);
            }
            else
            {
                PftFunctionManager.ExecuteFunction
                    (
                        name,
                        context,
                        this,
                        expression
                    );
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc/>
        public override void PrintDebug
            (
                TextWriter writer,
                int level
            )
        {
            for (int i = 0; i < level; i++)
            {
                writer.Write("| ");
            }
            writer.WriteLine("Function: " + Name);

            foreach (PftNode child in Children)
            {
                child.PrintDebug(writer, level + 1);
            }
        }

        #endregion
    }
}