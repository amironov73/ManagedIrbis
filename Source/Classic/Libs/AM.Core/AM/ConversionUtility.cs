// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConversionUtility.cs -- set of type conversion routines.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Reflection;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// Type conversion helpers.
    /// </summary>
    public static class ConversionUtility
    {
        #region Public methods

        /// <summary>
        /// Determines whether given value can be converted to
        /// the specified type.
        /// </summary>
        /// <param name="value">Value to be converted.</param>
        /// <returns>
        /// <c>true</c> if value can be converted;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool CanConvertTo<T>
            (
                [CanBeNull] object value
            )
        {
            if (value != null)
            {
                Type sourceType = value.GetType();
                Type targetType = typeof(T);

                if (targetType == sourceType)
                {
                    return true;
                }

#if !NETCORE && !WIN81 && !PORTABLE

                if (targetType.IsAssignableFrom(sourceType))
                {
                    return true;
                }

#endif

#if !WIN81 && !PORTABLE

                IConvertible convertible = value as IConvertible;
                if (convertible != null)
                {
                    return true; // ???
                }

#endif

                //TypeConverter converterFrom
                //    = TypeDescriptor.GetConverter(value);
                //if (converterFrom.CanConvertTo(targetType))
                //{
                //    return true;
                //}

                //TypeConverter converterTo
                //    = TypeDescriptor.GetConverter(targetType);
                //if (converterTo.CanConvertFrom(sourceType))
                //{
                //    return true;
                //}
            }
            return false;
        }

        /// <summary>
        /// Converts given value to the specified type.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <returns>Converted value.</returns>
        public static T ConvertTo<T>
            (
                [CanBeNull] object value
            )
        {
            if (value == null)
            {
                return default(T);
            }

            Type sourceType = value.GetType();
            Type targetType = typeof(T);

            if (targetType == typeof(string))
            {
                return (T)(object)value.ToString();
            }

#if !NETCORE && !WIN81 && !PORTABLE

            if (targetType.IsAssignableFrom(sourceType))
            {
                return (T)value;
            }

#endif

#if !WINMOBILE && !PocketPC && !SILVERLIGHT && !WIN81 && !PORTABLE

            IConvertible convertible = value as IConvertible;
            if (convertible != null)
            {
                return (T)Convert.ChangeType(value, targetType);
            }

#endif

            //TypeConverter converterFrom
            //    = TypeDescriptor.GetConverter(value);
            //if (converterFrom.CanConvertTo(targetType))
            //{
            //    return (T)converterFrom.ConvertTo
            //                (
            //                    value,
            //                    targetType
            //                );
            //}

            //TypeConverter converterTo
            //    = TypeDescriptor.GetConverter(targetType);
            //if (converterTo.CanConvertFrom(sourceType))
            //{
            //    return (T)converterTo.ConvertFrom(value);
            //}

            //foreach (MethodInfo miOpChangeType in
            //    sourceType.GetMethods(BindingFlags.Public | BindingFlags.Static))
            //{
            //    if (miOpChangeType.IsSpecialName
            //         && (miOpChangeType.Name == "op_Implicit"
            //              || miOpChangeType.Name == "op_Explicit"
            //            )
            //         && miOpChangeType.ReturnType.IsAssignableFrom(targetType)
            //        )
            //    {
            //        ParameterInfo[] psOpChangeType = miOpChangeType.GetParameters();
            //        if ((psOpChangeType.Length == 1)
            //             && (psOpChangeType[0].ParameterType == sourceType)
            //            )
            //        {
            //            return (T)miOpChangeType.Invoke(null, new [] { value });
            //        }
            //    }
            //}

            throw new ArsMagnaException();
        }

        /// <summary>
        /// Converts given object to boolean value.
        /// </summary>
        /// <param name="value">Object to be converted.</param>
        /// <returns>Converted value.</returns>
        /// <exception cref="FormatException">
        /// Value can't be converted.
        /// </exception>
        public static bool ToBoolean
            (
                [NotNull] object value
            )
        {
            Code.NotNull(value, "value");
            
            if (value is bool)
            {
                return (bool)value;
            }

            //if (value is bool?)
            //{
            //    return ((bool?)value).Value;
            //}

            bool result;

#if !PocketPC

            if (bool.TryParse(value as string, out result))
            {
                return result;
            }

#else
            try
            {
                result = bool.Parse(value as string);

                return result;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // Pass through
            }
#endif

            string svalue = value as string;
            if ((svalue == "false")
                 || (svalue == "0")
                 || (svalue == "no")
                 || (svalue == "n")
                 || (svalue == "off")
                 || (svalue == "negative")
                 || (svalue == "neg")
                 || (svalue == "disabled")
                 || (svalue == "incorrect")
                 || (svalue == "wrong")
                 || (svalue == "нет")
                )
            {
                return false;
            }
            if ((svalue == "true")
                 || (svalue == "1")
                 || (svalue == "yes")
                 || (svalue == "y")
                 || (svalue == "on")
                 || (svalue == "positiva")
                 || (svalue == "pos")
                 || (svalue == "enabled")
                 || (svalue == "correct")
                 || (svalue == "right")
                 || (svalue == "да")
                )
            {
                return true;
            }
            unchecked
            {
                if ((value is int)
                     || (value is uint)
                     || (value is byte)
                     || (value is sbyte)
                    )
                {
                    int intValue = (int)value;
                    return (intValue != 0);
                }
                if ((value is long)
                     || (value is ulong)
                    )
                {
                    long longValue = (long)value;
                    return (longValue != 0L);
                }
            }
            if (value is decimal)
            {
                decimal doubleValue = (decimal)value;
                return (doubleValue != 0m);
            }
            if ((value is float)
                 || (value is double)
                )
            {
                double doubleValue = (double)value;
                
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                return (doubleValue != 0.0);
            }

            throw new FormatException
                (
                    "Bad value " + value
                );
        }

#endregion
    }
}
