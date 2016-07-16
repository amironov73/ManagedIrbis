/* MagazineArticleInfo.cs -- информация о статье из журнала
 * Ars Magna project, http://arsmagna.ru
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
                IrbisRecord record
            )
        {
            if (ReferenceEquals(record, null))
            {
                throw new ArgumentNullException("record");
            }

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

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
        }

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
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
