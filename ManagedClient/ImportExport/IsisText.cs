using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedClient.ImportExport
{
    public static class IsisText
    {
        /// <summary>
        /// Формирует текстовое представление записи,
        /// характерное для ISIS.
        /// </summary>
        public static string RecordToIsisText
            (
                this IrbisRecord record
            )
        {
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
