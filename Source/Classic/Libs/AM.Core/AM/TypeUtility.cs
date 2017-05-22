// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TypeUtility.cs -- type related useful routines
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Reflection;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// Сборник полезных методов, работающих с информацией о типах.
    /// </summary>
    [PublicAPI]
    public static class TypeUtility
    {
        ///// <summary>
        ///// Получение списка всех типов-наследников указанного типа.
        ///// </summary>
        ///// <param name="parentType"></param>
        ///// <returns></returns>
        //public static Type[] GetAllDescendants 
        //    ( 
        //        [NotNull] Type parentType 
        //    )
        //{
        //    Code.NotNull(parentType, "parentType");

        //    Assembly[] assemblies 
        //        = AppDomain.CurrentDomain.GetAssemblies ();
        //    List <Type> result = new List <Type> ();
        //    foreach ( Assembly assembly in assemblies )
        //    {
        //        foreach ( Type type in assembly.GetTypes () )
        //        {
        //            if ( type.IsSubclassOf ( parentType ) )
        //            {
        //                result.Add ( type );
        //            }
        //        }
        //    }

        //    return result.ToArray ();
        //}

        /// <summary>
        /// Получение закрытого generic-типа, параметризованного
        /// указанными типами.
        /// </summary>
        public static Type GetGenericType
            (
                [NotNull] string genericTypeName,
                [NotNull] params string[] typeList
            )
        {
            Code.NotNullNorEmpty(genericTypeName, "genericTypeName");
            Code.NotNull(typeList, "typeList");

            // construct the mangled name
            string mangledName = genericTypeName + "`" + typeList.Length;

            // get the open generic type
            Type genericType = Type.GetType(mangledName, true);

            // construct the array of generic type parameters
            Type[] typeArgs = new Type[typeList.Length];
            for (int i = 0; i < typeList.Length; i++)
            {
                typeArgs[i] = Type.GetType(typeList[i]);
            }

            // get the closed generic type
            Type constructedType =
                genericType.MakeGenericType(typeArgs);

            return constructedType;
        }

        /// <summary>
        /// Gets type of the argument.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns></returns>
        public static Type GetType<T>(T arg)
            where T : class
        {
            return ReferenceEquals(arg, null)
                    ? typeof(T)
                    : arg.GetType();
        }
    }
}