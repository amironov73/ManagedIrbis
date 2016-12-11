// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EnumListBox.cs -- 
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
    public class EnumListBox
        : ListBox
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
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public int Value
        {
            [DebuggerStepThrough]
            get
            {
                int result = 0;
                foreach (EnumMemberInfo item in SelectedItems)
                {
                    result |= item.Value;
                }
                return result;
            }
        }

        #endregion

        #region Private members

        private void _SetEnumType
            (
                Type enumType
            )
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