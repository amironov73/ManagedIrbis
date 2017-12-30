// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* InvertedFile32.cs -- read inverted (index) file
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Read inverted (index) file of IRBIS32 database.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class InvertedFile32
        : IDisposable
    {
        #region Constants

        /// <summary>
        /// Node length for N01/L01 files.
        /// </summary>
        public const int NodeLength = 2048;

        #endregion

        #region Properties

        /// <summary>
        /// Name of the IFP file.
        /// </summary>
        [NotNull]
        public string FileName { get; private set; }

        /// <summary>
        /// Inverted file.
        /// </summary>
        [NotNull]
        public Stream Ifp { get; private set; }

        /// <summary>
        /// L01 node file.
        /// </summary>
        [NotNull]
        public Stream L01 { get; private set; }

        /// <summary>
        /// L02 node file.
        /// </summary>
        [NotNull]
        public Stream L02 { get; private set; }

        /// <summary>
        /// N01 node file.
        /// </summary>
        [NotNull]
        public Stream N01 { get; private set; }

        /// <summary>
        /// N02 node file.
        /// </summary>
        [NotNull]
        public Stream N02 { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public InvertedFile32
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            _encoding = IrbisEncoding.Ansi;

            FileName = fileName;

            Ifp = _OpenStream(fileName);
            L01 = _OpenStream(Path.ChangeExtension(fileName, ".l01"));
            L02 = _OpenStream(Path.ChangeExtension(fileName, ".l02"));
            N01 = _OpenStream(Path.ChangeExtension(fileName, ".n01"));
            N02 = _OpenStream(Path.ChangeExtension(fileName, ".n02"));
        }

        #endregion

        #region Private members

        private readonly Encoding _encoding;

        private static Stream _OpenStream
            (
                string fileName
            )
        {
            return new FileStream
                (
                    fileName,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite
                );
        }

        private long _NodeOffset
            (
                int nodeNumber
            )
        {
            long result
                = unchecked((((long)nodeNumber) - 1) * NodeLength);

            return result;
        }

        private NodeRecord _ReadNode
            (
                bool isLeaf,
                Stream stream,
                long offset
            )
        {
            stream.Position = offset;

            NodeRecord result = new NodeRecord(isLeaf)
            {
                _stream = stream,
                Leader =
                {
                    Number = stream.ReadInt32Network(),
                    Previous = stream.ReadInt32Network(),
                    Next = stream.ReadInt32Network(),
                    TermCount = stream.ReadInt16Network(),
                    FreeOffset = stream.ReadInt16Network()
                }
            };

            for (int i = 0; i < result.Leader.TermCount; i++)
            {
                NodeItem item = new NodeItem
                {
                    Length = stream.ReadInt16Network(),
                    KeyOffset = stream.ReadInt16Network(),
                    LowOffset = stream.ReadInt32Network(),
                    HighOffset = stream.ReadInt32Network()
                };
                result.Items.Add(item);
            }

            foreach (NodeItem item in result.Items)
            {
                stream.Position = offset + item.KeyOffset;

                byte[] buffer = StreamUtility.ReadBytes(stream, item.Length)
                    .ThrowIfNull("buffer");

                string text = _encoding.GetString(buffer, 0, buffer.Length);
                    item.Text = text;
            }

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Read given non-leaf node.
        /// </summary>
        [NotNull]
        public NodeRecord ReadNode
            (
                int number
            )
        {
            return _ReadNode(false, N01, _NodeOffset(number));
        }

        /// <summary>
        /// Read given leaf node.
        /// </summary>
        [NotNull]
        public NodeRecord ReadLeaf
            (
                int number
            )
        {
            number = Math.Abs(number);
            NodeRecord result = _ReadNode
                (
                    true,
                    L01,
                    _NodeOffset(number)
                );

            return result;
        }

        /// <summary>
        /// Read next node.
        /// </summary>
        /// <returns><c>null</c> if there is no next node.
        /// </returns>
        [CanBeNull]
        public NodeRecord ReadNext
            (
                [NotNull] NodeRecord record
            )
        {
            int number = record.Leader.Next;

            if (number < 0)
            {
                return null;
            }

            NodeRecord result = _ReadNode
                (
                    record.IsLeaf,
                    record._stream,
                    _NodeOffset(number)
                );

            return result;
        }

        /// <summary>
        /// Read previous node.
        /// </summary>
        /// <returns><c>null</c> if there is no previous node.
        /// </returns>
        [CanBeNull]
        public NodeRecord ReadPrevious(NodeRecord record)
        {
            int number = record.Leader.Previous;
            if (number < 0)
            {
                return null;
            }

            NodeRecord result = _ReadNode
                (
                    record.IsLeaf,
                    record._stream,
                    _NodeOffset(number)
                );

            return result;
        }

        /// <summary>
        /// Read <see cref="IfpRecord"/> from given offset.
        /// </summary>
        public IfpRecord ReadIfpRecord
            (
                long offset
            )
        {
            IfpRecord result = IfpRecord.Read(Ifp, offset);

            return result;
        }

        /// <summary>
        /// Search without truncation.
        /// </summary>
        [NotNull]
        public TermLink[] SearchExact
            (
                [CanBeNull] string key
            )
        {
            if (string.IsNullOrEmpty(key))
            {
                return TermLink.EmptyArray;
            }

            key = IrbisText.ToUpper(key);

            NodeRecord firstNode = ReadNode(1);
            NodeRecord rootNode
                = ReadNode(firstNode.Leader.Number);
            NodeRecord currentNode = rootNode;

            NodeItem goodItem = null;

            while (true)
            {
                bool found = false;
                bool beyond = false;

                if (ReferenceEquals(currentNode, null))
                {
                    break;
                }

                foreach (NodeItem item in currentNode.Items)
                {
                    int compareResult = string.CompareOrdinal
                        (
                            item.Text,
                            key
                        );
                    if (compareResult > 0)
                    {
                        beyond = true;
                        break;
                    }

                    goodItem = item;
                    found = true;

                    if ((compareResult == 0)
                        && currentNode.IsLeaf)
                    {
                        goto FOUND;
                    }

                }
                if (goodItem == null)
                {
                    break;
                }
                if (found)
                {
                    if (beyond || (currentNode.Leader.Next == -1))
                    {
                        currentNode = goodItem.RefersToLeaf
                            ? ReadLeaf(goodItem.LowOffset)
                            : ReadNode(goodItem.LowOffset);
                    }
                    else
                    {
                        currentNode = ReadNext(currentNode);
                    }
                }
                else
                {
                    currentNode = goodItem.RefersToLeaf
                        ? ReadLeaf(goodItem.LowOffset)
                        : ReadNode(goodItem.LowOffset);
                }
            }

        FOUND:
            if (goodItem != null)
            {
                IfpRecord ifp = ReadIfpRecord(goodItem.FullOffset);
                return ifp.Links.ToArray();
            }

            return TermLink.EmptyArray;
        }

        /// <summary>
        /// Search with truncation.
        /// </summary>
        [NotNull]
        public TermLink[] SearchTruncated
            (
                [CanBeNull] string key
            )
        {
            if (string.IsNullOrEmpty(key))
            {
                return TermLink.EmptyArray;
            }

            List<TermLink> result = new List<TermLink>();

            key = IrbisText.ToUpper(key);

            NodeRecord firstNode = ReadNode(1);
            NodeRecord rootNode = ReadNode
                (
                    firstNode.Leader.Number
                );
            NodeRecord currentNode = rootNode;

            NodeItem goodItem = null;
            while (true)
            {
                bool found = false;
                bool beyond = false;

                if (ReferenceEquals(currentNode, null))
                {
                    break;
                }

                foreach (NodeItem item in currentNode.Items)
                {
                    int compareResult = string.CompareOrdinal
                        (
                            item.Text,
                            key
                        );
                    if (compareResult > 0)
                    {
                        beyond = true;
                        break;
                    }

                    goodItem = item;
                    found = true;
                }
                if (goodItem == null)
                {
                    break;
                }
                if (found)
                {
                    if (beyond || (currentNode.Leader.Next == -1))
                    {
                        if (goodItem.RefersToLeaf)
                        {
                            goto FOUND;
                        }
                        currentNode = ReadNode(goodItem.LowOffset);
                    }
                    else
                    {
                        currentNode = ReadNext(currentNode);
                    }
                }
                else
                {
                    if (goodItem.RefersToLeaf)
                    {
                        goto FOUND;
                    }
                    currentNode = ReadNode(goodItem.LowOffset);
                }
            }

        FOUND:
            if (goodItem != null)
            {
                currentNode = ReadLeaf(goodItem.LowOffset);

                while (true)
                {
                    if (ReferenceEquals(currentNode, null))
                    {
                        break;
                    }

                    foreach (NodeItem item in currentNode.Items)
                    {
                        int compareResult = string.CompareOrdinal
                            (
                                item.Text,
                                key
                            );
                        if (compareResult >= 0)
                        {
                            bool starts = item.Text.StartsWith(key);
                            if ((compareResult > 0) && !starts)
                            {
                                goto DONE;
                            }
                            if (starts)
                            {
                                IfpRecord ifp
                                    = ReadIfpRecord(item.FullOffset);
                                result.AddRange(ifp.Links);
                            }
                        }
                    }
                    if (currentNode.Leader.Next > 0)
                    {
                        currentNode = ReadNext(currentNode);
                    }
                }

            }

        DONE:
            return result
                .Distinct()
                .ToArray();
        }

        /// <summary>
        /// Perform simple search.
        /// </summary>
        [NotNull]
        public int[] SearchSimple
            (
                [CanBeNull] string key
            )
        {
            if (string.IsNullOrEmpty(key))
            {
                return new int[0];
            }

            TermLink[] result = TermLink.EmptyArray;

            if (key.EndsWith("$"))
            {
                key = key.Substring(0, key.Length - 1);
                if (!string.IsNullOrEmpty(key))
                {
                    result = SearchTruncated(key);
                }
            }
            else
            {
                result = SearchExact(key);
            }

            return result
                .Select(link => link.Mfn)
                .Distinct()
                .ToArray();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Ifp.Dispose();
            L01.Dispose();
            L02.Dispose();
            N01.Dispose();
            N02.Dispose();
        }

        #endregion
    }
}

