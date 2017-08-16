// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BiblioDocument.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if ANDROID

extern alias json;

#endif

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
using AM.Text.Output;

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

        private int _itemCount;

        private void _NumberChapter
            (
                [NotNull] BiblioChapter chapter
            )
        {
            ItemCollection items = chapter.Items;

            if (!ReferenceEquals(items, null))
            {
                foreach (BiblioItem item in items)
                {
                    item.Number = ++_itemCount;
                }
            }

            foreach (BiblioChapter child in chapter.Children)
            {
                _NumberChapter(child);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Build dictionaries.
        /// </summary>
        public void BuildDictionaries
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin build dictionaries");

            foreach (BiblioChapter chapter in Chapters)
            {
                chapter.BuildDictionary(context);
            }

            log.WriteLine("End build dictionaries");
        }


        /// <summary>
        /// Build items.
        /// </summary>
        public void BuildItems
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin build items");

            foreach (BiblioChapter chapter in Chapters)
            {
                chapter.BuildItems(context);
            }

            log.WriteLine("End build items");
        }

        /// <summary>
        /// Gather records.
        /// </summary>
        public virtual void GatherRecords
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin gather records");

            foreach (BiblioChapter chapter in Chapters)
            {
                chapter.GatherRecords(context);
            }

            log.WriteLine("End gather records");
        }

        /// <summary>
        /// Gather terms.
        /// </summary>
        public virtual void GatherTerms
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin gather terms");

            foreach (BiblioChapter chapter in Chapters)
            {
                chapter.GatherTerms(context);
            }

            log.WriteLine("End gather terms");
        }

        /// <summary>
        /// Initialize the document.
        /// </summary>
        public virtual void Initialize
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin initialize the document");
            foreach (BiblioChapter chapter in Chapters)
            {
                chapter.Initialize(context);
            }
            log.WriteLine("End initialize the document");
        }

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

            // File.WriteAllText("_dump.json", obj.ToString());

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

        /// <summary>
        /// Number items.
        /// </summary>
        public virtual void NumberItems
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;            
            log.WriteLine("Begin number items");
            _itemCount = 0;
            foreach (BiblioChapter chapter in Chapters)
            {
                _NumberChapter(chapter);
            }
            log.WriteLine("Total items: {0}", _itemCount);
            log.WriteLine("End number items");

        }

        /// <summary>
        /// Render items.
        /// </summary>
        public virtual void RenderItems
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin render items");

            foreach (BiblioChapter chapter in Chapters)
            {
                chapter.Render(context);
            }

            log.WriteLine("End render items");
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
