// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Transport.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using MailKit.Net.Smtp;
using ManagedIrbis;
using MimeKit;

#endregion

// ReSharper disable StringLiteralTypo

namespace XamaWatcher
{
    class Transport
    {
        #region Properties

        public string Host { get; private set; }
        public int Port { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public string RequestDb { get; private set; }
        public string CatalogDb { get; private set; }
        public string ReaderDb { get; private set; }
        public string Place { get; private set; }
        public string My { get; private set; }
        public string MailLogin { get; private set; }
        public string MailPassword { get; private set; }
        public string MailFrom { get; private set; }
        public string MailSubject { get; private set; }
        public string MailServer { get; private set; }

        public int Auto { get; private set; }
        public bool Sound { get; private set; }

        #endregion

        #region Public methods

        public static Transport Load
            (
                Activity activity
            )
        {
            using (StreamReader reader
                = new StreamReader (activity.Assets.Open ("settings.txt")))
            {
                Transport result = new Transport
                {
                    Host = reader.ReadLine().ThrowIfNull(),
                    Port = int.Parse (reader.ReadLine ().ThrowIfNull()),
                    Login = reader.ReadLine ().ThrowIfNull(),
                    Password = reader.ReadLine ().ThrowIfNull(),
                    RequestDb = reader.ReadLine ().ThrowIfNull(),
                    CatalogDb = reader.ReadLine ().ThrowIfNull(),
                    ReaderDb = reader.ReadLine ().ThrowIfNull(),
                    Place = reader.ReadLine ().ThrowIfNull(),
                    My = reader.ReadLine ().ThrowIfNull(),
                    Auto = int.Parse (reader.ReadLine ().ThrowIfNull()),
                    Sound = Convert.ToBoolean (int.Parse (reader.ReadLine ().ThrowIfNull())),
                    MailLogin = reader.ReadLine().ThrowIfNull(),
                    MailPassword = reader.ReadLine().ThrowIfNull(),
                    MailFrom = reader.ReadLine().ThrowIfNull(),
                    MailSubject = reader.ReadLine().ThrowIfNull(),
                    MailServer = reader.ReadLine().ThrowIfNull()
                };

                return result;
            }
        }

        public async Task SendMail
            (
                BookRequest request,
                string body
            )
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(MailFrom, MailLogin));
            message.To.Add(new MailboxAddress(request.Reader.FullName, request.Reader.Email));
            message.Subject = MailSubject;
            message.Body = new TextPart("html")
            {
                Text = body
            };

            using (SmtpClient client = new SmtpClient())
            {
                client.Connect(MailServer, 587, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(MailLogin, MailPassword);
                await client.SendAsync(message);
            }
        }

        public IrbisConnection CreateClient()
        {
            IrbisConnection result = new IrbisConnection
            {
                Host = Host,
                Port = Port,
                Database = RequestDb,
                Username = Login,
                Password = Password
            };
            result.Connect();

            return result;
        }

        #endregion
    }
}