// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BinaryAttachment.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

// ReSharper disable VirtualMemberCallInConstructor

namespace UnsafeAM
{
    /// <summary>
    /// Binary attachment.
    /// </summary>
    [PublicAPI]
    public class BinaryAttachment
    {
        #region Properties

        /// <summary>
        /// Name of the attachment.
        /// </summary>
        [NotNull]
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Content of the attachment.
        /// </summary>
        [NotNull]
        public virtual byte[] Content { get; protected set; }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected BinaryAttachment()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name of the attachment.</param>
        /// <param name="content">Content of the attachment.</param>
        public BinaryAttachment
            (
                [NotNull] string name,
                [NotNull] byte[] content
            )
        {
            Code.NotNullNorEmpty(name, nameof(name));
            Code.NotNull(content, nameof(content));

            Name = name;
            Content = content;
        }

        #endregion

        #region Public methods

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            string result = $"{Name}: {Content.Length} bytes";

            return result;
        }

        #endregion
    }
}
