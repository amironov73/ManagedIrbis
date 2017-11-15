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

#if !PORTABLE && !WINMOBILE

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
            Chapters = new ChapterCollection(null);
        }

        #endregion

        #region Private members

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
                if (chapter.Active)
                {
                    chapter.BuildDictionary(context);
                }
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
                if (chapter.Active)
                {
                    chapter.BuildItems(context);
                }
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
                if (chapter.Active)
                {
                    chapter.GatherRecords(context);
                }
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
                if (chapter.Active)
                {
                    chapter.GatherTerms(context);
                }
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
                // Give the chapter a chance
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

#if PORTABLE || WINMOBILE

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

#elif NETCOREAPP2_0

                // TODO fix it
                // TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple

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
            context.ItemCount = 0;

            foreach (BiblioChapter chapter in Chapters)
            {
                if (chapter.Active)
                {
                    chapter.NumberItems(context);
                }
            }

            log.WriteLine("Total items: {0}", context.ItemCount);
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
                if (chapter.Active)
                {
                    chapter.Render(context);
                }
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

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
