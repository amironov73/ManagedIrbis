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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace IniReader
{
    /// <summary>
    /// Simple INI-file reader/writer.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [System.ComponentModel.DesignerCategory("Code")]
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class IniFile
        : Component
    {
        #region Nested classes

        /// <summary>
        /// INI-file section.
        /// </summary>
        public sealed class Section
            : IEnumerable
        {
            #region Properties

            private IniFile _owner;

            ///<summary>
            /// INI file owning this section.
            ///</summary>
            public IniFile Owner
            {
                [DebuggerStepThrough]
                get
                {
                    return _owner;
                }
            }

            private string _name;

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
                set
                {
                    _owner._sections.Remove(_name);
                    _name = value.Trim();
                    if ((_name != null)
                         && (_name == string.Empty))
                    {
                        throw new ArgumentException();
                    }
                    _owner._sections.Add(value, this);
                    _owner._modified = true;
                }
            }

            /// <summary>
            /// Access to the keys.
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public string this[string key]
            {
                [DebuggerStepThrough]
                get
                {
                    return _dictionary[key];
                }
                set
                {
                    _CheckKey(key);
                    _dictionary[key] = value;
                    _owner._modified = true;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public int Count
            {
                [DebuggerStepThrough]
                get
                {
                    return _dictionary.Count;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public IEnumerable<string> Keys
            {
                get
                {
                    return _dictionary.Keys;
                }
            }

            #endregion

            #region Construction

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="name"></param>
            internal Section(IniFile owner, string name)
            {
                _owner = owner;
                _name = name;
                _dictionary = new Dictionary<string, string>(Owner.GetComparer());
            }

            #endregion

            #region Private members

            internal Dictionary<string, string> _dictionary;

            private static void _CheckKey(string key)
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException();
                }
                if (key.Contains("="))
                {
                    throw new ArgumentException();
                }
            }

            #endregion

            #region Public methods

            /// <summary>
            /// 
            /// </summary>
            /// <param name="name"></param>
            /// <param name="value"></param>
            public void Add(string name, string value)
            {
                _CheckKey(name);
                _dictionary.Add(name, value);
                Owner._modified = true;
            }

            /// <summary>
            /// 
            /// </summary>
            public void Clear()
            {
                _dictionary.Clear();
                Owner._modified = true;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public bool ContainsKey(string key)
            {
                return _dictionary.ContainsKey(key);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="name"></param>
            public void Remove(string name)
            {
                _dictionary.Remove(name);
                Owner._modified = true;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="name"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public bool TryGetValue(string name, out string value)
            {
                return _dictionary.TryGetValue(name, out value);
            }

            #endregion

            #region IEnumerable members

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Collections.IEnumerator"></see> 
            /// object that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator GetEnumerator()
            {
                return _dictionary.GetEnumerator();
            }

            #endregion
        }

        #endregion

        #region Properties

        private string _fileName;

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(null)]
        public string FileName
        {
            [DebuggerStepThrough]
            get
            {
                return _fileName;
            }
            [DebuggerStepThrough]
            set
            {
                _fileName = value;
            }
        }

        private Encoding _encoding;

        ///<summary>
        /// 
        ///</summary>
        [Browsable(false)]
        [DefaultValue(null)]
        public Encoding Encoding
        {
            [DebuggerStepThrough]
            get
            {
                return _encoding;
            }
            [DebuggerStepThrough]
            set
            {
                _encoding = value;
            }
        }

        private Dictionary<string, Section> _sections;

        ///<summary>
        /// 
        ///</summary>
        [Browsable(false)]
        [
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden
                )]
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

        private bool _writable;

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(false)]
        public bool Writable
        {
            [DebuggerStepThrough]
            get
            {
                return _writable;
            }
            [DebuggerStepThrough]
            set
            {
                _writable = value;
            }
        }

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

            _writable = writable;
            _fileName = fileName;
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

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="container"></param>
        public IniFile
            (
                [NotNull] IContainer container
            )
        {
            Code.NotNull(container, "container");

            container.Add(this);
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

        private void _SaveSection(StreamWriter writer, Section section)
        {
            if (!string.IsNullOrEmpty(section.Name))
            {
                writer.WriteLine("[{0}]", section.Name);
            }
            Dictionary<string, string>.Enumerator line =
                section._dictionary.GetEnumerator();
            while (line.MoveNext())
            {
                writer.WriteLine("{0}={1}", line.Current.Key, line.Current.Value);
            }
            if (section.Count != 0)
            {
                writer.WriteLine();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Reread from file.
        /// </summary>
        public virtual void Reread()
        {
            Section section = _CreateSections();
            Encoding encoding = Encoding ?? Encoding.Default;
            using (StreamReader reader = new StreamReader(_fileName, encoding))
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
        /// 
        /// </summary>

        public Section CreateSection(string name)
        {
            Section result = new Section(this, name);
            Sections.Add(name, result);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Save
            (
                StreamWriter writer
            )
        {
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
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            Encoding encoding = Encoding ?? Encoding.Default;
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
            Save(FileName);
        }

        #endregion

        #region Component members

        /// <summary>
        /// 
        /// </summary>
        protected override void Dispose
            (
                bool disposing
            )
        {
            base.Dispose(disposing);
            if (Writable
                 && Modified
                 && !string.IsNullOrEmpty(FileName)
                )
            {
                Save();
            }
        }

        #endregion
    }
}