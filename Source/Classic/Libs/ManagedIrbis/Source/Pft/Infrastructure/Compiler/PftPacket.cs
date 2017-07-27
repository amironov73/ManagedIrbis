// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftPacket.cs -- 
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PftPacket
    {
        #region Properties

        /// <summary>
        /// Context.
        /// </summary>
        [NotNull]
        public PftContext Context { get; private set; }

        /// <summary>
        /// Current field (if any).
        /// </summary>
        [CanBeNull]
        public FieldSpecification CurrentField { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftPacket
            (
                [NotNull] PftContext context
            )
        {
            Code.NotNull(context, "context");

            Context = context;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Do the conditional literal.
        /// </summary>
        public void DoConditionalLiteral
            (
                [CanBeNull] string text,
                bool isSuffix
            )
        {
            // TODO implement
        }

        /// <summary>
        /// Do field.
        /// </summary>
        public void DoField
            (
                [NotNull] FieldSpecification field,
                [CanBeNull] Action leftHand,
                [CanBeNull] Action rightHand
            )
        {
            CurrentField = field;

            // TODO implement

            CurrentField = null;
        }

        /// <summary>
        /// Do group.
        /// </summary>
        public void DoGroup
            (
                [NotNull] Action action
            )
        {
            // TODO implement
        }

        /// <summary>
        /// Do the repeatable literal.
        /// </summary>
        public void DoRepeatableLiteral
            (
                [CanBeNull] string text,
                bool isPrefix,
                bool plus
            )
        {
            // TODO implement
        }

        /// <summary>
        /// Have the field?
        /// </summary>
        public bool HaveField
            (
                [NotNull] FieldSpecification field
            )
        {
            // TODO implement

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
