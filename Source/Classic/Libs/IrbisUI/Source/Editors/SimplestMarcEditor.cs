// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SimplestMarcEditor.cs -- 
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

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI.Editors
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public partial class SimplestMarcEditor
        : UserControl
    {
        #region Properties

        /// <summary>
        /// Whether the editor is read-only.
        /// </summary>
        public bool ReadOnly
        {
            get { return _gridView.ReadOnly; }
            set
            {
                _bindingNavigator.Enabled = !value;
                _gridView.ReadOnly = value;
            }
        }

        #endregion

        #region Constructions

        /// <summary>
        /// 
        /// </summary>
        public SimplestMarcEditor()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private RecordFieldCollection _originalFields;

        private List<FieldItem> _items;

        #endregion

        #region Public methods

        /// <summary>
        /// Get the fields.
        /// </summary>
        public void GetFields
            (
                [NotNull] RecordFieldCollection collection
            )
        {
            Code.NotNull(collection, "collection");

            collection.BeginUpdate();
            collection.Clear();
            collection.EnsureCapacity(_items.Count);

            foreach (FieldItem item in _items)
            {
                string tag = item.Tag;
                string text = item.Text;

                if (string.IsNullOrEmpty(tag)
                    || string.IsNullOrEmpty(text))
                {
                    continue;
                }

                RecordField field = RecordField.Parse(tag, text);
                if (field.Verify(false))
                {
                    collection.Add(field);
                }
            }

            collection.EndUpdate();
        }

        /// <summary>
        /// Set the fields.
        /// </summary>
        public void SetFields
            (
                [NotNull] RecordFieldCollection collection
            )
        {
            Code.NotNull(collection, "collection");

            _originalFields = collection;
            List<FieldItem> list = new List<FieldItem>(collection.Count);
            foreach (RecordField field in collection)
            {
                FieldItem item = new FieldItem
                {
                    Tag = field.Tag,
                    Text = field.ToText()
                };
                list.Add(item);
            }
            _items = list;
            _bindingSource.DataSource = _items;
        }

        #endregion
    }
}
