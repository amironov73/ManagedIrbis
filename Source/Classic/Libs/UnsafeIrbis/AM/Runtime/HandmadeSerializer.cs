// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HandmadeSerializer.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Reflection;

using UnsafeAM.IO;
using UnsafeAM.Logging;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.Runtime
{
    /// <summary>
    /// Simple serializer.
    /// </summary>
    [PublicAPI]
    public sealed class HandmadeSerializer
    {
        #region Properties

        /// <summary>
        /// Namespace for short type names.
        /// </summary>
        [CanBeNull]
        public string Namespace { get; set; }

        /// <summary>
        /// Assembly for short type names.
        /// </summary>
        [CanBeNull]
        public Assembly Assembly { get; set; }

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
            Code.NotNull(reader, nameof(reader));

            string typeName = reader.ReadString();

            if (PrefixLength == PrefixLength.Short)
            {
                typeName = Namespace + "." + typeName;
            }

            Type type = ReferenceEquals(Assembly, null)
                ? Type.GetType(typeName, true)
                : Assembly.GetType(typeName, true);

            IHandmadeSerializable result = (IHandmadeSerializable) Activator.CreateInstance(type);

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
            Code.NotNull(reader, nameof(reader));

            int count = reader.ReadPackedInt32();
            string typeName = reader.ReadString();

            if (PrefixLength == PrefixLength.Short)
            {
                typeName = Namespace + "." + typeName;
            }

            Type type = ReferenceEquals(Assembly, null)
                ? Type.GetType(typeName, true)
                : Assembly.GetType(typeName, true);

            IHandmadeSerializable[] result = new IHandmadeSerializable[count];

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
            Code.NotNull(obj, nameof(obj));

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
                    Log.Error
                        (
                            "HandmadeSerializer::GetTypeName: "
                            + "unexpected PrefixLength="
                            + PrefixLength
                        );

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
            Code.NotNull(writer, nameof(writer));
            Code.NotNull(obj, nameof(obj));

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
            Code.NotNull(writer, nameof(writer));
            Code.NotNull(array, nameof(array));

            int count = array.Length;
            if (count == 0)
            {
                Log.Error
                    (
                        "HandmadeSerializer::Serialize: "
                        + "count=0"
                    );

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
