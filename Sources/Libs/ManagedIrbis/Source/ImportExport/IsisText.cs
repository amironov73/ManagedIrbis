/* IsisText.cs --
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

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.ImportExport
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class IsisText
    {
        /// <summary>
        /// Формирует текстовое представление записи,
        /// характерное для ISIS.
        /// </summary>
        public static string RecordToIsisText
            (
                [NotNull] this MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            StringBuilder result = new StringBuilder();

            foreach (RecordField field in record.Fields)
            {
                result.AppendFormat
                    (
                        "<{0}>{1}</{0}>",
                        field.Tag,
                        field.ToText()
                    );
                result.AppendLine();
            }

            return result.ToString();
        }
    }
}
