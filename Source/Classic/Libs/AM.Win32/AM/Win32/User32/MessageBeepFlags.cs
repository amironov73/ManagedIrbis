// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MessageBeepFlags.cs --  
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Sound type, as identified by an entry in the registry.
    /// </summary>
    public enum MessageBeepFlags
    {
        /// <summary>
        /// Simple beep. If the sound card is not available, 
        /// the sound is generated using the speaker.
        /// </summary>
        SimpleBeep = unchecked ( (int) 0xFFFFFFFF ),

        /// <summary>
        /// SystemAsterisk.
        /// </summary>
        MB_ICONASTERISK = 0x40,

        /// <summary>
        /// SystemExclamation.
        /// </summary>
        MB_ICONEXCLAMATION = 0x30,

        /// <summary>
        /// SystemHand.
        /// </summary>
        MB_ICONHAND = 0x10,

        /// <summary>
        /// SystemQuestion.
        /// </summary>
        MB_ICONQUESTION = 0x20,

        /// <summary>
        /// SystemDefault.
        /// </summary>
        MB_OK = 0
    }
}
