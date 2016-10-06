/* System.cs -- temporary solution for .NET Core compability
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if NETCORE || WINMOBILE || PocketPC

using System;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NonSerializedAttribute
        : Attribute
    {
    }

    //public sealed class ArgumentOutOfRangeException
    //    : Exception
    //{
    //    public ArgumentOutOfRangeException()
    //    {
    //    }

    //    public ArgumentOutOfRangeException(string message) : base(message)
    //    {
    //    }

    //    public ArgumentOutOfRangeException(string message, Exception innerException) : base(message, innerException)
    //    {
    //    }

    //    public ArgumentOutOfRangeException(string paramName, object actualValue, string message)
    //        : base(message)
    //    {
            
    //    }
    //}
}

#endif
