// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BiblioChapter.cs -- 
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
using AM.Logging;
using AM.Runtime;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Reports;

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
    public class BiblioChapter
        : IAttributable,
        IVerifiable
    {
        #region Properties

        /// <inheritdoc cref="IAttributable.Attributes" />
        [NotNull]
        public ReportAttributes Attributes { get; private set; }

        /// <summary>
        /// Children chapters.
        /// </summary>
        [NotNull]
        [JsonProperty("children")]
        public ChapterCollection Children { get; private set; }

        /// <summary>
        /// Title of the chapter.
        /// </summary>
        [CanBeNull]
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ItemCollection Items { get; protected internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// 
        /// </summary>
        public BiblioChapter()
        {
            Attributes = new ReportAttributes();
            Children = new ChapterCollection(null, this);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Build dictionaries.
        /// </summary>
        public virtual void BuildDictionaries
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin build dictionaries {0}", this);

            foreach (BiblioChapter chapter in Children)
            {
                chapter.BuildDictionaries(context);
            }

            log.WriteLine("End build dictionaries {0}", this);
        }

        /// <summary>
        /// Build <inheritdoc cref="BiblioItem"/>s.
        /// </summary>
        public virtual void BuildItems
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin build items {0}", this);

            foreach (BiblioChapter chapter in Children)
            {
                chapter.BuildItems(context);
            }

            log.WriteLine("End build items {0}", this);
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
            log.WriteLine("Begin gather terms {0}", this);

            foreach (BiblioChapter chapter in Children)
            {
                chapter.GatherTerms(context);
            }

            log.WriteLine("End gather terms {0}", this);
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
            log.WriteLine("Begin gather records {0}", this);

            foreach (BiblioChapter chapter in Children)
            {
                chapter.GatherRecords(context);
            }

            log.WriteLine("End gather records {0}", this);
        }

        /// <summary>
        /// Initialize the chapter.
        /// </summary>
        public virtual void Initialize
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin initialize {0}", this);

            foreach (BiblioChapter child in Children)
            {
                child.Initialize(context);
            }

            log.WriteLine("End initialize {0}", this);
        }

        /// <summary>
        /// Render the chapter.
        /// </summary>
        public virtual void Render
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin render items {0}", this);

            foreach (BiblioChapter chapter in Children)
            {
                chapter.Render(context);
            }

            log.WriteLine("End render items {0}", this);
        }

        /// <summary>
        /// Walk over the chapter and its children.
        /// </summary>
        public void Walk
            (
                [NotNull] Action<BiblioChapter> action
            )
        {
            Code.NotNull(action, "action");

            action(this);
            foreach (BiblioChapter child in Children)
            {
                child.Walk(action);
            }
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public virtual bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<BiblioChapter> verifier
                = new Verifier<BiblioChapter>(this, throwOnError);

            verifier
                .NotNull(Children, "Children")
                .VerifySubObject(Children, "Children");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "{0}: {1}",
                    GetType().Name,
                    Title.ToVisibleString()
                );
        }

        #endregion
    }
}
