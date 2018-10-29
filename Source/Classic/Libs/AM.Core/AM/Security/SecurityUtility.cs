// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SecurityUtility.cs -- useful routines for X509 certificates
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Security.Cryptography.X509Certificates;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Security
{
    /// <summary>
    /// Collection of useful routines for X509 certificates.
    /// </summary>
    [PublicAPI]
    public static class SecurityUtility
    {
        #region Public methods

        /// <summary>
        /// Close the <see cref="X509Store"/>.
        /// </summary>
        public static void CloseStore
            (
                [NotNull] this X509Store store
            )
        {
            Code.NotNull(store, "store");

#if UAP
            ((IDisposable)store).Dispose();
#else
            store.Close();
#endif
        }

        /// <summary>
        /// Get certificate by the subject.
        /// </summary>
        [NotNull]
        public static X509Certificate GetRootCertificate
            (
                [NotNull] string subject
            )
        {
            Code.NotNullNorEmpty(subject, "subject");

            X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            foreach (X509Certificate2 certificate in store.Certificates)
            {
                if (certificate.Subject == subject)
                {
                    store.CloseStore();
                    return certificate;
                }
            }

            store.CloseStore();
            throw new Exception();
        }

        #endregion
    }
}