// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PropertyOrField.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
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
    public sealed class PropertyOrField
        : IComparable<PropertyOrField>
    {
        #region Properties

        /// <summary>
        /// Gets the field info.
        /// </summary>
        /// <value>The field info.</value>
        public FieldInfo FieldInfo
        {
            [DebuggerStepThrough]
            get
            {
                return (MemberInfo as FieldInfo);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is indexed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is indexed; otherwise, <c>false</c>.
        /// </value>
        public bool IsIndexed
        {
            [DebuggerStepThrough]
            get
            {
                return (IsProperty
                    && (PropertyInfo.GetIndexParameters().Length != 0));
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is property.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is property; otherwise, <c>false</c>.
        /// </value>
        public bool IsProperty
        {
            [DebuggerStepThrough]
            get
            {
                return (MemberInfo is PropertyInfo);
            }
        }

        private readonly MemberInfo _memberInfo;

        /// <summary>
        /// Gets the member info.
        /// </summary>
        /// <value>The member info.</value>
        public MemberInfo MemberInfo
        {
            [DebuggerStepThrough]
            get
            {
                return _memberInfo;
            }
        }

        /// <summary>
        /// Gets the type of the member.
        /// </summary>
        /// <value>The type of the member.</value>
        public Type MemberType
        {
            [DebuggerStepThrough]
            get
            {
                if (IsProperty)
                {
                    return PropertyInfo.PropertyType;
                }
                return FieldInfo.FieldType;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            [DebuggerStepThrough]
            get
            {
                return MemberInfo.Name;
            }
        }

        /// <summary>
        /// Gets the property info.
        /// </summary>
        /// <value>The property info.</value>
        public PropertyInfo PropertyInfo
        {
            [DebuggerStepThrough]
            get
            {
                return (MemberInfo as PropertyInfo);
            }
        }

        /// <summary>
        /// Gets a value indicating whether [read only].
        /// </summary>
        /// <value><c>true</c> if [read only]; 
        /// otherwise, <c>false</c>.</value>
        public bool ReadOnly
        {
            [DebuggerStepThrough]
            get
            {
                return (IsProperty && !PropertyInfo.CanWrite);
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyOrField"/> class.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        public PropertyOrField(PropertyInfo propertyInfo)
        {
            Code.NotNull(propertyInfo, "propertyInfo");

            _memberInfo = propertyInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyOrField"/> class.
        /// </summary>
        /// <param name="fieldInfo">The field info.</param>
        public PropertyOrField
            (
                FieldInfo fieldInfo
            )
        {
            Code.NotNull(fieldInfo, "fieldInfo");

            _memberInfo = fieldInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyOrField"/> class.
        /// </summary>
        /// <param name="memberInfo">The member info.</param>
        public PropertyOrField
            (
                MemberInfo memberInfo
            )
        {
            Code.NotNull(memberInfo, "memberInfo");

            if (!(memberInfo is PropertyInfo)
                 && !(memberInfo is FieldInfo))
            {
                Log.Error
                    (
                        "PropertyOrField::Constructor: "
                        + "member="
                        + memberInfo.Name
                        + "is neither property nor field"
                    );

                throw new ArgumentException
                    (
                        "Member is neither property nor field"
                    );
            }
            _memberInfo = memberInfo;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns>First found attribute or <c>null</c>.</returns>
        public T GetCustomAttribute<T>
            (
                bool inherit
            )
            where T : Attribute
        {
            foreach (T attribute
                in MemberInfo.GetCustomAttributes(typeof(T), inherit))
            {
                return attribute;
            }

            return null;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public object GetValue(object obj)
        {
            if (IsProperty)
            {
                return PropertyInfo.GetValue(obj, null);
            }
            return FieldInfo.GetValue(obj);
        }

        /// <summary>
        /// Checks whether the <see cref="PropertyOrField"/>
        /// haves the attribute.
        /// </summary>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public bool HaveAttribute<T>
            (
                bool inherit
            )
            where T : Attribute
        {
            return (GetCustomAttribute<T>(inherit) != null);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public void SetValue
            (
                object obj,
                object value
            )
        {
            if (IsProperty)
            {
                PropertyInfo.SetValue(obj, value, null);
            }
            else
            {
                FieldInfo.SetValue(obj, value);
            }
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current 
        /// <see cref="Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> that represents the current 
        /// <see cref="Object"/>.
        ///</returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region IComparable<PropertyOrField> members

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects 
        /// being compared. The return value has the following meanings: Value 
        /// Meaning Less than zero This object is less than the other parameter.
        /// Zero This object is equal to other. Greater than zero This object is 
        /// greater than other. 
        /// </returns>
        /// <param name="other">An object to compare with this object.
        /// </param>
        int IComparable<PropertyOrField>.CompareTo
            (
                [NotNull] PropertyOrField other
            )
        {
            return string.Compare(Name, other.Name);
        }

        #endregion
    }
}