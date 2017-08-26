// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AuthorInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
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
        /// Фамилия. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("familyName")]
        [JsonProperty("familyName")]
        [Description("Фамилия. Подполе a.")]
        public string FamilyName { get; set; }

        /// <summary>
        /// Инициалы (сокращение). Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("initials")]
        [JsonProperty("initials")]
        [Description("Инициалы (сокращение). Подполе b.")]
        public string Initials { get; set; }

        /// <summary>
        /// Расширение инициалов (имя и отчество). Подполе g.
        /// </summary>
        [CanBeNull]
        [SubField('g')]
        [XmlAttribute("fullName")]
        [JsonProperty("fullName")]
        [Description("Расширение инициалов (имя и отчество). Подполе g.")]
        public string FullName { get; set; }

        /// <summary>
        /// Инвертирование имени недопустимо? Подполе 9.
        /// </summary>
        [SubField('9')]
        [XmlAttribute("cantBeInverted")]
        [JsonProperty("cantBeInverted")]
        [Description("Инвертирование имени недопустимо? Подполе 9.")]
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
        public string Appendix { get; set; }

        /// <summary>
        /// Династический номер (римские цифры). Подполе d.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("number")]
        [JsonProperty("number")]
        [Description("Династический номер (римские цифры). Подполе d.")]
        public string Number { get; set; }

        /// <summary>
        /// Даты жизни. Подполе f.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlAttribute("dates")]
        [JsonProperty("dates")]
        [Description("Даты жизни. Подполе f.")]
        public string Dates { get; set; }

        /// <summary>
        /// Разночтение фамилии. Подполе r.
        /// </summary>
        [CanBeNull]
        [SubField('r')]
        [XmlAttribute("variant")]
        [JsonProperty("variant")]
        [Description("Разночтение фамилии. Подполе r.")]
        public string Variant { get; set; }

        /// <summary>
        /// Место работы автора. Подполе p.
        /// </summary>
        [CanBeNull]
        [SubField('p')]
        [XmlAttribute("workplace")]
        [JsonProperty("workplace")]
        [Description("Место работы автора. Подполе p.")]
        public string WorkPlace { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        public static AuthorInfo ParseField700
            (
                [NotNull] RecordField field
            )
        {
            AuthorInfo result = new AuthorInfo()
            {
                FamilyName = field.GetFirstSubFieldValue('a'),
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
                WorkPlace = field.GetFirstSubFieldValue('p')
            };

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
                .AddNonEmptySubField('1', Postfix)
                .AddNonEmptySubField('c', Appendix)
                .AddNonEmptySubField('d', Number)
                .AddNonEmptySubField('f', Dates)
                .AddNonEmptySubField('r', Variant)
                .AddNonEmptySubField('p', WorkPlace);
            if (CantBeInverted)
            {
                result.AddSubField('9', "1");
            }

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
