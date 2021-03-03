// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System;
using System.Linq;

using AM;
using AM.Istu.OldModel;
using AM.Text.Output;

using BLToolkit.Data;

using ManagedIrbis;

#endregion

namespace HudoInvent
{
    /// <summary>
    /// Центр управления полетами.
    /// </summary>
    static class ControlCenter
    {
        public static AbstractOutput Output;
        public static DbManager Database;
        public static IrbisConnection Connection;

        public static void Initialize
            (
                AbstractOutput output
            )
        {
            try
            {
                Output = output;
                Database = GetDatabase();
                Connection = GetConnection();
            }
            catch (Exception exception)
            {
                Output.WriteErrorLine("ERROR: {0}", exception);
                throw;
            }
        }

        private static DbManager GetDatabase()
        {
            WriteLog("Подключаемся к MSSQL");
            var result = new DbManager("kladovka");
            WriteLog("Успешно!");

            return result;
        }

        private static IrbisConnection GetConnection()
        {
            WriteLog("Подключаемся к ИРБИС64");
            var result = IrbisConnectionUtility.GetClientFromConfig();
            WriteLog("Успешно!");

            return result;
        }

        private static void WriteLog
            (
                string format,
                params object[] args
            )
        {
            Output?.WriteLine(format, args);
        }

        public static void IdleAction()
        {
            if (Connection != null
                && Connection.Connected
                && !Connection.Busy)
            {
                Connection.NoOp();
                WriteLog("No-operation");
            }
        }

        public static HudoInfo GetBook
            (
                string barcode
            )
        {
            var found = Connection.Search($"\"IN={barcode}\"");

            if (found.Length == 0)
            {
                WriteLog("Не найдена книга для штрих-кода {0}", barcode);
                return null;
            }

            if (found.Length != 1)
            {
                WriteLog("Много записей для штрих-кода {0}", barcode);
                return null;
            }

            var result = new HudoInfo(Connection, found[0])
            {
                Barcode = barcode,
            };

            var number = result.Exemplars
                .FirstOrDefault(e => e.Barcode.SameString(barcode))
                ?.Number;
            if (number != null)
            {
                var table = Database.GetTable<HudRecord>();
                result.Ticket = table.FirstOrDefault
                    (
                        item => item.Inventory == number
                    )
                    ?.Ticket;
            }

            result.Number = number;
            result.CurrentExemplar = result.Exemplars.FirstOrDefault
                (
                    one => one.Number.SameString(number)
                );

            return result;
        }

        public static bool ConfirmBook(HudoInfo book)
        {
            var field = book.CurrentExemplar?.Field;
            if (field == null)
            {
                WriteLog("Непонятки с экземплярами");
                return false;
            }

            field.SetSubField('s', IrbisDate.TodayText);
            Connection.WriteRecord(book.Record);

            WriteLog
                (
                    "Подтверждена книга: {0} => {1} => {2}",
                    book.Barcode,
                    book.Number,
                    book.Description
                );

            return true;
        }
    }
}
