// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisLoginCenter.cs -- вход в ИРБИС
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Globalization;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// Вход в ИРБИС.
    /// </summary>
    public static class IrbisLoginCenter
    {
        #region Public methods

        /// <summary>
        /// Простой вход в сервер.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static bool Login
            (
                [NotNull] this IrbisConnection connection,
                IWin32Window owner
            )
        {
            Code.NotNull(connection, "connection");

            using (IrbisLoginForm form = new IrbisLoginForm())
            {
                form.Username = connection.Username;
                form.Password = connection.Password;
                form.Text = string.Format
                    (
                        "{0} - {1}",
                        form.Text,
                        connection.Host
                    );

                if (ReferenceEquals(owner, null))
                {
                    form.StartPosition = FormStartPosition.CenterScreen;
                }

                DialogResult result = form.ShowDialog
                    (
                        owner
                    );
                if (result == DialogResult.OK)
                {
                    connection.Username = form.Username;
                    connection.Password = form.Password;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Вход в сервер с указанием адреса.
        /// </summary>
        public static bool Login2
            (
                [NotNull] this IrbisConnection connection,
                IWin32Window owner
            )
        {
            Code.NotNull(connection, "connection");

            using (IrbisLoginForm2 form = new IrbisLoginForm2())
            {
                form.Host = connection.Host;
                form.Port = connection.Port
                    .ToString(CultureInfo.InvariantCulture);
                form.Username = connection.Username;
                form.Password = connection.Password;
                form.Text = string.Format
                    (
                        "{0} - {1}",
                        form.Text,
                        connection.Host
                    );

                DialogResult result = form.ShowDialog
                    (
                        owner
                    );
                if (result == DialogResult.OK)
                {
                    connection.Host = form.Host;
                    connection.Port = int.Parse(form.Port);
                    connection.Username = form.Username;
                    connection.Password = form.Password;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Try to login.
        /// </summary>
        public static bool TryLogin
            (
                [NotNull] this IrbisConnection connection,
                IWin32Window owner
            )
        {
            Code.NotNull(connection, "connection");

            while (Login(connection, owner))
            {
                try
                {
                    connection.Connect();
                    if (connection.Connected)
                    {
                        return true;
                    }
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Try to login with host name.
        /// </summary>
        public static bool TryLogin2
            (
                [NotNull] this IrbisConnection connection,
                IWin32Window owner
            )
        {
            while (Login2(connection, owner))
            {
                try
                {
                    connection.Connect();
                    if (connection.Connected)
                    {
                        return true;
                    }
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                    return false;
                }
            }

            return false;
        }

        #endregion
    }
}
