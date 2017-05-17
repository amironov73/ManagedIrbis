// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MagazineRecord.cs --
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

using MoonSharp.Interpreter;

#endregion

namespace AM.Istu.OldModel
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class MagazineRecord
    {
        #region Properties

        /// <summary>
        /// Gets or sets the alert.
        /// </summary>
        [MapField("alert")]
        public string Alert { get; set; }

        /// <summary>
        /// Идентификатор RFID-метки.
        /// </summary>
        [MapField("rfid")]
        public string Rfid { get; set; }

        /// <summary>
        /// Штрих-код.
        /// </summary>
        [MapField("barcode")]
        public string Barcode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MapField("srok")]
        public DateTime Deadline { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MapField("id")]
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MapField("magazine")]
        public int Magazine { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MapField("machine")]
        public string Machine { get; set; }

        /// <summary>
        /// Gets or sets the moment.
        /// </summary>
        [MapField("moment")]
        public DateTime Moment { get; set; }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        [MapField("number")]
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the on hand.
        /// </summary>
        [MapField("onhand")]
        public string OnHand { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>The operator.</value>
        [MapField("operator")]
        public int Operator { get; set; }

        /// <summary>
        /// Gets or sets the operator2.
        /// </summary>
        [MapField("operator2")]
        public int Operator2 { get; set; }

        /// <summary>
        /// Gets or sets the period.
        /// </summary>
        [MapField("period")]
        public int Period { get; set; }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>The ticket.</value>
        [MapField("chb")]
        public string Ticket { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [MapField("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        [MapField("volume")]
        public int Volume { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        [MapField("year")]
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        [MapField("url")]
        public string Url { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            string result;

            if (Number == 0)
            {
                result = string.Format
                    (
                        "{0}. - Подшивка за {1} год",
                        Title,
                        Year
                    );
            }
            else if (Volume != 0)
            {
                result = string.Format
                    (
                        "{0}. - {1}. - Т. {2}. - № {3}",
                        Title,
                        Year,
                        Volume,
                        Number
                    );
            }
            else
            {
                result = string.Format
                    (
                        "{0}. - {1}. - № {2}",
                        Title,
                        Year,
                        Number
                    );
            }

            if (Period != 0)
            {
                result += string.Format(" - Кратность: {0}", Period);
            }

            return result;
        }

        #endregion
    }
}
