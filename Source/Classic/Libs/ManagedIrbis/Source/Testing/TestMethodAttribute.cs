// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TestClassAttribute.cs --
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

using ManagedIrbis;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Testing
{
    /// <summary>
    /// Attribute to mark test methods.
    /// </summary>
    [PublicAPI]
    public sealed class TestMethodAttribute
        : Attribute
    {
    }
}
