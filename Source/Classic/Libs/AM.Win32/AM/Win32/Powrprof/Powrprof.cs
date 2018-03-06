// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Powrprof.cs -- power profile management
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Prower profile management.
    /// </summary>
    [PublicAPI]
    public static class Powrprof
    {
        #region Constants

        /// <summary>
        /// DLL name.
        /// </summary>
        public const string DllName = "Powrprof.dll";

        #endregion

        #region Properties

        #endregion

        #region Interop

        /// <summary>
        /// Determines whether the computer supports hibernation.
        /// </summary>
        /// <returns>If the computer supports hibernation (power state S4) 
        /// and the file Hiberfil.sys is present on the system, the function 
        /// returns TRUE. Otherwise, the function returns FALSE.</returns>
        [DllImport(DllName, SetLastError = false)]
        public static extern bool IsPwrHibernateAllowed();

        /// <summary>
        /// Determines whether the computer supports the soft off power state.
        /// </summary>
        /// <returns>If the computer supports soft off (power state S5), 
        /// the function returns TRUE. Otherwise, the function returns FALSE.
        /// </returns>
        [DllImport(DllName, SetLastError = false)]
        public static extern bool IsPwrShutdownAllowed();

        /// <summary>
        /// Determines whether the computer supports the sleep states.
        /// </summary>
        /// <returns>If the computer supports the sleep states (S1, S2, 
        /// and S3), the function returns TRUE. Otherwise, the function 
        /// returns FALSE.</returns>
        [DllImport(DllName, SetLastError = false)]
        public static extern bool IsPwrSuspendAllowed();

        /// <summary>
        ///  Indicates the current state of the computer.
        /// </summary>
        /// <returns>If the system was restored to the working state 
        /// automatically and the user is not active, the function 
        /// returns TRUE. Otherwise, the function returns FALSE.</returns>
        [DllImport(DllName, SetLastError = false)]
        public static extern bool IsSystemResumeAutomatic();

        /// <summary>
        /// The SetSuspendState function suspends the system by shutting 
        /// power down. Depending on the Hibernate parameter, the system 
        /// either enters a suspend (sleep) state or hibernation (S4).
        /// </summary>
        /// <param name="hibernate">If this parameter is TRUE, 
        /// the system hibernates. If the parameter is FALSE, 
        /// the system is suspended.</param>
        /// <param name="forceCritical">If this parameter is TRUE, 
        /// the system suspends operation immediately; if it is FALSE, 
        /// the system broadcasts a PBT_APMQUERYSUSPEND event to each 
        /// application to request permission to suspend operation.</param>
        /// <param name="disableWakeEvent">If this parameter is TRUE, 
        /// the system disables all wake events. If the parameter is FALSE, 
        /// any system wake events remain enabled.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        /// <remarks>An application may use SetSuspendState to transition 
        /// the system from the working state to the standby (sleep), 
        /// or optionally, hibernate (S4) state. This function is similar 
        /// to the SetSystemPowerState function.</remarks>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetSuspendState
            (
                bool hibernate,
                bool forceCritical,
                bool disableWakeEvent
            );

        #endregion

        #region Public methods

        #endregion
    }
}