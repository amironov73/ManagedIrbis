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
using System.Linq;
using System.Reflection;

using AM.Reflection;

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
        /// Get <see cref="Assembly"/> for the type.
        /// </summary>
        [NotNull]
        public static Assembly GetAssembly
            (
                [NotNull] Type type
            )
        {
            Code.NotNull(type, "type");

            return type.Bridge().Assembly;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Type GetBaseType
            (
                [NotNull] Type type
            )
        {
            Code.NotNull(type, "type");

            return type.Bridge().BaseType;
        }

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
        /// 
        /// </summary>
        public static FieldInfo[] GetFields
            (
                [NotNull] Type type
            )
        {
            Code.NotNull(type, "type");

#if PORTABLE || WIN81

            return type.GetRuntimeFields().ToArray();

#elif UAP

            FieldInfo[] result = TypeExtensions.GetFields
                (
                    type,
                    BindingFlags.Public
                    | BindingFlags.NonPublic 
                    | BindingFlags.Instance
                );

            return result;

#else

            FieldInfo[] result = type.Bridge().GetFields
                (
                    BindingFlags.Public
                    | BindingFlags.NonPublic 
                    | BindingFlags.Instance
                );

            return result;
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        public static Type GetInterface
            (
                [NotNull] Type type,
                [NotNull] string name
            )
        {
            Code.NotNull(type, "type");
            Code.NotNullNorEmpty(name, "name");

#if PORTABLE

            // TODO implement

            return null;

#elif SILVERLIGHT

            return type.Bridge().GetInterface(name, false);

#elif UAP || WIN81

            // TODO implement

            return null;

#else

            return type.Bridge().GetInterface(name);

#endif
        }

        /// <summary>
        /// Gets type of the argument.
        /// </summary>
        public static Type GetType<T>
            (
                T arg
            )
            where T : class
        {
            return ReferenceEquals(arg, null)
                    ? typeof(T)
                    : arg.GetType();
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsAbstract
            (
                [NotNull] Type type
            )
        {
            Code.NotNull(type, "type");

            return type.Bridge().IsAbstract;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsClass
            (
                [NotNull] Type type
            )
        {
            Code.NotNull(type, "type");

            return type.Bridge().IsClass;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsComObject
            (
                [NotNull] Type type
            )
        {
            Code.NotNull(type, "type");

#if PORTABLE || SILVERLIGHT || UAP || WIN81

            return false;

#else

            return type.Bridge().IsCOMObject;

#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsEnum
            (
                [NotNull] Type type
            )
        {
            Code.NotNull(type, "type");

            return type.Bridge().IsEnum;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsGenericType
            (
                [NotNull] Type type
            )
        {
            Code.NotNull(type, "type");

            return type.Bridge().IsGenericType;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsInterface
            (
                [NotNull] Type type
            )
        {
            Code.NotNull(type, "type");

            return type.Bridge().IsInterface;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsPrimitive
            (
                [NotNull] Type type
            )
        {
            Code.NotNull(type, "type");

            return type.Bridge().IsPrimitive;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsPublic
            (
                [NotNull] Type type
            )
        {
            Code.NotNull(type, "type");

            return type.Bridge().IsPublic;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsSealed
            (
                [NotNull] Type type
            )
        {
            Code.NotNull(type, "type");

            return type.Bridge().IsSealed;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsValueType
            (
                [NotNull] Type type
            )
        {
            Code.NotNull(type, "type");

            return type.Bridge().IsValueType;
        }

    }
}