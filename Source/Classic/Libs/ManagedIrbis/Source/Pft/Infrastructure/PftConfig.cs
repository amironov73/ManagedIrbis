/* PftConfig.cs -- configuration for PFT scripting
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Configuration for PFT scripting
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class PftConfig
    {
        #region Constants

        /// <summary>
        /// Максимальное число повторений группы по умолчанию.
        /// </summary>
        public const int DefaultMaxRepeat = 500;

        #endregion

        #region Properties

        /// <summary>
        /// Максимальное число повторений группы.
        /// </summary>
        public static int MaxRepeat { get; set; }

        /// <summary>
        /// Выводить ли предупреждения?
        /// </summary>
        public static bool EnableWarnings { get; set; }

        #endregion

        #region Construction

        static PftConfig()
        {
            MaxRepeat = DefaultMaxRepeat;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
