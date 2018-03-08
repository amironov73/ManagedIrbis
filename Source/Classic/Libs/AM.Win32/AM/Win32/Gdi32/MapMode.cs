// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MapMode.cs -- specifies the GDI mapping mode.
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Specifies the GDI mapping mode.
    /// </summary>
    [PublicAPI]
    public enum MapMode
    {
        /// <summary>
        /// For FXCop.
        /// </summary>
        None = 0,

        /// <summary>
        /// Each logical unit is mapped to one device pixel. 
        /// Positive x is to the right; positive y is down.
        /// </summary>
        MM_TEXT = 1,

        /// <summary>
        /// Each logical unit is mapped to 0.1 millimeter. 
        /// Positive x is to the right; positive y is up.
        /// </summary>
        MM_LOMETRIC = 2,

        /// <summary>
        /// Each logical unit is mapped to 0.01 millimeter. 
        /// Positive x is to the right; positive y is up.
        /// </summary>
        MM_HIMETRIC = 3,

        /// <summary>
        /// Each logical unit is mapped to 0.01 inch. 
        /// Positive x is to the right; positive y is up.
        /// </summary>
        MM_LOENGLISH = 4,

        /// <summary>
        /// Each logical unit is mapped to 0.001 inch. 
        /// Positive x is to the right; positive y is up.
        /// </summary>
        MM_HIENGLISH = 5,

        /// <summary>
        /// Each logical unit is mapped to one twentieth of 
        /// a printer's point (1/1440 inch, also called a twip). 
        /// Positive x is to the right; positive y is up.
        /// </summary>
        MM_TWIPS = 6,

        /// <summary>
        /// Logical units are mapped to arbitrary units with equally 
        /// scaled axes; that is, one unit along the x-axis is equal 
        /// to one unit along the y-axis. Use the SetWindowExtEx and 
        /// SetViewportExtEx functions to specify the units and the 
        /// orientation of the axes. Graphics device interface (GDI) 
        /// makes adjustments as necessary to ensure the x and y units 
        /// remain the same size (When the window extent is set, the 
        /// viewport will be adjusted to keep the units isotropic).
        /// </summary>
        MM_ISOTROPIC = 7,

        /// <summary>
        /// Logical units are mapped to arbitrary units 
        /// with arbitrarily scaled axes. Use the SetWindowExtEx 
        /// and SetViewportExtEx functions to specify the units, 
        /// orientation, and scaling. 
        /// </summary>
        MM_ANISOTROPIC = 8,
    }
}
