// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FoundPanel.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Search;

#endregion

namespace AM.Windows.Irbis
{
    /// <summary>
    /// Interaction logic for FoundPanel.xaml
    /// </summary>
    [PublicAPI]
    public partial class FoundPanel
    {
        #region Properties

        /// <summary>
        /// Current term.
        /// </summary>
        [CanBeNull]
        public FoundLine Current
        {
            get
            {
                FoundLine result = linesGrid.SelectedValue as FoundLine;

                return result;
            }
        }

        /// <summary>
        /// Terms.
        /// </summary>
        [NotNull]
        public FoundLine[] Lines { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FoundPanel()
        {
            InitializeComponent();

            FoundLine[] initialValue = new FoundLine[0];
            Lines = initialValue; // make Resharper happy
            SetFound(initialValue);
        }

        #endregion

        #region Private members

        private void Window_OnSizeChanged
            (
                object sender,
                SizeChangedEventArgs e
            )
        {
            var width = linesGrid.ActualWidth
                - SystemParameters.VerticalScrollBarWidth;
            width -= gridView.Columns[0].Width;
            width -= gridView.Columns[1].Width;
            width -= gridView.Columns[2].Width;
            width -= 10;
            gridView.Columns[3].Width = width;
        }


        #endregion

        #region Public methods

        /// <summary>
        /// Set found lines.
        /// </summary>
        public void SetFound
            (
                [NotNull] IEnumerable<FoundLine> found
            )
        {
            Code.NotNull(found, "found");

            Lines = found.ToArray();
            linesGrid.ItemsSource = Lines;
        }

        #endregion
    }
}
