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

using AM;
using AM.IO;
using AM.Istu.OldModel;
using AM.Logging;
using AM.Text.Output;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

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

        private static string[] _preselected;

        private static string MiraJson()
        {
            var result = PathUtility.MapPath("mira.json");

            return result;
        }

        #endregion

        #region Public methods

        public static void SetPreselected
            (
                string[] preselected
            )
        {
            _preselected = preselected;
        }

        public static void LoadConfiguration()
        {
            Log.Trace("Reminder::LoadConfiguration: enter");

            _config = MiraConfiguration.LoadConfiguration(MiraJson());
            _config.Verify(true);

            Log.Trace("Reminder::LoadConfiguration: exit");
        }

        public static void DoWork()
        {
            Log.Trace("Reminder::DoWork: enter");

            var counter = 0;
            var rest = new RestClient(_config.BaseUri);
            var mira = GetClient();
            var kladovka = GetKladovka();
            var people = kladovka.Readers.Where
                (
                    r => !r.Blocked
                         && r.IstuID != 0
                         && r.Registered != null
                         && !r.Barcode.StartsWith("!")
                )
                .Select(r => new { r.Ticket, r.IstuID})
                .ToArray();

            Log.Info("Tickets with MIRA = " + people.Length );

            if (!ReferenceEquals(_preselected, null))
            {
                people = people
                    .Where(t => _preselected.ContainsNoCase(t.Ticket))
                    .ToArray();

                Log.Info("Filtered tickets = " + people.Length);
            }

            foreach (var pupil in people)
            {
                var ticket = pupil.Ticket;

                // научный фонд
                var sciFond = kladovka.Podsob.Count
                    (
                        book => (book.Ticket == ticket || book.OnHand == ticket)
                            && book.Deadline < DateTime.Now
                    );

                // учебный фонд
                var uchFond = kladovka.Ucheb.Count
                    (
                        book => (book.Ticket == ticket || book.OnHand == ticket)
                            && book.Deadline < DateTime.Now
                    );

                // художка
                var hudFond = kladovka.Hudo.Count
                    (
                        book => book.Ticket == ticket && book.Deadline < DateTime.Now
                    );

                // журналы
                var perio = kladovka.Perio.Count
                    (
                        issue => (issue.Ticket == ticket || issue.OnHand == ticket)
                            && issue.Deadline < DateTime.Now
                    );

                var total = sciFond + uchFond + hudFond + perio;

                if (total != 0)
                {
                    ++counter;
                    var request = mira.CreateRequest();
                    var miraid = pupil.IstuID.ToInvariantString();
                    request.AddParameter("miraid", miraid);
                    request.AddParameter("title", _config.Title);
                    var message = _config.Message.Replace
                        (
                            "{bookCount}",
                            total.ToInvariantString()
                        );
                    request.AddParameter("text", message);
                    var response = rest.Execute(request);

                    Log.Info($"Ticket={ticket}, MIRA={miraid}, books={total}, response={response.StatusCode}");
                }
            }

            Log.Info("Total sended = " + counter);

            Log.Trace("Reminder::DoWork: exit");
        }

        /// <summary>
        /// Создаём клиента для MSSQL.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public static Kladovka GetKladovka()
        {
            var irbis = new IrbisConnection();
            var db = new DbManager
                (
                    new Sql2008DataProvider(),
                    _config.MssqlConnectionString
                );
            var output = new ConsoleOutput();
            var result = new Kladovka(db, irbis, output);

            return result;
        }

        /// <summary>
        /// Подключаемся к серверу ИРБИС64.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public static IrbisConnection CreateConnection()
        {
            var result = new IrbisConnection(_config.IrbisConnectionString);

            return result;
        }

        /// <summary>
        /// Создаем клиента для MIRA.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public static MiraClient GetClient()
        {
            return new MiraClient(_config);
        }

        #endregion
    }
}
