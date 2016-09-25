/* RainbowItem.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [ToolboxItem(false)]
    [System.ComponentModel.DesignerCategory("Code")]
    public class RainbowItem
        : Component
    {
        ///<summary>
        /// Color.
        ///</summary>
        public Color Color { get; set; }

        ///<summary>
        /// Position.
        ///</summary>
        public float Position { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RainbowItem()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RainbowItem
            (
                Color color,
                float position
            )
        {
            Color = color;
            Position = position;
        }

    }
}
