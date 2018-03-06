// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BorderFlags3D.cs -- specifies the type of inner and outer edges to draw
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Specifies the type of inner and outer edges to draw.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum BorderFlags3D
    {
        /// <summary>
        /// Raised outer edge.
        /// </summary>
        BDR_RAISEDOUTER = 0x0001,

        /// <summary>
        /// Sunken outer edge.
        /// </summary>
        BDR_SUNKENOUTER = 0x0002,

        /// <summary>
        /// Raised inner edge.
        /// </summary>
        BDR_RAISEDINNER = 0x0004,

        /// <summary>
        /// Sunken inner edge.
        /// </summary>
        BDR_SUNKENINNER = 0x0008,

        /// <summary>
        /// ???
        /// </summary>
        BDR_OUTER = (BDR_RAISEDOUTER | BDR_SUNKENOUTER),

        /// <summary>
        /// ???
        /// </summary>
        BDR_INNER = (BDR_RAISEDINNER | BDR_SUNKENINNER),

        /// <summary>
        /// ???
        /// </summary>
        BDR_RAISED = (BDR_RAISEDOUTER | BDR_RAISEDINNER),

        /// <summary>
        /// ???
        /// </summary>
        BDR_SUNKEN = (BDR_SUNKENOUTER | BDR_SUNKENINNER),

        /// <summary>
        /// Combination of BDR_RAISEDOUTER and BDR_RAISEDINNER.
        /// </summary>
        EDGE_RAISED = (BDR_RAISEDOUTER | BDR_RAISEDINNER),

        /// <summary>
        /// Combination of BDR_SUNKENOUTER and BDR_SUNKENINNER.
        /// </summary>
        EDGE_SUNKEN = (BDR_SUNKENOUTER | BDR_SUNKENINNER),

        /// <summary>
        /// Combination of BDR_SUNKENOUTER and BDR_RAISEDINNER.
        /// </summary>
        EDGE_ETCHED = (BDR_SUNKENOUTER | BDR_RAISEDINNER),

        /// <summary>
        /// Combination of BDR_RAISEDOUTER and BDR_SUNKENINNER.
        /// </summary>
        EDGE_BUMP = (BDR_RAISEDOUTER | BDR_SUNKENINNER)
    }
}
