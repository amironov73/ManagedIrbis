/* MenuFile.cs -- MNU file handling
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Menus
{
    /// <summary>
    /// MNU file handling.
    /// </summary>
    [PublicAPI]
    [XmlRoot("menu")]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [JsonConverter(typeof(MenuConverter))]
#endif
    public sealed class MenuFile
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// End of menu marker.
        /// </summary>
        public const string StopMarker = "*****";

        #endregion

        #region Properties

        /// <summary>
        /// Name of menu file -- for identification
        /// purposes only.
        /// </summary>
        [JsonIgnore]
        public string FileName { get; set; }

        /// <summary>
        /// Gets the entries.
        /// </summary>
        [NotNull]
        [XmlElement("entry")]
        [JsonProperty("entries")]
        public NonNullCollection<MenuEntry> Entries
        {
            get
            {
                return _entries;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuFile()
        {
            _entries = new NonNullCollection<MenuEntry>();
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        internal MenuFile
            (
                NonNullCollection<MenuEntry> entries
            )
        {
            _entries = entries;
        }

        #endregion

        #region Private members

        // ReSharper disable once InconsistentNaming
        internal readonly NonNullCollection<MenuEntry> _entries;

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the specified code and comment.
        /// </summary>
        [NotNull]
        public MenuFile Add
            (
                [NotNull] string code,
                [CanBeNull] string comment
            )
        {
            Code.NotNull(code, "code");

            MenuEntry entry = new MenuEntry
            {
                Code = code,
                Comment = comment
            };
            _entries.Add(entry);

            return this;
        }

        /// <summary>
        /// Trims the code.
        /// </summary>
        [NotNull]
        public static string TrimCode
            (
                [NotNull] string code
            )
        {
            Code.NotNull(code, "code");

            code = code.Trim();
            string[] parts = code.Split(' ', '-', '=', ':');
            if (parts.Length != 0)
            {
                code = parts[0];
            }

            return code;
        }

        /// <summary>
        /// Finds the entry.
        /// </summary>
        [CanBeNull]
        public MenuEntry FindEntry
            (
                [NotNull] string code
            )
        {
            return _entries.FirstOrDefault
                (
                    entry => entry.Code.SameString(code)
                );
        }

        /// <summary>
        /// Finds the entry (case sensitive).
        /// </summary>
        [CanBeNull]
        public MenuEntry FindEntrySensitive
            (
                [NotNull] string code
            )
        {
            return _entries.FirstOrDefault
                (
                    entry => entry.Code.SameStringSensitive(code)
                );
        }

        /// <summary>
        /// Finds the entry.
        /// </summary>
        [CanBeNull]
        public MenuEntry GetEntry
            (
                [NotNull] string code
            )
        {
            Code.NotNull(code, "code");

            MenuEntry candidate = FindEntry(code);
            if (candidate != null)
            {
                return candidate;
            }

            code = code.Trim();
            candidate = FindEntry(code);
            if (candidate != null)
            {
                return candidate;
            }

            code = TrimCode(code);
            candidate = FindEntry(code);
            // ReSharper disable UseNullPropagation
            if (candidate != null)
            {
                return candidate;
            }
            // ReSharper restore UseNullPropagation

            return null;
        }


        /// <summary>
        /// Finds the entry (case sensitive).
        /// </summary>
        [CanBeNull]
        public MenuEntry GetEntrySensitive
            (
                [NotNull] string code
            )
        {
            Code.NotNull(code, "code");

            MenuEntry candidate = FindEntrySensitive(code);
            if (candidate != null)
            {
                return candidate;
            }

            code = code.Trim();
            candidate = FindEntrySensitive(code);
            if (candidate != null)
            {
                return candidate;
            }

            code = TrimCode(code);
            candidate = FindEntrySensitive(code);
            // ReSharper disable UseNullPropagation
            if (candidate != null)
            {
                return candidate;
            }
            // ReSharper restore UseNullPropagation

            return null;
        }

        /// <summary>
        /// Finds comment by the code.
        /// </summary>
        [CanBeNull]
        public string GetString
            (
                [NotNull] string code,
                [CanBeNull] string defaultValue
            )
        {
            Code.NotNull(code, "code");

            MenuEntry found = FindEntry(code);

            return found == null
                ? defaultValue
                : found.Comment;
        }

        /// <summary>
        /// Finds comment by the code.
        /// </summary>
        [CanBeNull]
        public string GetString
            (
                [NotNull] string code
            )
        {
            return GetString(code, null);
        }

        /// <summary>
        /// Finds comment by the code (case sensitive).
        /// </summary>
        [CanBeNull]
        public string GetStringSensitive
            (
                [NotNull] string code,
                [CanBeNull] string defaultValue
            )
        {
            Code.NotNull(code, "code");

            MenuEntry found = FindEntrySensitive(code);

            return found == null
                ? defaultValue
                : found.Comment;
        }

        /// <summary>
        /// Finds comment by the code (case sensitive).
        /// </summary>
        [CanBeNull]
        public string GetStringSensitive
            (
                [NotNull] string code
            )
        {
            return GetStringSensitive(code, null);
        }

        /// <summary>
        /// Parses the specified stream.
        /// </summary>
        [NotNull]
        public static MenuFile ParseStream
            (
                [NotNull] TextReader reader
            )
        {
            Code.NotNull(reader, "reader");

            MenuFile result = new MenuFile();

            while (true)
            {
                string code = reader.ReadLine();
                if (string.IsNullOrEmpty(code))
                {
                    // throw new FormatException();
                    break;
                }
                if (code.StartsWith(StopMarker))
                {
                    break;
                }

                string comment = reader.RequireLine();
                MenuEntry entry = new MenuEntry
                {
                    Code = code,
                    Comment = comment
                };
                result._entries.Add(entry);

            }

            return result;
        }

        /// <summary>
        /// Parses the local file.
        /// </summary>
        [NotNull]
        public static MenuFile ParseLocalFile
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
                MenuFile result = ParseStream(reader);
                result.FileName = Path.GetFileName(fileName);

                return result;
            }
        }

        /// <summary>
        /// Parses the local file.
        /// </summary>
        [NotNull]
        public static MenuFile ParseLocalFile
            (
                [NotNull] string fileName
            )
        {
            return ParseLocalFile
                (
                    fileName,
                    IrbisEncoding.Ansi
                );
        }

        /// <summary>
        /// Parse server response.
        /// </summary>
        [NotNull]
        public static MenuFile ParseServerResponse
            (
                [NotNull] ServerResponse response
            )
        {
            Code.NotNull(response, "response");

            TextReader reader = response.GetReader(IrbisEncoding.Ansi);
            MenuFile result = ParseStream(reader);

            return result;
        }

        /// <summary>
        /// Parse server response.
        /// </summary>
        [NotNull]
        public static MenuFile ParseServerResponse
            (
                [NotNull] string response
            )
        {
            Code.NotNullNorEmpty(response, "response");

            TextReader reader = new StringReader(response);
            MenuFile result = ParseStream(reader);

            return result;
        }

        /// <summary>
        /// Read <see cref="MenuFile"/> from server.
        /// </summary>
        [NotNull]
        public static MenuFile ReadFromServer
            (
                [NotNull] IrbisConnection connection,
                [NotNull] FileSpecification fileSpecification
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(fileSpecification, "fileSpecification");

            string response = connection
                .ReadTextFile(fileSpecification)
                .ThrowIfNull("ReadTextFile");
            MenuFile result = ParseServerResponse(response);

            return result;
        }

        /// <summary>
        /// Sorts the entries.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public MenuEntry[] SortEntries
            (
                MenuSort sortBy
            )
        {
            List<MenuEntry> copy = new List<MenuEntry>(_entries);
            switch (sortBy)
            {
                case MenuSort.ByCode:
                    copy = copy.OrderBy(entry => entry.Code).ToList();
                    break;
                case MenuSort.ByComment:
                    copy = copy.OrderBy(entry => entry.Comment).ToList();
                    break;
            }

            return copy.ToArray();
        }

        /// <summary>
        /// Builds text representation.
        /// </summary>
        public string ToText()
        {
            StringBuilder result = new StringBuilder();

            foreach (MenuEntry entry in _entries)
            {
                result.AppendLine(entry.Code);
                result.AppendLine(entry.Comment);
            }
            result.AppendLine(StopMarker);

            return result.ToString();
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
            FileName = reader.ReadNullableString();
            reader.ReadCollection(_entries);
        }

        /// <summary>
        /// Save object state to the stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(FileName);
            writer.WriteCollection(_entries);
        }

        #endregion

        #region Object members

        #endregion
    }
}
