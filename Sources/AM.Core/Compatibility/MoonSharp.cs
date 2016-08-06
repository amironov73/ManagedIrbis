/* MoonSharpUserData.cs -- temporary solution
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * State: poor
 */

#if NETCORE

#region Using directives

using System;

#endregion

namespace MoonSharp.Interpreter
{
    public class MoonSharpUserDataAttribute
        : Attribute
    {
    }

    public class Script
    {
    }

    public class DynValue
    {
        public static DynValue Nil { get { return new DynValue(); } }
    }
}

#endif
