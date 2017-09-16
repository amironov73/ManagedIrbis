// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftN.cs -- fake field reference
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Fake field reference.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftD
        : PftField
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftD()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftD
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftD
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            FieldSpecification specification = new FieldSpecification();
            if (!specification.Parse(text))
            {
                Log.Error
                    (
                        "PftD::Constructor: "
                        + "syntax error at: "
                        + text.ToVisibleString()
                    );

                throw new PftSyntaxException();
            }

            Apply(specification);
        }

        #endregion

        #region Private members

        private void _Execute
            (
                PftContext context
            )
        {
            try
            {
                context.CurrentField = this;

                string value = GetValue(context);
                if (!string.IsNullOrEmpty(value))
                {
                    context.Execute(LeftHand);
                }
            }
            finally
            {
                context.CurrentField = null;
            }
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftField.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (!ReferenceEquals(context.CurrentField, null))
            {
                Log.Error
                    (
                        "PftD::Execute: "
                        + "nested field detected"
                    );

                throw new PftSemanticException("nested field");
            }

            if (LeftHand.Count == 0)
            {
                Log.Warn
                    (
                        "PftD::Execute: "
                        + "no left hand nodes"
                    );
            }

            if (!ReferenceEquals(context.CurrentGroup, null))
            {
                _Execute(context);
            }
            else
            {
                context.DoRepeatableAction(_Execute, 1);
            }

            OnAfterExecution(context);
        }

        #endregion
    }
}
