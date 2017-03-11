// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisJson.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WINMOBILE && !PocketPC

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

#endregion

namespace ManagedIrbis.ImportExport
{
    /// <summary>
    /// 
    /// </summary>
    public static class IrbisJson
    {
        /// <summary>
        /// Строит представление записи в виде JSON,
        /// характерном для ИРБИС.
        /// </summary>
        public static string RecordToIrbisJson
            (
                [NotNull] this MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            JObject result = new JObject();

            string[] tags = record.Fields
                .Select(field => field.Tag)
                .Distinct()
                .ToArray();

            foreach (string tag in tags)
            {
                RecordField[] fields = record.Fields.GetField(tag);
                JProperty tagProperty = new JProperty(tag);
                result.Add(tagProperty);

                for (int i = 0; i < fields.Length; i++)
                {
                    JProperty repeatProperty 
                        = new JProperty(i.ToString());
                    tagProperty.Add(repeatProperty);
                    RecordField field = fields[i];
                    if (!string.IsNullOrEmpty(field.Value))
                    {
                        JProperty textProperty 
                            = new JProperty
                                (
                                    "*",
                                    field.Value
                                );
                        repeatProperty.Add(textProperty);
                    }
                    foreach (SubField subField in field.SubFields)
                    {
                        JProperty subProperty = new JProperty
                            (
                                subField.CodeString,
                                subField.Value
                            );
                        repeatProperty.Add(subProperty);
                    }
                }
            }

            return result.ToString();
        }

    }
}

#endif
