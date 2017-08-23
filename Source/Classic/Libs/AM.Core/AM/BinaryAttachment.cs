// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BinaryAttachment.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable VirtualMemberCallInConstructor

namespace AM
{
    /// <summary>
    /// Binary attachment.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
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
            Code.NotNullNorEmpty(name, "name");
            Code.NotNull(content, "content");

            Name = name;
            Content = content;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            string result = string.Format
                (
                    "{0}: {1} bytes",
                    Name,
                    Content.Length
                );

            return result;
        }

        #endregion
    }
}
