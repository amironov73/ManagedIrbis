// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* System.cs -- temporary solution for .NET Core compatibility
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if NETCORE || WINMOBILE || PocketPC || PORTABLE

using System;

namespace System
{
#if PocketPC || WINMOBILE

    /// <summary>
    /// Generic data converter.
    /// </summary>
    public delegate TOutput Converter<TInput, TOutput> (TInput input);

#else

    /// <summary>
    /// Generic data converter.
    /// </summary>
    public delegate TOutput Converter<in TInput, out TOutput> (TInput input);

#endif

    ///// <summary>
    ///// 
    ///// </summary>
    //public sealed class NonSerializedAttribute
    //    : Attribute
    //{
    //}

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
