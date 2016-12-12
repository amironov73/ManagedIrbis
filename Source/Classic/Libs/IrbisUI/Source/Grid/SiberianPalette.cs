// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SiberianPalette.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace IrbisUI.Grid
{
    /// <summary>
    /// Palette of UI colors.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SiberianPalette
    {
        #region Properties

        /// <summary>
        /// Background (non-selected) color.
        /// </summary>
        [JsonProperty("back-color")]
        public Color BackColor { get; set; }

        /// <summary>
        /// Background color for disabled elements.
        /// </summary>
        [JsonProperty("disabled-back-color")]
        public Color DisabledBackColor { get; set; }

        /// <summary>
        /// Foreground color for disabled elements.
        /// </summary>
        [JsonProperty("disabled-fore-color")]
        public Color DisabledForeColor { get; set; }

        /// <summary>
        /// Foreground (non-selected) color.
        /// </summary>
        [JsonProperty("fore-color")]
        public Color ForeColor { get; set; }

        /// <summary>
        /// Background color for header.
        /// </summary>
        [JsonProperty("header-back-color")]
        public Color HeaderBackColor { get; set; }

        /// <summary>
        /// Foreground color for header.
        /// </summary>
        [JsonProperty]
        public Color HeaderForeColor { get; set; }

        /// <summary>
        /// Line color.
        /// </summary>
        [JsonProperty("line-color")]
        public Color LineColor { get; set; }

        /// <summary>
        /// Name of the palette.
        /// </summary>
        [CanBeNull]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Selected background color.
        /// </summary>
        [JsonProperty("selected-back-color")]
        public Color SelectedBackColor { get; set; }

        /// <summary>
        /// Selected foreground color.
        /// </summary>
        [JsonProperty("selected-fore-color")]
        public Color SelectedForeColor { get; set; }

        /// <summary>
        /// Default palette.
        /// </summary>
        [NotNull]
        public static SiberianPalette DefaultPalette { get { return _defaultPalette; } }

        #endregion

        #region Construction

        static SiberianPalette()
        {
            _defaultPalette = new SiberianPalette
            {
                Name = "Default",
                
                BackColor = Color.White,
                ForeColor = Color.Black,

                HeaderBackColor = Color.LightGray,
                HeaderForeColor = Color.Black,

                LineColor = Color.Gray,

                DisabledBackColor = Color.White,
                DisabledForeColor = Color.DarkGray,

                SelectedBackColor = Color.Blue,
                SelectedForeColor = Color.White
            };
        }

        #endregion

        #region Private members

        private static readonly SiberianPalette _defaultPalette;

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the palette.
        /// </summary>
        [NotNull]
        public SiberianPalette Clone()
        {
            return (SiberianPalette) MemberwiseClone();
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return Name.ToVisibleString();
        }

        #endregion
    }
}
