/* PftGlobalGrid.cs -- 
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
    public partial class PftGlobalGrid
        : UserControl
    {
        #region Nested classes

        class GlobalInfo
        {
            #region Properties

            public int Number { get; set; }

            public string Value { get; set; }

            #endregion

            #region Public methods

            public static GlobalInfo FromGlobal
                (
                    PftGlobal variable
                )
            {
                string value = StringUtility.Join
                    (
                        Environment.NewLine,
                        variable.Fields.Select
                        (
                            f => f.ToText()
                        )
                    );

                GlobalInfo result = new GlobalInfo
                {
                    Number = variable.Number,
                    Value = value
                };

                return result;
            }

            #endregion
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor
        /// </summary>
        public PftGlobalGrid()
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
        /// Set globals.
        /// </summary>
        public void SetGlobals
            (
                [NotNull] PftGlobalManager manager
            )
        {
            Code.NotNull(manager, "manager");

            List<GlobalInfo> list = new List<GlobalInfo>();
            foreach (PftGlobal variable in manager.GetAllVariables())
            {
                GlobalInfo item = GlobalInfo.FromGlobal(variable);
                list.Add(item);
            }

            _grid.AutoGenerateColumns = false;
            _grid.DataSource = list.ToArray();
        }

        #endregion
    }
}
