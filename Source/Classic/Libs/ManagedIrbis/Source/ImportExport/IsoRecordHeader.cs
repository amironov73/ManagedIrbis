// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MarcRecordHeader.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
*/

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Marc;

#endregion

namespace ManagedIrbis.ImportExport
{
    /// <summary>
    /// Summary description for MarcRecordHeader.
    /// </summary>
    [PublicAPI]
    public sealed class IsoRecordHeader
    {
        #region Properties

        /// <summary>
        /// Статус записи.
        /// </summary>
        public MarcRecordStatus RecordStatus { get; set; }

        /// <summary>
        /// Тип записи.
        /// </summary>
        public MarcRecordType RecordType { get; set; }

        /// <summary>
        /// Библиографический указатель.
        /// </summary>
        public MarcBibliographicalIndex BibliographicalIndex { get; set; }


        /// <summary>
        /// Уровень описания.
        /// </summary>
        /// <value></value>
        public MarcBibliographicalLevel BibliographicalLevel { get; set; }

        /// <summary>
        /// Правила каталогизации.
        /// </summary>
        public MarcCatalogingRules CatalogingRules { get; set; }

        /// <summary>
        /// Наличие связанной записи.
        /// </summary>
        public MarcRelatedRecord RelatedRecord { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Encode the header.
        /// </summary>
        public void Encode
            (
                [NotNull] byte[] bytes,
                int offset
            )
        {
            Code.NotNull(bytes, "bytes");

            unchecked
            {
                bytes[offset] = (byte) RecordStatus;
                bytes[offset + 1] = (byte) RecordType;
                bytes[offset + 2] = (byte) BibliographicalIndex;
                bytes[offset + 3] = (byte) BibliographicalLevel;
                bytes[offset + 4] = (byte) CatalogingRules;
                bytes[offset + 5] = (byte) RelatedRecord;
            }
        }

        /// <summary>
        /// Parse text representation.
        /// </summary>
        [NotNull]
        public static IsoRecordHeader Parse
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            IsoRecordHeader result = new IsoRecordHeader
            {
                RecordStatus = (MarcRecordStatus) text[0],
                RecordType = (MarcRecordType) text[1],
                BibliographicalIndex = (MarcBibliographicalIndex) text[2],
                BibliographicalLevel = (MarcBibliographicalLevel) text[3],
                CatalogingRules = (MarcCatalogingRules)text[4],
                RelatedRecord = (MarcRelatedRecord)text[5]
            };

            return result;
        }

        /// <summary>
        /// Parse binary representation.
        /// </summary>
        [NotNull]
        public static IsoRecordHeader Parse
            (
                [NotNull] byte[] bytes,
                int offset
            )
        {
            Code.NotNull(bytes, "bytes");

            IsoRecordHeader result = new IsoRecordHeader
            {
                RecordStatus = (MarcRecordStatus) bytes[offset],
                RecordType = (MarcRecordType) bytes[offset + 1],
                BibliographicalIndex = (MarcBibliographicalIndex) bytes[offset + 2],
                BibliographicalLevel = (MarcBibliographicalLevel) bytes[offset + 3],
                CatalogingRules = (MarcCatalogingRules) bytes[offset + 4],
                RelatedRecord = (MarcRelatedRecord) bytes[offset + 5]
            };

            return result;
        }

        /// <summary>
        /// Заголовок по умолчанию.
        /// </summary>
        [NotNull]
        public static IsoRecordHeader GetDefault()
        {
            IsoRecordHeader result = new IsoRecordHeader
            {
                RecordStatus = MarcRecordStatus.New,
                RecordType = MarcRecordType.Text,
                BibliographicalIndex = MarcBibliographicalIndex.Monograph,
                BibliographicalLevel = MarcBibliographicalLevel.Unknown,
                CatalogingRules = MarcCatalogingRules.NotConforming,
                RelatedRecord = MarcRelatedRecord.NotRequired
            };

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            char[] result = new char[6];
            unchecked
            {
                result[0] = (char) RecordStatus;
                result[1] = (char) RecordType;
                result[2] = (char) BibliographicalIndex;
                result[3] = (char) BibliographicalLevel;
                result[4] = (char) CatalogingRules;
                result[5] = (char) RelatedRecord;
            }

            return new string(result);
        }

        #endregion
    }
}
