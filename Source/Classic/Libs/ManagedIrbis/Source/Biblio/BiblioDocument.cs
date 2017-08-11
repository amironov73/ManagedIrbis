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
using AM.Json;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

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

        ///// <summary>
        ///// Menu name.
        ///// </summary>
        //[CanBeNull]
        //[JsonProperty("menu")]
        //public string MenuName { get; set; }

        ///// <summary>
        ///// Record filter: search expression.
        ///// </summary>
        //[CanBeNull]
        //[JsonProperty("filter")]
        //public string Filter { get; set; }

        /// <summary>
        /// Record format.
        /// </summary>
        [CanBeNull]
        [JsonProperty("format")]
        public string Format { get; set; }

        /// <summary>
        /// Chapters.
        /// </summary>
        [NotNull]
        [JsonProperty("chapters")]
        public ChapterCollection Chapters { get; private set; }

        ///// <summary>
        ///// Global filter for the document.
        ///// </summary>
        //[JsonProperty("filter")]
        //public BiblioFilter Filter { get; set; }

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

            BiblioDocument result = JsonUtility
                .ReadObjectFromFile<BiblioDocument>(fileName);

            return result;
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
