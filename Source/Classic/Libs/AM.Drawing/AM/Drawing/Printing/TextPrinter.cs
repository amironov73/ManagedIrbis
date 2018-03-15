// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextPrinter.cs -- 
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 */

#region Using directives

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Drawing.Printing
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class TextPrinter
        : Component
    {
        #region Events

        /// <summary>
        /// 
        /// </summary>
        public event PrintEventHandler BeginPrint;

        /// <summary>
        /// 
        /// </summary>
        public event PrintEventHandler EndPrint;

        /// <summary>
        /// 
        /// </summary>
        public event PrintPageEventHandler PrintPage;

        /// <summary>
        /// 
        /// </summary>
        public event QueryPageSettingsEventHandler QueryPageSettings;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the borders.
        /// </summary>
        public RectangleF Borders { get; set; }

        /// <summary>
        /// Gets or sets the name of the document.
        /// </summary>
        [NotNull]
        public string DocumentName { get; set; }

        /// <summary>
        /// Gets the page number.
        /// </summary>
        public int PageNumber
        {
            get; protected set;
        }

        /// <summary>
        /// Gets or sets the page settings.
        /// </summary>
        public PageSettings PageSettings { get; set; }

        /// <summary>
        /// Gets or sets the print controller.
        /// </summary>
        public PrintController PrintController { get; set; }

        /// <summary>
        /// Gets or sets the printer settings.
        /// </summary>
        public PrinterSettings PrinterSettings { get; set; }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        public Color TextColor { get; set; }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        [NotNull]
        public Font TextFont { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TextPrinter"/> class.
        /// </summary>
        protected TextPrinter()
        {
            Borders = new RectangleF(10f, 10f, 10f, 10f);
            DocumentName = "Text document";
            TextColor = Color.Black;
            TextFont = new Font(FontFamily.GenericSerif, 12f);
        }

        #endregion

        #region Private members

        /// <summary>
        /// Called when [begin print].
        /// </summary>
        protected virtual void OnBeginPrint
            (
                object sender,
                PrintEventArgs e
            )
        {
            PrintEventHandler handler = BeginPrint;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Called when [end print].
        /// </summary>
        protected virtual void OnEndPrint
            (
                object sender,
                PrintEventArgs e
            )
        {
            PrintEventHandler handler = EndPrint;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Called when [print page].
        /// </summary>
        protected virtual void OnPrintPage
            (
                object sender,
                PrintPageEventArgs ea
            )
        {
            PrintPageEventHandler handler = PrintPage;
            if (handler != null)
            {
                handler(this, ea);
            }
        }

        /// <summary>
        /// Called when [query page settings].
        /// </summary>
        protected virtual void OnQueryPageSettings
            (
                object sender,
                QueryPageSettingsEventArgs e
            )
        {
            ++PageNumber;
            QueryPageSettingsEventHandler handler = QueryPageSettings;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Prints the specified text.
        /// </summary>
        public virtual bool Print(string text)
        {
            Code.NotNull(text, "text");

            using (PrintDocument document = new PrintDocument())
            {
                document.DocumentName = DocumentName;
                document.OriginAtMargins = false; // ???
                if (PageSettings != null)
                {
                    document.DefaultPageSettings = PageSettings;
                }
                if (PrintController != null)
                {
                    document.PrintController = PrintController;
                }
                if (PrinterSettings != null)
                {
                    document.PrinterSettings = PrinterSettings;
                }
                document.BeginPrint += OnBeginPrint;
                document.EndPrint += OnEndPrint;
                document.PrintPage += OnPrintPage;
                document.QueryPageSettings += OnQueryPageSettings;
                PageNumber = 1;
                document.Print();

                return true;
            }
        }

        #endregion
    }
}
