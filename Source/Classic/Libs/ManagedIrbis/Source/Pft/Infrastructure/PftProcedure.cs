// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftProcedure.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Serialization;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Procedure.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftProcedure
        : ICloneable
    {
        #region Properties

        /// <summary>
        /// Procedure body.
        /// </summary>
        [NotNull]
        public PftNodeCollection Body { get; set; }

        /// <summary>
        /// Procedure name.
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftProcedure()
        {
            // TODO fake parent?

            Body = new PftNodeCollection(null);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Deserialize the procedure.
        /// </summary>
        public void Deserialize
            (
                [NotNull] BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Name = reader.ReadNullableString();
            PftSerializer.Deserialize(reader, Body);
        }

        /// <summary>
        /// Execute the procedure.
        /// </summary>
        public void Execute
            (
                [NotNull] PftContext context,
                [CanBeNull] string argument
            )
        {
            Code.NotNull(context, "context");

            using (PftContextGuard guard = new PftContextGuard(context))
            {
                PftContext nested = guard.ChildContext;
                nested.Output = context.Output;
                PftVariableManager variables
                    = new PftVariableManager(context.Variables);
                variables.SetVariable("arg", argument);
                nested.SetVariables(variables);
                nested.Execute(Body);
            }
        }

        /// <summary>
        /// Serialize the procedure.
        /// </summary>
        public void Serialize
            (
                [NotNull] BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WriteNullable(Name);
            PftSerializer.Serialize(writer, Body);
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public object Clone()
        {
            PftProcedure result = (PftProcedure) MemberwiseClone();

            result.Body = Body.CloneNodes(null).ThrowIfNull();

            return result;
        }

        #endregion
    }
}
