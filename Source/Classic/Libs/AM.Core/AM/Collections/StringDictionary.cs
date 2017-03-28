// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StringDictionary.cs -- simple string-string dictionary
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// Simple "string-string" <see cref="Dictionary{T1,T2}"/>
    /// with saving-restoring facility.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("Count = {Count}")]
#endif
    public sealed class StringDictionary
        : Dictionary<string, string>,
        IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// End-of-Dictionary mark.
        /// </summary>
        public const string EndOfDictionary = "*****";

        #endregion

        #region Public methods

        /// <summary>
        /// Loads <see cref="StringDictionary"/> from 
        /// the specified <see cref="StreamReader"/>.
        /// </summary>
        [NotNull]
        public static StringDictionary Load
            (
                [NotNull] TextReader reader
            )
        {
            Code.NotNull(reader, "reader");

            StringDictionary result = new StringDictionary();
            while (true)
            {
                string key;
                string value;
                if (((key = reader.ReadLine()) == null)
                    || key.StartsWith(EndOfDictionary)
                    || ((value = reader.ReadLine()) == null))
                {
                    break;
                }
                result.Add(key, value);
            }

            return result;
        }

#if !WIN81 && !PORTABLE

        /// <summary>
        /// Loads <see cref="StringDictionary"/> from the specified file.
        /// </summary>
        [NotNull]
        public static StringDictionary Load
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(encoding, "encoding");

            using (TextReader reader = new StreamReader
                (
                    File.OpenRead
                    (
                        fileName
                    ),
                    encoding
                ))
            {
                return Load(reader);
            }
        }

#endif

        /// <summary>
        /// Saves the <see cref="StringDictionary"/> with specified writer.
        /// </summary>
        public void Save
            (
                [NotNull] TextWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            foreach (KeyValuePair<string, string> pair in this)
            {
                writer.WriteLine(pair.Key);
                writer.WriteLine(pair.Value);
            }

            writer.WriteLine(EndOfDictionary);
        }

#if !WIN81 && !PORTABLE

        /// <summary>
        /// Saves the <see cref="StringDictionary"/> to specified file.
        /// </summary>
        public void Save
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(encoding, "encoding");

            using (TextWriter writer
                = new StreamWriter(File.Create(fileName), encoding))
            {
                Save(writer);
            }
        }

#endif

#endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore the object state from the specified stream.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Clear();

            int count = reader.ReadPackedInt32();
            for (int i = 0; i < count; i++)
            {
                string key = reader.ReadString();
                string value = reader.ReadNullableString();
                Add(key, value);
            }
        }

        /// <summary>
        /// Save the object state to the specified stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WritePackedInt32(Count);
            foreach (KeyValuePair<string, string> pair in this)
            {
                writer.Write(pair.Key);
                writer.WriteNullable(pair.Value);
            }
        }

        #endregion
    }
}
