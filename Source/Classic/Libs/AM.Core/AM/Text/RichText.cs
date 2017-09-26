// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RichText.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text;

using JetBrains.Annotations;

#endregion

namespace AM.Text
{
    /// <summary>
    /// Rich text support.
    /// </summary>
    public static class RichText
    {
        #region Properties

        /// <summary>
        /// Central European prologue for RTF file.
        /// </summary>
        public static string CentralEuropeanPrologue
            = @"{\rtf1\ansi\ansicpg1250\deff0\deflang1033"
              + @"{\fonttbl{\f0\fnil\fcharset238 MS Sans Serif;}}"
              + @"\viewkind4\uc1\pard\f0\fs16 ";

        /// <summary>
        /// Common prologue for RTF file.
        /// </summary>
        public static string CommonPrologue
            = @"{\rtf1\ansi\deff0"
              + @"{\fonttbl{\f0\fnil\fcharset0 MS Sans Serif;}}"
              + @"\viewkind4\uc1\pard\f0\fs16 ";


        /// <summary>
        /// Western European prologue for RTF file.
        /// </summary>
        public static string WesternEuropeanPrologue
            = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033"
              + @"{\fonttbl{\f0\fnil\fcharset0 MS Sans Serif;}}"
              + @"\viewkind4\uc1\pard\f0\fs16 ";

        /// <summary>
        /// Russian prologue for RTF file.
        /// </summary>
        public static string RussianPrologue
            = @"{\rtf1\ansi\ansicpg1251\deff0\deflang1049"
              + @"{\fonttbl{\f0\fnil\fcharset204 MS Sans Serif;}}"
              + @"\viewkind4\uc1\pard\f0\fs16 ";

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Encode text.
        /// </summary>
        [CanBeNull]
        public static string Encode
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            int length = text.Length;
            int found = 0;
            for (int i = 0; i < length; i++)
            {
                char c = text[i];
                if (c == '{' || c == '}' || c == '\\')
                {
                    found++;
                }
            }
            if (found == 0)
            {
                return text;
            }

            StringBuilder result = new StringBuilder(length + found);
            foreach (char c in text)
            {
                switch (c)
                {
                    case '{':
                        result.Append("\\{");
                        break;

                    case '}':
                        result.Append("\\}");
                        break;

                    case '\\':
                        result.Append("\\\\");
                        break;

                    default:
                        result.Append(c);
                        break;
                }
            }

            return result.ToString();
        }

        #endregion
    }
}
