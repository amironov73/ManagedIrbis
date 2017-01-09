// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ColorUtility.cs -- useful routines for color manipulations
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Drawing;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Drawing
{
    /// <summary>
    /// <see cref="Color"/> manipulation helper methods.
    /// </summary>
    [PublicAPI]
    public static class ColorUtility
    {
        #region Private members

        /// <summary>
        /// For comparing with zero.
        /// </summary>
        private const double Tolerance = 0.001;

        /// <summary>
        /// Нормализация компонента цвета.
        /// </summary>
        private static int _Normalize
            (
                float component
            )
        {
            return Math.Max(0, Math.Min(255, (int)component));
        }

        private static int _ToRGB1
            (
                float rm1,
                float rm2,
                float rh
            )
        {
            if (rh > 360.0f)
            {
                rh -= 360.0f;
            }
            else if (rh < 0.0f)
            {
                rh += 360.0f;
            }

            if (rh < 60.0f)
            {
                rm1 = rm1 + (rm2 - rm1) * rh / 60.0f;
            }
            else if (rh < 180.0f)
            {
                rm1 = rm2;
            }
            else if (rh < 240.0f)
            {
                rm1 = rm1 + (rm2 - rm1) * (240.0f - rh) / 60.0f;
            }

            return _Normalize(rm1 * 255);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Смешение двух цветов в заданной пропорции.
        /// </summary>
        /// <param name="color1">Первый цвет.</param>
        /// <param name="color2">Второй цвет.</param>
        /// <param name="amount">Доля второго цвета (число от 0 до 1,
        /// 0 - остается только первый цвет, 1 - остается только второй
        /// цвет).</param>
        /// <returns>Смешанный цвет.</returns>
        public static Color Blend
            (
                Color color1,
                Color color2,
                float amount
            )
        {
            float amount1 = 1f - amount;
            int red = _Normalize(color1.R * amount1 + color2.R * amount);
            int green = _Normalize(color1.G * amount1 + color2.G * amount);
            int blue = _Normalize(color1.B * amount1 + color2.B * amount);

            return Color.FromArgb(red, green, blue);
        }

        /// <summary>
        /// Затемнение цвета (на самом деле лишь подмешивание черного цвета).
        /// </summary>
        /// <param name="color">Затемняемый цвет.</param>
        /// <param name="amount">Степень затемнения (доля черного цвета,
        /// число от 0 до 1).</param>
        /// <returns>Затемненный цвет.</returns>
        public static Color Darken
            (
                Color color,
                float amount
            )
        {
            return Blend(color, Color.Black, amount);
        }

        /// <summary>
        /// Создает цвет из компонент "Hue/Luminance/Saturation".
        /// </summary>
        public static Color FromHls
            (
                float hue,
                float luminance,
                float saturation
            )
        {
            Debug.Assert((hue >= 0f) && (hue <= 360f));
            Debug.Assert((luminance >= 0f) && (luminance <= 1f));
            Debug.Assert((saturation >= 0f) && (saturation <= 1f));

            int red, green, blue;
            if (Math.Abs(saturation) < Tolerance)
            {
                red = _Normalize(luminance * 255.0F);
                green = red;
                blue = red;
            }
            else
            {
                float rm2;

                if (luminance <= 0.5f)
                {
                    rm2 = luminance + luminance * saturation;
                }
                else
                {
                    rm2 = luminance + saturation
                        - luminance * saturation;
                }
                var rm1 = 2.0f * luminance - rm2;
                red = _ToRGB1(rm1, rm2, hue + 120.0f);
                green = _ToRGB1(rm1, rm2, hue);
                blue = _ToRGB1(rm1, rm2, hue - 120.0f);
            }

            return Color.FromArgb(red, green, blue);
        }

        /// <summary>
        /// Осветление цвета (на самом деле лишь подмешнивание белого цвета).
        /// </summary>
        /// <param name="color">Осветляемый цвет.</param>
        /// <param name="amount">Степень осветления (доля белого цвета,
        /// число от 0 до 1).</param>
        /// <returns>Осветленный цвет.</returns>
        public static Color Lighten
            (
                Color color,
                float amount
            )
        {
            return Blend(color, Color.White, amount);
        }

        /// <summary>
        /// Convert color to Blue-Green-Red representation.
        /// </summary>
        public static int ToBgr
            (
                Color color
            )
        {
            return unchecked((((color.B << 8) + color.G) << 8) + color.R);
        }

        /// <summary>
        /// Returns a color based on XAML color string.
        /// </summary>
        /// <param name="colorString">The color string.
        /// Any format used in XAML should work.</param>
        /// <returns>Parsed color</returns>
        /// <remarks>
        /// Borrowed from UWPCommunityToolkit:
        /// https://github.com/Microsoft/UWPCommunityToolkit/blob/dev/Microsoft.Toolkit.Uwp/Helpers/ColorHelper.cs
        /// </remarks>
        public static Color ToColor
            (
                [NotNull] this string colorString
            )
        {
            Code.NotNullNorEmpty(colorString, "colorString");

            if (colorString[0] == '#')
            {
                uint temp;
                byte a, r, g, b;

                switch (colorString.Length)
                {
                    case 9:
                        temp = Convert.ToUInt32(colorString.Substring(1), 16);
                        a = (byte)(temp >> 24);
                        r = (byte)((temp >> 16) & 0xff);
                        g = (byte)((temp >> 8) & 0xff);
                        b = (byte)(temp & 0xff);

                        return Color.FromArgb(a, r, g, b);

                    case 7:
                        temp = Convert.ToUInt32(colorString.Substring(1), 16);
                        r = (byte)((temp >> 16) & 0xff);
                        g = (byte)((temp >> 8) & 0xff);
                        b = (byte)(temp & 0xff);

                        return Color.FromArgb(255, r, g, b);

                    case 5:
                        temp = Convert.ToUInt16(colorString.Substring(1), 16);
                        a = (byte)(temp >> 12);
                        r = (byte)((temp >> 8) & 0xf);
                        g = (byte)((temp >> 4) & 0xf);
                        b = (byte)(temp & 0xf);
                        a = (byte)(a << 4 | a);
                        r = (byte)(r << 4 | r);
                        g = (byte)(g << 4 | g);
                        b = (byte)(b << 4 | b);

                        return Color.FromArgb(a, r, g, b);

                    case 4:
                        temp = Convert.ToUInt16(colorString.Substring(1), 16);
                        r = (byte)((temp >> 8) & 0xf);
                        g = (byte)((temp >> 4) & 0xf);
                        b = (byte)(temp & 0xf);
                        r = (byte)(r << 4 | r);
                        g = (byte)(g << 4 | g);
                        b = (byte)(b << 4 | b);

                        return Color.FromArgb(255, r, g, b);

                    default:
                        throw new FormatException
                            (
                                string.Format
                                (
                                    "The {0} string passed in the colorString "
                                    + "argument is not a recognized Color format.",
                                    colorString
                                )
                            );
                }
            }

            if (
                    colorString.Length > 3
                    && colorString[0] == 's'
                    && colorString[1] == 'c'
                    && colorString[2] == '#'
                )
            {
                string[] values = colorString.Split(',');

                if (values.Length == 4)
                {
                    double scA = double.Parse(values[0].Substring(3));
                    double scR = double.Parse(values[1]);
                    double scG = double.Parse(values[2]);
                    double scB = double.Parse(values[3]);

                    return Color.FromArgb
                        (
                            (byte)(scA * 255),
                            (byte)(scR * 255),
                            (byte)(scG * 255),
                            (byte)(scB * 255)
                        );
                }

                if (values.Length == 3)
                {
                    double scR = double.Parse(values[0].Substring(3));
                    double scG = double.Parse(values[1]);
                    double scB = double.Parse(values[2]);

                    return Color.FromArgb
                        (
                            255,
                            (byte)(scR * 255),
                            (byte)(scG * 255),
                            (byte)(scB * 255)
                        );
                }

                throw new FormatException
                    (
                        string.Format
                        (
                            "The {0} string passed in the colorString "
                            + "argument is not a recognized Color format "
                            + "(sc#[scA,]scR,scG,scB).", 
                            colorString
                        )
                    );
            }

            throw new FormatException
                (
                    string.Format
                    (
                        "The {0} string passed in the colorString argument "
                        + "is not a recognized Color.", 
                        colorString
                    )
                );
        }

        #endregion
    }
}
