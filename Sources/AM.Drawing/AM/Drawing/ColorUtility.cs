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
        /// <param name="color">Color value.</param>
        /// <returns></returns>
        public static int ToBgr
            (
                Color color
            )
        {
            return unchecked((((color.B << 8) + color.G) << 8) + color.R);
        }

        #endregion
    }
}
