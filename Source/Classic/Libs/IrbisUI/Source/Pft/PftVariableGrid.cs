// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftVariableGrid.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public partial class PftVariableGrid
        : UserControl
    {
        #region Nested classes

        class VariableInfo
        {
            #region Properties

            public string Name { get; set; }

            public object Value { get; set; }

            #endregion

            #region Public methods

            public static VariableInfo FromVariable
                (
                    PftVariable variable
                )
            {
                object value = variable.StringValue;
                if (variable.IsNumeric)
                {
                    value = variable.NumericValue;
                }

                VariableInfo result = new VariableInfo
                {
                    Name = variable.Name,
                    Value = value
                };

                return result;
            }

            #endregion
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariableGrid()
        {
            InitializeComponent();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Clear.
        /// </summary>
        public void Clear()
        {
            _grid.DataSource = null;
        }

        /// <summary>
        /// Set variables.
        /// </summary>
        public void SetVariables
            (
                [NotNull] PftVariableManager manager
            )
        {
            Code.NotNull(manager, "manager");

            PftVariable[] all = manager.GetAllVariables();
            List<VariableInfo> list = new List<VariableInfo>();
            foreach (PftVariable variable in all)
            {
                VariableInfo info = VariableInfo.FromVariable(variable);
                list.Add(info);
            }
            _grid.AutoGenerateColumns = false;
            _grid.DataSource = list.OrderBy(v => v.Name).ToArray();
        }

        #endregion
    }
}
