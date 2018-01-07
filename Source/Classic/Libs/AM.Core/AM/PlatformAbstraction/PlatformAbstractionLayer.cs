// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PlatformAbstractionLayer.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.PlatformAbstraction
{
    /// <summary>
    /// Platform abstraction level.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PlatformAbstractionLayer
        : IDisposable
    {
        #region Public methods

        /// <summary>
        /// Exit.
        /// </summary>
        public virtual void Exit
            (
                int exitCode
            )
        {
#if UAP

            Windows.ApplicationModel.Core.CoreApplication.Exit();

#elif WINMOBILE || PocketPC

            throw new NotImplementedException();

#else

            Environment.Exit(exitCode);

#endif
        }

        /// <summary>
        /// Fail fast.
        /// </summary>
        public virtual void FailFast
            (
                [NotNull] string message
            )
        {
#if PocketPC || WINMOBILE

            throw new NotImplementedException();

#else

            Environment.FailFast(message);

#endif
        }

        /// <summary>
        /// Get environment variable.
        /// </summary>
        [CanBeNull]
        public virtual string GetEnvironmentVariable
            (
                [NotNull] string variableName
            )
        {
#if PocketPC || WINMOBILE

            return null;

#else

            return Environment.GetEnvironmentVariable(variableName);

#endif
        }

        /// <summary>
        /// Get the machine name.
        /// </summary>
        [NotNull]
        public virtual string GetMachineName()
        {
#if WINMOBILE || PocketPC || UAP

            return "MACHINE";

#else

            return Environment.MachineName;

#endif
        }

        /// <summary>
        /// Get random number generator.
        /// </summary>
        public virtual Random GetRandomGenerator()
        {
            return new Random();
        }

        /// <summary>
        /// Get current date and time.
        /// </summary>
        public virtual DateTime Now()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// Get the operating system version.
        /// </summary>
        public virtual OperatingSystem OsVersion()
        {
#if UAP
            // TODO implement properly

            Version windows10 = new Version(10, 0, 16299, 0);
            OperatingSystem result = new OperatingSystem
                (
                    PlatformID.Win32NT,
                    windows10
                );

            return result;

#else

            return Environment.OSVersion;

#endif
        }

        /// <summary>
        /// Get today date.
        /// </summary>
        public virtual DateTime Today()
        {
            return DateTime.Today;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            // Nothing to do here
        }

        #endregion
    }
}
