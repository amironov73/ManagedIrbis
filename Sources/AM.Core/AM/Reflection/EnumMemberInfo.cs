/* EnumMemberInfo.cs -- information about Enum member.
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#if !NETCORE

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Reflection
{
    /// <summary>
    /// Information about <see cref="T:System.Enum"/> member.
    /// </summary>
    public sealed class EnumMemberInfo
    {
        #region Properties

        /// <summary>
        /// Gets the display name of the enum member.
        /// </summary>
        /// <value>Display name of the enum member.</value>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public int Value { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="EnumMemberInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="displayName">Name of the display.</param>
        /// <param name="value">The value.</param>
        public EnumMemberInfo
            (
                [NotNull] string name,
                [NotNull] string displayName,
                int value
            )
        {
            Code.NotNullNorEmpty(name, "name");
            Code.NotNullNorEmpty(displayName, "displayName");

            Name = name;
            DisplayName = displayName;
            Value = value;
        }

        #endregion

        #region Private members

        private enum SortBy
        {
            Name,
            FriendlyName,
            Value
        }

        private class MemberComparer
            : IComparer
        {
            private SortBy _sortBy;

            public MemberComparer(SortBy sortBy)
            {
                _sortBy = sortBy;
            }

            /// <summary>
            /// Compares two objects and returns a value indicating 
            /// whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.
            /// </param>
            /// <param name="y">The second object to compare.
            /// </param>
            /// <returns>
            /// Value Condition Less than zero x is less than y. 
            /// Zero x equals y. Greater than zero x is greater than y.
            /// </returns>
            /// <exception cref="T:System.ArgumentException">
            /// Neither x nor y implements the 
            /// <see cref="T:System.IComparable"/> interface.
            /// -or- x and y are of different types and neither one can 
            /// handle comparisons with the other. </exception>
            public int Compare(object x, object y)
            {
                EnumMemberInfo first = (EnumMemberInfo)x;
                EnumMemberInfo second = (EnumMemberInfo)y;
                switch (_sortBy)
                {
                    case SortBy.Name:
                        return string.Compare
                            (
                                first.Name,
                                second.Name,
                                StringComparison.InvariantCulture
                            );

                    case SortBy.FriendlyName:
                        return string.Compare
                            (
                                first.DisplayName,
                                second.DisplayName,
                                StringComparison.InvariantCulture
                            );

                    case SortBy.Value:
                        return first.Value - second.Value;
                }

                return 0;
            }
        }

        private static void _SortBy
            (
                EnumMemberInfo[] members,
                SortBy sortBy
            )
        {
            MemberComparer comparer = new MemberComparer(sortBy);
            Array.Sort(members, comparer);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Parses the specified enum type.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        public static EnumMemberInfo[] Parse(Type enumType)
        {
            // ArgumentUtility.NotNull ( enumType, "enumType" );
            List<EnumMemberInfo> result = new List<EnumMemberInfo>();
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("enumType");
            }
            Type underlyingType = Enum.GetUnderlyingType(enumType);
            switch (underlyingType.Name)
            {
                case "Byte":
                case "SByte":
                case "Int16":
                case "UInt16":
                case "Int32":
                case "UInt32":
                    break;
                default:
                    throw new ArgumentException("enumType");
            }
            foreach (string name in Enum.GetNames(enumType))
            {
                FieldInfo field =
                    enumType.GetField(name
                        /*, BindingFlags.Public | BindingFlags.GetField */ );
                DisplayNameAttribute titleAttribute
                    = (DisplayNameAttribute)Attribute.GetCustomAttribute
                        (
                            field,
                            typeof(DisplayNameAttribute)
                        );

                string displayName = (titleAttribute == null)
                                        ? name
                                        : titleAttribute.DisplayName;
                int value = (int)Enum.Parse(enumType, name);
                EnumMemberInfo info
                    = new EnumMemberInfo(name, displayName, value);
                result.Add(info);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Sorts the name of the by display.
        /// </summary>
        /// <param name="members">The members.</param>
        public static void SortByDisplayName(EnumMemberInfo[] members)
        {
            _SortBy(members, SortBy.FriendlyName);
        }

        /// <summary>
        /// Sorts the name of the by.
        /// </summary>
        /// <param name="members">The members.</param>
        public static void SortByName(EnumMemberInfo[] members)
        {
            _SortBy(members, SortBy.Name);
        }

        /// <summary>
        /// Sorts the by value.
        /// </summary>
        /// <param name="members">The members.</param>
        public static void SortByValue(EnumMemberInfo[] members)
        {
            _SortBy(members, SortBy.Value);
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="T:System.String"/>
        /// that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents 
        /// the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return DisplayName;
        }

        #endregion
    }
}

#endif
