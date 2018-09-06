// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LoginCenter.cs -- вход в ИРБИС
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Windows;

using CodeJam;

using ManagedIrbis;

#endregion

namespace AM.Windows.Irbis
{
    /// <summary>
    /// Вход в ИРБИС.
    /// </summary>
    public static class LoginCenter
    {
        #region Public methods

        /// <summary>
        /// Простой вход в сервер.
        /// </summary>
        public static bool Login
            (
                IIrbisConnection connection,
                Window owner
            )
        {
            Code.NotNull(connection, "connection");

            if (connection.Connected)
            {
                return true;
            }

            while(true)
            {
                LoginBox form = new LoginBox
                {
                    Owner = owner,
                    Username = connection.Username,
                    Password = connection.Password
                };
                form.Title = string.Format
                    (
                        "{0}: {1}",
                        form.Title,
                        connection.Host
                    );

                if (!ReferenceEquals(owner, null))
                {
                    form.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                }

                form.SetupFocus();

                if (form.ShowDialog() == false)
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(form.Username)
                    && !string.IsNullOrEmpty(form.Password))
                {
                    connection.Username = form.Username;
                    connection.Password = form.Password;

                    try
                    {
                        connection.Connect();
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    return true;
                }
            }
        }

        /// <summary>
        /// Расширенный вход в сервер (с указанием адреса).
        /// </summary>
        public static bool ExtendedLogin
            (
                IIrbisConnection connection,
                Window owner
            )
        {
            Code.NotNull(connection, "connection");

            if (connection.Connected)
            {
                return true;
            }

            while(true)
            {
                ExtendedLoginBox form = new ExtendedLoginBox
                {
                    Owner = owner,
                    Host = connection.Host,
                    Username = connection.Username,
                    Password = connection.Password
                };

                if (!ReferenceEquals(owner, null))
                {
                    form.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                }

                form.SetupFocus();

                if (form.ShowDialog() == false)
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(form.Host)
                    && !string.IsNullOrEmpty(form.Username)
                    && !string.IsNullOrEmpty(form.Password))
                {
                    connection.Host = form.Host;
                    connection.Username = form.Username;
                    connection.Password = form.Password;

                    try
                    {
                        connection.Connect();
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    return true;
                }
            }
        }

        #endregion
    }
}
