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
                form.UserName = connection.Username;
                form.UserPassword = connection.Password;
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
                    connection.Username = form.UserName;
                    connection.Password = form.UserPassword;
                    return true;
                }
            }

            return false;
        }

        ///// <summary>
        ///// Вход в сервер с указанием адреса.
        ///// </summary>
        ///// <param name="client"></param>
        ///// <param name="owner"></param>
        ///// <returns></returns>
        //public static bool Login2
        //    (
        //        this IrbisConnection client,
        //        IWin32Window owner
        //    )
        //{
        //    if (ReferenceEquals(client, null))
        //    {
        //        throw new ArgumentNullException("client");
        //    }

        //    using (IrbisLoginForm2 form = new IrbisLoginForm2())
        //    {
        //        form.Host = client.Host;
        //        form.Port = client.Port.ToString(CultureInfo.InvariantCulture);
        //        form.UserName = client.Username;
        //        form.UserPassword = client.Password;
        //        form.Text = string.Format
        //            (
        //                "{0} - {1}",
        //                form.Text,
        //                client.Host
        //            );

        //        DialogResult result = form.ShowDialog
        //            (
        //                owner
        //            );
        //        if (result == DialogResult.OK)
        //        {
        //            client.Host = form.Host;
        //            client.Port = int.Parse(form.Port);
        //            client.Username = form.UserName;
        //            client.Password = form.UserPassword;
        //            return true;
        //        }
        //    }

        //    return false;
        //}

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

        //public static bool TryLogin2
        //    (
        //        this IrbisConnection client,
        //        IWin32Window owner
        //    )
        //{
        //    while (Login2(client, owner))
        //    {
        //        try
        //        {
        //            client.Connect();
        //            if (client.Connected)
        //            {
        //                return true;
        //            }
        //        }
        //        // ReSharper disable once EmptyGeneralCatchClause
        //        catch
        //        {
        //            return false;
        //        }
        //    }

        //    return false;
        //}

        #endregion
    }
}
