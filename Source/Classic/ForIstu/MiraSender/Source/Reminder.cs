// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable ArrangeAccessorOwnerBody
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* Reminder.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.IO;
using AM.Istu.OldModel;
using AM.Logging;

using JetBrains.Annotations;

using ManagedIrbis;

using MiraInterop;
using RestSharp;

#endregion

namespace MiraSender
{
    internal static class Reminder
    {
        #region Private members

        /// <summary>
        /// Конфигурация.
        /// </summary>
        private static MiraConfiguration _config;

        private static string MiraJson()
        {
            var result = PathUtility.MapPath("mira.json");

            return result;
        }

        #endregion

        #region Public methods

        public static void LoadConfiguration()
        {
            Log.Trace("Reminder::LoadConfiguration: enter");

            _config = MiraConfiguration.LoadConfiguration(MiraJson());
            _config.Verify(true);

            Log.Trace("Reminder::LoadConfiguration: exit");
        }

        public static void DoWork()
        {

        }

        /// <summary>
        /// Создаём клиента для MSSQL.
        /// </summary>
        [NotNull]
        public static Kladovka GetKladovka()
        {
            var result = new Kladovka();

            return result;
        }

        /// <summary>
        /// Подключаемся к серверу ИРБИС64.
        /// </summary>
        [NotNull]
        public static IrbisConnection CreateConnection()
        {
            var result = new IrbisConnection(_config.IrbisConnectionString);

            return result;
        }

        /// <summary>
        /// Создаем клиента для MIRA.
        /// </summary>
        [NotNull]
        public static MiraClient GetClient()
        {
            return new MiraClient(_config);
        }

        #endregion
    }
}
