/* RfidFactory.cs --
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

#endregion

namespace AM.Rfid
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class RfidFactory
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Static constructor.
        /// </summary>
        static RfidFactory()
        {
            _registry = new Dictionary<string, Type>();
            _drivers = new Dictionary<string, RfidDriver>();
        }

        #endregion

        #region Private members

        private static readonly Dictionary<string, Type> _registry;

        private static readonly Dictionary<string, RfidDriver> _drivers;

        #endregion

        #region Public methods

        /// <summary>
        /// Inventory.
        /// </summary>
        [NotNull]
        public static string[] Inventory()
        {
            List<string> result = new List<string>();

            foreach (var pair in _drivers)
            {
                RfidDriver driver = pair.Value;
                string[] inventory = driver.Inventory();
                result.AddRange(inventory);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Select.
        /// </summary>
        public static void Select
            (
                [NotNull] string uid
            )
        {
            foreach (RfidDriver driver in _drivers.Values)
            {
                if ((driver.Capabilities & RfidCapabilities.Select) != 0)
                {
                    driver.Select(uid);
                }
            }
        }

        /// <summary>
        /// Set AFI.
        /// </summary>
        public static void SetAFI
            (
                string uid,
                int afi
            )
        {
            foreach (RfidDriver driver in _drivers.Values)
            {
                if ((driver.Capabilities & RfidCapabilities.AFI) != 0)
                {
                    driver.SetAFI
                        (
                            uid, 
                            afi
                        );
                }
            }
        }

        /// <summary>
        /// Set EAS.
        /// </summary>
        public static void SetEas
            (
                string uid,
                bool flag
            )
        {
            foreach (RfidDriver driver in _drivers.Values)
            {
                if ((driver.Capabilities & RfidCapabilities.EAS) != 0)
                {
                    driver.SetEAS(uid, flag);
                }
            }
        }

        /// <summary>
        /// Register driver.
        /// </summary>
        public static void RegisterDriver
            (
                string name,
                Type type
            )
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (ReferenceEquals(type, null))
            {
                throw new ArgumentNullException("type");
            }
            if (!type.IsSubclassOf(typeof (RfidDriver)))
            {
                throw new ArgumentOutOfRangeException("type");
            }

            _registry[name] = type;
        }

        /// <summary>
        /// Start driver.
        /// </summary>
        public static RfidDriver StartDriver
            (
                string name,
                object connectionData
            )
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (_drivers.ContainsKey(name))
            {
                return _drivers[name];
            }

            Type type = _registry[name];
            RfidDriver driver = (RfidDriver)Activator.CreateInstance(type);
            driver.Connect(connectionData);
            _drivers.Add(name, driver);
            return driver;
        }

        /// <summary>
        /// Stop all drivers.
        /// </summary>
        public static void StopAllDrivers()
        {
            foreach (RfidDriver driver in _drivers.Values)
            {
                driver.Disconnect();
            }
            _drivers.Clear();
        }

        #endregion
    }
}
