// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftProcedure.cs --
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

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Procedure.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftProcedure
        : ICloneable
    {
        #region Properties

        /// <summary>
        /// Procedure body.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftNode> Body { get; set; }

        /// <summary>
        /// Procedure name.
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftProcedure()
        {
            Body = new NonNullCollection<PftNode>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Execute the procedure.
        /// </summary>
        public void Execute
            (
                [NotNull] PftContext context,
                [CanBeNull] string argument
            )
        {
            Code.NotNull(context, "context");

            PftContext nested = context.Push();
            try
            {
                nested.Output = context.Output;
                PftVariableManager variables
                    = new PftVariableManager(context.Variables);
                variables.SetVariable("arg", argument);
                nested.SetVariables(variables);
                nested.Execute(Body);
            }
            finally
            {
                context.Pop();
            }
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc />
        public object Clone()
        {
            PftProcedure result = (PftProcedure) MemberwiseClone();

            result.Body = Body.CloneNodes().ThrowIfNull();

            return result;
        }

        #endregion
    }
}
