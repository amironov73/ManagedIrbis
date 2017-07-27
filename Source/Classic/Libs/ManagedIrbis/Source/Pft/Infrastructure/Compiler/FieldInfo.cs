// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Ast;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    internal sealed class FieldInfo
    {
        #region Properties

        [NotNull]
        public PftField Field { get; private set; }

        [NotNull]
        public FieldSpecification Specification { get; private set; }

        [NotNull]
        public string Text { get; private set; }

        public int Id { get; private set; }

        [NotNull]
        public string Reference { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldInfo
            (
                [NotNull] PftField field,
                int id
            )
        {
            Code.NotNull(field, "field");

            Field = field;
            Specification = field.ToSpecification();
            Text = Specification.ToString();
            Id = id;
            Reference = "Field" + Id;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Text;
        }

        #endregion
    }
}
