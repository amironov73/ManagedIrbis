// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GblNode.cs -- 
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
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Gbl.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class GblNode
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Parent node (if any).
        /// </summary>
        [CanBeNull]
        public GblNode Parent { get; internal set; }

        /// <summary>
        /// Первый параметр, как правило, спецификация поля/подполя.
        /// </summary>
        [CanBeNull]
        [XmlElement("parameter1")]
        [JsonProperty("parameter1")]
        public string Parameter1 { get; set; }

        /// <summary>
        /// Второй параметр, как правило, спецификация повторения.
        /// </summary>
        [CanBeNull]
        [XmlElement("parameter2")]
        [JsonProperty("parameter2")]
        public string Parameter2 { get; set; }

        /// <summary>
        /// Первый формат, например, выражение для замены.
        /// </summary>
        [CanBeNull]
        [XmlElement("format1")]
        [JsonProperty("format1")]
        public string Format1 { get; set; }

        /// <summary>
        /// Второй формат, например, заменяющее выражение.
        /// </summary>
        [CanBeNull]
        [XmlElement("format2")]
        [JsonProperty("format2")]
        public string Format2 { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        /// <summary>
        /// Called after node execution.
        /// </summary>
        protected virtual void OnAfterExecution
            (
                [NotNull] GblContext context
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Called before node execution.
        /// </summary>
        protected virtual void OnBeforeExecution
            (
                [NotNull] GblContext context
            )
        {
            // Nothing to do here
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Execute the node.
        /// </summary>
        public virtual void Execute
            (
                [NotNull] GblContext context
            )
        {
            Code.NotNull(context, "context");

            OnBeforeExecution(context);

            // Nothing to do here

            OnAfterExecution(context);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public virtual bool Verify
            (
                bool throwOnError
            )
        {
            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
