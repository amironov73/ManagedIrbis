using System;
using System.Linq;
using System.Text.RegularExpressions;

using AM;

namespace XamaWatcher
{
    internal sealed class NumberFilter
    {
        #region Nested classes

        public class Range
        {
            #region Properties

            public bool All { get; set; }

            public int Low { get; set; }

            public int High { get; set; }

            #endregion

            #region Private members

            private static readonly Regex _regex
            = new Regex(@"^\s*(?<low>\d+)\s*(?:-\s*(?<high>\d+))?\s*$");

            #endregion

            #region Public methods

            public bool CheckNumber(int n)
            {
                return All
                    || ((n >= Low) && (n <= High));
            }

            public static Range Parse(string text)
            {
                if (text.Contains("*"))
                {
                    return new Range{All = true};
                }
                Match match = _regex.Match(text);
                if (!match.Success)
                {
                    return null;
                }
                int low = int.Parse(match.Groups["low"].Value);
                int high = low;
                Group highGroup = match.Groups["high"];
                if (highGroup.Success)
                {
                    high = int.Parse(highGroup.Value);
                }
                Range result = new Range
                {
                    Low = low,
                    High = high
                };
                return result;
            }

            #endregion
        }

        #endregion

        #region Properties

        public string Specification { get; private set; }

        public Range[] Ranges { get; private set; }

        #endregion

        #region Construction

        public NumberFilter(string specification)
        {
            Specification = specification;
            Ranges = new Range[0];
        }

        #endregion

        #region Public methods

        public bool CheckNumber(string number)
        {
            if (!number.IsPositiveInteger())
            {
                return true;
            }
            int n = int.Parse(number);
            return Ranges
                .Any(range => range.CheckNumber(n));
        }

        public string[] FilterNumbers
        (
            string[] numbers
        )
        {
            return numbers
                .Where(CheckNumber)
                .ToArray();
        }

        public static NumberFilter ParseNumbers(string specification)
        {
            string[] parts = specification.Split(',', ';');
            NumberFilter result = new NumberFilter(specification)
            {
                Ranges = parts
                    .Select(part => Range.Parse(part))
                    .Where(range => range != null)
                    .ToArray()
                };
            return result;
        }

        #endregion
    }
    }

