// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GblFieldCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Gbl.Infrastructure.Ast
{
    /// <summary>
    /// Команда, принимающая спецификацию поля (возможно, с подполем)
    /// и повторения.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class GblFieldCommand
        : GblNode
    {
        #region Properties

        /// <summary>
        /// Field tag.
        /// </summary>
        public int FieldTag { get; set; }

        /// <summary>
        /// Subfield code.
        /// </summary>
        public char SubfieldCode { get; set; }

        /// <summary>
        /// Repeat.
        /// </summary>
        public RepeatSpecification Repeat { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region GblNode members

        /// <inheritdoc cref="GblNode.Initialize" />
        public override void Initialize
            (
                GblContext context,
                GblParser parser
            )
        {
            base.Initialize(context, parser);

            // Parse the field
            string fieldSpecification = Parameter1;
            if (string.IsNullOrEmpty(fieldSpecification))
            {
                throw new IrbisException();
            }

            if (fieldSpecification.Contains("^"))
            {
                string[] parts = StringUtility.SplitString
                    (
                        fieldSpecification,
                        CommonSeparators.Circumflex,
                        2
                    );
                if (parts.Length != 2)
                {
                    throw new IrbisException();
                }

                FieldTag = NumericUtility.ParseInt32(parts[0]);
                if (parts[1].Length != 1)
                {
                    throw new IrbisException();
                }

                SubfieldCode = parts[1].FirstChar();
            }
            else
            {
                FieldTag = NumericUtility.ParseInt32(fieldSpecification);
            }

            string parameter2 = Parameter2.ThrowIfNull();
            Repeat = RepeatSpecification.Parse(parameter2);
        }

        /// <summary>
        /// Execute the node.
        /// </summary>
        public override void Execute
            (
                GblContext context
            )
        {
            Code.NotNull(context, "context");

            OnBeforeExecution(context);

            // Nothing to do here

            OnAfterExecution(context);
        }

        #endregion
    }
}
