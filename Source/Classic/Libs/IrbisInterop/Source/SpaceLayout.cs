// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SpaceLayout.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using AM.Runtime;

using CodeJam;
using IrbisInterop;
using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace IrbisInterop
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SpaceLayout
    {
        #region Constants

        private const string MagicString = "MagicString";

        #endregion

        #region Properties

        /// <summary>
        /// Minimal version.
        /// </summary>
        [JsonProperty("min-version")]
        public string MinVersion { get; set; }

        /// <summary>
        /// Maximal version.
        /// </summary>
        [JsonProperty("max-version")]
        public string MaxVersion { get; set; }

        /// <summary>
        /// Offset of formatted record text in the space.
        /// </summary>
        [JsonProperty("formatted")]
        public int FormattedOffset { get; set; }

        /// <summary>
        /// Offset of native record in the space.
        /// </summary>
        [JsonProperty("record")]
        public int RecordOffset { get; set; }

        // =========================================================

        /// <summary>
        /// 
        /// </summary>
        public static int GlobalFormattedOffset { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static int GlobalRecordOffset { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Find the formatted text in the space.
        /// </summary>
        public void FindTheFormattedText
            (
                IntPtr space,
                int blockSize
            )
        {
            if (FormattedOffset != 0)
            {
                return;
            }

            if (GlobalFormattedOffset != 0)
            {
                FormattedOffset = GlobalFormattedOffset;
                return;
            }

            string format = "'" + MagicString + "'";
            int retCode = Irbis65Dll.IrbisInitPft(space, format);
            if (retCode < 0)
            {
                throw new IrbisException();
            }
            retCode = Irbis65Dll.IrbisFormat
            (
                space,
                0 /*номер полки*/,
                1,
                0,
                Irbis64Dll.BufferSize,
                Irbis64Dll.DllName
            );
            if (retCode < 0)
            {
                throw new IrbisException();
            }

            Irbis65Dll.IrbisInitUactab(space);

            Encoding encoding = IrbisEncoding.Utf8;
            int length = encoding.GetByteCount(MagicString);
            byte[] expected = new byte[length + 1];
            encoding.GetBytes(MagicString, 0, length, expected, 0);
            length++;

            //int minAddress = space.ToInt32() - 0x100000;
            //int maxAddress = space.ToInt32() + 0x100000;

            for (int offset = 4; offset < blockSize; offset++)
            {
                IntPtr pointer = Marshal.ReadIntPtr
                    (
                        space,
                        offset
                    );
                int address = pointer.ToInt32();
                //if (address < minAddress || address > maxAddress)
                //{
                //    continue;
                //}

                if (Win32.IsBadReadPtr(address, 32000))
                {
                    continue;
                }

                bool found = true;
                for (int i = 0; i < length; i++)
                {
                    byte b = Marshal.ReadByte(pointer, i);
                    if (b != expected[i])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    FormattedOffset = offset;
                    GlobalFormattedOffset = offset;
                    return;
                }
            }

            throw new IrbisException("formatted not found");
        }

        /// <summary>
        /// Find the record in the space.
        /// </summary>
        public void FindTheRecord
            (
                IntPtr space,
                int mfn,
                int blockSize
            )
        {
            if (RecordOffset != 0)
            {
                return;
            }

            if (GlobalRecordOffset != 0)
            {
                RecordOffset = GlobalRecordOffset;
                return;
            }

            //int minAddress = space.ToInt32() - 0x100000;
            //int maxAddress = space.ToInt32() + 0x100000;

            for (int offset = 4; offset < blockSize; offset++)
            {
                IntPtr pointer = Marshal.ReadIntPtr
                    (
                        space,
                        offset
                    );
                int address = pointer.ToInt32();
                //if (address < minAddress || address > maxAddress)
                //{
                //    continue;
                //}

                if (Win32.IsBadReadPtr(address, 32000))
                {
                    continue;
                }

                int canBeMfn = Marshal.ReadInt32(pointer);
                if (canBeMfn != mfn)
                {
                    continue;
                }
                int mustBeZero = Marshal.ReadInt32(pointer, 8);
                if (mustBeZero != 0)
                {
                    continue;
                }
                mustBeZero = Marshal.ReadInt32(pointer, 12);
                if (mustBeZero != 0)
                {
                    continue;
                }

                RecordOffset = offset;
                GlobalRecordOffset = offset;
                return;
            } 

            throw new IrbisException("record not found");
        }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public static SpaceLayout Version2012()
        {
            SpaceLayout result = new SpaceLayout
            {
                MinVersion = "2012.1",
                MaxVersion = "2013.1",
                RecordOffset = 577
            };

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public static SpaceLayout Version2014()
        {
            SpaceLayout result = new SpaceLayout
            {
                MinVersion = "2014.1",
                MaxVersion = "2015.1",
                RecordOffset = 626
            };

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public static SpaceLayout Version2016()
        {
            SpaceLayout result = new SpaceLayout
            {
                MinVersion = "2016.1",
                MaxVersion = "2016.1",
                RecordOffset = 2034
            };

            return result;
        }

        #endregion
    }
}
