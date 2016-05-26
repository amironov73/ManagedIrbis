/* SubFieldAttribute.cs -- отображение подполя на свойство
 * Ars Magna project, http://arsmagna.ru
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

namespace ManagedClient.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public sealed class SubFieldMapper
    {
        #region Properties

        #endregion

        #region Configuration

        #endregion

        #region Private members

        public static bool ToBoolean
            (
                [NotNull] SubField subField
            )
        {
            return string.IsNullOrEmpty(subField.Value);
        }

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

        public static DateTime ToDateTime
            (
                [NotNull] SubField subField
            )
        {
            return IrbisDate.ConvertStringToDate(subField.Value);
        }

        public static decimal ToDecimal
            (
                [NotNull] SubField subField
            )
        {
            decimal result;
            decimal.TryParse(subField.Value, out result);
            return result;
        }

        public static double ToDouble
            (
                [NotNull] SubField subField
            )
        {
            double result;
            double.TryParse(subField.Value, out result);
            return result;
        }

        public static int ToInt32
            (
                [NotNull] SubField subField
            )
        {
            int result;
            int.TryParse(subField.Value, out result);
            return result;
        }

        public static long ToInt64
            (
                [NotNull] SubField subField
            )
        {
            long result;
            long.TryParse(subField.Value, out result);
            return result;
        }

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
