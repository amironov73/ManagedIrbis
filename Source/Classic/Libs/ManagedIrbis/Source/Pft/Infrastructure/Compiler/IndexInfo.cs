// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IndexInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class IndexInfo
    {
        #region Properties

        /// <summary>
        /// Specification.
        /// </summary>
        public IndexSpecification Specification { get; private set; }

        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Reference.
        /// </summary>
        [NotNull]
        public string Reference { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IndexInfo
            (
                IndexSpecification specification,
                int id
            )
        {
            Specification = specification;
            Id = id;
            Reference = "Index" + Id.ToInvariantString();
        }

        #endregion

        #region Object members

        private bool Equals
            (
                IndexInfo other
        )
        {
            return Id == other.Id;
        }

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals
            (
                object obj
            )
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            IndexInfo info = obj as IndexInfo;

            return !ReferenceEquals(info, null)
                   && Equals(info);
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            return Id;
        }

        #endregion
    }
}
