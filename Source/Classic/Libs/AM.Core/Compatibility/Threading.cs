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