/* IrbisMenuUtility.cs -- MNU file extended handling.
 * Ars Magna project, http://arsmagna.ru
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

#endregion

namespace ManagedClient.Menus
{
    /// <summary>
    /// MNU file extended handling.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class IrbisMenuUtility
    {
        #region Public methods

        /// <summary>
        /// Adds the typed value with specified code.
        /// </summary>
        public static IrbisMenuFile Add<T>
            (
                [NotNull] this IrbisMenuFile menu,
                [NotNull] string code,
                [CanBeNull] T value
            )
        {
            Code.NotNull(menu, "menu");
            Code.NotNull(code, "code");

            if (ReferenceEquals(value, null))
            {
                menu.Add(code, string.Empty);
            }
            else
            {
                string textValue =
                    ConversionUtility
                        .ConvertTo<string>(value);
                menu.Add(code, textValue);
            }

            return menu;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        [CanBeNull]
        public static T GetValue<T>
            (
                [NotNull] this IrbisMenuFile menu,
                [NotNull] string code,
                [CanBeNull] T defaultValue
            )
        {
            Code.NotNull(menu, "menu");
            Code.NotNull(code, "code");

            IrbisMenuFile.Entry found = menu.FindEntry(code);

            return found == null
                ? defaultValue
                : ConversionUtility.ConvertTo<T>(found.Comment);
        }

        /// <summary>
        /// Gets the value (case sensitive).
        /// </summary>
        [CanBeNull]
        public static T GetValueSensitive<T>
            (
                [NotNull] this IrbisMenuFile menu,
                [NotNull] string code,
                T defaultValue
            )
        {
            Code.NotNull(menu, "menu");
            Code.NotNull(code, "code");

            IrbisMenuFile.Entry found = menu.FindEntrySensitive(code);

            return found == null
                ? defaultValue
                : ConversionUtility.ConvertTo<T>(found.Comment);
        }

        /// <summary>
        /// Collects the comments for code.
        /// </summary>
        [NotNull]
        public static string[] CollectStrings
            (
                [NotNull] this IrbisMenuFile menu,
                [NotNull] string code
            )
        {
            return menu.Entries
                .Where
                    (
                        entry => entry.Code.SameString(code)
                        || IrbisMenuFile.TrimCode(entry.Code)
                            .SameString(code)
                    )
                .Select(entry => entry.Comment)
                .ToArray();
        }



        #endregion
    }
}
