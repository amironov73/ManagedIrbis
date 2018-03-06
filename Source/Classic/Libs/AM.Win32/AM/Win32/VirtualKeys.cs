// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* VirtualKeys.cs -- key numbers as used by several system calls. 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Key numbers as used by several system calls.
	/// </summary>
	public enum VirtualKeys
	{
		/// <summary>
		/// Left mouse button.
		/// </summary>
		VK_LBUTTON = 0x01,

		/// <summary>
		/// Cancel button?
		/// </summary>
		VK_CANCEL = 0x03,

		/// <summary>
		/// Backspace.
		/// </summary>
		VK_BACK = 0x08,

		/// <summary>
		/// Tab.
		/// </summary>
		VK_TAB = 0x09,

		/// <summary>
		/// Form feed.
		/// </summary>
		VK_CLEAR = 0x0C,

		/// <summary>
		/// Enter.
		/// </summary>
		VK_RETURN = 0x0D,

		/// <summary>
		/// Shift key.
		/// </summary>
		VK_SHIFT = 0x10,

		/// <summary>
		/// Control key.
		/// </summary>
		VK_CONTROL = 0x11,

		/// <summary>
		/// Menu key.
		/// </summary>
		VK_MENU = 0x12,

		/// <summary>
		/// Caps lock.
		/// </summary>
		VK_CAPITAL = 0x14,

		/// <summary>
		/// Escape key.
		/// </summary>
		VK_ESCAPE = 0x1B,

		/// <summary>
		/// Space bar.
		/// </summary>
		VK_SPACE = 0x20,

		/// <summary>
		/// Page up.
		/// </summary>
		VK_PRIOR = 0x21,

		/// <summary>
		/// Page Down.
		/// </summary>
		VK_NEXT = 0x22,

		/// <summary>
		/// End.
		/// </summary>
		VK_END = 0x23,

		/// <summary>
		/// Home.
		/// </summary>
		VK_HOME = 0x24,

		/// <summary>
		/// Cursor left.
		/// </summary>
		VK_LEFT = 0x25,

		/// <summary>
		/// Cursor up.
		/// </summary>
		VK_UP = 0x26,

		/// <summary>
		/// Cursor right.
		/// </summary>
		VK_RIGHT = 0x27,

		/// <summary>
		/// Cursor down.
		/// </summary>
		VK_DOWN = 0x28,

		/// <summary>
		/// Select key.
		/// </summary>
		VK_SELECT = 0x29,

		/// <summary>
		/// Execute key.
		/// </summary>
		VK_EXECUTE = 0x2B,

		/// <summary>
		/// Print screen key.
		/// </summary>
		VK_SNAPSHOT = 0x2C,

		/// <summary>
		/// Help key
		/// </summary>
		VK_HELP = 0x2F,

		/// <summary>
		/// 0.
		/// </summary>
		VK_0 = 0x30,

		/// <summary>
		/// 1.
		/// </summary>
		VK_1 = 0x31,

		/// <summary>
		/// 2.
		/// </summary>
		VK_2 = 0x32,

		/// <summary>
		/// 3.
		/// </summary>
		VK_3 = 0x33,

		/// <summary>
		/// 4.
		/// </summary>
		VK_4 = 0x34,

		/// <summary>
		/// 5.
		/// </summary>
		VK_5 = 0x35,

		/// <summary>
		/// 6.
		/// </summary>
		VK_6 = 0x36,

		/// <summary>
		/// 7.
		/// </summary>
		VK_7 = 0x37,

		/// <summary>
		/// 8.
		/// </summary>
		VK_8 = 0x38,

		/// <summary>
		/// 9.</summary>
		VK_9 = 0x39,

		/// <summary>
		/// A.
		/// </summary>
		VK_A = 0x41,

		/// <summary>
		/// B.
		/// </summary>
		VK_B = 0x42,

		/// <summary>
		/// C.
		/// </summary>
		VK_C = 0x43,

		/// <summary>
		/// D.
		/// </summary>
		VK_D = 0x44,

		/// <summary>
		/// E.
		/// </summary>
		VK_E = 0x45,

		/// <summary>
		/// F.
		/// </summary>
		VK_F = 0x46,

		/// <summary>
		/// G.
		/// </summary>
		VK_G = 0x47,

		/// <summary>
		/// H.
		/// </summary>
		VK_H = 0x48,

		/// <summary>
		/// I.
		/// </summary>
		VK_I = 0x49,

		/// <summary>
		/// J.
		/// </summary>
		VK_J = 0x4A,

		/// <summary>
		/// K.
		/// </summary>
		VK_K = 0x4B,

		/// <summary>
		/// L.
		/// </summary>
		VK_L = 0x4C,

		/// <summary>
		/// M.
		/// </summary>
		VK_M = 0x4D,

		/// <summary>
		/// N.
		/// </summary>
		VK_N = 0x4E,

		/// <summary>
		/// O.
		/// </summary>
		VK_O = 0x4F,

		/// <summary>
		/// P.
		/// </summary>
		VK_P = 0x50,

		/// <summary>
		/// Q.
		/// </summary>
		VK_Q = 0x51,

		/// <summary>
		/// R.
		/// </summary>
		VK_R = 0x52,

		/// <summary>
		/// S.
		/// </summary>
		VK_S = 0x53,

		/// <summary>
		/// T.
		/// </summary>
		VK_T = 0x54,

		/// <summary>
		/// U.
		/// </summary>
		VK_U = 0x55,

		/// <summary>
		/// V.
		/// </summary>
		VK_V = 0x56,

		/// <summary>
		/// W.
		/// </summary>
		VK_W = 0x57,

		/// <summary>
		/// X.
		/// </summary>
		VK_X = 0x58,

		/// <summary>
		/// Y.
		/// </summary>
		VK_Y = 0x59,

		/// <summary>
		/// Z.
		/// </summary>
		VK_Z = 0x5A,

		/// <summary>
		/// Numeric keypad 0.
		/// </summary>
		VK_NUMPAD0 = 0x60,

		/// <summary>
		/// Numeric keypad 1.
		/// </summary>
		VK_NUMPAD1 = 0x61,

		/// <summary>
		/// Numeric keypad 2.
		/// </summary>
		VK_NUMPAD2 = 0x62,

		/// <summary>
		/// Numeric keypad 3.
		/// </summary>
		VK_NUMPAD3 = 0x63,

		/// <summary>
		/// Numeric keypad 4.
		/// </summary>
		VK_NUMPAD4 = 0x64,

		/// <summary>
		/// Numeric keypad 5.
		/// </summary>
		VK_NUMPAD5 = 0x65,

		/// <summary>
		/// Numeric keypad 6.
		/// </summary>
		VK_NUMPAD6 = 0x66,

		/// <summary>
		/// Numeric keypad 7.
		/// </summary>
		VK_NUMPAD7 = 0x67,

		/// <summary>
		/// Numeric keypad 8.
		/// </summary>
		VK_NUMPAD8 = 0x68,

		/// <summary>
		/// Numeric keypad 9.
		/// </summary>
		VK_NUMPAD9 = 0x69,

		/// <summary>
		/// Numeric keypad *.
		/// </summary>
		VK_MULTIPLY = 0x6A,

		/// <summary>
		/// Numeric keypad +.
		/// </summary>
		VK_ADD = 0x6B,
		
		/// <summary>
		/// Separator key.
		/// </summary>
		VK_SEPARATOR = 0x6C,

		/// <summary>
		/// Numeric keypad -.
		/// </summary>
		VK_SUBTRACT = 0x6D,

		/// <summary>
		/// Numeric keypad ..
		/// </summary>
		VK_DECIMAL = 0x6E,

		/// <summary>
		/// Numeric keypad /.
		/// </summary>
		VK_DIVIDE = 0x6F,

		/// <summary>
		/// Attn key.
		/// </summary>
		VK_ATTN = 0xF6,

		/// <summary>
		/// CrSel key.
		/// </summary>
		VK_CRSEL = 0xF7,

		/// <summary>
		/// ExSel key.
		/// </summary>
		VK_EXSEL = 0xF8,

		/// <summary>
		/// Erase EOF key.
		/// </summary>
		VK_EREOF = 0xF9,

		/// <summary>
		/// Play key.
		/// </summary>
		VK_PLAY = 0xFA,

		/// <summary>
		/// Zoom key.
		/// </summary>
		VK_ZOOM = 0xFB,
		
		/// <summary>
		/// Reserved.
		/// </summary>
		VK_NONAME = 0xFC,
		
		/// <summary>
		/// PA1 key.
		/// </summary>
		VK_PA1 = 0xFD,
		
		/// <summary>
		/// Clear key.
		/// </summary>
		VK_OEM_CLEAR = 0xFE,
		
		/// <summary>
		/// Left Windows key.
		/// </summary>
		VK_LWIN = 0x5B,
		
		/// <summary>
		/// Right Windows key.
		/// </summary>
		VK_RWIN = 0x5C,
		
		/// <summary>
		/// Applications key.
		/// </summary>
		VK_APPS = 0x5D,
		
		/// <summary>
		/// Left shift key.
		/// </summary>
		VK_LSHIFT = 0xA0,
		
		/// <summary>
		/// Right shift key.
		/// </summary>
		VK_RSHIFT = 0xA1,
		
		/// <summary>
		/// Left control key.
		/// </summary>
		VK_LCONTROL = 0xA2,
		
		/// <summary>
		/// Right control key.
		/// </summary>
		VK_RCONTROL = 0xA3,

		/// <summary>
		/// Left menu key.
		/// </summary>
		VK_LMENU = 0xA4,

		/// <summary>
		/// Right menu key.
		/// </summary>
		VK_RMENU = 0xA5
	}
}
