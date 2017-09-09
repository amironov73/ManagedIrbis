// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordViewGrid.cs -- 
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

using ManagedIrbis;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public partial class RecordViewGrid
        : UserControl
    {
        #region Nested classes

        class FieldInfo
        {
            #region Properties

            public int Tag { get; set; }

            public int Repeat { get; set; }

            public string Text { get; set; }

            #endregion

            #region Public methods

            public static FieldInfo FromField
                (
                    RecordField field
                )
            {
                FieldInfo result = new FieldInfo
                {
                    Tag = field.Tag,
                    Repeat = field.Repeat,
                    Text = field.ToText()
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
        public RecordViewGrid()
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
        /// Set record to view.
        /// </summary>
        public void SetRecord
            (
                [CanBeNull] MarcRecord record
            )
        {
            if (ReferenceEquals(record, null))
            {
                _grid.DataSource = null;
                return;
            }

            _grid.AutoGenerateColumns = false;
            _grid.DataSource = record.Fields
                // ReSharper disable once ConvertClosureToMethodGroup
                .Select(field => FieldInfo.FromField(field))
                .ToArray();
        }

        #endregion
    }
}
