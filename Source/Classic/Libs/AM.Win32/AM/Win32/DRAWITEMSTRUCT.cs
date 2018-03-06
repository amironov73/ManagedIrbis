// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DRAWITEMSTRUCT.cs -- how to paint owner-drawn control or menu item
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Provides information that the owner window uses
    /// to determine how to paint an owner-drawn control
    /// or menu item. The owner window of the owner-drawn control
    /// or menu item receives a pointer to this structure
    /// as the lParam parameter of the WM_DRAWITEM message.
    /// </summary>
    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct DRAWITEMSTRUCT
    {
        /// <summary>
        /// The control type.
        /// </summary>
        public int CtlType;

        /// <summary>
        /// The identifier of the combo box, list box, button,
        /// or static control. This member is not used for a menu item.
        /// </summary>
        public int CtlID;

        /// <summary>
        /// The menu item identifier for a menu item or the index
        /// of the item in a list box or combo box. For an empty
        /// list box or combo box, this member can be -1.
        /// This allows the application to draw only the focus
        /// rectangle at the coordinates specified by the rcItem
        /// member even though there are no items in the control.
        /// This indicates to the user whether the list box
        /// or combo box has the focus. How the bits are set
        /// in the itemAction member determines whether
        /// the rectangle is to be drawn as though the list box
        /// or combo box has the focus.
        /// </summary>
        public int itemID;

        /// <summary>
        /// The required drawing action.
        /// </summary>
        public int itemAction;

        /// <summary>
        /// The visual state of the item after the current
        /// drawing action takes place.
        /// </summary>
        public int itemState;

        /// <summary>
        /// A handle to the control for combo boxes, list boxes,
        /// buttons, and static controls. For menus, this member
        /// is a handle to the menu that contains the item.
        /// </summary>
        public IntPtr hwndItem;

        /// <summary>
        /// A handle to a device context; this device context
        /// must be used when performing drawing operations
        /// on the control.
        /// </summary>
        public IntPtr hDC;

        /// <summary>
        /// A rectangle that defines the boundaries of the control
        /// to be drawn. This rectangle is in the device context
        /// specified by the hDC member. The system automatically
        /// clips anything that the owner window draws in the device
        /// context for combo boxes, list boxes, and buttons,
        /// but does not clip menu items. When drawing menu items,
        /// the owner window must not draw outside the boundaries
        /// of the rectangle defined by the rcItem member.
        /// </summary>
        public Rectangle rcItem;

        /// <summary>
        /// The application-defined value associated with the menu item.
        /// For a control, this parameter specifies the value last
        /// assigned to the list box or combo box by
        /// the LB_SETITEMDATA or CB_SETITEMDATA message. 
        /// </summary>
        public IntPtr itemData;
    }
}
