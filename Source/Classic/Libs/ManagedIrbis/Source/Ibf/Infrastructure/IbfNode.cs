// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IbfNode.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ManagedIrbis.Ibf.Infrastructure
{
    //
    // http://sntnarciss.ru/irbis/spravka/wa0101104100.htm
    // http://sntnarciss.ru/irbis/spravka/wa0203050100.htm
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class IbfNode
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object UserData { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        /// <summary>
        /// Called after node execution.
        /// </summary>
        protected virtual void OnAfterExecution
            (
                [NotNull] IbfContext context
            )
        {
            // Nothing to do here yet
        }

        /// <summary>
        /// Called before node execution.
        /// </summary>
        protected virtual void OnBeforeExecution
            (
                [NotNull] IbfContext context
            )
        {
            // Nothing to do here yet
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Execute the command.
        /// </summary>
        public virtual void Execute
            (
                [NotNull] IbfContext context
            )
        {
            Code.NotNull(context, "context");

            OnBeforeExecution(context);

            // Nothing to do here yet

            OnAfterExecution(context);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public virtual void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");
        }

        #endregion

        #region IVerifiable methods

        /// <inheritdoc cref="IVerifiable.Verify" />
        public virtual bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<IbfNode> verifier = new Verifier<IbfNode>(this, throwOnError);

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            string result = GetType().Name;
            if (result.StartsWith("Ibf"))
            {
                result = result.Substring(3);
            }

            return result;
        }

        #endregion
    }
}
