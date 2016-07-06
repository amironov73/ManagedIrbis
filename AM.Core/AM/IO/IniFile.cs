/* IniFile.cs -- simple INI-file reader/writer
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;

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
    // ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    // ReSharper restore RedundantNameQualifier
    [DebuggerDisplay("{FileName}")]
    public class IniFile
        : IDisposable,
        IHandmadeSerializable
    {
        #region Nested classes

        #region IniItem

        /// <summary>
        /// Элемент (строка) INI-файла).
        /// </summary>
        [PublicAPI]
        [Serializable]
        [MoonSharpUserData]
        [DebuggerDisplay("{Name}={Value} [{Modified}]")]
        public sealed class IniItem
            : IHandmadeSerializable
        {
            #region Properties

            /// <summary>
            /// Key (name) of the item.
            /// </summary>
            [NotNull]
            public string Name { get; set; }

            /// <summary>
            /// Value of the item.
            /// </summary>
            [CanBeNull]
            public string Value { get; set; }

            /// <summary>
            /// Modification flag.
            /// </summary>
            public bool Modified { get; set; }

            #endregion

            #region Construction

            /// <summary>
            /// Constructor for internal use only.
            /// </summary>
            // ReSharper disable NotNullMemberIsNotInitialized
            internal IniItem()
            {
                // Leave Name=null for a while.
            }
            // ReSharper restore NotNullMemberIsNotInitialized

            /// <summary>
            /// Constructor.
            /// </summary>
            public IniItem
                (
                    [NotNull] string name,
                    [CanBeNull] string value
                )
            {
                Code.NotNullNorEmpty(name, "name");

                Name = name;
                Value = value;
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            public IniItem
                (
                    [NotNull] string name,
                    [CanBeNull] string value,
                    bool modified
                )
            {
                Code.NotNullNorEmpty(name, "name");

                Name = name;
                Value = value;
                Modified = modified;
            }

            #endregion

            #region IHandmadeSerializable members

            /// <summary>
            /// Restore object state from the stream.
            /// </summary>
            public void RestoreFromStream
                (
                    BinaryReader reader
                )
            {
                Name = reader.ReadString();
                Value = reader.ReadNullableString();
                Modified = reader.ReadBoolean();
            }

            /// <summary>
            /// Save object state to the stream.
            /// </summary>
            public void SaveToStream
                (
                    BinaryWriter writer
                )
            {
                writer.Write(Name);
                writer.WriteNullable(Value);
                writer.Write(Modified);
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
                return string.Format
                    (
                        "{0}={1} [{2}]",
                        Name,
                        Value,
                        Modified
                    );
            }

            #endregion

        }

        #endregion

        #region Section

        /// <summary>
        /// INI-file section.
        /// </summary>
        [PublicAPI]
        [DebuggerDisplay("{Name}")]
        public sealed class Section
            : IEnumerable,
            IHandmadeSerializable
        {
            #region Properties

            ///<summary>
            /// INI file owning this section.
            ///</summary>
            [NotNull]
            public IniFile Owner
            {
                [DebuggerStepThrough]
                get
                {
                    return _owner;
                }
            }

            ///<summary>
            /// Section name.
            ///</summary>
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
                    SetName(value);
                }
            }

            /// <summary>
            /// Access to the keys.
            /// </summary>
            [CanBeNull]
            public string this
                [
                    [NotNull] string key
                ]
            {
                [DebuggerStepThrough]
                get
                {
                    IniItem item;
                    Dictionary.TryGetValue(key, out item);

                    return item == null
                        ? null
                        : item.Value;
                }
                set
                {
                    _CheckKey(key);
                    Dictionary[key] = new IniItem
                        (
                            key,
                            value,
                            true
                        );
                    _owner._modified = true;
                }
            }

            /// <summary>
            /// Item count.
            /// </summary>
            public int Count
            {
                [DebuggerStepThrough]
                get
                {
                    return Dictionary.Count;
                }
            }

            /// <summary>
            /// All the keys for this section.
            /// </summary>
            public IEnumerable<string> Keys
            {
                get { return Dictionary.Keys; }
            }

            #endregion

            #region Construction

            /// <summary>
            /// Constructor.
            /// </summary>
            internal Section
                (
                    [NotNull] IniFile owner
                )
            {
                _owner = owner;
                Dictionary = new Dictionary<string, IniItem>
                    (
                        Owner.GetComparer()
                    );
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            internal Section
                (
                    [NotNull] IniFile owner,
                    [NotNull] string name
                )
                : this(owner)
            {
                _name = name;
            }

            #endregion

            #region Private members

            private string _name;

            private readonly IniFile _owner;

            internal readonly Dictionary<string, IniItem> Dictionary;

            private static void _CheckKey
                (
                    [NotNull] string key
                )
            {
                Code.NotNullNorEmpty(key, "key");

                if (key.Contains("="))
                {
                    throw new ArgumentException();
                }
            }

            #endregion

            #region Public methods

            /// <summary>
            /// Add the item to the current section.
            /// </summary>
            public void Add
                (
                    [NotNull] string name,
                    [CanBeNull] string value
                )
            {
                _CheckKey(name);

                Dictionary.Add(name, new IniItem(name, value, false));
                Owner._modified = true;
            }

            /// <summary>
            /// Clear this section.
            /// </summary>
            public void Clear()
            {
                Dictionary.Clear();
                Owner._modified = true;
            }

            /// <summary>
            /// Do section have item with given name (key)?
            /// </summary>
            public bool ContainsKey
                (
                    [NotNull] string key
                )
            {
                Code.NotNull(key, "key");

                return Dictionary.ContainsKey(key);
            }

            /// <summary>
            /// Gets the specified value.
            /// </summary>
            public T Get<T>
                (
                    [NotNull] string keyName,
                    [CanBeNull] T defaultValue
                )
            {
                Code.NotNull(keyName, "keyName");

                string textValue = this[keyName];
                if (string.IsNullOrEmpty(textValue))
                {
                    return defaultValue;
                }

                T result = ConversionUtility.ConvertTo<T>(textValue);
                return result;
            }

            /// <summary>
            /// Sets the specified value.
            /// </summary>
            public Section Set<T>
                (
                    [NotNull] string keyName,
                    [CanBeNull] T value
                )
            {
                Code.NotNull(keyName, "keyName");

                if (ReferenceEquals(value, null))
                {
                    this[keyName] = null;
                }
                else
                {
                    string textValue = ConversionUtility
                        .ConvertTo<string>
                        (
                            value
                        );
                    this[keyName] = textValue;
                }

                return this;
            }

            /// <summary>
            /// Remove item with given name (key).
            /// </summary>
            public void Remove
                (
                    [NotNull] string name
                )
            {
                Code.NotNull(name, "name");

                Dictionary.Remove(name);
                Owner._modified = true;
            }

            /// <summary>
            /// Sets the name for current section.
            /// </summary>
            public void SetName
                (
                    [NotNull] string name
                )
            {
                Code.NotNullNorEmpty(name, "name");

                if (!string.IsNullOrEmpty(_name))
                {
                    Owner.Sections.Remove(_name);
                }

                name = name.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException();
                }

                _name = name;

                Owner.Sections.Add(name, this);
                Owner._modified = true;
            }

            /// <summary>
            /// Trying to get value for given key.
            /// </summary>
            public bool TryGetValue
                (
                    [NotNull] string name,
                    out string value
                )
            {
                IniItem item;
                bool result = Dictionary.TryGetValue
                    (
                        name,
                        out item
                    );
                value = result
                    ? item.Value
                    : null;

                return result;
            }

            #endregion

            #region IEnumerable members

            /// <summary>
            /// Returns an enumerator that iterates
            /// through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Collections.IEnumerator"></see> 
            /// object that can be used to iterate
            /// through the collection.
            /// </returns>
            public IEnumerator GetEnumerator()
            {
                return Dictionary.GetEnumerator();
            }

            #endregion

            #region IHandmadeSerializable members

            /// <summary>
            /// Restore the object state from given stream.
            /// </summary>
            public void RestoreFromStream
                (
                    BinaryReader reader
                )
            {
                _name = reader.ReadNullableString();
                int count = reader.ReadPackedInt32();
                for (int i = 0; i < count; i++)
                {
                    IniItem item = new IniItem();
                    item.RestoreFromStream(reader);
                    Dictionary.Add(item.Name, item);
                }
            }

            /// <summary>
            /// Save the object state from given stream.
            /// </summary>
            public void SaveToStream
                (
                    BinaryWriter writer
                )
            {
                writer.WriteNullable(Name);
                writer.WritePackedInt32(Count);
                foreach (KeyValuePair<string, IniItem> item in Dictionary)
                {
                    item.Value.SaveToStream(writer);
                }
            }

            #endregion
        }

        #endregion

        #endregion

        #region Properties

        ///<summary>
        /// Name of the file.
        ///</summary>
        [CanBeNull]
        [DefaultValue(null)]
        public string FileName { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [Browsable(false)]
        [CanBeNull]
        [DefaultValue(null)]
        public Encoding Encoding { get; set; }

        private Dictionary<string, Section> _sections;

        ///<summary>
        /// 
        ///</summary>
        [Browsable(false)]
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Hidden)]
        public Dictionary<string, Section> Sections
        {
            [DebuggerStepThrough]
            get
            {
                return _sections;
            }
        }

        private bool _modified;

        /// <summary>
        /// Whether INI-file modified.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public bool Modified
        {
            [DebuggerStepThrough]
            get
            {
                return _modified;
            }
        }

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(false)]
        public bool Writable { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="encoding"></param>
        /// <param name="writable"></param>
        public IniFile
            (
                [NotNull] string fileName,
                Encoding encoding,
                bool writable
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            Writable = writable;
            FileName = fileName;
            Reread();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="writable"></param>
        public IniFile
            (
                [NotNull] string fileName,
                bool writable
            )
            : this(fileName, null, writable)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileName"></param>
        public IniFile
            (
                [NotNull] string fileName
            )
            : this(fileName, null, false)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IniFile()
        {
            _CreateSections();
        }

        #endregion

        #region Private members

        private Section _CreateSections()
        {
            _sections = new Dictionary<string, Section>(GetComparer());
            return CreateSection(string.Empty);
        }

        /// <summary>
        /// Get comparer for dictionary.
        /// </summary>
        /// <returns></returns>
        protected virtual IEqualityComparer<string> GetComparer()
        {
            return StringComparer.InvariantCultureIgnoreCase;
        }

        private void _SaveSection
            (
                TextWriter writer,
                Section section
            )
        {
            if (!string.IsNullOrEmpty(section.Name))
            {
                writer.WriteLine("[{0}]", section.Name);
            }

            Dictionary<string, IniItem>.Enumerator line =
                section.Dictionary.GetEnumerator();
            while (line.MoveNext())
            {
                writer.WriteLine
                    (
                        "{0}={1}",
                        line.Current.Key,
                        line.Current.Value.Value
                    );
            }
            if (section.Count != 0)
            {
                writer.WriteLine();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Sections access.
        /// </summary>
        public Section this[string name]
        {
            get
            {
                Section result;

                Sections.TryGetValue(name, out result);

                return result;
            }
        }

        /// <summary>
        /// Item access.
        /// </summary>
        public string this[string sectionName, string keyName]
        {
            get
            {
                Section section = this[sectionName];
                if (section == null)
                {
                    return null;
                }

                return section[keyName];
            }
            set
            {
                Section section = this[sectionName];
                if (section == null)
                {
                    section = CreateSection(sectionName);
                }

                section[keyName] = value;
            }
        }
        
        /// <summary>
        /// Gets the specified item.
        /// </summary>
        public T Get<T>
            (
                [NotNull] string sectionName,
                [NotNull] string keyName,
                T defaultValue
            )
        {
            Code.NotNull(sectionName, "sectionName");
            Code.NotNull(keyName, "keyName");

            string textValue = this[sectionName, keyName];
            if (string.IsNullOrEmpty(textValue))
            {
                return defaultValue;
            }

            T result = ConversionUtility.ConvertTo<T>(textValue);

            return result;
        }
        
        /// <summary>
        /// Sets the specified item.
        /// </summary>
        public IniFile Set<T>
            (
                [NotNull] string sectionName,
                [NotNull] string keyName,
                [CanBeNull] T value
            )
        {
            Code.NotNull(sectionName, "sectionName");
            Code.NotNull(keyName, "keyName");

            if (ReferenceEquals(value, null))
            {
                this[sectionName, keyName] = null;
            }
            else
            {
                string textValue = ConversionUtility
                    .ConvertTo<string>(value);
                this[sectionName, keyName] = textValue;
            }

            return this;
        }

        /// <summary>
        /// Reread from file.
        /// </summary>
        public void Reread()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                throw new ArsMagnaException();
            }

            Section section = _CreateSections();
            Encoding encoding = Encoding ?? Encoding.GetEncoding(0);
            using (StreamReader reader = new StreamReader
                (
                    File.OpenRead(FileName),
                    encoding
                ))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    line = line.Trim();
                    if (line.StartsWith(";"))
                    {
                        continue;
                    }
                    StringBuilder builder = new StringBuilder();
                    char chr;
                    string name, value;
                    if (line.StartsWith("["))
                    {
                        for (int i = 1; i < line.Length; i++)
                        {
                            if ((chr = line[i]) == ']')
                            {
                                break;
                            }
                            builder.Append(chr);
                        }
                        name = builder.ToString().Trim();
                        if (string.IsNullOrEmpty(name))
                        {
                            throw new FormatException();
                        }
                        section = CreateSection(name);
                    }
                    else
                    {
                        bool found = false;
                        int i;
                        for (i = 0; i < line.Length; i++)
                        {
                            if ((chr = line[i]) == '=')
                            {
                                found = true;
                                i++;
                                break;
                            }
                            builder.Append(chr);
                        }
                        if (!found)
                        {
                            throw new FormatException();
                        }
                        name = builder.ToString().Trim();
                        if (string.IsNullOrEmpty(name))
                        {
                            throw new FormatException();
                        }
                        value = line.Substring(i).Trim();
                        section[name] = value;
                    }
                }
            }
            _modified = false;
        }

        /// <summary>
        /// Создаем новую секцию.
        /// </summary>

        public Section CreateSection
            (
                [NotNull] string name
            )
        {
            Code.NotNull(name, "name");

            Section result = new Section(this, name);
            Sections.Add(name, result);

            return result;
        }

        /// <summary>
        /// Записываем в поток.
        /// </summary>
        public virtual void Save
            (
                [NotNull] TextWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            Section root;
            if (Sections.TryGetValue(string.Empty, out root))
            {
                _SaveSection(writer, root);
            }
            foreach (Section section in Sections.Values)
            {
                if (!string.IsNullOrEmpty(section.Name))
                {
                    _SaveSection(writer, section);
                }
            }
            _modified = false;
        }

        /// <summary>
        /// Записываем в файл.
        /// </summary>
        public void Save
            (
                [NotNull] string fileName
            )
        {
            Encoding encoding = Encoding ?? Encoding.GetEncoding(0);
            using (StreamWriter writer
                = new StreamWriter(fileName, false, encoding))
            {
                Save(writer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                throw new ArsMagnaException();
            }

            Save(FileName);
        }

        #endregion

        #region Component members

        /// <summary>
        /// 
        /// </summary>
        public void Dispose ()
        {
            if (Writable
                 && Modified
                 && !string.IsNullOrEmpty(FileName)
                )
            {
                Save();
            }
        }

        #endregion

        #region IHandmadeSerializable

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            FileName = reader.ReadNullableString();
            int count = reader.ReadPackedInt32();
            for (int i = 0; i < count; i++)
            {
                Section section = new Section(this);
                section.RestoreFromStream(reader);
                if (string.IsNullOrEmpty(section.Name))
                {
                    _sections[section.Name] = section;
                }
                else
                {
                    _sections.Add(section.Name, section);
                }
            }
        }

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(FileName);
            writer.WritePackedInt32(Sections.Count);
            foreach (KeyValuePair<string, Section> pair in Sections)
            {
                pair.Value.SaveToStream(writer);
            }
        }

        #endregion
    }
}