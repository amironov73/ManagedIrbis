/* ViewForm.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;

#endregion

namespace Inventory2017
{
    public partial class ViewForm 
        : XtraForm
    {
        #region Properties
        #endregion

        #region Construction

        public ViewForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Public methods

        public void SetDataSource
            (
                List<BookInfo> books
            )
        {
            gridControl1.DataSource = books;
        }

        #endregion
    }
}
