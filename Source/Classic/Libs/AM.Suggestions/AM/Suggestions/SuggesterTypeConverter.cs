/* SuggesterTypeConverter.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if CLASSIC

#region Using directives

using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Suggestions
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SuggesterTypeConverter
        : TypeConverter
    {
        #region Construction

        #endregion

        #region Private members

        private ISuggester _suggester;

        #endregion

        #region TypeConverter members

        /// <inheritdoc />
        public override StandardValuesCollection GetStandardValues
            (
                ITypeDescriptorContext context
            )
        {
            if (_suggester != null)
            {
                return new StandardValuesCollection
                    (
                        _suggester.SuggestedValues()
                    );
            }

            PropertyDescriptor propertyDescriptor
                = context.PropertyDescriptor;
            if (ReferenceEquals(propertyDescriptor, null))
            {
                return null;
            }

            AttributeCollection attributes = propertyDescriptor.Attributes;
            SuggestAttribute suggestAttribute
                = (SuggestAttribute)attributes[typeof(SuggestAttribute)];
            Type type = suggestAttribute.Type;
            _suggester = (ISuggester)Activator.CreateInstance(type);

            return new StandardValuesCollection
                (
                    _suggester.SuggestedValues()
                );
        }

        /// <inheritdoc />
        public override bool GetStandardValuesSupported
            (
                ITypeDescriptorContext context
            )
        {
            return true;
        }

        #endregion
    }
}

#endif
