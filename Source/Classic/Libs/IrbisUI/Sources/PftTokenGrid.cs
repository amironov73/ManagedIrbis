/* PftTokenGrid.cs -- 
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
    public partial class PftTokenGrid
        : UserControl
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftTokenGrid()
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
        /// Set tokens.
        /// </summary>
        public void SetTokens
            (
                [NotNull] PftTokenList tokens
            )
        {
            Code.NotNull(tokens, "tokens");

            _grid.AutoGenerateColumns = false;
            _grid.DataSource = tokens.ToArray();
        }

        #endregion
    }
}
