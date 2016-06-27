/* VerificationException.cs -- exception for IVerifiable interface.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// Exception for <see cref="IVerifiable"/> interface.
    /// </summary>
    [PublicAPI]
    [Serializable]

    public sealed class VerificationException
        : ApplicationException
    {
    }
}
