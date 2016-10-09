/* IndexableConverter.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Globalization;

#endregion

namespace AM.Collections
{
    //internal sealed class IndexableConverter
    //    : TypeConverter
    //{
    //    ///<summary>
    //    /// Returns whether this converter can convert an object of the given 
    //    /// type to the type of this converter, using the specified context.
    //    ///</summary>
    //    ///<returns>
    //    /// <c>true</c> if this converter can perform the conversion; 
    //    /// otherwise, <c>false</c>.
    //    ///</returns>
    //    ///<param name="context">An 
    //    /// <see cref="T:System.ComponentModel.ITypeDescriptorContext"/>
    //    /// that provides a format context.</param>
    //    ///<param name="sourceType">A <see cref="T:System.Type"/>
    //    /// that represents the type you want to convert from.</param>
    //    public override bool CanConvertFrom
    //        (
    //            ITypeDescriptorContext context,
    //            Type sourceType
    //        )
    //    {
    //        if (sourceType == typeof(string))
    //        {
    //            return true;
    //        }
    //        return base.CanConvertFrom(context, sourceType);
    //    }

    //    ///<summary>
    //    /// Returns whether this converter can convert the object to the 
    //    /// specified type, using the specified context.
    //    ///</summary>
    //    ///<returns>
    //    /// <c>true</c> if this converter can perform the conversion; 
    //    /// otherwise, <c>false</c>.
    //    ///</returns>
    //    ///<param name="context">An 
    //    /// <see cref="T:System.ComponentModel.ITypeDescriptorContext"/>
    //    /// that provides a format context.</param>
    //    ///<param name="destinationType">A <see cref="T:System.Type"/>
    //    /// that represents the type you want to convert to.</param>
    //    public override bool CanConvertTo
    //        (
    //            ITypeDescriptorContext context,
    //            Type destinationType
    //        )
    //    {
    //        if (destinationType == typeof(string))
    //        {
    //            return true;
    //        }
    //        return base.CanConvertTo(context, destinationType);
    //    }

    //    ///<summary>
    //    /// Converts the given object to the type of this converter, 
    //    /// using the specified context and culture information.
    //    ///</summary>
    //    ///<returns>
    //    /// An <see cref="T:System.Object"/>
    //    /// that represents the converted value.
    //    ///</returns>
    //    ///<param name="culture">The 
    //    /// <see cref="T:System.Globalization.CultureInfo"/>
    //    /// to use as the current culture.</param>
    //    ///<param name="context">An 
    //    /// <see cref="T:System.ComponentModel.ITypeDescriptorContext"/>
    //    /// that provides a format context. </param>
    //    ///<param name="value">The <see cref="T:System.Object"/>
    //    /// to convert. </param>
    //    ///<exception cref="T:System.NotSupportedException">
    //    /// The conversion cannot be performed.</exception>
    //    /// <exception cref="T:System.FormatException">
    //    /// Invalid format.
    //    /// </exception>
    //    public override object ConvertFrom
    //        (
    //            ITypeDescriptorContext context,
    //            CultureInfo culture,
    //            object value
    //        )
    //    {
    //        string input = value as string;
    //        if (!string.IsNullOrEmpty(input))
    //        {
    //            string[] tokens = input.Split(';');
    //            switch (tokens.Length)
    //            {
    //                case 2:
    //                    return new Pair<string, string>
    //                        (
    //                        tokens[0],
    //                        tokens[1]
    //                        );
    //                case 3:
    //                    return new Triplet<string, string, string>
    //                        (
    //                        tokens[0],
    //                        tokens[1],
    //                        tokens[2]
    //                        );
    //                case 4:
    //                    return new Quartet<string, string, string, string>
    //                        (
    //                        tokens[0],
    //                        tokens[1],
    //                        tokens[2],
    //                        tokens[3]
    //                        );
    //                default:
    //                    throw new FormatException();
    //            }
    //        }
    //        return base.ConvertFrom(context, culture, value);
    //    }

    //    ///<summary>
    //    /// Converts the given value object to the specified type, 
    //    /// using the specified context and culture information.
    //    ///</summary>
    //    ///<returns>
    //    /// An <see cref="T:System.Object"/>
    //    /// that represents the converted value.
    //    ///</returns>
    //    ///<param name="culture">A 
    //    /// <see cref="T:System.Globalization.CultureInfo"/>.
    //    /// If null is passed, the current culture is assumed.</param>
    //    ///<param name="context">An 
    //    /// <see cref="T:System.ComponentModel.ITypeDescriptorContext"/>
    //    /// that provides a format context. </param>
    //    ///<param name="destinationType">The 
    //    /// <see cref="T:System.Type"/> to convert the value parameter to.
    //    /// </param>
    //    ///<param name="value">The <see cref="T:System.Object"/>
    //    /// to convert.</param>
    //    ///<exception cref="T:System.NotSupportedException">
    //    /// The conversion cannot be performed.</exception>
    //    ///<exception cref="T:System.ArgumentNullException">
    //    /// The destinationType parameter is null.</exception>
    //    public override object ConvertTo
    //        (
    //        ITypeDescriptorContext context,
    //        CultureInfo culture,
    //        object value,
    //        Type destinationType)
    //    {
    //        if (value != null)
    //        {
    //            return value.ToString();
    //        }
    //        return base.ConvertTo(context, culture, value, destinationType);
    //    }
    //}
}