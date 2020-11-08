// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* HudRecord.cs --
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
    /// Сведения о выданной книге художественного фонда.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [TableName("hudtrans")]
    public class HudRecord
    {
        #region Properties

        /// <summary>
        /// Инвентарный номер.
        /// </summary>
        [MapField("invnum")]
        public string Inventory { get; set; }

        /// <summary>
        /// Дата выдачи.
        /// </summary>
        [MapField ("whn")]
        public DateTime Moment { get; set; }

        /// <summary>
        /// Табельный номер оператора.
        /// </summary>
        [MapField("operator")]
        public int Operator { get; set; }

        /// <summary>
        /// Номер читательского билета.
        /// </summary>
        [MapField("chb")]
        public string Ticket { get; set; }

        /// <summary>
        /// Счетчик продлений
        /// </summary>
        [MapField("prodlen")]
        public int Prolong { get; set; }

        /// <summary>
        /// Предполагаемая дата возврата.
        /// </summary>
        [MapField("srok")]
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Сообщение.
        /// </summary>
        [MapField("alert")]
        public string Alert { get; set; }

        /// <summary>
        /// RFID-метка.
        /// </summary>
        [MapField("rfid")]
        public string Rfid { get; set; }

        #endregion
    }
}
