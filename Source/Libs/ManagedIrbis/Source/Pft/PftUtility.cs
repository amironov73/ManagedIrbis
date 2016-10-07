/* PftUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Pft
{
    /// <summary>
    /// Utility routines for PFT scripting.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class PftUtility
    {
        #region Properties

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Format for data mode.
        /// </summary>
        [NotNull]
        public static string FormatDataMode
            (
                [NotNull] this RecordField field
            )
        {
            Code.NotNull(field, "field");

            string result = FormatHeaderMode(field);

            if (!result.EndsWith(".")
                &!result.EndsWith(". ")
                &!result.EndsWith(".  "))
            {
                result = result + ".";
            }

            if (result.EndsWith("  "))
            {
                // nothing to do
            }
            else if (result.EndsWith(" "))
            {
                result = result + " ";
            }
            else
            {
                result = result + "  ";
            }

            return result;
        }

        /// <summary>
        /// Format for header mode.
        /// </summary>
        [NotNull]
        public static string FormatHeaderMode
            (
                [NotNull] this RecordField field
            )
        {
            Code.NotNull(field, "field");

            StringBuilder result = new StringBuilder();

            result.Append(field.Value);
            foreach (SubField subField in field.SubFields)
            {
                string delimiter = ". ";
                char code = char.ToLower(subField.Code);
                switch (code)
                {
                    case 'a':
                        delimiter = "; ";
                        break;

                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                    case 'h':
                    case 'i':
                        delimiter = ", ";
                        break;
                }
                if (result.Length != 0)
                {
                    result.Append(delimiter);
                }

                string value = subField.Value;
                if (!string.IsNullOrEmpty(value))
                {
                    value = value
                        .Replace("><", "; ")
                        .Replace("<", string.Empty)
                        .Replace(">", string.Empty);
                }

                result.Append(value);
            }

            return result.ToString();
        }

        /// <summary>
        /// Format field according to specified output mode.
        /// </summary>
        [NotNull]
        public static string FormatField
            (
                [NotNull] this RecordField field,
                PftFieldOutputMode mode,
                bool uppercase
            )
        {
            Code.NotNull(field, "field");

            string result;

            switch (mode)
            {
                case PftFieldOutputMode.DataMode:
                    result = FormatDataMode(field);
                    break;

                case PftFieldOutputMode.HeaderMode:
                    result = FormatHeaderMode(field);
                    break;

                case PftFieldOutputMode.PreviewMode:
                    result = field.ToText();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (uppercase)
            {
                result = result.ToUpper();
            }

            return result;
        }


        #endregion
    }
}
