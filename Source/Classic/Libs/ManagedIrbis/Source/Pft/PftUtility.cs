// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using AM;
using AM.Collections;
using AM.ConsoleIO;
using AM.Logging;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable ConvertClosureToMethodGroup

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

        /// <summary>
        /// Digits.
        /// </summary>
        public static char[] Digits =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        /// <summary>
        /// Digits plus X.
        /// </summary>
        public static char[] DigitsX =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'X'
        };


        /// <summary>
        /// Letters.
        /// </summary>
        public static char[] Letters =
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
            'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
            'y', 'z',

            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
            'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
            'Y', 'Z',

            'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к',
            'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц',
            'ч', 'ш', 'щ', 'ь', 'ы', 'ъ', 'э', 'ю', 'я',

            'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К',
            'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц',
            'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ', 'Э', 'Ю', 'Я',
        };

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

        //=================================================

        /// <summary>
        /// Assign field.
        /// </summary>
        public static void AssignField
            (
                [NotNull] PftContext context,
                int tag,
                IndexSpecification index,
                [CanBeNull] string value
            )
        {
            Code.NotNull(context, "context");

            MarcRecord record = context.Record;
            if (ReferenceEquals(record, null))
            {
                Log.Error
                    (
                        "PftUtility::AssignField: "
                        + "record not set"
                    );

                return;
            }

            RecordField[] fields = record.Fields.GetField(tag);

            if (string.IsNullOrEmpty(value))
            {
                if (index.Kind == IndexKind.None
                    || index.Kind == IndexKind.AllRepeats)
                {
                    record.RemoveField(tag);
                }
                else
                {
                    int i = index.ComputeValue(context, fields);

                    if (i >= 0 && i < fields.Length)
                    {
                        record.Fields.Remove(fields[i]);
                    }
                }

                return;
            }

            string[] lines = value.SplitLines().NonEmptyLines().ToArray();
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

        //=================================================

        /// <summary>
        /// Assign subfield.
        /// </summary>
        public static void AssignSubField
            (
                [NotNull] PftContext context,
                int tag,
                IndexSpecification fieldIndex,
                char code,
                IndexSpecification subfieldIndex,
                [CanBeNull] string value
            )
        {
            Code.NotNull(context, "context");

            code = SubFieldCode.Normalize(code);

            MarcRecord record = context.Record;
            if (ReferenceEquals(record, null))
            {
                Log.Error
                    (
                        "PftUtility::AssignSubField: "
                        + "record not set"
                    );

                return;
            }

            RecordField[] fields = record.Fields.GetField(tag);

            if (ReferenceEquals(value, null))
            {
                // TODO implement properly

                return;
            }

            if (fieldIndex.Kind != IndexKind.None)
            {
                int i = fieldIndex.ComputeValue(context, fields);

                RecordField field = fields.GetOccurrence(i);
                if (ReferenceEquals(field, null))
                {
                    return;
                }

                fields = new[] { field };
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

            int current = 0;
            foreach (RecordField field in fields)
            {
                SubField[] subFields = field.GetSubField(code);

                if (subfieldIndex.Kind == IndexKind.None)
                {
                    foreach (SubField subField in subFields)
                    {
                        field.SubFields.Remove(subField);
                    }

                    if (current < newSubFields.Count)
                    {
                        SubField newSubField = newSubFields[current];
                        if (!ReferenceEquals(newSubField, null))
                        {
                            field.SubFields.Add(newSubField);
                        }
                    }
                    current++;
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
        /// Clone nodes.
        /// </summary>
        [CanBeNull]
        public static PftNodeCollection CloneNodes
            (
                [CanBeNull] this PftNodeCollection nodes,
                [CanBeNull] PftNode parent
            )
        {
            PftNodeCollection result = null;

            if (ReferenceEquals(nodes, null))
            {
                Log.Error
                    (
                        "PftUtility::CloneNodes: "
                        + "nodes are null"
                    );
            }
            else
            {
                result = new PftNodeCollection(parent);

                foreach (PftNode child1 in nodes)
                {
                    PftNode child2 = (PftNode)child1.Clone();
                    result.Add(child2);
                }
            }

            return result;
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
        /// Compile the program.
        /// </summary>
        [NotNull]
        public static PftProgram CompileProgram
            (
                [NotNull] string source
            )
        {
            Code.NotNull(source, "source");

            PftProgram result = ProgramCache.GetProgram(source);
            if (ReferenceEquals(result, null))
            {
                PftLexer lexer = new PftLexer();
                PftTokenList tokens = lexer.Tokenize(source);
                PftParser parser = new PftParser(tokens);
                result = parser.Parse();
                ProgramCache.AddProgram(source, result);
            }

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
                // Original formatter have the bug:
                //
                // if '':'' then '1' else '2' fi,
                // if 'ABC':'A' then '1' else '2' fi,
                // if '':'A' then '1' else '2' fi,
                // if 'A': '' then '1' else '2' fi
                //
                // produces 1122
                //
                // Thus not-empty string DOESNT contains empty one!
                // Bug discovered by Ivan Batrak

                return string.IsNullOrEmpty(outer);
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
                // Original formatter have the bug:
                //
                // if '':'' then '1' else '2' fi,
                // if 'ABC':'A' then '1' else '2' fi,
                // if '':'A' then '1' else '2' fi,
                // if 'A': '' then '1' else '2' fi
                //
                // produces 1122
                //
                // Thus not-empty string DOESNT contains empty one!
                // Bug discovered by Ivan Batrak

                return string.IsNullOrEmpty(outer);
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

#if WINMOBILE || PocketPC

            NumericUtility.TryParseDouble
                (
                    value,
                    out result
                );

#else

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

#endif

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

#if WINMOBILE || PocketPC

                if (NumericUtility.TryParseDouble
                    (
                        match.Value,
                        out value
                    ))
                {
                    result.Add(value);
                }

#else

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

#endif
            }

            return result.ToArray();
        }

        //=================================================

        /// <summary>
        /// Извлекает все слова на латинице и кириллице.
        /// </summary>
        public static string[] ExtractWords
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return StringUtility.EmptyArray;
            }

            List<string> result = new List<string>();
            TextNavigator navigator = new TextNavigator(text);
            StringBuilder builder = new StringBuilder();
            char c;
            while ((c = navigator.ReadChar()) != '\0')
            {
                if (c >= 0x0041 && c < 0x005B
                    || c >= 0x0061 && c < 0x007B
                    || c >= 0x0400 && c < 0x0460)
                {
                    builder.Append(c);
                }
                else
                {
                    if (builder.Length != 0)
                    {
                        result.Add(builder.ToString());
                        builder.Clear();
                    }
                }
            }

            if (builder.Length != 0)
            {
                result.Add(builder.ToString());
            }

            return result.ToArray();
        }

        //=================================================

        /// <summary>
        /// Build text representation of <see cref="FieldSpecification"/>'s.
        /// </summary>
        public static void FieldsToText
            (
                [NotNull] StringBuilder builder,
                [NotNull] IEnumerable<FieldSpecification> fields
            )
        {
            Code.NotNull(builder, "builder");
            Code.NotNull(fields, "fields");

            bool first = true;
            foreach (FieldSpecification field in fields.NonNullItems())
            {
                if (!first)
                {
                    builder.Append(", ");
                }
                builder.Append(field);
                first = false;
            }
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
                    Log.Error
                        (
                            "PftUtiltity::FormatField: "
                            + "unexpected data mode="
                            + mode.ToVisibleString()
                        );

                    throw new ArgumentOutOfRangeException();
            }

            if (uppercase)
            {
                result = IrbisText.ToUpper(result)
                    .ThrowIfNull("IrbisText.ToUpper");
            }

            return result;
        }

        //=================================================

        /// <summary>
        /// Format value like function f() does.
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

            // ibatrak
            // IRBIS uses banker's rounding

            // f(0.5,0,0) = 0
            // f(1.5,0,0) = 2
            // f(2.5,0,0) = 2
            // f(3.5,0,0) = 4
            // f(4.5,0,0) = 4
            // f(5.5,0,0) = 6
            // f(6.5,0,0) = 6
            // f(7.5,0,0) = 8
            // f(8.5,0,0) = 8
            // f(9.5,0,0) = 10
            // f(0.05,0,1) = 0.1
            // f(0.15,0,1) = 0.1
            // f(0.25,0,1) = 0.3
            // f(0.35,0,1) = 0.3
            // f(0.45,0,1) = 0.5
            // f(0.55,0,1) = 0.6
            // f(0.65,0,1) = 0.7
            // f(0.75,0,1) = 0.8
            // f(0.85,0,1) = 0.8
            // f(0.95,0,1) = 0.9
            // f(0.005,0,2) = 0.01
            // f(0.015,0,2) = 0.02
            // f(0.025,0,2) = 0.03
            // f(0.035,0,2) = 0.04
            // f(0.045,0,2) = 0.05
            // f(0.055,0,2) = 0.06
            // f(0.065,0,2) = 0.07
            // f(0.075,0,2) = 0.08
            // f(0.085,0,2) = 0.09
            // f(0.095,0,2) = 0.10

            switch (decimalPoints)
            {
                case 0:
#if WINMOBILE

                    value = Math.Round(value, decimalPoints);

#else

                    value = Math.Round
                        (
                            value,
                            decimalPoints,
                            MidpointRounding.ToEven
                        );
#endif
                    break;

                //case 1:
                //    // ReSharper disable CompareOfFloatsByEqualityOperator

                //    if (value == 0.05)
                //    {
                //        value = 0.1;
                //    }
                //    else if (value == 0.15)
                //    {
                //        value = 0.1;
                //    }
                //    else if (value == 0.25)
                //    {
                //        value = 0.3;
                //    }
                //    else if (value == 0.35)
                //    {
                //        value = 0.3;
                //    }
                //    else if (value == 0.45)
                //    {
                //        value = 0.5;
                //    }
                //    else if (value == 0.55)
                //    {
                //        value = 0.6;
                //    }
                //    else if (value == 0.65)
                //    {
                //        value = 0.7;
                //    }
                //    else if (value == 0.75)
                //    {
                //        value = 0.8;
                //    }
                //    else if (value == 0.85)
                //    {
                //        value = 0.8;
                //    }
                //    else if (value == 0.95)
                //    {
                //        value = 0.9;
                //    }
                //    break;

                //// ReSharper restore CompareOfFloatsByEqualityOperator
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
        /// Format term link for "*" method.
        /// </summary>
        public static bool FormatTermLink
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string database,
                [NotNull] TermLink link
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(link, "link");

            IrbisProvider provider = context.Provider;
            string saveDatabase = provider.Database;
            try
            {
                if (!string.IsNullOrEmpty(database))
                {
                    provider.Database = database;
                }

                MarcRecord record = provider.ReadRecord(link.Mfn);
                if (!ReferenceEquals(record, null))
                {
                    RecordField field = record.Fields.GetField
                        (
                            link.Tag,
                            link.Occurrence - 1
                        );
                    if (!ReferenceEquals(field, null))
                    {
                        string output = FormatField
                            (
                                field,
                                context.FieldOutputMode,
                                context.UpperMode
                            );
                        if (!string.IsNullOrEmpty(output))
                        {
                            context.WriteAndSetFlag(node, output);

                            return true;
                        }
                    }
                }
            }
            finally
            {
                provider.Database = saveDatabase;
            }

            return false;
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

            if (i >= 0 && i < array.Length)
            {
                return new[] { array[i] };
            }

            return new T[0];
        }

        //=================================================

        /// <summary>
        /// Get count of the fields.
        /// </summary>
        public static int GetFieldCount
            (
                [NotNull] PftContext context,
                params int[] tags
            )
        {
            Code.NotNull(context, "context");

            int result = 0;

            MarcRecord record = context.Record;
            if (!ReferenceEquals(record, null))
            {
                foreach (int tag in tags)
                {
                    int count = record.Fields.GetFieldCount(tag);
                    result = Math.Max(count, result);
                }
            }

            return result;
        }

        //=================================================

        /// <summary>
        /// Get value of the field.
        /// </summary>
        [NotNull]
        public static string[] GetFieldValue
            (
                [NotNull] PftContext context,
                int tag,
                IndexSpecification index
            )
        {
            Code.NotNull(context, "context");

            MarcRecord record = context.Record;
            if (ReferenceEquals(record, null))
            {
                return StringUtility.EmptyArray;
            }

            RecordField[] fields = record.Fields.GetField(tag);
            string[] result = fields.Select
                (
                    field => field.ToText()
                )
                .ToArray();

            result = GetArrayItem
                (
                    context,
                    result,
                    index
                );

            return result;
        }

        //=================================================

        /// <summary>
        /// Get value of the field.
        /// </summary>
        [CanBeNull]
        public static string GetFieldValue
            (
                [NotNull] PftContext context,
                [NotNull] RecordField field,
                char subFieldCode,
                IndexSpecification subFieldRepeat
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(field, "field");

            string result = null;

            if (subFieldCode == SubField.NoCode)
            {
                result = field.FormatField
                    (
                        context.FieldOutputMode,
                        context.UpperMode
                    );
            }
            else if (subFieldCode == '*')
            {
                result = field.GetValueOrFirstSubField();
            }
            else
            {
                SubField[] subFields = field.GetSubField(subFieldCode);
                subFields = GetArrayItem
                    (
                        context,
                        subFields,
                        subFieldRepeat
                    );
                SubField subField = subFields.GetOccurrence(0);
                if (!ReferenceEquals(subField, null))
                {
                    result = subField.Value;
                }
            }

            return result;
        }

        //=================================================

        private static readonly string[] _reservedWords =
        {
            "a",
            "abs",
            "absent",
            "all",
            "any",
            "and",
            "blank",
            "break",
            "ceil",
            "cseval",
            "div",
            "do",
            "else",
            "empty",
            "end",
            "eval",
            "f",
            "fmt",
            "false",
            "fi",
            "first",
            "floor",
            "for",
            "frac",
            "global",
            "have",
            "if",
            "l",
            "last",
            "local",
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
            "parallel",
            "pow",
            "present",
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
            "while",
            "with",
            "если",
            "иначе",
            "то"
        };

        /// <summary>
        /// Get array of reserved words.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static string[] GetReservedWords()
        {
            return _reservedWords;
        }

        //=================================================

        /// <summary>
        /// Get value of the subfield.
        /// </summary>
        [NotNull]
        public static string[] GetSubFieldValue
            (
                [NotNull] PftContext context,
                int tag,
                IndexSpecification fieldIndex,
                char code,
                IndexSpecification subfieldIndex
            )
        {
            Code.NotNull(context, "context");

            MarcRecord record = context.Record;
            if (ReferenceEquals(record, null))
            {
                Log.Error
                    (
                        "PftUtility::GetSubFieldValue: "
                        + "record not set"
                    );

                return StringUtility.EmptyArray;
            }

            code = SubFieldCode.Normalize(code);

            RecordField[] fields = record.Fields.GetField(tag);
            fields = GetArrayItem
                (
                    context,
                    fields,
                    fieldIndex
                );

            string[] result = fields.Select
                (
                    subField => subField.GetFirstSubFieldValue(code)
                                ?? string.Empty
                )
                .ToArray();

            result = GetArrayItem
                (
                    context,
                    result,
                    subfieldIndex
                );

            return result;
        }

        //=================================================

        /// <summary>
        /// Whether the node is complex expression?
        /// </summary>
        public static bool IsComplexExpression
            (
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");

            if (node.ComplexExpression)
            {
                return true;
            }

            NonNullCollection<PftNode> children
                = node.GetDescendants<PftNode>();
            bool result = children.Any(item => item.ComplexExpression);

            return result;
        }

        //=================================================

        /// <summary>
        /// Whether the node is complex expression?
        /// </summary>
        public static bool IsComplexExpression
            (
                [NotNull] IEnumerable<PftNode> nodes
            )
        {
            Code.NotNull(nodes, "nodes");

            bool result = nodes.Any(item => IsComplexExpression(item));

            return result;
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
        /// Build text representation of <see cref="PftNode"/>'s.
        /// </summary>
        public static void NodesToText
            (
                [NotNull] StringBuilder builder,
                [NotNull] IEnumerable<PftNode> nodes
            )
        {
            Code.NotNull(builder, "builder");
            Code.NotNull(nodes, "nodes");

            bool first = true;
            foreach (PftNode node in nodes.NonNullItems())
            {
                if (!first)
                {
                    builder.Append(' ');
                }
                builder.Append(node);
                first = false;
            }
        }

        //=================================================

        /// <summary>
        /// Build text representation of <see cref="PftNode"/>'s.
        /// </summary>
        public static void NodesToText
            (
                [NotNull] string delimiter,
                [NotNull] StringBuilder builder,
                [NotNull] IEnumerable<PftNode> nodes
            )
        {
            Code.NotNull(builder, "builder");
            Code.NotNull(nodes, "nodes");

            bool first = true;
            foreach (PftNode node in nodes.NonNullItems())
            {
                if (!first)
                {
                    builder.Append(delimiter);
                }
                builder.Append(node);
                first = false;
            }
        }

        //=================================================

        /// <summary>
        /// Parse the field.
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
        /// Whether the node requires server connection to evaluate.
        /// </summary>
        public static bool RequiresConnection
            (
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");

            if (node.RequiresConnection)
            {
                return true;
            }

            NonNullCollection<PftNode> children
                = node.GetDescendants<PftNode>();
            bool result = children.Any(item => item.RequiresConnection);

            return result;
        }

        //=================================================

        /// <summary>
        /// Whether the node requires server connection to evaluate.
        /// </summary>
        public static bool RequiresConnection
            (
                [NotNull] IEnumerable<PftNode> nodes
            )
        {
            Code.NotNull(nodes, "nodes");

            bool result = nodes.Any(item => RequiresConnection(item));

            return result;
        }

        //=================================================

        /// <summary>
        /// Extract substring in safe manner.
        /// </summary>
        [CanBeNull]
        internal static string SafeSubString
            (
                [CanBeNull] string text,
                int offset,
                int length
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (offset < 0)
            {
                offset = 0;
            }
            if (length <= 0)
            {
                return string.Empty;
            }
            if (offset >= text.Length)
            {
                return string.Empty;
            }

            try
            {
                checked
                {
                    if (offset + length > text.Length)
                    {
                        length = text.Length - offset;
                        if (length <= 0)
                        {
                            return string.Empty;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "PftUtility::SafeSubString",
                        exception
                    );

                Debug.WriteLine(exception);

                throw;
            }

            string result;

            try
            {
                result = text.Substring
                    (
                        offset,
                        length
                    );
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "PftUtility::SafeSubString",
                        exception
                    );

                Debug.WriteLine(exception);

                ConsoleInput.WriteLine(exception.ToString());

                throw;
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
            else if (index.Kind == IndexKind.AllRepeats)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = value;
                }
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
