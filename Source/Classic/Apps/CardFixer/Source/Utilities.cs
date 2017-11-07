using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

using ManagedIrbis;

namespace CardFixer
{
    static class Utilities
    {
        [NotNull]
        public static MarcRecord AddField
            (
                [NotNull] this MarcRecord record,
                int tag,
                string value
            )
        {
            record.Fields.Add(new RecordField(tag, value));

            return record;
        }

        [NotNull]
        public static MarcRecord AddField
            (
                [NotNull] this MarcRecord record,
                int tag,
                char code,
                string value,
                params object[] others
            )
        {
            RecordField field = new RecordField(tag);
            field.AddSubField(code, value);
            for (int i = 0; i < others.Length; i += 2)
            {
                char c = (char)others[i];
                string t = (string)others[i + 1];
                field.AddSubField(c, t);
            }
            record.Fields.Add(field);

            return record;
        }
    }
}
