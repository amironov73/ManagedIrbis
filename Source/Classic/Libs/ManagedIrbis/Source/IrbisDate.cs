// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisDate.cs -- строка с ИРБИС-датой
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: moderate
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Строка с ИРБИС-датой yyyyMMdd.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("{Text}")]
#endif
    public sealed class IrbisDate
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Формат конверсии по умолчанию.
        /// </summary>
        public const string DefaultFormat = "yyyyMMdd";

        #endregion

        #region Properties

        /// <summary>
        /// Формат конверсии.
        /// </summary>
        public static string ConversionFormat = DefaultFormat;

        /// <summary>
        /// В виде текста.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// В виде даты.
        /// </summary>
        public DateTime Date { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <remarks>
        /// Инициализирует сегодняшней датой.
        /// </remarks>
        public IrbisDate()
        {
            Date = DateTime.Today;
            Text = ConvertDateToString(Date);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public IrbisDate
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            Text = text;
            Date = ConvertStringToDate(text);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public IrbisDate
            (
                DateTime date
            )
        {
            Date = date;
            Text = ConvertDateToString(date);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Преобразование даты в строку.
        /// </summary>
        [NotNull]
        public static string ConvertDateToString
            (
                DateTime date
            )
        {
            return date.ToString(ConversionFormat);
        }

        /// <summary>
        /// Преобразование строки в дату.
        /// </summary>
        public static DateTime ConvertStringToDate
            (
                [CanBeNull] string date
            )
        {
            if (string.IsNullOrEmpty(date))
            {
                return DateTime.MinValue;
            }

            DateTime result;

#if !WINMOBILE && !PocketPC

            DateTime.TryParseExact
                (
                    date,
                    ConversionFormat,
                    CultureInfo.CurrentCulture,
                    DateTimeStyles.None,
                    out result
                );

#else

            result = DateTime.ParseExact
                (
                    date,
                    ConversionFormat,
                    CultureInfo.CurrentCulture,
                    DateTimeStyles.None
                );

#endif

            return result;
        }

        /// <summary>
        /// Неявное преобразование
        /// </summary>
        [NotNull]
        public static implicit operator IrbisDate
            (
                [NotNull] string text
            )
        {
            return new IrbisDate(text);
        }

        /// <summary>
        /// Неявное преобразование
        /// </summary>
        [NotNull]
        public static implicit operator IrbisDate
            (
                DateTime date
            )
        {
            return new IrbisDate(date);
        }

        /// <summary>
        /// Неявное преобразование
        /// </summary>
        [NotNull]
        public static implicit operator string 
            ( 
                [NotNull] IrbisDate date 
            )
        {
            Code.NotNull(date, "date");

            return date.Text;
        }

        /// <summary>
        /// Неявное преобразование
        /// </summary>
        public static implicit operator DateTime
            (
                [NotNull] IrbisDate date
            )
        {
            Code.NotNull(date, "date");

            return date.Date;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Text = reader.ReadString();
            Date = ConvertStringToDate(Text);
        }

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.Write(Text);
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return Text;
        }

        #endregion
    }
}
