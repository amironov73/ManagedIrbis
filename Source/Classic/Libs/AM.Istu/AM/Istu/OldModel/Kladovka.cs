// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Kladovka.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Configuration;
using AM.Data;
using AM.Logging;
using AM.Text;
using AM.Text.Output;
using BLToolkit.Data;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Linq;
using BLToolkit.Mapping;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Fields;
using MoonSharp.Interpreter;

#endregion

namespace AM.Istu.OldModel
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class Kladovka
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Table "attendances"
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public Table<AttendanceRecord> Attendances
        {
            get { return DB.GetTable<AttendanceRecord>(); }
        }

        /// <summary>
        /// Connection.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Database connection.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public DbManager DB { get; private set; }

        /// <summary>
        /// Abstract output.
        /// </summary>
        [CanBeNull]
        public AbstractOutput Output { get; private set; }

        /// <summary>
        /// Table "podsob".
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public Table<PodsobRecord> Podsob
        {
            get { return DB.GetTable<PodsobRecord>(); }
        }

        /// <summary>
        /// Table "readers".
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public Table<ReaderRecord> Readers
        {
            get { return DB.GetTable<ReaderRecord>(); }
        }

        /// <summary>
        /// Table "translator".
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public Table<TranslatorRecord> Translator
        {
            get { return DB.GetTable<TranslatorRecord>(); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Kladovka()
        {
            DB = new DbManager();
            Connection = IrbisConnectionUtility.GetClientFromConfig();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Kladovka
            (
                [CanBeNull] AbstractOutput output
            )
            : this()
        {
            Output = output;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Kladovka
            (
                [JetBrains.Annotations.NotNull] DbManager db,
                [JetBrains.Annotations.NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(db, "db");
            Code.NotNull(connection, "connection");

            DB = db;
            Connection = connection;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Kladovka
            (
                [JetBrains.Annotations.NotNull] DbManager db,
                [JetBrains.Annotations.NotNull] IrbisConnection connection,
                [CanBeNull] AbstractOutput output
            )
            : this(db, connection)
        {
            Output = output;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Kladovka
            (
                [JetBrains.Annotations.NotNull] string suffix,
                [JetBrains.Annotations.NotNull] string irbisConnectionString
            )
        {
            Code.NotNullNorEmpty(suffix, "suffix");
            Code.NotNullNorEmpty(irbisConnectionString, "irbisConnectionString");

            DB = new DbManager(suffix);
            Connection = new IrbisConnection(irbisConnectionString);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Create attendance.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public Kladovka CreateAttendance
            (
                [JetBrains.Annotations.NotNull] AttendanceRecord attendance
            )
        {
            Code.NotNull(attendance, "attendance");

            Query<AttendanceRecord>.Insert
                (
                    Attendances.DataContextInfo,
                    attendance
                );

            return this;
        }

        /// <summary>
        /// Create podsob record.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public Kladovka CreatePodsob
            (
                [JetBrains.Annotations.NotNull] PodsobRecord podsob
            )
        {
            Code.NotNull(podsob, "podsob");

            Query<PodsobRecord>.Insert
                (
                    Podsob.DataContextInfo,
                    podsob
                );

            return this;
        }

        /// <summary>
        /// Create reader.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public Kladovka CreateReader
            (
                [JetBrains.Annotations.NotNull] ReaderRecord reader
            )
        {
            Code.NotNull(reader, "reader");

            Query<ReaderRecord>.Insert
                (
                    Readers.DataContextInfo,
                    reader
                );

            return this;
        }

        /// <summary>
        /// Find reader by the barcode.
        /// </summary>
        [CanBeNull]
        public ReaderRecord FindReaderByBarcode
            (
                [JetBrains.Annotations.NotNull] string barcode
            )
        {
            Code.NotNullNorEmpty(barcode, "barcode");

            return Readers
                .FirstOrDefault(reader => reader.Barcode == barcode);
        }

        /// <summary>
        /// Find reader by the name.
        /// </summary>
        [CanBeNull]
        public ReaderRecord FindReaderByName
            (
                [JetBrains.Annotations.NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            return Readers
                .FirstOrDefault(reader => reader.Name.StartsWith(name));
        }

        /// <summary>
        /// Find reader by the ticket.
        /// </summary>
        [CanBeNull]
        public ReaderRecord FindReaderByTicket
            (
                [JetBrains.Annotations.NotNull] string ticket
            )
        {
            Code.NotNullNorEmpty(ticket, "ticket");

            return Readers
                .FirstOrDefault(reader => reader.Ticket == ticket);
        }

        /// <summary>
        /// Find reader by the RFID.
        /// </summary>
        public ReaderRecord FindReaderByRfid
            (
                [JetBrains.Annotations.NotNull] string rfid
            )
        {
            Code.NotNullNorEmpty(rfid, "rfid");

            return Readers
                .FirstOrDefault(reader => reader.Rfid == rfid);
        }

        /// <summary>
        /// Find records using specified expression.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public int[] FindRecords
            (
                [JetBrains.Annotations.NotNull] string format,
                params object[] args
            )
        {
            Code.NotNullNorEmpty(format, "format");

            int[] result = Connection.Search(format, args);

            return result;
        }

        /// <summary>
        /// Find podsob record by barcode or RFID.
        /// </summary>
        [CanBeNull]
        public PodsobRecord FindPodsobByBarcode
            (
                [JetBrains.Annotations.NotNull] string barcode
            )
        {
            Code.NotNullNorEmpty(barcode, "barcode");

            long inventory;
            PodsobRecord result = null;
            TranslatorRecord translator = GetTranslatorRecord(barcode);
            if (ReferenceEquals(translator, null))
            {
                if (NumericUtility.TryParseInt64(barcode, out inventory))
                {
                    result = GetPodsobRecord(inventory);
                }
            }
            else
            {
                inventory = translator.Inventory;
                result = GetPodsobRecord(inventory);
            }
            int[] found;
            if (ReferenceEquals(result, null))
            {
                found = FindRecords
                    (
                        "\"BAR={0}\" + \"RFID={0}\"",
                        barcode
                    );
                if (found.Length == 0)
                {
                    found = FindRecords
                        (
                            "\"IN={0}\"",
                            barcode
                        );
                }
            }
            else
            {
                found = FindRecords
                    (
                        "\"IN={0}\"",
                        inventory.ToInvariantString()
                    );
            }
            if (found.Length == 0)
            {
                return null;
            }

            MarcRecord record = ReadMarcRecord(found[0]);


            RecordField field = record.Fields
                .GetField(910)
                .FirstOrDefault
                (
                    f => barcode.SameString(f.GetFirstSubFieldValue('b'))
                        || barcode.SameString(f.GetFirstSubFieldValue('h'))
                        || barcode.SameString(f.GetFirstSubFieldValue('t'))
                );
            if (!ReferenceEquals(field, null))
            {
                NumericUtility.TryParseInt64
                    (
                        field.GetFirstSubFieldValue('b'),
                        out inventory
                    );
            }

            if (ReferenceEquals(result, null))
            {
                result = GetPodsobRecord(inventory);
                if (ReferenceEquals(result, null))
                {
                    result = new PodsobRecord();
                }
            }

            result.Record = record;
            result.Inventory = inventory;

            return result;
        }

        /// <summary>
        /// Get all marked exemplars.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public long[] GetAllMarked
            (
                [JetBrains.Annotations.NotNull] string place
            )
        {
            Code.NotNullNorEmpty(place, "place");

            Write("Marked: ");
            List<long> result = new List<long>();
            string expression = string.Format
                (
                    "\"INP={0}\"",
                    place
                );
            var batch = BatchRecordReader.Search
                (
                    Connection,
                    Connection.Database,
                    expression,
                    1000
                );
            foreach (MarcRecord record in batch)
            {
                Write(".");
                var exemplars = ExemplarInfo
                    .Parse(record)
                    .Where
                        (
                            ex => !string.IsNullOrEmpty(ex.CheckedDate)
                                && ex.RealPlace.SameString(place)
                        );
                foreach (ExemplarInfo exemplar in exemplars)
                {
                    long inventory;
                    if (NumericUtility.TryParseInt64
                        (
                            exemplar.Number,
                            out inventory
                        ))
                    {
                        result.Add(inventory);
                    }
                }
            }
            WriteLine("DONE");

            return result.ToArray();
        }

        /// <summary>
        /// Get all inventories from podsob table.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public long[] GetAllPodsob
            (
                [JetBrains.Annotations.NotNull] string place
            )
        {
            Code.NotNullNorEmpty(place, "place");

            long[] result = Podsob.Where
                (
                    rec => rec.Ticket == place
                )
                .Select
                (
                    rec => rec.Inventory
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get podsob record for the inventory number.
        /// </summary>
        [JetBrains.Annotations.CanBeNull]
        public PodsobRecord GetPodsobRecord
            (
                long inventory
            )
        {
            PodsobRecord result = Podsob.FirstOrDefault
                (
                    rec => rec.Inventory == inventory
                );

            return result;
        }

        /// <summary>
        /// Get translator record for the barcode or RFID.
        /// </summary>
        [JetBrains.Annotations.CanBeNull]
        public TranslatorRecord GetTranslatorRecord
            (
                [JetBrains.Annotations.CanBeNull] string barcode
            )
        {
            if (string.IsNullOrEmpty(barcode))
            {
                return null;
            }

            TranslatorRecord result = Translator.FirstOrDefault
                (
                    rec => rec.Inventory.ToString() == barcode
                           || rec.Barcode == barcode
                           || rec.Rfid == barcode
                );

            return result;
        }

        /// <summary>
        /// Read <see cref="MarcRecord"/> for given MFN.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public MarcRecord ReadMarcRecord
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            MarcRecord result = Connection.ReadRecord(mfn);
            result.Description = Connection.FormatRecord("@brief", mfn);

            return result;
        }

        /// <summary>
        /// Write some text.
        /// </summary>
        public void Write
            (
                [CanBeNull] string text
            )
        {
            if (!ReferenceEquals(Output, null)
                && !string.IsNullOrEmpty(text))
            {
                Output.Write(text);
            }
        }

        /// <summary>
        /// Write one line of text.
        /// </summary>
        public void WriteLine
            (
                [CanBeNull] string text
            )
        {
            if (!ReferenceEquals(Output, null))
            {
                if (string.IsNullOrEmpty(text))
                {
                    Output.WriteLine(string.Empty);
                }
                else
                {
                    Output.WriteLine(text);
                }
            }
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            DB.Dispose();
        }

        #endregion
    }
}
