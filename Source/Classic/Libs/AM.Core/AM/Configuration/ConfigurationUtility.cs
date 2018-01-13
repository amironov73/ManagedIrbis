// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConfigurationUtility.cs -- some useful routines for System.Configuration
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */


#region Using directives

using System;
using System.Globalization;

using AM.Logging;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#if WINMOBILE || PocketPC

using CM = OpenNETCF.Configuration.ConfigurationSettings;

#elif !DROID && !ANDROID && !UAP

using CM = System.Configuration.ConfigurationManager;

#endif

#endregion

namespace AM.Configuration
{
    /// <summary>
    /// Some useful routines for System.Configuration.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ConfigurationUtility
    {
        #region Private members

        private static IFormatProvider _FormatProvider
        {
            get
            {
                return CultureInfo.InvariantCulture;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Application.exe.config file name with full path.
        /// </summary>
        [NotNull]
        public static string ConfigFileName
        {
            get
            {
#if UAP

                // TODO Implement properly

                return "config";

#else

                return string.Concat
                    (
                        RuntimeUtility.ExecutableFileName,
                        ".config"
                    );

#endif
            }
        }

        /// <summary>
        /// Получаем сеттинг из возможных кандидатов.
        /// </summary>
        [CanBeNull]
        public static string FindSetting
            (
                [NotNull] params string[] candidates
            )
        {
#if !DROID && !ANDROID && !UAP

            foreach (string candidate in candidates.NonEmptyLines())
            {
                string setting = CM.AppSettings[candidate];
                if (!string.IsNullOrEmpty(setting))
                {
                    return setting;
                }
            }

#endif

            return null;
        }

        /// <summary>
        /// Get boolean value from application configuration.
        /// </summary>
        public static bool GetBoolean
            (
                [NotNull] string key,
                bool defaultValue
            )
        {
#if DROID || ANDROID || UAP

            return defaultValue;
#else

            bool result = defaultValue;
            string s = CM.AppSettings[key];
            if (!string.IsNullOrEmpty(s))
            {
                result = ConversionUtility.ToBoolean(s);
            }

            return result;

#endif
        }

        /// <summary>
        /// Get 16-bit integer value from application configuration.
        /// </summary>
        public static short GetInt16
            (
                [NotNull] string key,
                short defaultValue
            )
        {
#if DROID || ANDROID || UAP

            return defaultValue;
#else

            short result;
            string s = CM.AppSettings[key];

            if (!NumericUtility.TryParseInt16(s, out result))
            {
                result = defaultValue;
            }

            return result;

#endif
        }

        /// <summary>
        /// Get unsigned 16-bit integer.
        /// </summary>
        [CLSCompliant(false)]
        public static ushort GetUInt16
            (
                [NotNull] string key,
                ushort defaultValue
            )
        {
#if DROID || ANDROID || UAP

            return defaultValue;
#else

            ushort result;
            string s = CM.AppSettings[key];

            if (!NumericUtility.TryParseUInt16(s, out result))
            {
                result = defaultValue;
            }

            return result;

#endif
        }

        /// <summary>
        /// Get 32-bit integer value
        /// from application configuration.
        /// </summary>
        public static int GetInt32
            (
                [NotNull] string key,
                int defaultValue
            )
        {
#if DROID || ANDROID || UAP

            return defaultValue;
#else

            int result;
            string s = CM.AppSettings[key];

            if (!NumericUtility.TryParseInt32(s, out result))
            {
                result = defaultValue;
            }

            return result;

#endif
        }

        /// <summary>
        /// Get unsigned 32-bit integer value
        /// from application configuration.
        /// </summary>
        [CLSCompliant (false)]
        public static uint GetUInt32
            (
                [NotNull] string key,
                uint defaultValue
            )
        {
#if DROID || ANDROID || UAP

            return defaultValue;
#else

            uint result;
            string s = CM.AppSettings[key];

            if (!NumericUtility.TryParseUInt32(s, out result))
            {
                result = defaultValue;
            }

            return result;

#endif
        }

        /// <summary>
        /// Get 64-bit integer value
        /// from application configuration.
        /// </summary>
        public static long GetInt64
            (
                [NotNull] string key,
                long defaultValue
            )
        {
#if DROID || ANDROID || UAP

            return defaultValue;
#else

            long result;
            string s = CM.AppSettings[key];

            if (!NumericUtility.TryParseInt64(s, out result))
            {
                result = defaultValue;
            }

            return result;

#endif
        }

        /// <summary>
        /// Get usingned 64-bit integer value
        /// from application configuration.
        /// </summary>
        [CLSCompliant(false)]
        public static ulong GetUInt64
            (
                [NotNull] string key,
                ulong defaultValue
            )
        {
#if DROID || ANDROID || UAP

            return defaultValue;
#else

            ulong result;
            string s = CM.AppSettings[key];

            if (!NumericUtility.TryParseUInt64(s, out result))
            {
                result = defaultValue;
            }

            return result;

#endif
        }

        /// <summary>
        /// Get single-precision float value from application configuration.
        /// </summary>
        public static float GetSingle
            (
                [NotNull] string key,
                float defaultValue
            )
        {
#if DROID || ANDROID || UAP

            return defaultValue;
#else

            float result;
            string s = CM.AppSettings[key];

            if (!NumericUtility.TryParseFloat(s, out result))
            {
                result = defaultValue;
            }

            return result;

#endif
        }

        /// <summary>
        /// Get double-precision float value from application configuration.
        /// </summary>
        public static double GetDouble
            (
                [NotNull] string key,
                double defaultValue
            )
        {
#if DROID || ANDROID || UAP

            return defaultValue;
#else

            double result;
            string s = CM.AppSettings[key];

            if (!NumericUtility.TryParseDouble(s, out result))
            {
                result = defaultValue;
            }

            return result;

#endif
        }

        /// <summary>
        /// Get decimal value from application configuration.
        /// </summary>
        public static decimal GetDecimal
            (
                [NotNull] string key,
                decimal defaultValue
            )
        {
#if DROID || ANDROID || UAP

            return defaultValue;
#else

            decimal result;
            string s = CM.AppSettings[key];

            if (!NumericUtility.TryParseDecimal(s, out result))
            {
                result = defaultValue;
            }

            return result;

#endif
        }

        /// <summary>
        /// Get string value from application configuration.
        /// </summary>
        [CanBeNull]
        public static string GetString
            (
                [NotNull] string key,
                [CanBeNull] string defaultValue
            )
        {
            Code.NotNullNorEmpty(key, "key");

#if DROID || ANDROID || UAP

            return defaultValue;
#else

            string result = defaultValue;
            string s = CM.AppSettings[key];

            if (!string.IsNullOrEmpty(s))
            {
                result = s;
            }

            return result;

#endif
        }

        /// <summary>
        /// Get string value from application configuration.
        /// </summary>
        [CanBeNull]
        public static string GetString
            (
                [NotNull] string key
            )
        {
            return GetString(key, null);
        }

        /// <summary>
        /// Get string value from application configuration.
        /// </summary>
        [NotNull]
        public static string RequireString
            (
                [NotNull] string key
            )
        {
            Code.NotNullNorEmpty(key, "key");

            string result = GetString(key, null);

            if (ReferenceEquals(result, null))
            {
                Log.Error
                    (
                        "ConfigurationUtility::RequireString: "
                        + "key '"
                        + key
                        + "' not set"
                    );

                throw new ArgumentNullException
                    (
                        "configuration key '" + key + "' not set"
                    );
            }

            return result;
        }

        /// <summary>
        /// Get date or time value from application configuration.
        /// </summary>
        public static DateTime GetDateTime
            (
                [NotNull] string key,
                DateTime defaultValue
            )
        {
#if DROID || ANDROID || UAP

            return defaultValue;
#else

            string s = CM.AppSettings[key];

            if (!string.IsNullOrEmpty(s))
            {
                defaultValue = DateTime.Parse
                    (
                        s,
                        _FormatProvider
                    );
            }

            return defaultValue;

#endif
        }

        #endregion
    }
}

