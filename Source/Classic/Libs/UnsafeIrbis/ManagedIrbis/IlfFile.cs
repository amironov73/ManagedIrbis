// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IlfFile.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using UnsafeAM;
using UnsafeAM.Collections;
using UnsafeAM.IO;
using UnsafeAM.Logging;
using UnsafeAM.Runtime;
using UnsafeAM.Text;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

namespace UnsafeIrbis
{
    //
    // ILF - Архив текстовых файлов Irbis Library Files
    //
    // ILF-файлы – специфические для ИРБИС текстовые файлы,
    // содержащие независимые поименованные разделы.
    //
    // Могут использоваться для хранения основных текстовых
    // ресурсов баз данных: форматов (PFT), рабочих листов (WS),
    // вложенных РЛ (WSS), справочников (MNU),
    // таблиц переформатирования (FST) и др.
    //
    // При этом предлагается следующая структура имен ILF-файлов:
    // <ИМЯ_БД>_<ТИП>.ILF
    // Например:
    // Ibis_pft.ilf – ILF-файл для хранения форматов БД IBIS.
    //
    // С форума:
    //
    // * Сервер ищет файлы сначала в ilf затем в директории БД.
    // * Сервер пересылает клиенту ИРБИС64 файлы по одному.
    // * Клиент ИРБИС64 кэширует скачанные файлы. Так чтобы
    // заметить изменения, например в рабочих листах,
    // нужно выполнить режим ОБНОВИТЬ КОНТЕКСТ.
    // * Распаковка ILF - это задача исключительно сервера.
    //

    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    public sealed class IlfFile
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Magic string in header.
        /// </summary>
        public const string MagicString = "IRBIS_LIBRARY_01";

        #endregion

        #region Nested classes

