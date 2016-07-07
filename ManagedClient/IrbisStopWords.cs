/* IrbisStopWords.cs -- STW file handling
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
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
            _dictionary = new Dictionary<string, object>();
        }

        #endregion

        #region Private members

        private readonly Dictionary<string, object> _dictionary;

        #endregion

        #region Public methods

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
                    Encoding.GetEncoding(0)
                );

            return ParseLines
                (
                    name,
                    lines
                );
        }

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
