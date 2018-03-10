// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* REOBJECT.cs -- OLE or image object in a rich edit control
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Contains information about an OLE or image object in a rich edit control.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [StructLayout ( LayoutKind.Sequential )]
    public struct REOBJECT
    {
        #region Public members

        /// <summary>
        /// Size of structure.
        /// </summary>
        public int cbStruct;

        /// <summary>
        /// Character position of object.
        /// </summary>
        public int cp;

        /// <summary>
        /// Class ID of object.
        /// </summary>
        Guid clsid;

        /// <summary>
        /// OLE object interface.
        /// </summary>
        IntPtr poleobj;

        /// <summary>
        /// Associated storage interface.
        /// </summary>
        IntPtr pstg;

        /// <summary>
        /// Associated client site interface.
        /// </summary>
        IntPtr polesite;

        /// <summary>
        /// Size of object (may be 0,0).
        /// </summary>
        SIZEL sizel;

        /// <summary>
        /// Display aspect to use.
        /// </summary>
        public int dvaspect;

        /// <summary>
        /// Object status flags.
        /// </summary>
        public int dwFlags;

        /// <summary>
        /// Dword for user's use.
        /// </summary>
        public int dwUser;

        #endregion
    }
}
