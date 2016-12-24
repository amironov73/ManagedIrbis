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

            // ReSharper disable VirtualMemberCallInConstructor

            Name = name;
            Content = content;

            // ReSharper restore VirtualMemberCallInConstructor
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
