// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DepersonalizedReader.cs -- обезличенная информация о читателе.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Logging;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Обезличенная информация о читателе.
    /// </summary>
    [PublicAPI]
    [XmlRoot("reader")]
    [MoonSharpUserData]
    public sealed class DepersonalizedReader
    {
        #region Properties

        /// <summary>
        /// Дата рождения. Поле 21.
        /// </summary>
        [CanBeNull]
        [Field("21")]
        [XmlAttribute("dateOfBirth")]
        [JsonProperty("dateOfBirth")]
        public string DateOfBirth { get; set; }

        /// <summary>
        /// Номер читательского. Поле 30.
        /// </summary>
        [CanBeNull]
        [Field("30")]
        [XmlAttribute("ticket")]
        [JsonProperty("ticket")]
        public string Ticket { get; set; }

        /// <summary>
        /// Пол. Поле 23.
        /// </summary>
        [CanBeNull]
        [Field("23")]
        [XmlAttribute("gender")]
        [JsonProperty("gender")]
        public string Gender { get; set; }

        /// <summary>
        /// Категория. Поле 50.
        /// </summary>
        [CanBeNull]
        [Field("50")]
        [XmlAttribute("category")]
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>
        /// Дата записи. Поле 51.
        /// </summary>
        [CanBeNull]
        [Field("51")]
        [XmlAttribute("registrationDate")]
        [JsonProperty("registrationDate")]
        public string RegistrationDateString { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime RegistrationDate
        {
            get
            {
                return IrbisDate.ConvertStringToDate
                (
                    RegistrationDateString
                );
            }
        }

        /// <summary>
        /// Возраст, годы
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public int Age
        {
            get
            {
                string yearText = DateOfBirth;

                if (string.IsNullOrEmpty(yearText))
                {
                    return 0;
                }

                if (yearText.Length > 4)
                {
                    yearText = yearText.Substring(1, 4);
                }
#if PocketPC
                int year = int.Parse(yearText);
#else
                int year;
                if (!int.TryParse(yearText, out year))
                {
                    return 0;
                }
#endif
                return DateTime.Today.Year - year;
            }
        }

        /// <summary>
        /// Visits.
        /// </summary>
        public List<VisitInfo> Visits { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DepersonalizedReader()
        {
            Visits = new List<VisitInfo>();
        }

        #endregion

        #region Private members

        private static void _DegreaseVisitInfo
            (
                [NotNull] VisitInfo visit
            )
        {
            visit.Description = null;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add the visits if not yet.
        /// </summary>
        public void AddVisits
            (
                [NotNull] IEnumerable<VisitInfo> visits
            )
        {
            Code.NotNull(visits, "visits");

            foreach (VisitInfo visit in visits)
            {
                bool found = false;
                foreach (VisitInfo other in Visits)
                {
                    if (visit.SameVisit(other))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    _DegreaseVisitInfo(visit);
                    Visits.Add(visit);
                }
            }
        }

        /// <summary>
        /// Create <see cref="DepersonalizedReader"/>
        /// from the <see cref="ReaderInfo"/>.
        /// </summary>
        [NotNull]
        public static DepersonalizedReader FromReaderInfo
            (
                [NotNull] ReaderInfo readerInfo
            )
        {
            Code.NotNull(readerInfo, "readerInfo");

            DepersonalizedReader result = new DepersonalizedReader
            {
                Ticket = readerInfo.Ticket,
                Category = readerInfo.Category,
                DateOfBirth = readerInfo.DateOfBirth,
                Gender = readerInfo.Gender,
                RegistrationDateString = readerInfo.RegistrationDateString
            };
            result.Visits.AddRange(readerInfo.Visits);
            foreach (VisitInfo visit in result.Visits)
            {
                _DegreaseVisitInfo(visit);
            }

            return result;
        }

        /// <summary>
        /// Convert back to the record.
        /// </summary>
        [NotNull]
        public MarcRecord ToMarcRecord()
        {
            MarcRecord result = new MarcRecord();

            result
                .AddNonEmptyField(21, DateOfBirth)
                .AddNonEmptyField(30, Ticket)
                .AddNonEmptyField(23, Gender)
                .AddNonEmptyField(50, Category)
                .AddNonEmptyField(51, RegistrationDateString);

            foreach (VisitInfo visit in Visits)
            {
                result.AddField(visit.ToField());
            }

            return result;
        }

        #endregion
    }
}
