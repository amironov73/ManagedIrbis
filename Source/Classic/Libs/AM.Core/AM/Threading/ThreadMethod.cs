// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ThreadMethod.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if CLASSIC

namespace AM.Threading
{
    /// <summary>
    /// Делегат для <see cref="ThreadRunner"/>.
    /// </summary>
    public delegate void ThreadMethod(object[] parameters);
}

#endif
