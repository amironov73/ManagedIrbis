/* RusMarcMetadataFormat.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

#endregion

namespace ManagedClient.Metadata
{
    /// <summary>
    /// Формат метаданных RusMarc.
    /// </summary>
    [PublicAPI]
    public abstract class RusMarcMetadataFormat
        : MarcMetadataFormat
    {
    }
}
