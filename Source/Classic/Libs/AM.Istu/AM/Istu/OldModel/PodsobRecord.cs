// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PodsobRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Configuration;
using AM.Data;
using AM.Logging;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

#endregion

namespace AM.Istu.OldModel
{
    /// <summary>
    /// Информация о подсобном фонде.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [TableName("podsob")]
    public class PodsobRecord
    {
        #region Properties

        ///<summary>
        /// Инвентарный номер книги.
        ///</summary>
        [MapField("INVENT")]
        public long Inventory { get; set; }

        ///<summary>
        /// Номер читательского билета.
        ///</summary>
        [MapField("CHB")]
        public string Ticket { get; set; }

        ///<summary>
        /// Дополнительная информация о читателе.
        ///</summary>
        [MapField("IDENT")]
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Момент выдачи.
        /// </summary>
        [MapField("WHE")]
        public DateTime Moment { get; set; }

        ///<summary>
        /// Табельный номер оператора.
        ///</summary>
        [MapField("operator")]
        public int Operator { get; set; }

        ///<summary>
        /// Предполагаемый срок возврата.
        ///</summary>
        [MapField("srok")]
        public DateTime Deadline { get; set; }

        ///<summary>
        /// Количество продлений.
        ///</summary>
        [MapField("prodlen")]
        public int Prolongation { get; set; }

        /// <summary>
        /// На руках у читателя.
        /// </summary>
        [MapField("onhand")]
        public string OnHand { get; set; }

        /// <summary>
        /// Примечания об экземпляре книги.
        /// </summary>
        [MapField("alert")]
        public string Alert { get; set; }

        /// <summary>
        /// Контрольный экземпляр.
        /// </summary>
        [MapField("pilot")]
        public char Pilot { get; set; }

        /// <summary>
        /// Место хранения ЦОР, ЦНИ и т. д.
        /// </summary>
        [MapField("sigla")]
        public string Sigla { get; set; }

        /// <summary>
        /// MARC record.
        /// </summary>
        [CanBeNull]
        [MapIgnore]
        public MarcRecord Record { get; set; }

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
