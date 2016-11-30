// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AuthentificationResult.cs -- 
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
using System.Xml.Serialization;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Authentication
{
    /// <summary>
    /// Credentials.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class AuthenticationResult
    {
        #region Properties

        /// <summary>
        /// Successfull?
        /// </summary>
        public bool Success { get; internal set; }

        /// <summary>
        /// Error message (if any).
        /// </summary>
        [CanBeNull]
        public string ErrorMessage { get; internal set; }

        #endregion
    }
}
