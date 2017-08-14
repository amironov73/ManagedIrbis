// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BiblioDocument.cs -- 
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
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#if !PORTABLE

using AM.Json;

#endif

#if !WINMOBILE

using System.Runtime.Serialization.Formatters;

#endif

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class BiblioDocument
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Chapters.
        /// </summary>
        [NotNull]
        [JsonProperty("chapters")]
        public ChapterCollection Chapters { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BiblioDocument()
        {
            Chapters = new ChapterCollection(this, null);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Load the file.
        /// </summary>
        [NotNull]
        public static BiblioDocument LoadFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

#if PORTABLE

            throw new NotSupportedException();

#else

            string contents = File.ReadAllText
                (
                    fileName,
                    IrbisEncoding.Utf8
                );
            JObject obj = JObject.Parse(contents);

            JsonUtility.ExpandTypes
                (
                    obj,
                    "ManagedIrbis.Biblio",
                    "ManagedIrbis"
                );

            File.WriteAllText("_dump.json", obj.ToString());

            JsonSerializer serializer = new JsonSerializer
            {
                TypeNameHandling = TypeNameHandling.Objects,
#if ANDROID
                TypeNameAssemblyFormat = json::System.Runtime
                    .Serialization.Formatters
                    .FormatterAssemblyStyle.Simple
#else
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
#endif            
            };

            BiblioDocument result = obj.ToObject<BiblioDocument>
                (
                    serializer
                );

            return result;

#endif
        }

        #endregion

        #region IVerifiable

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<BiblioDocument> verifier
                = new Verifier<BiblioDocument>(this, throwOnError);

            verifier
                .VerifySubObject(Chapters, "Chapters");
                //.VerifySubObject(Filter, "Filter");

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
