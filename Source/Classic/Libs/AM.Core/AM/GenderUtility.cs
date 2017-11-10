// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GenderUtility.cs -- для работы с перечислением Gender.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: good
 */

#region Using directives

using System;

using AM.Logging;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// Для работы с перечислением <see cref="Gender"/>.
    /// </summary>
    public static class GenderUtility
    {
        #region Public methods

        /// <summary>
        /// Разбор строки.
        /// </summary>
        public static Gender Parse
            (
                [CanBeNull] string text
            )
        {
            switch (text)
            {
                case "М":
                case "м":
                case "МУЖ":
                case "муж":
                case "МУЖСКОЙ":
                case "мужской":
                case "M":
                case "m":
                case "MALE":
                case "Male":
                case "male":
                    return Gender.Male;

                case "Ж":
                case "ж":
                case "ЖЕН":
                case "жен":
                case "ЖЕНСКИЙ":
                case "женский":
                case "F":
                case "f":
                case "FEMALE":
                case "Female":
                case "female":
                    return Gender.Female;

                default:
                    return Gender.NotSet;
            }
        }

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        [NotNull]
        public static string ToString
            (
                Gender gender
            )
        {
            switch (gender)
            {
                case Gender.NotSet:
                    return "н/д";

                case Gender.Male:
                    return "мужской";

                case Gender.Female:
                    return "женский";

                default:
                    Log.Error
                        (
                            "GenderUtility::ToString: "
                            + "gender="
                            + gender
                        );

                    throw new ArgumentOutOfRangeException
                        (
                            "gender",
                            gender,
                            null
                        );
            }
        }

        /// <summary>
        /// Проверка.
        /// </summary>
        public static bool Verify
            (
                Gender gender,
                bool throwOnError
            )
        {
            switch (gender)
            {
                case Gender.NotSet:
                case Gender.Male:
                case Gender.Female:
                    return true;

                default:
                    Log.Error
                        (
                            "GenderUtility::Verify: "
                            + "gender="
                            + gender
                        );

                    if (throwOnError)
                    {
                        throw new ArgumentOutOfRangeException
                            (
                                "gender",
                                gender,
                                null
                            );
                    }

                    return false;
            }
        }

        #endregion
    }
}
