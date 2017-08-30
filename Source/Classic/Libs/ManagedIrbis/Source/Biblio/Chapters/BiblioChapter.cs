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
using AM.Text;
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

        /// <summary>
        /// Whether the chapter is active?
        /// </summary>
        [JsonProperty("active")]
        public bool Active { get; set; }

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
        /// Parent chapter (if any).
        /// </summary>
        [CanBeNull]
        [JsonIgnore]
        public BiblioChapter Parent { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public ItemCollection Items { get; protected internal set; }

        /// <summary>
        /// Whether the chapter is for service purpose?
        /// </summary>
        public virtual bool IsServiceChapter { get { return false; } }

        /// <summary>
        /// Special settings associated with the chapter
        /// and its children.
        /// </summary>
        [CanBeNull]
        [JsonProperty("settings")]
        public SpecialSettings Settings { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BiblioChapter()
        {
            Active = true;
            Attributes = new ReportAttributes();
            Children = new ChapterCollection(this);
            Settings = new SpecialSettings();
        }

        #endregion

        #region Private members

        /// <summary>
        /// Get property value.
        /// </summary>
        [CanBeNull]
        protected TResult GetProperty<TChapter, TResult>
            (
                [NotNull] Func<TChapter, TResult> func
            )
            where TChapter: BiblioChapter
        {
            Code.NotNull(func, "func");

            BiblioChapter chapter = this;
            while (!ReferenceEquals(chapter, null))
            {
                TChapter subChapter = chapter as TChapter;
                if (!ReferenceEquals(subChapter, null))
                {
                    TResult result = func(subChapter);
                    if (!ReferenceEquals(result, null))
                    {
                        return result;
                    }
                }

                chapter = chapter.Parent;
            }

            return default(TResult);
        }

        /// <summary>
        /// Render children chapters.
        /// </summary>
        protected virtual void RenderChildren
            (
                [NotNull] BiblioContext context
            )
        {
            foreach (BiblioChapter child in Children)
            {
                if (child.Active)
                {
                    child.Render(context);
                }
            }
        }

        /// <summary>
        /// Render the chapter title.
        /// </summary>
        protected virtual void RenderTitle
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");
            IrbisReport report = processor.Report
                .ThrowIfNull("processor.Report");

            if (!string.IsNullOrEmpty(Title))
            {
                ReportBand title = new ParagraphBand
                {
                    StyleSpecification = @"\s1\plain\f1\fs40\sb400\sa400\b "
                };
                report.Body.Add(title);
                title.Cells.Add(new SimpleTextCell(Title));
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Build dictionaries.
        /// </summary>
        public virtual void BuildDictionary
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin build dictionaries {0}", this);

            foreach (BiblioChapter child in Children)
            {
                if (child.Active)
                {
                    child.BuildDictionary(context);
                }
            }

            log.WriteLine("End build dictionaries {0}", this);
        }

        /// <summary>
        /// Build <see cref="BiblioItem"/>s.
        /// </summary>
        public virtual void BuildItems
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin build items {0}", this);

            foreach (BiblioChapter child in Children)
            {
                if (child.Active)
                {
                    child.BuildItems(context);
                }
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

            foreach (BiblioChapter child in Children)
            {
                if (child.Active)
                {
                    child.GatherTerms(context);
                }
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

            foreach (BiblioChapter child in Children)
            {
                if (child.Active)
                {
                    child.GatherRecords(context);
                }
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
                // Give the chapter a chance
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

            RenderTitle(context);
            RenderChildren(context);

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
