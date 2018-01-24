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
              + @"{\fonttbl{\f0\fnil\fcharset204 Times New Roman;}"
              + @"{\f1\fnil\fcharset238 Times New Roman;}}"
              + @"{\stylesheet{\s0\f0\fs24\snext0 Normal;}"
              + @"{\s1\f1\fs40\b\snext0 Heading;}}"
              + @"\viewkind4\uc1\pard\f0\fs16 ";

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Decode text.
        /// </summary>
        [CanBeNull]
        public static string Decode
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            int length = text.Length;

            StringBuilder result = new StringBuilder(length);
            TextNavigator navigator = new TextNavigator(text);
            while (!navigator.IsEOF)
            {
                string chunk = navigator.ReadUntil('\\');
                result.Append(chunk);
                char prefix = navigator.ReadChar();
                if (prefix != '\\')
                {
                    break;
                }
                char c = navigator.ReadChar();
                if (c == '\0')
                {
                    result.Append(prefix);
                    break;
                }
                if (c != 'u')
                {
                    result.Append(prefix);
                    result.Append(c);
                    continue;
                }
                StringBuilder buffer = new StringBuilder();
                while (!navigator.IsEOF)
                {
                    c = navigator.ReadChar();
                    if (!char.IsDigit(c))
                    {
                        break;
                    }
                    buffer.Append(c);
                }
                if (buffer.Length != 0)
                {
                    c = (char)int.Parse(buffer.ToString());
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Encode text.
        /// </summary>
        [CanBeNull]
        public static string Encode
            (
                [CanBeNull] string text,
                [CanBeNull] UnicodeRange goodRange
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            int length = text.Length;

            StringBuilder result = new StringBuilder(length);
            foreach (char c in text)
            {
                if (c < 0x20)
                {
                    result.AppendFormat("\\'{0:x2}", (byte)c);
                }
                else if (c < 0x80)
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
                else if (c < 0x100)
                {
                    result.AppendFormat("\\'{0:x2}", (byte)c);
                }
                else
                {
                    bool simple = false;
                    if (!ReferenceEquals(goodRange, null))
                    {
                        if (c >= goodRange.From && c <= goodRange.To)
                        {
                            simple = true;
                        }
                    }
                    if (simple)
                    {
                        result.Append(c);
                    }
                    else
                    {
                        // После \u следующий символ съедается
                        // поэтому подсовываем знак вопроса
                        result.AppendFormat("\\u{0}?", (short)c);
                    }
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Encode text.
        /// </summary>
        [CanBeNull]
        public static string Encode2
            (
                [CanBeNull] string text,
                [CanBeNull] UnicodeRange goodRange
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            int length = text.Length;

            StringBuilder result = new StringBuilder(length);
            foreach (char c in text)
            {
                if (c < 0x20)
                {
                    result.AppendFormat("\\'{0:x2}", (byte)c);
                }
                else if (c < 0x80)
                {
                    result.Append(c);
                }
                else if (c < 0x100)
                {
                    result.AppendFormat("\\'{0:x2}", (byte)c);
                }
                else
                {
                    bool simple = false;
                    if (!ReferenceEquals(goodRange, null))
                    {
                        if (c >= goodRange.From && c <= goodRange.To)
                        {
                            simple = true;
                        }
                    }
                    if (simple)
                    {
                        result.Append(c);
                    }
                    else
                    {
                        // После \u следующий символ съедается
                        // поэтому подсовываем знак вопроса
                        result.AppendFormat("\\u{0}?", (int)c);
                    }
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Encode text.
        /// </summary>
        [CanBeNull]
        public static string Encode3
            (
                [CanBeNull] string text,
                [CanBeNull] UnicodeRange goodRange,
                [CanBeNull] string modeSwitch
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            int length = text.Length;

            StringBuilder result = new StringBuilder(length);
            foreach (char c in text)
            {
                if (c < 0x20)
                {
                    result.AppendFormat("\\'{0:x2}", (byte)c);
                }
                else if (c < 0x80)
                {
                    result.Append(c);
                }
                else if (c < 0x100)
                {
                    result.Append('{');
                    result.AppendFormat("{0}\\'{1:x2}", modeSwitch, (byte)c);
                    result.Append('}');
                }
                else
                {
                    bool simple = false;
                    if (!ReferenceEquals(goodRange, null))
                    {
                        if (c >= goodRange.From && c <= goodRange.To)
                        {
                            simple = true;
                        }
                    }
                    if (simple)
                    {
                        result.Append(c);
                    }
                    else
                    {
                        // После \u следующий символ съедается
                        // поэтому подсовываем знак вопроса
                        result.AppendFormat("\\u{0}?", (int)c);
                    }
                }
            }

            return result.ToString();
        }


        #endregion
    }
}

