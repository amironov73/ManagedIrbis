// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using ManagedIrbis;
using ManagedIrbis.Fields;

#endregion

namespace HudoInvent
{
    /// <summary>
    /// Информация о  проверяемой книге художественного фонда.
    /// </summary>
    sealed class HudoInfo
    {
        /// <summary>
        /// Библиографическая запись плюс более полное биб. описание.
        /// </summary>
        public MarcRecord Record;

        /// <summary>
        /// Инвентарный номер, соответствующий штрих-коду.
        /// </summary>
        public string Number;

        /// <summary>
        /// Прочитанный штрих-код.
        /// </summary>
        public string Barcode;

        /// <summary>
        /// Краткое биб. описание.
        /// </summary>
        public string Description;

        /// <summary>
        /// Книга на руках: номер билета.
        /// </summary>
        public string Ticket;

        /// <summary>
        /// Все экземпляры данной книги.
        /// </summary>
        public ExemplarInfo[] Exemplars;

        /// <summary>
        /// Проверяемый экземпляр.
        /// </summary>
        public ExemplarInfo CurrentExemplar;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public HudoInfo
            (
                IIrbisConnection connection,
                int mfn
            )
        {
            Record = connection.ReadRecord
                (
                    connection.Database,
                    mfn,
                    false,
                    "@"
                );
            Description = connection.FormatRecord("@brief", mfn);
            Exemplars = ExemplarInfo.Parse(Record);
        }
    }
}
