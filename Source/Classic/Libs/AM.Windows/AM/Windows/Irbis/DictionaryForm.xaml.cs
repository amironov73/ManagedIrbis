// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DictionaryForm.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using ManagedIrbis.Search;

#endregion

namespace AM.Windows.Irbis
{
    /// <summary>
    /// Interaction logic for DictionaryForm.xaml
    /// </summary>
    public partial class DictionaryForm
    {
        #region Properties

        /// <summary>
        /// Current item.
        /// </summary>
        public TermInfo CurrentItem
        {
            get
            {
                TermInfo result = listGrid.SelectedValue as TermInfo;

                return result;
            }
        }

        /// <summary>
        /// Terms.
        /// </summary>
        public TermInfo[] Terms { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DictionaryForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private void Window_SizeChanged
            (
                object sender,
                SizeChangedEventArgs e
            )
        {
            double workingWidth = listGrid.ActualWidth
                - SystemParameters.VerticalScrollBarWidth;
            gridView.Columns[0].Width = workingWidth * 0.1;
            gridView.Columns[1].Width = workingWidth - gridView.Columns[0].Width;
        }

        private void GoButton_OnClick
            (
                object sender,
                RoutedEventArgs e
            )
        {
            DialogResult = true;
        }

        private void CancelButton_OnClick
            (
                object sender,
                RoutedEventArgs e
            )
        {
            DialogResult = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Set terms.
        /// </summary>
        public void SetTerms(TermInfo[] terms)
        {
            Terms = terms;
            listGrid.ItemsSource = terms;
        }

        #endregion
    }
}
