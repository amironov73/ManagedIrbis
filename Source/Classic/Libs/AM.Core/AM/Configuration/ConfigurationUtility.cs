// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConfigurationUtility.cs -- some useful routines for System.Configuration
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !NETCORE

#region Using directives

using System;
using System.Globalization;

using AM.Logging;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using CM = System.Configuration.ConfigurationManager;

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
                return string.Concat
                    (
                        RuntimeUtility.ExecutableFileName,
                        ".config"
                    );
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
            foreach (string candidate in candidates.NonEmptyLines())
            {
                string setting = CM.AppSettings[candidate];
                if (!string.IsNullOrEmpty(setting))
                {
                    return setting;
                }
            }
            return null;
        }

        /// <summary>
        /// Get boolean value from application configuration.
        /// </summary>
        public static bool GetBoolean
            (
                [NotNull] string key,
                bool defaultValue = false
            )
        {
            string s = CM.AppSettings[key];
            if (!string.IsNullOrEmpty(s))
            {
                defaultValue = ConversionUtility.ToBoolean(s);
            }
            return defaultValue;
        }

        /// <summary>
        /// Get 16-bit integer value from application configuration.
        /// </summary>
        public static short GetInt16
            (
                [NotNull] string key,
                short defaultValue = 0
            )
        {
            string s = CM.AppSettings[key];
            if (!string.IsNullOrEmpty(s))
            {
                defaultValue = short.Parse
                    (
                        s,
                        _FormatProvider
                    );
            }

            return defaultValue;
        }

        /// <summary>
        /// Get unsigned 16-bit integer.
        /// </summary>
        [CLSCompliant(false)]
        public static ushort GetUInt16
            (
                [NotNull] string key,
                ushort defaultValue = 0
            )
        {
            string s = CM.AppSettings[key];
            if (!string.IsNullOrEmpty(s))
            {
                defaultValue = ushort.Parse
                    (
                        s,
                        _FormatProvider
                    );
            }

            return defaultValue;
        }

        /// <summary>
        /// Get 32-bit integer value
        /// from application configuration.
        /// </summary>
        public static int GetInt32
            (
                [NotNull] string key,
                int defaultValue = 0
            )
        {
            string s = CM.AppSettings[key];
            if (!string.IsNullOrEmpty(s))
            {
                defaultValue = int.Parse
                    (
                        s,
                        _FormatProvider
                    );
            }
            return defaultValue;
        }

        /// <summary>
        /// Get unsigned 32-bit integer value
        /// from application configuration.
        /// </summary>
        [CLSCompliant (false)]
        public static uint GetUInt32
            (
                [NotNull] string key,
                uint defaultValue = 0U
            )
        {
            string s = CM.AppSettings[key];
            if (!string.IsNullOrEmpty(s))
            {
                defaultValue = uint.Parse
                    (
                        s,
                        _FormatProvider
                    );
            }

            return defaultValue;
        }

        /// <summary>
        /// Get 64-bit integer value
        /// from application configuration.
        /// </summary>
        public static long GetInt64
            (
                [NotNull] string key,
                long defaultValue = 0L
            )
        {
            string s = CM.AppSettings[key];
            if (!string.IsNullOrEmpty(s))
            {
                defaultValue = long.Parse
                    (
                        s,
                        _FormatProvider
                    );
            }
            return defaultValue;
        }

        /// <summary>
        /// Get usingned 64-bit integer value
        /// from application configuration.
        /// </summary>
        [CLSCompliant(false)]
        public static ulong GetUInt64
            (
                [NotNull] string key,
                ulong defaultValue = 0UL
            )
        {
            string s = CM.AppSettings[key];
            if (!string.IsNullOrEmpty(s))
            {
                defaultValue = ulong.Parse
                    (
                        s,
                        _FormatProvider
                    );
            }

            return defaultValue;
        }

        /// <summary>
        /// Get single-precision float value from application configuration.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float GetSingle
            (
                [NotNull] string key,
                float defaultValue = 0.0f
            )
        {
            string s = CM.AppSettings[key];
            if (!string.IsNullOrEmpty(s))
            {
                defaultValue = float.Parse
                    (
                        s,
                        _FormatProvider
                    );
            }
            return defaultValue;
        }

        /// <summary>
        /// Get double-precision float value from application configuration.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double GetDouble
            (
                [NotNull] string key,
                double defaultValue = 0.0
            )
        {
            string s = CM.AppSettings[key];
            if (!string.IsNullOrEmpty(s))
            {
                defaultValue = double.Parse
                    (
                        s,
                        _FormatProvider
                    );
            }
            return defaultValue;
        }

        /// <summary>
        /// Get decimal value from application configuration.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal GetDecimal
            (
                [NotNull] string key,
                decimal defaultValue = 0.0m
            )
        {
            string s = CM.AppSettings[key];
            if (!string.IsNullOrEmpty(s))
            {
                defaultValue = decimal.Parse
                    (
                        s,
                        _FormatProvider
                    );
            }
            return defaultValue;
        }

        /// <summary>
        /// Get string value from application configuration.
        /// </summary>
        [CanBeNull]
        public static string GetString
            (
                [NotNull] string key,
                [CanBeNull] string defaultValue = null
            )
        {
            Code.NotNullNorEmpty(key, "key");

            string s = CM.AppSettings[key];
            if (!string.IsNullOrEmpty(s))
            {
                defaultValue = s;
            }

            return defaultValue;
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

            string result = GetString(key);

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
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime GetDateTime
            (
                [NotNull] string key,
                DateTime defaultValue
            )
        {
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
        }

        #endregion
    }
}

#endif
