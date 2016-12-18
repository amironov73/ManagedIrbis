// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PaintUtility.cs -- general painting routines 
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Drawing
{
    /// <summary>
    /// General painting routines.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class PaintUtility
    {
        #region Private members

        private static void _DrawPath
            (
                Graphics g,
                Pen pen,
                GraphicsPath path
            )
        {
            if (pen != null)
            {
                g.DrawPath(pen, path);
            }
        }

        private static PointF _LastPoint
            (
                GraphicsPath path
            )
        {
            PointF[] points = path.PathPoints;
            return points[points.Length - 1];
        }

        private static ColorMatrix _Blend(float val)
        {
            float[][] matrixItems =
                {
                    new float[] {1, 0, 0, 0, 0},
                    new float[] {0, 1, 0, 0, 0},
                    new float[] {0, 0, 1, 0, 0},
                    new float[] {0, 0, 0, val, 0},
                    new float[] {0, 0, 0, 0, 1}
                };
            return new ColorMatrix(matrixItems);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Смешение рисунков.
        /// </summary>
        /// <param name="bitmap1"></param>
        /// <param name="bitmap2"></param>
        /// <param name="amount">Степень проявления второго рисунка:
        /// от 0.0f до 1.0f.</param>
        /// <returns></returns>
        /// <remarks>Результирующий рисунок имеет размер первого.
        /// </remarks>
        public static Bitmap BlendImages
            (
                [NotNull] Bitmap bitmap1,
                [NotNull] Bitmap bitmap2,
                float amount
            )
        {
            Bitmap result = new Bitmap(bitmap1);
            using (Graphics g = Graphics.FromImage(result))
            {
                Rectangle r = new Rectangle(0,
                                              0,
                                              bitmap1.Width,
                                              bitmap1.Height);
                DrawSemitransparentImage(g, bitmap2, r, amount);
            }

            return result;
        }

        /// <summary>
        /// Creates the rounded rectangle path.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static GraphicsPath CreateRoundedRectanglePath
            (
                Rectangle r,
                Size s
            )
        {
            int height = s.Height;
            int height2 = height / 2;
            int width = s.Width;
            int width2 = width / 2;

            GraphicsPath result = new GraphicsPath();
            result.AddLine(r.Left + width2, r.Top, r.Right - width2, r.Top);
            result.AddArc(r.Right - width, r.Top, width, height, 270, 90);
            result.AddLine(r.Right, r.Top + height2, r.Right, r.Bottom - height2);
            result.AddArc(r.Right - width, r.Bottom - height, width, height, 0, 90);
            result.AddLine(r.Right - width2, r.Bottom, r.Left + width2, r.Bottom);
            result.AddArc(r.Left, r.Bottom - height, width, height, 90, 90);
            result.AddLine(r.Left, r.Bottom - height2, r.Left, r.Top + height2);
            result.AddArc(r.Left, r.Top, width, height, 180, 90);
            //result.CloseFigure ();

            return result;
        }

        /// <summary>
        /// Рисует трехмерную "коробку".
        /// </summary>
        public static void DrawBox3D
            (
                [NotNull] Graphics g,
                Rectangle rect,
                Size size,
                [CanBeNull] Brush mainBrush,
                [CanBeNull] Brush auxBrush,
                [CanBeNull] Pen pen
            )
        {
            if (!ReferenceEquals(mainBrush, null))
            {
                g.FillRectangle(mainBrush, rect);
            }
            Point[] top = Points
                (
                    rect.Left,
                    rect.Top,
                    rect.Left + size.Width,
                    rect.Top - size.Height,
                    rect.Right + size.Width,
                    rect.Top - size.Height,
                    rect.Right,
                    rect.Top
                );
            Point[] right = Points
                (
                    rect.Right,
                    rect.Top,
                    rect.Right + size.Width,
                    rect.Top - size.Height,
                    rect.Right + size.Width,
                    rect.Bottom - size.Height,
                    rect.Right,
                    rect.Bottom
                );
            if (!ReferenceEquals(auxBrush, null))
            {
                g.FillPolygon(auxBrush, top);
                g.FillPolygon(auxBrush, right);
            }
            if (!ReferenceEquals(pen, null))
            {
                g.DrawRectangle(pen, rect);
                g.DrawPolygon(pen, top);
                g.DrawPolygon(pen, right);
            }
        }

        /// <summary>
        /// Рисует конус.
        /// </summary>
        public static void DrawCone
            (
                [NotNull] Graphics g,
                Rectangle rect,
                int cap,
                [CanBeNull] Brush brush,
                [CanBeNull] Pen pen
            )
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddLines(Points
                    (
                        rect.Left,
                        rect.Bottom,
                        rect.Left + rect.Width / 2,
                        rect.Top,
                        rect.Right,
                        rect.Bottom
                    ));
                path.AddArc
                    (
                        rect.Left,
                        rect.Bottom - cap,
                        rect.Width,
                        cap * 2,
                        0,
                        180
                    );
                if (!ReferenceEquals(brush, null))
                {
                    using (Region region = new Region(path))
                    {
                        g.FillRegion(brush, region);
                    }
                }
                if (!ReferenceEquals(pen, null))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        /// <summary>
        /// Закрашивает и обводит замкнутую кривую линию.
        /// </summary>
        public static void DrawClosedCurve
            (
                [NotNull] Graphics g,
                [CanBeNull] Brush brush,
                [CanBeNull] Pen pen,
                params int[] points
            )
        {
            Point[] pts = Points(points);
            if (!ReferenceEquals(brush, null))
            {
                g.FillClosedCurve(brush, pts);
            }
            if (!ReferenceEquals(pen, null))
            {
                g.DrawClosedCurve(pen, pts);
            }
        }

        /// <summary>
        /// Рисует цилиндр.
        /// </summary>
        public static void DrawCylinder
            (
                [NotNull] Graphics g,
                Rectangle rect,
                int cap,
                [CanBeNull] Brush mainBrush,
                [CanBeNull] Brush auxBrush,
                [CanBeNull] Pen pen
            )
        {
            Rectangle capRect = new Rectangle
                (
                    rect.Left,
                    rect.Top - cap,
                    rect.Width,
                    cap * 2
                );
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddLines(Points
                    (
                        rect.Left,
                        rect.Bottom,
                        rect.Left,
                        rect.Top,
                        rect.Right,
                        rect.Top,
                        rect.Right,
                        rect.Bottom
                    ));
                path.AddArc
                    (
                        rect.Left,
                        rect.Bottom - cap,
                        rect.Width,
                        cap * 2,
                        0,
                        180
                    );
                if (!ReferenceEquals(mainBrush, null))
                {
                    using (Region region = new Region(path))
                    {
                        g.FillRegion(mainBrush, region);
                    }
                }
                if (!ReferenceEquals(pen, null))
                {
                    g.DrawPath(pen, path);
                }
                if (!ReferenceEquals(auxBrush, null))
                {
                    g.FillEllipse(auxBrush, capRect);
                }
                if (!ReferenceEquals(pen, null))
                {
                    g.DrawEllipse(pen, capRect);
                }
            }
        }

        /// <summary>
        /// Рисование цилиндрика.
        /// </summary>
        public static void DrawCylinder3D
            (
                Graphics g,
                Rectangle rectangle,
                int capHeight,
                Color color,
                Pen pen
            )
        {
            using (LinearGradientBrush mainBrush =
                new LinearGradientBrush(rectangle, color, color, 0f))
            {
                using (Brush capBrush = new SolidBrush(
                    ColorUtility.Darken(color, 0.3f)))
                {
                    ColorBlend blend = new ColorBlend
                    {
                        Colors = new[]
                        {
                            color,
                            ColorUtility.Lighten(color, 0.8f),
                            color,
                            ColorUtility.Darken(color, 0.6f)
                        },
                        Positions = new[]
                        {
                            0f,
                            0.15f,
                            0.50f,
                            1f
                        }
                    };
                    mainBrush.InterpolationColors = blend;
                    DrawCylinder
                        (
                        g,
                        rectangle,
                        capHeight,
                        mainBrush,
                        capBrush,
                        null
                        );
                }
            }
        }

        /// <summary>
        /// Рисование с учетом гаммы.
        /// </summary>
        public static void DrawImageWithGamma
            (
                [NotNull] Graphics g,
                [NotNull] Image image,
                Rectangle rectangle,
                float gamma
            )
        {
            using (ImageAttributes attributes
                = new ImageAttributes())
            {
                attributes.SetGamma(gamma);
                g.DrawImage
                    (
                        image,
                        rectangle,
                        0,
                        0,
                        image.Width,
                        image.Height,
                        GraphicsUnit.Pixel,
                        attributes
                    );
            }
        }

        /// <summary>
        /// Рисует линию.
        /// </summary>
        public static void DrawLines
            (
                [NotNull] Graphics g,
                [NotNull] Pen pen,
                params int[] points
            )
        {
            g.DrawLines(pen, Points(points));
        }

        /// <summary>
        /// Закрашивает и обводит многоугольник.
        /// </summary>
        public static void DrawPolygon
            (
                [NotNull] Graphics g,
                [CanBeNull] Brush brush,
                [CanBeNull] Pen pen,
                params int[] points
            )
        {
            Point[] arrayOfPoints = Points(points);
            if (!ReferenceEquals(brush, null))
            {
                g.FillPolygon(brush, arrayOfPoints);
            }
            if (!ReferenceEquals(pen, null))
            {
                g.DrawPolygon(pen, arrayOfPoints);
            }
        }

        /// <summary>
        /// Рисует прямоугольник со скругленными углами.
        /// </summary>
        public static void DrawRoundRectangle
            (
                Graphics g,
                Pen pen,
                Rectangle rect,
                Size corners
            )
        {
            int width1 = corners.Width;
            int height1 = corners.Height;
            int width2 = width1 / 2;
            int height2 = height1 / 2;

            // Верхний сегмент
            g.DrawLine(pen,
                         rect.Left + width2,
                         rect.Top,
                         rect.Right - width2,
                         rect.Top);
            // Верхний правый
            g.DrawArc(pen,
                        rect.Right - width1,
                        rect.Top,
                        width1,
                        height1,
                        270,
                        90);
            // Правый сегмент
            g.DrawLine(pen,
                         rect.Right,
                         rect.Top + height2,
                         rect.Right,
                         rect.Bottom - height2);
            // Правый нижний
            g.DrawArc(pen,
                        rect.Right - width1,
                        rect.Bottom - height1,
                        width1,
                        height1,
                        0,
                        90);
            // Нижний сегмент
            g.DrawLine(pen,
                         rect.Left + width2,
                         rect.Bottom,
                         rect.Right - width2,
                         rect.Bottom);
            // Левый нижний
            g.DrawArc(pen,
                        rect.Left,
                        rect.Bottom - height1,
                        width1,
                        height1,
                        90,
                        90);
            // Левый сегмент
            g.DrawLine(pen,
                         rect.Left,
                         rect.Top + height2,
                         rect.Left,
                         rect.Bottom - height2);
            // Левый верхний
            g.DrawArc(pen, rect.Left, rect.Top, width1, height1, 180, 90);
        }

        /// <summary>
        /// Полупрозрачное рисование.
        /// </summary>
        public static void DrawSemitransparentImage
            (
                [NotNull] Graphics g,
                [NotNull] Image image,
                Rectangle rectangle,
                float amount
            )
        {
            ColorMatrix m = _Blend(amount);
            using (ImageAttributes attributes
                = new ImageAttributes())
            {
                attributes.SetColorMatrix
                    (
                        m,
                        ColorMatrixFlag.Default,
                        ColorAdjustType.Bitmap
                    );
                g.DrawImage
                    (
                        image,
                        rectangle,
                        0,
                        0,
                        image.Width,
                        image.Height,
                        GraphicsUnit.Pixel,
                        attributes
                    );
            }
        }

        /// <summary>
        /// Закрашивает прямоугольник со скругленными углами.
        /// </summary>
        public static void FillRoundRectangle
            (
                Graphics g,
                Brush brush,
                Rectangle rect,
                Size corners
            )
        {
            int width1 = corners.Width;
            int height1 = corners.Height;
            int width2 = width1 / 2;
            int height2 = height1 / 2;

            GraphicsPath path = new GraphicsPath();
            // Верхний сегмент
            path.AddLine(rect.Left + width2,
                           rect.Top,
                           rect.Right - width2,
                           rect.Top);
            // Верхний правый
            path.AddArc(rect.Right - width1,
                          rect.Top,
                          width1,
                          height1,
                          270,
                          90);
            // Правый сегмент
            path.AddLine(rect.Right,
                           rect.Top + height2,
                           rect.Right,
                           rect.Bottom - height2);
            // Правый нижний
            path.AddArc(rect.Right - width1,
                          rect.Bottom - height1,
                          width1,
                          height1,
                          0,
                          90);
            // Нижний сегмент
            path.AddLine(rect.Left + width2,
                           rect.Bottom,
                           rect.Right - width2,
                           rect.Bottom);
            // Левый нижний
            path.AddArc(rect.Left,
                          rect.Bottom - height1,
                          width1,
                          height1,
                          90,
                          90);
            // Левый сегмент
            path.AddLine(rect.Left,
                           rect.Top + height2,
                           rect.Left,
                           rect.Bottom - height2);
            // Левый верхний
            path.AddArc(rect.Left, rect.Top, width1, height1, 180, 90);
            path.CloseFigure();
            Region region = new Region(path);
            g.FillRegion(brush, region);
            region.Dispose();
            path.Dispose();
        }

        /// <summary>
        /// Вписывает текст в прямоугольник, заполняя его полностью.
        /// </summary>
        public static void FitString
            (
                Graphics g,
                string text,
                Rectangle rect,
                Font font,
                Brush brush,
                Pen pen
            )
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddString
                    (
                    text,
                    font.FontFamily,
                    (int)font.Style,
                    font.Size,
                    new Point(0, 0),
                        TextFormat.NearNear
                    );
                RectangleF textRect = path.GetBounds();
                PointF[] points = new PointF[]
                    {
                        new PointF ( rect.Left, rect.Top ),
                        new PointF ( rect.Right, rect.Top ),
                        new PointF ( rect.Left, rect.Bottom )
                    };
                using (new GraphicsStateSaver(g))
                {
                    g.Transform = new Matrix(textRect, points);
                    if (brush != null)
                    {
                        g.FillPath(brush, path);
                    }
                    if (pen != null)
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
        }

        /// <summary>
        /// Выводит строку с толстой обводкой.
        /// </summary>
        public static void HollowString
            (
                Graphics g,
                string text,
                Rectangle rect,
                Font font,
                Brush brush,
                Pen pen,
                int width,
                StringFormat format
            )
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddString(text,
                                 font.FontFamily,
                                 (int)font.Style,
                                 font.Size,
                                 rect,
                                 format);
                using (Pen tempPen = new Pen(Color.Black, width))
                {
                    tempPen.LineJoin = LineJoin.Round;
                    path.Widen(tempPen);
                }
                using (new GraphicsStateSaver(g))
                {
                    using (Region clipRegion = new Region(rect))
                    {
                        g.Clip = clipRegion;
                        if (pen != null)
                        {
                            g.DrawPath(pen, path);
                        }
                        if (brush != null)
                        {
                            g.FillPath(brush, path);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Нормализация угла, выраженного в градусах (приведение
        /// к интервалу от 0 до 360 градусов).
        /// </summary>
        public static float NormalizeAngle
            (
                float angle
            )
        {
            while (angle > 360f)
            {
                angle -= 360f;
            }
            while (angle < 0f)
            {
                angle += 360f;
            }

            return angle;
        }

        /// <summary>
        /// Рисует текст с ободкой.
        /// </summary>
        public static void PaintString
            (
                Graphics g,
                string text,
                Rectangle rect,
                Font font,
                Brush brush,
                Pen pen,
                StringFormat format
            )
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddString(text,
                                 font.FontFamily,
                                 (int)font.Style,
                                 font.Size,
                                 rect,
                                 format);
                using (new GraphicsStateSaver(g))
                {
                    using (Region clipRegion = new Region(rect))
                    {
                        g.Clip = clipRegion;
                        if (brush != null)
                        {
                            g.FillPath(brush, path);
                        }
                        if (pen != null)
                        {
                            g.DrawPath(pen, path);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Создает массив точек.
        /// </summary>
        public static Point[] Points
            (
                params int[] values
            )
        {
            Debug.Assert((values.Length % 2) == 0);
            Point[] result = new Point[values.Length / 2];
            for (int i = 0,
                      j = 0;
                  i < result.Length;
                  i++)
            {
                result[i].X = values[j++];
                result[i].Y = values[j++];
            }
            return result;
        }

        /// <summary>
        /// Создает массив точек.
        /// </summary>
        public static PointF[] Points
            (
                params float[] values
            )
        {
            Debug.Assert((values.Length % 2) == 0);
            PointF[] result = new PointF[values.Length / 2];
            for (int i = 0,
                      j = 0;
                  i < result.Length;
                  i++)
            {
                result[i].X = values[j++];
                result[i].Y = values[j++];
            }
            return result;
        }

        /// <summary>
        /// Преобразует систему координат в очень простую: точка (0,0)
        /// расположена точно в центре, от нее до любой границы ровно 1000
        /// попугаев.
        /// </summary>
        public static void UniformCoordinateSystem
            (
                [NotNull]Graphics g,
                Size size
            )
        {
            g.TranslateTransform
                (
                    (float)size.Width / 2,
                    (float)size.Height / 2
                );
            float inches = Math.Min(size.Width / g.DpiX, size.Height / g.DpiY);
            g.ScaleTransform(inches * g.DpiX / 2000, inches * g.DpiY / 2000);
        }

        /// <summary>
        /// Рисование шарика.
        /// </summary>
        public static void DrawSphere
            (
                Graphics g,
                Rectangle rectangle,
                Color color,
                Pen pen
            )
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(rectangle);
                using (PathGradientBrush brush =
                    new PathGradientBrush(path))
                {
                    Color lightColor = ColorUtility.Lighten(color, 0.8f);
                    Color darkColor = ColorUtility.Darken(color, 0.6f);
                    ColorBlend blend = new ColorBlend
                    {
                        Colors = new[]
                        {
                            darkColor,
                            color,
                            lightColor
                        },
                        Positions = new[]
                        {
                            0f,
                            0.25f,
                            1f
                        }
                    };
                    brush.InterpolationColors = blend;
                    brush.CenterPoint = new PointF
                        (
                        rectangle.Left + rectangle.Width * 0.3f,
                        rectangle.Top + rectangle.Width * 0.3f
                        );
                    g.FillPath(brush, path);
                }
                _DrawPath(g, pen, path);
            }
        }

        /// <summary>
        /// Рисование куска торта.
        /// </summary>
        public static void DrawPieSlice
            (
                [NotNull] Graphics g,
                Rectangle rectangle,
                float startAngle,
                float sweepAngle,
                int height,
                Color color,
                [NotNull] Pen pen
            )
        {
            // Мы не хотим морочиться с отрицательными углами.
            if (sweepAngle < 0)
            {
                startAngle += sweepAngle;
                sweepAngle = -sweepAngle;
            }
            startAngle = NormalizeAngle(startAngle);
            float endAngle = NormalizeAngle(startAngle + sweepAngle);
            Rectangle bottom = rectangle;
            bottom.Y += height;
            float rx = rectangle.Width / 2f;
            float ry = rectangle.Height / 2f;
            float cx = rectangle.Left + rx;
            float cy = rectangle.Top + ry;
            Color lighterColor = ColorUtility.Lighten(color, 0.5f);
            Color darkerColor = ColorUtility.Darken(color, 0.5f);

            // Шрифт нужен лишь для отладочной печати
            //using ( Font font = new Font ( "Arial", 8f ) )
            using (Brush mainBrush = new SolidBrush(color))
            {
                using (Brush lighterBrush = new SolidBrush(lighterColor))
                {
                    using (Brush darkerBrush = new SolidBrush(darkerColor))
                    {
                        using (LinearGradientBrush borderBrush =
                            new LinearGradientBrush(rectangle,
                                                      lighterColor,
                                                      darkerColor,
                                                      0f))
                        {
                            using (GraphicsPath arcPath = new GraphicsPath())
                            {
                                ColorBlend blend = new ColorBlend(3)
                                {
                                    Colors = new[]
                                    {
                                        color,
                                        lighterColor,
                                        color,
                                        darkerColor
                                    },
                                    Positions = new[]
                                    {
                                        0f,
                                        0.15f,
                                        0.50f,
                                        1f
                                    }
                                };
                                borderBrush.InterpolationColors = blend;

                                arcPath.AddPie(rectangle, startAngle, sweepAngle);
                                PointF[] pathPoints = arcPath.PathPoints;
                                PointF topCenter = new PointF(cx, cy);
                                PointF bottomCenter = new PointF(cx, cy + height);
                                PointF topStart = pathPoints[1];
                                PointF bottomStart = topStart;
                                bottomStart.Y += height;
                                PointF topEnd = pathPoints[pathPoints.Length - 1];
                                PointF bottomEnd = topEnd;
                                bottomEnd.Y += height;

                                PointF[] startEdge = new PointF[5];
                                startEdge[0] = topCenter;
                                startEdge[1] = topStart;
                                startEdge[2] = bottomStart;
                                startEdge[3] = bottomCenter;
                                startEdge[4] = topCenter;

                                PointF[] sweepEdge = new PointF[5];
                                sweepEdge[0] = topCenter;
                                sweepEdge[1] = topEnd;
                                sweepEdge[2] = bottomEnd;
                                sweepEdge[3] = bottomCenter;
                                sweepEdge[4] = topCenter;

                                if ((endAngle > 270)
                                     || (endAngle < 90))
                                {
                                    Brush edgeBrush = (endAngle < 90)
                                                          ? lighterBrush
                                                          : darkerBrush;
                                    using (GraphicsPath path = new GraphicsPath())
                                    {
                                        path.AddLines(sweepEdge);
                                        g.FillPath(edgeBrush, path);
                                        _DrawPath(g, pen, path);
                                    }
                                }

                                if ((startAngle > 90)
                                     && (startAngle < 270))
                                {
                                    Brush edgeBrush = (startAngle > 180)
                                                          ? lighterBrush
                                                          : darkerBrush;
                                    using (GraphicsPath path = new GraphicsPath())
                                    {
                                        path.AddLines(startEdge);
                                        g.FillPath(edgeBrush, path);
                                        _DrawPath(g, pen, path);
                                    }
                                }

                                float angle1 = startAngle;
                                if (angle1 > endAngle)
                                {
                                    angle1 = 0f;
                                }
                                float angle2 = Math.Min(endAngle, 180f);
                                if ((angle1 <= 180)
                                     || (angle2 <= 180)
                                    )
                                {
                                    using (GraphicsPath path = new GraphicsPath())
                                    {
                                        // ReSharper disable CompareOfFloatsByEqualityOperator
                                        if (angle2 == endAngle) //-V3024
                                        // ReSharper restore CompareOfFloatsByEqualityOperator
                                        {
                                            path.AddLine(topEnd, bottomEnd);
                                        }
                                        else
                                        {
                                            path.AddLine(cx - rx,
                                                           cy,
                                                           cx - rx,
                                                           cy + height);
                                        }
                                        float delta = angle2 - angle1;
                                        if (angle1 > angle2)
                                        {
                                            delta = angle2;
                                        }
                                        delta = -delta;
                                        path.AddArc(bottom, angle2, delta);
                                        PointF bottomLast = _LastPoint(path);
                                        PointF topLast = bottomLast;
                                        topLast.Y -= height;
                                        path.AddLine(bottomLast, topLast);
                                        g.FillPath(borderBrush, path);
                                        _DrawPath(g, pen, path);
                                    }
                                }

                                // Случай, когда нужны два бортика.
                                if ((startAngle < 180f)
                                     && (endAngle < startAngle))
                                {
                                    using (GraphicsPath path = new GraphicsPath())
                                    {
                                        float delta = 180f - startAngle;
                                        path.AddLine(topStart, bottomStart);
                                        path.AddArc(bottom, startAngle, delta);
                                        path.AddLine(cx - rx, cy + height, cx - rx, cy);
                                        g.FillPath(borderBrush, path);
                                        _DrawPath(g, pen, path);
                                    }
                                }

                                g.FillPath(mainBrush, arcPath);
                                _DrawPath(g, pen, arcPath);

                                // Отладка (интересно, все-таки, что же получилось ;)
                                //				g.DrawString ( startAngle.ToString (), font, Brushes.Red,
                                //					pathPoints [ 1 ], TextFormat.CenterCenter );
                                //				g.DrawString ( endAngle.ToString (), font, Brushes.SteelBlue,
                                //					pathPoints [ pathPoints.Length - 1 ],
                                //					TextFormat.CenterCenter );
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}