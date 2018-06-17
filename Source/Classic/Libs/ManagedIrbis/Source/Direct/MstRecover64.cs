// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MstRecover64.cs -- MST file recover
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

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// MST file recover.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class MstRecover64
    {
        #region Public methods

        /// <summary>
        /// Find records.
        /// </summary>
        [NotNull]
        public static FoundRecord[] FindRecords
            (
                [NotNull] string fileName
            )
        {
            Code.FileExists(fileName, "fileName");

            Dictionary<int, FoundRecord> dictionary = new Dictionary<int, FoundRecord>();
            using (Stream stream = File.OpenRead(fileName))
            {
                long fileLength = stream.Length;
                //MstControlRecord64 control = MstControlRecord64.Read(stream);
                //long position = MstControlRecord64.RecordSize;
                long position = 0x24;
                stream.Position = position;
                //int nextMfn = control.NextMfn;

                while (position < fileLength)
                {
                    try
                    {
                        MstRecordLeader64 leader = MstRecordLeader64.Read(stream);
                        if (leader.Mfn == 0)
                        {
                            break;
                        }

                        int mfn = leader.Mfn;
                        int length = leader.Length;
                        FoundRecord record = new FoundRecord
                        {
                            Mfn = mfn,
                            Position = position,
                            Length = length,
                            FieldCount = leader.Nvf,
                            Version = leader.Version,
                            Flags = leader.Status
                        };
                        dictionary[mfn] = record;

                        position += length;
                        stream.Position = position;
                    }
                    catch
                    {
                        break;
                    }
                }
            }

            return dictionary.Values.ToArray();
        }

        #endregion
    }
}
