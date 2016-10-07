/* SubFieldAttribute.cs -- отображение подполя на свойство
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SubFieldMapper
    {
        #region Properties

        #endregion

        #region Configuration

        #endregion

        #region Private members

        /// <summary>
        /// Преобразование в булево значение.
        /// </summary>
        public static bool ToBoolean
            (
                [NotNull] SubField subField
            )
        {
            return string.IsNullOrEmpty(subField.Value);
        }

        /// <summary>
        /// Преобразование в символ.
        /// </summary>
        public static char ToChar
            (
                [NotNull] SubField subField
            )
        {
            string text = subField.Value;
            return string.IsNullOrEmpty(text)
                ? '\0'
                : text[0];
        }

        /// <summary>
        /// Преобразование в дату.
        /// </summary>
        public static DateTime ToDateTime
            (
                [NotNull] SubField subField
            )
        {
            return IrbisDate.ConvertStringToDate(subField.Value);
        }

        /// <summary>
        /// Преобразование в число с фиксированной точкой.
        /// </summary>
        public static decimal ToDecimal
            (
                [NotNull] SubField subField
            )
        {
            decimal result;

#if !WINMOBILE && !PocketPC

            decimal.TryParse(subField.Value, out result);

#else

            result = decimal.Parse(subField.Value);

#endif

            return result;
        }

        /// <summary>
        /// Преобразование в число с плавающей точкой
        /// двойной точностью.
        /// </summary>
        public static double ToDouble
            (
                [NotNull] SubField subField
            )
        {
            double result;

#if !WINMOBILE && !PocketPC

            double.TryParse(subField.Value, out result);

#else

            result = double.Parse(subField.Value);

#endif

            return result;
        }

        /// <summary>
        /// Преобразование в число с плавающей точкой
        /// одинарной точности.
        /// </summary>
        public static float ToSingle
            (
                [NotNull] SubField subField
            )
        {
            float result;

#if !WINMOBILE && !PocketPC

            float.TryParse(subField.Value, out result);

#else

            result = float.Parse(subField.Value);

#endif

            return result;
        }

        /// <summary>
        /// Преобразование в 16-битное целое со знаком.
        /// </summary>
        public static short ToInt16
            (
                [NotNull] SubField subField
            )
        {
            short result;

#if !WINMOBILE && !PocketPC

            short.TryParse(subField.Value, out result);

#else
            result = short.Parse(subField.Value);

#endif

            return result;
        }

        /// <summary>
        /// Преобразование в 32-битное целое со знаком.
        /// </summary>
        public static int ToInt32
            (
                [NotNull] SubField subField
            )
        {
            int result;

#if !WINMOBILE && !PocketPC

            int.TryParse(subField.Value, out result);

#else

            result = int.Parse(subField.Value);

#endif

            return result;
        }

        /// <summary>
        /// Преобразование в 64-битное целое со знаком.
        /// </summary>
        public static long ToInt64
            (
                [NotNull] SubField subField
            )
        {
            long result;

#if !WINMOBILE && !PocketPC

            long.TryParse(subField.Value, out result);

#else
            result = long.Parse(subField.Value);

#endif

            return result;
        }

        /// <summary>
        /// Преобразование в строку (тривиальное).
        /// </summary>
        [CanBeNull]
        public static string ToString
            (
                [NotNull] SubField subField
            )
        {
            return subField.Value;
        }

        #endregion
    }
}
