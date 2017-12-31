// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TaggedClassCollector.cs -- collects tagged classes in given assemblies 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Reflection;

using JetBrains.Annotations;

#endregion

namespace AM.Reflection
{
    /// <summary>
    /// Collects tagged classes in given assemblies.
    /// </summary>
    [PublicAPI]
    public static class TaggedClassesCollector
    {
        #region Public methods

        /// <summary>
        /// Collect tagged class in given assembly.
        /// </summary>
        /// <param name="asm">Assembly to scan.</param>
        /// <param name="tagName">Tag.</param>
        /// <returns>Found classes.</returns>
        [NotNull]
        [ItemNotNull]
        public static Type[] Collect
            (
                [NotNull] Assembly asm,
                [NotNull] string tagName
            )
        {
            List<Type> list = new List<Type>();

            foreach (Type type in asm.GetTypes())
            {
                var attributes = type.Bridge().GetCustomAttributes
                    (
                        typeof(TaggedClassAttribute),
                        true
                    );

                foreach (TaggedClassAttribute attribute in attributes)
                {
                    if (attribute.Tag == tagName)
                    {
                        list.Add(type);
                    }
                }
            }

            return list.ToArray();
        }

        ///// <summary>
        ///// Collect tagged classes in all assemblies.
        ///// </summary>
        ///// <param name="tagName">Tag.</param>
        ///// <returns>Found classes.</returns>
        //[NotNull]
        //[ItemNotNull]
        //public static Type[] Collect
        //    (
        //        [NotNull] string tagName
        //    )
        //{
        //    List<Type> list = new List<Type>();
        //    Assembly[] assemblies = AppDomain.CurrentDomain
        //        .GetAssemblies();

        //    foreach (Assembly asm in assemblies)
        //    {
        //        Type[] types = Collect(asm, tagName);
        //        list.AddRange(types);
        //    }

        //    return list.ToArray();
        //}

        #endregion
    }
}
