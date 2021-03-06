﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AutoDisposeAttribute.cs -- mark instance field as auto-disposable 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// Mark instance field as auto-disposable.
    /// </summary>
    [PublicAPI]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class AutoDisposeAttribute
        : Attribute
    {
    }
}