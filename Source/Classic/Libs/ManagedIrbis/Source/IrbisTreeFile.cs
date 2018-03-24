// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisTreeFile.cs -- TRE files handling
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Menus;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// TRE files handling
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisTreeFile
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Tabulation
        /// </summary>
        public const char Indent = '\x09';

        #endregion

        #region Nested classes

        /// <summary>
        /// Tree item
        /// </summary>
        [PublicAPI]
        [MoonSharpUserData]
        [DebuggerDisplay("{Value}")]
        public sealed class Item
            : IHandmadeSerializable,
            IVerifiable
        {
            #region Properties

            /// <summary>
            /// Children.
            /// </summary>
            [NotNull]
            [ItemNotNull]
            [JsonProperty("children")]
            public NonNullCollection<Item> Children
            {
                get { return _children; }
            }

            /// <summary>
            /// Delimiter.
            /// </summary>
            public static string Delimiter
            {
                get { return _delimiter; }
                set { SetDelimiter(value); }
            }

            /// <summary>
            /// Prefix.
            /// </summary>
            [JsonIgnore]
            public string Prefix { get { return _prefix; }}

            /// <summary>
            /// Suffix.
            /// </summary>
            [JsonIgnore]
            public string Suffix { get { return _suffix; } }

            /// <summary>
            /// Value.
            /// </summary>
            [CanBeNull]
            [JsonProperty("value")]
            public string Value
            {
                get { return _value; }
                set
                {
                    SetValue(value);
                }
            }

            #endregion

            #region Construction

            /// <summary>
            /// Constructor.
            /// </summary>
            public Item()
            {
                _children = new NonNullCollection<Item>();
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            public Item
                (
                    [CanBeNull] string value
                )
                : this()
            {
                SetValue(value);
            }

            #endregion

            #region Private members

            private readonly NonNullCollection<Item> _children;

            private static string _delimiter = " - ";

            private string _prefix, _suffix, _value;

            internal int _level;

            #endregion

            #region Public methods

            /// <summary>
            /// Add child.
            /// </summary>
            [NotNull]
            public Item AddChild
                (
                    [CanBeNull] string value
                )
            {
                Item result = new Item(value);
                Children.Add(result);

                return result;
            }

            /// <summary>
            /// Set the delimiter.
            /// </summary>
            public static void SetDelimiter
                (
                    [CanBeNull] string value
                )
            {
                _delimiter = value;
            }

            /// <summary>
            /// Set the value.
            /// </summary>
            public void SetValue
                (
                    [CanBeNull] string value
                )
            {
                Code.NotNullNorEmpty(value, "value");

                _value = value;
                _prefix = null;
                _suffix = null;

                if (!string.IsNullOrEmpty(Delimiter)
                    && !string.IsNullOrEmpty(value))
                {

#if !WINMOBILE && !PocketPC

                    string[] parts = value.Split
                        (
                            new [] {Delimiter},
                            2,
                            StringSplitOptions.None
                        );

                    _prefix = parts[0];
                    if (parts.Length != 1)
                    {
                        _suffix = parts[1];
                    }

#else

                    // TODO Implement

                    Log.Error
                        (
                            "IrbisTreeFile.Item::SetValue: "
                            + "not implemented"
                        );

                    throw new NotImplementedException();

#endif
                }
            }

            /// <summary>
            /// Convert to array of menu items.
            /// </summary>
            [NotNull]
            public MenuEntry[] ToMenu()
            {
                List<MenuEntry> result = new List<MenuEntry>
                {
                    new MenuEntry
                    {
                        Code = Prefix,
                        Comment = Suffix
                    }
                };

                foreach (Item child in Children)
                {
                    result.AddRange(child.ToMenu());
                }

                return result.ToArray();
            }

            /// <summary>
            /// Walk over the tree.
            /// </summary>
            public void Walk
                (
                    [NotNull] Action<Item> action
                )
            {
                Code.NotNull(action, "action");

                action(this);
                foreach (Item child in Children)
                {
                    child.Walk(action);
                }
            }

            #endregion

            #region IHandmadeSerializable members

            /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
            public void RestoreFromStream
                (
                    BinaryReader reader
                )
            {
                Value = reader.ReadNullableString();
                reader.ReadCollection(Children);
            }

            /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
            public void SaveToStream
                (
                    BinaryWriter writer
                )
            {
                writer.WriteNullable(Value);
                writer.WriteCollection(Children);
            }

            #endregion

            #region IVerifiable members

            /// <inheritdoc cref="IVerifiable.Verify" />
            public bool Verify
                (
                    bool throwException
                )
            {
                bool result = !string.IsNullOrEmpty(Value);

                if (result &&
                    Children.Count != 0)
                {
                    result = Children.All
                        (
                            child => child.Verify(throwException)
                        );
                }

                if (!result)
                {
                    Log.Error
                        (
                            "IrbisTreeFile::Verify: "
                            + "verification error"
                        );

                    if (throwException)
                    {
                        throw new VerificationException();
                    }
                }

                return result;
            }


            #endregion

            #region Object members

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// File name.
        /// </summary>
        [CanBeNull]
        public string FileName { get; set; }

        /// <summary>
        /// Root items.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public NonNullCollection<Item> Roots
        {
            get { return _roots; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisTreeFile()
        {
            _roots = new NonNullCollection<Item>();
        }

        #endregion

        #region Private members

        private readonly NonNullCollection<Item> _roots;

        /// <summary>
        /// Determines indent level of the string.
        /// </summary>
        private static int CountIndent
            (
                [NotNull] string line
            )
        {
            int result = 0;

            foreach (char c in line)
            {
                if (c == Indent)
                {
                    result++;
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        private static int _ArrangeLevel
            (
                List<Item> items,
                int level,
                int index,
                int count
            )
        {
            int next = index + 1;
            int level2 = level + 1;

            while (next < count)
            {
                if (items[next]._level <= level)
                {
                    break;
                }

                if (items[next]._level == level2)
                {
                    items[index].Children.Add(items[next]);
                }

                next++;
            }

            return next;
        }

        private static void _ArrangeLevel
            (
                List<Item> items,
                int level
            )
        {
            int count = items.Count;
            int index = 0;

            while (index < count)
            {
                int next = _ArrangeLevel
                    (
                        items,
                        level,
                        index,
                        count
                    );

                index = next;
            }
        }

        private static void _WriteLevel
            (
                TextWriter writer,
                NonNullCollection<Item> items,
                int level
            )
        {
            foreach (Item item in items)
            {
                for (int i = 0; i < level; i++)
                {
                    writer.Write(Indent);
                }
                writer.WriteLine(item.Value);

                _WriteLevel
                    (
                        writer,
                        item.Children,
                        level + 1
                    );
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add root item.
        /// </summary>
        [NotNull]
        public Item AddRoot
            (
                [CanBeNull] string value
            )
        {
            Item result = new Item(value);
            Roots.Add(result);

            return result;
        }

        /// <summary>
        /// Parse specified stream.
        /// </summary>
        [NotNull]
        [MustUseReturnValue]
        public static IrbisTreeFile ParseStream
            (
                [NotNull] TextReader reader
            )
        {
            Code.NotNull(reader, "reader");

            IrbisTreeFile result = new IrbisTreeFile();

            List<Item> list = new List<Item>();
            string line = reader.ReadLine();
            if (ReferenceEquals(line, null))
            {
                goto DONE;
            }
            if (CountIndent(line) != 0)
            {
                Log.Error
                    (
                        "IrbisTreeFile::ParseStream: "
                        + "indent != 0"
                    );

                throw new FormatException();
            }
            list.Add(new Item(line));

            int currentLevel = 0;
            while ((line = reader.ReadLine()) != null)
            {
                int level = CountIndent(line);
                if (level > currentLevel + 1)
                {
                    Log.Error
                        (
                            "IrbisTreeFile::ParseStream: "
                            + "level > currentLevel + 1"
                        );

                    throw new FormatException();
                }
                currentLevel = level;
                line = line.TrimStart(Indent);
                Item item = new Item(line)
                {
                    _level = currentLevel
                };
                list.Add(item);
            }

            int maxLevel = list.Max(item => item._level);
            for (int level = 0; level < maxLevel; level++)
            {
                _ArrangeLevel(list, level);
            }

            var roots = list.Where(item => item._level == 0);
            result.Roots.AddRange(roots);

DONE:
            return result;
        }

        /// <summary>
        /// Read local file.
        /// </summary>
        [NotNull]
        [MustUseReturnValue]
        public static IrbisTreeFile ReadLocalFile
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(encoding, "encoding");

            using (StreamReader reader = TextReaderUtility.OpenRead
                    (
                        fileName,
                        encoding
                    ))
            {
                IrbisTreeFile result = ParseStream(reader);
                result.FileName = Path.GetFileName(fileName);
                
                return result;
            }
        }

        /// <summary>
        /// Save to text stream.
        /// </summary>
        public void Save
            (
                [NotNull] TextWriter writer
            )
        {
            _WriteLevel
                (
                    writer,
                    Roots,
                    0
                );
        }

        /// <summary>
        /// Save to local file.
        /// </summary>
        public void SaveToLocalFile
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(encoding, "encoding");

            using (StreamWriter writer = TextWriterUtility.Create
                    (
                        fileName,
                        encoding
                    ))
            {
                Save(writer);
            }
        }

        /// <summary>
        /// Convert tree to menu.
        /// </summary>
        [NotNull]
        public MenuFile ToMenu()
        {
            MenuFile result = new MenuFile();

            foreach (Item root in Roots)
            {
                result.Entries.AddRange(root.ToMenu());
            }

            return result;
        }

        /// <summary>
        /// Walk over the tree.
        /// </summary>
        public void Walk
            (
                [NotNull] Action<Item> action
            )
        {
            Code.NotNull(action, "action");

            foreach (Item child in Roots)
            {
                child.Walk(action);
            }
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            FileName = reader.ReadNullableString();
            reader.ReadCollection(Roots);
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(FileName);
            writer.WriteCollection(Roots);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwException
            )
        {
            bool result = Roots.Count != 0
                && Roots.All
                    (
                        root => root.Verify(throwException)
                    );

            if (!result)
            {
                Log.Error
                    (
                        "IrbisTreeFile::Verify: "
                        + "verification error"
                    );

                if (throwException)
                {
                    throw new VerificationException();
                }
            }

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
