// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftHave.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

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
    public sealed class PftHave
        : PftCondition
    {
        #region Properties

        /// <summary>
        /// Variable reference.
        /// </summary>
        [CanBeNull]
        public PftVariableReference Variable { get; set; }

        /// <summary>
        /// Function or procedure name.
        /// </summary>
        [CanBeNull]
        public string Identifier { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftHave()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftHave
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Have);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region ICloneable members

        /// <inheritdoc />
        public override object Clone()
        {
            PftHave result = (PftHave) base.Clone();

            if (!ReferenceEquals(Variable, null))
            {
                result.Variable = (PftVariableReference) Variable.Clone();
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

            Value = false;

            string name;

            if (!ReferenceEquals(Variable, null))
            {
                name = Variable.Name;
                if (!string.IsNullOrEmpty(name))
                {
                    Value = !ReferenceEquals
                        (
                            context.Variables.GetExistingVariable(name),
                            null
                        );
                }
            }
            else
            {
                name = Identifier;
                if (!string.IsNullOrEmpty(name))
                {
                    Value = context.Functions.HaveFunction(name)
                        || context.Procedures.HaveProcedure(name);
                }
            }

            OnAfterExecution(context);
        }

        #endregion
    }
}
