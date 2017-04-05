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
        /// Center.
        /// </summary>
        public const string Center = "Center";

        /// <summary>
        /// Font name (family).
        /// </summary>
        public const string FontName = "FontName";

        /// <summary>
        /// Font size.
        /// </summary>
        public const string FontSize = "FontSize";

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
        /// Italic.
        /// </summary>
        public const string Italic = "Italic";

        /// <summary>
        /// Left justify.
        /// </summary>
        public const string Left = "Left";

        /// <summary>
        /// Red color.
        /// </summary>
        public const string Red = "Red";

        /// <summary>
        /// Right justify.
        /// </summary>
        public const string Right = "Right";

        /// <summary>
        /// Width.
        /// </summary>
        public const string Width = "Width";

        #endregion
    }
}
