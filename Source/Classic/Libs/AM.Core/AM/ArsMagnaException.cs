// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ArsMagnaException.cs -- file manipulation routines
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public class ArsMagnaException
        : Exception,
        IAttachmentContainer
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ArsMagnaException()
        {
            Attachments = new List<BinaryAttachment>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ArsMagnaException
            (
                string message
            )
            : base(message)
        {
            Attachments = new List<BinaryAttachment>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ArsMagnaException
            (
                string message,
                Exception innerException
            )
            : base(message, innerException)
        {
            Attachments = new List<BinaryAttachment>();
        }

        #endregion

        #region Private members

        /// <summary>
        /// List of attachments.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        protected List<BinaryAttachment> Attachments
        {
            get; private set;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Attach some binary data.
        /// </summary>
        [NotNull]
        public ArsMagnaException Attach
            (
                [NotNull] BinaryAttachment attachment
            )
        {
            Code.NotNull(attachment, "attachment");

            Attachments.Add(attachment);

            return this;
        }

        #endregion

        #region IAttachmentContainer members

        /// <inheritdoc cref="IAttachmentContainer.ListAttachments" />
        public BinaryAttachment[] ListAttachments()
        {
            return Attachments.ToArray();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="Exception.ToString" />
        public override string ToString()
        {
            if (Attachments.Count == 0)
            {
                return base.ToString();
            }

            StringBuilder result = new StringBuilder
                (
                    base.ToString()
                );
            result.AppendLine();

            foreach (BinaryAttachment attachment in Attachments)
            {
                result.AppendLine();
                result.AppendLine("Attachment: " + attachment.Name);

                byte[] content = attachment.Content;
                int length = content.Length;
                int offset = 0;

                while (offset + 16 < length)
                {
                    result.AppendFormat("{0:X4}:", offset);
                    for (int i = 0; i < 16; i++)
                    {
                        result.AppendFormat
                            (
                                " {0:X2}",
                                content[offset + i]
                            );
                    }

                    offset += 16;
                    result.AppendLine();
                }
                if (offset != length)
                {
                    result.AppendFormat("{0:X4}:", offset);

                    while (offset < length)
                    {
                        result.AppendFormat
                            (
                                " {0:X2}",
                                content[offset]
                            );
                        offset++;
                    }
                    result.AppendLine();
                }
            }

            return result.ToString();
        }

        #endregion
    }
}
