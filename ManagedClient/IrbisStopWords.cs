/* IrbisStopWords.cs -- STW file handling
 * Ars Magna project, http://arsmagna.ru
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
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// STW file handling
    /// </summary>
    [PublicAPI]
    [Serializable]
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
                string fileName
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

        public static IrbisStopWords ParseLines
            (
                string name,
                string[] lines
            )
        {
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

        public static IrbisStopWords ParseText
            (
                string name,
                string text
            )
        {
            string[] lines = text.SplitLines();

            return ParseLines
                (
                    name,
                    lines
                );
        }

        /// <summary>
        /// Parse the file.
        /// </summary>
        public static IrbisStopWords ParseFile
            (
                string fileName
            )
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            string[] lines = File.ReadAllLines
                (
                    fileName,
                    Encoding.Default
                );
            return ParseLines
                (
                    name,
                    lines
                );
        }

        public string[] ToLines()
        {
            return _dictionary
                .Keys
                .OrderBy(word => word)
                .ToArray();
        }

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
