// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MagazineArticleInfo.cs -- информация о статье из журнала
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Xml.Serialization;

using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Magazines
{
    /// <summary>
    /// Информация о статье из журнала/сборника.
    /// </summary>
    [PublicAPI]
    [XmlRoot("article")]
    [MoonSharpUserData]
    public sealed class MagazineArticleInfo
        : IHandmadeSerializable
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор записи.
        /// </summary>
        public static MagazineArticleInfo Parse
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            MagazineArticleInfo result = new MagazineArticleInfo();

            return result;
        }

        /// <summary>
        /// Разбор поля (330 или 922).
        /// </summary>
        public static MagazineArticleInfo Parse
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            MagazineArticleInfo result = new MagazineArticleInfo();
            return result;
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
        }

        /// <inheritdoc />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
        }

        #endregion

        #region Object members

        #endregion
    }
}
