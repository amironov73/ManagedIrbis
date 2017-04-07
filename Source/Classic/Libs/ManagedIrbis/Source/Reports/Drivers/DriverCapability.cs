// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DriverCapability.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Capabilities of the <see cref="ReportDriver"/>.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum DriverCapability
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Arial, Times, Courier etc.
        /// </summary>
        FontFace = 0x0001,

        /// <summary>
        /// Font size.
        /// </summary>
        FontSize = 0x0002,
        
        /// <summary>
        /// Bold, Italic, Underline.
        /// </summary>
        FontDecoration = 0x0004,

        /// <summary>
        /// Red, Green, Blue.
        /// </summary>
        FontColor = 0x0008,

        /// <summary>
        /// Left, Right, Center, Justify.
        /// </summary>
        ParagraphAlignment = 0x0010,

        /// <summary>
        /// Pictures.
        /// </summary>
        Image = 0x0020,

        /// <summary>
        /// Table.
        /// </summary>
        Table = 0x0040,

        /// <summary>
        /// Section.
        /// </summary>
        Section = 0x0080,

        /// <summary>
        /// Cell span.
        /// </summary>
        CellSpan = 0x0100
    }
}
