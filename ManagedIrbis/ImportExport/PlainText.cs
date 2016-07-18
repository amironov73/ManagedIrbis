using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedIrbis.ImportExport
{
    /// <summary>
    /// 
    /// </summary>
    public static class PlainText
    {
        /// <summary>
        /// Формирует плоское текстовое представление записи.
        /// </summary>
        /// <returns></returns>
        public static string ToPlainText
            (
                this MarcRecord record
            )
        {
            StringBuilder result = new StringBuilder();

            foreach (RecordField field in record.Fields)
            {
                result.AppendFormat("{0}#", field.Tag);
                //bool begin = true;
                if (!string.IsNullOrEmpty(field.Value))
                {
                    result.Append(field.Value);
                    //begin = false;
                }
                foreach (SubField subField in field.SubFields)
                {
                    //if (!begin)
                    //{
                    //    result.Append(" ");
                    //}
                    result.Append('^');
                    result.Append(subField.Code);
                    result.Append(subField.Value);
                    //begin = false;
                }
                result.AppendLine();
            }

            return result.ToString();
        }

    }
}
