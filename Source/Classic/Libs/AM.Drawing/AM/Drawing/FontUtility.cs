// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FontUtility.cs -- font manipulation helpers
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Drawing;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Drawing
{
    /// <summary>
    /// <see cref="Font"/> manipulation helper methods.
    /// </summary>
    [PublicAPI]
    public static class FontUtility
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Converts <see cref="Font"/> to a <see cref="String"/>
        /// (e. g. for XML serialization).
        /// </summary>
        [NotNull]
        public static string FontToString
            (
                [NotNull] Font font
            )
        {
            Code.NotNull(font, "font");

            string result = string.Format
                (
                    "{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                    font.Name,
                    font.Size,
                    font.Unit,
                    font.Bold,
                    font.Italic,
                    font.Underline,
                    font.Strikeout
                );
            return result;
        }

        /// <summary>
        /// Restores <see cref="Font"/> from a <see cref="String"/>
        /// (e. g. during XML deserialization).
        /// </summary>
        [NotNull]
        public static Font StringToFont
            (
                [NotNull] string fontSpecification
            )
        {
            Code.NotNullNorEmpty(fontSpecification, "fontSpecification");

            string[] parts = fontSpecification.Split('|');
            if (parts.Length != 7)
            {
                throw new ArgumentException("fontSpecification");
            }
            string fontName = parts[0];
            float emSize = float.Parse(parts[1]);
            GraphicsUnit unit
                = (GraphicsUnit)Enum.Parse(typeof(GraphicsUnit), parts[2]);
            bool bold = bool.Parse(parts[3]);
            bool italic = bool.Parse(parts[4]);
            bool underline = bool.Parse(parts[5]);
            bool strikeout = bool.Parse(parts[6]);
            FontStyle style = FontStyle.Regular;
            if (bold)
            {
                style |= FontStyle.Bold;
            }
            if (italic)
            {
                style |= FontStyle.Italic;
            }
            if (underline)
            {
                style |= FontStyle.Underline;
            }
            if (strikeout)
            {
                style |= FontStyle.Strikeout;
            }
            Font result = new Font
                (
                    fontName,
                    emSize,
                    style,
                    unit
                );

            return result;
        }

        #endregion
    }
}
