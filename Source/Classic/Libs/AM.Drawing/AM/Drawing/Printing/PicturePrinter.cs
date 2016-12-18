// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PicturePrinter.cs -- 
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
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
    public class PicturePrinter
        : Component
    {
        #region Properties

        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        /// <value>The document.</value>
        public PrintDocument Document { get; set; }

        /// <summary>
        /// Gets or sets the picture.
        /// </summary>
        /// <value>The picture.</value>
        public Image Image { get; set; }

        private float _imageScale = 1f;

        /// <summary>
        /// Gets or sets the image scale.
        /// </summary>
        /// <value>The image scale.</value>
        [DefaultValue(1f)]
        public float ImageScale
        {
            [DebuggerStepThrough]
            get
            {
                return _imageScale;
            }
            [DebuggerStepThrough]
            set
            {
                _imageScale = value;
            }
        }

        /// <summary>
        /// Gets or sets the image position.
        /// </summary>
        /// <value>The image position.</value>
        public ImagePosition ImagePosition { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private void _Print(bool preview)
        {
            if (Document == null)
            {
                Document = new PrintDocument();
            }
            if (preview)
            {
                Document.PrintController = new PreviewPrintController();
            }
            Document.DocumentName = Title;
            Document.PrintPage += _PrintPage;
            Document.Print();
        }

        private void _PrintPage
            (
                object sender,
                PrintPageEventArgs args
            )
        {
            float scale = ImageScale;
            float boundLeft = args.MarginBounds.Left / 100f;
            float boundTop = args.MarginBounds.Top / 100f;
            float boundWidth = args.MarginBounds.Width / 100f;
            float boundHeight = args.MarginBounds.Height / 100f;
            float imageWidth = Image.Width / Image.HorizontalResolution;
            float imageHeight = Image.Height / Image.VerticalResolution;
            if (scale <= 0f)
            {
                scale = Math.Min
                    (
                    boundWidth / imageWidth,
                    boundHeight / imageHeight
                    );
            }
            PointF location = new PointF(boundLeft, boundTop);
            SizeF size = new SizeF
                (
                imageWidth * scale,
                imageHeight * scale
                );
            switch (ImagePosition)
            {
                case ImagePosition.PageCenter:
                    location.X += (boundWidth - size.Width) / 2;
                    location.Y += (boundHeight - size.Height) / 2;
                    break;
                case ImagePosition.TopLeftCorner:
                    break;
                default:
                    throw new ApplicationException();
            }
            Graphics g = args.Graphics;
            g.PageUnit = GraphicsUnit.Inch;
            g.DrawImage
                (
                Image,
                new RectangleF(location, size)
                );
            args.HasMorePages = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Previews the specified <see cref="P:Image"/>.
        /// </summary>
        public void Preview()
        {
            _Print(true);
        }

        /// <summary>
        /// Prints the specified <see cref="P:Image"/>.
        /// </summary>
        public void Print()
        {
            _Print(false);
        }

        #endregion
    }
}
