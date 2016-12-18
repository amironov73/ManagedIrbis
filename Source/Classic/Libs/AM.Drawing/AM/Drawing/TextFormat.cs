// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextFormat.cs -- 
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Drawing;

using JetBrains.Annotations;

#endregion

namespace AM.Drawing
{
    /// <summary>
    /// Заранее подготовленные значения StringFormat.
    /// </summary>
    /// <remarks>Полученные значения не должны удаляться
    /// с помощью Dispose.</remarks>
    [PublicAPI]
    public sealed class TextFormat
    {
        #region Constants

        private const StringAlignment Near = StringAlignment.Near;

        private const StringAlignment Center = StringAlignment.Center;

        private const StringAlignment Far = StringAlignment.Far;

        #endregion

        #region Private members

        private static StringFormat _Format
            (
                StringAlignment stringAlignment,
                StringAlignment lineAlignment
            )
        {
            StringFormat result =
                (StringFormat)StringFormat.GenericTypographic.Clone();
            result.Alignment = stringAlignment;
            result.LineAlignment = lineAlignment;

            return result;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Alignment = Near, LineAlignment = Near.
        /// </summary>
        public static readonly StringFormat NearNear
            = _Format ( Near, Near );

        /// <summary>
        /// Alignment = Near, LineAlignment = Center.
        /// </summary>
        public static readonly StringFormat NearCenter
            = _Format ( Near, Center );

        /// <summary>
        /// Alignment = Near, LineAlignment = Far.
        /// </summary>
        public static readonly StringFormat NearFar
            = _Format ( Near, Far );

        /// <summary>
        /// Alignment = Center, LineAlignment = Near.
        /// </summary>
        public static readonly StringFormat CenterNear
            = _Format ( Center, Near );

        /// <summary>
        /// Alignment = Center, LineAlignment = Center.
        /// </summary>
        public static readonly StringFormat CenterCenter
            = _Format ( Center, Center );

        /// <summary>
        /// Alignment = Center, LineAlignment = Far.
        /// </summary>
        public static readonly StringFormat CenterFar
            = _Format ( Center, Far );

        /// <summary>
        /// Alignment = Far, LineAlignment = Near.
        /// </summary>
        public static readonly StringFormat FarNear
            = _Format ( Far, Near );

        /// <summary>
        /// Alignment = Far, LineAlignment = Center.
        /// </summary>
        public static readonly StringFormat FarCenter
            = _Format ( Far, Center );

        /// <summary>
        /// Alignment = Far, LineAlignment = Far.
        /// </summary>
        public static readonly StringFormat FarFar
            = _Format ( Far, Far );

        #endregion
    }
}
