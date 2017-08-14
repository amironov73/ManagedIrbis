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
        public ItemCollection Items { get; private set; }

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
        /// Render the chapter.
        /// </summary>
        public virtual void Render
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            // Nothing to do here.

            Log.Warn
                (
                    "BiblioChapter::Render: "
                    + "must be overriden"
                );
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
