// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisResource.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Linq;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Irbis resource.
    /// </summary>
    [PublicAPI]
    public sealed class IrbisResource<T>
    {
        #region Properties

        /// <summary>
        /// Resource name.
        /// </summary>
        [NotNull]
        public string Name { get; private set; }

        /// <summary>
        /// Resource content.
        /// </summary>
        [CanBeNull]
        public T Content { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisResource
            (
                [NotNull] string name,
                [CanBeNull] T content
            )
        {
            Code.NotNullNorEmpty(name, "name");

            Name = name;
            Content = content;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "{0}: {1}",
                    Name,
                    Content.ToVisibleString()
                );
        }

        #endregion
    }
}
