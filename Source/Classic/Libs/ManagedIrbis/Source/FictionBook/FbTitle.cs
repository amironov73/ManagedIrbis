// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FbTitle.cs --
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
    /// Данные о книге FictionBook.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FbTitle
    {
        #region Properties

        /// <summary>
        /// Автор.
        /// </summary>
        [XmlElement("author")]
        public FbAuthor[] Author { get; set; }

        /// <summary>
        /// Жанр.
        /// </summary>
        [XmlElement("genre")]
        public string[] Genres { get; set; }

        /// <summary>
        /// Заглавие книги.
        /// </summary>
        [XmlElement("book-title")]
        public string Title { get; set; }

        ///// <summary>
        ///// Аннотация.
        ///// </summary>
        //[XmlElement("annotation")]
        //public string Annotation { get; set; }

        /// <summary>
        /// Ключевые слова.
        /// </summary>
        [XmlElement("keywords")]
        public string Keywords { get; set; }

        /// <summary>
        /// Язык.
        /// </summary>
        [XmlElement("lang")]
        public string Language { get; set; }

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
