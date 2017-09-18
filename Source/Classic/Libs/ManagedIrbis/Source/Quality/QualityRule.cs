// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* QualityRule.cs -- abstract base class for all the quality rules
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Linq;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Quality
{
    /// <summary>
    /// Abstract base class for all the quality rules.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class QualityRule
    {
        #region Properties

        /// <summary>
        /// Specification of the fields to check.
        /// </summary>
        [NotNull]
        public abstract string FieldSpec { get; }

        /// <summary>
        /// Клиент.
        /// </summary>
        [NotNull]
        [JsonIgnore]
        public IrbisConnection Connection
        {
            get { return Context.Connection; }
        }

        /// <summary>
        /// Текущий контекст.
        /// </summary>
        [NotNull]
        [JsonIgnore]
        public RuleContext Context { get; protected set; }

        /// <summary>
        /// Текущая проверяемая запись.
        /// </summary>
        [NotNull]
        [JsonIgnore]
        public MarcRecord Record { get { return Context.Record; } }

        /// <summary>
        /// Накопленный отчёт.
        /// </summary>
        [NotNull]
        [JsonIgnore]
        public RuleReport Report { get; protected set; }

        /// <summary>
        /// Рабочий лист.
        /// </summary>
        [CanBeNull]
        [JsonIgnore]
        public string Worksheet
        {
            get { return Record.FM(920); }
        }

        #endregion

        #region Private members

        /// <summary>
        /// Add detected defect.
        /// </summary>
        protected void AddDefect
            (
                int tag,
                int damage,
                [NotNull] string format,
                params object[] args
            )
        {
            Code.Positive(tag, "tag");
            Code.Nonnegative(damage, "damage");
            Code.NotNullNorEmpty(format, "format");

            FieldDefect defect = new FieldDefect
            {
                Field = tag,
                Damage = damage,
                Message = string.Format(format, args)
            };
            Report.Defects.Add(defect);
        }

        /// <summary>
        /// Add detected defect.
        /// </summary>
        protected void AddDefect
            (
                [NotNull] RecordField field,
                int damage,
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNull(field, "field");
            Code.Nonnegative(damage, "damage");
            Code.NotNullNorEmpty(format, "format");

            FieldDefect defect = new FieldDefect
            {
                Field = field.Tag,
                Repeat = field.Repeat,
                Value = field.ToText(),
                Damage = damage,
                Message = string.Format(format, args)
            };
            Report.Defects.Add(defect);
        }

        /// <summary>
        /// Add detected defect.
        /// </summary>
        protected void AddDefect
            (
                [NotNull] RecordField field,
                [NotNull] SubField subfield,
                int damage,
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNull(field, "field");
            Code.NotNull(subfield, "subfield");
            Code.Nonnegative(damage, "damage");
            Code.NotNullNorEmpty(format, "format");

            FieldDefect defect = new FieldDefect
            {
                Field = field.Tag,
                Repeat = field.Repeat,
                Subfield = subfield.Code.ToString(),
                Value = subfield.Value,
                Damage = damage,
                Message = string.Format(format, args)
            };
            Report.Defects.Add(defect);
        }

        /// <summary>
        /// Begin the record checking.
        /// </summary>
        protected void BeginCheck
            (
                [NotNull] RuleContext context
            )
        {
            Code.NotNull(context, "context");

            Context = context;
            Report = new RuleReport();
        }

        /// <summary>
        /// Cache the menu.
        /// </summary>
        [NotNull]
        protected MenuFile CacheMenu
            (
                [NotNull] string name,
                [CanBeNull] MenuFile menu
            )
        {
            Code.NotNullNorEmpty(name, "name");

            menu = menu ?? 
                MenuFile.ReadFromServer
                (
                    Connection,
                    new FileSpecification(IrbisPath.MasterFile, name)
                );

            return menu;
        }

        /// <summary>
        /// Check the value against the menu.
        /// </summary>
        protected bool CheckForMenu
            (
                [CanBeNull] MenuFile menu,
                [CanBeNull] string value
            )
        {
            if (ReferenceEquals(menu, null))
            {
                return true;
            }
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            MenuEntry entry = menu.GetEntrySensitive(value);

            return !ReferenceEquals(entry, null);
        }

        /// <summary>
        /// Get text at specified position in the string.
        /// </summary>
        protected static string GetTextAtPosition
            (
                [NotNull] string text,
                int position
            )
        {
            int length = text.Length;
            int start = Math.Max(0, position - 1);
            int stop = Math.Min(length - 1, position + 2);
            while (start >= 0 && text[start] == ' ')
            {
                start--;
            }
            while (start >= 0 && text[start] != ' ')
            {
                start--;
            }
            start = Math.Max(0, start);
            while (stop < length && text[stop] == ' ')
            {
                stop++;
            }
            while (stop < length && text[stop] != ' ')
            {
                stop++;
            }
            stop = Math.Min(length - 1, stop);

            return text.Substring
                (
                    start,
                    stop - start + 1
                )
                .Trim();
        }

        /// <summary>
        /// Show double whitespace in the text.
        /// </summary>
        protected static string ShowDoubleWhiteSpace
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            int position = text.IndexOf
                (
                    "  ",
                    StringComparison.Ordinal
                );

            return GetTextAtPosition
                (
                    text,
                    position
                );
        }

        /// <summary>
        /// Check the subfield for whitespace.
        /// </summary>
        protected void CheckWhitespace
            (
                [NotNull] RecordField field,
                [NotNull] SubField subfield
            )
        {
            Code.NotNull(field, "field");
            Code.NotNull(subfield, "subfield");

            string text = subfield.Value;
            
            if (string.IsNullOrEmpty(text))
            {
                AddDefect
                    (
                        field,
                        subfield,
                        1,
                        "Пустое подполе {0}^{1}",
                        field.Tag,
                        subfield.Code
                    );
                return;
            }

            if (text.StartsWith(" "))
            {
                AddDefect
                    (
                        field,
                        subfield,
                        1,
                        "Подполе {0}^{1} начинается с пробела",
                        field.Tag,
                        subfield.Code
                    );
            }

            if (text.EndsWith(" "))
            {
                AddDefect
                    (
                        field,
                        subfield,
                        1,
                        "Подполе {0}^{1} оканчивается пробелом",
                        field.Tag,
                        subfield.Code
                    );
            }

            if (text.Contains("  "))
            {
                AddDefect
                    (
                        field,
                        subfield,
                        1,
                        "Подполе {0}^{1} содержит двойной пробел: {2}",
                        field.Tag,
                        subfield.Code,
                        ShowDoubleWhiteSpace(text)
                    );
            }
        }

        /// <summary>
        /// Check for whitespace.
        /// </summary>
        protected void CheckWhitespace
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            string text = field.Value;
            if (!string.IsNullOrEmpty(text))
            {
                if (text.StartsWith(" "))
                {
                    AddDefect
                        (
                            field,
                            1,
                            "Поле {0} начинается с пробела",
                            field.Tag
                        );
                }
                if (text.EndsWith(" "))
                {
                    AddDefect
                        (
                            field,
                            1,
                            "Поле {0} оканчивается пробелом",
                            field.Tag
                        );
                }
                if (text.Contains("  "))
                {
                    AddDefect
                        (
                            field,
                            1,
                            "Поле {0} содержит двойной пробел: {1}",
                            field.Tag,
                            ShowDoubleWhiteSpace(text)
                        );
                }
            }

            foreach (SubField subfield in field.SubFields)
            {
                CheckWhitespace
                    (
                        field,
                        subfield
                    );
            }
        }

        /// <summary>
        /// End the record checking.
        /// </summary>
        [NotNull]
        protected RuleReport EndCheck()
        {
            Report.Damage = Report.Defects
                .Sum(defect => defect.Damage);

            return Report;
        }

        /// <summary>
        /// Whether the current working list is ASP?
        /// </summary>
        protected bool IsAsp()
        {
            return Worksheet.SameString("ASP");
        }

        /// <summary>
        /// Whether the current working list is book-specific:
        /// PAZK, SPEC or PVK?
        /// </summary>
        protected bool IsBook()
        {
            string worksheet = Worksheet;
            return worksheet.SameString("PAZK")
                   || worksheet.SameString("SPEC")
                   || worksheet.SameString("PVK");
        }

        /// <summary>
        /// Whether the current working list is PAZK?
        /// </summary>
        protected bool IsPazk()
        {
            return Worksheet.SameString("PAZK");
        }

        /// <summary>
        /// Whether the current working list is SPEC?
        /// </summary>
        protected bool IsSpec()
        {
            return Worksheet.SameString("SPEC");
        }

        /// <summary>
        /// Get fields of the current record for the rule
        /// according the <see cref="FieldSpec"/>.
        /// </summary>
        [NotNull]
        protected RecordField[] GetFields()
        {
            return Record.Fields
                .GetFieldBySpec(FieldSpec);
        }

        /// <summary>
        /// Must not contain subfields.
        /// </summary>
        protected void MustNotContainSubfields
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            if (field.SubFields.Count != 0)
            {
                AddDefect
                    (
                        field,
                        20,
                        "Поле {0} содержит подполя",
                        field.Tag
                    );
            }
        }

        /// <summary>
        /// Asserts that the field must not contain plain text value.
        /// </summary>
        protected void MustNotContainText
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            if (!string.IsNullOrEmpty(field.Value))
            {
                AddDefect
                    (
                        field,
                        20,
                        "Поле {0} должно состоять только из подполей",
                        field.Tag
                    );
            }
        }

        /// <summary>
        /// Asserts that the field must not contain
        /// repeatable subfields.
        /// </summary>
        protected void MustNotRepeatSubfields
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            var grouped = field.SubFields
                .GroupBy(sf => sf.CodeString.ToLowerInvariant());
            foreach (var grp in grouped)
            {
                if (grp.Count() != 1)
                {
                    AddDefect
                        (
                            field,
                            20,
                            "Подполе {0}^{1} повторяется",
                            field.Tag,
                            grp.Key
                        );
                }
            }
        }

        /// <summary>
        /// Asserts that the field must be unique.
        /// </summary>
        protected void MustBeUniqueField
            (
                [NotNull] RecordField[] fields
            )
        {
            Code.NotNull(fields, "fields");

            var grouped = fields
                .GroupBy
                (
                    f => f.Value
                        .ThrowIfNull("field.Value")
                        .ToLowerInvariant()
                )
                ;
            foreach (var grp in grouped)
            {
                if (grp.Count() != 1)
                {
                    AddDefect
                        (
                            grp.First(),
                            20,
                            "Поле {0} содержит повторяющееся значение {1}",
                            grp.First().Tag,
                            grp.Key
                        );
                }
            }
        }

        /// <summary>
        /// Asserts that her subfield must be non-empty.
        /// </summary>
        protected void MustBeNonEmptySubfield
            (
                [NotNull] RecordField field,
                char code
            )
        {
            Code.NotNull(field, "field");

            var selected = field.SubFields
                .GetSubField(new[] {code})
                .Where(sf => string.IsNullOrEmpty(sf.Value));
            foreach (SubField subField in selected)
            {
                AddDefect
                    (
                        field,
                        subField,
                        5,
                        "Подполе {0}^{1} пустое",
                        field.Tag,
                        subField.Code
                    );
            }
        }

        /// <summary>
        /// Asserts that subfields of the fields must be unique.
        /// </summary>
        protected void MustBeUniqueSubfield
            (
                [NotNull] RecordField[] fields,
                char code
            )
        {
            Code.NotNull(fields, "fields");

            var grouped = fields
                .SelectMany(f => f.SubFields)
                .GetSubField(new[] {code})
                .GroupBy
                (
                    sf => sf.Value
                        .ThrowIfNull("field.Value")
                        .ToLowerInvariant()
                );
            foreach (var grp in grouped)
            {
                if (grp.Count() != 1)
                {
                    AddDefect
                        (
                            fields[0],
                            grp.First(),
                            5,
                            "Подполе {0}^{1} содержит неуникальное значение {2}",
                            fields[0].Tag,
                            grp.First().Code,
                            grp.Key
                        );
                }
            }
        }

        /// <summary>
        /// Asserts that subfields of the fields must be unique.
        /// </summary>
        protected void MustBeUniqueSubfield
            (
                [NotNull] RecordField[] fields,
                params char[] codes
            )
        {
            Code.NotNull(fields, "fields");

            foreach (char code in codes)
            {
                MustBeUniqueSubfield
                    (
                        fields,
                        code
                    );
            }
        }

        /// <summary>
        /// Asserts that the field must not contain whitespace.
        /// </summary>
        protected void MustNotContainWhitespace
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            string text = field.Value;
            if (!string.IsNullOrEmpty(text)
                && text.ContainsWhitespace())
            {
                AddDefect
                    (
                        field,
                        3,
                        "Поле {0} содержит пробельные символы",
                        field.Tag
                    );
            }
        }

        /// <summary>
        /// Asserts that the subfield must not contain whitespace.
        /// </summary>
        protected void MustNotContainWhitespace
            (
                [NotNull] RecordField field,
                [NotNull] SubField subField
            )
        {
            Code.NotNull(field, "field");
            Code.NotNull(subField, "subField");

            string text = subField.Value;
            if (!string.IsNullOrEmpty(text)
                && text.ContainsWhitespace())
            {
                AddDefect
                    (
                        field,
                        subField,
                        3,
                        "Подполе {0}^{1} содержит пробельные символы",
                        field.Tag,
                        subField.Code
                    );
            }
        }

        /// <summary>
        /// Asserts that subfields of the field must not contain whitespace.
        /// </summary>
        protected void MustNotContainWhitespace
            (
                [NotNull] RecordField field,
                params char[] codes
            )
        {
            Code.NotNull(field, "field");

            foreach (char code in codes)
            {
                SubField[] subFields = field.GetSubField(code);
                foreach (SubField subField in subFields)
                {
                    MustNotContainWhitespace
                        (
                            field,
                            subField
                        );
                }
            }
        }

        /// <summary>
        /// Asserts that the field must not contain bad characters.
        /// </summary>
        protected void MustNotContainBadCharacters
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            string text = field.Value;
            if (!string.IsNullOrEmpty(text))
            {
                int position = RuleUtility.BadCharacterPosition(text);
                if (position >= 0)
                {
                    AddDefect
                        (
                            field,
                            3,
                            "Поле {0} содержит запрещённые символы: {1}",
                            GetTextAtPosition(text, position)
                        );
                }
            }
        }

        /// <summary>
        /// Asserts that the subfield must not contain bad characters.
        /// </summary>
        protected void MustNotContainBadCharacters
            (
                [NotNull] RecordField field,
                [NotNull] SubField subField
            )
        {
            Code.NotNull(field, "field");
            Code.NotNull(subField, "subField");

            string text = subField.Value;
            if (!string.IsNullOrEmpty(text))
            {
                int position = RuleUtility.BadCharacterPosition(text);
                if (position >= 0)
                {
                    AddDefect
                        (
                            field,
                            subField,
                            3,
                            "Подполе {0}^{1} содержит "
                            + "запрещённые символы: {2}",
                            field.Tag,
                            subField.Code,
                            GetTextAtPosition(text, position)
                        );
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Check the record.
        /// </summary>
        [NotNull]
        public abstract RuleReport CheckRecord
            (
                [NotNull] RuleContext context
            );

        #endregion
    }
}
