// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SiberianMeasureEventArgs.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI.Grid
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SiberianMeasureEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Dimensions.
        /// </summary>
        [NotNull]
        public SiberianDimensions Dimensions { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dimensions"></param>
        public SiberianMeasureEventArgs
            (
                [NotNull] SiberianDimensions dimensions
            )
        {
            Code.NotNull(dimensions, "dimensions");

            Dimensions = dimensions;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return Dimensions.ToString();
        }

        #endregion
    }
}
