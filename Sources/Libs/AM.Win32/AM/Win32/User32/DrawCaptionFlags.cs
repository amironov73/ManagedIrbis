/* DrawCaptionFlags.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Specifies window caption drawing options.
	/// </summary>
	[Flags]
	public enum DrawCaptionFlags
	{
		/// <summary>
		/// The function uses the colors that denote an active caption.
		/// </summary>
		DC_ACTIVE = 0x0001,

		/// <summary>
		/// The function draws a small caption, using the current 
		/// small caption font.
		/// </summary>
		DC_SMALLCAP = 0x0002,

		/// <summary>
		/// The function draws the icon when drawing the caption text.
		/// </summary>
		DC_ICON = 0x0004,

		/// <summary>
		/// The function draws the caption text when drawing the caption.
		/// </summary>
		DC_TEXT = 0x0008,

		/// <summary>
		/// The function draws the caption as a button.
		/// </summary>
		DC_INBUTTON = 0x0010,

		/// <summary>
		/// <para>Windows 98/Me, Windows 2000/XP: When this flag is set, 
		/// the function uses COLOR_GRADIENTACTIVECAPTION (if the 
		/// DC_ACTIVE flag was set) or COLOR_GRADIENTINACTIVECAPTION 
		/// for the title-bar color.</para>
		/// <para>If this flag is not set, the function uses 
		/// COLOR_ACTIVECAPTION or COLOR_INACTIVECAPTION for both colors.
		/// </para>
		/// </summary>
		DC_GRADIENT = 0x0020,

		/// <summary>
		/// Windows XP: If set, the function draws the buttons in the 
		/// caption bar (to minimize, restore, or close an application).
		/// </summary>
		DC_BUTTONS = 0x1000
	}
}
