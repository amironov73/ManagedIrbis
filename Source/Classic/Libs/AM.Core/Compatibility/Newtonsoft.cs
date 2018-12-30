// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Newtonsoft.cs --
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
    [FlagsAttribute]
    public enum DefaultValueHandling
    {
        Include = 0,
        Ignore = 1,
        Populate = 2,
        IgnoreAndPopulate = 3
    }

    public enum NullValueHandling
    {
        Include = 0,
        Ignore = 1
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class JsonPropertyAttribute
        : Attribute
    {
        public DefaultValueHandling DefaultValueHandling { get; set; }

        public NullValueHandling NullValueHandling { get; set; }

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
        public class JToken
        {
            public T Value<T>() { return default(T); }
            public T[] Values<T>() { return new T[0]; }
            public bool HasValues { get { return false; } }
        }

        public class JValue: JToken
        {
        }

        public class JContainer: JToken {}

        public class JObject
           : JContainer
        {
           public JToken SelectToken(string path)
           {
               return null;
           }
        }

       public class JArray: JContainer {}

       public class JProperty: JContainer
       {
           public string Name;
           public JToken Value;
       }
    }
}

#endif
