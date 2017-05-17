// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReaderRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Configuration;
using AM.Data;
using AM.Suggestions;

using BLToolkit.Mapping;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Istu.OldModel
{
    [PublicAPI]
    [MoonSharpUserData]
    public class ReaderRecord
    {
        #region Properties

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>The tag.</value>
        [MapIgnore]
        [HiddenColumn]
        [Browsable(false)]
        public object Tag { get; set; }

        ///// <summary>
        ///// Gets or sets the loans.
        ///// </summary>
        ///// <value>
        ///// The loans.
        ///// </value>
        //[MapIgnore]
        //[HiddenColumn]
        //[Browsable(false)]
        //public List<Loan> Loans { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MapIgnore]
        [HiddenColumn]
        [Browsable(false)]
        public bool IsNew
        {
            [DebuggerStepThrough]
            get
            {
                return ID == 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [MapIgnore]
        [HiddenColumn]
        [Browsable(false)]
        public bool IsValid
        {
            [DebuggerStepThrough]
            get
            {
                return !IsNew
                       && !string.IsNullOrEmpty(Ticket)
                       && !string.IsNullOrEmpty(Name);
            }
        }

        /// <summary>
        /// Идентификатор в базе.
        /// </summary>
        [MapField("id")]
        public int ID;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _name;

        ///<summary>
        /// ФИО.
        ///</summary>
        [MapField("name")]
        [SortIndex(0)]
        [ColumnIndex(1)]
        [ColumnWidth(180)]
        [ColumnHeader("ФИО")]
        //[DisplayTitle("Фамилия, имя, отчество")]
        [Description("Фамилия, имя, отчество читателя. Контролируется системой. Для отключения контроля установите DontVerify (Не контролировать ФИО)")]
        public string Name
        {
            [DebuggerStepThrough]
            get
            {
                return _name;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _name = value;
                }
                if (!Immutable)
                {
                    _name = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _category;

        /// <summary>
        /// Категория: студент, преподаватель, сотрудник.
        /// </summary>
        [SortIndex(2)]
        [ColumnIndex(2)]
        //[DisplayTitle("Категория")]
        [ColumnHeader("Категория")]
        //[MapField("category", IsNullable = true)]
        //[Suggest(typeof(CategorySuggestor))]
        //[TypeConverter(typeof(SuggestorTypeConverter))]
        [Description("Категория читателя: студент, преподаватель, сотрудник и т. д.")]
        public string Category
        {
            [DebuggerStepThrough]
            get
            {
                return _category;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _category = value;
                }
                if (!Immutable)
                {
                    _category = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _department;

        /// <summary>
        /// Факультет.
        /// </summary>
        [SortIndex(3)]
        [ColumnIndex(3)]
        //[DisplayTitle("Факультет")]
        [ColumnHeader("Факультет")]
        //[MapField("department", IsNullable = true)]
        //[Suggest(typeof(DepartmentSuggestor))]
        //[TypeConverter(typeof(SuggestorTypeConverter))]
        public string Department
        {
            [DebuggerStepThrough]
            get
            {
                return _department;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _department = value;
                }
                if (!Immutable)
                {
                    _department = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _ticket;

        /// <summary>
        /// Номер читательского билета.
        /// </summary>
        [SortIndex(1)]
        [ColumnWidth(50)]
        [ColumnIndex(0)]
        [ColumnHeader("№ ч.б.")]
        //[MapField("ticket", IsNullable = true)]
        //[DisplayTitle("Номер читательского")]
        public string Ticket
        {
            [DebuggerStepThrough]
            get
            {
                return _ticket;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _ticket = value;
                }
                if (!Immutable)
                {
                    _ticket = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _registered;

        /// <summary>
        /// Дата регистрации.
        /// </summary>
        [SortIndex(4)]
        [HiddenColumn]
        //[MapField("registered", IsNullable = true)]
        //[DisplayTitle("Дата регистрации")]
        //[Suggest(typeof(TodaySuggestor))]
        //[TypeConverter ( typeof ( SuggestorTypeConverter ) )]
        //[Editor(typeof(TodayEditor), typeof(UITypeEditor))]
        public string Registered
        {
            [DebuggerStepThrough]
            get
            {
                return _registered;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _registered = value;
                }
                if (!Immutable)
                {
                    _registered = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private short _reregistered;

        /// <summary>
        /// Год последней перерегистрации.
        /// </summary>
        [SortIndex(5)]
        [HiddenColumn]
        //[MapField("reregistered", IsNullable = true)]
        //[DisplayTitle("Год перерегистрации")]
        public short Reregistered
        {
            [DebuggerStepThrough]
            get
            {
                return _reregistered;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _reregistered = value;
                }
                if (!Immutable)
                {
                    _reregistered = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _address;

        /// <summary>
        /// Домашний адрес.
        /// </summary>
        [SortIndex(1077)]
        [HiddenColumn]
        //[MapField("address", IsNullable = true)]
        //[DisplayTitle("Домашний адрес")]
        [Description("Используйте справочник КЛАДР")]
        //[Editor(typeof(AddressEditor), typeof(UITypeEditor))]
        public string Address
        {
            [DebuggerStepThrough]
            get
            {
                return _address;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _address = value;
                }
                if (!Immutable)
                {
                    _address = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _group;

        /// <summary>
        /// Группа (для студентов).
        /// </summary>
        [SortIndex(6)]
        [ColumnWidth(50)]
        [ColumnIndex(4)]
        [ColumnHeader("Группа")]
        //[MapField("group", IsNullable = true)]
        //[DisplayTitle("Группа")]
        public string Group
        {
            [DebuggerStepThrough]
            get
            {
                return _group;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _group = value;
                }
                if (!Immutable)
                {
                    _group = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _barcode;

        /// <summary>
        /// Штрих-код читательского билета.
        /// </summary>
        [SortIndex(8)]
        [HiddenColumn]
        //[MapField("barcode", IsNullable = true)]
        //[DisplayTitle("Штрих-код читательского билета")]
        [Description("Штрих-код читательского билета. Если начинается с восклицательного знака, то штрих-код не присвоен")]
        public string Barcode
        {
            [DebuggerStepThrough]
            get
            {
                return _barcode;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _barcode = value;
                }
                if (!Immutable)
                {
                    _barcode = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _blocked;

        /// <summary>
        /// Блокировка (например, из-за задолженности).
        /// </summary>
        [SortIndex(13)]
        [HiddenColumn]
        [MapField("blocked")]
        //[DisplayTitle("Блокирован")]
        [BLToolkit.Mapping.DefaultValue(false)]
        public bool Blocked
        {
            [DebuggerStepThrough]
            get
            {
                return _blocked;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _blocked = value;
                }
                if (!Immutable)
                {
                    _blocked = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _gone;

        /// <summary>
        /// Подписал обходной лист.
        /// </summary>
        [SortIndex(14)]
        [HiddenColumn]
        [MapField("podpisal")]
        //[DisplayTitle("Подписал обходной лист")]
        [BLToolkit.Mapping.DefaultValue(false)]
        public bool Gone
        {
            [DebuggerStepThrough]
            get
            {
                return _gone;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _gone = value;
                }
                if (!Immutable)
                {
                    _gone = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _password;

        /// <summary>
        /// Пароль доступа к сайту.
        /// </summary>
        [SortIndex(16)]
        [HiddenColumn]
        //[DisplayTitle("Пароль входа на сайт")]
        //[MapField("password", IsNullable = true)]
        public string Password
        {
            [DebuggerStepThrough]
            get
            {
                return _password;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _password = value;
                }
                if (!Immutable)
                {
                    _password = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _mail;

        /// <summary>
        /// Электронный адрес для уведомления.
        /// </summary>
        [SortIndex(115)]
        [HiddenColumn]
        //[DisplayTitle("Адрес электронной почты")]
        //[MapField("mail", IsNullable = true)]
        public string Mail
        {
            [DebuggerStepThrough]
            get
            {
                return _mail;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _mail = value;
                }
                if (!Immutable)
                {
                    _mail = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _jobTitle;

        ///<summary>
        /// Занимаемая должность.
        ///</summary>
        [SortIndex(11)]
        [HiddenColumn]
        //[DisplayTitle("Должность")]
        //[MapField("job", IsNullable = true)]
        [Description("Занимаемая должность")]
        public string JobTitle
        {
            [DebuggerStepThrough]
            get
            {
                return _jobTitle;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _jobTitle = value;
                }
                if (!Immutable)
                {
                    _jobTitle = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _cathedra;

        ///<summary>
        /// 
        ///</summary>
        [SortIndex(12)]
        [HiddenColumn]
        //[DisplayTitle("Кафедра")]
        //[MapField("cathedra", IsNullable = true)]
        //[Suggest(typeof(ChairSuggestor))]
        //[TypeConverter(typeof(SuggestorTypeConverter))]
        public string Cathedra
        {
            [DebuggerStepThrough]
            get
            {
                return _cathedra;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _cathedra = value;
                }
                if (!Immutable)
                {
                    _cathedra = value;
                }
            }
        }

        /// <summary>
        /// Штрих-код линейки.
        /// </summary>
        //[MapField("proxy", IsNullable = true)]
        public string Proxy;

        /// <summary>
        /// Код оператора, записавшего читателя
        /// </summary>
        //[MapField("operator", IsNullable = true)]
        public int Operator;

        /// <summary>
        /// Момент записи в библиотеку
        /// </summary>
        //[MapField("whn", IsNullable = true)]
        public DateTime Moment;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _phone;

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>The phone.</value>
        [SortIndex(109)]
        [HiddenColumn]
        //[MapField("phone", IsNullable = true)]
        //[DisplayTitle("Телефон")]
        [Description("Номер домашнего телефона (необязательно)")]
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _phone = value;
                }
                if (!Immutable)
                {
                    _phone = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _debtor;

        /// <summary>
        /// Должник.
        /// </summary>
        [SortIndex(17)]
        [HiddenColumn]
        [MapField("debtor")]
        //[DisplayTitle("Должник")]
        [BLToolkit.Mapping.DefaultValue(false)]
        [Description("Имеет задолженность перед библиотекой (по бумажным формулярам)")]
        public bool Debtor
        {
            [DebuggerStepThrough]
            get
            {
                return _debtor;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _debtor = value;
                }
                if (!Immutable)
                {
                    _debtor = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _istuID;

        /// <summary>
        /// Gets or sets the istu ID.
        /// </summary>
        /// <value>The istu ID.</value>
        [Browsable(false)]
        [HiddenColumn]
        [MapField("istuID")]
        [BLToolkit.Mapping.DefaultValue(0)]
        public int IstuID
        {
            get
            {
                return _istuID;
            }
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _istuID = value;
                }
                if (!Immutable)
                {
                    _istuID = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _academic;

        /// <summary>
        /// 
        /// </summary>
        [SortIndex(27)]
        [HiddenColumn]
        [MapField("academ")]
        //[DisplayTitle("Хвостист")]
        [BLToolkit.Mapping.DefaultValue(false)]
        public bool Academic
        {
            get
            {
                return _academic;
            }
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _academic = value;
                }
                if (!Immutable)
                {
                    _academic = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _dontVerify;

        /// <summary>
        /// Gets or sets a value indicating whether [dont verify].
        /// </summary>
        /// <value><c>true</c> if [dont verify]; otherwise, <c>false</c>.</value>
        [SortIndex(28)]
        [HiddenColumn]
        [MapField("dontVerify")]
        //[DisplayTitle("Не проверять ФИО")]
        [BLToolkit.Mapping.DefaultValue(false)]
        public bool DontVerify
        {
            get
            {
                return _dontVerify;
            }
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _dontVerify = value;
                }
                if (!Immutable)
                {
                    _dontVerify = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _workplace;

        /// <summary>
        /// Gets or sets the workplace.
        /// </summary>
        /// <value>The workplace.</value>
        [SortIndex(29)]
        [HiddenColumn]
        [MapField("workplace")]
        //[DisplayTitle("Место работы")]
        public string Workplace
        {
            get
            {
                return _workplace;
            }
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _workplace = value;
                }
                if (!Immutable)
                {
                    _workplace = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _everlasting;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ReaderRecord"/> is everlasting.
        /// </summary>
        /// <value><c>true</c> if everlasting; otherwise, <c>false</c>.</value>
        [SortIndex(30)]
        [HiddenColumn]
        [MapField("everlasting")]
        //[DisplayTitle("Вечник")]
        [BLToolkit.Mapping.DefaultValue(false)]
        [Description("Должник-вечник (да-нет)")]
        public bool Everlasting
        {
            [DebuggerStepThrough]
            get
            {
                return _everlasting;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _everlasting = value;
                }
                if (!Immutable)
                {
                    _everlasting = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _birthdate;

        /// <summary>
        /// Gets or sets the birthdate.
        /// </summary>
        /// <value>The birthdate.</value>
        [SortIndex(131)]
        [HiddenColumn]
        [ColumnIndex(100)]
        [ColumnHeader("Год рожд.")]
        [ColumnWidth(30)]
        [MapField("birthdate")]
        //[DisplayTitle("Дата рождения")]
        [Description("Достаточно ввести только год рождения (4 цифры)")]
        //[Editor(typeof(TodayEditor), typeof(UITypeEditor))]
        public string Birthdate
        {
            [DebuggerStepThrough]
            get
            {
                return _birthdate;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _birthdate = value;
                }
                if (!Immutable)
                {
                    _birthdate = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _alert;

        /// <summary>
        /// Gets or sets the alert.
        /// </summary>
        /// <value>The alert.</value>
        [SortIndex(90)]
        [HiddenColumn]
        [MapField("alert")]
        //[DisplayTitle("Сообщение")]
        //[SuggestFile("Alert.txt")]
        //[Editor(typeof(SuggestEditor), typeof(UITypeEditor))]
        [Description("Текст сообщения, которое будет появляться каждый раз при посещении читателем библиотеки.")]
        public string Alert
        {
            [DebuggerStepThrough]
            get
            {
                return _alert;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _alert = value;
                }
                if (!Immutable)
                {
                    _alert = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _rfid;

        /// <summary>
        /// Gets or sets the RFID card identifier.
        /// </summary>
        /// <value>The RFID.</value>
        [SortIndex(90)]
        [HiddenColumn]
        [MapField("rfid")]
        //[DisplayTitle("RFID")]
        [Description("Идентификатор читательской карточки")]
        public string Rfid
        {
            [DebuggerStepThrough]
            get
            {
                return _rfid;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _rfid = value;
                }
                if (!Immutable)
                {
                    _rfid = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _comment;

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        [SortIndex(100)]
        [HiddenColumn]
        [MapField("comment")]
        //[DisplayTitle("Примечания")]
        [Description("Комментарии в произвольной форме")]
        public string Comment
        {
            [DebuggerStepThrough]
            get
            {
                return _comment;
            }
            [DebuggerStepThrough]
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _comment = value;
                }
                if (!Immutable)
                {
                    _comment = value;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _family;

        /// <summary>
        /// Семейный абонемент
        /// </summary>
        [SortIndex(101)]
        [HiddenColumn]
        [MapField("family")]
        //[DisplayTitle("Семья")]
        [Description("Семейный абонемент художественной литературы")]
        //[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string Family
        {
            get
            {
                return _family;
            }
            set
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _family = value;
                }
                if (!Immutable)
                {
                    _family = value;
                }
            }
        }

        private bool _immutable;

        /// <summary>
        /// Неизменяемый
        /// </summary>
        [SortIndex(102)]
        [HiddenColumn]
        [MapField("immutable")]
        //[DisplayTitle("Неизменяемый")]
        [Description("Запрет внесения изменений")]
        [BLToolkit.Mapping.DefaultValue(false)]
        public bool Immutable
        {
            get { return _immutable; }
            set
            {
                StackTrace trace = new StackTrace();
                //Debug.WriteLine(trace.GetFrame(2).GetMethod().Name);
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _immutable = value;
                }
                if (ConfigurationUtility.GetBoolean("write-immutable", false))
                {
                    _immutable = value;
                }
            }
        }

        private bool _notify;

        /// <summary>
        /// Извещение
        /// </summary>
        [SortIndex(102)]
        [HiddenColumn]
        [MapField("notify")]
        //[DisplayTitle("Извещение")]
        [Description("Извещение о приходе")]
        [BLToolkit.Mapping.DefaultValue(false)]
        public bool Notify
        {
            get { return _notify; }
            set
            {
                StackTrace trace = new StackTrace();
                //Debug.WriteLine(trace.GetFrame(2).GetMethod().Name);
                if (trace.GetFrame(2).GetMethod().Name
                    == "Rsdn.Framework.Data.Mapping.IMapDataReceiver.SetFieldValue")
                {
                    _notify = value;
                }
                if (ConfigurationUtility.GetBoolean("write-notify", false))
                {
                    _notify = value;
                }
            }
        }

        /// <summary>
        /// Последнее посещение
        /// </summary>
        [SortIndex(201)]
        //[HiddenColumn]
        [MapIgnore]
        //[Browsable(false)]
        [HiddenColumn]
        [ColumnIndex(200)]
        [ColumnHeader("Посещение")]
        [ColumnWidth(60)]
        [BLToolkit.Mapping.DefaultValue(null)]
        //[DisplayTitle("Последнее посещение")]
        [Description("Дата и время последнего посещеня библиотеки")]
        public DateTime LastSeen { get; set; }

        /// <summary>
        /// Последнее посещение
        /// </summary>
        [SortIndex(202)]
        //[HiddenColumn]
        [MapIgnore]
        //[Browsable(false)]
        [HiddenColumn]
        [ColumnIndex(201)]
        [ColumnWidth(60)]
        [ColumnHeader("Оператор")]
        [BLToolkit.Mapping.DefaultValue(null)]
        //[DisplayTitle("Последний оператор")]
        [Description("Оператор, зарегистрировавший последнее посещение")]
        public string LastOperator { get; set; }

        #endregion
    }
}
