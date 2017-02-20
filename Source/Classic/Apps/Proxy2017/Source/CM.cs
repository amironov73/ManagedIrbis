// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CM.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Proxy2017
{
    /// <summary>
    /// Простой доступ к настройкам из App.config
    /// </summary>
    static class CM
    {
        public static string GetString
            (
                string name,
                string defaultValue
            )
        {
            string result = ConfigurationManager.AppSettings[name];
            if (string.IsNullOrEmpty(result))
            {
                result = defaultValue;
            }

            return result;
        }

        // ====================================================================

        public static int GetInt32
            (
                string name,
                int defaultValue
            )
        {
            string textValue = ConfigurationManager.AppSettings[name];
            if (string.IsNullOrEmpty(textValue))
            {
                return defaultValue;
            }

            int result;
            if (!int.TryParse(textValue, out result))
            {
                return defaultValue;
            }

            return result;
        }

        // ====================================================================

        public static bool GetBoolean
            (
                string name,
                bool defaultValue
            )
        {
            string textValue = ConfigurationManager.AppSettings[name];
            if (string.IsNullOrEmpty(textValue))
            {
                return defaultValue;
            }

            bool result;
            if (!bool.TryParse(textValue, out result))
            {
                return defaultValue;
            }

            return result;
        }

        // ====================================================================

        public static IPAddress GetAddress
            (
                string name,
                IPAddress defaultValue
            )
        {
            string textValue = ConfigurationManager.AppSettings[name];
            if (string.IsNullOrEmpty(textValue))
            {
                return defaultValue;
            }

            //Dns.GetHostAddresses(textValue);

            IPAddress result;
            if (!IPAddress.TryParse(textValue, out result))
            {
                return defaultValue;
            }

            return result;
        }
    }
}
