// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RainbowControl.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolboxBitmap(typeof(RainbowControl), "Images.RainbowControl.bmp")]
    public class RainbowControl
        : Control
    {
        ///<summary>
        /// 
        ///</summary>
        [NotNull]
        public RainbowItemList Items { get; private set; }

        //private Panel panel1;
        private Bitmap _triangle;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="RainbowControl"/> class.
        /// </summary>
        public RainbowControl()
        {
            const float delta = 1.0f / 6.0f;

            // ReSharper disable VirtualMemberCallInConstructor
            DoubleBuffered = true;
            // ReSharper restore VirtualMemberCallInConstructor

            Items = new RainbowItemList();

            ResizeRedraw = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            
            //InitializeComponent ();
            Items.Add(Color.Red, delta * 0f);
            Items.Add(Color.Orange, delta * 1f);
            Items.Add(Color.Yellow, delta * 2f);
            Items.Add(Color.Green, delta * 3f);
            Items.Add(Color.LightBlue, delta * 4f);
            Items.Add(Color.DarkBlue, delta * 5f);
            Items.Add(Color.Violet, 1.00f);
            using (Stream stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("AM.Windows.Forms.Images.Triangle.bmp"))
            {
                if (!ReferenceEquals(stream, null))
                {
                    _triangle = (Bitmap) Image.FromStream(stream);
                    _triangle.MakeTransparent(_triangle.GetPixel(0, 0));
                }
            }
        }

        private int _ItemPos(RainbowItem item)
        {
            return (int)(Width * item.Position) - _triangle.Width / 2;
        }

        /// <inheritdoc />
        protected override void OnPaint
            (
                PaintEventArgs e
            )
        {
            Graphics g = e.Graphics;
            if (Items.Count < 2)
            {
                return;
            }

            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (Items[0].Position != 0f) //-v3024
            // ReSharper restore CompareOfFloatsByEqualityOperator
            {
                Items[0].Position = 0f;
            }

            RainbowItem last = Items[Items.Count - 1];
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (last.Position != 1f) //-v3024
            // ReSharper restore CompareOfFloatsByEqualityOperator
            {
                last.Position = 1f;
            }

            Rectangle r = ClientRectangle;
            r.Height -= _triangle.Height;
            using (LinearGradientBrush brush = new LinearGradientBrush
                (
                    r,
                    Items[0].Color,
                    Items[Items.Count - 1].Color,
                    0f
                ))
            {
                ColorBlend blend = new ColorBlend(Items.Count)
                {
                    Colors = new Color[Items.Count]
                };

                for (int i = 0; i < Items.Count; i++)
                {
                    blend.Colors[i] = Items[i].Color;
                    blend.Positions[i] = Items[i].Position;
                }
                brush.InterpolationColors = blend;
                g.FillRectangle(brush, r);
            }
            foreach (RainbowItem item in Items)
            {
                g.DrawImageUnscaled
                    (
                        _triangle,
                        _ItemPos(item),
                        Height - _triangle.Height
                    );
            }
            if (_drawMouse)
            {
                g.DrawLine
                    (
                        Pens.LightGray,
                        _mousePos,
                        0,
                        _mousePos,
                        Height
                    );
            }
            base.OnPaint(e);
        }

        private RainbowItem _moving;
        private int _startX, _minX, _maxX;

        /// <inheritdoc />
        protected override void OnMouseDown
            (
                MouseEventArgs e
            )
        {
            bool moving = false;
            int previousIndex = 0;
            bool badIndex = false;
            for (int index = 0; index < Items.Count; index++)
            {
                RainbowItem item = Items[index];
                int pos = _ItemPos(item);
                if ((e.X >= pos) && (e.X <= pos + _triangle.Width))
                {
                    if ((index > 0) && (index < Items.Count - 1))
                    {
                        Capture = true;
                        _moving = item;
                        _startX = e.X;
                        _minX = (int)(Items[index - 1].Position * Width) + 1;
                        _maxX = (int)(Items[index + 1].Position * Width) - 1;
                        moving = true;
                    }
                    else
                    {
                        badIndex = true;
                    }
                    break;
                }
                if (pos < e.X)
                {
                    previousIndex = index;
                }
            }
            if (!moving && !badIndex)
            {
                Items.Insert(previousIndex + 1,
                    new RainbowItem(Items[previousIndex].Color,
                    ((float)e.X) / Width));
                Invalidate();
                OnMouseDown(e);

            }
            base.OnMouseDown(e);
        }

        /// <inheritdoc />
        protected override void OnMouseUp
            (
                MouseEventArgs e
            )
        {
            Capture = false;
            _moving = null;
            base.OnMouseUp(e);
        }

        private int _mousePos;
        private bool _drawMouse;

        /// <inheritdoc />
        protected override void OnMouseEnter
            (
                EventArgs e
            )
        {
            _drawMouse = true;
            base.OnMouseEnter(e);
        }

        /// <inheritdoc />
        protected override void OnMouseLeave
            (
                EventArgs e
            )
        {
            _drawMouse = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        /// <inheritdoc />
        protected override void OnMouseMove
            (
                MouseEventArgs e
            )
        {
            if (Capture)
            {
                if (e.Y > Height)
                {
                    int index = Items.IndexOf(_moving);
                    if ((index > 0) && (e.X == _startX)
                        && (index < (Items.Count - 1)))
                    {
                        Items.RemoveAt(index);
                        Capture = false;
                    }
                }
                else if ((e.X >= 0) && (e.X < Width)
                    && (e.X >= _minX) && (e.X <= _maxX))
                {
                    if (_moving != null)
                    {
                        _moving.Position = ((float)e.X) / Width;
                    }
                }
            }
            _mousePos = e.X;
            Invalidate();
            base.OnMouseMove(e);
        }

        /// <inheritdoc />
        protected override void OnMouseDoubleClick
            (
                MouseEventArgs mea
            )
        {
            foreach (RainbowItem item in Items)
            {
                int pos = _ItemPos(item);
                if ((mea.X >= pos) && (mea.X <= pos + _triangle.Width))
                {
                    ColorDialog dialog = new ColorDialog
                    {
                        FullOpen = true,
                        Color = item.Color
                    };
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        item.Color = dialog.Color;
                        Invalidate();
                    }
                }
            }
        }


    }
}
