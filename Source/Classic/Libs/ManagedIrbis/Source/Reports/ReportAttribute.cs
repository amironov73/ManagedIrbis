// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportAttribute.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ReportAttribute
    {
        #region Constants

        /// <summary>
        /// Background color.
        /// </summary>
        public const string BackColor = "BackColor";

        /// <summary>
        /// Black color.
        /// </summary>
        public const string Black = "Black";

        /// <summary>
        /// Blue color.
        /// </summary>
        public const string Blue = "Blue";

        /// <summary>
        /// Bold.
        /// </summary>
        public const string Bold = "Bold";

        /// <summary>
        /// Borders.
        /// </summary>
        public const string Borders = "Borders";

        /// <summary>
        /// Center.
        /// </summary>
        public const string Center = "Center";

        /// <summary>
        /// Column offset.
        /// </summary>
        public const string Column = "Column";

        /// <summary>
        /// Font name (family).
        /// </summary>
        public const string FontName = "FontName";

        /// <summary>
        /// Font size.
        /// </summary>
        public const string FontSize = "FontSize";

        /// <summary>
        /// Foreground color.
        /// </summary>
        public const string ForeColor = "ForeColor";

        /// <summary>
        /// Gray color.
        /// </summary>
        public const string Gray = "Gray";

        /// <summary>
        /// Green color.
        /// </summary>
        public const string Green = "Green";

        /// <summary>
        /// Height.
        /// </summary>
        public const string Height = "Height";

        /// <summary>
        /// Horizontal alignment.
        /// </summary>
        public const string HorizontalAlign = "HorizontalAlign";

        /// <summary>
        /// Italic.
        /// </summary>
        public const string Italic = "Italic";

        /// <summary>
        /// Left justify.
        /// </summary>
        public const string Left = "Left";

        /// <summary>
        /// Cell is a number.
        /// </summary>
        public const string Number = "Number";

        /// <summary>
        /// Red color.
        /// </summary>
        public const string Red = "Red";

        /// <summary>
        /// Row offset.
        /// </summary>
        public const string Row = "Row";

        /// <summary>
        /// Right justify.
        /// </summary>
        public const string Right = "Right";

        /// <summary>
        /// Cell merge.
        /// </summary>
        public const string Span = "Span";

        /// <summary>
        /// Underline.
        /// </summary>
        public const string Underline = "Underline";

        /// <summary>
        /// Vertical alignment.
        /// </summary>
        public const string VerticalAlign = "VerticalAlign";

        /// <summary>
        /// Width.
        /// </summary>
        public const string Width = "Width";

        /// <summary>
        /// Text wrapping.
        /// </summary>
        public const string WrapText = "WrapText";

        #endregion
    }
}
