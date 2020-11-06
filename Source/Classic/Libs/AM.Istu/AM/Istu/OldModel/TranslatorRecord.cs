// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* TranslatorRecord.cs -- 
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
    /// Трансляция штрих-кодов в инвентарные номера
    /// научного фонда.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [TableName("translator")]
    public class TranslatorRecord
    {
        #region Properties

        /// <summary>
        /// Инвентарный номер.
        /// </summary>
        [MapField("invnum")]
        public int Inventory { get; set; }

        /// <summary>
        /// Штрих-код.
        /// </summary>
        [MapField("barcode")]
        public string Barcode { get; set; }

        /// <summary>
        /// Момент приписки
        /// </summary>
        [MapField("whn")]
        public DateTime Moment { get; set; }

        /// <summary>
        /// Взято на обработку.
        /// </summary>
        [MapField("taken")]
        public bool Taken { get; set; }

        /// <summary>
        /// Дополнительная информация об экземпляре.
        /// </summary>
        [MapField("info")]
        public string Info { get; set; }

        /// <summary>
        /// Табельный номер оператора.
        /// </summary>
        [MapField("operator")]
        public int Operator { get; set; }

        /// <summary>
        /// Контрольный экземпляр.
        /// </summary>
        [MapField("pilot")]
        public bool Pilot { get; set; }

        /// <summary>
        /// RFID.
        /// </summary>
        [MapField("rfid")]
        public string Rfid { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Inventory.ToInvariantString();
        }

        #endregion
    }
}
