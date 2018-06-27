// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FbDescription.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using AM;
using AM.Logging;
using AM.Text;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.FictionBook
{
    /// <summary>
    /// Заголовок, описывающий документ FictionBook.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FbDescription
    {
    }
}
