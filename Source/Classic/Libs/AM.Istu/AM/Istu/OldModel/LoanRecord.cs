// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Loan.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Configuration;
using AM.Data;

using BLToolkit.Mapping;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

#endregion

namespace AM.Istu.OldModel
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class LoanRecord
    {
        #region Properties

        /// <summary>
        /// Какие-нибудь дополнительные данные, 
        /// сопровождающие информацию о выданной книге.
        /// В настоящее время не используется.
        /// </summary>
        [HiddenColumn]
        [MapIgnore]
        [Browsable(false)]
        public object Context { get; set; }

        ///<summary>
        /// MFN записи о книге в каталоге.
        ///</summary>
        [ColumnIndex(0)]
        [ColumnWidth(30)]
        [ColumnHeader("MFN")]
        //[DisplayTitle("MFN")]
        [ReadOnly(true)]
        public int Mfn { get; set; }

        ///<summary>
        /// Инвентарный номер книги.
        ///</summary>
        [ColumnIndex(1)]
        [ColumnWidth(50)]
        [ColumnHeader("Номер")]
        //[DisplayTitle("Номер")]
        [ReadOnly(true)]
        public string Number { get; set; }

        ///<summary>
        /// Номер читательского билета,
        /// кому выдана книга.
        ///</summary>
        [ColumnIndex(3)]
        [ColumnWidth(50)]
        [ColumnHeader("На руках")]
        //[DisplayTitle("На руках")]
        public string Ticket { get; set; }

        ///<summary>
        /// ФИО читателя, кому выдана книга.
        ///</summary>
        [ColumnIndex(4)]
        [ColumnWidth(100)]
        [ColumnHeader("ФИО читателя")]
        //[DisplayTitle("ФИО читателя")]
        public string Name { get; set; }

        ///<summary>
        /// Краткое библиографическое описание книги.
        ///</summary>
        [ColumnIndex(2)]
        [ColumnWidth(150)]
        [ColumnHeader("Библиографическое описание")]
        //[DisplayTitle("Библиографическое описание")]
        public string Description { get; set; }

        ///<summary>
        /// Крайний срок возврата книги.
        ///</summary>
        [ColumnIndex(5)]
        [ColumnWidth(60)]
        [ColumnHeader("Срок")]
        //[DisplayTitle("Срок")]
        public DateTime Deadline { get; set; }

        ///<summary>
        /// Счетчик продлений выдачи.
        ///</summary>
        [ColumnIndex(6)]
        [ColumnWidth(30)]
        //[System.ComponentModel.DefaultValue(0)]
        [ColumnHeader("Продл.")]
        //[DisplayTitle("Продления")]
        public int ProlongationCount { get; set; }

        ///<summary>
        /// Идентификатор оператора.
        ///</summary>
        [HiddenColumn]
        //[DisplayTitle("Оператор")]
        public int Operator { get; set; }

        ///<summary>
        /// Дата выдачи книги читателю.
        ///</summary>
        [HiddenColumn]
        //[DisplayTitle("Дата выдачи")]
        public DateTime Moment { get; set; }

        /// <summary>
        /// Признак контрольного экземпляра.
        /// </summary>
        /// <value>Если не пустая строка,
        /// значит, контрольный экземпляр.</value>
        //[DisplayTitle("Контрольный экземпляр")]
        [ColumnHeader("")]
        [ColumnIndex(10)]
        [ColumnWidth(10)]
        //[System.ComponentModel.DefaultValue("")]
        public string PilotCopy { get; set; }

        /// <summary>
        /// Читатель, которому выдана книга
        /// (для книг из подсобного фонда
        /// читального зала).
        /// </summary>
        /// <value>The on hand.</value>
        //[DisplayTitle("У читателя")]
        [ColumnHeader("")]
        [ColumnIndex(9)]
        [ColumnWidth(20)]
        //[System.ComponentModel.DefaultValue("")]
        public string OnHand { get; set; }

        /// <summary>
        /// Номер карточки безинвентарного учета
        /// (для многоэкземплярной литературы).
        /// </summary>
        /// <value>The card number.</value>
        //[DisplayTitle("Карточка комплектования")]
        [HiddenColumn]
        [ColumnWidth(20)]
        public string CardNumber { get; set; }

        /// <summary>
        /// Примечания об экземпляре документа.
        /// </summary>
        //[DisplayTitle("Примечания")]
        [HiddenColumn]
        public string Alert { get; set; }

        /// <summary>
        /// Цена экземпляра
        /// </summary>
        //[DisplayTitle("Цена")]
        [HiddenColumn]
        public decimal? Price { get; set; }

        /// <summary>
        /// Код записи в электронном каталоге
        /// (поле 903).
        /// </summary>
        /// <value>The book ID.</value>
        [HiddenColumn]
        //[DisplayTitle("Код в каталоге")]
        public string BookID { get; set; }

        /// <summary>
        /// Идентификатор RFID-метки.
        /// </summary>
        //[DisplayTitle("RFID")]
        [HiddenColumn]
        public string Rfid { get; set; }

        /// <summary>
        /// Для обозначения книг ЦОР, ЦНИ и проч.
        /// </summary>
        //[DisplayTitle("Сигла")]
        [ColumnHeader("Сигла")]
        [ColumnIndex(10)]
        [ColumnWidth(20)]
        public string Sigla { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
