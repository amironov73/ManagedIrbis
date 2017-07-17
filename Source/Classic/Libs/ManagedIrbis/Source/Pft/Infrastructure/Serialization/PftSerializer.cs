// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftSerializer.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM;
using AM.IO;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;

using MoonSharp.Interpreter;

#if !SILVERLIGHT

using System.IO.Compression;

#endif

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class PftSerializer
    {
        #region Private members

        private static readonly byte[] _signature =
        {
            0x21, 0x41, 0x53, 0x54
        };


        private static int _CurrentVersion()
        {
            int result =
#if !NETCORE && !SILVERLIGHT && !UAP && !WIN81 && !PORTABLE

                IrbisConnection.ClientVersion.Revision;

#else
                1800;
#endif

            return result;
        }

        #endregion

        #region Public method

        /// <summary>
        /// Deserialize the node.
        /// </summary>
        [NotNull]
        public static PftNode Deserialize
            (
                [NotNull] BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            byte code = reader.ReadByte();
            TypeMap mapping = TypeMap.FindCode(code);

            if (ReferenceEquals(mapping, null))
            {
                Log.Error
                    (
                        "PftSerializer::Deserialize: "
                        + "unknown code="
                        + code
                    );
                Log.Error
                    (
                        "PftSerializer::Deserialize: "
                        + "offset="
                        + reader.BaseStream.Position.ToString("X")
                    );

                throw new PftSerializationException
                    (
                        "Unknown code="
                        + code
                    );
            }

            PftNode result;

            try
            {
#if PORTABLE || WIN81

                result = (PftNode) Activator.CreateInstance(mapping.Type);

#else

                result = mapping.Create();

#endif
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "PftSerializer::Deserialize",
                        exception
                    );
                Log.Error
                    (
                        "PftSerializer::Deserialize: "
                        + "can't create instance of "
                        + mapping.Type.AssemblyQualifiedName
                    );
                Log.Error
                    (
                        "PftSerializer::Deserialize: "
                        + "problem with code="
                        + code
                    );
                Log.Error
                    (
                        "PftSerializer::Deserialize: "
                        + "offset="
                        + reader.BaseStream.Position.ToString("X")
                    );

                throw new PftSerializationException
                    (
                        "AST deserialization error",
                        exception
                    );
            }

            result.Deserialize(reader);

            return result;
        }

        /// <summary>
        /// Deserialize the node collection.
        /// </summary>
        public static void Deserialize
            (
                [NotNull] BinaryReader reader,
                [NotNull] ICollection<PftNode> nodes
            )
        {
            Code.NotNull(reader, "reader");
            Code.NotNull(nodes, "nodes");

            int count = reader.ReadPackedInt32();
            for (int i = 0; i < count; i++)
            {
                PftNode node = Deserialize(reader);
                nodes.Add(node);
            }
        }

        /// <summary>
        /// Deserialize nullable node.
        /// </summary>
        [CanBeNull]
        public static PftNode DeserializeNullable
            (
                [NotNull] BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            bool flag = reader.ReadBoolean();
            PftNode result = flag
                ? Deserialize(reader)
                : null;

            return result;
        }

        /// <summary>
        /// Restore the program from the byte array.
        /// </summary>
        [NotNull]
        public static PftNode FromMemory
            (
                [NotNull] byte[] bytes
            )
        {
            Code.NotNull(bytes, "bytes");

            PftNode result;
            MemoryStream memory = new MemoryStream(bytes);

#if SILVERLIGHT

            using (BinaryReader reader
                = new BinaryReader(memory, IrbisEncoding.Utf8))
            {
                result = Read(reader);
            }

#else

            using (DeflateStream compressor
                = new DeflateStream(memory, CompressionMode.Decompress))
            using (BinaryReader reader
                = new BinaryReader(compressor, IrbisEncoding.Utf8))
            {
                result = Read(reader);
            }

#endif

            return result;
        }

        /// <summary>
        /// Read the AST from the stream.
        /// </summary>
        [NotNull]
        public static PftNode Read
            (
                [NotNull] BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            byte[] signature = new byte[4];
            reader.Read(signature, 0, 4);
            if (ArrayUtility.Compare(signature, _signature) != 0)
            {
                throw new IrbisException();
            }
            int actualVersion = reader.ReadInt32();
            int expectedVersion = _CurrentVersion();
            if (actualVersion != expectedVersion)
            {
                throw new IrbisException();
            }
            /*int offset = */ reader.ReadInt32();
            //reader.BaseStream.Position = offset;
            PftNode result = Deserialize(reader);

            return result;
        }

        /// <summary>
        /// Read the AST from the file.
        /// </summary>
        public static PftNode Read
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

