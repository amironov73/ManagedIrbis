// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusAt.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using AM;

using ManagedIrbis.Infrastructure;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    static class UniforPlusAt
    {
        #region Private members

        #endregion

        #region Public methods

        //
        // SAMPLE:
        //
        // 0
        // 2441#0
        // 0#1
        // {
        // "200":
        //   {
        //     "0":
        //     {
        //       "A":"Заглавие",
        //       "E":"подзаголовочное",
        //       "F":"И. И. Иванов, П. П. Петров"
        //     }
        //   },
        // "210":
        //   {
        //     "0":
        //     {
        //       "a":"Иркутск",
        //       "d":"2016"
        //     }
        //   },
        // "215":
        //   {
        //     "0":
        //     {
        //       "a":"123"
        //     }
        //   },
        // "300":
        //   {
        //     "0":
        //     {
        //       "*":"Первое примечание"
        //     },
        //     "1":
        //     {
        //       "*":"Второе примечание"
        //     },
        //     "2":
        //     {
        //       "*":"Третье примечание"
        //     }
        //   },
        // "700":
        //   {
        //   "0":
        //     {
        //       "a":"Иванов","b":"И. И."
        //     }
        //   },
        // "701":
        //   {
        //     "0":
        //     {
        //       "a":"Петров",
        //       "b":"П. П."
        //     }
        //   },
        // "903":
        //   {
        //     "0":
        //     {
        //       "*":"-760577424"
        //     }
        //   },
        // "907":
        //   {
        //     "0":
        //     {
        //       "A":"20160805"
        //     }
        //   },
        // "920":
        //   {
        //     "0":
        //     {
        //       "*":"PAZK"
        //     }
        //   }
        // }

        /// <summary>
        /// Выдать содержимое документа полностью в формате JSON.
        /// </summary>
        public static void FormatJson
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
#if !PocketPC && !WINMOBILE

            MarcRecord record = context.Record;
            if (ReferenceEquals(record, null))
            {
                return;
            }

            int[] tags = record.Fields.Select
                (
                    field => field.Tag
                )
                .OrderBy(tag => tag)
                .Distinct()
                .ToArray();

            JObject obj = new JObject();

            foreach (int tag in tags)
            {
                JObject tagObj = new JObject();
                obj.Add(tag.ToInvariantString(), tagObj);

                RecordField[] fields = record.Fields.GetField(tag);

                for (int i = 0; i < fields.Length; i++)
                {
                    JObject fieldObj = new JObject();
                    tagObj.Add
                        (
                            i.ToInvariantString(),
                            fieldObj
                        );

                    RecordField field = fields[i];
                    if (!string.IsNullOrEmpty(field.Value))
                    {
                        fieldObj.Add("*", field.Value);
                    }

                    foreach (SubField subField in field.SubFields)
                    {
                        fieldObj.Add(subField.CodeString, subField.Value);
                    }
                }
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("0");
            builder.AppendFormat("{0}#0", record.Mfn);
            builder.AppendLine();
            builder.AppendFormat("0#{0}", (int) record.Status);
            builder.AppendLine();

            string output = builder + obj.ToString(Formatting.Indented);
            context.Write(node, output);
            context.OutputFlag = true;

#endif
        }

        #endregion
    }
}
