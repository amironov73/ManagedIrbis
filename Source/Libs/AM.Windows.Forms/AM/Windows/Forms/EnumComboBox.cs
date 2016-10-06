/* EnumComboBox.cs -- 
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

using AM;
using AM.Reflection;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    public class EnumComboBox
        : ComboBox
    {
        #region Properties

        private Type _enumType;

        /// <summary>
        /// Gets or sets the type of the enum.
        /// </summary>
        /// <value>The type of the enum.</value>
        [DefaultValue(null)]
        [TypeConverter(typeof(EnumTypeConverter))]
        public Type EnumType
        {
            [DebuggerStepThrough]
            get
            {
                return _enumType;
            }
            set
            {
                _SetEnumType(value);
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public int? Value
        {
            get
            {
                int? result = null;
                EnumMemberInfo member = (EnumMemberInfo)SelectedItem;
                if (member != null)
                {
                    result = member.Value;
                }
                return result;
            }
            set
            {
                if (value == null)
                {
                    SelectedItem = null;
                }
                else
                {
                    foreach (EnumMemberInfo info in Items)
                    {
                        if (info.Value == value)
                        {
                            SelectedItem = info;
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EnumComboBox()
        {
            _SetupControl();
        }

        #endregion

        #region Private members

        private void _SetupControl()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void _SetEnumType(Type enumType)
        {
            Code.NotNull(enumType, "enumType");

            _enumType = enumType;
            EnumMemberInfo[] members = EnumMemberInfo.Parse(enumType);
            Items.Clear();
            Items.AddRange(members);
        }

        #endregion
    }
}