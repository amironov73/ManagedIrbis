// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LoginCenter.cs -- вход в ИРБИС
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using Windows.UI.Xaml.Controls;

using CodeJam;

using ManagedIrbis;

#endregion

namespace IrbisUI
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
                IIrbisConnection connection
            )
        {
            Code.NotNull(connection, nameof(connection));

            if (connection.Connected)
            {
                return true;
            }

            while(true)
            {
                LoginBox form = new LoginBox
                {
                    Username = connection.Username,
                    Password = connection.Password
                };
                form.Title = string.Format
                    (
                        "{0}: {1}",
                        form.Title,
                        connection.Host
                    );

                form.SetupFocus();

                var result = form.ShowAsync().GetResults();

                if (result != ContentDialogResult.Primary)
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

        #endregion
    }
}
