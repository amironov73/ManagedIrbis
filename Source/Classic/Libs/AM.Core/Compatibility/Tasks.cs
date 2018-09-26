/* Tasks.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if WINMOBILE || PocketPC

using System;

#pragma warning disable 1591
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable IntroduceOptionalParameters.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable InconsistentNaming

namespace System.Threading
{
    public class CancellationTokenSource : IDisposable
    {
        public bool IsCancellationRequested { get { return false; } }

        public CancellationToken Token { get { return new CancellationToken(); } }

        public void Cancel()
        {
        }

        public void Dispose()
        {
        }
    }

    public struct CancellationToken
    {
        public bool IsCancellationRequested { get { return true; } }

        public bool CanBeCanceled { get { return true; } }
    }
}

//namespace System.Threading.Tasks
//{
    ///// <summary>
    /////
    ///// </summary>
    //public sealed class Task
    //{
    //    public Task()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task(Action action)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Start()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static void WaitAll
    //        (
    //            Task[] tasks
    //        )
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
//}

#endif
