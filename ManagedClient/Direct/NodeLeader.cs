/* NodeLeader.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Лидер записи в N01, L01
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [DebuggerDisplay("Number={Number}, Previous={Previous}, Next={Next}, "
        + "TermCount={TermCount}, FreeOffset={FreeOffset}")]
    public sealed class NodeLeader
    {
        #region Properties

        /// <summary>
        /// Номер записи (начиная с 1; в N01 номер первой записи
        /// равен номеру корневой записи дерева
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Номер предыдущей записи (-1, если нет)
        /// </summary>
        public int Previous { get; set; }

        /// <summary>
        /// Номер следующей записи (-1, если нет)
        /// </summary>
        public int Next { get; set; }

        /// <summary>
        /// Число ключей в записи
        /// </summary>
        public int TermCount { get; set; }

        /// <summary>
        /// Смещение на свободную позицию в записи
        /// (от начала записи)
        /// </summary>
        public int FreeOffset { get; set; }

        #endregion

        #region Public methods

        public static NodeLeader Read(Stream stream)
        {
            NodeLeader result = new NodeLeader
                {
                    Number = stream.ReadInt32Network(),
                    Previous = stream.ReadInt32Network(),
                    Next = stream.ReadInt32Network(),
                    TermCount = stream.ReadInt32Network(),
                    FreeOffset = stream.ReadInt32Network()
                };

            return result;
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return string.Format
                (
                    "Number: {0}, Previous: {1}, "
                    + "Next: {2}, TermCount: {3}, "
                    + "FreeOffset: {4}", 
                    Number, 
                    Previous, 
                    Next, 
                    TermCount, 
                    FreeOffset
                );
        }

        #endregion
    }
}
