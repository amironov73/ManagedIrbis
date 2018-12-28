// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConfigDatabase.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM.Text;

using ManagedIrbis;

#endregion

namespace InventoryControl
{
    public class ConfigDatabase
    {
        #region Nested classes

        class ConfigLine
        {
            #region Properties

            public NumberText From { get; set; }

            public NumberText To { get; set; }

            #endregion
        }

        #endregion

        #region Construction

        public ConfigDatabase
            (
                IrbisConnection client
            )
        {
            _lines = new List<ConfigLine>();

            using (new IrbisContextSaver(client))
            {
                client.Database = "CONFIG";
                MarcRecord record = client
                    .SearchReadOneRecord("RL=OBRAB");
                if (record != null)
                {
                    RecordField[] fields = record.Fields
                        .GetField(100);
                    _lines.AddRange(fields.Select(_ParseField));
                }
            }
        }

        #endregion

        #region Private members

        private readonly List<ConfigLine> _lines;

        private static ConfigLine _ParseField
            (
                RecordField field
            )
        {
            ConfigLine result = new ConfigLine
            {
                From = field.GetFirstSubFieldValue('a'),
                To = field.GetFirstSubFieldValue('b')
            };
            return result;
        }

        #endregion

        #region Public methods

        public bool BookInWork
            (
                string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            NumberText number = text;
            foreach (var line in _lines)
            {
                if ((line.From <= number)
                    && (line.To >= number))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
