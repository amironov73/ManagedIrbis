// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BiblioItem.cs -- 
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
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

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
    public sealed class BiblioItem
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Chapter the item belongs to.
        /// </summary>
        [CanBeNull]
        public BiblioChapter Chapter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Record.
        /// </summary>
        [CanBeNull]
        public MarcRecord Record { get; set; }

        /// <summary>
        /// Bibliographical description.
        /// </summary>
        [CanBeNull]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        public string Order { get; set; }

        /// <summary>
        /// Terms.
        /// </summary>
        [NotNull]
        public NonNullCollection<BiblioTerm> Terms { get; private set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [JsonIgnore]
        public object UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BiblioItem()
        {
            Terms = new NonNullCollection<BiblioTerm>();
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<BiblioItem> verifier
                = new Verifier<BiblioItem>(this, throwOnError);

            // TODO do something

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = StringBuilderCache.Acquire();
            result.Append(Order);
            result.AppendLine();
            result.Append(Description);

            return StringBuilderCache.GetStringAndRelease(result);
        }

        #endregion
    }
}
