// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MappedInvertedFile64.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !FW35 && !UAP && !WINMOBILE && !PocketPC

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;

using AM;
using AM.IO;
using AM.Logging;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class MappedInvertedFile64
        : IDisposable
    {
        #region Constants

        /// <summary>
        /// Длина записи N01/L01.
        /// </summary>
        public const int NodeLength = 2048;

        /// <summary>
        /// ibatrak максимальный размер термина
        /// </summary>
        public const int MaxTermSize = 255;

        /// <summary>
        /// ibatrak размер блока
        /// </summary>
        public const int BlockSize = 2050048;

        #endregion

        #region Properties

        /// <summary>
        /// File name.
        /// </summary>
        [NotNull]
        public string FileName { get; private set; }

        /// <summary>
        /// IFP file.
        /// </summary>
        [NotNull]
        public MemoryMappedViewStream Ifp { get; private set; }

        /// <summary>
        /// Control record of the IFP file.
        /// </summary>
        [NotNull]
        public IfpControlRecord64 IfpControlRecord { get; private set; }

        /// <summary>
        /// L01 node file.
        /// </summary>
        [NotNull]
        public MemoryMappedViewStream L01 { get; private set; }

        /// <summary>
        /// N01 node file.
        /// </summary>
        [NotNull]
        public MemoryMappedViewStream N01 { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MappedInvertedFile64
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            _lockObject = new object();
            _encoding = new UTF8Encoding(false, true);

            FileName = fileName;

            _ifpFile = DirectUtility.OpenMemoryMappedFile(fileName);
            _l01File = DirectUtility.OpenMemoryMappedFile(Path.ChangeExtension(fileName, ".l01"));
            _n01File = DirectUtility.OpenMemoryMappedFile(Path.ChangeExtension(fileName, ".n01"));

            Ifp = _ifpFile.CreateViewStream(0, 0, MemoryMappedFileAccess.Read);
            L01 = _l01File.CreateViewStream(0, 0, MemoryMappedFileAccess.Read);
            N01 = _n01File.CreateViewStream(0, 0, MemoryMappedFileAccess.Read);

            IfpControlRecord = IfpControlRecord64.Read(Ifp);
        }

        #endregion

        #region Private members

        private readonly object _lockObject;

        private readonly Encoding _encoding;

        private readonly MemoryMappedFile _ifpFile, _l01File, _n01File;

        private long _NodeOffset
            (
                int nodeNumber
            )
        {
            long result = (nodeNumber - 1) * (long)NodeLength;

            return result;
        }

        private NodeRecord64 _ReadNode
            (
                bool isLeaf,
                Stream stream,
                long offset
            )
        {
            lock (_lockObject)
            {
                stream.Position = offset;

                NodeRecord64 result = new NodeRecord64(isLeaf)
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
                    NodeItem64 item = new NodeItem64
                    {
                        Length = stream.ReadInt16Network(),
                        KeyOffset = stream.ReadInt16Network(),
                        LowOffset = stream.ReadInt32Network(),
                        HighOffset = stream.ReadInt32Network()
                    };
                    result.Items.Add(item);
                }

                foreach (NodeItem64 item in result.Items)
                {
                    stream.Position = offset + item.KeyOffset;
                    byte[] buffer = StreamUtility.ReadBytes(stream, item.Length)
                        .ThrowIfNull("buffer");

                    string text = EncodingUtility.GetString
                    (
                        _encoding,
                        buffer
                    );

                    item.Text = text;
                }

                return result;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Read non-leaf node by number.
        /// </summary>
        [NotNull]
        public NodeRecord64 ReadNode
            (
                int number
            )
        {
            NodeRecord64 result = _ReadNode
                (
                    false,
                    N01,
                    _NodeOffset(number)
                );

            return result;
        }

        /// <summary>
        /// Read leaf node by number.
        /// </summary>
        [NotNull]
        public NodeRecord64 ReadLeaf
            (
                int number
            )
        {
            number = Math.Abs(number);
            NodeRecord64 result = _ReadNode
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
        public NodeRecord64 ReadNext
            (
                [NotNull] NodeRecord64 record
            )
        {
            int number = record.Leader.Next;

            if (number < 0)
            {
                return null;
            }

            NodeRecord64 result = _ReadNode
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
        public NodeRecord64 ReadPrevious
            (
                [NotNull] NodeRecord64 record
            )
        {
            int number = record.Leader.Previous;
            if (number < 0)
            {
                return null;
            }

            NodeRecord64 result = _ReadNode
                (
                    record.IsLeaf,
                    record._stream,
                    _NodeOffset(number)
                );

            return result;
        }

        /// <summary>
        /// Read <see cref="IfpRecord64"/> from given offset.
        /// </summary>
        [NotNull]
        public IfpRecord64 ReadIfpRecord
            (
                long offset
            )
        {
            IfpRecord64 result = IfpRecord64.Read(Ifp, offset);

            return result;
        }

        /// <summary>
        /// Read terms.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public TermInfo[] ReadTerms
            (
                [NotNull] TermParameters parameters
            )
        {
            Code.NotNull(parameters, "parameters");

            // TODO Implement reverse order

            lock (_lockObject)
            {
                string key = parameters.StartTerm;
                if (string.IsNullOrEmpty(key))
                {
                    return TermInfo.EmptyArray;
                }

                List<TermInfo> result = new List<TermInfo>();
                try
                {
                    key = IrbisText.ToUpper(key);

                    NodeRecord64 firstNode = ReadNode(1);
                    NodeRecord64 rootNode = ReadNode(firstNode.Leader.Number);
                    NodeRecord64 currentNode = rootNode;

                    NodeItem64 goodItem = null, candidate = null;
                    int goodIndex = 0;
                    while (true)
                    {
                        bool found = false;
                        bool beyond = false;

                        if (ReferenceEquals(currentNode, null))
                        {
                            break;
                        }

                        for (int index = 0; index < currentNode.Leader.TermCount; index++)
                        {
                            NodeItem64 item = currentNode.Items[index];
                            int compareResult = string.CompareOrdinal
                            (
                                item.Text,
                                key
                            );
                            if (compareResult > 0)
                            {
                                candidate = item;
                                goodIndex = index;
                                beyond = true;
                                break;
                            }

                            goodItem = item;
                            goodIndex = index;
                            found = true;

                            if (compareResult == 0
                                && currentNode.IsLeaf)
                            {
                                goto FOUND;
                            }

                        }

                        if (ReferenceEquals(goodItem, null))
                        {
                            break;
                        }

                        if (found)
                        {
                            if (beyond || currentNode.Leader.Next == -1)
                            {
                                if (currentNode.IsLeaf)
                                {
                                    goodItem = candidate;
                                    goto FOUND;
                                }

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
                    if (!ReferenceEquals(goodItem, null))
                    {
                        int count = parameters.NumberOfTerms;
                        while (count > 0)
                        {
                            if (ReferenceEquals(currentNode, null))
                            {
                                break;
                            }

                            TermInfo term = new TermInfo
                            {
                                Text = goodItem.Text,
                                Count = 0
                            };
                            long offset = goodItem.FullOffset;
                            if (offset <= 0)
                            {
                                break;
                            }

                            IfpRecord64 ifp = ReadIfpRecord(offset);
                            term.Count += ifp.BlockLinkCount;
                            result.Add(term);
                            count--;
                            if (count > 0)
                            {
                                if (goodIndex >= currentNode.Leader.TermCount)
                                {
                                    currentNode = ReadNext(currentNode);
                                    goodIndex = 0;
                                }
                                else
                                {
                                    goodIndex++;
                                    goodItem = currentNode.Items[goodIndex];
                                }
                            }
                        }

                        return result.ToArray();
                    }
                }
                catch (Exception exception)
                {
                    Log.TraceException
                        (
                            "InvertedFile64::SearchExact",
                            exception
                        );
                }
            }

            return TermInfo.EmptyArray;
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

            lock (_lockObject)
            {
                try
                {
                    key = IrbisText.ToUpper(key);

                    NodeRecord64 firstNode = ReadNode(1);
                    NodeRecord64 rootNode = ReadNode(firstNode.Leader.Number);
                    NodeRecord64 currentNode = rootNode;

                    NodeItem64 goodItem = null;
                    while (true)
                    {
                        bool found = false;
                        bool beyond = false;

                        if (ReferenceEquals(currentNode, null))
                        {
                            break;
                        }

                        foreach (NodeItem64 item in currentNode.Items)
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

                            if (compareResult == 0
                                && currentNode.IsLeaf)
                            {
                                goto FOUND;
                            }

                        }

                        if (ReferenceEquals(goodItem, null))
                        {
                            break;
                        }

                        if (found)
                        {
                            if (beyond || currentNode.Leader.Next == -1)
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

                            if (currentNode.Items.Count == 0)
                            {
                                goodItem = null;
                                break;
                            }
                        }
                    }

                    FOUND:
                    if (goodItem != null)
                    {
                        // ibatrak записи могут иметь ссылки на следующие

                        List<TermLink> result = new List<TermLink>();
                        long offset = goodItem.FullOffset;
                        while (offset > 0)
                        {
                            IfpRecord64 ifp = ReadIfpRecord(offset);
                            result.AddRange(ifp.Links);
                            offset = ifp.FullOffset > 0
                                ? ifp.FullOffset
                                : 0;
                        }

                        return result
                            .Distinct()
                            .ToArray();
                        // ibatrak до сюда
                    }
                }
                catch (Exception exception)
                {
                    Log.TraceException
                        (
                            "InvertedFile64::SearchExact",
                            exception
                        );
                }
            }

            return TermLink.EmptyArray;
        }

        /// <summary>
        /// Search with truncation.
        /// </summary>
        [NotNull]
        public TermLink[] SearchStart
            (
                [CanBeNull] string key
            )
        {
            if (string.IsNullOrEmpty(key))
            {
                return TermLink.EmptyArray;
            }

            lock (_lockObject)
            {
                List<TermLink> result = new List<TermLink>();

                key = IrbisText.ToUpper(key);

                NodeRecord64 firstNode = ReadNode(1);
                NodeRecord64 rootNode = ReadNode(firstNode.Leader.Number);
                NodeRecord64 currentNode = rootNode;

                NodeItem64 goodItem = null;
                while (true)
                {
                    bool found = false;
                    bool beyond = false;

                    if (ReferenceEquals(currentNode, null))
                    {
                        break;
                    }

                    foreach (NodeItem64 item in currentNode.Items)
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
                        if (beyond || currentNode.Leader.Next == -1)
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

                        foreach (NodeItem64 item in currentNode.Items)
                        {
                            int compareResult = string.CompareOrdinal
                                (
                                    item.Text,
                                    key
                                );
                            if (compareResult >= 0)
                            {
                                bool starts = item.Text.StartsWith(key);
                                if (compareResult > 0 && !starts)
                                {
                                    goto DONE;
                                }

                                if (starts)
                                {
                                    //ibatrak записи могут иметь ссылки на следующие

                                    var offset = item.FullOffset;
                                    while (offset > 0)
                                    {
                                        IfpRecord64 ifp = ReadIfpRecord(offset);
                                        result.AddRange(ifp.Links);
                                        offset = ifp.FullOffset > 0
                                            ? ifp.FullOffset
                                            : 0;
                                    }

                                    //ibatrak до сюда
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

            lock (_lockObject)
            {
                TermLink[] result = TermLink.EmptyArray;

                if (key.EndsWith("$"))
                {
                    key = key.Substring(0, key.Length - 1);
                    if (!string.IsNullOrEmpty(key))
                    {
                        result = SearchStart(key);
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
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            Ifp.Dispose();
            _ifpFile.Dispose();

            L01.Dispose();
            _l01File.Dispose();

            N01.Dispose();
            _n01File.Dispose();
        }

        #endregion
    }
}

#endif
