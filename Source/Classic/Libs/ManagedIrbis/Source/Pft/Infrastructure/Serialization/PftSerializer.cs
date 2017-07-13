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

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using MoonSharp.Interpreter;

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
        #region Nested classes

        class TypeMap
        {
            public byte Code;

            public Type Type;
        }

        #endregion

        #region Private members

        private static readonly byte[] _signature =
        {
            0x21, 0x41, 0x53, 0x54
        };

        private static readonly TypeMap[] _map =
        {
            new TypeMap { Code=1, Type=typeof(PftA) },
            new TypeMap { Code=2, Type=typeof(PftAbs) },
            new TypeMap { Code=3, Type=typeof(PftAll) },
            new TypeMap { Code=4, Type=typeof(PftAny) },
            new TypeMap { Code=5, Type=typeof(PftAssignment) },
            new TypeMap { Code=6, Type=typeof(PftBang) },
            new TypeMap { Code=7, Type=typeof(PftBlank) },
            new TypeMap { Code=8, Type=typeof(PftBoolean) },
            new TypeMap { Code=9, Type=typeof(PftBreak) },
            new TypeMap { Code=10, Type=typeof(PftC) },
            new TypeMap { Code=11, Type=typeof(PftCeil) },
            new TypeMap { Code=12, Type=typeof(PftCodeBlock) },
            new TypeMap { Code=13, Type=typeof(PftComma) },
            new TypeMap { Code=14, Type=typeof(PftComment) },
            new TypeMap { Code=15, Type=typeof(PftComparison) },
            new TypeMap { Code=16, Type=typeof(PftCondition) },
            new TypeMap { Code=17, Type=typeof(PftConditionalLiteral) },
            new TypeMap { Code=18, Type=typeof(PftConditionalStatement) },
            new TypeMap { Code=19, Type=typeof(PftConditionAndOr) },
            new TypeMap { Code=20, Type=typeof(PftConditionNot) },
            new TypeMap { Code=21, Type=typeof(PftConditionParenthesis) },
            new TypeMap { Code=22, Type=typeof(PftD) },
            new TypeMap { Code=23, Type=typeof(PftEat) },
            new TypeMap { Code=24, Type=typeof(PftEmpty) },
            new TypeMap { Code=25, Type=typeof(PftF) },
            new TypeMap { Code=26, Type=typeof(PftF2) },
            new TypeMap { Code=27, Type=typeof(PftFalse) },
            new TypeMap { Code=28, Type=typeof(PftField) },
            new TypeMap { Code=29, Type=typeof(PftFieldAssignment) },
            new TypeMap { Code=30, Type=typeof(PftFirst) },
            new TypeMap { Code=31, Type=typeof(PftFloor) },
            new TypeMap { Code=32, Type=typeof(PftFor) },
            new TypeMap { Code=33, Type=typeof(PftForEach) },
            new TypeMap { Code=34, Type=typeof(PftFrac) },
            new TypeMap { Code=35, Type=typeof(PftFrom) },
            new TypeMap { Code=36, Type=typeof(PftFunctionCall) },
            new TypeMap { Code=37, Type=typeof(PftG) },
            new TypeMap { Code=38, Type=typeof(PftGroup) },
            new TypeMap { Code=39, Type=typeof(PftHash) },
            new TypeMap { Code=40, Type=typeof(PftHave) },
            new TypeMap { Code=41, Type=typeof(PftInclude) },
            new TypeMap { Code=42, Type=typeof(PftL) },
            new TypeMap { Code=43, Type=typeof(PftLast) },
            new TypeMap { Code=44, Type=typeof(PftLocal) },
            new TypeMap { Code=45, Type=typeof(PftMfn) },
            new TypeMap { Code=46, Type=typeof(PftMinus) },
            new TypeMap { Code=37, Type=typeof(PftMode) },
            new TypeMap { Code=38, Type=typeof(PftN) },
            new TypeMap { Code=39, Type=typeof(PftNested) },
            new TypeMap { Code=40, Type=typeof(PftNl) },
            new TypeMap { Code=41, Type=typeof(PftNode) },
            new TypeMap { Code=42, Type=typeof(PftNumeric) },
            new TypeMap { Code=43, Type=typeof(PftNumericExpression) },
            new TypeMap { Code=44, Type=typeof(PftNumericLiteral) },
            new TypeMap { Code=45, Type=typeof(PftOrphan) },
            new TypeMap { Code=46, Type=typeof(PftP) },
            new TypeMap { Code=47, Type=typeof(PftParallelFor) },
            new TypeMap { Code=48, Type=typeof(PftParallelForEach) },
            new TypeMap { Code=49, Type=typeof(PftParallelGroup) },
            new TypeMap { Code=50, Type=typeof(PftParallelWith) },
            new TypeMap { Code=51, Type=typeof(PftPercent) },
            new TypeMap { Code=52, Type=typeof(PftPow) },
            new TypeMap { Code=53, Type=typeof(PftProcedureDefinition) },
            new TypeMap { Code=54, Type=typeof(PftProgram) },
            new TypeMap { Code=55, Type=typeof(PftRef) },
            new TypeMap { Code=56, Type=typeof(PftRepeatableLiteral) },
            new TypeMap { Code=57, Type=typeof(PftRound) },
            new TypeMap { Code=58, Type=typeof(PftRsum) },
            new TypeMap { Code=59, Type=typeof(PftS) },
            new TypeMap { Code=60, Type=typeof(PftSemicolon) },
            new TypeMap { Code=61, Type=typeof(PftSign) },
            new TypeMap { Code=62, Type=typeof(PftSlash) },
            new TypeMap { Code=63, Type=typeof(PftTrue) },
            new TypeMap { Code=64, Type=typeof(PftTrunc) },
            new TypeMap { Code=65, Type=typeof(PftUnconditionalLiteral) },
            new TypeMap { Code=66, Type=typeof(PftUnifor) },
            new TypeMap { Code=67, Type=typeof(PftV) },
            new TypeMap { Code=68, Type=typeof(PftVal) },
            new TypeMap { Code=69, Type=typeof(PftVariableReference) },
            new TypeMap { Code=70, Type=typeof(PftVerbatim) },
            new TypeMap { Code=71, Type=typeof(PftWhile) },
            new TypeMap { Code=72, Type=typeof(PftWith) },
            new TypeMap { Code=73, Type=typeof(PftX) }
        };

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
            TypeMap mapping = null;
            for (int i = 0; i < _map.Length; i++)
            {
                if (code == _map[i].Code)
                {
                    mapping = _map[i];
                    break;
                }
            }

            if (ReferenceEquals(mapping, null))
            {
                Log.Error
                    (
                        "PftSerializer::Deserialize: "
                        + "unknown code="
                        + code
                    );

                throw new IrbisException
                    (
                        "Unknown code="
                        + code
                    );
            }

            PftNode result
                = (PftNode)Activator.CreateInstance(mapping.Type);

            result.DeserializeAst(reader);

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
            /* int version = */ reader.ReadInt32();
            int offset = reader.ReadInt32();
            reader.BaseStream.Position = offset;
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

            using (Stream stream = File.OpenRead(fileName))
            using (BinaryReader reader
                = new BinaryReader(stream, IrbisEncoding.Utf8))
            {
                PftNode result = Read(reader);

                return result;
            }
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
            int version =
#if !NETCORE && !SILVERLIGHT && !UAP && !WIN81 && !PORTABLE

                 IrbisConnection.ClientVersion.Build;

#else
                1800;
#endif
            writer.Write(version);
            writer.Write(8);
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

            using (Stream stream = File.Create(fileName))
            using (BinaryWriter writer
                = new BinaryWriter(stream, IrbisEncoding.Utf8))
            {
                Save(rootNode, writer);
            }
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
            TypeMap mapping = null;
            for (int i = 0; i < _map.Length; i++)
            {
                if (nodeType == _map[i].Type)
                {
                    mapping = _map[i];
                    break;
                }
            }

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
            node.SerializeAst(writer);
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

        #endregion
    }
}
