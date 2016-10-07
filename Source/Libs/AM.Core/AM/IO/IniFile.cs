/* IniFile.cs -- simple INI-file reader/writer
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using AM.Collections;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.IO
{
    // ReSharper disable ClassWithVirtualMembersNeverInherited.Global

    /// <summary>
    /// Simple INI-file reader/writer.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("{FileName}")]
#endif
#if !SILVERLIGHT
    // ReSharper disable once RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
#endif
    public class IniFile
        : IHandmadeSerializable,
        IEnumerable<IniFile.Section>,
        IDisposable
    {
        #region Nested classes

        /// <summary>
        /// Line (element) of the INI-file.
        /// </summary>
        [PublicAPI]
        [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
        [DebuggerDisplay("{Key}={Value} [{Modified}]")]
#endif
        public sealed class Line
            : IHandmadeSerializable
        {
            #region Properties

            /// <summary>
            /// Key (name) of the element.
            /// </summary>
            [NotNull]
            public string Key
            {
                get { return _key; }
                //private set
                //{
                //    CheckName(value);
                //    _name = value;
                //}
            }

            /// <summary>
            /// Value of the element.
            /// </summary>
            [CanBeNull]
            public string Value
            {
                get { return _value; }
                set
                {
                    _value = value;
                    Modified = true;
                }
            }

            /// <summary>
            /// Modification flag.
            /// </summary>
            public bool Modified { get; set; }

            #endregion

            #region Construction

            /// <summary>
            /// Default constructor.
            /// </summary>
            public Line()
            {
                // Leave Name=null for a while.
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            public Line
                (
                    [NotNull] string key,
                    [CanBeNull] string value
                )
            {
                CheckKeyName(key);

                _key = key;
                _value = value;
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            public Line
                (
                    [NotNull] string key,
                    [CanBeNull] string value,
                    bool modified
                )
            {
                CheckKeyName(key);

                _key = key;
                _value = value;
                Modified = modified;
            }

            #endregion

            #region Private members

            private string _key;
            private string _value;

            #endregion

            #region Public methods

            /// <summary>
            /// Write the line to the stream.
            /// </summary>
            public void Write
                (
                    [NotNull] TextWriter writer
                )
            {
                Code.NotNull(writer, "writer");

                if (string.IsNullOrEmpty(Value))
                {
                    writer.WriteLine(Key);
                }
                else
                {
                    writer.WriteLine
                        (
                            "{0}={1}",
                            Key, Value
                        );
                }
            }

            #endregion

            #region IHandmadeSerializable members

            /// <inheritdoc />
            public void RestoreFromStream
                (
                    BinaryReader reader
                )
            {
                Code.NotNull(reader, "reader");

                _key = reader.ReadNullableString();
                _value = reader.ReadNullableString();
                Modified = reader.ReadBoolean();
            }

            /// <inheritdoc />
            public void SaveToStream
                (
                    BinaryWriter writer
                )
            {
                Code.NotNull(writer, "writer");

                writer
                    .WriteNullable(Key)
                    .WriteNullable(Value)
                    .Write(Modified);
            }

            #endregion

            #region Object members

            /// <inheritdoc />
            public override string ToString()
            {
                string result = string.Format
                    (
                        "{0}={1}{2}",
                        Key,
                        Value,
                        Modified ? " [modified]" : string.Empty
                    );

                return result;
            }

            #endregion
        }

        // =========================================================

        /// <summary>
        /// INI-file section.
        /// </summary>
        [PublicAPI]
        [MoonSharpUserData]
        public sealed class Section
            : IHandmadeSerializable,
            IEnumerable<Line>
        {
            #region Properties

            /// <summary>
            /// Count of lines.
            /// </summary>
            public int Count
            {
                get { return _lines.Count; }
            }

            /// <summary>
            /// All the keys of the section.
            /// </summary>
            [NotNull]
            public IEnumerable<string> Keys
            {
                get
                {
                    foreach (Line line in _lines)
                    {
                        yield return line.Key;
                    }
                }
            }

            /// <summary>
            /// Section is modified?
            /// </summary>
            public bool Modified { get; set; }

            /// <summary>
            /// Section name.
            /// </summary>
            [CanBeNull]
            public string Name
            {
                [DebuggerStepThrough]
                get { return _name; }
                set
                {
                    SetName(value.ThrowIfNull("value"));
                }
            }

            /// <summary>
            /// INI-file.
            /// </summary>
            [NotNull]
            public IniFile Owner { get; private set; }

            /// <summary>
            /// Indexer.
            /// </summary>
            public string this[[NotNull] string key]
            {
                get { return GetValue(key, null); }
                set { SetValue(key, value); }
            }

            #endregion

            #region Construction

            internal Section
            (
                [NotNull] IniFile owner,
                [CanBeNull] string name
            )
            {
                Owner = owner;
                _name = name;
                _lines = new NonNullCollection<Line>();
            }

            #endregion

            #region Private members

            private string _name;

            private NonNullCollection<Line> _lines;

            #endregion

            #region Public methods

            /// <summary>
            /// Add new item to the section.
            /// </summary>
            public void Add
                (
                    [NotNull] string key,
                    [CanBeNull] string value
                )
            {
                Line line = new Line(key, value);
                Add(line);
            }

            /// <summary>
            /// Add new line to the section.
            /// </summary>
            public void Add
                (
                    [NotNull] Line line
                )
            {
                Code.NotNull(line, "line");
                CheckKeyName(line.Key);
                if (ContainsKey(line.Key))
                {
                    throw new DuplicateKeyException("key");
                }

                _lines.Add(line);
            }

            /// <summary>
            /// Clear the section.
            /// </summary>
            public void Clear()
            {
                _lines.Clear();
                Modified = true;
                Owner.Modified = true;
            }

            /// <summary>
            /// Whether the section have line with given key?
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public bool ContainsKey
                (
                    [NotNull] string key
                )
            {
                Code.NotNullNorEmpty(key, "key");

                foreach (Line line in _lines)
                {
                    if (line.Key.SameString(key))
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// Get value associated with specified key.
            /// </summary>
            [CanBeNull]
            public string GetValue
                (
                    [NotNull] string key,
                    [CanBeNull] string defaultValue
                )
            {
                CheckKeyName(key);

                foreach (Line line in _lines)
                {
                    if (line.Key.SameString(key))
                    {
                        return line.Value;
                    }
                }

                return defaultValue;
            }

            /// <summary>
            /// Get value associated with given key.
            /// </summary>
            [CanBeNull]
            public T GetValue<T>
                (
                    [NotNull] string key,
                    [CanBeNull] T defaultValue
                )
            {
                Code.NotNullNorEmpty(key, "key");

                string value = GetValue(key, null);
                if (string.IsNullOrEmpty(value))
                {
                    return defaultValue;
                }

                T result = ConversionUtility.ConvertTo<T>(value);

                return result;
            }

            /// <summary>
            /// Remove specified key.
            /// </summary>
            [NotNull]
            public Section Remove
                (
                    [NotNull] string key
                )
            {
                CheckKeyName(key);

                foreach (Line line in _lines)
                {
                    if (line.Key.SameString(key))
                    {
                        _lines.Remove(line);
                        Modified = true;
                        Owner.Modified = true;
                        break;
                    }
                }

                return this;
            }

            /// <summary>
            /// Set name of the section.
            /// </summary>
            public void SetName
                (
                    [NotNull] string name
                )
            {
                Code.NotNullNorEmpty(name, "name");
                _name = name;
                Modified = true;
                Owner.Modified = true;
            }

            /// <summary>
            /// Set value associated with given key.
            /// </summary>
            [NotNull]
            public Section SetValue
                (
                    [NotNull] string key,
                    [CanBeNull] string value
                )
            {
                CheckKeyName(key);

                Line target = null;
                foreach (Line line in _lines)
                {
                    if (line.Key.SameString(key))
                    {
                        target = line;
                        break;
                    }
                }

                if (ReferenceEquals(target, null))
                {
                    target = new Line(key, value);
                    _lines.Add(target);
                }

                target.Value = value;

                return this;
            }

            /// <summary>
            /// Set value associate with given key.
            /// </summary>
            [NotNull]
            public Section SetValue<T>
                (
                    [NotNull] string key,
                    T value
                )
            {
                CheckKeyName(key);

                if (ReferenceEquals(value, null))
                {
                    SetValue(key, null);
                }
                else
                {
                    string text
                        = ConversionUtility.ConvertTo<string>(value);
                    SetValue(key, text);
                }

                return this;
            }

            /// <summary>
            /// Try to get value for given key.
            /// </summary>
            public bool TryGetValue
                (
                    [NotNull] string key,
                    out string value
                )
            {
                CheckKeyName(key);

                foreach (Line line in _lines)
                {
                    if (line.Key.SameString(key))
                    {
                        value = line.Value;
                        return true;
                    }
                }

                value = null;

                return false;
            }

            #endregion

            #region IHandmadeSerializable members

            /// <inheritdoc />
            public void RestoreFromStream
                (
                    BinaryReader reader
                )
            {
                _name = reader.ReadNullableString();
                _lines = reader.ReadNonNullCollection<Line>();
            }

            /// <inheritdoc />
            public void SaveToStream
                (
                    BinaryWriter writer
                )
            {
                writer.WriteNullable(_name);
                writer.WriteCollection(_lines);
            }

            #endregion

            #region IEnumerable<Line> members

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            /// <inheritdoc />
            public IEnumerator<Line> GetEnumerator()
            {
                return _lines.GetEnumerator();
            }

            #endregion
        }

        #endregion

        // =========================================================

        #region Properties

        /// <summary>
        /// Encoding.
        /// </summary>
        [CanBeNull]
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Name of the file.
        /// </summary>
        [CanBeNull]
        public string FileName { get; set; }

        /// <summary>
        /// Modified?
        /// </summary>
        public bool Modified { get; set; }

        /// <summary>
        /// Section indexer.
        /// </summary>
        [CanBeNull]
        public Section this[[NotNull] string sectionName]
        {
            get { return GetSection(sectionName); }
        }

        /// <summary>
        /// Value indexer.
        /// </summary>
        [CanBeNull]
        public string this
            [
                [NotNull] string sectionName,
                [NotNull] string keyName
            ]
        {
            get { return GetValue(sectionName, keyName, null); }
            set { SetValue(sectionName, keyName, value); }
        }

        /// <summary>
        /// Writable?
        /// </summary>
        public bool Writable { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IniFile()
        {
            _sections = new NonNullCollection<Section>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IniFile
            (
                [NotNull] string fileName
            )
            : this (fileName, null, false)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IniFile
            (
                [NotNull] string fileName,
                [CanBeNull] Encoding encoding,
                bool writable
            )
            : this()
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            FileName = fileName;
            Encoding = encoding;
            Writable = writable;

            Read();
        }

        #endregion

        #region Private members

        private NonNullCollection<Section> _sections;

        internal static void CheckKeyName
            (
                string keyName
            )
        {
            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentException("keyName");
            }

            if (keyName.Contains("="))
            {
                throw new ArgumentException("key");
            }
        }

        private static void _SaveSection
            (
                [NotNull] TextWriter writer,
                [NotNull] Section section
            )
        {
            if (!string.IsNullOrEmpty(section.Name))
            {
                writer.WriteLine
                    (
                        "[{0}]", section.Name
                    );
            }

            foreach (Line line in section)
            {
                line.Write(writer);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Clear the INI-file.
        /// </summary>
        public IniFile Clear()
        {
            _sections.Clear();

            return this;
        }

        /// <summary>
        /// Clear modification flag in all sections and lines.
        /// </summary>
        public void ClearModification()
        {
            Modified = false;

            foreach (Section section in _sections)
            {
                section.Modified = false;
                foreach (Line line in section)
                {
                    line.Modified = false;
                }
            }
        }

        /// <summary>
        /// Contains section with given name?
        /// </summary>
        public bool ContainsSection
            (
                [NotNull] string name
            )
        {
            CheckKeyName(name);

            foreach (Section section in _sections)
            {
                if (section.Name.SameString(name))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Create section with specified name.
        /// </summary>
        [NotNull]
        public Section CreateSection
            (
                [NotNull] string name
            )
        {
            CheckKeyName(name);

            if (ContainsSection(name))
            {
                throw new DuplicateKeyException("name");
            }

            Section result = new Section(this, name);
            _sections.Add(result);

            return result;
        }

        /// <summary>
        /// Get or create (if not exist) section with given name.
        /// </summary>
        [NotNull]
        public Section GetOrCreateSection
            (
                [NotNull] string name
            )
        {
            CheckKeyName(name);

            Section result = GetSection(name)
                ?? CreateSection(name);

            return result;
        }

        /// <summary>
        /// Get section with given name.
        /// </summary>
        [CanBeNull]
        public Section GetSection
            (
                [NotNull] string name
            )
        {
            CheckKeyName(name);

            foreach (Section section in _sections)
            {
                if (section.Name.SameString(name))
                {
                    return section;
                }
            }

            return null;
        }

        /// <summary>
        /// Get value from the given section and key.
        /// </summary>
        [CanBeNull]
        public string GetValue
            (
                [NotNull] string sectionName,
                [NotNull] string keyName,
                [CanBeNull] string defaultValue
            )
        {
            Section section = GetSection(sectionName);
            string result = section == null
                ? defaultValue
                : section.GetValue(keyName, defaultValue);

            return result;
        }

        /// <summary>
        /// Get value from the given section and key.
        /// </summary>
        [CanBeNull]
        public T GetValue<T>
            (
                [NotNull] string sectionName,
                [NotNull] string keyName,
                [CanBeNull] T defaultValue
            )
        {
            Section section = GetSection(sectionName);
            T result = section == null
                ? defaultValue
                : section.GetValue(keyName, defaultValue);

            return result;
        }

        /// <summary>
        /// Remove specified section.
        /// </summary>
        [NotNull]
        public IniFile RemoveSection
            (
                [NotNull] string name
            )
        {
            CheckKeyName(name);

            foreach (Section section in _sections)
            {
                if (section.Name.SameString(name))
                {
                    _sections.Remove(section);
                    break;
                }
            }

            return this;
        }

        /// <summary>
        /// Remove specified value.
        /// </summary>
        [NotNull]
        public IniFile RemoveValue
            (
                [NotNull] string sectionName,
                [NotNull] string keyName
            )
        {
            Section section = GetSection(sectionName);
            if (section != null)
            {
                section.Remove(keyName);
            }

            return this;
        }

        /// <summary>
        /// Reread from the file.
        /// </summary>
        public void Read()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                return;
            }


#if !SILVERLIGHT

            Encoding encoding = Encoding ?? Encoding.GetEncoding(0);

#else

            Encoding encoding = Encoding ?? Encoding.GetEncoding("windows-1251");

#endif

            Read(FileName, encoding);
        }

        /// <summary>
        /// Reread from the file.
        /// </summary>
        public void Read
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(encoding, "encoding");

            using (StreamReader reader = new StreamReader
                (
                    File.OpenRead(fileName),
                    encoding
                ))
            {
                Read(reader);
            }
        }

        /// <summary>
        /// Reread from the stream.
        /// </summary>
        public void Read
            (
                [NotNull] TextReader reader
            )
        {
            Code.NotNull(reader, "reader");

            char[] separators = {'='};
            _sections.Clear();
            Section section = null;

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                if (line.StartsWith("["))
                {
                    if (!line.EndsWith("]"))
                    {
                        throw new FormatException();
                    }

                    string name = line.Substring(1, line.Length - 2);
                    section = CreateSection(name);
                }
                else
                {
                    if (section == null)
                    {
                        section = new Section(this, null);
                        _sections.Add(section);
                    }

#if WINMOBILE || PocketPC || SILVERLIGHT

                    // TODO Implement properly

                    string[] parts = line.Split(separators);

#else

                    string[] parts = line.Split(separators,2);

#endif

                    string key = parts[0];
                    string value = parts.Length == 2
                        ? parts[1]
                        : null;
                    section.SetValue(key, value);
                }
            }

            ClearModification();
        }

        /// <summary>
        /// Write INI-file into the stream.
        /// </summary>
        /// <param name="writer"></param>
        public void Save
            (
                [NotNull] TextWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            bool first = true;
            foreach (Section section in _sections)
            {
                if (!first)
                {
                    writer.WriteLine();
                }

                _SaveSection
                    (
                        writer,
                        section
                    );

                first = false;
            }

            Modified = false;
        }

        /// <summary>
        /// Save the INI-file to specified file.
        /// </summary>
        public void Save
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

#if !SILVERLIGHT

            Encoding encoding = Encoding ?? Encoding.GetEncoding(0);


#else

            Encoding encoding = Encoding ?? Encoding.GetEncoding("windows-1251");

#endif

            using (StreamWriter writer = new StreamWriter
                (
                    File.Create(fileName),
                    encoding
                ))
            {
                Save(writer);
            }
        }

        /// <summary>
        /// Set value for specified section and key.
        /// </summary>
        [NotNull]
        public IniFile SetValue
            (
                [NotNull] string sectionName,
                [NotNull] string keyName,
                [CanBeNull] string value
            )
        {
            Section section = GetOrCreateSection(sectionName);
            section.SetValue(keyName, value);

            return this;
        }

        /// <summary>
        /// Set value for specified section and key.
        /// </summary>
        [NotNull]
        public IniFile SetValue<T>
            (
                [NotNull] string sectionName,
                [NotNull] string keyName,
                [CanBeNull] T value
            )
        {
            Section section = GetOrCreateSection(sectionName);
            section.SetValue(keyName, value);

            return this;
        }

        /// <summary>
        /// Write modified values to the stream.
        /// </summary>
        public void WriteModifiedValues
            (
                [NotNull] TextWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            bool first = true;
            foreach (Section section in _sections)
            {
                Line[] lines = section
                    .Where(line => line.Modified)
                    .ToArray();

                if (lines.Length != 0)
                {
                    if (!first)
                    {
                        writer.WriteLine();
                    }

                    if (!string.IsNullOrEmpty(section.Name))
                    {
                        writer.WriteLine
                            (
                                "[{0}]",
                                section.Name
                            );
                    }

                    foreach (Line line in lines)
                    {
                        line.Write(writer);
                    }

                    first = false;
                }
                else if (section.Modified)
                {
                    if (!first)
                    {
                        writer.WriteLine();
                    }
                    _SaveSection(writer, section);
                    first = false;
                }
            }
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            FileName = reader.ReadNullableString();
            string encodingName = reader.ReadNullableString();
            Encoding = string.IsNullOrEmpty(encodingName)
                ? null
                : Encoding.GetEncoding(encodingName);
            Modified = reader.ReadBoolean();
            _sections.Clear();
            int count = reader.ReadPackedInt32();
            for (int i = 0; i < count; i++)
            {
                Section section = new Section(this, null);
                section.RestoreFromStream(reader);
                _sections.Add(section);
            }
        }

        /// <inheritdoc />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WriteNullable(FileName);

#if WINMOBILE || PocketPC || SILVERLIGHT

            string encodingName = null;
#else

            string encodingName = Encoding == null
                ? null
                : Encoding.EncodingName;

#endif

            writer.WriteNullable(encodingName);
            writer.Write(Modified);
            writer.WritePackedInt32(_sections.Count);
            foreach (Section section in _sections)
            {
                section.SaveToStream(writer);
            }
        }

        #endregion

        #region IEnumerable<Section> members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<Section> GetEnumerator()
        {
            return _sections.GetEnumerator();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc />
        public void Dispose()
        {
            if (Writable
                && Modified
                && !string.IsNullOrEmpty(FileName))
            {
                Save(FileName);
            }
        }

        #endregion
    }
}
