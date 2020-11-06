// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* UchRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Istu.OldModel
{
    /// <summary>
    /// Информация о подсобном фонде.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [TableName("uchtrans")]
    public class UchRecord
    {
        #region Properties

        /// <summary>
        /// Штрих-код книги.
        /// </summary>
        [MapField("barcode")]
        public string Barcode { get; set; }

        /// <summary>
        /// Номер карточки комплектования.
        /// </summary>
        [MapField("cardnum")]
        public string CardNumber { get; set; }

        /// <summary>
        /// Момент выдачи.
        /// </summary>
        [MapField("whn")]
        public DateTime Moment { get; set; }

        ///<summary>
        /// Табельный номер оператора.
        ///</summary>
        [MapField("operator")]
        public int Operator { get; set; }

        /// <summary>
        /// Примечания.
        /// </summary>
        [MapField("remarks")]
        public string Remarks { get; set; }

        /// <summary>
        /// Номер читательского билета.
        /// </summary>
        [MapField("chb")]
        public string Ticket { get; set; }

        /// <summary>
        /// Инвентарный номер.
        /// </summary>
        [MapField("invnum")]
        public string Inventory { get; set; }

        /// <summary>
        /// Цена экземпляра
        /// </summary>
        [MapField("price")]
        public decimal Price { get; set; }

        ///<summary>
        /// Количество продлений.
        ///</summary>
        [MapField("prodlen")]
        public int Prolongation { get; set; }

        ///<summary>
        /// Предполагаемый срок возврата.
        ///</summary>
        [MapField("srok")]
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Снова оператор.
        /// </summary>
        [MapField("operator2")]
        public int Operator2 { get; set; }

        /// <summary>
        /// Имя машины.
        /// </summary>
        [MapField("machine")]
        public string Machine { get; set; }

        /// <summary>
        /// Находится на руках у читателя, номер билета.
        /// </summary>
        [MapField("onhand")]
        public string OnHand { get; set; }

        /// <summary>
        /// Сообщение о книге.
        /// </summary>
        [MapField("alert")]
        public string Alert { get; set; }

        /// <summary>
        /// Дата инвентаризации.
        /// </summary>
        [MapField("seen")]
        public DateTime Seen { get; set; }

        /// <summary>
        /// Оператор инвентаризации.
        /// </summary>
        [MapField("seenby")]
        public int SeenBy { get; set; }

        /// <summary>
        /// RFID-метка.
        /// </summary>
        [MapField("rfid")]
        public string Rfid { get; set; }

        /// <summary>
        /// Место хранения ЦОР, ЦНИ и т. д.
        /// </summary>
        [MapField("sigla")]
        public string Sigla { get; set; }

        #endregion
    }
}
