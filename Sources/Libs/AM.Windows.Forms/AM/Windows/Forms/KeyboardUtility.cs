/* KeyboardUtility.cs--
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class KeyboardUtility
    {

#if NOTDEF

        /// <summary>
        /// The ToUnicodeEx function translates the specified 
        /// virtual-key code and keyboard state to the corresponding 
        /// Unicode character or characters.
        /// </summary>
        /// <param name="wVirtKey">Specifies the virtual-key code 
        /// to be translated.</param>
        /// <param name="wScanCode">Specifies the hardware scan 
        /// code of the key to be translated. The high-order bit 
        /// of this value is set if the key is up.</param>
        /// <param name="lpKeyState">Pointer to a 256-byte array 
        /// that contains the current keyboard state. Each element 
        /// (byte) in the array contains the state of one key. 
        /// If the high-order bit of a byte is set, the key is down.
        /// </param>
        /// <param name="pwszBuff">Pointer to the buffer that 
        /// receives the translated Unicode character or characters. 
        /// However, this buffer may be returned without being 
        /// null-terminated even though the variable name suggests 
        /// that it is null-terminated.</param>
        /// <param name="cchBuff">Specifies the size, in wide 
        /// characters, of the buffer pointed to by the pwszBuff 
        /// parameter.</param>
        /// <param name="wFlags">Specifies the behavior of the 
        /// function. If bit 0 is set, a menu is active. Bits 
        /// 1 through 31 are reserved.</param>
        /// <param name="dwhkl">Input locale identifier used to 
        /// translate the specified code. This parameter can be 
        /// any input locale identifier previously returned by 
        /// the LoadKeyboardLayout function.</param>
        /// <returns>The function returns one of the following
        ///  values.
        /// <list type="table">
        /// <item><term>-1</term><description>The specified virtual 
        /// key is a dead-key character
        ///  (accent or diacritic). This value is returned 
        /// regardless of the keyboard layout, even if several 
        /// characters have been typed and are stored in the 
        /// keyboard state. If possible, even with Unicode 
        /// keyboard layouts, the function has written a spacing 
        /// version of the dead-key character to the buffer 
        /// specified by pwszBuff. For example, the function 
        /// writes the character SPACING ACUTE (0x00B4), 
        /// rather than the character NON_SPACING ACUTE (0x0301).
        /// </description></item>
        /// <item><term>0</term><description>The specified virtual 
        /// key has no translation for 
        /// the current state of the keyboard. Nothing was 
        /// written to the buffer specified by pwszBuff.
        /// </description></item>
        /// <item><term>1</term><description>One character 
        /// was written to the buffer specified 
        /// by pwszBuff.</description></item>
        /// <item><term>2</term><description>or more Two 
        /// or more characters were written 
        /// to the buffer specified by pwszBuff. The most 
        /// common cause for this is that a dead-key character 
        /// (accent or diacritic) stored in the keyboard layout 
        /// could not be combined with the specified virtual key 
        /// to form a single character. However, the buffer may 
        /// contain more characters than the return value specifies. 
        /// When this happens, any extra characters are invalid 
        /// and should be ignored.
        /// </description></item>
        /// </list>
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int ToUnicodeEx
            (
                uint wVirtKey,
                uint wScanCode,
                byte[] lpKeyState,
                StringBuilder pwszBuff,
                int cchBuff,
                uint wFlags,
                IntPtr dwhkl
            );


        /// <summary>
        /// The GetKeyboardState function copies the status 
        /// of the 256 virtual keys to the specified buffer.
        /// </summary>
        /// <param name="lpKeyState">Pointer to the 256-byte 
        /// array that receives the status data for each 
        /// virtual key.</param>
        /// <returns>If the function succeeds, the return 
        /// value is nonzero. If the function fails, the 
        /// return value is zero.</returns>
        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState
            (
            byte[] lpKeyState
            );

        /// <summary>
        /// Translates virtual key to character if possile.
        /// </summary>
        /// <param name="virtualKey">The virtual key.</param>
        /// <param name="scanCode">The scan code.</param>
        /// <param name="keyboardState">State of the keyboard.
        /// </param>
        /// <returns>Translated character or <c>null</c>.
        /// </returns>
        public static char? KeyToChar
            (
                uint virtualKey,
                uint scanCode,
                byte[] keyboardState
            )
        {
            StringBuilder buffer = new StringBuilder(10);
            int ex = ToUnicodeEx
                (
                    virtualKey,
                    scanCode,
                    keyboardState,
                    buffer,
                    buffer.Capacity,
                    0,
                    Application.CurrentInputLanguage.Handle
                );
            if (ex > 0)
            {
                char firstChar = buffer[0];
                if (char.IsLetterOrDigit(firstChar)
                    || (char.IsWhiteSpace(firstChar) 
                        && (firstChar != '\t'))
                    || char.IsPunctuation(firstChar))
                {
                    return firstChar;
                }
            }
            return null;
        }

        /// <summary>
        /// Processes a key message and generates 
        /// the appropriate control events. 
        /// </summary>
        /// <param name="m">A Message, passed by reference, 
        /// that represents the window message to process.
        /// </param>
        /// <returns>Character code if the message was processed 
        /// by the control; otherwise, <c>null</c>.</returns>
        public static char? ProcessKeyEventArgs
            (
                ref Message m
            )
        {
            if (m.Msg == 0x100) // WM_KEYDOWN
            {
                byte[] keyboardState = new byte[256];
                bool result = GetKeyboardState(keyboardState);
                if (result)
                {
                    uint virtualKey = unchecked((uint)m.WParam.ToInt64());
                    uint scanCode = unchecked((uint)m.LParam.ToInt64());
                    char? letter = KeyToChar
                        (
                            virtualKey, 
                            scanCode, 
                            keyboardState
                        );
                    return letter;
                }
                //m.Result = IntPtr.Zero;
            }

            return null;
        }

#endif
    }
}
