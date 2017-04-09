// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ChapterCollection.cs -- 
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
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

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
    public sealed class ChapterCollection
        : NonNullCollection<BiblioChapter>
    {
        #region Properties

        /// <summary>
        /// Document.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public BiblioDocument Document
        {
            get { return _document; }
            internal set
            {
                SetDocument(value);
            }
        }

        /// <summary>
        /// Parent.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public BiblioChapter Parent
        {
            get { return _parent; }
            internal set
            {
                SetParent(value);
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChapterCollection()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChapterCollection
            (
                [CanBeNull] BiblioDocument document,
                [CanBeNull] BiblioChapter parent
            )
        {
            Document = document;
            Parent = parent;
        }

        #endregion

        #region Private members

        private BiblioDocument _document;

        private BiblioChapter _parent;

        internal void SetDocument
            (
                BiblioDocument document
            )
        {
            _document = document;
        }

        internal void SetParent
            (
                BiblioChapter parent
            )
        {
            _parent = parent;
        }

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
