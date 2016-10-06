/* JetBrains2.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if WINMOBILE || PocketPC

using System;

#pragma warning disable 1591
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable IntroduceOptionalParameters.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable InconsistentNaming

namespace JetBrains.Annotations
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AssertionMethodAttribute
        : Attribute
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class ItemNotNullAttribute
        : Attribute
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class NoEnumerationAttribute
        : Attribute
    {
    }
}

#endif
