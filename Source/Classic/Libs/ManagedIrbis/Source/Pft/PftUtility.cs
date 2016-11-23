/* PftUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;

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

        private static string _ReadTo
            (
                StringReader reader,
                char delimiter
            )
        {
            StringBuilder result = new StringBuilder();

            while (true)
            {
                int next = reader.Read();
                if (next < 0)
                {
                    break;
                }
                char c = (char)next;
                if (c == delimiter)
                {
                    break;
                }
                result.Append(c);
            }

            return result.ToString();
        }

        [NotNull]
        private static RecordField _ParseLine
            (
                [NotNull] string line
            )
        {
            Code.NotNull(line, "line");

            StringReader reader = new StringReader(line);
            RecordField result = new RecordField
            {
                Value = _ReadTo(reader, '^')
            };

            while (true)
            {
                int next = reader.Read();
                if (next < 0)
                {
                    break;
                }

                char code = char.ToLower((char)next);
                string text = _ReadTo(reader, '^');
                SubField subField = new SubField
                {
                    Code = code,
                    Value = text
                };
                result.SubFields.Add(subField);
            }

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Assign field.
        /// </summary>
        public static void AssignField
            (
                [NotNull] PftContext context,
                [NotNull] string tag,
                IndexSpecification index,
                [CanBeNull] string value
            )
        {
            Code.NotNull(context, "context");
            Code.NotNullNorEmpty(tag, "tag");

            tag = FieldTag.Normalize(tag);

            MarcRecord record = context.Record;
            if (ReferenceEquals(record, null))
            {
                return;
            }

            RecordField[] fields = record.Fields.GetField(tag);
            string[] lines = value.SplitLines()
                .NonEmptyLines()
                .ToArray();
            List<RecordField> newFields = new List<RecordField>();
            foreach (string line in lines)
            {
                RecordField field = ParseField(line);
                field.Tag = tag;
                newFields.Add(field);
            }

            if (index.Kind == IndexKind.None)
            {
                foreach (RecordField field in fields)
                {
                    record.Fields.Remove(field);
                }

                record.Fields.AddRange(newFields);
            }
            else
            {
                int i = index.ComputeValue(context, fields);

                if (newFields.Count == 0)
                {
                    if (i >= 0 && i < fields.Length)
                    {
                        record.Fields.Remove(fields[i]);
                    }
                }
                else
                {
                    if (i >= fields.Length)
                    {
                        record.Fields.AddRange(newFields);
                    }
                    else
                    {
                        int position = record.Fields.IndexOf(fields[i]);
                        fields[i].AssignFrom(newFields[0]);

                        for (int j = 1; j < newFields.Count; j++)
                        {
                            record.Fields.Insert
                                (
                                    position + j,
                                    newFields[j]
                                );
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Assign subfield.
        /// </summary>
        public static void AssignSubField
            (
                [NotNull] PftContext context,
                [NotNull] string tag,
                IndexSpecification fieldIndex,
                char code,
                IndexSpecification subfieldIndex,
                [CanBeNull] string value
            )
        {
            Code.NotNull(context, "context");
            Code.NotNullNorEmpty(tag, "tag");

            tag = FieldTag.Normalize(tag);
            code = SubFieldCode.Normalize(code);

            MarcRecord record = context.Record;
            if (ReferenceEquals(record, null))
            {
                return;
            }

            RecordField[] fields = record.Fields.GetField(tag);
            if (fieldIndex.Kind != IndexKind.None)
            {
                int i = fieldIndex.ComputeValue(context, fields);

                RecordField field = fields.GetOccurrence(i);
                if (ReferenceEquals(field, null))
                {
                    return;
                }

                fields = new[] {field};
            }

            string[] lines = value.SplitLines()
                .NonEmptyLines()
                .ToArray();
            List<SubField> newSubFields = new List<SubField>();
            foreach (string line in lines)
            {
                SubField subField = new SubField(code, line);
                newSubFields.Add(subField);
            }

            foreach (RecordField field in fields)
            {
                SubField[] subFields = field.GetSubField(code);

                if (subfieldIndex.Kind == IndexKind.None)
                {
                    foreach (SubField subField in subFields)
                    {
                        field.SubFields.Remove(subField);
                    }

                    foreach (SubField subField in newSubFields)
                    {
                        field.SubFields.Add(subField.Clone());
                    }
                }
                else
                {
                    int i = subfieldIndex.ComputeValue(context, subFields);

                    if (i >= subFields.Length)
                    {
                        field.SubFields.AddRange(newSubFields);
                    }
                    else
                    {
                        int position = field.SubFields.IndexOf(subFields[i]);
                        field.SubFields[i].Value = newSubFields[0].Value;

                        for (int j = 1; j < newSubFields.Count; j++)
                        {
                            field.SubFields.Insert
                                (
                                    position + j,
                                    newSubFields[j]
                                );
                        }
                    }
                }
            }
        }

        //=================================================

        /// <summary>
        /// Compare two strings.
        /// </summary>
        public static int CompareStrings
            (
                [CanBeNull] string first,
                [CanBeNull] string second
            )
        {
            int result = string.Compare
                (
                    first,
                    second,
                    StringComparison.CurrentCultureIgnoreCase
                );

            return result;
        }

        //=================================================

        /// <summary>
        /// Whether one string contains another.
        /// </summary>
        public static bool ContainsSubString
            (
                [CanBeNull] string outer,
                [CanBeNull] string inner
            )
        {
            if (string.IsNullOrEmpty(inner))
            {
                return true;
            }
            if (string.IsNullOrEmpty(outer))
            {
                return false;
            }

            outer = outer.ToLower();
            inner = inner.ToLower();

            bool result = outer.Contains(inner);

            return result;
        }

        //=================================================

        /// <summary>
        /// Whether one string contains another.
        /// </summary>
        public static bool ContainsSubStringSensitive
            (
                [CanBeNull] string outer,
                [CanBeNull] string inner
            )
        {
            if (string.IsNullOrEmpty(inner))
            {
                return true;
            }
            if (string.IsNullOrEmpty(outer))
            {
                return false;
            }

            bool result = outer.Contains(inner);

            return result;
        }

        //=================================================

        /// <summary>
        /// Extract numeric value from the input text.
        /// </summary>
        public static double ExtractNumericValue
            (
                [CanBeNull] string input
            )
        {
            if (string.IsNullOrEmpty(input))
            {
                return 0.0;
            }

            Match match = Regex.Match
                (
                    input,
                    "[-]?[0-9]*[\\.]?[0-9]*"
                );
            if (!match.Success)
            {
                return 0.0;
            }

            string value = match.Value;
            double result;
            double.TryParse
                (
                    value,
                    NumberStyles.AllowDecimalPoint
                    | NumberStyles.AllowLeadingSign
                    | NumberStyles.AllowExponent
                    | NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out result
                );

            return result;
        }

        //=================================================

        /// <summary>
        /// Extract numeric values from the input text.
        /// </summary>
        public static double[] ExtractNumericValues
            (
                [CanBeNull] string input
            )
        {
            if (string.IsNullOrEmpty(input))
            {
                return new double[0];
            }

            List<double> result = new List<double>();
            MatchCollection matches = Regex.Matches
                (
                    input,
                    "[-]?[0-9]*[\\.]?[0-9]*"
                );
            foreach (Match match in matches)
            {
                double value;
                if (double.TryParse
                    (
                        match.Value,
                        NumberStyles.AllowDecimalPoint
                        | NumberStyles.AllowLeadingSign
                        | NumberStyles.AllowExponent
                        | NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out value
                    ))
                {
                    result.Add(value);
                }
            }

            return result.ToArray();
        }

        //=================================================

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
                & !result.EndsWith(". ")
                & !result.EndsWith(".  "))
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

        //=================================================

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

        //=================================================

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

        //=================================================

        /// <summary>
        /// Format value like function f does.
        /// </summary>
        public static string FormatLikeF
            (
                double value,
                int arg2,
                int arg3
            )
        {
            int minLength = 1;
            if (arg2 < 0)
            {
                if (arg3 < 0)
                {
                    minLength = 16;
                }
            }
            else
            {
                minLength = arg2;
            }


            bool useE = true;
            int decimalPoints = 0;
            if (arg3 >= 0)
            {
                useE = false;
                decimalPoints = arg3;
            }

            string format = useE
                ? string.Format("E{0}", minLength)
                : string.Format("F{0}", decimalPoints);

            string result = value.ToString
                (
                    format,
                    CultureInfo.InvariantCulture
                )
                .PadLeft
                (
                    minLength,
                    ' '
                );

            return result;
        }

        //=================================================

        /// <summary>
        /// Get array item according to specification
        /// </summary>
        [NotNull]
        public static T[] GetArrayItem<T>
            (
                [NotNull] PftContext context,
                [NotNull] T[] array,
                IndexSpecification index
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(array, "array");

            if (index.Kind == IndexKind.None)
            {
                return array;
            }

            int i = index.ComputeValue(context, array);

            if (i >= 0
                && i < array.Length)
            {
                return new[] { array[i] };
            }

            return new T[0];
        }

        //=================================================

        /// <summary>
        /// Get array of reserved words.
        /// </summary>
        public static string[] GetReservedWords()
        {
            return new[]
            {
                "a",
                "abs",
                "and",
                "blank",
                "break",
                "ceil",
                "div",
                "do",
                "else",
                "empty",
                "end",
                "f",
                "f2",
                "false",
                "fi",
                "floor",
                "for",
                "frac",
                "have",
                "if",
                "l",
                "mdl",
                "mdu",
                "mfn",
                "mhl",
                "mhu",
                "mpl",
                "mpu",
                "not",
                "or",
                "p",
                "pow",
                "proc",
                "ravr",
                "ref",
                "rmax",
                "rmin",
                "round",
                "rsum",
                "s",
                "sign",
                "then",
                "true",
                "trunc",
                "uf",
                "unifor",
                "val",
                "while"
            };
        }

        //=================================================

        /// <summary>
        /// Whether the node collection represents
        /// numeric or string expression.
        /// </summary>
        public static bool IsNumeric
            (
                [NotNull] PftContext context,
                [NotNull] IList<PftNode> nodes
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(nodes, "nodes");

            if (nodes.Count == 0
                || nodes.Count > 1)
            {
                return true;
            }

            return IsNumeric
                (
                    context,
                    nodes[0]
                );
        }

        //=================================================

        /// <summary>
        /// Heuristics: whether given node is
        /// text or numeric.
        /// </summary>
        public static bool IsNumeric
            (
                [NotNull] PftContext context,
                [NotNull] PftNode node
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(node, "node");

            PftVariableReference reference = node as PftVariableReference;
            if (!ReferenceEquals(reference, null)
                && !ReferenceEquals(reference.Name, null))
            {
                PftVariable variable
                    = context.Variables.GetExistingVariable(reference.Name);
                if (!ReferenceEquals(variable, null))
                {
                    return variable.IsNumeric;
                }

                // TODO: some heuristic?
                return false;
            }

            return node is PftNumeric;
        }

        //=================================================

        /// <summary>
        /// Parse field.
        /// </summary>
        [NotNull]
        public static RecordField ParseField
            (
                [NotNull] string line
            )
        {
            Code.NotNullNorEmpty(line, "line");

            return _ParseLine(line);
        }

        //=================================================

        /// <summary>
        /// Prepare text for <see cref="PftUnconditionalLiteral"/>,
        /// <see cref="PftConditionalLiteral"/>,
        /// <see cref="PftRepeatableLiteral"/>.
        /// </summary>
        [CanBeNull]
        public static string PrepareText
            (
                [CanBeNull] string text
            )
        {
            string result = text;

            if (!string.IsNullOrEmpty(text))
            {
                result = text
                    .Replace("\r", string.Empty)
                    .Replace("\n", string.Empty);
            }

            return result;
        }

        //=================================================

        /// <summary>
        /// Set array item according to index specification
        /// </summary>
        [NotNull]
        public static T[] SetArrayItem<T>
            (
                [NotNull] PftContext context,
                [NotNull] T[] array,
                IndexSpecification index,
                [CanBeNull] T value
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(array, "array");

            if (index.Kind == IndexKind.None)
            {
                array = new[] { value };
            }
            else
            {
                int i = index.ComputeValue(context, array);

                if (i >= 0)
                {
                    if (i >= array.Length)
                    {
                        Array.Resize(ref array, i + 1);
                    }
                    array[i] = value;
                }
            }

            return array;
        }

        //=================================================

        #endregion
    }
}
