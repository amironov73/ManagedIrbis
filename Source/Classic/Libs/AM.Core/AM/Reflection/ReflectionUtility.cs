// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReflectionUtility.cs -- 
 *  Ars Magna project, http://arsmagna.ru
 *  ------------------------------------------------------
 *  Status: poor
 */

#if !NETCORE

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

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
        /// 
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>
            (
                [NotNull] Type classType
            )
            where T : Attribute
        {
            Code.NotNull(classType, "classType");

            object[] all = classType.GetCustomAttributes
                (
                    typeof(T),
                    false
                );
            
            return (T)(all.Length == 0
                            ? null
                            : all[0]);
        }

        /// <summary>
        /// Gets the custom attribute.
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

            object[] all = classType.GetCustomAttributes
                (
                    typeof(T),
                    inherit
                );

            return (T) all.FirstOrDefault();
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns></returns>
        [CanBeNull]
        public static T GetCustomAttribute<T>
            (
                [NotNull] PropertyInfo propertyInfo
            )
            where T : Attribute
        {
            Code.NotNull(propertyInfo, "propertyInfo");

            object[] all = propertyInfo.GetCustomAttributes
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
        }

        /// <summary>
        /// Determines whether the specified type has attribute.
        /// </summary>
        public static bool HasAttribute<T>
            (
                Type type,
                bool inherit
            )
            where T : Attribute
        {
            Code.NotNull(type, "type");

            return GetCustomAttribute<T>(type, inherit) != null;
        }

        /// <summary>
        /// Set field value either public or private.
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="target"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetFieldValue<TTarget, TValue>
            (
                TTarget target, 
                [NotNull] string fieldName, 
                TValue value
            )
            where TTarget : class
        {
            Code.NotNullNorEmpty(fieldName, "fieldName");

            FieldInfo fieldInfo = typeof(TTarget).GetField
                (
                    fieldName,
                    BindingFlags.Public | BindingFlags.NonPublic
                    | BindingFlags.Instance | BindingFlags.Static
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
        }

        /// <summary>
        /// Get property value either public or private.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetPropertyValue<T>
            (
                T target, 
                [NotNull] string propertyName
            )
        {
            Code.NotNullNorEmpty(propertyName, "propertyName");

            PropertyInfo propertyInfo = typeof(T).GetProperty
                (
                    propertyName,
                    BindingFlags.Public | BindingFlags.NonPublic
                    | BindingFlags.Instance | BindingFlags.Static
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
        }

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

            PropertyInfo propertyInfo = typeof(TTarget).GetProperty
                (
                    propertyName,
                    BindingFlags.Public | BindingFlags.NonPublic
                    | BindingFlags.Instance | BindingFlags.Static
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
        }

        #endregion
    }
}

#endif