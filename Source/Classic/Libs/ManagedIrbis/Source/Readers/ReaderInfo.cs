// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReaderInfo.cs -- информация о читателе.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using AM.Collections;
using AM.IO;
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
    /// Информация о читателе.
    /// </summary>
    [PublicAPI]
    [XmlRoot("reader")]
    [MoonSharpUserData]
    public sealed class ReaderInfo
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// ФИО. Комбинируется из полей 10, 11 и 12.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public string FullName { get; set; }

        /// <summary>
        /// Фамилия. Поле 10.
        /// </summary>
        [CanBeNull]
        [Field("10")]
        [XmlAttribute("familyName")]
        [JsonProperty("familyName")]
        public string FamilyName { get; set; }

        /// <summary>
        /// Имя. Поле 11.
        /// </summary>
        [CanBeNull]
        [Field("11")]
        [XmlAttribute("firstName")]
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество. Поле 12.
        /// </summary>
        [CanBeNull]
        [Field("12")]
        [XmlAttribute("patronymic")]
        [JsonProperty("patronymic")]
        public string Patronymic { get; set; }

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
        /// Домашний адрес. Поле 13.
        /// </summary>
        [CanBeNull]
        [XmlElement("address")]
        [JsonProperty("address")]
        public ReaderAddress Address { get; set; }

        /// <summary>
        /// Место работы. Поле 15.
        /// </summary>
        [CanBeNull]
        [Field("15")]
        [XmlAttribute("workPlace")]
        [JsonProperty("workPlace")]
        public string WorkPlace { get; set; }

        /// <summary>
        /// Образование. Поле 20.
        /// </summary>
        [CanBeNull]
        [Field("20")]
        [XmlAttribute("education")]
        [JsonProperty("education")]
        public string Education { get; set; }

        /// <summary>
        /// Электронная почта. Поле 32.
        /// </summary>
        [CanBeNull]
        [Field("32")]
        [XmlAttribute("email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Домашний телефон. Поле 17.
        /// </summary>
        [CanBeNull]
        [Field("17")]
        [XmlAttribute("homePhone")]
        [JsonProperty("homePhone")]
        public string HomePhone { get; set; }

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
        /// Запись/перерегистрация в библиотеку.
        /// Поле 51.
        /// </summary>
        [XmlArray("enrollment")]
        [JsonProperty("enrollment")]
        public ReaderRegistration[] Enrollment { get; set; }

        /// <summary>
        /// Дата перерегистрации. Поле 52.
        /// </summary>
        [XmlArray("registrations")]
        [JsonProperty("registrations")]
        public ReaderRegistration[] Registrations { get; set; }

        /// <summary>
        /// Дата последней перерегистрации.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime LastRegistrationDate
        {
            get
            {
                if (Registrations.IsNullOrEmpty())
                {
                    return DateTime.MinValue;
                }

                return Registrations.Last().Date;
            }
        }

        /// <summary>
        /// Последнее место регистрации.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public string LastRegistrationPlace
        {
            get
            {
                if (Registrations.IsNullOrEmpty())
                {
                    return null;
                }

                return Registrations.Last().Chair;
            }
        }

        /// <summary>
        /// Разрешенные места получения литературы. Поле 56.
        /// </summary>
        [CanBeNull]
        [Field("56")]
        [XmlAttribute("enabledPlaces")]
        [JsonProperty("enabledPlaces")]
        public string EnabledPlaces { get; set; }

        /// <summary>
        /// Запрещенные места получения литературы. Поле 57.
        /// </summary>
        [CanBeNull]
        [Field("57")]
        [XmlAttribute("disabledPlaces")]
        [JsonProperty("disabledPlaces")]
        public string DisabledPlaces { get; set; }

        /// <summary>
        /// Право пользования библиотекой. Поле 29.
        /// </summary>
        [CanBeNull]
        [Field("29")]
        [XmlAttribute("rights")]
        [JsonProperty("rights")]
        public string Rights { get; set; }

        /// <summary>
        /// Примечания. Поле 33.
        /// </summary>
        [CanBeNull]
        [Field("33")]
        [XmlAttribute("remarks")]
        [JsonProperty("remarks")]
        public string Remarks { get; set; }

        /// <summary>
        /// Фотография читателя. Поле 950.
        /// </summary>
        [CanBeNull]
        [Field("950")]
        [XmlAttribute("photoFile")]
        [JsonProperty("photoFile")]
        public string PhotoFile { get; set; }

        /// <summary>
        /// Информация о посещениях.
        /// </summary>
        [XmlArray("visits")]
        [JsonProperty("visits")]
        public VisitInfo[] Visits { get; set; }

        /// <summary>
        /// Профили обслуживания ИРИ.
        /// </summary>
        [XmlArray("iri")]
        [JsonProperty("iri")]
        public IriProfile[] Profiles { get; set; }

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
        /// Возрастная категория.
        /// </summary>
        [NotNull]
        [XmlIgnore]
        [JsonIgnore]
        public string AgeCategory
        {
            get
            {
                int age = Age;
                if (age > 65) return "> 65";
                if (age >= 55) return "55-64";
                if (age >= 45) return "45-54";
                if (age >= 35) return "35-44";
                if (age >= 25) return "25-34";
                if (age >= 18) return "18-24";
                return "< 18";
            }
        }

        /// <summary>
        /// Произвольные данные, ассоциированные с читателем.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        /// <summary>
        /// Дата первого посещения
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime FirstVisitDate
        {
            get
            {
                if (Visits.IsNullOrEmpty())
                {
                    return DateTime.MinValue;
                }

                return Visits.First().DateGiven;
            }
        }

        /// <summary>
        /// Дата последнего посещения.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime LastVisitDate
        {
            get
            {
                if (Visits.IsNullOrEmpty())
                {
                    return DateTime.MinValue;
                }

                return Visits.Last().DateGiven;
            }
        }

        /// <summary>
        /// Кафедра последнего посещения.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public string LastVisitPlace
        {
            get
            {
                if (Visits.IsNullOrEmpty())
                {
                    return null;
                }

                return Visits.Last().Department;
            }
        }

        /// <summary>
        /// Последний обслуживавший библиотекарь.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public string LastVisitResponsible
        {
            get
            {
                if (Visits.IsNullOrEmpty())
                {
                    return null;
                }

                return Visits.Last().Responsible;
            }
        }

        /// <summary>
        /// Reader status.
        /// </summary>
        [CanBeNull]
        public string Status { get; set; }

        /// <summary>
        /// Расформатированное описание.
        /// Не соответствует никакому полю.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// MFN записи.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonProperty("mfn")]
        public int Mfn { get; set; }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        [NotNull]
        public static ReaderInfo Parse
            (
                [NotNull] MarcRecord record
            )
        {
            // TODO Support for unknown fields

            Code.NotNull(record, "record");

            ReaderInfo result = new ReaderInfo
                {
                    FamilyName = record.FM("10"),
                    FirstName = record.FM("11"),
                    Patronymic = record.FM("12"),
                    DateOfBirth = record.FM("21"),
                    Ticket = record.FM("30"),
                    Gender = record.FM("23"),
                    Category = record.FM("50"),
                    Address = ReaderAddress.Parse
                        (
                            record.Fields
                            .GetField("13")
                            .FirstOrDefault()
                        ),
                    WorkPlace = record.FM("15"),
                    Education = record.FM("20"),
                    Email = record.FM("32"),
                    HomePhone = record.FM("17"),
                    RegistrationDateString = record.FM("51"),

                    Enrollment = record.Fields
                        .GetField("51")

#if !WINMOBILE && !PocketPC

                        .Select(ReaderRegistration.Parse)

#else

                        .Select(field => ReaderRegistration.Parse(field))

#endif

                            .ToArray(),

                    Registrations = record.Fields
                        .GetField("52")

#if !WINMOBILE && !PocketPC

                        .Select(ReaderRegistration.Parse)

#else

                        .Select(field => ReaderRegistration.Parse(field))

#endif

                            .ToArray(),
                    
                    EnabledPlaces = record.FM("56"),
                    DisabledPlaces = record.FM("57"),
                    Rights = record.FM("29"),
                    Remarks = record.FM("33"),
                    PhotoFile = record.FM("950"),
                    
                    Visits = record.Fields
                        .GetField("40")

#if !WINMOBILE && !PocketPC

                        .Select(VisitInfo.Parse)

#else

                        .Select(field => VisitInfo.Parse(field))

#endif

                            .ToArray(),

                    Profiles = IriProfile.ParseRecord(record),
                    Status = record.FM("2015") ?? "0"
                };

            foreach (ReaderRegistration registration in result.Registrations)
            {
                registration.Reader = result;
            }
            foreach (VisitInfo visit in result.Visits)
            {
                visit.Reader = result;
            }

            string fullName = result.FamilyName;
            if (!string.IsNullOrEmpty(result.FirstName))
            {
                fullName = fullName + " " + result.FirstName;
            }
            if (!string.IsNullOrEmpty(result.Patronymic))
            {
                fullName = fullName + " " + result.Patronymic;
            }
            result.FullName = fullName;

            result.Mfn = record.Mfn;

            return result;
        }

#if !WIN81

        /// <summary>
        /// Считывание из файла.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        public static ReaderInfo[] ReadFromFile
        (
            [NotNull] string fileName
        )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            ReaderInfo[] result = SerializationUtility
                .RestoreArrayFromFile<ReaderInfo>(fileName);

            return result;
        }

        /// <summary>
        /// Сохранение в файле.
        /// </summary>
        public static void SaveToFile
        (
            [NotNull] string fileName,
            [NotNull] [ItemNotNull] ReaderInfo[] readers
        )
        {
            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(readers, "readers");

            readers.SaveToFile(fileName);
        }

#endif

        /// <summary>
        /// Формирование записи по данным о читателе.
        /// </summary>
        [NotNull]
        public MarcRecord ToRecord()
        {
            // TODO Implement

            throw new NotImplementedException();
        }

        #region IHandmadeSerializable members

        /// <inheritdoc />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WriteNullable(FullName);
            writer.WriteNullable(FamilyName);
            writer.WriteNullable(FirstName);
            writer.WriteNullable(Patronymic);
            writer.WriteNullable(DateOfBirth);
            writer.WriteNullable(Ticket);
            writer.WriteNullable(Gender);
            writer.WriteNullable(Category);
            writer.WriteNullable(Address);
            writer.WriteNullable(WorkPlace);
            writer.WriteNullable(Education);
            writer.WriteNullable(Email);
            writer.WriteNullable(HomePhone);
            writer.WriteNullable(RegistrationDateString);
            Enrollment.SaveToStream(writer);
            Registrations.SaveToStream(writer);
            writer.WriteNullable(EnabledPlaces);
            writer.WriteNullable(DisabledPlaces);
            writer.WriteNullable(Rights);
            writer.WriteNullable(Remarks);
            writer.WriteNullable(PhotoFile);
            Visits.SaveToStream(writer);
            Profiles.SaveToStream(writer);
            writer.WriteNullable(Description);
            writer.WritePackedInt32(Mfn);
        }

        /// <inheritdoc />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            FullName = reader.ReadNullableString();
            FamilyName = reader.ReadNullableString();
            FirstName = reader.ReadNullableString();
            Patronymic = reader.ReadNullableString();
            DateOfBirth = reader.ReadNullableString();
            Ticket = reader.ReadNullableString();
            Gender = reader.ReadNullableString();
            Category = reader.ReadNullableString();
            Address = reader.RestoreNullable<ReaderAddress>();
            WorkPlace = reader.ReadNullableString();
            Education = reader.ReadNullableString();
            Email = reader.ReadNullableString();
            HomePhone = reader.ReadNullableString();
            RegistrationDateString = reader.ReadNullableString();
            Enrollment = reader.ReadArray<ReaderRegistration>();
            Registrations = reader.ReadArray<ReaderRegistration>();
            EnabledPlaces = reader.ReadNullableString();
            DisabledPlaces = reader.ReadNullableString();
            Rights = reader.ReadNullableString();
            Remarks = reader.ReadNullableString();
            PhotoFile = reader.ReadNullableString();
            Visits = reader.ReadArray<VisitInfo>();
            Profiles = reader.ReadArray<IriProfile>();
            Description = reader.ReadNullableString();
            Mfn = reader.ReadPackedInt32();
        }

        #endregion

        #endregion

        #region Object members

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format
                (
                    "{0} - {1}", 
                    Ticket,
                    FullName
                );
        }

        #endregion
    }
}

