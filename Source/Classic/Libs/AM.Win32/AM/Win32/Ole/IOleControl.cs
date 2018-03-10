// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IOleControl.cs -- provides the features for supporting keyboard mnemonics, ambient properties, and events in control objects
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Provides the features for supporting keyboard mnemonics,
    /// ambient properties, and events in control objects.
    /// </summary>
    [PublicAPI]
    [ComImport]
    [Guid("B196B288-BAB4-101A-B69C-00AA00341D07")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleControl
    {
        /// <summary>
        /// Retrieves information about the control's keyboard mnemonics and behavior.
        /// </summary>
        [PreserveSig]
        int GetControlInfo([Out] object pCI);

        /// <summary>
        /// Informs a control that the user has pressed a keystroke that represents a keyboard mneumonic.
        /// </summary>
        [PreserveSig]
        int OnMnemonic([In] ref MSG pMsg);

        /// <summary>
        /// Informs a control that one or more of the container's ambient properties has changed.
        /// </summary>
        [PreserveSig]
        int OnAmbientPropertyChange(int dispID);

        /// <summary>
        /// Indicates whether the container is ignoring or accepting events from the control.
        /// </summary>
        [PreserveSig]
        int FreezeEvents(int bFreeze);
    }
}