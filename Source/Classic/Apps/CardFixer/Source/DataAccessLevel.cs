/* DataAccessLevel.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using JetBrains.Annotations;

using AM.Text.Output;
using ManagedIrbis;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace CardFixer
{

    [PublicAPI]
    public sealed class DataAccessLevel
        : IDisposable
    {
        #region Properties

        public DbManager Db { get; private set; }

        public IrbisConnection Client { get; private set; }

        public AbstractOutput Output { get; private set; }

        public Table<Card> Cards
        {
            get { return Db.GetTable<Card>(); }
        }

        public Table<WrittenOff> BadNumbers
        {
            get { return Db.GetTable<WrittenOff>(); }
        } 

        #endregion

        #region Construction

        public DataAccessLevel
            (
                AbstractOutput output
            )
        {
            if (ReferenceEquals(output, null))
            {
                throw new ArgumentNullException("output");
            }
            Output = output;

            string sqlConnectionString
                = CM.AppSettings["sql-connection-string"];

            Db = new DbManager
                (
                   new Sql2008DataProvider(),
                   sqlConnectionString
                );
            Output.WriteLine
                (
                    "Подключились к SQL-серверу"
                );

            string irbisConnectionString
                = CM.AppSettings["irbis-connection-string"];
            Client = new IrbisConnection();
            Client.ParseConnectionString(irbisConnectionString);
            Client.Connect();
            Output.WriteLine("Подключились к ИРБИС-серверу");

            _timer = new Timer
            {
                Enabled = true,
                Interval = 60 * 1000
            };
            _timer.Tick += _timer_Tick;
        }

        #endregion

        #region Private members

        private Timer _timer;

        private void _timer_Tick
            (
                object sender, 
                EventArgs e
            )
        {
            if ((Output != null)
                && (Client != null))
            {
                Output.WriteLine("NO-OP");
                Client.NoOp();
            }
        }

        #endregion

        #region Public methods

        public int GetTotalCardCount()
        {
            return Cards.Count();
        }

        public int GetBadNumberCount()
        {
            return BadNumbers.Count();
        }

        public int GetIrbisCardCount()
        {
            return Client.GetMaxMfn
                (
                    CM.AppSettings["cards-database"]
                ) - 1;
        }

        public int[] GetBoxNumbers
            (
                CardStatus status
            )
        {
            return Cards
                .Where(card => card.Status == status)
                .Select(card => card.BoxNumber)
                .Distinct()
                .OrderBy(number => number)
                .ToArray();
        }

        public Card[] GetCards
            (
                int boxNumber,
                CardStatus status
            )
        {
            return Cards
                .Where
                (
                    card => (card.BoxNumber == boxNumber)
                            && (card.Status == status)
                )
                .OrderBy(card => card.Id)
                .ToArray();
        }

        public bool CheckWrittenOff
            (
                [JetBrains.Annotations.NotNull] string number
            )
        {
            if (string.IsNullOrEmpty(number))
            {
                throw new ArgumentNullException("number");
            }

            bool result = BadNumbers
                .Count(n => n.Number == number) != 0;

            return result;
        }

        private InventoryResult _CheckInventory
            (
                IrbisConnection client,
                string number
            )
        {
            if (string.IsNullOrEmpty(number))
            {
                throw new ArgumentNullException("number");
            }

            InventoryResult result = new InventoryResult
            {
                Number = number
            };

            int[] found = client.Search
                (
                    "\"IN={0}\"",
                    number
                );

            bool writtenOff = CheckWrittenOff(number);
            if (writtenOff)
            {
                if (found.Length != 0)
                {
                    result.Status = InventoryStatus.Problem;
                    result.Text = "проблема: то ли списан, то ли нет";
                }
                else
                {
                    result.Status = InventoryStatus.WrittenOff;
                    result.Text = "списан";
                }
            }
            else
            {
                switch (found.Length)
                {
                    case 0:
                        result.Status = InventoryStatus.NotFound;
                        result.Text = "не найден";
                        break;

                    case 1:
                        string format = CM.AppSettings["format"];
                        string description = client.FormatRecord
                            (
                                format,
                                found[0]
                            );
                        if (!string.IsNullOrEmpty(description))
                        {
                            description = description.Trim();
                        }
                        result.Status = InventoryStatus.Found;
                        result.Text = string.Format
                            (
                                "найден: {0}",
                                description
                            );
                        break;

                    default:
                        result.Status = InventoryStatus.Problem;
                        result.Text =
                            "проблема: много найдено";
                        break;
                }
            }

            return result;
        }

        [JetBrains.Annotations.NotNull]
        public InventoryResult CheckInventory
            (
                [JetBrains.Annotations.NotNull] string number
            )
        {
            return _CheckInventory
                (
                    Client,
                    number
                );
        }

        public InventoryResult CheckInventory2
            (
                [JetBrains.Annotations.NotNull] string number
            )
        {
            using (new IrbisContextSaver(Client))
            {
                Client.Database = CM.AppSettings["cards-database"];

                return _CheckInventory
                    (
                        Client,
                        number
                    );
            }
        }

        public InventoryResult CheckInventoryAll
            (
                [JetBrains.Annotations.NotNull] string number
            )
        {
            InventoryResult result = CheckInventory(number);
            if (result.Status == InventoryStatus.NotFound)
            {
                result = CheckInventory2(number);
            }
            return result;
        }

        public void UpdateCard
            (
                [JetBrains.Annotations.NotNull] Card card,
                CardStatus newStatus
            )
        {
            if (ReferenceEquals(card, null))
            {
                throw new ArgumentNullException("card");
            }

            if (card.Status == newStatus)
            {
                Output.WriteLine
                    (
                        "Нет нужды в обновлении статуса карты {0}",
                        card
                    );
                return;
            }

            card.Status = newStatus;
            Output.WriteLine
                (
                    "Статус карты {0} изменен на {1}",
                    card.Id,
                    newStatus
                );
            Db.Update(card);
            Output.WriteLine
                (
                    "Обновлена карточка: {0}",
                    card
                );
        }

        public string CreateRandomIndex
            (
                int length
            )
        {
            StringBuilder result = new StringBuilder(length);
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                char c = (char)(byte) random.Next(48, 58);
                result.Append(c);
            }
            return "-" + result;
        }

        public string IdToPath
            (
                string id
            )
        {
            string result = string.Format
                (
                    "{0}\\{1}\\{2}",
                    id.Substring(0,4),
                    id.Substring(4,2),
                    id.Substring(6,2)
                );
            return result;
        }

        public void CreateIrbisRecord
            (
                [JetBrains.Annotations.NotNull] Card card,
                [JetBrains.Annotations.NotNull] InventoryResult[] numbers
            )
        {
            if (ReferenceEquals(card, null))
            {
                throw new ArgumentNullException("card");
            }

            MarcRecord record = new MarcRecord
            {
                Database = CM.AppSettings["cards-database"]
            };
            record.AddField(920, "PAZK")
                .AddField(900, 't', "a", 'b', "05")
                .AddField(102, "RU")
                .AddField(101, "rus")
                .AddField(919, 'a', "rus", 'n', "02",
                    'k', "PSBO")
                .AddField(999, "0000000")
                .AddField(903, CreateRandomIndex(9))
                .AddField(200, 'a', "Нет заглавия");
            RecordField field = new RecordField("2020");
            field.AddSubField('a', card.Id)
                .AddSubField('b', IdToPath(card.Id))
                .AddSubField('c', "4")
                .AddNonEmptySubField('d', card.Operator)
                .AddNonEmptySubField('e', card.Remark);
            record.Fields.Add(field);
            foreach (InventoryResult number in numbers)
            {
                field = new RecordField("910");
                field.AddSubField('a', "0")
                    .AddSubField('b', number.Number)
                    .AddSubField('c', "?");
                string place = CM.AppSettings["cards-place"];
                if (!string.IsNullOrEmpty(place))
                {
                    field.AddSubField('d', place);
                }
                record.Fields.Add(field);
            }

            Client.WriteRecord(record, false, true);
            Output.WriteLine
                (
                    "Создана ИРБИС-запись MFN {0}",
                    record.Mfn
                );
        }

        public int UpdateRecord
            (
                string number,
                string cardId
            )
        {
            using (new IrbisContextSaver(Client))
            {
                Client.Database = CM.AppSettings["cards-database"];

                int[] found = Client.Search
                    (
                        "\"IN={0}\"",
                        number
                    );
                MarcRecord record = Client.ReadRecord(found[0]);
                RecordField oldField = record.Fields.GetField(2020).First();
                int index = record.Fields.IndexOf(oldField);
                RecordField newField = new RecordField(2020)
                    .AddSubField('a', cardId)
                    .AddSubField('b', IdToPath(cardId))
                    .AddSubField('c', "4");
                record.Fields.Insert(index, newField);
                Client.WriteRecord(record);
                return record.Mfn;
            }
        }

        #endregion

        #region IDisposable members

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Tick -= _timer_Tick;
                _timer.Dispose();
                _timer = null;
            }

            if (Db != null)
            {
                Db.Dispose();
                Db = null;
            }

            if (Client != null)
            {
                Client.Dispose();
                Client = null;
            }
        }

        #endregion
    }
}
    