#if PORTABLE || WIN81

            throw new NotSupportedException();

#else

            using (Stream stream = File.OpenRead(fileName))

#if SILVERLIGHT

            using (BinaryReader reader
                = new BinaryReader(stream, IrbisEncoding.Utf8))
            {
                PftNode result = Read(reader);

                return result;
            }

#else

            using (DeflateStream compressor
                = new DeflateStream(stream, CompressionMode.Decompress))
            using (BinaryReader reader
                = new BinaryReader(compressor, IrbisEncoding.Utf8))
            {
                PftNode result = Read(reader);

                return result;
            }

#endif

#endif

        }

        /// <summary>
        /// Save the program to the stream.
        /// </summary>
        public static void Save
            (
                [NotNull] PftNode rootNode,
                [NotNull] BinaryWriter writer
            )
        {
            Code.NotNull(rootNode, "rootNode");
            Code.NotNull(writer, "writer");

            writer.Write(_signature);
            int version = _CurrentVersion();
            writer.Write(version);
            writer.Write(12);
            Serialize(writer, rootNode);
        }

        /// <summary>
        /// Save the program to the file.
        /// </summary>
        public static void Save
            (
                [NotNull] PftNode rootNode,
                [NotNull] string fileName
            )
        {
            Code.NotNull(rootNode, "rootNode");
            Code.NotNullNorEmpty(fileName, "fileName");

#if PORTABLE || WIN81

            throw new NotSupportedException();
#else

            using (Stream stream = File.Create(fileName))

#if SILVERLIGHT

            using (BinaryWriter writer
                = new BinaryWriter(stream, IrbisEncoding.Utf8))
            {
                Save(rootNode, writer);
            }

#else

            using (DeflateStream compressor
                = new DeflateStream(stream, CompressionMode.Compress))
            using (BinaryWriter writer
                = new BinaryWriter(compressor, IrbisEncoding.Utf8))
            {
                Save(rootNode, writer);
            }

#endif

#endif
        }

        /// <summary>
        /// Serialize the node.
        /// </summary>
        public static void Serialize
            (
                [NotNull] BinaryWriter writer,
                [NotNull] PftNode node
            )
        {
            Code.NotNull(writer, "writer");
            Code.NotNull(node, "node");

            Type nodeType = node.GetType();
            TypeMap mapping = TypeMap.FindType(nodeType);

            if (ReferenceEquals(mapping, null))
            {
                Log.Error
                    (
                        "PftSerializer::Serialize: "
                        + "unknown node type="
                        + nodeType.AssemblyQualifiedName
                    );
                PftNodeInfo nodeInfo = node.GetNodeInfo();
                Log.Error
                    (
                        nodeInfo.ToString()
                    );

                throw new IrbisException
                    (
                        "Unknown node type="
                        + nodeType.AssemblyQualifiedName
                    );
            }

            writer.Write(mapping.Code);
            node.Serialize(writer);
        }

        /// <summary>
        /// Serialize the collection of <see cref="PftNode"/>s.
        /// </summary>
        public static void Serialize
            (
                [NotNull] BinaryWriter writer,
                [NotNull] ICollection<PftNode> nodes
            )
        {
            Code.NotNull(writer, "writer");
            Code.NotNull(nodes, "nodes");

            writer.WritePackedInt32(nodes.Count);
            foreach (PftNode node in nodes)
            {
                Serialize(writer, node);
            }
        }

        /// <summary>
        /// Serialize nullable node.
        /// </summary>
        public static void SerializeNullable
            (
                [NotNull] BinaryWriter writer,
                [CanBeNull] PftNode node
            )
        {
            Code.NotNull(writer, "writer");

            if (ReferenceEquals(node, null))
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                Serialize(writer, node);
            }
        }

        /// <summary>
        /// Save the program to byte array.
        /// </summary>
        [NotNull]
        public static byte[] ToMemory
            (
                [NotNull] PftNode rootNode
            )
        {
            Code.NotNull(rootNode, "rootNode");

            MemoryStream memory = new MemoryStream();

#if SILVERLIGHT

            using (BinaryWriter writer
                = new BinaryWriter(memory, IrbisEncoding.Utf8))
            {
                Save(rootNode, writer);
            }

#else

            using (DeflateStream compressor
                = new DeflateStream(memory, CompressionMode.Compress))
            using (BinaryWriter writer
                = new BinaryWriter(compressor, IrbisEncoding.Utf8))
            {
                Save(rootNode, writer);
            }

#endif

            return memory.ToArray();
        }

        #endregion
    }
}
