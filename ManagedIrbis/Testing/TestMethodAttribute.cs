/* TestClassAttribute.cs --
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
