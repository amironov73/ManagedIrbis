// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MorphologyEntry.cs
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

#endregion

namespace ManagedIrbis.Morphology
{
#if NOTDEF

    public sealed class MorphologyEntry
    {
        #region Properties

        // Поле 10
        public string MainTerm { get; set; }

        // Поле 11
        public string Dictionary { get; set; }

        // Поле 12
        public string Language { get; set; }

        // Поле 20
        public string[] Forms { get; set; }

        #endregion

        #region Public methods

        public static MorphologyEntry Parse
            (
                IrbisRecord record
            )
        {
            MorphologyEntry result = new MorphologyEntry
            {
                MainTerm = record.FM("10"),
                Dictionary = record.FM("11"),
                Language = record.FM("12"),
                Forms = record.FMA("20")
            };

            return result;
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return MainTerm;
        }

        #endregion
    }

#endif
}
