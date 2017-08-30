// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AuthorInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class AuthorInfo
    {
        #region Properties

        /// <summary>
        /// Empty array of the <see cref="AuthorInfo"/>.
        /// </summary>
        public static readonly AuthorInfo[] EmptyArray = new AuthorInfo[0];

        /// <summary>
        /// Known tags.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static string[] AllKnownTags { get { return _allKnownTags; } }

        /// <summary>
        /// Known tags.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static string[] KnownTags1 { get { return _knownTags1; } }

        /// <summary>
        /// Known tags.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static string[] KnownTags2 { get { return _knownTags2; } }

        /// <summary>
        /// Known tags.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static string[] KnownTags3 { get { return _knownTags3; } }

        /// <summary>
        /// Known tags.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static string[] KnownTags4 { get { return _knownTags4; } }

        /// <summary>
        /// Known tags.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static string[] KnownTags5 { get { return _knownTags5; } }

        /// <summary>
        /// Фамилия. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("familyName")]
        [JsonProperty("familyName")]
        [Description("Фамилия. Подполе a.")]
        [DisplayName("Фамилия (без инициалов)")]
        public string FamilyName { get; set; }

        /// <summary>
        /// Инициалы (сокращение). Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("initials")]
        [JsonProperty("initials")]
        [Description("Инициалы (сокращение). Подполе b.")]
        [DisplayName("Инициалы (сокращенные)")]
        public string Initials { get; set; }

        /// <summary>
        /// Расширение инициалов (имя и отчество). Подполе g.
        /// </summary>
        [CanBeNull]
        [SubField('g')]
        [XmlAttribute("fullName")]
        [JsonProperty("fullName")]
        [Description("Расширение инициалов (имя и отчество). Подполе g.")]
        [DisplayName("Расширение инициалов (имя и отчество)")]
        public string FullName { get; set; }

        /// <summary>
        /// Инвертирование имени недопустимо? Подполе 9.
        /// </summary>
        [SubField('9')]
        [XmlAttribute("cantBeInverted")]
        [JsonProperty("cantBeInverted")]
        [Description("Инвертирование имени недопустимо? Подполе 9.")]
        [DisplayName("Инвертирование имени недопустимо")]
        public bool CantBeInverted { get; set; }

        /// <summary>
        /// Неотъемлемая часть имени (отец, сын, младший, старший
        /// и т. п.). Подполе 1.
        /// </summary>
        [CanBeNull]
        [SubField('1')]
        [XmlAttribute("postfix")]
        [JsonProperty("postfix")]
        [Description("Неотъемлемая часть имени. Подполе 1.")]
        [DisplayName("Неотъемлемая часть имени")]
        public string Postfix { get; set; }

        /// <summary>
        /// Дополнения к имени кроме дат (род деятельности, звание,
        /// титул и т. д.). Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("appendix")]
        [JsonProperty("appendix")]
        [Description("Дополнения к имени кроме дат. Подполе c.")]
        [DisplayName("Дополнения к имени кроме дат")]
        public string Appendix { get; set; }

        /// <summary>
        /// Династический номер (римские цифры). Подполе d.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("number")]
        [JsonProperty("number")]
        [Description("Династический номер (римские цифры). Подполе d.")]
        [DisplayName("Династический номер (римские цифры)")]
        public string Number { get; set; }

        /// <summary>
        /// Даты жизни. Подполе f.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlAttribute("dates")]
        [JsonProperty("dates")]
        [Description("Даты жизни. Подполе f.")]
        [DisplayName("Даты жизни")]
        public string Dates { get; set; }

        /// <summary>
        /// Разночтение фамилии. Подполе r.
        /// </summary>
        [CanBeNull]
        [SubField('r')]
        [XmlAttribute("variant")]
        [JsonProperty("variant")]
        [Description("Разночтение фамилии. Подполе r.")]
        [DisplayName("Разночтение фамилии")]
        public string Variant { get; set; }

        /// <summary>
        /// Место работы автора. Подполе p.
        /// </summary>
        [CanBeNull]
        [SubField('p')]
        [XmlAttribute("workplace")]
        [JsonProperty("workplace")]
        [Description("Место работы автора. Подполе p.")]
        [DisplayName("Место работы автора")]
        public string WorkPlace { get; set; }

        /// <summary>
        /// Associated field.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [DisplayName("Поле с подполями")]
        public RecordField Field { get; private set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private static readonly string[] _allKnownTags =
        {
            "330", "391", "454", "470", "481", "488", "600",
            "700", "701", "702", "922", "925", "926", "961", "970"
        };

        private static readonly string[] _knownTags1 =
            { "391", "470", "700", "701", "702", "926", "961", "970" };

        private static readonly string[] _knownTags2 =
            { "330", "922", "925" };

        private static readonly string[] _knownTags3 =
            { "481", "488" };

        private static readonly string[] _knownTags4 =
            { "600" };

        private static readonly string[] _knownTags5 =
            { "454" };

        private static readonly char[] _first330 =
            { 'f', '?', 'x', '=' };

        private static readonly char[] _second330 =
            { '2', ',', '<', '+' };

        private static readonly char[] _third330 =
            { '3', ';', '>', '-' };

        private static readonly char[] _first481 =
            { 'x', '?', '9', '=' };

        private static readonly char[] _first454 =
            { 'd', '\0', 'x', '\0' };

        private static readonly char[] _second454 =
            { 'e', '\0', '<', '\0' };

        private static readonly char[] _third454 =
            { 'f', '\0', '>', '\0' };

        private static readonly char[] _delimiters =
            { ' ', ',' };

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the <see cref="AuthorInfo"/>
        /// to the <see cref="RecordField"/>.
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            string tag = field.Tag.ThrowIfNull("field.Tag");
            if (tag.OneOf(KnownTags1))
            {
                ApplyToField700(field);
            }
            else if (tag.OneOf(KnownTags2))
            {
                ApplyToField330(field);
            }
            else if (tag.OneOf(KnownTags3))
            {
                ApplyToField481(field);
            }
            else if (tag.OneOf(KnownTags4))
            {
                ApplyToField600(field);
            }
            else if (tag.OneOf(KnownTags5))
            {
                ApplyToField454(field);
            }
            else
            {
                throw new IrbisException("Don't know to handle the field");
            }
        }

        /// <summary>
        /// Apply the <see cref="AuthorInfo"/>
        /// to the <see cref="RecordField"/>.
        /// </summary>
        public void ApplyToField700
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField('a', FamilyName)
                .ApplySubField('b', Initials)
                .ApplySubField('g', FullName)
                .ApplySubField('9', CantBeInverted ? "1" : null)
                .ApplySubField('1', Postfix)
                .ApplySubField('c', Appendix)
                .ApplySubField('d', Number)
                .ApplySubField('f', Dates)
                .ApplySubField('r', Variant)
                .ApplySubField('p', WorkPlace);
        }

        /// <summary>
        /// Apply the <see cref="AuthorInfo"/>
        /// to the <see cref="RecordField"/>.
        /// </summary>
        public void ApplyToField600
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            string withInitials = FamilyName;
            if (!string.IsNullOrEmpty(Initials))
            {
                withInitials = withInitials + " " + Initials;
            }

            field
                .ApplySubField('a', withInitials)
                .ApplySubField('g', FullName)
                .ApplySubField('9', CantBeInverted ? "1" : null)
                .ApplySubField('1', Postfix)
                .ApplySubField('c', Appendix)
                .ApplySubField('d', Number)
                .ApplySubField('f', Dates)
                .ApplySubField('r', Variant)
                .ApplySubField('p', WorkPlace);
        }

        /// <summary>
        /// Apply the <see cref="AuthorInfo"/>
        /// to the <see cref="RecordField"/>.
        /// </summary>
        public void ApplyToField330
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            if (!ApplyOneAuthor(field, _first330))
            {
                if (!ApplyOneAuthor(field, _second330))
                {
                    ApplyOneAuthor(field, _third330);
                }
            }
        }

        /// <summary>
        /// Apply the <see cref="AuthorInfo"/>
        /// to the <see cref="RecordField"/>.
        /// </summary>
        public void ApplyToField454
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            if (!ApplyOneAuthor(field, _first454))
            {
                if (!ApplyOneAuthor(field, _second454))
                {
                    ApplyOneAuthor(field, _third454);
                }
            }
        }

        /// <summary>
        /// Apply the <see cref="AuthorInfo"/>
        /// to the <see cref="RecordField"/>.
        /// </summary>
        public void ApplyToField481
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            if (!ApplyOneAuthor(field, _first481))
            {
                if (!ApplyOneAuthor(field, _second330))
                {
                    ApplyOneAuthor(field, _third330);
                }
            }
        }

        /// <summary>
        /// Apply the info to one of authors.
        /// </summary>
        public bool ApplyOneAuthor
            (
                [NotNull] RecordField field,
                [NotNull] char[] subFields
            )
        {
            Code.NotNull(field, "field");
            Code.NotNull(subFields, "subFields");
            if (subFields.Length != 4)
            {
                throw new IrbisException();
            }

            string withInitials = field.GetFirstSubFieldValue(subFields[0]);
            if (string.IsNullOrEmpty(withInitials))
            {
                return false;
            }

            TextNavigator navigator = new TextNavigator(withInitials);
            string familyName = navigator.ReadUntil(_delimiters);
            if (!familyName.SameString(FamilyName))
            {
                return false;
            }

            withInitials = FamilyName;
            if (!string.IsNullOrEmpty(Initials))
            {
                withInitials = withInitials + " " + Initials;
            }

            field
                .ApplySubField(subFields[0], withInitials)
                .ApplySubField(subFields[1], FullName)
                .ApplySubField(subFields[2], CantBeInverted ? "1" : null)
                .ApplySubField(subFields[3], WorkPlace);

            return true;
        }

        /// <summary>
        /// Extract family name from the text.
        /// </summary>
        [CanBeNull]
        public static string ExtractFamilyName
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            TextNavigator navigator = new TextNavigator(text);
            string result = navigator.ReadUntil(_delimiters);

            return result;
        }

        /// <summary>
        /// Parse the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static AuthorInfo[] ParseRecord
            (
                [NotNull] MarcRecord record,
                [NotNull][ItemNotNull] string[] tags
            )
        {
            Code.NotNull(record, "record");
            Code.NotNull(tags, "tags");

            List<AuthorInfo> result = new List<AuthorInfo>();
            foreach (RecordField field in record.Fields)
            {
                if (field.Tag.OneOf(tags))
                {
                    AuthorInfo[] authors = ParseField(field);
                    result.AddRange(authors);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static AuthorInfo[] ParseField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            AuthorInfo one;
            string tag = field.Tag.ThrowIfNull("field.Tag");
            if (tag.OneOf(KnownTags1))
            {
                one = ParseField700(field);
                if (!ReferenceEquals(one, null))
                {
                    return new[] { one };
                }

                return EmptyArray;
            }
            if (tag.OneOf(KnownTags2))
            {
                return ParseField330(field);
            }
            if (tag.OneOf(KnownTags3))
            {
                return ParseField481(field);
            }
            if (tag.OneOf(KnownTags4))
            {
                one = ParseField600(field);
                if (!ReferenceEquals(one, null))
                {
                    return new[] {one};
                }

                return EmptyArray;
            }
            if (tag.OneOf(KnownTags5))
            {
                return ParseField454(field);
            }

            throw new IrbisException("Don't know how to handle field");
        }

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        [CanBeNull]
        public static AuthorInfo ParseField700
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            string familyName = field.GetFirstSubFieldValue('a');
            if (string.IsNullOrEmpty(familyName))
            {
                return null;
            }

            AuthorInfo result = new AuthorInfo
            {
                FamilyName = familyName,
                Initials = field.GetFirstSubFieldValue('b'),
                FullName = field.GetFirstSubFieldValue('g'),
                CantBeInverted = !string.IsNullOrEmpty
                    (
                        field.GetFirstSubFieldValue('9')
                    ),
                Postfix = field.GetFirstSubFieldValue('1'),
                Appendix = field.GetFirstSubFieldValue('c'),
                Number = field.GetFirstSubFieldValue('d'),
                Dates = field.GetFirstSubFieldValue('f'),
                Variant = field.GetFirstSubFieldValue('r'),
                WorkPlace = field.GetFirstSubFieldValue('p'),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        [CanBeNull]
        public static AuthorInfo ParseField600
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            string withInitials = field.GetFirstSubFieldValue('a');
            if (string.IsNullOrEmpty(withInitials))
            {
                return null;
            }
            TextNavigator navigator = new TextNavigator(withInitials);
            string familyName = navigator.ReadUntil(_delimiters);
            navigator.SkipChar(_delimiters);
            string initials = navigator.GetRemainingText();

            AuthorInfo result = new AuthorInfo
            {
                FamilyName = familyName,
                Initials = initials,
                FullName = field.GetFirstSubFieldValue('g'),
                CantBeInverted = !string.IsNullOrEmpty
                    (
                        field.GetFirstSubFieldValue('9')
                    ),
                Postfix = field.GetFirstSubFieldValue('1'),
                Appendix = field.GetFirstSubFieldValue('c'),
                Number = field.GetFirstSubFieldValue('d'),
                Dates = field.GetFirstSubFieldValue('f'),
                Variant = field.GetFirstSubFieldValue('r'),
                WorkPlace = field.GetFirstSubFieldValue('p'),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static AuthorInfo[] ParseField330
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            // TODO parse other authors

            List<AuthorInfo> result = new List<AuthorInfo>();

            AuthorInfo first = ParseOneAuthor(field, _first330);
            if (!ReferenceEquals(first, null))
            {
                result.Add(first);
            }

            AuthorInfo second = ParseOneAuthor(field, _second330);
            if (!ReferenceEquals(second, null))
            {
                result.Add(second);
            }

            AuthorInfo third = ParseOneAuthor(field, _third330);
            if (!ReferenceEquals(third, null))
            {
                result.Add(third);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static AuthorInfo[] ParseField454
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            // TODO parse other authors

            List<AuthorInfo> result = new List<AuthorInfo>();

            AuthorInfo first = ParseOneAuthor(field, _first454);
            if (!ReferenceEquals(first, null))
            {
                result.Add(first);
            }

            // Second author layout same as for 330
            AuthorInfo second = ParseOneAuthor(field, _second454);
            if (!ReferenceEquals(second, null))
            {
                result.Add(second);
            }

            // Third author layout same as for 330
            AuthorInfo third = ParseOneAuthor(field, _third454);
            if (!ReferenceEquals(third, null))
            {
                result.Add(third);
            }

            return result.ToArray();
        }


        /// <summary>
        /// Parse the specified field.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static AuthorInfo[] ParseField481
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            // TODO parse other authors

            List<AuthorInfo> result = new List<AuthorInfo>();

            AuthorInfo first = ParseOneAuthor(field, _first481);
            if (!ReferenceEquals(first, null))
            {
                result.Add(first);
            }

            // Second author layout same as for 330
            AuthorInfo second = ParseOneAuthor(field, _second330);
            if (!ReferenceEquals(second, null))
            {
                result.Add(second);
            }

            // Third author layout same as for 330
            AuthorInfo third = ParseOneAuthor(field, _third330);
            if (!ReferenceEquals(third, null))
            {
                result.Add(third);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Parse one of the authors.
        /// </summary>
        [CanBeNull]
        public static AuthorInfo ParseOneAuthor
            (
                [NotNull] RecordField field,
                [NotNull] char[] subFields
            )
        {
            Code.NotNull(field, "field");
            Code.NotNull(subFields, "subFields");
            if (subFields.Length != 4)
            {
                throw new IrbisException();
            }

            string withInitials = field.GetFirstSubFieldValue(subFields[0]);
            if (string.IsNullOrEmpty(withInitials))
            {
                return null;
            }

            AuthorInfo result = new AuthorInfo();
            TextNavigator navigator = new TextNavigator(withInitials);
            result.CantBeInverted = !string.IsNullOrEmpty
                (
                    field.GetFirstSubFieldValue(subFields[2])
                );
            result.FamilyName = navigator.ReadUntil(_delimiters);
            navigator.SkipChar(_delimiters);
            result.Initials = navigator.GetRemainingText();
            result.FullName = field.GetFirstSubFieldValue(subFields[1]);
            result.WorkPlace = field.GetFirstSubFieldValue(subFields[3]);
            result.Field = field;

            return result;
        }

        /// <summary>
        /// Transform back to field.
        /// </summary>
        [NotNull]
        public RecordField ToField700
            (
                [NotNull] string tag
            )
        {
            RecordField result = new RecordField(tag)
                .AddNonEmptySubField('a', FamilyName)
                .AddNonEmptySubField('b', Initials)
                .AddNonEmptySubField('g', FullName)
                .AddNonEmptySubField('9', CantBeInverted ? "1" : null)
                .AddNonEmptySubField('1', Postfix)
                .AddNonEmptySubField('c', Appendix)
                .AddNonEmptySubField('d', Number)
                .AddNonEmptySubField('f', Dates)
                .AddNonEmptySubField('r', Variant)
                .AddNonEmptySubField('p', WorkPlace);

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return FamilyName.ToVisibleString();
        }

        #endregion
    }
}
