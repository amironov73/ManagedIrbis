// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DataCell.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class DataCell
        : ReportCell
    {
        #region Properties

        /// <summary>
        /// Index.
        /// </summary>
        [JsonProperty("index")]
        [XmlAttribute("index")]
        public int Index { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region ReportCell

        /// <inheritdoc cref="ReportCell.Compute"/>
        public override string Compute
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            string result = null;
            MarcRecord record = context.CurrentRecord;
            if (!ReferenceEquals(record, null))
            {
                object[] data = record.UserData as object[];
                if (!ReferenceEquals(data, null))
                {
                    object obj = data.GetOccurrence(Index);
                    result = obj.NullableToString();
                }
                else
                {
                    IList<object> list = record.UserData as IList<object>;
                    if (!ReferenceEquals(list, null))
                    {
                        object obj = list.GetItem(Index);
                        result = obj.NullableToString();
                    }
                }
            }

            return result;
        }

        /// <inheritdoc cref="ReportCell.Render"/>
        public override void Render
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            string text = Compute(context);

            ReportDriver driver = context.Driver;
            driver.BeginCell(context, this);
            driver.Write(context, text);
            driver.EndCell(context, this);
        }

        #endregion

        #region Object members

        #endregion
    }
}
