// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SiberianMenuGrid.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Menus;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI.Grid
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SiberianMenuGrid
        : SiberianGrid
    {
        #region Properties

        /// <summary>
        /// Column for menu code.
        /// </summary>
        [NotNull]
        public SiberianMenuCodeColumn CodeColumn { get; private set; }

        /// <summary>
        /// Column for menu comment.
        /// </summary>
        [NotNull]
        public SiberianMenuCommentColumn CommentColumn { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianMenuGrid()
        {
            HeaderHeight = 26;

            CodeColumn = (SiberianMenuCodeColumn)CreateColumn<SiberianMenuCodeColumn>();
            CodeColumn.Title = "Code";
            CodeColumn.FillWidth = 100;

            CommentColumn = (SiberianMenuCommentColumn)CreateColumn<SiberianMenuCommentColumn>();
            CommentColumn.Title = "Comment";
            CommentColumn.FillWidth = 100;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Use given WSS.
        /// </summary>
        public void Load
            (
                [NotNull] MenuEntry[] entries
            )
        {
            Code.NotNull(entries, "entries");

            Rows.Clear();

            foreach (MenuEntry entry in entries)
            {
                CreateRow(entry);
            }
        }

        #endregion

        #region SiberianGrid members

        /// <inheritdoc/>
        protected override SiberianRow CreateRow()
        {
            SiberianRow result = base.CreateRow();
            result.Height = 24;

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
