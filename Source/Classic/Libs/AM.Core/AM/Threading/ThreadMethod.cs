/* ThreadMethod.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace AM.Threading
{
    /// <summary>
    /// Делегат для <see cref="ThreadRunner"/>.
    /// </summary>
    public delegate void ThreadMethod(object[] parameters);
}