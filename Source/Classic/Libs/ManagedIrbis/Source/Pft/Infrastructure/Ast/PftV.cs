/* PftV.cs -- ссылка на поле
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Text;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Ссылка на поле.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftV
        : PftField
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftV()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftV
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            FieldSpecification2 specification = new FieldSpecification2();
            if (!specification.Parse(text))
            {
                throw new PftSyntaxException();
            }

            Apply(specification);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftV
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.V);

            FieldSpecification2 specification = ((FieldSpecification2)token.UserData)
                .ThrowIfNull("token.UserData");
            Apply(specification);
        }

        #endregion

        #region Private members

        private int _count;

        private void _Execute
            (
                PftContext context
            )
        {
            try
            {
                context.CurrentField = this;

                context.Execute(LeftHand);

                string value = GetValue(context);
                if (!string.IsNullOrEmpty(value))
                {
                    if (Indent != 0
                        && IsFirstRepeat(context))
                    {
                        value = new string(' ', Indent) + value;
                    }

                    context.Write(this, value);
                }
                if (HaveRepeat(context))
                {
                    context.OutputFlag = true;
                }

                context.Execute(RightHand);
            }
            finally
            {
                context.CurrentField = null;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Число повторений _поля_ в записи.
        /// </summary>
        public int GetCount
            (
                [NotNull] PftContext context
            )
        {
            Code.NotNull(context, "context");

            MarcRecord record = context.Record;
            if (record == null
                || string.IsNullOrEmpty(Tag))
            {
                return 0;
            }

            int result = record.Fields.GetField(Tag).Length;
            //if (IndexTo != 0
            //    && IndexTo <= result)
            //{
            //    result = IndexTo - 1;
            //}

            return result;
        }

        #endregion

        #region PftField members

        /// <inheritdoc/>
        public override bool IsLastRepeat
            (
                PftContext context
            )
        {
            return context.Index >= (_count - 1);
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

            if (context.CurrentField != null)
            {
                throw new PftSemanticException("nested field");
            }

            if (context.CurrentGroup != null)
            {
                if (IsFirstRepeat(context))
                {
                    _count = GetCount(context);
                }

                _Execute(context);
            }
            else
            {
                _count = GetCount(context);

                context.DoRepeatableAction(_Execute);
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc />
        public override void Write
            (
                StreamWriter writer
            )
        {
            foreach (PftNode node in LeftHand)
            {
                node.Write(writer);
            }

            writer.Write(ToString());

            foreach (PftNode node in RightHand)
            {
                node.Write(writer);
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return ToSpecification().ToString();
        }

        #endregion
    }
}
