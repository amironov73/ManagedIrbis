// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BookData.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Direct;
using ManagedIrbis.Readers;
using ManagedIrbis.Search;

#endregion

namespace CountGiving
{
    [XmlRoot("book")]
    sealed class BookData
        : IHandmadeSerializable
    {
        #region Properties

        [XmlAttribute("mfn")]
        public int Mfn { get; set; }

        [XmlAttribute("count")]
        public int Count { get; set; }

        [XmlArrayItem("words")]
        public int[] Words { get; set; }

        #endregion

        #region IHandmadeSerializable members

        public void RestoreFromStream(BinaryReader reader)
        {
            Mfn = reader.ReadPackedInt32();
            Count = reader.ReadPackedInt32();
            Words = reader.ReadInt32Array();
        }

        public void SaveToStream(BinaryWriter writer)
        {
            writer
                .WritePackedInt32(Mfn)
                .WritePackedInt32(Count)
                .WriteArray(Words);
        }

        #endregion
    }
}
