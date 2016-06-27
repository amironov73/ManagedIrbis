/* IlfFile.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
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

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
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
        [Serializable]
        [MoonSharpUserData]
        public sealed class Entry
        {
            #region Properties

            public int Position { get; set; }

            public DateTime Date { get; set; }

            public bool Deleted { get; set; }

            public string Name { get; set; }

            public int Number { get; set; }

            public short Flags { get; set; }

            public int DataLength { get; set; }

            public string Description { get; set; }

            public string Data { get; set; }

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
        public NonNullCollection<Entry> Entries
        {
            get { return _entries; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IlfFile()
        {
            _entries = new NonNullCollection<Entry>();
        }

        #endregion

        #region Private members

        private readonly NonNullCollection<Entry> _entries;

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
            Entry entry = Entries.FirstOrDefault
                (
                    e => e.Name.SameString(fileName)
                );

            return ReferenceEquals(entry, null)
                ? null
                : entry.Data;
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
            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(encoding, "encoding");

            IlfFile result = new IlfFile();

            using (Stream stream = File.OpenRead(fileName))
            using (BinaryReader reader
                = new BinaryReader(stream, encoding))
            {
                byte[] magicBytes = reader.ReadBytes(16);
                string magicString = encoding.GetString(magicBytes);
                if (magicString != MagicString)
                {
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
                        Date = DateTime.FromOADate(reader.ReadDouble()),
                        Deleted = reader.ReadInt16() != 0
                    };
                    byte[] bytes = new byte[24];
                    stream.Read(bytes, 0, 24);
                    string name = encoding.GetString(bytes);
                    entry.Name = name.TrimEnd('\0');

                    result.Entries.Add(entry);
                }

                string[] separators = {"\r\n"};

                foreach (Entry entry in result.Entries)
                {
                    stream.Position = entry.Position;
                    entry.Number = reader.ReadInt32();
                    entry.DataLength = reader.ReadInt32();
                    entry.Flags = reader.ReadInt16();
                    byte[] bytes = reader.ReadBytes(entry.DataLength);
                    string text = encoding.GetString(bytes);
                    string[] parts = text.Split(separators, 2, StringSplitOptions.None);
                    entry.Description = parts[0];
                    entry.Data = parts[1];
                }
            }

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore object state from the given stream.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save object state to the given stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IVerifiable members

        public bool Verify(bool throwOnError)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
