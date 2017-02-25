// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisStopWords.cs -- STW file handling
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Linq;

using AM;
using AM.Collections;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    //
    // STW file example:
    //
    // A
    // ABOUT
    // AFTER
    // AGAINST
    // ALL
    // ALS
    // AN
    // AND
    // AS
    // AT
    // AUF
    // AUS
    // AUX
    // B
    // BIJ
    // BY
    //

    /// <summary>
    /// STW file handling.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisStopWords
    {
        #region Properties

        /// <summary>
        /// File name (for identification only).
        /// </summary>
        [CanBeNull]
        public string FileName { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisStopWords()
        {
            _dictionary = new CaseInsensitiveDictionary<object>();
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="IrbisStopWords"/> class.
        /// </summary>
        /// <param name="fileName">The name.</param>
        public IrbisStopWords
            (
                [CanBeNull] string fileName
            )
        {
            FileName = fileName;
            _dictionary = new CaseInsensitiveDictionary<object>();
        }

        #endregion

        #region Private members

        private readonly CaseInsensitiveDictionary<object> _dictionary;

        #endregion

        #region Public methods

        /// <summary>
        /// Load stopword list from server.
        /// </summary>
        [NotNull]
        public static IrbisStopWords FromServer
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            string database = connection.Database
                .ThrowIfNull("database not set");
            string fileName = database + ".stw";

            return FromServer
                (
                    connection,
                    database,
                    fileName
                );
        }

        /// <summary>
        /// Load stopword list from server.
        /// </summary>
        [NotNull]
        public static IrbisStopWords FromServer
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string database,
                [NotNull] string fileName
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");
            Code.NotNullNorEmpty(fileName, "fileName");

            FileSpecification specification
                = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        database,
                        fileName
                    );

            string text = connection.ReadTextFile
                (
                    specification
                );

            if (string.IsNullOrEmpty(text))
            {
                text = string.Empty;
            }

            IrbisStopWords result = ParseText
                (
                    fileName,
                    text
                );

            return result;
        }

        /// <summary>
        /// Is given word is stopword?
        /// </summary>
        public bool IsStopWord
            (
                [CanBeNull] string word
            )
        {
            if (string.IsNullOrEmpty(word))
            {
                return true;
            }

            word = word.Trim();
            if (string.IsNullOrEmpty(word))
            {
                return true;
            }

            return _dictionary.ContainsKey(word);
        }

        /// <summary>
        /// Parse array of plain text lines.
        /// </summary>
        [NotNull]
        public static IrbisStopWords ParseLines
            (
                [CanBeNull] string name,
                [NotNull][ItemNotNull] string[] lines
            )
        {
            Code.NotNull(lines, "lines");

            IrbisStopWords result = new IrbisStopWords(name);

            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                {
                    result._dictionary[trimmed] = null;
                }
            }

            return result;
        }

        /// <summary>
        /// Parse plain text.
        /// </summary>
        [NotNull]
        public static IrbisStopWords ParseText
            (
                [CanBeNull] string name,
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            string[] lines = text.SplitLines();

            return ParseLines
                (
                    name,
                    lines
                );
        }

#if !WINMOBILE && !PocketPC && !SILVERLIGHT && !WIN81

        /// <summary>
        /// Parse the text file.
        /// </summary>
        [NotNull]
        public static IrbisStopWords ParseFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            string name = Path.GetFileNameWithoutExtension(fileName);
            string[] lines = File.ReadAllLines
                (
                    fileName,
                    IrbisEncoding.Ansi
                );

            return ParseLines
                (
                    name,
                    lines
                );
        }

#endif

        /// <summary>
        /// Convert <see cref="IrbisStopWords"/> to array
        /// of text lines.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public string[] ToLines()
        {
            return _dictionary
                .Keys
                .OrderBy(word => word)
                .ToArray();
        }

        /// <summary>
        /// Convert <see cref="IrbisStopWords"/> to plain text.
        /// </summary>
        [NotNull]
        public string ToText()
        {
            return string.Join
                (
                    Environment.NewLine,
                    ToLines()
                );
        }

        #endregion
    }
}

