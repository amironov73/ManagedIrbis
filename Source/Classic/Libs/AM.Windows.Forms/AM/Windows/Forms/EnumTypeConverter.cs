/* EnumTypeConverter.cs -- 
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Globalization;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class EnumTypeConverter
        : TypeConverter
    {
        /// <summary>
        /// Returns whether this converter can convert
        /// an object of the given type to the type
        /// of this converter, using the specified context.
        /// </summary>
        /// <param name="context">An
        /// <see cref="T:System.ComponentModel.ITypeDescriptorContext"/>
        /// that provides a format context.</param>
        /// <param name="sourceType">A <see cref="T:System.Type"/>
        /// that represents the type you want to convert from.</param>
        /// <returns>
        /// true if this converter can perform the conversion;
        /// otherwise, false.
        /// </returns>
        public override bool CanConvertFrom
            (
                ITypeDescriptorContext context,
                Type sourceType
            )
        {
            if ((sourceType == typeof(string))
                 || (sourceType == typeof(Type)))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Returns whether this converter can convert
        /// the object to the specified type, using
        /// the specified context.
        /// </summary>
        /// <param name="context">An
        /// <see cref="T:System.ComponentModel.ITypeDescriptorContext"/>
        /// that provides a format context.</param>
        /// <param name="destinationType">A <see cref="T:System.Type"/>
        /// that represents the type you want to convert to.</param>
        /// <returns>
        /// true if this converter can perform the conversion;
        /// otherwise, false.
        /// </returns>
        public override bool CanConvertTo
            (
                ITypeDescriptorContext context,
                Type destinationType
            )
        {
            if ((destinationType == typeof(string))
                 || (destinationType == typeof(Type)))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"></see> to use as the current culture.</param>
        /// <param name="value">The <see cref="T:System.Object"></see> to convert.</param>
        /// <returns>
        /// An <see cref="T:System.Object"></see> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
        public override object ConvertFrom
            (
                ITypeDescriptorContext context,
                CultureInfo culture,
                object value
            )
        {
            string s = value as string;
            if (!string.IsNullOrEmpty(s))
            {
                return Type.GetType(s);
            }
            Type t = value as Type;
            if (t != null)
            {
                return t.FullName;
            }

            return base.ConvertFrom
                (
                    context,
                    culture,
                    value
                );
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"></see>. If null is passed, the current culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object"></see> to convert.</param>
        /// <param name="destinationType">The <see cref="T:System.Type"></see> to convert the value parameter to.</param>
        /// <returns>
        /// An <see cref="T:System.Object"></see> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
        /// <exception cref="T:System.ArgumentNullException">The destinationType parameter is null. </exception>
        public override object ConvertTo
            (
                ITypeDescriptorContext context,
                CultureInfo culture,
                object value,
                Type destinationType
            )
        {
            string s = value as string;
            if (!string.IsNullOrEmpty(s)
                 && (destinationType == typeof(Type)))
            {
                return Type.GetType(s);
            }
            Type t = value as Type;
            if ((t != null)
                 && (destinationType == typeof(string)))
            {
                return t.FullName;
            }

            return base.ConvertTo
                (
                    context,
                    culture,
                    value,
                    destinationType
                );
        }
    }
}