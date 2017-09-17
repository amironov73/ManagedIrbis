// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NotImplemented exception.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if SILVERLIGHT

namespace System
{
    public class NotImplementedException
        : Exception
    {
        public NotImplementedException ()
        {
        }

        public NotImplementedException (string message)
        {
        }
    }
}

#endif
