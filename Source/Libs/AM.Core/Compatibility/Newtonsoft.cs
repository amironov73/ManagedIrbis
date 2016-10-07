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

namespace Newtonsoft.Json
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class JsonPropertyAttribute
        : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public JsonPropertyAttribute()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public JsonPropertyAttribute(string text)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class JsonIgnoreAttribute
        : Attribute
    {
        
    }

    namespace Linq
    {
        /// <summary>
        /// 
        /// </summary>
        public class DummyClass
        {
        }
    }
}

#endif
