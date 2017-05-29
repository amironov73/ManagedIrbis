// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

// ReSharper disable ConvertClosureToMethodGroup

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

        #region Public methods

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
        /// Преобразование в булево значение.
        /// </summary>
        public static bool ToBoolean
            (
                [NotNull] RecordField field,
                char code
            )
        {
            bool result = false;
            SubField subField = field.GetFirstSubField(code);
            if (!ReferenceEquals(subField, null))
            {
                result = ToBoolean(subField);
            }

            return result;
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
        /// Преобразование в дату.
        /// </summary>
        [CanBeNull]
        public static DateTime? ToDateTime
            (
                [NotNull] RecordField field,
                char code
            )
        {
            DateTime? result = null;
            SubField subField = field.GetFirstSubField(code);
            if (!ReferenceEquals(subField, null))
            {
                result = ToDateTime(subField);
            }

            return result;
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

            NumericUtility.TryParseDecimal(subField.Value, out result);

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

            NumericUtility.TryParseDouble(subField.Value, out result);

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

            NumericUtility.TryParseFloat(subField.Value, out result);

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

            NumericUtility.TryParseInt16(subField.Value, out result);

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

            NumericUtility.TryParseInt32(subField.Value, out result);

            return result;
        }

        /// <summary>
        /// Преобразование в 32-битное целое со знаком.
        /// </summary>
        public static int? ToInt32
            (
                [NotNull] RecordField field,
                char code
            )
        {
            int? result = null;
            SubField subField = field.GetFirstSubField(code);
            if (!ReferenceEquals(subField, null))
            {
                result = ToInt32(subField);
            }

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

            NumericUtility.TryParseInt64(subField.Value, out result);

            return result;
        }

        /// <summary>
        /// Преобразование в 64-битное целое со знаком.
        /// </summary>
        public static long ToInt64
            (
                [NotNull] RecordField field,
                char code
            )
        {
            long result = 0;
            SubField subField = field.GetFirstSubField(code);
            if (!ReferenceEquals(subField, null))
            {
                result = ToInt64(subField);
            }

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
