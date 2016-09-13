/* MarcRecordHeader.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
*/

#region Using directives

using ManagedIrbis.Marc;

#endregion

namespace ManagedIrbis.ImportExport
{
    /// <summary>
    /// Summary description for MarcRecordHeader.
    /// </summary>
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

        #region Object members

        #endregion

        #region Private members


        #endregion

        #region Public methods

        /// <summary>
        /// Parse text representation.
        /// </summary>
        public static IsoRecordHeader Parse
            (
                string text
            )
        {
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
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static IsoRecordHeader Parse
            (
                byte[] bytes
            )
        {
            IsoRecordHeader result = new IsoRecordHeader
            {
                RecordStatus = (MarcRecordStatus) bytes[0],
                RecordType = (MarcRecordType) bytes[1],
                BibliographicalIndex = (MarcBibliographicalIndex) bytes[2],
                BibliographicalLevel = (MarcBibliographicalLevel) bytes[3],
                CatalogingRules = (MarcCatalogingRules) bytes[4],
                RelatedRecord = (MarcRelatedRecord) bytes[5]
            };

            return result;
        }

        /// <summary>
        /// Заголовок по умолчанию.
        /// </summary>
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
    }
}
