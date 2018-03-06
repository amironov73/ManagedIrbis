// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ACCEL.cs -- defines an accelerator key used in an accelerator table
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Defines an accelerator key used in an accelerator table.
    /// </summary>
    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public class ACCEL
    {
        /// <summary>
        /// The accelerator behavior.
        /// </summary>
        public byte fVirt;

        /// <summary>
        /// The accelerator key.
        /// </summary>
        public short key;

        /// <summary>
        /// The accelerator identifier.
        /// </summary>
        public short cmd;
    }
}
