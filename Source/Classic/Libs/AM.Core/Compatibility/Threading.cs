// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Threading.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if WINMOBILE || PocketPC

namespace System.Threading
{
  public class Semaphore
  {
    public Semaphore() {}

    public Semaphore(int i1, int i2) {}

    public void WaitOne () {}

    public void Release () {}

    public void Close () {}
  }
}

#endif