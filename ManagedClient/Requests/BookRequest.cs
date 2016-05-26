/* BookRequest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

#endregion

namespace ManagedClient.Requests
{
    using Readers;

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class BookRequest
    {
        #region Properties

        /// <summary>
        /// MFN записи с заказом.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Краткое описание заказанного издания.
        /// Поле 201.
        /// </summary>
        public string BookDescription { get; set; }

        /// <summary>
        /// Шифр заказанного издания.
        /// Поле 903.
        /// </summary>
        public string BookCode { get; set; }

        /// <summary>
        /// Дата создания заказа.
        /// Поле 40.
        /// </summary>
        public string RequestDate { get; set; }

        /// <summary>
        /// Идентификатор читателя.
        /// Поле 30.
        /// </summary>
        public string ReaderID { get; set; }

        /// <summary>
        /// Краткое описание читателя.
        /// Поле 31.
        /// </summary>
        public string ReaderDescription { get; set; }

        /// <summary>
        /// Имя БД электронного каталога.
        /// Поле 1.
        /// Как правило, IBIS.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Сведения об отказе.
        /// Поле 44.
        /// Подполе A: причина отказа.
        /// Подполе B: дата.
        /// </summary>
        public string RejectInfo { get; set; }

        /// <summary>
        /// Место выдачи.
        /// Поле 102.
        /// Часто равно *.
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// Ответственное лицо.
        /// Поле 50.
        /// Берется из логина.
        /// </summary>
        public string ResponsiblePerson { get; set; }

        /// <summary>
        /// Библиографическая запись о книге.
        /// </summary>
        public IrbisRecord BookRecord { get; set; }

        /// <summary>
        /// Сведения о читателе.
        /// </summary>
        public ReaderInfo Reader { get; set; }

        /// <summary>
        /// Свободные инвентарные номера.
        /// </summary>
        public string[] FreeNumbers { get; set; }

        /// <summary>
        /// Свободные номера, предназначенные для данного АРМ.
        /// </summary>
        public string[] MyNumbers { get; set; }

        /// <summary>
        /// Запись, на осонове которой построен запрос
        /// </summary>
        public IrbisRecord Record { get; set; }

        #endregion

        #region Private members

        private static void _AddField
            (
                IrbisRecord record,
                string tag,
                string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                record.Fields.Add(new RecordField(tag, text));
            }
        }

        #endregion

        #region Public methods

        public static BookRequest Parse(IrbisRecord record)
        {
            BookRequest result = new BookRequest
                                     {
                                         Mfn = record.Mfn,
                                         BookDescription = record.FM("201"),
                                         BookCode = record.FM("903"),
                                         RequestDate = record.FM("40"),
                                         ReaderID = record.FM("30"),
                                         ReaderDescription = record.FM("31"),
                                         Database = record.FM("1"),
                                         RejectInfo = record.FM("44"),
                                         Place = record.FM("102"),
                                         ResponsiblePerson = record.FM("50"),
                                         Record = record
                                     };

            return result;
        }

        public IrbisRecord Encode()
        {
            IrbisRecord result = new IrbisRecord
                                     {
                                         Mfn = Mfn
                                     };

            _AddField(result, "201", BookDescription);
            _AddField(result, "903", BookCode);
            _AddField(result, "40", RequestDate);
            _AddField(result, "30", ReaderID);
            _AddField(result, "31", ReaderDescription);
            _AddField(result, "1", Database);
            _AddField(result, "44", RejectInfo);
            _AddField(result, "102", Place);
            _AddField(result, "50", ResponsiblePerson);

            return result;
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat
                (
                    "Читатель: {0}",
                    ReaderDescription
                );
            result.AppendLine();
            result.AppendLine();
            result.AppendLine(BookDescription);
            result.AppendLine();
            result.AppendFormat
                (
                    "Свободные экземпляры: {0}",
                    string.Join(", ", FreeNumbers)
                );
            result.AppendLine();
            result.AppendFormat
                (
                    "Мои экземпляры: {0}",
                    string.Join(", ", MyNumbers)
                );
            result.AppendLine();
            result.AppendFormat
                (
                    "Место выдачи: {0}",
                    Place
                );
            result.AppendLine();

            return result.ToString();
        }

        #endregion
    }
}
