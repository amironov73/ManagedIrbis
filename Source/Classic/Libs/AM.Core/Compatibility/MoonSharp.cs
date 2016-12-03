// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MoonSharpUserData.cs -- temporary solution
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * State: poor
 */

#if WINMOBILE || PocketPC

#region Using directives

using System;

#endregion

namespace MoonSharp.Interpreter
{
    /// <summary>
    /// 
    /// </summary>
    public class MoonSharpUserDataAttribute
        : Attribute
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class Script
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class DynValue
    {
        /// <summary>
        /// 
        /// </summary>
        public static DynValue Nil { get { return new DynValue(); } }
    }
}

#endif
