/* MarcMetadataFormat.cs --
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

namespace ManagedIrbis.Metadata
{
    /// <summary>
    /// Основанный на MARC формат метаданных.
    /// </summary>
    [PublicAPI]
    public abstract class MarcMetadataFormat
        : AbstractMetadataFormat
    {
    }
}
