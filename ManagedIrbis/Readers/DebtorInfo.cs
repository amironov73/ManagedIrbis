/* DebtorInfo.cs -- информация о задолжнике.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AM.IO;
using AM.Runtime;

using JetBrains.Annotations;
using ManagedIrbis.Fields;
using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Информация о задолжнике
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class DebtorInfo
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Фамилия, имя, отчество
        /// </summary>
        [CanBeNull]
        public string Fio { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        [CanBeNull]
        public string Birthdate { get; set; }

        /// <summary>
        /// Номер читательского билета
        /// </summary>
        [CanBeNull]
        public string Ticket { get; set; }


        /// <summary>
        /// Пол
        /// </summary>
        [CanBeNull]
        public string Gender { get; set; }

        /// <summary>
        /// Категория
        /// </summary>
        [CanBeNull]
        public string Category { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        [CanBeNull]
        public string Address { get; set; }

        /// <summary>
        /// Место работы
        /// </summary>
        [CanBeNull]
        public string Work { get; set; }

        /// <summary>
        /// Электронная почта
        /// </summary>
        [CanBeNull]
        public string Email { get; set; }

        /// <summary>
        /// Домашний телефон
        /// </summary>
        [CanBeNull]
        public string HomePhone { get; set; }

        /// <summary>
        /// Возраст
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Примечания
        /// </summary>
        [CanBeNull]
        public string Remarks { get; set; }

        /// <summary>
        /// MFN записи
        /// </summary>
        public int Mfn { get; set; }


        /// <summary>
        /// Расформатированное описание
        /// </summary>
        [CanBeNull]
        public string Description { get; set; }

        /// <summary>
        /// Произвольные данные
        /// </summary>
        [CanBeNull]
        public object UserData { get; set; }

        /// <summary>
        /// Задолженные экземпляры
        /// </summary>
        [CanBeNull]
        public ExemplarInfo[] Exemplars { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Формирование задолжника из читателя
        /// </summary>
        [NotNull]
        public static DebtorInfo FromReader
            (
                [NotNull] ReaderInfo reader
            )
        {
            if (ReferenceEquals(reader, null))
            {
                throw new ArgumentNullException("reader");
            }

            DebtorInfo result = new DebtorInfo();



            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Просим объект записаться в поток.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Fio)
                .WriteNullable(Birthdate)
                .WriteNullable(Ticket)
                .WriteNullable(Gender)
                .WriteNullable(Category)
                .WriteNullable(Address)
                .WriteNullable(Work)
                .WriteNullable(Email)
                .WriteNullable(HomePhone)
                .WritePackedInt32(Age)
                .WriteNullable(Remarks)
                .WritePackedInt32(Mfn)
                .WriteNullable(Description);

            Exemplars.SaveToStream(writer);
        }

        /// <summary>
        /// Восстанавливаем объект из потока.
        /// </summary>
        public void RestoreFromStream
            (
                [NotNull] BinaryReader reader
            )
        {
            Fio = reader.ReadNullableString();
            Birthdate = reader.ReadNullableString();
            Ticket = reader.ReadNullableString();
            Gender = reader.ReadNullableString();
            Category = reader.ReadNullableString();
            Address = reader.ReadNullableString();
            Work = reader.ReadNullableString();
            Email = reader.ReadNullableString();
            HomePhone = reader.ReadNullableString();
            Age = reader.ReadPackedInt32();
            Remarks = reader.ReadNullableString();
            Mfn = reader.ReadPackedInt32();
            Description = reader.ReadNullableString();
            Exemplars = reader.ReadArray<ExemplarInfo>();
        }

        #endregion

        #region Object members

        #endregion
    }
}
