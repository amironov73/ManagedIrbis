// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FbDescription.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using AM;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.FictionBook
{
    /// <summary>
    /// Заголовок, описывающий документ FictionBook.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FbDescription
    {
        #region Properties

        /// <summary>
        /// Данные о книге.
        /// </summary>
        [XmlElement("title-info")]
        public FbTitle Title { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Title.ToVisibleString();
        }

        #endregion
    }
}
