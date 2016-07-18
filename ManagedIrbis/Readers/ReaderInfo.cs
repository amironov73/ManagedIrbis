/* ReaderInfo.cs -- информация о читателе.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

using BLToolkit.Mapping;

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Информация о читателе.
    /// </summary>
    [PublicAPI]
    [XmlRoot("reader")]
    public sealed class ReaderInfo
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// ФИО. Комбинируется из полей 10, 11 и 12.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [CanBeNull]
        [MapField("name")]
        public string Fio { get; set; }

        /// <summary>
        /// Фамилия. Поле 10.
        /// </summary>
        [XmlAttribute("family-name")]
        [JsonProperty("family-name")]
        [CanBeNull]
        [MapIgnore]
        public string FamilyName { get; set; }

        /// <summary>
        /// Имя. Поле 11.
        /// </summary>
        [XmlAttribute("first-name")]
        [JsonProperty("first-name")]
        [CanBeNull]
        [MapIgnore]
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество. Поле 12.
        /// </summary>
        [XmlAttribute("patronym")]
        [JsonProperty("patronym")]
        [CanBeNull]
        [MapIgnore]
        public string Patronym { get; set; }

        /// <summary>
        /// Дата рождения. Поле 21.
        /// </summary>
        [XmlAttribute("birthdate")]
        [JsonProperty("birthdate")]
        [CanBeNull]
        [MapField("birthdate")]
        public string Birthdate { get; set; }

        /// <summary>
        /// Номер читательского. Поле 30.
        /// </summary>
        [XmlAttribute("ticket")]
        [JsonProperty("ticket")]
        [CanBeNull]
        [MapField("ticket")]
        public string Ticket { get; set; }

        /// <summary>
        /// Пол. Поле 23.
        /// </summary>
        [XmlAttribute("gender")]
        [JsonProperty("gender")]
        [CanBeNull]
        [MapField("gender")]
        public string Gender { get; set; }

        /// <summary>
        /// Категория. Поле 50.
        /// </summary>
        [XmlAttribute("category")]
        [JsonProperty("category")]
        [CanBeNull]
        [MapField("category")]
        public string Category { get; set; }

        /// <summary>
        /// Домашний адрес. Поле 13.
        /// </summary>
        [XmlAttribute("address")]
        [JsonProperty("address")]
        [CanBeNull]
        [MapIgnore]
        public ReaderAddress Address { get; set; }

        /// <summary>
        /// Место работы. Поле 15.
        /// </summary>
        [XmlAttribute("work")]
        [JsonProperty("work")]
        [CanBeNull]
        [MapField("work")]
        public string Work { get; set; }

        /// <summary>
        /// Образование. Поле 20.
        /// </summary>
        [XmlAttribute("education")]
        [JsonProperty("education")]
        [CanBeNull]
        [MapField("education")]
        public string Education { get; set; }

        /// <summary>
        /// Электронная почта. Поле 32.
        /// </summary>
        [XmlAttribute("email")]
        [JsonProperty("email")]
        [CanBeNull]
        [MapField("email")]
        public string Email { get; set; }

        /// <summary>
        /// Домашний телефон. Поле 17.
        /// </summary>
        [XmlAttribute("home-phone")]
        [JsonProperty("home-phone")]
        [CanBeNull]
        [MapField("homephone")]
        public string HomePhone { get; set; }

        /// <summary>
        /// Дата записи. Поле 51.
        /// </summary>
        [XmlAttribute("registration-date")]
        [JsonProperty("registration-date")]
        [CanBeNull]
        [MapField("registration")]
        public string RegistrationDateString { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [MapIgnore]
        public DateTime RegistrationDate
        {
            get
            {
                return IrbisDate.ConvertStringToDate(RegistrationDateString);
            }
        }

        /// <summary>
        /// Запись/перерегистрация в библиотеку.
        /// Поле 51.
        /// </summary>
        [XmlArray("enrollment")]
        [JsonProperty("enrollment")]
        public ReaderRegistration[] Enrollment;

        /// <summary>
        /// Дата перерегистрации. Поле 52.
        /// </summary>
        [XmlArray("registrations")]
        [JsonProperty("registrations")]
        public ReaderRegistration[] Registrations;

        /// <summary>
        /// Дата последней перерегистрации.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [MapIgnore]
        public DateTime LastRegistrationDate
        {
            get
            {
                if ((Registrations == null) 
                    || (Registrations.Length == 0))
                {
                    return DateTime.MinValue;
                }
                return Registrations.Last().Date;
            }
        }

        /// <summary>
        /// Последнее место регистрации.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [CanBeNull]
        [MapIgnore]
        public string LastRegistrationPlace
        {
            get
            {
                if ((Registrations == null) 
                    || (Registrations.Length == 0))
                {
                    return null;
                }
                return Registrations.Last().Chair;
            }
        }

        /// <summary>
        /// Разрешенные места получения литературы. Поле 56.
        /// </summary>
        [XmlAttribute("enabled-places")]
        [JsonProperty("enabled-places")]
        [MapField("enabledplaces")]
        public string EnabledPlaces { get; set; }

        /// <summary>
        /// Запрещенные места получения литературы. Поле 57.
        /// </summary>
        [XmlAttribute("disabled-places")]
        [JsonProperty("disabled-places")]
        [MapField("disabledplaces")]
        public string DisabledPlaces { get; set; }

        /// <summary>
        /// Право пользования библиотекой. Поле 29.
        /// </summary>
        [XmlAttribute("rights")]
        [JsonProperty("rights")]
        [MapField("rights")]
        public string Rights { get; set; }

        /// <summary>
        /// Примечания. Поле 33.
        /// </summary>
        [XmlAttribute("remarks")]
        [JsonProperty("remarks")]
        [MapField("remarks")]
        public string Remarks { get; set; }

        /// <summary>
        /// Фотография читателя. Поле 950.
        /// </summary>
        [XmlAttribute("photo-file")]
        [JsonProperty("photo-file")]
        [MapField("photofile")]
        public string PhotoFile { get; set; }

        /// <summary>
        /// Информация о посещениях.
        /// </summary>
        [XmlArray("visits")]
        [JsonProperty("visits")]
        [MapIgnore]
        public VisitInfo[] Visits;

        /// <summary>
        /// Профили обслуживания ИРИ.
        /// </summary>
        [XmlArray("iri")]
        [JsonProperty("iri")]
        [MapIgnore]
        public IriProfile[] Profiles;

        /// <summary>
        /// Возраст, годы
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [MapIgnore]
        public int Age
        {
            get
            {
                string yearText = Birthdate;
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
        [XmlIgnore]
        [JsonIgnore]
        [MapIgnore]
        [JetBrains.Annotations.NotNull]
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
        [XmlIgnore]
        [JsonIgnore]
        [MapIgnore]
        public object UserData
        {
            get { return _userData; }
            set { _userData = value; }
        }

        /// <summary>
        /// Дата первого посещения
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [MapIgnore]
        public DateTime FirstVisitDate
        {
            get
            {
                if ((Visits == null) || (Visits.Length == 0))
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
        [MapIgnore]
        public DateTime LastVisitDate
        {
            get
            {
                if ((Visits == null) || (Visits.Length == 0))
                {
                    return DateTime.MinValue;
                }
                return Visits.Last().DateGiven;
            }
        }

        /// <summary>
        /// Кафедра последнего посещения.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [CanBeNull]
        [MapIgnore]
        public string LastVisitPlace
        {
            get
            {
                if ((Visits == null) || (Visits.Length == 0))
                {
                    return null;
                }
                return Visits.Last().Department;
            }
        }

        /// <summary>
        /// Последний обслуживавший библиотекарь.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [CanBeNull]
        [MapIgnore]
        public string LastVisitResponsible
        {
            get
            {
                if ((Visits == null) || (Visits.Length == 0))
                {
                    return null;
                }
                return Visits.Last().Responsible;
            }
        }

        /// <summary>
        /// Расформатированное описание.
        /// Не соответствует никакому полю.
        /// </summary>
        [XmlAttribute("description")]
        [JsonProperty("description")]
        [MapField("description")]
        [CanBeNull]
        public string Description { get; set; }

        /// <summary>
        /// MFN записи.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonProperty("mfn")]
        [MapIgnore]
        public int Mfn { get; set; }

        #endregion

        #region Private members

        //[NonSerialized]
        private object _userData;

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        [JetBrains.Annotations.NotNull]
        public static ReaderInfo Parse
            (
                [JetBrains.Annotations.NotNull] MarcRecord record
            )
        {
            if (ReferenceEquals(record, null))
            {
                throw new ArgumentNullException("record");
            }

            ReaderInfo result = new ReaderInfo
                                    {
                                        FamilyName = record.FM("10"),
                                        FirstName = record.FM("11"),
                                        Patronym = record.FM("12"),
                                        Birthdate = record.FM("21"),
                                        Ticket = record.FM("30"),
                                        Gender = record.FM("23"),
                                        Category = record.FM("50"),
                                        Address = ReaderAddress.Parse
                                            (
                                                record.Fields
                                                .GetField("13")
                                                .FirstOrDefault()
                                            ),
                                        Work = record.FM("15"),
                                        Education = record.FM("20"),
                                        Email = record.FM("32"),
                                        HomePhone = record.FM("17"),
                                        RegistrationDateString = record.FM("51"),
                                        Enrollment = record.Fields
                                            .GetField("51")
                                            .Select(ReaderRegistration.Parse)
                                            .ToArray(),
                                        Registrations = record.Fields
                                            .GetField("52")
                                            .Select(ReaderRegistration.Parse)
                                            .ToArray(),
                                        EnabledPlaces = record.FM("56"),
                                        DisabledPlaces = record.FM("57"),
                                        Rights = record.FM("29"),
                                        Remarks = record.FM("33"),
                                        PhotoFile = record.FM("950"),
                                        Visits = record.Fields
                                            .GetField("40")
                                            .Select(VisitInfo.Parse)
                                            .ToArray(),
                                        Profiles = IriProfile.ParseRecord(record)
                                    };

            foreach (ReaderRegistration registration in result.Registrations)
            {
                registration.Reader = result;
            }
            foreach (VisitInfo visit in result.Visits)
            {
                visit.Reader = result;
            }

            string fio = result.FamilyName;
            if (!string.IsNullOrEmpty(result.FirstName))
            {
                fio = fio + " " + result.FirstName;
            }
            if (!string.IsNullOrEmpty(result.Patronym))
            {
                fio = fio + " " + result.Patronym;
            }
            result.Fio = fio;

            return result;
        }

        /// <summary>
        /// Формирование записи по данным о читателе.
        /// </summary>
        /// <returns></returns>
        [JetBrains.Annotations.NotNull]
        public MarcRecord ToRecord()
        {
            throw new NotImplementedException();
        }

        #region Ручная сериализация

        /// <summary>
        /// Сохранение в поток.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(Fio);
            writer.WriteNullable(FamilyName);
            writer.WriteNullable(FirstName);
            writer.WriteNullable(Patronym);
            writer.WriteNullable(Birthdate);
            writer.WriteNullable(Ticket);
            writer.WriteNullable(Gender);
            writer.WriteNullable(Category);
            writer.WriteNullable(Address);
            writer.WriteNullable(Work);
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

        /// <summary>
        /// Считывание из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Fio = reader.ReadNullableString();
            FamilyName = reader.ReadNullableString();
            FirstName = reader.ReadNullableString();
            Patronym = reader.ReadNullableString();
            Birthdate = reader.ReadNullableString();
            Ticket = reader.ReadNullableString();
            Gender = reader.ReadNullableString();
            Category = reader.ReadNullableString();
            Address = reader.RestoreNullable<ReaderAddress>();
            Work = reader.ReadNullableString();
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

        /// <summary>
        /// Сохранение в файле.
        /// </summary>
        public static void SaveToFile
            (
                [JetBrains.Annotations.NotNull] string fileName,
                [JetBrains.Annotations.NotNull]
                [ItemNotNull] ReaderInfo[] readers
            )
        {
            readers.SaveToZipFile(fileName);
        }

        /// <summary>
        /// Считывание из файла.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        public static ReaderInfo[] ReadFromFile
            (
                [JetBrains.Annotations.NotNull] string fileName
            )
        {
            ReaderInfo[] result = SerializationUtility
                .RestoreArrayFromZipFile<ReaderInfo>(fileName);

            return result;
        }

        #endregion

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format
                (
                    "{0} - {1}", 
                    Ticket,
                    Fio
                );
        }

        #endregion
    }
}
