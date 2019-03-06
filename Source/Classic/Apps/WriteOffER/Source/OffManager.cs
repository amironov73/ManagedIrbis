// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OffManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Linq;
using System.Text.RegularExpressions;

using AM;
using AM.Configuration;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Menus;

#endregion

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable UseStringInterpolation
// ReSharper disable UseNameofExpression
// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo

namespace WriteOffER
{
    public class OffManager
    {
        #region Public methods

        /// <summary>
        /// Output.
        /// </summary>
        [NotNull]
        public AbstractOutput Output { get; private set; }

        /// <summary>
        /// Connection.
        /// </summary>
        public IIrbisConnection Connection { get; private set; }

        /// <summary>
        /// Prefix
        /// </summary>
        public PrefixInfo Prefix { get; private set; }

        /// <summary>
        /// Price converter.
        /// </summary>
        [NotNull]
        public PriceMenu PriceConverter { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public OffManager
            (
                [NotNull] AbstractOutput output,
                [NotNull] IIrbisConnection connection,
                [NotNull] PrefixInfo prefix
            )
        {
            Code.NotNull(output, "output");
            Code.NotNull(connection, "connection");
            Code.NotNull(prefix, "prefix");

            Output = output;
            Connection = connection;
            Prefix = prefix;
            PriceConverter = PriceMenu.FromConnection(Connection);
        }

        #endregion

        #region Private members

        private string GetYear
            (
                [NotNull] MarcRecord record
            )
        {
            string result = record.FM(210, 'd');
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM(461, 'h');
            }

            if (string.IsNullOrEmpty(result))
            {
                result = record.FM(461, 'z');
            }

            if (string.IsNullOrEmpty(result))
            {
                result = record.FM(934);
            }

            if (string.IsNullOrEmpty(result))
            {
                return result;
            }

            Match match = Regex.Match(result, @"\d{4}");
            if (match.Success)
            {
                result = match.Value;
            }

            return result;
        }

        private decimal GetPrice
            (
                [NotNull] MarcRecord record
            )
        {
            string priceText = record.FM(10, 'd');

            return priceText.SafeToDecimal(0.0m);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Write the text.
        /// </summary>
        public void WriteLine
            (
                [NotNull] string format,
                params object[] args
            )
        {
            Output.WriteLine(format, args);
        }


        /// <summary>
        /// Get info for the number.
        /// </summary>
        [CanBeNull]
        public OffInfo GetInfo
            (
                [NotNull] string number
            )
        {
            Code.NotNullNorEmpty(number, "number");

            string expression = string.Format
                (
                    "\"{0}{1}\"",
                    Prefix.ThrowIfNull("Prefix").Prefix.ThrowIfNull("Prefix"),
                    number
                );

            int[] found = Connection.Search(expression);
            if (found.Length == 0)
            {
                WriteLine("Не найдено: {0}", number);
                return null;
            }

            if (found.Length != 1)
            {
                WriteLine("Найдено много: {0}", number);
                return null;
            }

            OffInfo result = new OffInfo();
            MarcRecord record = Connection.ReadRecord(found.First());
            result.Record = record;
            result.Number = number;
            string format = ConfigurationUtility
                .GetString("format", "@brief")
                .ThrowIfNull("format");
            result.Description = Connection.FormatRecord(format, record.Mfn);
            result.Year = GetYear(record);
            result.Price = GetPrice(record);
            result.Coefficient = PriceConverter.GetCoefficient(result.Year);

            return result;
        }

        #endregion
    }
}
