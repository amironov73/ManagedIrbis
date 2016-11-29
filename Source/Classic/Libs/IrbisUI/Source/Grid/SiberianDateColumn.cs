/* SiberianDateColumn.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Collections;
using AM.IO;
using AM.Reflection;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI.Grid
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SiberianDateColumn
        : SiberianColumn
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianDateColumn()
        {
            //BackColor = Color.White;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region SiberianColumn members

        /// <inheritdoc/>
        public override SiberianCell CreateCell()
        {
            SiberianCell result = new SiberianDateCell();
            result.Column = this;

            return result;
        }

        /// <inheritdoc />
        public override Control CreateEditor
            (
                SiberianCell cell,
                bool edit,
                object state
            )
        {
            Code.NotNull(cell, "cell");

            return null;
        }

        /// <inheritdoc />
        public override void GetData
            (
                object theObject,
                SiberianCell cell
            )
        {
            SiberianDateCell dateCell = (SiberianDateCell)cell;

            if (!string.IsNullOrEmpty(Member)
                && !ReferenceEquals(theObject, null))
            {
                Type type = theObject.GetType();
                MemberInfo memberInfo = type.GetMember(Member)
                    .First();
                PropertyOrField property = new PropertyOrField
                    (
                        memberInfo
                    );

                object value = property.GetValue(theObject);
                dateCell.Date = ReferenceEquals(value, null)
                    ? DateTime.MinValue
                    : (DateTime)value;
            }
        }

        /// <inheritdoc />
        public override void PutData
            (
                object theObject,
                SiberianCell cell
            )
        {
            SiberianDateCell dateCell = (SiberianDateCell)cell;

            if (!string.IsNullOrEmpty(Member)
                && !ReferenceEquals(theObject, null))
            {
                Type type = theObject.GetType();
                MemberInfo memberInfo = type.GetMember(Member)
                    .First();
                PropertyOrField property = new PropertyOrField
                    (
                        memberInfo
                    );

                property.SetValue(theObject, dateCell.Date);
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
