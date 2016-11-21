/* PftRepeatableLiteral.cs -- повторяющийся литерал
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Определяет текст, который будет выведен только
    /// в случае присутствия в записи соответствующего
    /// ему поля или подполя. Однако, если поле
    /// повторяющееся, литерал будет выводиться
    /// для каждого экземпляра поля/подполя.
    /// Повторяющиеся литералы заключаются
    /// в вертикальные черты (|), например, |Автор: |.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftRepeatableLiteral
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Prefix or postfix?
        /// </summary>
        public bool IsPrefix { get; set; }

        /// <summary>
        /// Plus?
        /// </summary>
        public bool Plus { get; set; }

        /// <inheritdoc/>
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = PftUtility.PrepareText(value); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftRepeatableLiteral()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftRepeatableLiteral
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            Text = text;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftRepeatableLiteral
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.RepeatableLiteral);

            try
            {
                Text = token.Text.ThrowIfNull("token.Text");
            }
            catch (Exception exception)
            {
                throw new PftSyntaxException(token, exception);
            }
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

            PftField field = context.CurrentField;

            if (!ReferenceEquals(field, null))
            {
                bool flag = field.HaveRepeat(context);

                if (flag && Plus)
                {
                    flag = IsPrefix
                        ? !field.IsFirstRepeat(context)
                        : !field.IsLastRepeat(context);
                }

                if (flag)
                {
                    string text = Text;
                    if (context.UpperMode
                        && !ReferenceEquals(text, null))
                    {
                        text = text.ToUpper();
                    }
                    context.Write(this, text);
                }
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc />
        public override void Write
            (
                StreamWriter writer
            )
        {
            writer.Write('|');
            writer.Write(Text);
            writer.Write('|');
        }


        #endregion
    }
}
