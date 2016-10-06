﻿/* HandmadeSerializer.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

using AM.IO;
#if !NETCORE

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Runtime
{
    /// <summary>
    /// Simple serializer.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class HandmadeSerializer
    {
        #region Properties

        /// <summary>
        /// Namespace for short type names.
        /// </summary>
        [CanBeNull]
        public string Namespace { get; set; }

        /// <summary>
        /// Prefix length.
        /// </summary>
        public PrefixLength PrefixLength { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public HandmadeSerializer()
        {
            PrefixLength = PrefixLength.Full;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public HandmadeSerializer
            (
                PrefixLength prefixLength
            )
        {
            PrefixLength = prefixLength;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Deserialize object.
        /// </summary>
        [NotNull]
        public IHandmadeSerializable Deserialize
            (
                [NotNull] BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            string typeName = reader.ReadString();

            if (PrefixLength == PrefixLength.Short)
            {
                typeName = Namespace + "." + typeName;
            }

            Type type = Type.GetType(typeName, true);
            IHandmadeSerializable result
                = (IHandmadeSerializable) Activator.CreateInstance(type);

            result.RestoreFromStream(reader);

            return result;
        }

        /// <summary>
        /// Deserialize array of objects.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public IHandmadeSerializable[] DeserializeArray
            (
                [NotNull] BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            int count = reader.ReadPackedInt32();
            string typeName = reader.ReadString();

            if (PrefixLength == PrefixLength.Short)
            {
                typeName = Namespace + "." + typeName;
            }
            Type type = Type.GetType(typeName, true);

            IHandmadeSerializable[] result
                = new IHandmadeSerializable[count];

            for (int i = 0; i < count; i++)
            {
                IHandmadeSerializable obj
                    = (IHandmadeSerializable) Activator.CreateInstance(type);
                obj.RestoreFromStream(reader);
                result[i] = obj;
            }

            return result;
        }

        /// <summary>
        /// Get name of specified type.
        /// </summary>
        [NotNull]
        public string GetTypeName
            (
                [NotNull] object obj
            )
        {
            Code.NotNull(obj, "obj");

            Type type = obj.GetType();
            string result;

            switch (PrefixLength)
            {
                case PrefixLength.Short:
                    result = type.Name;
                    break;

                case PrefixLength.Moderate:
                    result = type.FullName;
                    break;

                case PrefixLength.Full:
                    result = type.AssemblyQualifiedName;
                    break;

                default:
                    throw new InvalidOperationException();
            }

            return result.ThrowIfNull("result");
        }

        /// <summary>
        /// Serialize the object.
        /// </summary>
        [NotNull]
        public HandmadeSerializer Serialize
            (
                [NotNull] BinaryWriter writer,
                [NotNull] IHandmadeSerializable obj
            )
        {
            Code.NotNull(writer, "writer");
            Code.NotNull(obj, "obj");

            string typeName = GetTypeName(obj);
            writer.Write(typeName);

            obj.SaveToStream(writer);

            return this;
        }

        /// <summary>
        /// Serialize the object.
        /// </summary>
        [NotNull]
        public HandmadeSerializer Serialize
            (
                [NotNull] BinaryWriter writer,
                [NotNull][ItemNotNull] IHandmadeSerializable[] array
            )
        {
            Code.NotNull(writer, "writer");
            Code.NotNull(array, "array");

            int count = array.Length;
            if (count == 0)
            {
                throw new ArgumentException();
            }

            writer.WritePackedInt32(count);

            IHandmadeSerializable first = array[0];

            string typeName = GetTypeName(first);
            writer.Write(typeName);

            foreach (IHandmadeSerializable obj in array)
            {
                obj.SaveToStream(writer);
            }

            return this;
        }

        #endregion
    }
}

#endif
