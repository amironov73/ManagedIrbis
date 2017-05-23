// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReflectionUtility.cs -- 
 *  Ars Magna project, http://arsmagna.ru
 *  ------------------------------------------------------
 *  Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#if WIN81

using MvvmCross.Platform;

#endif

#endregion

namespace AM.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ReflectionUtility
    {
        #region Private members

#if FW45

        private const MethodImplOptions Aggressive
            = MethodImplOptions.AggressiveInlining;

#else

        private const MethodImplOptions Aggressive
            = (MethodImplOptions)0;

#endif

        #endregion

        #region Public methods

        ///// <summary>
        ///// Получение списка всех типов, загруженных на данный момент
        ///// в текущий домен.
        ///// </summary>
        ///// <returns></returns>
        ///// <remarks>Осторожно: могут быть загружены сборки только 
        ///// для рефлексии. Типы в них непригодны для использования.
        ///// </remarks>
        //public static Type[] GetAllTypes()
        //{
        //    Assembly[] assemblies = AppDomain.CurrentDomain
        //        .GetAssemblies();
        //    List<Type> result = new List<Type>();
        //    foreach (Assembly assembly in assemblies)
        //    {
        //        result.AddRange(assembly.GetTypes());
        //    }

        //    return result.ToArray();
        //}

        /// <summary>
        /// Bridge for NETCORE and UAP.
        /// </summary>
#if NETCORE || UAP || PORTABLE || WIN81

        [MethodImpl(Aggressive)]
        public static TypeInfo Bridge
            (
                [NotNull] this Type type
            )
        {
            Code.NotNull(type, "type");

            return type.GetTypeInfo();
        }

#else

        [MethodImpl(Aggressive)]
        public static Type Bridge
            (
                [NotNull] this Type type
            )
        {
            Code.NotNull(type, "type");

            return type;
        }

#endif

        /// <summary>
        /// Get the custom attribute.
        /// </summary>
        public static T GetCustomAttribute<T>
            (
                [NotNull] Type classType
            )
            where T : Attribute
        {
            Code.NotNull(classType, "classType");

#if PORTABLE

            throw new NotSupportedException();

#else

            var all = classType.Bridge().GetCustomAttributes
                (
                    typeof(T),
                    false
                );
            
            return (T)all.FirstOrDefault();

#endif
        }

        /// <summary>
        /// Get the custom attribute.
        /// </summary>
        [CanBeNull]
        public static T GetCustomAttribute<T>
            (
                [NotNull] Type classType,
                bool inherit
            )
            where T : Attribute
        {
            Code.NotNull(classType, "classType");

#if PORTABLE || WIN81

            throw new NotSupportedException();

#else

            var all = classType.Bridge().GetCustomAttributes
                (
                    typeof(T),
                    inherit
                );

            return (T) all.FirstOrDefault();

#endif
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        [CanBeNull]
        public static T GetCustomAttribute<T>
            (
                [NotNull] FieldInfo fieldInfo
            )
            where T : Attribute
        {
            Code.NotNull(fieldInfo, "fieldInfo");

            var all = fieldInfo.GetCustomAttributes
                (
                    typeof(T),
                    false
                );

            return (T)all.FirstOrDefault();
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        [CanBeNull]
        public static T GetCustomAttribute<T>
            (
                [NotNull] PropertyInfo propertyInfo
            )
            where T : Attribute
        {
            Code.NotNull(propertyInfo, "propertyInfo");

            var all = propertyInfo.GetCustomAttributes
                (
                    typeof(T),
                    false
                );

            return (T) all.FirstOrDefault();
        }

        ///// <summary>
        ///// Gets the custom attribute.
        ///// </summary>
        //[CanBeNull]
        //public static T GetCustomAttribute<T>
        //    (
        //        [NotNull] PropertyDescriptor propertyDescriptor
        //    )
        //    where T : Attribute
        //{
        //    return (T)propertyDescriptor.Attributes[typeof(T)];
        //}

        ///// <summary>
        ///// Get default constructor for given type.
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public static ConstructorInfo GetDefaultConstructor
        //    (
        //        [NotNull] Type type
        //    )
        //{
        //    ConstructorInfo result = type.GetConstructor
        //        (
        //            BindingFlags.Instance | BindingFlags.Public,
        //            null,
        //            Type.EmptyTypes,
        //            null
        //        );
        //    return result;
        //}

        /// <summary>
        /// Get field value either public or private.
        /// </summary>
        public static object GetFieldValue<T>
            (
                T target, 
                [NotNull] string fieldName
            )
        {
            Code.NotNullNorEmpty(fieldName, "fieldName");

#if PORTABLE || WIN81

            throw new NotSupportedException();

#else

            FieldInfo fieldInfo = typeof(T).GetField
                (
                    fieldName,
                    BindingFlags.Public | BindingFlags.NonPublic
                    | BindingFlags.Instance | BindingFlags.Static
                );
            if (ReferenceEquals(fieldInfo, null))
            {
                Log.Error
                    (
                        "ReflectionUtility::GeFieldValue: "
                        + "can't find field="
                        + fieldName
                    );

                throw new ArgumentException("fieldName");
            }

            return fieldInfo.GetValue(target);

#endif
        }

        /// <summary>
        /// Determines whether the specified type has the attribute.
        /// </summary>
        public static bool HasAttribute<T>
            (
                Type type,
                bool inherit
            )
            where T : Attribute
        {
            Code.NotNull(type, "type");

            return !ReferenceEquals
                (
                    GetCustomAttribute<T>(type, inherit),
                    null
                );
        }

        /// <summary>
        /// Set field value either public or private.
        /// </summary>
        public static void SetFieldValue<TTarget, TValue>
            (
                TTarget target, 
                [NotNull] string fieldName, 
                TValue value
            )
            where TTarget : class
        {
            Code.NotNullNorEmpty(fieldName, "fieldName");

#if PORTABLE

            throw new NotSupportedException();

#else

            FieldInfo fieldInfo = typeof(TTarget).GetField
                (
                    fieldName,
#if WIN81

                    BindingFlags.Public
                    | BindingFlags.Instance | BindingFlags.Static

#else

                    BindingFlags.Public | BindingFlags.NonPublic
                    | BindingFlags.Instance | BindingFlags.Static

#endif
                );
            if (ReferenceEquals(fieldInfo, null))
            {
                Log.Error
                    (
                        "ReflectionUtility::SetFieldValue: "
                        + "can't find field="
                        + fieldName
                    );

                throw new ArgumentException("fieldName");
            }

            fieldInfo.SetValue(target, value);

#endif
        }

        /// <summary>
        /// Get property value either public or private.
        /// </summary>
        public static object GetPropertyValue<T>
            (
                T target, 
                [NotNull] string propertyName
            )
        {
            Code.NotNullNorEmpty(propertyName, "propertyName");

#if PORTABLE

            throw new NotSupportedException();

#else

            PropertyInfo propertyInfo = typeof(T).GetProperty
                (
                    propertyName,

#if WIN81

                    BindingFlags.Public
                    | BindingFlags.Instance | BindingFlags.Static

#else

                    BindingFlags.Public | BindingFlags.NonPublic
                    | BindingFlags.Instance | BindingFlags.Static

#endif
                );
            if (ReferenceEquals(propertyInfo, null))
            {
                Log.Error
                    (
                        "ReflectionUtility::GetPropertyValue: "
                        + "can't find property="
                        + propertyName
                    );

                throw new ArgumentException("propertyName");
            }

            return propertyInfo.GetValue(target, null);

#endif
        }

#if !PORTABLE

        /// <summary>
        /// Gets the properties and fields.
        /// </summary>
        public static PropertyOrField[] GetPropertiesAndFields
            (
                Type type,
                BindingFlags bindingFlags
            )
        {
            Code.NotNull(type, "type");

            List<PropertyOrField> result = new List<PropertyOrField>();
            foreach (PropertyInfo property in type.GetProperties(bindingFlags))
            {
                result.Add(new PropertyOrField(property));
            }
            foreach (FieldInfo field in type.GetFields(bindingFlags))
            {
                result.Add(new PropertyOrField(field));
            }

            return result.ToArray();
        }

#endif

        /// <summary>
        /// Set property value either public or private.
        /// </summary>
        public static void SetPropertyValue<TTarget, TValue>
            (
                TTarget target, 
                [NotNull] string propertyName, 
                TValue value
            )
        {
            Code.NotNullNorEmpty(propertyName, "propertyName");

#if PORTABLE

            throw new NotSupportedException();

#else

            PropertyInfo propertyInfo = typeof(TTarget).GetProperty
                (
                    propertyName,

#if WIN81

                    BindingFlags.Public
                    | BindingFlags.Instance | BindingFlags.Static

#else

                    BindingFlags.Public | BindingFlags.NonPublic
                    | BindingFlags.Instance | BindingFlags.Static

#endif
                );
            if (ReferenceEquals(propertyInfo, null))
            {
                Log.Error
                    (
                        "ReflectionUtility::SetPropertyValue: "
                        + "can't find property="
                        + propertyName
                    );

                throw new ArgumentException("propertyName");
            }

            propertyInfo.SetValue(target, value, null);

#endif
        }

        #endregion
    }
}