        /// <summary>
        /// Entry.
        /// </summary>
        [DebuggerDisplay("[{Number}] {Name}")]
        public sealed class Entry
            : IHandmadeSerializable,
            IVerifiable
        {
            #region Properties

            /// <summary>
            /// Смещение данных от начала файла.
            /// </summary>
            public int Position { get; set; }

            /// <summary>
            /// Дата создания.
            /// </summary>
            public DateTime Date { get; set; }

            /// <summary>
            /// Удалено?
            /// </summary>
            public bool Deleted { get; set; }

            /// <summary>
            /// Имя.
            /// </summary>
            [CanBeNull]
            public string Name { get; set; }

            /// <summary>
            /// Порядковый номер.
            /// </summary>
            public int Number { get; set; }

            /// <summary>
            /// Флаги.
            /// </summary>
            public short Flags { get; set; }

            /// <summary>
            /// Количество данных (вместе с описанием).
            /// </summary>
            public int DataLength { get; set; }

            /// <summary>
            /// Описание в произвольной форме.
            /// </summary>
            [CanBeNull]
            public string Description { get; set; }

            /// <summary>
            /// Собственно данные.
            /// </summary>
            [CanBeNull]
            public string Data { get; set; }

            #endregion

            #region IHandmadeSerializable members


            /// <summary>
            /// Restore object state from the specified stream.
            /// </summary>
            public void RestoreFromStream
                (
                    BinaryReader reader
                )
            {
                Code.NotNull(reader, nameof(reader));

                Position = reader.ReadPackedInt32();
                Date = reader.ReadDateTime();
                Deleted = reader.ReadBoolean();
                Name = reader.ReadNullableString();
                Number = reader.ReadPackedInt32();
                Flags = reader.ReadInt16();
                DataLength = reader.ReadPackedInt32();
                Description = reader.ReadNullableString();
                Data = reader.ReadNullableString();
            }

            /// <summary>
            /// Save object state to the specified stream.
            /// </summary>
            public void SaveToStream
                (
                    BinaryWriter writer
                )
            {
                Code.NotNull(writer, nameof(writer));

                writer.WritePackedInt32(Position);
                writer.Write(Date);
                writer.Write(Deleted);
                writer.WriteNullable(Name);
                writer.WritePackedInt32(Number);
                writer.Write(Flags);
                writer.WritePackedInt32(DataLength);
                writer.WriteNullable(Description);
                writer.WriteNullable(Data);
            }


            #endregion

            #region IVerifiable members

            /// <summary>
            /// Verify object state.
            /// </summary>
            public bool Verify
                (
                    bool throwOnError
                )
            {
                Verifier<Entry> verifier = new Verifier<Entry>(this, throwOnError);

                verifier
                    .NotNullNorEmpty(Name, nameof(Name))
                    .NotNullNorEmpty(Data, nameof(Data));

                return verifier.Result;
            }

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Unknown.
        /// </summary>
        public int Unknown1 { get; set; }

        /// <summary>
        /// Slot count.
        /// </summary>
        public int SlotCount { get; set; }

        /// <summary>
        /// Entry count.
        /// </summary>
        public int EntryCount { get; set; }

        /// <summary>
        /// Write count.
        /// </summary>
        public int WriteCount { get; set; }

        /// <summary>
        /// Delete count.
        /// </summary>
        public int DeleteCount { get; set; }

        /// <summary>
        /// Entries.
        /// </summary>
        [NotNull]
        public NonNullCollection<Entry> Entries { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IlfFile()
        {
            Entries = new NonNullCollection<Entry>();
        }

        #endregion

        #region Private members

        #region Borrowed from reference source

        // see http://referencesource.microsoft.com/#mscorlib/system/datetime.cs,3509f50c41a3c37b

        private const int DaysPerYear = 365;
        private const int DaysPer4Years = DaysPerYear * 4 + 1;
        private const int DaysPer100Years = DaysPer4Years * 25 - 1;
        private const int DaysPer400Years = DaysPer100Years * 4 + 1;
        private const int DaysTo1899 = DaysPer400Years * 4
            + DaysPer100Years * 3 - 367;

        private const long TicksPerMillisecond = 10000;
        private const long TicksPerSecond = TicksPerMillisecond * 1000;
        private const long TicksPerMinute = TicksPerSecond * 60;
        private const long TicksPerHour = TicksPerMinute * 60;
        private const long TicksPerDay = TicksPerHour * 24;

        private const int MillisPerSecond = 1000;
        private const int MillisPerMinute = MillisPerSecond * 60;
        private const int MillisPerHour = MillisPerMinute * 60;
        private const int MillisPerDay = MillisPerHour * 24;

        private const long DoubleDateOffset = DaysTo1899 * TicksPerDay;

        private static long DoubleDateToTicks(double value)
        {

            long millis = (long)(value * MillisPerDay
                + (value >= 0 ? 0.5 : -0.5));

            if (millis < 0)
            {
                millis -= (millis % MillisPerDay) * 2;
            }

            millis += DoubleDateOffset / TicksPerMillisecond;

            return millis * TicksPerMillisecond;
        }

        private static DateTime FromOADate(double d)
        {
            return new DateTime
                (
                    DoubleDateToTicks(d),
                    DateTimeKind.Unspecified
                );
        }

        #endregion

        #endregion

        #region Public methods

        /// <summary>
        /// Get file by name.
        /// </summary>
        [CanBeNull]
        public string GetFile
            (
                [NotNull] string fileName
            )
        {
            Entry entry = Entries.FirstOrDefault(e => e.Name.SameString(fileName));

            return entry?.Data;
        }

        /// <summary>
        /// Read local file.
        /// </summary>
        [NotNull]
        public static IlfFile ReadLocalFile
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNullNorEmpty(fileName, nameof(fileName));
            Code.NotNull(encoding, nameof(encoding));

            IlfFile result = new IlfFile();

            using (Stream stream = File.OpenRead(fileName))
            using (BinaryReader reader = new BinaryReader(stream, encoding))
            {
                string magicString = reader.ReadString(MagicString.Length);
                if (magicString != MagicString)
                {
                    Log.Error
                        (
                            nameof(IlfFile) + "::" + nameof(ReadLocalFile) + ": "
                            + "wrong magic string="
                            + magicString
                        );

                    throw new FormatException();
                }

                result.Unknown1 = reader.ReadInt32();
                result.SlotCount = reader.ReadInt32();
                result.EntryCount = reader.ReadInt32();
                result.WriteCount = reader.ReadInt32();
                result.DeleteCount = reader.ReadInt32();

                for (int i = 0; i < result.EntryCount; i++)
                {
                    Entry entry = new Entry
                    {
                        Position = reader.ReadInt32(),
                        Date = FromOADate(reader.ReadDouble()),
                        Deleted = reader.ReadInt16() != 0
                    };
                    string name = reader.ReadString(24);
                    entry.Name = name.TrimEnd('\0');

                    result.Entries.Add(entry);
                }

                string[] separators = CommonSeparators.MsDos;

                foreach (Entry entry in result.Entries)
                {
                    stream.Position = entry.Position;
                    entry.Number = reader.ReadInt32();
                    entry.DataLength = reader.ReadInt32();
                    entry.Flags = reader.ReadInt16();
                    char[] chars = reader.ReadChars(entry.DataLength);
                    string text = new string(chars);

                    string[] parts = text.Split
                        (
                            separators,
                            2,
                            StringSplitOptions.None
                        );
                    entry.Description = parts[0];
                    entry.Data = parts.Length > 1
                        ? parts[1]
                        : string.Empty;
                }
            }

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, nameof(reader));

            reader.ReadCollection(Entries);
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, nameof(writer));

            writer.WriteCollection(Entries);
        }


        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<IlfFile> verifier = new Verifier<IlfFile>(this, throwOnError);

            foreach (Entry entry in Entries)
            {
                verifier.VerifySubObject(entry, nameof(entry));
            }

            return verifier.Result;
        }

        #endregion
    }
}

