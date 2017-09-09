// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SiglaStamper.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

#endregion

namespace Sigler
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SiglaStamper
        : IDisposable
    {
        #region Properties

        [NotNull]
        public IrbisConnection Connection { get; private set; }

        [NotNull]
        public AbstractOutput Output { get; private set; }

        #endregion

        #region Construction

        public SiglaStamper
            (
                [NotNull] string connectionString,
                [NotNull] AbstractOutput output
            )
        {
            Output = output;
            IrbisConnection connection
                = new IrbisConnection(connectionString);
            Connection = connection;
        }

        #endregion

        #region Private members

        private Stopwatch _stopwatch;

        /// <summary>
        /// Форматируем отрезок времени
        /// в виде ЧЧ:ММ:СС.
        /// </summary>
        private static string _FormatTimeSpan
            (
                TimeSpan timeSpan
            )
        {
            string result = string.Format
                (
                    "{0:00}:{1:00}:{2:00}",
                    timeSpan.Hours,
                    timeSpan.Minutes,
                    timeSpan.Seconds
                );

            return result;
        }

        #endregion

        #region Public methods

        public void ProcessRecord
            (
                [NotNull] MarcRecord record,
                [NotNull] string newSigla,
                [NotNull] string number
            )
        {
            RecordField field = record.Fields
                .GetField(910)
                .GetField('b', number)
                .FirstOrDefault();
            if (ReferenceEquals(field, null))
            {
                Output.WriteLine("{0}: no 910", number);

                return;
            }

            string existingSigla = field.GetSubFieldValue('d', 0);
            if (newSigla.SameString(existingSigla))
            {
                Output.Write("{0} ", record.Mfn);
            }
            else
            {
                if (field.GetSubFieldValue('a', 0).SameString("5"))
                {
                    field.SetSubField('a', "0");
                }
                field.SetSubField('d', newSigla);

                Connection.WriteRecord(record, false, true);
                Output.Write("[{0}] ", record.Mfn);
            }
        }

        public void ProcessNumber
            (
                int index,
                [NotNull] string sigla,
                [NotNull] string number
            )
        {
            number = number.Trim();
            if (string.IsNullOrEmpty(number))
            {
                return;
            }

            Output.Write
                (
                    "{0,6}) {1} ",
                    index,
                    _FormatTimeSpan(_stopwatch.Elapsed)
                );

            int[] mfns = Connection.Search("\"IN=" + number + "\"");
            if (mfns.Length == 0)
            {
                Output.WriteLine("{0}: not found", number);

                return;
            }

            Output.Write("{0}: ", number);

            foreach (int mfn in mfns)
            {
                MarcRecord record = Connection.ReadRecord(mfn);
                ProcessRecord
                (
                    record,
                    sigla,
                    number
                );
            }

            Console.WriteLine();
        }

        public void ProcessFile
            (
                [NotNull] string fileName
            )
        {
            Code.FileExists(fileName, "fileName");

            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            string sigla = Path.GetFileNameWithoutExtension(fileName);

            int index = 0;

            using (StreamReader reader
                = new StreamReader(fileName, Encoding.Default))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    ProcessNumber
                    (
                        ++index,
                        sigla,
                        line
                    );
                }
            }
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            Connection.Dispose();
        }

        #endregion

        #region Object members

        #endregion
    }
}
